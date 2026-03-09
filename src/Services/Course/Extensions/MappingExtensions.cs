using System.Text.Json;
using Course.Service.DTOs;
using Shared.Entities;
using Shared.Models;

namespace Course.Service.Extensions;

/// <summary>
/// Extension methods for mapping entities to DTOs
/// </summary>
public static class MappingExtensions
{
    /// <summary>
    /// Maps a CurriculumLevel entity to LevelDto
    /// </summary>
    public static LevelDto ToLevelDto(this CurriculumLevel level)
    {
        return new LevelDto(
            level.Id,
            level.Number,
            level.Title,
            level.Description,
            level.RequiredXP,
            level.Courses?.Count ?? 0,
            EstimateHours(level.Courses)
        );
    }

    /// <summary>
    /// Maps a CurriculumLevel entity to LevelDetailDto
    /// </summary>
    public static LevelDetailDto ToLevelDetailDto(this CurriculumLevel level)
    {
        var courses = level.Courses?
            .OrderBy(c => c.OrderIndex)
            .Select(c => c.ToCourseSummaryDto())
            .ToList() ?? new List<CourseSummaryDto>();

        ProjectSummaryDto? project = level.Project != null
            ? new ProjectSummaryDto(
                level.Project.Id,
                level.Project.Title,
                level.Project.Description)
            : null;

        return new LevelDetailDto(
            level.Id,
            level.Number,
            level.Title,
            level.Description,
            level.RequiredXP,
            courses,
            project
        );
    }

    /// <summary>
    /// Maps a Course entity to CourseSummaryDto
    /// </summary>
    public static CourseSummaryDto ToCourseSummaryDto(this Shared.Entities.Course course)
    {
        var topics = DeserializeJsonArray(course.Topics);

        return new CourseSummaryDto(
            course.Id,
            course.Title,
            course.Description,
            course.Level.ToString(),
            course.LevelId,
            course.Duration,
            course.LessonCount,
            topics,
            course.OrderIndex
        );
    }

    /// <summary>
    /// Maps a Course entity to CourseDetailDto
    /// </summary>
    public static CourseDetailDto ToCourseDetailDto(this Shared.Entities.Course course)
    {
        var topics = DeserializeJsonArray(course.Topics);
        
        var lessons = course.Lessons?
            .OrderBy(l => l.OrderIndex)
            .Select(l => l.ToLessonSummaryDto())
            .ToList() ?? new List<LessonSummaryDto>();

        return new CourseDetailDto(
            course.Id,
            course.Title,
            course.Description,
            course.Level.ToString(),
            course.LevelId,
            course.CurriculumLevel?.Title,
            course.Duration,
            course.LessonCount,
            topics,
            lessons
        );
    }

    /// <summary>
    /// Maps a Lesson entity to LessonSummaryDto
    /// </summary>
    public static LessonSummaryDto ToLessonSummaryDto(this Lesson lesson)
    {
        return new LessonSummaryDto(
            lesson.Id,
            lesson.Title,
            lesson.Duration,
            lesson.Difficulty,
            lesson.EstimatedMinutes,
            lesson.OrderIndex,
            false // TODO: Check user completion status from context
        );
    }

    /// <summary>
    /// Maps a Lesson entity to LessonDetailDto
    /// </summary>
    public static LessonDetailDto ToLessonDetailDto(this Lesson lesson)
    {
        LessonContentDto? structuredContent = null;

        // Deserialize structured content if available
        if (!string.IsNullOrEmpty(lesson.StructuredContent))
        {
            try
            {
                var content = JsonSerializer.Deserialize<LessonContent>(
                    lesson.StructuredContent,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                
                structuredContent = content?.ToLessonContentDto();
            }
            catch (JsonException)
            {
                // If deserialization fails, structured content will remain null
                // and the frontend will fall back to HTML content
            }
        }

        var prerequisites = DeserializeJsonArray(lesson.Prerequisites);

        return new LessonDetailDto(
            lesson.Id,
            lesson.Title,
            lesson.Content,
            structuredContent,
            lesson.Duration,
            lesson.Difficulty,
            lesson.EstimatedMinutes,
            prerequisites,
            lesson.OrderIndex,
            false // TODO: Check user completion status from context
        );
    }

    /// <summary>
    /// Maps a LessonContent model to LessonContentDto
    /// </summary>
    public static LessonContentDto ToLessonContentDto(this LessonContent content)
    {
        return new LessonContentDto(
            content.Objectives ?? new List<string>(),
            content.Theory?
                .Select(t => new TheorySectionDto(t.Heading, t.Content, t.Order))
                .ToList() ?? new List<TheorySectionDto>(),
            content.CodeExamples?
                .Select(c => new CodeExampleDto(
                    c.Title,
                    c.Code,
                    c.Language,
                    c.Explanation,
                    c.IsRunnable))
                .ToList() ?? new List<CodeExampleDto>(),
            content.Exercises?
                .Select(e => new ExerciseDto(
                    e.Title,
                    e.Description,
                    e.Difficulty.ToString(),
                    e.StarterCode,
                    e.Hints ?? new List<string>()))
                .ToList() ?? new List<ExerciseDto>(),
            content.Summary ?? string.Empty
        );
    }

    /// <summary>
    /// Estimates total hours for a collection of courses
    /// </summary>
    private static int EstimateHours(ICollection<Shared.Entities.Course>? courses)
    {
        if (courses == null || !courses.Any())
            return 0;

        // Simple estimation: sum all lesson counts and multiply by average lesson time (45 min)
        var totalLessons = courses.Sum(c => c.LessonCount);
        return (int)Math.Ceiling(totalLessons * 0.75); // 45 minutes per lesson = 0.75 hours
    }

    /// <summary>
    /// Deserializes a JSON array string to a list of strings
    /// Returns empty list if deserialization fails or input is null/empty
    /// </summary>
    private static List<string> DeserializeJsonArray(string? json)
    {
        if (string.IsNullOrWhiteSpace(json))
            return new List<string>();

        try
        {
            return JsonSerializer.Deserialize<List<string>>(json) ?? new List<string>();
        }
        catch (JsonException)
        {
            return new List<string>();
        }
    }
}
