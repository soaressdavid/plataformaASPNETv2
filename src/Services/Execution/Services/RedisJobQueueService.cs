namespace Execution.Service.Services;

/// <summary>
/// Type alias for RedisJobQueueService from Shared.Services
/// </summary>
public class RedisJobQueueService : Shared.Services.RedisJobQueueService, IJobQueueService
{
    public RedisJobQueueService(StackExchange.Redis.IConnectionMultiplexer redis)
        : base(redis)
    {
    }
}
