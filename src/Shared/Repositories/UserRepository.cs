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

    /// <summary>
    /// Permanently deletes a user and all associated data (GDPR compliance).
    /// This bypasses soft delete and removes all data from the database.
    /// </summary>
    public async Task<bool> HardDeleteAsync(Guid userId)
    {
        return await _retryPolicy.ExecuteAsync(async () =>
        {
            var user = await _context.Users
                .Include(u => u.Progress)
                .Include(u => u.Enrollments)
                .Include(u => u.Submissions)
                .Include(u => u.LessonCompletions)
                .Include(u => u.ProjectProgresses)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                return false;
            }

            // Remove all related data first (cascade delete)
            if (user.Progress != null)
            {
                _context.Progresses.Remove(user.Progress);
            }

            _context.Enrollments.RemoveRange(user.Enrollments);
            _context.Submissions.RemoveRange(user.Submissions);
            _context.LessonCompletions.RemoveRange(user.LessonCompletions);
            _context.ProjectProgresses.RemoveRange(user.ProjectProgresses);

            // Finally remove the user
            _context.Users.Remove(user);

            await _context.SaveChangesAsync();
            return true;
        });
    }
}
