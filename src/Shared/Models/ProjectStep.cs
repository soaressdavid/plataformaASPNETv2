using System.ComponentModel.DataAnnotations;

namespace Shared.Models;

/// <summary>
/// A step in a project with instructions and validation criteria.
/// </summary>
public class ProjectStep
{
    /// <summary>
    /// Step order in the project
    /// </summary>
    [Required]
    public int Order { get; set; }

    /// <summary>
    /// Step title
    /// </summary>
    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Detailed instructions for this step
    /// </summary>
    [Required]
    public string Instructions { get; set; } = string.Empty;

    /// <summary>
    /// Starting code template for this step
    /// </summary>
    public string StarterCode { get; set; } = string.Empty;

    /// <summary>
    /// Validation criteria to assess step completion
    /// </summary>
    public List<string> ValidationCriteria { get; set; } = new List<string>();
}
