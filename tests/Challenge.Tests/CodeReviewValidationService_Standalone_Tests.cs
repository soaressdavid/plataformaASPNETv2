using System.Text.Json;
using Challenge.Services;
using Shared.DTOs;
using Shared.Entities;
using Xunit;

namespace Challenge.Tests;

/// <summary>
/// Standalone unit tests for CodeReviewValidationService
/// Validates: Requirement 9.2 - Bug Fixing exercises where users identify and fix errors
/// </summary>
public class CodeReviewValidationService_Standalone_Tests
{
    [Fact]
    public void ValidateCodeReview_AllBugsFound_Returns100Percent()
    {
        // Arrange
        var service = new CodeReviewValidationService();
        var challenge = new Shared.Entities.Challenge
        {
            Id = Guid.NewGuid(),
            Title = "Test",
            Description = "Test",
            Difficulty = Difficulty.Medium,
            StarterCode = "test",
            IsCodeReviewChallenge = true,
            ExpectedIssues = JsonSerializer.Serialize(new[]
            {
                new ExpectedBug { LineNumber = 5, Description = "Bug 1", Category = "Logic", Severity = "High" },
                new ExpectedBug { LineNumber = 10, Description = "Bug 2", Category = "Logic", Severity = "Medium" }
            })
        };

        var identifiedIssues = new List<IdentifiedIssue>
        {
            new() { LineNumber = 5, Description = "Found bug 1", Severity = "High" },
            new() { LineNumber = 10, Description = "Found bug 2", Severity = "Medium" }
        };

        // Act
        var result = service.ValidateCodeReview(challenge, identifiedIssues);

        // Assert
        Assert.True(result.Success);
        Assert.Equal(100, result.AccuracyPercentage);
        Assert.Equal(2, result.CorrectlyIdentified);
        Assert.Equal(0, result.MissedBugs);
    }

    [Fact]
    public void ValidateCodeReview_SomeBugsMissed_ReturnsPartialScore()
    {
        // Arrange
        var service = new CodeReviewValidationService();
        var challenge = new Shared.Entities.Challenge
        {
            Id = Guid.NewGuid(),
            Title = "Test",
            Description = "Test",
            Difficulty = Difficulty.Medium,
            StarterCode = "test",
            IsCodeReviewChallenge = true,
            ExpectedIssues = JsonSerializer.Serialize(new[]
            {
                new ExpectedBug { LineNumber = 5, Description = "Bug 1", Category = "Logic", Severity = "High" },
                new ExpectedBug { LineNumber = 10, Description = "Bug 2", Category = "Logic", Severity = "Medium" }
            })
        };

        var identifiedIssues = new List<IdentifiedIssue>
        {
            new() { LineNumber = 5, Description = "Found bug 1", Severity = "High" }
        };

        // Act
        var result = service.ValidateCodeReview(challenge, identifiedIssues);

        // Assert
        Assert.False(result.Success); // Less than 70%
        Assert.Equal(50, result.AccuracyPercentage);
        Assert.Equal(1, result.CorrectlyIdentified);
        Assert.Equal(1, result.MissedBugs);
    }
}
