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

    // Time Attack configuration
    public bool SupportsTimeAttack { get; set; }
    public int TimeAttackLimitSeconds { get; set; } = 900; // 15 minutes default

    // Code Review configuration
    public bool IsCodeReviewChallenge { get; set; }
    public string? CodeToReview { get; set; } // Code with intentional bugs
    public string? ExpectedIssues { get; set; } // JSON array of expected bug locations and descriptions

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
