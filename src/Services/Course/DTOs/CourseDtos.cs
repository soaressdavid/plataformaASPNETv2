namespace Course.Service.DTOs;

/// <summary>
/// DTO for course summary information
/// </summary>
public record CourseSummaryDto(
    Guid Id,
    string Title,
    string Description,
    string Level,
    Guid? LevelId,
    string Duration,
    int LessonCount,
    List<string> Topics,
    int OrderIndex
);

/// <summary>
/// DTO for detailed course information including lessons
/// </summary>
public record CourseDetailDto(
    Guid Id,
    string Title,
    string Description,
    string Level,
    Guid? LevelId,
    string? LevelTitle,
    string Duration,
    int LessonCount,
    List<string> Topics,
    List<LessonSummaryDto> Lessons
);

/// <summary>
/// Response wrapper for list of courses
/// </summary>
public record CourseListResponse(List<CourseSummaryDto> Courses);
