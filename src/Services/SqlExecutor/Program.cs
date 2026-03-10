using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Health checks
builder.Services.AddHealthChecks();

// CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

app.UseCors();
app.UseHttpsRedirection();

app.MapHealthChecks("/health");

// SqlExecutor MOCK - Versão Simples que Funciona
app.MapPost("/api/sql/execute", async (HttpContext context, ILogger<Program> logger) =>
{
    try
    {
        using var reader = new StreamReader(context.Request.Body);
        var body = await reader.ReadToEndAsync();
        
        var request = JsonSerializer.Deserialize<SqlRequest>(body, new JsonSerializerOptions 
        { 
            PropertyNameCaseInsensitive = true 
        });

        if (request == null || string.IsNullOrWhiteSpace(request.Query))
        {
            return Results.BadRequest(new { error = "SQL é obrigatório" });
        }

        logger.LogInformation("Mock SQL execution: {SqlLength} characters", request.Query.Length);

        // Simular tempo de execução
        await Task.Delay(200);
        
        var sql = request.Query.ToLower().Trim();
        
        if (sql.StartsWith("select"))
        {
            // Simular dados de SELECT
            var mockData = new List<Dictionary<string, object>>();
            
            if (sql.Contains("users"))
            {
                mockData.Add(new Dictionary<string, object> { {"Id", 1}, {"Name", "Alice Johnson"}, {"Email", "alice@example.com"} });
                mockData.Add(new Dictionary<string, object> { {"Id", 2}, {"Name", "Bob Smith"}, {"Email", "bob@example.com"} });
                mockData.Add(new Dictionary<string, object> { {"Id", 3}, {"Name", "Carol Davis"}, {"Email", "carol@example.com"} });
            }
            else if (sql.Contains("courses"))
            {
                mockData.Add(new Dictionary<string, object> { {"Id", 1}, {"Title", "C# Básico"}, {"Level", 0} });
                mockData.Add(new Dictionary<string, object> { {"Id", 2}, {"Title", "C# Intermediário"}, {"Level", 1} });
            }
            else if (sql.Contains("count"))
            {
                mockData.Add(new Dictionary<string, object> { {"COUNT(*)", 6} });
            }
            else
            {
                mockData.Add(new Dictionary<string, object> { {"Id", 1}, {"Data", "Exemplo"}, {"Status", "Ativo"} });
            }
            
            return Results.Ok(new
            {
                success = true,
                data = mockData,
                rowsAffected = mockData.Count,
                message = $"✅ Query executada com sucesso (MOCK). {mockData.Count} linha(s) retornada(s)."
            });
        }
        else if (sql.StartsWith("insert"))
        {
            return Results.Ok(new
            {
                success = true,
                rowsAffected = 1,
                message = "✅ INSERT executado com sucesso (MOCK). 1 linha(s) inserida(s)."
            });
        }
        else if (sql.StartsWith("update"))
        {
            return Results.Ok(new
            {
                success = true,
                rowsAffected = 2,
                message = "✅ UPDATE executado com sucesso (MOCK). 2 linha(s) atualizada(s)."
            });
        }
        else if (sql.StartsWith("delete"))
        {
            return Results.Ok(new
            {
                success = true,
                rowsAffected = 1,
                message = "✅ DELETE executado com sucesso (MOCK). 1 linha(s) removida(s)."
            });
        }
        else if (sql.StartsWith("create"))
        {
            return Results.Ok(new
            {
                success = true,
                rowsAffected = 0,
                message = "✅ CREATE TABLE executado com sucesso (MOCK). Tabela criada."
            });
        }
        else
        {
            return Results.Ok(new
            {
                success = true,
                rowsAffected = 1,
                message = "✅ Comando SQL executado com sucesso (MOCK)."
            });
        }
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Mock SQL execution failed");
        return Results.Json(new
        {
            success = false,
            error = $"Erro na execução SQL: {ex.Message}"
        }, statusCode: 500);
    }
});

app.Run();

public class SqlRequest
{
    public string Query { get; set; } = "";
}
