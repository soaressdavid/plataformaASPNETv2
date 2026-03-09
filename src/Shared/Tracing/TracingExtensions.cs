using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Shared.Tracing;

public static class TracingExtensions
{
    public static IServiceCollection AddDistributedTracing(
        this IServiceCollection services,
        string serviceName,
        string? jaegerEndpoint = null)
    {
        services.AddOpenTelemetry()
            .WithTracing(builder =>
            {
                builder
                    .SetResourceBuilder(ResourceBuilder.CreateDefault()
                        .AddService(serviceName)
                        .AddTelemetrySdk()
                        .AddEnvironmentVariableDetector())
                    .AddAspNetCoreInstrumentation(options =>
                    {
                        options.RecordException = true;
                        options.Filter = (httpContext) =>
                        {
                            // Não rastrear health checks e metrics
                            var path = httpContext.Request.Path.Value ?? "";
                            return !path.Contains("/health") && !path.Contains("/metrics");
                        };
                    })
                    .AddHttpClientInstrumentation(options =>
                    {
                        options.RecordException = true;
                    })
                    .AddSqlClientInstrumentation(options =>
                    {
                        options.SetDbStatementForText = true;
                        options.RecordException = true;
                    });

                // Exportar para Jaeger se configurado
                if (!string.IsNullOrEmpty(jaegerEndpoint))
                {
                    builder.AddJaegerExporter(options =>
                    {
                        options.AgentHost = jaegerEndpoint;
                        options.AgentPort = 6831;
                    });
                }
                else
                {
                    // Console exporter para desenvolvimento
                    builder.AddConsoleExporter();
                }
            });

        return services;
    }
}
