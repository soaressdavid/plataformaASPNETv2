using StackExchange.Redis;

namespace Shared.Configuration;

/// <summary>
/// Redis configuration options for distributed caching
/// Validates: Requirements 22.1, 22.2, 22.3
/// </summary>
public class RedisConfiguration
{
    public string ConnectionString { get; set; } = "localhost:6379";
    public bool UseCluster { get; set; } = true;
    public int ConnectTimeout { get; set; } = 5000;
    public int SyncTimeout { get; set; } = 5000;
    public int ConnectRetry { get; set; } = 3;
    public bool AbortOnConnectFail { get; set; } = false;
    public string? Password { get; set; }
    public bool AllowAdmin { get; set; } = false;
    
    /// <summary>
    /// Cache TTL configurations for different data types
    /// Validates: Requirements 22.4, 22.5, 22.6
    /// </summary>
    public CacheTTLConfiguration TTL { get; set; } = new();
}

public class CacheTTLConfiguration
{
    /// <summary>
    /// TTL for code execution results (Requirement 22.4)
    /// </summary>
    public TimeSpan ExecutionResults { get; set; } = TimeSpan.FromHours(1);
    
    /// <summary>
    /// TTL for leaderboard data (Requirement 22.5)
    /// </summary>
    public TimeSpan LeaderboardData { get; set; } = TimeSpan.FromMinutes(5);
    
    /// <summary>
    /// TTL for course content (Requirement 22.6)
    /// </summary>
    public TimeSpan CourseContent { get; set; } = TimeSpan.FromHours(24);
    
    /// <summary>
    /// TTL for SQL session databases (30 minutes as per context)
    /// </summary>
    public TimeSpan SqlSession { get; set; } = TimeSpan.FromMinutes(30);
    
    /// <summary>
    /// TTL for rate limiting data (1 hour)
    /// </summary>
    public TimeSpan RateLimit { get; set; } = TimeSpan.FromHours(1);
}

/// <summary>
/// Factory for creating and managing Redis connections with connection pooling
/// Validates: Requirements 22.1, 22.2, 22.3
/// </summary>
public static class RedisConnectionFactory
{
    private static IConnectionMultiplexer? _connection;
    private static readonly object _lock = new();
    
    /// <summary>
    /// Gets or creates a singleton Redis connection with connection pooling
    /// Thread-safe implementation using double-check locking pattern
    /// </summary>
    public static IConnectionMultiplexer GetConnection(RedisConfiguration config)
    {
        if (_connection == null || !_connection.IsConnected)
        {
            lock (_lock)
            {
                if (_connection == null || !_connection.IsConnected)
                {
                    _connection?.Dispose();
                    _connection = CreateConnection(config);
                }
            }
        }
        
        return _connection;
    }
    
    private static IConnectionMultiplexer CreateConnection(RedisConfiguration config)
    {
        var options = new ConfigurationOptions
        {
            EndPoints = { config.ConnectionString },
            ConnectTimeout = config.ConnectTimeout,
            SyncTimeout = config.SyncTimeout,
            ConnectRetry = config.ConnectRetry,
            AbortOnConnectFail = config.AbortOnConnectFail,
            AllowAdmin = config.AllowAdmin,
            
            // Connection pooling configuration
            // StackExchange.Redis automatically manages connection pooling
            // These settings optimize for high-throughput scenarios
            KeepAlive = 60, // Send keepalive every 60 seconds
            ReconnectRetryPolicy = new ExponentialRetry(5000), // Exponential backoff for reconnection
            
            // SSL/TLS configuration (disabled for internal cluster communication)
            Ssl = false,
            
            // Command map - disable dangerous commands in production
            CommandMap = CommandMap.Create(new HashSet<string>
            {
                "FLUSHDB", "FLUSHALL", "KEYS", "SHUTDOWN"
            }, available: false)
        };
        
        if (!string.IsNullOrEmpty(config.Password))
        {
            options.Password = config.Password;
        }
        
        var connection = ConnectionMultiplexer.Connect(options);
        
        // Register connection events for monitoring
        connection.ConnectionFailed += (sender, args) =>
        {
            Console.WriteLine($"Redis connection failed: {args.Exception?.Message}");
        };
        
        connection.ConnectionRestored += (sender, args) =>
        {
            Console.WriteLine($"Redis connection restored: {args.ConnectionType}");
        };
        
        connection.ErrorMessage += (sender, args) =>
        {
            Console.WriteLine($"Redis error: {args.Message}");
        };
        
        return connection;
    }
    
    /// <summary>
    /// Disposes the Redis connection (should only be called on application shutdown)
    /// </summary>
    public static void DisposeConnection()
    {
        lock (_lock)
        {
            _connection?.Dispose();
            _connection = null;
        }
    }
}
