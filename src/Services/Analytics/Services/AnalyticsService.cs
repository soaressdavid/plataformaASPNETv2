using Analytics.Service.DTOs;

namespace Analytics.Service.Services;

public class AnalyticsService : IAnalyticsService
{
    private readonly ILogger<AnalyticsService> _logger;
    
    // In-memory storage (replace with time-series database like InfluxDB or TimescaleDB in production)
    private static readonly List<TelemetryEvent> _events = new();
    private static readonly List<LessonCompletionEvent> _lessonCompletions = new();
    private static readonly List<ChallengeCompletionEvent> _challengeCompletions = new();
    private static readonly List<ContentViewEvent> _contentViews = new();
    private static readonly List<UserActivityEvent> _userActivities = new();

    public AnalyticsService(ILogger<AnalyticsService> logger)
    {
        _logger = logger;
    }

    #region Event Tracking

    public Task TrackEventAsync(TelemetryEvent telemetryEvent)
    {
        telemetryEvent.Timestamp = DateTime.UtcNow;
        _events.Add(telemetryEvent);
        
        _logger.LogInformation(
            "Tracked event: {EventType} for user {UserId}",
            telemetryEvent.EventType,
            telemetryEvent.UserId);
        
        return Task.CompletedTask;
    }

    public Task TrackLessonCompletionAsync(LessonCompletionEvent completionEvent)
    {
        completionEvent.CompletionTime = DateTime.UtcNow;
        _lessonCompletions.Add(completionEvent);
        
        _logger.LogInformation(
            "Tracked lesson completion: Lesson {LessonId} by user {UserId} in {TimeMs}ms",
            completionEvent.LessonId,
            completionEvent.UserId,
            completionEvent.CompletionTimeMs);
        
        return Task.CompletedTask;
    }

    public Task TrackChallengeCompletionAsync(ChallengeCompletionEvent completionEvent)
    {
        completionEvent.CompletionTime = DateTime.UtcNow;
        _challengeCompletions.Add(completionEvent);
        
        _logger.LogInformation(
            "Tracked challenge completion: Challenge {ChallengeId} by user {UserId}, Passed: {Passed}",
            completionEvent.ChallengeId,
            completionEvent.UserId,
            completionEvent.Passed);
        
        return Task.CompletedTask;
    }

    public Task TrackContentViewAsync(ContentViewEvent viewEvent)
    {
        if (viewEvent.ViewStartTime == default)
        {
            viewEvent.ViewStartTime = DateTime.UtcNow;
        }
        
        _contentViews.Add(viewEvent);
        
        _logger.LogInformation(
            "Tracked content view: Content {ContentId} by user {UserId}",
            viewEvent.ContentId,
            viewEvent.UserId);
        
        return Task.CompletedTask;
    }

    public Task TrackUserActivityAsync(UserActivityEvent activityEvent)
    {
        activityEvent.Timestamp = DateTime.UtcNow;
        _userActivities.Add(activityEvent);
        
        _logger.LogInformation(
            "Tracked user activity: {ActivityType} by user {UserId}",
            activityEvent.ActivityType,
            activityEvent.UserId);
        
        return Task.CompletedTask;
    }

    #endregion

    #region Lesson Metrics

    public Task<LessonMetrics> GetLessonMetricsAsync(Guid lessonId)
    {
        var completions = _lessonCompletions.Where(c => c.LessonId == lessonId).ToList();
        var views = _contentViews.Where(v => v.ContentId == lessonId).ToList();
        var dropOffs = views.Count(v => !v.Completed);

        var metrics = new LessonMetrics
        {
            LessonId = lessonId,
            CompletionCount = completions.Count,
            AverageCompletionTimeMs = completions.Any() 
                ? (long)completions.Average(c => c.CompletionTimeMs) 
                : 0,
            ViewCount = views.Count,
            DropOffCount = dropOffs,
            CompletionRate = views.Count > 0 
                ? (double)completions.Count / views.Count 
                : 0
        };

        return Task.FromResult(metrics);
    }

