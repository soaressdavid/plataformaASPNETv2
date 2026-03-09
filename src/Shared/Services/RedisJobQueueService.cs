using StackExchange.Redis;
using System.Text.Json;
using Shared.Interfaces;
using Shared.Models;

namespace Shared.Services;

/// <summary>
/// Redis-based implementation of job queue service
/// </summary>
public class RedisJobQueueService : IJobQueueService
{
    private readonly IConnectionMultiplexer _redis;
    private readonly IDatabase _db;
    private const string QueueKey = "execution_queue";
    private const string ResultKeyPrefix = "result:";
    private static readonly TimeSpan ResultTTL = TimeSpan.FromMinutes(5);

    public RedisJobQueueService(IConnectionMultiplexer redis)
    {
        _redis = redis ?? throw new ArgumentNullException(nameof(redis));
        _db = _redis.GetDatabase();
    }

    public async Task EnqueueJobAsync(ExecutionJob job)
    {
        if (job == null)
            throw new ArgumentNullException(nameof(job));

        var json = JsonSerializer.Serialize(job);
        await _db.ListLeftPushAsync(QueueKey, json);
    }

    public async Task<ExecutionJob?> DequeueJobAsync(TimeSpan timeout)
    {
        var result = await _db.ListRightPopAsync(QueueKey);
        if (result.IsNullOrEmpty)
        {
            return null;
        }

        return JsonSerializer.Deserialize<ExecutionJob>(result.ToString());
    }

    public async Task StoreResultAsync(ExecutionResult result)
    {
        if (result == null)
            throw new ArgumentNullException(nameof(result));

        var json = JsonSerializer.Serialize(result);
        var key = $"{ResultKeyPrefix}{result.JobId}";
        await _db.StringSetAsync(key, json, ResultTTL);
    }

    public async Task<ExecutionResult?> GetResultAsync(Guid jobId)
    {
        var key = $"{ResultKeyPrefix}{jobId}";
        var result = await _db.StringGetAsync(key);
        
        if (result.IsNullOrEmpty)
        {
            return null;
        }

        return JsonSerializer.Deserialize<ExecutionResult>(result.ToString());
    }

    public async Task<ExecutionJob?> GetJobAsync(Guid jobId)
    {
        // Get all jobs from the queue
        var jobs = await _db.ListRangeAsync(QueueKey);
        
        // Search for the job with matching ID
        foreach (var jobJson in jobs)
        {
            if (!jobJson.IsNullOrEmpty)
            {
                var job = JsonSerializer.Deserialize<ExecutionJob>(jobJson.ToString());
                if (job != null && job.JobId == jobId)
                {
                    return job;
                }
            }
        }
        
        return null;
    }

    public async Task RemoveJobAsync(Guid jobId)
    {
        // Get all jobs from the queue
        var jobs = await _db.ListRangeAsync(QueueKey);
        
        // Search for the job with matching ID
        foreach (var jobJson in jobs)
        {
            if (!jobJson.IsNullOrEmpty)
            {
                var job = JsonSerializer.Deserialize<ExecutionJob>(jobJson.ToString());
                if (job != null && job.JobId == jobId)
                {
                    // Remove the job from the queue
                    await _db.ListRemoveAsync(QueueKey, jobJson);
                    return;
                }
            }
        }
    }

    public async Task<long> GetQueueDepthAsync()
    {
        return await _db.ListLengthAsync(QueueKey);
    }
}
