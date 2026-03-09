using Microsoft.EntityFrameworkCore;
using Progress.Service.Models;
using Shared.Data;
using Shared.Entities;
using Shared.Interfaces;
using Shared.Services;

namespace Progress.Service.Services;

public class ProgressService
{
    private readonly IProgressRepository _progressRepository;
    private readonly ApplicationDbContext _dbContext;

    public ProgressService(IProgressRepository progressRepository, ApplicationDbContext dbContext)
    {
        _progressRepository = progressRepository;
        _dbContext = dbContext;
    }

    /// <summary>
    /// Gets the dashboard data for a user including XP, level, solved challenges, 
    /// completed projects, streak, and courses in progress.
    /// </summary>
    public async Task<DashboardResponse> GetDashboardAsync(Guid userId)
    {
        // Get or create progress record
        var progress = await _progressRepository.GetByUserIdAsync(userId);
        if (progress == null)
        {
            progress = new Shared.Entities.Progress
            {
                UserId = userId,
                TotalXP = 0,
                CurrentLevel = 0,
                LearningStreak = 0,
                LastActivityAt = DateTime.UtcNow
            };
            progress = await _progressRepository.CreateAsync(progress);
        }

        // Get solved challenges count
        var solvedChallenges = await _dbContext.Submissions
            .Where(s => s.UserId == userId && s.Passed)
            .Select(s => s.ChallengeId)
            .Distinct()
            .CountAsync();

        // Get completed projects count (placeholder - projects not yet implemented)
        var completedProjects = 0;

        // Get courses in progress
        var coursesInProgress = await GetCoursesInProgressAsync(userId);

        // Calculate XP to next level
        var xpToNextLevel = XPCalculator.CalculateXPToNextLevel(progress.CurrentLevel, progress.TotalXP);

        return new DashboardResponse(
            CurrentXP: progress.TotalXP,
            CurrentLevel: progress.CurrentLevel,
            XPToNextLevel: xpToNextLevel,
            SolvedChallenges: solvedChallenges,
            CompletedProjects: completedProjects,
            LearningStreak: progress.LearningStreak,
            CoursesInProgress: coursesInProgress
        );
    }

    /// <summary>
    /// Awards XP to a user, updates their level, and updates last activity timestamp.
    /// </summary>
    public virtual async Task AwardXPAsync(Guid userId, int xpAmount)
    {
        if (xpAmount <= 0)
            throw new ArgumentException("XP amount must be positive", nameof(xpAmount));

        // Get or create progress record
        var progress = await _progressRepository.GetByUserIdAsync(userId);
        if (progress == null)
        {
            progress = new Shared.Entities.Progress
            {
                UserId = userId,
                TotalXP = 0,
                CurrentLevel = 0,
                LearningStreak = 0,
                LastActivityAt = DateTime.UtcNow
            };
            progress = await _progressRepository.CreateAsync(progress);
        }

        // Get all activity dates for streak calculation
        var activityDates = await GetUserActivityDatesAsync(userId);
        activityDates.Add(DateTime.UtcNow);

        // Update progress
        progress.TotalXP += xpAmount;
        progress.CurrentLevel = XPCalculator.CalculateLevel(progress.TotalXP);
        progress.LearningStreak = StreakCalculator.CalculateStreak(activityDates, TimeZoneInfo.Utc);
        progress.LastActivityAt = DateTime.UtcNow;

        await _progressRepository.UpdateAsync(progress);
    }

    /// <summary>
    /// Gets the leaderboard with top students ranked by XP.
    /// </summary>
    public async Task<List<Shared.Interfaces.LeaderboardEntry>> GetLeaderboardAsync(int count = 100)
    {
        if (count <= 0)
            throw new ArgumentException("Count must be positive", nameof(count));

        return await _progressRepository.GetTopAsync(count);
    }

    /// <summary>
    /// Gets courses in progress for a user with completion percentage.
    /// Course progress calculation: (completed lessons / total lessons * 100)
    /// </summary>
    private async Task<List<CourseProgress>> GetCoursesInProgressAsync(Guid userId)
    {
        var enrollments = await _dbContext.Enrollments
            .Include(e => e.Course)
            .ThenInclude(c => c.Lessons)
            .Where(e => e.UserId == userId)
            .ToListAsync();

        var coursesProgress = new List<CourseProgress>();

        foreach (var enrollment in enrollments)
        {
            var course = enrollment.Course;
            var totalLessons = course.Lessons.Count;

            if (totalLessons == 0)
                continue;

            // Get completed lessons for this course
            var lessonIds = course.Lessons.Select(l => l.Id).ToList();
            var completedLessons = await _dbContext.LessonCompletions
                .Where(lc => lc.UserId == userId && lessonIds.Contains(lc.LessonId))
                .CountAsync();

            // Calculate completion percentage
            var completionPercentage = (int)Math.Round((double)completedLessons / totalLessons * 100);

            // Only include courses that are in progress (not 100% complete)
            if (completionPercentage < 100)
            {
                coursesProgress.Add(new CourseProgress(
                    CourseId: course.Id,
                    Title: course.Title,
                    CompletedLessons: completedLessons,
                    TotalLessons: totalLessons,
                    CompletionPercentage: completionPercentage
                ));
            }
        }

        return coursesProgress;
    }

    /// <summary>
    /// Gets all activity dates for a user (submissions and lesson completions).
    /// </summary>
    private async Task<List<DateTime>> GetUserActivityDatesAsync(Guid userId)
    {
        var activityDates = new List<DateTime>();

        // Get submission dates
        var submissionDates = await _dbContext.Submissions
            .Where(s => s.UserId == userId)
            .Select(s => s.CreatedAt)
            .ToListAsync();
        activityDates.AddRange(submissionDates);

        // Get lesson completion dates
        var lessonCompletionDates = await _dbContext.LessonCompletions
            .Where(lc => lc.UserId == userId)
            .Select(lc => lc.CompletedAt)
            .ToListAsync();
        activityDates.AddRange(lessonCompletionDates);

        return activityDates;
    }
}
