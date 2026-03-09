using Microsoft.EntityFrameworkCore;
using Progress.Service.Services;
using Shared.Data;
using Shared.Entities;
using Shared.Repositories;

namespace Progress.Tests;

/// <summary>
/// Unit tests for ProgressService functionality.
/// </summary>
public class ProgressServiceTests : IDisposable
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ProgressService _progressService;

    public ProgressServiceTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _dbContext = new ApplicationDbContext(options);
        var progressRepository = new ProgressRepository(_dbContext);
        _progressService = new ProgressService(progressRepository, _dbContext);
    }

    [Fact]
    public async Task GetDashboardAsync_ReturnsDefaultValuesForNewUser()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new User
        {
            Id = userId,
            Name = "Test User",
            Email = "test@example.com",
            PasswordHash = "hash"
        };
        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();

        // Act
        var dashboard = await _progressService.GetDashboardAsync(userId);

        // Assert
        Assert.Equal(0, dashboard.CurrentXP);
        Assert.Equal(0, dashboard.CurrentLevel);
        Assert.True(dashboard.XPToNextLevel > 0);
        Assert.Equal(0, dashboard.SolvedChallenges);
        Assert.Equal(0, dashboard.CompletedProjects);
        Assert.Equal(0, dashboard.LearningStreak);
        Assert.Empty(dashboard.CoursesInProgress);
    }

    [Fact]
    public async Task AwardXPAsync_CreatesProgressRecordForNewUser()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new User
        {
            Id = userId,
            Name = "Test User",
            Email = "test@example.com",
            PasswordHash = "hash"
        };
        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();

        // Act
        await _progressService.AwardXPAsync(userId, 100);

        // Assert
        var dashboard = await _progressService.GetDashboardAsync(userId);
        Assert.Equal(100, dashboard.CurrentXP);
        Assert.Equal(1, dashboard.CurrentLevel); // floor(sqrt(100/100)) = 1
    }

    [Fact]
    public async Task AwardXPAsync_UpdatesExistingProgress()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new User
        {
            Id = userId,
            Name = "Test User",
            Email = "test@example.com",
            PasswordHash = "hash"
        };
        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();

        // Act
        await _progressService.AwardXPAsync(userId, 100);
        await _progressService.AwardXPAsync(userId, 50);

        // Assert
        var dashboard = await _progressService.GetDashboardAsync(userId);
        Assert.Equal(150, dashboard.CurrentXP);
    }

    [Fact]
    public async Task AwardXPAsync_ThrowsForNegativeXP()
    {
        // Arrange
        var userId = Guid.NewGuid();

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(
            async () => await _progressService.AwardXPAsync(userId, -10)
        );
    }

    [Fact]
    public async Task AwardXPAsync_ThrowsForZeroXP()
    {
        // Arrange
        var userId = Guid.NewGuid();

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(
            async () => await _progressService.AwardXPAsync(userId, 0)
        );
    }

    [Fact]
    public async Task GetDashboardAsync_CountsSolvedChallenges()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new User
        {
            Id = userId,
            Name = "Test User",
            Email = "test@example.com",
            PasswordHash = "hash"
        };
        _dbContext.Users.Add(user);

        // Create challenges
        var challenge1 = new Challenge
        {
            Id = Guid.NewGuid(),
            Title = "Challenge 1",
            Description = "Description",
            Difficulty = Difficulty.Easy,
            StarterCode = "code"
        };
        var challenge2 = new Challenge
        {
            Id = Guid.NewGuid(),
            Title = "Challenge 2",
            Description = "Description",
            Difficulty = Difficulty.Medium,
            StarterCode = "code"
        };
        _dbContext.Challenges.AddRange(challenge1, challenge2);

        // Create passing submissions
        var submission1 = new Submission
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            ChallengeId = challenge1.Id,
            Code = "solution",
            Passed = true,
            Result = "Success"
        };
        var submission2 = new Submission
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            ChallengeId = challenge2.Id,
            Code = "solution",
            Passed = true,
            Result = "Success"
        };
        _dbContext.Submissions.AddRange(submission1, submission2);
        await _dbContext.SaveChangesAsync();

        // Act
        var dashboard = await _progressService.GetDashboardAsync(userId);

        // Assert
        Assert.Equal(2, dashboard.SolvedChallenges);
    }

    [Fact]
    public async Task GetDashboardAsync_DoesNotCountFailedSubmissions()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new User
        {
            Id = userId,
            Name = "Test User",
            Email = "test@example.com",
            PasswordHash = "hash"
        };
        _dbContext.Users.Add(user);

        var challenge = new Challenge
        {
            Id = Guid.NewGuid(),
            Title = "Challenge",
            Description = "Description",
            Difficulty = Difficulty.Easy,
            StarterCode = "code"
        };
        _dbContext.Challenges.Add(challenge);

        // Create failing submission
        var submission = new Submission
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            ChallengeId = challenge.Id,
            Code = "solution",
            Passed = false,
            Result = "Failed"
        };
        _dbContext.Submissions.Add(submission);
        await _dbContext.SaveChangesAsync();

        // Act
        var dashboard = await _progressService.GetDashboardAsync(userId);

        // Assert
        Assert.Equal(0, dashboard.SolvedChallenges);
    }

    [Fact]
    public async Task GetDashboardAsync_CountsEachChallengeOnce()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new User
        {
            Id = userId,
            Name = "Test User",
            Email = "test@example.com",
            PasswordHash = "hash"
        };
        _dbContext.Users.Add(user);

        var challenge = new Challenge
        {
            Id = Guid.NewGuid(),
            Title = "Challenge",
            Description = "Description",
            Difficulty = Difficulty.Easy,
            StarterCode = "code"
        };
        _dbContext.Challenges.Add(challenge);

        // Create multiple passing submissions for same challenge
        var submission1 = new Submission
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            ChallengeId = challenge.Id,
            Code = "solution1",
            Passed = true,
            Result = "Success"
        };
        var submission2 = new Submission
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            ChallengeId = challenge.Id,
            Code = "solution2",
            Passed = true,
            Result = "Success"
        };
        _dbContext.Submissions.AddRange(submission1, submission2);
        await _dbContext.SaveChangesAsync();

        // Act
        var dashboard = await _progressService.GetDashboardAsync(userId);

        // Assert
        Assert.Equal(1, dashboard.SolvedChallenges); // Should count challenge only once
    }

    [Fact]
    public async Task GetDashboardAsync_CalculatesCourseProgress()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new User
        {
            Id = userId,
            Name = "Test User",
            Email = "test@example.com",
            PasswordHash = "hash"
        };
        _dbContext.Users.Add(user);

        // Create course with 4 lessons
        var course = new Course
        {
            Id = Guid.NewGuid(),
            Title = "Test Course",
            Description = "Description",
            Level = Level.Beginner
        };
        _dbContext.Courses.Add(course);

        var lessons = new List<Lesson>();
        for (int i = 0; i < 4; i++)
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

        // Enroll user
        var enrollment = new Enrollment
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            CourseId = course.Id
        };
        _dbContext.Enrollments.Add(enrollment);

        // Complete 2 out of 4 lessons
        for (int i = 0; i < 2; i++)
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

        // Act
        var dashboard = await _progressService.GetDashboardAsync(userId);

        // Assert
        Assert.Single(dashboard.CoursesInProgress);
        var courseProgress = dashboard.CoursesInProgress[0];
        Assert.Equal(course.Id, courseProgress.CourseId);
        Assert.Equal(2, courseProgress.CompletedLessons);
        Assert.Equal(4, courseProgress.TotalLessons);
        Assert.Equal(50, courseProgress.CompletionPercentage); // 2/4 * 100 = 50%
    }

    [Fact]
    public async Task GetDashboardAsync_ExcludesFullyCompletedCourses()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new User
        {
            Id = userId,
            Name = "Test User",
            Email = "test@example.com",
            PasswordHash = "hash"
        };
        _dbContext.Users.Add(user);

        // Create course with 2 lessons
        var course = new Course
        {
            Id = Guid.NewGuid(),
            Title = "Test Course",
            Description = "Description",
            Level = Level.Beginner
        };
        _dbContext.Courses.Add(course);

        var lessons = new List<Lesson>();
        for (int i = 0; i < 2; i++)
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

        // Enroll user
        var enrollment = new Enrollment
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            CourseId = course.Id
        };
        _dbContext.Enrollments.Add(enrollment);

        // Complete all lessons
        foreach (var lesson in lessons)
        {
            var completion = new LessonCompletion
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                LessonId = lesson.Id
            };
            _dbContext.LessonCompletions.Add(completion);
        }

        await _dbContext.SaveChangesAsync();

        // Act
        var dashboard = await _progressService.GetDashboardAsync(userId);

        // Assert
        Assert.Empty(dashboard.CoursesInProgress); // 100% complete courses should not be in progress
    }

    [Fact]
    public async Task GetLeaderboardAsync_ReturnsTopUsers()
    {
        // Arrange
        for (int i = 0; i < 5; i++)
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

            // Award different XP amounts
            await _progressService.AwardXPAsync(userId, (5 - i) * 100);
        }

        // Act
        var leaderboard = await _progressService.GetLeaderboardAsync(3);

        // Assert
        Assert.Equal(3, leaderboard.Count);
        Assert.Equal(500, leaderboard[0].XP); // Highest XP first
        Assert.Equal(400, leaderboard[1].XP);
        Assert.Equal(300, leaderboard[2].XP);
    }

    [Fact]
    public async Task GetLeaderboardAsync_ThrowsForInvalidCount()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(
            async () => await _progressService.GetLeaderboardAsync(0)
        );
        await Assert.ThrowsAsync<ArgumentException>(
            async () => await _progressService.GetLeaderboardAsync(-1)
        );
    }

    [Fact]
    public async Task AwardXPAsync_UpdatesStreak()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new User
        {
            Id = userId,
            Name = "Test User",
            Email = "test@example.com",
            PasswordHash = "hash"
        };
        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();

        // Act
        await _progressService.AwardXPAsync(userId, 100);

        // Assert
        var dashboard = await _progressService.GetDashboardAsync(userId);
        Assert.True(dashboard.LearningStreak >= 1); // Should have at least 1 day streak
    }

    public void Dispose()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }
}
