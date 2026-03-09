using System.ComponentModel.DataAnnotations;

namespace Shared.Entities;

/// <summary>
/// Represents a collaborative coding session
/// Validates: Requirement 32.1, 32.2, 32.8
/// </summary>
public class CollaborativeSession
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Challenge or playground being worked on
    /// </summary>
    public Guid? ChallengeId { get; set; }

    /// <summary>
    /// Session name for identification
    /// </summary>
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Current code content (synchronized)
    /// </summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// Programming language
    /// </summary>
    [MaxLength(50)]
    public string Language { get; set; } = "csharp";

    /// <summary>
    /// Session status
    /// </summary>
    public SessionStatus Status { get; set; } = SessionStatus.Active;

    /// <summary>
    /// User who created the session
    /// </summary>
    public Guid CreatedByUserId { get; set; }

    /// <summary>
    /// When the session was created
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// When the session was last updated
    /// </summary>
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// When the session ended
    /// </summary>
    public DateTime? EndedAt { get; set; }

    /// <summary>
    /// Soft delete flag
    /// </summary>
    public bool IsDeleted { get; set; }

    // Navigation properties
    public virtual User CreatedByUser { get; set; } = null!;
    public virtual Challenge? Challenge { get; set; }
    public virtual ICollection<CollaborativeSessionParticipant> Participants { get; set; } = new List<CollaborativeSessionParticipant>();
}

public enum SessionStatus
{
    Active,
    Completed,
    Abandoned
}
