using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using Shared.Exceptions;
using Shared.Middleware;
using System.Text.Json;
using Xunit;

namespace Shared.Tests.Middleware;

public class ExceptionHandlingMiddlewareTests
{
    private readonly Mock<ILogger<ExceptionHandlingMiddleware>> _loggerMock;
    private readonly DefaultHttpContext _httpContext;

    public ExceptionHandlingMiddlewareTests()
    {
        _loggerMock = new Mock<ILogger<ExceptionHandlingMiddleware>>();
        _httpContext = new DefaultHttpContext();
        _httpContext.Response.Body = new MemoryStream();
    }

    [Fact]
    public async Task InvokeAsync_NoException_CallsNextMiddleware()
    {
        // Arrange
        var nextCalled = false;
        RequestDelegate next = (HttpContext ctx) =>
        {
            nextCalled = true;
            return Task.CompletedTask;
        };

        var middleware = new ExceptionHandlingMiddleware(next, _loggerMock.Object);

        // Act
        await middleware.InvokeAsync(_httpContext);

        // Assert
        Assert.True(nextCalled);
        Assert.Equal(200, _httpContext.Response.StatusCode);
    }

    [Fact]
    public async Task InvokeAsync_ValidationException_Returns400()
    {
        // Arrange
        var validationErrors = new Dictionary<string, string[]>
        {
            ["Title"] = new[] { "Title is required" }
        };
        
        RequestDelegate next = (HttpContext ctx) =>
        {
            throw new ValidationException(validationErrors);
        };

        var middleware = new ExceptionHandlingMiddleware(next, _loggerMock.Object);

        // Act
        await middleware.InvokeAsync(_httpContext);

        // Assert
        Assert.Equal(400, _httpContext.Response.StatusCode);
        Assert.Equal("application/problem+json", _httpContext.Response.ContentType);
    }

    [Fact]
    public async Task InvokeAsync_NotFoundException_Returns404()
    {
        // Arrange
        RequestDelegate next = (HttpContext ctx) =>
        {
            throw new NotFoundException("Course", 123);
        };

        var middleware = new ExceptionHandlingMiddleware(next, _loggerMock.Object);

        // Act
        await middleware.InvokeAsync(_httpContext);

        // Assert
        Assert.Equal(404, _httpContext.Response.StatusCode);
        Assert.Equal("application/problem+json", _httpContext.Response.ContentType);
    }

    [Fact]
    public async Task InvokeAsync_BusinessRuleException_Returns422()
    {
        // Arrange
        RequestDelegate next = (HttpContext ctx) =>
        {
            throw new BusinessRuleException("DeleteCourseRule", "Cannot delete course with active students");
        };

        var middleware = new ExceptionHandlingMiddleware(next, _loggerMock.Object);

        // Act
        await middleware.InvokeAsync(_httpContext);

        // Assert
        Assert.Equal(422, _httpContext.Response.StatusCode);
        Assert.Equal("application/problem+json", _httpContext.Response.ContentType);
    }

    [Fact]
    public async Task InvokeAsync_DomainException_Returns400()
    {
        // Arrange
        RequestDelegate next = (HttpContext ctx) =>
        {
            // Use a concrete implementation of DomainException
            throw new ValidationException(new Dictionary<string, string[]>
            {
                ["Field"] = new[] { "Invalid operation" }
            });
        };

        var middleware = new ExceptionHandlingMiddleware(next, _loggerMock.Object);

        // Act
        await middleware.InvokeAsync(_httpContext);

        // Assert
        Assert.Equal(400, _httpContext.Response.StatusCode);
        Assert.Equal("application/problem+json", _httpContext.Response.ContentType);
    }

    [Fact]
    public async Task InvokeAsync_UnhandledException_Returns500()
    {
        // Arrange
        RequestDelegate next = (HttpContext ctx) =>
        {
            throw new InvalidOperationException("Unexpected error");
        };

        var middleware = new ExceptionHandlingMiddleware(next, _loggerMock.Object);

        // Act
        await middleware.InvokeAsync(_httpContext);

        // Assert
        Assert.Equal(500, _httpContext.Response.StatusCode);
        Assert.Equal("application/problem+json", _httpContext.Response.ContentType);
    }

    [Fact]
    public async Task InvokeAsync_ValidationException_ReturnsCorrectErrorResponse()
    {
        // Arrange
        var validationErrors = new Dictionary<string, string[]>
        {
            ["Title"] = new[] { "Title is required", "Title must be at least 3 characters" },
            ["Description"] = new[] { "Description is required" }
        };
        
        RequestDelegate next = (HttpContext ctx) =>
        {
            throw new ValidationException(validationErrors);
        };

        var middleware = new ExceptionHandlingMiddleware(next, _loggerMock.Object);

        // Act
        await middleware.InvokeAsync(_httpContext);

        // Assert
        _httpContext.Response.Body.Seek(0, SeekOrigin.Begin);
        var responseBody = await new StreamReader(_httpContext.Response.Body).ReadToEndAsync();
        
        Assert.Contains("Title is required", responseBody);
        Assert.Contains("Description is required", responseBody);
    }

    [Fact]
    public async Task InvokeAsync_Exception_LogsError()
    {
        // Arrange
        var exception = new InvalidOperationException("Test error");
        RequestDelegate next = (HttpContext ctx) =>
        {
            throw exception;
        };

        var middleware = new ExceptionHandlingMiddleware(next, _loggerMock.Object);

        // Act
        await middleware.InvokeAsync(_httpContext);

        // Assert
        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => true),
                It.IsAny<Exception>(),
                It.Is<Func<It.IsAnyType, Exception?, string>>((v, t) => true)),
            Times.Once);
    }
}
