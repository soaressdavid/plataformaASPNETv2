namespace Course.Service.DTOs;

/// <summary>
/// DTO for lesson summary information
/// </summary>
public record LessonSummaryDto(
    Guid Id,
    string Title,
    string Duration,
    string Difficulty,
    int EstimatedMinutes,
    int Order,
    bool IsCompleted
);

/// <summary>
/// DTO for detailed lesson information including content
/// </summary>
public record LessonDetailDto(
    Guid Id,
    string Title,
    string? Content,
    LessonContentDto? StructuredContent,
    string Duration,
    string Difficulty,
    int EstimatedMinutes,
    List<string> Prerequisites,
    int Order,
    bool IsCompleted
);

/// <summary>
/// Response wrapper for list of lessons
/// </summary>
public record LessonListResponse(List<LessonSummaryDto> Lessons);
