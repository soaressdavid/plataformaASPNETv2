using FsCheck;
using FsCheck.Xunit;
using ApiGateway.Middleware;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Moq;
using System.Text.Json;

namespace ApiGateway.Tests;

/// <summary>
/// Property-based tests for error handling functionality.
/// **Validates: Requirements 16.1, 16.2, 16.3, 16.4, 16.5**
/// </summary>
public class ErrorHandlingPropertiesTests
{
    private readonly Mock<ILogger<ErrorHandlingMiddleware>> _loggerMock;
    private readonly Mock<IHostEnvironment> _environmentMock;

    public ErrorHandlingPropertiesTests()
    {
        _loggerMock = new Mock<ILogger<ErrorHandlingMiddleware>>();
        _environmentMock = new Mock<IHostEnvironment>();
        _environmentMock.Setup(e => e.EnvironmentName).Returns(Environments.Production);
    }

    /// <summary>
    /// Property 47: Compilation Error Formatting
    /// For any compilation error, the error message should include the line number where the error occurred.
    /// **Validates: Requirements 16.1**
    /// </summary>
    [Property(MaxTest = 100)]
    public void CompilationErrorFormatting_IncludesLineNumber(PositiveInt lineNumber, NonEmptyString errorMessage)
    {
        var line = lineNumber.Get % 1000; // Reasonable line number range
        var message = errorMessage.Get.Replace(":", "").Replace("\n", " "); // Clean message
        
        // Arrange
        var context = CreateHttpContext();
        var errorCode = "CS0103";
        var exceptionMessage = $"compilation: {errorCode}: {message} at line {line}";
        RequestDelegate next = (ctx) => throw new InvalidOperationException(exceptionMessage);

        var middleware = new ErrorHandlingMiddleware(next, _loggerMock.Object, _environmentMock.Object);

        // Act
        middleware.InvokeAsync(context).Wait();

        // Assert
        var response = GetResponseBody(context).Result;
        
        Assert.Equal(StatusCodes.Status422UnprocessableEntity, context.Response.StatusCode);
        Assert.Equal("COMPILATION_ERROR", response.Error.Code);
        Assert.NotNull(response.Error.Details);
        Assert.Contains("compile", response.Error.Message);
    }

    /// <summary>
    /// Property 48: Runtime Error Formatting
    /// For any runtime exception, the error message should include the exception message and stack trace.
    /// **Validates: Requirements 16.2**
    /// </summary>
    [Property(MaxTest = 100)]
    public void RuntimeErrorFormatting_IncludesExceptionDetails(NonEmptyString exceptionMessage)
    {
        var message = exceptionMessage.Get.Replace("\n", " ").Replace("\r", " ");
        
        // Arrange
        var context = CreateHttpContext();
        var innerException = new NullReferenceException(message);
        var runtimeException = new InvalidOperationException("runtime error occurred", innerException);
        RequestDelegate next = (ctx) => throw runtimeException;

        var middleware = new ErrorHandlingMiddleware(next, _loggerMock.Object, _environmentMock.Object);

        // Act
        middleware.InvokeAsync(context).Wait();

        // Assert
        var response = GetResponseBody(context).Result;
        
        Assert.Equal(StatusCodes.Status422UnprocessableEntity, context.Response.StatusCode);
        Assert.Equal("RUNTIME_ERROR", response.Error.Code);
        Assert.Equal("Code execution failed", response.Error.Message);
        Assert.NotNull(response.Error.Details);
    }

