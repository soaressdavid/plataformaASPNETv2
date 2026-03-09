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
    /// Supports Time Attack mode with bonus XP based on completion time.
    /// </summary>
    public async Task<SubmissionResult> SubmitSolutionAsync(
        Guid userId, 
        Guid challengeId, 
        string code, 
        bool isTimeAttack = false, 
        int? completionTimeSeconds = null)
    {
        // Get the challenge with test cases
        var challenge = await _challengeRepository.GetByIdAsync(challengeId);
        if (challenge == null)
        {
            throw new ArgumentException($"Challenge with ID {challengeId} not found", nameof(challengeId));
        }

        // Validate Time Attack mode
        if (isTimeAttack && !challenge.SupportsTimeAttack)
        {
            throw new InvalidOperationException($"Challenge {challengeId} does not support Time Attack mode");
        }

        if (isTimeAttack && completionTimeSeconds.HasValue && completionTimeSeconds.Value > challenge.TimeAttackLimitSeconds)
        {
            throw new InvalidOperationException($"Time Attack submission exceeded time limit of {challenge.TimeAttackLimitSeconds} seconds");
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
        int? timeAttackBonusXP = null;
        
        if (allTestsPassed)
        {
            xpAwarded = CalculateXP(challenge.Difficulty);
            
            // Add Time Attack bonus if applicable
            if (isTimeAttack && completionTimeSeconds.HasValue)
            {
                int remainingSeconds = challenge.TimeAttackLimitSeconds - completionTimeSeconds.Value;
                timeAttackBonusXP = XPCalculator.CalculateTimeAttackBonus(remainingSeconds);
                xpAwarded += timeAttackBonusXP.Value;
            }
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
            IsTimeAttack = isTimeAttack,
            CompletionTimeSeconds = completionTimeSeconds,
            TimeAttackBonusXP = timeAttackBonusXP,
            CreatedAt = DateTime.UtcNow
        };

        await _submissionRepository.CreateAsync(submission);

        // Return result
        return new SubmissionResult
        {
            SubmissionId = submission.Id,
            AllTestsPassed = allTestsPassed,
            TestResults = testResults,
            XpAwarded = xpAwarded,
            TimeAttackBonusXP = timeAttackBonusXP,
            CompletionTimeSeconds = completionTimeSeconds
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

    /// <summary>
    /// Gets Time Attack leaderboard for a specific challenge.
    /// Returns top submissions sorted by fastest completion time.
    /// </summary>
    public async Task<List<TimeAttackLeaderboardEntry>> GetTimeAttackLeaderboardAsync(Guid challengeId, int limit = 100)
    {
        var submissions = await _submissionRepository.GetTimeAttackLeaderboardAsync(challengeId, limit);
        
        return submissions.Select((s, index) => new TimeAttackLeaderboardEntry
        {
            Rank = index + 1,
            UserId = s.UserId,
            UserName = s.User?.Name ?? "Unknown",
            CompletionTimeSeconds = s.CompletionTimeSeconds ?? 0,
            BonusXP = s.TimeAttackBonusXP ?? 0,
            SubmittedAt = s.CreatedAt
        }).ToList();
    }

    /// <summary>
    /// Gets user's best time for a specific challenge.
    /// </summary>
    public async Task<Submission?> GetUserBestTimeAsync(Guid userId, Guid challengeId)
    {
        var submissions = await _submissionRepository.GetByUserAndChallengeAsync(userId, challengeId);
        
        return submissions
            .Where(s => s.IsTimeAttack && s.Passed && s.CompletionTimeSeconds.HasValue)
            .OrderBy(s => s.CompletionTimeSeconds)
            .FirstOrDefault();
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
    public int? TimeAttackBonusXP { get; set; }
    public int? CompletionTimeSeconds { get; set; }
}

/// <summary>
/// Time Attack leaderboard entry.
/// </summary>
public class TimeAttackLeaderboardEntry
{
    public int Rank { get; set; }
    public Guid UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public int CompletionTimeSeconds { get; set; }
    public int BonusXP { get; set; }
    public DateTime SubmittedAt { get; set; }
}
