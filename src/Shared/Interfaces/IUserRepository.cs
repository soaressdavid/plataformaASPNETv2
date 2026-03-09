using Shared.Entities;

namespace Shared.Interfaces;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByEmailAsync(string email);
    Task<bool> HardDeleteAsync(Guid userId);
}
