using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Shared.Messaging;

/// <summary>
/// Background service that initializes RabbitMQ topology on application startup
/// </summary>
public class RabbitMQTopologyHostedService : IHostedService
{
    private readonly RabbitMQTopologyInitializer _initializer;
    private readonly ILogger<RabbitMQTopologyHostedService> _logger;

    public RabbitMQTopologyHostedService(
        RabbitMQTopologyInitializer initializer,
        ILogger<RabbitMQTopologyHostedService> logger)
    {
        _initializer = initializer ?? throw new ArgumentNullException(nameof(initializer));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("RabbitMQ Topology Hosted Service starting...");

        try
        {
            // Wait a bit for RabbitMQ to be fully ready
            await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);

            // Initialize topology
            await _initializer.InitializeAsync(cancellationToken);

            _logger.LogInformation("RabbitMQ Topology Hosted Service started successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to start RabbitMQ Topology Hosted Service");
            // Don't throw - allow the application to start even if RabbitMQ is not ready
            // The connection manager will retry when messages are published
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("RabbitMQ Topology Hosted Service stopping...");
        return Task.CompletedTask;
    }
}
