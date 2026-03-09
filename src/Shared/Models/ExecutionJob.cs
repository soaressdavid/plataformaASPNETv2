namespace Shared.Models;

/// <summary>
/// Represents a code execution job
/// </summary>
public class ExecutionJob
{
    public Guid JobId { get; set; }
    public string Code { get; set; } = string.Empty;
    public List<string> Files { get; set; } = new();
    public string EntryPoint { get; set; } = string.Empty;
    public DateTime EnqueuedAt { get; set; }
}
