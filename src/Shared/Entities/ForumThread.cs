using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Shared.Models;

namespace Shared.Entities;

/// <summary>
/// Represents a discussion thread in the forum
/// </summary>
public class ForumThread : BaseEntity
{
    [Required]
    [MaxLength(300)]
    public string Title { get; set; } = string.Empty;

    [Required]
    public string Content { get; set; } = string.Empty;

    [Required]
    public Guid AuthorId { get; set; }

    /// <summary>
    /// Optional reference to a challenge (if thread is challenge-specific)
    /// </summary>
    public Guid? ChallengeId { get; set; }

    /// <summary>
    /// Optional reference to a lesson (if thread is lesson-specific)
    /// </summary>
    public Guid? LessonId { get; set; }

    /// <summary>
    /// Category for general forum threads
    /// </summary>
    [MaxLength(100)]
    public string? Category { get; set; }

    /// <summary>
    /// Number of views
    /// </summary>
    public int ViewCount { get; set; } = 0;

    /// <summary>
    /// Whether the thread is pinned
    /// </summary>
    public bool IsPinned { get; set; } = false;

    /// <summary>
    /// Whether the thread is locked (no new posts)
    /// </summary>
    public bool IsLocked { get; set; } = false;

    /// <summary>
    /// ID of the accepted answer post (if any)
    /// </summary>
    public Guid? AcceptedAnswerId { get; set; }

    // Navigation properties
    [ForeignKey(nameof(AuthorId))]
    public User Author { get; set; } = null!;

    [ForeignKey(nameof(ChallengeId))]
    public Challenge? Challenge { get; set; }

    [ForeignKey(nameof(LessonId))]
    public Lesson? Lesson { get; set; }

    public ICollection<ForumPost> Posts { get; set; } = new List<ForumPost>();

    [ForeignKey(nameof(AcceptedAnswerId))]
    public ForumPost? AcceptedAnswer { get; set; }
}
