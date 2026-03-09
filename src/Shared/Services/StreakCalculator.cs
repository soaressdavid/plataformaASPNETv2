namespace Shared.Services;

/// <summary>
/// Provides learning streak calculation functionality for the gamification system.
/// </summary>
public static class StreakCalculator
{
    /// <summary>
    /// Calculates the learning streak by counting consecutive days with activity.
    /// Uses timezone-aware date comparison.
    /// </summary>
    /// <param name="activityDates">List of activity timestamps in UTC</param>
    /// <param name="userTimeZone">User's timezone for date comparison</param>
    /// <returns>The number of consecutive days with activity</returns>
    public static int CalculateStreak(List<DateTime> activityDates, TimeZoneInfo userTimeZone)
    {
        if (activityDates == null || activityDates.Count == 0)
            return 0;

        // Convert all dates to user's timezone and get unique dates (day level)
        var uniqueDates = activityDates
            .Select(utcDate => TimeZoneInfo.ConvertTimeFromUtc(utcDate, userTimeZone).Date)
            .Distinct()
            .OrderByDescending(d => d)
            .ToList();

        if (uniqueDates.Count == 0)
            return 0;

        // Get today in user's timezone
        var today = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, userTimeZone).Date;

        // Check if the most recent activity is today or yesterday
        // If it's older than yesterday, streak is broken
        var mostRecentActivity = uniqueDates[0];
        var daysSinceLastActivity = (today - mostRecentActivity).Days;

        if (daysSinceLastActivity > 1)
            return 0; // Streak is broken

        // Count consecutive days
        int streak = 1;
        for (int i = 1; i < uniqueDates.Count; i++)
        {
            var currentDate = uniqueDates[i];
            var previousDate = uniqueDates[i - 1];
            var daysDifference = (previousDate - currentDate).Days;

            if (daysDifference == 1)
            {
                // Consecutive day
                streak++;
            }
            else
            {
                // Gap in streak, stop counting
                break;
            }
        }

        return streak;
    }

    /// <summary>
    /// Calculates the learning streak using UTC dates (assumes all dates are in UTC).
    /// </summary>
    /// <param name="activityDates">List of activity timestamps in UTC</param>
    /// <returns>The number of consecutive days with activity</returns>
    public static int CalculateStreak(List<DateTime> activityDates)
    {
        return CalculateStreak(activityDates, TimeZoneInfo.Utc);
    }

    /// <summary>
    /// Checks if a streak is still active (activity within the last 24 hours in user's timezone).
    /// </summary>
    /// <param name="lastActivityAt">The last activity timestamp in UTC</param>
    /// <param name="userTimeZone">User's timezone</param>
    /// <returns>True if the streak is still active, false otherwise</returns>
    public static bool IsStreakActive(DateTime lastActivityAt, TimeZoneInfo userTimeZone)
    {
        var today = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, userTimeZone).Date;
        var lastActivityDate = TimeZoneInfo.ConvertTimeFromUtc(lastActivityAt, userTimeZone).Date;
        var daysSinceLastActivity = (today - lastActivityDate).Days;

        // Streak is active if last activity was today or yesterday
        return daysSinceLastActivity <= 1;
    }
}
