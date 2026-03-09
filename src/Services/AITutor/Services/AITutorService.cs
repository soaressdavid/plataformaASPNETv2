using System.Text.Json;
using AITutor.Service.Models;

namespace AITutor.Service.Services;

public class AITutorService : IAITutorService
{
    private readonly IGroqApiClient _groqApiClient;
    private readonly CodeAnalysisPromptBuilder _promptBuilder;
    private readonly ILogger<AITutorService> _logger;
    private const int MaxRetries = 2;
    private const int RetryDelayMilliseconds = 1000;

    public AITutorService(
        IGroqApiClient groqApiClient,
        CodeAnalysisPromptBuilder promptBuilder,
        ILogger<AITutorService> logger)
    {
        _groqApiClient = groqApiClient ?? throw new ArgumentNullException(nameof(groqApiClient));
        _promptBuilder = promptBuilder ?? throw new ArgumentNullException(nameof(promptBuilder));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<CodeReviewResponse> ReviewCodeAsync(
        string code, 
        string? context = null, 
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(code))
        {
            throw new ArgumentException("Code cannot be null or empty", nameof(code));
        }

        _logger.LogInformation("Starting code review for code with length {CodeLength}", code.Length);

        var messages = _promptBuilder.BuildMessages(code, context);
        
        GroqResponse? groqResponse = null;
        Exception? lastException = null;

        // Retry logic: 2 retries with 1-second delay
        for (int attempt = 0; attempt <= MaxRetries; attempt++)
        {
            try
            {
                if (attempt > 0)
                {
                    _logger.LogInformation("Retrying code review request (attempt {Attempt} of {MaxRetries})", attempt, MaxRetries);
                    await Task.Delay(RetryDelayMilliseconds, cancellationToken);
                }

                groqResponse = await _groqApiClient.SendChatCompletionAsync(messages, cancellationToken);
                
                // If successful, break out of retry loop
                break;
            }
            catch (Exception ex)
            {
                lastException = ex;
                
                if (attempt < MaxRetries)
                {
                    _logger.LogWarning(ex, "Code review request failed on attempt {Attempt}, will retry", attempt + 1);
                }
                else
                {
                    _logger.LogError(ex, "Code review failed after {MaxRetries} retries", MaxRetries + 1);
                    throw new InvalidOperationException($"Code review failed after {MaxRetries + 1} attempts", ex);
                }
            }
        }

        // Parse the API response (groqResponse is guaranteed to be non-null here)
        var reviewResponse = ParseGroqResponse(groqResponse!);

        _logger.LogInformation(
            "Code review completed successfully. Overall score: {Score}, Suggestions: {SuggestionCount}",
            reviewResponse.OverallScore, reviewResponse.Suggestions.Count);

        return reviewResponse;
    }

