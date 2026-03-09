using Microsoft.EntityFrameworkCore;
using Shared.Data;
using Shared.Entities;
using Shared.Interfaces;

namespace Shared.Repositories;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _retryPolicy.ExecuteAsync(async () =>
            await _context.Users
                .Include(u => u.Progress)
                .FirstOrDefaultAsync(u => u.Email == email)
        );
    }

    public override async Task<User?> GetByIdAsync(Guid id)
    {
        return await _retryPolicy.ExecuteAsync(async () =>
            await _context.Users
                .Include(u => u.Progress)
                .Include(u => u.Enrollments)
                .Include(u => u.Submissions)
                .Include(u => u.LessonCompletions)
                .FirstOrDefaultAsync(u => u.Id == id)
        );
    }
}
