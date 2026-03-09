using StackExchange.Redis;

namespace Shared.Services;

/// <summary>
/// Cache service for leaderboard data using Redis sorted sets
/// Validates: Requirements 22.2, 22.5 - Cache leaderboard data with 5 min TTL
/// </summary>
public interface ILeaderboardCacheService
{
    Task UpdateUserScoreAsync(string userId, int totalXP, CancellationToken cancellationToken = default);
    Task<LeaderboardEntry[]> GetTopUsersAsync(int count = 100, CancellationToken cancellationToken = default);
    Task<LeaderboardEntry[]> GetWeeklyTopUsersAsync(int count = 100, CancellationToken cancellationToken = default);
    Task<LeaderboardEntry[]> GetMonthlyTopUsersAsync(int count = 100, CancellationToken cancellationToken = default);
    Task<long?> GetUserRankAsync(string userId, CancellationToken cancellationToken = default);
}

public class LeaderboardEntry
{
    public int Rank { get; set; }
    public string UserId { get; set; } = string.Empty;
    public int TotalXP { get; set; }
}

public class LeaderboardCacheService : ILeaderboardCacheService
{
    private readonly IRedisCacheService _cache;
    private readonly TimeSpan _ttl;
    private const string GlobalKey = "leaderboard:global";
    private const string WeeklyKeyPrefix = "leaderboard:week:";
    private const string MonthlyKeyPrefix = "leaderboard:month:";

    public LeaderboardCacheService(IRedisCacheService cache, Configuration.RedisConfiguration config)
    {
        _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        _ttl = config.TTL.LeaderboardData;
    }

    /// <summary>
    /// Updates user score in all leaderboards
    /// Validates: Requirement 22.5 - Leaderboard data caching
    /// </summary>
    public async Task UpdateUserScoreAsync(string userId, int totalXP, CancellationToken cancellationToken = default)
    {
        // Update global leaderboard
        await _cache.SortedSetAddAsync(GlobalKey, userId, totalXP, cancellationToken);
        
        // Update weekly leaderboard
        var weekKey = $"{WeeklyKeyPrefix}{GetCurrentWeek()}";
        await _cache.SortedSetAddAsync(weekKey, userId, totalXP, cancellationToken);
        
        // Update monthly leaderboard
        var monthKey = $"{MonthlyKeyPrefix}{DateTime.UtcNow:yyyy-MM}";
        await _cache.SortedSetAddAsync(monthKey, userId, totalXP, cancellationToken);
    }

    /// <summary>
    /// Gets top users from global leaderboard
    /// Validates: Requirement 22.5 - Cache TTL to 5 minutes for leaderboard data
    /// </summary>
    public async Task<LeaderboardEntry[]> GetTopUsersAsync(int count = 100, CancellationToken cancellationToken = default)
    {
        var entries = await _cache.SortedSetRangeByRankWithScoresAsync(
            GlobalKey, 
            0, 
            count - 1, 
            Order.Descending,
            cancellationToken
        );
        
        return entries.Select((e, i) => new LeaderboardEntry
        {
            Rank = i + 1,
            UserId = e.Element.ToString(),
            TotalXP = (int)e.Score
        }).ToArray();
    }

    /// <summary>
    /// Gets top users from weekly leaderboard
    /// </summary>
    public async Task<LeaderboardEntry[]> GetWeeklyTopUsersAsync(int count = 100, CancellationToken cancellationToken = default)
    {
        var weekKey = $"{WeeklyKeyPrefix}{GetCurrentWeek()}";
        var entries = await _cache.SortedSetRangeByRankWithScoresAsync(
            weekKey, 
            0, 
            count - 1, 
            Order.Descending,
            cancellationToken
        );
        
        return entries.Select((e, i) => new LeaderboardEntry
        {
            Rank = i + 1,
            UserId = e.Element.ToString(),
            TotalXP = (int)e.Score
        }).ToArray();
    }

    /// <summary>
    /// Gets top users from monthly leaderboard
    /// </summary>
    public async Task<LeaderboardEntry[]> GetMonthlyTopUsersAsync(int count = 100, CancellationToken cancellationToken = default)
    {
        var monthKey = $"{MonthlyKeyPrefix}{DateTime.UtcNow:yyyy-MM}";
        var entries = await _cache.SortedSetRangeByRankWithScoresAsync(
            monthKey, 
            0, 
            count - 1, 
            Order.Descending,
            cancellationToken
        );
        
        return entries.Select((e, i) => new LeaderboardEntry
        {
            Rank = i + 1,
            UserId = e.Element.ToString(),
            TotalXP = (int)e.Score
        }).ToArray();
    }

    /// <summary>
    /// Gets user's rank in global leaderboard
    /// </summary>
    public async Task<long?> GetUserRankAsync(string userId, CancellationToken cancellationToken = default)
    {
        // Note: This would need to be implemented using the underlying IDatabase
        // For now, returning null as a placeholder
        return null;
    }

    private string GetCurrentWeek()
    {
        var now = DateTime.UtcNow;
        var jan1 = new DateTime(now.Year, 1, 1);
        var daysOffset = DayOfWeek.Monday - jan1.DayOfWeek;
        var firstMonday = jan1.AddDays(daysOffset);
        var weekNumber = ((now - firstMonday).Days / 7) + 1;
        return $"{now.Year}-W{weekNumber:D2}";
    }
}
