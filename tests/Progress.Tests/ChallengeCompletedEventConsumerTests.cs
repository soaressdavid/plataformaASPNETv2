using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Progress.Service.Consumers;
using Progress.Service.Services;
using RabbitMQ.Client;
using Shared.Interfaces;
using Shared.Models;
using Xunit;

namespace Progress.Tests;

/// <summary>
/// Unit tests for ChallengeCompletedEventConsumer
/// Validates: Requirement 11.6 - RabbitMQ for asynchronous communication
/// </summary>
public class ChallengeCompletedEventConsumerTests
{
    private readonly Mock<IRabbitMQConnectionManager> _connectionManagerMock;
    private readonly Mock<IServiceScopeFactory> _serviceScopeFactoryMock;
    private readonly Mock<IServiceScope> _serviceScopeMock;
    private readonly Mock<ProgressService> _progressServiceMock;
    private readonly Mock<ILogger<ChallengeCompletedEventConsumer>> _loggerMock;
    private readonly Mock<IModel> _channelMock;

    public ChallengeCompletedEventConsumerTests()
    {
        _connectionManagerMock = new Mock<IRabbitMQConnectionManager>();
        _serviceScopeFactoryMock = new Mock<IServiceScopeFactory>();
        _serviceScopeMock = new Mock<IServiceScope>();
        _progressServiceMock = new Mock<ProgressService>(MockBehavior.Strict, null!, null!);
        _loggerMock = new Mock<ILogger<ChallengeCompletedEventConsumer>>();
        _channelMock = new Mock<IModel>();

        // Setup connection manager to return mock channel
        _connectionManagerMock
            .Setup(m => m.CreateChannel())
            .Returns(_channelMock.Object);

        // Setup service provider to return scoped services
        var scopeServiceProviderMock = new Mock<IServiceProvider>();
        scopeServiceProviderMock
            .Setup(sp => sp.GetService(typeof(ProgressService)))
            .Returns(_progressServiceMock.Object);

        _serviceScopeMock
            .Setup(s => s.ServiceProvider)
            .Returns(scopeServiceProviderMock.Object);

        // Mock IServiceScopeFactory instead of extension method
        _serviceScopeFactoryMock
            .Setup(f => f.CreateScope())
            .Returns(_serviceScopeMock.Object);
    }

    [Fact]
    public void Constructor_WithNullConnectionManager_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            new ChallengeCompletedEventConsumer(null!, _serviceScopeFactoryMock.Object, _loggerMock.Object));
    }

    [Fact]
    public void Constructor_WithNullServiceProvider_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            new ChallengeCompletedEventConsumer(_connectionManagerMock.Object, null!, _loggerMock.Object));
    }

    [Fact]
    public void Constructor_WithNullLogger_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            new ChallengeCompletedEventConsumer(_connectionManagerMock.Object, _serviceScopeFactoryMock.Object, null!));
    }

    [Fact]
    public async Task ProcessEventAsync_WithValidEvent_AwardsXP()
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

        _progressServiceMock
            .Setup(ps => ps.AwardXPAsync(challengeEvent.UserId, challengeEvent.XpAwarded))
            .Returns(Task.CompletedTask);

        var consumer = new ChallengeCompletedEventConsumer(
            _connectionManagerMock.Object,
            _serviceScopeFactoryMock.Object,
            _loggerMock.Object);

        // Use reflection to access protected method
        var method = typeof(ChallengeCompletedEventConsumer)
            .GetMethod("ProcessEventAsync", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        // Act
        await (Task)method!.Invoke(consumer, new object[] { challengeEvent, CancellationToken.None })!;

        // Assert
        _progressServiceMock.Verify(
            ps => ps.AwardXPAsync(challengeEvent.UserId, challengeEvent.XpAwarded),
            Times.Once);
    }

    [Fact]
    public async Task ProcessEventAsync_WithEasyChallenge_Awards10XP()
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

        _progressServiceMock
            .Setup(ps => ps.AwardXPAsync(challengeEvent.UserId, 10))
            .Returns(Task.CompletedTask);

        var consumer = new ChallengeCompletedEventConsumer(
            _connectionManagerMock.Object,
            _serviceScopeFactoryMock.Object,
            _loggerMock.Object);

        var method = typeof(ChallengeCompletedEventConsumer)
            .GetMethod("ProcessEventAsync", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        // Act
        await (Task)method!.Invoke(consumer, new object[] { challengeEvent, CancellationToken.None })!;

        // Assert
        _progressServiceMock.Verify(
            ps => ps.AwardXPAsync(challengeEvent.UserId, 10),
            Times.Once);
    }

    [Fact]
    public async Task ProcessEventAsync_WithHardChallenge_Awards50XP()
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

        _progressServiceMock
            .Setup(ps => ps.AwardXPAsync(challengeEvent.UserId, 50))
            .Returns(Task.CompletedTask);

        var consumer = new ChallengeCompletedEventConsumer(
            _connectionManagerMock.Object,
            _serviceScopeFactoryMock.Object,
            _loggerMock.Object);

        var method = typeof(ChallengeCompletedEventConsumer)
            .GetMethod("ProcessEventAsync", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        // Act
        await (Task)method!.Invoke(consumer, new object[] { challengeEvent, CancellationToken.None })!;

        // Assert
        _progressServiceMock.Verify(
            ps => ps.AwardXPAsync(challengeEvent.UserId, 50),
            Times.Once);
    }

    [Fact]
    public async Task ProcessEventAsync_WhenServiceThrows_RethrowsException()
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

        _progressServiceMock
            .Setup(ps => ps.AwardXPAsync(It.IsAny<Guid>(), It.IsAny<int>()))
            .ThrowsAsync(new InvalidOperationException("Database error"));

        var consumer = new ChallengeCompletedEventConsumer(
            _connectionManagerMock.Object,
            _serviceScopeFactoryMock.Object,
            _loggerMock.Object);

        var method = typeof(ChallengeCompletedEventConsumer)
            .GetMethod("ProcessEventAsync", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            await (Task)method!.Invoke(consumer, new object[] { challengeEvent, CancellationToken.None })!);

        Assert.Equal("Database error", exception.Message);
    }
}
