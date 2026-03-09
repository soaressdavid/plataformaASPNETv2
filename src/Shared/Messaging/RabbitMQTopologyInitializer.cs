using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using Shared.Interfaces;

namespace Shared.Messaging;

/// <summary>
/// Initializes RabbitMQ topology (exchanges, queues, bindings, dead letter queues)
/// </summary>
public class RabbitMQTopologyInitializer
{
    private readonly IRabbitMQConnectionManager _connectionManager;
    private readonly ILogger<RabbitMQTopologyInitializer> _logger;

    public RabbitMQTopologyInitializer(
        IRabbitMQConnectionManager connectionManager,
        ILogger<RabbitMQTopologyInitializer> logger)
    {
        _connectionManager = connectionManager ?? throw new ArgumentNullException(nameof(connectionManager));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Initializes all exchanges, queues, and bindings for the platform
    /// </summary>
    public async Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Starting RabbitMQ topology initialization...");

        try
        {
            await CreateExchangesAsync(cancellationToken);
            await CreateQueuesAsync(cancellationToken);
            await CreateDeadLetterQueuesAsync(cancellationToken);

            _logger.LogInformation("RabbitMQ topology initialization completed successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to initialize RabbitMQ topology");
            throw;
        }
    }

    private async Task CreateExchangesAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating exchanges...");

        var exchanges = new[]
        {
            new { Name = "learning.events", Type = ExchangeType.Topic, Description = "Main event exchange for all domain events" },
            new { Name = "execution.events", Type = ExchangeType.Topic, Description = "Code execution events" },
            new { Name = "gamification.events", Type = ExchangeType.Topic, Description = "Gamification events (XP, achievements, levels)" },
            new { Name = "notification.events", Type = ExchangeType.Topic, Description = "Notification events" },
            new { Name = "analytics.events", Type = ExchangeType.Topic, Description = "Analytics and telemetry events" },
            new { Name = "dlx.events", Type = ExchangeType.Topic, Description = "Dead letter exchange for failed messages" }
        };

