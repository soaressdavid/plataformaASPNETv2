namespace Shared.Services;

/// <summary>
/// Cache service for rate limiting
/// Supports AI Tutor rate limiting: 10 req/hour free, 50 req/hour premium
/// </summary>
public interface IRateLimitCacheService
{
    Task<bool> CheckRateLimitAsync(string userId, string resource, int limit, TimeSpan window, CancellationToken cancellationToken = default);
    Task<int> GetRemainingRequestsAsync(string userId, string resource, int limit, TimeSpan window, CancellationToken cancellationToken = default);
    Task ResetRateLimitAsync(string userId, string resource, CancellationToken cancellationToken = default);
}

public class RateLimitCacheService : IRateLimitCacheService
{
    private readonly IRedisCacheService _cache;
    private const string KeyPrefix = "ratelimit:";

    public RateLimitCacheService(IRedisCacheService cache)
    {
        _cache = cache ?? throw new ArgumentNullException(nameof(cache));
    }

    /// <summary>
    /// Checks if user has exceeded rate limit
    /// Returns true if request is allowed, false if rate limit exceeded
    /// </summary>
    public async Task<bool> CheckRateLimitAsync(
        string userId, 
        string resource, 
        int limit, 
        TimeSpan window, 
        CancellationToken cancellationToken = default)
    {
        var key = GenerateKey(userId, resource);
        var current = await _cache.IncrementAsync(key, 1, cancellationToken);
        
        if (current == 1)
        {
            // First request in window, set expiry
            await _cache.SetAsync(key, current, window, cancellationToken);
        }
        
        return current <= limit;
    }

    /// <summary>
    /// Gets remaining requests in current window
    /// </summary>
    public async Task<int> GetRemainingRequestsAsync(
        string userId, 
        string resource, 
        int limit, 
        TimeSpan window, 
        CancellationToken cancellationToken = default)
    {
        var key = GenerateKey(userId, resource);
        var current = await _cache.GetAsync<long>(key, cancellationToken);
        
        if (current == null || current == 0)
        {
            return limit;
        }
        
        var remaining = limit - (int)current;
        return Math.Max(0, remaining);
    }

    /// <summary>
    /// Resets rate limit for user and resource
    /// </summary>
    public async Task ResetRateLimitAsync(
        string userId, 
        string resource, 
        CancellationToken cancellationToken = default)
    {
        var key = GenerateKey(userId, resource);
        await _cache.DeleteAsync(key, cancellationToken);
    }

    private string GenerateKey(string userId, string resource)
    {
        var hour = DateTime.UtcNow.ToString("yyyy-MM-dd-HH");
        return $"{KeyPrefix}{resource}:{userId}:{hour}";
    }
}
