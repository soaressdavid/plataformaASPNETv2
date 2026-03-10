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
        logger.LogDebug("Original code: {Code}", request.Code);

        var startTime = DateTime.UtcNow;
        
        // Preparar código para compilação
        var codeToCompile = request.Code.Trim();
        
        // Verificar se o código já tem uma estrutura completa (classe Program com Main)
        bool hasMainMethod = codeToCompile.Contains("static void Main") || 
                           codeToCompile.Contains("static async Task Main") ||
                           codeToCompile.Contains("static int Main") ||
                           codeToCompile.Contains("static async Task<int> Main");
        bool hasClassProgram = codeToCompile.Contains("class Program");
        bool hasUsings = codeToCompile.Contains("using System");
        
        string fullCode;
        
        if (hasMainMethod && hasClassProgram)
        {
            // Código já tem estrutura completa
            if (hasUsings)
            {
                // Já tem usings, usar como está
                fullCode = codeToCompile;
            }
            else
            {
                // Adicionar usings necessários
                fullCode = $@"using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

{codeToCompile}";
            }
        }
        else if (hasMainMethod && !hasClassProgram)
        {
            // Tem Main mas não tem classe, envolver em classe
            fullCode = $@"using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Program
{{
{codeToCompile}
}}";
        }
        else
        {
            // Código simples, envolver em Main method
            fullCode = $@"using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Program
{{
    public static void Main()
    {{
        {codeToCompile}
    }}
}}";
        }
        
        logger.LogDebug("Generated code for compilation: {FullCode}", fullCode);

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
            var errors = result.Diagnostics
                .Where(d => d.IsWarningAsError || d.Severity == DiagnosticSeverity.Error)
                .Select(d => {
                    var lineSpan = d.Location.GetLineSpan();
                    var line = lineSpan.StartLinePosition.Line + 1;
                    var message = d.GetMessage();
                    
                    // Ajustar número da linha para o código original (subtraindo as linhas de using e classe)
                    var adjustedLine = Math.Max(1, line - 7);
                    
                    return $"Linha {adjustedLine}: {message}";
                })
                .ToList();

            var errorMessage = string.Join("\n", errors);
            
            logger.LogWarning("Compilation failed: {Errors}", errorMessage);

            return Results.Json(new
            {
                jobId = Guid.NewGuid().ToString(),
                status = "Failed",
                output = "",
                error = $"Erro de Compilação:\n{errorMessage}",
                executionTimeMs = (int)(DateTime.UtcNow - startTime).TotalMilliseconds
            }, statusCode: 400);
        }

        // Executar código compilado
        ms.Seek(0, SeekOrigin.Begin);
        var assembly = Assembly.Load(ms.ToArray());
        
        // Procurar por qualquer classe que tenha um método Main
        var mainMethod = assembly.GetTypes()
            .SelectMany(t => t.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
            .FirstOrDefault(m => m.Name == "Main");

        if (mainMethod == null)
        {
            return Results.Json(new
            {
                jobId = Guid.NewGuid().ToString(),
                status = "Failed",
                output = "",
                error = "Método Main não encontrado. Certifique-se de que seu código tem um ponto de entrada válido.",
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
            var task = Task.Run(() => {
                if (mainMethod.GetParameters().Length > 0)
                {
                    // Main method with string[] args parameter
                    mainMethod.Invoke(null, new object[] { new string[0] });
                }
                else
                {
                    // Main method without parameters
                    mainMethod.Invoke(null, null);
                }
            });
            await task.WaitAsync(TimeSpan.FromSeconds(10));
            
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