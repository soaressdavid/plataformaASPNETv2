namespace Shared.Models;

/// <summary>
/// Represents the result of a code execution
/// </summary>
public class ExecutionResult
{
    public Guid JobId { get; set; }
    public ExecutionStatus Status { get; set; }
    public string Output { get; set; } = string.Empty;
    public string? Error { get; set; }
    public int ExitCode { get; set; }
    public long ExecutionTimeMs { get; set; }
    public DateTime CompletedAt { get; set; }
}

/// <summary>
/// Execution status enumeration
/// </summary>
public enum ExecutionStatus
{
    Queued,
    Running,
    Completed,
    Failed,
    Timeout,
    MemoryExceeded
}
