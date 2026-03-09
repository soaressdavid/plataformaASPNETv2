using Shared.Models;

namespace Shared.Entities;

/// <summary>
/// Represents a daily or weekly mission
/// Validates: Requirements 16.1, 16.2, 16.3
/// </summary>
public class Mission : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public MissionType Type { get; set; } // Daily or Weekly
    public MissionCategory Category { get; set; } // Learning, Challenge, Social
    
    /// <summary>
    /// Objective criteria (JSON)
    /// Example: {"type": "complete_lessons", "count": 3}
    /// </summary>
    public string Objective { get; set; } = string.Empty;
    
    public int XPReward { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsActive { get; set; }
    
    // Navigation properties
    public ICollection<UserMission> UserMissions { get; set; } = new List<UserMission>();
}

/// <summary>
/// Represents a user's mission progress
/// </summary>
public class UserMission : BaseEntity
{
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    
    public Guid MissionId { get; set; }
    public Mission Mission { get; set; } = null!;
    
    public int Progress { get; set; }
    public int ProgressTarget { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime? CompletedAt { get; set; }
    public DateTime AssignedAt { get; set; }
}

public enum MissionType
{
    Daily,
    Weekly
}

public enum MissionCategory
{
    Learning,
    Challenge,
    Social,
    Practice
}
