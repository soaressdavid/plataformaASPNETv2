namespace SqlExecutor.Service.Services;

/// <summary>
/// Interface for managing SQL Server containers for isolated query execution
/// Validates: Requirements 2.1, 2.2, 2.3
/// </summary>
public interface ISqlContainerManager
{
    /// <summary>
    /// Creates a new SQL Server container with isolated temporary database
    /// </summary>
    Task<string> CreateContainerAsync(string sessionId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Executes a SQL query in the specified container
    /// </summary>
    Task<SqlExecutionResult> ExecuteQueryAsync(string containerId, string query, int timeoutSeconds = 5, CancellationToken cancellationToken = default);

    /// <summary>
    /// Destroys a container and cleans up resources
    /// </summary>
    Task DestroyContainerAsync(string containerId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Cleans up inactive containers (30 min inactivity)
    /// </summary>
    Task CleanupInactiveContainersAsync(CancellationToken cancellationToken = default);
}

/// <summary>
/// Result of SQL query execution
/// </summary>
public class SqlExecutionResult
{
    public bool Success { get; set; }
    public List<Dictionary<string, object>>? Results { get; set; }
    public int RowsAffected { get; set; }
    public string? Error { get; set; }
    public int ExecutionTimeMs { get; set; }
    public bool TimedOut { get; set; }
}
