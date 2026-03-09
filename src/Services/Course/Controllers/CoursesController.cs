using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Course.Service.DTOs;
using Course.Service.Extensions;
using Shared.Data;

namespace Course.Service.Controllers;

/// <summary>
/// Controller for managing courses and lessons
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class CoursesController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<CoursesController> _logger;

    public CoursesController(ApplicationDbContext context, ILogger<CoursesController> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Get all courses with optional filters
    /// </summary>
    /// <param name="levelId">Optional curriculum level ID filter</param>
    /// <param name="level">Optional difficulty level filter (Beginner, Intermediate, Advanced)</param>
    /// <returns>List of courses</returns>
    [HttpGet]
    public async Task<ActionResult<CourseListResponse>> GetAll(
        [FromQuery] Guid? levelId = null,
        [FromQuery] string? level = null)
    {
        try
        {
            var query = _context.Courses.AsQueryable();

            // Apply filters
            if (levelId.HasValue)
            {
                query = query.Where(c => c.LevelId == levelId);
            }

            if (!string.IsNullOrEmpty(level))
            {
                query = query.Where(c => c.Level.ToString() == level);
            }

            var courses = await query
                .OrderBy(c => c.OrderIndex)
                .ToListAsync();

            var courseDtos = courses.Select(c => c.ToCourseSummaryDto()).ToList();

            return Ok(new CourseListResponse(courseDtos));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving courses");
            return StatusCode(500, new { message = "An error occurred while retrieving courses" });
        }
    }

    /// <summary>
    /// Get a specific course by ID with full details
    /// </summary>
    /// <param name="id">Course ID</param>
    /// <returns>Course details including lessons</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<CourseDetailDto>> GetById(Guid id)
    {
        try
        {
            var course = await _context.Courses
                .Include(c => c.CurriculumLevel)
                .Include(c => c.Lessons.OrderBy(l => l.OrderIndex))
                .FirstOrDefaultAsync(c => c.Id == id);

            if (course == null)
            {
                return NotFound(new { message = "Course not found" });
            }

            var courseDto = course.ToCourseDetailDto();
            return Ok(courseDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving course {CourseId}", id);
            return StatusCode(500, new { message = "An error occurred while retrieving the course" });
        }
    }

    /// <summary>
    /// Get all lessons for a specific course
    /// </summary>
    /// <param name="id">Course ID</param>
    /// <returns>List of lessons</returns>
    [HttpGet("{id}/lessons")]
    public async Task<ActionResult<LessonListResponse>> GetLessons(Guid id)
    {
        try
        {
            // Verify course exists
            var courseExists = await _context.Courses.AnyAsync(c => c.Id == id);
            if (!courseExists)
            {
                return NotFound(new { message = "Course not found" });
            }

            var lessons = await _context.Lessons
                .Where(l => l.CourseId == id)
                .OrderBy(l => l.OrderIndex)
                .ToListAsync();

            var lessonDtos = lessons.Select(l => l.ToLessonSummaryDto()).ToList();

            return Ok(new LessonListResponse(lessonDtos));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving lessons for course {CourseId}", id);
            return StatusCode(500, new { message = "An error occurred while retrieving lessons" });
        }
    }

    /// <summary>
    /// Get a specific lesson with full content
    /// </summary>
    /// <param name="courseId">Course ID</param>
    /// <param name="lessonId">Lesson ID</param>
    /// <returns>Lesson details with structured content</returns>
    [HttpGet("{courseId}/lessons/{lessonId}")]
    public async Task<ActionResult<LessonDetailDto>> GetLesson(Guid courseId, Guid lessonId)
    {
        try
        {
            var lesson = await _context.Lessons
                .Include(l => l.Course)
                .FirstOrDefaultAsync(l => l.Id == lessonId && l.CourseId == courseId);

            if (lesson == null)
            {
                return NotFound(new { message = "Lesson not found" });
            }

            var lessonDto = lesson.ToLessonDetailDto();
            return Ok(lessonDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving lesson {LessonId} for course {CourseId}", lessonId, courseId);
            return StatusCode(500, new { message = "An error occurred while retrieving the lesson" });
        }
    }

    /// <summary>
    /// Mark a lesson as complete
    /// </summary>
    /// <param name="courseId">Course ID</param>
    /// <param name="lessonId">Lesson ID</param>
    /// <returns>Completion result</returns>
    [HttpPost("{courseId}/lessons/{lessonId}/complete")]
    public async Task<ActionResult> CompleteLesson(Guid courseId, Guid lessonId)
    {
        try
        {
            // Verify lesson exists and belongs to the course
            var lesson = await _context.Lessons
                .FirstOrDefaultAsync(l => l.Id == lessonId && l.CourseId == courseId);

            if (lesson == null)
            {
                return NotFound(new { message = "Lesson not found" });
            }

            // TODO: Implement actual lesson completion logic with user context
            // For now, return a mock response
            return Ok(new
            {
                success = true,
                message = "Aula concluída com sucesso!",
                xpAwarded = 50,
                nextLessonId = (Guid?)null // Will be set based on lesson order
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error completing lesson {LessonId} for course {CourseId}", lessonId, courseId);
            return StatusCode(500, new { message = "An error occurred while completing the lesson" });
        }
    }
}
