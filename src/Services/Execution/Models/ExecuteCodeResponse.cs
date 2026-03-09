namespace Execution.Service.Models;

/// <summary>
/// Response model for code execution request
/// </summary>
public class ExecuteCodeResponse
{
    public Guid JobId { get; set; }
    public string Status { get; set; } = string.Empty;
}
