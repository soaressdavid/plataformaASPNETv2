using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Interfaces;
using Shared.Messaging;

namespace Shared.Extensions;

/// <summary>
/// Extension methods for registering RabbitMQ services
/// </summary>
public static class RabbitMQServiceExtensions
{
    /// <summary>
    /// Adds RabbitMQ services to the service collection
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <param name="configuration">The configuration</param>
    /// <param name="initializeTopology">Whether to initialize topology on startup (default: true)</param>
    /// <returns>The service collection for chaining</returns>
    public static IServiceCollection AddRabbitMQ(
        this IServiceCollection services,
        IConfiguration configuration,
        bool initializeTopology = true)
    {
        // Register RabbitMQ options
        services.Configure<RabbitMQOptions>(
            configuration.GetSection(RabbitMQOptions.SectionName));

        // Register connection manager as singleton
        services.AddSingleton<IRabbitMQConnectionManager, RabbitMQConnectionManager>();

        // Register event publisher as singleton
        services.AddSingleton<IEventPublisher, RabbitMQEventPublisher>();

        // Register topology initializer
        services.AddSingleton<RabbitMQTopologyInitializer>();

        // Optionally register hosted service for topology initialization
        if (initializeTopology)
        {
            services.AddHostedService<RabbitMQTopologyHostedService>();
        }

        return services;
    }

    /// <summary>
    /// Adds RabbitMQ services with custom options
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <param name="configureOptions">Action to configure RabbitMQ options</param>
    /// <param name="initializeTopology">Whether to initialize topology on startup (default: true)</param>
    /// <returns>The service collection for chaining</returns>
    public static IServiceCollection AddRabbitMQ(
        this IServiceCollection services,
        Action<RabbitMQOptions> configureOptions,
        bool initializeTopology = true)
    {
        // Register RabbitMQ options
        services.Configure(configureOptions);

        // Register connection manager as singleton
        services.AddSingleton<IRabbitMQConnectionManager, RabbitMQConnectionManager>();

        // Register event publisher as singleton
        services.AddSingleton<IEventPublisher, RabbitMQEventPublisher>();

        // Register topology initializer
        services.AddSingleton<RabbitMQTopologyInitializer>();

        // Optionally register hosted service for topology initialization
        if (initializeTopology)
        {
            services.AddHostedService<RabbitMQTopologyHostedService>();
        }

        return services;
    }
}
