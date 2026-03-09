using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using System.Diagnostics;

namespace Shared.Telemetry;

/// <summary>
/// Service for tracking custom telemetry events and metrics
/// </summary>
public interface ICustomTelemetryTracker
{
    void TrackCodeExecution(string userId, string challengeId, long executionTimeMs, bool success, string? error = null);
    void TrackSqlExecution(string userId, string queryHash, long executionTimeMs, bool success, int rowCount = 0);
    void TrackCompilationError(string userId, string challengeId, string errorType, string errorMessage);
    void TrackUserActivity(string userId, string activityType, Dictionary<string, string>? properties = null);
    void TrackApiPerformance(string endpoint, string method, long durationMs, int statusCode);
    void TrackContainerOperation(string operation, long durationMs, bool success);
    void TrackDatabaseQuery(string operation, string table, long durationMs, bool success);
    void TrackCacheOperation(string operation, string cacheKey, bool hit);
    void TrackBusinessEvent(string eventName, Dictionary<string, string>? properties = null, Dictionary<string, double>? metrics = null);
    IOperationHolder<RequestTelemetry> StartOperation(string operationName);
    IOperationHolder<DependencyTelemetry> StartDependencyOperation(string dependencyName, string dependencyType);
}

/// <summary>
/// Implementation of custom telemetry tracking using Application Insights
/// </summary>
public class CustomTelemetryTracker : ICustomTelemetryTracker
{
    private readonly TelemetryClient? _telemetryClient;
    private readonly bool _isEnabled;

    public CustomTelemetryTracker(TelemetryClient? telemetryClient = null)
    {
        _telemetryClient = telemetryClient;
        _isEnabled = telemetryClient != null;
    }

    public void TrackCodeExecution(string userId, string challengeId, long executionTimeMs, bool success, string? error = null)
    {
        if (!_isEnabled) return;

        var properties = new Dictionary<string, string>
        {
            ["UserId"] = userId,
            ["ChallengeId"] = challengeId,
            ["Success"] = success.ToString(),
            ["ExecutionTimeMs"] = executionTimeMs.ToString()
        };

        if (!string.IsNullOrEmpty(error))
        {
            properties["Error"] = error;
        }

        var metrics = new Dictionary<string, double>
        {
            ["ExecutionTime"] = executionTimeMs
        };

        _telemetryClient!.TrackEvent("CodeExecution", properties, metrics);
        _telemetryClient.TrackMetric("CodeExecutionTime", executionTimeMs, properties);

        if (!success)
        {
            _telemetryClient.TrackMetric("CodeExecutionFailures", 1, properties);
        }
    }

    public void TrackSqlExecution(string userId, string queryHash, long executionTimeMs, bool success, int rowCount = 0)
    {
        if (!_isEnabled) return;

        var properties = new Dictionary<string, string>
        {
            ["UserId"] = userId,
            ["QueryHash"] = queryHash,
            ["Success"] = success.ToString(),
            ["RowCount"] = rowCount.ToString()
        };

        var metrics = new Dictionary<string, double>
        {
            ["ExecutionTime"] = executionTimeMs,
            ["RowCount"] = rowCount
        };

        _telemetryClient!.TrackEvent("SqlExecution", properties, metrics);
        _telemetryClient.TrackMetric("SqlExecutionTime", executionTimeMs, properties);
    }

    public void TrackCompilationError(string userId, string challengeId, string errorType, string errorMessage)
    {
        if (!_isEnabled) return;

        var properties = new Dictionary<string, string>
        {
            ["UserId"] = userId,
            ["ChallengeId"] = challengeId,
            ["ErrorType"] = errorType,
            ["ErrorMessage"] = errorMessage
        };

        _telemetryClient!.TrackEvent("CompilationError", properties);
        _telemetryClient.TrackMetric("CompilationErrors", 1, properties);
    }

    public void TrackUserActivity(string userId, string activityType, Dictionary<string, string>? properties = null)
    {
        if (!_isEnabled) return;

        var props = properties ?? new Dictionary<string, string>();
        props["UserId"] = userId;
        props["ActivityType"] = activityType;
        props["Timestamp"] = DateTime.UtcNow.ToString("O");

        _telemetryClient!.TrackEvent("UserActivity", props);
    }

