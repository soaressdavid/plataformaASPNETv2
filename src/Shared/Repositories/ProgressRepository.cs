using Microsoft.EntityFrameworkCore;
using Shared.Data;
using Shared.Entities;
using Shared.Interfaces;

namespace Shared.Repositories;

public class ProgressRepository : BaseRepository<Progress>, IProgressRepository
{
    public ProgressRepository(ApplicationDbContext context) : base(context)
    {
    }

    public override async Task<Progress?> GetByIdAsync(Guid id)
    {
        return await _retryPolicy.ExecuteAsync(async () =>
            await _context.Progresses
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.Id == id)
        );
    }

    public async Task<Progress?> GetByUserIdAsync(Guid userId)
    {
        return await _retryPolicy.ExecuteAsync(async () =>
            await _context.Progresses
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.UserId == userId)
        );
    }

    public async Task<List<LeaderboardEntry>> GetTopAsync(int count)
    {
        return await _retryPolicy.ExecuteAsync(async () =>
        {
            var topProgress = await _context.Progresses
                .Include(p => p.User)
                .OrderByDescending(p => p.TotalXP)
                .Take(count)
                .ToListAsync();

            return topProgress
                .Select((p, index) => new LeaderboardEntry(
                    Rank: index + 1,
                    Name: p.User.Name,
                    XP: p.TotalXP,
                    Level: p.CurrentLevel
                ))
                .ToList();
        });
    }
}
