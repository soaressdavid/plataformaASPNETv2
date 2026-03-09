using Execution.Service.Models;
using Execution.Service.Services;
using Moq;
using StackExchange.Redis;
using System.Text.Json;
using ExecutionResult = Shared.Models.ExecutionResult;
using ExecutionJob = Shared.Models.ExecutionJob;
using ExecutionStatus = Shared.Models.ExecutionStatus;

namespace Execution.Tests;

public class RedisJobQueueServiceTests
{
    private readonly Mock<IConnectionMultiplexer> _mockRedis;
    private readonly Mock<IDatabase> _mockDb;
    private readonly RedisJobQueueService _service;

    public RedisJobQueueServiceTests()
    {
        _mockRedis = new Mock<IConnectionMultiplexer>();
        _mockDb = new Mock<IDatabase>();
        _mockRedis.Setup(r => r.GetDatabase(It.IsAny<int>(), It.IsAny<object>()))
            .Returns(_mockDb.Object);
        
        _service = new RedisJobQueueService(_mockRedis.Object);
    }

    [Fact]
    public async Task EnqueueJobAsync_WithValidJob_AddsToRedisQueue()
    {
        // Arrange
        var job = new ExecutionJob
        {
            JobId = Guid.NewGuid(),
            Code = "Console.WriteLine(\"Hello\");",
            Files = new List<string> { "Program.cs" },
            EntryPoint = "Program.cs",
            EnqueuedAt = DateTime.UtcNow
        };

        _mockDb.Setup(db => db.ListLeftPushAsync(
            It.IsAny<RedisKey>(),
            It.IsAny<RedisValue>(),
            It.IsAny<When>(),
            It.IsAny<CommandFlags>()))
            .ReturnsAsync(1);

        // Act
        await _service.EnqueueJobAsync(job);

        // Assert
        _mockDb.Verify(db => db.ListLeftPushAsync(
            It.Is<RedisKey>(k => k == "execution_queue"),
            It.Is<RedisValue>(v => v.ToString().Contains(job.JobId.ToString())),
            It.IsAny<When>(),
            It.IsAny<CommandFlags>()), Times.Once);
    }

