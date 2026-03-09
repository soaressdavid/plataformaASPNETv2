using Prometheus;

namespace Shared.Metrics;

/// <summary>
/// Central registry for all application metrics
/// </summary>
public static class MetricsRegistry
{
    // HTTP Metrics
    public static readonly Counter HttpRequestsTotal = Prometheus.Metrics.CreateCounter(
        "http_requests_total",
        "Total number of HTTP requests",
        new CounterConfiguration
        {
            LabelNames = new[] { "method", "endpoint", "status_code" }
        });

    public static readonly Histogram HttpRequestDuration = Prometheus.Metrics.CreateHistogram(
        "http_request_duration_seconds",
        "Duration of HTTP requests in seconds",
        new HistogramConfiguration
        {
            LabelNames = new[] { "method", "endpoint" },
            Buckets = Histogram.ExponentialBuckets(0.001, 2, 10) // 1ms to ~1s
        });

    public static readonly Counter HttpRequestErrors = Prometheus.Metrics.CreateCounter(
        "http_request_errors_total",
        "Total number of HTTP request errors",
        new CounterConfiguration
        {
            LabelNames = new[] { "method", "endpoint", "error_type" }
        });

    // Database Metrics
    public static readonly Counter DatabaseQueriesTotal = Prometheus.Metrics.CreateCounter(
        "database_queries_total",
        "Total number of database queries",
        new CounterConfiguration
        {
            LabelNames = new[] { "operation", "table" }
        });

    public static readonly Histogram DatabaseQueryDuration = Prometheus.Metrics.CreateHistogram(
        "database_query_duration_seconds",
        "Duration of database queries in seconds",
        new HistogramConfiguration
        {
            LabelNames = new[] { "operation", "table" },
            Buckets = Histogram.ExponentialBuckets(0.001, 2, 10)
        });

    public static readonly Counter DatabaseErrors = Prometheus.Metrics.CreateCounter(
        "database_errors_total",
        "Total number of database errors",
        new CounterConfiguration
        {
            LabelNames = new[] { "operation", "error_type" }
        });

    // Business Metrics
    public static readonly Counter CoursesCreated = Prometheus.Metrics.CreateCounter(
        "courses_created_total",
        "Total number of courses created");

    public static readonly Counter LessonsCreated = Prometheus.Metrics.CreateCounter(
        "lessons_created_total",
        "Total number of lessons created");

    public static readonly Counter UsersRegistered = Prometheus.Metrics.CreateCounter(
        "users_registered_total",
        "Total number of users registered");

    public static readonly Counter LessonsCompleted = Prometheus.Metrics.CreateCounter(
        "lessons_completed_total",
        "Total number of lessons completed",
        new CounterConfiguration
        {
            LabelNames = new[] { "level" }
        });

    public static readonly Gauge ActiveUsers = Prometheus.Metrics.CreateGauge(
        "active_users",
        "Number of currently active users");

    public static readonly Gauge ActiveSessions = Prometheus.Metrics.CreateGauge(
        "active_sessions",
        "Number of currently active sessions");

    // Cache Metrics
    public static readonly Counter CacheHits = Prometheus.Metrics.CreateCounter(
        "cache_hits_total",
        "Total number of cache hits",
        new CounterConfiguration
        {
            LabelNames = new[] { "cache_name" }
        });

    public static readonly Counter CacheMisses = Prometheus.Metrics.CreateCounter(
        "cache_misses_total",
        "Total number of cache misses",
        new CounterConfiguration
        {
            LabelNames = new[] { "cache_name" }
        });

    // Resilience Metrics
    public static readonly Counter RetryAttempts = Prometheus.Metrics.CreateCounter(
        "retry_attempts_total",
        "Total number of retry attempts",
        new CounterConfiguration
        {
            LabelNames = new[] { "operation", "attempt" }
        });

    public static readonly Counter CircuitBreakerOpened = Prometheus.Metrics.CreateCounter(
        "circuit_breaker_opened_total",
        "Total number of times circuit breaker opened",
        new CounterConfiguration
        {
            LabelNames = new[] { "operation" }
        });

    public static readonly Gauge CircuitBreakerState = Prometheus.Metrics.CreateGauge(
        "circuit_breaker_state",
        "Current state of circuit breaker (0=Closed, 1=Open, 2=HalfOpen)",
        new GaugeConfiguration
        {
            LabelNames = new[] { "operation" }
        });

    // Validation Metrics
    public static readonly Counter ValidationErrors = Prometheus.Metrics.CreateCounter(
        "validation_errors_total",
        "Total number of validation errors",
        new CounterConfiguration
        {
            LabelNames = new[] { "validator", "field" }
        });
}
