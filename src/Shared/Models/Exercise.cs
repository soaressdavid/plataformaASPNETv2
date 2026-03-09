using System.ComponentModel.DataAnnotations;

namespace Shared.Models;

/// <summary>
/// A practice exercise within a lesson.
/// Exercises help learners apply concepts taught in the lesson.
/// </summary>
public class Exercise
{
    /// <summary>
    /// Exercise title
    /// </summary>
    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Detailed description of what the learner should do
    /// </summary>
    [Required]
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Difficulty level (Fácil, Médio, Difícil)
    /// </summary>
    [Required]
    public ExerciseDifficulty Difficulty { get; set; } = ExerciseDifficulty.Fácil;

    /// <summary>
    /// Starting code template for the exercise
    /// </summary>
    public string StarterCode { get; set; } = string.Empty;

    /// <summary>
    /// Hints to help learners solve the exercise
    /// </summary>
    public List<string> Hints { get; set; } = new List<string>();
}

/// <summary>
/// Exercise difficulty levels
/// </summary>
public enum ExerciseDifficulty
{
    Fácil,
    Médio,
    Difícil
}
