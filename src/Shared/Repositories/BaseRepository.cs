using Microsoft.EntityFrameworkCore;
using Polly;
using Polly.Retry;
using Shared.Data;
using Shared.Interfaces;
using Shared.Models;

namespace Shared.Repositories;

public abstract class BaseRepository<T> : IRepository<T> where T : BaseEntity
{
    protected readonly ApplicationDbContext _context;
    protected readonly AsyncRetryPolicy _retryPolicy;

    protected BaseRepository(ApplicationDbContext context)
    {
        _context = context;
        
        // Configure retry policy: 3 retries with exponential backoff (100ms, 200ms, 400ms)
        _retryPolicy = Policy
            .Handle<DbUpdateException>()
            .Or<TimeoutException>()
            .Or<InvalidOperationException>(ex => ex.Message.Contains("connection"))
            .WaitAndRetryAsync(
                retryCount: 3,
                sleepDurationProvider: retryAttempt => TimeSpan.FromMilliseconds(100 * Math.Pow(2, retryAttempt - 1))
            );
    }

    public virtual async Task<T?> GetByIdAsync(Guid id)
    {
        return await _retryPolicy.ExecuteAsync(async () =>
            await _context.Set<T>().FindAsync(id)
        );
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _retryPolicy.ExecuteAsync(async () =>
            await _context.Set<T>().ToListAsync()
        );
    }

    public virtual async Task<T> CreateAsync(T entity)
    {
        return await _retryPolicy.ExecuteAsync(async () =>
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        });
    }

    public virtual async Task UpdateAsync(T entity)
    {
        await _retryPolicy.ExecuteAsync(async () =>
        {
            _context.Set<T>().Update(entity);
            await _context.SaveChangesAsync();
        });
    }

    public virtual async Task DeleteAsync(Guid id)
    {
        await _retryPolicy.ExecuteAsync(async () =>
        {
            var entity = await _context.Set<T>().FindAsync(id);
            if (entity != null)
            {
                _context.Set<T>().Remove(entity);
                await _context.SaveChangesAsync();
            }
        });
    }
}
