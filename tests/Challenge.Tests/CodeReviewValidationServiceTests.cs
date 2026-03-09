using System.Text.Json;
using Challenge.Services;
using Shared.DTOs;
using Shared.Entities;
using Xunit;

namespace Challenge.Tests;

/// <summary>
/// Unit tests for CodeReviewValidationService
/// Validates: Requirement 9.2 - Bug Fixing exercises where users identify and fix errors
/// </summary>
public class CodeReviewValidationServiceTests
{
    private readonly CodeReviewValidationService _service;

    public CodeReviewValidationServiceTests()
    {
        _service = new CodeReviewValidationService();
    }

    [Fact]
    public void ValidateCodeReview_AllBugsIdentified_ReturnsFullScore()
    {
        // Arrange
        var challenge = CreateChallenge(new[]
        {
            new ExpectedBug { LineNumber = 5, Description = "Null reference", Category = "NullReference", Severity = "High" },
            new ExpectedBug { LineNumber = 12, Description = "Off-by-one error", Category = "LogicError", Severity = "Medium" }
        });

        var identifiedIssues = new List<IdentifiedIssue>
        {
            new() { LineNumber = 5, Description = "Missing null check", Severity = "High" },
            new() { LineNumber = 12, Description = "Loop condition error", Severity = "Medium" }
        };

        // Act
        var result = _service.ValidateCodeReview(challenge, identifiedIssues);

        // Assert
        Assert.True(result.Success);
        Assert.Equal(2, result.TotalExpectedBugs);
        Assert.Equal(2, result.CorrectlyIdentified);
        Assert.Equal(0, result.MissedBugs);
        Assert.Equal(0, result.FalsePositives);
        Assert.Equal(100, result.AccuracyPercentage);
        Assert.True(result.XpAwarded > 0);
    }

    [Fact]
    public void ValidateCodeReview_SomeBugsMissed_ReturnsPartialScore()
    {
        // Arrange
        var challenge = CreateChallenge(new[]
        {
            new ExpectedBug { LineNumber = 5, Description = "Null reference", Category = "NullReference", Severity = "High" },
            new ExpectedBug { LineNumber = 12, Description = "Off-by-one error", Category = "LogicError", Severity = "Medium" },
            new ExpectedBug { LineNumber = 18, Description = "SQL injection", Category = "SecurityIssue", Severity = "High" }
        });

        var identifiedIssues = new List<IdentifiedIssue>
        {
            new() { LineNumber = 5, Description = "Missing null check", Severity = "High" }
        };

        // Act
        var result = _service.ValidateCodeReview(challenge, identifiedIssues);

        // Assert
        Assert.False(result.Success); // Less than 70% accuracy
        Assert.Equal(3, result.TotalExpectedBugs);
        Assert.Equal(1, result.CorrectlyIdentified);
        Assert.Equal(2, result.MissedBugs);
        Assert.Equal(0, result.FalsePositives);
        Assert.Equal(33.33, result.AccuracyPercentage, 2);
    }

    [Fact]
    public void ValidateCodeReview_WithFalsePositives_PenalizesScore()
    {
        // Arrange
        var challenge = CreateChallenge(new[]
        {
            new ExpectedBug { LineNumber = 5, Description = "Null reference", Category = "NullReference", Severity = "High" }
        });

        var identifiedIssues = new List<IdentifiedIssue>
        {
            new() { LineNumber = 5, Description = "Missing null check", Severity = "High" },
            new() { LineNumber = 10, Description = "Not a real bug", Severity = "Low" },
            new() { LineNumber = 15, Description = "Another false positive", Severity = "Low" }
        };

        // Act
        var result = _service.ValidateCodeReview(challenge, identifiedIssues);

        // Assert
        Assert.True(result.Success); // 100% accuracy on real bugs
        Assert.Equal(1, result.CorrectlyIdentified);
        Assert.Equal(2, result.FalsePositives);
        Assert.Equal(100, result.AccuracyPercentage);
        // XP should be reduced due to false positives
    }

    [Fact]
    public void ValidateCodeReview_LineNumberTolerance_AcceptsNearbyLines()
    {
        // Arrange
        var challenge = CreateChallenge(new[]
        {
            new ExpectedBug { LineNumber = 10, Description = "Bug at line 10", Category = "LogicError", Severity = "Medium" }
        });

        var identifiedIssues = new List<IdentifiedIssue>
        {
            new() { LineNumber = 11, Description = "Found bug near line 10", Severity = "Medium" } // Within 2 lines tolerance
        };

        // Act
        var result = _service.ValidateCodeReview(challenge, identifiedIssues);

        // Assert
        Assert.True(result.Success);
        Assert.Equal(1, result.CorrectlyIdentified);
        Assert.Equal(100, result.AccuracyPercentage);
    }

