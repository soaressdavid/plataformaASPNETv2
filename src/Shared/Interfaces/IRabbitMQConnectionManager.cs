using RabbitMQ.Client;

namespace Shared.Interfaces;

/// <summary>
/// Manages RabbitMQ connections and channels with automatic reconnection and retry logic
/// </summary>
public interface IRabbitMQConnectionManager : IDisposable
{
    /// <summary>
    /// Gets the current connection. Creates a new connection if none exists or if the current one is closed.
    /// </summary>
    /// <returns>An active RabbitMQ connection</returns>
    IConnection GetConnection();

    /// <summary>
    /// Creates a new channel from the connection pool
    /// </summary>
    /// <returns>A new RabbitMQ channel</returns>
    IModel CreateChannel();

    /// <summary>
    /// Declares an exchange with the specified configuration
    /// </summary>
    /// <param name="exchangeName">Name of the exchange</param>
    /// <param name="exchangeType">Type of exchange (direct, topic, fanout, headers)</param>
    /// <param name="durable">Whether the exchange survives broker restart</param>
    /// <param name="autoDelete">Whether the exchange is deleted when no longer in use</param>
    Task DeclareExchangeAsync(string exchangeName, string exchangeType, bool durable = true, bool autoDelete = false);

    /// <summary>
    /// Declares a queue with the specified configuration
    /// </summary>
    /// <param name="queueName">Name of the queue</param>
    /// <param name="durable">Whether the queue survives broker restart</param>
    /// <param name="exclusive">Whether the queue is exclusive to this connection</param>
    /// <param name="autoDelete">Whether the queue is deleted when no longer in use</param>
    Task DeclareQueueAsync(string queueName, bool durable = true, bool exclusive = false, bool autoDelete = false);

    /// <summary>
    /// Binds a queue to an exchange with a routing key
    /// </summary>
    /// <param name="queueName">Name of the queue</param>
    /// <param name="exchangeName">Name of the exchange</param>
    /// <param name="routingKey">Routing key for message routing</param>
    Task BindQueueAsync(string queueName, string exchangeName, string routingKey);

    /// <summary>
    /// Checks if the connection is currently open and healthy
    /// </summary>
    bool IsConnected { get; }
}
