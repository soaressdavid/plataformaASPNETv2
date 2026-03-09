using Microsoft.EntityFrameworkCore;
using Shared.Data;
using Shared.Entities;
using Shared.Interfaces;

namespace Course.Service.Services;

/// <summary>
/// Service for managing courses, lessons, enrollments, and lesson completions.
/// </summary>
public class CourseService
{
    private readonly ICourseRepository _courseRepository;
    private readonly ApplicationDbContext _context;

    public CourseService(ICourseRepository courseRepository, ApplicationDbContext context)
    {
        _courseRepository = courseRepository;
        _context = context;
    }

    /// <summary>
    /// Gets all courses.
    /// </summary>
    public async Task<List<Shared.Entities.Course>> GetAllCoursesAsync()
    {
        var courses = await _courseRepository.GetAllAsync();
        return courses.ToList();
    }

    /// <summary>
    /// Gets a course by ID with its lessons.
    /// </summary>
    public async Task<Shared.Entities.Course?> GetCourseByIdAsync(Guid courseId)
    {
        return await _courseRepository.GetByIdAsync(courseId);
    }

    /// <summary>
    /// Gets all lessons for a course, ordered by OrderIndex.
    /// </summary>
    public async Task<List<Lesson>> GetLessonsAsync(Guid courseId)
    {
        return await _courseRepository.GetLessonsAsync(courseId);
    }

    /// <summary>
    /// Enrolls a user in a course.
    /// </summary>
    public async Task<Enrollment> EnrollAsync(Guid userId, Guid courseId)
    {
        // Check if already enrolled
        var existingEnrollment = await _context.Enrollments
            .FirstOrDefaultAsync(e => e.UserId == userId && e.CourseId == courseId);

        if (existingEnrollment != null)
        {
            return existingEnrollment;
        }

        // Create new enrollment
        var enrollment = new Enrollment
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            CourseId = courseId,
            EnrolledAt = DateTime.UtcNow,
            LastAccessedAt = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Enrollments.Add(enrollment);
        await _context.SaveChangesAsync();

        return enrollment;
    }

    /// <summary>
    /// Marks a lesson as complete for a user and returns the next lesson ID if available.
    /// </summary>
    public async Task<(bool Success, Guid? NextLessonId)> CompleteLessonAsync(Guid userId, Guid lessonId)
    {
        // Check if already completed
        var existingCompletion = await _context.LessonCompletions
            .FirstOrDefaultAsync(lc => lc.UserId == userId && lc.LessonId == lessonId);

        if (existingCompletion == null)
        {
            // Create lesson completion
            var completion = new LessonCompletion
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                LessonId = lessonId,
                CompletedAt = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.LessonCompletions.Add(completion);
            await _context.SaveChangesAsync();
        }

        // Find the next lesson
        var currentLesson = await _context.Lessons
            .FirstOrDefaultAsync(l => l.Id == lessonId);

        if (currentLesson == null)
        {
            return (false, null);
        }

        var nextLesson = await _context.Lessons
            .Where(l => l.CourseId == currentLesson.CourseId && l.OrderIndex > currentLesson.OrderIndex)
            .OrderBy(l => l.OrderIndex)
            .FirstOrDefaultAsync();

        return (true, nextLesson?.Id);
    }
}
