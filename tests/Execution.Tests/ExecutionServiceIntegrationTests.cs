using Execution.Service.Models;
using Execution.Service.Services;
using Microsoft.Extensions.Logging;
using Moq;
using Worker.Service;
using Shared.Interfaces;
using ExecutionResult = Shared.Models.ExecutionResult;
using ExecutionStatus = Shared.Models.ExecutionStatus;
using ExecutionJob = Shared.Models.ExecutionJob;
using CodeScanResult = Shared.Interfaces.CodeScanResult;
using IJobQueueService = Execution.Service.Services.IJobQueueService;

namespace Execution.Tests;

/// <summary>
/// Integration tests for the complete execution service flow
/// Tests end-to-end scenarios from job submission to result retrieval
/// Validates: Requirements 3.1, 3.2, 3.6, 3.7, 3.8, 3.9, 12.2, 12.3
/// </summary>
public class ExecutionServiceIntegrationTests
{
    /// <summary>
    /// Validates complete execution flow: enqueue -> dequeue -> execute -> store result
    /// Validates Requirements 3.1, 3.2, 3.6, 12.2, 12.3
    /// </summary>
    [Fact]
    public async Task CompleteExecutionFlow_SuccessfulExecution()
    {
        // Arrange
        var job = new ExecutionJob
        {
            JobId = Guid.NewGuid(),
            Code = "Console.WriteLine(\"Hello, World!\");",
            Files = new List<string> { "Program.cs" },
            EntryPoint = "Program.cs",
            EnqueuedAt = DateTime.UtcNow
        };

        var mockLogger = new Mock<ILogger<ExecutionWorker>>();
        var mockJobQueue = new Mock<IJobQueueService>();
        var mockCodeScanner = new Mock<IProhibitedCodeScanner>();
        var mockContainerManager = new Mock<IDockerContainerManager>();

        var containerId = "test-container-123";
        var executionResult = new ContainerExecutionResult
        {
            Output = "Hello, World!",
            Error = string.Empty,
            ExitCode = 0,
            ExecutionTimeMs = 150,
            TimedOut = false,
            MemoryExceeded = false
        };

        // Setup: Job is enqueued and can be dequeued
        mockJobQueue.Setup(q => q.EnqueueJobAsync(job))
            .Returns(Task.CompletedTask);
        
        var dequeueCount = 0;
        mockJobQueue.Setup(q => q.DequeueJobAsync(It.IsAny<TimeSpan>()))
            .ReturnsAsync(() =>
            {
                if (dequeueCount == 0)
                {
                    dequeueCount++;
                    return job;
                }
                return null; // Return null after first dequeue to stop the loop
            });

        // Setup: Code passes security scan
        mockCodeScanner.Setup(s => s.ScanCode(job.Code))
            .Returns(new CodeScanResult { IsSafe = true });

        // Setup: Container executes successfully
        mockContainerManager.Setup(m => m.CreateContainerAsync(job.Code, job.Files, job.EntryPoint))
            .ReturnsAsync(containerId);
        
        mockContainerManager.Setup(m => m.StartContainerAsync(containerId, It.IsAny<TimeSpan>()))
            .ReturnsAsync(executionResult);
        
        mockContainerManager.Setup(m => m.StopAndRemoveContainerAsync(containerId))
            .Returns(Task.CompletedTask);

        ExecutionResult? storedResult = null;
        mockJobQueue.Setup(q => q.StoreResultAsync(It.IsAny<ExecutionResult>()))
            .Callback<ExecutionResult>(r => storedResult = r)
            .Returns(Task.CompletedTask);

        var worker = new ExecutionWorker(
            mockLogger.Object,
            mockJobQueue.Object,
            mockCodeScanner.Object,
            mockContainerManager.Object);

        // Act
        // 1. Enqueue job
        await mockJobQueue.Object.EnqueueJobAsync(job);

        // 2. Worker processes job
        var cts = new CancellationTokenSource();
        var workerTask = worker.StartAsync(cts.Token);
        await Task.Delay(1000);
        cts.Cancel();
        await workerTask;

        // Assert
        // Verify complete flow executed
        mockJobQueue.Verify(q => q.EnqueueJobAsync(job), Times.Once);
        mockJobQueue.Verify(q => q.DequeueJobAsync(It.IsAny<TimeSpan>()), Times.AtLeastOnce);
        mockCodeScanner.Verify(s => s.ScanCode(job.Code), Times.Once);
        mockContainerManager.Verify(m => m.CreateContainerAsync(job.Code, job.Files, job.EntryPoint), Times.Once);
        mockContainerManager.Verify(m => m.StartContainerAsync(containerId, It.IsAny<TimeSpan>()), Times.Once);
        mockContainerManager.Verify(m => m.StopAndRemoveContainerAsync(containerId), Times.Once);
        mockJobQueue.Verify(q => q.StoreResultAsync(It.IsAny<ExecutionResult>()), Times.Once);

        // Verify result is correct
        Assert.NotNull(storedResult);
        Assert.Equal(job.JobId, storedResult.JobId);
        Assert.Equal(ExecutionStatus.Completed, storedResult.Status);
        Assert.Equal("Hello, World!", storedResult.Output);
        Assert.Equal(0, storedResult.ExitCode);
    }