        foreach (var exchange in exchanges)
        {
            await _connectionManager.DeclareExchangeAsync(
                exchangeName: exchange.Name,
                exchangeType: exchange.Type,
                durable: true,
                autoDelete: false);

            _logger.LogInformation("Created exchange: {ExchangeName} ({Description})", exchange.Name, exchange.Description);
        }
    }

    private async Task CreateQueuesAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating queues and bindings...");

        // Code Executor queues
        await CreateQueueWithDLXAsync(
            queueName: "code-executor.requests",
            exchangeName: "execution.events",
            routingKey: "execution.request.*",
            dlxRoutingKey: "code-executor.failed",
            messageTtl: 300000, // 5 minutes
            maxLength: 10000);

        await CreateQueueWithDLXAsync(
            queueName: "code-executor.results",
            exchangeName: "execution.events",
            routingKey: "execution.completed.*",
            dlxRoutingKey: "code-executor.results.failed",
            messageTtl: 300000);

        // SQL Executor queues
        await CreateQueueWithDLXAsync(
            queueName: "sql-executor.requests",
            exchangeName: "execution.events",
            routingKey: "sql.request.*",
            dlxRoutingKey: "sql-executor.failed",
            messageTtl: 300000,
            maxLength: 10000);

        await CreateQueueWithDLXAsync(
            queueName: "sql-executor.results",
            exchangeName: "execution.events",
            routingKey: "sql.completed.*",
            dlxRoutingKey: "sql-executor.results.failed",
            messageTtl: 300000);

        // Gamification queues
        await CreateQueueWithDLXAsync(
            queueName: "gamification.xp-awarded",
            exchangeName: "gamification.events",
            routingKey: "gamification.xp.*",
            dlxRoutingKey: "gamification.xp.failed");

        await CreateQueueWithDLXAsync(
            queueName: "gamification.achievements",
            exchangeName: "gamification.events",
            routingKey: "gamification.achievement.*",
            dlxRoutingKey: "gamification.achievement.failed");

        await CreateQueueWithDLXAsync(
            queueName: "gamification.level-up",
            exchangeName: "gamification.events",
            routingKey: "gamification.levelup.*",
            dlxRoutingKey: "gamification.levelup.failed");

        // Notification queues
        await CreateQueueWithDLXAsync(
            queueName: "notification.email",
            exchangeName: "notification.events",
            routingKey: "notification.email.*",
            dlxRoutingKey: "notification.email.failed",
            messageTtl: 600000); // 10 minutes

        await CreateQueueWithDLXAsync(
            queueName: "notification.push",
            exchangeName: "notification.events",
            routingKey: "notification.push.*",
            dlxRoutingKey: "notification.push.failed",
            messageTtl: 600000);

        await CreateQueueWithDLXAsync(
            queueName: "notification.in-app",
            exchangeName: "notification.events",
            routingKey: "notification.inapp.*",
            dlxRoutingKey: "notification.inapp.failed");

        // Analytics queues
        await CreateQueueWithDLXAsync(
            queueName: "analytics.telemetry",
            exchangeName: "analytics.events",
            routingKey: "analytics.#",
            dlxRoutingKey: "analytics.telemetry.failed",
            messageTtl: 3600000); // 1 hour

        // AI Tutor queues
        await CreateQueueWithDLXAsync(
            queueName: "ai-tutor.requests",
            exchangeName: "learning.events",
            routingKey: "ai.request.*",
            dlxRoutingKey: "ai-tutor.failed",
            messageTtl: 300000,
            maxLength: 5000);
    }

    private async Task CreateDeadLetterQueuesAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating dead letter queues...");

        var dlqConfigs = new[]
        {
            new { Name = "dlq.code-executor", RoutingKey = "code-executor.*" },
            new { Name = "dlq.sql-executor", RoutingKey = "sql-executor.*" },
            new { Name = "dlq.gamification", RoutingKey = "gamification.*" },
            new { Name = "dlq.notification", RoutingKey = "notification.*" },
            new { Name = "dlq.analytics", RoutingKey = "analytics.*" },
            new { Name = "dlq.ai-tutor", RoutingKey = "ai-tutor.*" }
        };

        foreach (var dlq in dlqConfigs)
        {
            using var channel = _connectionManager.CreateChannel();

            // Declare dead letter queue (no DLX for DLQs themselves)
            channel.QueueDeclare(
                queue: dlq.Name,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            // Bind to dead letter exchange
            channel.QueueBind(
                queue: dlq.Name,
                exchange: "dlx.events",
                routingKey: dlq.RoutingKey);

            _logger.LogInformation("Created dead letter queue: {QueueName} with routing key {RoutingKey}",
                dlq.Name, dlq.RoutingKey);
        }
    }

    private async Task CreateQueueWithDLXAsync(
        string queueName,
        string exchangeName,
        string routingKey,
        string dlxRoutingKey,
        int? messageTtl = null,
        int? maxLength = null)
    {
        using var channel = _connectionManager.CreateChannel();

        var arguments = new Dictionary<string, object>
        {
            { "x-dead-letter-exchange", "dlx.events" },
            { "x-dead-letter-routing-key", dlxRoutingKey }
        };

        if (messageTtl.HasValue)
        {
            arguments["x-message-ttl"] = messageTtl.Value;
        }

        if (maxLength.HasValue)
        {
            arguments["x-max-length"] = maxLength.Value;
        }

        await Task.Run(() =>
        {
            // Declare queue with DLX configuration
            channel.QueueDeclare(
                queue: queueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: arguments);

            // Bind queue to exchange
            channel.QueueBind(
                queue: queueName,
                exchange: exchangeName,
                routingKey: routingKey);
        });

        _logger.LogInformation(
            "Created queue: {QueueName} bound to {ExchangeName} with routing key {RoutingKey}",
            queueName, exchangeName, routingKey);
    }
}
