using Shared.Models;

namespace Shared.Interfaces;

/// <summary>
/// Interface for publishing domain events to the message bus
/// </summary>
public interface IEventPublisher
{
    /// <summary>
    /// Publishes a domain event to RabbitMQ with appropriate routing
    /// </summary>
    /// <typeparam name="TEvent">Type of the domain event</typeparam>
    /// <param name="event">The event to publish</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Task representing the asynchronous operation</returns>
    Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default) 
        where TEvent : DomainEvent;
}
