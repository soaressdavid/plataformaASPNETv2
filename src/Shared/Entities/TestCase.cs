using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Shared.Models;

namespace Shared.Entities;

public class TestCase : BaseEntity
{
    [Required]
    public Guid ChallengeId { get; set; }

    [Required]
    public string Input { get; set; } = string.Empty;

    [Required]
    public string ExpectedOutput { get; set; } = string.Empty;

    public int OrderIndex { get; set; }

    public bool IsHidden { get; set; }

    // Navigation properties
    [ForeignKey(nameof(ChallengeId))]
    public Challenge Challenge { get; set; } = null!;
}
