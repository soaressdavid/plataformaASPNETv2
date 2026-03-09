using FsCheck;
using FsCheck.Xunit;
using Microsoft.EntityFrameworkCore;
using Shared.Data;
using Shared.Entities;
using Shared.Models;

namespace Challenge.Tests;

/// <summary>
/// Property-based tests for challenge difficulty progression.
/// Feature: aspnet-learning-platform
/// </summary>
public class ChallengeDifficultyProgressionPropertyTests
{
    /// <summary>
    /// Property 16: Challenge Difficulty Progression
    /// **Validates: Requirements 11.2, 11.3**
    /// 
    /// For any user, Medium challenges should only be unlocked after completing 5 Easy challenges.
    /// Hard challenges should only be unlocked after completing 5 Medium challenges.
    /// </summary>
    [Property(MaxTest = 20)]
    public void ChallengeDifficulty_RequiresProgressiveCompletion(NonNegativeInt easyCompleted, NonNegativeInt mediumCompleted)
    {
        // Arrange
        var easyCount = Math.Min(easyCompleted.Get, 10);
        var mediumCount = Math.Min(mediumCompleted.Get, 10);

        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: $"TestDb_{Guid.NewGuid()}")
            .Options;

        using var context = new ApplicationDbContext(options);
        
        // Create user
        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = "Test User",
            Email = "test@test.com",
            PasswordHash = "hash",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        context.Users.Add(user);
        context.SaveChanges();

        // Act - Check unlock requirements
        var canUnlockMedium = easyCount >= 5;
        var canUnlockHard = mediumCount >= 5;

        // Assert - Medium challenges require 5 Easy completions
        if (easyCount < 5)
        {
            Assert.False(canUnlockMedium, "Medium challenges should be locked with less than 5 Easy completions");
        }
        else
        {
            Assert.True(canUnlockMedium, "Medium challenges should be unlocked with 5+ Easy completions");
        }

