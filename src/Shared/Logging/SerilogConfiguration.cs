using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;

namespace Shared.Logging;

/// <summary>
/// Centralized Serilog configuration for all microservices
/// </summary>
public static class SerilogConfiguration
{
    /// <summary>
    /// Configures Serilog with console and file sinks, structured logging, and environment-specific log levels
    /// </summary>
    /// <param name="builder">The WebApplicationBuilder to configure</param>
    /// <param name="serviceName">The name of the service (e.g., "ApiGateway", "AuthService")</param>
    public static void ConfigureSerilog(this WebApplicationBuilder builder, string serviceName)
    {
        var environment = builder.Environment.EnvironmentName;
        var configuration = builder.Configuration;

        // Configure Serilog
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .Enrich.FromLogContext()
            .Enrich.WithProperty("ServiceName", serviceName)
            .Enrich.WithProperty("Environment", environment)
            .Enrich.WithMachineName()
            .WriteTo.Console(
                outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] [{ServiceName}] {Message:lj} {Properties:j}{NewLine}{Exception}")
            .WriteTo.File(
                formatter: new CompactJsonFormatter(),
                path: $"logs/{serviceName}-.log",
                rollingInterval: RollingInterval.Day,
                retainedFileCountLimit: 30,
                fileSizeLimitBytes: 100_000_000, // 100MB
                rollOnFileSizeLimit: true)
            .CreateLogger();

        // Replace default logging with Serilog
        builder.Host.UseSerilog();
    }

    /// <summary>
    /// Gets the minimum log level based on the environment
    /// </summary>
    public static LogEventLevel GetMinimumLogLevel(string environment)
    {
        return environment switch
        {
            "Development" => LogEventLevel.Debug,
            "Staging" => LogEventLevel.Information,
            "Production" => LogEventLevel.Warning,
            _ => LogEventLevel.Information
        };
    }

    /// <summary>
    /// Adds Serilog request logging middleware with enrichment
    /// </summary>
    public static void UseSerilogRequestLogging(this WebApplication app)
    {
        app.UseSerilogRequestLogging(options =>
        {
            options.MessageTemplate = "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";
            options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
            {
                diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value);
                diagnosticContext.Set("RequestScheme", httpContext.Request.Scheme);
                diagnosticContext.Set("RemoteIpAddress", httpContext.Connection.RemoteIpAddress);
                diagnosticContext.Set("UserAgent", httpContext.Request.Headers["User-Agent"].ToString());
                
                // Add user context if available
                if (httpContext.Items.TryGetValue("UserId", out var userId))
                {
                    diagnosticContext.Set("UserId", userId);
                }
                
                // Add request ID for tracing
                if (httpContext.Items.TryGetValue("RequestId", out var requestId))
                {
                    diagnosticContext.Set("RequestId", requestId);
                }
                
                // Add trace ID for distributed tracing
                if (httpContext.Request.Headers.TryGetValue("X-Trace-Id", out var traceId))
                {
                    diagnosticContext.Set("TraceId", traceId.ToString());
                }
            };
        });
    }
}
