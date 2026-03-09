using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Shared.Exceptions;
using Shared.Models;
using System.Net;
using System.Text.Json;

namespace Shared.Middleware;

/// <summary>
/// Global exception handling middleware
/// Catches all unhandled exceptions and returns standardized error responses
/// </summary>
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true
    };

    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
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
        var correlationId = context.Request.Headers["X-Correlation-ID"].FirstOrDefault() ?? traceId;

        _logger.LogError(exception,
            "Unhandled exception occurred. TraceId: {TraceId}, CorrelationId: {CorrelationId}, Path: {Path}",
            traceId, correlationId, context.Request.Path);

        var errorResponse = exception switch
        {
            NotFoundException notFoundEx => CreateErrorResponse(
                HttpStatusCode.NotFound,
                "Resource Not Found",
                notFoundEx.Message,
                traceId,
                notFoundEx.Metadata),

            ValidationException validationEx => CreateErrorResponse(
                HttpStatusCode.BadRequest,
                "Validation Error",
                validationEx.Message,
                traceId,
                validationEx.Metadata,
                validationEx.Errors),

            BusinessRuleException businessEx => CreateErrorResponse(
                HttpStatusCode.UnprocessableEntity,
                "Business Rule Violation",
                businessEx.Message,
                traceId,
                businessEx.Metadata),

            DomainException domainEx => CreateErrorResponse(
                HttpStatusCode.BadRequest,
                "Domain Error",
                domainEx.Message,
                traceId,
                domainEx.Metadata),

            UnauthorizedAccessException => CreateErrorResponse(
                HttpStatusCode.Unauthorized,
                "Unauthorized",
                "You are not authorized to access this resource",
                traceId),

            _ => CreateErrorResponse(
                HttpStatusCode.InternalServerError,
                "Internal Server Error",
                "An unexpected error occurred. Please try again later.",
                traceId)
        };

        context.Response.ContentType = "application/problem+json";
        context.Response.StatusCode = errorResponse.Status;

        var json = JsonSerializer.Serialize(errorResponse, JsonOptions);
        await context.Response.WriteAsync(json);
    }

    private static ErrorResponse CreateErrorResponse(
        HttpStatusCode statusCode,
        string title,
        string detail,
        string traceId,
        Dictionary<string, object>? extensions = null,
        Dictionary<string, string[]>? errors = null)
    {
        return new ErrorResponse
        {
            Type = $"https://httpstatuses.com/{(int)statusCode}",
            Title = title,
            Status = (int)statusCode,
            Detail = detail,
            TraceId = traceId,
            Timestamp = DateTime.UtcNow,
            Extensions = extensions,
            Errors = errors
        };
    }
}