    public void TrackApiPerformance(string endpoint, string method, long durationMs, int statusCode)
    {
        if (!_isEnabled) return;

        var properties = new Dictionary<string, string>
        {
            ["Endpoint"] = endpoint,
            ["Method"] = method,
            ["StatusCode"] = statusCode.ToString()
        };

        var metrics = new Dictionary<string, double>
        {
            ["Duration"] = durationMs
        };

        _telemetryClient!.TrackMetric("ApiResponseTime", durationMs, properties);

        // Track slow requests
        if (durationMs > 2000)
        {
            _telemetryClient.TrackEvent("SlowApiRequest", properties, metrics);
        }

        // Track errors
        if (statusCode >= 400)
        {
            var errorType = statusCode >= 500 ? "ServerError" : "ClientError";
            properties["ErrorType"] = errorType;
            _telemetryClient.TrackEvent("ApiError", properties);
        }
    }

    public void TrackContainerOperation(string operation, long durationMs, bool success)
    {
        if (!_isEnabled) return;

        var properties = new Dictionary<string, string>
        {
            ["Operation"] = operation,
            ["Success"] = success.ToString()
        };

        var metrics = new Dictionary<string, double>
        {
            ["Duration"] = durationMs
        };

        _telemetryClient!.TrackEvent("ContainerOperation", properties, metrics);
        _telemetryClient.TrackMetric("ContainerOperationTime", durationMs, properties);

        if (!success)
        {
            _telemetryClient.TrackMetric("ContainerOperationFailures", 1, properties);
        }
    }

    public void TrackDatabaseQuery(string operation, string table, long durationMs, bool success)
    {
        if (!_isEnabled) return;

        var properties = new Dictionary<string, string>
        {
            ["Operation"] = operation,
            ["Table"] = table,
            ["Success"] = success.ToString()
        };

        var metrics = new Dictionary<string, double>
        {
            ["Duration"] = durationMs
        };

        _telemetryClient!.TrackMetric("DatabaseQueryTime", durationMs, properties);

        if (!success)
        {
            _telemetryClient.TrackEvent("DatabaseQueryError", properties);
        }
    }

    public void TrackCacheOperation(string operation, string cacheKey, bool hit)
    {
        if (!_isEnabled) return;

        var properties = new Dictionary<string, string>
        {
            ["Operation"] = operation,
            ["CacheKey"] = cacheKey,
            ["Hit"] = hit.ToString()
        };

        _telemetryClient!.TrackEvent("CacheOperation", properties);
        _telemetryClient.TrackMetric(hit ? "CacheHits" : "CacheMisses", 1, properties);
    }

    public void TrackBusinessEvent(string eventName, Dictionary<string, string>? properties = null, Dictionary<string, double>? metrics = null)
    {
        if (!_isEnabled) return;

        _telemetryClient!.TrackEvent(eventName, properties, metrics);
    }

    public IOperationHolder<RequestTelemetry> StartOperation(string operationName)
    {
        if (!_isEnabled)
        {
            return new NoOpOperationHolder<RequestTelemetry>();
        }

        return _telemetryClient!.StartOperation<RequestTelemetry>(operationName);
    }

    public IOperationHolder<DependencyTelemetry> StartDependencyOperation(string dependencyName, string dependencyType)
    {
        if (!_isEnabled)
        {
            return new NoOpOperationHolder<DependencyTelemetry>();
        }

        var dependency = new DependencyTelemetry
        {
            Name = dependencyName,
            Type = dependencyType,
            Timestamp = DateTimeOffset.UtcNow
        };

        return _telemetryClient!.StartOperation(dependency);
    }
}

/// <summary>
/// No-op operation holder for when Application Insights is not configured
/// </summary>
internal class NoOpOperationHolder<T> : IOperationHolder<T> where T : class, new()
{
    public T Telemetry { get; } = new T();

    public void Dispose()
    {
        // No-op
    }
}
