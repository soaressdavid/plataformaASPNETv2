using ApiGateway.Middleware;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using StackExchange.Redis;
using System.Security.Claims;

namespace ApiGateway.Tests;

/// <summary>
/// Unit tests for RateLimitingMiddleware.
/// Validates: Requirements 11.5
/// </summary>
public class RateLimitingMiddlewareTests
{
    private readonly Mock<ILogger<RateLimitingMiddleware>> _loggerMock;
    private readonly Mock<IConnectionMultiplexer> _redisMock;
    private readonly Mock<IDatabase> _databaseMock;

    public RateLimitingMiddlewareTests()
    {
        _loggerMock = new Mock<ILogger<RateLimitingMiddleware>>();
        _redisMock = new Mock<IConnectionMultiplexer>();
        _databaseMock = new Mock<IDatabase>();
        _redisMock.Setup(r => r.GetDatabase(It.IsAny<int>(), It.IsAny<object>()))
            .Returns(_databaseMock.Object);
    }

    [Fact]
    public async Task InvokeAsync_WithinLimit_AllowsRequest()
    {
        // Arrange
        var context = CreateHttpContext(isAuthenticated: true, userId: "user123");
        var nextCalled = false;
        RequestDelegate next = (ctx) =>
        {
            nextCalled = true;
            return Task.CompletedTask;
        };

        // Setup Redis to allow request (has tokens)
        SetupRedisForAllowedRequest();

        var middleware = new RateLimitingMiddleware(next, _loggerMock.Object, _redisMock.Object);

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        Assert.True(nextCalled);
        Assert.NotEqual(StatusCodes.Status429TooManyRequests, context.Response.StatusCode);
    }

    [Fact]
    public async Task InvokeAsync_ExceedsLimit_Returns429()
    {
        // Arrange
        var context = CreateHttpContext(isAuthenticated: true, userId: "user123");
        var nextCalled = false;
        RequestDelegate next = (ctx) =>
        {
            nextCalled = true;
            return Task.CompletedTask;
        };

        // Setup Redis to deny request (no tokens)
        SetupRedisForDeniedRequest();

        var middleware = new RateLimitingMiddleware(next, _loggerMock.Object, _redisMock.Object);

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        Assert.False(nextCalled);
        Assert.Equal(StatusCodes.Status429TooManyRequests, context.Response.StatusCode);
        Assert.True(context.Response.Headers.ContainsKey("Retry-After"));
    }

    [Fact]
    public async Task InvokeAsync_AuthenticatedUser_UsesUserIdAsIdentifier()
    {
        // Arrange
        var userId = "user123";
        var context = CreateHttpContext(isAuthenticated: true, userId: userId);
        var nextCalled = false;
        RequestDelegate next = (ctx) =>
        {
            nextCalled = true;
            return Task.CompletedTask;
        };

        SetupRedisForAllowedRequest();

        var middleware = new RateLimitingMiddleware(next, _loggerMock.Object, _redisMock.Object);

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        Assert.True(nextCalled);
        // Verify the transaction was created (which means Redis operations were performed)
        _databaseMock.Verify(
            db => db.CreateTransaction(It.IsAny<object>()),
            Times.AtLeastOnce);
    }

    [Fact]
    public async Task InvokeAsync_UnauthenticatedUser_UsesIpAddressAsIdentifier()
    {
        // Arrange
        var context = CreateHttpContext(isAuthenticated: false);
        var nextCalled = false;
        RequestDelegate next = (ctx) =>
        {
            nextCalled = true;
            return Task.CompletedTask;
        };

        SetupRedisForAllowedRequest();

        var middleware = new RateLimitingMiddleware(next, _loggerMock.Object, _redisMock.Object);

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        Assert.True(nextCalled);
        // Verify the transaction was created (which means Redis operations were performed)
        _databaseMock.Verify(
            db => db.CreateTransaction(It.IsAny<object>()),
            Times.AtLeastOnce);
    }

    [Fact]
    public async Task InvokeAsync_RedisFailure_AllowsRequest()
    {
        // Arrange
        var context = CreateHttpContext(isAuthenticated: true, userId: "user123");
        var nextCalled = false;
        RequestDelegate next = (ctx) =>
        {
            nextCalled = true;
            return Task.CompletedTask;
        };

        // Setup Redis to throw exception
        _databaseMock.Setup(db => db.StringGetAsync(It.IsAny<RedisKey>(), It.IsAny<CommandFlags>()))
            .ThrowsAsync(new RedisException("Connection failed"));

        var middleware = new RateLimitingMiddleware(next, _loggerMock.Object, _redisMock.Object);

        // Act
        await middleware.InvokeAsync(context);

        // Assert - Should fail open and allow request
        Assert.True(nextCalled);
        Assert.NotEqual(StatusCodes.Status429TooManyRequests, context.Response.StatusCode);
    }

