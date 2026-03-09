namespace Course.Service.DTOs;

/// <summary>
/// DTO for structured lesson content
/// </summary>
public record LessonContentDto(
    List<string> Objectives,
    List<TheorySectionDto> Theory,
    List<CodeExampleDto> CodeExamples,
    List<ExerciseDto> Exercises,
    string Summary
);

/// <summary>
/// DTO for theory section
/// </summary>
public record TheorySectionDto(
    string Heading,
    string Content,
    int Order
);

/// <summary>
/// DTO for code example
/// </summary>
public record CodeExampleDto(
    string Title,
    string Code,
    string Language,
    string Explanation,
    bool IsRunnable
);

/// <summary>
/// DTO for exercise
/// </summary>
public record ExerciseDto(
    string Title,
    string Description,
    string Difficulty,
    string StarterCode,
    List<string> Hints
);
