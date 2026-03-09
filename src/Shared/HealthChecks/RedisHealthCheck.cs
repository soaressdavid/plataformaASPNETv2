using Microsoft.Extensions.Diagnostics.HealthChecks;
using StackExchange.Redis;

namespace Shared.HealthChecks;

/// <summary>
/// Health check for Redis connectivity
/// </summary>
public class RedisHealthCheck : IHealthCheck
{
    private readonly string _connectionString;
    private readonly TimeSpan _timeout;

    public RedisHealthCheck(string connectionString, TimeSpan? timeout = null)
    {
        _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        _timeout = timeout ?? TimeSpan.FromSeconds(5);
    }

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var options = ConfigurationOptions.Parse(_connectionString);
            options.ConnectTimeout = (int)_timeout.TotalMilliseconds;
            options.SyncTimeout = (int)_timeout.TotalMilliseconds;
            options.AbortOnConnectFail = false;

            using var connection = await ConnectionMultiplexer.ConnectAsync(options);
            
            if (!connection.IsConnected)
            {
                return HealthCheckResult.Unhealthy("Redis is not connected");
            }

            var database = connection.GetDatabase();
            
            // Perform a PING operation to verify Redis is responsive
            using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            cts.CancelAfter(_timeout);

            var pingTime = await database.PingAsync();

            var endpoints = connection.GetEndPoints();
            var endpointInfo = endpoints.Length > 0 ? endpoints[0].ToString() : "unknown";

            return HealthCheckResult.Healthy("Redis is responsive", new Dictionary<string, object>
            {
                { "endpoint", endpointInfo ?? "unknown" },
                { "pingTimeMs", pingTime.TotalMilliseconds },
                { "isConnected", connection.IsConnected }
            });
        }
        catch (OperationCanceledException)
        {
            return HealthCheckResult.Unhealthy($"Redis health check timed out after {_timeout.TotalSeconds} seconds");
        }
        catch (RedisConnectionException ex)
        {
            return HealthCheckResult.Unhealthy($"Redis connection failed: {ex.Message}", ex);
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy($"Redis health check failed: {ex.Message}", ex);
        }
    }
}
