using System.ComponentModel.DataAnnotations;
using Shared.Models;

namespace Shared.Entities;

/// <summary>
/// Tracks a user's progress through a guided project.
/// </summary>
public class ProjectProgress : BaseEntity
{
    [Required]
    public Guid UserId { get; set; }

    [Required]
    public Guid ProjectId { get; set; }

    [Required]
    public int CurrentStep { get; set; } = 1; // Current step the user is on (1-indexed)

    [Required]
    public string CompletedSteps { get; set; } = "[]"; // JSON array of completed step numbers

    public bool IsCompleted { get; set; } = false;

    public DateTime? CompletedAt { get; set; }

    // Navigation properties
    public User User { get; set; } = null!;
    public Project Project { get; set; } = null!;
}
