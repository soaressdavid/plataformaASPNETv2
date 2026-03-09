using StackExchange.Redis;
using System.Text.Json;
using Shared.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Shared.Services;

/// <summary>
/// Distributed cache service using Redis
/// Validates: Requirements 22.1, 22.2, 22.3, 22.4, 22.5, 22.6, 22.7, 22.8
/// </summary>
public interface IRedisCacheService
{
    Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default);
    Task<bool> SetAsync<T>(string key, T value, TimeSpan? expiry = null, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(string key, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(string key, CancellationToken cancellationToken = default);
    Task<long> IncrementAsync(string key, long value = 1, CancellationToken cancellationToken = default);
    Task<bool> SetAddAsync(string key, string value, CancellationToken cancellationToken = default);
    Task<string[]> SetMembersAsync(string key, CancellationToken cancellationToken = default);
    Task<bool> SortedSetAddAsync(string key, string member, double score, CancellationToken cancellationToken = default);
    Task<SortedSetEntry[]> SortedSetRangeByRankWithScoresAsync(string key, long start = 0, long stop = -1, Order order = Order.Ascending, CancellationToken cancellationToken = default);
    Task InvalidateAsync(string pattern, CancellationToken cancellationToken = default);
}

public class RedisCacheService : IRedisCacheService
{
    private readonly IConnectionMultiplexer _redis;
    private readonly IDatabase _db;
    private readonly CacheTTLConfiguration _ttlConfig;
    private readonly JsonSerializerOptions _jsonOptions;

    public RedisCacheService(IConnectionMultiplexer redis, RedisConfiguration config)
    {
        _redis = redis ?? throw new ArgumentNullException(nameof(redis));
        _db = _redis.GetDatabase();
        _ttlConfig = config.TTL;
        
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            WriteIndented = false
        };
    }

    /// <summary>
    /// Gets a cached value by key
    /// Validates: Requirement 22.7 - Return cached result without re-execution
    /// </summary>
    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
    {
        var value = await _db.StringGetAsync(key);
        
        if (value.IsNullOrEmpty)
        {
            return default;
        }
        
        return JsonSerializer.Deserialize<T>((string)value!, _jsonOptions);
    }

    /// <summary>
    /// Sets a cached value with optional expiry
    /// Validates: Requirements 22.1, 22.4, 22.5, 22.6 - Cache with appropriate TTL
    /// </summary>
    public async Task<bool> SetAsync<T>(string key, T value, TimeSpan? expiry = null, CancellationToken cancellationToken = default)
    {
        var json = JsonSerializer.Serialize(value, _jsonOptions);
        return await _db.StringSetAsync(key, json, expiry);
    }

    /// <summary>
    /// Deletes a cached value
    /// Validates: Requirement 22.8 - Invalidate cache entries when content is updated
    /// </summary>
    public async Task<bool> DeleteAsync(string key, CancellationToken cancellationToken = default)
    {
        return await _db.KeyDeleteAsync(key);
    }

    /// <summary>
    /// Checks if a key exists in cache
    /// </summary>
    public async Task<bool> ExistsAsync(string key, CancellationToken cancellationToken = default)
    {
        return await _db.KeyExistsAsync(key);
    }

    /// <summary>
    /// Increments a counter (useful for rate limiting)
    /// </summary>
    public async Task<long> IncrementAsync(string key, long value = 1, CancellationToken cancellationToken = default)
    {
        return await _db.StringIncrementAsync(key, value);
    }

    /// <summary>
    /// Adds a value to a set
    /// </summary>
    public async Task<bool> SetAddAsync(string key, string value, CancellationToken cancellationToken = default)
    {
        return await _db.SetAddAsync(key, value);
    }

    /// <summary>
    /// Gets all members of a set
    /// </summary>
    public async Task<string[]> SetMembersAsync(string key, CancellationToken cancellationToken = default)
    {
        var values = await _db.SetMembersAsync(key);
        return values.Select(v => v.ToString()).ToArray();
    }

    /// <summary>
    /// Adds a member to a sorted set with a score (useful for leaderboards)
    /// Validates: Requirement 22.5 - Leaderboard data caching
    /// </summary>
    public async Task<bool> SortedSetAddAsync(string key, string member, double score, CancellationToken cancellationToken = default)
    {
        return await _db.SortedSetAddAsync(key, member, score);
    }

    /// <summary>
    /// Gets a range of members from a sorted set with their scores
    /// Validates: Requirement 22.5 - Leaderboard data retrieval
    /// </summary>
    public async Task<SortedSetEntry[]> SortedSetRangeByRankWithScoresAsync(
        string key, 
        long start = 0, 
        long stop = -1, 
        Order order = Order.Ascending, 
        CancellationToken cancellationToken = default)
    {
        return await _db.SortedSetRangeByRankWithScoresAsync(key, start, stop, order);
    }

    /// <summary>
    /// Invalidates cache entries matching a pattern
    /// Validates: Requirement 22.8 - Invalidate cache when content is updated
    /// Note: This is an expensive operation and should be used sparingly
    /// </summary>
    public async Task InvalidateAsync(string pattern, CancellationToken cancellationToken = default)
    {
        var endpoints = _redis.GetEndPoints();
        
        foreach (var endpoint in endpoints)
        {
            var server = _redis.GetServer(endpoint);
            
            if (server.IsConnected)
            {
                await foreach (var key in server.KeysAsync(pattern: pattern))
                {
                    await _db.KeyDeleteAsync(key);
                }
            }
        }
    }
}

/// <summary>
/// Extension methods for registering Redis services
/// </summary>
public static class RedisCacheServiceExtensions
{
    /// <summary>
    /// Adds Redis cache service with connection pooling to the service collection
    /// Validates: Requirements 22.1, 22.2, 22.3
    /// </summary>
    public static IServiceCollection AddRedisCache(
        this IServiceCollection services, 
        Action<RedisConfiguration> configureOptions)
    {
        var config = new RedisConfiguration();
        configureOptions(config);
        
        // Register configuration
        services.AddSingleton(config);
        
        // Register connection multiplexer as singleton (connection pooling)
        services.AddSingleton<IConnectionMultiplexer>(sp =>
        {
            return RedisConnectionFactory.GetConnection(config);
        });
        
        // Register cache service
        services.AddSingleton<IRedisCacheService, RedisCacheService>();
        
        return services;
    }
}
