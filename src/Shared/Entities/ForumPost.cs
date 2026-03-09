using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Shared.Models;

namespace Shared.Entities;

/// <summary>
/// Represents a post/reply in a forum thread
/// </summary>
public class ForumPost : BaseEntity
{
    [Required]
    public Guid ThreadId { get; set; }

    [Required]
    public Guid AuthorId { get; set; }

    [Required]
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// Number of upvotes
    /// </summary>
    public int Upvotes { get; set; } = 0;

    /// <summary>
    /// Whether this post has been edited
    /// </summary>
    public bool IsEdited { get; set; } = false;

    /// <summary>
    /// Last edit timestamp
    /// </summary>
    public DateTime? LastEditedAt { get; set; }

    /// <summary>
    /// Whether this post has been reported
    /// </summary>
    public bool IsReported { get; set; } = false;

    /// <summary>
    /// Whether this post is marked as the accepted answer
    /// </summary>
    public bool IsAcceptedAnswer { get; set; } = false;

    // Navigation properties
    [ForeignKey(nameof(ThreadId))]
    public ForumThread Thread { get; set; } = null!;

    [ForeignKey(nameof(AuthorId))]
    public User Author { get; set; } = null!;

    public ICollection<ForumPostVote> Votes { get; set; } = new List<ForumPostVote>();
}
