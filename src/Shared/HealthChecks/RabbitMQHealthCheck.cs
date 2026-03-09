using Microsoft.Extensions.Diagnostics.HealthChecks;
using RabbitMQ.Client;

namespace Shared.HealthChecks;

/// <summary>
/// Health check for RabbitMQ connectivity
/// </summary>
public class RabbitMqHealthCheck : IHealthCheck
{
    private readonly string _connectionString;

    public RabbitMqHealthCheck(string connectionString)
    {
        _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
    }

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var factory = new ConnectionFactory
            {
                Uri = new Uri(_connectionString),
                RequestedConnectionTimeout = TimeSpan.FromSeconds(5),
                SocketReadTimeout = TimeSpan.FromSeconds(5),
                SocketWriteTimeout = TimeSpan.FromSeconds(5)
            };

            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            if (connection.IsOpen && channel.IsOpen)
            {
                return await Task.FromResult(HealthCheckResult.Healthy("RabbitMQ is responsive", new Dictionary<string, object>
                {
                    ["endpoint"] = connection.Endpoint.ToString(),
                    ["isOpen"] = connection.IsOpen
                }));
            }

            return await Task.FromResult(HealthCheckResult.Degraded("RabbitMQ connection is not fully open"));
        }
        catch (Exception ex)
        {
            return await Task.FromResult(HealthCheckResult.Unhealthy(
                "RabbitMQ is unavailable",
                ex,
                new Dictionary<string, object>
                {
                    ["error"] = ex.Message
                }));
        }
    }
}