    [Fact]
    public async Task EnqueueJobAsync_WithNullJob_ThrowsArgumentNullException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => 
            _service.EnqueueJobAsync(null!));
    }

    [Fact]
    public async Task DequeueJobAsync_WithJobInQueue_ReturnsJob()
    {
        // Arrange
        var job = new ExecutionJob
        {
            JobId = Guid.NewGuid(),
            Code = "Console.WriteLine(\"Hello\");",
            Files = new List<string> { "Program.cs" },
            EntryPoint = "Program.cs",
            EnqueuedAt = DateTime.UtcNow
        };

        var jobJson = JsonSerializer.Serialize(job);
        _mockDb.Setup(db => db.ListRightPopAsync(
            It.IsAny<RedisKey>(),
            It.IsAny<CommandFlags>()))
            .ReturnsAsync(new RedisValue(jobJson));

        // Act
        var result = await _service.DequeueJobAsync(TimeSpan.FromSeconds(5));

        // Assert
        Assert.NotNull(result);
        Assert.Equal(job.JobId, result.JobId);
        Assert.Equal(job.Code, result.Code);
    }

    [Fact]
    public async Task DequeueJobAsync_WithEmptyQueue_ReturnsNull()
    {
        // Arrange
        _mockDb.Setup(db => db.ListRightPopAsync(
            It.IsAny<RedisKey>(),
            It.IsAny<CommandFlags>()))
            .ReturnsAsync(RedisValue.Null);

        // Act
        var result = await _service.DequeueJobAsync(TimeSpan.FromSeconds(5));

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task StoreResultAsync_WithValidResult_StoresInRedisWithTTL()
    {
        // Arrange
        var result = new ExecutionResult
        {
            JobId = Guid.NewGuid(),
            Status = ExecutionStatus.Completed,
            Output = "Hello, World!",
            Error = null,
            ExitCode = 0,
            ExecutionTimeMs = 150,
            CompletedAt = DateTime.UtcNow
        };

        _mockDb.Setup(db => db.StringSetAsync(
            It.IsAny<RedisKey>(),
            It.IsAny<RedisValue>(),
            It.IsAny<TimeSpan?>(),
            It.IsAny<bool>(),
            It.IsAny<When>(),
            It.IsAny<CommandFlags>()))
            .ReturnsAsync(true);

        // Act
        await _service.StoreResultAsync(result);

        // Assert
        _mockDb.Verify(db => db.StringSetAsync(
            It.Is<RedisKey>(k => k == $"result:{result.JobId}"),
            It.Is<RedisValue>(v => v.ToString().Contains(result.JobId.ToString())),
            It.Is<TimeSpan?>(ttl => ttl == TimeSpan.FromMinutes(5)),
            It.IsAny<bool>(),
            It.IsAny<When>(),
            It.IsAny<CommandFlags>()), Times.Once);
    }

    [Fact]
    public async Task StoreResultAsync_WithNullResult_ThrowsArgumentNullException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => 
            _service.StoreResultAsync(null!));
    }

    [Fact]
    public async Task GetResultAsync_WithExistingResult_ReturnsResult()
    {
        // Arrange
        var jobId = Guid.NewGuid();
        var result = new ExecutionResult
        {
            JobId = jobId,
            Status = ExecutionStatus.Completed,
            Output = "Hello, World!",
            Error = null,
            ExitCode = 0,
            ExecutionTimeMs = 150,
            CompletedAt = DateTime.UtcNow
        };

        var resultJson = JsonSerializer.Serialize(result);
        _mockDb.Setup(db => db.StringGetAsync(
            It.IsAny<RedisKey>(),
            It.IsAny<CommandFlags>()))
            .ReturnsAsync(new RedisValue(resultJson));

        // Act
        var retrievedResult = await _service.GetResultAsync(jobId);

        // Assert
        Assert.NotNull(retrievedResult);
        Assert.Equal(result.JobId, retrievedResult.JobId);
        Assert.Equal(result.Status, retrievedResult.Status);
        Assert.Equal(result.Output, retrievedResult.Output);
    }

    [Fact]
    public async Task GetResultAsync_WithNonExistingResult_ReturnsNull()
    {
        // Arrange
        var jobId = Guid.NewGuid();
        _mockDb.Setup(db => db.StringGetAsync(
            It.IsAny<RedisKey>(),
            It.IsAny<CommandFlags>()))
            .ReturnsAsync(RedisValue.Null);

        // Act
        var result = await _service.GetResultAsync(jobId);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task EnqueueJobAsync_SerializesJobCorrectly()
    {
        // Arrange
        var job = new ExecutionJob
        {
            JobId = Guid.NewGuid(),
            Code = "var x = 42;",
            Files = new List<string> { "Program.cs", "Helper.cs" },
            EntryPoint = "Program.cs",
            EnqueuedAt = DateTime.UtcNow
        };

        RedisValue capturedValue = RedisValue.Null;
        _mockDb.Setup(db => db.ListLeftPushAsync(
            It.IsAny<RedisKey>(),
            It.IsAny<RedisValue>(),
            It.IsAny<When>(),
            It.IsAny<CommandFlags>()))
            .Callback<RedisKey, RedisValue, When, CommandFlags>((k, v, w, f) => capturedValue = v)
            .ReturnsAsync(1);

        // Act
        await _service.EnqueueJobAsync(job);

        // Assert
        var deserializedJob = JsonSerializer.Deserialize<ExecutionJob>(capturedValue.ToString());
        Assert.NotNull(deserializedJob);
        Assert.Equal(job.JobId, deserializedJob.JobId);
        Assert.Equal(job.Code, deserializedJob.Code);
        Assert.Equal(job.Files.Count, deserializedJob.Files.Count);
    }

    [Fact]
    public async Task GetJobAsync_WithJobInQueue_ReturnsJob()
    {
        // Arrange
        var targetJobId = Guid.NewGuid();
        var job1 = new ExecutionJob
        {
            JobId = Guid.NewGuid(),
            Code = "Console.WriteLine(\"Job 1\");",
            EnqueuedAt = DateTime.UtcNow
        };
        var job2 = new ExecutionJob
        {
            JobId = targetJobId,
            Code = "Console.WriteLine(\"Job 2\");",
            EnqueuedAt = DateTime.UtcNow
        };

        var jobs = new RedisValue[]
        {
            new RedisValue(JsonSerializer.Serialize(job1)),
            new RedisValue(JsonSerializer.Serialize(job2))
        };

        _mockDb.Setup(db => db.ListRangeAsync(
            It.IsAny<RedisKey>(),
            It.IsAny<long>(),
            It.IsAny<long>(),
            It.IsAny<CommandFlags>()))
            .ReturnsAsync(jobs);

        // Act
        var result = await _service.GetJobAsync(targetJobId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(targetJobId, result.JobId);
        Assert.Equal(job2.Code, result.Code);
    }

    [Fact]
    public async Task GetJobAsync_WithJobNotInQueue_ReturnsNull()
    {
        // Arrange
        var targetJobId = Guid.NewGuid();
        var job1 = new ExecutionJob
        {
            JobId = Guid.NewGuid(),
            Code = "Console.WriteLine(\"Job 1\");",
            EnqueuedAt = DateTime.UtcNow
        };

        var jobs = new RedisValue[]
        {
            new RedisValue(JsonSerializer.Serialize(job1))
        };

        _mockDb.Setup(db => db.ListRangeAsync(
            It.IsAny<RedisKey>(),
            It.IsAny<long>(),
            It.IsAny<long>(),
            It.IsAny<CommandFlags>()))
            .ReturnsAsync(jobs);

        // Act
        var result = await _service.GetJobAsync(targetJobId);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetJobAsync_WithEmptyQueue_ReturnsNull()
    {
        // Arrange
        var targetJobId = Guid.NewGuid();
        var jobs = Array.Empty<RedisValue>();

        _mockDb.Setup(db => db.ListRangeAsync(
            It.IsAny<RedisKey>(),
            It.IsAny<long>(),
            It.IsAny<long>(),
            It.IsAny<CommandFlags>()))
            .ReturnsAsync(jobs);

        // Act
        var result = await _service.GetJobAsync(targetJobId);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task RemoveJobAsync_WithJobInQueue_RemovesJob()
    {
        // Arrange
        var targetJobId = Guid.NewGuid();
        var job1 = new ExecutionJob
        {
            JobId = Guid.NewGuid(),
            Code = "Console.WriteLine(\"Job 1\");",
            EnqueuedAt = DateTime.UtcNow
        };
        var job2 = new ExecutionJob
        {
            JobId = targetJobId,
            Code = "Console.WriteLine(\"Job 2\");",
            EnqueuedAt = DateTime.UtcNow
        };

        var job2Json = JsonSerializer.Serialize(job2);
        var jobs = new RedisValue[]
        {
            new RedisValue(JsonSerializer.Serialize(job1)),
            new RedisValue(job2Json)
        };

        _mockDb.Setup(db => db.ListRangeAsync(
            It.IsAny<RedisKey>(),
            It.IsAny<long>(),
            It.IsAny<long>(),
            It.IsAny<CommandFlags>()))
            .ReturnsAsync(jobs);

        _mockDb.Setup(db => db.ListRemoveAsync(
            It.IsAny<RedisKey>(),
            It.IsAny<RedisValue>(),
            It.IsAny<long>(),
            It.IsAny<CommandFlags>()))
            .ReturnsAsync(1);

        // Act
        await _service.RemoveJobAsync(targetJobId);

        // Assert
        _mockDb.Verify(db => db.ListRemoveAsync(
            It.Is<RedisKey>(k => k == "execution_queue"),
            It.Is<RedisValue>(v => v.ToString() == job2Json),
            It.IsAny<long>(),
            It.IsAny<CommandFlags>()), Times.Once);
    }

    [Fact]
    public async Task RemoveJobAsync_WithJobNotInQueue_DoesNotRemoveAnything()
    {
        // Arrange
        var targetJobId = Guid.NewGuid();
        var job1 = new ExecutionJob
        {
            JobId = Guid.NewGuid(),
            Code = "Console.WriteLine(\"Job 1\");",
            EnqueuedAt = DateTime.UtcNow
        };

        var jobs = new RedisValue[]
        {
            new RedisValue(JsonSerializer.Serialize(job1))
        };

        _mockDb.Setup(db => db.ListRangeAsync(
            It.IsAny<RedisKey>(),
            It.IsAny<long>(),
            It.IsAny<long>(),
            It.IsAny<CommandFlags>()))
            .ReturnsAsync(jobs);

        // Act
        await _service.RemoveJobAsync(targetJobId);

        // Assert
        _mockDb.Verify(db => db.ListRemoveAsync(
            It.IsAny<RedisKey>(),
            It.IsAny<RedisValue>(),
            It.IsAny<long>(),
            It.IsAny<CommandFlags>()), Times.Never);
    }
}
