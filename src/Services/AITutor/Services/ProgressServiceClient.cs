using System.Net.Http.Json;

namespace AITutor.Service.Services;

/// <summary>
/// HTTP client for communicating with the Progress Service.
/// Handles XP queries and deductions for hint system.
/// </summary>
public class ProgressServiceClient : IProgressService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ProgressServiceClient> _logger;

    public ProgressServiceClient(
        HttpClient httpClient,
        ILogger<ProgressServiceClient> logger)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<int> GetUserXPAsync(string userId)
    {
        try
        {
            var response = await _httpClient.GetAsync($"/api/progress/{userId}/xp");
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<XPResponse>();
            return result?.TotalXP ?? 0;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting XP for user {UserId}", userId);
            throw;
        }
    }

    public async Task<int> DeductXPAsync(string userId, int amount)
    {
        try
        {
            var request = new DeductXPRequest { Amount = amount };
            var response = await _httpClient.PostAsJsonAsync($"/api/progress/{userId}/deduct-xp", request);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<XPResponse>();
            return result?.TotalXP ?? 0;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deducting {Amount} XP from user {UserId}", amount, userId);
            throw;
        }
    }

    private record XPResponse(int TotalXP);
    private record DeductXPRequest
    {
        public int Amount { get; init; }
    }
}
