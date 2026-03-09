using Shared.Entities;
using Shared.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Shared.Services;

/// <summary>
/// Service for managing daily and weekly missions
/// Validates: Requirements 16.1, 16.2, 16.3
/// </summary>
public class MissionService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<MissionService> _logger;

    public MissionService(
        ApplicationDbContext context,
        ILogger<MissionService> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Gets active missions for a user
    /// </summary>
    public async Task<List<UserMissionProgress>> GetUserMissionsAsync(Guid userId)
    {
        var now = DateTime.UtcNow;
        
        var activeMissions = await _context.Set<Mission>()
            .Where(m => m.IsActive && !m.IsDeleted && m.StartDate <= now && m.EndDate >= now)
            .ToListAsync();

        var userMissions = await _context.Set<UserMission>()
            .Include(um => um.Mission)
            .Where(um => um.UserId == userId && !um.IsDeleted)
            .ToListAsync();

        var progress = new List<UserMissionProgress>();

        foreach (var mission in activeMissions)
        {
            var userMission = userMissions.FirstOrDefault(um => um.MissionId == mission.Id);
            
            if (userMission == null)
            {
                // Assign mission to user
                userMission = await AssignMissionToUserAsync(userId, mission.Id);
            }

            progress.Add(new UserMissionProgress
            {
                Mission = mission,
                Progress = userMission?.Progress ?? 0,
                ProgressTarget = userMission?.ProgressTarget ?? GetProgressTarget(mission),
                IsCompleted = userMission?.IsCompleted ?? false,
                CompletedAt = userMission?.CompletedAt,
                ProgressPercentage = CalculateProgressPercentage(userMission, mission)
            });
        }

        return progress;
    }

    /// <summary>
    /// Assigns a mission to a user
    /// </summary>
    public async Task<UserMission> AssignMissionToUserAsync(Guid userId, Guid missionId)
    {
        var mission = await _context.Set<Mission>()
            .FirstOrDefaultAsync(m => m.Id == missionId);

        if (mission == null)
            throw new InvalidOperationException("Mission not found");

        var userMission = new UserMission
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            MissionId = missionId,
            Progress = 0,
            ProgressTarget = GetProgressTarget(mission),
            IsCompleted = false,
            AssignedAt = DateTime.UtcNow
        };

        _context.Set<UserMission>().Add(userMission);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Assigned mission {MissionId} to user {UserId}", missionId, userId);

        return userMission;
    }

    /// <summary>
    /// Updates mission progress
    /// </summary>
    public async Task UpdateMissionProgressAsync(Guid userId, string missionType, int progressIncrement = 1)
    {
        var now = DateTime.UtcNow;
        
        var activeMissions = await _context.Set<Mission>()
            .Where(m => m.IsActive && !m.IsDeleted && 
                       m.StartDate <= now && m.EndDate >= now &&
                       m.Category.ToString() == missionType)
            .ToListAsync();

        foreach (var mission in activeMissions)
        {
            var userMission = await _context.Set<UserMission>()
                .FirstOrDefaultAsync(um => um.UserId == userId && um.MissionId == mission.Id);

            if (userMission == null)
            {
                userMission = await AssignMissionToUserAsync(userId, mission.Id);
            }

            if (!userMission.IsCompleted)
            {
                userMission.Progress += progressIncrement;

                if (userMission.Progress >= userMission.ProgressTarget)
                {
                    await CompleteMissionAsync(userId, mission.Id);
                }
                else
                {
                    await _context.SaveChangesAsync();
                }
            }
        }
    }

    /// <summary>
    /// Completes a mission and awards XP
    /// </summary>
    public async Task<bool> CompleteMissionAsync(Guid userId, Guid missionId)
    {
        var userMission = await _context.Set<UserMission>()
            .Include(um => um.Mission)
            .FirstOrDefaultAsync(um => um.UserId == userId && um.MissionId == missionId);

        if (userMission == null || userMission.IsCompleted)
            return false;

        userMission.IsCompleted = true;
        userMission.CompletedAt = DateTime.UtcNow;
        userMission.Progress = userMission.ProgressTarget;

        // Award XP
        var user = await _context.Users
            .Include(u => u.Progress)
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user?.Progress != null)
        {
            user.Progress.TotalXP += userMission.Mission.XPReward;
            user.Progress.CurrentLevel = XPCalculator.CalculateLevel(user.Progress.TotalXP);
        }

        await _context.SaveChangesAsync();

        _logger.LogInformation("User {UserId} completed mission {MissionId}, XP: {XP}", 
            userId, missionId, userMission.Mission.XPReward);

        return true;
    }

    /// <summary>
    /// Resets daily missions (called by background job)
    /// </summary>
    public async Task ResetDailyMissionsAsync()
    {
        var yesterday = DateTime.UtcNow.AddDays(-1);
        
        // Mark expired daily missions as inactive
        var expiredMissions = await _context.Set<Mission>()
            .Where(m => m.Type == MissionType.Daily && m.EndDate < yesterday)
            .ToListAsync();

        foreach (var mission in expiredMissions)
        {
            mission.IsActive = false;
        }

        // Create new daily missions
        await CreateDailyMissionsAsync();

        await _context.SaveChangesAsync();

        _logger.LogInformation("Reset daily missions");
    }

    /// <summary>
    /// Resets weekly missions (called by background job)
    /// </summary>
    public async Task ResetWeeklyMissionsAsync()
    {
        var lastWeek = DateTime.UtcNow.AddDays(-7);
        
        // Mark expired weekly missions as inactive
        var expiredMissions = await _context.Set<Mission>()
            .Where(m => m.Type == MissionType.Weekly && m.EndDate < lastWeek)
            .ToListAsync();

        foreach (var mission in expiredMissions)
        {
            mission.IsActive = false;
        }

        // Create new weekly missions
        await CreateWeeklyMissionsAsync();

        await _context.SaveChangesAsync();

        _logger.LogInformation("Reset weekly missions");
    }

    private async Task CreateDailyMissionsAsync()
    {
        var today = DateTime.UtcNow.Date;
        var tomorrow = today.AddDays(1);

        var dailyMissions = new[]
        {
            new Mission
            {
                Id = Guid.NewGuid(),
                Title = "Complete 3 Lessons",
                Description = "Complete any 3 lessons today",
                Type = MissionType.Daily,
                Category = MissionCategory.Learning,
                Objective = JsonSerializer.Serialize(new { type = "complete_lessons", count = 3 }),
                XPReward = 50,
                StartDate = today,
                EndDate = tomorrow,
                IsActive = true
            },
            new Mission
            {
                Id = Guid.NewGuid(),
                Title = "Solve 2 Challenges",
                Description = "Successfully solve 2 coding challenges",
                Type = MissionType.Daily,
                Category = MissionCategory.Challenge,
                Objective = JsonSerializer.Serialize(new { type = "solve_challenges", count = 2 }),
                XPReward = 75,
                StartDate = today,
                EndDate = tomorrow,
                IsActive = true
            }
        };

        _context.Set<Mission>().AddRange(dailyMissions);
    }

    private async Task CreateWeeklyMissionsAsync()
    {
        var today = DateTime.UtcNow.Date;
        var nextWeek = today.AddDays(7);

        var weeklyMissions = new[]
        {
            new Mission
            {
                Id = Guid.NewGuid(),
                Title = "Complete 10 Lessons",
                Description = "Complete 10 lessons this week",
                Type = MissionType.Weekly,
                Category = MissionCategory.Learning,
                Objective = JsonSerializer.Serialize(new { type = "complete_lessons", count = 10 }),
                XPReward = 200,
                StartDate = today,
                EndDate = nextWeek,
                IsActive = true
            },
            new Mission
            {
                Id = Guid.NewGuid(),
                Title = "Maintain 7-Day Streak",
                Description = "Keep your learning streak alive for 7 days",
                Type = MissionType.Weekly,
                Category = MissionCategory.Practice,
                Objective = JsonSerializer.Serialize(new { type = "maintain_streak", count = 7 }),
                XPReward = 300,
                StartDate = today,
                EndDate = nextWeek,
                IsActive = true
            }
        };

        _context.Set<Mission>().AddRange(weeklyMissions);
    }

    private int GetProgressTarget(Mission mission)
    {
        try
        {
            var objective = JsonSerializer.Deserialize<Dictionary<string, object>>(mission.Objective);
            if (objective != null && objective.ContainsKey("count"))
            {
                return int.Parse(objective["count"].ToString() ?? "1");
            }
        }
        catch
        {
            // Fallback
        }
        return 1;
    }

    private double CalculateProgressPercentage(UserMission? userMission, Mission mission)
    {
        if (userMission == null)
            return 0;

        if (userMission.ProgressTarget == 0)
            return 0;

        return Math.Min(100, (userMission.Progress * 100.0) / userMission.ProgressTarget);
    }
}

public class UserMissionProgress
{
    public Mission Mission { get; set; } = null!;
    public int Progress { get; set; }
    public int ProgressTarget { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime? CompletedAt { get; set; }
    public double ProgressPercentage { get; set; }
}
