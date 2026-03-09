namespace Shared.Models;

/// <summary>
/// Event published when a student successfully completes a challenge
/// </summary>
public class ChallengeCompletedEvent : DomainEvent
{
    public override string EventType => "challenge.completed";

    /// <summary>
    /// ID of the user who completed the challenge
    /// </summary>
    public Guid UserId { get; init; }

    /// <summary>
    /// ID of the completed challenge
    /// </summary>
    public Guid ChallengeId { get; init; }

    /// <summary>
    /// Difficulty level of the challenge (Easy, Medium, Hard)
    /// </summary>
    public string Difficulty { get; init; } = string.Empty;

    /// <summary>
    /// XP awarded for completing the challenge
    /// </summary>
    public int XpAwarded { get; init; }

    /// <summary>
    /// ID of the submission
    /// </summary>
    public Guid SubmissionId { get; init; }
}
