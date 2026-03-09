namespace AITutor.Service.Models;

public record CodeReviewResponse
{
    public required List<Feedback> Suggestions { get; init; }
    public int OverallScore { get; init; }
    public required List<string> SecurityIssues { get; init; }
    public required List<string> PerformanceIssues { get; init; }
}

public record Feedback
{
    public required FeedbackType Type { get; init; }
    public required string Message { get; init; }
    public int LineNumber { get; init; }
    public required string CodeExample { get; init; }
}

public enum FeedbackType
{
    Security,
    Performance,
    BestPractice,
    Architecture
}
