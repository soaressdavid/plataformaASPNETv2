using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Shared.Models;

namespace Shared.Entities;

public class Enrollment : BaseEntity
{
    [Required]
    public Guid UserId { get; set; }

    [Required]
    public Guid CourseId { get; set; }

    public DateTime EnrolledAt { get; set; } = DateTime.UtcNow;

    public DateTime? LastAccessedAt { get; set; }

    // Navigation properties
    [ForeignKey(nameof(UserId))]
    public User User { get; set; } = null!;

    [ForeignKey(nameof(CourseId))]
    public Course Course { get; set; } = null!;
}
