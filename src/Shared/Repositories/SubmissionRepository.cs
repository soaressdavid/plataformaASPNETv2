using Microsoft.EntityFrameworkCore;
using Shared.Data;
using Shared.Entities;
using Shared.Interfaces;

namespace Shared.Repositories;

public class SubmissionRepository : BaseRepository<Submission>, ISubmissionRepository
{
    public SubmissionRepository(ApplicationDbContext context) : base(context)
    {
    }

    public override async Task<Submission?> GetByIdAsync(Guid id)
    {
        return await _retryPolicy.ExecuteAsync(async () =>
            await _context.Submissions
                .Include(s => s.User)
                .Include(s => s.Challenge)
                .FirstOrDefaultAsync(s => s.Id == id)
        );
    }

    public async Task<List<Submission>> GetByUserAndChallengeAsync(Guid userId, Guid challengeId)
    {
        return await _retryPolicy.ExecuteAsync(async () =>
            await _context.Submissions
                .Where(s => s.UserId == userId && s.ChallengeId == challengeId)
                .OrderByDescending(s => s.CreatedAt)
                .ToListAsync()
        );
    }

    public async Task<List<Submission>> GetTimeAttackLeaderboardAsync(Guid challengeId, int limit = 100)
    {
        return await _retryPolicy.ExecuteAsync(async () =>
            await _context.Submissions
                .Include(s => s.User)
                .Where(s => s.ChallengeId == challengeId && 
                           s.IsTimeAttack && 
                           s.Passed && 
                           s.CompletionTimeSeconds.HasValue)
                .GroupBy(s => s.UserId)
                .Select(g => g.OrderBy(s => s.CompletionTimeSeconds).First())
                .OrderBy(s => s.CompletionTimeSeconds)
                .Take(limit)
                .ToListAsync()
        );
    }
}
