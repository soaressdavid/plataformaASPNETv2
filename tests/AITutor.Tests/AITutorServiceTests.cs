using AITutor.Service.Models;
using AITutor.Service.Services;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace AITutor.Tests;

public class AITutorServiceTests
{
    private readonly Mock<IGroqApiClient> _mockGroqApiClient;
    private readonly Mock<ILogger<AITutorService>> _mockLogger;
    private readonly CodeAnalysisPromptBuilder _promptBuilder;
    private readonly AITutorService _service;

    public AITutorServiceTests()
    {
        _mockGroqApiClient = new Mock<IGroqApiClient>();
        _mockLogger = new Mock<ILogger<AITutorService>>();
        _promptBuilder = new CodeAnalysisPromptBuilder();
        _service = new AITutorService(_mockGroqApiClient.Object, _promptBuilder, _mockLogger.Object);
    }

    [Fact]
    public async Task ReviewCodeAsync_WithValidCode_ReturnsCodeReviewResponse()
    {
        // Arrange
        var code = "public class Test { }";
        var groqResponse = CreateValidGroqResponse();
        
        _mockGroqApiClient
            .Setup(x => x.SendChatCompletionAsync(It.IsAny<List<Message>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(groqResponse);

        // Act
        var result = await _service.ReviewCodeAsync(code);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Suggestions);
        Assert.NotNull(result.SecurityIssues);
        Assert.NotNull(result.PerformanceIssues);
        _mockGroqApiClient.Verify(x => x.SendChatCompletionAsync(It.IsAny<List<Message>>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ReviewCodeAsync_WithNullCode_ThrowsArgumentException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _service.ReviewCodeAsync(null!));
    }

    [Fact]
    public async Task ReviewCodeAsync_WithEmptyCode_ThrowsArgumentException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _service.ReviewCodeAsync(string.Empty));
    }

