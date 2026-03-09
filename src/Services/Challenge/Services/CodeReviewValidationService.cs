using System.Text.Json;
using Shared.DTOs;
using Shared.Entities;

namespace Challenge.Services;

/// <summary>
/// Service for validating code review challenge submissions
/// Validates: Requirement 9.2 - Bug Fixing exercises where users identify and fix errors
/// </summary>
public class CodeReviewValidationService
{
    /// <summary>
    /// Validates user's identified issues against expected bugs
    /// </summary>
    public CodeReviewValidationResponse ValidateCodeReview(
        Shared.Entities.Challenge challenge,
        List<IdentifiedIssue> identifiedIssues)
    {
        if (string.IsNullOrEmpty(challenge.ExpectedIssues))
        {
            throw new InvalidOperationException("Challenge does not have expected issues defined");
        }

        var expectedBugs = JsonSerializer.Deserialize<List<ExpectedBug>>(challenge.ExpectedIssues)
            ?? new List<ExpectedBug>();

        var bugResults = new List<BugValidationResult>();
        var correctlyIdentified = 0;

        // Check each expected bug
        foreach (var expectedBug in expectedBugs)
        {
            // Find if user identified this bug (within 2 lines tolerance)
            var userIssue = identifiedIssues.FirstOrDefault(i =>
                Math.Abs(i.LineNumber - expectedBug.LineNumber) <= 2);

            var wasIdentified = userIssue != null;
            if (wasIdentified)
            {
                correctlyIdentified++;
            }

            bugResults.Add(new BugValidationResult
            {
                LineNumber = expectedBug.LineNumber,
                ExpectedDescription = expectedBug.Description,
                WasIdentified = wasIdentified,
                UserDescription = userIssue?.Description
            });
        }

        // Calculate false positives (issues identified that aren't real bugs)
        var falsePositives = 0;
        foreach (var identifiedIssue in identifiedIssues)
        {
            var isRealBug = expectedBugs.Any(eb =>
                Math.Abs(eb.LineNumber - identifiedIssue.LineNumber) <= 2);
            
            if (!isRealBug)
            {
                falsePositives++;
            }
        }

        var missedBugs = expectedBugs.Count - correctlyIdentified;
        var accuracyPercentage = expectedBugs.Count > 0
            ? (double)correctlyIdentified / expectedBugs.Count * 100
            : 0;

        // Calculate XP based on accuracy
        var xpAwarded = CalculateXP(challenge.Difficulty, accuracyPercentage, falsePositives);

        return new CodeReviewValidationResponse
        {
            Success = accuracyPercentage >= 70, // 70% accuracy required to pass
            TotalExpectedBugs = expectedBugs.Count,
            CorrectlyIdentified = correctlyIdentified,
            MissedBugs = missedBugs,
            FalsePositives = falsePositives,
            AccuracyPercentage = Math.Round(accuracyPercentage, 2),
            XpAwarded = xpAwarded,
            BugResults = bugResults
        };
    }

    /// <summary>
    /// Calculates XP based on difficulty, accuracy, and false positives
    /// </summary>
    private int CalculateXP(Difficulty difficulty, double accuracyPercentage, int falsePositives)
    {
        // Base XP by difficulty
        var baseXP = difficulty switch
        {
            Difficulty.Easy => 10,
            Difficulty.Medium => 25,
            Difficulty.Hard => 50,
            _ => 10
        };

        // Apply accuracy multiplier
        var accuracyMultiplier = accuracyPercentage / 100.0;
        var xp = (int)(baseXP * accuracyMultiplier);

        // Penalize false positives (reduce XP by 10% per false positive, max 50% reduction)
        var falsePositivePenalty = Math.Min(falsePositives * 0.1, 0.5);
        xp = (int)(xp * (1 - falsePositivePenalty));

        // Minimum 1 XP if any bugs were correctly identified
        return Math.Max(xp, accuracyPercentage > 0 ? 1 : 0);
    }
}
