namespace Course.Service.DTOs;

/// <summary>
/// DTO for curriculum level summary information
/// </summary>
public record LevelDto(
    Guid Id,
    int Number,
    string Title,
    string Description,
    int RequiredXP,
    int CourseCount,
    int EstimatedHours
);

/// <summary>
/// DTO for detailed curriculum level information including courses
/// </summary>
public record LevelDetailDto(
    Guid Id,
    int Number,
    string Title,
    string Description,
    int RequiredXP,
    List<CourseSummaryDto> Courses,
    ProjectSummaryDto? Project
);

/// <summary>
/// DTO for project summary information
/// </summary>
public record ProjectSummaryDto(
    Guid Id,
    string Title,
    string Description
);

/// <summary>
/// Response wrapper for list of levels
/// </summary>
public record LevelListResponse(List<LevelDto> Levels);
