using System.ComponentModel.DataAnnotations;

namespace Shared.Entities;

/// <summary>
/// Represents a chat room (global course chat or direct message)
/// Validates: Requirements 34.2, 34.3
/// </summary>
public class ChatRoom
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;
    
    public ChatRoomType Type { get; set; }
    
    /// <summary>
    /// For course rooms, this is the course ID
    /// For direct messages, this is null
    /// </summary>
    public Guid? CourseId { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public bool IsDeleted { get; set; }
    
    // Navigation properties
    public virtual ICollection<ChatMessage> Messages { get; set; } = new List<ChatMessage>();
    public virtual ICollection<ChatRoomMember> Members { get; set; } = new List<ChatRoomMember>();
}

public enum ChatRoomType
{
    Global,
    Course,
    DirectMessage
}
