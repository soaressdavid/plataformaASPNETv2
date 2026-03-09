using System.ComponentModel.DataAnnotations;
using Shared.Models;

namespace Shared.Entities;

/// <summary>
/// Represents a level in the 15-level curriculum structure (Level 0-15).
/// Each level contains multiple courses and one capstone project.
/// </summary>
public class CurriculumLevel : BaseEntity
{
    /// <summary>
    /// Level number (0-15)
    /// </summary>
    [Required]
    [Range(0, 15)]
    public int Number { get; set; }

    /// <summary>
    /// Level title (e.g., "Programming Fundamentals", "C# Basics")
    /// </summary>
    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Detailed description of what learners will achieve in this level
    /// </summary>
    [Required]
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Experience points required to unlock this level
    /// </summary>
    [Required]
    public int RequiredXP { get; set; }

    // Navigation properties
    /// <summary>
    /// Courses contained in this level
    /// </summary>
    public ICollection<Course> Courses { get; set; } = new List<Course>();

    /// <summary>
    /// Capstone project for this level
    /// </summary>
    public Project? Project { get; set; }
}
