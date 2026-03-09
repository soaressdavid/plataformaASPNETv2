using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Progress.Service.Consumers;
using RabbitMQ.Client;
using Shared.Data;
using Shared.Entities;
using Shared.Interfaces;
using Shared.Models;
using Xunit;

namespace Progress.Tests;

/// <summary>
/// Unit tests for LessonCompletedEventConsumer
/// Validates: Requirement 11.6 - RabbitMQ for asynchronous communication
/// </summary>
public class LessonCompletedEventConsumerTests
{
    private readonly Mock<IRabbitMQConnectionManager> _connectionManagerMock;
    private readonly Mock<IServiceScopeFactory> _serviceScopeFactoryMock;
    private readonly Mock<ILogger<LessonCompletedEventConsumer>> _loggerMock;
    private readonly Mock<IModel> _channelMock;

    public LessonCompletedEventConsumerTests()
    {
        _connectionManagerMock = new Mock<IRabbitMQConnectionManager>();
        _serviceScopeFactoryMock = new Mock<IServiceScopeFactory>();
        _loggerMock = new Mock<ILogger<LessonCompletedEventConsumer>>();
        _channelMock = new Mock<IModel>();

        // Setup connection manager to return mock channel
        _connectionManagerMock
            .Setup(m => m.CreateChannel())
            .Returns(_channelMock.Object);
    }

