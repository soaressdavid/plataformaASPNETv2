using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Shared.Models;

namespace Shared.Entities;

public class Progress : BaseEntity
{
    [Required]
    public Guid UserId { get; set; }

    public int TotalXP { get; set; }

    public int CurrentLevel { get; set; }

    public int LearningStreak { get; set; }

    public DateTime LastActivityAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    [ForeignKey(nameof(UserId))]
    public User User { get; set; } = null!;

    // Computed property
    [NotMapped]
    public int XPToNextLevel => ((CurrentLevel + 1) * (CurrentLevel + 1) * 100) - TotalXP;
}
