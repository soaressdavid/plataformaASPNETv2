namespace AITutor.Service.Configuration;

public class GroqSettings
{
    public const string SectionName = "Groq";
    
    public string ApiKey { get; set; } = string.Empty;
    public string ApiUrl { get; set; } = "https://api.groq.com/openai/v1/chat/completions";
    public string Model { get; set; } = "llama-3.1-70b-versatile";
    public double Temperature { get; set; } = 0.3;
    public int MaxTokens { get; set; } = 2000;
    public int TimeoutSeconds { get; set; } = 10;
}
