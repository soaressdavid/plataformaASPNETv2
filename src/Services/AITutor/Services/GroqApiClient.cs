using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using AITutor.Service.Configuration;
using AITutor.Service.Models;
using Microsoft.Extensions.Options;

namespace AITutor.Service.Services;

public class GroqApiClient : IGroqApiClient
{
    private readonly HttpClient _httpClient;
    private readonly GroqSettings _settings;
    private readonly ILogger<GroqApiClient> _logger;

    public GroqApiClient(
        HttpClient httpClient,
        IOptions<GroqSettings> settings,
        ILogger<GroqApiClient> logger)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _settings = settings?.Value ?? throw new ArgumentNullException(nameof(settings));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        ConfigureHttpClient();
    }

    private void ConfigureHttpClient()
    {
        _httpClient.BaseAddress = new Uri(_settings.ApiUrl);
        _httpClient.Timeout = TimeSpan.FromSeconds(_settings.TimeoutSeconds);
        _httpClient.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", _settings.ApiKey);
        _httpClient.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
    }

    public async Task<GroqResponse> SendChatCompletionAsync(
        List<Message> messages, 
        CancellationToken cancellationToken = default)
    {
        if (messages == null || messages.Count == 0)
        {
            throw new ArgumentException("Messages cannot be null or empty", nameof(messages));
        }

        var request = new GroqRequest
        {
            Model = _settings.Model,
            Messages = messages,
            Temperature = _settings.Temperature,
            MaxTokens = _settings.MaxTokens
        };

        _logger.LogInformation(
            "Sending chat completion request to Groq API with model {Model}, temperature {Temperature}, max tokens {MaxTokens}",
            _settings.Model, _settings.Temperature, _settings.MaxTokens);

        try
        {
            var jsonContent = JsonSerializer.Serialize(request, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
            });

            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(string.Empty, content, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync(cancellationToken);
                _logger.LogError(
                    "Groq API request failed with status {StatusCode}: {ErrorContent}",
                    response.StatusCode, errorContent);
                
                throw new HttpRequestException(
                    $"Groq API request failed with status {response.StatusCode}: {errorContent}");
            }

            var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
            var groqResponse = JsonSerializer.Deserialize<GroqResponse>(responseContent, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
            });

            if (groqResponse == null)
            {
                throw new InvalidOperationException("Failed to deserialize Groq API response");
            }

            _logger.LogInformation(
                "Successfully received response from Groq API. Tokens used: {TotalTokens}",
                groqResponse.Usage?.TotalTokens ?? 0);

            return groqResponse;
        }
        catch (TaskCanceledException ex) when (ex.InnerException is TimeoutException)
        {
            _logger.LogError(ex, "Groq API request timed out after {TimeoutSeconds} seconds", _settings.TimeoutSeconds);
            throw new TimeoutException($"Groq API request timed out after {_settings.TimeoutSeconds} seconds", ex);
        }
        catch (TaskCanceledException ex)
        {
            _logger.LogError(ex, "Groq API request was cancelled");
            throw;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP request to Groq API failed");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error occurred while calling Groq API");
            throw;
        }
    }
}
