using Xunit;
using Moq;
using Microsoft.EntityFrameworkCore;
using Shared.Services;
using Shared.Entities;
using Shared.Data;
using Microsoft.Extensions.Logging;

namespace Gamification.Tests;

/// <summary>
/// Unit tests for Mission Service
/// Validates: Requirements 16.1, 16.2, 16.3
/// </summary>
public class MissionServiceTests
{
    private readonly Mock<ILogger<MissionService>> _loggerMock;
    private readonly ApplicationDbContext _context;
    private readonly MissionService _service;

    public MissionServiceTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new ApplicationDbContext(options);
        _loggerMock = new Mock<ILogger<MissionService>>();
        _service = new MissionService(_context, _loggerMock.Object);
    }

    [Fact]
    public async Task GetUserMissionsAsync_ReturnsOnlyActiveMissions()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var now = DateTime.UtcNow;

        var activeMission = new Mission
        {
            Id = Guid.NewGuid(),
            Title = "Active Mission",
            Description = "Test",
            Type = MissionType.Daily,
            Category = MissionCategory.Learning,
            Objective = "{\"type\":\"complete_lessons\",\"count\":3}",
            XPReward = 50,
            StartDate = now.AddHours(-1),
            EndDate = now.AddHours(1),
            IsActive = true
        };

        var expiredMission = new Mission
        {
            Id = Guid.NewGuid(),
            Title = "Expired Mission",
            Description = "Test",
            Type = MissionType.Daily,
            Category = MissionCategory.Learning,
            Objective = "{\"type\":\"complete_lessons\",\"count\":3}",
            XPReward = 50,
            StartDate = now.AddDays(-2),
            EndDate = now.AddDays(-1),
            IsActive = true
        };

        _context.Set<Mission>().AddRange(activeMission, expiredMission);
        await _context.SaveChangesAsync();

        // Act
        var result = await _service.GetUserMissionsAsync(userId);

        // Assert
        Assert.Single(result);
        Assert.Equal("Active Mission", result[0].Mission.Title);
    }

    [Fact]
    public async Task AssignMissionToUserAsync_CreatesUserMission()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var missionId = Guid.NewGuid();

        var mission = new Mission
        {
            Id = missionId,
            Title = "Test Mission",
            Description = "Test",
            Type = MissionType.Daily,
            Category = MissionCategory.Learning,
            Objective = "{\"type\":\"complete_lessons\",\"count\":5}",
            XPReward = 50,
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddDays(1),
            IsActive = true
        };

        _context.Set<Mission>().Add(mission);
        await _context.SaveChangesAsync();

        // Act
        var result = await _service.AssignMissionToUserAsync(userId, missionId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(userId, result.UserId);
        Assert.Equal(missionId, result.MissionId);
        Assert.Equal(0, result.Progress);
        Assert.Equal(5, result.ProgressTarget);
        Assert.False(result.IsCompleted);
    }

    [Fact]
    public async Task UpdateMissionProgressAsync_IncrementsProgress()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var missionId = Guid.NewGuid();
        var now = DateTime.UtcNow;

        var mission = new Mission
        {
            Id = missionId,
            Title = "Test Mission",
            Description = "Test",
            Type = MissionType.Daily,
            Category = MissionCategory.Learning,
            Objective = "{\"type\":\"complete_lessons\",\"count\":5}",
            XPReward = 50,
            StartDate = now.AddHours(-1),
            EndDate = now.AddHours(1),
            IsActive = true
        };

        var userMission = new UserMission
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            MissionId = missionId,
            Progress = 2,
            ProgressTarget = 5,
            IsCompleted = false,
            AssignedAt = DateTime.UtcNow
        };

        _context.Set<Mission>().Add(mission);
        _context.Set<UserMission>().Add(userMission);
        await _context.SaveChangesAsync();

        // Act
        await _service.UpdateMissionProgressAsync(userId, "Learning", 1);

        // Assert
        var updated = await _context.Set<UserMission>()
            .FirstAsync(um => um.Id == userMission.Id);

        Assert.Equal(3, updated.Progress);
        Assert.False(updated.IsCompleted);
    }

    [Fact]
    public async Task CompleteMissionAsync_AwardsXPAndMarksComplete()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var missionId = Guid.NewGuid();

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

        var mission = new Mission
        {
            Id = missionId,
            Title = "Test Mission",
            Description = "Test",
            Type = MissionType.Daily,
            Category = MissionCategory.Learning,
            Objective = "{\"type\":\"complete_lessons\",\"count\":3}",
            XPReward = 50,
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddDays(1),
            IsActive = true
        };

        var userMission = new UserMission
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            MissionId = missionId,
            Mission = mission,
            Progress = 2,
            ProgressTarget = 3,
            IsCompleted = false,
            AssignedAt = DateTime.UtcNow
        };

        _context.Users.Add(user);
        _context.Set<Mission>().Add(mission);
        _context.Set<UserMission>().Add(userMission);
        await _context.SaveChangesAsync();

        // Act
        var result = await _service.CompleteMissionAsync(userId, missionId);

        // Assert
        Assert.True(result);

        var updated = await _context.Set<UserMission>()
            .FirstAsync(um => um.Id == userMission.Id);

        Assert.True(updated.IsCompleted);
        Assert.NotNull(updated.CompletedAt);
        Assert.Equal(3, updated.Progress);

        var updatedUser = await _context.Users
            .Include(u => u.Progress)
            .FirstAsync(u => u.Id == userId);

        Assert.Equal(150, updatedUser.Progress!.TotalXP); // 100 + 50
    }

    [Fact]
    public async Task CompleteMissionAsync_AlreadyCompleted_ReturnsFalse()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var missionId = Guid.NewGuid();

        var userMission = new UserMission
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            MissionId = missionId,
            Progress = 3,
            ProgressTarget = 3,
            IsCompleted = true,
            CompletedAt = DateTime.UtcNow,
            AssignedAt = DateTime.UtcNow
        };

        _context.Set<UserMission>().Add(userMission);
        await _context.SaveChangesAsync();

        // Act
        var result = await _service.CompleteMissionAsync(userId, missionId);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task UpdateMissionProgressAsync_CompletesWhenTargetReached()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var missionId = Guid.NewGuid();
        var now = DateTime.UtcNow;

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

        var mission = new Mission
        {
            Id = missionId,
            Title = "Test Mission",
            Description = "Test",
            Type = MissionType.Daily,
            Category = MissionCategory.Learning,
            Objective = "{\"type\":\"complete_lessons\",\"count\":3}",
            XPReward = 50,
            StartDate = now.AddHours(-1),
            EndDate = now.AddHours(1),
            IsActive = true
        };

        var userMission = new UserMission
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            MissionId = missionId,
            Mission = mission,
            Progress = 2,
            ProgressTarget = 3,
            IsCompleted = false,
            AssignedAt = DateTime.UtcNow
        };

        _context.Users.Add(user);
        _context.Set<Mission>().Add(mission);
        _context.Set<UserMission>().Add(userMission);
        await _context.SaveChangesAsync();

        // Act
        await _service.UpdateMissionProgressAsync(userId, "Learning", 1);

        // Assert
        var updated = await _context.Set<UserMission>()
            .FirstAsync(um => um.Id == userMission.Id);

        Assert.True(updated.IsCompleted);
        Assert.NotNull(updated.CompletedAt);

        var updatedUser = await _context.Users
            .Include(u => u.Progress)
            .FirstAsync(u => u.Id == userId);

        Assert.Equal(150, updatedUser.Progress!.TotalXP);
    }
}
