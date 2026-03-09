using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Text.Json;

namespace Shared.HealthChecks;

/// <summary>
/// Extension methods for configuring health checks
/// </summary>
public static class HealthCheckExtensions
{
    /// <summary>
    /// Adds SQL Server health check
    /// </summary>
    public static IHealthChecksBuilder AddSqlServerHealthCheck(
        this IHealthChecksBuilder builder,
        string connectionString,
        string name = "sqlserver",
        HealthStatus? failureStatus = null,
        IEnumerable<string>? tags = null,
        TimeSpan? timeout = null)
    {
        return builder.Add(new HealthCheckRegistration(
            name,
            sp => new SqlServerHealthCheck(connectionString),
            failureStatus,
            tags,
            timeout));
    }

    /// <summary>
    /// Adds Redis health check
    /// </summary>
    public static IHealthChecksBuilder AddRedisHealthCheck(
        this IHealthChecksBuilder builder,
        string connectionString,
        string name = "redis",
        HealthStatus? failureStatus = null,
        IEnumerable<string>? tags = null,
        TimeSpan? timeout = null)
    {
        return builder.Add(new HealthCheckRegistration(
            name,
            sp => new RedisHealthCheck(connectionString, timeout),
            failureStatus,
            tags,
            timeout));
    }

    /// <summary>
    /// Adds RabbitMQ health check
    /// </summary>
    public static IHealthChecksBuilder AddRabbitMQHealthCheck(
        this IHealthChecksBuilder builder,
        string connectionString,
        string name = "rabbitmq",
        HealthStatus? failureStatus = null,
        IEnumerable<string>? tags = null,
        TimeSpan? timeout = null)
    {
        return builder.Add(new HealthCheckRegistration(
            name,
            sp => new RabbitMqHealthCheck(connectionString),
            failureStatus,
            tags,
            timeout));
    }

    /// <summary>
    /// Adds health checks from configuration
    /// </summary>
    public static IHealthChecksBuilder AddPlatformHealthChecks(
        this IServiceCollection services,
        IConfiguration configuration,
        string serviceName)
    {
        var builder = services.AddHealthChecks();

        // Only add SQL Server health check for services that need it
        // API Gateway doesn't need database health check
        if (serviceName != "ApiGateway")
        {
            var sqlServerConnectionString = configuration.GetConnectionString("DefaultConnection");
            if (!string.IsNullOrEmpty(sqlServerConnectionString))
            {
                // Log the connection string for debugging (mask password)
                var maskedConnectionString = sqlServerConnectionString.Contains("Password=") 
                    ? System.Text.RegularExpressions.Regex.Replace(sqlServerConnectionString, @"Password=[^;]+", "Password=***")
                    : sqlServerConnectionString;
                Console.WriteLine($"[{serviceName}] SQL Server Connection String: {maskedConnectionString}");
                
                builder.AddSqlServerHealthCheck(
                    sqlServerConnectionString,
                    name: "sqlserver",
                    tags: new[] { "database", "sqlserver" },
                    timeout: TimeSpan.FromSeconds(5));
            }

            // Add Redis health check if configured
            var redisConnectionString = configuration.GetConnectionString("Redis");
            if (!string.IsNullOrEmpty(redisConnectionString))
            {
                builder.AddRedisHealthCheck(
                    redisConnectionString,
                    name: "redis",
                    tags: new[] { "cache", "redis" },
                    timeout: TimeSpan.FromSeconds(5));
            }

            // Add RabbitMQ health check if configured
            var rabbitMQHost = configuration["RabbitMQ:Host"];
            if (!string.IsNullOrEmpty(rabbitMQHost))
            {
                var port = configuration.GetValue<int>("RabbitMQ:Port", 5672);
                var username = configuration["RabbitMQ:Username"] ?? "guest";
                var password = configuration["RabbitMQ:Password"] ?? "guest";

                // URL encode the password to handle special characters
                var encodedPassword = Uri.EscapeDataString(password);

                // Build RabbitMQ connection string
                var rabbitMQConnectionString = $"amqp://{username}:{encodedPassword}@{rabbitMQHost}:{port}";
                Console.WriteLine($"[{serviceName}] RabbitMQ Connection String: amqp://{username}:***@{rabbitMQHost}:{port}");
                
                builder.Add(new HealthCheckRegistration(
                    "rabbitmq",
                    sp => new RabbitMqHealthCheck(rabbitMQConnectionString),
                    null,
                    new[] { "messaging", "rabbitmq" },
                    TimeSpan.FromSeconds(5)));
            }
        }

        return builder;
    }

    /// <summary>
    /// Maps health check endpoints with detailed JSON response
    /// </summary>
    public static IEndpointConventionBuilder MapPlatformHealthChecks(
        this IEndpointRouteBuilder endpoints,
        string pattern = "/health")
    {
        return endpoints.MapHealthChecks(pattern, new HealthCheckOptions
        {
            ResponseWriter = WriteHealthCheckResponse
        });
    }

    /// <summary>
    /// Writes a detailed JSON health check response
    /// </summary>
    private static async Task WriteHealthCheckResponse(HttpContext context, HealthReport report)
    {
        context.Response.ContentType = "application/json";

        var response = new
        {
            status = report.Status.ToString(),
            timestamp = DateTime.UtcNow,
            duration = report.TotalDuration.TotalMilliseconds,
            checks = report.Entries.Select(entry => new
            {
                name = entry.Key,
                status = entry.Value.Status.ToString(),
                description = entry.Value.Description,
                duration = entry.Value.Duration.TotalMilliseconds,
                data = entry.Value.Data,
                exception = entry.Value.Exception?.Message,
                tags = entry.Value.Tags
            })
        };

        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(response, options));
    }
}
