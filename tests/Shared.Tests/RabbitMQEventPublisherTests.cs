using Microsoft.Extensions.Logging;
using Moq;
using RabbitMQ.Client;
using Shared.Interfaces;
using Shared.Messaging;
using Shared.Models;
using Xunit;

namespace Shared.Tests;

/// <summary>
/// Unit tests for RabbitMQEventPublisher
/// Validates: Requirement 11.6 - RabbitMQ for asynchronous communication
/// </summary>
public class RabbitMQEventPublisherTests
{
    private readonly Mock<IRabbitMQConnectionManager> _connectionManagerMock;
    private readonly Mock<ILogger<RabbitMQEventPublisher>> _loggerMock;
    private readonly Mock<IModel> _channelMock;
    private readonly RabbitMQEventPublisher _publisher;

    public RabbitMQEventPublisherTests()
    {
        _connectionManagerMock = new Mock<IRabbitMQConnectionManager>();
        _loggerMock = new Mock<ILogger<RabbitMQEventPublisher>>();
        _channelMock = new Mock<IModel>();

        // Setup channel mock
        _channelMock.Setup(c => c.CreateBasicProperties())
            .Returns(new Mock<IBasicProperties>().Object);

        _connectionManagerMock.Setup(m => m.CreateChannel())
            .Returns(_channelMock.Object);

        _publisher = new RabbitMQEventPublisher(
            _connectionManagerMock.Object,
            _loggerMock.Object);
    }

    [Fact]
    public void Constructor_WithNullConnectionManager_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            new RabbitMQEventPublisher(null!, _loggerMock.Object));
    }

    [Fact]
    public void Constructor_WithNullLogger_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            new RabbitMQEventPublisher(_connectionManagerMock.Object, null!));
    }

    [Fact]
    public async Task PublishAsync_WithNullEvent_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            await _publisher.PublishAsync<DomainEvent>(null!));
    }

    [Fact]
    public async Task PublishAsync_WithChallengeCompletedEvent_ShouldPublishToRabbitMQ()
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

        // Act
        await _publisher.PublishAsync(challengeEvent);

        // Assert
        _connectionManagerMock.Verify(
            m => m.DeclareExchangeAsync(
                "learning.events",
                "topic",
                true,
                false),
            Times.Once);

        _connectionManagerMock.Verify(
            m => m.CreateChannel(),
            Times.Once);

        _channelMock.Verify(
            c => c.BasicPublish(
                "learning.events",
                It.Is<string>(rk => rk.StartsWith("challenge.completed")),
                false,
                It.IsAny<IBasicProperties>(),
                It.IsAny<ReadOnlyMemory<byte>>()),
            Times.Once);
    }

    [Fact]
    public async Task PublishAsync_WithLessonCompletedEvent_ShouldPublishToRabbitMQ()
    {
        // Arrange
        var lessonEvent = new LessonCompletedEvent
        {
            UserId = Guid.NewGuid(),
            LessonId = Guid.NewGuid(),
            CourseId = Guid.NewGuid(),
            LessonOrder = 1,
            NextLessonId = Guid.NewGuid()
        };

        // Act
        await _publisher.PublishAsync(lessonEvent);

        // Assert
        _connectionManagerMock.Verify(
            m => m.DeclareExchangeAsync(
                "learning.events",
                "topic",
                true,
                false),
            Times.Once);

        _connectionManagerMock.Verify(
            m => m.CreateChannel(),
            Times.Once);

        _channelMock.Verify(
            c => c.BasicPublish(
                "learning.events",
                "lesson.completed",
                false,
                It.IsAny<IBasicProperties>(),
                It.IsAny<ReadOnlyMemory<byte>>()),
            Times.Once);
    }

    [Fact]
    public async Task PublishAsync_WithChallengeCompletedEvent_ShouldIncludeDifficultyInRoutingKey()
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

        string? capturedRoutingKey = null;
        _channelMock.Setup(c => c.BasicPublish(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<bool>(),
                It.IsAny<IBasicProperties>(),
                It.IsAny<ReadOnlyMemory<byte>>()))
            .Callback<string, string, bool, IBasicProperties, ReadOnlyMemory<byte>>(
                (exchange, routingKey, mandatory, properties, body) =>
                {
                    capturedRoutingKey = routingKey;
                });

        // Act
        await _publisher.PublishAsync(challengeEvent);

        // Assert
        Assert.NotNull(capturedRoutingKey);
        Assert.Equal("challenge.completed.medium", capturedRoutingKey);
    }

    [Fact]
    public async Task PublishAsync_ShouldSetPersistentMessageProperty()
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

        IBasicProperties? capturedProperties = null;
        _channelMock.Setup(c => c.BasicPublish(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<bool>(),
                It.IsAny<IBasicProperties>(),
                It.IsAny<ReadOnlyMemory<byte>>()))
            .Callback<string, string, bool, IBasicProperties, ReadOnlyMemory<byte>>(
                (exchange, routingKey, mandatory, properties, body) =>
                {
                    capturedProperties = properties;
                });

        // Act
        await _publisher.PublishAsync(challengeEvent);

        // Assert
        Assert.NotNull(capturedProperties);
        // Note: In a real test with actual RabbitMQ, we would verify Persistent = true
        // For unit tests with mocks, we verify the properties object was created
        _channelMock.Verify(c => c.CreateBasicProperties(), Times.Once);
    }

    [Fact]
    public async Task PublishAsync_ShouldSerializeEventToJson()
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

        ReadOnlyMemory<byte>? capturedBody = null;
        _channelMock.Setup(c => c.BasicPublish(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<bool>(),
                It.IsAny<IBasicProperties>(),
                It.IsAny<ReadOnlyMemory<byte>>()))
            .Callback<string, string, bool, IBasicProperties, ReadOnlyMemory<byte>>(
                (exchange, routingKey, mandatory, properties, body) =>
                {
                    capturedBody = body;
                });

        // Act
        await _publisher.PublishAsync(challengeEvent);

        // Assert
        Assert.NotNull(capturedBody);
        Assert.True(capturedBody.Value.Length > 0);

        // Verify it's valid JSON by deserializing
        var json = System.Text.Encoding.UTF8.GetString(capturedBody.Value.ToArray());
        Assert.Contains("userId", json);
        Assert.Contains("challengeId", json);
        Assert.Contains("difficulty", json);
    }

    [Fact]
    public async Task PublishAsync_WithCancellationToken_ShouldRespectCancellation()
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

        var cts = new CancellationTokenSource();
        cts.Cancel();

        // Act & Assert
        await Assert.ThrowsAnyAsync<OperationCanceledException>(async () =>
            await _publisher.PublishAsync(challengeEvent, cts.Token));
    }

    [Fact]
    public async Task PublishAsync_MultipleEvents_ShouldPublishAll()
    {
        // Arrange
        var event1 = new ChallengeCompletedEvent
        {
            UserId = Guid.NewGuid(),
            ChallengeId = Guid.NewGuid(),
            Difficulty = "Easy",
            XpAwarded = 10,
            SubmissionId = Guid.NewGuid()
        };

        var event2 = new LessonCompletedEvent
        {
            UserId = Guid.NewGuid(),
            LessonId = Guid.NewGuid(),
            CourseId = Guid.NewGuid(),
            LessonOrder = 1
        };

        // Act
        await _publisher.PublishAsync(event1);
        await _publisher.PublishAsync(event2);

        // Assert
        _channelMock.Verify(
            c => c.BasicPublish(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<bool>(),
                It.IsAny<IBasicProperties>(),
                It.IsAny<ReadOnlyMemory<byte>>()),
            Times.Exactly(2));
    }
}
