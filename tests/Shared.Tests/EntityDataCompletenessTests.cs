using Shared.Entities;
using Xunit;

namespace Shared.Tests;

public class EntityDataCompletenessTests
{
    // Property 33: Entity Data Completeness
    // For any entity (User, Course, Lesson, Challenge, Submission) stored in the database,
    // it should have all required fields populated with valid values.

    [Fact]
    public void User_WithAllRequiredFields_ShouldBeValid()
    {
        // Arrange & Act
        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = "Test User",
            Email = "test@example.com",
            PasswordHash = "hashed_password",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        // Assert
        Assert.NotEqual(Guid.Empty, user.Id);
        Assert.False(string.IsNullOrWhiteSpace(user.Name));
        Assert.False(string.IsNullOrWhiteSpace(user.Email));
        Assert.Contains("@", user.Email);
        Assert.False(string.IsNullOrWhiteSpace(user.PasswordHash));
        Assert.NotEqual(default, user.CreatedAt);
        Assert.NotEqual(default, user.UpdatedAt);
    }

    [Fact]
    public void Course_WithAllRequiredFields_ShouldBeValid()
    {
        // Arrange & Act
        var course = new Course
        {
            Id = Guid.NewGuid(),
            Title = "Test Course",
            Description = "Test Description",
            Level = Level.Beginner,
            OrderIndex = 1,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        // Assert
        Assert.NotEqual(Guid.Empty, course.Id);
        Assert.False(string.IsNullOrWhiteSpace(course.Title));
        Assert.False(string.IsNullOrWhiteSpace(course.Description));
        Assert.True(Enum.IsDefined(typeof(Level), course.Level));
        Assert.True(course.OrderIndex > 0);
        Assert.NotEqual(default, course.CreatedAt);
        Assert.NotEqual(default, course.UpdatedAt);
    }

    [Fact]
    public void Challenge_WithAllRequiredFields_ShouldBeValid()
    {
        // Arrange & Act
        var challenge = new Challenge
        {
            Id = Guid.NewGuid(),
            Title = "Test Challenge",
            Description = "Test Description",
            Difficulty = Difficulty.Easy,
            StarterCode = "// starter code",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        // Assert
        Assert.NotEqual(Guid.Empty, challenge.Id);
        Assert.False(string.IsNullOrWhiteSpace(challenge.Title));
        Assert.False(string.IsNullOrWhiteSpace(challenge.Description));
        Assert.True(Enum.IsDefined(typeof(Difficulty), challenge.Difficulty));
        Assert.False(string.IsNullOrWhiteSpace(challenge.StarterCode));
        Assert.NotEqual(default, challenge.CreatedAt);
        Assert.NotEqual(default, challenge.UpdatedAt);
    }

    [Fact]
    public void Submission_WithAllRequiredFields_ShouldBeValid()
    {
        // Arrange & Act
        var submission = new Submission
        {
            Id = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            ChallengeId = Guid.NewGuid(),
            Code = "// submitted code",
            Passed = true,
            Result = "All tests passed",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        // Assert
        Assert.NotEqual(Guid.Empty, submission.Id);
        Assert.NotEqual(Guid.Empty, submission.UserId);
        Assert.NotEqual(Guid.Empty, submission.ChallengeId);
        Assert.False(string.IsNullOrWhiteSpace(submission.Code));
        Assert.NotEqual(default, submission.CreatedAt);
        Assert.NotEqual(default, submission.UpdatedAt);
    }

    [Fact]
    public void Lesson_WithAllRequiredFields_ShouldBeValid()
    {
        // Arrange & Act
        var lesson = new Lesson
        {
            Id = Guid.NewGuid(),
            CourseId = Guid.NewGuid(),
            Title = "Test Lesson",
            Content = "Test Content",
            OrderIndex = 1,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        // Assert
        Assert.NotEqual(Guid.Empty, lesson.Id);
        Assert.NotEqual(Guid.Empty, lesson.CourseId);
        Assert.False(string.IsNullOrWhiteSpace(lesson.Title));
        Assert.False(string.IsNullOrWhiteSpace(lesson.Content));
        Assert.True(lesson.OrderIndex > 0);
        Assert.NotEqual(default, lesson.CreatedAt);
        Assert.NotEqual(default, lesson.UpdatedAt);
    }
}
