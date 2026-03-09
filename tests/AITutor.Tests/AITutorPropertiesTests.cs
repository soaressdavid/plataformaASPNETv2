using AITutor.Service.Models;
using AITutor.Service.Services;
using FsCheck;
using FsCheck.Xunit;
using Microsoft.Extensions.Logging;
using Moq;

namespace AITutor.Tests;

/// <summary>
/// Property-based tests for AI Tutor Service functionality.
/// Feature: aspnet-learning-platform
/// </summary>
public class AITutorPropertiesTests
{
    /// <summary>
    /// Property 9: AI Code Review Integration
    /// **Validates: Requirements 4.1**
    /// 
    /// For any code review request, the AI Tutor should call the Groq API with the submitted 
    /// code and return structured feedback.
    /// </summary>
    [Property(MaxTest = 20)]
    public void AICodeReview_CallsGroqAPIAndReturnsStructuredFeedback(NonEmptyString code)
    {
        // Arrange
        var codeStr = code.Get;
        var mockGroqApiClient = new Mock<IGroqApiClient>();
        var mockLogger = new Mock<ILogger<AITutorService>>();
        var promptBuilder = new CodeAnalysisPromptBuilder();
        
        var groqResponse = CreateValidGroqResponse();
        
        mockGroqApiClient
            .Setup(x => x.SendChatCompletionAsync(It.IsAny<List<Message>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(groqResponse);
        
        var service = new AITutorService(mockGroqApiClient.Object, promptBuilder, mockLogger.Object);
        
        // Act
        var result = service.ReviewCodeAsync(codeStr).GetAwaiter().GetResult();
        
        // Assert - Verify Groq API was called
        mockGroqApiClient.Verify(
            x => x.SendChatCompletionAsync(It.IsAny<List<Message>>(), It.IsAny<CancellationToken>()), 
            Times.Once);
        
        // Assert - Verify structured feedback is returned
        Assert.NotNull(result);
        Assert.NotNull(result.Suggestions);
        Assert.NotNull(result.SecurityIssues);
        Assert.NotNull(result.PerformanceIssues);
    }

    /// <summary>
    /// Property 10: AI Feedback Structure
    /// **Validates: Requirements 4.7**
    /// 
    /// For any AI code review response, the feedback should include specific code examples 
    /// for each suggestion.
    /// </summary>
    [Property(MaxTest = 20)]
    public void AIFeedback_IncludesCodeExamplesForEachSuggestion(NonEmptyString code)
    {
        // Arrange
        var codeStr = code.Get;
        var mockGroqApiClient = new Mock<IGroqApiClient>();
        var mockLogger = new Mock<ILogger<AITutorService>>();
        var promptBuilder = new CodeAnalysisPromptBuilder();
        
        // Create a response with multiple suggestions, each having a code example
        var jsonResponse = @"{
            ""suggestions"": [
                {
                    ""type"": ""Security"",
                    ""message"": ""SQL injection vulnerability detected"",
                    ""lineNumber"": 10,
                    ""codeExample"": ""Use parameterized queries: cmd.Parameters.AddWithValue(\""@id\"", id)""
                },
                {
                    ""type"": ""Performance"",
                    ""message"": ""N+1 query problem detected"",
                    ""lineNumber"": 15,
                    ""codeExample"": ""Use Include() for eager loading: context.Users.Include(u => u.Orders)""
                },
                {
                    ""type"": ""BestPractice"",
                    ""message"": ""Use async/await for I/O operations"",
                    ""lineNumber"": 20,
                    ""codeExample"": ""async Task<User> GetUserAsync(int id) { return await context.Users.FindAsync(id); }""
                }
            ],
            ""overallScore"": 70,
            ""securityIssues"": [""SQL Injection""],
            ""performanceIssues"": [""N+1 Query""]
        }";
        
        var groqResponse = CreateGroqResponseWithJson(jsonResponse);
        
        mockGroqApiClient
            .Setup(x => x.SendChatCompletionAsync(It.IsAny<List<Message>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(groqResponse);
        
        var service = new AITutorService(mockGroqApiClient.Object, promptBuilder, mockLogger.Object);
        
        // Act
        var result = service.ReviewCodeAsync(codeStr).GetAwaiter().GetResult();
        
        // Assert - Every suggestion must have a non-empty code example
        Assert.True(result.Suggestions.Count > 0);
        Assert.All(result.Suggestions, s => Assert.False(string.IsNullOrWhiteSpace(s.CodeExample)));
    }

    /// <summary>
    /// Property: AI Tutor handles various code lengths
    /// 
    /// For any valid code string (short or long), the AI Tutor should successfully process 
    /// the request and return a valid response.
    /// </summary>
    [Property(MaxTest = 20)]
    public void AICodeReview_HandlesVariousCodeLengths(NonEmptyString code)
    {
        // Arrange
        var codeStr = code.Get;
        var mockGroqApiClient = new Mock<IGroqApiClient>();
        var mockLogger = new Mock<ILogger<AITutorService>>();
        var promptBuilder = new CodeAnalysisPromptBuilder();
        
        var groqResponse = CreateValidGroqResponse();
        
        mockGroqApiClient
            .Setup(x => x.SendChatCompletionAsync(It.IsAny<List<Message>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(groqResponse);
        
        var service = new AITutorService(mockGroqApiClient.Object, promptBuilder, mockLogger.Object);
        
        // Act
        var result = service.ReviewCodeAsync(codeStr).GetAwaiter().GetResult();
        
        // Assert - Should return valid response regardless of code length
        Assert.NotNull(result);
        Assert.NotNull(result.Suggestions);
        Assert.True(result.OverallScore >= 0 && result.OverallScore <= 100);
    }

    /// <summary>
    /// Property: AI Tutor categorizes feedback correctly
    /// 
    /// For any code review response with suggestions, each suggestion should have a valid 
    /// feedback type (Security, Performance, BestPractice, or Architecture).
    /// </summary>
    [Property(MaxTest = 20)]
    public void AIFeedback_HasValidFeedbackTypes(NonEmptyString code)
    {
        // Arrange
        var codeStr = code.Get;
        var mockGroqApiClient = new Mock<IGroqApiClient>();
        var mockLogger = new Mock<ILogger<AITutorService>>();
        var promptBuilder = new CodeAnalysisPromptBuilder();
        
        var jsonResponse = @"{
            ""suggestions"": [
                {
                    ""type"": ""Security"",
                    ""message"": ""Security issue"",
                    ""lineNumber"": 1,
                    ""codeExample"": ""Fix security""
                },
                {
                    ""type"": ""Performance"",
                    ""message"": ""Performance issue"",
                    ""lineNumber"": 2,
                    ""codeExample"": ""Fix performance""
                },
                {
                    ""type"": ""BestPractice"",
                    ""message"": ""Best practice issue"",
                    ""lineNumber"": 3,
                    ""codeExample"": ""Fix best practice""
                },
                {
                    ""type"": ""Architecture"",
                    ""message"": ""Architecture issue"",
                    ""lineNumber"": 4,
                    ""codeExample"": ""Fix architecture""
                }
            ],
            ""overallScore"": 75,
            ""securityIssues"": [],
            ""performanceIssues"": []
        }";
        
        var groqResponse = CreateGroqResponseWithJson(jsonResponse);
        
        mockGroqApiClient
            .Setup(x => x.SendChatCompletionAsync(It.IsAny<List<Message>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(groqResponse);
        
        var service = new AITutorService(mockGroqApiClient.Object, promptBuilder, mockLogger.Object);
        
        // Act
        var result = service.ReviewCodeAsync(codeStr).GetAwaiter().GetResult();
        
        // Assert - All feedback types should be valid enum values
        var validTypes = new[] { 
            FeedbackType.Security, 
            FeedbackType.Performance, 
            FeedbackType.BestPractice, 
            FeedbackType.Architecture 
        };
        
        Assert.All(result.Suggestions, s => Assert.Contains(s.Type, validTypes));
    }

    /// <summary>
    /// Property: AI Tutor provides line numbers for suggestions
    /// 
    /// For any code review response with suggestions, each suggestion should have a line number 
    /// that is non-negative.
    /// </summary>
    [Property(MaxTest = 20)]
    public void AIFeedback_ProvidesValidLineNumbers(NonEmptyString code)
    {
        // Arrange
        var codeStr = code.Get;
        var mockGroqApiClient = new Mock<IGroqApiClient>();
        var mockLogger = new Mock<ILogger<AITutorService>>();
        var promptBuilder = new CodeAnalysisPromptBuilder();
        
        var jsonResponse = @"{
            ""suggestions"": [
                {
                    ""type"": ""Security"",
                    ""message"": ""Issue at line 5"",
                    ""lineNumber"": 5,
                    ""codeExample"": ""Fix code""
                },
                {
                    ""type"": ""Performance"",
                    ""message"": ""Issue at line 10"",
                    ""lineNumber"": 10,
                    ""codeExample"": ""Optimize code""
                }
            ],
            ""overallScore"": 80,
            ""securityIssues"": [],
            ""performanceIssues"": []
        }";
        
        var groqResponse = CreateGroqResponseWithJson(jsonResponse);
        
        mockGroqApiClient
            .Setup(x => x.SendChatCompletionAsync(It.IsAny<List<Message>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(groqResponse);
        
        var service = new AITutorService(mockGroqApiClient.Object, promptBuilder, mockLogger.Object);
        
        // Act
        var result = service.ReviewCodeAsync(codeStr).GetAwaiter().GetResult();
        
        // Assert - All line numbers should be non-negative
        Assert.All(result.Suggestions, s => Assert.True(s.LineNumber >= 0));
    }

    /// <summary>
    /// Property: AI Tutor handles context parameter
    /// 
    /// For any code review request with context, the context should be passed to the Groq API 
    /// and the service should return a valid response.
    /// </summary>
    [Property(MaxTest = 20)]
    public void AICodeReview_HandlesContextParameter(NonEmptyString code, NonEmptyString context)
    {
        // Arrange
        var codeStr = code.Get;
        var contextStr = context.Get;
        var mockGroqApiClient = new Mock<IGroqApiClient>();
        var mockLogger = new Mock<ILogger<AITutorService>>();
        var promptBuilder = new CodeAnalysisPromptBuilder();
        
        var groqResponse = CreateValidGroqResponse();
        
        mockGroqApiClient
            .Setup(x => x.SendChatCompletionAsync(It.IsAny<List<Message>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(groqResponse);
        
        var service = new AITutorService(mockGroqApiClient.Object, promptBuilder, mockLogger.Object);
        
        // Act
        var result = service.ReviewCodeAsync(codeStr, contextStr).GetAwaiter().GetResult();
        
        // Assert - Should return valid response with context
        mockGroqApiClient.Verify(
            x => x.SendChatCompletionAsync(
                It.Is<List<Message>>(m => m.Any(msg => msg.Content.Contains(contextStr))), 
                It.IsAny<CancellationToken>()), 
            Times.Once);
        
        Assert.NotNull(result);
    }

    // Helper methods to create test data
    private static GroqResponse CreateValidGroqResponse()
    {
        var jsonContent = @"{
            ""suggestions"": [],
            ""overallScore"": 100,
            ""securityIssues"": [],
            ""performanceIssues"": []
        }";

        return CreateGroqResponseWithJson(jsonContent);
    }

    private static GroqResponse CreateGroqResponseWithJson(string jsonContent)
    {
        return new GroqResponse
        {
            Id = "test-id",
            Object = "chat.completion",
            Created = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
            Model = "llama-3.1-70b-versatile",
            Choices = new List<Choice>
            {
                new Choice
                {
                    Index = 0,
                    Message = new Message
                    {
                        Role = "assistant",
                        Content = jsonContent
                    },
                    FinishReason = "stop"
                }
            },
            Usage = new Usage
            {
                PromptTokens = 100,
                CompletionTokens = 50,
                TotalTokens = 150
            }
        };
    }

    /// <summary>
    /// Property 13: AI Rate Limiting
    /// **Validates: Requirements 8.10**
    /// 
    /// For any user, after exceeding the rate limit (10 requests/hour for free users, 
    /// 50 requests/hour for premium users), subsequent requests should be rejected 
    /// with a rate limit error.
    /// </summary>
    [Property(MaxTest = 20)]
    public void AIRateLimiting_EnforcesRequestLimits(NonEmptyString userId, bool isPremium)
    {
        // Arrange
        var userIdStr = userId.Get;
        var requestLimit = isPremium ? 50 : 10;
        
        var mockGroqApiClient = new Mock<IGroqApiClient>();
        var mockLogger = new Mock<ILogger<AITutorService>>();
        var mockRateLimitService = new Mock<IRateLimitCacheService>();
        var promptBuilder = new CodeAnalysisPromptBuilder();
        
        var groqResponse = CreateValidGroqResponse();
        
        mockGroqApiClient
            .Setup(x => x.SendChatCompletionAsync(It.IsAny<List<Message>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(groqResponse);
        
        // Setup rate limit service to track requests
        var requestCount = 0;
        mockRateLimitService
            .Setup(x => x.IncrementRequestCountAsync(userIdStr, It.IsAny<int>()))
            .ReturnsAsync(() => ++requestCount);
        
        mockRateLimitService
            .Setup(x => x.GetRequestCountAsync(userIdStr))
            .ReturnsAsync(() => requestCount);
        
        var service = new AITutorService(
            mockGroqApiClient.Object, 
            promptBuilder, 
            mockLogger.Object,
            mockRateLimitService.Object);
        
        // Act - Make requests up to the limit
        for (int i = 0; i < requestLimit; i++)
        {
            var result = service.ReviewCodeWithRateLimitAsync(userIdStr, "test code", isPremium)
                .GetAwaiter().GetResult();
            
            // Should succeed within limit
            Assert.True(result.Success);
            Assert.NotNull(result.Feedback);
        }
        
        // Act - Exceed the limit
        var exceededResult = service.ReviewCodeWithRateLimitAsync(userIdStr, "test code", isPremium)
            .GetAwaiter().GetResult();
        
        // Assert - Should be rate limited
        Assert.False(exceededResult.Success);
        Assert.True(exceededResult.IsRateLimited);
        Assert.Contains("rate limit", exceededResult.ErrorMessage.ToLower());
    }

    /// <summary>
    /// Property 23: Hint XP Deduction
    /// **Validates: Requirements 25.5, 25.6, 25.7**
    /// 
    /// For any hint request, the user's XP should be deducted by the hint cost 
    /// (5 XP for level 1, 10 XP for level 2, 20 XP for level 3). The deduction 
    /// should be atomic and consistent.
    /// </summary>
    [Property(MaxTest = 20)]
    public void HintXPDeduction_DeductsCorrectAmount(NonEmptyString userId, PositiveInt initialXP)
    {
        // Arrange
        var userIdStr = userId.Get;
        var userXP = initialXP.Get;
        
        var mockGroqApiClient = new Mock<IGroqApiClient>();
        var mockLogger = new Mock<ILogger<AITutorService>>();
        var mockProgressService = new Mock<IProgressService>();
        var promptBuilder = new CodeAnalysisPromptBuilder();
        
        var groqResponse = CreateValidGroqResponse();
        
        mockGroqApiClient
            .Setup(x => x.SendChatCompletionAsync(It.IsAny<List<Message>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(groqResponse);
        
        // Setup progress service to track XP
        var currentXP = userXP;
        mockProgressService
            .Setup(x => x.GetUserXPAsync(userIdStr))
            .ReturnsAsync(() => currentXP);
        
        mockProgressService
            .Setup(x => x.DeductXPAsync(userIdStr, It.IsAny<int>()))
            .ReturnsAsync((string uid, int amount) =>
            {
                currentXP = Math.Max(0, currentXP - amount);
                return currentXP;
            });
        
        var service = new AITutorService(
            mockGroqApiClient.Object, 
            promptBuilder, 
            mockLogger.Object,
            null,
            mockProgressService.Object);
        
        // Test each hint level
        var hintCosts = new[] { 5, 10, 20 }; // Level 1, 2, 3
        
        for (int level = 0; level < hintCosts.Length; level++)
        {
            var xpBefore = currentXP;
            var expectedCost = hintCosts[level];
            
            // Skip if not enough XP
            if (xpBefore < expectedCost)
                break;
            
            // Act - Request hint
            var result = service.GetHintAsync(userIdStr, "challenge-id", level + 1)
                .GetAwaiter().GetResult();
            
            // Assert - XP should be deducted by exact amount
            var xpAfter = currentXP;
            var actualDeduction = xpBefore - xpAfter;
            
            Assert.Equal(expectedCost, actualDeduction);
            Assert.True(result.Success);
            Assert.NotNull(result.Hint);
        }
    }

    /// <summary>
    /// Property 24: XP Non-Negative Constraint
    /// **Validates: Requirements 25.8**
    /// 
    /// For any hint request, if the user does not have enough XP to pay for the hint, 
    /// the request should be rejected and XP should remain unchanged (never go negative).
    /// </summary>
    [Property(MaxTest = 20)]
    public void XPNonNegative_NeverGoesNegative(NonEmptyString userId, NonNegativeInt initialXP)
    {
        // Arrange
        var userIdStr = userId.Get;
        var userXP = Math.Min(initialXP.Get, 15); // Ensure low XP for testing
        
        var mockGroqApiClient = new Mock<IGroqApiClient>();
        var mockLogger = new Mock<ILogger<AITutorService>>();
        var mockProgressService = new Mock<IProgressService>();
        var promptBuilder = new CodeAnalysisPromptBuilder();
        
        var groqResponse = CreateValidGroqResponse();
        
        mockGroqApiClient
            .Setup(x => x.SendChatCompletionAsync(It.IsAny<List<Message>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(groqResponse);
        
        // Setup progress service to track XP
        var currentXP = userXP;
        mockProgressService
            .Setup(x => x.GetUserXPAsync(userIdStr))
            .ReturnsAsync(() => currentXP);
        
        mockProgressService
            .Setup(x => x.DeductXPAsync(userIdStr, It.IsAny<int>()))
            .ReturnsAsync((string uid, int amount) =>
            {
                // Enforce non-negative constraint
                if (currentXP < amount)
                    return currentXP; // Don't deduct if insufficient
                
                currentXP -= amount;
                return currentXP;
            });
        
        var service = new AITutorService(
            mockGroqApiClient.Object, 
            promptBuilder, 
            mockLogger.Object,
            null,
            mockProgressService.Object);
        
        // Act - Request expensive hint (20 XP for level 3)
        var result = service.GetHintAsync(userIdStr, "challenge-id", 3)
            .GetAwaiter().GetResult();
        
        // Assert - If insufficient XP, request should fail and XP unchanged
        if (userXP < 20)
        {
            Assert.False(result.Success);
            Assert.Equal(userXP, currentXP); // XP unchanged
            Assert.True(currentXP >= 0); // Never negative
            Assert.Contains("insufficient", result.ErrorMessage.ToLower());
        }
        else
        {
            // If sufficient XP, should succeed and XP should be deducted
            Assert.True(result.Success);
            Assert.Equal(userXP - 20, currentXP);
            Assert.True(currentXP >= 0); // Still non-negative
        }
        
        // Assert - XP should NEVER be negative regardless of operations
        Assert.True(currentXP >= 0, $"XP went negative: {currentXP}");
    }

    /// <summary>
    /// Property: Rate limit resets after time window
    /// 
    /// For any user, after the rate limit time window expires (1 hour), 
    /// the request count should reset and new requests should be allowed.
    /// </summary>
    [Property(MaxTest = 20)]
    public void RateLimit_ResetsAfterTimeWindow(NonEmptyString userId, bool isPremium)
    {
        // Arrange
        var userIdStr = userId.Get;
        var requestLimit = isPremium ? 50 : 10;
        
        var mockGroqApiClient = new Mock<IGroqApiClient>();
        var mockLogger = new Mock<ILogger<AITutorService>>();
        var mockRateLimitService = new Mock<IRateLimitCacheService>();
        var promptBuilder = new CodeAnalysisPromptBuilder();
        
        var groqResponse = CreateValidGroqResponse();
        
        mockGroqApiClient
            .Setup(x => x.SendChatCompletionAsync(It.IsAny<List<Message>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(groqResponse);
        
        // Setup rate limit service with time-based reset
        var requestCount = 0;
        var lastResetTime = DateTime.UtcNow;
        
        mockRateLimitService
            .Setup(x => x.IncrementRequestCountAsync(userIdStr, It.IsAny<int>()))
            .ReturnsAsync(() =>
            {
                // Simulate 1-hour window reset
                if ((DateTime.UtcNow - lastResetTime).TotalHours >= 1)
                {
                    requestCount = 0;
                    lastResetTime = DateTime.UtcNow;
                }
                return ++requestCount;
            });
        
        mockRateLimitService
            .Setup(x => x.GetRequestCountAsync(userIdStr))
            .ReturnsAsync(() => requestCount);
        
        mockRateLimitService
            .Setup(x => x.ResetRequestCountAsync(userIdStr))
            .ReturnsAsync(() =>
            {
                requestCount = 0;
                lastResetTime = DateTime.UtcNow;
                return true;
            });
        
        var service = new AITutorService(
            mockGroqApiClient.Object, 
            promptBuilder, 
            mockLogger.Object,
            mockRateLimitService.Object);
        
        // Act - Fill up the rate limit
        for (int i = 0; i < requestLimit; i++)
        {
            service.ReviewCodeWithRateLimitAsync(userIdStr, "test code", isPremium)
                .GetAwaiter().GetResult();
        }
        
        // Verify rate limited
        var limitedResult = service.ReviewCodeWithRateLimitAsync(userIdStr, "test code", isPremium)
            .GetAwaiter().GetResult();
        Assert.False(limitedResult.Success);
        Assert.True(limitedResult.IsRateLimited);
        
        // Act - Simulate time window reset
        mockRateLimitService.Object.ResetRequestCountAsync(userIdStr).GetAwaiter().GetResult();
        
        // Act - Try again after reset
        var afterResetResult = service.ReviewCodeWithRateLimitAsync(userIdStr, "test code", isPremium)
            .GetAwaiter().GetResult();
        
        // Assert - Should succeed after reset
        Assert.True(afterResetResult.Success);
        Assert.False(afterResetResult.IsRateLimited);
    }
}
