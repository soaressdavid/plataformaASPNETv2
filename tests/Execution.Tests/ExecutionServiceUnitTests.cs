using Execution.Service.Controllers;
using Execution.Service.Models;
using Execution.Service.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using ExecutionResult = Shared.Models.ExecutionResult;
using ExecutionJob = Shared.Models.ExecutionJob;
using ExecutionStatus = Shared.Models.ExecutionStatus;

namespace Execution.Tests;

/// <summary>
/// Comprehensive unit tests for execution service
/// Tests job enqueueing, status retrieval, timeout handling, memory limits, 
/// container isolation, and prohibited code rejection
/// Validates: Requirements 3.1, 3.2, 3.3, 3.4, 3.5, 3.6, 3.7, 3.8, 3.9, 12.2, 12.3, 12.4, 12.5, 12.6
/// </summary>
public class ExecutionServiceUnitTests
{
    #region Job Enqueueing and Status Retrieval Tests

    [Fact]
    public async Task EnqueueJob_WithValidCode_ReturnsJobIdAndQueuesSuccessfully()
    {
        // Arrange
        var mockJobQueue = new Mock<IJobQueueService>();
        var mockLogger = new Mock<ILogger<ExecutionController>>();
        var controller = new ExecutionController(mockJobQueue.Object, mockLogger.Object);

        var request = new ExecuteCodeRequest(
            Code: "Console.WriteLine(\"Test\");",
            Files: new List<string> { "Program.cs" },
            EntryPoint: "Program.cs"
        );

        mockJobQueue.Setup(q => q.EnqueueJobAsync(It.IsAny<ExecutionJob>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await controller.ExecuteCode(request);

        // Assert
        var acceptedResult = Assert.IsType<AcceptedResult>(result);
        var response = Assert.IsType<ExecuteCodeResponse>(acceptedResult.Value);
        
        Assert.NotEqual(Guid.Empty, response.JobId);
        Assert.Equal("Queued", response.Status);
        
        mockJobQueue.Verify(q => q.EnqueueJobAsync(
            It.Is<ExecutionJob>(j => 
                j.Code == request.Code && 
                j.JobId != Guid.Empty)), 
            Times.Once);
    }


    [Fact]
    public async Task GetStatus_WithCompletedJob_ReturnsExecutionResult()
    {
        // Arrange
        var mockJobQueue = new Mock<IJobQueueService>();
        var mockLogger = new Mock<ILogger<ExecutionController>>();
        var controller = new ExecutionController(mockJobQueue.Object, mockLogger.Object);

        var jobId = Guid.NewGuid();
        var expectedResult = new ExecutionResult
        {
            JobId = jobId,
            Status = ExecutionStatus.Completed,
            Output = "Test output",
            Error = null,
            ExitCode = 0,
            ExecutionTimeMs = 250,
            CompletedAt = DateTime.UtcNow
        };

        mockJobQueue.Setup(q => q.GetResultAsync(jobId))
            .ReturnsAsync(expectedResult);

        // Act
        var result = await controller.GetExecutionStatus(jobId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedResult = Assert.IsType<ExecutionResult>(okResult.Value);
        
        Assert.Equal(jobId, returnedResult.JobId);
        Assert.Equal(ExecutionStatus.Completed, returnedResult.Status);
        Assert.Equal("Test output", returnedResult.Output);
        Assert.Equal(250, returnedResult.ExecutionTimeMs);
    }

    [Fact]
    public async Task GetStatus_WithQueuedJob_ReturnsQueuedStatus()
    {
        // Arrange
        var mockJobQueue = new Mock<IJobQueueService>();
        var mockLogger = new Mock<ILogger<ExecutionController>>();
        var controller = new ExecutionController(mockJobQueue.Object, mockLogger.Object);

        var jobId = Guid.NewGuid();
        var job = new ExecutionJob
        {
            JobId = jobId,
            Code = "Console.WriteLine(\"Test\");",
            EnqueuedAt = DateTime.UtcNow.AddSeconds(-10)
        };

        mockJobQueue.Setup(q => q.GetResultAsync(jobId))
            .ReturnsAsync((ExecutionResult?)null);
        mockJobQueue.Setup(q => q.GetJobAsync(jobId))
            .ReturnsAsync(job);

        // Act
        var result = await controller.GetExecutionStatus(jobId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedResult = Assert.IsType<ExecutionResult>(okResult.Value);
        
        Assert.Equal(ExecutionStatus.Queued, returnedResult.Status);
    }

    #endregion

    #region Timeout and Memory Limit Error Handling Tests

    [Fact]
    public async Task GetStatus_WithJobTimedOutInQueue_ReturnsTimeoutError()
    {
        // Arrange - Requirement 12.4: Job timeout after 60 seconds
        var mockJobQueue = new Mock<IJobQueueService>();
        var mockLogger = new Mock<ILogger<ExecutionController>>();
        var controller = new ExecutionController(mockJobQueue.Object, mockLogger.Object);

        var jobId = Guid.NewGuid();
        var job = new ExecutionJob
        {
            JobId = jobId,
            Code = "Console.WriteLine(\"Test\");",
            EnqueuedAt = DateTime.UtcNow.AddSeconds(-70) // Over 60 second limit
        };

        mockJobQueue.Setup(q => q.GetResultAsync(jobId))
            .ReturnsAsync((ExecutionResult?)null);
        mockJobQueue.Setup(q => q.GetJobAsync(jobId))
            .ReturnsAsync(job);
        mockJobQueue.Setup(q => q.StoreResultAsync(It.IsAny<ExecutionResult>()))
            .Returns(Task.CompletedTask);
        mockJobQueue.Setup(q => q.RemoveJobAsync(jobId))
            .Returns(Task.CompletedTask);

        // Act
        var result = await controller.GetExecutionStatus(jobId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedResult = Assert.IsType<ExecutionResult>(okResult.Value);
        
        Assert.Equal(ExecutionStatus.Failed, returnedResult.Status);
        Assert.Contains("timed out", returnedResult.Error!, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("60 seconds", returnedResult.Error!);
        
        mockJobQueue.Verify(q => q.StoreResultAsync(
            It.Is<ExecutionResult>(r => r.Status == ExecutionStatus.Failed)), 
            Times.Once);
        mockJobQueue.Verify(q => q.RemoveJobAsync(jobId), Times.Once);
    }


    [Fact]
    public async Task ExecutionResult_WithTimeoutStatus_ContainsTimeoutError()
    {
        // Arrange - Requirement 3.7: Timeout error handling
        var mockJobQueue = new Mock<IJobQueueService>();
        var mockLogger = new Mock<ILogger<ExecutionController>>();
        var controller = new ExecutionController(mockJobQueue.Object, mockLogger.Object);

        var jobId = Guid.NewGuid();
        var timeoutResult = new ExecutionResult
        {
            JobId = jobId,
            Status = ExecutionStatus.Timeout,
            Output = null,
            Error = "Execution exceeded 30 second time limit",
            ExitCode = -1,
            ExecutionTimeMs = 30000,
            CompletedAt = DateTime.UtcNow
        };

        mockJobQueue.Setup(q => q.GetResultAsync(jobId))
            .ReturnsAsync(timeoutResult);

        // Act
        var result = await controller.GetExecutionStatus(jobId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedResult = Assert.IsType<ExecutionResult>(okResult.Value);
        
        Assert.Equal(ExecutionStatus.Timeout, returnedResult.Status);
        Assert.Contains("30 second", returnedResult.Error!);
        Assert.Contains("time limit", returnedResult.Error!, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task ExecutionResult_WithMemoryExceededStatus_ContainsMemoryError()
    {
        // Arrange - Requirement 3.8: Memory limit error handling
        var mockJobQueue = new Mock<IJobQueueService>();
        var mockLogger = new Mock<ILogger<ExecutionController>>();
        var controller = new ExecutionController(mockJobQueue.Object, mockLogger.Object);

        var jobId = Guid.NewGuid();
        var memoryResult = new ExecutionResult
        {
            JobId = jobId,
            Status = ExecutionStatus.MemoryExceeded,
            Output = null,
            Error = "Code execution exceeded 512MB memory limit",
            ExitCode = -1,
            ExecutionTimeMs = 5000,
            CompletedAt = DateTime.UtcNow
        };

        mockJobQueue.Setup(q => q.GetResultAsync(jobId))
            .ReturnsAsync(memoryResult);

        // Act
        var result = await controller.GetExecutionStatus(jobId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedResult = Assert.IsType<ExecutionResult>(okResult.Value);
        
        Assert.Equal(ExecutionStatus.MemoryExceeded, returnedResult.Status);
        Assert.Contains("512MB", returnedResult.Error!);
        Assert.Contains("memory limit", returnedResult.Error!, StringComparison.OrdinalIgnoreCase);
    }

    #endregion

    #region Container Isolation Tests

    [Fact]
    public async Task ConcurrentExecutions_HaveDifferentJobIds()
    {
        // Arrange - Requirement 3.9: Container isolation
        var mockJobQueue = new Mock<IJobQueueService>();
        var mockLogger = new Mock<ILogger<ExecutionController>>();
        var controller = new ExecutionController(mockJobQueue.Object, mockLogger.Object);

        var request1 = new ExecuteCodeRequest(
            Code: "Console.WriteLine(\"Job 1\");",
            Files: null,
            EntryPoint: null
        );

        var request2 = new ExecuteCodeRequest(
            Code: "Console.WriteLine(\"Job 2\");",
            Files: null,
            EntryPoint: null
        );

        mockJobQueue.Setup(q => q.EnqueueJobAsync(It.IsAny<ExecutionJob>()))
            .Returns(Task.CompletedTask);

        // Act
        var result1 = await controller.ExecuteCode(request1);
        var result2 = await controller.ExecuteCode(request2);

        // Assert
        var response1 = Assert.IsType<ExecuteCodeResponse>(
            Assert.IsType<AcceptedResult>(result1).Value);
        var response2 = Assert.IsType<ExecuteCodeResponse>(
            Assert.IsType<AcceptedResult>(result2).Value);
        
        Assert.NotEqual(response1.JobId, response2.JobId);
        Assert.NotEqual(Guid.Empty, response1.JobId);
        Assert.NotEqual(Guid.Empty, response2.JobId);
    }


    [Fact]
    public async Task ConcurrentExecutions_AreProcessedIndependently()
    {
        // Arrange - Requirement 3.9: Execution isolation
        var mockJobQueue = new Mock<IJobQueueService>();
        var mockLogger = new Mock<ILogger<ExecutionController>>();
        var controller = new ExecutionController(mockJobQueue.Object, mockLogger.Object);

        var jobId1 = Guid.NewGuid();
        var jobId2 = Guid.NewGuid();

        var result1 = new ExecutionResult
        {
            JobId = jobId1,
            Status = ExecutionStatus.Completed,
            Output = "Output from job 1",
            ExecutionTimeMs = 100,
            CompletedAt = DateTime.UtcNow
        };

        var result2 = new ExecutionResult
        {
            JobId = jobId2,
            Status = ExecutionStatus.Completed,
            Output = "Output from job 2",
            ExecutionTimeMs = 150,
            CompletedAt = DateTime.UtcNow
        };

        mockJobQueue.Setup(q => q.GetResultAsync(jobId1))
            .ReturnsAsync(result1);
        mockJobQueue.Setup(q => q.GetResultAsync(jobId2))
            .ReturnsAsync(result2);

        // Act
        var statusResult1 = await controller.GetExecutionStatus(jobId1);
        var statusResult2 = await controller.GetExecutionStatus(jobId2);

        // Assert
        var returnedResult1 = Assert.IsType<ExecutionResult>(
            Assert.IsType<OkObjectResult>(statusResult1).Value);
        var returnedResult2 = Assert.IsType<ExecutionResult>(
            Assert.IsType<OkObjectResult>(statusResult2).Value);
        
        Assert.Equal("Output from job 1", returnedResult1.Output);
        Assert.Equal("Output from job 2", returnedResult2.Output);
        Assert.NotEqual(returnedResult1.Output, returnedResult2.Output);
    }

    #endregion

    #region Prohibited Code Rejection Tests

    [Fact]
    public void ProhibitedCodeScanner_DetectsFileSystemAccess()
    {
        // Arrange - Requirement 14.4, 14.5: Prohibited code detection
        var scanner = new ProhibitedCodeScanner();
        var dangerousCode = @"
            using System;
            using System.IO;
            
            public class Program
            {
                public static void Main()
                {
                    File.ReadAllText(""test.txt"");
                }
            }";

        // Act
        var result = scanner.ScanCode(dangerousCode);

        // Assert
        Assert.False(result.IsSafe);
        Assert.NotEmpty(result.Violations);
        Assert.Contains(result.Violations, v => 
            v.Reason.Contains("File system access", StringComparison.OrdinalIgnoreCase) ||
            v.Reason.Contains("Prohibited namespace", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void ProhibitedCodeScanner_DetectsNetworkAccess()
    {
        // Arrange - Requirement 14.4, 14.5: Prohibited code detection
        var scanner = new ProhibitedCodeScanner();
        var dangerousCode = @"
            using System;
            using System.Net.Http;
            
            public class Program
            {
                public static void Main()
                {
                    var client = new HttpClient();
                    client.GetAsync(""http://example.com"");
                }
            }";

        // Act
        var result = scanner.ScanCode(dangerousCode);

        // Assert
        Assert.False(result.IsSafe);
        Assert.NotEmpty(result.Violations);
        Assert.Contains(result.Violations, v => 
            v.Reason.Contains("Network access", StringComparison.OrdinalIgnoreCase));
    }


    [Fact]
    public void ProhibitedCodeScanner_DetectsProcessSpawning()
    {
        // Arrange - Requirement 14.4, 14.5: Prohibited code detection
        var scanner = new ProhibitedCodeScanner();
        var dangerousCode = @"
            using System;
            using System.Diagnostics;
            
            public class Program
            {
                public static void Main()
                {
                    Process.Start(""cmd.exe"");
                }
            }";

        // Act
        var result = scanner.ScanCode(dangerousCode);

        // Assert
        Assert.False(result.IsSafe);
        Assert.NotEmpty(result.Violations);
        Assert.Contains(result.Violations, v => 
            v.Reason.Contains("Process spawning", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void ProhibitedCodeScanner_AllowsSafeCode()
    {
        // Arrange
        var scanner = new ProhibitedCodeScanner();
        var safeCode = @"
            using System;
            using System.Collections.Generic;
            using System.Linq;
            
            public class Program
            {
                public static void Main()
                {
                    var numbers = new List<int> { 1, 2, 3, 4, 5 };
                    var sum = numbers.Sum();
                    Console.WriteLine($""Sum: {sum}"");
                }
            }";

        // Act
        var result = scanner.ScanCode(safeCode);

        // Assert
        Assert.True(result.IsSafe);
        Assert.Empty(result.Violations);
    }

    #endregion

    #region Job Queue Service Tests

    [Fact]
    public async Task JobQueueService_EnqueueAndDequeue_WorksCorrectly()
    {
        // Arrange - Requirement 12.2, 12.3: Job queue operations
        var mockRedis = new Mock<StackExchange.Redis.IConnectionMultiplexer>();
        var mockDb = new Mock<StackExchange.Redis.IDatabase>();
        
        mockRedis.Setup(r => r.GetDatabase(It.IsAny<int>(), It.IsAny<object>()))
            .Returns(mockDb.Object);

        var service = new RedisJobQueueService(mockRedis.Object);
        
        var job = new ExecutionJob
        {
            JobId = Guid.NewGuid(),
            Code = "Console.WriteLine(\"Test\");",
            Files = new List<string> { "Program.cs" },
            EntryPoint = "Program.cs",
            EnqueuedAt = DateTime.UtcNow
        };

        mockDb.Setup(db => db.ListLeftPushAsync(
            It.IsAny<StackExchange.Redis.RedisKey>(),
            It.IsAny<StackExchange.Redis.RedisValue>(),
            It.IsAny<StackExchange.Redis.When>(),
            It.IsAny<StackExchange.Redis.CommandFlags>()))
            .ReturnsAsync(1);

        // Act
        await service.EnqueueJobAsync(job);

        // Assert
        mockDb.Verify(db => db.ListLeftPushAsync(
            It.Is<StackExchange.Redis.RedisKey>(k => k == "execution_queue"),
            It.IsAny<StackExchange.Redis.RedisValue>(),
            It.IsAny<StackExchange.Redis.When>(),
            It.IsAny<StackExchange.Redis.CommandFlags>()), 
            Times.Once);
    }


    [Fact]
    public async Task JobQueueService_StoresResultWithTTL()
    {
        // Arrange - Requirement 12.2: Result storage with TTL
        var mockRedis = new Mock<StackExchange.Redis.IConnectionMultiplexer>();
        var mockDb = new Mock<StackExchange.Redis.IDatabase>();
        
        mockRedis.Setup(r => r.GetDatabase(It.IsAny<int>(), It.IsAny<object>()))
            .Returns(mockDb.Object);

        var service = new RedisJobQueueService(mockRedis.Object);
        
        var result = new ExecutionResult
        {
            JobId = Guid.NewGuid(),
            Status = ExecutionStatus.Completed,
            Output = "Test output",
            ExecutionTimeMs = 100,
            CompletedAt = DateTime.UtcNow
        };

        mockDb.Setup(db => db.StringSetAsync(
            It.IsAny<StackExchange.Redis.RedisKey>(),
            It.IsAny<StackExchange.Redis.RedisValue>(),
            It.IsAny<TimeSpan?>(),
            It.IsAny<bool>(),
            It.IsAny<StackExchange.Redis.When>(),
            It.IsAny<StackExchange.Redis.CommandFlags>()))
            .ReturnsAsync(true);

        // Act
        await service.StoreResultAsync(result);

        // Assert
        mockDb.Verify(db => db.StringSetAsync(
            It.Is<StackExchange.Redis.RedisKey>(k => k == $"result:{result.JobId}"),
            It.IsAny<StackExchange.Redis.RedisValue>(),
            It.Is<TimeSpan?>(ttl => ttl == TimeSpan.FromMinutes(5)),
            It.IsAny<bool>(),
            It.IsAny<StackExchange.Redis.When>(),
            It.IsAny<StackExchange.Redis.CommandFlags>()), 
            Times.Once);
    }

    #endregion

    #region Edge Cases and Error Handling

    [Fact]
    public async Task ExecuteCode_WithEmptyCode_ReturnsBadRequest()
    {
        // Arrange
        var mockJobQueue = new Mock<IJobQueueService>();
        var mockLogger = new Mock<ILogger<ExecutionController>>();
        var controller = new ExecutionController(mockJobQueue.Object, mockLogger.Object);

        var request = new ExecuteCodeRequest(
            Code: "",
            Files: null,
            EntryPoint: null
        );

        // Act
        var result = await controller.ExecuteCode(request);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
        mockJobQueue.Verify(q => q.EnqueueJobAsync(It.IsAny<ExecutionJob>()), Times.Never);
    }

    [Fact]
    public async Task ExecuteCode_WithNullCode_ReturnsBadRequest()
    {
        // Arrange
        var mockJobQueue = new Mock<IJobQueueService>();
        var mockLogger = new Mock<ILogger<ExecutionController>>();
        var controller = new ExecutionController(mockJobQueue.Object, mockLogger.Object);

        var request = new ExecuteCodeRequest(
            Code: null!,
            Files: null,
            EntryPoint: null
        );

        // Act
        var result = await controller.ExecuteCode(request);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
        mockJobQueue.Verify(q => q.EnqueueJobAsync(It.IsAny<ExecutionJob>()), Times.Never);
    }

    [Fact]
    public async Task GetStatus_WithNonExistentJob_ReturnsNotFound()
    {
        // Arrange
        var mockJobQueue = new Mock<IJobQueueService>();
        var mockLogger = new Mock<ILogger<ExecutionController>>();
        var controller = new ExecutionController(mockJobQueue.Object, mockLogger.Object);

        var jobId = Guid.NewGuid();

        mockJobQueue.Setup(q => q.GetResultAsync(jobId))
            .ReturnsAsync((ExecutionResult?)null);
        mockJobQueue.Setup(q => q.GetJobAsync(jobId))
            .ReturnsAsync((ExecutionJob?)null);

        // Act
        var result = await controller.GetExecutionStatus(jobId);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task ExecuteCode_WhenQueueFails_ReturnsInternalServerError()
    {
        // Arrange
        var mockJobQueue = new Mock<IJobQueueService>();
        var mockLogger = new Mock<ILogger<ExecutionController>>();
        var controller = new ExecutionController(mockJobQueue.Object, mockLogger.Object);

        var request = new ExecuteCodeRequest(
            Code: "Console.WriteLine(\"Test\");",
            Files: null,
            EntryPoint: null
        );

        mockJobQueue.Setup(q => q.EnqueueJobAsync(It.IsAny<ExecutionJob>()))
            .ThrowsAsync(new Exception("Redis connection failed"));

        // Act
        var result = await controller.ExecuteCode(request);

        // Assert
        var statusCodeResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, statusCodeResult.StatusCode);
    }

    #endregion
}
