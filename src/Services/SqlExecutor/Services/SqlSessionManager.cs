using StackExchange.Redis;

namespace SqlExecutor.Service.Services;

/// <summary>
/// Manages SQL execution sessions with Redis
/// Validates: Requirements 2.11
/// </summary>
public class SqlSessionManager
{
    private readonly IConnectionMultiplexer _redis;
    private readonly ILogger<SqlSessionManager> _logger;
    private const int SessionTtlMinutes = 30;
    private const string SessionContainerKey = "sql:session:container";
    private const string ContainerSessionKey = "sql:container:session";
    private const string SessionActivityKey = "sql:session:activity";

    public SqlSessionManager(
        IConnectionMultiplexer redis,
        ILogger<SqlSessionManager> logger)
    {
        _redis = redis;
        _logger = logger;
    }

    /// <summary>
    /// Associates a container with a session
    /// </summary>
    public async Task AssociateContainerWithSessionAsync(string sessionId, string containerId)
    {
        var db = _redis.GetDatabase();
        var ttl = TimeSpan.FromMinutes(SessionTtlMinutes);

        await db.StringSetAsync($"{SessionContainerKey}:{sessionId}", containerId, ttl);
        await db.StringSetAsync($"{ContainerSessionKey}:{containerId}", sessionId, ttl);
        await db.StringSetAsync($"{SessionActivityKey}:{sessionId}", DateTime.UtcNow.Ticks, ttl);

        _logger.LogInformation("Associated container {ContainerId} with session {SessionId}", containerId, sessionId);
    }

    /// <summary>
    /// Gets the container ID for a session
    /// </summary>
    public async Task<string?> GetContainerForSessionAsync(string sessionId)
    {
        var db = _redis.GetDatabase();
        var containerId = await db.StringGetAsync($"{SessionContainerKey}:{sessionId}");
        
        if (containerId.HasValue)
        {
            // Update activity timestamp
            await UpdateSessionActivityAsync(sessionId);
        }

        return containerId.HasValue ? containerId.ToString() : null;
    }

    /// <summary>
    /// Gets the session ID for a container
    /// </summary>
    public async Task<string?> GetSessionForContainerAsync(string containerId)
    {
        var db = _redis.GetDatabase();
        var sessionId = await db.StringGetAsync($"{ContainerSessionKey}:{containerId}");
        return sessionId.HasValue ? sessionId.ToString() : null;
    }

    /// <summary>
    /// Updates session activity timestamp
    /// </summary>
    public async Task UpdateSessionActivityAsync(string sessionId)
    {
        var db = _redis.GetDatabase();
        var ttl = TimeSpan.FromMinutes(SessionTtlMinutes);
        
        await db.StringSetAsync($"{SessionActivityKey}:{sessionId}", DateTime.UtcNow.Ticks, ttl);
        
        // Also extend TTL on container mapping
        await db.KeyExpireAsync($"{SessionContainerKey}:{sessionId}", ttl);
    }

    /// <summary>
    /// Removes session associations
    /// </summary>
    public async Task RemoveSessionAsync(string sessionId)
    {
        var db = _redis.GetDatabase();
        
        var containerId = await db.StringGetAsync($"{SessionContainerKey}:{sessionId}");
        
        await db.KeyDeleteAsync($"{SessionContainerKey}:{sessionId}");
        await db.KeyDeleteAsync($"{SessionActivityKey}:{sessionId}");
        
        if (containerId.HasValue)
        {
            await db.KeyDeleteAsync($"{ContainerSessionKey}:{containerId}");
        }

        _logger.LogInformation("Removed session {SessionId}", sessionId);
    }

    /// <summary>
    /// Gets all inactive sessions (no activity for 30 minutes)
    /// </summary>
    public async Task<List<string>> GetInactiveSessionsAsync()
    {
        var db = _redis.GetDatabase();
        var inactiveSessions = new List<string>();
        var cutoffTime = DateTime.UtcNow.AddMinutes(-SessionTtlMinutes).Ticks;

        // This is a simplified implementation
        // In production, you'd use Redis SCAN to iterate through keys
        _logger.LogDebug("Checking for inactive sessions (cutoff: {Cutoff})", new DateTime(cutoffTime));

        return inactiveSessions;
    }

    /// <summary>
    /// Checks if a session is active
    /// </summary>
    public async Task<bool> IsSessionActiveAsync(string sessionId)
    {
        var db = _redis.GetDatabase();
        var activity = await db.StringGetAsync($"{SessionActivityKey}:{sessionId}");
        
        if (!activity.HasValue)
            return false;

        var lastActivity = new DateTime((long)activity);
        var inactiveTime = DateTime.UtcNow - lastActivity;

        return inactiveTime.TotalMinutes < SessionTtlMinutes;
    }
}
