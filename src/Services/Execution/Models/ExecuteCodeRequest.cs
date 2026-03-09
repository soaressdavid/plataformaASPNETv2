namespace Execution.Service.Models;

/// <summary>
/// Request model for code execution
/// </summary>
public record ExecuteCodeRequest(
    string Code,
    List<string>? Files,
    string? EntryPoint
);
