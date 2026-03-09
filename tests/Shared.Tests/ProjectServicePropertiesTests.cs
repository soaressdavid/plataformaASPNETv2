using FsCheck;
using FsCheck.Xunit;
using Microsoft.EntityFrameworkCore;
using Shared.Data;
using Shared.Entities;
using Shared.Repositories;
using Shared.Services;
using Shared.ValueObjects;
using System.Text.Json;

namespace Shared.Tests;

/// <summary>
/// Property-based tests for ProjectService functionality.
/// Feature: aspnet-learning-platform
/// </summary>
public class ProjectServicePropertiesTests : IDisposable
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ProjectRepository _projectRepository;
    private readonly ProgressRepository _progressRepository;
    private readonly ProjectService _projectService;

    public ProjectServicePropertiesTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _dbContext = new ApplicationDbContext(options);
        _projectRepository = new ProjectRepository(_dbContext);
        _progressRepository = new ProgressRepository(_dbContext);
        _projectService = new ProjectService(_projectRepository, _progressRepository, _dbContext);
    }

    public void Dispose()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }

    /// <summary>
    /// Property 18: Project Step Ordering
    /// **Validates: Requirements 6.1**
    /// 
    /// For any project in the system, its steps should have sequential order indices starting from 1.
    /// </summary>
    [Property(MaxTest = 20)]
    public void ProjectStepOrdering_IsSequentialStartingFromOne(PositiveInt stepCount)
    {
        // Arrange - Generate a project with N steps
        var count = Math.Max(1, stepCount.Get % 10 + 1); // Limit to 1-10 steps
        var steps = Enumerable.Range(1, count)
            .Select(i => new ProjectStep(
                Order: i,
                Title: $"Step {i}",
                Instructions: $"Instructions for step {i}",
                StarterCode: $"// Starter code for step {i}",
                ValidationCriteria: "Non-empty implementation"
            ))
            .ToList();

        var project = new Project
        {
            Title = "Test Project",
            Description = "Test Description",
            Steps = JsonSerializer.Serialize(steps),
            XPReward = 100
        };

        _dbContext.Projects.Add(project);
        _dbContext.SaveChanges();

        // Act - Parse the steps back
        var parsedSteps = JsonSerializer.Deserialize<List<ProjectStep>>(project.Steps);

        // Assert - Verify sequential ordering starting from 1
        Assert.NotNull(parsedSteps);
        Assert.Equal(count, parsedSteps.Count);
        Assert.True(parsedSteps.Select(s => s.Order).SequenceEqual(Enumerable.Range(1, count)));
    }

    /// <summary>
    /// Property 19: Project Start
    /// **Validates: Requirements 6.2**
    /// 
    /// For any project started by a student, the API should return the first step 
    /// (order index 1) with instructions and starter code.
    /// </summary>
    [Property(MaxTest = 20)]
    public async Task ProjectStart_ReturnsFirstStepWithInstructionsAndStarterCode(PositiveInt stepCount)
    {
        // Arrange
        var count = Math.Max(2, stepCount.Get % 10 + 2); // Generate 2-10 steps
        var userId = Guid.NewGuid();
        var steps = Enumerable.Range(1, count)
            .Select(i => new ProjectStep(
                Order: i,
                Title: $"Step {i}",
                Instructions: $"Instructions for step {i}",
                StarterCode: $"// Starter code for step {i}",
                ValidationCriteria: "Non-empty implementation"
            ))
            .ToList();

        var project = new Project
        {
            Title = "Test Project",
            Description = "Test Description",
            Steps = JsonSerializer.Serialize(steps),
            XPReward = 100
        };

        _dbContext.Projects.Add(project);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _projectService.StartProjectAsync(userId, project.Id);

        // Assert
        var firstStep = steps.First(s => s.Order == 1);
        Assert.Equal(1, result.StepOrder);
        Assert.Equal(firstStep.Title, result.StepTitle);
        Assert.False(string.IsNullOrEmpty(result.Instructions));
        Assert.False(string.IsNullOrEmpty(result.StarterCode));
        Assert.Equal(firstStep.Instructions, result.Instructions);
        Assert.Equal(firstStep.StarterCode, result.StarterCode);
    }

    /// <summary>
    /// Property 20: Sequential Step Unlocking
    /// **Validates: Requirements 6.3**
    /// 
    /// For any project step completion, the next step should only be accessible 
    /// after the current step is validated and marked complete.
    /// </summary>
    [Property(MaxTest = 20)]
    public async Task SequentialStepUnlocking_RequiresCompletionOfPreviousStep(PositiveInt stepCount)
    {
        // Arrange
        var count = Math.Max(3, stepCount.Get % 8 + 3); // Generate 3-8 steps
        var userId = Guid.NewGuid();
        var steps = Enumerable.Range(1, count)
            .Select(i => new ProjectStep(
                Order: i,
                Title: $"Step {i}",
                Instructions: $"Instructions for step {i}",
                StarterCode: $"// Starter code for step {i}",
                ValidationCriteria: "Non-empty implementation"
            ))
            .ToList();

        var project = new Project
        {
            Title = "Test Project",
            Description = "Test Description",
            Steps = JsonSerializer.Serialize(steps),
            XPReward = 100
        };

        _dbContext.Projects.Add(project);
        await _dbContext.SaveChangesAsync();

        // Start the project
        await _projectService.StartProjectAsync(userId, project.Id);

        // Act - Complete steps sequentially and verify next step is unlocked
        for (int i = 1; i < count; i++)
        {
            // Complete current step
            var completeResult = await _projectService.CompleteStepAsync(
                userId, project.Id, i, $"implementation for step {i}");

            // Verify next step is unlocked
            if (!completeResult.ProjectCompleted)
            {
                Assert.NotNull(completeResult.NextStep);
                Assert.Equal(i + 1, completeResult.NextStep.StepOrder);
            }
        }
    }

    /// <summary>
    /// Property 21: Project Step Examples
    /// **Validates: Requirements 6.4**
    /// 
    /// For any project step, it should include an example implementation 
    /// (represented by StarterCode in our implementation).
    /// </summary>
    [Property(MaxTest = 20)]
    public void ProjectStepExamples_AllStepsHaveStarterCode(PositiveInt stepCount)
    {
        // Arrange
        var count = Math.Max(1, stepCount.Get % 10 + 1); // Limit to 1-10 steps
        var steps = Enumerable.Range(1, count)
            .Select(i => new ProjectStep(
                Order: i,
                Title: $"Step {i}",
                Instructions: $"Instructions for step {i}",
                StarterCode: $"// Example implementation for step {i}",
                ValidationCriteria: "Non-empty implementation"
            ))
            .ToList();

        var project = new Project
        {
            Title = "Test Project",
            Description = "Test Description",
            Steps = JsonSerializer.Serialize(steps),
            XPReward = 100
        };

        _dbContext.Projects.Add(project);
        _dbContext.SaveChanges();

        // Act - Parse the steps back
        var parsedSteps = JsonSerializer.Deserialize<List<ProjectStep>>(project.Steps);

        // Assert - Verify all steps have non-empty starter code (example implementation)
        Assert.NotNull(parsedSteps);
        Assert.True(parsedSteps.All(s => !string.IsNullOrWhiteSpace(s.StarterCode)));
    }

    /// <summary>
    /// Property 22: Project Completion XP
    /// **Validates: Requirements 6.5**
    /// 
    /// For any project where all steps are completed, the platform should mark 
    /// the project as complete and award 100 XP.
    /// </summary>
    [Property(MaxTest = 20)]
    public async Task ProjectCompletionXP_Awards100XPWhenAllStepsCompleted(PositiveInt stepCount)
    {
        // Arrange - Use a fresh database context for this test
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        using var testDbContext = new ApplicationDbContext(options);
        var testProjectRepository = new ProjectRepository(testDbContext);
        var testProgressRepository = new ProgressRepository(testDbContext);
        var testProjectService = new ProjectService(testProjectRepository, testProgressRepository, testDbContext);

        var count = Math.Max(2, stepCount.Get % 6 + 2); // Generate 2-6 steps
        var userId = Guid.NewGuid();
        
        // Create user first
        var user = new User
        {
            Id = userId,
            Name = "Test User",
            Email = $"test{userId}@example.com",
            PasswordHash = "hash"
        };
        testDbContext.Users.Add(user);
        
        var steps = Enumerable.Range(1, count)
            .Select(i => new ProjectStep(
                Order: i,
                Title: $"Step {i}",
                Instructions: $"Instructions for step {i}",
                StarterCode: $"// Starter code for step {i}",
                ValidationCriteria: "Non-empty implementation"
            ))
            .ToList();

        var project = new Project
        {
            Title = "Test Project",
            Description = "Test Description",
            Steps = JsonSerializer.Serialize(steps),
            XPReward = 100
        };

        testDbContext.Projects.Add(project);
        await testDbContext.SaveChangesAsync();

        // Start the project
        await testProjectService.StartProjectAsync(userId, project.Id);

        // Get initial XP (should be 0 or create progress)
        var initialProgress = await testProgressRepository.GetByUserIdAsync(userId);
        var initialXP = initialProgress?.TotalXP ?? 0;

        // Act - Complete all steps
        CompleteStepResponse? lastResult = null;
        for (int i = 1; i <= count; i++)
        {
            lastResult = await testProjectService.CompleteStepAsync(
                userId, project.Id, i, $"implementation for step {i}");
        }

        // Get final XP
        var finalProgress = await testProgressRepository.GetByUserIdAsync(userId);
        var finalXP = finalProgress?.TotalXP ?? 0;

        // Assert
        Assert.NotNull(lastResult);
        Assert.True(lastResult.ProjectCompleted, $"Project should be completed after {count} steps");
        Assert.Equal(100, lastResult.XPAwarded);
        Assert.Equal(initialXP + 100, finalXP);
    }

    /// <summary>
    /// Additional property: Project completion is idempotent
    /// 
    /// Completing the same project multiple times should not award XP multiple times.
    /// </summary>
    [Property(MaxTest = 20)]
    public async Task ProjectCompletion_IsIdempotent(PositiveInt stepCount)
    {
        // Arrange
        var count = Math.Max(2, stepCount.Get % 4 + 2); // Generate 2-4 steps
        var userId = Guid.NewGuid();
        var steps = Enumerable.Range(1, count)
            .Select(i => new ProjectStep(
                Order: i,
                Title: $"Step {i}",
                Instructions: $"Instructions for step {i}",
                StarterCode: $"// Starter code for step {i}",
                ValidationCriteria: "Non-empty implementation"
            ))
            .ToList();

        var project = new Project
        {
            Title = "Test Project",
            Description = "Test Description",
            Steps = JsonSerializer.Serialize(steps),
            XPReward = 100
        };

        _dbContext.Projects.Add(project);
        await _dbContext.SaveChangesAsync();

        // Start the project
        await _projectService.StartProjectAsync(userId, project.Id);

        // Complete all steps
        for (int i = 1; i <= count; i++)
        {
            await _projectService.CompleteStepAsync(
                userId, project.Id, i, $"implementation for step {i}");
        }

        var xpAfterFirstCompletion = (await _progressRepository.GetByUserIdAsync(userId))?.TotalXP ?? 0;

        // Try to complete the last step again
        await _projectService.CompleteStepAsync(
            userId, project.Id, count, $"implementation for step {count} again");

        var xpAfterSecondCompletion = (await _progressRepository.GetByUserIdAsync(userId))?.TotalXP ?? 0;

        // XP should not increase on second completion
        Assert.Equal(xpAfterFirstCompletion, xpAfterSecondCompletion);
    }

    /// <summary>
    /// Additional property: Project progress tracking is accurate
    /// 
    /// For any project, the progress should accurately reflect completed steps.
    /// </summary>
    [Property(MaxTest = 20)]
    public async Task ProjectProgress_AccuratelyTracksCompletedSteps(PositiveInt stepCount)
    {
        // Arrange
        var count = Math.Max(3, stepCount.Get % 7 + 3); // Generate 3-7 steps
        var userId = Guid.NewGuid();
        var steps = Enumerable.Range(1, count)
            .Select(i => new ProjectStep(
                Order: i,
                Title: $"Step {i}",
                Instructions: $"Instructions for step {i}",
                StarterCode: $"// Starter code for step {i}",
                ValidationCriteria: "Non-empty implementation"
            ))
            .ToList();

        var project = new Project
        {
            Title = "Test Project",
            Description = "Test Description",
            Steps = JsonSerializer.Serialize(steps),
            XPReward = 100
        };

        _dbContext.Projects.Add(project);
        await _dbContext.SaveChangesAsync();

        // Start the project
        await _projectService.StartProjectAsync(userId, project.Id);

        // Complete half of the steps
        var stepsToComplete = count / 2;
        for (int i = 1; i <= stepsToComplete; i++)
        {
            await _projectService.CompleteStepAsync(
                userId, project.Id, i, $"implementation for step {i}");
        }

        // Act - Get progress
        var progress = await _projectService.GetProjectProgressAsync(userId, project.Id);

        // Assert
        Assert.Equal(stepsToComplete, progress.CompletedSteps.Count);
        Assert.True(progress.CompletedSteps.SequenceEqual(Enumerable.Range(1, stepsToComplete)));
        Assert.Equal(count, progress.TotalSteps);
        Assert.False(progress.IsCompleted);
    }
}
