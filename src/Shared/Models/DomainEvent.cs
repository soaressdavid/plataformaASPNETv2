namespace Shared.Models;

/// <summary>
/// Base class for all domain events in the platform
/// </summary>
public abstract class DomainEvent
{
    /// <summary>
    /// Unique identifier for this event
    /// </summary>
    public Guid EventId { get; init; } = Guid.NewGuid();

    /// <summary>
    /// Timestamp when the event occurred
    /// </summary>
    public DateTime OccurredAt { get; init; } = DateTime.UtcNow;

    /// <summary>
    /// Type of the event (used for routing)
    /// </summary>
    public abstract string EventType { get; }
}
