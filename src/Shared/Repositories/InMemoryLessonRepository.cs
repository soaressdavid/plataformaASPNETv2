using Shared.Entities;
using Shared.Interfaces;

namespace Shared.Repositories;

/// <summary>
/// In-memory implementation of ILessonRepository for development and testing.
/// Stores lessons in a Dictionary for fast access without database dependency.
/// </summary>
public class InMemoryLessonRepository : ILessonRepository
{
    private readonly Dictionary<Guid, Lesson> _lessons = new();
    private readonly object _lock = new();

    public Task<Lesson?> GetByIdAsync(Guid id)
    {
        lock (_lock)
        {
            _lessons.TryGetValue(id, out var lesson);
            return Task.FromResult(lesson);
        }
    }

    public Task<IEnumerable<Lesson>> GetAllAsync()
    {
        lock (_lock)
        {
            return Task.FromResult<IEnumerable<Lesson>>(_lessons.Values.ToList());
        }
    }

    public Task<List<Lesson>> GetByCourseIdAsync(Guid courseId)
    {
        lock (_lock)
        {
            var lessons = _lessons.Values
                .Where(l => l.CourseId == courseId)
                .OrderBy(l => l.OrderIndex)
                .ToList();
            return Task.FromResult(lessons);
        }
    }

    public Task<List<Lesson>> SearchAsync(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            return Task.FromResult(new List<Lesson>());
        }

        lock (_lock)
        {
            var lowerQuery = query.ToLowerInvariant();
            var results = _lessons.Values
                .Where(l => 
                    l.Title.ToLowerInvariant().Contains(lowerQuery) ||
                    (l.StructuredContent != null && 
                     l.StructuredContent.ToLowerInvariant().Contains(lowerQuery)))
                .ToList();
            return Task.FromResult(results);
        }
    }

    public Task<Lesson> CreateAsync(Lesson entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        lock (_lock)
        {
            if (entity.Id == Guid.Empty)
            {
                entity.Id = Guid.NewGuid();
            }

            if (_lessons.ContainsKey(entity.Id))
            {
                throw new InvalidOperationException($"Lesson with ID {entity.Id} already exists.");
            }

            entity.CreatedAt = DateTime.UtcNow;
            entity.UpdatedAt = DateTime.UtcNow;
            _lessons[entity.Id] = entity;
            return Task.FromResult(entity);
        }
    }

    public Task UpdateAsync(Lesson entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        lock (_lock)
        {
            if (!_lessons.ContainsKey(entity.Id))
            {
                throw new InvalidOperationException($"Lesson with ID {entity.Id} does not exist.");
            }

            entity.UpdatedAt = DateTime.UtcNow;
            _lessons[entity.Id] = entity;
            return Task.CompletedTask;
        }
    }

    public Task DeleteAsync(Guid id)
    {
        lock (_lock)
        {
            _lessons.Remove(id);
            return Task.CompletedTask;
        }
    }
}
