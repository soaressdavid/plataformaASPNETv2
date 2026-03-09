using StackExchange.Redis;

namespace AITutor.Service.Services;

/// <summary>
/// Redis-based rate limiting service for AI Tutor requests.
/// Implements sliding window rate limiting with 1-hour windows.
/// </summary>
public class RateLimitCacheService : IRateLimitCacheService
{
    private readonly IConnectionMultiplexer _redis;
    private readonly ILogger<RateLimitCacheService> _logger;
    private const string KeyPrefix = "ai_tutor:rate_limit:";

    public RateLimitCacheService(
        IConnectionMultiplexer redis,
        ILogger<RateLimitCacheService> logger)
    {
        _redis = redis ?? throw new ArgumentNullException(nameof(redis));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<int> GetRequestCountAsync(string userId)
    {
        try
        {
            var db = _redis.GetDatabase();
            var key = GetKey(userId);
            var value = await db.StringGetAsync(key);

            if (value.IsNullOrEmpty)
            {
                return 0;
            }

            return (int)value;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting request count for user {UserId}", userId);
            return 0;
        }
    }

    public async Task<int> IncrementRequestCountAsync(string userId, int ttlSeconds)
    {
        try
        {
            var db = _redis.GetDatabase();
            var key = GetKey(userId);

            // Increment counter
            var newCount = await db.StringIncrementAsync(key);

            // Set expiration if this is the first request in the window
            if (newCount == 1)
            {
                await db.KeyExpireAsync(key, TimeSpan.FromSeconds(ttlSeconds));
            }

            _logger.LogDebug("Incremented request count for user {UserId} to {Count}", userId, newCount);

            return (int)newCount;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error incrementing request count for user {UserId}", userId);
            throw;
        }
    }

    public async Task<bool> ResetRequestCountAsync(string userId)
    {
        try
        {
            var db = _redis.GetDatabase();
            var key = GetKey(userId);
            var deleted = await db.KeyDeleteAsync(key);

            _logger.LogInformation("Reset request count for user {UserId}", userId);

            return deleted;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error resetting request count for user {UserId}", userId);
            return false;
        }
    }

    private string GetKey(string userId) => $"{KeyPrefix}{userId}";
}