    private CodeReviewResponse ParseGroqResponse(GroqResponse groqResponse)
    {
        if (groqResponse.Choices == null || groqResponse.Choices.Count == 0)
        {
            _logger.LogWarning("Groq response contains no choices, returning empty review");
            return CreateEmptyReview();
        }

        var messageContent = groqResponse.Choices[0].Message.Content;
        
        if (string.IsNullOrWhiteSpace(messageContent))
        {
            _logger.LogWarning("Groq response message content is empty, returning empty review");
            return CreateEmptyReview();
        }

        try
        {
            // Extract JSON from markdown code blocks if present
            var jsonContent = ExtractJsonFromMarkdown(messageContent);

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            var parsedResponse = JsonSerializer.Deserialize<ParsedApiResponse>(jsonContent, options);

            if (parsedResponse == null)
            {
                _logger.LogWarning("Failed to deserialize Groq response, returning empty review");
                return CreateEmptyReview();
            }

            // Convert parsed response to CodeReviewResponse
            var suggestions = parsedResponse.Suggestions?.Select(s => new Feedback
            {
                Type = ParseFeedbackType(s.Type),
                Message = s.Message ?? string.Empty,
                LineNumber = s.LineNumber,
                CodeExample = s.CodeExample ?? string.Empty
            }).ToList() ?? new List<Feedback>();

            return new CodeReviewResponse
            {
                Suggestions = suggestions,
                OverallScore = parsedResponse.OverallScore,
                SecurityIssues = parsedResponse.SecurityIssues ?? new List<string>(),
                PerformanceIssues = parsedResponse.PerformanceIssues ?? new List<string>()
            };
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "Failed to parse JSON from Groq response: {Content}", messageContent);
            return CreateEmptyReview();
        }
    }

    private string ExtractJsonFromMarkdown(string content)
    {
        // Check if content is wrapped in markdown code blocks
        var jsonStartMarker = "```json";
        var codeBlockEndMarker = "```";

        var jsonStartIndex = content.IndexOf(jsonStartMarker, StringComparison.OrdinalIgnoreCase);
        
        if (jsonStartIndex >= 0)
        {
            // Skip past the ```json marker
            var jsonContentStart = jsonStartIndex + jsonStartMarker.Length;
            var jsonEndIndex = content.IndexOf(codeBlockEndMarker, jsonContentStart, StringComparison.OrdinalIgnoreCase);
            
            if (jsonEndIndex > jsonContentStart)
            {
                return content.Substring(jsonContentStart, jsonEndIndex - jsonContentStart).Trim();
            }
        }

        // If no markdown code blocks found, return content as-is
        return content.Trim();
    }

    private FeedbackType ParseFeedbackType(string? type)
    {
        if (string.IsNullOrWhiteSpace(type))
        {
            return FeedbackType.BestPractice;
        }

        return type.ToLowerInvariant() switch
        {
            "security" => FeedbackType.Security,
            "performance" => FeedbackType.Performance,
            "bestpractice" => FeedbackType.BestPractice,
            "architecture" => FeedbackType.Architecture,
            _ => FeedbackType.BestPractice
        };
    }

    private CodeReviewResponse CreateEmptyReview()
    {
        return new CodeReviewResponse
        {
            Suggestions = new List<Feedback>(),
            OverallScore = 0,
            SecurityIssues = new List<string>(),
            PerformanceIssues = new List<string>()
        };
    }

    // Internal class for parsing the API response
    private class ParsedApiResponse
    {
        public List<ParsedFeedback>? Suggestions { get; set; }
        public int OverallScore { get; set; }
        public List<string>? SecurityIssues { get; set; }
        public List<string>? PerformanceIssues { get; set; }
    }

    private class ParsedFeedback
    {
        public string? Type { get; set; }
        public string? Message { get; set; }
        public int LineNumber { get; set; }
        public string? CodeExample { get; set; }
    }

    // Rate limiting and hint methods for property tests

    private readonly IRateLimitCacheService? _rateLimitService;
    private readonly IProgressService? _progressService;

    public AITutorService(
        IGroqApiClient groqApiClient,
        CodeAnalysisPromptBuilder promptBuilder,
        ILogger<AITutorService> logger,
        IRateLimitCacheService? rateLimitService = null,
        IProgressService? progressService = null)
        : this(groqApiClient, promptBuilder, logger)
    {
        _rateLimitService = rateLimitService;
        _progressService = progressService;
    }

    public async Task<RateLimitedResponse> ReviewCodeWithRateLimitAsync(
        string userId,
        string code,
        bool isPremium,
        CancellationToken cancellationToken = default)
    {
        if (_rateLimitService == null)
        {
            throw new InvalidOperationException("Rate limit service not configured");
        }

        var requestLimit = isPremium ? 50 : 10;
        var currentCount = await _rateLimitService.GetRequestCountAsync(userId);

        if (currentCount >= requestLimit)
        {
            _logger.LogWarning("User {UserId} exceeded rate limit ({CurrentCount}/{Limit})", 
                userId, currentCount, requestLimit);
            
            return RateLimitedResponse.RateLimited(
                $"Rate limit exceeded. {(isPremium ? "Premium" : "Free")} users are limited to {requestLimit} requests per hour.");
        }

        // Increment request count
        await _rateLimitService.IncrementRequestCountAsync(userId, 3600); // 1 hour TTL

        // Perform code review
        var feedback = await ReviewCodeAsync(code, null, cancellationToken);

        return RateLimitedResponse.CreateSuccess(feedback);
    }

    public async Task<HintResponse> GetHintAsync(
        string userId,
        string challengeId,
        int hintLevel,
        CancellationToken cancellationToken = default)
    {
        if (_progressService == null)
        {
            throw new InvalidOperationException("Progress service not configured");
        }

        // Validate hint level
        if (hintLevel < 1 || hintLevel > 3)
        {
            return HintResponse.Failed("Hint level must be between 1 and 3");
        }

        // Determine XP cost based on hint level
        var xpCost = hintLevel switch
        {
            1 => 5,
            2 => 10,
            3 => 20,
            _ => 0
        };

        // Check if user has enough XP
        var currentXP = await _progressService.GetUserXPAsync(userId);
        if (currentXP < xpCost)
        {
            _logger.LogWarning("User {UserId} has insufficient XP for hint level {Level} (has {CurrentXP}, needs {Cost})",
                userId, hintLevel, currentXP, xpCost);
            
            return HintResponse.Failed($"Insufficient XP. You need {xpCost} XP but only have {currentXP} XP.");
        }

        // Deduct XP
        var newXP = await _progressService.DeductXPAsync(userId, xpCost);

        _logger.LogInformation("Deducted {XPCost} XP from user {UserId} for hint level {Level}. New XP: {NewXP}",
            xpCost, userId, hintLevel, newXP);

        // Generate hint (simplified for testing)
        var hint = $"Hint level {hintLevel} for challenge {challengeId}";

        return HintResponse.CreateSuccess(hint, xpCost, newXP);
    }
}

// Response models for rate limiting and hints

public record RateLimitedResponse
{
    public bool Success { get; init; }
    public bool IsRateLimited { get; init; }
    public CodeReviewResponse? Feedback { get; init; }
    public string ErrorMessage { get; init; } = string.Empty;

    public static RateLimitedResponse CreateSuccess(CodeReviewResponse feedback) =>
        new() { Success = true, Feedback = feedback };

    public static RateLimitedResponse RateLimited(string message) =>
        new() { Success = false, IsRateLimited = true, ErrorMessage = message };
}

public record HintResponse
{
    public bool Success { get; init; }
    public string Hint { get; init; } = string.Empty;
    public int XPCost { get; init; }
    public int RemainingXP { get; init; }
    public string ErrorMessage { get; init; } = string.Empty;

    public static HintResponse CreateSuccess(string hint, int xpCost, int remainingXP) =>
        new() { Success = true, Hint = hint, XPCost = xpCost, RemainingXP = remainingXP };

    public static HintResponse Failed(string message) =>
        new() { Success = false, ErrorMessage = message };
}

// Service interfaces for dependency injection

public interface IRateLimitCacheService
{
    Task<int> GetRequestCountAsync(string userId);
    Task<int> IncrementRequestCountAsync(string userId, int ttlSeconds);
    Task<bool> ResetRequestCountAsync(string userId);
}

public interface IProgressService
{
    Task<int> GetUserXPAsync(string userId);
    Task<int> DeductXPAsync(string userId, int amount);
}
