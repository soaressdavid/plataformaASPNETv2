# Redis Cache Integration Example

This document shows how to integrate Redis caching into microservices.

## Example: Execution Service with Redis Cache

### 1. Update Program.cs

```csharp
using Shared.Logging;
using Shared.Metrics;
using Shared.HealthChecks;
using Shared.Services;
using Shared.Configuration;
using Execution.Service;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);
builder.ConfigureSerilog("ExecutionService");

// Add Redis cache with connection pooling
builder.Services.AddRedisCache(config =>
{
    config.ConnectionString = builder.Configuration.GetConnectionString("Redis") 
        ?? "redis-cluster-service.aspnet-learning-platform.svc.cluster.local:6379";
    config.UseCluster = true;
    config.ConnectTimeout = 5000;
    config.SyncTimeout = 5000;
    config.ConnectRetry = 3;
    config.AbortOnConnectFail = false;
});

// Add specialized cache services
builder.Services.AddSingleton<ICodeExecutionCacheService, CodeExecutionCacheService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddSingleton<SimpleCodeExecutor>();

// Add health checks
builder.Services.AddPlatformHealthChecks(builder.Configuration, "ExecutionService");

var app = builder.Build();
app.UseCors("AllowAll");

// Add Prometheus metrics
app.UsePrometheusMetrics();

// Code execution with caching
app.MapPost("/api/code/execute", async (
    HttpContext context, 
    SimpleCodeExecutor executor, 
    ICodeExecutionCacheService cache,
    ILogger<Program> logger) =>
{
    var stopwatch = System.Diagnostics.Stopwatch.StartNew();
    
    try
    {
        using var reader = new StreamReader(context.Request.Body);
        var body = await reader.ReadToEndAsync();
        
        var request = JsonSerializer.Deserialize<ExecuteRequest>(body, new JsonSerializerOptions 
        { 
            PropertyNameCaseInsensitive = true 
        });

        if (request == null || string.IsNullOrWhiteSpace(request.Code))
        {
            return Results.BadRequest(new { error = "Code is required" });
        }

        logger.LogInformation("Executing code: {CodeLength} characters", request.Code.Length);

        // Check cache first (Requirement 22.7)
        var cachedResult = await cache.GetCachedResultAsync(request.Code, request.Input ?? "");
        
        if (cachedResult != null)
        {
            logger.LogInformation("Cache hit for code execution");
            ApplicationMetrics.CacheHitCount.Inc();
            
            return Results.Ok(new
            {
                jobId = Guid.NewGuid().ToString(),
                status = cachedResult.Status,
                output = cachedResult.Output,
                error = cachedResult.Error,
                executionTimeMs = cachedResult.ExecutionTimeMs,
                cached = true
            });
        }
        
        ApplicationMetrics.CacheMissCount.Inc();

        // Execute code
        var result = await executor.ExecuteAsync(request.Code);
        stopwatch.Stop();

        // Cache the result (Requirement 22.4 - 1 hour TTL)
        await cache.CacheResultAsync(request.Code, request.Input ?? "", result);

        // Track execution metrics
        ApplicationMetrics.ExecutionCount
            .WithLabels(result.Status)
            .Inc();

        ApplicationMetrics.ExecutionDuration
            .WithLabels(result.Status)
            .Observe(stopwatch.Elapsed.TotalSeconds);

        if (result.Status == "Completed")
        {
            ApplicationMetrics.ExecutionSuccessCount.Inc();
        }
        else
        {
            ApplicationMetrics.ExecutionFailureCount
                .WithLabels(result.Status)
                .Inc();
        }

        var response = new
        {
            jobId = Guid.NewGuid().ToString(),
            status = result.Status,
            output = result.Output,
            error = result.Error,
            executionTimeMs = result.ExecutionTimeMs,
            cached = false
        };

        logger.LogInformation("Execution completed: Status={Status}, Time={Time}ms", result.Status, result.ExecutionTimeMs);

        return Results.Ok(response);
    }
    catch (Exception ex)
    {
        stopwatch.Stop();
        
        logger.LogError(ex, "Code execution failed");
        
        // Track failure metrics
        ApplicationMetrics.ExecutionCount
            .WithLabels("Failed")
            .Inc();
        
        ApplicationMetrics.ExecutionFailureCount
            .WithLabels("Exception")
            .Inc();
        
        return Results.Json(new
        {
            jobId = Guid.NewGuid().ToString(),
            status = "Failed",
            output = "",
            executionTimeMs = 0,
            error = $"Internal error: {ex.Message}"
        }, statusCode: 500);
    }
});

app.MapPlatformHealthChecks("/health");

app.Run();

public class ExecuteRequest
{
    public string Code { get; set; } = "";
    public string? Input { get; set; }
}
```

### 2. Update appsettings.json

```json
{
  "ConnectionStrings": {
    "Redis": "redis-cluster-service.aspnet-learning-platform.svc.cluster.local:6379"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}
```

### 3. Add Metrics for Cache Monitoring

```csharp
// In Shared/Metrics/ApplicationMetrics.cs
public static class ApplicationMetrics
{
    // Existing metrics...
    
    // Cache metrics (Requirement 22.10)
    public static readonly Counter CacheHitCount = Metrics.CreateCounter(
        "cache_hits_total",
        "Total number of cache hits"
    );
    
    public static readonly Counter CacheMissCount = Metrics.CreateCounter(
        "cache_misses_total",
        "Total number of cache misses"
    );
    
    public static readonly Gauge CacheHitRate = Metrics.CreateGauge(
        "cache_hit_rate",
        "Cache hit rate percentage"
    );
}
```

## Example: Leaderboard Service with Redis

