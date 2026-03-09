using Microsoft.EntityFrameworkCore;
using Shared.Data;
using Shared.Entities;
using Shared.Interfaces;

namespace Shared.Repositories;

public class ChallengeRepository : BaseRepository<Challenge>, IChallengeRepository
{
    public ChallengeRepository(ApplicationDbContext context) : base(context)
    {
    }

    public override async Task<Challenge?> GetByIdAsync(Guid id)
    {
        return await _retryPolicy.ExecuteAsync(async () =>
            await _context.Challenges
                .Include(c => c.TestCases)
                .Include(c => c.Submissions)
                .FirstOrDefaultAsync(c => c.Id == id)
        );
    }

    public override async Task<IEnumerable<Challenge>> GetAllAsync()
    {
        return await _retryPolicy.ExecuteAsync(async () =>
            await _context.Challenges
                .Include(c => c.TestCases)
                .ToListAsync()
        );
    }

    public async Task<List<TestCase>> GetTestCasesAsync(Guid challengeId)
    {
        return await _retryPolicy.ExecuteAsync(async () =>
            await _context.TestCases
                .Where(tc => tc.ChallengeId == challengeId)
                .OrderBy(tc => tc.OrderIndex)
                .ToListAsync()
        );
    }
}