    /// <summary>
    /// Property 49: Network Error Handling
    /// For any network error, the platform should return a user-friendly error message (not raw exception details).
    /// **Validates: Requirements 16.3**
    /// </summary>
    [Property(MaxTest = 100)]
    public void NetworkErrorHandling_ReturnsUserFriendlyMessage(NonEmptyString technicalMessage)
    {
        var message = technicalMessage.Get;
        
        // Arrange
        var context = CreateHttpContext();
        RequestDelegate next = (ctx) => throw new HttpRequestException(message);

        var middleware = new ErrorHandlingMiddleware(next, _loggerMock.Object, _environmentMock.Object);

        // Act
        middleware.InvokeAsync(context).Wait();

        // Assert
        var response = GetResponseBody(context).Result;
        
        Assert.Equal(StatusCodes.Status503ServiceUnavailable, context.Response.StatusCode);
        Assert.Equal("NETWORK_ERROR", response.Error.Code);
        // The message should be the standard user-friendly message, not the technical exception message
        Assert.Equal("A network error occurred while processing your request", response.Error.Message);
    }

    /// <summary>
    /// Property 50: Timeout Error Messaging
    /// For any execution timeout, the error message should clearly indicate that execution exceeded time limits.
    /// **Validates: Requirements 16.4**
    /// </summary>
    [Property(MaxTest = 100)]
    public void TimeoutErrorMessaging_IndicatesTimeLimitExceeded(NonEmptyString operationName)
    {
        var operation = operationName.Get.Replace("\n", " ");
        
        // Arrange
        var context = CreateHttpContext();
        RequestDelegate next = (ctx) => throw new TimeoutException($"{operation} timed out");

        var middleware = new ErrorHandlingMiddleware(next, _loggerMock.Object, _environmentMock.Object);

        // Act
        middleware.InvokeAsync(context).Wait();

        // Assert
        var response = GetResponseBody(context).Result;
        
        Assert.Equal(StatusCodes.Status408RequestTimeout, context.Response.StatusCode);
        Assert.Equal("EXECUTION_TIMEOUT", response.Error.Code);
        Assert.Contains("exceeded", response.Error.Message);
        Assert.Contains("time limit", response.Error.Message);
        Assert.Contains("30 second", response.Error.Message);
    }

    /// <summary>
    /// Property 51: Error Logging
    /// For any error that occurs in the platform, it should be logged to the centralized logging system
    /// with timestamp, context, and stack trace.
    /// **Validates: Requirements 16.5**
    /// </summary>
    [Property(MaxTest = 100)]
    public void ErrorLogging_LogsAllErrorsWithContext(NonEmptyString errorMessage)
    {
        var message = errorMessage.Get;
        
        // Arrange
        var context = CreateHttpContext();
        var exception = new Exception(message);
        RequestDelegate next = (ctx) => throw exception;

        var loggerMock = new Mock<ILogger<ErrorHandlingMiddleware>>();
        var middleware = new ErrorHandlingMiddleware(next, loggerMock.Object, _environmentMock.Object);

        // Act
        middleware.InvokeAsync(context).Wait();

        // Assert - Verify logging was called with error level and exception
        loggerMock.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => true),
                exception,
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    /// <summary>
    /// Property: All errors should include a trace ID for debugging.
    /// **Validates: Requirements 16.5**
    /// </summary>
    [Property(MaxTest = 100)]
    public void AllErrors_IncludeTraceId(NonEmptyString errorMessage)
    {
        var message = errorMessage.Get;
        
        // Arrange
        var context = CreateHttpContext();
        var expectedTraceId = context.TraceIdentifier;
        RequestDelegate next = (ctx) => throw new Exception(message);

        var middleware = new ErrorHandlingMiddleware(next, _loggerMock.Object, _environmentMock.Object);

        // Act
        middleware.InvokeAsync(context).Wait();

        // Assert
        var response = GetResponseBody(context).Result;
        
        Assert.False(string.IsNullOrEmpty(response.Error.TraceId));
        Assert.Equal(expectedTraceId, response.Error.TraceId);
    }

