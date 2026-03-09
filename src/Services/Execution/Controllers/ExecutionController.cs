using Execution.Service.Models;
using Execution.Service.Services;
using Microsoft.AspNetCore.Mvc;
using ExecutionResult = Shared.Models.ExecutionResult;
using ExecutionJob = Shared.Models.ExecutionJob;
using ExecutionStatus = Shared.Models.ExecutionStatus;

namespace Execution.Service.Controllers;

/// <summary>
/// Controller for code execution operations
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ExecutionController : ControllerBase
{
    private readonly IJobQueueService _jobQueueService;
    private readonly ILogger<ExecutionController> _logger;
    private const int JobTimeoutSeconds = 60;

    public ExecutionController(IJobQueueService jobQueueService, ILogger<ExecutionController> logger)
    {
        _jobQueueService = jobQueueService;
        _logger = logger;
    }

    /// <summary>
    /// Executes code by enqueueing it for processing
    /// </summary>
    [HttpPost("execute")]
    public async Task<IActionResult> ExecuteCode([FromBody] ExecuteCodeRequest request)
    {
        try
        {
            // Validate request
            if (string.IsNullOrWhiteSpace(request.Code))
            {
                return BadRequest(new { error = "Code is required" });
            }

            // Create execution job
            var job = new ExecutionJob
            {
                JobId = Guid.NewGuid(),
                Code = request.Code,
                Files = request.Files ?? new List<string>(),
                EntryPoint = request.EntryPoint ?? "Program.cs",
                EnqueuedAt = DateTime.UtcNow
            };

            // Enqueue job
            await _jobQueueService.EnqueueJobAsync(job);

            _logger.LogInformation("Enqueued job {JobId} for execution", job.JobId);

            // Return response
            var response = new ExecuteCodeResponse
            {
                JobId = job.JobId,
                Status = "Queued"
            };

            return Accepted(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to enqueue code execution");
            return StatusCode(500, new { error = "Failed to enqueue code execution", details = ex.Message });
        }
    }

    /// <summary>
    /// Gets the execution status and result for a job
    /// </summary>
    [HttpGet("status/{jobId}")]
    public async Task<IActionResult> GetExecutionStatus(Guid jobId)
    {
        try
        {
            // Check if result exists
            var result = await _jobQueueService.GetResultAsync(jobId);
            if (result != null)
            {
                return Ok(result);
            }

            // Check if job is still in queue
            var job = await _jobQueueService.GetJobAsync(jobId);
            if (job == null)
            {
                return NotFound(new { error = $"Job {jobId} not found" });
            }

            // Check if job has timed out (> 60 seconds in queue)
            var queueTime = DateTime.UtcNow - job.EnqueuedAt;
            if (queueTime.TotalSeconds > JobTimeoutSeconds)
            {
                // Mark job as timed out
                var timeoutResult = new ExecutionResult
                {
                    JobId = jobId,
                    Status = ExecutionStatus.Failed,
                    Output = string.Empty,
                    Error = $"Job timed out after {JobTimeoutSeconds} seconds in queue",
                    ExitCode = 0,
                    ExecutionTimeMs = 0,
                    CompletedAt = DateTime.UtcNow
                };

                await _jobQueueService.StoreResultAsync(timeoutResult);
                await _jobQueueService.RemoveJobAsync(jobId);

                _logger.LogWarning("Job {JobId} timed out after {Seconds} seconds", jobId, JobTimeoutSeconds);

                return Ok(timeoutResult);
            }

            // Job is still queued
            var queuedResult = new ExecutionResult
            {
                JobId = jobId,
                Status = ExecutionStatus.Queued,
                Output = string.Empty,
                Error = null,
                ExitCode = 0,
                ExecutionTimeMs = 0,
                CompletedAt = DateTime.MinValue
            };

            return Ok(queuedResult);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get execution status for job {JobId}", jobId);
            return StatusCode(500, new { error = "Failed to get execution status", details = ex.Message });
        }
    }
}
