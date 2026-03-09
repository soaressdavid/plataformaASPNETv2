using Execution.Service.Models;
using Execution.Service.Services;
using Microsoft.Extensions.Logging;
using Moq;
using Worker.Service;
using Xunit;
using IProhibitedCodeScanner = Shared.Interfaces.IProhibitedCodeScanner;
using CodeScanResult = Shared.Interfaces.CodeScanResult;
using CodeViolation = Shared.Interfaces.CodeViolation;
using ExecutionResult = Shared.Models.ExecutionResult;
using ExecutionStatus = Shared.Models.ExecutionStatus;
using ExecutionJob = Shared.Models.ExecutionJob;

namespace Execution.Tests;

/// <summary>
/// Unit tests for ExecutionWorker
/// Tests job processing, error handling, and requeue logic
/// </summary>
public class ExecutionWorkerTests
{
    private readonly Mock<ILogger<ExecutionWorker>> _mockLogger;
    private readonly Mock<IJobQueueService> _mockJobQueue;
    private readonly Mock<IProhibitedCodeScanner> _mockCodeScanner;
    private readonly Mock<IDockerContainerManager> _mockContainerManager;

    public ExecutionWorkerTests()
    {
        _mockLogger = new Mock<ILogger<ExecutionWorker>>();
        _mockJobQueue = new Mock<IJobQueueService>();
        _mockCodeScanner = new Mock<IProhibitedCodeScanner>();
        _mockContainerManager = new Mock<IDockerContainerManager>();
    }

    [Fact]
    public async Task ProcessJob_WithSafeCode_ExecutesSuccessfully()
    {
        // Arrange
        var job = new ExecutionJob
        {
            JobId = Guid.NewGuid(),
            Code = "Console.WriteLine(\"Hello World\");",
            Files = new List<string>(),
            EntryPoint = "Program.cs",
            EnqueuedAt = DateTime.UtcNow
        };

        var scanResult = new CodeScanResult { IsSafe = true };
        var containerId = "test-container-123";
        var executionResult = new ContainerExecutionResult
        {
            Output = "Hello World",
            Error = string.Empty,
            ExitCode = 0,
            ExecutionTimeMs = 100,
            TimedOut = false,
            MemoryExceeded = false
        };

        // Setup mock to return job once, then null (to prevent infinite loop)
        var jobReturned = false;
        _mockJobQueue.Setup(q => q.DequeueJobAsync(It.IsAny<TimeSpan>()))
            .ReturnsAsync(() =>
            {
                if (!jobReturned)
                {
                    jobReturned = true;
                    return job;
                }
                return null;
            });
        _mockCodeScanner.Setup(s => s.ScanCode(job.Code))
            .Returns(scanResult);
        _mockContainerManager.Setup(m => m.CreateContainerAsync(job.Code, job.Files, job.EntryPoint))
            .ReturnsAsync(containerId);
        _mockContainerManager.Setup(m => m.StartContainerAsync(containerId, It.IsAny<TimeSpan>()))
            .ReturnsAsync(executionResult);
        _mockContainerManager.Setup(m => m.StopAndRemoveContainerAsync(containerId))
            .Returns(Task.CompletedTask);

        var worker = new ExecutionWorker(
            _mockLogger.Object,
            _mockJobQueue.Object,
            _mockCodeScanner.Object,
            _mockContainerManager.Object);

        // Act
        var cts = new CancellationTokenSource();
        var workerTask = worker.StartAsync(cts.Token);
        
        // Give the worker time to process the job
        await Task.Delay(1000);
        
        // Stop the worker
        cts.Cancel();
        await workerTask;

        // Assert
        _mockJobQueue.Verify(q => q.DequeueJobAsync(It.IsAny<TimeSpan>()), Times.AtLeastOnce);
        _mockCodeScanner.Verify(s => s.ScanCode(job.Code), Times.Once);
        _mockContainerManager.Verify(m => m.CreateContainerAsync(job.Code, job.Files, job.EntryPoint), Times.Once);
        _mockContainerManager.Verify(m => m.StartContainerAsync(containerId, It.IsAny<TimeSpan>()), Times.Once);
        _mockContainerManager.Verify(m => m.StopAndRemoveContainerAsync(containerId), Times.Once);
        _mockJobQueue.Verify(q => q.StoreResultAsync(It.Is<ExecutionResult>(r => 
            r.JobId == job.JobId && 
            r.Status == ExecutionStatus.Completed)), Times.Once);
    }

