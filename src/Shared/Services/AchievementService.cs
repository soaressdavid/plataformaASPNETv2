using Shared.Entities;
using Shared.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Shared.Services;

/// <summary>
/// Service for managing achievements and badges
/// Validates: Requirements 15.1, 15.2, 15.3, 15.4
/// </summary>
public class AchievementService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<AchievementService> _logger;

    public AchievementService(
        ApplicationDbContext context,
        ILogger<AchievementService> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Gets all available achievements
    /// </summary>
    public async Task<List<Achievement>> GetAllAchievementsAsync()
    {
        return await _context.Set<Achievement>()
            .Where(a => !a.IsDeleted)
            .OrderBy(a => a.DisplayOrder)
            .ToListAsync();
    }

    /// <summary>
    /// Gets user's earned achievements
    /// </summary>
    public async Task<List<UserAchievement>> GetUserAchievementsAsync(Guid userId)
    {
        return await _context.Set<UserAchievement>()
            .Include(ua => ua.Achievement)
            .Where(ua => ua.UserId == userId && !ua.IsDeleted)
            .OrderByDescending(ua => ua.EarnedAt)
            .ToListAsync();
    }

    /// <summary>
    /// Gets user's achievement progress
    /// </summary>
    public async Task<List<AchievementProgress>> GetUserAchievementProgressAsync(Guid userId)
    {
        var allAchievements = await GetAllAchievementsAsync();
        var earnedAchievements = await GetUserAchievementsAsync(userId);
        var earnedIds = earnedAchievements.Select(ua => ua.AchievementId).ToHashSet();

        var progress = new List<AchievementProgress>();

        foreach (var achievement in allAchievements)
        {
            var userAchievement = earnedAchievements.FirstOrDefault(ua => ua.AchievementId == achievement.Id);
            
            progress.Add(new AchievementProgress
            {
                Achievement = achievement,
                IsEarned = earnedIds.Contains(achievement.Id),
                EarnedAt = userAchievement?.EarnedAt,
                Progress = userAchievement?.Progress ?? 0,
                ProgressTarget = userAchievement?.ProgressTarget ?? GetProgressTarget(achievement),
                ProgressPercentage = CalculateProgressPercentage(userAchievement, achievement)
            });
        }

        return progress;
    }

    /// <summary>
    /// Checks and awards achievements based on user activity
    /// </summary>
    public async Task CheckAndAwardAchievementsAsync(Guid userId, string activityType, int activityCount = 1)
    {
        var user = await _context.Users
            .Include(u => u.Progress)
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
            return;

        var achievements = await _context.Set<Achievement>()
            .Where(a => !a.IsDeleted && a.Category == activityType)
            .ToListAsync();

        foreach (var achievement in achievements)
        {
            var criteria = ParseCriteria(achievement.Criteria);
            
            if (await MeetsCriteriaAsync(userId, criteria, activityCount))
            {
                await AwardAchievementAsync(userId, achievement.Id);
            }
        }
    }

    /// <summary>
    /// Awards an achievement to a user
    /// </summary>
    public async Task<bool> AwardAchievementAsync(Guid userId, Guid achievementId)
    {
        // Check if already earned
        var existing = await _context.Set<UserAchievement>()
            .FirstOrDefaultAsync(ua => ua.UserId == userId && ua.AchievementId == achievementId);

        if (existing != null)
        {
            _logger.LogInformation("User {UserId} already has achievement {AchievementId}", userId, achievementId);
            return false;
        }

        var achievement = await _context.Set<Achievement>()
            .FirstOrDefaultAsync(a => a.Id == achievementId);

        if (achievement == null)
            return false;

        var userAchievement = new UserAchievement
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            AchievementId = achievementId,
            EarnedAt = DateTime.UtcNow,
            Progress = GetProgressTarget(achievement),
            ProgressTarget = GetProgressTarget(achievement)
        };

        _context.Set<UserAchievement>().Add(userAchievement);

        // Award XP
        var user = await _context.Users
            .Include(u => u.Progress)
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user?.Progress != null)
        {
            user.Progress.TotalXP += achievement.XPReward;
            user.Progress.CurrentLevel = XPCalculator.CalculateLevel(user.Progress.TotalXP);
        }

        await _context.SaveChangesAsync();

        _logger.LogInformation("Awarded achievement {AchievementId} to user {UserId}, XP: {XP}", 
            achievementId, userId, achievement.XPReward);

        return true;
    }

    /// <summary>
    /// Updates progress towards an achievement
    /// </summary>
    public async Task UpdateAchievementProgressAsync(Guid userId, Guid achievementId, int progress)
    {
        var userAchievement = await _context.Set<UserAchievement>()
            .FirstOrDefaultAsync(ua => ua.UserId == userId && ua.AchievementId == achievementId);

        if (userAchievement == null)
        {
            var achievement = await _context.Set<Achievement>()
                .FirstOrDefaultAsync(a => a.Id == achievementId);

            if (achievement == null)
                return;

            userAchievement = new UserAchievement
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                AchievementId = achievementId,
                Progress = progress,
                ProgressTarget = GetProgressTarget(achievement),
                EarnedAt = DateTime.UtcNow
            };

            _context.Set<UserAchievement>().Add(userAchievement);
        }
        else
        {
            userAchievement.Progress = progress;
        }

        // Check if achievement is complete
        if (userAchievement.Progress >= userAchievement.ProgressTarget)
        {
            await AwardAchievementAsync(userId, achievementId);
        }

        await _context.SaveChangesAsync();
    }

    private Dictionary<string, object> ParseCriteria(string criteriaJson)
    {
        try
        {
            return JsonSerializer.Deserialize<Dictionary<string, object>>(criteriaJson) 
                ?? new Dictionary<string, object>();
        }
        catch
        {
            return new Dictionary<string, object>();
        }
    }

    private async Task<bool> MeetsCriteriaAsync(Guid userId, Dictionary<string, object> criteria, int activityCount)
    {
        if (!criteria.ContainsKey("type") || !criteria.ContainsKey("count"))
            return false;

        var type = criteria["type"].ToString();
        var requiredCount = int.Parse(criteria["count"].ToString() ?? "0");

        // This is a simplified check - in production, you'd query actual user data
        return activityCount >= requiredCount;
    }

    private int GetProgressTarget(Achievement achievement)
    {
        var criteria = ParseCriteria(achievement.Criteria);
        if (criteria.ContainsKey("count"))
        {
            return int.Parse(criteria["count"].ToString() ?? "1");
        }
        return 1;
    }

    private double CalculateProgressPercentage(UserAchievement? userAchievement, Achievement achievement)
    {
        if (userAchievement == null)
            return 0;

        if (userAchievement.ProgressTarget == 0)
            return 0;

        return Math.Min(100, (userAchievement.Progress * 100.0) / userAchievement.ProgressTarget);
    }
}

public class AchievementProgress
{
    public Achievement Achievement { get; set; } = null!;
    public bool IsEarned { get; set; }
    public DateTime? EarnedAt { get; set; }
    public int Progress { get; set; }
    public int ProgressTarget { get; set; }
    public double ProgressPercentage { get; set; }
}