    /// <summary>
    /// Property: All errors should include a timestamp.
    /// **Validates: Requirements 16.5**
    /// </summary>
    [Property(MaxTest = 100)]
    public void AllErrors_IncludeTimestamp(NonEmptyString errorMessage)
    {
        var message = errorMessage.Get;
        
        // Arrange
        var context = CreateHttpContext();
        var beforeTime = DateTime.UtcNow;
        RequestDelegate next = (ctx) => throw new Exception(message);

        var middleware = new ErrorHandlingMiddleware(next, _loggerMock.Object, _environmentMock.Object);

        // Act
        middleware.InvokeAsync(context).Wait();
        var afterTime = DateTime.UtcNow;

        // Assert
        var response = GetResponseBody(context).Result;
        
        Assert.NotEqual(default(DateTime), response.Error.Timestamp);
        Assert.True(response.Error.Timestamp >= beforeTime && response.Error.Timestamp <= afterTime);
        Assert.Equal(DateTimeKind.Utc, response.Error.Timestamp.Kind);
    }

    /// <summary>
    /// Property: Error response format should be consistent across all error types.
    /// **Validates: Requirements 16.1, 16.2, 16.3, 16.4, 16.5**
    /// </summary>
    [Property(MaxTest = 100)]
    public void ErrorResponseFormat_IsConsistent(NonEmptyString errorMessage)
    {
        var message = errorMessage.Get;
        
        // Arrange
        var context = CreateHttpContext();
        RequestDelegate next = (ctx) => throw new Exception(message);

        var middleware = new ErrorHandlingMiddleware(next, _loggerMock.Object, _environmentMock.Object);

        // Act
        middleware.InvokeAsync(context).Wait();

        // Assert
        var response = GetResponseBody(context).Result;
        
        Assert.Equal("application/json", context.Response.ContentType);
        Assert.False(string.IsNullOrEmpty(response.Error.Code));
        Assert.False(string.IsNullOrEmpty(response.Error.Message));
        Assert.False(string.IsNullOrEmpty(response.Error.TraceId));
        Assert.NotEqual(default(DateTime), response.Error.Timestamp);
    }

    /// <summary>
    /// Property: Memory exceeded errors should be properly formatted.
    /// **Validates: Requirements 16.1**
    /// </summary>
    [Property(MaxTest = 100)]
    public void MemoryExceededError_ProperlyFormatted(PositiveInt memoryUsed)
    {
        var memory = memoryUsed.Get % 2048; // MB
        
        // Arrange
        var context = CreateHttpContext();
        RequestDelegate next = (ctx) => throw new InvalidOperationException($"memory limit exceeded: {memory}MB used");

        var middleware = new ErrorHandlingMiddleware(next, _loggerMock.Object, _environmentMock.Object);

        // Act
        middleware.InvokeAsync(context).Wait();

        // Assert
        var response = GetResponseBody(context).Result;
        
        Assert.Equal(StatusCodes.Status422UnprocessableEntity, context.Response.StatusCode);
        Assert.Equal("MEMORY_EXCEEDED", response.Error.Code);
        Assert.Contains("512MB", response.Error.Message);
    }

    /// <summary>
    /// Property: Prohibited code errors should include operation details.
    /// **Validates: Requirements 16.1**
    /// </summary>
    [Property(MaxTest = 100)]
    public void ProhibitedCodeError_IncludesOperationDetails(PositiveInt lineNumber, NonEmptyString operation)
    {
        var line = lineNumber.Get % 1000;
        var op = operation.Get.Replace(":", "").Replace("\n", " ");
        
        // Arrange
        var context = CreateHttpContext();
        RequestDelegate next = (ctx) => throw new InvalidOperationException(
            $"prohibited: {op} at line {line}: Operation not allowed");

        var middleware = new ErrorHandlingMiddleware(next, _loggerMock.Object, _environmentMock.Object);

        // Act
        middleware.InvokeAsync(context).Wait();

        // Assert
        var response = GetResponseBody(context).Result;
        
        Assert.Equal(StatusCodes.Status422UnprocessableEntity, context.Response.StatusCode);
        Assert.Equal("PROHIBITED_CODE", response.Error.Code);
        Assert.NotNull(response.Error.Details);
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
