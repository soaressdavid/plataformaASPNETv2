using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Shared.Data;
using Shared.Entities;
using Shared.Interfaces;
using Shared.ValueObjects;

namespace Shared.Services;

/// <summary>
/// Service for managing guided projects with step-by-step progression.
/// </summary>
public class ProjectService
{
    private readonly IProjectRepository _projectRepository;
    private readonly IProgressRepository _progressRepository;
    private readonly ApplicationDbContext _dbContext;

    public ProjectService(
        IProjectRepository projectRepository,
        IProgressRepository progressRepository,
        ApplicationDbContext dbContext)
    {
        _projectRepository = projectRepository;
        _progressRepository = progressRepository;
        _dbContext = dbContext;
    }

    /// <summary>
    /// Starts a project for a user and returns the first step.
    /// </summary>
    /// <param name="userId">The user ID</param>
    /// <param name="projectId">The project ID</param>
    /// <returns>The first project step</returns>
    public async Task<ProjectStepResponse> StartProjectAsync(Guid userId, Guid projectId)
    {
        // Get the project
        var project = await _projectRepository.GetByIdAsync(projectId);
        if (project == null)
            throw new ArgumentException($"Project with ID {projectId} not found", nameof(projectId));

        // Parse project steps
        var steps = JsonSerializer.Deserialize<List<ProjectStep>>(project.Steps);
        if (steps == null || steps.Count == 0)
            throw new InvalidOperationException($"Project {projectId} has no steps");

        // Get or create project progress
        var progress = await _dbContext.ProjectProgresses
            .FirstOrDefaultAsync(pp => pp.UserId == userId && pp.ProjectId == projectId);

        if (progress == null)
        {
            progress = new ProjectProgress
            {
                UserId = userId,
                ProjectId = projectId,
                CurrentStep = 1,
                CompletedSteps = "[]",
                IsCompleted = false
            };
            _dbContext.ProjectProgresses.Add(progress);
            await _dbContext.SaveChangesAsync();
        }

        // Get the first step (order 1)
        var firstStep = steps.FirstOrDefault(s => s.Order == 1);
        if (firstStep == null)
            throw new InvalidOperationException($"Project {projectId} has no step with order 1");

        return new ProjectStepResponse(
            ProjectId: projectId,
            ProjectTitle: project.Title,
            StepOrder: firstStep.Order,
            StepTitle: firstStep.Title,
            Instructions: firstStep.Instructions,
            StarterCode: firstStep.StarterCode,
            TotalSteps: steps.Count,
            IsLastStep: firstStep.Order == steps.Count
        );
    }

    /// <summary>
    /// Completes a step and unlocks the next step if validation passes.
    /// Awards 100 XP when all steps are completed.
    /// </summary>
    /// <param name="userId">The user ID</param>
    /// <param name="projectId">The project ID</param>
    /// <param name="stepOrder">The step order to complete</param>
    /// <param name="implementation">The user's implementation code</param>
    /// <returns>Result with next step information or completion status</returns>
    public async Task<CompleteStepResponse> CompleteStepAsync(
        Guid userId,
        Guid projectId,
        int stepOrder,
        string implementation)
    {
        // Get the project
        var project = await _projectRepository.GetByIdAsync(projectId);
        if (project == null)
            throw new ArgumentException($"Project with ID {projectId} not found", nameof(projectId));

        // Parse project steps
        var steps = JsonSerializer.Deserialize<List<ProjectStep>>(project.Steps);
        if (steps == null || steps.Count == 0)
            throw new InvalidOperationException($"Project {projectId} has no steps");

        // Get project progress
        var progress = await _dbContext.ProjectProgresses
            .FirstOrDefaultAsync(pp => pp.UserId == userId && pp.ProjectId == projectId);

        if (progress == null)
            throw new InvalidOperationException($"User {userId} has not started project {projectId}");

        // Validate the step exists
        var step = steps.FirstOrDefault(s => s.Order == stepOrder);
        if (step == null)
            throw new ArgumentException($"Step {stepOrder} not found in project {projectId}", nameof(stepOrder));

        // Validate implementation (basic validation - check not empty)
        if (string.IsNullOrWhiteSpace(implementation))
            throw new ArgumentException("Implementation cannot be empty", nameof(implementation));

        // TODO: In a real implementation, we would validate against ValidationCriteria
        // For now, we'll accept any non-empty implementation

        // Update completed steps
        var completedSteps = JsonSerializer.Deserialize<List<int>>(progress.CompletedSteps) ?? new List<int>();
        if (!completedSteps.Contains(stepOrder))
        {
            completedSteps.Add(stepOrder);
            completedSteps.Sort();
            progress.CompletedSteps = JsonSerializer.Serialize(completedSteps);
        }

        // Check if all steps are completed
        var allStepsCompleted = completedSteps.Count == steps.Count;

        if (allStepsCompleted && !progress.IsCompleted)
        {
            // Mark project as completed
            progress.IsCompleted = true;
            progress.CompletedAt = DateTime.UtcNow;

            // Award XP (100 XP for project completion)
            // Get or create user progress
            var userProgress = await _progressRepository.GetByUserIdAsync(userId);
            if (userProgress == null)
            {
                userProgress = new Progress
                {
                    UserId = userId,
                    TotalXP = 0,
                    CurrentLevel = 0,
                    LearningStreak = 0,
                    LastActivityAt = DateTime.UtcNow
                };
                userProgress = await _progressRepository.CreateAsync(userProgress);
            }

            // Update XP and level
            userProgress.TotalXP += project.XPReward;
            userProgress.CurrentLevel = XPCalculator.CalculateLevel(userProgress.TotalXP);
            userProgress.LastActivityAt = DateTime.UtcNow;
            await _progressRepository.UpdateAsync(userProgress);

            await _dbContext.SaveChangesAsync();

            return new CompleteStepResponse(
                Success: true,
                StepCompleted: stepOrder,
                ProjectCompleted: true,
                NextStep: null,
                XPAwarded: project.XPReward
            );
        }

        // Update current step to next uncompleted step
        var nextStep = steps
            .Where(s => !completedSteps.Contains(s.Order))
            .OrderBy(s => s.Order)
            .FirstOrDefault();

        if (nextStep != null)
        {
            progress.CurrentStep = nextStep.Order;
        }

        await _dbContext.SaveChangesAsync();

        return new CompleteStepResponse(
            Success: true,
            StepCompleted: stepOrder,
            ProjectCompleted: false,
            NextStep: nextStep != null ? new ProjectStepResponse(
                ProjectId: projectId,
                ProjectTitle: project.Title,
                StepOrder: nextStep.Order,
                StepTitle: nextStep.Title,
                Instructions: nextStep.Instructions,
                StarterCode: nextStep.StarterCode,
                TotalSteps: steps.Count,
                IsLastStep: nextStep.Order == steps.Count
            ) : null,
            XPAwarded: 0
        );
    }