    public async Task<List<LowCompletionLesson>> GetLowCompletionLessonsAsync(double threshold = 0.5)
    {
        var lessonIds = _contentViews.Select(v => v.ContentId).Distinct();
        var lowCompletionLessons = new List<LowCompletionLesson>();

        foreach (var lessonId in lessonIds)
        {
            var metrics = await GetLessonMetricsAsync(lessonId);
            
            if (metrics.CompletionRate < threshold && metrics.ViewCount >= 10)
            {
                lowCompletionLessons.Add(new LowCompletionLesson
                {
                    LessonId = lessonId,
                    LessonTitle = $"Lesson {lessonId}",
                    CompletionCount = metrics.CompletionCount,
                    CompletionRate = metrics.CompletionRate,
                    ViewCount = metrics.ViewCount
                });
            }
        }

        return lowCompletionLessons.OrderBy(l => l.CompletionRate).ToList();
    }

    #endregion

    #region Retention Analysis

    public async Task<List<RetentionMetrics>> GetRetentionMetricsAsync(DateTime startDate, DateTime endDate)
    {
        var cohorts = await GetUserCohortsAsync(startDate, endDate);
        var retentionMetrics = new List<RetentionMetrics>();

        foreach (var cohort in cohorts)
        {
            var day7Retention = await CalculateCohortRetentionAsync(cohort, 7);
            var day30Retention = await CalculateCohortRetentionAsync(cohort, 30);
            var day90Retention = await CalculateCohortRetentionAsync(cohort, 90);

            retentionMetrics.Add(new RetentionMetrics
            {
                CohortStartDate = cohort.StartDate,
                CohortSize = cohort.Size,
                Day7Retention = day7Retention,
                Day30Retention = day30Retention,
                Day90Retention = day90Retention
            });
        }

        return retentionMetrics;
    }

    public Task<ActiveUsersMetrics> GetActiveUsersAsync(DateTime startDate, DateTime endDate)
    {
        var activitiesInRange = _userActivities
            .Where(a => a.Timestamp >= startDate && a.Timestamp <= endDate)
            .ToList();

        var uniqueUsers = activitiesInRange.Select(a => a.UserId).Distinct().ToList();
        
        var dailyBreakdown = activitiesInRange
            .GroupBy(a => a.Timestamp.Date)
            .ToDictionary(
                g => g.Key,
                g => g.Select(a => a.UserId).Distinct().Count()
            );

        var now = DateTime.UtcNow;
        var todayStart = now.Date;
        var weekStart = now.AddDays(-7);
        var monthStart = now.AddDays(-30);

        var metrics = new ActiveUsersMetrics
        {
            StartDate = startDate,
            EndDate = endDate,
            DailyActiveUsers = _userActivities
                .Where(a => a.Timestamp >= todayStart)
                .Select(a => a.UserId)
                .Distinct()
                .Count(),
            WeeklyActiveUsers = _userActivities
                .Where(a => a.Timestamp >= weekStart)
                .Select(a => a.UserId)
                .Distinct()
                .Count(),
            MonthlyActiveUsers = _userActivities
                .Where(a => a.Timestamp >= monthStart)
                .Select(a => a.UserId)
                .Distinct()
                .Count(),
            DailyBreakdown = dailyBreakdown
        };

        return Task.FromResult(metrics);
    }

    #endregion

    #region Drop-Off Analysis

