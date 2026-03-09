using StackExchange.Redis;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace Execution.Service.Services;

/// <summary>
/// Caches execution results in Redis to avoid re-executing identical code
/// Validates: Requirement 22.7 (cache successful execution results)
/// Task 6.14: Add Redis caching for execution results
/// </summary>
public class ExecutionCacheService : IExecutionCacheService
{
    private readonly IConnectionMultiplexer _redis;
    private readonly ILogger<ExecutionCacheService> _logger;

    // Cache configuration
    private const int CacheTtlHours = 1;
    private const string CacheKeyPrefix = "execution:result:";

    public ExecutionCacheService(
        IConnectionMultiplexer redis,
        ILogger<ExecutionCacheService> logger)
    {
        _redis = redis;
        _logger = logger;
    }

    /// <summary>
    /// Generates a cache key from code content using SHA256 hash
    /// </summary>
    public string GenerateCacheKey(string code)
    {
        using var sha256 = SHA256.Create();
        var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(code));
        var hashString = Convert.ToHexString(hashBytes).ToLowerInvariant();
        return $"{CacheKeyPrefix}{hashString}";
    }

    /// <summary>
    /// Attempts to retrieve cached execution result
    /// Returns null if not found or expired
    /// </summary>
    public async Task<DockerExecutionResult?> GetCachedResultAsync(string code)
    {
        try
        {
            var cacheKey = GenerateCacheKey(code);
            var db = _redis.GetDatabase();

            var cachedJson = await db.StringGetAsync(cacheKey);

            if (!cachedJson.HasValue)
            {
                _logger.LogDebug("Cache miss for key {CacheKey}", cacheKey);
                return null;
            }

            var result = JsonSerializer.Deserialize<DockerExecutionResult>((string)cachedJson!);
            
            if (result != null)
            {
                _logger.LogInformation("Cache hit for key {CacheKey}", cacheKey);
            }

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to retrieve cached result");
            return null; // Fail gracefully
        }
    }

    /// <summary>
    /// Caches successful execution results with 1-hour TTL
    /// Only caches completed executions (not errors or timeouts)
    /// </summary>
    public async Task CacheResultAsync(string code, DockerExecutionResult result)
    {
        // Only cache successful executions
        if (result.Status != ExecutionStatus.Completed)
        {
            _logger.LogDebug("Skipping cache for non-successful execution (status: {Status})", result.Status);
            return;
        }

        try
        {
            var cacheKey = GenerateCacheKey(code);
            var db = _redis.GetDatabase();

            var json = JsonSerializer.Serialize(result);
            var ttl = TimeSpan.FromHours(CacheTtlHours);

            await db.StringSetAsync(cacheKey, json, ttl);

            _logger.LogInformation("Cached execution result with key {CacheKey} (TTL: {TTL}h)", cacheKey, CacheTtlHours);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to cache execution result");
            // Don't throw - caching is optional
        }
    }

    /// <summary>
    /// Invalidates cached result for specific code
    /// </summary>
    public async Task InvalidateCacheAsync(string code)
    {
        try
        {
            var cacheKey = GenerateCacheKey(code);
            var db = _redis.GetDatabase();

            await db.KeyDeleteAsync(cacheKey);

            _logger.LogInformation("Invalidated cache for key {CacheKey}", cacheKey);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to invalidate cache");
        }
    }

    /// <summary>
    /// Gets cache statistics
    /// </summary>
    public async Task<CacheStatistics> GetStatisticsAsync()
    {
        try
        {
            var db = _redis.GetDatabase();
            var server = _redis.GetServer(_redis.GetEndPoints().First());

            // Count keys with our prefix
            var keys = server.Keys(pattern: $"{CacheKeyPrefix}*").ToList();
            var totalKeys = keys.Count;

            // Calculate total memory used (approximate)
            long totalMemoryBytes = 0;
            foreach (var key in keys.Take(100)) // Sample first 100 keys
            {
                var value = await db.StringGetAsync(key);
                if (value.HasValue)
                {
                    totalMemoryBytes += Encoding.UTF8.GetByteCount(value!);
                }
            }

            // Extrapolate if we sampled
            if (totalKeys > 100)
            {
                totalMemoryBytes = (long)(totalMemoryBytes * (totalKeys / 100.0));
            }

            return new CacheStatistics
            {
                TotalCachedResults = totalKeys,
                EstimatedMemoryBytes = totalMemoryBytes,
                TtlHours = CacheTtlHours
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get cache statistics");
            return new CacheStatistics();
        }
    }

    /// <summary>
    /// Clears all cached execution results
    /// </summary>
    public async Task ClearAllAsync()
    {
        try
        {
            var server = _redis.GetServer(_redis.GetEndPoints().First());
            var keys = server.Keys(pattern: $"{CacheKeyPrefix}*").ToArray();

            if (keys.Length > 0)
            {
                var db = _redis.GetDatabase();
                await db.KeyDeleteAsync(keys);
                _logger.LogInformation("Cleared {Count} cached execution results", keys.Length);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to clear cache");
        }
    }
}

/// <summary>
/// Interface for execution cache service
/// </summary>
public interface IExecutionCacheService
{
    string GenerateCacheKey(string code);
    Task<DockerExecutionResult?> GetCachedResultAsync(string code);
    Task CacheResultAsync(string code, DockerExecutionResult result);
    Task InvalidateCacheAsync(string code);
    Task<CacheStatistics> GetStatisticsAsync();
    Task ClearAllAsync();
}

/// <summary>
/// Cache statistics
/// </summary>
public class CacheStatistics
{
    public int TotalCachedResults { get; set; }
    public long EstimatedMemoryBytes { get; set; }
    public int TtlHours { get; set; }

    public string EstimatedMemoryMB => $"{EstimatedMemoryBytes / (1024.0 * 1024.0):F2} MB";
}
