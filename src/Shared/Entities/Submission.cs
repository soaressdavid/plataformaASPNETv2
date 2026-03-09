using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Shared.Models;

namespace Shared.Entities;

public class Submission : BaseEntity
{
    [Required]
    public Guid UserId { get; set; }

    [Required]
    public Guid ChallengeId { get; set; }

    [Required]
    public string Code { get; set; } = string.Empty;

    public bool Passed { get; set; }

    public string Result { get; set; } = string.Empty;

    // Time Attack fields
    public bool IsTimeAttack { get; set; }
    public int? CompletionTimeSeconds { get; set; }
    public int? TimeAttackBonusXP { get; set; }

    // Navigation properties
    [ForeignKey(nameof(UserId))]
    public User User { get; set; } = null!;

    [ForeignKey(nameof(ChallengeId))]
    public Challenge Challenge { get; set; } = null!;
}
