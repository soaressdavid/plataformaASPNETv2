namespace Shared.DTOs;

/// <summary>
/// Represents an issue identified by the user in code review
/// </summary>
public class IdentifiedIssue
{
    public int LineNumber { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Severity { get; set; } = "Medium"; // Low, Medium, High
}

/// <summary>
/// Represents an expected bug in the code review challenge
/// </summary>
public class ExpectedBug
{
    public int LineNumber { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty; // e.g., "NullReference", "LogicError", "SecurityIssue"
    public string Severity { get; set; } = "Medium";
}

/// <summary>
/// Request to submit code review findings
/// </summary>
public class SubmitCodeReviewRequest
{
    public string UserId { get; set; } = string.Empty;
    public string ChallengeId { get; set; } = string.Empty;
    public List<IdentifiedIssue> IdentifiedIssues { get; set; } = new();
}

/// <summary>
/// Response after validating code review submission
/// </summary>
public class CodeReviewValidationResponse
{
    public bool Success { get; set; }
    public int TotalExpectedBugs { get; set; }
    public int CorrectlyIdentified { get; set; }
    public int MissedBugs { get; set; }
    public int FalsePositives { get; set; }
    public double AccuracyPercentage { get; set; }
    public int XpAwarded { get; set; }
    public List<BugValidationResult> BugResults { get; set; } = new();
}

/// <summary>
/// Validation result for a single bug
/// </summary>
public class BugValidationResult
{
    public int LineNumber { get; set; }
    public string ExpectedDescription { get; set; } = string.Empty;
    public bool WasIdentified { get; set; }
    public string? UserDescription { get; set; }
}
