using Shared.Logging;
using Shared.HealthChecks;
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

// Add health checks
builder.Services.AddPlatformHealthChecks(builder.Configuration, "ExecutionService");

var app = builder.Build();
app.UseCors("AllowAll");

// Execution Service MOCK - Versão Simples que Funciona
app.MapPost("/api/code/execute", async (HttpContext context, ILogger<Program> logger) =>
{
    try
    {
        logger.LogInformation("Received code execution request");
        
        using var reader = new StreamReader(context.Request.Body);
        var body = await reader.ReadToEndAsync();
        
        logger.LogInformation("Request body: {Body}", body);
        
        if (string.IsNullOrWhiteSpace(body))
        {
            return Results.BadRequest(new { error = "Request body is empty" });
        }
        
        var request = JsonSerializer.Deserialize<ExecuteRequest>(body, new JsonSerializerOptions 
        { 
            PropertyNameCaseInsensitive = true 
        });

        if (request == null)
        {
            return Results.BadRequest(new { error = "Invalid JSON format" });
        }

        if (string.IsNullOrWhiteSpace(request.Code))
        {
            return Results.BadRequest(new { error = "Code is required" });
        }

        logger.LogInformation("Mock execution: {CodeLength} characters", request.Code.Length);

        // Simular tempo de execução
        await Task.Delay(300);
        
        var output = "✅ Código executado com sucesso (MOCK)!\n\n";
        
        // Simular diferentes outputs baseado no código
        if (request.Code.Contains("Hello") || request.Code.Contains("hello"))
        {
            output += "Hello World!";
        }
        else if (request.Code.Contains("for") && request.Code.Contains("i"))
        {
            output += "Loop executado:\n0\n1\n2\n3\n4";
        }
        else if (request.Code.Contains("Console.WriteLine"))
        {
            output += "Output do Console.WriteLine executado";
        }
        else if (request.Code.Contains("int") || request.Code.Contains("string"))
        {
            output += "Variáveis declaradas e inicializadas";
        }
        else if (request.Code.Contains("class") || request.Code.Contains("public"))
        {
            output += "Classe compilada e executada com sucesso";
        }
        else
        {
            output += "Código C# executado com sucesso";
        }

        var response = new
        {
            jobId = Guid.NewGuid().ToString(),
            status = "Completed",
            output = output,
            error = "",
            executionTimeMs = 300
        };

        logger.LogInformation("Mock execution completed successfully");

        return Results.Ok(response);
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Mock execution failed: {Message}", ex.Message);
        
        return Results.Json(new
        {
            jobId = Guid.NewGuid().ToString(),
            status = "Failed",
            output = "",
            executionTimeMs = 0,
            error = $"Erro na execução: {ex.Message}"
        }, statusCode: 500);
    }
});

app.MapPlatformHealthChecks("/health");

app.Run();

public class ExecuteRequest
{
    public string Code { get; set; } = "";
}
