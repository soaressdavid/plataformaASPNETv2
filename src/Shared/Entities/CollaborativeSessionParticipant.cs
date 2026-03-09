using System.ComponentModel.DataAnnotations;

namespace Shared.Entities;

/// <summary>
/// Represents a participant in a collaborative session
/// Validates: Requirement 32.1, 32.6, 32.9
/// </summary>
public class CollaborativeSessionParticipant
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Session this participant belongs to
    /// </summary>
    public Guid SessionId { get; set; }

    /// <summary>
    /// User participating in the session
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Role in the session
    /// </summary>
    public ParticipantRole Role { get; set; } = ParticipantRole.Collaborator;

    /// <summary>
    /// When the user joined the session
    /// </summary>
    public DateTime JoinedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// When the user left the session
    /// </summary>
    public DateTime? LeftAt { get; set; }

    /// <summary>
    /// Whether the user is currently active in the session
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// XP earned from this session (split equally on completion)
    /// </summary>
    public int XPEarned { get; set; }

    // Navigation properties
    public virtual CollaborativeSession Session { get; set; } = null!;
    public virtual User User { get; set; } = null!;
}

public enum ParticipantRole
{
    Owner,
    Collaborator
}
