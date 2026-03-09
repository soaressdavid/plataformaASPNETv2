using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Shared.Models;

namespace Shared.Entities;

public class Course : BaseEntity
{
    /// <summary>
    /// ID of the curriculum level this course belongs to
    /// </summary>
    public Guid? LevelId { get; set; }

    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required]
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Difficulty level (Iniciante, Intermediário, Avançado)
    /// </summary>
    [Required]
    public Level Level { get; set; }

    /// <summary>
    /// Estimated duration (e.g., "4 semanas", "6 semanas")
    /// </summary>
    [MaxLength(50)]
    public string Duration { get; set; } = string.Empty;

    /// <summary>
    /// Number of lessons in this course
    /// </summary>
    public int LessonCount { get; set; }

    /// <summary>
    /// Topics covered in this course (stored as JSON array)
    /// </summary>
    public string Topics { get; set; } = "[]";

    public int OrderIndex { get; set; }

    // Navigation properties
    [ForeignKey(nameof(LevelId))]
    public CurriculumLevel? CurriculumLevel { get; set; }

    public ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();
    public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
}

public enum Level
{
    Beginner,
    Intermediate,
    Advanced
}
