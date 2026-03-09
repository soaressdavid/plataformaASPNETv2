namespace Shared.Services;

/// <summary>
/// Provides XP and level calculation functionality for the gamification system.
/// </summary>
public static class XPCalculator
{
    /// <summary>
    /// Calculates the level based on total XP using the formula: floor(sqrt(TotalXP / 100))
    /// </summary>
    /// <param name="totalXP">The total XP earned by the student</param>
    /// <returns>The calculated level</returns>
    public static int CalculateLevel(int totalXP)
    {
        if (totalXP < 0)
            return 0;
        
        return (int)Math.Floor(Math.Sqrt(totalXP / 100.0));
    }

    /// <summary>
    /// Calculates the XP required to reach the next level
    /// </summary>
    /// <param name="currentLevel">The current level</param>
    /// <param name="totalXP">The total XP earned</param>
    /// <returns>The XP needed to reach the next level</returns>
    public static int CalculateXPToNextLevel(int currentLevel, int totalXP)
    {
        var nextLevel = currentLevel + 1;
        var xpForNextLevel = nextLevel * nextLevel * 100;
        var xpNeeded = xpForNextLevel - totalXP;
        
        return Math.Max(0, xpNeeded);
    }

    /// <summary>
    /// Calculates the XP required to reach a specific level
    /// </summary>
    /// <param name="level">The target level</param>
    /// <returns>The total XP required to reach that level</returns>
    public static int CalculateXPForLevel(int level)
    {
        if (level <= 0)
            return 0;
        
        return level * level * 100;
    }
}
