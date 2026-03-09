using Shared.Models;

namespace Shared.Interfaces;

/// <summary>
/// Interface for job queue operations
/// </summary>
public interface IJobQueueService
{
    /// <summary>
    /// Enqueues a job for execution
    /// </summary>
    Task EnqueueJobAsync(ExecutionJob job);

    /// <summary>
    /// Dequeues a job from the queue (blocking operation)
    /// </summary>
    Task<ExecutionJob?> DequeueJobAsync(TimeSpan timeout);

    /// <summary>
    /// Stores execution result in cache
    /// </summary>
    Task StoreResultAsync(ExecutionResult result);

    /// <summary>
    /// Retrieves execution result from cache
    /// </summary>
    Task<ExecutionResult?> GetResultAsync(Guid jobId);

    /// <summary>
    /// Gets a specific job from the queue by ID
    /// </summary>
    Task<ExecutionJob?> GetJobAsync(Guid jobId);

    /// <summary>
    /// Removes a specific job from the queue
    /// </summary>
    Task RemoveJobAsync(Guid jobId);

    /// <summary>
    /// Gets the current depth of the execution queue
    /// </summary>
    Task<long> GetQueueDepthAsync();
}
