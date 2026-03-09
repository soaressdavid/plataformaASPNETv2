using Shared.Entities;
using Shared.Interfaces;
using Shared.Services;

namespace Challenge.Service.Services;

/// <summary>
/// Service for managing challenges and submissions.
/// </summary>
public class ChallengeService
{
    private readonly IChallengeRepository _challengeRepository;
    private readonly ISubmissionRepository _submissionRepository;
    private readonly TestCaseExecutor _testCaseExecutor;

    public ChallengeService(
        IChallengeRepository challengeRepository,
        ISubmissionRepository submissionRepository,
        TestCaseExecutor testCaseExecutor)
    {
        _challengeRepository = challengeRepository;
        _submissionRepository = submissionRepository;
        _testCaseExecutor = testCaseExecutor;
    }

    /// <summary>
    /// Gets all challenges with optional filtering by difficulty.
    /// </summary>
    public async Task<List<Shared.Entities.Challenge>> GetAllChallengesAsync(Difficulty? difficulty = null)
    {
        var challenges = await _challengeRepository.GetAllAsync();
        
        if (difficulty.HasValue)
        {
            challenges = challenges.Where(c => c.Difficulty == difficulty.Value);
        }
        
        return challenges.ToList();
    }

    /// <summary>
    /// Gets a challenge by ID with its test cases.
    /// </summary>
    public async Task<Shared.Entities.Challenge?> GetChallengeByIdAsync(Guid challengeId)
    {
        return await _challengeRepository.GetByIdAsync(challengeId);
    }

    /// <summary>
    /// Submits a solution for a challenge and executes all test cases.
    /// Awards XP based on difficulty if all tests pass.
    /// </summary>
    public async Task<SubmissionResult> SubmitSolutionAsync(Guid userId, Guid challengeId, string code)
    {
        // Get the challenge with test cases
        var challenge = await _challengeRepository.GetByIdAsync(challengeId);
        if (challenge == null)
        {
            throw new ArgumentException($"Challenge with ID {challengeId} not found", nameof(challengeId));
        }

        // Get test cases
        var testCases = await _challengeRepository.GetTestCasesAsync(challengeId);
        if (testCases.Count == 0)
        {
            throw new InvalidOperationException($"Challenge {challengeId} has no test cases");
        }

        // Execute all test cases
        var testResults = await _testCaseExecutor.ExecuteAllTestCasesAsync(code, testCases);

        // Determine if all tests passed
        var allTestsPassed = testResults.All(r => r.Passed);

        // Calculate XP award
        int xpAwarded = 0;
        if (allTestsPassed)
        {
            xpAwarded = CalculateXP(challenge.Difficulty);
        }

        // Create submission record
        var submission = new Submission
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            ChallengeId = challengeId,
            Code = code,
            Passed = allTestsPassed,
            Result = allTestsPassed ? "All tests passed" : "Some tests failed",
            CreatedAt = DateTime.UtcNow
        };

        await _submissionRepository.CreateAsync(submission);

        // Return result
        return new SubmissionResult
        {
            SubmissionId = submission.Id,
            AllTestsPassed = allTestsPassed,
            TestResults = testResults,
            XpAwarded = xpAwarded
        };
    }

    /// <summary>
    /// Calculates XP award based on challenge difficulty.
    /// Easy: 10 XP, Medium: 25 XP, Hard: 50 XP
    /// </summary>
    private int CalculateXP(Difficulty difficulty)
    {
        return difficulty switch
        {
            Difficulty.Easy => 10,
            Difficulty.Medium => 25,
            Difficulty.Hard => 50,
            _ => 0
        };
    }

    /// <summary>
    /// Gets submission history for a user and challenge.
    /// </summary>
    public async Task<List<Submission>> GetSubmissionHistoryAsync(Guid userId, Guid challengeId)
    {
        var submissions = await _submissionRepository.GetByUserAndChallengeAsync(userId, challengeId);
        return submissions.OrderByDescending(s => s.CreatedAt).ToList();
    }
}

/// <summary>
/// Result of a challenge submission.
/// </summary>
public class SubmissionResult
{
    public Guid SubmissionId { get; set; }
    public bool AllTestsPassed { get; set; }
    public List<TestResult> TestResults { get; set; } = new();
    public int XpAwarded { get; set; }
}
