using Execution.Service.Models;
using Execution.Service.Services;
using FsCheck;
using FsCheck.Xunit;
using Moq;
using Worker.Service;
using Microsoft.Extensions.Logging;
using Xunit;
using Shared.Interfaces;
using ExecutionResult = Shared.Models.ExecutionResult;
using ExecutionStatus = Shared.Models.ExecutionStatus;
using ExecutionJob = Shared.Models.ExecutionJob;
using CodeScanResult = Shared.Interfaces.CodeScanResult;
using IJobQueueService = Execution.Service.Services.IJobQueueService;

namespace Execution.Tests;

/// <summary>
/// Property-based tests for execution service functionality.
/// Feature: aspnet-learning-platform
/// </summary>
public class ExecutionServicePropertiesTests
{
    /// <summary>
    /// Property 8: Execution Isolation
    /// **Validates: Requirements 3.9**
    /// 
    /// For any two concurrent code execution requests, they should execute in separate 
    /// containers without interfering with each other's output, variables, or state.
    /// </summary>
    [Property(MaxTest = 50)]
    public async Task Property8_ExecutionIsolation_PreventsCrossTalk(
        NonEmptyString code1, 
        NonEmptyString code2,
        NonEmptyString output1,
        NonEmptyString output2)
    {
        // Arrange - Create two distinct jobs
        var job1 = new ExecutionJob
        {
            JobId = Guid.NewGuid(),
            Code = code1.Get,
            Files = new List<string> { "Program.cs" },
            EntryPoint = "Program.cs",
            EnqueuedAt = DateTime.UtcNow
        };

        var job2 = new ExecutionJob
        {
            JobId = Guid.NewGuid(),
            Code = code2.Get,
            Files = new List<string> { "Program.cs" },
            EntryPoint = "Program.cs",
            EnqueuedAt = DateTime.UtcNow
        };

        // Setup mocks for two separate workers
        var mockLogger1 = new Mock<ILogger<ExecutionWorker>>();
        var mockLogger2 = new Mock<ILogger<ExecutionWorker>>();
        var mockJobQueue1 = new Mock<IJobQueueService>();
        var mockJobQueue2 = new Mock<IJobQueueService>();
        var mockCodeScanner1 = new Mock<IProhibitedCodeScanner>();
        var mockCodeScanner2 = new Mock<IProhibitedCodeScanner>();
        var mockContainerManager1 = new Mock<IDockerContainerManager>();
        var mockContainerManager2 = new Mock<IDockerContainerManager>();

        // Container IDs must be different to ensure isolation
        var containerId1 = $"container-{job1.JobId}";
        var containerId2 = $"container-{job2.JobId}";

        var executionResult1 = new ContainerExecutionResult
        {
            Output = output1.Get,
            Error = string.Empty,
            ExitCode = 0,
            ExecutionTimeMs = 100,
            TimedOut = false,
            MemoryExceeded = false
        };

        var executionResult2 = new ContainerExecutionResult
        {
            Output = output2.Get,
            Error = string.Empty,
            ExitCode = 0,
            ExecutionTimeMs = 150,
            TimedOut = false,
            MemoryExceeded = false
        };

        // Setup worker 1 - return job once, then null
        var job1Returned = false;
        mockJobQueue1.Setup(q => q.DequeueJobAsync(It.IsAny<TimeSpan>()))
            .ReturnsAsync(() =>
            {
                if (!job1Returned)
                {
                    job1Returned = true;
                    return job1;
                }
                return null;
            });
        mockCodeScanner1.Setup(s => s.ScanCode(job1.Code))
            .Returns(new CodeScanResult { IsSafe = true });
        mockContainerManager1.Setup(m => m.CreateContainerAsync(job1.Code, job1.Files, job1.EntryPoint))
            .ReturnsAsync(containerId1);
        mockContainerManager1.Setup(m => m.StartContainerAsync(containerId1, It.IsAny<TimeSpan>()))
            .ReturnsAsync(executionResult1);
        mockContainerManager1.Setup(m => m.StopAndRemoveContainerAsync(containerId1))
            .Returns(Task.CompletedTask);

        ExecutionResult? capturedResult1 = null;
        mockJobQueue1.Setup(q => q.StoreResultAsync(It.IsAny<ExecutionResult>()))
            .Callback<ExecutionResult>(r => capturedResult1 = r)
            .Returns(Task.CompletedTask);

        // Setup worker 2 - return job once, then null
        var job2Returned = false;
        mockJobQueue2.Setup(q => q.DequeueJobAsync(It.IsAny<TimeSpan>()))
            .ReturnsAsync(() =>
            {
                if (!job2Returned)
                {
                    job2Returned = true;
                    return job2;
                }
                return null;
            });
        mockCodeScanner2.Setup(s => s.ScanCode(job2.Code))
            .Returns(new CodeScanResult { IsSafe = true });
        mockContainerManager2.Setup(m => m.CreateContainerAsync(job2.Code, job2.Files, job2.EntryPoint))
            .ReturnsAsync(containerId2);
        mockContainerManager2.Setup(m => m.StartContainerAsync(containerId2, It.IsAny<TimeSpan>()))
            .ReturnsAsync(executionResult2);
        mockContainerManager2.Setup(m => m.StopAndRemoveContainerAsync(containerId2))
            .Returns(Task.CompletedTask);

        ExecutionResult? capturedResult2 = null;
        mockJobQueue2.Setup(q => q.StoreResultAsync(It.IsAny<ExecutionResult>()))
            .Callback<ExecutionResult>(r => capturedResult2 = r)
            .Returns(Task.CompletedTask);

        var worker1 = new ExecutionWorker(
            mockLogger1.Object,
            mockJobQueue1.Object,
            mockCodeScanner1.Object,
            mockContainerManager1.Object);

        var worker2 = new ExecutionWorker(
            mockLogger2.Object,
            mockJobQueue2.Object,
            mockCodeScanner2.Object,
            mockContainerManager2.Object);

        // Act - Execute both jobs concurrently
        var cts1 = new CancellationTokenSource();
        var cts2 = new CancellationTokenSource();
        
        var worker1Task = worker1.StartAsync(cts1.Token);
        var worker2Task = worker2.StartAsync(cts2.Token);
        
        // Give workers time to process jobs (shorter delay since we only have one job each)
        await Task.Delay(500);
        
        // Stop workers
        cts1.Cancel();
        cts2.Cancel();
        
        try
        {
            await Task.WhenAll(worker1Task, worker2Task);
        }
        catch (OperationCanceledException)
        {
            // Expected when cancelling
        }

        // Assert - Verify isolation
        Assert.NotNull(capturedResult1);
        Assert.NotNull(capturedResult2);

        // Different job IDs
        Assert.NotEqual(capturedResult1.JobId, capturedResult2.JobId);
        Assert.Equal(job1.JobId, capturedResult1.JobId);
        Assert.Equal(job2.JobId, capturedResult2.JobId);

        // Different container IDs were used
        mockContainerManager1.Verify(m => m.CreateContainerAsync(job1.Code, job1.Files, job1.EntryPoint), Times.Once);
        mockContainerManager2.Verify(m => m.CreateContainerAsync(job2.Code, job2.Files, job2.EntryPoint), Times.Once);

        // Each job got its own output (no cross-talk)
        Assert.Equal(output1.Get, capturedResult1.Output);
        Assert.Equal(output2.Get, capturedResult2.Output);
        
        // Outputs should be independent (unless they happen to be the same by chance)
        if (output1.Get != output2.Get)
        {
            Assert.NotEqual(capturedResult1.Output, capturedResult2.Output);
        }

        // Both containers were cleaned up
        mockContainerManager1.Verify(m => m.StopAndRemoveContainerAsync(containerId1), Times.Once);
        mockContainerManager2.Verify(m => m.StopAndRemoveContainerAsync(containerId2), Times.Once);
    }

