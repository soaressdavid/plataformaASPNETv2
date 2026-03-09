using Shared.Logging;
using Shared.Metrics;
using Shared.HealthChecks;
using Shared.Configuration;
using Execution.Service;
using Execution.Service.Services;
using System.Text.Json;
using Docker.DotNet;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);
builder.ConfigureSerilog("ExecutionService");

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Configure Redis
var redisConfig = builder.Configuration.GetSection("Redis").Get<RedisConfiguration>() ?? new RedisConfiguration();
var redisConnection = RedisConnectionFactory.GetConnection(redisConfig);
builder.Services.AddSingleton<IConnectionMultiplexer>(redisConnection);

// Configure Docker client
// Windows uses named pipe, Linux uses Unix socket
var dockerUri = OperatingSystem.IsWindows() 
    ? new Uri("npipe://./pipe/docker_engine")
    : new Uri("unix:///var/run/docker.sock");
var dockerClient = new DockerClientConfiguration(dockerUri).CreateClient();
builder.Services.AddSingleton<IDockerClient>(dockerClient);

// Register services
builder.Services.AddSingleton<SimpleCodeExecutor>();
builder.Services.AddSingleton<IJobQueueService, RedisJobQueueService>();
builder.Services.AddSingleton<IContainerPoolManager, ContainerPoolManager>();
builder.Services.AddSingleton<ProhibitedCodeScanner>();
builder.Services.AddSingleton<IDockerCodeExecutor, DockerCodeExecutor>();
builder.Services.AddSingleton<IExecutionCacheService, ExecutionCacheService>();
builder.Services.AddSingleton<ICodeCoverageService, CodeCoverageService>();
builder.Services.AddHostedService<ContainerPoolMaintenanceService>();

// Add health checks
builder.Services.AddPlatformHealthChecks(builder.Configuration, "ExecutionService");

var app = builder.Build();
app.UseCors("AllowAll");

// Initialize container pool warm pool
var poolManager = app.Services.GetRequiredService<IContainerPoolManager>();
var logger = app.Services.GetRequiredService<ILogger<Program>>();

logger.LogInformation("Initializing container pool warm pool...");
await poolManager.InitializeWarmPoolAsync();
logger.LogInformation("Container pool warm pool initialized");

// Add Prometheus metrics
app.UsePrometheusMetrics();

// Simple synchronous code execution
app.MapPost("/api/code/execute", async (HttpContext context, SimpleCodeExecutor executor, ILogger<Program> logger) =>
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

        var result = await executor.ExecuteAsync(request.Code);
        stopwatch.Stop();

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
            executionTimeMs = result.ExecutionTimeMs
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

// Container pool statistics endpoint
app.MapGet("/api/pool/stats", async (IContainerPoolManager poolManager) =>
{
    var stats = await poolManager.GetPoolStatsAsync();
    return Results.Ok(stats);
});

// Docker execution endpoint with caching
app.MapPost("/api/code/execute-docker", async (
    HttpContext context,
    IDockerCodeExecutor dockerExecutor,
    IExecutionCacheService cacheService,
    ILogger<Program> logger) =>
{
    try
    {
        using var reader = new StreamReader(context.Request.Body);
        var body = await reader.ReadToEndAsync();
        
        var request = JsonSerializer.Deserialize<DockerExecuteRequest>(body, new JsonSerializerOptions 
        { 
            PropertyNameCaseInsensitive = true 
        });

        if (request == null || string.IsNullOrWhiteSpace(request.Code))
        {
            return Results.BadRequest(new { error = "Code is required" });
        }

        logger.LogInformation("Docker execution request: {CodeLength} characters, Session: {SessionId}",
            request.Code.Length, request.SessionId ?? "none");

        // Check cache first
        var cachedResult = await cacheService.GetCachedResultAsync(request.Code);
        if (cachedResult != null)
        {
            logger.LogInformation("Returning cached result for code hash");
            return Results.Ok(cachedResult);
        }

        // Execute in Docker container
        var result = await dockerExecutor.ExecuteAsync(
            request.Code,
            request.SessionId,
            request.TimeoutSeconds ?? 60);

        // Cache successful results
        await cacheService.CacheResultAsync(request.Code, result);

        return Results.Ok(result);
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Docker execution failed");
        return Results.Json(new
        {
            jobId = Guid.NewGuid(),
            status = "Failed",
            error = $"Internal error: {ex.Message}"
        }, statusCode: 500);
    }
});

// Code coverage endpoint
app.MapPost("/api/code/coverage", async (
    HttpContext context,
    ICodeCoverageService coverageService,
    ILogger<Program> logger) =>
{
    try
    {
        using var reader = new StreamReader(context.Request.Body);
        var body = await reader.ReadToEndAsync();
        
        var request = JsonSerializer.Deserialize<CoverageRequest>(body, new JsonSerializerOptions 
        { 
            PropertyNameCaseInsensitive = true 
        });

        if (request == null || string.IsNullOrWhiteSpace(request.Code))
        {
            return Results.BadRequest(new { error = "Code is required" });
        }

        logger.LogInformation("Coverage calculation request");

        CodeCoverageResult result;
        
        if (!string.IsNullOrWhiteSpace(request.TestCode))
        {
            // Calculate actual coverage with tests
            result = await coverageService.CalculateCoverageAsync(request.Code, request.TestCode);
        }
        else
        {
            // Estimate coverage without tests
            result = await coverageService.EstimateCoverageAsync(request.Code);
        }

        return Results.Ok(result);
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Coverage calculation failed");
        return Results.Json(new
        {
            success = false,
            error = $"Internal error: {ex.Message}"
        }, statusCode: 500);
    }
});

// Cache statistics endpoint
app.MapGet("/api/cache/stats", async (IExecutionCacheService cacheService) =>
{
    var stats = await cacheService.GetStatisticsAsync();
    return Results.Ok(stats);
});

// Clear cache endpoint
app.MapDelete("/api/cache/clear", async (IExecutionCacheService cacheService) =>
{
    await cacheService.ClearAllAsync();
    return Results.Ok(new { message = "Cache cleared successfully" });
});

app.Run();

public class ExecuteRequest
{
    public string Code { get; set; } = "";
}

public class DockerExecuteRequest
{
    public string Code { get; set; } = "";
    public string? SessionId { get; set; }
    public int? TimeoutSeconds { get; set; }
}

public class CoverageRequest
{
    public string Code { get; set; } = "";
    public string? TestCode { get; set; }
}
