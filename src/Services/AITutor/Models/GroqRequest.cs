namespace AITutor.Service.Models;

public record GroqRequest
{
    public required string Model { get; init; }
    public required List<Message> Messages { get; init; }
    public double Temperature { get; init; }
    public int MaxTokens { get; init; }
}

public record Message
{
    public required string Role { get; init; }
    public required string Content { get; init; }
}
