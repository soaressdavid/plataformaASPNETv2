using System.Data;
using System.Diagnostics;
using Microsoft.Data.SqlClient;

namespace SqlExecutor.Service.Services;

/// <summary>
/// Manages SQL Server containers for isolated query execution
/// Validates: Requirements 2.1, 2.2, 2.3, 2.4, 2.8, 2.9
/// </summary>
public class SqlContainerManager : ISqlContainerManager
{
    private readonly ILogger<SqlContainerManager> _logger;
    private readonly IConfiguration _configuration;
    private const int MaxResultSetSize = 1000;
    private const string SqlServerImage = "mcr.microsoft.com/mssql/server:2022-latest";
    private const string SqlPassword = "SqlExecutor@2026!";

    public SqlContainerManager(
        ILogger<SqlContainerManager> logger,
        IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }

    /// <summary>
    /// Creates a new SQL Server container with isolated temporary database
    /// </summary>
    public async Task<string> CreateContainerAsync(string sessionId, CancellationToken cancellationToken = default)
    {
        try
        {
            var containerName = $"sqlexec-{sessionId}";
            var databaseName = $"TempDB_{sessionId.Replace("-", "")}";

            _logger.LogInformation("Creating SQL Server container {ContainerName} for session {SessionId}", containerName, sessionId);

            // Create container with resource limits
            var createArgs = $"run -d --name {containerName} " +
                           $"-e \"ACCEPT_EULA=Y\" " +
                           $"-e \"SA_PASSWORD={SqlPassword}\" " +
                           $"-e \"MSSQL_PID=Express\" " +
                           $"--memory=512m " +
                           $"--cpus=0.5 " +
                           $"--network=none " +
                           $"{SqlServerImage}";

            var containerId = await ExecuteDockerCommandAsync(createArgs, cancellationToken);

            if (string.IsNullOrEmpty(containerId))
            {
                throw new Exception("Failed to create container");
            }

            _logger.LogInformation("Container {ContainerId} created, waiting for SQL Server to start...", containerId);

            // Wait for SQL Server to be ready (max 30 seconds)
            var ready = await WaitForSqlServerAsync(containerId, cancellationToken);
            if (!ready)
            {
                await DestroyContainerAsync(containerId, cancellationToken);
                throw new Exception("SQL Server failed to start within timeout");
            }

            // Create temporary database
            await CreateTemporaryDatabaseAsync(containerId, databaseName, cancellationToken);

            _logger.LogInformation("Container {ContainerId} ready with database {DatabaseName}", containerId, databaseName);

            return containerId;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating container for session {SessionId}", sessionId);
            throw;
        }
    }

    /// <summary>
    /// Executes a SQL query in the specified container
    /// </summary>
    public async Task<SqlExecutionResult> ExecuteQueryAsync(
        string containerId, 
        string query, 
        int timeoutSeconds = 5, 
        CancellationToken cancellationToken = default)
    {
        var stopwatch = Stopwatch.StartNew();
        var result = new SqlExecutionResult();

        try
        {
            _logger.LogInformation("Executing query in container {ContainerId} with {Timeout}s timeout", containerId, timeoutSeconds);

            // Get database name from container
            var databaseName = await GetDatabaseNameAsync(containerId, cancellationToken);

            // Execute query with timeout
            using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            cts.CancelAfter(TimeSpan.FromSeconds(timeoutSeconds));

            try
            {
                var queryResult = await ExecuteQueryInContainerAsync(containerId, databaseName, query, cts.Token);
                
                result.Success = true;
                result.Results = queryResult.Results;
                result.RowsAffected = queryResult.RowsAffected;
                result.ExecutionTimeMs = (int)stopwatch.ElapsedMilliseconds;
                result.TimedOut = false;

                _logger.LogInformation("Query executed successfully in {ElapsedMs}ms, {RowCount} rows returned", 
                    result.ExecutionTimeMs, result.Results?.Count ?? 0);
            }
            catch (OperationCanceledException) when (cts.Token.IsCancellationRequested && !cancellationToken.IsCancellationRequested)
            {
                result.Success = false;
                result.TimedOut = true;
                result.ExecutionTimeMs = (int)stopwatch.ElapsedMilliseconds;
                result.Error = $"Query execution exceeded {timeoutSeconds} second timeout";

                _logger.LogWarning("Query timed out after {TimeoutSeconds}s", timeoutSeconds);
            }
        }
        catch (Exception ex)
        {
            result.Success = false;
            result.Error = ex.Message;
            result.ExecutionTimeMs = (int)stopwatch.ElapsedMilliseconds;

            _logger.LogError(ex, "Error executing query in container {ContainerId}", containerId);
        }

        return result;
    }

    /// <summary>
    /// Destroys a container and cleans up resources
    /// </summary>
    public async Task DestroyContainerAsync(string containerId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Destroying container {ContainerId}", containerId);

            // Stop container
            await ExecuteDockerCommandAsync($"stop {containerId}", cancellationToken);

            // Remove container
            await ExecuteDockerCommandAsync($"rm {containerId}", cancellationToken);

            _logger.LogInformation("Container {ContainerId} destroyed", containerId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error destroying container {ContainerId}", containerId);
            throw;
        }
    }

    /// <summary>
    /// Cleans up inactive containers (30 min inactivity)
    /// </summary>
    public async Task CleanupInactiveContainersAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Starting cleanup of inactive containers");

