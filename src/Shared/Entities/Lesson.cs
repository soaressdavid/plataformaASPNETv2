using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Shared.Models;

namespace Shared.Entities;

public class Lesson : BaseEntity
{
    [Required]
    public Guid CourseId { get; set; }

    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Legacy content field (HTML string) - kept for backward compatibility
    /// </summary>
    [Required]
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// Structured lesson content following the lesson template
    /// Stored as JSON string
    /// </summary>
    public string? StructuredContent { get; set; }

    /// <summary>
    /// Estimated duration (e.g., "45 min", "60 min")
    /// </summary>
    [MaxLength(50)]
    public string Duration { get; set; } = string.Empty;

    /// <summary>
    /// Difficulty level (Iniciante, Intermediário, Avançado)
    /// </summary>
    [MaxLength(50)]
    public string Difficulty { get; set; } = "Iniciante";

    /// <summary>
    /// Estimated completion time in minutes
    /// </summary>
    public int EstimatedMinutes { get; set; }

    /// <summary>
    /// Prerequisites (lesson IDs that should be completed first)
    /// Stored as JSON array
    /// </summary>
    public string Prerequisites { get; set; } = "[]";

    /// <summary>
    /// Content version number for versioning support
    /// </summary>
    public int Version { get; set; } = 1;

    public int OrderIndex { get; set; }

    // Navigation properties
    [ForeignKey(nameof(CourseId))]
    public Course Course { get; set; } = null!;
    
    public ICollection<LessonCompletion> Completions { get; set; } = new List<LessonCompletion>();
}
