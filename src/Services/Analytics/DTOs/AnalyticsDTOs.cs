namespace Analytics.Service.DTOs;

public class TelemetryEvent
{
    public string EventType { get; set; } = string.Empty;
    public Guid UserId { get; set; }
    public Guid? ContentId { get; set; }
    public DateTime Timestamp { get; set; }
    public Dictionary<string, object> Properties { get; set; } = new();
}

public class LessonMetrics
{
    public Guid LessonId { get; set; }
    public int CompletionCount { get; set; }
    public long AverageCompletionTimeMs { get; set; }
    public double CompletionRate { get; set; }
    public int ViewCount { get; set; }
    public int DropOffCount { get; set; }
}

public class LowCompletionLesson
{
    public Guid LessonId { get; set; }
    public string LessonTitle { get; set; } = string.Empty;
    public int CompletionCount { get; set; }
    public double CompletionRate { get; set; }
    public int ViewCount { get; set; }
}

public class RetentionMetrics
{
    public DateTime CohortStartDate { get; set; }
    public int CohortSize { get; set; }
    public double Day7Retention { get; set; }
    public double Day30Retention { get; set; }
    public double Day90Retention { get; set; }
}

public class ActiveUsersMetrics
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int DailyActiveUsers { get; set; }
    public int WeeklyActiveUsers { get; set; }
    public int MonthlyActiveUsers { get; set; }
    public Dictionary<DateTime, int> DailyBreakdown { get; set; } = new();
}

public class ContentDropOffAnalysis
{
    public Guid ContentId { get; set; }
    public int TotalViews { get; set; }
    public int TotalDropOffs { get; set; }
    public double DropOffRate { get; set; }
    public List<DropOffPoint> DropOffPoints { get; set; } = new();
}

public class DropOffPoint
{
    public string Location { get; set; } = string.Empty;
    public int DropOffCount { get; set; }
    public double DropOffPercentage { get; set; }
    public long AverageTimeSpentMs { get; set; }
}

public class HighDropOffContent
{
    public Guid ContentId { get; set; }
    public string ContentTitle { get; set; } = string.Empty;
    public double DropOffRate { get; set; }
    public int TotalViews { get; set; }
    public int TotalDropOffs { get; set; }
}

public class UserActivityEvent
{
    public Guid UserId { get; set; }
    public DateTime Timestamp { get; set; }
    public string ActivityType { get; set; } = string.Empty;
    public Dictionary<string, object> Metadata { get; set; } = new();
}

public class ContentViewEvent
{
    public Guid UserId { get; set; }
    public Guid ContentId { get; set; }
    public DateTime ViewStartTime { get; set; }
    public DateTime? ViewEndTime { get; set; }
    public long? DurationMs { get; set; }
    public bool Completed { get; set; }
}

public class LessonCompletionEvent
{
    public Guid UserId { get; set; }
    public Guid LessonId { get; set; }
    public DateTime CompletionTime { get; set; }
    public long CompletionTimeMs { get; set; }
    public int Score { get; set; }
}

public class ChallengeCompletionEvent
{
    public Guid UserId { get; set; }
    public Guid ChallengeId { get; set; }
    public DateTime CompletionTime { get; set; }
    public long CompletionTimeMs { get; set; }
    public bool Passed { get; set; }
    public int Attempts { get; set; }
}

public class UserCohort
{
    public DateTime StartDate { get; set; }
    public List<Guid> UserIds { get; set; } = new();
    public int Size => UserIds.Count;
}

public class DashboardMetrics
{
    public int TotalUsers { get; set; }
    public int ActiveUsersToday { get; set; }
    public int ActiveUsersThisWeek { get; set; }
    public int ActiveUsersThisMonth { get; set; }
    public int TotalLessonsCompleted { get; set; }
    public int TotalChallengesCompleted { get; set; }
    public double AverageSessionDurationMinutes { get; set; }
    public double UserRetention7Day { get; set; }
    public double UserRetention30Day { get; set; }
    public List<PopularContent> MostPopularContent { get; set; } = new();
    public List<LowCompletionLesson> ProblematicContent { get; set; } = new();
}

public class PopularContent
{
    public Guid ContentId { get; set; }
    public string ContentTitle { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public int ViewCount { get; set; }
    public int CompletionCount { get; set; }
    public double CompletionRate { get; set; }
}

public class UserEngagementMetrics
{
    public Guid UserId { get; set; }
    public int TotalSessions { get; set; }
    public long TotalTimeSpentMs { get; set; }
    public int LessonsCompleted { get; set; }
    public int ChallengesCompleted { get; set; }
    public DateTime LastActiveDate { get; set; }
    public int CurrentStreak { get; set; }
    public double EngagementScore { get; set; }
}