    /// <summary>
    /// Validates timeout handling in complete execution flow
    /// Validates Requirements 3.4, 3.7, 14.3
    /// </summary>
    [Fact]
    public async Task CompleteExecutionFlow_TimeoutHandling()
    {
        // Arrange
        var job = new ExecutionJob
        {
            JobId = Guid.NewGuid(),
            Code = "while(true) { }",
            Files = new List<string> { "Program.cs" },
            EntryPoint = "Program.cs",
            EnqueuedAt = DateTime.UtcNow
        };

        var mockLogger = new Mock<ILogger<ExecutionWorker>>();
        var mockJobQueue = new Mock<IJobQueueService>();
        var mockCodeScanner = new Mock<IProhibitedCodeScanner>();
        var mockContainerManager = new Mock<IDockerContainerManager>();

        var containerId = "test-container-timeout";
        var executionResult = new ContainerExecutionResult
        {
            Output = string.Empty,
            Error = "Execution exceeded 30 second time limit",
            ExitCode = -1,
            ExecutionTimeMs = 30000,
            TimedOut = true,
            MemoryExceeded = false
        };

        var dequeueCount = 0;
        mockJobQueue.Setup(q => q.DequeueJobAsync(It.IsAny<TimeSpan>()))
            .ReturnsAsync(() =>
            {
                if (dequeueCount == 0)
                {
                    dequeueCount++;
                    return job;
                }
                return null; // Return null after first dequeue to stop the loop
            });
        
        mockCodeScanner.Setup(s => s.ScanCode(job.Code))
            .Returns(new CodeScanResult { IsSafe = true });
        
        mockContainerManager.Setup(m => m.CreateContainerAsync(job.Code, job.Files, job.EntryPoint))
            .ReturnsAsync(containerId);
        
        mockContainerManager.Setup(m => m.StartContainerAsync(containerId, It.IsAny<TimeSpan>()))
            .ReturnsAsync(executionResult);
        
        mockContainerManager.Setup(m => m.StopAndRemoveContainerAsync(containerId))
            .Returns(Task.CompletedTask);

        ExecutionResult? storedResult = null;
        mockJobQueue.Setup(q => q.StoreResultAsync(It.IsAny<ExecutionResult>()))
            .Callback<ExecutionResult>(r => storedResult = r)
            .Returns(Task.CompletedTask);

        var worker = new ExecutionWorker(
            mockLogger.Object,
            mockJobQueue.Object,
            mockCodeScanner.Object,
            mockContainerManager.Object);

        // Act
        var cts = new CancellationTokenSource();
        var workerTask = worker.StartAsync(cts.Token);
        await Task.Delay(1000);
        cts.Cancel();
        await workerTask;

        // Assert
        Assert.NotNull(storedResult);
        Assert.Equal(ExecutionStatus.Timeout, storedResult.Status);
        Assert.Contains("time limit", storedResult.Error, StringComparison.OrdinalIgnoreCase);
        Assert.Equal(30000, storedResult.ExecutionTimeMs);
        
        // Verify container was cleaned up even after timeout
        mockContainerManager.Verify(m => m.StopAndRemoveContainerAsync(containerId), Times.Once);
    }

