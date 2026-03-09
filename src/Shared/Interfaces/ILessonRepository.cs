using Shared.Entities;

namespace Shared.Interfaces;

public interface ILessonRepository : IRepository<Lesson>
{
    Task<List<Lesson>> GetByCourseIdAsync(Guid courseId);
    Task<List<Lesson>> SearchAsync(string query);
}
