using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Shared.Metrics;
using System.Diagnostics;

namespace Shared.Middleware;

/// <summary>
/// Middleware to automatically capture HTTP request metrics
/// </summary>
public class MetricsMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<MetricsMiddleware> _logger;

    public MetricsMiddleware(RequestDelegate next, ILogger<MetricsMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Skip metrics endpoint itself
        if (context.Request.Path.StartsWithSegments("/metrics"))
        {
            await _next(context);
            return;
        }

        var stopwatch = Stopwatch.StartNew();
        var method = context.Request.Method;
        var path = context.Request.Path.Value ?? "/";

        try
        {
            await _next(context);
        }
        finally
        {
            stopwatch.Stop();
            var duration = stopwatch.Elapsed.TotalSeconds;
            var statusCode = context.Response.StatusCode;

            // Record metrics
            MetricsExtensions.RecordHttpRequest(method, path, statusCode, duration);

            // Log slow requests
            if (duration > 1.0) // More than 1 second
            {
                _logger.LogWarning(
                    "Slow request detected: {Method} {Path} took {Duration}ms (Status: {StatusCode})",
                    method, path, stopwatch.ElapsedMilliseconds, statusCode);
            }
        }
    }
}
