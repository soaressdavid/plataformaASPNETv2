using System.ComponentModel.DataAnnotations;
using Shared.Models;

namespace Shared.Entities;

public class Challenge : BaseEntity
{
    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required]
    public string Description { get; set; } = string.Empty;

    [Required]
    public Difficulty Difficulty { get; set; }

    [Required]
    public string StarterCode { get; set; } = string.Empty;

    // Navigation properties
    public ICollection<TestCase> TestCases { get; set; } = new List<TestCase>();
    public ICollection<Submission> Submissions { get; set; } = new List<Submission>();
}

public enum Difficulty
{
    Easy,
    Medium,
    Hard
}
