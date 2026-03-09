using Shared.Entities;

namespace Shared.Interfaces;

public interface IProgressRepository : IRepository<Progress>
{
    Task<Progress?> GetByUserIdAsync(Guid userId);
    Task<List<LeaderboardEntry>> GetTopAsync(int count);
}

public record LeaderboardEntry(int Rank, string Name, int XP, int Level);
