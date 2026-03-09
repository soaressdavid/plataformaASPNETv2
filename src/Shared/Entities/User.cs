using System.ComponentModel.DataAnnotations;
using Shared.Models;

namespace Shared.Entities;

public class User : BaseEntity
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [MaxLength(255)]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string PasswordHash { get; set; } = string.Empty;

    // Navigation properties
    public Progress? Progress { get; set; }
    public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    public ICollection<Submission> Submissions { get; set; } = new List<Submission>();
    public ICollection<LessonCompletion> LessonCompletions { get; set; } = new List<LessonCompletion>();
    public ICollection<ProjectProgress> ProjectProgresses { get; set; } = new List<ProjectProgress>();
}
