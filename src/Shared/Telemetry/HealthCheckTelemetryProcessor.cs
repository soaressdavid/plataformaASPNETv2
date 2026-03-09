using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;

namespace Shared.Telemetry;

/// <summary>
/// Telemetry processor that filters out health check requests to reduce noise
/// </summary>
public class HealthCheckTelemetryProcessor : ITelemetryProcessor
{
    private readonly ITelemetryProcessor _next;
    private static readonly string[] HealthCheckPaths = 
    {
        "/health",
        "/healthz",
        "/health/ready",
        "/health/live",
        "/metrics"
    };

    public HealthCheckTelemetryProcessor(ITelemetryProcessor next)
    {
        _next = next ?? throw new ArgumentNullException(nameof(next));
    }

    public void Process(ITelemetry item)
    {
        // Filter out health check requests
        if (item is RequestTelemetry request)
        {
            var url = request.Url?.AbsolutePath ?? string.Empty;
            
            if (HealthCheckPaths.Any(path => 
                url.Contains(path, StringComparison.OrdinalIgnoreCase)))
            {
                // Don't send to Application Insights
                return;
            }
        }

        // Pass through all other telemetry
        _next.Process(item);
    }
}
