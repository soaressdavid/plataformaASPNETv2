namespace Execution.Service.Services;

/// <summary>
/// Interface for managing a pool of Docker containers for code execution
/// Validates: Requirements 7.1, 7.2, 21.1
/// </summary>
public interface IContainerPoolManager
{
    /// <summary>
    /// Initializes the warm pool with pre-initialized containers
    /// </summary>
    Task InitializeWarmPoolAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Acquires a container from the pool for execution
    /// Returns container ID or null if pool is exhausted
    /// </summary>
    Task<string?> AcquireContainerAsync(string? sessionId = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Releases a container back to the pool for reuse
    /// </summary>
    Task ReleaseContainerAsync(string containerId, string? sessionId = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Destroys a container and removes it from the pool
    /// </summary>
    Task DestroyContainerAsync(string containerId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the current pool statistics
    /// </summary>
    Task<ContainerPoolStats> GetPoolStatsAsync();

    /// <summary>
    /// Performs health check on pool containers
    /// </summary>
    Task PerformHealthCheckAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Cleans up expired containers and sessions
    /// </summary>
    Task CleanupExpiredContainersAsync(CancellationToken cancellationToken = default);
}

/// <summary>
/// Statistics about the container pool
/// </summary>
public class ContainerPoolStats
{
    public int TotalContainers { get; set; }
    public int AvailableContainers { get; set; }
    public int InUseContainers { get; set; }
    public int WarmPoolSize { get; set; }
    public long QueueLength { get; set; }
    public DateTime LastScalingAction { get; set; }
}