    [Fact]
    public void Constructor_WithNullConnectionManager_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            new LessonCompletedEventConsumer(null!, _serviceScopeFactoryMock.Object, _loggerMock.Object));
    }

    [Fact]
    public void Constructor_WithNullServiceProvider_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            new LessonCompletedEventConsumer(_connectionManagerMock.Object, null!, _loggerMock.Object));
    }

    [Fact]
    public void Constructor_WithNullLogger_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            new LessonCompletedEventConsumer(_connectionManagerMock.Object, _serviceScopeFactoryMock.Object, null!));
    }

    [Fact]
    public async Task ProcessEventAsync_WithValidEvent_CreatesLessonCompletion()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        var dbContext = new ApplicationDbContext(options);

        var lessonEvent = new LessonCompletedEvent
        {
            UserId = Guid.NewGuid(),
            LessonId = Guid.NewGuid(),
            CourseId = Guid.NewGuid(),
            LessonOrder = 1,
            NextLessonId = Guid.NewGuid()
        };

        // Setup service provider to return scoped services
        var scopeMock = new Mock<IServiceScope>();
        var scopeServiceProviderMock = new Mock<IServiceProvider>();
        
        scopeServiceProviderMock
            .Setup(sp => sp.GetService(typeof(ApplicationDbContext)))
            .Returns(dbContext);

        scopeMock
            .Setup(s => s.ServiceProvider)
            .Returns(scopeServiceProviderMock.Object);

        _serviceScopeFactoryMock
            .Setup(sp => sp.CreateScope())
            .Returns(scopeMock.Object);

        var consumer = new LessonCompletedEventConsumer(
            _connectionManagerMock.Object,
            _serviceScopeFactoryMock.Object,
            _loggerMock.Object);

        // Use reflection to access protected method
        var method = typeof(LessonCompletedEventConsumer)
            .GetMethod("ProcessEventAsync", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        // Act
        await (Task)method!.Invoke(consumer, new object[] { lessonEvent, CancellationToken.None })!;

        // Assert
        var lessonCompletion = await dbContext.LessonCompletions
            .FirstOrDefaultAsync(lc => lc.UserId == lessonEvent.UserId && lc.LessonId == lessonEvent.LessonId);

        Assert.NotNull(lessonCompletion);
        Assert.Equal(lessonEvent.UserId, lessonCompletion.UserId);
        Assert.Equal(lessonEvent.LessonId, lessonCompletion.LessonId);
        Assert.Equal(lessonEvent.OccurredAt, lessonCompletion.CompletedAt);
    }

    [Fact]
    public async Task ProcessEventAsync_WithDuplicateEvent_SkipsCreation()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        var dbContext = new ApplicationDbContext(options);

        var userId = Guid.NewGuid();
        var lessonId = Guid.NewGuid();

        // Create existing lesson completion
        var existingCompletion = new LessonCompletion
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            LessonId = lessonId,
            CompletedAt = DateTime.UtcNow.AddDays(-1)
        };
        dbContext.LessonCompletions.Add(existingCompletion);
        await dbContext.SaveChangesAsync();

        var lessonEvent = new LessonCompletedEvent
        {
            UserId = userId,
            LessonId = lessonId,
            CourseId = Guid.NewGuid(),
            LessonOrder = 1,
            NextLessonId = Guid.NewGuid()
        };

        // Setup service provider
        var scopeMock = new Mock<IServiceScope>();
        var scopeServiceProviderMock = new Mock<IServiceProvider>();
        
        scopeServiceProviderMock
            .Setup(sp => sp.GetService(typeof(ApplicationDbContext)))
            .Returns(dbContext);

        scopeMock
            .Setup(s => s.ServiceProvider)
            .Returns(scopeServiceProviderMock.Object);

        _serviceScopeFactoryMock
            .Setup(sp => sp.CreateScope())
            .Returns(scopeMock.Object);

        var consumer = new LessonCompletedEventConsumer(
            _connectionManagerMock.Object,
            _serviceScopeFactoryMock.Object,
            _loggerMock.Object);

        var method = typeof(LessonCompletedEventConsumer)
            .GetMethod("ProcessEventAsync", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        // Act
        await (Task)method!.Invoke(consumer, new object[] { lessonEvent, CancellationToken.None })!;

        // Assert - should still have only one completion
        var completionCount = await dbContext.LessonCompletions
            .CountAsync(lc => lc.UserId == userId && lc.LessonId == lessonId);

        Assert.Equal(1, completionCount);
    }

    [Fact]
    public async Task ProcessEventAsync_WithMultipleLessons_CreatesMultipleCompletions()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        var dbContext = new ApplicationDbContext(options);

        var userId = Guid.NewGuid();
        var courseId = Guid.NewGuid();

        var lesson1Event = new LessonCompletedEvent
        {
            UserId = userId,
            LessonId = Guid.NewGuid(),
            CourseId = courseId,
            LessonOrder = 1,
            NextLessonId = Guid.NewGuid()
        };

        var lesson2Event = new LessonCompletedEvent
        {
            UserId = userId,
            LessonId = Guid.NewGuid(),
            CourseId = courseId,
            LessonOrder = 2,
            NextLessonId = null
        };

        // Setup service provider
        var scopeMock = new Mock<IServiceScope>();
        var scopeServiceProviderMock = new Mock<IServiceProvider>();
        
        scopeServiceProviderMock
            .Setup(sp => sp.GetService(typeof(ApplicationDbContext)))
            .Returns(dbContext);

        scopeMock
            .Setup(s => s.ServiceProvider)
            .Returns(scopeServiceProviderMock.Object);

        _serviceScopeFactoryMock
            .Setup(sp => sp.CreateScope())
            .Returns(scopeMock.Object);

        var consumer = new LessonCompletedEventConsumer(
            _connectionManagerMock.Object,
            _serviceScopeFactoryMock.Object,
            _loggerMock.Object);

        var method = typeof(LessonCompletedEventConsumer)
            .GetMethod("ProcessEventAsync", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        // Act
        await (Task)method!.Invoke(consumer, new object[] { lesson1Event, CancellationToken.None })!;
        await (Task)method!.Invoke(consumer, new object[] { lesson2Event, CancellationToken.None })!;

        // Assert
        var completionCount = await dbContext.LessonCompletions
            .CountAsync(lc => lc.UserId == userId);

        Assert.Equal(2, completionCount);
    }
}

