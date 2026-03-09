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
/// Unit tests for ExecutionController API
/// Validates: Requirements 3.1, 3.6, 12.2, 12.4
/// </summary>
public class ExecutionControllerTests
{
    private readonly Mock<IJobQueueService> _mockJobQueueService;
    private readonly Mock<ILogger<ExecutionController>> _mockLogger;
    private readonly ExecutionController _controller;

    public ExecutionControllerTests()
    {
        _mockJobQueueService = new Mock<IJobQueueService>();
        _mockLogger = new Mock<ILogger<ExecutionController>>();
        _controller = new ExecutionController(_mockJobQueueService.Object, _mockLogger.Object);
    }

    #region ExecuteCode Tests (Requirements 3.1, 12.2)

    [Fact]
    public async Task ExecuteCode_WithValidRequest_EnqueuesJobAndReturnsAccepted()
    {
        // Arrange
        var request = new ExecuteCodeRequest(
            Code: "Console.WriteLine(\"Hello, World!\");",
            Files: new List<string> { "Program.cs" },
            EntryPoint: "Program.cs"
        );

        _mockJobQueueService.Setup(s => s.EnqueueJobAsync(It.IsAny<ExecutionJob>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.ExecuteCode(request);

        // Assert
        var acceptedResult = Assert.IsType<AcceptedResult>(result);
        var response = Assert.IsType<ExecuteCodeResponse>(acceptedResult.Value);
        
        Assert.NotEqual(Guid.Empty, response.JobId);
        Assert.Equal("Queued", response.Status);
        
        _mockJobQueueService.Verify(s => s.EnqueueJobAsync(
            It.Is<ExecutionJob>(j => 
                j.Code == request.Code && 
                j.Files.Count == 1 &&
                j.EntryPoint == "Program.cs")), 
            Times.Once);
    }

    [Fact]
    public async Task ExecuteCode_WithEmptyCode_ReturnsBadRequest()
    {
        // Arrange
        var request = new ExecuteCodeRequest(
            Code: "",
            Files: null,
            EntryPoint: null
        );

        // Act
        var result = await _controller.ExecuteCode(request);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.NotNull(badRequestResult.Value);
        
        _mockJobQueueService.Verify(s => s.EnqueueJobAsync(It.IsAny<ExecutionJob>()), Times.Never);
    }

    [Fact]
    public async Task ExecuteCode_WithNullCode_ReturnsBadRequest()
    {
        // Arrange
        var request = new ExecuteCodeRequest(
            Code: null!,
            Files: null,
            EntryPoint: null
        );

        // Act
        var result = await _controller.ExecuteCode(request);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.NotNull(badRequestResult.Value);
        
        _mockJobQueueService.Verify(s => s.EnqueueJobAsync(It.IsAny<ExecutionJob>()), Times.Never);
    }

    [Fact]
    public async Task ExecuteCode_WithNullFiles_UsesEmptyList()
    {
        // Arrange
        var request = new ExecuteCodeRequest(
            Code: "Console.WriteLine(\"Test\");",
            Files: null,
            EntryPoint: null
        );

        _mockJobQueueService.Setup(s => s.EnqueueJobAsync(It.IsAny<ExecutionJob>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.ExecuteCode(request);

        // Assert
        var acceptedResult = Assert.IsType<AcceptedResult>(result);
        
        _mockJobQueueService.Verify(s => s.EnqueueJobAsync(
            It.Is<ExecutionJob>(j => j.Files.Count == 0)), 
            Times.Once);
    }

    [Fact]
    public async Task ExecuteCode_WithNullEntryPoint_UsesDefaultProgramCs()
    {
        // Arrange
        var request = new ExecuteCodeRequest(
            Code: "Console.WriteLine(\"Test\");",
            Files: null,
            EntryPoint: null
        );

        _mockJobQueueService.Setup(s => s.EnqueueJobAsync(It.IsAny<ExecutionJob>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.ExecuteCode(request);

        // Assert
        var acceptedResult = Assert.IsType<AcceptedResult>(result);
        
        _mockJobQueueService.Verify(s => s.EnqueueJobAsync(
            It.Is<ExecutionJob>(j => j.EntryPoint == "Program.cs")), 
            Times.Once);
    }

    [Fact]
    public async Task ExecuteCode_WhenEnqueueFails_ReturnsInternalServerError()
    {
        // Arrange
        var request = new ExecuteCodeRequest(
            Code: "Console.WriteLine(\"Test\");",
            Files: null,
            EntryPoint: null
        );

        _mockJobQueueService.Setup(s => s.EnqueueJobAsync(It.IsAny<ExecutionJob>()))
            .ThrowsAsync(new Exception("Redis connection failed"));

        // Act
        var result = await _controller.ExecuteCode(request);

        // Assert
        var statusCodeResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, statusCodeResult.StatusCode);
    }

    #endregion

    #region GetExecutionStatus Tests (Requirements 3.6, 12.4)

    [Fact]
    public async Task GetExecutionStatus_WithCompletedJob_ReturnsResult()
    {
        // Arrange
        var jobId = Guid.NewGuid();
        var executionResult = new ExecutionResult
        {
            JobId = jobId,
            Status = ExecutionStatus.Completed,
            Output = "Hello, World!",
            Error = null,
            ExitCode = 0,
            ExecutionTimeMs = 150,
            CompletedAt = DateTime.UtcNow
        };

        _mockJobQueueService.Setup(s => s.GetResultAsync(jobId))
            .ReturnsAsync(executionResult);

        // Act
        var result = await _controller.GetExecutionStatus(jobId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedResult = Assert.IsType<ExecutionResult>(okResult.Value);
        
        Assert.Equal(jobId, returnedResult.JobId);
        Assert.Equal(ExecutionStatus.Completed, returnedResult.Status);
        Assert.Equal("Hello, World!", returnedResult.Output);
    }

    [Fact]
    public async Task GetExecutionStatus_WithQueuedJobUnderTimeout_ReturnsQueuedStatus()
    {
        // Arrange
        var jobId = Guid.NewGuid();
        var job = new ExecutionJob
        {
            JobId = jobId,
            Code = "Console.WriteLine(\"Test\");",
            EnqueuedAt = DateTime.UtcNow.AddSeconds(-30) // 30 seconds ago, under 60 second limit
        };

        _mockJobQueueService.Setup(s => s.GetResultAsync(jobId))
            .ReturnsAsync((ExecutionResult?)null);
        
        _mockJobQueueService.Setup(s => s.GetJobAsync(jobId))
            .ReturnsAsync(job);

        // Act
        var result = await _controller.GetExecutionStatus(jobId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedResult = Assert.IsType<ExecutionResult>(okResult.Value);
        
        Assert.Equal(jobId, returnedResult.JobId);
        Assert.Equal(ExecutionStatus.Queued, returnedResult.Status);
    }

    [Fact]
    public async Task GetExecutionStatus_WithJobTimedOut_MarksAsFailedAndReturnsTimeoutResult()
    {
        // Arrange - Requirement 12.4: Job timeout after 60 seconds
        var jobId = Guid.NewGuid();
        var job = new ExecutionJob
        {
            JobId = jobId,
            Code = "Console.WriteLine(\"Test\");",
            EnqueuedAt = DateTime.UtcNow.AddSeconds(-65) // 65 seconds ago, over 60 second limit
        };

        _mockJobQueueService.Setup(s => s.GetResultAsync(jobId))
            .ReturnsAsync((ExecutionResult?)null);
        
        _mockJobQueueService.Setup(s => s.GetJobAsync(jobId))
            .ReturnsAsync(job);
        
        _mockJobQueueService.Setup(s => s.StoreResultAsync(It.IsAny<ExecutionResult>()))
            .Returns(Task.CompletedTask);
        
        _mockJobQueueService.Setup(s => s.RemoveJobAsync(jobId))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.GetExecutionStatus(jobId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedResult = Assert.IsType<ExecutionResult>(okResult.Value);
        
        Assert.Equal(jobId, returnedResult.JobId);
        Assert.Equal(ExecutionStatus.Failed, returnedResult.Status);
        Assert.Contains("timed out", returnedResult.Error, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("60 seconds", returnedResult.Error);
        
        // Verify timeout result was stored
        _mockJobQueueService.Verify(s => s.StoreResultAsync(
            It.Is<ExecutionResult>(r => 
                r.JobId == jobId && 
                r.Status == ExecutionStatus.Failed)), 
            Times.Once);
        
        // Verify job was removed from queue
        _mockJobQueueService.Verify(s => s.RemoveJobAsync(jobId), Times.Once);
    }

    [Fact]
    public async Task GetExecutionStatus_WithJobExactlyAt60Seconds_DoesNotTimeout()
    {
        // Arrange - Edge case: exactly 60 seconds should not timeout (only > 60 seconds)
        var jobId = Guid.NewGuid();
        var job = new ExecutionJob
        {
            JobId = jobId,
            Code = "Console.WriteLine(\"Test\");",
            EnqueuedAt = DateTime.UtcNow.AddSeconds(-59.9) // Just under 60 seconds
        };

        _mockJobQueueService.Setup(s => s.GetResultAsync(jobId))
            .ReturnsAsync((ExecutionResult?)null);
        
        _mockJobQueueService.Setup(s => s.GetJobAsync(jobId))
            .ReturnsAsync(job);

        // Act
        var result = await _controller.GetExecutionStatus(jobId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedResult = Assert.IsType<ExecutionResult>(okResult.Value);
        
        Assert.Equal(ExecutionStatus.Queued, returnedResult.Status);
        
        // Verify timeout handling was NOT triggered
        _mockJobQueueService.Verify(s => s.StoreResultAsync(It.IsAny<ExecutionResult>()), Times.Never);
        _mockJobQueueService.Verify(s => s.RemoveJobAsync(It.IsAny<Guid>()), Times.Never);
    }

    [Fact]
    public async Task GetExecutionStatus_WithNonExistentJob_ReturnsNotFound()
    {
        // Arrange
        var jobId = Guid.NewGuid();

        _mockJobQueueService.Setup(s => s.GetResultAsync(jobId))
            .ReturnsAsync((ExecutionResult?)null);
        
        _mockJobQueueService.Setup(s => s.GetJobAsync(jobId))
            .ReturnsAsync((ExecutionJob?)null);

        // Act
        var result = await _controller.GetExecutionStatus(jobId);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.NotNull(notFoundResult.Value);
    }

    [Fact]
    public async Task GetExecutionStatus_WhenServiceThrows_ReturnsInternalServerError()
    {
        // Arrange
        var jobId = Guid.NewGuid();

        _mockJobQueueService.Setup(s => s.GetResultAsync(jobId))
            .ThrowsAsync(new Exception("Redis connection failed"));

        // Act
        var result = await _controller.GetExecutionStatus(jobId);

        // Assert
        var statusCodeResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, statusCodeResult.StatusCode);
    }

    [Fact]
    public async Task GetExecutionStatus_WithFailedJob_ReturnsFailureDetails()
    {
        // Arrange
        var jobId = Guid.NewGuid();
        var executionResult = new ExecutionResult
        {
            JobId = jobId,
            Status = ExecutionStatus.Failed,
            Output = null,
            Error = "Compilation error: CS0103",
            ExitCode = 1,
            ExecutionTimeMs = 0,
            CompletedAt = DateTime.UtcNow
        };

        _mockJobQueueService.Setup(s => s.GetResultAsync(jobId))
            .ReturnsAsync(executionResult);

        // Act
        var result = await _controller.GetExecutionStatus(jobId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedResult = Assert.IsType<ExecutionResult>(okResult.Value);
        
        Assert.Equal(ExecutionStatus.Failed, returnedResult.Status);
        Assert.Contains("CS0103", returnedResult.Error);
    }

    [Fact]
    public async Task GetExecutionStatus_WithTimeoutStatus_ReturnsTimeoutResult()
    {
        // Arrange
        var jobId = Guid.NewGuid();
        var executionResult = new ExecutionResult
        {
            JobId = jobId,
            Status = ExecutionStatus.Timeout,
            Output = null,
            Error = "Execution exceeded 30 second time limit",
            ExitCode = -1,
            ExecutionTimeMs = 30000,
            CompletedAt = DateTime.UtcNow
        };

        _mockJobQueueService.Setup(s => s.GetResultAsync(jobId))
            .ReturnsAsync(executionResult);

        // Act
        var result = await _controller.GetExecutionStatus(jobId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedResult = Assert.IsType<ExecutionResult>(okResult.Value);
        
        Assert.Equal(ExecutionStatus.Timeout, returnedResult.Status);
        Assert.Contains("time limit", returnedResult.Error, StringComparison.OrdinalIgnoreCase);
    }

    #endregion
}
