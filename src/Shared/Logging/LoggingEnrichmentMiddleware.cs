using Microsoft.AspNetCore.Http;
using Serilog.Context;

namespace Shared.Logging;

/// <summary>
/// Middleware that enriches logs with user context, request ID, and trace ID
/// </summary>
public class LoggingEnrichmentMiddleware
{
    private readonly RequestDelegate _next;

    public LoggingEnrichmentMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Generate or extract request ID
        var requestId = context.Request.Headers["X-Request-Id"].FirstOrDefault() 
                        ?? Guid.NewGuid().ToString();
        context.Items["RequestId"] = requestId;

        // Extract or generate trace ID for distributed tracing
        var traceId = context.Request.Headers["X-Trace-Id"].FirstOrDefault() 
                      ?? Guid.NewGuid().ToString();
        context.Items["TraceId"] = traceId;

        // Add response headers for tracking
        context.Response.Headers["X-Request-Id"] = requestId;
        context.Response.Headers["X-Trace-Id"] = traceId;

        // Push properties to Serilog's LogContext
        using (LogContext.PushProperty("RequestId", requestId))
        using (LogContext.PushProperty("TraceId", traceId))
        using (LogContext.PushProperty("RequestPath", context.Request.Path))
        using (LogContext.PushProperty("RequestMethod", context.Request.Method))
        {
            // Extract user ID if available (set by authentication middleware)
            if (context.Items.TryGetValue("UserId", out var userId))
            {
                using (LogContext.PushProperty("UserId", userId))
                {
                    await _next(context);
                }
            }
            else
            {
                await _next(context);
            }
        }
    }
}
