using Shared.Logging;
using Shared.HealthChecks;
using System.Text.Json;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using System.Reflection;
using System.Text;

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

// Add health checks
builder.Services.AddPlatformHealthChecks(builder.Configuration, "ExecutionService");

var app = builder.Build();
app.UseCors("AllowAll");

// Execution Service REAL - Compilação e Execução Real de C#
app.MapPost("/api/code/execute", async (HttpContext context, ILogger<Program> logger) =>
{
    try
    {
        logger.LogInformation("Received code execution request");
        
        using var reader = new StreamReader(context.Request.Body);
        var body = await reader.ReadToEndAsync();
        
        if (string.IsNullOrWhiteSpace(body))
        {
            return Results.BadRequest(new { error = "Request body is empty" });
        }
        
        var request = JsonSerializer.Deserialize<ExecuteRequest>(body, new JsonSerializerOptions 
        { 
            PropertyNameCaseInsensitive = true 
        });

        if (request == null || string.IsNullOrWhiteSpace(request.Code))
        {
            return Results.BadRequest(new { error = "Code is required" });
        }

        logger.LogInformation("Executing real C# code: {CodeLength} characters", request.Code.Length);

        var startTime = DateTime.UtcNow;
        
        // Preparar código para compilação
        var fullCode = $@"
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Program
{{
    public static void Main()
    {{
        {request.Code}
    }}
}}";

        // Compilar código
        var syntaxTree = CSharpSyntaxTree.ParseText(fullCode);
        
        var references = new[]
        {
            MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(Console).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(System.Runtime.AssemblyTargetedPatchBandAttribute).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(Microsoft.CSharp.RuntimeBinder.CSharpArgumentInfo).Assembly.Location),
            MetadataReference.CreateFromFile(Assembly.Load("System.Runtime").Location),
            MetadataReference.CreateFromFile(Assembly.Load("System.Collections").Location),
            MetadataReference.CreateFromFile(Assembly.Load("System.Linq").Location)
        };

        var compilation = CSharpCompilation.Create(
            "DynamicAssembly",
            new[] { syntaxTree },
            references,
            new CSharpCompilationOptions(OutputKind.ConsoleApplication));

        using var ms = new MemoryStream();
        var result = compilation.Emit(ms);

        if (!result.Success)
        {
            var errors = string.Join("\n", result.Diagnostics
                .Where(d => d.IsWarningAsError || d.Severity == DiagnosticSeverity.Error)
                .Select(d => $"Line {d.Location.GetLineSpan().StartLinePosition.Line + 1}: {d.GetMessage()}"));

            logger.LogWarning("Compilation failed: {Errors}", errors);

            return Results.Json(new
            {
                jobId = Guid.NewGuid().ToString(),
                status = "Failed",
                output = "",
                error = $"Compilation Error:\n{errors}",
                executionTimeMs = (int)(DateTime.UtcNow - startTime).TotalMilliseconds
            }, statusCode: 400);
        }

        // Executar código compilado
        ms.Seek(0, SeekOrigin.Begin);
        var assembly = Assembly.Load(ms.ToArray());
        var type = assembly.GetType("Program");
        var method = type?.GetMethod("Main");

        if (method == null)
        {
            return Results.Json(new
            {
                jobId = Guid.NewGuid().ToString(),
                status = "Failed",
                output = "",
                error = "Main method not found",
                executionTimeMs = (int)(DateTime.UtcNow - startTime).TotalMilliseconds
            }, statusCode: 400);
        }

        // Capturar output do console
        var originalOut = Console.Out;
        var originalError = Console.Error;
        
        using var outputWriter = new StringWriter();
        using var errorWriter = new StringWriter();
        
        Console.SetOut(outputWriter);
        Console.SetError(errorWriter);

        try
        {
            // Executar com timeout de 10 segundos
            var task = Task.Run(() => method.Invoke(null, null));
            var completed = await task.WaitAsync(TimeSpan.FromSeconds(10));
            
            var output = outputWriter.ToString();
            var errorOutput = errorWriter.ToString();
            
            var finalOutput = output;
            if (!string.IsNullOrEmpty(errorOutput))
            {
                finalOutput += "\nErrors:\n" + errorOutput;
            }

            var executionTime = (int)(DateTime.UtcNow - startTime).TotalMilliseconds;

            logger.LogInformation("Code executed successfully in {ExecutionTime}ms", executionTime);

            return Results.Ok(new
            {
                jobId = Guid.NewGuid().ToString(),
                status = "Completed",
                output = string.IsNullOrEmpty(finalOutput) ? "Code executed successfully (no output)" : finalOutput,
                error = "",
                executionTimeMs = executionTime
            });
        }
        catch (TimeoutException)
        {
            return Results.Json(new
            {
                jobId = Guid.NewGuid().ToString(),
                status = "Failed",
                output = "",
                error = "Execution timeout (10 seconds)",
                executionTimeMs = 10000
            }, statusCode: 408);
        }
        catch (Exception ex)
        {
            var executionTime = (int)(DateTime.UtcNow - startTime).TotalMilliseconds;
            
            return Results.Json(new
            {
                jobId = Guid.NewGuid().ToString(),
                status = "Failed",
                output = "",
                error = $"Runtime Error: {ex.Message}",
                executionTimeMs = executionTime
            }, statusCode: 400);
        }
        finally
        {
            Console.SetOut(originalOut);
            Console.SetError(originalError);
        }
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Execution service error: {Message}", ex.Message);
        
        return Results.Json(new
        {
            jobId = Guid.NewGuid().ToString(),
            status = "Failed",
            output = "",
            error = $"Service Error: {ex.Message}",
            executionTimeMs = 0
        }, statusCode: 500);
    }
});

app.MapPlatformHealthChecks("/health");

app.Run();

public class ExecuteRequest
{
    public string Code { get; set; } = "";
}