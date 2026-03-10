using Shared.Logging;
using Shared.Metrics;
using Shared.HealthChecks;
using Shared.Configuration;
using Execution.Service;
using System.Text.Json;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Reflection;
using System.Text;
using System.Diagnostics;

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

// Register services (SEM DOCKER)
builder.Services.AddSingleton<InProcessCodeExecutor>();
builder.Services.AddSingleton<ProhibitedCodeScanner>();

// Add health checks
builder.Services.AddPlatformHealthChecks(builder.Configuration, "ExecutionService");

var app = builder.Build();
app.UseCors("AllowAll");

// Add Prometheus metrics
app.UsePrometheusMetrics();

// Simple synchronous code execution (SEM DOCKER)
app.MapPost("/api/code/execute", async (HttpContext context, InProcessCodeExecutor executor, ILogger<Program> logger) =>
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

// Executor de código sem Docker (in-process)
public class InProcessCodeExecutor
{
    private readonly ILogger<InProcessCodeExecutor> _logger;
    private readonly ProhibitedCodeScanner _scanner;

    public InProcessCodeExecutor(ILogger<InProcessCodeExecutor> logger, ProhibitedCodeScanner scanner)
    {
        _logger = logger;
        _scanner = scanner;
    }

    public async Task<ExecutionResult> ExecuteAsync(string code)
    {
        var stopwatch = Stopwatch.StartNew();
        
        try
        {
            // Verificar código proibido
            var scanResult = _scanner.ScanCode(code);
            if (!scanResult.IsAllowed)
            {
                return new ExecutionResult
                {
                    Status = "Blocked",
                    Error = $"Código contém operações proibidas: {string.Join(", ", scanResult.ViolatedRules)}",
                    ExecutionTimeMs = (int)stopwatch.ElapsedMilliseconds
                };
            }

            // Compilar código
            var compilation = await CompileCodeAsync(code);
            if (!compilation.Success)
            {
                return new ExecutionResult
                {
                    Status = "CompilationError",
                    Error = compilation.Error,
                    ExecutionTimeMs = (int)stopwatch.ElapsedMilliseconds
                };
            }

            // Executar código
            var output = await ExecuteCompiledCodeAsync(compilation.Assembly);
            
            stopwatch.Stop();
            
            return new ExecutionResult
            {
                Status = "Completed",
                Output = output,
                ExecutionTimeMs = (int)stopwatch.ElapsedMilliseconds
            };
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            _logger.LogError(ex, "Execution failed");
            
            return new ExecutionResult
            {
                Status = "RuntimeError",
                Error = ex.Message,
                ExecutionTimeMs = (int)stopwatch.ElapsedMilliseconds
            };
        }
    }

    private async Task<CompilationResult> CompileCodeAsync(string code)
    {
        try
        {
            // Adicionar using statements se não existirem
            var fullCode = $@"
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Program
{{
    public static void Main()
    {{
        {code}
    }}
}}";

            var syntaxTree = CSharpSyntaxTree.ParseText(fullCode);
            
            var references = new[]
            {
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Console).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(System.Runtime.AssemblyTargetedPatchBandAttribute).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Microsoft.CSharp.RuntimeBinder.CSharpArgumentInfo).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(System.Linq.Enumerable).Assembly.Location)
            };

            var compilation = CSharpCompilation.Create(
                "DynamicCode",
                new[] { syntaxTree },
                references,
                new CSharpCompilationOptions(OutputKind.ConsoleApplication));

            using var ms = new MemoryStream();
            var result = compilation.Emit(ms);

            if (!result.Success)
            {
                var errors = string.Join("\n", result.Diagnostics
                    .Where(d => d.Severity == DiagnosticSeverity.Error)
                    .Select(d => d.GetMessage()));

                return new CompilationResult { Success = false, Error = errors };
            }

            ms.Seek(0, SeekOrigin.Begin);
            var assembly = Assembly.Load(ms.ToArray());
            
            return new CompilationResult { Success = true, Assembly = assembly };
        }
        catch (Exception ex)
        {
            return new CompilationResult { Success = false, Error = ex.Message };
        }
    }

    private async Task<string> ExecuteCompiledCodeAsync(Assembly assembly)
    {
        var output = new StringBuilder();
        var originalOut = Console.Out;
        
        try
        {
            // Redirecionar Console.WriteLine para capturar output
            using var writer = new StringWriter(output);
            Console.SetOut(writer);
            
            // Executar com timeout
            var task = Task.Run(() =>
            {
                var entryPoint = assembly.EntryPoint;
                entryPoint?.Invoke(null, new object[] { new string[0] });
            });

            // Timeout de 10 segundos
            if (await Task.WhenAny(task, Task.Delay(10000)) == task)
            {
                await task; // Aguardar conclusão
                return output.ToString();
            }
            else
            {
                throw new TimeoutException("Código executou por mais de 10 segundos");
            }
        }
        finally
        {
            Console.SetOut(originalOut);
        }
    }
}

public class CompilationResult
{
    public bool Success { get; set; }
    public string Error { get; set; } = "";
    public Assembly? Assembly { get; set; }
}

public class ExecuteRequest
{
    public string Code { get; set; } = "";
}

public class ExecutionResult
{
    public string Status { get; set; } = "";
    public string Output { get; set; } = "";
    public string Error { get; set; } = "";
    public int ExecutionTimeMs { get; set; }
}
