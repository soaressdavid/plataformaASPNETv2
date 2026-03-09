using System.Net;
using System.Text.Json;

namespace ApiGateway.Middleware;

/// <summary>
/// Global exception handling middleware that catches all unhandled exceptions,
/// maps them to appropriate HTTP status codes, and returns consistent error responses.
/// Validates: Requirements 16.1, 16.2, 16.3, 16.4, 16.5
/// </summary>
public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlingMiddleware> _logger;
    private readonly IHostEnvironment _environment;

    public ErrorHandlingMiddleware(
        RequestDelegate next,
        ILogger<ErrorHandlingMiddleware> logger,
        IHostEnvironment environment)
    {
        _next = next ?? throw new ArgumentNullException(nameof(next));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _environment = environment ?? throw new ArgumentNullException(nameof(environment));
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var traceId = context.TraceIdentifier;
        var timestamp = DateTime.UtcNow;

        // Log the error with full context
        _logger.LogError(
            exception,
            "Unhandled exception occurred. TraceId: {TraceId}, Path: {Path}, Method: {Method}, User: {User}",
            traceId,
            context.Request.Path,
            context.Request.Method,
            context.User?.Identity?.Name ?? "Anonymous");

        // Map exception to status code and error response
        var (statusCode, errorCode, message, details) = MapExceptionToError(exception);

        // Set response properties
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        // Create error response
        var errorResponse = new
        {
            error = new
            {
                code = errorCode,
                message = message,
                details = details,
                timestamp = timestamp,
                traceId = traceId
            }
        };

        // Serialize and write response
        var json = JsonSerializer.Serialize(errorResponse, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = _environment.IsDevelopment()
        });

        await context.Response.WriteAsync(json);
    }

    /// <summary>
    /// Maps exceptions to appropriate HTTP status codes and error details.
    /// Returns tuple of (statusCode, errorCode, message, details).
    /// </summary>
    private (HttpStatusCode statusCode, string errorCode, string message, object? details) MapExceptionToError(Exception exception)
    {
        return exception switch
        {
            // Validation errors (400 Bad Request)
            ArgumentException or ArgumentNullException => (
                HttpStatusCode.BadRequest,
                "VALIDATION_ERROR",
                "Invalid input provided",
                new { field = GetFieldFromException(exception), message = exception.Message }
            ),

            // Authentication errors (401 Unauthorized)
            UnauthorizedAccessException => (
                HttpStatusCode.Unauthorized,
                "AUTHENTICATION_ERROR",
                "Authentication failed or token is invalid",
                null
            ),

            // Not found errors (404 Not Found)
            KeyNotFoundException => (
                HttpStatusCode.NotFound,
                "RESOURCE_NOT_FOUND",
                "The requested resource was not found",
                new { message = exception.Message }
            ),

            // Timeout errors (408 Request Timeout)
            TimeoutException => (
                HttpStatusCode.RequestTimeout,
                "EXECUTION_TIMEOUT",
                "Code execution exceeded 30 second time limit",
                null
            ),

            // Compilation/execution errors (422 Unprocessable Entity)
            InvalidOperationException when exception.Message.Contains("compilation") => (
                HttpStatusCode.UnprocessableEntity,
                "COMPILATION_ERROR",
                "Code failed to compile",
                ExtractCompilationErrors(exception)
            ),

            InvalidOperationException when exception.Message.Contains("runtime") => (
                HttpStatusCode.UnprocessableEntity,
                "RUNTIME_ERROR",
                "Code execution failed",
                ExtractRuntimeError(exception)
            ),

            InvalidOperationException when exception.Message.Contains("memory") => (
                HttpStatusCode.UnprocessableEntity,
                "MEMORY_EXCEEDED",
                "Code execution exceeded 512MB memory limit",
                null
            ),

            InvalidOperationException when exception.Message.Contains("prohibited") => (
                HttpStatusCode.UnprocessableEntity,
                "PROHIBITED_CODE",
                "Code contains prohibited operations",
                ExtractProhibitedOperations(exception)
            ),

            // Network errors (503 Service Unavailable)
            HttpRequestException => (
                HttpStatusCode.ServiceUnavailable,
                "NETWORK_ERROR",
                "A network error occurred while processing your request",
                _environment.IsDevelopment() ? new { message = exception.Message } : null
            ),

            // Service unavailable (503)
            InvalidOperationException when exception.Message.Contains("unavailable") => (
                HttpStatusCode.ServiceUnavailable,
                "SERVICE_UNAVAILABLE",
                "The requested service is temporarily unavailable",
                null
            ),

            // Default to internal server error (500)
            _ => (
                HttpStatusCode.InternalServerError,
                "INTERNAL_SERVER_ERROR",
                "An unexpected error occurred while processing your request",
                _environment.IsDevelopment() ? new { message = exception.Message, stackTrace = exception.StackTrace } : null
            )
        };
    }

    /// <summary>
    /// Extracts field name from validation exceptions.
    /// </summary>
    private string GetFieldFromException(Exception exception)
    {
        if (exception is ArgumentException argEx && !string.IsNullOrEmpty(argEx.ParamName))
        {
            return argEx.ParamName;
        }

        if (exception is ArgumentNullException nullEx && !string.IsNullOrEmpty(nullEx.ParamName))
        {
            return nullEx.ParamName;
        }

        return "unknown";
    }

    /// <summary>
    /// Extracts compilation error details from exception message.
    /// Expected format: "compilation: CS0103: The name 'variableName' does not exist in the current context at line 15"
    /// </summary>
    private object ExtractCompilationErrors(Exception exception)
    {
        var message = exception.Message;
        var errors = new List<object>();

        // Try to parse line number and error message
        if (message.Contains("at line"))
        {
            var parts = message.Split("at line");
            if (parts.Length == 2 && int.TryParse(parts[1].Trim(), out var lineNumber))
            {
                errors.Add(new
                {
                    line = lineNumber,
                    message = parts[0].Replace("compilation:", "").Trim()
                });
            }
        }

        return errors.Count > 0 ? errors : new[] { new { message = exception.Message } };
    }

    /// <summary>
    /// Extracts runtime error details from exception.
    /// </summary>
    private object ExtractRuntimeError(Exception exception)
    {
        var innerException = exception.InnerException ?? exception;
        
        return new
        {
            exception = innerException.GetType().Name,
            message = innerException.Message,
            stackTrace = _environment.IsDevelopment() ? innerException.StackTrace : null
        };
    }

    /// <summary>
    /// Extracts prohibited operation details from exception message.
    /// Expected format: "prohibited: System.IO.File.ReadAllText at line 8: File system access is not allowed"
    /// </summary>
    private object ExtractProhibitedOperations(Exception exception)
    {
        var message = exception.Message;
        var operations = new List<object>();

        // Try to parse operation details
        if (message.Contains("at line"))
        {
            var parts = message.Split("at line");
            if (parts.Length == 2)
            {
                var operationPart = parts[0].Replace("prohibited:", "").Trim();
                var linePart = parts[1].Trim();
                
                var lineAndReason = linePart.Split(':');
                if (lineAndReason.Length >= 2 && int.TryParse(lineAndReason[0].Trim(), out var lineNumber))
                {
                    operations.Add(new
                    {
                        line = lineNumber,
                        operation = operationPart,
                        reason = lineAndReason[1].Trim()
                    });
                }
            }
        }

        return operations.Count > 0 ? operations : new[] { new { message = exception.Message } };
    }
}
