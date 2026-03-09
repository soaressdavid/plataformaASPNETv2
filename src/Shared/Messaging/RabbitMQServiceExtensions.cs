using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Interfaces;

namespace Shared.Messaging;

/// <summary>
/// Extension methods for registering RabbitMQ services
/// </summary>
public static class RabbitMQServiceExtensions
{
    /// <summary>
    /// Adds RabbitMQ connection management services to the service collection
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <param name="configuration">The configuration containing RabbitMQ settings</param>
    /// <returns>The service collection for chaining</returns>
    public static IServiceCollection AddRabbitMQ(this IServiceCollection services, IConfiguration configuration)
    {
        // Register RabbitMQ options
        services.Configure<RabbitMQOptions>(configuration.GetSection(RabbitMQOptions.SectionName));

        // Register connection manager as singleton (one connection per application)
        services.AddSingleton<IRabbitMQConnectionManager, RabbitMQConnectionManager>();

        // Register event publisher as singleton
        services.AddSingleton<IEventPublisher, RabbitMQEventPublisher>();

        return services;
    }

    /// <summary>
    /// Adds RabbitMQ connection management services with custom options
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <param name="configureOptions">Action to configure RabbitMQ options</param>
    /// <returns>The service collection for chaining</returns>
    public static IServiceCollection AddRabbitMQ(this IServiceCollection services, Action<RabbitMQOptions> configureOptions)
    {
        services.Configure(configureOptions);
        services.AddSingleton<IRabbitMQConnectionManager, RabbitMQConnectionManager>();
        services.AddSingleton<IEventPublisher, RabbitMQEventPublisher>();

        return services;
    }
}
