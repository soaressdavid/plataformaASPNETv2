using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Shared.Data;
using Shared.Entities;
using Shared.Repositories;
using Shared.Services;
using Shared.ValueObjects;
using Xunit;

namespace Shared.Tests;

/// <summary>
/// Unit tests for ProjectService functionality.
/// </summary>
public class ProjectServiceTests : IDisposable
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ProjectRepository _projectRepository;
    private readonly ProgressRepository _progressRepository;
    private readonly ProjectService _projectService;

    public ProjectServiceTests()
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

    [Fact]
    public async Task StartProjectAsync_ReturnsFirstStep()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new User
        {
            Id = userId,
            Name = "Test User",
            Email = "test@example.com",
            PasswordHash = "hash"
        };
        _dbContext.Users.Add(user);

        var steps = new List<ProjectStep>
        {
            new ProjectStep(1, "Step 1", "Instructions 1", "Code 1", "Criteria 1"),
            new ProjectStep(2, "Step 2", "Instructions 2", "Code 2", "Criteria 2"),
            new ProjectStep(3, "Step 3", "Instructions 3", "Code 3", "Criteria 3")
        };

        var project = new Project
        {
            Id = Guid.NewGuid(),
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
        Assert.NotNull(result);
        Assert.Equal(project.Id, result.ProjectId);
        Assert.Equal("Test Project", result.ProjectTitle);
        Assert.Equal(1, result.StepOrder);
        Assert.Equal("Step 1", result.StepTitle);
        Assert.Equal("Instructions 1", result.Instructions);
        Assert.Equal("Code 1", result.StarterCode);
        Assert.Equal(3, result.TotalSteps);
        Assert.False(result.IsLastStep);
    }

    [Fact]
    public async Task CompleteStepAsync_UnlocksNextStep()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new User
        {
            Id = userId,
            Name = "Test User",
            Email = "test@example.com",
            PasswordHash = "hash"
        };
        _dbContext.Users.Add(user);

        var steps = new List<ProjectStep>
        {
            new ProjectStep(1, "Step 1", "Instructions 1", "Code 1", "Criteria 1"),
            new ProjectStep(2, "Step 2", "Instructions 2", "Code 2", "Criteria 2")
        };

        var project = new Project
        {
            Id = Guid.NewGuid(),
            Title = "Test Project",
            Description = "Test Description",
            Steps = JsonSerializer.Serialize(steps),
            XPReward = 100
        };
        _dbContext.Projects.Add(project);
        await _dbContext.SaveChangesAsync();

        // Start the project first
        await _projectService.StartProjectAsync(userId, project.Id);

        // Act
        var result = await _projectService.CompleteStepAsync(userId, project.Id, 1, "my implementation");

        // Assert
        Assert.True(result.Success);
        Assert.Equal(1, result.StepCompleted);
        Assert.False(result.ProjectCompleted);
        Assert.NotNull(result.NextStep);
        Assert.Equal(2, result.NextStep!.StepOrder);
        Assert.Equal("Step 2", result.NextStep.StepTitle);
        Assert.Equal(0, result.XPAwarded);
    }

    [Fact]
    public async Task CompleteStepAsync_AwardsXPOnProjectCompletion()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new User
        {
            Id = userId,
            Name = "Test User",
            Email = "test@example.com",
            PasswordHash = "hash"
        };
        _dbContext.Users.Add(user);

        var steps = new List<ProjectStep>
        {
            new ProjectStep(1, "Step 1", "Instructions 1", "Code 1", "Criteria 1"),
            new ProjectStep(2, "Step 2", "Instructions 2", "Code 2", "Criteria 2")
        };

        var project = new Project
        {
            Id = Guid.NewGuid(),
            Title = "Test Project",
            Description = "Test Description",
            Steps = JsonSerializer.Serialize(steps),
            XPReward = 100
        };
        _dbContext.Projects.Add(project);
        await _dbContext.SaveChangesAsync();

        // Start the project
        await _projectService.StartProjectAsync(userId, project.Id);

        // Complete first step
        await _projectService.CompleteStepAsync(userId, project.Id, 1, "implementation 1");

        // Act - Complete last step
        var result = await _projectService.CompleteStepAsync(userId, project.Id, 2, "implementation 2");

        // Assert
        Assert.True(result.Success);
        Assert.Equal(2, result.StepCompleted);
        Assert.True(result.ProjectCompleted);
        Assert.Null(result.NextStep);
        Assert.Equal(100, result.XPAwarded);

        // Verify XP was awarded
        var progress = await _progressRepository.GetByUserIdAsync(userId);
        Assert.NotNull(progress);
        Assert.Equal(100, progress!.TotalXP);
    }

    [Fact]
    public async Task GetProjectProgressAsync_ReturnsCorrectProgress()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new User
        {
            Id = userId,
            Name = "Test User",
            Email = "test@example.com",
            PasswordHash = "hash"
        };
        _dbContext.Users.Add(user);

        var steps = new List<ProjectStep>
        {
            new ProjectStep(1, "Step 1", "Instructions 1", "Code 1", "Criteria 1"),
            new ProjectStep(2, "Step 2", "Instructions 2", "Code 2", "Criteria 2"),
            new ProjectStep(3, "Step 3", "Instructions 3", "Code 3", "Criteria 3")
        };

        var project = new Project
        {
            Id = Guid.NewGuid(),
            Title = "Test Project",
            Description = "Test Description",
            Steps = JsonSerializer.Serialize(steps),
            XPReward = 100
        };
        _dbContext.Projects.Add(project);
        await _dbContext.SaveChangesAsync();

        // Start and complete first step
        await _projectService.StartProjectAsync(userId, project.Id);
        await _projectService.CompleteStepAsync(userId, project.Id, 1, "implementation 1");

        // Act
        var result = await _projectService.GetProjectProgressAsync(userId, project.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(project.Id, result.ProjectId);
        Assert.Equal("Test Project", result.ProjectTitle);
        Assert.Equal(2, result.CurrentStep); // Should be on step 2 now
        Assert.Single(result.CompletedSteps);
        Assert.Contains(1, result.CompletedSteps);
        Assert.Equal(3, result.TotalSteps);
        Assert.False(result.IsCompleted);
        Assert.Null(result.CompletedAt);
    }

    [Fact]
    public async Task GetProjectProgressAsync_ReturnsEmptyForUnstartedProject()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new User
        {
            Id = userId,
            Name = "Test User",
            Email = "test@example.com",
            PasswordHash = "hash"
        };
        _dbContext.Users.Add(user);

        var steps = new List<ProjectStep>
        {
            new ProjectStep(1, "Step 1", "Instructions 1", "Code 1", "Criteria 1")
        };

        var project = new Project
        {
            Id = Guid.NewGuid(),
            Title = "Test Project",
            Description = "Test Description",
            Steps = JsonSerializer.Serialize(steps),
            XPReward = 100
        };
        _dbContext.Projects.Add(project);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _projectService.GetProjectProgressAsync(userId, project.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(project.Id, result.ProjectId);
        Assert.Equal(0, result.CurrentStep);
        Assert.Empty(result.CompletedSteps);
        Assert.False(result.IsCompleted);
    }

    [Fact]
    public async Task StartProjectAsync_ThrowsException_WhenProjectNotFound()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var nonExistentProjectId = Guid.NewGuid();

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(
            async () => await _projectService.StartProjectAsync(userId, nonExistentProjectId)
        );
    }

    [Fact]
    public async Task CompleteStepAsync_ThrowsException_WhenProjectNotStarted()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new User
        {
            Id = userId,
            Name = "Test User",
            Email = "test@example.com",
            PasswordHash = "hash"
        };
        _dbContext.Users.Add(user);

        var steps = new List<ProjectStep>
        {
            new ProjectStep(1, "Step 1", "Instructions 1", "Code 1", "Criteria 1")
        };

        var project = new Project
        {
            Id = Guid.NewGuid(),
            Title = "Test Project",
            Description = "Test Description",
            Steps = JsonSerializer.Serialize(steps),
            XPReward = 100
        };
        _dbContext.Projects.Add(project);
        await _dbContext.SaveChangesAsync();

        // Act & Assert - Try to complete step without starting project
        await Assert.ThrowsAsync<InvalidOperationException>(
            async () => await _projectService.CompleteStepAsync(userId, project.Id, 1, "implementation")
        );
    }

    [Fact]
    public async Task CompleteStepAsync_ThrowsException_WhenImplementationIsEmpty()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new User
        {
            Id = userId,
            Name = "Test User",
            Email = "test@example.com",
            PasswordHash = "hash"
        };
        _dbContext.Users.Add(user);

        var steps = new List<ProjectStep>
        {
            new ProjectStep(1, "Step 1", "Instructions 1", "Code 1", "Criteria 1")
        };

        var project = new Project
        {
            Id = Guid.NewGuid(),
            Title = "Test Project",
            Description = "Test Description",
            Steps = JsonSerializer.Serialize(steps),
            XPReward = 100
        };
        _dbContext.Projects.Add(project);
        await _dbContext.SaveChangesAsync();

        // Start the project
        await _projectService.StartProjectAsync(userId, project.Id);

        // Act & Assert - Try to complete with empty implementation
        await Assert.ThrowsAsync<ArgumentException>(
            async () => await _projectService.CompleteStepAsync(userId, project.Id, 1, "")
        );

        // Also test with whitespace
        await Assert.ThrowsAsync<ArgumentException>(
            async () => await _projectService.CompleteStepAsync(userId, project.Id, 1, "   ")
        );
    }

    [Fact]
    public async Task CompleteStepAsync_ThrowsException_WhenStepDoesNotExist()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new User
        {
            Id = userId,
            Name = "Test User",
            Email = "test@example.com",
            PasswordHash = "hash"
        };
        _dbContext.Users.Add(user);

        var steps = new List<ProjectStep>
        {
            new ProjectStep(1, "Step 1", "Instructions 1", "Code 1", "Criteria 1")
        };

        var project = new Project
        {
            Id = Guid.NewGuid(),
            Title = "Test Project",
            Description = "Test Description",
            Steps = JsonSerializer.Serialize(steps),
            XPReward = 100
        };
        _dbContext.Projects.Add(project);
        await _dbContext.SaveChangesAsync();

        // Start the project
        await _projectService.StartProjectAsync(userId, project.Id);

        // Act & Assert - Try to complete non-existent step
        await Assert.ThrowsAsync<ArgumentException>(
            async () => await _projectService.CompleteStepAsync(userId, project.Id, 99, "implementation")
        );
    }

    [Fact]
    public async Task CompleteStepAsync_AllowsCompletingSameStepTwice()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new User
        {
            Id = userId,
            Name = "Test User",
            Email = "test@example.com",
            PasswordHash = "hash"
        };
        _dbContext.Users.Add(user);

        var steps = new List<ProjectStep>
        {
            new ProjectStep(1, "Step 1", "Instructions 1", "Code 1", "Criteria 1"),
            new ProjectStep(2, "Step 2", "Instructions 2", "Code 2", "Criteria 2")
        };

        var project = new Project
        {
            Id = Guid.NewGuid(),
            Title = "Test Project",
            Description = "Test Description",
            Steps = JsonSerializer.Serialize(steps),
            XPReward = 100
        };
        _dbContext.Projects.Add(project);
        await _dbContext.SaveChangesAsync();

        // Start the project
        await _projectService.StartProjectAsync(userId, project.Id);

        // Complete step 1 twice
        var result1 = await _projectService.CompleteStepAsync(userId, project.Id, 1, "implementation 1");
        var result2 = await _projectService.CompleteStepAsync(userId, project.Id, 1, "implementation 1 revised");

        // Assert - Both should succeed
        Assert.True(result1.Success);
        Assert.True(result2.Success);

        // Verify step 1 is only counted once in completed steps
        var progress = await _projectService.GetProjectProgressAsync(userId, project.Id);
        Assert.Single(progress.CompletedSteps);
        Assert.Contains(1, progress.CompletedSteps);
    }

    [Fact]
    public async Task StartProjectAsync_ReturnsFirstStep_WhenProjectAlreadyStarted()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new User
        {
            Id = userId,
            Name = "Test User",
            Email = "test@example.com",
            PasswordHash = "hash"
        };
        _dbContext.Users.Add(user);

        var steps = new List<ProjectStep>
        {
            new ProjectStep(1, "Step 1", "Instructions 1", "Code 1", "Criteria 1"),
            new ProjectStep(2, "Step 2", "Instructions 2", "Code 2", "Criteria 2")
        };

        var project = new Project
        {
            Id = Guid.NewGuid(),
            Title = "Test Project",
            Description = "Test Description",
            Steps = JsonSerializer.Serialize(steps),
            XPReward = 100
        };
        _dbContext.Projects.Add(project);
        await _dbContext.SaveChangesAsync();

        // Start the project twice
        var result1 = await _projectService.StartProjectAsync(userId, project.Id);
        var result2 = await _projectService.StartProjectAsync(userId, project.Id);

        // Assert - Both should return the first step
        Assert.Equal(1, result1.StepOrder);
        Assert.Equal(1, result2.StepOrder);
        Assert.Equal("Step 1", result1.StepTitle);
        Assert.Equal("Step 1", result2.StepTitle);
    }

    [Fact]
    public async Task CompleteStepAsync_SingleStepProject_AwardsXPImmediately()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new User
        {
            Id = userId,
            Name = "Test User",
            Email = "test@example.com",
            PasswordHash = "hash"
        };
        _dbContext.Users.Add(user);

        var steps = new List<ProjectStep>
        {
            new ProjectStep(1, "Only Step", "Instructions", "Code", "Criteria")
        };

        var project = new Project
        {
            Id = Guid.NewGuid(),
            Title = "Single Step Project",
            Description = "Test Description",
            Steps = JsonSerializer.Serialize(steps),
            XPReward = 100
        };
        _dbContext.Projects.Add(project);
        await _dbContext.SaveChangesAsync();

        // Start the project
        var startResult = await _projectService.StartProjectAsync(userId, project.Id);

        // Assert - First step should be marked as last step
        Assert.True(startResult.IsLastStep);

        // Act - Complete the only step
        var result = await _projectService.CompleteStepAsync(userId, project.Id, 1, "implementation");

        // Assert
        Assert.True(result.Success);
        Assert.True(result.ProjectCompleted);
        Assert.Null(result.NextStep);
        Assert.Equal(100, result.XPAwarded);
    }

    [Fact]
    public async Task CompleteStepAsync_UpdatesUserLevel_WhenXPThresholdReached()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new User
        {
            Id = userId,
            Name = "Test User",
            Email = "test@example.com",
            PasswordHash = "hash"
        };
        _dbContext.Users.Add(user);

        // Create user progress with 0 XP (level 0)
        var userProgress = new Progress
        {
            UserId = userId,
            TotalXP = 0,
            CurrentLevel = 0,
            LearningStreak = 0,
            LastActivityAt = DateTime.UtcNow
        };
        _dbContext.Progresses.Add(userProgress);

        var steps = new List<ProjectStep>
        {
            new ProjectStep(1, "Step 1", "Instructions 1", "Code 1", "Criteria 1")
        };

        var project = new Project
        {
            Id = Guid.NewGuid(),
            Title = "Test Project",
            Description = "Test Description",
            Steps = JsonSerializer.Serialize(steps),
            XPReward = 100 // This should bring user to level 1 (sqrt(100/100) = 1)
        };
        _dbContext.Projects.Add(project);
        await _dbContext.SaveChangesAsync();

        // Start and complete the project
        await _projectService.StartProjectAsync(userId, project.Id);
        await _projectService.CompleteStepAsync(userId, project.Id, 1, "implementation");

        // Assert - User should now be level 1
        var progress = await _progressRepository.GetByUserIdAsync(userId);
        Assert.NotNull(progress);
        Assert.Equal(100, progress!.TotalXP);
        Assert.Equal(1, progress.CurrentLevel);
    }

    [Fact]
    public async Task CompleteStepAsync_AllowsCompletingStepsOutOfOrder()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new User
        {
            Id = userId,
            Name = "Test User",
            Email = "test@example.com",
            PasswordHash = "hash"
        };
        _dbContext.Users.Add(user);

        var steps = new List<ProjectStep>
        {
            new ProjectStep(1, "Step 1", "Instructions 1", "Code 1", "Criteria 1"),
            new ProjectStep(2, "Step 2", "Instructions 2", "Code 2", "Criteria 2"),
            new ProjectStep(3, "Step 3", "Instructions 3", "Code 3", "Criteria 3")
        };

        var project = new Project
        {
            Id = Guid.NewGuid(),
            Title = "Test Project",
            Description = "Test Description",
            Steps = JsonSerializer.Serialize(steps),
            XPReward = 100
        };
        _dbContext.Projects.Add(project);
        await _dbContext.SaveChangesAsync();

        // Start the project
        await _projectService.StartProjectAsync(userId, project.Id);

        // Complete steps out of order: 2, 1, 3
        var result2 = await _projectService.CompleteStepAsync(userId, project.Id, 2, "implementation 2");
        var result1 = await _projectService.CompleteStepAsync(userId, project.Id, 1, "implementation 1");
        var result3 = await _projectService.CompleteStepAsync(userId, project.Id, 3, "implementation 3");

        // Assert - All should succeed
        Assert.True(result2.Success);
        Assert.True(result1.Success);
        Assert.True(result3.Success);

        // Last completion should mark project as complete
        Assert.True(result3.ProjectCompleted);
        Assert.Equal(100, result3.XPAwarded);

        // Verify all steps are completed
        var progress = await _projectService.GetProjectProgressAsync(userId, project.Id);
        Assert.Equal(3, progress.CompletedSteps.Count);
        Assert.True(progress.IsCompleted);
    }

    [Fact]
    public async Task CompleteStepAsync_DoesNotAwardXPTwice_WhenProjectAlreadyCompleted()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new User
        {
            Id = userId,
            Name = "Test User",
            Email = "test@example.com",
            PasswordHash = "hash"
        };
        _dbContext.Users.Add(user);

        var steps = new List<ProjectStep>
        {
            new ProjectStep(1, "Step 1", "Instructions 1", "Code 1", "Criteria 1")
        };

        var project = new Project
        {
            Id = Guid.NewGuid(),
            Title = "Test Project",
            Description = "Test Description",
            Steps = JsonSerializer.Serialize(steps),
            XPReward = 100
        };
        _dbContext.Projects.Add(project);
        await _dbContext.SaveChangesAsync();

        // Start and complete the project
        await _projectService.StartProjectAsync(userId, project.Id);
        var result1 = await _projectService.CompleteStepAsync(userId, project.Id, 1, "implementation 1");

        // Complete the same step again
        var result2 = await _projectService.CompleteStepAsync(userId, project.Id, 1, "implementation 1 revised");

        // Assert - First completion awards XP
        Assert.Equal(100, result1.XPAwarded);
        Assert.True(result1.ProjectCompleted);

        // Second completion should not award XP again and returns false for ProjectCompleted
        // (since the project was already completed, this is not a "new" completion)
        Assert.Equal(0, result2.XPAwarded);
        Assert.False(result2.ProjectCompleted);

        // Verify total XP is only 100
        var progress = await _progressRepository.GetByUserIdAsync(userId);
        Assert.NotNull(progress);
        Assert.Equal(100, progress!.TotalXP);
    }
}
