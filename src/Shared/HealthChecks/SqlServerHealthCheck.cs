using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Data.SqlClient;

namespace Shared.HealthChecks;

/// <summary>
/// Health check for SQL Server database connectivity and responsiveness
/// </summary>
public class SqlServerHealthCheck : IHealthCheck
{
    private readonly string _connectionString;

    public SqlServerHealthCheck(string connectionString)
    {
        _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
    }

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        try
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);

            // Test query execution
            using var command = connection.CreateCommand();
            command.CommandText = "SELECT 1";
            command.CommandTimeout = 5; // 5 second timeout
            
            var result = await command.ExecuteScalarAsync(cancellationToken);

            if (result?.ToString() == "1")
            {
                return HealthCheckResult.Healthy("SQL Server is responsive", new Dictionary<string, object>
                {
                    ["server"] = connection.DataSource,
                    ["database"] = connection.Database,
                    ["responseTime"] = "< 5s"
                });
            }

            return HealthCheckResult.Degraded("SQL Server responded but with unexpected result");
        }
        catch (SqlException ex)
        {
            return HealthCheckResult.Unhealthy(
                "SQL Server is unavailable",
                ex,
                new Dictionary<string, object>
                {
                    ["error"] = ex.Message,
                    ["errorNumber"] = ex.Number
                });
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy(
                "SQL Server health check failed",
                ex,
                new Dictionary<string, object>
                {
                    ["error"] = ex.Message
                });
        }
    }
}
