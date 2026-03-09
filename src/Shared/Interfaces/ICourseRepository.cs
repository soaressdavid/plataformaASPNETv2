using Shared.Entities;

namespace Shared.Interfaces;

public interface ICourseRepository : IRepository<Course>
{
    Task<List<Lesson>> GetLessonsAsync(Guid courseId);
}
