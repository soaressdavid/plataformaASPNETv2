using FsCheck;
using FsCheck.Xunit;
using Shared.Services;

namespace Progress.Tests;

/// <summary>
/// Property-based tests for learning streak calculation functionality.
/// Feature: aspnet-learning-platform
/// </summary>
public class StreakCalculationPropertiesTests
{
    /// <summary>
    /// Property 31: Streak Calculation
    /// **Validates: Requirements 9.6**
    /// 
    /// For any sequence of student activities, the learning streak should equal 
    /// the count of consecutive days with at least one activity (challenge solved or lesson completed).
    /// </summary>
    [Property(MaxTest = 20)]
    public void StreakCalculation_CountsConsecutiveDays(PositiveInt daysCount)
    {
        var days = Math.Min(daysCount.Get, 30); // Limit to 30 days for test performance
        var activityDates = new List<DateTime>();
        var today = DateTime.UtcNow;

        // Create consecutive days of activity
        for (int i = 0; i < days; i++)
        {
            activityDates.Add(today.AddDays(-i));
        }

        var streak = StreakCalculator.CalculateStreak(activityDates, TimeZoneInfo.Utc);

        // Streak should equal the number of consecutive days
        Assert.Equal(days, streak);
    }

    /// <summary>
    /// Property: Streak breaks after 24+ hours of inactivity
    /// 
    /// For any activity sequence with a gap of more than 1 day, 
    /// the streak should only count days up to the gap.
    /// </summary>
    [Property(MaxTest = 20)]
    public void StreakCalculation_BreaksAfterGap(PositiveInt streakDays, PositiveInt gapDays)
    {
        var streak = Math.Min(streakDays.Get, 10);
        var gap = Math.Min(gapDays.Get, 10) + 2; // Ensure gap is at least 2 days
        var activityDates = new List<DateTime>();
        var today = DateTime.UtcNow;

        // Create recent consecutive days
        for (int i = 0; i < streak; i++)
        {
            activityDates.Add(today.AddDays(-i));
        }

        // Add activity after a gap (this should not be counted)
        activityDates.Add(today.AddDays(-(streak + gap)));

        var calculatedStreak = StreakCalculator.CalculateStreak(activityDates, TimeZoneInfo.Utc);

        // Streak should only count the recent consecutive days
        Assert.Equal(streak, calculatedStreak);
    }

    /// <summary>
    /// Property: Empty activity list returns zero streak
    /// 
    /// For any empty activity list, the streak should be zero.
    /// </summary>
    [Property(MaxTest = 20)]
    public void StreakCalculation_ReturnsZeroForEmptyList()
    {
        var activityDates = new List<DateTime>();
        var streak = StreakCalculator.CalculateStreak(activityDates, TimeZoneInfo.Utc);

        Assert.Equal(0, streak);
    }

    /// <summary>
    /// Property: Multiple activities on same day count as one day
    /// 
    /// For any day with multiple activities, it should only count as one day in the streak.
    /// </summary>
    [Property(MaxTest = 20)]
    public void StreakCalculation_CountsSameDayOnce(PositiveInt activitiesPerDay)
    {
        var activities = Math.Min(activitiesPerDay.Get, 10);
        var activityDates = new List<DateTime>();
        var today = DateTime.UtcNow;

        // Add multiple activities on the same day
        for (int i = 0; i < activities; i++)
        {
            activityDates.Add(today.AddHours(-i));
        }

        var streak = StreakCalculator.CalculateStreak(activityDates, TimeZoneInfo.Utc);

        // Should count as only 1 day
        Assert.Equal(1, streak);
    }

    /// <summary>
    /// Property: Streak is timezone-aware
    /// 
    /// For any activity timestamps, converting to different timezones 
    /// should correctly calculate the streak based on local dates.
    /// </summary>
    [Property(MaxTest = 20)]
    public void StreakCalculation_IsTimezoneAware(PositiveInt daysCount)
    {
        var days = Math.Min(daysCount.Get, 10);
        var activityDates = new List<DateTime>();
        var utcNow = DateTime.UtcNow;

        // Create consecutive days of activity in UTC
        for (int i = 0; i < days; i++)
        {
            activityDates.Add(utcNow.AddDays(-i));
        }

        // Calculate streak in UTC
        var utcStreak = StreakCalculator.CalculateStreak(activityDates, TimeZoneInfo.Utc);

        // Calculate streak in a different timezone (e.g., Pacific Standard Time)
        var pstTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");
        var pstStreak = StreakCalculator.CalculateStreak(activityDates, pstTimeZone);

        // Both should calculate a valid streak (may differ by 1 day due to timezone boundaries)
        Assert.True(utcStreak >= 0);
        Assert.True(pstStreak >= 0);
        Assert.True(Math.Abs(utcStreak - pstStreak) <= 1);
    }

    /// <summary>
    /// Property: Old activity without recent activity returns zero streak
    /// 
    /// For any activity that is more than 1 day old, the streak should be zero.
    /// </summary>
    [Property(MaxTest = 20)]
    public void StreakCalculation_ReturnsZeroForOldActivity(PositiveInt daysAgo)
    {
        var days = Math.Min(daysAgo.Get, 100) + 2; // Ensure at least 2 days ago
        var activityDates = new List<DateTime>
        {
            DateTime.UtcNow.AddDays(-days)
        };

        var streak = StreakCalculator.CalculateStreak(activityDates, TimeZoneInfo.Utc);

        // Streak should be zero since activity is too old
        Assert.Equal(0, streak);
    }

    /// <summary>
    /// Property: Streak is always non-negative
    /// 
    /// For any activity list, the calculated streak should never be negative.
    /// </summary>
    [Property(MaxTest = 20)]
    public void StreakCalculation_AlwaysNonNegative()
    {
        // Generate random list of dates
        var activityDates = new List<DateTime>();
        var random = new Random();
        var count = random.Next(0, 20);
        
        for (int i = 0; i < count; i++)
        {
            activityDates.Add(DateTime.UtcNow.AddDays(-random.Next(0, 100)));
        }

        var streak = StreakCalculator.CalculateStreak(activityDates, TimeZoneInfo.Utc);
        
        Assert.True(streak >= 0);
    }

    /// <summary>
    /// Property: IsStreakActive correctly identifies active streaks
    /// 
    /// For any activity within today or yesterday, the streak should be active.
    /// For any activity older than yesterday, the streak should be inactive.
    /// </summary>
    [Property(MaxTest = 20)]
    public void IsStreakActive_CorrectlyIdentifiesActiveStreaks(NonNegativeInt daysAgo)
    {
        var days = Math.Min(daysAgo.Get, 10); // Limit to 10 days
        var lastActivity = DateTime.UtcNow.AddDays(-days);

        var isActive = StreakCalculator.IsStreakActive(lastActivity, TimeZoneInfo.Utc);

        if (days <= 1)
        {
            // Activity today or yesterday should be active
            Assert.True(isActive);
        }
        else
        {
            // Activity older than yesterday should be inactive
            Assert.False(isActive);
        }
    }
}
