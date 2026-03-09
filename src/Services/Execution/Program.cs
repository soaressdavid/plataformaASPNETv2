using Shared.Logging;
using Shared.Metrics;
using Shared.HealthChecks;
using Execution.Service;
using System.Text.Json;

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

builder.Services.AddSingleton<SimpleCodeExecutor>();

// Add health checks
builder.Services.AddPlatformHealthChecks(builder.Configuration, "ExecutionService");

var app = builder.Build();
app.UseCors("AllowAll");

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

app.Run();

public class ExecuteRequest
{
    public string Code { get; set; } = "";
}