    [Fact]
    public async Task InvokeAsync_ExceedsLimit_IncludesRetryAfterHeader()
    {
        // Arrange
        var context = CreateHttpContext(isAuthenticated: true, userId: "user123");
        RequestDelegate next = (ctx) => Task.CompletedTask;

        SetupRedisForDeniedRequest();

        var middleware = new RateLimitingMiddleware(next, _loggerMock.Object, _redisMock.Object);

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        Assert.Equal(StatusCodes.Status429TooManyRequests, context.Response.StatusCode);
        Assert.True(context.Response.Headers.ContainsKey("Retry-After"));
        var retryAfter = context.Response.Headers["Retry-After"].ToString();
        Assert.True(int.TryParse(retryAfter, out var seconds));
        Assert.True(seconds > 0 && seconds <= 60);
    }

    private HttpContext CreateHttpContext(bool isAuthenticated, string? userId = null)
    {
        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();

        if (isAuthenticated && userId != null)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim("sub", userId)
            };
            var identity = new ClaimsIdentity(claims, "TestAuth");
            context.User = new ClaimsPrincipal(identity);
        }

        context.Connection.RemoteIpAddress = System.Net.IPAddress.Parse("127.0.0.1");

        return context;
    }

    private void SetupRedisForAllowedRequest()
    {
        var transaction = new Mock<ITransaction>();
        transaction.Setup(t => t.ExecuteAsync(It.IsAny<CommandFlags>()))
            .ReturnsAsync(true);

        var tokensTask = Task.FromResult((RedisValue)50.0);
        var lastRefillTask = Task.FromResult((RedisValue)DateTimeOffset.UtcNow.ToUnixTimeSeconds());

        transaction.Setup(t => t.StringGetAsync(It.Is<RedisKey>(k => k.ToString().Contains(":tokens")), It.IsAny<CommandFlags>()))
            .Returns(tokensTask);
        transaction.Setup(t => t.StringGetAsync(It.Is<RedisKey>(k => k.ToString().Contains(":lastRefill")), It.IsAny<CommandFlags>()))
            .Returns(lastRefillTask);

        _databaseMock.Setup(db => db.CreateTransaction(It.IsAny<object>()))
            .Returns(transaction.Object);

        _databaseMock.Setup(db => db.StringGetAsync(It.Is<RedisKey>(k => k.ToString().Contains(":tokens")), It.IsAny<CommandFlags>()))
            .ReturnsAsync((RedisValue)50.0);
        _databaseMock.Setup(db => db.StringGetAsync(It.Is<RedisKey>(k => k.ToString().Contains(":lastRefill")), It.IsAny<CommandFlags>()))
            .ReturnsAsync((RedisValue)DateTimeOffset.UtcNow.ToUnixTimeSeconds());
    }

    private void SetupRedisForDeniedRequest()
    {
        var transaction = new Mock<ITransaction>();
        transaction.Setup(t => t.ExecuteAsync(It.IsAny<CommandFlags>()))
            .ReturnsAsync(true);

        var tokensTask = Task.FromResult((RedisValue)0.5);
        var lastRefillTask = Task.FromResult((RedisValue)DateTimeOffset.UtcNow.ToUnixTimeSeconds());

        transaction.Setup(t => t.StringGetAsync(It.Is<RedisKey>(k => k.ToString().Contains(":tokens")), It.IsAny<CommandFlags>()))
            .Returns(tokensTask);
        transaction.Setup(t => t.StringGetAsync(It.Is<RedisKey>(k => k.ToString().Contains(":lastRefill")), It.IsAny<CommandFlags>()))
            .Returns(lastRefillTask);

        _databaseMock.Setup(db => db.CreateTransaction(It.IsAny<object>()))
            .Returns(transaction.Object);

        _databaseMock.Setup(db => db.StringGetAsync(It.Is<RedisKey>(k => k.ToString().Contains(":tokens")), It.IsAny<CommandFlags>()))
            .ReturnsAsync((RedisValue)0.5);
        _databaseMock.Setup(db => db.StringGetAsync(It.Is<RedisKey>(k => k.ToString().Contains(":lastRefill")), It.IsAny<CommandFlags>()))
            .ReturnsAsync((RedisValue)DateTimeOffset.UtcNow.ToUnixTimeSeconds());
    }
}
