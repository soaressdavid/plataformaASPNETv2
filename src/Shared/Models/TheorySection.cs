using System.ComponentModel.DataAnnotations;

namespace Shared.Models;

/// <summary>
/// A theory section within a lesson.
/// Each section should contain 200-500 words of content.
/// </summary>
public class TheorySection
{
    /// <summary>
    /// Section heading
    /// </summary>
    [Required]
    [MaxLength(200)]
    public string Heading { get; set; } = string.Empty;

    /// <summary>
    /// Section content (200-500 words)
    /// </summary>
    [Required]
    [MinLength(200)]
    [MaxLength(5000)]
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// Display order within the lesson
    /// </summary>
    [Required]
    public int Order { get; set; }
}
