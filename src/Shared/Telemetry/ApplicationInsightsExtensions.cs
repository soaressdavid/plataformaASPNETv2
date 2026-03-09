using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Shared.Telemetry;

/// <summary>
/// Extension methods for configuring Application Insights telemetry
/// </summary>
public static class ApplicationInsightsExtensions
{
    /// <summary>
    /// Adds Application Insights telemetry to the service collection
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <param name="configuration">The configuration</param>
    /// <param name="serviceName">The name of the service for telemetry</param>
    /// <returns>The service collection for chaining</returns>
    public static IServiceCollection AddApplicationInsightsTelemetry(
        this IServiceCollection services,
        IConfiguration configuration,
        string serviceName)
    {
        var connectionString = configuration["ApplicationInsights:ConnectionString"];
        
        if (string.IsNullOrEmpty(connectionString))
        {
            // If no connection string, use instrumentation key (legacy)
            var instrumentationKey = configuration["ApplicationInsights:InstrumentationKey"];
            
            if (string.IsNullOrEmpty(instrumentationKey))
            {
                // No Application Insights configured - skip
                return services;
            }
        }

        // Add Application Insights telemetry
        services.AddApplicationInsightsTelemetry(options =>
        {
            if (!string.IsNullOrEmpty(connectionString))
            {
                options.ConnectionString = connectionString;
            }
            
            options.EnableAdaptiveSampling = true;
            options.EnableQuickPulseMetricStream = true;
            options.EnablePerformanceCounterCollectionModule = true;
            options.EnableDependencyTrackingTelemetryModule = true;
            options.EnableEventCounterCollectionModule = true;
        });

        // Add custom telemetry initializer for service name and correlation
        services.AddSingleton<ITelemetryInitializer>(sp => 
            new ServiceNameTelemetryInitializer(serviceName));
        
        services.AddSingleton<ITelemetryInitializer, CorrelationTelemetryInitializer>();

        // Add custom telemetry processor for filtering
        services.AddSingleton<ITelemetryProcessor, HealthCheckTelemetryProcessor>();

        return services;
    }

    /// <summary>
    /// Adds Application Insights telemetry for worker services (non-HTTP)
    /// </summary>
    public static IServiceCollection AddApplicationInsightsWorkerTelemetry(
        this IServiceCollection services,
        IConfiguration configuration,
        string serviceName)
    {
        var connectionString = configuration["ApplicationInsights:ConnectionString"];
        
        if (string.IsNullOrEmpty(connectionString))
        {
            return services;
        }

        services.AddApplicationInsightsTelemetryWorkerService(options =>
        {
            options.ConnectionString = connectionString;
            options.EnableAdaptiveSampling = true;
            options.EnablePerformanceCounterCollectionModule = true;
            options.EnableDependencyTrackingTelemetryModule = true;
        });

        services.AddSingleton<ITelemetryInitializer>(sp => 
            new ServiceNameTelemetryInitializer(serviceName));

        return services;
    }
}