    /// <summary>
    /// Property 39: Concurrent Worker Processing
    /// **Validates: Requirements 12.5**
    /// 
    /// For any set of jobs in the queue, multiple workers should be able to process 
    /// them concurrently without conflicts.
    /// </summary>
    [Property(MaxTest = 50)]
    public async Task Property39_ConcurrentWorkerProcessing_ProcessesJobsWithoutConflicts(
        PositiveInt jobCount)
    {
        // Limit job count to reasonable number for testing
        var actualJobCount = Math.Min(jobCount.Get, 10);
        
        // Arrange - Create multiple jobs
        var jobs = Enumerable.Range(0, actualJobCount)
            .Select(i => new ExecutionJob
            {
                JobId = Guid.NewGuid(),
                Code = $"Console.WriteLine(\"Job {i}\");",
                Files = new List<string> { "Program.cs" },
                EntryPoint = "Program.cs",
                EnqueuedAt = DateTime.UtcNow
            })
            .ToList();

        // Create a shared queue that multiple workers will access
        var jobQueue = new Queue<ExecutionJob>(jobs);
        var processedJobs = new List<ExecutionResult>();
        var lockObject = new object();

        // Create multiple workers (2 workers for concurrent processing)
        var workers = new List<ExecutionWorker>();
        var workerMocks = new List<(Mock<IJobQueueService>, Mock<IDockerContainerManager>)>();

        for (int w = 0; w < 2; w++)
        {
            var mockLogger = new Mock<ILogger<ExecutionWorker>>();
            var mockJobQueue = new Mock<IJobQueueService>();
            var mockCodeScanner = new Mock<IProhibitedCodeScanner>();
            var mockContainerManager = new Mock<IDockerContainerManager>();

            // Setup dequeue to pull from shared queue
            mockJobQueue.Setup(q => q.DequeueJobAsync(It.IsAny<TimeSpan>()))
                .ReturnsAsync(() =>
                {
                    lock (lockObject)
                    {
                        return jobQueue.Count > 0 ? jobQueue.Dequeue() : null;
                    }
                });

            // Setup code scanner
            mockCodeScanner.Setup(s => s.ScanCode(It.IsAny<string>()))
                .Returns(new CodeScanResult { IsSafe = true });

            // Setup container manager
            mockContainerManager.Setup(m => m.CreateContainerAsync(
                It.IsAny<string>(), 
                It.IsAny<List<string>>(), 
                It.IsAny<string>()))
                .ReturnsAsync((string code, List<string> files, string entry) => 
                    $"container-{Guid.NewGuid()}");

            mockContainerManager.Setup(m => m.StartContainerAsync(
                It.IsAny<string>(), 
                It.IsAny<TimeSpan>()))
                .ReturnsAsync((string containerId, TimeSpan timeout) => 
                    new ContainerExecutionResult
                    {
                        Output = $"Output from {containerId}",
                        Error = string.Empty,
                        ExitCode = 0,
                        ExecutionTimeMs = 100,
                        TimedOut = false,
                        MemoryExceeded = false
                    });

            mockContainerManager.Setup(m => m.StopAndRemoveContainerAsync(It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            // Capture results
            mockJobQueue.Setup(q => q.StoreResultAsync(It.IsAny<ExecutionResult>()))
                .Callback<ExecutionResult>(r =>
                {
                    lock (lockObject)
                    {
                        processedJobs.Add(r);
                    }
                })
                .Returns(Task.CompletedTask);

            workerMocks.Add((mockJobQueue, mockContainerManager));

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

        // Wait for all jobs to be processed (or timeout)
        var maxWaitTime = TimeSpan.FromSeconds(10);
        var startTime = DateTime.UtcNow;
        while (processedJobs.Count < actualJobCount && DateTime.UtcNow - startTime < maxWaitTime)
        {
            await Task.Delay(100);
        }

        // Stop all workers
        foreach (var cts in cancellationTokenSources)
        {
            cts.Cancel();
        }
        await Task.WhenAll(workerTasks);

        // Assert - Verify all jobs were processed without conflicts
        Assert.Equal(actualJobCount, processedJobs.Count);

        // All job IDs should be unique (no duplicate processing)
        var uniqueJobIds = processedJobs.Select(r => r.JobId).Distinct().Count();
        Assert.Equal(actualJobCount, uniqueJobIds);

        // All original job IDs should be in the results
        var originalJobIds = jobs.Select(j => j.JobId).OrderBy(id => id).ToList();
        var processedJobIds = processedJobs.Select(r => r.JobId).OrderBy(id => id).ToList();
        Assert.Equal(originalJobIds, processedJobIds);

        // All jobs should have completed successfully
        Assert.All(processedJobs, result =>
        {
            Assert.Equal(ExecutionStatus.Completed, result.Status);
            Assert.NotNull(result.Output);
        });
    }

    /// <summary>
    /// Property 40: Job Retry on Worker Failure
    /// **Validates: Requirements 12.6**
    /// 
    /// For any job where the worker fails during processing, the platform should 
    /// requeue the job for another worker to retry.
    /// </summary>
    [Property(MaxTest = 50)]
    public async Task Property40_JobRetryOnWorkerFailure_RequeuesJob(
        NonEmptyString code,
        NonEmptyString entryPoint)
    {
        // Arrange - Create a job
        var job = new ExecutionJob
        {
            JobId = Guid.NewGuid(),
            Code = code.Get,
            Files = new List<string> { "Program.cs" },
            EntryPoint = entryPoint.Get,
            EnqueuedAt = DateTime.UtcNow // Recent job (not old)
        };

        var mockLogger = new Mock<ILogger<ExecutionWorker>>();
        var mockJobQueue = new Mock<IJobQueueService>();
        var mockCodeScanner = new Mock<IProhibitedCodeScanner>();
        var mockContainerManager = new Mock<IDockerContainerManager>();

        // Setup dequeue to return the job once, then null
        var jobReturned = false;
        mockJobQueue.Setup(q => q.DequeueJobAsync(It.IsAny<TimeSpan>()))
            .ReturnsAsync(() =>
            {
                if (!jobReturned)
                {
                    jobReturned = true;
                    return job;
                }
                return null;
            });

        // Setup code scanner to pass
        mockCodeScanner.Setup(s => s.ScanCode(job.Code))
            .Returns(new CodeScanResult { IsSafe = true });

        // Setup container manager to fail (simulating worker failure)
        mockContainerManager.Setup(m => m.CreateContainerAsync(
            It.IsAny<string>(), 
            It.IsAny<List<string>>(), 
            It.IsAny<string>()))
            .ThrowsAsync(new Exception("Docker daemon not available"));

        ExecutionJob? requeuedJob = null;
        mockJobQueue.Setup(q => q.EnqueueJobAsync(It.IsAny<ExecutionJob>()))
            .Callback<ExecutionJob>(j => requeuedJob = j)
            .Returns(Task.CompletedTask);

        var worker = new ExecutionWorker(
            mockLogger.Object,
            mockJobQueue.Object,
            mockCodeScanner.Object,
            mockContainerManager.Object);

        // Act - Start worker and let it process the failing job
        var cts = new CancellationTokenSource();
        var workerTask = worker.StartAsync(cts.Token);
        
        // Wait for worker to process and requeue (shorter delay)
        await Task.Delay(1000);
        
        cts.Cancel();
        try
        {
            await workerTask;
        }
        catch (OperationCanceledException)
        {
            // Expected when cancelling
        }

        // Assert - Verify job was requeued after failure
        Assert.NotNull(requeuedJob);
        Assert.Equal(job.JobId, requeuedJob.JobId);
        Assert.Equal(job.Code, requeuedJob.Code);
        Assert.Equal(job.EntryPoint, requeuedJob.EntryPoint);

        // Verify the worker attempted to create container (and failed)
        mockContainerManager.Verify(
            m => m.CreateContainerAsync(job.Code, job.Files, job.EntryPoint), 
            Times.Exactly(3)); // Should retry 3 times

        // Verify job was requeued
        mockJobQueue.Verify(q => q.EnqueueJobAsync(job), Times.Once);
    }

    /// <summary>
    /// Property: Old Jobs Are Not Requeued on Failure
    /// 
    /// For any job that is too old (more than 5 minutes), if the worker fails during 
    /// processing, the platform should NOT requeue the job but instead store a failure result.
    /// </summary>
    [Property(MaxTest = 50)]
    public async Task OldJobsNotRequeued_OnWorkerFailure(
        NonEmptyString code,
        NonEmptyString entryPoint)
    {
        // Arrange - Create an old job (more than 5 minutes old)
        var job = new ExecutionJob
        {
            JobId = Guid.NewGuid(),
            Code = code.Get,
            Files = new List<string> { "Program.cs" },
            EntryPoint = entryPoint.Get,
            EnqueuedAt = DateTime.UtcNow.AddMinutes(-10) // Old job
        };

        var mockLogger = new Mock<ILogger<ExecutionWorker>>();
        var mockJobQueue = new Mock<IJobQueueService>();
        var mockCodeScanner = new Mock<IProhibitedCodeScanner>();
        var mockContainerManager = new Mock<IDockerContainerManager>();

        // Setup dequeue to return the job once, then null
        var jobReturned = false;
        mockJobQueue.Setup(q => q.DequeueJobAsync(It.IsAny<TimeSpan>()))
            .ReturnsAsync(() =>
            {
                if (!jobReturned)
                {
                    jobReturned = true;
                    return job;
                }
                return null;
            });

        // Setup code scanner to pass
        mockCodeScanner.Setup(s => s.ScanCode(job.Code))
            .Returns(new CodeScanResult { IsSafe = true });

        // Setup container manager to fail
        mockContainerManager.Setup(m => m.CreateContainerAsync(
            It.IsAny<string>(), 
            It.IsAny<List<string>>(), 
            It.IsAny<string>()))
            .ThrowsAsync(new Exception("Docker daemon not available"));

        ExecutionResult? storedResult = null;
        mockJobQueue.Setup(q => q.StoreResultAsync(It.IsAny<ExecutionResult>()))
            .Callback<ExecutionResult>(r => storedResult = r)
            .Returns(Task.CompletedTask);

        var worker = new ExecutionWorker(
            mockLogger.Object,
            mockJobQueue.Object,
            mockCodeScanner.Object,
            mockContainerManager.Object);

        // Act - Start worker and let it process the failing job
        var cts = new CancellationTokenSource();
        var workerTask = worker.StartAsync(cts.Token);
        
        await Task.Delay(1000);
        
        cts.Cancel();
        try
        {
            await workerTask;
        }
        catch (OperationCanceledException)
        {
            // Expected when cancelling
        }

        // Assert - Verify job was NOT requeued
        mockJobQueue.Verify(q => q.EnqueueJobAsync(It.IsAny<ExecutionJob>()), Times.Never);

        // Verify failure result was stored instead
        Assert.NotNull(storedResult);
        Assert.Equal(job.JobId, storedResult.JobId);
        Assert.Equal(ExecutionStatus.Failed, storedResult.Status);
        Assert.NotNull(storedResult.Error);
        Assert.Contains("Worker failure", storedResult.Error);
    }
}
