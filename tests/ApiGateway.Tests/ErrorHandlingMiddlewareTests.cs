using ApiGateway.Middleware;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Moq;
using System.Net;
using System.Text.Json;

namespace ApiGateway.Tests;

/// <summary>
/// Unit tests for ErrorHandlingMiddleware.
/// Validates: Requirements 16.1, 16.2, 16.3, 16.4, 16.5
/// </summary>
public class ErrorHandlingMiddlewareTests
{
    private readonly Mock<ILogger<ErrorHandlingMiddleware>> _loggerMock;
    private readonly Mock<IHostEnvironment> _environmentMock;

    public ErrorHandlingMiddlewareTests()
    {
        _loggerMock = new Mock<ILogger<ErrorHandlingMiddleware>>();
        _environmentMock = new Mock<IHostEnvironment>();
        _environmentMock.Setup(e => e.EnvironmentName).Returns(Environments.Production);
    }

    [Fact]
    public async Task InvokeAsync_NoException_CallsNextMiddleware()
    {
        // Arrange
        var context = CreateHttpContext();
        var nextCalled = false;
        RequestDelegate next = (ctx) =>
        {
            nextCalled = true;
            return Task.CompletedTask;
        };

        var middleware = new ErrorHandlingMiddleware(next, _loggerMock.Object, _environmentMock.Object);

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        Assert.True(nextCalled);
    }

    [Fact]
    public async Task InvokeAsync_ArgumentException_Returns400BadRequest()
    {
        // Arrange
        var context = CreateHttpContext();
        RequestDelegate next = (ctx) => throw new ArgumentException("Invalid email format", "email");

        var middleware = new ErrorHandlingMiddleware(next, _loggerMock.Object, _environmentMock.Object);

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        Assert.Equal(StatusCodes.Status400BadRequest, context.Response.StatusCode);
        Assert.Equal("application/json", context.Response.ContentType);

        var response = await GetResponseBody(context);
        Assert.NotNull(response);
        Assert.Equal("VALIDATION_ERROR", response.Error.Code);
        Assert.Equal("Invalid input provided", response.Error.Message);
        Assert.NotNull(response.Error.TraceId);
        Assert.NotEqual(default(DateTime), response.Error.Timestamp);
    }

    [Fact]
    public async Task InvokeAsync_UnauthorizedAccessException_Returns401Unauthorized()
    {
        // Arrange
        var context = CreateHttpContext();
        RequestDelegate next = (ctx) => throw new UnauthorizedAccessException("Invalid token");

        var middleware = new ErrorHandlingMiddleware(next, _loggerMock.Object, _environmentMock.Object);

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        Assert.Equal(StatusCodes.Status401Unauthorized, context.Response.StatusCode);

        var response = await GetResponseBody(context);
        Assert.NotNull(response);
        Assert.Equal("AUTHENTICATION_ERROR", response.Error.Code);
        Assert.Equal("Authentication failed or token is invalid", response.Error.Message);
    }

    [Fact]
    public async Task InvokeAsync_KeyNotFoundException_Returns404NotFound()
    {
        // Arrange
        var context = CreateHttpContext();
        RequestDelegate next = (ctx) => throw new KeyNotFoundException("Challenge not found");

        var middleware = new ErrorHandlingMiddleware(next, _loggerMock.Object, _environmentMock.Object);

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        Assert.Equal(StatusCodes.Status404NotFound, context.Response.StatusCode);

        var response = await GetResponseBody(context);
        Assert.NotNull(response);
        Assert.Equal("RESOURCE_NOT_FOUND", response.Error.Code);
        Assert.Equal("The requested resource was not found", response.Error.Message);
    }

    [Fact]
    public async Task InvokeAsync_TimeoutException_Returns408RequestTimeout()
    {
        // Arrange
        var context = CreateHttpContext();
        RequestDelegate next = (ctx) => throw new TimeoutException("Execution timed out");

        var middleware = new ErrorHandlingMiddleware(next, _loggerMock.Object, _environmentMock.Object);

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        Assert.Equal(StatusCodes.Status408RequestTimeout, context.Response.StatusCode);

        var response = await GetResponseBody(context);
        Assert.NotNull(response);
        Assert.Equal("EXECUTION_TIMEOUT", response.Error.Code);
        Assert.Equal("Code execution exceeded 30 second time limit", response.Error.Message);
    }