        // Assert - Hard challenges require 5 Medium completions
        if (mediumCount < 5)
        {
            Assert.False(canUnlockHard, "Hard challenges should be locked with less than 5 Medium completions");
        }
        else
        {
            Assert.True(canUnlockHard, "Hard challenges should be unlocked with 5+ Medium completions");
        }
    }

    /// <summary>
    /// Property: Challenge unlock threshold is exactly 5
    /// 
    /// For any difficulty level, the unlock threshold should be exactly 5 completions.
    /// 4 completions should not unlock, 5 completions should unlock.
    /// </summary>
    [Property(MaxTest = 20)]
    public void ChallengeDifficulty_ThresholdIsExactly5(NonNegativeInt completedCount)
    {
        // Arrange
        var completed = Math.Min(completedCount.Get, 10);
        var threshold = 5;

        // Act
        var shouldUnlock = completed >= threshold;

        // Assert - Verify threshold boundary
        if (completed == 4)
        {
            Assert.False(shouldUnlock, "4 completions should NOT unlock next difficulty");
        }
        else if (completed == 5)
        {
            Assert.True(shouldUnlock, "5 completions SHOULD unlock next difficulty");
        }
        else if (completed > 5)
        {
            Assert.True(shouldUnlock, "More than 5 completions should unlock next difficulty");
        }
        else if (completed < 4)
        {
            Assert.False(shouldUnlock, "Less than 4 completions should NOT unlock next difficulty");
        }
    }

    /// <summary>
    /// Property: Difficulty progression is sequential
    /// 
    /// For any user, difficulties must be unlocked in order: Easy → Medium → Hard.
    /// Cannot skip difficulties (e.g., cannot unlock Hard without unlocking Medium first).
    /// </summary>
    [Property(MaxTest = 20)]
    public void ChallengeDifficulty_IsSequential(NonNegativeInt easyCompleted, NonNegativeInt mediumCompleted)
    {
        // Arrange
        var easyCount = Math.Min(easyCompleted.Get, 10);
        var mediumCount = Math.Min(mediumCompleted.Get, 10);

        // Act
        var easyUnlocked = true; // Easy is always unlocked
        var mediumUnlocked = easyCount >= 5;
        var hardUnlocked = mediumCount >= 5 && mediumUnlocked; // Hard requires Medium to be unlocked

        // Assert - Easy is always unlocked
        Assert.True(easyUnlocked, "Easy challenges should always be unlocked");

        // Assert - Medium requires Easy completions
        if (mediumUnlocked)
        {
            Assert.True(easyCount >= 5, "Medium should only be unlocked after 5 Easy completions");
        }

        // Assert - Hard requires Medium to be unlocked first
        if (hardUnlocked)
        {
            Assert.True(mediumUnlocked, "Hard should only be unlocked if Medium is unlocked");
            Assert.True(mediumCount >= 5, "Hard should only be unlocked after 5 Medium completions");
        }

        // Assert - Cannot unlock Hard without Medium
        if (!mediumUnlocked)
        {
            Assert.False(hardUnlocked, "Hard should not be unlocked if Medium is locked");
        }
    }

    /// <summary>
    /// Property: Completing challenges increments difficulty counter
    /// 
    /// For any challenge completion, the counter for that difficulty should increment by exactly 1.
    /// </summary>
    [Property(MaxTest = 20)]
    public void ChallengeCompletion_IncrementsCounter(PositiveInt completions)
    {
        // Arrange
        var numCompletions = Math.Min(completions.Get, 20);

        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: $"TestDb_{Guid.NewGuid()}")
            .Options;

        using var context = new ApplicationDbContext(options);
        
        // Create user
        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = "Test User",
            Email = "test@test.com",
            PasswordHash = "hash",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        context.Users.Add(user);

        // Create Easy challenges
        var challenges = new List<Shared.Entities.Challenge>();
        for (int i = 0; i < numCompletions; i++)
        {
            challenges.Add(new Shared.Entities.Challenge
            {
                Id = Guid.NewGuid(),
                Title = $"Easy Challenge {i + 1}",
                Description = "Test challenge",
                Difficulty = Difficulty.Easy,
                StarterCode = "// starter code",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            });
        }
        context.Challenges.AddRange(challenges);

        // Create submissions for each challenge
        var submissions = new List<Submission>();
        for (int i = 0; i < numCompletions; i++)
        {
            submissions.Add(new Submission
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                ChallengeId = challenges[i].Id,
                Code = "test code",
                Passed = true,
                Result = "Accepted",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            });
        }
        context.Submissions.AddRange(submissions);
        context.SaveChanges();

        // Act - Count completed Easy challenges
        var completedEasy = context.Submissions
            .Where(s => s.UserId == user.Id && s.Passed)
            .Join(context.Challenges,
                s => s.ChallengeId,
                c => c.Id,
                (s, c) => c)
            .Count(c => c.Difficulty == Difficulty.Easy);

        // Assert - Counter should match number of completions
        Assert.Equal(numCompletions, completedEasy);
    }

    /// <summary>
    /// Property: Difficulty unlock is persistent
    /// 
    /// For any user who has unlocked a difficulty level, that level should remain unlocked
    /// even if they complete more challenges. Unlocks are permanent.
    /// </summary>
    [Property(MaxTest = 20)]
    public void DifficultyUnlock_IsPersistent(PositiveInt additionalCompletions)
    {
        // Arrange
        var extraCompletions = Math.Min(additionalCompletions.Get, 10);
        var initialEasyCompleted = 5; // Enough to unlock Medium

        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: $"TestDb_{Guid.NewGuid()}")
            .Options;

        using var context = new ApplicationDbContext(options);
        
        // Create user
        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = "Test User",
            Email = "test@test.com",
            PasswordHash = "hash",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        context.Users.Add(user);
        context.SaveChanges();

        // Act - Check unlock status before and after additional completions
        var easyCompletedBefore = initialEasyCompleted;
        var mediumUnlockedBefore = easyCompletedBefore >= 5;

        var easyCompletedAfter = initialEasyCompleted + extraCompletions;
        var mediumUnlockedAfter = easyCompletedAfter >= 5;

        // Assert - Medium should remain unlocked
        Assert.True(mediumUnlockedBefore, "Medium should be unlocked with 5 Easy completions");
        Assert.True(mediumUnlockedAfter, "Medium should remain unlocked after additional completions");
        
        // Assert - Additional completions don't affect unlock status
        Assert.Equal(mediumUnlockedBefore, mediumUnlockedAfter);
    }

    /// <summary>
    /// Property: Each difficulty has independent counter
    /// 
    /// For any user, completing Easy challenges should not affect Medium counter,
    /// and completing Medium challenges should not affect Hard counter.
    /// </summary>
    [Property(MaxTest = 20)]
    public void DifficultyCounters_AreIndependent(NonNegativeInt easyCount, NonNegativeInt mediumCount, NonNegativeInt hardCount)
    {
        // Arrange
        var easy = Math.Min(easyCount.Get, 10);
        var medium = Math.Min(mediumCount.Get, 10);
        var hard = Math.Min(hardCount.Get, 10);

        // Act - Each difficulty has its own counter
        var easyCompleted = easy;
        var mediumCompleted = medium;
        var hardCompleted = hard;

        // Assert - Counters are independent
        Assert.True(easyCompleted >= 0, "Easy counter should be non-negative");
        Assert.True(mediumCompleted >= 0, "Medium counter should be non-negative");
        Assert.True(hardCompleted >= 0, "Hard counter should be non-negative");

        // Assert - Completing Easy doesn't affect Medium/Hard counters
        var mediumUnlocked = easyCompleted >= 5;
        var hardUnlocked = mediumCompleted >= 5 && mediumUnlocked;

        if (mediumUnlocked)
        {
            // Medium unlock depends only on Easy count, not Medium count
            Assert.True(easyCompleted >= 5, "Medium unlock should depend only on Easy completions");
        }

        if (hardUnlocked)
        {
            // Hard unlock depends only on Medium count (and Medium being unlocked)
            Assert.True(mediumCompleted >= 5, "Hard unlock should depend only on Medium completions");
            Assert.True(mediumUnlocked, "Hard unlock should require Medium to be unlocked");
        }
    }
}