    [Fact]
    public void ValidateCodeReview_NoBugsIdentified_ReturnsZeroScore()
    {
        // Arrange
        var challenge = CreateChallenge(new[]
        {
            new ExpectedBug { LineNumber = 5, Description = "Null reference", Category = "NullReference", Severity = "High" }
        });

        var identifiedIssues = new List<IdentifiedIssue>();

        // Act
        var result = _service.ValidateCodeReview(challenge, identifiedIssues);

        // Assert
        Assert.False(result.Success);
        Assert.Equal(0, result.CorrectlyIdentified);
        Assert.Equal(1, result.MissedBugs);
        Assert.Equal(0, result.AccuracyPercentage);
        Assert.Equal(0, result.XpAwarded);
    }

    [Fact]
    public void ValidateCodeReview_DifficultyAffectsXP_HardGivesMoreXP()
    {
        // Arrange
        var easyChallenge = CreateChallenge(
            new[] { new ExpectedBug { LineNumber = 5, Description = "Bug", Category = "LogicError", Severity = "Low" } },
            Difficulty.Easy
        );

        var hardChallenge = CreateChallenge(
            new[] { new ExpectedBug { LineNumber = 5, Description = "Bug", Category = "LogicError", Severity = "High" } },
            Difficulty.Hard
        );

        var identifiedIssues = new List<IdentifiedIssue>
        {
            new() { LineNumber = 5, Description = "Found bug", Severity = "Medium" }
        };

        // Act
        var easyResult = _service.ValidateCodeReview(easyChallenge, identifiedIssues);
        var hardResult = _service.ValidateCodeReview(hardChallenge, identifiedIssues);

        // Assert
        Assert.True(hardResult.XpAwarded > easyResult.XpAwarded);
    }

    [Fact]
    public void ValidateCodeReview_70PercentAccuracy_PassesChallenge()
    {
        // Arrange
        var challenge = CreateChallenge(new[]
        {
            new ExpectedBug { LineNumber = 5, Description = "Bug 1", Category = "LogicError", Severity = "Medium" },
            new ExpectedBug { LineNumber = 10, Description = "Bug 2", Category = "LogicError", Severity = "Medium" },
            new ExpectedBug { LineNumber = 15, Description = "Bug 3", Category = "LogicError", Severity = "Medium" },
            new ExpectedBug { LineNumber = 20, Description = "Bug 4", Category = "LogicError", Severity = "Medium" },
            new ExpectedBug { LineNumber = 25, Description = "Bug 5", Category = "LogicError", Severity = "Medium" },
            new ExpectedBug { LineNumber = 30, Description = "Bug 6", Category = "LogicError", Severity = "Medium" },
            new ExpectedBug { LineNumber = 35, Description = "Bug 7", Category = "LogicError", Severity = "Medium" },
            new ExpectedBug { LineNumber = 40, Description = "Bug 8", Category = "LogicError", Severity = "Medium" },
            new ExpectedBug { LineNumber = 45, Description = "Bug 9", Category = "LogicError", Severity = "Medium" },
            new ExpectedBug { LineNumber = 50, Description = "Bug 10", Category = "LogicError", Severity = "Medium" }
        });

        // Identify exactly 7 out of 10 bugs (70%)
        var identifiedIssues = new List<IdentifiedIssue>
        {
            new() { LineNumber = 5, Description = "Found", Severity = "Medium" },
            new() { LineNumber = 10, Description = "Found", Severity = "Medium" },
            new() { LineNumber = 15, Description = "Found", Severity = "Medium" },
            new() { LineNumber = 20, Description = "Found", Severity = "Medium" },
            new() { LineNumber = 25, Description = "Found", Severity = "Medium" },
            new() { LineNumber = 30, Description = "Found", Severity = "Medium" },
            new() { LineNumber = 35, Description = "Found", Severity = "Medium" }
        };

        // Act
        var result = _service.ValidateCodeReview(challenge, identifiedIssues);

        // Assert
        Assert.True(result.Success); // Exactly 70% should pass
        Assert.Equal(70, result.AccuracyPercentage);
    }

    [Fact]
    public void ValidateCodeReview_InvalidChallenge_ThrowsException()
    {
        // Arrange
        var challenge = new Shared.Entities.Challenge
        {
            Id = Guid.NewGuid(),
            Title = "Test",
            Difficulty = Difficulty.Easy,
            ExpectedIssues = null // No expected issues
        };

        var identifiedIssues = new List<IdentifiedIssue>();

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() =>
            _service.ValidateCodeReview(challenge, identifiedIssues)
        );
    }

    private Shared.Entities.Challenge CreateChallenge(ExpectedBug[] bugs, Difficulty difficulty = Difficulty.Medium)
    {
        return new Shared.Entities.Challenge
        {
            Id = Guid.NewGuid(),
            Title = "Test Code Review Challenge",
            Description = "Find the bugs",
            Difficulty = difficulty,
            IsCodeReviewChallenge = true,
            ExpectedIssues = JsonSerializer.Serialize(bugs)
        };
    }
}
