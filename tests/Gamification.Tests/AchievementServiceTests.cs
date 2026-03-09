using Xunit;
using Moq;
using Microsoft.EntityFrameworkCore;
using Shared.Services;
using Shared.Entities;
using Shared.Data;
using Microsoft.Extensions.Logging;

namespace Gamification.Tests;

/// <summary>
/// Unit tests for Achievement Service
/// Validates: Requirements 15.1, 15.2, 15.3, 15.4
/// </summary>
public class AchievementServiceTests
{
    private readonly Mock<ILogger<AchievementService>> _loggerMock;
    private readonly ApplicationDbContext _context;
    private readonly AchievementService _service;

    public AchievementServiceTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new ApplicationDbContext(options);
        _loggerMock = new Mock<ILogger<AchievementService>>();
        _service = new AchievementService(_context, _loggerMock.Object);
    }

    [Fact]
    public async Task GetAllAchievementsAsync_ReturnsOnlyNonDeleted()
    {
        // Arrange
        var achievement1 = new Achievement
        {
            Id = Guid.NewGuid(),
            Name = "First Steps",
            Description = "Complete your first lesson",
            IsDeleted = false
        };

        var achievement2 = new Achievement
        {
            Id = Guid.NewGuid(),
            Name = "Deleted Achievement",
            Description = "This should not appear",
            IsDeleted = true
        };

        _context.Set<Achievement>().AddRange(achievement1, achievement2);
        await _context.SaveChangesAsync();

        // Act
        var result = await _service.GetAllAchievementsAsync();

        // Assert
        Assert.Single(result);
        Assert.Equal("First Steps", result[0].Name);
    }

    [Fact]
    public async Task AwardAchievementAsync_FirstTime_ReturnsTrue()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var achievementId = Guid.NewGuid();

        var user = new User
        {
            Id = userId,
            Name = "Test User",
            Email = "test@example.com",
            PasswordHash = "hash",
            Progress = new Progress
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                TotalXP = 100,
                CurrentLevel = 1
            }
        };

        var achievement = new Achievement
        {
            Id = achievementId,
            Name = "Test Achievement",
            Description = "Test",
            XPReward = 50,
            Criteria = "{\"type\":\"lessons_completed\",\"count\":1}"
        };

        _context.Users.Add(user);
        _context.Set<Achievement>().Add(achievement);
        await _context.SaveChangesAsync();

        // Act
        var result = await _service.AwardAchievementAsync(userId, achievementId);

        // Assert
        Assert.True(result);
        
        var userAchievement = await _context.Set<UserAchievement>()
            .FirstOrDefaultAsync(ua => ua.UserId == userId && ua.AchievementId == achievementId);
        
        Assert.NotNull(userAchievement);
        
        var updatedUser = await _context.Users
            .Include(u => u.Progress)
            .FirstAsync(u => u.Id == userId);
        
        Assert.Equal(150, updatedUser.Progress!.TotalXP); // 100 + 50
    }

    [Fact]
    public async Task AwardAchievementAsync_AlreadyEarned_ReturnsFalse()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var achievementId = Guid.NewGuid();

        var userAchievement = new UserAchievement
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            AchievementId = achievementId,
            EarnedAt = DateTime.UtcNow
        };

        _context.Set<UserAchievement>().Add(userAchievement);
        await _context.SaveChangesAsync();

        // Act
        var result = await _service.AwardAchievementAsync(userId, achievementId);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task GetUserAchievementsAsync_ReturnsOnlyUserAchievements()
    {
        // Arrange
        var userId1 = Guid.NewGuid();
        var userId2 = Guid.NewGuid();
        var achievementId = Guid.NewGuid();

        var achievement = new Achievement
        {
            Id = achievementId,
            Name = "Test Achievement",
            Description = "Test"
        };

        var userAchievement1 = new UserAchievement
        {
            Id = Guid.NewGuid(),
            UserId = userId1,
            AchievementId = achievementId,
            EarnedAt = DateTime.UtcNow,
            Achievement = achievement
        };

        var userAchievement2 = new UserAchievement
        {
            Id = Guid.NewGuid(),
            UserId = userId2,
            AchievementId = achievementId,
            EarnedAt = DateTime.UtcNow,
            Achievement = achievement
        };

        _context.Set<Achievement>().Add(achievement);
        _context.Set<UserAchievement>().AddRange(userAchievement1, userAchievement2);
        await _context.SaveChangesAsync();

        // Act
        var result = await _service.GetUserAchievementsAsync(userId1);

        // Assert
        Assert.Single(result);
        Assert.Equal(userId1, result[0].UserId);
    }

    [Fact]
    public async Task UpdateAchievementProgressAsync_CreatesNewProgress()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var achievementId = Guid.NewGuid();

        var achievement = new Achievement
        {
            Id = achievementId,
            Name = "Test Achievement",
            Description = "Test",
            Criteria = "{\"type\":\"challenges_completed\",\"count\":10}"
        };

        _context.Set<Achievement>().Add(achievement);
        await _context.SaveChangesAsync();

        // Act
        await _service.UpdateAchievementProgressAsync(userId, achievementId, 5);

        // Assert
        var userAchievement = await _context.Set<UserAchievement>()
            .FirstOrDefaultAsync(ua => ua.UserId == userId && ua.AchievementId == achievementId);

        Assert.NotNull(userAchievement);
        Assert.Equal(5, userAchievement.Progress);
        Assert.Equal(10, userAchievement.ProgressTarget);
    }

    [Fact]
    public async Task GetUserAchievementProgressAsync_ReturnsAllAchievementsWithProgress()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var achievementId1 = Guid.NewGuid();
        var achievementId2 = Guid.NewGuid();

        var achievement1 = new Achievement
        {
            Id = achievementId1,
            Name = "Achievement 1",
            Description = "Test 1",
            Criteria = "{\"type\":\"test\",\"count\":10}"
        };

        var achievement2 = new Achievement
        {
            Id = achievementId2,
            Name = "Achievement 2",
            Description = "Test 2",
            Criteria = "{\"type\":\"test\",\"count\":5}"
        };

        var userAchievement = new UserAchievement
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            AchievementId = achievementId1,
            Progress = 5,
            ProgressTarget = 10,
            EarnedAt = DateTime.UtcNow
        };

        _context.Set<Achievement>().AddRange(achievement1, achievement2);
        _context.Set<UserAchievement>().Add(userAchievement);
        await _context.SaveChangesAsync();

        // Act
        var result = await _service.GetUserAchievementProgressAsync(userId);

        // Assert
        Assert.Equal(2, result.Count);
        
        var progress1 = result.First(p => p.Achievement.Id == achievementId1);
        Assert.True(progress1.IsEarned);
        Assert.Equal(50.0, progress1.ProgressPercentage);

        var progress2 = result.First(p => p.Achievement.Id == achievementId2);
        Assert.False(progress2.IsEarned);
        Assert.Equal(0.0, progress2.ProgressPercentage);
    }
}
