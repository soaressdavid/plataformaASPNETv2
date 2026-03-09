using AITutor.Service.Models;
using AITutor.Service.Services;
using FsCheck;
using FsCheck.Xunit;
using Microsoft.Extensions.Logging;
using Moq;

namespace AITutor.Tests;

/// <summary>
/// Property-based tests for AI Tutor Service functionality.
/// Feature: aspnet-learning-platform
/// </summary>
public class AITutorPropertiesTests
{
    /// <summary>
    /// Property 9: AI Code Review Integration
    /// **Validates: Requirements 4.1**
    /// 
    /// For any code review request, the AI Tutor should call the Groq API with the submitted 
    /// code and return structured feedback.
    /// </summary>
    [Property(MaxTest = 20)]
    public void AICodeReview_CallsGroqAPIAndReturnsStructuredFeedback(NonEmptyString code)
    {
        // Arrange
        var codeStr = code.Get;
        var mockGroqApiClient = new Mock<IGroqApiClient>();
        var mockLogger = new Mock<ILogger<AITutorService>>();
        var promptBuilder = new CodeAnalysisPromptBuilder();
        
        var groqResponse = CreateValidGroqResponse();
        
        mockGroqApiClient
            .Setup(x => x.SendChatCompletionAsync(It.IsAny<List<Message>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(groqResponse);
        
        var service = new AITutorService(mockGroqApiClient.Object, promptBuilder, mockLogger.Object);
        
        // Act
        var result = service.ReviewCodeAsync(codeStr).GetAwaiter().GetResult();
        
        // Assert - Verify Groq API was called
        mockGroqApiClient.Verify(
            x => x.SendChatCompletionAsync(It.IsAny<List<Message>>(), It.IsAny<CancellationToken>()), 
            Times.Once);
        
        // Assert - Verify structured feedback is returned
        Assert.NotNull(result);
        Assert.NotNull(result.Suggestions);
        Assert.NotNull(result.SecurityIssues);
        Assert.NotNull(result.PerformanceIssues);
    }

    /// <summary>
    /// Property 10: AI Feedback Structure
    /// **Validates: Requirements 4.7**
    /// 
    /// For any AI code review response, the feedback should include specific code examples 
    /// for each suggestion.
    /// </summary>
    [Property(MaxTest = 20)]
    public void AIFeedback_IncludesCodeExamplesForEachSuggestion(NonEmptyString code)
    {
        // Arrange
        var codeStr = code.Get;
        var mockGroqApiClient = new Mock<IGroqApiClient>();
        var mockLogger = new Mock<ILogger<AITutorService>>();
        var promptBuilder = new CodeAnalysisPromptBuilder();
        
        // Create a response with multiple suggestions, each having a code example
        var jsonResponse = @"{
            ""suggestions"": [
                {
                    ""type"": ""Security"",
                    ""message"": ""SQL injection vulnerability detected"",
                    ""lineNumber"": 10,
                    ""codeExample"": ""Use parameterized queries: cmd.Parameters.AddWithValue(\""@id\"", id)""
                },
                {
                    ""type"": ""Performance"",
                    ""message"": ""N+1 query problem detected"",
                    ""lineNumber"": 15,
                    ""codeExample"": ""Use Include() for eager loading: context.Users.Include(u => u.Orders)""
                },
                {
                    ""type"": ""BestPractice"",
                    ""message"": ""Use async/await for I/O operations"",
                    ""lineNumber"": 20,
                    ""codeExample"": ""async Task<User> GetUserAsync(int id) { return await context.Users.FindAsync(id); }""
                }
            ],
            ""overallScore"": 70,
            ""securityIssues"": [""SQL Injection""],
            ""performanceIssues"": [""N+1 Query""]
        }";
        
        var groqResponse = CreateGroqResponseWithJson(jsonResponse);
        
        mockGroqApiClient
            .Setup(x => x.SendChatCompletionAsync(It.IsAny<List<Message>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(groqResponse);
        
        var service = new AITutorService(mockGroqApiClient.Object, promptBuilder, mockLogger.Object);
        
        // Act
        var result = service.ReviewCodeAsync(codeStr).GetAwaiter().GetResult();
        
        // Assert - Every suggestion must have a non-empty code example
        Assert.True(result.Suggestions.Count > 0);
        Assert.All(result.Suggestions, s => Assert.False(string.IsNullOrWhiteSpace(s.CodeExample)));
    }

    /// <summary>
    /// Property: AI Tutor handles various code lengths
    /// 
    /// For any valid code string (short or long), the AI Tutor should successfully process 
    /// the request and return a valid response.
    /// </summary>
    [Property(MaxTest = 20)]
    public void AICodeReview_HandlesVariousCodeLengths(NonEmptyString code)
    {
        // Arrange
        var codeStr = code.Get;
        var mockGroqApiClient = new Mock<IGroqApiClient>();
        var mockLogger = new Mock<ILogger<AITutorService>>();
        var promptBuilder = new CodeAnalysisPromptBuilder();
        
        var groqResponse = CreateValidGroqResponse();
        
        mockGroqApiClient
            .Setup(x => x.SendChatCompletionAsync(It.IsAny<List<Message>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(groqResponse);
        
        var service = new AITutorService(mockGroqApiClient.Object, promptBuilder, mockLogger.Object);
        
        // Act
        var result = service.ReviewCodeAsync(codeStr).GetAwaiter().GetResult();
        
        // Assert - Should return valid response regardless of code length
        Assert.NotNull(result);
        Assert.NotNull(result.Suggestions);
        Assert.True(result.OverallScore >= 0 && result.OverallScore <= 100);
    }

    /// <summary>
    /// Property: AI Tutor categorizes feedback correctly
    /// 
    /// For any code review response with suggestions, each suggestion should have a valid 
    /// feedback type (Security, Performance, BestPractice, or Architecture).
    /// </summary>
    [Property(MaxTest = 20)]
    public void AIFeedback_HasValidFeedbackTypes(NonEmptyString code)
    {
        // Arrange
        var codeStr = code.Get;
        var mockGroqApiClient = new Mock<IGroqApiClient>();
        var mockLogger = new Mock<ILogger<AITutorService>>();
        var promptBuilder = new CodeAnalysisPromptBuilder();
        
        var jsonResponse = @"{
            ""suggestions"": [
                {
                    ""type"": ""Security"",
                    ""message"": ""Security issue"",
                    ""lineNumber"": 1,
                    ""codeExample"": ""Fix security""
                },
                {
                    ""type"": ""Performance"",
                    ""message"": ""Performance issue"",
                    ""lineNumber"": 2,
                    ""codeExample"": ""Fix performance""
                },
                {
                    ""type"": ""BestPractice"",
                    ""message"": ""Best practice issue"",
                    ""lineNumber"": 3,
                    ""codeExample"": ""Fix best practice""
                },
                {
                    ""type"": ""Architecture"",
                    ""message"": ""Architecture issue"",
                    ""lineNumber"": 4,
                    ""codeExample"": ""Fix architecture""
                }
            ],
            ""overallScore"": 75,
            ""securityIssues"": [],
            ""performanceIssues"": []
        }";
        
        var groqResponse = CreateGroqResponseWithJson(jsonResponse);
        
        mockGroqApiClient
            .Setup(x => x.SendChatCompletionAsync(It.IsAny<List<Message>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(groqResponse);
        
        var service = new AITutorService(mockGroqApiClient.Object, promptBuilder, mockLogger.Object);
        
        // Act
        var result = service.ReviewCodeAsync(codeStr).GetAwaiter().GetResult();
        
        // Assert - All feedback types should be valid enum values
        var validTypes = new[] { 
            FeedbackType.Security, 
            FeedbackType.Performance, 
            FeedbackType.BestPractice, 
            FeedbackType.Architecture 
        };
        
        Assert.All(result.Suggestions, s => Assert.Contains(s.Type, validTypes));
    }

    /// <summary>
    /// Property: AI Tutor provides line numbers for suggestions
    /// 
    /// For any code review response with suggestions, each suggestion should have a line number 
    /// that is non-negative.
    /// </summary>
    [Property(MaxTest = 20)]
    public void AIFeedback_ProvidesValidLineNumbers(NonEmptyString code)
    {
        // Arrange
        var codeStr = code.Get;
        var mockGroqApiClient = new Mock<IGroqApiClient>();
        var mockLogger = new Mock<ILogger<AITutorService>>();
        var promptBuilder = new CodeAnalysisPromptBuilder();
        
        var jsonResponse = @"{
            ""suggestions"": [
                {
                    ""type"": ""Security"",
                    ""message"": ""Issue at line 5"",
                    ""lineNumber"": 5,
                    ""codeExample"": ""Fix code""
                },
                {
                    ""type"": ""Performance"",
                    ""message"": ""Issue at line 10"",
                    ""lineNumber"": 10,
                    ""codeExample"": ""Optimize code""
                }
            ],
            ""overallScore"": 80,
            ""securityIssues"": [],
            ""performanceIssues"": []
        }";
        
        var groqResponse = CreateGroqResponseWithJson(jsonResponse);
        
        mockGroqApiClient
            .Setup(x => x.SendChatCompletionAsync(It.IsAny<List<Message>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(groqResponse);
        
        var service = new AITutorService(mockGroqApiClient.Object, promptBuilder, mockLogger.Object);
        
        // Act
        var result = service.ReviewCodeAsync(codeStr).GetAwaiter().GetResult();
        
        // Assert - All line numbers should be non-negative
        Assert.All(result.Suggestions, s => Assert.True(s.LineNumber >= 0));
    }

    /// <summary>
    /// Property: AI Tutor handles context parameter
    /// 
    /// For any code review request with context, the context should be passed to the Groq API 
    /// and the service should return a valid response.
    /// </summary>
    [Property(MaxTest = 20)]
    public void AICodeReview_HandlesContextParameter(NonEmptyString code, NonEmptyString context)
    {
        // Arrange
        var codeStr = code.Get;
        var contextStr = context.Get;
        var mockGroqApiClient = new Mock<IGroqApiClient>();
        var mockLogger = new Mock<ILogger<AITutorService>>();
        var promptBuilder = new CodeAnalysisPromptBuilder();
        
        var groqResponse = CreateValidGroqResponse();
        
        mockGroqApiClient
            .Setup(x => x.SendChatCompletionAsync(It.IsAny<List<Message>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(groqResponse);
        
        var service = new AITutorService(mockGroqApiClient.Object, promptBuilder, mockLogger.Object);
        
        // Act
        var result = service.ReviewCodeAsync(codeStr, contextStr).GetAwaiter().GetResult();
        
        // Assert - Should return valid response with context
        mockGroqApiClient.Verify(
            x => x.SendChatCompletionAsync(
                It.Is<List<Message>>(m => m.Any(msg => msg.Content.Contains(contextStr))), 
                It.IsAny<CancellationToken>()), 
            Times.Once);
        
        Assert.NotNull(result);
    }

    // Helper methods to create test data
    private static GroqResponse CreateValidGroqResponse()
    {
        var jsonContent = @"{
            ""suggestions"": [],
            ""overallScore"": 100,
            ""securityIssues"": [],
            ""performanceIssues"": []
        }";

        return CreateGroqResponseWithJson(jsonContent);
    }

    private static GroqResponse CreateGroqResponseWithJson(string jsonContent)
    {
        return new GroqResponse
        {
            Id = "test-id",
            Object = "chat.completion",
            Created = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
            Model = "llama-3.1-70b-versatile",
            Choices = new List<Choice>
            {
                new Choice
                {
                    Index = 0,
                    Message = new Message
                    {
                        Role = "assistant",
                        Content = jsonContent
                    },
                    FinishReason = "stop"
                }
            },
            Usage = new Usage
            {
                PromptTokens = 100,
                CompletionTokens = 50,
                TotalTokens = 150
            }
        };
    }
}
