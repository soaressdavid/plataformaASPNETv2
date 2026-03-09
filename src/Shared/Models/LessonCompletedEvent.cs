namespace Shared.Models;

/// <summary>
/// Event published when a student completes a lesson
/// </summary>
public class LessonCompletedEvent : DomainEvent
{
    public override string EventType => "lesson.completed";

    /// <summary>
    /// ID of the user who completed the lesson
    /// </summary>
    public Guid UserId { get; init; }

    /// <summary>
    /// ID of the completed lesson
    /// </summary>
    public Guid LessonId { get; init; }

    /// <summary>
    /// ID of the course containing the lesson
    /// </summary>
    public Guid CourseId { get; init; }

    /// <summary>
    /// Order index of the completed lesson within the course
    /// </summary>
    public int LessonOrder { get; init; }

    /// <summary>
    /// ID of the next lesson (if any)
    /// </summary>
    public Guid? NextLessonId { get; init; }
}
