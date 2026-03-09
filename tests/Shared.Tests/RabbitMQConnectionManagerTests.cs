using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Shared.Messaging;
using Xunit;

namespace Shared.Tests;

public class RabbitMQConnectionManagerTests
{
    private readonly Mock<ILogger<RabbitMQConnectionManager>> _loggerMock;
    private readonly RabbitMQOptions _options;

    public RabbitMQConnectionManagerTests()
    {
        _loggerMock = new Mock<ILogger<RabbitMQConnectionManager>>();
        _options = new RabbitMQOptions
        {
            HostName = "localhost",
            Port = 5672,
            UserName = "guest",
            Password = "guest",
            VirtualHost = "/",
            MaxRetryAttempts = 3,
            RetryDelayMs = 100,
            ConnectionTimeoutSeconds = 30,
            RequestedHeartbeat = 60
        };
    }

    [Fact]
    public void Constructor_WithValidOptions_ShouldInitialize()
    {
        // Arrange
        var optionsMock = Options.Create(_options);

        // Act
        var manager = new RabbitMQConnectionManager(optionsMock, _loggerMock.Object);

        // Assert
        Assert.NotNull(manager);
        Assert.False(manager.IsConnected); // Not connected until GetConnection is called
    }

    [Fact]
    public void Constructor_WithNullOptions_ShouldThrowArgumentNullException()
    {
        // Arrange
        IOptions<RabbitMQOptions>? nullOptions = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => 
            new RabbitMQConnectionManager(nullOptions!, _loggerMock.Object));
    }

    [Fact]
    public void Constructor_WithNullLogger_ShouldThrowArgumentNullException()
    {
        // Arrange
        var optionsMock = Options.Create(_options);

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => 
            new RabbitMQConnectionManager(optionsMock, null!));
    }

    [Fact(Skip = "Requires RabbitMQ running - use integration test environment")]
    public async Task DeclareExchangeAsync_WithNullExchangeName_ShouldThrowArgumentException()
    {
        // Arrange
        var optionsMock = Options.Create(_options);
        using var manager = new RabbitMQConnectionManager(optionsMock, _loggerMock.Object);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(async () =>
            await manager.DeclareExchangeAsync(null!, "topic"));
    }

    [Fact(Skip = "Requires RabbitMQ running - use integration test environment")]
    public async Task DeclareExchangeAsync_WithEmptyExchangeName_ShouldThrowArgumentException()
    {
        // Arrange
        var optionsMock = Options.Create(_options);
        using var manager = new RabbitMQConnectionManager(optionsMock, _loggerMock.Object);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(async () =>
            await manager.DeclareExchangeAsync("", "topic"));
    }

    [Fact(Skip = "Requires RabbitMQ running - use integration test environment")]
    public async Task DeclareExchangeAsync_WithNullExchangeType_ShouldThrowArgumentException()
    {
        // Arrange
        var optionsMock = Options.Create(_options);
        using var manager = new RabbitMQConnectionManager(optionsMock, _loggerMock.Object);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(async () =>
            await manager.DeclareExchangeAsync("test.exchange", null!));
    }

    [Fact(Skip = "Requires RabbitMQ running - use integration test environment")]
    public async Task DeclareQueueAsync_WithNullQueueName_ShouldThrowArgumentException()
    {
        // Arrange
        var optionsMock = Options.Create(_options);
        using var manager = new RabbitMQConnectionManager(optionsMock, _loggerMock.Object);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(async () =>
            await manager.DeclareQueueAsync(null!));
    }

    [Fact(Skip = "Requires RabbitMQ running - use integration test environment")]
    public async Task DeclareQueueAsync_WithEmptyQueueName_ShouldThrowArgumentException()
    {
        // Arrange
        var optionsMock = Options.Create(_options);
        using var manager = new RabbitMQConnectionManager(optionsMock, _loggerMock.Object);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(async () =>
            await manager.DeclareQueueAsync(""));
    }

    [Fact(Skip = "Requires RabbitMQ running - use integration test environment")]
    public async Task BindQueueAsync_WithNullQueueName_ShouldThrowArgumentException()
    {
        // Arrange
        var optionsMock = Options.Create(_options);
        using var manager = new RabbitMQConnectionManager(optionsMock, _loggerMock.Object);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(async () =>
            await manager.BindQueueAsync(null!, "exchange", "routingKey"));
    }

    [Fact(Skip = "Requires RabbitMQ running - use integration test environment")]
    public async Task BindQueueAsync_WithNullExchangeName_ShouldThrowArgumentException()
    {
        // Arrange
        var optionsMock = Options.Create(_options);
        using var manager = new RabbitMQConnectionManager(optionsMock, _loggerMock.Object);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(async () =>
            await manager.BindQueueAsync("queue", null!, "routingKey"));
    }

    [Fact(Skip = "Requires RabbitMQ running - use integration test environment")]
    public async Task BindQueueAsync_WithNullRoutingKey_ShouldThrowArgumentException()
    {
        // Arrange
        var optionsMock = Options.Create(_options);
        using var manager = new RabbitMQConnectionManager(optionsMock, _loggerMock.Object);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(async () =>
            await manager.BindQueueAsync("queue", "exchange", null!));
    }

    [Fact]
    public void Dispose_ShouldNotThrow()
    {
        // Arrange
        var optionsMock = Options.Create(_options);
        var manager = new RabbitMQConnectionManager(optionsMock, _loggerMock.Object);

        // Act & Assert
        manager.Dispose();
        // Should not throw
    }

    [Fact]
    public void Dispose_CalledMultipleTimes_ShouldNotThrow()
    {
        // Arrange
        var optionsMock = Options.Create(_options);
        var manager = new RabbitMQConnectionManager(optionsMock, _loggerMock.Object);

        // Act & Assert
        manager.Dispose();
        manager.Dispose();
        manager.Dispose();
        // Should not throw
    }

    [Fact]
    public void GetConnection_AfterDispose_ShouldThrowObjectDisposedException()
    {
        // Arrange
        var optionsMock = Options.Create(_options);
        var manager = new RabbitMQConnectionManager(optionsMock, _loggerMock.Object);
        manager.Dispose();

        // Act & Assert
        Assert.Throws<ObjectDisposedException>(() => manager.GetConnection());
    }

    [Fact]
    public void CreateChannel_AfterDispose_ShouldThrowObjectDisposedException()
    {
        // Arrange
        var optionsMock = Options.Create(_options);
        var manager = new RabbitMQConnectionManager(optionsMock, _loggerMock.Object);
        manager.Dispose();

        // Act & Assert
        Assert.Throws<ObjectDisposedException>(() => manager.CreateChannel());
    }
}
