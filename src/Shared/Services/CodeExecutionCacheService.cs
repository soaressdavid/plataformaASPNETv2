using System.Security.Cryptography;
using System.Text;
using Shared.Models;

namespace Shared.Services;

/// <summary>
/// Cache service for code execution results
/// Validates: Requirements 22.1, 22.4 - Cache execution results with 1h TTL
/// </summary>
public interface ICodeExecutionCacheService
{
    Task<ExecutionResult?> GetCachedResultAsync(string code, string input, CancellationToken cancellationToken = default);
    Task CacheResultAsync(string code, string input, ExecutionResult result, CancellationToken cancellationToken = default);
    Task InvalidateAsync(string code, CancellationToken cancellationToken = default);
}

public class CodeExecutionCacheService : ICodeExecutionCacheService
{
    private readonly IRedisCacheService _cache;
    private readonly TimeSpan _ttl;
    private const string KeyPrefix = "exec:";

    public CodeExecutionCacheService(IRedisCacheService cache, Configuration.RedisConfiguration config)
    {
        _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        _ttl = config.TTL.ExecutionResults;
    }

    /// <summary>
    /// Gets cached execution result if available
    /// Validates: Requirement 22.7 - Return cached result without re-execution
    /// </summary>
    public async Task<ExecutionResult?> GetCachedResultAsync(
        string code, 
        string input, 
        CancellationToken cancellationToken = default)
    {
        var key = GenerateCacheKey(code, input);
        return await _cache.GetAsync<ExecutionResult>(key, cancellationToken);
    }

    /// <summary>
    /// Caches execution result with 1 hour TTL
    /// Validates: Requirement 22.4 - Cache TTL to 1 hour for execution results
    /// </summary>
    public async Task CacheResultAsync(
        string code, 
        string input, 
        ExecutionResult result, 
        CancellationToken cancellationToken = default)
    {
        var key = GenerateCacheKey(code, input);
        await _cache.SetAsync(key, result, _ttl, cancellationToken);
    }

    /// <summary>
    /// Invalidates cached results for specific code
    /// Validates: Requirement 22.8 - Invalidate cache when content is updated
    /// </summary>
    public async Task InvalidateAsync(string code, CancellationToken cancellationToken = default)
    {
        var hash = ComputeHash(code);
        var pattern = $"{KeyPrefix}{hash}:*";
        await _cache.InvalidateAsync(pattern, cancellationToken);
    }

    private string GenerateCacheKey(string code, string input)
    {
        var codeHash = ComputeHash(code);
        var inputHash = ComputeHash(input);
        return $"{KeyPrefix}{codeHash}:{inputHash}";
    }

    private string ComputeHash(string input)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(input));
        return Convert.ToHexString(bytes).ToLowerInvariant();
    }
}
