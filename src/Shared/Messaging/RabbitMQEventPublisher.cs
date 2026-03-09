using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using Shared.Interfaces;
using Shared.Models;
using System.Text;
using System.Text.Json;

namespace Shared.Messaging;

/// <summary>
/// Publishes domain events to RabbitMQ with JSON serialization, routing, and retry logic
/// </summary>
public class RabbitMQEventPublisher : IEventPublisher
{
    private readonly IRabbitMQConnectionManager _connectionManager;
    private readonly ILogger<RabbitMQEventPublisher> _logger;
    private readonly ResiliencePipeline _retryPipeline;
    private readonly JsonSerializerOptions _jsonOptions;

    private const string ExchangeName = "learning.events";
    private const string ExchangeType = "topic";
    private const int MaxRetryAttempts = 3;
    private const int RetryDelayMs = 500;

    public RabbitMQEventPublisher(
        IRabbitMQConnectionManager connectionManager,
        ILogger<RabbitMQEventPublisher> logger)
    {
        _connectionManager = connectionManager ?? throw new ArgumentNullException(nameof(connectionManager));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        // Configure JSON serialization options
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false
        };

        // Configure retry policy with exponential backoff
        _retryPipeline = new ResiliencePipelineBuilder()
            .AddRetry(new RetryStrategyOptions
            {
                MaxRetryAttempts = MaxRetryAttempts,
                Delay = TimeSpan.FromMilliseconds(RetryDelayMs),
                BackoffType = DelayBackoffType.Exponential,
                UseJitter = true,
                OnRetry = args =>
                {
                    _logger.LogWarning(
                        "Event publish failed. Attempt {Attempt} of {MaxAttempts}. Retrying in {Delay}ms. Error: {Error}",
                        args.AttemptNumber + 1,
                        MaxRetryAttempts,
                        args.RetryDelay.TotalMilliseconds,
                        args.Outcome.Exception?.Message);
                    return ValueTask.CompletedTask;
                },
                ShouldHandle = new PredicateBuilder()
                    .Handle<BrokerUnreachableException>()
                    .Handle<OperationInterruptedException>()
                    .Handle<TimeoutException>()
                    .Handle<AlreadyClosedException>()
            })
            .Build();

        _logger.LogInformation("RabbitMQ Event Publisher initialized");
    }

    /// <summary>
    /// Publishes a domain event to RabbitMQ with appropriate routing
    /// </summary>
    public async Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default) 
        where TEvent : DomainEvent
    {
        if (@event == null)
        {
            throw new ArgumentNullException(nameof(@event));
        }

        try
        {
            await _retryPipeline.ExecuteAsync(async ct =>
            {
                await PublishEventInternalAsync(@event, ct);
            }, cancellationToken);

            _logger.LogInformation(
                "Successfully published event {EventType} with ID {EventId}",
                @event.EventType,
                @event.EventId);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Failed to publish event {EventType} with ID {EventId} after {MaxAttempts} attempts",
                @event.EventType,
                @event.EventId,
                MaxRetryAttempts);
            throw;
        }
    }

    private async Task PublishEventInternalAsync<TEvent>(TEvent @event, CancellationToken cancellationToken) 
        where TEvent : DomainEvent
    {
        // Ensure exchange exists
        await EnsureExchangeExistsAsync();

        // Serialize event to JSON
        var json = JsonSerializer.Serialize(@event, _jsonOptions);
        var body = Encoding.UTF8.GetBytes(json);

        // Generate routing key based on event type
        var routingKey = GenerateRoutingKey(@event);

        // Publish to RabbitMQ
        await Task.Run(() =>
        {
            using var channel = _connectionManager.CreateChannel();

            var properties = channel.CreateBasicProperties();
            properties.Persistent = true; // Ensure message survives broker restart
            properties.ContentType = "application/json";
            properties.ContentEncoding = "utf-8";
            properties.Timestamp = new AmqpTimestamp(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
            properties.MessageId = @event.EventId.ToString();
            properties.Type = @event.EventType;

            // Add custom headers
            properties.Headers = new Dictionary<string, object>
            {
                { "event-type", @event.EventType },
                { "event-id", @event.EventId.ToString() },
                { "occurred-at", @event.OccurredAt.ToString("O") }
            };

            channel.BasicPublish(
                exchange: ExchangeName,
                routingKey: routingKey,
                mandatory: false,
                basicProperties: properties,
                body: body);

            _logger.LogDebug(
                "Published event {EventType} to exchange {Exchange} with routing key {RoutingKey}",
                @event.EventType,
                ExchangeName,
                routingKey);
        }, cancellationToken);
    }

    /// <summary>
    /// Generates a routing key based on the event type
    /// Format: {event.type}.{specific.details}
    /// Examples: 
    /// - challenge.completed.easy
    /// - challenge.completed.medium
    /// - lesson.completed
    /// </summary>
    private string GenerateRoutingKey<TEvent>(TEvent @event) where TEvent : DomainEvent
    {
        var baseKey = @event.EventType;

        // Add specific routing details based on event type
        return @event switch
        {
            ChallengeCompletedEvent challengeEvent => 
                $"{baseKey}.{challengeEvent.Difficulty.ToLowerInvariant()}",
            
            LessonCompletedEvent lessonEvent => 
                baseKey, // Simple routing for lesson events
            
            _ => baseKey // Default routing key
        };
    }

    /// <summary>
    /// Ensures the exchange exists (idempotent operation)
    /// </summary>
    private async Task EnsureExchangeExistsAsync()
    {
        try
        {
            await _connectionManager.DeclareExchangeAsync(
                exchangeName: ExchangeName,
                exchangeType: ExchangeType,
                durable: true,
                autoDelete: false);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to declare exchange {ExchangeName}, it may already exist", ExchangeName);
            // Exchange may already exist, which is fine
        }
    }
}
