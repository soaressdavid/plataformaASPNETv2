namespace Worker.Service;

/// <summary>
/// Interface for managing Docker containers
/// </summary>
public interface IDockerContainerManager
{
    /// <summary>
    /// Creates a Docker container for code execution
    /// </summary>
    Task<string> CreateContainerAsync(string code, List<string> files, string entryPoint);

    /// <summary>
    /// Starts a container and waits for execution to complete
    /// </summary>
    Task<ContainerExecutionResult> StartContainerAsync(string containerId, TimeSpan timeout);

    /// <summary>
    /// Stops and removes a container
    /// </summary>
    Task StopAndRemoveContainerAsync(string containerId);
}

/// <summary>
/// Result of container execution
/// </summary>
public class ContainerExecutionResult
{
    public string Output { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
    public int ExitCode { get; set; }
    public long ExecutionTimeMs { get; set; }
    public bool TimedOut { get; set; }
    public bool MemoryExceeded { get; set; }
}
