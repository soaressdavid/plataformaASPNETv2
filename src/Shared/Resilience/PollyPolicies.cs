using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;
using Polly.CircuitBreaker;
using Polly.Timeout;

namespace Shared.Resilience;

/// <summary>
/// Centralized Polly resilience policies for enterprise-grade reliability
/// </summary>
public static class PollyPolicies
{
    /// <summary>
    /// Creates a retry policy with exponential backoff for HTTP requests
    /// </summary>
    public static AsyncRetryPolicy<HttpResponseMessage> GetHttpRetryPolicy(ILogger logger)
    {
        return Policy
            .HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
            .Or<HttpRequestException>()
            .Or<TimeoutException>()
            .WaitAndRetryAsync(
                retryCount: 3,
                sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                onRetry: (outcome, timespan, retryCount, context) =>
                {
                    logger.LogWarning(
                        "Retry {RetryCount} after {Delay}s due to {Reason}",
                        retryCount,
                        timespan.TotalSeconds,
                        outcome.Exception?.Message ?? outcome.Result?.StatusCode.ToString() ?? "Unknown"
                    );
                });
    }

    /// <summary>
    /// Creates a circuit breaker policy for HTTP requests
    /// </summary>
    public static AsyncCircuitBreakerPolicy<HttpResponseMessage> GetHttpCircuitBreakerPolicy(ILogger logger)
    {
        return Policy
            .HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
            .Or<HttpRequestException>()
            .Or<TimeoutException>()
            .CircuitBreakerAsync(
                handledEventsAllowedBeforeBreaking: 5,
                durationOfBreak: TimeSpan.FromSeconds(30),
                onBreak: (outcome, duration) =>
                {
                    logger.LogError(
                        "Circuit breaker opened for {Duration}s after {FailureCount} failures",
                        duration.TotalSeconds,
                        5
                    );
                },
                onReset: () =>
                {
                    logger.LogInformation("Circuit breaker reset - service is healthy again");
                },
                onHalfOpen: () =>
                {
                    logger.LogInformation("Circuit breaker half-open - testing if service is healthy");
                });
    }

    /// <summary>
    /// Creates a timeout policy for HTTP requests
    /// </summary>
    public static AsyncTimeoutPolicy<HttpResponseMessage> GetHttpTimeoutPolicy(int timeoutSeconds = 30)
    {
        return Policy.TimeoutAsync<HttpResponseMessage>(TimeSpan.FromSeconds(timeoutSeconds));
    }

    /// <summary>
    /// Creates a combined policy with retry, circuit breaker, and timeout
    /// </summary>
    public static IAsyncPolicy<HttpResponseMessage> GetCombinedHttpPolicy(ILogger logger, int timeoutSeconds = 30)
    {
        var retry = GetHttpRetryPolicy(logger);
        var circuitBreaker = GetHttpCircuitBreakerPolicy(logger);
        var timeout = GetHttpTimeoutPolicy(timeoutSeconds);

        // Order: Timeout -> Retry -> Circuit Breaker
        return Policy.WrapAsync(timeout, retry, circuitBreaker);
    }

    /// <summary>
    /// Creates a retry policy for database operations
    /// </summary>
    public static AsyncRetryPolicy GetDatabaseRetryPolicy(ILogger logger)
    {
        return Policy
            .Handle<Exception>(ex => IsTransientDatabaseError(ex))
            .WaitAndRetryAsync(
                retryCount: 3,
                sleepDurationProvider: retryAttempt => TimeSpan.FromMilliseconds(100 * Math.Pow(2, retryAttempt)),
                onRetry: (exception, timespan, retryCount, context) =>
                {
                    logger.LogWarning(
                        exception,
                        "Database retry {RetryCount} after {Delay}ms",
                        retryCount,
                        timespan.TotalMilliseconds
                    );
                });
    }

    /// <summary>
    /// Creates a timeout policy for database operations
    /// </summary>
    public static AsyncTimeoutPolicy GetDatabaseTimeoutPolicy(int timeoutSeconds = 30)
    {
        return Policy.TimeoutAsync(TimeSpan.FromSeconds(timeoutSeconds));
    }

    /// <summary>
    /// Creates a combined policy for database operations
    /// </summary>
    public static IAsyncPolicy GetCombinedDatabasePolicy(ILogger logger, int timeoutSeconds = 30)
    {
        var retry = GetDatabaseRetryPolicy(logger);
        var timeout = GetDatabaseTimeoutPolicy(timeoutSeconds);

        return Policy.WrapAsync(timeout, retry);
    }

    /// <summary>
    /// Determines if an exception is a transient database error
    /// </summary>
    private static bool IsTransientDatabaseError(Exception ex)
    {
        // Common transient SQL errors
        var transientErrorNumbers = new[]
        {
            -2,     // Timeout
            -1,     // Connection broken
            2,      // Network error
            53,     // Connection failed
            64,     // Server not found
            233,    // Connection initialization error
            10053,  // Transport-level error
            10054,  // Connection forcibly closed
            10060,  // Network timeout
            40197,  // Service error
            40501,  // Service busy
            40613,  // Database unavailable
            49918,  // Cannot process request
            49919,  // Cannot process create/update
            49920   // Cannot process delete
        };

        var message = ex.Message;
        return transientErrorNumbers.Any(num => message.Contains($"error {num}")) ||
               message.Contains("timeout", StringComparison.OrdinalIgnoreCase) ||
               message.Contains("deadlock", StringComparison.OrdinalIgnoreCase) ||
               message.Contains("connection", StringComparison.OrdinalIgnoreCase);
    }
}
