using System.ComponentModel.DataAnnotations;

namespace Shared.Entities;

/// <summary>
/// Represents a chat message in a room
/// Validates: Requirements 34.1, 34.5, 34.6, 34.8
/// </summary>
public class ChatMessage
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    
    [Required]
    public Guid ChatRoomId { get; set; }
    
    [Required]
    public Guid UserId { get; set; }
    
    [Required]
    [MaxLength(2000)]
    public string Content { get; set; } = string.Empty;
    
    public ChatMessageType Type { get; set; } = ChatMessageType.Text;
    
    /// <summary>
    /// For code snippets, stores the language
    /// </summary>
    [MaxLength(50)]
    public string? CodeLanguage { get; set; }
    
    /// <summary>
    /// Emoji reactions (JSON array of {emoji, userIds[]})
    /// </summary>
    public string? Reactions { get; set; }
    
    public bool IsReported { get; set; }
    public bool IsModerated { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public bool IsDeleted { get; set; }
    
    // Navigation properties
    public virtual ChatRoom ChatRoom { get; set; } = null!;
    public virtual User User { get; set; } = null!;
}

public enum ChatMessageType
{
    Text,
    Code,
    System
}
