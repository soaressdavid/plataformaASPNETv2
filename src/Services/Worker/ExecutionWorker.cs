using System.Diagnostics;
using Shared.Models;
using Shared.Interfaces;
using Shared.Metrics;

namespace Worker.Service;

/// <summary>
/// Background worker that processes code execution jobs from Redis queue
/// Validates: Requirements 3.2, 3.6, 3.7, 3.8, 3.9, 12.3, 12.5, 12.6
/// </summary>
public class ExecutionWorker : BackgroundService
{
    private readonly ILogger<ExecutionWorker> _logger;
    private readonly IJobQueueService _jobQueue;
    private readonly IProhibitedCodeScanner _codeScanner;
    private readonly IDockerContainerManager _containerManager;
    private static readonly TimeSpan PollTimeout = TimeSpan.FromSeconds(5);
    private static readonly TimeSpan ExecutionTimeout = TimeSpan.FromSeconds(30);
    private const int MaxRetries = 3;

    public ExecutionWorker(
        ILogger<ExecutionWorker> logger,
        IJobQueueService jobQueue,
        IProhibitedCodeScanner codeScanner,
        IDockerContainerManager containerManager)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _jobQueue = jobQueue ?? throw new ArgumentNullException(nameof(jobQueue));
        _codeScanner = codeScanner ?? throw new ArgumentNullException(nameof(codeScanner));
        _containerManager = containerManager ?? throw new ArgumentNullException(nameof(containerManager));
    }

    /// <summary>
    /// Main worker loop that polls Redis queue and processes jobs
    /// </summary>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Execution Worker started at: {time}", DateTimeOffset.Now);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                // Poll Redis queue with BRPOP (blocking pop)
                // Validates: Requirements 12.3
                var job = await _jobQueue.DequeueJobAsync(PollTimeout);

                if (job != null)
                {
                    _logger.LogInformation("Processing job {JobId}", job.JobId);
                    
                    // Process the job
                    await ProcessJobAsync(job, stoppingToken);
                }
            }
            catch (OperationCanceledException)
            {
                // Expected when stopping
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in worker loop");
                
                // Wait a bit before retrying to avoid tight error loops
                await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
            }
        }

        _logger.LogInformation("Execution Worker stopped at: {time}", DateTimeOffset.Now);
    }

    /// <summary>
    /// Processes a single execution job
    /// Validates: Requirements 3.2, 3.6, 3.7, 3.8, 3.9, 12.6
    /// </summary>
    private async Task ProcessJobAsync(ExecutionJob job, CancellationToken cancellationToken)
    {
        var stopwatch = Stopwatch.StartNew();
        string? containerId = null;
        var retryCount = 0;
        var workerId = Environment.MachineName;

        // Track worker utilization (job started)
        ApplicationMetrics.WorkerUtilization
            .WithLabels(workerId)
            .Set(1.0);

        try
        {
            // Step 1: Scan code for prohibited operations
            // Validates: Requirements 14.4, 14.5
            _logger.LogDebug("Scanning code for job {JobId}", job.JobId);
            var scanResult = _codeScanner.ScanCode(job.Code);

            if (!scanResult.IsSafe)
            {
                _logger.LogWarning("Job {JobId} contains prohibited code", job.JobId);
                
                var prohibitedResult = new ExecutionResult
                {
                    JobId = job.JobId,
                    Status = ExecutionStatus.Failed,
                    Error = FormatProhibitedCodeError(scanResult),
                    ExitCode = -1,
                    ExecutionTimeMs = stopwatch.ElapsedMilliseconds,
                    CompletedAt = DateTime.UtcNow
                };

                await _jobQueue.StoreResultAsync(prohibitedResult);
                
                // Track metrics
                ApplicationMetrics.JobsProcessed
                    .WithLabels(workerId, "Failed")
                    .Inc();
                
                return;
            }

            // Step 2: Create container with resource limits
            // Validates: Requirements 3.3, 3.4, 3.5, 14.1, 14.2
            while (retryCount < MaxRetries)
            {
                try
                {
                    _logger.LogDebug("Creating container for job {JobId} (attempt {Attempt})", 
                        job.JobId, retryCount + 1);
                    
                    containerId = await _containerManager.CreateContainerAsync(
                        job.Code, 
                        job.Files, 
                        job.EntryPoint);
                    
                    break; // Success, exit retry loop
                }
                catch (Exception ex)
                {
                    retryCount++;
                    _logger.LogWarning(ex, 
                        "Failed to create container for job {JobId} (attempt {Attempt}/{MaxRetries})", 
                        job.JobId, retryCount, MaxRetries);

                    if (retryCount >= MaxRetries)
                    {
                        throw; // Re-throw after max retries
                    }

                    // Wait before retrying
                    await Task.Delay(TimeSpan.FromMilliseconds(100 * retryCount), cancellationToken);
                }
            }

            if (string.IsNullOrEmpty(containerId))
            {
                throw new InvalidOperationException("Failed to create container");
            }

            // Step 3: Execute code in container with timeout
            // Validates: Requirements 3.7, 3.8, 14.3
            _logger.LogDebug("Starting container {ContainerId} for job {JobId}", containerId, job.JobId);
            
            var executionResult = await _containerManager.StartContainerAsync(
                containerId, 
                ExecutionTimeout);

            // Step 4: Capture output and determine status
            // Validates: Requirements 3.6
            var status = DetermineExecutionStatus(executionResult);
            
            var result = new ExecutionResult
            {
                JobId = job.JobId,
                Status = status,
                Output = executionResult.Output,
                Error = executionResult.Error,
                ExitCode = executionResult.ExitCode,
                ExecutionTimeMs = executionResult.ExecutionTimeMs,
                CompletedAt = DateTime.UtcNow
            };

            // Step 5: Store execution results in Redis
            // Validates: Requirements 12.3
            _logger.LogInformation(
                "Job {JobId} completed with status {Status} in {ExecutionTime}ms", 
                job.JobId, status, executionResult.ExecutionTimeMs);
            
            await _jobQueue.StoreResultAsync(result);
            
            // Track metrics
            ApplicationMetrics.JobsProcessed
                .WithLabels(workerId, status.ToString())
                .Inc();
            
            ApplicationMetrics.JobProcessingDuration
                .WithLabels(workerId)
                .Observe(stopwatch.Elapsed.TotalSeconds);
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning("Job {JobId} was cancelled", job.JobId);
            
            // Store cancellation result
            var cancelResult = new ExecutionResult
            {
                JobId = job.JobId,
                Status = ExecutionStatus.Failed,
                Error = "Job processing was cancelled",
                ExitCode = -1,
                ExecutionTimeMs = stopwatch.ElapsedMilliseconds,
                CompletedAt = DateTime.UtcNow
            };
            
            await _jobQueue.StoreResultAsync(cancelResult);
            
            // Track metrics
            ApplicationMetrics.JobsProcessed
                .WithLabels(workerId, "Cancelled")
                .Inc();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing job {JobId}", job.JobId);

            // Implement job requeue on worker failure
            // Validates: Requirements 12.6
            try
            {
                // Check if job should be requeued (not too old)
                var jobAge = DateTime.UtcNow - job.EnqueuedAt;
                if (jobAge < TimeSpan.FromMinutes(5))
                {
                    _logger.LogInformation("Requeuing job {JobId} due to worker failure", job.JobId);
                    await _jobQueue.EnqueueJobAsync(job);
                }
                else
                {
                    _logger.LogWarning("Job {JobId} is too old ({Age}), not requeuing", 
                        job.JobId, jobAge);
                    
                    // Store failure result
                    var failureResult = new ExecutionResult
                    {
                        JobId = job.JobId,
                        Status = ExecutionStatus.Failed,
                        Error = $"Worker failure: {ex.Message}",
                        ExitCode = -1,
                        ExecutionTimeMs = stopwatch.ElapsedMilliseconds,
                        CompletedAt = DateTime.UtcNow
                    };
                    
                    await _jobQueue.StoreResultAsync(failureResult);
                }
            }
            catch (Exception requeueEx)
            {
                _logger.LogError(requeueEx, "Failed to requeue job {JobId}", job.JobId);
            }
        }
        finally
        {
            // Step 6: Cleanup container
            // Validates: Requirements 14.6
            if (!string.IsNullOrEmpty(containerId))
            {
                try
                {
                    _logger.LogDebug("Cleaning up container {ContainerId}", containerId);
                    await _containerManager.StopAndRemoveContainerAsync(containerId);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error cleaning up container {ContainerId}", containerId);
                }
            }

            stopwatch.Stop();
            
            // Track worker utilization (job completed)
            ApplicationMetrics.WorkerUtilization
                .WithLabels(workerId)
                .Set(0.0);
        }
    }

    /// <summary>
    /// Determines the execution status based on container execution result
    /// Validates: Requirements 3.7, 3.8
    /// </summary>
    private ExecutionStatus DetermineExecutionStatus(ContainerExecutionResult executionResult)
    {
        // Check for timeout
        if (executionResult.TimedOut)
        {
            return ExecutionStatus.Timeout;
        }

        // Check for memory exceeded
        if (executionResult.MemoryExceeded)
        {
            return ExecutionStatus.MemoryExceeded;
        }

        // Check exit code
        if (executionResult.ExitCode == 0)
        {
            return ExecutionStatus.Completed;
        }

        return ExecutionStatus.Failed;
    }

    /// <summary>
    /// Formats prohibited code violations into an error message
    /// </summary>
    private string FormatProhibitedCodeError(CodeScanResult scanResult)
    {
        var violations = scanResult.Violations
            .Select(v => $"Line {v.Line}: {v.Operation} - {v.Reason}")
            .ToList();

        return $"Code contains prohibited operations:\n{string.Join("\n", violations)}";
    }
}
