using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;

namespace Shared.Telemetry;

/// <summary>
/// Telemetry initializer that adds service name to all telemetry items
/// </summary>
public class ServiceNameTelemetryInitializer : ITelemetryInitializer
{
    private readonly string _serviceName;

    public ServiceNameTelemetryInitializer(string serviceName)
    {
        _serviceName = serviceName ?? throw new ArgumentNullException(nameof(serviceName));
    }

    public void Initialize(ITelemetry telemetry)
    {
        if (telemetry == null)
        {
            return;
        }

        // Add service name as a custom property
        if (!telemetry.Context.GlobalProperties.ContainsKey("ServiceName"))
        {
            telemetry.Context.GlobalProperties.Add("ServiceName", _serviceName);
        }

        // Set cloud role name for Application Map
        if (string.IsNullOrEmpty(telemetry.Context.Cloud.RoleName))
        {
            telemetry.Context.Cloud.RoleName = _serviceName;
        }
    }
}
