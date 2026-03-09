using Microsoft.Extensions.Caching.Memory;
using Microsoft.EntityFrameworkCore;
using Shared.Data;
using Shared.Entities;

namespace Course.Service.Services;

/// <summary>
/// Service for caching curriculum levels to improve performance
/// </summary>
public class CachedLevelsService
{
    private readonly IMemoryCache _cache;
    private readonly ApplicationDbContext _context;
    private readonly ILogger<CachedLevelsService> _logger;
    private const string LEVELS_CACHE_KEY = "curriculum_levels";
    private static readonly TimeSpan CacheDuration = TimeSpan.FromHours(24);

    public CachedLevelsService(
        IMemoryCache cache, 
        ApplicationDbContext context,
        ILogger<CachedLevelsService> logger)
    {
        _cache = cache;
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Get all curriculum levels from cache or database
    /// </summary>
    /// <returns>List of curriculum levels with courses</returns>
    public async Task<List<CurriculumLevel>> GetAllLevelsAsync()
    {
        if (!_cache.TryGetValue(LEVELS_CACHE_KEY, out List<CurriculumLevel>? levels))
        {
            _logger.LogInformation("Cache miss for curriculum levels, fetching from database");

            levels = await _context.Set<CurriculumLevel>()
                .Include(l => l.Courses)
                .OrderBy(l => l.Number)
                .ToListAsync();

            var cacheOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(CacheDuration)
                .SetPriority(CacheItemPriority.High);

            _cache.Set(LEVELS_CACHE_KEY, levels, cacheOptions);

            _logger.LogInformation("Cached {Count} curriculum levels for {Duration} hours", 
                levels.Count, CacheDuration.TotalHours);
        }
        else
        {
            _logger.LogDebug("Cache hit for curriculum levels");
        }

        return levels ?? new List<CurriculumLevel>();
    }

    /// <summary>
    /// Invalidate the levels cache
    /// </summary>
    public void InvalidateCache()
    {
        _cache.Remove(LEVELS_CACHE_KEY);
        _logger.LogInformation("Curriculum levels cache invalidated");
    }
}
