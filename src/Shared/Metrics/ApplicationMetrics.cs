using Prometheus;

namespace Shared.Metrics;

/// <summary>
/// Centralized application metrics for Prometheus
/// Validates: Requirements 3.6
/// </summary>
public static class ApplicationMetrics
{
    // API Gateway Metrics
    public static readonly Counter RequestCount = Prometheus.Metrics
        .CreateCounter(
            "api_requests_total",
            "Total number of API requests",
            new CounterConfiguration
            {
                LabelNames = new[] { "service", "method", "endpoint", "status_code" }
            });

    public static readonly Histogram RequestDuration = Prometheus.Metrics
        .CreateHistogram(
            "api_request_duration_seconds",
            "Duration of API requests in seconds",
            new HistogramConfiguration
            {
                LabelNames = new[] { "service", "method", "endpoint" },
                Buckets = Histogram.ExponentialBuckets(0.001, 2, 10) // 1ms to ~1s
            });

    public static readonly Counter ErrorCount = Prometheus.Metrics
        .CreateCounter(
            "api_errors_total",
            "Total number of API errors",
            new CounterConfiguration
            {
                LabelNames = new[] { "service", "error_type" }
            });

    // Code Execution Service Metrics
    public static readonly Gauge QueueDepth = Prometheus.Metrics
        .CreateGauge(
            "execution_queue_depth",
            "Number of jobs waiting in the execution queue");

    public static readonly Counter ExecutionCount = Prometheus.Metrics
        .CreateCounter(
            "code_executions_total",
            "Total number of code executions",
            new CounterConfiguration
            {
                LabelNames = new[] { "status" }
            });

    public static readonly Histogram ExecutionDuration = Prometheus.Metrics
        .CreateHistogram(
            "code_execution_duration_seconds",
            "Duration of code executions in seconds",
            new HistogramConfiguration
            {
                LabelNames = new[] { "status" },
                Buckets = Histogram.ExponentialBuckets(0.1, 2, 10) // 100ms to ~100s
            });

    public static readonly Counter ExecutionSuccessCount = Prometheus.Metrics
        .CreateCounter(
            "code_executions_success_total",
            "Total number of successful code executions");

    public static readonly Counter ExecutionFailureCount = Prometheus.Metrics
        .CreateCounter(
            "code_executions_failure_total",
            "Total number of failed code executions",
            new CounterConfiguration
            {
                LabelNames = new[] { "failure_type" }
            });

    // Worker Metrics
    public static readonly Gauge WorkerUtilization = Prometheus.Metrics
        .CreateGauge(
            "worker_utilization",
            "Current worker utilization (0-1)",
            new GaugeConfiguration
            {
                LabelNames = new[] { "worker_id" }
            });

    public static readonly Counter JobsProcessed = Prometheus.Metrics
        .CreateCounter(
            "worker_jobs_processed_total",
            "Total number of jobs processed by workers",
            new CounterConfiguration
            {
                LabelNames = new[] { "worker_id", "status" }
            });

    public static readonly Histogram JobProcessingDuration = Prometheus.Metrics
        .CreateHistogram(
            "worker_job_processing_duration_seconds",
            "Duration of job processing in seconds",
            new HistogramConfiguration
            {
                LabelNames = new[] { "worker_id" },
                Buckets = Histogram.ExponentialBuckets(0.1, 2, 10)
            });

    // Challenge Service Metrics
    public static readonly Counter ChallengeSubmissions = Prometheus.Metrics
        .CreateCounter(
            "challenge_submissions_total",
            "Total number of challenge submissions",
            new CounterConfiguration
            {
                LabelNames = new[] { "difficulty", "result" }
            });

    public static readonly Histogram TestCaseExecutionDuration = Prometheus.Metrics
        .CreateHistogram(
            "test_case_execution_duration_seconds",
            "Duration of test case execution in seconds",
            new HistogramConfiguration
            {
                Buckets = Histogram.ExponentialBuckets(0.01, 2, 10)
            });

    // AI Tutor Service Metrics
    public static readonly Counter AIReviewRequests = Prometheus.Metrics
        .CreateCounter(
            "ai_review_requests_total",
            "Total number of AI code review requests",
            new CounterConfiguration
            {
                LabelNames = new[] { "status" }
            });

    public static readonly Histogram AIReviewDuration = Prometheus.Metrics
        .CreateHistogram(
            "ai_review_duration_seconds",
            "Duration of AI code reviews in seconds",
            new HistogramConfiguration
            {
                Buckets = Histogram.ExponentialBuckets(0.1, 2, 10)
            });

    // Database Metrics
    public static readonly Counter DatabaseQueries = Prometheus.Metrics
        .CreateCounter(
            "database_queries_total",
            "Total number of database queries",
            new CounterConfiguration
            {
                LabelNames = new[] { "operation", "entity" }
            });

    public static readonly Histogram DatabaseQueryDuration = Prometheus.Metrics
        .CreateHistogram(
            "database_query_duration_seconds",
            "Duration of database queries in seconds",
            new HistogramConfiguration
            {
                LabelNames = new[] { "operation", "entity" },
                Buckets = Histogram.ExponentialBuckets(0.001, 2, 10)
            });

    public static readonly Counter DatabaseErrors = Prometheus.Metrics
        .CreateCounter(
            "database_errors_total",
            "Total number of database errors",
            new CounterConfiguration
            {
                LabelNames = new[] { "operation", "entity" }
            });
}
