# RabbitMQ Integration Example

This document provides practical examples of integrating RabbitMQ message queue into microservices.

## Table of Contents

1. [Service Registration](#service-registration)
2. [Publishing Events](#publishing-events)
3. [Consuming Messages](#consuming-messages)
4. [Error Handling](#error-handling)
5. [Testing](#testing)

## Service Registration

### Program.cs Configuration

```csharp
using Shared.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add RabbitMQ services with automatic topology initialization
builder.Services.AddRabbitMQ(builder.Configuration, initializeTopology: true);

// Or configure manually
builder.Services.AddRabbitMQ(options =>
{
    options.HostName = builder.Configuration["RabbitMQ:HostName"] ?? "localhost";
    options.Port = int.Parse(builder.Configuration["RabbitMQ:Port"] ?? "5672");
    options.UserName = builder.Configuration["RabbitMQ:UserName"] ?? "guest";
    options.Password = builder.Configuration["RabbitMQ:Password"] ?? "guest";
    options.MaxRetryAttempts = 5;
    options.RetryDelayMs = 1000;
}, initializeTopology: true);

var app = builder.Build();
app.Run();
```

### appsettings.json

```json
{
  "RabbitMQ": {
    "HostName": "localhost",
    "Port": 5672,
    "UserName": "platform_user",
    "Password": "SimplePass123",
    "VirtualHost": "/",
    "MaxRetryAttempts": 5,
    "RetryDelayMs": 1000,
    "ConnectionTimeoutSeconds": 30,
    "RequestedHeartbeat": 60
  }
}
```

## Publishing Events

### Example 1: Challenge Completed Event

```csharp
using Shared.Interfaces;
using Shared.Models;

public class ChallengeService
{
    private readonly IEventPublisher _eventPublisher;
    private readonly ILogger<ChallengeService> _logger;

    public ChallengeService(
        IEventPublisher eventPublisher,
        ILogger<ChallengeService> logger)
    {
        _eventPublisher = eventPublisher;
        _logger = logger;
    }

    public async Task CompleteChallengeAsync(
        string userId, 
        string challengeId, 
        string difficulty,
        int xpAwarded)
    {
        // Business logic to complete challenge
        // ...

        // Publish event
        var @event = new ChallengeCompletedEvent
        {
            EventId = Guid.NewGuid(),
            EventType = "challenge.completed",
            OccurredAt = DateTime.UtcNow,
            UserId = userId,
            ChallengeId = challengeId,
            Difficulty = difficulty,
            XPAwarded = xpAwarded
        };

        try
        {
            await _eventPublisher.PublishAsync(@event);
            _logger.LogInformation(
                "Published challenge completed event for user {UserId}, challenge {ChallengeId}",
                userId, challengeId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, 
                "Failed to publish challenge completed event for user {UserId}",
                userId);
            // Event publishing failure should not fail the main operation
            // The event will be retried automatically by the publisher
        }
    }
}
```

### Example 2: Code Execution Request

```csharp
using Shared.Interfaces;

public class CodeExecutionService
{
    private readonly IEventPublisher _eventPublisher;

    public async Task<string> QueueExecutionAsync(
        string userId, 
        string code, 
        string language)
    {
        var executionId = Guid.NewGuid().ToString();

        var @event = new CodeExecutionRequestEvent
        {
            EventId = Guid.NewGuid(),
            EventType = "execution.request.code",
            OccurredAt = DateTime.UtcNow,
            ExecutionId = executionId,
            UserId = userId,
            Code = code,
            Language = language,
            TimeoutSeconds = 60
        };

        await _eventPublisher.PublishAsync(@event);

        return executionId;
    }
}
```

### Example 3: Notification Event

```csharp
public class NotificationService
{
    private readonly IEventPublisher _eventPublisher;

    public async Task SendEmailNotificationAsync(
        string userId, 
        string subject, 
        string body)
    {
        var @event = new NotificationEvent
        {
            EventId = Guid.NewGuid(),
            EventType = "notification.email.send",
            OccurredAt = DateTime.UtcNow,
            UserId = userId,
            NotificationType = "Email",
            Subject = subject,
            Body = body
        };

        await _eventPublisher.PublishAsync(@event);
    }
}
```

## Consuming Messages

### Example 1: Gamification Consumer (XP Awards)

```csharp
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Shared.Interfaces;
using System.Text;
using System.Text.Json;

public class GamificationXPConsumer : BackgroundService
{
    private readonly IRabbitMQConnectionManager _connectionManager;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<GamificationXPConsumer> _logger;
    private IModel? _channel;

    public GamificationXPConsumer(
        IRabbitMQConnectionManager connectionManager,
        IServiceProvider serviceProvider,
        ILogger<GamificationXPConsumer> logger)
    {
        _connectionManager = connectionManager;
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Gamification XP Consumer starting...");

        // Create channel
        _channel = _connectionManager.CreateChannel();

        // Set prefetch count (process 10 messages at a time)
        _channel.BasicQos(prefetchSize: 0, prefetchCount: 10, global: false);

        // Create async consumer
        var consumer = new AsyncEventingBasicConsumer(_channel);
        
        consumer.Received += async (sender, ea) =>
        {
            try
            {
                // Deserialize message
                var body = ea.Body.ToArray();
                var json = Encoding.UTF8.GetString(body);
                var @event = JsonSerializer.Deserialize<ChallengeCompletedEvent>(json);

                if (@event == null)
                {
                    _logger.LogWarning("Received null event, rejecting message");
                    _channel.BasicNack(ea.DeliveryTag, false, false);
                    return;
                }

                _logger.LogInformation(
                    "Processing XP award for user {UserId}, challenge {ChallengeId}",
                    @event.UserId, @event.ChallengeId);

                // Process message using scoped service
                using var scope = _serviceProvider.CreateScope();
                var gamificationService = scope.ServiceProvider
                    .GetRequiredService<IGamificationService>();

                await gamificationService.AwardXPAsync(
                    @event.UserId, 
                    @event.XPAwarded, 
                    $"Challenge {@ event.ChallengeId} completed");

                // Acknowledge message
                _channel.BasicAck(ea.DeliveryTag, false);

                _logger.LogInformation(
                    "Successfully processed XP award for user {UserId}",
                    @event.UserId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing XP award message");

                // Check retry count
                var retryCount = GetRetryCount(ea.BasicProperties);
                
                if (retryCount < 3)
                {
                    // Requeue for retry
                    _logger.LogWarning(
                        "Requeuing message (retry {RetryCount}/3)",
                        retryCount + 1);
                    
                    _channel.BasicNack(ea.DeliveryTag, false, true);
                }
                else
                {
                    // Send to dead letter queue
                    _logger.LogError(
                        "Max retries exceeded, sending to dead letter queue");
                    
                    _channel.BasicNack(ea.DeliveryTag, false, false);
                }
            }
        };

        // Start consuming
        _channel.BasicConsume(
            queue: "gamification.xp-awarded",
            autoAck: false,
            consumer: consumer);

        _logger.LogInformation("Gamification XP Consumer started");

        // Keep running until cancellation
        await Task.Delay(Timeout.Infinite, stoppingToken);
    }

    private int GetRetryCount(IBasicProperties properties)
    {
        if (properties.Headers != null && 
            properties.Headers.TryGetValue("x-retry-count", out var value))
        {
            return Convert.ToInt32(value);
        }
        return 0;
    }

    public override void Dispose()
    {
        _channel?.Close();
        _channel?.Dispose();
        base.Dispose();
    }
}
```

### Example 2: Notification Email Consumer

```csharp
public class NotificationEmailConsumer : BackgroundService
{
    private readonly IRabbitMQConnectionManager _connectionManager;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<NotificationEmailConsumer> _logger;
    private IModel? _channel;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _channel = _connectionManager.CreateChannel();
        _channel.BasicQos(prefetchSize: 0, prefetchCount: 5, global: false);

        var consumer = new AsyncEventingBasicConsumer(_channel);
        
        consumer.Received += async (sender, ea) =>
        {
            try
            {
                var body = ea.Body.ToArray();
                var json = Encoding.UTF8.GetString(body);
                var @event = JsonSerializer.Deserialize<NotificationEvent>(json);

                using var scope = _serviceProvider.CreateScope();
                var emailService = scope.ServiceProvider
                    .GetRequiredService<IEmailService>();

                await emailService.SendEmailAsync(
                    @event.UserId,
                    @event.Subject,
                    @event.Body);

                _channel.BasicAck(ea.DeliveryTag, false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending email notification");
                _channel.BasicNack(ea.DeliveryTag, false, false);
            }
        };

        _channel.BasicConsume(
            queue: "notification.email",
            autoAck: false,
            consumer: consumer);

        await Task.Delay(Timeout.Infinite, stoppingToken);
    }
}
```

### Example 3: Analytics Telemetry Consumer

```csharp
public class AnalyticsTelemetryConsumer : BackgroundService
{
    private readonly IRabbitMQConnectionManager _connectionManager;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<AnalyticsTelemetryConsumer> _logger;
    private IModel? _channel;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _channel = _connectionManager.CreateChannel();
        
        // Higher prefetch for analytics (can batch process)
        _channel.BasicQos(prefetchSize: 0, prefetchCount: 50, global: false);

        var consumer = new AsyncEventingBasicConsumer(_channel);
        var batch = new List<TelemetryEvent>();
        var batchTimer = new Timer(async _ => await ProcessBatch(batch), 
            null, TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(10));

        consumer.Received += async (sender, ea) =>
        {
            try
            {
                var body = ea.Body.ToArray();
                var json = Encoding.UTF8.GetString(body);
                var @event = JsonSerializer.Deserialize<TelemetryEvent>(json);

                lock (batch)
                {
                    batch.Add(@event);
                }

                // Process batch if it reaches 100 events
                if (batch.Count >= 100)
                {
                    await ProcessBatch(batch);
                }

                _channel.BasicAck(ea.DeliveryTag, false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing telemetry event");
                _channel.BasicNack(ea.DeliveryTag, false, false);
            }
        };

        _channel.BasicConsume(
            queue: "analytics.telemetry",
            autoAck: false,
            consumer: consumer);

        await Task.Delay(Timeout.Infinite, stoppingToken);
    }

    private async Task ProcessBatch(List<TelemetryEvent> batch)
    {
        if (batch.Count == 0) return;

        List<TelemetryEvent> eventsToProcess;
        lock (batch)
        {
            eventsToProcess = new List<TelemetryEvent>(batch);
            batch.Clear();
        }

        using var scope = _serviceProvider.CreateScope();
        var analyticsService = scope.ServiceProvider
            .GetRequiredService<IAnalyticsService>();

        await analyticsService.ProcessBatchAsync(eventsToProcess);

        _logger.LogInformation(
            "Processed batch of {Count} telemetry events",
            eventsToProcess.Count);
    }
}
```

## Error Handling

### Retry Strategy

```csharp
public class ResilientConsumer : BackgroundService
{
    private const int MaxRetries = 3;
    private const int RetryDelayMs = 1000;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var consumer = new AsyncEventingBasicConsumer(_channel);
        
        consumer.Received += async (sender, ea) =>
        {
            var retryCount = GetRetryCount(ea.BasicProperties);

            try
            {
                await ProcessMessageAsync(ea);
                _channel.BasicAck(ea.DeliveryTag, false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, 
                    "Error processing message (retry {RetryCount}/{MaxRetries})",
                    retryCount, MaxRetries);

                if (retryCount < MaxRetries)
                {
                    // Delay before retry
                    await Task.Delay(RetryDelayMs * (retryCount + 1));

                    // Increment retry count
                    var properties = _channel.CreateBasicProperties();
                    properties.Headers = new Dictionary<string, object>
                    {
                        { "x-retry-count", retryCount + 1 }
                    };

                    // Republish with updated retry count
                    _channel.BasicPublish(
                        exchange: "",
                        routingKey: ea.RoutingKey,
                        basicProperties: properties,
                        body: ea.Body);

                    // Ack original message
                    _channel.BasicAck(ea.DeliveryTag, false);
                }
                else
                {
                    // Send to DLQ
                    _channel.BasicNack(ea.DeliveryTag, false, false);
                }
            }
        };
    }
}
```

### Circuit Breaker Pattern

```csharp
using Polly;
using Polly.CircuitBreaker;

public class CircuitBreakerConsumer : BackgroundService
{
    private readonly AsyncCircuitBreakerPolicy _circuitBreaker;

    public CircuitBreakerConsumer()
    {
        _circuitBreaker = Policy
            .Handle<Exception>()
            .CircuitBreakerAsync(
                exceptionsAllowedBeforeBreaking: 5,
                durationOfBreak: TimeSpan.FromMinutes(1),
                onBreak: (ex, duration) =>
                {
                    _logger.LogWarning(
                        "Circuit breaker opened for {Duration}",
                        duration);
                },
                onReset: () =>
                {
                    _logger.LogInformation("Circuit breaker reset");
                });
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var consumer = new AsyncEventingBasicConsumer(_channel);
        
        consumer.Received += async (sender, ea) =>
        {
            try
            {
                await _circuitBreaker.ExecuteAsync(async () =>
                {
                    await ProcessMessageAsync(ea);
                });

                _channel.BasicAck(ea.DeliveryTag, false);
            }
            catch (BrokenCircuitException)
            {
                _logger.LogWarning("Circuit breaker is open, requeuing message");
                _channel.BasicNack(ea.DeliveryTag, false, true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing message");
                _channel.BasicNack(ea.DeliveryTag, false, false);
            }
        };
    }
}
```

## Testing

### Unit Test Example

```csharp
using Moq;
using Xunit;

public class ChallengeServiceTests
{
    [Fact]
    public async Task CompleteChallengeAsync_ShouldPublishEvent()
    {
        // Arrange
        var mockPublisher = new Mock<IEventPublisher>();
        var service = new ChallengeService(mockPublisher.Object, Mock.Of<ILogger<ChallengeService>>());

        // Act
        await service.CompleteChallengeAsync("user123", "challenge456", "Medium", 25);

        // Assert
        mockPublisher.Verify(
            p => p.PublishAsync(
                It.Is<ChallengeCompletedEvent>(e => 
                    e.UserId == "user123" && 
                    e.ChallengeId == "challenge456" &&
                    e.XPAwarded == 25),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
```

### Integration Test Example

```csharp
public class RabbitMQIntegrationTests : IClassFixture<RabbitMQFixture>
{
    private readonly RabbitMQFixture _fixture;

    public RabbitMQIntegrationTests(RabbitMQFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task PublishAndConsume_ShouldWork()
    {
        // Arrange
        var publisher = _fixture.GetEventPublisher();
        var receivedEvent = new TaskCompletionSource<ChallengeCompletedEvent>();

        // Setup consumer
        var channel = _fixture.CreateChannel();
        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.Received += async (sender, ea) =>
        {
            var body = ea.Body.ToArray();
            var json = Encoding.UTF8.GetString(body);
            var @event = JsonSerializer.Deserialize<ChallengeCompletedEvent>(json);
            receivedEvent.SetResult(@event);
            channel.BasicAck(ea.DeliveryTag, false);
        };

        channel.BasicConsume("gamification.xp-awarded", false, consumer);

        // Act
        var @event = new ChallengeCompletedEvent
        {
            EventId = Guid.NewGuid(),
            EventType = "challenge.completed",
            OccurredAt = DateTime.UtcNow,
            UserId = "test-user",
            ChallengeId = "test-challenge",
            XPAwarded = 25
        };

        await publisher.PublishAsync(@event);

        // Assert
        var result = await receivedEvent.Task.WaitAsync(TimeSpan.FromSeconds(5));
        Assert.Equal("test-user", result.UserId);
        Assert.Equal(25, result.XPAwarded);
    }
}
```

## Best Practices

1. **Always use `autoAck: false`** and manually acknowledge messages after successful processing
2. **Set appropriate prefetch counts** based on message processing time
3. **Implement retry logic** with exponential backoff
4. **Use dead letter queues** for failed messages
5. **Log all message processing** for debugging and monitoring
6. **Use scoped services** in consumers to avoid memory leaks
7. **Implement circuit breakers** for external dependencies
8. **Batch process** when possible for better performance
9. **Monitor queue lengths** and consumer lag
10. **Test with actual RabbitMQ** instance, not just mocks

## Troubleshooting

### Consumer Not Receiving Messages

1. Check queue bindings: `rabbitmqctl list_bindings`
2. Verify routing keys match
3. Check consumer is connected: `rabbitmqctl list_consumers`
4. Verify queue has messages: `rabbitmqctl list_queues`

### Messages Going to DLQ

1. Check consumer logs for errors
2. Verify message format matches expected schema
3. Check retry count in message headers
4. Review DLQ messages in management UI

### High Memory Usage

1. Reduce prefetch count
2. Implement message batching
3. Increase consumer count
4. Check for message buildup in queues

## Additional Resources

- [RabbitMQ .NET Client Documentation](https://www.rabbitmq.com/dotnet-api-guide.html)
- [RabbitMQ Tutorials](https://www.rabbitmq.com/getstarted.html)
- [Polly Resilience Library](https://github.com/App-vNext/Polly)
