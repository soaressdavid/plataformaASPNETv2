using Microsoft.EntityFrameworkCore;
using Shared.Data;
using Shared.Entities;
using Shared.Interfaces;

namespace Shared.Repositories;

public class ProjectRepository : BaseRepository<Project>, IProjectRepository
{
    public ProjectRepository(ApplicationDbContext context) : base(context)
    {
    }

    public override async Task<Project?> GetByIdAsync(Guid id)
    {
        return await _retryPolicy.ExecuteAsync(async () =>
            await _context.Projects
                .FirstOrDefaultAsync(p => p.Id == id)
        );
    }

    public override async Task<IEnumerable<Project>> GetAllAsync()
    {
        return await _retryPolicy.ExecuteAsync(async () =>
            await _context.Projects
                .ToListAsync()
        );
    }
}