    /// <summary>
    /// Validates memory limit handling in complete execution flow
    /// Validates Requirements 3.3, 3.8
    /// </summary>
    [Fact]
    public async Task CompleteExecutionFlow_MemoryLimitHandling()
    {
        // Arrange
        var job = new ExecutionJob
        {
            JobId = Guid.NewGuid(),
            Code = "var list = new List<byte[]>(); while(true) { list.Add(new byte[1024*1024]); }",
            Files = new List<string> { "Program.cs" },
            EntryPoint = "Program.cs",
            EnqueuedAt = DateTime.UtcNow
        };

        var mockLogger = new Mock<ILogger<ExecutionWorker>>();
        var mockJobQueue = new Mock<IJobQueueService>();
        var mockCodeScanner = new Mock<IProhibitedCodeScanner>();
        var mockContainerManager = new Mock<IDockerContainerManager>();

        var containerId = "test-container-memory";
        var executionResult = new ContainerExecutionResult
        {
            Output = string.Empty,
            Error = "Code execution exceeded 512MB memory limit",
            ExitCode = -1,
            ExecutionTimeMs = 5000,
            TimedOut = false,
            MemoryExceeded = true
        };

        var dequeueCount = 0;
        mockJobQueue.Setup(q => q.DequeueJobAsync(It.IsAny<TimeSpan>()))
            .ReturnsAsync(() =>
            {
                if (dequeueCount == 0)
                {
                    dequeueCount++;
                    return job;
                }
                return null; // Return null after first dequeue to stop the loop
            });
        
        mockCodeScanner.Setup(s => s.ScanCode(job.Code))
            .Returns(new CodeScanResult { IsSafe = true });
        
        mockContainerManager.Setup(m => m.CreateContainerAsync(job.Code, job.Files, job.EntryPoint))
            .ReturnsAsync(containerId);
        
        mockContainerManager.Setup(m => m.StartContainerAsync(containerId, It.IsAny<TimeSpan>()))
            .ReturnsAsync(executionResult);
        
        mockContainerManager.Setup(m => m.StopAndRemoveContainerAsync(containerId))
            .Returns(Task.CompletedTask);

        ExecutionResult? storedResult = null;
        mockJobQueue.Setup(q => q.StoreResultAsync(It.IsAny<ExecutionResult>()))
            .Callback<ExecutionResult>(r => storedResult = r)
            .Returns(Task.CompletedTask);

        var worker = new ExecutionWorker(
            mockLogger.Object,
            mockJobQueue.Object,
            mockCodeScanner.Object,
            mockContainerManager.Object);

        // Act
        var cts = new CancellationTokenSource();
        var workerTask = worker.StartAsync(cts.Token);
        await Task.Delay(1000);
        cts.Cancel();
        await workerTask;

        // Assert
        Assert.NotNull(storedResult);
        Assert.Equal(ExecutionStatus.MemoryExceeded, storedResult.Status);
        Assert.Contains("512MB", storedResult.Error);
        Assert.Contains("memory", storedResult.Error, StringComparison.OrdinalIgnoreCase);
        
        // Verify container was cleaned up even after memory exceeded
        mockContainerManager.Verify(m => m.StopAndRemoveContainerAsync(containerId), Times.Once);
    }

