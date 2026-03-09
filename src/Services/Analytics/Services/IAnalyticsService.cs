using Analytics.Service.DTOs;

namespace Analytics.Service.Services;

public interface IAnalyticsService
{
    // Event Tracking
    Task TrackEventAsync(TelemetryEvent telemetryEvent);
    Task TrackLessonCompletionAsync(LessonCompletionEvent completionEvent);
    Task TrackChallengeCompletionAsync(ChallengeCompletionEvent completionEvent);
    Task TrackContentViewAsync(ContentViewEvent viewEvent);
    Task TrackUserActivityAsync(UserActivityEvent activityEvent);

    // Lesson Metrics
    Task<LessonMetrics> GetLessonMetricsAsync(Guid lessonId);
    Task<List<LowCompletionLesson>> GetLowCompletionLessonsAsync(double threshold = 0.5);
    
    // Retention Analysis
    Task<List<RetentionMetrics>> GetRetentionMetricsAsync(DateTime startDate, DateTime endDate);
    Task<ActiveUsersMetrics> GetActiveUsersAsync(DateTime startDate, DateTime endDate);
    
    // Drop-Off Analysis
    Task<ContentDropOffAnalysis> GetContentDropOffAsync(Guid contentId);
    Task<List<HighDropOffContent>> GetHighDropOffContentAsync(double threshold = 0.5);
    
    // Dashboard Metrics
    Task<DashboardMetrics> GetDashboardMetricsAsync();
    Task<UserEngagementMetrics> GetUserEngagementMetricsAsync(Guid userId);
    
    // Cohort Analysis
    Task<List<UserCohort>> GetUserCohortsAsync(DateTime startDate, DateTime endDate);
    Task<double> CalculateCohortRetentionAsync(UserCohort cohort, int days);
}
