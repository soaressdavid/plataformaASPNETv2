namespace AITutor.Service.Models;

public record CodeReviewRequest
{
    public required string Code { get; init; }
    public string? Context { get; init; }
}
