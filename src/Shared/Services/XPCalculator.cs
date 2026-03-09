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

    /// <summary>
    /// Gets the streak multiplier based on consecutive days
    /// </summary>
    public static decimal GetStreakMultiplier(int streakDays)
    {
        return streakDays switch
        {
            >= 100 => 1.5m,
            >= 30 => 1.2m,
            >= 7 => 1.1m,
            _ => 1.0m
        };
    }

    /// <summary>
    /// Calculates XP with streak multiplier applied
    /// </summary>
    public static int CalculateXPWithStreak(int baseXP, int streakDays)
    {
        var multiplier = GetStreakMultiplier(streakDays);
        return (int)(baseXP * multiplier);
    }

    /// <summary>
    /// Gets base XP for an activity type
    /// </summary>
    public static int GetBaseXP(ActivityType activityType)
    {
        return activityType switch
        {
            ActivityType.LessonCompleted => 5,
            ActivityType.EasyChallenge => 10,
            ActivityType.MediumChallenge => 25,
            ActivityType.HardChallenge => 50,
            ActivityType.ProjectCompleted => 100,
            ActivityType.HelpfulForumPost => 10,
            ActivityType.DailyMissionCompleted => 50,
            ActivityType.WeeklyMissionCompleted => 200,
            _ => 0
        };
    }

    /// <summary>
    /// Calculates time attack bonus based on remaining time
    /// </summary>
    public static int CalculateTimeAttackBonus(int remainingSeconds)
    {
        return remainingSeconds switch
        {
            >= 600 => 50,  // 10+ minutes
            >= 300 => 30,  // 5-10 minutes
            _ => 10        // < 5 minutes
        };
    }

    /// <summary>
    /// Calculates total XP with all factors (base + time bonus) * streak multiplier
    /// </summary>
    public static int CalculateTotalXP(int baseXP, int streakDays, int timeBonus = 0)
    {
        return CalculateXPWithStreak(baseXP + timeBonus, streakDays);
    }

    /// <summary>
    /// Calculates progress percentage to next level
    /// </summary>
    public static double CalculateProgressToNextLevel(int currentXP, int currentLevel)
    {
        var xpForCurrentLevel = CalculateXPForLevel(currentLevel);
        var xpForNextLevel = CalculateXPForLevel(currentLevel + 1);
        var xpNeeded = xpForNextLevel - xpForCurrentLevel;
        var xpProgress = currentXP - xpForCurrentLevel;
        
        if (xpNeeded == 0) return 100.0;
        
        return Math.Round((xpProgress * 100.0) / xpNeeded, 2);
    }

    /// <summary>
    /// Calculates XP for next level (alternative method name for compatibility)
    /// </summary>
    public static int CalculateXPForNextLevel(int currentLevel)
    {
        var xpForCurrentLevel = CalculateXPForLevel(currentLevel);
        var xpForNextLevel = CalculateXPForLevel(currentLevel + 1);
        return xpForNextLevel - xpForCurrentLevel;
    }
}

public enum ActivityType
{
    LessonCompleted,
    EasyChallenge,
    MediumChallenge,
    HardChallenge,
    ProjectCompleted,
    HelpfulForumPost,
    DailyMissionCompleted,
    WeeklyMissionCompleted
}
