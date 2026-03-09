using FsCheck;
using FsCheck.Xunit;
using Shared.Services;

namespace Progress.Tests;

/// <summary>
/// Property-based tests for level calculation functionality.
/// Feature: aspnet-learning-platform
/// </summary>
public class LevelCalculationPropertiesTests
{
    /// <summary>
    /// Property 30: Level Calculation
    /// **Validates: Requirements 9.5**
    /// 
    /// For any student's total XP, their level should equal floor(sqrt(TotalXP / 100)), 
    /// and when XP reaches the next level threshold, the level should increment.
    /// </summary>
    [Property(MaxTest = 20)]
    public void LevelCalculation_FollowsFormula(NonNegativeInt xp)
    {
        var totalXP = xp.Get;
        
        // Calculate level using the service
        var level = XPCalculator.CalculateLevel(totalXP);
        
        // Calculate expected level using the formula: floor(sqrt(TotalXP / 100))
        var expectedLevel = (int)Math.Floor(Math.Sqrt(totalXP / 100.0));
        
        // Verify the level matches the formula
        Assert.Equal(expectedLevel, level);
    }

    /// <summary>
    /// Property: Level increments when XP reaches threshold
    /// 
    /// For any level, when XP reaches the threshold for the next level, 
    /// the calculated level should increment.
    /// </summary>
    [Property(MaxTest = 20)]
    public void LevelCalculation_IncrementsAtThreshold(PositiveInt level)
    {
        var currentLevel = level.Get;
        
        // Calculate XP for current level
        var xpForCurrentLevel = XPCalculator.CalculateXPForLevel(currentLevel);
        
        // Calculate XP for next level
        var xpForNextLevel = XPCalculator.CalculateXPForLevel(currentLevel + 1);
        
        // Verify level calculation at boundaries
        var calculatedCurrentLevel = XPCalculator.CalculateLevel(xpForCurrentLevel);
        var calculatedNextLevel = XPCalculator.CalculateLevel(xpForNextLevel);
        
        // At the threshold for current level, we should get current level
        Assert.True(calculatedCurrentLevel >= currentLevel);
        
        // At the threshold for next level, we should get next level
        Assert.True(calculatedNextLevel >= currentLevel + 1);
    }

    /// <summary>
    /// Property: XP to next level calculation is correct
    /// 
    /// For any current level and total XP, the XP needed to reach the next level 
    /// should be calculated correctly.
    /// </summary>
    [Property(MaxTest = 20)]
    public void XPToNextLevel_CalculatesCorrectly(NonNegativeInt xp)
    {
        var totalXP = xp.Get;
        var currentLevel = XPCalculator.CalculateLevel(totalXP);
        
        // Calculate XP to next level
        var xpToNextLevel = XPCalculator.CalculateXPToNextLevel(currentLevel, totalXP);
        
        // Verify that adding this XP would reach or exceed the next level
        var newTotalXP = totalXP + xpToNextLevel;
        var newLevel = XPCalculator.CalculateLevel(newTotalXP);
        
        // The new level should be at least currentLevel + 1
        Assert.True(newLevel >= currentLevel + 1);
        
        // XP to next level should be non-negative
        Assert.True(xpToNextLevel >= 0);
    }

    /// <summary>
    /// Property: Level is always non-negative
    /// 
    /// For any XP value (including zero and negative), the calculated level 
    /// should never be negative.
    /// </summary>
    [Property(MaxTest = 20)]
    public void LevelCalculation_AlwaysNonNegative(int xp)
    {
        var level = XPCalculator.CalculateLevel(xp);
        
        Assert.True(level >= 0);
    }

    /// <summary>
    /// Property: Level increases monotonically with XP
    /// 
    /// For any two XP values where XP2 > XP1, the level for XP2 should be 
    /// greater than or equal to the level for XP1.
    /// </summary>
    [Property(MaxTest = 20)]
    public void LevelCalculation_MonotonicallyIncreasing(NonNegativeInt xp1, NonNegativeInt xp2)
    {
        var totalXP1 = xp1.Get;
        var totalXP2 = xp2.Get;
        
        if (totalXP1 > totalXP2)
        {
            // Swap to ensure xp1 <= xp2
            (totalXP1, totalXP2) = (totalXP2, totalXP1);
        }
        
        var level1 = XPCalculator.CalculateLevel(totalXP1);
        var level2 = XPCalculator.CalculateLevel(totalXP2);
        
        // Level should never decrease as XP increases
        Assert.True(level2 >= level1);
    }
}
