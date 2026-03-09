using Execution.Service.Models;
using Execution.Service.Services;
using FsCheck;
using FsCheck.Xunit;
using Moq;
using StackExchange.Redis;
using System.Text.Json;
using ExecutionResult = Shared.Models.ExecutionResult;
using ExecutionJob = Shared.Models.ExecutionJob;
using ExecutionStatus = Shared.Models.ExecutionStatus;

namespace Execution.Tests;

/// <summary>
/// Property-based tests for job queue functionality.
/// Feature: aspnet-learning-platform
/// </summary>
public class JobQueuePropertiesTests
{
    /// <summary>
    /// Helper to create a mock Redis service
    /// </summary>
    private RedisJobQueueService CreateMockService(Mock<IDatabase> mockDb)
    {
        var mockRedis = new Mock<IConnectionMultiplexer>();
        mockRedis.Setup(r => r.GetDatabase(It.IsAny<int>(), It.IsAny<object>()))
            .Returns(mockDb.Object);
        return new RedisJobQueueService(mockRedis.Object);
    }

    /// <summary>
    /// Property 6: Code Execution Enqueueing
    /// **Validates: Requirements 3.1, 12.2**
    /// 
    /// For any code execution request, the platform should add a job to the queue 
    /// with the submitted code and return a job identifier.
    /// </summary>
    [Property(MaxTest = 100)]
    public void Property6_CodeExecutionEnqueueing_AddsJobToQueueWithJobId(NonEmptyString code, NonEmptyString entryPoint)
    {
        // Arrange
        var job = new ExecutionJob
        {
            JobId = Guid.NewGuid(),
            Code = code.Get,
            Files = new List<string> { "Program.cs" },
            EntryPoint = entryPoint.Get,
            EnqueuedAt = DateTime.UtcNow
        };

        var mockDb = new Mock<IDatabase>();
        RedisValue capturedValue = RedisValue.Null;
        mockDb.Setup(db => db.ListLeftPushAsync(
            It.IsAny<RedisKey>(),
            It.IsAny<RedisValue>(),
            It.IsAny<When>(),
            It.IsAny<CommandFlags>()))
            .Callback<RedisKey, RedisValue, When, CommandFlags>((k, v, w, f) => capturedValue = v)
            .ReturnsAsync(1);

        var service = CreateMockService(mockDb);

        // Act
        service.EnqueueJobAsync(job).Wait();

        // Assert - Verify job was added to queue
        mockDb.Verify(db => db.ListLeftPushAsync(
            It.Is<RedisKey>(k => k == "execution_queue"),
            It.IsAny<RedisValue>(),
            It.IsAny<When>(),
            It.IsAny<CommandFlags>()), Times.Once);

        // Assert - Verify the enqueued job contains the submitted code and has a job identifier
        var deserializedJob = JsonSerializer.Deserialize<ExecutionJob>(capturedValue.ToString());
        Assert.NotNull(deserializedJob);
        Assert.Equal(job.JobId, deserializedJob.JobId);
        Assert.Equal(job.Code, deserializedJob.Code);
        Assert.NotEqual(Guid.Empty, deserializedJob.JobId);
    }

    /// <summary>
    /// Property 7: Job Processing
    /// **Validates: Requirements 3.2, 12.3**
    /// 
    /// For any job in the queue, an available worker should dequeue it and process it, 
    /// returning execution results.
    /// </summary>
    [Property(MaxTest = 100)]
    public void Property7_JobProcessing_DequeuesJobAndReturnsResults(NonEmptyString code, NonEmptyString output)
    {
        // Arrange
        var job = new ExecutionJob
        {
            JobId = Guid.NewGuid(),
            Code = code.Get,
            Files = new List<string> { "Program.cs" },
            EntryPoint = "Program.cs",
            EnqueuedAt = DateTime.UtcNow
        };

        var result = new ExecutionResult
        {
            JobId = job.JobId,
            Status = ExecutionStatus.Completed,
            Output = output.Get,
            Error = null,
            ExitCode = 0,
            ExecutionTimeMs = 150,
            CompletedAt = DateTime.UtcNow
        };

        var mockDb = new Mock<IDatabase>();

        // Setup: Job is in the queue
        var jobJson = JsonSerializer.Serialize(job);
        mockDb.Setup(db => db.ListRightPopAsync(
            It.Is<RedisKey>(k => k == "execution_queue"),
            It.IsAny<CommandFlags>()))
            .ReturnsAsync(new RedisValue(jobJson));

        // Setup: Result storage
        mockDb.Setup(db => db.StringSetAsync(
            It.IsAny<RedisKey>(),
            It.IsAny<RedisValue>(),
            It.IsAny<TimeSpan?>(),
            It.IsAny<bool>(),
            It.IsAny<When>(),
            It.IsAny<CommandFlags>()))
            .ReturnsAsync(true);

        // Setup: Result retrieval
        var resultJson = JsonSerializer.Serialize(result);
        mockDb.Setup(db => db.StringGetAsync(
            It.Is<RedisKey>(k => k == $"result:{job.JobId}"),
            It.IsAny<CommandFlags>()))
            .ReturnsAsync(new RedisValue(resultJson));

        var service = CreateMockService(mockDb);

        // Act - Worker dequeues job
        var dequeuedJob = service.DequeueJobAsync(TimeSpan.FromSeconds(5)).Result;

        // Assert - Job was dequeued successfully
        Assert.NotNull(dequeuedJob);
        Assert.Equal(job.JobId, dequeuedJob.JobId);
        Assert.Equal(job.Code, dequeuedJob.Code);

        // Act - Worker stores execution result
        service.StoreResultAsync(result).Wait();

        // Act - Retrieve execution result
        var retrievedResult = service.GetResultAsync(job.JobId).Result;

        // Assert - Execution results are returned
        Assert.NotNull(retrievedResult);
        Assert.Equal(result.JobId, retrievedResult.JobId);
        Assert.Equal(result.Status, retrievedResult.Status);
        Assert.Equal(result.Output, retrievedResult.Output);
        Assert.Equal(result.Error, retrievedResult.Error);
        Assert.Equal(result.ExitCode, retrievedResult.ExitCode);
    }