    public Task<ContentDropOffAnalysis> GetContentDropOffAsync(Guid contentId)
    {
        var views = _contentViews.Where(v => v.ContentId == contentId).ToList();
        var dropOffs = views.Where(v => !v.Completed).ToList();

        // Analyze drop-off points (simplified - in production, track specific sections)
        var dropOffPoints = new List<DropOffPoint>();
        
        if (dropOffs.Any())
        {
            var earlyDropOffs = dropOffs.Where(d => d.DurationMs < 60000).ToList();
            var midDropOffs = dropOffs.Where(d => d.DurationMs >= 60000 && d.DurationMs < 300000).ToList();
            var lateDropOffs = dropOffs.Where(d => d.DurationMs >= 300000).ToList();

            if (earlyDropOffs.Any())
            {
                dropOffPoints.Add(new DropOffPoint
                {
                    Location = "Introduction (0-1 min)",
                    DropOffCount = earlyDropOffs.Count,
                    DropOffPercentage = (double)earlyDropOffs.Count / views.Count,
                    AverageTimeSpentMs = (long)earlyDropOffs.Average(d => d.DurationMs ?? 0)
                });
            }

            if (midDropOffs.Any())
            {
                dropOffPoints.Add(new DropOffPoint
                {
                    Location = "Middle Section (1-5 min)",
                    DropOffCount = midDropOffs.Count,
                    DropOffPercentage = (double)midDropOffs.Count / views.Count,
                    AverageTimeSpentMs = (long)midDropOffs.Average(d => d.DurationMs ?? 0)
                });
            }

            if (lateDropOffs.Any())
            {
                dropOffPoints.Add(new DropOffPoint
                {
                    Location = "Advanced Section (5+ min)",
                    DropOffCount = lateDropOffs.Count,
                    DropOffPercentage = (double)lateDropOffs.Count / views.Count,
                    AverageTimeSpentMs = (long)lateDropOffs.Average(d => d.DurationMs ?? 0)
                });
            }
        }

        var analysis = new ContentDropOffAnalysis
        {
            ContentId = contentId,
            TotalViews = views.Count,
            TotalDropOffs = dropOffs.Count,
            DropOffRate = views.Count > 0 ? (double)dropOffs.Count / views.Count : 0,
            DropOffPoints = dropOffPoints
        };

        return Task.FromResult(analysis);
    }

    public async Task<List<HighDropOffContent>> GetHighDropOffContentAsync(double threshold = 0.5)
    {
        var contentIds = _contentViews.Select(v => v.ContentId).Distinct();
        var highDropOffContent = new List<HighDropOffContent>();

        foreach (var contentId in contentIds)
        {
            var analysis = await GetContentDropOffAsync(contentId);
            
            if (analysis.DropOffRate > threshold && analysis.TotalViews >= 10)
            {
                highDropOffContent.Add(new HighDropOffContent
                {
                    ContentId = contentId,
                    ContentTitle = $"Content {contentId}",
                    DropOffRate = analysis.DropOffRate,
                    TotalViews = analysis.TotalViews,
                    TotalDropOffs = analysis.TotalDropOffs
                });
            }
        }

        return highDropOffContent.OrderByDescending(c => c.DropOffRate).ToList();
    }

    #endregion

    #region Dashboard Metrics

    public async Task<DashboardMetrics> GetDashboardMetricsAsync()
    {
        var now = DateTime.UtcNow;
        var activeUsers = await GetActiveUsersAsync(now.AddMonths(-1), now);
        var allUsers = _userActivities.Select(a => a.UserId).Distinct().ToList();

        // Calculate retention
        var cohorts = await GetUserCohortsAsync(now.AddMonths(-3), now);
        var avgRetention7 = cohorts.Any() 
            ? await Task.WhenAll(cohorts.Select(c => CalculateCohortRetentionAsync(c, 7)))
            : new[] { 0.0 };
        var avgRetention30 = cohorts.Any()
            ? await Task.WhenAll(cohorts.Select(c => CalculateCohortRetentionAsync(c, 30)))
            : new[] { 0.0 };

        // Get popular content
        var contentViews = _contentViews
            .GroupBy(v => v.ContentId)
            .Select(g => new PopularContent
            {
                ContentId = g.Key,
                ContentTitle = $"Content {g.Key}",
                ContentType = "Lesson",
                ViewCount = g.Count(),
                CompletionCount = g.Count(v => v.Completed),
                CompletionRate = g.Count() > 0 ? (double)g.Count(v => v.Completed) / g.Count() : 0
            })
            .OrderByDescending(c => c.ViewCount)
            .Take(10)
            .ToList();

        var problematicContent = await GetLowCompletionLessonsAsync(0.5);

        var metrics = new DashboardMetrics
        {
            TotalUsers = allUsers.Count,
            ActiveUsersToday = activeUsers.DailyActiveUsers,
            ActiveUsersThisWeek = activeUsers.WeeklyActiveUsers,
            ActiveUsersThisMonth = activeUsers.MonthlyActiveUsers,
            TotalLessonsCompleted = _lessonCompletions.Count,
            TotalChallengesCompleted = _challengeCompletions.Count(c => c.Passed),
            AverageSessionDurationMinutes = _contentViews.Any() 
                ? _contentViews.Average(v => (v.DurationMs ?? 0) / 60000.0) 
                : 0,
            UserRetention7Day = avgRetention7.Any() ? avgRetention7.Average() : 0,
            UserRetention30Day = avgRetention30.Any() ? avgRetention30.Average() : 0,
            MostPopularContent = contentViews,
            ProblematicContent = problematicContent.Take(10).ToList()
        };

        return metrics;
    }

