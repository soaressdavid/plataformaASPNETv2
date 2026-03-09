# AI Tutor Service

The AI Tutor Service provides AI-powered code review and feedback using the Groq API with the llama-3.1-70b-versatile model.

## Groq API Client Configuration

The Groq API client is configured with the following settings:

- **Model**: `llama-3.1-70b-versatile`
- **Temperature**: `0.3` (more deterministic responses)
- **Max Tokens**: `2000`
- **Timeout**: `10 seconds`

### Configuration

The Groq API client is configured in `appsettings.json`:

```json
{
  "Groq": {
    "ApiKey": "",
    "ApiUrl": "https://api.groq.com/openai/v1/chat/completions",
    "Model": "llama-3.1-70b-versatile",
    "Temperature": 0.3,
    "MaxTokens": 2000,
    "TimeoutSeconds": 10
  }
}
```

For development, set your API key in `appsettings.Development.json`:

```json
{
  "Groq": {
    "ApiKey": "your-groq-api-key-here"
  }
}
```

### Features

- **HTTP Client Configuration**: Uses `HttpClient` with proper timeout handling
- **Authentication**: Bearer token authentication with Groq API key
- **Error Handling**: Comprehensive error handling for timeouts, HTTP errors, and deserialization failures
- **Logging**: Detailed logging for debugging and monitoring
- **Timeout Handling**: 10-second timeout for all API requests

### Usage

The `IGroqApiClient` interface provides a simple method to send chat completion requests:

```csharp
public interface IGroqApiClient
{
    Task<GroqResponse> SendChatCompletionAsync(
        List<Message> messages, 
        CancellationToken cancellationToken = default);
}
```

Example usage:

```csharp
var messages = new List<Message>
{
    new Message { Role = "system", Content = "You are a helpful coding assistant." },
    new Message { Role = "user", Content = "Review this C# code..." }
};

var response = await groqApiClient.SendChatCompletionAsync(messages);
var feedback = response.Choices.FirstOrDefault()?.Message.Content;
```

### Testing

A test endpoint is available at `POST /api/health/test-groq` to verify the Groq API integration:

```bash
curl -X POST http://localhost:5000/api/health/test-groq \
  -H "Content-Type: application/json" \
  -d '{"message": "Hello, Groq!"}'
```

### Requirements Validation

This implementation satisfies:
- **Requirement 4.1**: AI Tutor analyzes code using Groq API
- **Requirement 4.6**: Returns feedback within 10 seconds (enforced by timeout)

### Error Handling

The client handles the following error scenarios:

1. **Timeout**: Returns `TimeoutException` after 10 seconds
2. **HTTP Errors**: Logs and throws `HttpRequestException` with status code
3. **Deserialization Errors**: Throws `InvalidOperationException`
4. **Cancellation**: Properly handles `CancellationToken`

All errors are logged with appropriate context for debugging.
