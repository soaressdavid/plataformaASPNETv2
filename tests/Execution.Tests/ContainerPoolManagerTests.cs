using Xunit;
using Moq;
using Docker.DotNet;
using Docker.DotNet.Models;
using StackExchange.Redis;
using Microsoft.Extensions.Logging;
using Execution.Service.Services;

namespace Execution.Tests;

/// <summary>
/// Unit tests for ContainerPoolManager
/// Validates: Requirements 7.1, 7.2, 21.1
/// </summary>
public class ContainerPoolManagerTests
{
    private readonly Mock<IDockerClient> _mockDockerClient;
    private readonly Mock<IConnectionMultiplexer> _mockRedis;
    private readonly Mock<IJobQueueService> _mockJobQueue;
    private readonly Mock<ILogger<ContainerPoolManager>> _mockLogger;
    private readonly Mock<IDatabase> _mockDatabase;
    private readonly Mock<IContainerOperations> _mockContainers;

    public ContainerPoolManagerTests()
    {
        _mockDockerClient = new Mock<IDockerClient>();
        _mockRedis = new Mock<IConnectionMultiplexer>();
        _mockJobQueue = new Mock<IJobQueueService>();
        _mockLogger = new Mock<ILogger<ContainerPoolManager>>();
        _mockDatabase = new Mock<IDatabase>();
        _mockContainers = new Mock<IContainerOperations>();

        _mockRedis.Setup(r => r.GetDatabase(It.IsAny<int>(), It.IsAny<object>()))
            .Returns(_mockDatabase.Object);
        
        _mockDockerClient.Setup(d => d.Containers).Returns(_mockContainers.Object);
    }

