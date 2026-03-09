namespace Progress.Service.Models;

public record DashboardResponse(
    int CurrentXP,
    int CurrentLevel,
    int XPToNextLevel,
    int SolvedChallenges,
    int CompletedProjects,
    int LearningStreak,
    List<CourseProgress> CoursesInProgress
);

public record CourseProgress(
    Guid CourseId,
    string Title,
    int CompletedLessons,
    int TotalLessons,
    int CompletionPercentage
);
