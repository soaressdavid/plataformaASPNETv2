using SqlExecutor.Service.Services;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Register services (SEM DOCKER)
builder.Services.AddSingleton<InProcessSqlExecutor>();
builder.Services.AddSingleton<SqlQueryValidator>();

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
app.UseAuthorization();

app.MapControllers();
app.MapHealthChecks("/health");

// Endpoint direto para execução SQL (sem Docker)
app.MapPost("/api/sql/execute", async (HttpContext context, InProcessSqlExecutor executor, ILogger<Program> logger) =>
{
    try
    {
        using var reader = new StreamReader(context.Request.Body);
        var body = await reader.ReadToEndAsync();
        
        var request = JsonSerializer.Deserialize<SqlExecuteRequest>(body, new JsonSerializerOptions 
        { 
            PropertyNameCaseInsensitive = true 
        });

        if (request == null || string.IsNullOrWhiteSpace(request.Sql))
        {
            return Results.BadRequest(new { error = "SQL is required" });
        }

        logger.LogInformation("Executing SQL: {SqlLength} characters", request.Sql.Length);

        var result = await executor.ExecuteAsync(request.Sql);

        return Results.Ok(result);
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "SQL execution failed");
        return Results.Json(new
        {
            success = false,
            error = $"Internal error: {ex.Message}"
        }, statusCode: 500);
    }
});

app.Run();

// Executor SQL sem Docker (in-process com transações)
public class InProcessSqlExecutor
{
    private readonly ILogger<InProcessSqlExecutor> _logger;
    private readonly SqlQueryValidator _validator;
    private readonly string _connectionString;

    public InProcessSqlExecutor(ILogger<InProcessSqlExecutor> logger, SqlQueryValidator validator, IConfiguration configuration)
    {
        _logger = logger;
        _validator = validator;
        
        // Usar connection string do appsettings
        _connectionString = configuration.GetConnectionString("DefaultConnection") 
            ?? "Server=(localdb)\\MSSQLLocalDB;Database=AspNetLearningPlatform;Integrated Security=true;TrustServerCertificate=True;";
    }

    public async Task<SqlExecutionResult> ExecuteAsync(string sql)
    {
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        
        try
        {
            // Validar SQL
            var validation = _validator.ValidateQuery(sql);
            if (!validation.IsValid)
            {
                return new SqlExecutionResult
                {
                    Success = false,
                    Error = $"SQL inválido: {string.Join(", ", validation.Errors)}",
                    ExecutionTimeMs = (int)stopwatch.ElapsedMilliseconds
                };
            }

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            
            using var transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted);
            
            try
            {
                using var command = new SqlCommand(sql, connection, transaction);
                command.CommandTimeout = 30; // 30 segundos timeout
                
                var result = new SqlExecutionResult { Success = true };
                
                // Verificar se é query (SELECT) ou comando (INSERT/UPDATE/DELETE/CREATE)
                var trimmedSql = sql.Trim().ToUpperInvariant();
                
                if (trimmedSql.StartsWith("SELECT") || trimmedSql.StartsWith("WITH"))
                {
                    // Query - retornar dados
                    using var reader = await command.ExecuteReaderAsync();
                    var data = new List<Dictionary<string, object>>();
                    
                    while (await reader.ReadAsync())
                    {
                        var row = new Dictionary<string, object>();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            var value = reader.GetValue(i);
                            row[reader.GetName(i)] = value == DBNull.Value ? null : value;
                        }
                        data.Add(row);
                        
                        // Limitar a 1000 linhas para evitar sobrecarga
                        if (data.Count >= 1000)
                        {
                            break;
                        }
                    }
                    
                    result.Data = data;
                    result.RowsAffected = data.Count;
                    result.Message = $"Query executada com sucesso. {data.Count} linha(s) retornada(s).";
                }
                else
                {
                    // Comando - executar e retornar linhas afetadas
                    var rowsAffected = await command.ExecuteNonQueryAsync();
                    result.RowsAffected = rowsAffected;
                    result.Message = $"Comando executado com sucesso. {rowsAffected} linha(s) afetada(s).";
                }
                
                // SEMPRE fazer rollback para não persistir dados (ambiente de teste)
                transaction.Rollback();
                
                stopwatch.Stop();
                result.ExecutionTimeMs = (int)stopwatch.ElapsedMilliseconds;
                
                return result;
            }
            catch (SqlException ex)
            {
                transaction.Rollback();
                
                return new SqlExecutionResult
                {
                    Success = false,
                    Error = $"Erro SQL: {ex.Message}",
                    ExecutionTimeMs = (int)stopwatch.ElapsedMilliseconds
                };
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "SQL execution failed");
            
            return new SqlExecutionResult
            {
                Success = false,
                Error = $"Erro interno: {ex.Message}",
                ExecutionTimeMs = (int)stopwatch.ElapsedMilliseconds
            };
        }
    }
}

public class SqlExecuteRequest
{
    public string Sql { get; set; } = "";
}

public class SqlExecutionResult
{
    public bool Success { get; set; }
    public string Error { get; set; } = "";
    public string Message { get; set; } = "";
    public List<Dictionary<string, object>>? Data { get; set; }
    public int RowsAffected { get; set; }
    public int ExecutionTimeMs { get; set; }
}