    [Fact]
    public async Task ProcessJob_WithProhibitedCode_RejectsExecution()
    {
        // Arrange
        var job = new ExecutionJob
        {
            JobId = Guid.NewGuid(),
            Code = "System.IO.File.ReadAllText(\"test.txt\");",
            Files = new List<string>(),
            EntryPoint = "Program.cs",
            EnqueuedAt = DateTime.UtcNow
        };

        var scanResult = new CodeScanResult
        {
            IsSafe = false,
            Violations = new List<CodeViolation>
            {
                new CodeViolation
                {
                    Line = 1,
                    Operation = "System.IO.File.ReadAllText",
                    Reason = "File system access is not allowed"
                }
            }
        };

        // Setup mock to return job once, then null (to prevent infinite loop)
        var jobReturned = false;
        _mockJobQueue.Setup(q => q.DequeueJobAsync(It.IsAny<TimeSpan>()))
            .ReturnsAsync(() =>
            {
                if (!jobReturned)
                {
                    jobReturned = true;
                    return job;
                }
                return null;
            });
        _mockCodeScanner.Setup(s => s.ScanCode(job.Code))
            .Returns(scanResult);

        var worker = new ExecutionWorker(
            _mockLogger.Object,
            _mockJobQueue.Object,
            _mockCodeScanner.Object,
            _mockContainerManager.Object);

        // Act
        var cts = new CancellationTokenSource();
        var workerTask = worker.StartAsync(cts.Token);
        
        await Task.Delay(1000);
        
        cts.Cancel();
        await workerTask;

        // Assert
        _mockCodeScanner.Verify(s => s.ScanCode(job.Code), Times.Once);
        _mockContainerManager.Verify(m => m.CreateContainerAsync(It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<string>()), Times.Never);
        _mockJobQueue.Verify(q => q.StoreResultAsync(It.Is<ExecutionResult>(r => 
            r.JobId == job.JobId && 
            r.Status == ExecutionStatus.Failed &&
            r.Error!.Contains("prohibited"))), Times.Once);
    }

    [Fact]
    public async Task ProcessJob_WithTimeout_ReturnsTimeoutStatus()
    {
        // Arrange
        var job = new ExecutionJob
        {
            JobId = Guid.NewGuid(),
            Code = "while(true) { }",
            Files = new List<string>(),
            EntryPoint = "Program.cs",
            EnqueuedAt = DateTime.UtcNow
        };

        var scanResult = new CodeScanResult { IsSafe = true };
        var containerId = "test-container-123";
        var executionResult = new ContainerExecutionResult
        {
            Output = string.Empty,
            Error = "Execution exceeded 30 second time limit",
            ExitCode = -1,
            ExecutionTimeMs = 30000,
            TimedOut = true,
            MemoryExceeded = false
        };

        // Setup mock to return job once, then null (to prevent infinite loop)
        var jobReturned = false;
        _mockJobQueue.Setup(q => q.DequeueJobAsync(It.IsAny<TimeSpan>()))
            .ReturnsAsync(() =>
            {
                if (!jobReturned)
                {
                    jobReturned = true;
                    return job;
                }
                return null;
            });
        _mockCodeScanner.Setup(s => s.ScanCode(job.Code))
            .Returns(scanResult);
        _mockContainerManager.Setup(m => m.CreateContainerAsync(job.Code, job.Files, job.EntryPoint))
            .ReturnsAsync(containerId);
        _mockContainerManager.Setup(m => m.StartContainerAsync(containerId, It.IsAny<TimeSpan>()))
            .ReturnsAsync(executionResult);
        _mockContainerManager.Setup(m => m.StopAndRemoveContainerAsync(containerId))
            .Returns(Task.CompletedTask);

        var worker = new ExecutionWorker(
            _mockLogger.Object,
            _mockJobQueue.Object,
            _mockCodeScanner.Object,
            _mockContainerManager.Object);

        // Act
        var cts = new CancellationTokenSource();
        var workerTask = worker.StartAsync(cts.Token);
        
        await Task.Delay(1000);
        
        cts.Cancel();
        await workerTask;

        // Assert
        _mockJobQueue.Verify(q => q.StoreResultAsync(It.Is<ExecutionResult>(r => 
            r.JobId == job.JobId && 
            r.Status == ExecutionStatus.Timeout)), Times.Once);
        _mockContainerManager.Verify(m => m.StopAndRemoveContainerAsync(containerId), Times.Once);
    }

