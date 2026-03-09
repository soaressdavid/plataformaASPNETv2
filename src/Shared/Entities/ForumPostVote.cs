using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Shared.Models;

namespace Shared.Entities;

/// <summary>
/// Represents a user's vote on a forum post
/// </summary>
public class ForumPostVote : BaseEntity
{
    [Required]
    public Guid PostId { get; set; }

    [Required]
    public Guid UserId { get; set; }

    /// <summary>
    /// Vote value: 1 for upvote, -1 for downvote
    /// </summary>
    [Required]
    public int VoteValue { get; set; } = 1;

    // Navigation properties
    [ForeignKey(nameof(PostId))]
    public ForumPost Post { get; set; } = null!;

    [ForeignKey(nameof(UserId))]
    public User User { get; set; } = null!;
}