    [Fact]
    public async Task ReviewCodeAsync_WithWhitespaceCode_ThrowsArgumentException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _service.ReviewCodeAsync("   "));
    }

    [Fact]
    public async Task ReviewCodeAsync_ParsesSecurityFeedback_Correctly()
    {
        // Arrange
        var code = "public class Test { }";
        var jsonResponse = @"{
            ""suggestions"": [
                {
                    ""type"": ""Security"",
                    ""message"": ""SQL injection vulnerability detected"",
                    ""lineNumber"": 10,
                    ""codeExample"": ""Use parameterized queries""
                }
            ],
            ""overallScore"": 60,
            ""securityIssues"": [""SQL Injection""],
            ""performanceIssues"": []
        }";
        
        var groqResponse = CreateGroqResponseWithJson(jsonResponse);
        
        _mockGroqApiClient
            .Setup(x => x.SendChatCompletionAsync(It.IsAny<List<Message>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(groqResponse);

        // Act
        var result = await _service.ReviewCodeAsync(code);

        // Assert
        Assert.Single(result.Suggestions);
        Assert.Equal(FeedbackType.Security, result.Suggestions[0].Type);
        Assert.Equal("SQL injection vulnerability detected", result.Suggestions[0].Message);
        Assert.Equal(10, result.Suggestions[0].LineNumber);
        Assert.Equal("Use parameterized queries", result.Suggestions[0].CodeExample);
        Assert.Equal(60, result.OverallScore);
        Assert.Single(result.SecurityIssues);
        Assert.Equal("SQL Injection", result.SecurityIssues[0]);
    }

    [Fact]
    public async Task ReviewCodeAsync_ParsesPerformanceFeedback_Correctly()
    {
        // Arrange
        var code = "public class Test { }";
        var jsonResponse = @"{
            ""suggestions"": [
                {
                    ""type"": ""Performance"",
                    ""message"": ""N+1 query problem detected"",
                    ""lineNumber"": 15,
                    ""codeExample"": ""Use Include() for eager loading""
                }
            ],
            ""overallScore"": 70,
            ""securityIssues"": [],
            ""performanceIssues"": [""N+1 Query""]
        }";
        
        var groqResponse = CreateGroqResponseWithJson(jsonResponse);
        
        _mockGroqApiClient
            .Setup(x => x.SendChatCompletionAsync(It.IsAny<List<Message>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(groqResponse);

        // Act
        var result = await _service.ReviewCodeAsync(code);

        // Assert
        Assert.Single(result.Suggestions);
        Assert.Equal(FeedbackType.Performance, result.Suggestions[0].Type);
        Assert.Single(result.PerformanceIssues);
    }

    [Fact]
    public async Task ReviewCodeAsync_ParsesBestPracticeFeedback_Correctly()
    {
        // Arrange
        var code = "public class Test { }";
        var jsonResponse = @"{
            ""suggestions"": [
                {
                    ""type"": ""BestPractice"",
                    ""message"": ""Use async/await for I/O operations"",
                    ""lineNumber"": 20,
                    ""codeExample"": ""async Task<T> MethodAsync()""
                }
            ],
            ""overallScore"": 80,
            ""securityIssues"": [],
            ""performanceIssues"": []
        }";
        
        var groqResponse = CreateGroqResponseWithJson(jsonResponse);
        
        _mockGroqApiClient
            .Setup(x => x.SendChatCompletionAsync(It.IsAny<List<Message>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(groqResponse);

        // Act
        var result = await _service.ReviewCodeAsync(code);

        // Assert
        Assert.Single(result.Suggestions);
        Assert.Equal(FeedbackType.BestPractice, result.Suggestions[0].Type);
    }

    [Fact]
    public async Task ReviewCodeAsync_ParsesArchitectureFeedback_Correctly()
    {
        // Arrange
        var code = "public class Test { }";
        var jsonResponse = @"{
            ""suggestions"": [
                {
                    ""type"": ""Architecture"",
                    ""message"": ""Violates Single Responsibility Principle"",
                    ""lineNumber"": 5,
                    ""codeExample"": ""Split into separate classes""
                }
            ],
            ""overallScore"": 65,
            ""securityIssues"": [],
            ""performanceIssues"": []
        }";
        
        var groqResponse = CreateGroqResponseWithJson(jsonResponse);
        
        _mockGroqApiClient
            .Setup(x => x.SendChatCompletionAsync(It.IsAny<List<Message>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(groqResponse);

        // Act
        var result = await _service.ReviewCodeAsync(code);

        // Assert
        Assert.Single(result.Suggestions);
        Assert.Equal(FeedbackType.Architecture, result.Suggestions[0].Type);
    }

    [Fact]
    public async Task ReviewCodeAsync_WithMarkdownCodeBlock_ExtractsJsonCorrectly()
    {
        // Arrange
        var code = "public class Test { }";
        var jsonResponse = @"```json
{
    ""suggestions"": [],
    ""overallScore"": 90,
    ""securityIssues"": [],
    ""performanceIssues"": []
}
```";
        
        var groqResponse = CreateGroqResponseWithJson(jsonResponse);
        
        _mockGroqApiClient
            .Setup(x => x.SendChatCompletionAsync(It.IsAny<List<Message>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(groqResponse);

        // Act
        var result = await _service.ReviewCodeAsync(code);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(90, result.OverallScore);
    }

    [Fact]
    public async Task ReviewCodeAsync_WithMultipleSuggestions_ParsesAllCorrectly()
    {
        // Arrange
        var code = "public class Test { }";
        var jsonResponse = @"{
            ""suggestions"": [
                {
                    ""type"": ""Security"",
                    ""message"": ""Security issue"",
                    ""lineNumber"": 1,
                    ""codeExample"": ""Fix 1""
                },
                {
                    ""type"": ""Performance"",
                    ""message"": ""Performance issue"",
                    ""lineNumber"": 2,
                    ""codeExample"": ""Fix 2""
                },
                {
                    ""type"": ""BestPractice"",
                    ""message"": ""Best practice issue"",
                    ""lineNumber"": 3,
                    ""codeExample"": ""Fix 3""
                }
            ],
            ""overallScore"": 75,
            ""securityIssues"": [""Issue 1""],
            ""performanceIssues"": [""Issue 2""]
        }";
        
        var groqResponse = CreateGroqResponseWithJson(jsonResponse);
        
        _mockGroqApiClient
            .Setup(x => x.SendChatCompletionAsync(It.IsAny<List<Message>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(groqResponse);

        // Act
        var result = await _service.ReviewCodeAsync(code);

        // Assert
        Assert.Equal(3, result.Suggestions.Count);
        Assert.Equal(FeedbackType.Security, result.Suggestions[0].Type);
        Assert.Equal(FeedbackType.Performance, result.Suggestions[1].Type);
        Assert.Equal(FeedbackType.BestPractice, result.Suggestions[2].Type);
    }

    [Fact]
    public async Task ReviewCodeAsync_WithInvalidJson_ReturnsEmptyReview()
    {
        // Arrange
        var code = "public class Test { }";
        var groqResponse = CreateGroqResponseWithJson("invalid json {{{");
        
        _mockGroqApiClient
            .Setup(x => x.SendChatCompletionAsync(It.IsAny<List<Message>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(groqResponse);

        // Act
        var result = await _service.ReviewCodeAsync(code);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result.Suggestions);
        Assert.Equal(0, result.OverallScore);
        Assert.Empty(result.SecurityIssues);
        Assert.Empty(result.PerformanceIssues);
    }

    [Fact]
    public async Task ReviewCodeAsync_WithEmptyChoices_ReturnsEmptyReview()
    {
        // Arrange
        var code = "public class Test { }";
        var groqResponse = new GroqResponse
        {
            Id = "test-id",
            Object = "chat.completion",
            Created = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
            Model = "llama-3.1-70b-versatile",
            Choices = new List<Choice>(),
            Usage = null
        };
        
        _mockGroqApiClient
            .Setup(x => x.SendChatCompletionAsync(It.IsAny<List<Message>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(groqResponse);

        // Act
        var result = await _service.ReviewCodeAsync(code);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result.Suggestions);
        Assert.Equal(0, result.OverallScore);
    }

    [Fact]
    public async Task ReviewCodeAsync_WithContext_PassesContextToPromptBuilder()
    {
        // Arrange
        var code = "public class Test { }";
        var context = "This is a test controller";
        var groqResponse = CreateValidGroqResponse();
        
        _mockGroqApiClient
            .Setup(x => x.SendChatCompletionAsync(It.IsAny<List<Message>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(groqResponse);

        // Act
        var result = await _service.ReviewCodeAsync(code, context);

        // Assert
        Assert.NotNull(result);
        _mockGroqApiClient.Verify(x => x.SendChatCompletionAsync(
            It.Is<List<Message>>(m => m.Any(msg => msg.Content.Contains(context))), 
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ReviewCodeAsync_OnFirstFailure_RetriesRequest()
    {
        // Arrange
        var code = "public class Test { }";
        var groqResponse = CreateValidGroqResponse();
        
        _mockGroqApiClient
            .SetupSequence(x => x.SendChatCompletionAsync(It.IsAny<List<Message>>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new HttpRequestException("Network error"))
            .ReturnsAsync(groqResponse);

        // Act
        var result = await _service.ReviewCodeAsync(code);

        // Assert
        Assert.NotNull(result);
        _mockGroqApiClient.Verify(x => x.SendChatCompletionAsync(It.IsAny<List<Message>>(), It.IsAny<CancellationToken>()), Times.Exactly(2));
    }

    [Fact]
    public async Task ReviewCodeAsync_OnSecondFailure_RetriesAgain()
    {
        // Arrange
        var code = "public class Test { }";
        var groqResponse = CreateValidGroqResponse();
        
        _mockGroqApiClient
            .SetupSequence(x => x.SendChatCompletionAsync(It.IsAny<List<Message>>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new HttpRequestException("Network error 1"))
            .ThrowsAsync(new HttpRequestException("Network error 2"))
            .ReturnsAsync(groqResponse);

        // Act
        var result = await _service.ReviewCodeAsync(code);

        // Assert
        Assert.NotNull(result);
        _mockGroqApiClient.Verify(x => x.SendChatCompletionAsync(It.IsAny<List<Message>>(), It.IsAny<CancellationToken>()), Times.Exactly(3));
    }

    [Fact]
    public async Task ReviewCodeAsync_AfterMaxRetries_ThrowsInvalidOperationException()
    {
        // Arrange
        var code = "public class Test { }";
        
        _mockGroqApiClient
            .Setup(x => x.SendChatCompletionAsync(It.IsAny<List<Message>>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new HttpRequestException("Network error"));

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _service.ReviewCodeAsync(code));
        Assert.Contains("failed after 3 attempts", exception.Message);
        _mockGroqApiClient.Verify(x => x.SendChatCompletionAsync(It.IsAny<List<Message>>(), It.IsAny<CancellationToken>()), Times.Exactly(3));
    }

    [Fact]
    public async Task ReviewCodeAsync_WithUnknownFeedbackType_DefaultsToBestPractice()
    {
        // Arrange
        var code = "public class Test { }";
        var jsonResponse = @"{
            ""suggestions"": [
                {
                    ""type"": ""UnknownType"",
                    ""message"": ""Some message"",
                    ""lineNumber"": 1,
                    ""codeExample"": ""Some example""
                }
            ],
            ""overallScore"": 80,
            ""securityIssues"": [],
            ""performanceIssues"": []
        }";
        
        var groqResponse = CreateGroqResponseWithJson(jsonResponse);
        
        _mockGroqApiClient
            .Setup(x => x.SendChatCompletionAsync(It.IsAny<List<Message>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(groqResponse);

        // Act
        var result = await _service.ReviewCodeAsync(code);

        // Assert
        Assert.Single(result.Suggestions);
        Assert.Equal(FeedbackType.BestPractice, result.Suggestions[0].Type);
    }

    [Fact]
    public async Task ReviewCodeAsync_WithTimeoutException_RetriesAndSucceeds()
    {
        // Arrange
        var code = "public class Test { }";
        var groqResponse = CreateValidGroqResponse();
        
        _mockGroqApiClient
            .SetupSequence(x => x.SendChatCompletionAsync(It.IsAny<List<Message>>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new TaskCanceledException("Request timeout"))
            .ReturnsAsync(groqResponse);

        // Act
        var result = await _service.ReviewCodeAsync(code);

        // Assert
        Assert.NotNull(result);
        _mockGroqApiClient.Verify(x => x.SendChatCompletionAsync(It.IsAny<List<Message>>(), It.IsAny<CancellationToken>()), Times.Exactly(2));
    }

    [Fact]
    public async Task ReviewCodeAsync_WithTimeoutExceptionOnAllRetries_ThrowsInvalidOperationException()
    {
        // Arrange
        var code = "public class Test { }";
        
        _mockGroqApiClient
            .Setup(x => x.SendChatCompletionAsync(It.IsAny<List<Message>>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new TaskCanceledException("Request timeout"));

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _service.ReviewCodeAsync(code));
        Assert.Contains("failed after 3 attempts", exception.Message);
        _mockGroqApiClient.Verify(x => x.SendChatCompletionAsync(It.IsAny<List<Message>>(), It.IsAny<CancellationToken>()), Times.Exactly(3));
    }

    [Fact]
    public async Task ReviewCodeAsync_WithCancellationToken_PassesToApiClient()
    {
        // Arrange
        var code = "public class Test { }";
        var groqResponse = CreateValidGroqResponse();
        var cts = new CancellationTokenSource();
        
        _mockGroqApiClient
            .Setup(x => x.SendChatCompletionAsync(It.IsAny<List<Message>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(groqResponse);

        // Act
        var result = await _service.ReviewCodeAsync(code, null, cts.Token);

        // Assert
        Assert.NotNull(result);
        _mockGroqApiClient.Verify(x => x.SendChatCompletionAsync(
            It.IsAny<List<Message>>(), 
            cts.Token), Times.Once);
    }

    [Fact]
    public async Task ReviewCodeAsync_WithNullSuggestions_ReturnsEmptyList()
    {
        // Arrange
        var code = "public class Test { }";
        var jsonResponse = @"{
            ""suggestions"": null,
            ""overallScore"": 85,
            ""securityIssues"": null,
            ""performanceIssues"": null
        }";
        
        var groqResponse = CreateGroqResponseWithJson(jsonResponse);
        
        _mockGroqApiClient
            .Setup(x => x.SendChatCompletionAsync(It.IsAny<List<Message>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(groqResponse);

        // Act
        var result = await _service.ReviewCodeAsync(code);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result.Suggestions);
        Assert.Empty(result.SecurityIssues);
        Assert.Empty(result.PerformanceIssues);
        Assert.Equal(85, result.OverallScore);
    }

    [Fact]
    public async Task ReviewCodeAsync_WithEmptyMessageContent_ReturnsEmptyReview()
    {
        // Arrange
        var code = "public class Test { }";
        var groqResponse = new GroqResponse
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
                        Content = string.Empty
                    },
                    FinishReason = "stop"
                }
            },
            Usage = null
        };
        
        _mockGroqApiClient
            .Setup(x => x.SendChatCompletionAsync(It.IsAny<List<Message>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(groqResponse);

        // Act
        var result = await _service.ReviewCodeAsync(code);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result.Suggestions);
        Assert.Equal(0, result.OverallScore);
    }

    [Fact]
    public async Task ReviewCodeAsync_WithMixedSecurityAndPerformanceIssues_ParsesCorrectly()
    {
        // Arrange
        var code = "public class Test { }";
        var jsonResponse = @"{
            ""suggestions"": [
                {
                    ""type"": ""Security"",
                    ""message"": ""Hardcoded credentials detected"",
                    ""lineNumber"": 5,
                    ""codeExample"": ""Use configuration or secrets manager""
                },
                {
                    ""type"": ""Performance"",
                    ""message"": ""Inefficient LINQ query"",
                    ""lineNumber"": 12,
                    ""codeExample"": ""Use Where().Select() instead of Select().Where()""
                }
            ],
            ""overallScore"": 55,
            ""securityIssues"": [""Hardcoded Credentials"", ""Missing Input Validation""],
            ""performanceIssues"": [""Inefficient Query"", ""Missing Index""]
        }";
        
        var groqResponse = CreateGroqResponseWithJson(jsonResponse);
        
        _mockGroqApiClient
            .Setup(x => x.SendChatCompletionAsync(It.IsAny<List<Message>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(groqResponse);

        // Act
        var result = await _service.ReviewCodeAsync(code);

        // Assert
        Assert.Equal(2, result.Suggestions.Count);
        Assert.Equal(2, result.SecurityIssues.Count);
        Assert.Equal(2, result.PerformanceIssues.Count);
        Assert.Equal(55, result.OverallScore);
        Assert.Contains("Hardcoded Credentials", result.SecurityIssues);
        Assert.Contains("Inefficient Query", result.PerformanceIssues);
    }

    [Fact]
    public async Task ReviewCodeAsync_WithNullFeedbackType_DefaultsToBestPractice()
    {
        // Arrange
        var code = "public class Test { }";
        var jsonResponse = @"{
            ""suggestions"": [
                {
                    ""type"": null,
                    ""message"": ""Some suggestion"",
                    ""lineNumber"": 1,
                    ""codeExample"": ""Example code""
                }
            ],
            ""overallScore"": 80,
            ""securityIssues"": [],
            ""performanceIssues"": []
        }";
        
        var groqResponse = CreateGroqResponseWithJson(jsonResponse);
        
        _mockGroqApiClient
            .Setup(x => x.SendChatCompletionAsync(It.IsAny<List<Message>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(groqResponse);

        // Act
        var result = await _service.ReviewCodeAsync(code);

        // Assert
        Assert.Single(result.Suggestions);
        Assert.Equal(FeedbackType.BestPractice, result.Suggestions[0].Type);
    }

    [Fact]
    public async Task ReviewCodeAsync_WithCaseInsensitiveFeedbackTypes_ParsesCorrectly()
    {
        // Arrange
        var code = "public class Test { }";
        var jsonResponse = @"{
            ""suggestions"": [
                {
                    ""type"": ""SECURITY"",
                    ""message"": ""Security issue"",
                    ""lineNumber"": 1,
                    ""codeExample"": ""Fix""
                },
                {
                    ""type"": ""performance"",
                    ""message"": ""Performance issue"",
                    ""lineNumber"": 2,
                    ""codeExample"": ""Fix""
                },
                {
                    ""type"": ""BestPractice"",
                    ""message"": ""Best practice issue"",
                    ""lineNumber"": 3,
                    ""codeExample"": ""Fix""
                },
                {
                    ""type"": ""ARCHITECTURE"",
                    ""message"": ""Architecture issue"",
                    ""lineNumber"": 4,
                    ""codeExample"": ""Fix""
                }
            ],
            ""overallScore"": 70,
            ""securityIssues"": [],
            ""performanceIssues"": []
        }";
        
        var groqResponse = CreateGroqResponseWithJson(jsonResponse);
        
        _mockGroqApiClient
            .Setup(x => x.SendChatCompletionAsync(It.IsAny<List<Message>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(groqResponse);

        // Act
        var result = await _service.ReviewCodeAsync(code);

        // Assert
        Assert.Equal(4, result.Suggestions.Count);
        Assert.Equal(FeedbackType.Security, result.Suggestions[0].Type);
        Assert.Equal(FeedbackType.Performance, result.Suggestions[1].Type);
        Assert.Equal(FeedbackType.BestPractice, result.Suggestions[2].Type);
        Assert.Equal(FeedbackType.Architecture, result.Suggestions[3].Type);
    }

    private GroqResponse CreateValidGroqResponse()
    {
        var jsonContent = @"{
            ""suggestions"": [],
            ""overallScore"": 100,
            ""securityIssues"": [],
            ""performanceIssues"": []
        }";

        return CreateGroqResponseWithJson(jsonContent);
    }

    private GroqResponse CreateGroqResponseWithJson(string jsonContent)
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
