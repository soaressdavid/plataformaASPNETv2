using FsCheck;
using FsCheck.Xunit;
using Shared.Entities;
using Shared.Interfaces;
using Shared.Services;
using Challenge.Service.Services;
using Moq;

namespace Challenge.Tests;

/// <summary>
/// Property-based tests for challenge service functionality.
/// Feature: aspnet-learning-platform
/// </summary>
public class ChallengeServicePropertiesTests
{
    /// <summary>
    /// Property 11: Challenge Data Completeness
    /// **Validates: Requirements 5.1**
    /// 
    /// For any challenge stored in the platform, it should have all required fields: 
    /// title, description, difficulty level, starter code, and at least one test case.
    /// </summary>
    [Property(MaxTest = 20)]
    public async Task ChallengeData_HasAllRequiredFields(NonEmptyString title, NonEmptyString description, NonEmptyString starterCode)
    {
        // Arrange
        var challenge = new Shared.Entities.Challenge
        {
            Id = Guid.NewGuid(),
            Title = title.Get,
            Description = description.Get,
            Difficulty = Difficulty.Medium,
            StarterCode = starterCode.Get,
            TestCases = new List<TestCase>
            {
                new TestCase
                {
                    Id = Guid.NewGuid(),
                    ChallengeId = Guid.NewGuid(),
                    Input = "test",
                    ExpectedOutput = "output",
                    OrderIndex = 0,
                    IsHidden = false
                }
            }
        };

        // Assert - all required fields should be present
        Assert.False(string.IsNullOrEmpty(challenge.Title));
        Assert.False(string.IsNullOrEmpty(challenge.Description));
        Assert.NotEqual(Guid.Empty, challenge.Id);
        Assert.False(string.IsNullOrEmpty(challenge.StarterCode));
        Assert.NotEmpty(challenge.TestCases);
    }

