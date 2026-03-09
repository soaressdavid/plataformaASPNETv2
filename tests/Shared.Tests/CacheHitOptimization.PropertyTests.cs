using Xunit;
using Moq;
using Shared.Services;
using Shared.Models;
using Shared.Configuration;

namespace Tests;

/// <summary>
/// Property-Based Tests for Cache Hit Optimization
/// Feature: platform-evolution-saas
/// 
/// **Validates: Requirements 22.7**
/// 
/// Property 22: Cache Hit Optimization
/// For any code execution request with a code hash that exists in the cache,
/// the result SHALL be returned from cache without creating a new container
/// or re-executing the code.
/// </summary>
public class CacheHitOptimizationPropertyTests
{
    private Mock<IRedisCacheService> CreateMockRedisCache()
    {
        return new Mock<IRedisCacheService>();
    }
    
    private RedisConfiguration CreateTestConfig()
    {
        return new RedisConfiguration
        {
            TTL = new CacheTTLConfiguration
            {
                ExecutionResults = TimeSpan.FromHours(1)
            }
        };
    }
    
    /// <summary>
    /// Property 22: Cache Hit Optimization
    /// GIVEN identical code execution requests
    /// WHEN the same code is executed multiple times
    /// THEN subsequent executions should return cached results
    /// AND cache hit rate should be > 0% for repeated requests
    /// </summary>
    [Fact]
    public async Task Property22_CacheHitOptimization_ReturnsCachedResults()
    {
        // Arrange
        var mockRedisCache = CreateMockRedisCache();
        var config = CreateTestConfig();
        
        var cacheService = new CodeExecutionCacheService(mockRedisCache.Object, config);
        
        var code = "Console.WriteLine(\"Hello World\");";
        var input = "";
        
        var expectedResult = new ExecutionResult
        {
            JobId = Guid.NewGuid(),
            Status = ExecutionStatus.Completed,
            Output = "Hello World",
            ExitCode = 0,
            ExecutionTimeMs = 100,
            CompletedAt = DateTime.UtcNow
        };
        
        // Setup mock to return null on first call (cache miss), then return cached result
        var callCount = 0;
        mockRedisCache
            .Setup(m => m.GetAsync<ExecutionResult>(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(() =>
            {
                callCount++;
                return callCount == 1 ? null : expectedResult;
            });
        
        mockRedisCache
            .Setup(m => m.SetAsync(It.IsAny<string>(), It.IsAny<ExecutionResult>(), It.IsAny<TimeSpan>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        
        // Act - First execution (cache miss)
        var firstResult = await cacheService.GetCachedResultAsync(code, input);
        
        // Cache the result
        await cacheService.CacheResultAsync(code, input, expectedResult);
        
        // Act - Second execution (cache hit)
        var secondResult = await cacheService.GetCachedResultAsync(code, input);
        
        // Assert
        Assert.Null(firstResult); // First call should be cache miss
        Assert.NotNull(secondResult); // Second call should be cache hit
        Assert.Equal(expectedResult.JobId, secondResult.JobId);
        Assert.Equal(expectedResult.Status, secondResult.Status);
        Assert.Equal(expectedResult.Output, secondResult.Output);
    }
    
    /// <summary>
    /// Property 22.1: Cache Key Consistency
    /// GIVEN the same code and input
    /// WHEN cache key is generated multiple times
    /// THEN the same cache key should be produced
    /// </summary>
    [Fact]
    public async Task Property22_1_CacheKeyConsistency()
    {
        // Arrange
        var mockRedisCache = CreateMockRedisCache();
        var config = CreateTestConfig();
        var cacheService = new CodeExecutionCacheService(mockRedisCache.Object, config);
        
        var code = "Console.WriteLine(\"Test\");";
        var input = "test input";
        
        var capturedKeys = new List<string>();
        mockRedisCache
            .Setup(m => m.GetAsync<ExecutionResult>(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Callback<string, CancellationToken>((key, _) => capturedKeys.Add(key))
            .ReturnsAsync((ExecutionResult?)null);
        
        // Act - Call multiple times with same code and input
        await cacheService.GetCachedResultAsync(code, input);
        await cacheService.GetCachedResultAsync(code, input);
        await cacheService.GetCachedResultAsync(code, input);
        
        // Assert - All keys should be identical
        Assert.Equal(3, capturedKeys.Count);
        Assert.Equal(capturedKeys[0], capturedKeys[1]);
        Assert.Equal(capturedKeys[1], capturedKeys[2]);
    }

    
    /// <summary>
    /// Property 22.2: Different Code Produces Different Cache Keys
    /// GIVEN different code strings
    /// WHEN cache keys are generated
    /// THEN different cache keys should be produced
    /// </summary>
    [Fact]
    public async Task Property22_2_DifferentCodeProducesDifferentKeys()
    {
        // Arrange
        var mockRedisCache = CreateMockRedisCache();
        var config = CreateTestConfig();
        var cacheService = new CodeExecutionCacheService(mockRedisCache.Object, config);
        
        var code1 = "Console.WriteLine(\"Hello\");";
        var code2 = "Console.WriteLine(\"World\");";
        var input = "";
        
        var capturedKeys = new List<string>();
        mockRedisCache
            .Setup(m => m.GetAsync<ExecutionResult>(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Callback<string, CancellationToken>((key, _) => capturedKeys.Add(key))
            .ReturnsAsync((ExecutionResult?)null);
        
        // Act
        await cacheService.GetCachedResultAsync(code1, input);
        await cacheService.GetCachedResultAsync(code2, input);
        
        // Assert - Keys should be different
        Assert.Equal(2, capturedKeys.Count);
        Assert.NotEqual(capturedKeys[0], capturedKeys[1]);
    }
    
    /// <summary>
    /// Property 22.3: Cache TTL Configuration
    /// GIVEN a cache service with configured TTL
    /// WHEN caching a result
    /// THEN the TTL should be set to 1 hour as per requirements
    /// </summary>
    [Fact]
    public async Task Property22_3_CacheTTLIsOneHour()
    {
        // Arrange
        var mockRedisCache = CreateMockRedisCache();
        var config = CreateTestConfig();
        var cacheService = new CodeExecutionCacheService(mockRedisCache.Object, config);
        
        var result = new ExecutionResult
        {
            JobId = Guid.NewGuid(),
            Status = ExecutionStatus.Completed,
            Output = "Test",
            ExitCode = 0,
            ExecutionTimeMs = 100,
            CompletedAt = DateTime.UtcNow
        };
        
        TimeSpan? capturedTTL = null;
        mockRedisCache
            .Setup(m => m.SetAsync(It.IsAny<string>(), It.IsAny<ExecutionResult>(), It.IsAny<TimeSpan?>(), It.IsAny<CancellationToken>()))
            .Callback<string, ExecutionResult, TimeSpan?, CancellationToken>((_, _, ttl, _) => capturedTTL = ttl)
            .ReturnsAsync(true);
        
        // Act
        await cacheService.CacheResultAsync("test code", "test input", result);
        
        // Assert
        Assert.NotNull(capturedTTL);
        Assert.Equal(TimeSpan.FromHours(1), capturedTTL.Value);
    }

    
    /// <summary>
    /// Property 22.4: Cache Invalidation
    /// GIVEN cached results for specific code
    /// WHEN cache is invalidated for that code
    /// THEN subsequent lookups should miss the cache
    /// </summary>
    [Fact]
    public async Task Property22_4_CacheInvalidation()
    {
        // Arrange
        var mockRedisCache = CreateMockRedisCache();
        var config = CreateTestConfig();
        var cacheService = new CodeExecutionCacheService(mockRedisCache.Object, config);
        
        var code = "Console.WriteLine(\"Test\");";
        var input = "";
        
        var result = new ExecutionResult
        {
            JobId = Guid.NewGuid(),
            Status = ExecutionStatus.Completed,
            Output = "Test",
            ExitCode = 0,
            ExecutionTimeMs = 100,
            CompletedAt = DateTime.UtcNow
        };
        
        var isInvalidated = false;
        mockRedisCache
            .Setup(m => m.GetAsync<ExecutionResult>(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(() => isInvalidated ? null : result);
        
        mockRedisCache
            .Setup(m => m.InvalidateAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Callback(() => isInvalidated = true)
            .Returns(Task.CompletedTask);
        
        // Act
        var beforeInvalidation = await cacheService.GetCachedResultAsync(code, input);
        await cacheService.InvalidateAsync(code);
        var afterInvalidation = await cacheService.GetCachedResultAsync(code, input);
        
        // Assert
        Assert.NotNull(beforeInvalidation); // Should have cached result before invalidation
        Assert.Null(afterInvalidation); // Should not have cached result after invalidation
    }
    
    /// <summary>
    /// Property 22.5: Cache Hit Rate Calculation
    /// GIVEN multiple executions with some repeated code
    /// WHEN calculating cache hit rate
    /// THEN hit rate should be > 0% for repeated requests
    /// </summary>
    [Fact]
    public async Task Property22_5_CacheHitRateGreaterThanZeroForRepeatedRequests()
    {
        // Arrange
        var mockRedisCache = CreateMockRedisCache();
        var config = CreateTestConfig();
        var cacheService = new CodeExecutionCacheService(mockRedisCache.Object, config);
        
        var cachedResults = new Dictionary<string, ExecutionResult>();
        
        mockRedisCache
            .Setup(m => m.GetAsync<ExecutionResult>(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((string key, CancellationToken _) => 
                cachedResults.ContainsKey(key) ? cachedResults[key] : null);
        
        mockRedisCache
            .Setup(m => m.SetAsync(It.IsAny<string>(), It.IsAny<ExecutionResult>(), It.IsAny<TimeSpan?>(), It.IsAny<CancellationToken>()))
            .Callback<string, ExecutionResult, TimeSpan?, CancellationToken>((key, result, _, _) => 
                cachedResults[key] = result)
            .ReturnsAsync(true);
        
        var result = new ExecutionResult
        {
            JobId = Guid.NewGuid(),
            Status = ExecutionStatus.Completed,
            Output = "Test",
            ExitCode = 0,
            ExecutionTimeMs = 100,
            CompletedAt = DateTime.UtcNow
        };
        
        // Act - Execute same code multiple times
        var code = "Console.WriteLine(\"Hello\");";
        var input = "";
        
        int totalRequests = 0;
        int cacheHits = 0;
        
        // First execution - cache miss
        var firstResult = await cacheService.GetCachedResultAsync(code, input);
        totalRequests++;
        if (firstResult != null) cacheHits++;
        
        // Cache the result
        await cacheService.CacheResultAsync(code, input, result);
        
        // Subsequent executions - should be cache hits
        for (int i = 0; i < 5; i++)
        {
            var cachedResult = await cacheService.GetCachedResultAsync(code, input);
            totalRequests++;
            if (cachedResult != null) cacheHits++;
        }
        
        // Calculate hit rate
        var hitRate = (double)cacheHits / totalRequests * 100;
        
        // Assert
        Assert.True(hitRate > 0, $"Cache hit rate should be > 0% for repeated requests. Actual: {hitRate}%");
        Assert.Equal(5, cacheHits); // 5 out of 6 requests should be cache hits
        Assert.Equal(83.33, hitRate, 2); // ~83.33% hit rate
    }
}
