using Microsoft.EntityFrameworkCore;
using Shared.Data;
using Shared.Entities;
using Shared.Interfaces;

namespace Shared.Repositories;

public class CourseRepository : BaseRepository<Course>, ICourseRepository
{
    public CourseRepository(ApplicationDbContext context) : base(context)
    {
    }

    public override async Task<Course?> GetByIdAsync(Guid id)
    {
        return await _retryPolicy.ExecuteAsync(async () =>
            await _context.Courses
                .Include(c => c.Lessons)
                .Include(c => c.Enrollments)
                .FirstOrDefaultAsync(c => c.Id == id)
        );
    }

    public override async Task<IEnumerable<Course>> GetAllAsync()
    {
        return await _retryPolicy.ExecuteAsync(async () =>
            await _context.Courses
                .Include(c => c.Lessons)
                .ToListAsync()
        );
    }

    public async Task<List<Lesson>> GetLessonsAsync(Guid courseId)
    {
        return await _retryPolicy.ExecuteAsync(async () =>
            await _context.Lessons
                .Where(l => l.CourseId == courseId)
                .OrderBy(l => l.OrderIndex)
                .ToListAsync()
        );
    }
}
