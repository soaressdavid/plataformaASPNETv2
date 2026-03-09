using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Shared.Models;

namespace Shared.Entities;

public class Project : BaseEntity
{
    /// <summary>
    /// ID of the curriculum level this project belongs to
    /// </summary>
    public Guid? LevelId { get; set; }

    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required]
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Project objectives (stored as JSON array)
    /// </summary>
    public string Objectives { get; set; } = "[]";

    /// <summary>
    /// Technical scope and technologies used (stored as JSON array)
    /// </summary>
    public string TechnicalScope { get; set; } = "[]";

    /// <summary>
    /// Project steps (stored as JSON array of ProjectStep objects)
    /// </summary>
    [Required]
    public string Steps { get; set; } = string.Empty;

    [Required]
    public int XPReward { get; set; } = 100;

    // Navigation properties
    [ForeignKey(nameof(LevelId))]
    public CurriculumLevel? CurriculumLevel { get; set; }
}
