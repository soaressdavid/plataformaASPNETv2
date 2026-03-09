using Microsoft.AspNetCore.Http;
using System.Diagnostics;

namespace Shared.Metrics;

/// <summary>
/// Middleware to track API request metrics
/// Tracks request counts, response times, and error rates
/// Validates: Requirements 3.6
/// </summary>
public class MetricsMiddleware
{
    private readonly RequestDelegate _next;
    private readonly string _serviceName;

    public MetricsMiddleware(RequestDelegate next, string serviceName)
    {
        _next = next ?? throw new ArgumentNullException(nameof(next));
        _serviceName = serviceName ?? throw new ArgumentNullException(nameof(serviceName));
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
        var endpoint = context.Request.Path.Value ?? "/";

        try
        {
            await _next(context);
            stopwatch.Stop();

            // Record successful request metrics
            var statusCode = context.Response.StatusCode.ToString();
            
            ApplicationMetrics.RequestCount
                .WithLabels(_serviceName, method, endpoint, statusCode)
                .Inc();

            ApplicationMetrics.RequestDuration
                .WithLabels(_serviceName, method, endpoint)
                .Observe(stopwatch.Elapsed.TotalSeconds);

            // Track errors (4xx and 5xx)
            if (context.Response.StatusCode >= 400)
            {
                var errorType = context.Response.StatusCode >= 500 ? "server_error" : "client_error";
                ApplicationMetrics.ErrorCount
                    .WithLabels(_serviceName, errorType)
                    .Inc();
            }
        }
        catch (Exception)
        {
            stopwatch.Stop();

            // Record error metrics
            ApplicationMetrics.RequestCount
                .WithLabels(_serviceName, method, endpoint, "500")
                .Inc();

            ApplicationMetrics.RequestDuration
                .WithLabels(_serviceName, method, endpoint)
                .Observe(stopwatch.Elapsed.TotalSeconds);

            ApplicationMetrics.ErrorCount
                .WithLabels(_serviceName, "exception")
                .Inc();

            throw;
        }
    }
}
