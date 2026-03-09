using System.ComponentModel.DataAnnotations;

namespace Shared.Entities;

/// <summary>
/// Represents a user's membership in a chat room
/// Validates: Requirements 34.3, 34.4, 34.7
/// </summary>
public class ChatRoomMember
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    
    [Required]
    public Guid ChatRoomId { get; set; }
    
    [Required]
    public Guid UserId { get; set; }
    
    public bool IsMuted { get; set; }
    public DateTime? MutedUntil { get; set; }
    
    public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
    public DateTime? LastReadAt { get; set; }
    
    /// <summary>
    /// Blocked user IDs (JSON array)
    /// </summary>
    public string? BlockedUserIds { get; set; }
    
    // Navigation properties
    public virtual ChatRoom ChatRoom { get; set; } = null!;
    public virtual User User { get; set; } = null!;
}
