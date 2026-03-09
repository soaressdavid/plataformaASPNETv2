using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Http;

namespace Shared.Telemetry;

/// <summary>
/// Telemetry initializer that adds correlation ID from HTTP context to all telemetry
/// </summary>
public class CorrelationTelemetryInitializer : ITelemetryInitializer
{
    private readonly IHttpContextAccessor? _httpContextAccessor;
    private const string CorrelationIdHeader = "X-Correlation-ID";

    public CorrelationTelemetryInitializer(IHttpContextAccessor? httpContextAccessor = null)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public void Initialize(ITelemetry telemetry)
    {
        if (telemetry == null)
        {
            return;
        }

        var httpContext = _httpContextAccessor?.HttpContext;
        if (httpContext == null)
        {
            return;
        }

        // Get correlation ID from request headers
        if (httpContext.Request.Headers.TryGetValue(CorrelationIdHeader, out var correlationId) &&
            !string.IsNullOrWhiteSpace(correlationId))
        {
            // Add as custom property
            if (!telemetry.Context.GlobalProperties.ContainsKey("CorrelationId"))
            {
                telemetry.Context.GlobalProperties.Add("CorrelationId", correlationId.ToString());
            }

            // For request telemetry, also set as custom dimension
            if (telemetry is RequestTelemetry requestTelemetry)
            {
                if (!requestTelemetry.Properties.ContainsKey("CorrelationId"))
                {
                    requestTelemetry.Properties.Add("CorrelationId", correlationId.ToString());
                }
            }

            // For dependency telemetry
            if (telemetry is DependencyTelemetry dependencyTelemetry)
            {
                if (!dependencyTelemetry.Properties.ContainsKey("CorrelationId"))
                {
                    dependencyTelemetry.Properties.Add("CorrelationId", correlationId.ToString());
                }
            }
        }

        // Add user ID if authenticated
        if (httpContext.User?.Identity?.IsAuthenticated == true)
        {
            var userId = httpContext.User.FindFirst("sub")?.Value 
                      ?? httpContext.User.FindFirst("userId")?.Value;
            
            if (!string.IsNullOrEmpty(userId))
            {
                telemetry.Context.User.Id = userId;
                
                if (!telemetry.Context.GlobalProperties.ContainsKey("UserId"))
                {
                    telemetry.Context.GlobalProperties.Add("UserId", userId);
                }
            }
        }
    }
}