    [Fact]
    public async Task ProcessJob_WithMemoryExceeded_ReturnsMemoryExceededStatus()
    {
        // Arrange
        var job = new ExecutionJob
        {
            JobId = Guid.NewGuid(),
            Code = "var list = new List<byte[]>(); while(true) { list.Add(new byte[1024*1024]); }",
            Files = new List<string>(),
            EntryPoint = "Program.cs",
            EnqueuedAt = DateTime.UtcNow
        };

        var scanResult = new CodeScanResult { IsSafe = true };
        var containerId = "test-container-123";
        var executionResult = new ContainerExecutionResult
        {
            Output = string.Empty,
            Error = "Code execution exceeded 512MB memory limit",
            ExitCode = -1,
            ExecutionTimeMs = 5000,
            TimedOut = false,
            MemoryExceeded = true
        };

        // Setup mock to return job once, then null (to prevent infinite loop)
        var jobReturned = false;
        _mockJobQueue.Setup(q => q.DequeueJobAsync(It.IsAny<TimeSpan>()))
            .ReturnsAsync(() =>
            {
                if (!jobReturned)
                {
                    jobReturned = true;
                    return job;
                }
                return null;
            });
        _mockCodeScanner.Setup(s => s.ScanCode(job.Code))
            .Returns(scanResult);
        _mockContainerManager.Setup(m => m.CreateContainerAsync(job.Code, job.Files, job.EntryPoint))
            .ReturnsAsync(containerId);
        _mockContainerManager.Setup(m => m.StartContainerAsync(containerId, It.IsAny<TimeSpan>()))
            .ReturnsAsync(executionResult);
        _mockContainerManager.Setup(m => m.StopAndRemoveContainerAsync(containerId))
            .Returns(Task.CompletedTask);

        var worker = new ExecutionWorker(
            _mockLogger.Object,
            _mockJobQueue.Object,
            _mockCodeScanner.Object,
            _mockContainerManager.Object);

        // Act
        var cts = new CancellationTokenSource();
        var workerTask = worker.StartAsync(cts.Token);
        
        await Task.Delay(1000);
        
        cts.Cancel();
        await workerTask;

        // Assert
        _mockJobQueue.Verify(q => q.StoreResultAsync(It.Is<ExecutionResult>(r => 
            r.JobId == job.JobId && 
            r.Status == ExecutionStatus.MemoryExceeded)), Times.Once);
        _mockContainerManager.Verify(m => m.StopAndRemoveContainerAsync(containerId), Times.Once);
    }

    [Fact]
    public async Task ProcessJob_WithContainerCreationFailure_RetriesAndRequeues()
    {
        // Arrange
        var job = new ExecutionJob
        {
            JobId = Guid.NewGuid(),
            Code = "Console.WriteLine(\"Test\");",
            Files = new List<string>(),
            EntryPoint = "Program.cs",
            EnqueuedAt = DateTime.UtcNow
        };

        var scanResult = new CodeScanResult { IsSafe = true };

        // Setup mock to return job once, then null (to prevent infinite loop)
        var jobReturned = false;
        _mockJobQueue.Setup(q => q.DequeueJobAsync(It.IsAny<TimeSpan>()))
            .ReturnsAsync(() =>
            {
                if (!jobReturned)
                {
                    jobReturned = true;
                    return job;
                }
                return null;
            });
        _mockCodeScanner.Setup(s => s.ScanCode(job.Code))
            .Returns(scanResult);
        _mockContainerManager.Setup(m => m.CreateContainerAsync(job.Code, job.Files, job.EntryPoint))
            .ThrowsAsync(new Exception("Docker daemon not available"));

        var worker = new ExecutionWorker(
            _mockLogger.Object,
            _mockJobQueue.Object,
            _mockCodeScanner.Object,
            _mockContainerManager.Object);

        // Act
        var cts = new CancellationTokenSource();
        var workerTask = worker.StartAsync(cts.Token);
        
        await Task.Delay(2000);
        
        cts.Cancel();
        await workerTask;

        // Assert
        // Should retry 3 times
        _mockContainerManager.Verify(m => m.CreateContainerAsync(job.Code, job.Files, job.EntryPoint), Times.Exactly(3));
        // Should requeue the job
        _mockJobQueue.Verify(q => q.EnqueueJobAsync(job), Times.Once);
    }

    [Fact]
    public async Task ProcessJob_WithOldJob_DoesNotRequeue()
    {
        // Arrange
        var job = new ExecutionJob
        {
            JobId = Guid.NewGuid(),
            Code = "Console.WriteLine(\"Test\");",
            Files = new List<string>(),
            EntryPoint = "Program.cs",
            EnqueuedAt = DateTime.UtcNow.AddMinutes(-10) // Old job
        };

        var scanResult = new CodeScanResult { IsSafe = true };

        // Setup mock to return job once, then null (to prevent infinite loop)
        var jobReturned = false;
        _mockJobQueue.Setup(q => q.DequeueJobAsync(It.IsAny<TimeSpan>()))
            .ReturnsAsync(() =>
            {
                if (!jobReturned)
                {
                    jobReturned = true;
                    return job;
                }
                return null;
            });
        _mockCodeScanner.Setup(s => s.ScanCode(job.Code))
            .Returns(scanResult);
        _mockContainerManager.Setup(m => m.CreateContainerAsync(job.Code, job.Files, job.EntryPoint))
            .ThrowsAsync(new Exception("Docker daemon not available"));

        var worker = new ExecutionWorker(
            _mockLogger.Object,
            _mockJobQueue.Object,
            _mockCodeScanner.Object,
            _mockContainerManager.Object);

        // Act
        var cts = new CancellationTokenSource();
        var workerTask = worker.StartAsync(cts.Token);
        
        await Task.Delay(2000);
        
        cts.Cancel();
        await workerTask;

        // Assert
        // Should NOT requeue old jobs
        _mockJobQueue.Verify(q => q.EnqueueJobAsync(job), Times.Never);
        // Should store failure result instead
        _mockJobQueue.Verify(q => q.StoreResultAsync(It.Is<ExecutionResult>(r => 
            r.JobId == job.JobId && 
            r.Status == ExecutionStatus.Failed)), Times.Once);
    }
}
