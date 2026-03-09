using System.ComponentModel.DataAnnotations;

namespace Shared.Models;

/// <summary>
/// Structured content for a lesson following the lesson template.
/// Contains objectives, theory sections, code examples, exercises, and summary.
/// </summary>
public class LessonContent
{
    /// <summary>
    /// Learning objectives (3-7 items required)
    /// </summary>
    [Required]
    [MinLength(3)]
    [MaxLength(7)]
    public List<string> Objectives { get; set; } = new List<string>();

    /// <summary>
    /// Theory sections (200-500 words each)
    /// </summary>
    [Required]
    [MinLength(1)]
    public List<TheorySection> Theory { get; set; } = new List<TheorySection>();

    /// <summary>
    /// Code examples (minimum 2 required)
    /// </summary>
    [Required]
    [MinLength(2)]
    public List<CodeExample> CodeExamples { get; set; } = new List<CodeExample>();

    /// <summary>
    /// Practice exercises (minimum 3 required)
    /// </summary>
    [Required]
    [MinLength(3)]
    public List<Exercise> Exercises { get; set; } = new List<Exercise>();

    /// <summary>
    /// Lesson summary
    /// </summary>
    [Required]
    public string Summary { get; set; } = string.Empty;
}
