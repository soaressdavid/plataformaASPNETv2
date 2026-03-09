using Shared.Entities;

namespace Shared.Interfaces;

public interface ISubmissionRepository : IRepository<Submission>
{
    Task<List<Submission>> GetByUserAndChallengeAsync(Guid userId, Guid challengeId);
}
