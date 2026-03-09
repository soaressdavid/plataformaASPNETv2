namespace ApiGateway.Configuration;

/// <summary>
/// Configuration for Redis connection used by rate limiting middleware.
/// </summary>
public class RedisConfiguration
{
    public string ConnectionString { get; set; } = "localhost:6379";
}