    /// <summary>
    /// Validates prohibited code rejection in complete execution flow
    /// Validates Requirements 14.4, 14.5
    /// </summary>
    [Fact]
    public async Task CompleteExecutionFlow_ProhibitedCodeRejection()
    {
        // Arrange
        var job = new ExecutionJob
        {
            JobId = Guid.NewGuid(),
            Code = "System.IO.File.ReadAllText(\"secret.txt\");",
            Files = new List<string> { "Program.cs" },
            EntryPoint = "Program.cs",
            EnqueuedAt = DateTime.UtcNow
        };

        var mockLogger = new Mock<ILogger<ExecutionWorker>>();
        var mockJobQueue = new Mock<IJobQueueService>();
        var mockCodeScanner = new Mock<IProhibitedCodeScanner>();
        var mockContainerManager = new Mock<IDockerContainerManager>();

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

        mockJobQueue.Setup(q => q.DequeueJobAsync(It.IsAny<TimeSpan>()))
            .ReturnsAsync(job);
        
        mockCodeScanner.Setup(s => s.ScanCode(job.Code))
            .Returns(scanResult);

        ExecutionResult? storedResult = null;
        mockJobQueue.Setup(q => q.StoreResultAsync(It.IsAny<ExecutionResult>()))
            .Callback<ExecutionResult>(r => storedResult = r)
            .Returns(Task.CompletedTask);

        var worker = new ExecutionWorker(
            mockLogger.Object,
            mockJobQueue.Object,
            mockCodeScanner.Object,
            mockContainerManager.Object);

        // Act
        var cts = new CancellationTokenSource();
        var workerTask = worker.StartAsync(cts.Token);
        await Task.Delay(1000);
        cts.Cancel();
        await workerTask;

        // Assert
        Assert.NotNull(storedResult);
        Assert.Equal(ExecutionStatus.Failed, storedResult.Status);
        Assert.Contains("prohibited", storedResult.Error, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("File system access", storedResult.Error);
        
        // Verify container was NEVER created for prohibited code
        mockContainerManager.Verify(
            m => m.CreateContainerAsync(It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<string>()), 
            Times.Never);
    }

    /// <summary>
    /// Validates multiple concurrent executions maintain isolation
    /// Validates Requirements 3.9
    /// </summary>
    [Fact]
    public async Task ConcurrentExecutions_MaintainIsolation()
    {
        // Arrange - Create 3 different jobs
        var jobs = new[]
        {
            new ExecutionJob
            {
                JobId = Guid.NewGuid(),
                Code = "Console.WriteLine(\"Job 1\");",
                Files = new List<string> { "Program.cs" },
                EntryPoint = "Program.cs",
                EnqueuedAt = DateTime.UtcNow
            },
            new ExecutionJob
            {
                JobId = Guid.NewGuid(),
                Code = "Console.WriteLine(\"Job 2\");",
                Files = new List<string> { "Program.cs" },
                EntryPoint = "Program.cs",
                EnqueuedAt = DateTime.UtcNow
            },
            new ExecutionJob
            {
                JobId = Guid.NewGuid(),
                Code = "Console.WriteLine(\"Job 3\");",
                Files = new List<string> { "Program.cs" },
                EntryPoint = "Program.cs",
                EnqueuedAt = DateTime.UtcNow
            }
        };

        var results = new List<ExecutionResult>();
        var containerIds = new List<string>();
        var lockObject = new object();

        // Create 3 workers
        var workers = new List<ExecutionWorker>();
        for (int i = 0; i < 3; i++)
        {
            var jobIndex = i;
            var mockLogger = new Mock<ILogger<ExecutionWorker>>();
            var mockJobQueue = new Mock<IJobQueueService>();
            var mockCodeScanner = new Mock<IProhibitedCodeScanner>();
            var mockContainerManager = new Mock<IDockerContainerManager>();

            var dequeueCount = 0;
            mockJobQueue.Setup(q => q.DequeueJobAsync(It.IsAny<TimeSpan>()))
                .ReturnsAsync(() =>
                {
                    if (dequeueCount == 0)
                    {
                        dequeueCount++;
                        return jobs[jobIndex];
                    }
                    return null; // Return null after first dequeue to stop the loop
                });

            mockCodeScanner.Setup(s => s.ScanCode(It.IsAny<string>()))
                .Returns(new CodeScanResult { IsSafe = true });

            var containerId = $"container-{jobs[jobIndex].JobId}";
            mockContainerManager.Setup(m => m.CreateContainerAsync(
                It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<string>()))
                .ReturnsAsync(() =>
                {
                    lock (lockObject)
                    {
                        containerIds.Add(containerId);
                    }
                    return containerId;
                });

            mockContainerManager.Setup(m => m.StartContainerAsync(It.IsAny<string>(), It.IsAny<TimeSpan>()))
                .ReturnsAsync(new ContainerExecutionResult
                {
                    Output = $"Job {jobIndex + 1}",
                    Error = string.Empty,
                    ExitCode = 0,
                    ExecutionTimeMs = 100,
                    TimedOut = false,
                    MemoryExceeded = false
                });

            mockContainerManager.Setup(m => m.StopAndRemoveContainerAsync(It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            mockJobQueue.Setup(q => q.StoreResultAsync(It.IsAny<ExecutionResult>()))
                .Callback<ExecutionResult>(r =>
                {
                    lock (lockObject)
                    {
                        results.Add(r);
                    }
                })
                .Returns(Task.CompletedTask);

            var worker = new ExecutionWorker(
                mockLogger.Object,
                mockJobQueue.Object,
                mockCodeScanner.Object,
                mockContainerManager.Object);

            workers.Add(worker);
        }

        // Act - Start all workers concurrently
        var cancellationTokenSources = workers.Select(_ => new CancellationTokenSource()).ToList();
        var workerTasks = workers.Select((w, i) => w.StartAsync(cancellationTokenSources[i].Token)).ToList();

        await Task.Delay(1500);

        foreach (var cts in cancellationTokenSources)
        {
            cts.Cancel();
        }
        await Task.WhenAll(workerTasks);

        // Assert
        Assert.Equal(3, results.Count);
        Assert.Equal(3, containerIds.Count);

        // All container IDs should be unique (isolation)
        Assert.Equal(3, containerIds.Distinct().Count());

        // All job IDs should be unique
        var jobIds = results.Select(r => r.JobId).ToList();
        Assert.Equal(3, jobIds.Distinct().Count());

        // Each job should have its own output
        Assert.Contains(results, r => r.Output.Contains("Job 1"));
        Assert.Contains(results, r => r.Output.Contains("Job 2"));
        Assert.Contains(results, r => r.Output.Contains("Job 3"));

        // All executions should be successful
        Assert.All(results, r => Assert.Equal(ExecutionStatus.Completed, r.Status));
    }

    /// <summary>
    /// Validates that execution results are returned within acceptable time
    /// Validates Requirement 3.6
    /// </summary>
    [Fact]
    public async Task ExecutionResults_ReturnedWithinAcceptableTime()
    {
        // Arrange
        var job = new ExecutionJob
        {
            JobId = Guid.NewGuid(),
            Code = "Console.WriteLine(\"Fast execution\");",
            Files = new List<string> { "Program.cs" },
            EntryPoint = "Program.cs",
            EnqueuedAt = DateTime.UtcNow
        };

        var mockLogger = new Mock<ILogger<ExecutionWorker>>();
        var mockJobQueue = new Mock<IJobQueueService>();
        var mockCodeScanner = new Mock<IProhibitedCodeScanner>();
        var mockContainerManager = new Mock<IDockerContainerManager>();

        mockJobQueue.Setup(q => q.DequeueJobAsync(It.IsAny<TimeSpan>()))
            .ReturnsAsync(job);
        
        mockCodeScanner.Setup(s => s.ScanCode(job.Code))
            .Returns(new CodeScanResult { IsSafe = true });
        
        mockContainerManager.Setup(m => m.CreateContainerAsync(job.Code, job.Files, job.EntryPoint))
            .ReturnsAsync("fast-container");
        
        mockContainerManager.Setup(m => m.StartContainerAsync(It.IsAny<string>(), It.IsAny<TimeSpan>()))
            .ReturnsAsync(new ContainerExecutionResult
            {
                Output = "Fast execution",
                Error = string.Empty,
                ExitCode = 0,
                ExecutionTimeMs = 150, // Well under 5 seconds
                TimedOut = false,
                MemoryExceeded = false
            });
        
        mockContainerManager.Setup(m => m.StopAndRemoveContainerAsync(It.IsAny<string>()))
            .Returns(Task.CompletedTask);

        ExecutionResult? storedResult = null;
        mockJobQueue.Setup(q => q.StoreResultAsync(It.IsAny<ExecutionResult>()))
            .Callback<ExecutionResult>(r => storedResult = r)
            .Returns(Task.CompletedTask);

        var worker = new ExecutionWorker(
            mockLogger.Object,
            mockJobQueue.Object,
            mockCodeScanner.Object,
            mockContainerManager.Object);

        // Act
        var startTime = DateTime.UtcNow;
        var cts = new CancellationTokenSource();
        var workerTask = worker.StartAsync(cts.Token);
        await Task.Delay(1000);
        cts.Cancel();
        await workerTask;
        var endTime = DateTime.UtcNow;

        // Assert
        Assert.NotNull(storedResult);
        Assert.Equal(ExecutionStatus.Completed, storedResult.Status);
        
        // Verify execution time is well under 5 seconds (Requirement 3.6)
        Assert.True(storedResult.ExecutionTimeMs < 5000, 
            $"Execution should complete within 5 seconds, but took {storedResult.ExecutionTimeMs}ms");
        
        // Verify total processing time is reasonable
        var totalTime = (endTime - startTime).TotalMilliseconds;
        Assert.True(totalTime < 10000, 
            $"Total processing time should be reasonable, but took {totalTime}ms");
    }
}
