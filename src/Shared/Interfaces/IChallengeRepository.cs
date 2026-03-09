using Shared.Entities;

namespace Shared.Interfaces;

public interface IChallengeRepository : IRepository<Challenge>
{
    Task<List<TestCase>> GetTestCasesAsync(Guid challengeId);
}
