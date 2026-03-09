using Xunit;
using Shared.Services;
using Shared.Entities;

namespace Gamification.Tests;

/// <summary>
/// Unit tests for XP calculation logic
/// Validates: Requirements 12.1, 12.2, 12.3, 12.4, 12.5, 12.11
/// </summary>
public class XPCalculatorTests
{
    [Fact]
    public void CalculateLevel_WithZeroXP_ReturnsLevelZero()
    {
        // Arrange
        int totalXP = 0;

        // Act
        int level = XPCalculator.CalculateLevel(totalXP);

        // Assert
        Assert.Equal(0, level);
    }

    [Theory]
    [InlineData(100, 1)]
    [InlineData(400, 2)]
    [InlineData(900, 3)]
    [InlineData(1600, 4)]
    [InlineData(2500, 5)]
    [InlineData(10000, 10)]
    public void CalculateLevel_WithVariousXP_ReturnsCorrectLevel(int totalXP, int expectedLevel)
    {
        // Act
        int level = XPCalculator.CalculateLevel(totalXP);

        // Assert
        Assert.Equal(expectedLevel, level);
    }

    [Fact]
    public void CalculateXPForNextLevel_ReturnsCorrectAmount()
    {
        // Arrange
        int currentLevel = 5;

        // Act
        int xpNeeded = XPCalculator.CalculateXPForNextLevel(currentLevel);

        // Assert
        // Level 6 requires 3600 XP total, Level 5 requires 2500 XP
        Assert.Equal(1100, xpNeeded);
    }

    [Theory]
    [InlineData(0, 1)]
    [InlineData(7, 1.1)]
    [InlineData(30, 1.2)]
    [InlineData(100, 1.5)]
    public void GetStreakMultiplier_WithVariousStreaks_ReturnsCorrectMultiplier(
        int streakDays, 
        double expectedMultiplier)
    {
        // Act
        decimal multiplier = XPCalculator.GetStreakMultiplier(streakDays);

        // Assert
        Assert.Equal((decimal)expectedMultiplier, multiplier);
    }

    [Fact]
    public void CalculateXPWithStreak_AppliesMultiplierCorrectly()
    {
        // Arrange
        int baseXP = 100;
        int streakDays = 30;

        // Act
        int finalXP = XPCalculator.CalculateXPWithStreak(baseXP, streakDays);

        // Assert
        // 100 * 1.2 = 120
        Assert.Equal(120, finalXP);
    }

    [Theory]
    [InlineData(ActivityType.LessonCompleted, 5)]
    [InlineData(ActivityType.EasyChallenge, 10)]
    [InlineData(ActivityType.MediumChallenge, 25)]
    [InlineData(ActivityType.HardChallenge, 50)]
    [InlineData(ActivityType.ProjectCompleted, 100)]
    public void GetBaseXP_ForActivityType_ReturnsCorrectAmount(
        ActivityType activityType, 
        int expectedXP)
    {
        // Act
        int xp = XPCalculator.GetBaseXP(activityType);

        // Assert
        Assert.Equal(expectedXP, xp);
    }

    [Fact]
    public void CalculateTimeAttackBonus_WithHighRemainingTime_ReturnsMaxBonus()
    {
        // Arrange
        int remainingSeconds = 700; // 11+ minutes

        // Act
        int bonus = XPCalculator.CalculateTimeAttackBonus(remainingSeconds);

        // Assert
        Assert.Equal(50, bonus);
    }

    [Theory]
    [InlineData(700, 50)]  // 11+ minutes
    [InlineData(400, 30)]  // 6-10 minutes
    [InlineData(200, 10)]  // <6 minutes
    public void CalculateTimeAttackBonus_WithVariousTimes_ReturnsCorrectBonus(
        int remainingSeconds, 
        int expectedBonus)
    {
        // Act
        int bonus = XPCalculator.CalculateTimeAttackBonus(remainingSeconds);

        // Assert
        Assert.Equal(expectedBonus, bonus);
    }

    [Fact]
    public void CalculateTotalXP_WithAllFactors_ReturnsCorrectTotal()
    {
        // Arrange
        int baseXP = 50; // Hard challenge
        int streakDays = 30; // 1.2x multiplier
        int timeBonus = 30;

        // Act
        int totalXP = XPCalculator.CalculateTotalXP(baseXP, streakDays, timeBonus);

        // Assert
        // (50 + 30) * 1.2 = 96
        Assert.Equal(96, totalXP);
    }

    [Fact]
    public void CalculateProgressToNextLevel_ReturnsCorrectPercentage()
    {
        // Arrange
        int currentXP = 2700; // Level 5 (2500 XP) + 200 XP
        int currentLevel = 5;

        // Act
        double progress = XPCalculator.CalculateProgressToNextLevel(currentXP, currentLevel);

        // Assert
        // Need 1100 XP for level 6, have 200 XP progress
        // 200 / 1100 = 18.18%
        Assert.Equal(18.18, progress, 2);
    }
}
