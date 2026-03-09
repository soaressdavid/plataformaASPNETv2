using Docker.DotNet;
using Docker.DotNet.Models;
using StackExchange.Redis;
using System.Collections.Concurrent;

namespace Execution.Service.Services;

/// <summary>
/// Manages a pool of Docker containers for code execution with warm pool, auto-scaling, and session reuse
/// Validates: Requirements 7.1, 7.2, 21.1
/// </summary>
public class ContainerPoolManager : IContainerPoolManager, IDisposable
{
    private readonly IDockerClient _dockerClient;
    private readonly IConnectionMultiplexer _redis;
    private readonly IJobQueueService _jobQueueService;
    private readonly ILogger<ContainerPoolManager> _logger;
    
    // Configuration constants
    private const int WarmPoolSize = 10;
    private const int MaxPoolSize = 100;
    private const int SessionTtlMinutes = 5;
    private const int ContainerHealthCheckIntervalSeconds = 30;
    private const int CleanupIntervalSeconds = 60;
    
    // Pool management
    private readonly ConcurrentQueue<string> _availableContainers = new();
    private readonly ConcurrentDictionary<string, ContainerInfo> _allContainers = new();
    private readonly SemaphoreSlim _poolSemaphore = new(MaxPoolSize);
    private readonly SemaphoreSlim _scalingSemaphore = new(1, 1);
    
    // Redis keys
    private const string SessionContainerKey = "execution:session:container";
    private const string ContainerSessionKey = "execution:container:session";
    
    private DateTime _lastScalingAction = DateTime.UtcNow;
    private bool _disposed;

    public ContainerPoolManager(
        IDockerClient dockerClient,
        IConnectionMultiplexer redis,
        IJobQueueService jobQueueService,
        ILogger<ContainerPoolManager> logger)
    {
        _dockerClient = dockerClient;
        _redis = redis;
        _jobQueueService = jobQueueService;
        _logger = logger;
    }

    /// <summary>
    /// Initializes the warm pool with pre-initialized containers
    /// Validates: Requirement 7.1 (warm pool with 10 pre-initialized containers)
    /// </summary>
    public async Task InitializeWarmPoolAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Initializing warm pool with {Size} containers", WarmPoolSize);

        var tasks = new List<Task>();
        for (int i = 0; i < WarmPoolSize; i++)
        {
            tasks.Add(CreateAndAddContainerAsync(cancellationToken));
        }

        await Task.WhenAll(tasks);
        
