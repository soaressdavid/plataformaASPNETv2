using Microsoft.EntityFrameworkCore;
using Shared.Data;
using Shared.Entities;
using Shared.Repositories;
using Xunit;

namespace Shared.Tests;

/// <summary>
/// Unit tests for repository CRUD operations
/// **Validates: Requirements 10.1, 10.2, 10.3, 10.4, 10.5, 10.7**
/// </summary>
public class RepositoryTests
{
    private ApplicationDbContext CreateInMemoryContext(string databaseName)
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: databaseName)
            .Options;

        return new ApplicationDbContext(options);
    }

    #region UserRepository Tests

    [Fact]
    public async Task UserRepository_CreateAsync_ShouldCreateUser()
    {
        // Arrange
        var dbName = $"TestDb_{Guid.NewGuid()}";
        using var context = CreateInMemoryContext(dbName);
        var repository = new UserRepository(context);

        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = "John Doe",
            Email = "john@example.com",
            PasswordHash = "hashedpassword",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        // Act
        var result = await repository.CreateAsync(user);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(user.Id, result.Id);
        Assert.Equal(user.Name, result.Name);
        Assert.Equal(user.Email, result.Email);
    }

    [Fact]
    public async Task UserRepository_GetByIdAsync_ShouldReturnUser()
    {
        // Arrange
        var dbName = $"TestDb_{Guid.NewGuid()}";
        using var context = CreateInMemoryContext(dbName);
        var repository = new UserRepository(context);

        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = "Jane Doe",
            Email = "jane@example.com",
            PasswordHash = "hashedpassword",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await repository.CreateAsync(user);

        // Act
        var result = await repository.GetByIdAsync(user.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(user.Id, result.Id);
        Assert.Equal(user.Name, result.Name);
    }

    [Fact]
    public async Task UserRepository_GetByEmailAsync_ShouldReturnUser()
    {
        // Arrange
        var dbName = $"TestDb_{Guid.NewGuid()}";
        using var context = CreateInMemoryContext(dbName);
        var repository = new UserRepository(context);

        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = "Bob Smith",
            Email = "bob@example.com",
            PasswordHash = "hashedpassword",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await repository.CreateAsync(user);

        // Act
        var result = await repository.GetByEmailAsync("bob@example.com");

        // Assert
        Assert.NotNull(result);
        Assert.Equal(user.Id, result.Id);
        Assert.Equal(user.Email, result.Email);
    }

    [Fact]
    public async Task UserRepository_UpdateAsync_ShouldUpdateUser()
    {
        // Arrange
        var dbName = $"TestDb_{Guid.NewGuid()}";
        using var context = CreateInMemoryContext(dbName);
        var repository = new UserRepository(context);

        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = "Alice",
            Email = "alice@example.com",
            PasswordHash = "hashedpassword",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await repository.CreateAsync(user);

        // Act
        user.Name = "Alice Updated";
        await repository.UpdateAsync(user);

        // Assert
        var updated = await repository.GetByIdAsync(user.Id);
        Assert.NotNull(updated);
        Assert.Equal("Alice Updated", updated.Name);
    }

    [Fact]
    public async Task UserRepository_DeleteAsync_ShouldDeleteUser()
    {
        // Arrange
        var dbName = $"TestDb_{Guid.NewGuid()}";
        using var context = CreateInMemoryContext(dbName);
        var repository = new UserRepository(context);

        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = "Charlie",
            Email = "charlie@example.com",
            PasswordHash = "hashedpassword",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await repository.CreateAsync(user);

        // Act
        await repository.DeleteAsync(user.Id);

        // Assert
        var deleted = await repository.GetByIdAsync(user.Id);
        Assert.Null(deleted);
    }

    #endregion

    #region CourseRepository Tests

    [Fact]
    public async Task CourseRepository_CreateAsync_ShouldCreateCourse()
    {
        // Arrange
        var dbName = $"TestDb_{Guid.NewGuid()}";
        using var context = CreateInMemoryContext(dbName);
        var repository = new CourseRepository(context);

        var course = new Course
        {
            Id = Guid.NewGuid(),
            Title = "ASP.NET Core Basics",
            Description = "Learn the basics",
            Level = Level.Beginner,
            OrderIndex = 1,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        // Act
        var result = await repository.CreateAsync(course);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(course.Id, result.Id);
        Assert.Equal(course.Title, result.Title);
    }

    [Fact]
    public async Task CourseRepository_GetLessonsAsync_ShouldReturnLessonsInOrder()
    {
        // Arrange
        var dbName = $"TestDb_{Guid.NewGuid()}";
        using var context = CreateInMemoryContext(dbName);
        var repository = new CourseRepository(context);

        var course = new Course
        {
            Id = Guid.NewGuid(),
            Title = "Test Course",
            Description = "Description",
            Level = Level.Beginner,
            OrderIndex = 1,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await repository.CreateAsync(course);

        var lesson1 = new Lesson
        {
            Id = Guid.NewGuid(),
            CourseId = course.Id,
            Title = "Lesson 1",
            Content = "Content 1",
            OrderIndex = 1,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var lesson2 = new Lesson
        {
            Id = Guid.NewGuid(),
            CourseId = course.Id,
            Title = "Lesson 2",
            Content = "Content 2",
            OrderIndex = 2,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        context.Lessons.Add(lesson1);
        context.Lessons.Add(lesson2);
        await context.SaveChangesAsync();

        // Act
        var lessons = await repository.GetLessonsAsync(course.Id);

        // Assert
        Assert.Equal(2, lessons.Count);
        Assert.Equal(1, lessons[0].OrderIndex);
        Assert.Equal(2, lessons[1].OrderIndex);
    }

    #endregion

    #region ChallengeRepository Tests

    [Fact]
    public async Task ChallengeRepository_CreateAsync_ShouldCreateChallenge()
    {
        // Arrange
        var dbName = $"TestDb_{Guid.NewGuid()}";
        using var context = CreateInMemoryContext(dbName);
        var repository = new ChallengeRepository(context);

        var challenge = new Challenge
        {
            Id = Guid.NewGuid(),
            Title = "FizzBuzz",
            Description = "Implement FizzBuzz",
            Difficulty = Difficulty.Easy,
            StarterCode = "// Your code here",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        // Act
        var result = await repository.CreateAsync(challenge);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(challenge.Id, result.Id);
        Assert.Equal(challenge.Title, result.Title);
    }

    [Fact]
    public async Task ChallengeRepository_GetTestCasesAsync_ShouldReturnTestCasesInOrder()
    {
        // Arrange
        var dbName = $"TestDb_{Guid.NewGuid()}";
        using var context = CreateInMemoryContext(dbName);
        var repository = new ChallengeRepository(context);

        var challenge = new Challenge
        {
            Id = Guid.NewGuid(),
            Title = "Test Challenge",
            Description = "Description",
            Difficulty = Difficulty.Medium,
            StarterCode = "// code",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await repository.CreateAsync(challenge);

        var testCase1 = new TestCase
        {
            Id = Guid.NewGuid(),
            ChallengeId = challenge.Id,
            Input = "1",
            ExpectedOutput = "1",
            OrderIndex = 1,
            IsHidden = false,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var testCase2 = new TestCase
        {
            Id = Guid.NewGuid(),
            ChallengeId = challenge.Id,
            Input = "2",
            ExpectedOutput = "2",
            OrderIndex = 2,
            IsHidden = false,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        context.TestCases.Add(testCase1);
        context.TestCases.Add(testCase2);
        await context.SaveChangesAsync();

        // Act
        var testCases = await repository.GetTestCasesAsync(challenge.Id);

        // Assert
        Assert.Equal(2, testCases.Count);
        Assert.Equal(1, testCases[0].OrderIndex);
        Assert.Equal(2, testCases[1].OrderIndex);
    }

    #endregion

    #region SubmissionRepository Tests

    [Fact]
    public async Task SubmissionRepository_CreateAsync_ShouldCreateSubmission()
    {
        // Arrange
        var dbName = $"TestDb_{Guid.NewGuid()}";
        using var context = CreateInMemoryContext(dbName);
        var repository = new SubmissionRepository(context);

        var submission = new Submission
        {
            Id = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            ChallengeId = Guid.NewGuid(),
            Code = "// solution code",
            Passed = true,
            Result = "All tests passed",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        // Act
        var result = await repository.CreateAsync(submission);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(submission.Id, result.Id);
        Assert.True(result.Passed);
    }

    [Fact]
    public async Task SubmissionRepository_GetByUserAndChallengeAsync_ShouldReturnSubmissionsInDescendingOrder()
    {
        // Arrange
        var dbName = $"TestDb_{Guid.NewGuid()}";
        using var context = CreateInMemoryContext(dbName);
        var repository = new SubmissionRepository(context);

        var userId = Guid.NewGuid();
        var challengeId = Guid.NewGuid();

        var submission1 = new Submission
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            ChallengeId = challengeId,
            Code = "// attempt 1",
            Passed = false,
            Result = "Failed",
            CreatedAt = DateTime.UtcNow.AddMinutes(-10),
            UpdatedAt = DateTime.UtcNow.AddMinutes(-10)
        };

        var submission2 = new Submission
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            ChallengeId = challengeId,
            Code = "// attempt 2",
            Passed = true,
            Result = "Passed",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await repository.CreateAsync(submission1);
        await repository.CreateAsync(submission2);

        // Act
        var submissions = await repository.GetByUserAndChallengeAsync(userId, challengeId);

        // Assert
        Assert.Equal(2, submissions.Count);
        Assert.Equal(submission2.Id, submissions[0].Id); // Most recent first
        Assert.Equal(submission1.Id, submissions[1].Id);
    }

    #endregion

    #region ProgressRepository Tests

    [Fact]
    public async Task ProgressRepository_CreateAsync_ShouldCreateProgress()
    {
        // Arrange
        var dbName = $"TestDb_{Guid.NewGuid()}";
        using var context = CreateInMemoryContext(dbName);
        var repository = new ProgressRepository(context);

        var progress = new Progress
        {
            Id = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            TotalXP = 100,
            CurrentLevel = 1,
            LearningStreak = 5,
            LastActivityAt = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        // Act
        var result = await repository.CreateAsync(progress);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(progress.Id, result.Id);
        Assert.Equal(100, result.TotalXP);
    }

    [Fact]
    public async Task ProgressRepository_GetByUserIdAsync_ShouldReturnProgress()
    {
        // Arrange
        var dbName = $"TestDb_{Guid.NewGuid()}";
        using var context = CreateInMemoryContext(dbName);
        var progressRepository = new ProgressRepository(context);
        var userRepository = new UserRepository(context);

        // Create user first
        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = "Test User",
            Email = "testuser@example.com",
            PasswordHash = "hash",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await userRepository.CreateAsync(user);

        var progress = new Progress
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            TotalXP = 250,
            CurrentLevel = 2,
            LearningStreak = 10,
            LastActivityAt = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await progressRepository.CreateAsync(progress);

        // Act
        var result = await progressRepository.GetByUserIdAsync(user.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(user.Id, result.UserId);
        Assert.Equal(250, result.TotalXP);
    }

    [Fact]
    public async Task ProgressRepository_GetTopAsync_ShouldReturnLeaderboard()
    {
        // Arrange
        var dbName = $"TestDb_{Guid.NewGuid()}";
        using var context = CreateInMemoryContext(dbName);
        var repository = new ProgressRepository(context);
        var userRepository = new UserRepository(context);

        // Create users
        var user1 = new User
        {
            Id = Guid.NewGuid(),
            Name = "User 1",
            Email = "user1@example.com",
            PasswordHash = "hash",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var user2 = new User
        {
            Id = Guid.NewGuid(),
            Name = "User 2",
            Email = "user2@example.com",
            PasswordHash = "hash",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var user3 = new User
        {
            Id = Guid.NewGuid(),
            Name = "User 3",
            Email = "user3@example.com",
            PasswordHash = "hash",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await userRepository.CreateAsync(user1);
        await userRepository.CreateAsync(user2);
        await userRepository.CreateAsync(user3);

        // Create progress records
        var progress1 = new Progress
        {
            Id = Guid.NewGuid(),
            UserId = user1.Id,
            TotalXP = 500,
            CurrentLevel = 2,
            LearningStreak = 5,
            LastActivityAt = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var progress2 = new Progress
        {
            Id = Guid.NewGuid(),
            UserId = user2.Id,
            TotalXP = 1000,
            CurrentLevel = 3,
            LearningStreak = 10,
            LastActivityAt = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var progress3 = new Progress
        {
            Id = Guid.NewGuid(),
            UserId = user3.Id,
            TotalXP = 250,
            CurrentLevel = 1,
            LearningStreak = 3,
            LastActivityAt = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await repository.CreateAsync(progress1);
        await repository.CreateAsync(progress2);
        await repository.CreateAsync(progress3);

        // Act
        var leaderboard = await repository.GetTopAsync(3);

        // Assert
        Assert.Equal(3, leaderboard.Count);
        Assert.Equal(1, leaderboard[0].Rank);
        Assert.Equal("User 2", leaderboard[0].Name);
        Assert.Equal(1000, leaderboard[0].XP);
        Assert.Equal(2, leaderboard[1].Rank);
        Assert.Equal("User 1", leaderboard[1].Name);
        Assert.Equal(3, leaderboard[2].Rank);
        Assert.Equal("User 3", leaderboard[2].Name);
    }

    #endregion
}