```csharp
using Shared.Services;
using Shared.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add Redis cache
builder.Services.AddRedisCache(config =>
{
    config.ConnectionString = builder.Configuration.GetConnectionString("Redis") 
        ?? "redis-cluster-service.aspnet-learning-platform.svc.cluster.local:6379";
    config.UseCluster = true;
});

// Add leaderboard cache service
builder.Services.AddSingleton<ILeaderboardCacheService, LeaderboardCacheService>();

var app = builder.Build();

// Get top users endpoint
app.MapGet("/api/leaderboard/top", async (
    ILeaderboardCacheService leaderboard,
    int count = 100) =>
{
    var topUsers = await leaderboard.GetTopUsersAsync(count);
    return Results.Ok(topUsers);
});

// Update user score endpoint
app.MapPost("/api/leaderboard/update", async (
    ILeaderboardCacheService leaderboard,
    UpdateScoreRequest request) =>
{
    await leaderboard.UpdateUserScoreAsync(request.UserId, request.TotalXP);
    return Results.Ok();
});

app.Run();

public record UpdateScoreRequest(string UserId, int TotalXP);
```

## Example: AI Tutor Service with Rate Limiting

```csharp
using Shared.Services;
using Shared.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add Redis cache
builder.Services.AddRedisCache(config =>
{
    config.ConnectionString = builder.Configuration.GetConnectionString("Redis") 
        ?? "redis-cluster-service.aspnet-learning-platform.svc.cluster.local:6379";
    config.UseCluster = true;
});

// Add rate limit cache service
builder.Services.AddSingleton<IRateLimitCacheService, RateLimitCacheService>();

var app = builder.Build();

// AI Tutor endpoint with rate limiting
app.MapPost("/api/ai/analyze", async (
    IRateLimitCacheService rateLimit,
    HttpContext context,
    AnalyzeRequest request) =>
{
    var userId = context.User.FindFirst("sub")?.Value ?? "anonymous";
    var isPremium = context.User.IsInRole("Premium");
    
    // Rate limit: 10 req/hour for free, 50 req/hour for premium
    var limit = isPremium ? 50 : 10;
    var allowed = await rateLimit.CheckRateLimitAsync(
        userId, 
        "ai-tutor", 
        limit, 
        TimeSpan.FromHours(1)
    );
    
    if (!allowed)
    {
        var remaining = await rateLimit.GetRemainingRequestsAsync(
            userId, 
            "ai-tutor", 
            limit, 
            TimeSpan.FromHours(1)
        );
        
        return Results.Json(new
        {
            error = "Rate limit exceeded",
            limit = limit,
            remaining = remaining,
            resetIn = "1 hour"
        }, statusCode: 429);
    }
    
    // Process AI request...
    var result = await ProcessAIRequest(request);
    
    return Results.Ok(result);
});

app.Run();

public record AnalyzeRequest(string Code);
```

## Testing Redis Connection

```csharp
// Add a health check endpoint
app.MapGet("/health/redis", async (IRedisCacheService cache) =>
{
    try
    {
        var testKey = "health:check";
        var testValue = DateTime.UtcNow.ToString();
        
        await cache.SetAsync(testKey, testValue, TimeSpan.FromSeconds(10));
        var retrieved = await cache.GetAsync<string>(testKey);
        
        if (retrieved == testValue)
        {
            return Results.Ok(new { status = "healthy", message = "Redis is working" });
        }
        
        return Results.Json(new { status = "unhealthy", message = "Redis read/write mismatch" }, statusCode: 503);
    }
    catch (Exception ex)
    {
        return Results.Json(new { status = "unhealthy", message = ex.Message }, statusCode: 503);
    }
});
```

## Cache Invalidation Example

```csharp
// When course content is updated
app.MapPut("/api/courses/{id}", async (
    string id,
    IRedisCacheService cache,
    UpdateCourseRequest request) =>
{
    // Update course in database...
    await UpdateCourseInDatabase(id, request);
    
    // Invalidate cache (Requirement 22.8)
    await cache.InvalidateAsync($"course:{id}:*");
    
    return Results.Ok();
});
```

## Monitoring Cache Hit Rate

```csharp
// Background service to monitor cache hit rate (Requirement 22.10)
public class CacheMonitoringService : BackgroundService
{
    private readonly IRedisCacheService _cache;
    private readonly ILogger<CacheMonitoringService> _logger;
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                // Calculate hit rate from metrics
                var hits = ApplicationMetrics.CacheHitCount.Value;
                var misses = ApplicationMetrics.CacheMissCount.Value;
                var total = hits + misses;
                
                if (total > 0)
                {
                    var hitRate = (double)hits / total * 100;
                    ApplicationMetrics.CacheHitRate.Set(hitRate);
                    
                    // Alert if hit rate drops below 70% (Requirement 22.10)
                    if (hitRate < 70)
                    {
                        _logger.LogWarning(
                            "Cache hit rate is below 70%: {HitRate:F2}% (Hits: {Hits}, Misses: {Misses})",
                            hitRate, hits, misses
                        );
                    }
                }
                
                await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error monitoring cache hit rate");
            }
        }
    }
}
```

## Best Practices

1. **Always use connection pooling**: Use the singleton `IConnectionMultiplexer` pattern
2. **Set appropriate TTLs**: Follow the requirements (1h for execution, 5min for leaderboard, 24h for content)
3. **Handle cache misses gracefully**: Always have a fallback to the primary data source
4. **Monitor cache hit rate**: Alert if it drops below 70%
5. **Invalidate on updates**: Clear cache when underlying data changes
6. **Use typed keys**: Prefix keys with resource type (e.g., `exec:`, `leaderboard:`, `course:`)
7. **Implement retry logic**: Use exponential backoff for transient failures
8. **Test cache failures**: Ensure application works even if Redis is unavailable
