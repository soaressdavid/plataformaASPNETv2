using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Prometheus;

namespace Shared.Metrics;

/// <summary>
/// Extension methods for configuring Prometheus metrics
/// </summary>
public static class MetricsExtensions
{
    /// <summary>
    /// Adds Prometheus metrics collection to the application
    /// </summary>
    public static IServiceCollection AddPrometheusMetrics(this IServiceCollection services)
    {
        // Metrics are automatically collected via static registry
        // No additional service registration needed
        return services;
    }

    /// <summary>
    /// Configures Prometheus metrics endpoints and middleware
    /// </summary>
    public static IApplicationBuilder UsePrometheusMetrics(this IApplicationBuilder app)
    {
        // Add HTTP metrics middleware
        app.UseHttpMetrics(options =>
        {
            // Customize which routes to track
            options.ReduceStatusCodeCardinality();
            
            // Add custom labels
            options.AddCustomLabel("service", context =>
            {
                return context.Request.Host.Host;
            });
        });

        // Expose /metrics endpoint for Prometheus scraping
        app.UseMetricServer();

        return app;
    }

    /// <summary>
    /// Records HTTP request metrics
    /// </summary>
    public static void RecordHttpRequest(string method, string endpoint, int statusCode, double durationSeconds)
    {
        MetricsRegistry.HttpRequestsTotal
            .WithLabels(method, endpoint, statusCode.ToString())
            .Inc();

        MetricsRegistry.HttpRequestDuration
            .WithLabels(method, endpoint)
            .Observe(durationSeconds);

        if (statusCode >= 400)
        {
            var errorType = statusCode >= 500 ? "server_error" : "client_error";
            MetricsRegistry.HttpRequestErrors
                .WithLabels(method, endpoint, errorType)
                .Inc();
        }
    }

    /// <summary>
    /// Records database query metrics
    /// </summary>
    public static void RecordDatabaseQuery(string operation, string table, double durationSeconds, bool success = true)
    {
        MetricsRegistry.DatabaseQueriesTotal
            .WithLabels(operation, table)
            .Inc();

        MetricsRegistry.DatabaseQueryDuration
            .WithLabels(operation, table)
            .Observe(durationSeconds);

        if (!success)
        {
            MetricsRegistry.DatabaseErrors
                .WithLabels(operation, "query_failed")
                .Inc();
        }
    }

    /// <summary>
    /// Records cache hit or miss
    /// </summary>
    public static void RecordCacheAccess(string cacheName, bool hit)
    {
        if (hit)
        {
            MetricsRegistry.CacheHits.WithLabels(cacheName).Inc();
        }
        else
        {
            MetricsRegistry.CacheMisses.WithLabels(cacheName).Inc();
        }
    }

    /// <summary>
    /// Records retry attempt
    /// </summary>
    public static void RecordRetryAttempt(string operation, int attemptNumber)
    {
        MetricsRegistry.RetryAttempts
            .WithLabels(operation, attemptNumber.ToString())
            .Inc();
    }

    /// <summary>
    /// Records circuit breaker state change
    /// </summary>
    public static void RecordCircuitBreakerState(string operation, CircuitBreakerState state)
    {
        var stateValue = state switch
        {
            CircuitBreakerState.Closed => 0,
            CircuitBreakerState.Open => 1,
            CircuitBreakerState.HalfOpen => 2,
            _ => -1
        };

        MetricsRegistry.CircuitBreakerState
            .WithLabels(operation)
            .Set(stateValue);

        if (state == CircuitBreakerState.Open)
        {
            MetricsRegistry.CircuitBreakerOpened
                .WithLabels(operation)
                .Inc();
        }
    }

    /// <summary>
    /// Records validation error
    /// </summary>
    public static void RecordValidationError(string validator, string field)
    {
        MetricsRegistry.ValidationErrors
            .WithLabels(validator, field)
            .Inc();
    }

    /// <summary>
    /// Records business event
    /// </summary>
    public static void RecordCourseCreated()
    {
        MetricsRegistry.CoursesCreated.Inc();
    }

    public static void RecordLessonCreated()
    {
        MetricsRegistry.LessonsCreated.Inc();
    }

    public static void RecordUserRegistered()
    {
        MetricsRegistry.UsersRegistered.Inc();
    }

    public static void RecordLessonCompleted(int level)
    {
        MetricsRegistry.LessonsCompleted
            .WithLabels(level.ToString())
            .Inc();
    }

    public static void SetActiveUsers(int count)
    {
        MetricsRegistry.ActiveUsers.Set(count);
    }

    public static void SetActiveSessions(int count)
    {
        MetricsRegistry.ActiveSessions.Set(count);
    }
}

/// <summary>
/// Circuit breaker state enum
/// </summary>
public enum CircuitBreakerState
{
    Closed,
    Open,
    HalfOpen
}