    [Fact]
    public async Task InitializeWarmPoolAsync_CreatesCorrectNumberOfContainers()
    {
        // Arrange
        var createCount = 0;
        _mockContainers.Setup(c => c.CreateContainerAsync(
                It.IsAny<CreateContainerParameters>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(() => new CreateContainerResponse { ID = $"container_{++createCount}" });

        _mockContainers.Setup(c => c.StartContainerAsync(
                It.IsAny<string>(),
                It.IsAny<ContainerStartParameters>(),
                It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(true));

        var poolManager = new ContainerPoolManager(
            _mockDockerClient.Object,
            _mockRedis.Object,
            _mockJobQueue.Object,
            _mockLogger.Object);

        // Act
        await poolManager.InitializeWarmPoolAsync();

        // Assert
        var stats = await poolManager.GetPoolStatsAsync();
        Assert.Equal(10, stats.TotalContainers); // Warm pool size is 10
        Assert.Equal(10, stats.AvailableContainers);
        Assert.Equal(0, stats.InUseContainers);
    }

    [Fact]
    public async Task AcquireContainerAsync_ReturnsContainerFromPool()
    {
        // Arrange
        _mockContainers.Setup(c => c.CreateContainerAsync(
                It.IsAny<CreateContainerParameters>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new CreateContainerResponse { ID = "test-container" });

        _mockContainers.Setup(c => c.StartContainerAsync(
                It.IsAny<string>(),
                It.IsAny<ContainerStartParameters>(),
                It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(true));

        var poolManager = new ContainerPoolManager(
            _mockDockerClient.Object,
            _mockRedis.Object,
            _mockJobQueue.Object,
            _mockLogger.Object);

        await poolManager.InitializeWarmPoolAsync();

        // Act
        var containerId = await poolManager.AcquireContainerAsync();

        // Assert
        Assert.NotNull(containerId);
        var stats = await poolManager.GetPoolStatsAsync();
        Assert.Equal(9, stats.AvailableContainers);
        Assert.Equal(1, stats.InUseContainers);
    }

    [Fact]
    public async Task AcquireContainerAsync_WithSessionId_ReusesContainerForSameSession()
    {
        // Arrange
        var sessionId = "test-session-123";
        
        _mockContainers.Setup(c => c.CreateContainerAsync(
                It.IsAny<CreateContainerParameters>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new CreateContainerResponse { ID = "test-container" });

        _mockContainers.Setup(c => c.StartContainerAsync(
                It.IsAny<string>(),
                It.IsAny<ContainerStartParameters>(),
                It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(true));

        // Mock Redis to return the same container for the session
        _mockDatabase.Setup(d => d.StringGetAsync(
                It.Is<RedisKey>(k => k.ToString().Contains(sessionId)),
                It.IsAny<CommandFlags>()))
            .ReturnsAsync(RedisValue.Null);

        _mockDatabase.Setup(d => d.StringSetAsync(
                It.IsAny<RedisKey>(),
                It.IsAny<RedisValue>(),
                It.IsAny<TimeSpan?>(),
                It.IsAny<bool>(),
                It.IsAny<When>(),
                It.IsAny<CommandFlags>()))
            .ReturnsAsync(true);

        var poolManager = new ContainerPoolManager(
            _mockDockerClient.Object,
            _mockRedis.Object,
            _mockJobQueue.Object,
            _mockLogger.Object);

        await poolManager.InitializeWarmPoolAsync();

        // Act - First acquisition
        var containerId1 = await poolManager.AcquireContainerAsync(sessionId);
        
        // Setup Redis to return the container for subsequent calls
        _mockDatabase.Setup(d => d.StringGetAsync(
                It.Is<RedisKey>(k => k.ToString().Contains(sessionId)),
                It.IsAny<CommandFlags>()))
            .ReturnsAsync(new RedisValue(containerId1));

        // Act - Second acquisition with same session
        var containerId2 = await poolManager.AcquireContainerAsync(sessionId);

        // Assert - Should return the same container
        Assert.Equal(containerId1, containerId2);
    }

    [Fact]
    public async Task ReleaseContainerAsync_ReturnsContainerToPool()
    {
        // Arrange
        _mockContainers.Setup(c => c.CreateContainerAsync(
                It.IsAny<CreateContainerParameters>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new CreateContainerResponse { ID = "test-container" });

        _mockContainers.Setup(c => c.StartContainerAsync(
                It.IsAny<string>(),
                It.IsAny<ContainerStartParameters>(),
                It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(true));

        var poolManager = new ContainerPoolManager(
            _mockDockerClient.Object,
            _mockRedis.Object,
            _mockJobQueue.Object,
            _mockLogger.Object);

        await poolManager.InitializeWarmPoolAsync();
        var containerId = await poolManager.AcquireContainerAsync();

        // Act
        await poolManager.ReleaseContainerAsync(containerId!);

        // Assert
        var stats = await poolManager.GetPoolStatsAsync();
        Assert.Equal(10, stats.AvailableContainers);
        Assert.Equal(0, stats.InUseContainers);
    }

    [Fact]
    public async Task DestroyContainerAsync_RemovesContainerFromPool()
    {
        // Arrange
        var createCount = 0;
        _mockContainers.Setup(c => c.CreateContainerAsync(
                It.IsAny<CreateContainerParameters>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(() => new CreateContainerResponse { ID = $"container_{++createCount}" });

        _mockContainers.Setup(c => c.StartContainerAsync(
                It.IsAny<string>(),
                It.IsAny<ContainerStartParameters>(),
                It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(true));

        _mockContainers.Setup(c => c.StopContainerAsync(
                It.IsAny<string>(),
                It.IsAny<ContainerStopParameters>(),
                It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(true));

        _mockContainers.Setup(c => c.RemoveContainerAsync(
                It.IsAny<string>(),
                It.IsAny<ContainerRemoveParameters>(),
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _mockDatabase.Setup(d => d.StringGetAsync(
                It.IsAny<RedisKey>(),
                It.IsAny<CommandFlags>()))
            .ReturnsAsync(RedisValue.Null);

        _mockDatabase.Setup(d => d.KeyDeleteAsync(
                It.IsAny<RedisKey>(),
                It.IsAny<CommandFlags>()))
            .ReturnsAsync(true);

        var poolManager = new ContainerPoolManager(
            _mockDockerClient.Object,
            _mockRedis.Object,
            _mockJobQueue.Object,
            _mockLogger.Object);

        await poolManager.InitializeWarmPoolAsync();
        var containerId = await poolManager.AcquireContainerAsync();

        // Act
        await poolManager.DestroyContainerAsync(containerId!);

        // Assert
        var stats = await poolManager.GetPoolStatsAsync();
        Assert.Equal(9, stats.TotalContainers);
    }

    [Fact]
    public async Task GetPoolStatsAsync_ReturnsCorrectStatistics()
    {
        // Arrange
        var createCount = 0;
        _mockContainers.Setup(c => c.CreateContainerAsync(
                It.IsAny<CreateContainerParameters>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(() => new CreateContainerResponse { ID = $"container_{++createCount}" });

        _mockContainers.Setup(c => c.StartContainerAsync(
                It.IsAny<string>(),
                It.IsAny<ContainerStartParameters>(),
                It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(true));

        _mockJobQueue.Setup(q => q.GetQueueDepthAsync())
            .ReturnsAsync(5);

        var poolManager = new ContainerPoolManager(
            _mockDockerClient.Object,
            _mockRedis.Object,
            _mockJobQueue.Object,
            _mockLogger.Object);

        await poolManager.InitializeWarmPoolAsync();

        // Act
        var stats = await poolManager.GetPoolStatsAsync();

        // Assert
        Assert.Equal(10, stats.TotalContainers);
        Assert.Equal(10, stats.AvailableContainers);
        Assert.Equal(0, stats.InUseContainers);
        Assert.Equal(10, stats.WarmPoolSize);
        Assert.Equal(5, stats.QueueLength);
    }

    [Fact]
    public async Task PerformHealthCheckAsync_RemovesUnhealthyContainers()
    {
        // Arrange
        _mockContainers.Setup(c => c.CreateContainerAsync(
                It.IsAny<CreateContainerParameters>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new CreateContainerResponse { ID = "test-container" });

        _mockContainers.Setup(c => c.StartContainerAsync(
                It.IsAny<string>(),
                It.IsAny<ContainerStartParameters>(),
                It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(true));

        // Mock unhealthy container
        _mockContainers.Setup(c => c.InspectContainerAsync(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ContainerInspectResponse
            {
                State = new ContainerState { Status = "exited" }
            });

        _mockContainers.Setup(c => c.StopContainerAsync(
                It.IsAny<string>(),
                It.IsAny<ContainerStopParameters>(),
                It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(true));

        _mockContainers.Setup(c => c.RemoveContainerAsync(
                It.IsAny<string>(),
                It.IsAny<ContainerRemoveParameters>(),
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _mockDatabase.Setup(d => d.StringGetAsync(
                It.IsAny<RedisKey>(),
                It.IsAny<CommandFlags>()))
            .ReturnsAsync(RedisValue.Null);

        _mockDatabase.Setup(d => d.KeyDeleteAsync(
                It.IsAny<RedisKey>(),
                It.IsAny<CommandFlags>()))
            .ReturnsAsync(true);

        var poolManager = new ContainerPoolManager(
            _mockDockerClient.Object,
            _mockRedis.Object,
            _mockJobQueue.Object,
            _mockLogger.Object);

        await poolManager.InitializeWarmPoolAsync();

        // Act
        await poolManager.PerformHealthCheckAsync();

        // Assert
        var stats = await poolManager.GetPoolStatsAsync();
        Assert.Equal(0, stats.TotalContainers); // All containers should be removed as unhealthy
    }

    [Fact]
    public async Task AcquireContainerAsync_ReturnsNull_WhenPoolExhausted()
    {
        // Arrange
        _mockContainers.Setup(c => c.CreateContainerAsync(
                It.IsAny<CreateContainerParameters>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new CreateContainerResponse { ID = "test-container" });

        _mockContainers.Setup(c => c.StartContainerAsync(
                It.IsAny<string>(),
                It.IsAny<ContainerStartParameters>(),
                It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(true));

        _mockJobQueue.Setup(q => q.GetQueueDepthAsync())
            .ReturnsAsync(0);

        var poolManager = new ContainerPoolManager(
            _mockDockerClient.Object,
            _mockRedis.Object,
            _mockJobQueue.Object,
            _mockLogger.Object);

        await poolManager.InitializeWarmPoolAsync();

        // Acquire all containers
        for (int i = 0; i < 10; i++)
        {
            await poolManager.AcquireContainerAsync();
        }

        // Act - Try to acquire when pool is exhausted
        var containerId = await poolManager.AcquireContainerAsync();

        // Assert
        Assert.Null(containerId);
    }
}