        _logger.LogInformation("Warm pool initialized with {Count} containers", _availableContainers.Count);
    }

    /// <summary>
    /// Acquires a container from the pool, with session reuse support
    /// Validates: Requirements 7.1, 7.2 (container reuse for same user session with 5 min TTL)
    /// </summary>
    public async Task<string?> AcquireContainerAsync(string? sessionId = null, CancellationToken cancellationToken = default)
    {
        // Check if session has an existing container (session reuse)
        if (!string.IsNullOrEmpty(sessionId))
        {
            var existingContainer = await GetSessionContainerAsync(sessionId);
            if (existingContainer != null && _allContainers.ContainsKey(existingContainer))
            {
                _logger.LogInformation("Reusing container {ContainerId} for session {SessionId}", existingContainer, sessionId);
                
                // Update session TTL
                await UpdateSessionTtlAsync(sessionId, existingContainer);
                
                return existingContainer;
            }
        }

        // Try to get container from available pool
        if (_availableContainers.TryDequeue(out var containerId))
        {
            if (_allContainers.TryGetValue(containerId, out var containerInfo))
            {
                containerInfo.InUse = true;
                containerInfo.LastUsed = DateTime.UtcNow;
                
                // Associate with session if provided
                if (!string.IsNullOrEmpty(sessionId))
                {
                    await AssociateContainerWithSessionAsync(sessionId, containerId);
                }
                
                _logger.LogInformation("Acquired container {ContainerId} from pool", containerId);
                return containerId;
            }
        }

        // Pool exhausted - try to scale up
        await TryScaleUpAsync(cancellationToken);

        // Try again after scaling
        if (_availableContainers.TryDequeue(out containerId))
        {
            if (_allContainers.TryGetValue(containerId, out var containerInfo))
            {
                containerInfo.InUse = true;
                containerInfo.LastUsed = DateTime.UtcNow;
                
                if (!string.IsNullOrEmpty(sessionId))
                {
                    await AssociateContainerWithSessionAsync(sessionId, containerId);
                }
                
                return containerId;
            }
        }

        _logger.LogWarning("Container pool exhausted, no containers available");
        return null;
    }

    /// <summary>
    /// Releases a container back to the pool for reuse
    /// </summary>
    public async Task ReleaseContainerAsync(string containerId, string? sessionId = null, CancellationToken cancellationToken = default)
    {
        if (!_allContainers.TryGetValue(containerId, out var containerInfo))
        {
            _logger.LogWarning("Attempted to release unknown container {ContainerId}", containerId);
            return;
        }

        // If session is provided, keep the association for reuse (with TTL)
        if (!string.IsNullOrEmpty(sessionId))
        {
            containerInfo.InUse = false;
            containerInfo.LastUsed = DateTime.UtcNow;
            _logger.LogInformation("Released container {ContainerId} but keeping session {SessionId} association", containerId, sessionId);
            return;
        }

        // No session - clean and return to pool
        await CleanContainerAsync(containerId, cancellationToken);
        
        containerInfo.InUse = false;
        containerInfo.LastUsed = DateTime.UtcNow;
        _availableContainers.Enqueue(containerId);
        
        _logger.LogInformation("Released container {ContainerId} back to pool", containerId);
    }

    /// <summary>
    /// Destroys a container and removes it from the pool
    /// </summary>
    public async Task DestroyContainerAsync(string containerId, CancellationToken cancellationToken = default)
    {
        try
        {
            // Remove from tracking
            _allContainers.TryRemove(containerId, out _);
            
            // Remove session associations
            await RemoveContainerSessionAsync(containerId);
            
            // Stop and remove container
            await _dockerClient.Containers.StopContainerAsync(
                containerId,
                new ContainerStopParameters { WaitBeforeKillSeconds = 5 },
                cancellationToken);
            
            await _dockerClient.Containers.RemoveContainerAsync(
                containerId,
                new ContainerRemoveParameters { Force = true },
                cancellationToken);
            
            _poolSemaphore.Release();
            
            _logger.LogInformation("Destroyed container {ContainerId}", containerId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to destroy container {ContainerId}", containerId);
        }
    }

    /// <summary>
    /// Gets current pool statistics
    /// </summary>
    public async Task<ContainerPoolStats> GetPoolStatsAsync()
    {
        var queueDepth = await _jobQueueService.GetQueueDepthAsync();
        
        var inUse = _allContainers.Values.Count(c => c.InUse);
        var total = _allContainers.Count;
        
        return new ContainerPoolStats
        {
            TotalContainers = total,
            AvailableContainers = _availableContainers.Count,
            InUseContainers = inUse,
            WarmPoolSize = WarmPoolSize,
            QueueLength = queueDepth,
            LastScalingAction = _lastScalingAction
        };
    }

    /// <summary>
    /// Performs health check on pool containers
    /// </summary>
    public async Task PerformHealthCheckAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Performing health check on {Count} containers", _allContainers.Count);

        var unhealthyContainers = new List<string>();

        foreach (var kvp in _allContainers)
        {
            try
            {
                var container = await _dockerClient.Containers.InspectContainerAsync(kvp.Key, cancellationToken);
                
                if (container.State.Status != "running")
                {
                    unhealthyContainers.Add(kvp.Key);
                    _logger.LogWarning("Container {ContainerId} is not running (status: {Status})", kvp.Key, container.State.Status);
                }
            }
            catch (Exception ex)
            {
                unhealthyContainers.Add(kvp.Key);
                _logger.LogError(ex, "Health check failed for container {ContainerId}", kvp.Key);
            }
        }

        // Remove unhealthy containers
        foreach (var containerId in unhealthyContainers)
        {
            await DestroyContainerAsync(containerId, cancellationToken);
        }

        if (unhealthyContainers.Any())
        {
            _logger.LogInformation("Removed {Count} unhealthy containers", unhealthyContainers.Count);
        }
    }

    /// <summary>
    /// Cleans up expired containers and sessions
    /// Validates: Requirement 7.2 (5 min TTL for session containers)
    /// </summary>
    public async Task CleanupExpiredContainersAsync(CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;
        var expiredContainers = _allContainers
            .Where(kvp => !kvp.Value.InUse && (now - kvp.Value.LastUsed).TotalMinutes > SessionTtlMinutes)
            .Select(kvp => kvp.Key)
            .ToList();

        foreach (var containerId in expiredContainers)
        {
            _logger.LogInformation("Cleaning up expired container {ContainerId}", containerId);
            await DestroyContainerAsync(containerId, cancellationToken);
        }

        // Scale down if we have too many idle containers
        await TryScaleDownAsync(cancellationToken);
    }

    /// <summary>
    /// Auto-scaling based on queue length
    /// Validates: Requirement 21.1 (auto-scaling based on queue length, max 100 containers)
    /// </summary>
    private async Task TryScaleUpAsync(CancellationToken cancellationToken)
    {
        if (!await _scalingSemaphore.WaitAsync(0, cancellationToken))
        {
            return; // Another scaling operation in progress
        }

        try
        {
            var stats = await GetPoolStatsAsync();
            
            // Don't scale if at max capacity
            if (stats.TotalContainers >= MaxPoolSize)
            {
                _logger.LogWarning("Cannot scale up: at maximum pool size ({Max})", MaxPoolSize);
                return;
            }

            // Scale up if queue is building up
            var scaleUpNeeded = stats.QueueLength > stats.AvailableContainers;
            
            if (scaleUpNeeded)
            {
                var containersToAdd = Math.Min(
                    (int)stats.QueueLength - stats.AvailableContainers,
                    MaxPoolSize - stats.TotalContainers
                );

                _logger.LogInformation("Scaling up: adding {Count} containers (queue: {Queue}, available: {Available})",
                    containersToAdd, stats.QueueLength, stats.AvailableContainers);

                var tasks = new List<Task>();
                for (int i = 0; i < containersToAdd; i++)
                {
                    tasks.Add(CreateAndAddContainerAsync(cancellationToken));
                }

                await Task.WhenAll(tasks);
                _lastScalingAction = DateTime.UtcNow;
            }
        }
        finally
        {
            _scalingSemaphore.Release();
        }
    }

    /// <summary>
    /// Scales down when there are too many idle containers
    /// </summary>
    private async Task TryScaleDownAsync(CancellationToken cancellationToken)
    {
        if (!await _scalingSemaphore.WaitAsync(0, cancellationToken))
        {
            return;
        }

        try
        {
            var stats = await GetPoolStatsAsync();
            
            // Keep at least warm pool size
            if (stats.TotalContainers <= WarmPoolSize)
            {
                return;
            }

            // Scale down if we have too many idle containers (more than 2x warm pool size)
            var excessContainers = stats.AvailableContainers - (WarmPoolSize * 2);
            
            if (excessContainers > 0)
            {
                _logger.LogInformation("Scaling down: removing {Count} excess containers", excessContainers);

                for (int i = 0; i < excessContainers; i++)
                {
                    if (_availableContainers.TryDequeue(out var containerId))
                    {
                        await DestroyContainerAsync(containerId, cancellationToken);
                    }
                }

                _lastScalingAction = DateTime.UtcNow;
            }
        }
        finally
        {
            _scalingSemaphore.Release();
        }
    }

    /// <summary>
    /// Creates a new container and adds it to the pool
    /// </summary>
    private async Task CreateAndAddContainerAsync(CancellationToken cancellationToken)
    {
        await _poolSemaphore.WaitAsync(cancellationToken);

        try
        {
            // Create container with resource limits
            var createParams = new CreateContainerParameters
            {
                Image = "mcr.microsoft.com/dotnet/sdk:8.0",
                HostConfig = new HostConfig
                {
                    Memory = 512 * 1024 * 1024, // 512MB
                    NanoCPUs = 1_000_000_000, // 1 CPU core
                    NetworkMode = "none" // No network access
                },
                Cmd = new[] { "tail", "-f", "/dev/null" }, // Keep container running
                WorkingDir = "/workspace"
            };

            var response = await _dockerClient.Containers.CreateContainerAsync(createParams, cancellationToken);
            var containerId = response.ID;

            // Start container
            await _dockerClient.Containers.StartContainerAsync(
                containerId,
                new ContainerStartParameters(),
                cancellationToken);

            // Add to pool
            var containerInfo = new ContainerInfo
            {
                ContainerId = containerId,
                CreatedAt = DateTime.UtcNow,
                LastUsed = DateTime.UtcNow,
                InUse = false
            };

            _allContainers[containerId] = containerInfo;
            _availableContainers.Enqueue(containerId);

            _logger.LogInformation("Created and added container {ContainerId} to pool", containerId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create container");
            throw;
        }
        finally
        {
            _poolSemaphore.Release();
        }
    }

    /// <summary>
    /// Cleans a container by removing all files in workspace
    /// </summary>
    private async Task CleanContainerAsync(string containerId, CancellationToken cancellationToken)
    {
        try
        {
            var execParams = new ContainerExecCreateParameters
            {
                Cmd = new[] { "sh", "-c", "rm -rf /workspace/* /workspace/.*" },
                AttachStdout = false,
                AttachStderr = false
            };

            var execResponse = await _dockerClient.Exec.ExecCreateContainerAsync(containerId, execParams, cancellationToken);
            await _dockerClient.Exec.StartContainerExecAsync(execResponse.ID, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to clean container {ContainerId}", containerId);
        }
    }

    // Redis session management methods

    private async Task<string?> GetSessionContainerAsync(string sessionId)
    {
        var db = _redis.GetDatabase();
        var containerId = await db.StringGetAsync($"{SessionContainerKey}:{sessionId}");
        return containerId.HasValue ? containerId.ToString() : null;
    }

    private async Task AssociateContainerWithSessionAsync(string sessionId, string containerId)
    {
        var db = _redis.GetDatabase();
        var ttl = TimeSpan.FromMinutes(SessionTtlMinutes);
        
        await db.StringSetAsync($"{SessionContainerKey}:{sessionId}", containerId, ttl);
        await db.StringSetAsync($"{ContainerSessionKey}:{containerId}", sessionId, ttl);
    }

    private async Task UpdateSessionTtlAsync(string sessionId, string containerId)
    {
        var db = _redis.GetDatabase();
        var ttl = TimeSpan.FromMinutes(SessionTtlMinutes);
        
        await db.KeyExpireAsync($"{SessionContainerKey}:{sessionId}", ttl);
        await db.KeyExpireAsync($"{ContainerSessionKey}:{containerId}", ttl);
    }

    private async Task RemoveContainerSessionAsync(string containerId)
    {
        var db = _redis.GetDatabase();
        var sessionId = await db.StringGetAsync($"{ContainerSessionKey}:{containerId}");
        
        if (sessionId.HasValue)
        {
            await db.KeyDeleteAsync($"{SessionContainerKey}:{sessionId}");
        }
        
        await db.KeyDeleteAsync($"{ContainerSessionKey}:{containerId}");
    }

    public void Dispose()
    {
        if (_disposed) return;

        _poolSemaphore.Dispose();
        _scalingSemaphore.Dispose();
        _disposed = true;
    }
}

/// <summary>
/// Information about a container in the pool
/// </summary>
internal class ContainerInfo
{
    public required string ContainerId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastUsed { get; set; }
    public bool InUse { get; set; }
}