    [Fact]
    public async Task InvokeAsync_CompilationError_Returns422WithLineNumber()
    {
        // Arrange
        var context = CreateHttpContext();
        RequestDelegate next = (ctx) => throw new InvalidOperationException(
            "compilation: CS0103: The name 'variableName' does not exist in the current context at line 15");

        var middleware = new ErrorHandlingMiddleware(next, _loggerMock.Object, _environmentMock.Object);

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        Assert.Equal(StatusCodes.Status422UnprocessableEntity, context.Response.StatusCode);

        var response = await GetResponseBody(context);
        Assert.NotNull(response);
        Assert.Equal("COMPILATION_ERROR", response.Error.Code);
        Assert.Equal("Code failed to compile", response.Error.Message);
        Assert.NotNull(response.Error.Details);
    }

    [Fact]
    public async Task InvokeAsync_RuntimeError_Returns422WithExceptionDetails()
    {
        // Arrange
        var context = CreateHttpContext();
        var innerException = new NullReferenceException("Object reference not set to an instance of an object");
        RequestDelegate next = (ctx) => throw new InvalidOperationException("runtime error occurred", innerException);

        var middleware = new ErrorHandlingMiddleware(next, _loggerMock.Object, _environmentMock.Object);

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        Assert.Equal(StatusCodes.Status422UnprocessableEntity, context.Response.StatusCode);

        var response = await GetResponseBody(context);
        Assert.NotNull(response);
        Assert.Equal("RUNTIME_ERROR", response.Error.Code);
        Assert.Equal("Code execution failed", response.Error.Message);
    }

    [Fact]
    public async Task InvokeAsync_MemoryExceeded_Returns422()
    {
        // Arrange
        var context = CreateHttpContext();
        RequestDelegate next = (ctx) => throw new InvalidOperationException("memory limit exceeded");

        var middleware = new ErrorHandlingMiddleware(next, _loggerMock.Object, _environmentMock.Object);

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        Assert.Equal(StatusCodes.Status422UnprocessableEntity, context.Response.StatusCode);

        var response = await GetResponseBody(context);
        Assert.NotNull(response);
        Assert.Equal("MEMORY_EXCEEDED", response.Error.Code);
        Assert.Equal("Code execution exceeded 512MB memory limit", response.Error.Message);
    }

    [Fact]
    public async Task InvokeAsync_ProhibitedCode_Returns422WithOperationDetails()
    {
        // Arrange
        var context = CreateHttpContext();
        RequestDelegate next = (ctx) => throw new InvalidOperationException(
            "prohibited: System.IO.File.ReadAllText at line 8: File system access is not allowed");

        var middleware = new ErrorHandlingMiddleware(next, _loggerMock.Object, _environmentMock.Object);

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        Assert.Equal(StatusCodes.Status422UnprocessableEntity, context.Response.StatusCode);

        var response = await GetResponseBody(context);
        Assert.NotNull(response);
        Assert.Equal("PROHIBITED_CODE", response.Error.Code);
        Assert.Equal("Code contains prohibited operations", response.Error.Message);
    }

    [Fact]
    public async Task InvokeAsync_HttpRequestException_Returns503ServiceUnavailable()
    {
        // Arrange
        var context = CreateHttpContext();
        RequestDelegate next = (ctx) => throw new HttpRequestException("Network connection failed");

        var middleware = new ErrorHandlingMiddleware(next, _loggerMock.Object, _environmentMock.Object);

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        Assert.Equal(StatusCodes.Status503ServiceUnavailable, context.Response.StatusCode);

        var response = await GetResponseBody(context);
        Assert.NotNull(response);
        Assert.Equal("NETWORK_ERROR", response.Error.Code);
        Assert.Equal("A network error occurred while processing your request", response.Error.Message);
    }

    [Fact]
    public async Task InvokeAsync_ServiceUnavailable_Returns503()
    {
        // Arrange
        var context = CreateHttpContext();
        RequestDelegate next = (ctx) => throw new InvalidOperationException("Service unavailable");

        var middleware = new ErrorHandlingMiddleware(next, _loggerMock.Object, _environmentMock.Object);

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        Assert.Equal(StatusCodes.Status503ServiceUnavailable, context.Response.StatusCode);

        var response = await GetResponseBody(context);
        Assert.NotNull(response);
        Assert.Equal("SERVICE_UNAVAILABLE", response.Error.Code);
    }

