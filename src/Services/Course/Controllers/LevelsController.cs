using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Course.Service.DTOs;
using Course.Service.Extensions;
using Course.Service.Services;
using Shared.Data;

namespace Course.Service.Controllers;

/// <summary>
/// Controller for managing curriculum levels
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class LevelsController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly CachedLevelsService _cachedLevelsService;
    private readonly ILogger<LevelsController> _logger;

    public LevelsController(
        ApplicationDbContext context, 
        CachedLevelsService cachedLevelsService,
        ILogger<LevelsController> logger)
    {
        _context = context;
        _cachedLevelsService = cachedLevelsService;
        _logger = logger;
    }

    /// <summary>
    /// Get all curriculum levels ordered by number
    /// </summary>
    /// <returns>List of all curriculum levels</returns>
    [HttpGet]
    [ProducesResponseType(typeof(LevelListResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<LevelListResponse>> GetAll()
    {
        try
        {
            _logger.LogInformation("Fetching all curriculum levels");

            var levels = await _cachedLevelsService.GetAllLevelsAsync();

            var levelDtos = levels.Select(l => l.ToLevelDto()).ToList();

            return Ok(new LevelListResponse(levelDtos));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching curriculum levels");
            return StatusCode(500, new { error = "An error occurred while fetching levels" });
        }
    }

    /// <summary>
    /// Get a specific level with its courses
    /// </summary>
    /// <param name="id">Level ID</param>
    /// <returns>Level details with courses</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(LevelDetailDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<LevelDetailDto>> GetById(Guid id)
    {
        try
        {
            _logger.LogInformation("Fetching level {LevelId}", id);

            var level = await _context.Set<Shared.Entities.CurriculumLevel>()
                .Include(l => l.Courses)
                .Include(l => l.Project)
                .FirstOrDefaultAsync(l => l.Id == id);

            if (level == null)
            {
                _logger.LogWarning("Level {LevelId} not found", id);
                return NotFound(new { message = "Level not found" });
            }

            var levelDetailDto = level.ToLevelDetailDto();

            return Ok(levelDetailDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching level {LevelId}", id);
            return StatusCode(500, new { error = "An error occurred while fetching level details" });
        }
    }

    /// <summary>
    /// Get all courses for a specific level
    /// </summary>
    /// <param name="id">Level ID</param>
    /// <returns>List of courses for the level</returns>
    [HttpGet("{id}/courses")]
    [ProducesResponseType(typeof(CourseListResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CourseListResponse>> GetCourses(Guid id)
    {
        try
        {
            _logger.LogInformation("Fetching courses for level {LevelId}", id);

            // First check if the level exists
            var levelExists = await _context.Set<Shared.Entities.CurriculumLevel>()
                .AnyAsync(l => l.Id == id);

            if (!levelExists)
            {
                _logger.LogWarning("Level {LevelId} not found", id);
                return NotFound(new { message = "Level not found" });
            }

            var courses = await _context.Courses
                .Where(c => c.LevelId == id)
                .OrderBy(c => c.OrderIndex)
                .ToListAsync();

            var courseDtos = courses.Select(c => c.ToCourseSummaryDto()).ToList();

            return Ok(new CourseListResponse(courseDtos));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching courses for level {LevelId}", id);
            return StatusCode(500, new { error = "An error occurred while fetching courses" });
        }
    }

    /// <summary>
    /// Invalidate the levels cache (admin endpoint)
    /// </summary>
    /// <returns>Success message</returns>
    [HttpPost("cache/invalidate")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult InvalidateCache()
    {
        try
        {
            _logger.LogInformation("Invalidating levels cache");
            _cachedLevelsService.InvalidateCache();
            return Ok(new { message = "Cache invalidated successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error invalidating cache");
            return StatusCode(500, new { error = "An error occurred while invalidating cache" });
        }
    }
}