            // List all sqlexec containers
            var listArgs = "ps -a --filter \"name=sqlexec-\" --format \"{{.ID}}\"";
            var containerIds = await ExecuteDockerCommandAsync(listArgs, cancellationToken);

            if (string.IsNullOrEmpty(containerIds))
            {
                _logger.LogInformation("No containers found for cleanup");
                return;
            }

            var ids = containerIds.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            
            foreach (var id in ids)
            {
                try
                {
                    // Check container age
                    var inspectArgs = $"inspect --format \"{{{{.State.StartedAt}}}}\" {id}";
                    var startedAt = await ExecuteDockerCommandAsync(inspectArgs, cancellationToken);
                    
                    if (DateTime.TryParse(startedAt, out var startTime))
                    {
                        var age = DateTime.UtcNow - startTime.ToUniversalTime();
                        if (age.TotalMinutes > 30)
                        {
                            _logger.LogInformation("Cleaning up container {ContainerId} (age: {Age} minutes)", id, age.TotalMinutes);
                            await DestroyContainerAsync(id, cancellationToken);
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Error checking container {ContainerId} for cleanup", id);
                }
            }

            _logger.LogInformation("Cleanup completed");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during container cleanup");
        }
    }

    private async Task<bool> WaitForSqlServerAsync(string containerId, CancellationToken cancellationToken)
    {
        const int maxRetries = 30;
        const int delayMs = 1000;

        for (int i = 0; i < maxRetries; i++)
        {
            try
            {
                var testQuery = $"exec {containerId} /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P \"{SqlPassword}\" -Q \"SELECT 1\" -C";
                var result = await ExecuteDockerCommandAsync(testQuery, cancellationToken);
                
                if (!string.IsNullOrEmpty(result) && !result.Contains("error", StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            catch
            {
                // Ignore errors during startup
            }

            await Task.Delay(delayMs, cancellationToken);
        }

        return false;
    }

    private async Task CreateTemporaryDatabaseAsync(string containerId, string databaseName, CancellationToken cancellationToken)
    {
        var createDbQuery = $"CREATE DATABASE [{databaseName}]";
        var sqlCmd = $"exec {containerId} /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P \"{SqlPassword}\" -Q \"{createDbQuery}\" -C";
        
        await ExecuteDockerCommandAsync(sqlCmd, cancellationToken);
        
        _logger.LogInformation("Created temporary database {DatabaseName} in container {ContainerId}", databaseName, containerId);
    }

    private async Task<string> GetDatabaseNameAsync(string containerId, CancellationToken cancellationToken)
    {
        // Extract session ID from container name
        var inspectArgs = $"inspect --format \"{{{{.Name}}}}\" {containerId}";
        var containerName = await ExecuteDockerCommandAsync(inspectArgs, cancellationToken);
        
        var sessionId = containerName.Replace("/sqlexec-", "").Trim();
        return $"TempDB_{sessionId.Replace("-", "")}";
    }

    private async Task<(List<Dictionary<string, object>>? Results, int RowsAffected)> ExecuteQueryInContainerAsync(
        string containerId, 
        string databaseName, 
        string query, 
        CancellationToken cancellationToken)
    {
        // For simplicity, we'll execute via sqlcmd and parse results
        // In production, you'd use a proper SQL connection
        var escapedQuery = query.Replace("\"", "\\\"");
        var sqlCmd = $"exec {containerId} /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P \"{SqlPassword}\" -d {databaseName} -Q \"{escapedQuery}\" -h -1 -s \"|\" -W -C";
        
        var output = await ExecuteDockerCommandAsync(sqlCmd, cancellationToken);
        
        // Parse output into results
        var results = ParseSqlCmdOutput(output);
        
        return (results, 0); // RowsAffected would need to be parsed from output
    }

    private List<Dictionary<string, object>>? ParseSqlCmdOutput(string output)
    {
        if (string.IsNullOrWhiteSpace(output))
            return null;

        var lines = output.Split('\n', StringSplitOptions.RemoveEmptyEntries)
                         .Select(l => l.Trim())
                         .Where(l => !string.IsNullOrEmpty(l))
                         .ToList();

        if (lines.Count < 2)
            return null;

        // First line is headers
        var headers = lines[0].Split('|', StringSplitOptions.TrimEntries);
        var results = new List<Dictionary<string, object>>();

        // Parse data rows (skip header)
        for (int i = 1; i < lines.Count && results.Count < MaxResultSetSize; i++)
        {
            var values = lines[i].Split('|', StringSplitOptions.TrimEntries);
            
            if (values.Length != headers.Length)
                continue;

            var row = new Dictionary<string, object>();
            for (int j = 0; j < headers.Length; j++)
            {
                row[headers[j]] = values[j];
            }
            
            results.Add(row);
        }

        return results;
    }

    private async Task<string> ExecuteDockerCommandAsync(string arguments, CancellationToken cancellationToken)
    {
        using var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "docker",
                Arguments = arguments,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            }
        };

        process.Start();

        var output = await process.StandardOutput.ReadToEndAsync(cancellationToken);
        var error = await process.StandardError.ReadToEndAsync(cancellationToken);

        await process.WaitForExitAsync(cancellationToken);

        if (process.ExitCode != 0 && !string.IsNullOrEmpty(error))
        {
            _logger.LogWarning("Docker command failed: {Error}", error);
        }

        return output.Trim();
    }
}