    [Fact]
    public async Task InvokeAsync_UnhandledException_Returns500InternalServerError()
    {
        // Arrange
        var context = CreateHttpContext();
        RequestDelegate next = (ctx) => throw new Exception("Unexpected error");

        var middleware = new ErrorHandlingMiddleware(next, _loggerMock.Object, _environmentMock.Object);

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        Assert.Equal(StatusCodes.Status500InternalServerError, context.Response.StatusCode);

        var response = await GetResponseBody(context);
        Assert.NotNull(response);
        Assert.Equal("INTERNAL_SERVER_ERROR", response.Error.Code);
        Assert.Equal("An unexpected error occurred while processing your request", response.Error.Message);
    }

    [Fact]
    public async Task InvokeAsync_Exception_LogsErrorWithContext()
    {
        // Arrange
        var context = CreateHttpContext();
        var exception = new Exception("Test exception");
        RequestDelegate next = (ctx) => throw exception;

        var middleware = new ErrorHandlingMiddleware(next, _loggerMock.Object, _environmentMock.Object);

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => true),
                exception,
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Fact]
    public async Task InvokeAsync_Exception_IncludesTraceIdInResponse()
    {
        // Arrange
        var context = CreateHttpContext();
        var expectedTraceId = context.TraceIdentifier;
        RequestDelegate next = (ctx) => throw new Exception("Test exception");

        var middleware = new ErrorHandlingMiddleware(next, _loggerMock.Object, _environmentMock.Object);

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        var response = await GetResponseBody(context);
        Assert.NotNull(response);
        Assert.Equal(expectedTraceId, response.Error.TraceId);
    }

    [Fact]
    public async Task InvokeAsync_Exception_IncludesTimestampInResponse()
    {
        // Arrange
        var context = CreateHttpContext();
        var beforeTime = DateTime.UtcNow;
        RequestDelegate next = (ctx) => throw new Exception("Test exception");

        var middleware = new ErrorHandlingMiddleware(next, _loggerMock.Object, _environmentMock.Object);

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        var afterTime = DateTime.UtcNow;
        var response = await GetResponseBody(context);
        Assert.NotNull(response);
        
        var timestamp = response.Error.Timestamp;
        Assert.True(timestamp >= beforeTime && timestamp <= afterTime);
    }

    [Fact]
    public async Task InvokeAsync_DevelopmentEnvironment_IncludesStackTrace()
    {
        // Arrange
        _environmentMock.Setup(e => e.EnvironmentName).Returns(Environments.Development);
        var context = CreateHttpContext();
        RequestDelegate next = (ctx) => throw new Exception("Test exception");

        var middleware = new ErrorHandlingMiddleware(next, _loggerMock.Object, _environmentMock.Object);

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        var response = await GetResponseBody(context);
        Assert.NotNull(response);
        Assert.NotNull(response.Error.Details);
    }

    [Fact]
    public async Task InvokeAsync_ProductionEnvironment_HidesStackTrace()
    {
        // Arrange
        _environmentMock.Setup(e => e.EnvironmentName).Returns(Environments.Production);
        var context = CreateHttpContext();
        RequestDelegate next = (ctx) => throw new Exception("Test exception");

        var middleware = new ErrorHandlingMiddleware(next, _loggerMock.Object, _environmentMock.Object);

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        var response = await GetResponseBody(context);
        Assert.NotNull(response);
        // In production, details should be null for generic errors
        Assert.Null(response.Error.Details);
    }

    private HttpContext CreateHttpContext()
    {
        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();
        context.TraceIdentifier = Guid.NewGuid().ToString();
        return context;
    }

    private async Task<ErrorResponse> GetResponseBody(HttpContext context)
    {
        context.Response.Body.Seek(0, SeekOrigin.Begin);
        using var reader = new StreamReader(context.Response.Body);
        var body = await reader.ReadToEndAsync();
        return JsonSerializer.Deserialize<ErrorResponse>(body, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;
    }

    private class ErrorResponse
    {
        public ErrorDetail Error { get; set; } = null!;
    }

    private class ErrorDetail
    {
        public string Code { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public object? Details { get; set; }
        public DateTime Timestamp { get; set; }
        public string TraceId { get; set; } = string.Empty;
    }
}