    /// <summary>
    /// Gets the current progress for a user's project.
    /// </summary>
    /// <param name="userId">The user ID</param>
    /// <param name="projectId">The project ID</param>
    /// <returns>Project progress information</returns>
    public async Task<ProjectProgressResponse> GetProjectProgressAsync(Guid userId, Guid projectId)
    {
        // Get the project
        var project = await _projectRepository.GetByIdAsync(projectId);
        if (project == null)
            throw new ArgumentException($"Project with ID {projectId} not found", nameof(projectId));

        // Parse project steps
        var steps = JsonSerializer.Deserialize<List<ProjectStep>>(project.Steps);
        if (steps == null || steps.Count == 0)
            throw new InvalidOperationException($"Project {projectId} has no steps");

        // Get project progress
        var progress = await _dbContext.ProjectProgresses
            .FirstOrDefaultAsync(pp => pp.UserId == userId && pp.ProjectId == projectId);

        if (progress == null)
        {
            // User hasn't started the project yet
            return new ProjectProgressResponse(
                ProjectId: projectId,
                ProjectTitle: project.Title,
                CurrentStep: 0,
                CompletedSteps: new List<int>(),
                TotalSteps: steps.Count,
                IsCompleted: false,
                CompletedAt: null
            );
        }

        var completedSteps = JsonSerializer.Deserialize<List<int>>(progress.CompletedSteps) ?? new List<int>();

        return new ProjectProgressResponse(
            ProjectId: projectId,
            ProjectTitle: project.Title,
            CurrentStep: progress.CurrentStep,
            CompletedSteps: completedSteps,
            TotalSteps: steps.Count,
            IsCompleted: progress.IsCompleted,
            CompletedAt: progress.CompletedAt
        );
    }
}

/// <summary>
/// Response model for a project step.
/// </summary>
public record ProjectStepResponse(
    Guid ProjectId,
    string ProjectTitle,
    int StepOrder,
    string StepTitle,
    string Instructions,
    string StarterCode,
    int TotalSteps,
    bool IsLastStep
);

/// <summary>
/// Response model for completing a step.
/// </summary>
public record CompleteStepResponse(
    bool Success,
    int StepCompleted,
    bool ProjectCompleted,
    ProjectStepResponse? NextStep,
    int XPAwarded
);

/// <summary>
/// Response model for project progress.
/// </summary>
public record ProjectProgressResponse(
    Guid ProjectId,
    string ProjectTitle,
    int CurrentStep,
    List<int> CompletedSteps,
    int TotalSteps,
    bool IsCompleted,
    DateTime? CompletedAt
);
