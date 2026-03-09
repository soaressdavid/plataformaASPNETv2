namespace AITutor.Service.Models;

public record GroqResponse
{
    public required string Id { get; init; }
    public required string Object { get; init; }
    public long Created { get; init; }
    public required string Model { get; init; }
    public required List<Choice> Choices { get; init; }
    public Usage? Usage { get; init; }
}

public record Choice
{
    public int Index { get; init; }
    public required Message Message { get; init; }
    public string? FinishReason { get; init; }
}

public record Usage
{
    public int PromptTokens { get; init; }
    public int CompletionTokens { get; init; }
    public int TotalTokens { get; init; }
}
