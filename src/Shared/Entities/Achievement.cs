using Shared.Models;

namespace Shared.Entities;

/// <summary>
/// Represents an achievement that users can earn
/// Validates: Requirements 15.1, 15.2, 15.3, 15.4
/// </summary>
public class Achievement : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty; // Learning, Challenge, Streak, Social
    public string BadgeIcon { get; set; } = string.Empty;
    public string BadgeColor { get; set; } = "#FFD700"; // Gold default
    
    /// <summary>
    /// Criteria for earning this achievement (JSON)
    /// Example: {"type": "challenges_completed", "count": 10, "difficulty": "Easy"}
    /// </summary>
    public string Criteria { get; set; } = string.Empty;
    
    public int XPReward { get; set; }
    public bool IsHidden { get; set; } // Hidden until earned
    public int DisplayOrder { get; set; }
    
    // Navigation properties
    public ICollection<UserAchievement> UserAchievements { get; set; } = new List<UserAchievement>();
}

/// <summary>
/// Represents a user's earned achievement
/// </summary>
public class UserAchievement : BaseEntity
{
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    
    public Guid AchievementId { get; set; }
    public Achievement Achievement { get; set; } = null!;
    
    public DateTime EarnedAt { get; set; }
    public int Progress { get; set; } // For tracking progress towards achievement
    public int ProgressTarget { get; set; } // Target value to complete achievement
}