    /// <summary>
    /// Property: Job Round Trip Preserves Data
    /// 
    /// For any job enqueued and then dequeued, the dequeued job should contain 
    /// the same data as the original job.
    /// </summary>
    [Property(MaxTest = 100)]
    public void JobRoundTrip_PreservesAllJobData(NonEmptyString code, NonEmptyString entryPoint)
    {
        // Arrange
        var job = new ExecutionJob
        {
            JobId = Guid.NewGuid(),
            Code = code.Get,
            Files = new List<string> { "Program.cs", "Helper.cs" },
            EntryPoint = entryPoint.Get,
            EnqueuedAt = DateTime.UtcNow
        };

        var mockDb = new Mock<IDatabase>();
        RedisValue capturedValue = RedisValue.Null;
        mockDb.Setup(db => db.ListLeftPushAsync(
            It.IsAny<RedisKey>(),
            It.IsAny<RedisValue>(),
            It.IsAny<When>(),
            It.IsAny<CommandFlags>()))
            .Callback<RedisKey, RedisValue, When, CommandFlags>((k, v, w, f) => capturedValue = v)
            .ReturnsAsync(1);

        mockDb.Setup(db => db.ListRightPopAsync(
            It.IsAny<RedisKey>(),
            It.IsAny<CommandFlags>()))
            .ReturnsAsync(() => capturedValue);

        var service = CreateMockService(mockDb);

        // Act
        service.EnqueueJobAsync(job).Wait();
        var dequeuedJob = service.DequeueJobAsync(TimeSpan.FromSeconds(5)).Result;

        // Assert - All job data is preserved
        Assert.NotNull(dequeuedJob);
        Assert.Equal(job.JobId, dequeuedJob.JobId);
        Assert.Equal(job.Code, dequeuedJob.Code);
        Assert.Equal(job.Files.Count, dequeuedJob.Files.Count);
        Assert.Equal(job.EntryPoint, dequeuedJob.EntryPoint);
        
        // Verify all files are preserved
        for (int i = 0; i < job.Files.Count; i++)
        {
            Assert.Equal(job.Files[i], dequeuedJob.Files[i]);
        }
    }

    /// <summary>
    /// Property: Result Storage and Retrieval Preserves Data
    /// 
    /// For any execution result stored and then retrieved, the retrieved result 
    /// should contain the same data as the original result.
    /// </summary>
    [Property(MaxTest = 100)]
    public void ResultRoundTrip_PreservesAllResultData(NonEmptyString output, int exitCode)
    {
        // Arrange
        var result = new ExecutionResult
        {
            JobId = Guid.NewGuid(),
            Status = ExecutionStatus.Completed,
            Output = output.Get,
            Error = null,
            ExitCode = exitCode,
            ExecutionTimeMs = 150,
            CompletedAt = DateTime.UtcNow
        };

        var mockDb = new Mock<IDatabase>();
        RedisValue capturedValue = RedisValue.Null;
        mockDb.Setup(db => db.StringSetAsync(
            It.IsAny<RedisKey>(),
            It.IsAny<RedisValue>(),
            It.IsAny<TimeSpan?>(),
            It.IsAny<bool>(),
            It.IsAny<When>(),
            It.IsAny<CommandFlags>()))
            .Callback<RedisKey, RedisValue, TimeSpan?, bool, When, CommandFlags>(
                (k, v, ttl, keepTtl, when, flags) => capturedValue = v)
            .ReturnsAsync(true);

        mockDb.Setup(db => db.StringGetAsync(
            It.IsAny<RedisKey>(),
            It.IsAny<CommandFlags>()))
            .ReturnsAsync(() => capturedValue);

        var service = CreateMockService(mockDb);

        // Act
        service.StoreResultAsync(result).Wait();
        var retrievedResult = service.GetResultAsync(result.JobId).Result;

        // Assert - All result data is preserved
        Assert.NotNull(retrievedResult);
        Assert.Equal(result.JobId, retrievedResult.JobId);
        Assert.Equal(result.Status, retrievedResult.Status);
        Assert.Equal(result.Output, retrievedResult.Output);
        Assert.Equal(result.Error, retrievedResult.Error);
        Assert.Equal(result.ExitCode, retrievedResult.ExitCode);
        Assert.Equal(result.ExecutionTimeMs, retrievedResult.ExecutionTimeMs);
    }
}
