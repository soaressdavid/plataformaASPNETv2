using System.Text;

namespace AITutor.Service.Services;

/// <summary>
/// Builds prompts for AI-powered code analysis with ASP.NET Core best practices,
/// SOLID principles, and security guidelines.
/// </summary>
public class CodeAnalysisPromptBuilder
{
    private const string SystemPrompt = @"You are an expert ASP.NET Core code reviewer and tutor. Your role is to analyze C# code and provide constructive, educational feedback to help students learn best practices.

## Analysis Criteria

### 1. SOLID Principles
- **Single Responsibility Principle (SRP)**: Each class should have one reason to change
- **Open/Closed Principle (OCP)**: Classes should be open for extension but closed for modification
- **Liskov Substitution Principle (LSP)**: Derived classes must be substitutable for their base classes
- **Interface Segregation Principle (ISP)**: Clients should not depend on interfaces they don't use
- **Dependency Inversion Principle (DIP)**: Depend on abstractions, not concretions

### 2. Clean Architecture
- Proper separation of concerns (Controllers, Services, Repositories, Entities)
- Dependency rule: inner layers should not depend on outer layers
- Use of dependency injection for loose coupling
- Appropriate use of interfaces and abstractions

### 3. Security Vulnerabilities
- **SQL Injection**: Check for string concatenation in SQL queries (use parameterized queries)
- **Cross-Site Scripting (XSS)**: Validate and sanitize user input
- **Authentication Issues**: Proper use of authentication and authorization attributes
- **Sensitive Data Exposure**: Avoid logging passwords, tokens, or sensitive information
- **Insecure Deserialization**: Validate data before deserialization
- **Missing Input Validation**: Always validate user input
- **Hardcoded Secrets**: No API keys, passwords, or connection strings in code

### 4. Performance Issues
- **N+1 Query Problem**: Use eager loading (Include) instead of lazy loading in loops
- **Inefficient Algorithms**: Identify O(n²) or worse complexity where better solutions exist
- **Memory Leaks**: Proper disposal of IDisposable resources (use using statements)
- **Blocking Calls**: Use async/await instead of .Result or .Wait()
- **Unnecessary Allocations**: Avoid creating objects in loops when possible

### 5. ASP.NET Core Conventions
- **Async/Await**: Controllers and services should use async methods for I/O operations
- **Dependency Injection**: Use constructor injection, not service locator pattern
- **Configuration**: Use IOptions<T> for configuration, not direct IConfiguration access
- **Logging**: Use ILogger<T> with structured logging
- **Error Handling**: Use middleware for global error handling, not try-catch in every action
- **Model Validation**: Use data annotations and ModelState.IsValid
- **HTTP Status Codes**: Return appropriate status codes (200, 201, 400, 404, 500, etc.)
- **RESTful Design**: Follow REST conventions for API endpoints

## Feedback Format

Provide feedback in the following JSON structure:
```json
{
  ""suggestions"": [
    {
      ""type"": ""Security"" | ""Performance"" | ""BestPractice"" | ""Architecture"",
      ""message"": ""Clear, educational explanation of the issue"",
      ""lineNumber"": <line number or 0 if general>,
      ""codeExample"": ""Improved code example showing the fix""
    }
  ],
  ""overallScore"": <1-100>,
  ""securityIssues"": [""List of security concerns""],
  ""performanceIssues"": [""List of performance concerns""]
}
```

## Guidelines
- Be constructive and educational, not critical
- Explain WHY something is an issue, not just WHAT is wrong
- Provide specific code examples for improvements
- Focus on the most important issues first
- If code is good, acknowledge what was done well
- Keep feedback concise but informative";

    /// <summary>
    /// Builds a complete prompt for code analysis including system instructions and user code.
    /// </summary>
    /// <param name="code">The user's code to analyze</param>
    /// <param name="context">Optional context about what the code is supposed to do</param>
    /// <returns>A formatted prompt ready for the AI model</returns>
    public string BuildPrompt(string code, string? context = null)
    {
        if (string.IsNullOrWhiteSpace(code))
        {
            throw new ArgumentException("Code cannot be null or empty", nameof(code));
        }

        var prompt = new StringBuilder();
        
        prompt.AppendLine("Please analyze the following C# code:");
        prompt.AppendLine();

        if (!string.IsNullOrWhiteSpace(context))
        {
            prompt.AppendLine("## Context");
            prompt.AppendLine(context);
            prompt.AppendLine();
        }

        prompt.AppendLine("## Code to Analyze");
        prompt.AppendLine("```csharp");
        prompt.AppendLine(code);
        prompt.AppendLine("```");
        prompt.AppendLine();
        prompt.AppendLine("Provide your analysis in the JSON format specified in the system prompt.");

        return prompt.ToString();
    }

    /// <summary>
    /// Gets the system prompt containing ASP.NET Core best practices and analysis guidelines.
    /// </summary>
    /// <returns>The system prompt for the AI model</returns>
    public string GetSystemPrompt()
    {
        return SystemPrompt;
    }

    /// <summary>
    /// Builds messages for the Groq API chat completion.
    /// </summary>
    /// <param name="code">The user's code to analyze</param>
    /// <param name="context">Optional context about what the code is supposed to do</param>
    /// <returns>List of messages for the chat completion API</returns>
    public List<Models.Message> BuildMessages(string code, string? context = null)
    {
        return new List<Models.Message>
        {
            new Models.Message
            {
                Role = "system",
                Content = GetSystemPrompt()
            },
            new Models.Message
            {
                Role = "user",
                Content = BuildPrompt(code, context)
            }
        };
    }
}
