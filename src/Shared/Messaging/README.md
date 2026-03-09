# RabbitMQ Connection Management

This module provides RabbitMQ connection and channel management with automatic reconnection, retry logic, and channel pooling for the ASP.NET Core Learning Platform.

## Features

- **Automatic Reconnection**: Automatically reconnects when connection is lost
- **Retry Logic**: Exponential backoff retry strategy for transient failures
- **Channel Pooling**: Reuses channels for better performance
- **Connection Events**: Comprehensive logging of connection lifecycle events
- **Thread-Safe**: Safe for concurrent access across multiple threads
- **Topology Recovery**: Automatically recovers exchanges, queues, and bindings after reconnection

## Configuration

Add the following configuration to your `appsettings.json`:

```json
{
  "RabbitMQ": {
    "HostName": "localhost",
    "Port": 5672,
    "UserName": "guest",
    "Password": "guest",
    "VirtualHost": "/",
    "MaxRetryAttempts": 5,
    "RetryDelayMs": 1000,
    "ConnectionTimeoutSeconds": 30,
    "RequestedHeartbeat": 60
  }
}
```

## Registration

Register RabbitMQ services in your `Program.cs` or `Startup.cs`:

```csharp
using Shared.Messaging;

// Using configuration
builder.Services.AddRabbitMQ(builder.Configuration);

// Or with custom options
builder.Services.AddRabbitMQ(options =>
{
    options.HostName = "rabbitmq.example.com";
    options.Port = 5672;
    options.UserName = "myuser";
    options.Password = "mypassword";
    options.MaxRetryAttempts = 3;
});
```

## Usage

### Basic Usage

Inject `IRabbitMQConnectionManager` into your services:

```csharp
using Shared.Interfaces;
using RabbitMQ.Client;

public class MyService
{
    private readonly IRabbitMQConnectionManager _connectionManager;
    private readonly ILogger<MyService> _logger;

    public MyService(IRabbitMQConnectionManager connectionManager, ILogger<MyService> logger)
    {
        _connectionManager = connectionManager;
        _logger = logger;
    }

    public async Task InitializeAsync()
    {
        // Declare exchange
        await _connectionManager.DeclareExchangeAsync(
            exchangeName: "learning.events",
            exchangeType: "topic",
            durable: true,
            autoDelete: false
        );

        // Declare queue
        await _connectionManager.DeclareQueueAsync(
            queueName: "challenge.completed",
            durable: true,
            exclusive: false,
            autoDelete: false
        );

        // Bind queue to exchange
        await _connectionManager.BindQueueAsync(
            queueName: "challenge.completed",
            exchangeName: "learning.events",
            routingKey: "challenge.completed.*"
        );
    }

    public async Task PublishMessageAsync(string message)
    {
        using var channel = _connectionManager.CreateChannel();
        
        var body = Encoding.UTF8.GetBytes(message);
        var properties = channel.CreateBasicProperties();
        properties.Persistent = true;
        properties.ContentType = "application/json";
        properties.Timestamp = new AmqpTimestamp(DateTimeOffset.UtcNow.ToUnixTimeSeconds());

        channel.BasicPublish(
            exchange: "learning.events",
            routingKey: "challenge.completed.easy",
            mandatory: false,
            basicProperties: properties,
            body: body
        );

        _logger.LogInformation("Published message to RabbitMQ");
    }
}
```

### Declaring Exchanges

```csharp
// Direct exchange
await _connectionManager.DeclareExchangeAsync("direct.exchange", "direct");

// Topic exchange
await _connectionManager.DeclareExchangeAsync("topic.exchange", "topic");

// Fanout exchange
await _connectionManager.DeclareExchangeAsync("fanout.exchange", "fanout");

// Headers exchange
await _connectionManager.DeclareExchangeAsync("headers.exchange", "headers");
```

### Declaring Queues

```csharp
// Durable queue (survives broker restart)
await _connectionManager.DeclareQueueAsync("my.queue", durable: true);

// Exclusive queue (deleted when connection closes)
await _connectionManager.DeclareQueueAsync("temp.queue", exclusive: true);

// Auto-delete queue (deleted when no consumers)
await _connectionManager.DeclareQueueAsync("auto.queue", autoDelete: true);
```

### Binding Queues to Exchanges

```csharp
// Direct binding
await _connectionManager.BindQueueAsync("my.queue", "direct.exchange", "my.routing.key");

// Topic binding with wildcards
await _connectionManager.BindQueueAsync("events.queue", "topic.exchange", "events.*.created");

// Multiple bindings
await _connectionManager.BindQueueAsync("all.queue", "topic.exchange", "events.#");
```

## Connection Health Check

Check if the connection is healthy:

```csharp
if (_connectionManager.IsConnected)
{
    _logger.LogInformation("RabbitMQ is connected");
}
else
{
    _logger.LogWarning("RabbitMQ is not connected");
}
```

## Error Handling

The connection manager automatically handles:

- **Connection failures**: Retries with exponential backoff
- **Channel failures**: Creates new channels automatically
- **Network interruptions**: Automatic reconnection
- **Broker restarts**: Topology recovery

All errors are logged with appropriate log levels.

## Best Practices

