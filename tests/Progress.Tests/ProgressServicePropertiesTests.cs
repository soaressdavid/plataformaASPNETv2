using FsCheck;
using FsCheck.Xunit;
using Microsoft.EntityFrameworkCore;
using Progress.Service.Services;
using Shared.Data;
using Shared.Entities;
using Shared.Repositories;
using Shared.Services;

namespace Progress.Tests;

/// <summary>
/// Property-based tests for ProgressService functionality.
/// Feature: aspnet-learning-platform
/// </summary>
public class ProgressServicePropertiesTests : IDisposable
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ProgressService _progressService;

    public ProgressServicePropertiesTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _dbContext = new ApplicationDbContext(options);
        var progressRepository = new ProgressRepository(_dbContext);
        _progressService = new ProgressService(progressRepository, _dbContext);
    }

    /// <summary>
    /// Property 28: Dashboard Data Completeness
    /// **Validates: Requirements 8.1, 8.2, 8.3, 8.4**
    /// 
    /// For any dashboard request, the response should include current XP, level, 
    /// solved challenge counts by difficulty, completed project count, and learning streak.
    /// </summary>
    [Property(MaxTest = 20)]
    public async Task Dashboard_ContainsAllRequiredData(Guid userId, NonNegativeInt xp)
    {
        var totalXP = xp.Get;

        // Create user
        var user = new User
        {
            Id = userId,
            Name = "Test User",
            Email = $"test{userId}@example.com",
            PasswordHash = "hash"
        };
        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();

        // Award XP to create progress
        if (totalXP > 0)
        {
            await _progressService.AwardXPAsync(userId, totalXP);
        }

        // Get dashboard
        var dashboard = await _progressService.GetDashboardAsync(userId);

        // Verify all required fields are present
        Assert.True(dashboard.CurrentXP >= 0);
        Assert.True(dashboard.CurrentLevel >= 0);
        Assert.True(dashboard.XPToNextLevel >= 0);
        Assert.True(dashboard.SolvedChallenges >= 0);
        Assert.True(dashboard.CompletedProjects >= 0);
        Assert.True(dashboard.LearningStreak >= 0);
        Assert.NotNull(dashboard.CoursesInProgress);
    }

    /// <summary>
    /// Property 29: Course Progress Calculation
    /// **Validates: Requirements 8.5**
    /// 
    /// For any enrolled course, the completion percentage should equal 
    /// (completed lessons / total lessons) * 100.
    /// </summary>
    [Property(MaxTest = 20)]
    public async Task CourseProgress_CalculatesCorrectly(Guid userId, PositiveInt totalLessons, NonNegativeInt completedLessons)
    {
        var total = Math.Min(totalLessons.Get, 20);
        var completed = Math.Min(completedLessons.Get, total);

        // Create user
        var user = new User
        {
            Id = userId,
            Name = "Test User",
            Email = $"test{userId}@example.com",
            PasswordHash = "hash"
        };
        _dbContext.Users.Add(user);

        // Create course with lessons
        var course = new Course
        {
            Id = Guid.NewGuid(),
            Title = "Test Course",
            Description = "Test Description",
            Level = Level.Beginner
        };
        _dbContext.Courses.Add(course);

        var lessons = new List<Lesson>();
        for (int i = 0; i < total; i++)
        {
            var lesson = new Lesson
            {
                Id = Guid.NewGuid(),
                CourseId = course.Id,
                Title = $"Lesson {i + 1}",
                Content = "Content",
                OrderIndex = i + 1
            };
            lessons.Add(lesson);
            _dbContext.Lessons.Add(lesson);
        }

        // Enroll user in course
        var enrollment = new Enrollment
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            CourseId = course.Id
        };
        _dbContext.Enrollments.Add(enrollment);

        // Complete some lessons
        for (int i = 0; i < completed; i++)
        {
            var completion = new LessonCompletion
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                LessonId = lessons[i].Id
            };
            _dbContext.LessonCompletions.Add(completion);
        }

        await _dbContext.SaveChangesAsync();

        // Get dashboard
        var dashboard = await _progressService.GetDashboardAsync(userId);

        // Calculate expected percentage
        var expectedPercentage = (int)Math.Round((double)completed / total * 100);

        // If course is 100% complete, it won't be in the "in progress" list
        if (expectedPercentage < 100)
        {
            var courseProgress = dashboard.CoursesInProgress.FirstOrDefault(cp => cp.CourseId == course.Id);
            Assert.NotNull(courseProgress);
            Assert.Equal(completed, courseProgress.CompletedLessons);
            Assert.Equal(total, courseProgress.TotalLessons);
            Assert.Equal(expectedPercentage, courseProgress.CompletionPercentage);
        }
    }

    /// <summary>
    /// Property 32: Leaderboard Ranking
    /// **Validates: Requirements 9.7**
    /// 
    /// For any set of students, the leaderboard should rank them in descending order 
    /// by total XP, with the top 100 displayed.
    /// </summary>
    [Property(MaxTest = 20)]
    public async Task Leaderboard_RanksInDescendingOrderByXP(PositiveInt userCount)
    {
        var count = Math.Min(userCount.Get, 10); // Limit to 10 users for test performance

        // Create users with different XP amounts
        var users = new List<(Guid userId, int xp)>();
        for (int i = 0; i < count; i++)
        {
            var userId = Guid.NewGuid();
            var xp = (i + 1) * 100; // Each user has different XP

            var user = new User
            {
                Id = userId,
                Name = $"User {i}",
                Email = $"user{i}@example.com",
                PasswordHash = "hash"
            };
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();

            // Award XP
            await _progressService.AwardXPAsync(userId, xp);
            users.Add((userId, xp));
        }

        // Get leaderboard
        var leaderboard = await _progressService.GetLeaderboardAsync(100);

        // Verify ranking is in descending order by XP
        for (int i = 0; i < leaderboard.Count - 1; i++)
        {
            Assert.True(leaderboard[i].XP >= leaderboard[i + 1].XP);
        }

        // Verify rank numbers are sequential
        for (int i = 0; i < leaderboard.Count; i++)
        {
            Assert.Equal(i + 1, leaderboard[i].Rank);
        }
    }

    /// <summary>
    /// Property: XP award updates level correctly
    /// 
    /// For any XP award, the user's level should be updated according to the formula.
    /// </summary>
    [Property(MaxTest = 20)]
    public async Task AwardXP_UpdatesLevelCorrectly(Guid userId, PositiveInt xp)
    {
        var xpAmount = Math.Min(xp.Get, 10000);

        // Create user
        var user = new User
        {
            Id = userId,
            Name = "Test User",
            Email = $"test{userId}@example.com",
            PasswordHash = "hash"
        };
        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();

        // Award XP
        await _progressService.AwardXPAsync(userId, xpAmount);

        // Get dashboard
        var dashboard = await _progressService.GetDashboardAsync(userId);

        // Verify level matches the formula
        var expectedLevel = XPCalculator.CalculateLevel(xpAmount);
        Assert.Equal(expectedLevel, dashboard.CurrentLevel);
        Assert.Equal(xpAmount, dashboard.CurrentXP);
    }

    /// <summary>
    /// Property: Multiple XP awards accumulate correctly
    /// 
    /// For any sequence of XP awards, the total XP should be the sum of all awards.
    /// </summary>
    [Property(MaxTest = 20)]
    public async Task AwardXP_AccumulatesCorrectly(Guid userId, PositiveInt award1, PositiveInt award2)
    {
        var xp1 = Math.Min(award1.Get, 1000);
        var xp2 = Math.Min(award2.Get, 1000);

        // Create user
        var user = new User
        {
            Id = userId,
            Name = "Test User",
            Email = $"test{userId}@example.com",
            PasswordHash = "hash"
        };
        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();

        // Award XP twice
        await _progressService.AwardXPAsync(userId, xp1);
        await _progressService.AwardXPAsync(userId, xp2);

        // Get dashboard
        var dashboard = await _progressService.GetDashboardAsync(userId);

        // Verify total XP is the sum
        Assert.Equal(xp1 + xp2, dashboard.CurrentXP);
    }

    /// <summary>
    /// Property: Leaderboard respects count limit
    /// 
    /// For any count parameter, the leaderboard should return at most that many entries.
    /// </summary>
    [Property(MaxTest = 20)]
    public async Task Leaderboard_RespectsCountLimit(PositiveInt count)
    {
        var limit = Math.Min(count.Get, 50);

        // Create more users than the limit
        for (int i = 0; i < limit + 10; i++)
        {
            var userId = Guid.NewGuid();
            var user = new User
            {
                Id = userId,
                Name = $"User {i}",
                Email = $"user{i}@example.com",
                PasswordHash = "hash"
            };
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();

            await _progressService.AwardXPAsync(userId, (i + 1) * 10);
        }

        // Get leaderboard with limit
        var leaderboard = await _progressService.GetLeaderboardAsync(limit);

        // Verify count doesn't exceed limit
        Assert.True(leaderboard.Count <= limit);
    }

    /// <summary>
    /// Property: Dashboard returns empty courses list for user with no enrollments
    /// 
    /// For any user without enrollments, the courses in progress list should be empty.
    /// </summary>
    [Property(MaxTest = 20)]
    public async Task Dashboard_ReturnsEmptyCoursesForNoEnrollments(Guid userId)
    {
        // Create user
        var user = new User
        {
            Id = userId,
            Name = "Test User",
            Email = $"test{userId}@example.com",
            PasswordHash = "hash"
        };
        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();

        // Get dashboard
        var dashboard = await _progressService.GetDashboardAsync(userId);

        // Verify courses in progress is empty
        Assert.Empty(dashboard.CoursesInProgress);
    }

    public void Dispose()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }
}