    public Task<UserEngagementMetrics> GetUserEngagementMetricsAsync(Guid userId)
    {
        var userActivities = _userActivities.Where(a => a.UserId == userId).ToList();
        var userViews = _contentViews.Where(v => v.UserId == userId).ToList();
        var userLessons = _lessonCompletions.Where(c => c.UserId == userId).ToList();
        var userChallenges = _challengeCompletions.Where(c => c.UserId == userId && c.Passed).ToList();

        // Calculate streak
        var activityDates = userActivities
            .Select(a => a.Timestamp.Date)
            .Distinct()
            .OrderByDescending(d => d)
            .ToList();

        var currentStreak = 0;
        var today = DateTime.UtcNow.Date;
        
        foreach (var date in activityDates)
        {
            if (date == today.AddDays(-currentStreak))
            {
                currentStreak++;
            }
            else
            {
                break;
            }
        }

        // Calculate engagement score (0-100)
        var engagementScore = CalculateEngagementScore(
            userActivities.Count,
            userLessons.Count,
            userChallenges.Count,
            currentStreak
        );

        var metrics = new UserEngagementMetrics
        {
            UserId = userId,
            TotalSessions = userActivities.Count,
            TotalTimeSpentMs = userViews.Sum(v => v.DurationMs ?? 0),
            LessonsCompleted = userLessons.Count,
            ChallengesCompleted = userChallenges.Count,
            LastActiveDate = userActivities.Any() 
                ? userActivities.Max(a => a.Timestamp) 
                : DateTime.MinValue,
            CurrentStreak = currentStreak,
            EngagementScore = engagementScore
        };

        return Task.FromResult(metrics);
    }

    #endregion

    #region Cohort Analysis

    public Task<List<UserCohort>> GetUserCohortsAsync(DateTime startDate, DateTime endDate)
    {
        var cohorts = new List<UserCohort>();
        var currentDate = startDate.Date;

        while (currentDate <= endDate.Date)
        {
            var usersInCohort = _userActivities
                .Where(a => a.Timestamp.Date == currentDate)
                .Select(a => a.UserId)
                .Distinct()
                .ToList();

            if (usersInCohort.Any())
            {
                cohorts.Add(new UserCohort
                {
                    StartDate = currentDate,
                    UserIds = usersInCohort
                });
            }

            currentDate = currentDate.AddDays(7); // Weekly cohorts
        }

        return Task.FromResult(cohorts);
    }

    public Task<double> CalculateCohortRetentionAsync(UserCohort cohort, int days)
    {
        var targetDate = cohort.StartDate.AddDays(days);
        var activeUsers = _userActivities
            .Where(a => a.Timestamp.Date == targetDate && cohort.UserIds.Contains(a.UserId))
            .Select(a => a.UserId)
            .Distinct()
            .Count();

        var retention = cohort.Size > 0 ? (double)activeUsers / cohort.Size : 0;
        return Task.FromResult(retention);
    }

    #endregion

    #region Helper Methods

    private double CalculateEngagementScore(
        int totalSessions,
        int lessonsCompleted,
        int challengesCompleted,
        int currentStreak)
    {
        // Weighted scoring system
        var sessionScore = Math.Min(totalSessions * 0.5, 25);
        var lessonScore = Math.Min(lessonsCompleted * 2, 35);
        var challengeScore = Math.Min(challengesCompleted * 3, 25);
        var streakScore = Math.Min(currentStreak * 1.5, 15);

        return sessionScore + lessonScore + challengeScore + streakScore;
    }

    #endregion
}
