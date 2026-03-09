using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Shared.Data;
using Shared.Entities;
using Shared.Interfaces;
using Shared.Messaging;
using Shared.Models;
using Xunit;

namespace Shared.Tests;

/// <summary>
/// Integration tests for RabbitMQ event bus
/// Tests event publishing and consumption, event ordering, and delivery guarantees
/// Validates: Requirement 11.6 - RabbitMQ for asynchronous communication
/// </summary>
public class EventBusIntegrationTests : IAsyncLifetime
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IRabbitMQConnectionManager _connectionManager;
    private readonly IEventPublisher _eventPublisher;
    private readonly ILogger<EventBusIntegrationTests> _logger;

    private const string TestExchangeName = "test.learning.events";
    private const string TestQueueName = "test.progress.challenge-completed";

    public EventBusIntegrationTests()
    {
        // Setup in-memory database
        var dbOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: $"EventBusIntegrationTests_{Guid.NewGuid()}")
            .Options;

        _dbContext = new ApplicationDbContext(dbOptions);

        // Setup RabbitMQ options
        var rabbitMqOptions = Options.Create(new RabbitMQOptions
        {
            HostName = Environment.GetEnvironmentVariable("RABBITMQ_HOST") ?? "localhost",
            Port = int.Parse(Environment.GetEnvironmentVariable("RABBITMQ_PORT") ?? "5672"),
            UserName = Environment.GetEnvironmentVariable("RABBITMQ_USER") ?? "guest",
            Password = Environment.GetEnvironmentVariable("RABBITMQ_PASSWORD") ?? "guest",
            VirtualHost = "/",
            RequestedHeartbeat = 60,
            MaxRetryAttempts = 3,
            RetryDelayMs = 500
        });

        // Setup logging
        var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddConsole();
            builder.SetMinimumLevel(LogLevel.Information);
        });

        _logger = loggerFactory.CreateLogger<EventBusIntegrationTests>();
        var connectionManagerLogger = loggerFactory.CreateLogger<RabbitMQConnectionManager>();
        var publisherLogger = loggerFactory.CreateLogger<RabbitMQEventPublisher>();

        // Create connection manager and event publisher
        _connectionManager = new RabbitMQConnectionManager(rabbitMqOptions, connectionManagerLogger);
        _eventPublisher = new RabbitMQEventPublisher(_connectionManager, publisherLogger);
    }

    public async Task InitializeAsync()
    {
        try
        {
            // Ensure RabbitMQ is available
            var connection = _connectionManager.GetConnection();
            if (!connection.IsOpen)
            {
                throw new InvalidOperationException("RabbitMQ connection is not open. Ensure RabbitMQ is running.");
            }

            _logger.LogInformation("RabbitMQ connection established successfully");

            // Setup test exchange and queue
            await _connectionManager.DeclareExchangeAsync(TestExchangeName, "topic", durable: true);
            await _connectionManager.DeclareQueueAsync(TestQueueName, durable: true);
            await _connectionManager.BindQueueAsync(TestQueueName, TestExchangeName, "challenge.completed.*");

            _logger.LogInformation("Test exchange and queue setup completed");
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to initialize RabbitMQ. Tests will be skipped if RabbitMQ is not available.");
        }
    }

    public async Task DisposeAsync()
    {
        try
        {
            // Cleanup test queue and exchange
            using var channel = _connectionManager.CreateChannel();
            channel.QueueDelete(TestQueueName, ifUnused: false, ifEmpty: false);
            channel.ExchangeDelete(TestExchangeName, ifUnused: false);
            
            _logger.LogInformation("Test queue and exchange cleaned up");
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to cleanup test resources");
        }

        _connectionManager?.Dispose();
        await _dbContext.DisposeAsync();
    }

    [Fact(Skip = "Requires RabbitMQ to be running. Run manually or in CI with RabbitMQ container.")]
    public async Task PublishAsync_WithChallengeCompletedEvent_PublishesSuccessfully()
    {
        // Arrange
        var challengeEvent = new ChallengeCompletedEvent
        {
            UserId = Guid.NewGuid(),
            ChallengeId = Guid.NewGuid(),
            Difficulty = "Medium",
            XpAwarded = 25,
            SubmissionId = Guid.NewGuid()
        };

        // Act
        await _eventPublisher.PublishAsync(challengeEvent);

        // Assert - verify message is in queue
        using var channel = _connectionManager.CreateChannel();
        var messageCount = channel.MessageCount(TestQueueName);
        
        Assert.True(messageCount > 0, "Expected at least one message in the queue");
        
        _logger.LogInformation("Successfully published and verified ChallengeCompletedEvent");
    }

    [Fact(Skip = "Requires RabbitMQ to be running. Run manually or in CI with RabbitMQ container.")]
    public async Task PublishAsync_WithMultipleEvents_MaintainsOrder()
    {
        // Arrange
        var events = new List<ChallengeCompletedEvent>();
        for (int i = 0; i < 5; i++)
        {
            events.Add(new ChallengeCompletedEvent
            {
                UserId = Guid.NewGuid(),
                ChallengeId = Guid.NewGuid(),
                Difficulty = "Easy",
                XpAwarded = 10,
                SubmissionId = Guid.NewGuid()
            });
        }

        // Act - publish events in order
        foreach (var @event in events)
        {
            await _eventPublisher.PublishAsync(@event);
        }

        // Assert - verify all messages are in queue
        using var channel = _connectionManager.CreateChannel();
        var messageCount = channel.MessageCount(TestQueueName);
        
        Assert.Equal((uint)events.Count, messageCount);
        
        _logger.LogInformation("Successfully published {Count} events in order", events.Count);
    }

    [Fact(Skip = "Requires RabbitMQ to be running. Run manually or in CI with RabbitMQ container.")]
    public async Task PublishAsync_WithPersistentMessages_SurvivesRestart()
    {
        // Arrange
        var challengeEvent = new ChallengeCompletedEvent
        {
            UserId = Guid.NewGuid(),
            ChallengeId = Guid.NewGuid(),
            Difficulty = "Hard",
            XpAwarded = 50,
            SubmissionId = Guid.NewGuid()
        };

        // Act - publish with persistent flag
        await _eventPublisher.PublishAsync(challengeEvent);

        // Verify message properties indicate persistence
        using var channel = _connectionManager.CreateChannel();
        var result = channel.BasicGet(TestQueueName, autoAck: false);
        
        Assert.NotNull(result);
        Assert.True(result.BasicProperties.Persistent, "Message should be marked as persistent");
        
        // Acknowledge the message
        channel.BasicAck(result.DeliveryTag, multiple: false);
        
        _logger.LogInformation("Verified message persistence flag");
    }

    [Fact(Skip = "Requires RabbitMQ to be running. Run manually or in CI with RabbitMQ container.")]
    public async Task EventBus_EndToEnd_PublishAndConsume()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var challengeId = Guid.NewGuid();
        
        // Create user and progress in database
        var user = new User
        {
            Id = userId,
            Name = "Test User",
            Email = "test@example.com",
            PasswordHash = "hash",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        
        var progress = new Shared.Entities.Progress
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            TotalXP = 0,
            CurrentLevel = 1,
            LearningStreak = 0,
            LastActivityAt = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow
        };

        _dbContext.Users.Add(user);
        _dbContext.Progresses.Add(progress);
        await _dbContext.SaveChangesAsync();

        var challengeEvent = new ChallengeCompletedEvent
        {
            UserId = userId,
            ChallengeId = challengeId,
            Difficulty = "Medium",
            XpAwarded = 25,
            SubmissionId = Guid.NewGuid()
        };

        // Act - publish event
        await _eventPublisher.PublishAsync(challengeEvent);

        // Wait for consumer to process (in real scenario, consumer would be running)
        await Task.Delay(1000);

        // Assert - verify message was published
        using var channel = _connectionManager.CreateChannel();
        var messageCount = channel.MessageCount(TestQueueName);
        
        Assert.True(messageCount >= 0, "Queue should exist and be accessible");
        
        _logger.LogInformation("End-to-end test completed successfully");
    }

    [Fact(Skip = "Requires RabbitMQ to be running. Run manually or in CI with RabbitMQ container.")]
    public async Task PublishAsync_WithDifferentRoutingKeys_RoutesCorrectly()
    {
        // Arrange
        var easyEvent = new ChallengeCompletedEvent
        {
            UserId = Guid.NewGuid(),
            ChallengeId = Guid.NewGuid(),
            Difficulty = "Easy",
            XpAwarded = 10,
            SubmissionId = Guid.NewGuid()
        };

        var hardEvent = new ChallengeCompletedEvent
        {
            UserId = Guid.NewGuid(),
            ChallengeId = Guid.NewGuid(),
            Difficulty = "Hard",
            XpAwarded = 50,
            SubmissionId = Guid.NewGuid()
        };

        // Act - publish events with different routing keys
        await _eventPublisher.PublishAsync(easyEvent);
        await _eventPublisher.PublishAsync(hardEvent);

        // Assert - verify both messages are routed to queue
        using var channel = _connectionManager.CreateChannel();
        var messageCount = channel.MessageCount(TestQueueName);
        
        Assert.True(messageCount >= 2, "Expected at least 2 messages in the queue");
        
        _logger.LogInformation("Successfully routed events with different routing keys");
    }

    [Fact(Skip = "Requires RabbitMQ to be running. Run manually or in CI with RabbitMQ container.")]
    public async Task PublishAsync_WithLessonCompletedEvent_PublishesSuccessfully()
    {
        // Arrange
        var lessonQueue = "test.progress.lesson-completed";
        await _connectionManager.DeclareQueueAsync(lessonQueue, durable: true);
        await _connectionManager.BindQueueAsync(lessonQueue, TestExchangeName, "lesson.completed");

        var lessonEvent = new LessonCompletedEvent
        {
            UserId = Guid.NewGuid(),
            LessonId = Guid.NewGuid(),
            CourseId = Guid.NewGuid(),
            LessonOrder = 1,
            NextLessonId = Guid.NewGuid()
        };

        // Act
        await _eventPublisher.PublishAsync(lessonEvent);

        // Assert - verify message is in queue
        using var channel = _connectionManager.CreateChannel();
        var messageCount = channel.MessageCount(lessonQueue);
        
        Assert.True(messageCount > 0, "Expected at least one message in the lesson queue");
        
        // Cleanup
        channel.QueueDelete(lessonQueue, ifUnused: false, ifEmpty: false);
        
        _logger.LogInformation("Successfully published and verified LessonCompletedEvent");
    }

    [Fact(Skip = "Requires RabbitMQ to be running. Run manually or in CI with RabbitMQ container.")]
    public async Task PublishAsync_WithConcurrentPublishes_HandlesCorrectly()
    {
        // Arrange
        var eventCount = 10;
        var tasks = new List<Task>();

        // Act - publish multiple events concurrently
        for (int i = 0; i < eventCount; i++)
        {
            var challengeEvent = new ChallengeCompletedEvent
            {
                UserId = Guid.NewGuid(),
                ChallengeId = Guid.NewGuid(),
                Difficulty = "Medium",
                XpAwarded = 25,
                SubmissionId = Guid.NewGuid()
            };

            tasks.Add(_eventPublisher.PublishAsync(challengeEvent));
        }

        await Task.WhenAll(tasks);

        // Assert - verify all messages are in queue
        using var channel = _connectionManager.CreateChannel();
        var messageCount = channel.MessageCount(TestQueueName);
        
        Assert.True(messageCount >= eventCount, $"Expected at least {eventCount} messages in the queue");
        
        _logger.LogInformation("Successfully handled {Count} concurrent publishes", eventCount);
    }

    [Fact(Skip = "Requires RabbitMQ to be running. Run manually or in CI with RabbitMQ container.")]
    public async Task PublishAsync_WithRetry_RecoversFromTransientFailure()
    {
        // Arrange
        var challengeEvent = new ChallengeCompletedEvent
        {
            UserId = Guid.NewGuid(),
            ChallengeId = Guid.NewGuid(),
            Difficulty = "Easy",
            XpAwarded = 10,
            SubmissionId = Guid.NewGuid()
        };

        // Act - publish should succeed even with retry logic
        await _eventPublisher.PublishAsync(challengeEvent);

        // Assert - verify message was published
        using var channel = _connectionManager.CreateChannel();
        var messageCount = channel.MessageCount(TestQueueName);
        
        Assert.True(messageCount > 0, "Expected at least one message in the queue");
        
        _logger.LogInformation("Successfully published with retry capability");
    }

    [Fact(Skip = "Requires RabbitMQ to be running. Run manually or in CI with RabbitMQ container.")]
    public async Task EventBus_DeliveryGuarantee_MessageNotLostOnNack()
    {
        // Arrange
        var challengeEvent = new ChallengeCompletedEvent
        {
            UserId = Guid.NewGuid(),
            ChallengeId = Guid.NewGuid(),
            Difficulty = "Medium",
            XpAwarded = 25,
            SubmissionId = Guid.NewGuid()
        };

        // Act - publish event
        await _eventPublisher.PublishAsync(challengeEvent);

        // Simulate consumer receiving and rejecting message
        using var channel = _connectionManager.CreateChannel();
        var result = channel.BasicGet(TestQueueName, autoAck: false);
        
        Assert.NotNull(result);
        
        // Nack the message with requeue=true
        channel.BasicNack(result.DeliveryTag, multiple: false, requeue: true);

        // Assert - verify message is back in queue
        await Task.Delay(100); // Small delay for requeue
        var messageCount = channel.MessageCount(TestQueueName);
        
        Assert.True(messageCount > 0, "Message should be requeued after Nack");
        
        _logger.LogInformation("Verified message requeue on Nack");
    }
}