1. **Singleton Registration**: The connection manager should be registered as a singleton (one connection per application)
2. **Channel Disposal**: Always dispose channels after use (use `using` statement)
3. **Durable Exchanges/Queues**: Use durable exchanges and queues for production to survive broker restarts
4. **Persistent Messages**: Set `Persistent = true` on message properties for important messages
5. **Heartbeats**: Configure appropriate heartbeat intervals to detect connection issues
6. **Monitoring**: Monitor connection events and errors in your logging system

## Docker Compose

Add RabbitMQ to your `docker-compose.yml`:

```yaml
services:
  rabbitmq:
    image: rabbitmq:3-management-alpine
    container_name: rabbitmq
    ports:
      - "5672:5672"   # AMQP port
      - "15672:15672" # Management UI
    environment:
      RABBITMQ_DEFAULT_USER: guest
      RABBITMQ_DEFAULT_PASS: guest
    volumes:
      - rabbitmq_data:/var/lib/rabbitmq
    networks:
      - learning-platform

volumes:
  rabbitmq_data:
```

Access the management UI at http://localhost:15672 (username: guest, password: guest)

## Troubleshooting

### Connection Refused

- Ensure RabbitMQ is running: `docker ps | grep rabbitmq`
- Check the hostname and port in configuration
- Verify firewall rules allow connections to port 5672

### Authentication Failed

- Verify username and password in configuration
- Check RabbitMQ user permissions: `rabbitmqctl list_users`

### Connection Timeout

- Increase `ConnectionTimeoutSeconds` in configuration
- Check network connectivity to RabbitMQ server
- Verify RabbitMQ is not overloaded

### Channel Closed Unexpectedly

- Check RabbitMQ logs for errors
- Verify exchange and queue declarations are correct
- Ensure message size doesn't exceed broker limits

## Event Publisher

The `RabbitMQEventPublisher` provides a high-level interface for publishing domain events to RabbitMQ with automatic serialization, routing, and retry logic.

### Features

- **JSON Serialization**: Automatically serializes events to JSON
- **Type-Based Routing**: Generates routing keys based on event type and properties
- **Retry Logic**: Exponential backoff retry strategy for publish failures (3 retries)
- **Persistent Messages**: Messages survive broker restarts
- **Event Metadata**: Includes event ID, timestamp, and type in message headers

### Domain Events

Domain events inherit from `DomainEvent` base class:

```csharp
public abstract class DomainEvent
{
    public Guid EventId { get; init; }
    public DateTime OccurredAt { get; init; }
    public abstract string EventType { get; }
}
```

Built-in events:
- `ChallengeCompletedEvent`: Published when a student completes a challenge
- `LessonCompletedEvent`: Published when a student completes a lesson

### Usage

Inject `IEventPublisher` into your services:

```csharp
using Shared.Interfaces;
using Shared.Models;

public class ChallengeService
{
    private readonly IEventPublisher _eventPublisher;

    public ChallengeService(IEventPublisher eventPublisher)
    {
        _eventPublisher = eventPublisher;
    }

    public async Task CompleteChallenge(Guid userId, Guid challengeId, string difficulty, int xpAwarded)
    {
        // ... business logic ...

        // Publish event
        var challengeEvent = new ChallengeCompletedEvent
        {
            UserId = userId,
            ChallengeId = challengeId,
            Difficulty = difficulty,
            XpAwarded = xpAwarded,
            SubmissionId = submissionId
        };

        await _eventPublisher.PublishAsync(challengeEvent);
    }
}
```

### Routing Keys

The event publisher generates routing keys based on event type:

- `ChallengeCompletedEvent`: `challenge.completed.{difficulty}` (e.g., `challenge.completed.easy`)
- `LessonCompletedEvent`: `lesson.completed`

This allows consumers to subscribe to specific event types using topic exchange patterns:
- `challenge.completed.*` - All challenge completions
- `challenge.completed.hard` - Only hard challenge completions
- `lesson.completed` - All lesson completions
- `#` - All events

### Creating Custom Events

To create a custom domain event:

```csharp
public class ProjectCompletedEvent : DomainEvent
{
    public override string EventType => "project.completed";
    
    public Guid UserId { get; init; }
    public Guid ProjectId { get; init; }
    public int XpAwarded { get; init; }
}
```

Then publish it:

```csharp
var projectEvent = new ProjectCompletedEvent
{
    UserId = userId,
    ProjectId = projectId,
    XpAwarded = 100
};

await _eventPublisher.PublishAsync(projectEvent);
```

### Error Handling

The event publisher automatically retries failed publishes with exponential backoff:
- Retry 1: 500ms delay
- Retry 2: 1000ms delay
- Retry 3: 2000ms delay

If all retries fail, an exception is thrown and logged.

### Message Properties

Published messages include:
- **Persistent**: `true` (survives broker restart)
- **ContentType**: `application/json`
- **ContentEncoding**: `utf-8`
- **MessageId**: Event ID
- **Type**: Event type
- **Timestamp**: Unix timestamp
- **Headers**:
  - `event-type`: Event type string
  - `event-id`: Event ID string
  - `occurred-at`: ISO 8601 timestamp

## Related Documentation

- [RabbitMQ .NET Client Documentation](https://www.rabbitmq.com/dotnet-api-guide.html)
- [RabbitMQ Tutorials](https://www.rabbitmq.com/getstarted.html)
- [Polly Retry Policies](https://github.com/App-vNext/Polly)