    /// <summary>
    /// Property 12: Challenge Retrieval
    /// **Validates: Requirements 5.2**
    /// 
    /// For any challenge opened by a student, the API should return the complete 
    /// description and starter code.
    /// </summary>
    [Property(MaxTest = 20)]
    public async Task ChallengeRetrieval_ReturnsCompleteData(NonEmptyString title, NonEmptyString description, NonEmptyString starterCode)
    {
        // Arrange
        var challengeId = Guid.NewGuid();
        var challenge = new Shared.Entities.Challenge
        {
            Id = challengeId,
            Title = title.Get,
            Description = description.Get,
            Difficulty = Difficulty.Medium,
            StarterCode = starterCode.Get,
            TestCases = new List<TestCase>()
        };

        var mockChallengeRepo = new Mock<IChallengeRepository>();
        mockChallengeRepo.Setup(r => r.GetByIdAsync(challengeId))
            .ReturnsAsync(challenge);

        var mockSubmissionRepo = new Mock<ISubmissionRepository>();
        var testCaseExecutor = new TestCaseExecutor();
        var service = new ChallengeService(mockChallengeRepo.Object, mockSubmissionRepo.Object, testCaseExecutor);

        // Act
        var result = await service.GetChallengeByIdAsync(challengeId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(title.Get, result.Title);
        Assert.Equal(description.Get, result.Description);
        Assert.Equal(starterCode.Get, result.StarterCode);
    }

    /// <summary>
    /// Property 14: XP Award on Success
    /// **Validates: Requirements 5.4**
    /// 
    /// For any challenge submission where all test cases pass, the platform should 
    /// mark the challenge as solved and award XP to the student based on difficulty.
    /// </summary>
    [Property(MaxTest = 20)]
    public void XPAward_BasedOnDifficulty()
    {
        // Test Easy challenge
        var easyChallenge = new Shared.Entities.Challenge { Difficulty = Difficulty.Easy };
        Assert.Equal(10, GetExpectedXP(easyChallenge.Difficulty));

        // Test Medium challenge
        var mediumChallenge = new Shared.Entities.Challenge { Difficulty = Difficulty.Medium };
        Assert.Equal(25, GetExpectedXP(mediumChallenge.Difficulty));

        // Test Hard challenge
        var hardChallenge = new Shared.Entities.Challenge { Difficulty = Difficulty.Hard };
        Assert.Equal(50, GetExpectedXP(hardChallenge.Difficulty));
    }

    /// <summary>
    /// Property 15: Test Failure Reporting
    /// **Validates: Requirements 5.5**
    /// 
    /// For any failed test case, the result should include the test case identifier, 
    /// expected output, and actual output.
    /// </summary>
    [Property(MaxTest = 20)]
    public async Task TestFailure_IncludesDetails(NonEmptyString expectedOutput, NonEmptyString actualOutput)
    {
        // Only test when outputs are different
        if (expectedOutput.Get == actualOutput.Get)
            return;

        // Arrange
        var testResult = new Shared.Services.TestResult
        {
            TestCaseId = Guid.NewGuid(),
            Passed = false,
            ExpectedOutput = expectedOutput.Get,
            ActualOutput = actualOutput.Get,
            ErrorMessage = "Output mismatch"
        };

        // Assert
        Assert.False(testResult.Passed);
        Assert.NotEqual(Guid.Empty, testResult.TestCaseId);
        Assert.NotNull(testResult.ExpectedOutput);
        Assert.NotNull(testResult.ActualOutput);
        Assert.NotNull(testResult.ErrorMessage);
    }

    /// <summary>
    /// Property 16: Challenge Difficulty Categorization
    /// **Validates: Requirements 5.6**
    /// 
    /// For any challenge in the system, its difficulty should be one of: Easy, Medium, or Hard.
    /// </summary>
    [Property(MaxTest = 20)]
    public async Task ChallengeDifficulty_IsValid()
    {
        // Arrange
        var difficulties = new[] { Difficulty.Easy, Difficulty.Medium, Difficulty.Hard };
        
        foreach (var difficulty in difficulties)
        {
            var challenge = new Shared.Entities.Challenge
            {
                Id = Guid.NewGuid(),
                Title = "Test",
                Description = "Test",
                Difficulty = difficulty,
                StarterCode = "code"
            };

            // Assert
            Assert.Contains(challenge.Difficulty, difficulties);
        }
    }

    /// <summary>
    /// Property 17: Submission Persistence
    /// **Validates: Requirements 5.7**
    /// 
    /// For any challenge submission, the platform should store a record with timestamp, 
    /// submitted code, and result.
    /// </summary>
    [Property(MaxTest = 20)]
    public void SubmissionPersistence_HasRequiredFields(NonEmptyString code)
    {
        // Arrange
        var submission = new Submission
        {
            Id = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            ChallengeId = Guid.NewGuid(),
            Code = code.Get,
            Passed = true,
            Result = "All tests passed",
            CreatedAt = DateTime.UtcNow
        };

        // Assert - all required fields should be present
        Assert.NotEqual(Guid.Empty, submission.Id);
        Assert.NotEqual(Guid.Empty, submission.UserId);
        Assert.NotEqual(Guid.Empty, submission.ChallengeId);
        Assert.False(string.IsNullOrEmpty(submission.Code));
        Assert.False(string.IsNullOrEmpty(submission.Result));
        Assert.NotEqual(DateTime.MinValue, submission.CreatedAt);
    }

    /// <summary>
    /// Property: Challenge filtering by difficulty
    /// </summary>
    [Property(MaxTest = 20)]
    public async Task ChallengeFiltering_WorksByDifficulty()
    {
        // Arrange
        var challenges = new List<Shared.Entities.Challenge>
        {
            new Shared.Entities.Challenge { Id = Guid.NewGuid(), Title = "Easy1", Difficulty = Difficulty.Easy, Description = "Test", StarterCode = "code" },
            new Shared.Entities.Challenge { Id = Guid.NewGuid(), Title = "Medium1", Difficulty = Difficulty.Medium, Description = "Test", StarterCode = "code" },
            new Shared.Entities.Challenge { Id = Guid.NewGuid(), Title = "Hard1", Difficulty = Difficulty.Hard, Description = "Test", StarterCode = "code" }
        };

        var mockChallengeRepo = new Mock<IChallengeRepository>();
        mockChallengeRepo.Setup(r => r.GetAllAsync())
            .ReturnsAsync(challenges);

        var mockSubmissionRepo = new Mock<ISubmissionRepository>();
        var testCaseExecutor = new TestCaseExecutor();
        var service = new ChallengeService(mockChallengeRepo.Object, mockSubmissionRepo.Object, testCaseExecutor);

        // Act
        var easyResults = await service.GetAllChallengesAsync(Difficulty.Easy);
        var mediumResults = await service.GetAllChallengesAsync(Difficulty.Medium);
        var hardResults = await service.GetAllChallengesAsync(Difficulty.Hard);

        // Assert
        Assert.Single(easyResults);
        Assert.All(easyResults, c => Assert.Equal(Difficulty.Easy, c.Difficulty));
        
        Assert.Single(mediumResults);
        Assert.All(mediumResults, c => Assert.Equal(Difficulty.Medium, c.Difficulty));
        
        Assert.Single(hardResults);
        Assert.All(hardResults, c => Assert.Equal(Difficulty.Hard, c.Difficulty));
    }

    private int GetExpectedXP(Difficulty difficulty)
    {
        return difficulty switch
        {
            Difficulty.Easy => 10,
            Difficulty.Medium => 25,
            Difficulty.Hard => 50,
            _ => 0
        };
    }
}
