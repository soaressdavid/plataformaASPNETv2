using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Shared.Interfaces;
using Shared.Models;
using System.Text;
using System.Text.Json;

namespace Shared.Messaging;

/// <summary>
/// Base class for RabbitMQ event consumers that run as background services
/// Provides common functionality for consuming events from RabbitMQ queues
/// </summary>
/// <typeparam name="TEvent">Type of domain event to consume</typeparam>
public abstract class EventConsumerBase<TEvent> : BackgroundService where TEvent : DomainEvent
{
    private readonly IRabbitMQConnectionManager _connectionManager;
    private readonly ILogger _logger;
    private readonly string _queueName;
    private readonly string _exchangeName;
    private readonly string _routingKey;
    private IModel? _channel;

    protected EventConsumerBase(
        IRabbitMQConnectionManager connectionManager,
        ILogger logger,
        string queueName,
        string exchangeName,
        string routingKey)
    {
        _connectionManager = connectionManager ?? throw new ArgumentNullException(nameof(connectionManager));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _queueName = queueName ?? throw new ArgumentNullException(nameof(queueName));
        _exchangeName = exchangeName ?? throw new ArgumentNullException(nameof(exchangeName));
        _routingKey = routingKey ?? throw new ArgumentNullException(nameof(routingKey));
    }

    /// <summary>
    /// Starts the consumer and begins listening for events
    /// </summary>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation(
            "{ConsumerName} starting. Queue: {QueueName}, Exchange: {ExchangeName}, RoutingKey: {RoutingKey}",
            GetType().Name, _queueName, _exchangeName, _routingKey);

        try
        {
            // Ensure exchange and queue exist
            await _connectionManager.DeclareExchangeAsync(_exchangeName, ExchangeType.Topic, durable: true);
            await _connectionManager.DeclareQueueAsync(_queueName, durable: true);
            await _connectionManager.BindQueueAsync(_queueName, _exchangeName, _routingKey);

            // Create channel for consuming
            _channel = _connectionManager.CreateChannel();
            _channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

            // Set up async consumer
            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.Received += async (sender, eventArgs) =>
            {
                await HandleMessageAsync(eventArgs, stoppingToken);
            };

            // Start consuming
            _channel.BasicConsume(
                queue: _queueName,
                autoAck: false,
                consumer: consumer);

            _logger.LogInformation("{ConsumerName} started successfully", GetType().Name);

            // Keep the service running
            await Task.Delay(Timeout.Infinite, stoppingToken);
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("{ConsumerName} is stopping", GetType().Name);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "{ConsumerName} encountered an error", GetType().Name);
            throw;
        }
    }

    /// <summary>
    /// Handles incoming messages from RabbitMQ
    /// </summary>
    private async Task HandleMessageAsync(BasicDeliverEventArgs eventArgs, CancellationToken cancellationToken)
    {
        var messageId = eventArgs.BasicProperties?.MessageId ?? "unknown";
        
        try
        {
            // Deserialize message
            var body = eventArgs.Body.ToArray();
            var json = Encoding.UTF8.GetString(body);
            
            _logger.LogDebug(
                "{ConsumerName} received message {MessageId}. Body: {Json}",
                GetType().Name, messageId, json);

            var @event = JsonSerializer.Deserialize<TEvent>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (@event == null)
            {
                _logger.LogWarning(
                    "{ConsumerName} failed to deserialize message {MessageId}",
                    GetType().Name, messageId);
                
                // Reject and don't requeue invalid messages
                _channel?.BasicNack(eventArgs.DeliveryTag, multiple: false, requeue: false);
                return;
            }

            // Process the event
            await ProcessEventAsync(@event, cancellationToken);

            // Acknowledge successful processing
            _channel?.BasicAck(eventArgs.DeliveryTag, multiple: false);
            
            _logger.LogInformation(
                "{ConsumerName} successfully processed event {EventId} (message {MessageId})",
                GetType().Name, @event.EventId, messageId);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "{ConsumerName} error processing message {MessageId}",
                GetType().Name, messageId);

            // Reject and requeue for retry
            _channel?.BasicNack(eventArgs.DeliveryTag, multiple: false, requeue: true);
        }
    }

    /// <summary>
    /// Processes the domain event. Implement this method in derived classes.
    /// </summary>
    /// <param name="event">The domain event to process</param>
    /// <param name="cancellationToken">Cancellation token</param>
    protected abstract Task ProcessEventAsync(TEvent @event, CancellationToken cancellationToken);

    /// <summary>
    /// Cleanup resources when stopping
    /// </summary>
    public override void Dispose()
    {
        _logger.LogInformation("{ConsumerName} disposing", GetType().Name);
        
        _channel?.Close();
        _channel?.Dispose();
        
        base.Dispose();
    }
}
