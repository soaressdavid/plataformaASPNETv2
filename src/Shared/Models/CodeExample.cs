using System.ComponentModel.DataAnnotations;

namespace Shared.Models;

/// <summary>
/// A code example within a lesson.
/// Code examples should be syntactically valid and executable.
/// </summary>
public class CodeExample
{
    /// <summary>
    /// Title of the code example
    /// </summary>
    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// The actual code
    /// </summary>
    [Required]
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// Programming language (e.g., "csharp", "sql", "json")
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string Language { get; set; } = "csharp";

    /// <summary>
    /// Explanation of what the code does
    /// </summary>
    [Required]
    public string Explanation { get; set; } = string.Empty;

    /// <summary>
    /// Whether this code can be executed in an interactive environment
    /// </summary>
    public bool IsRunnable { get; set; } = true;
}
