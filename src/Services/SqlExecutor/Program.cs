using System.Text.Json;
using Microsoft.Data.Sqlite;
using System.Data;

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

// Criar banco SQLite com dados de exemplo
var dbPath = "sqlpractice.db";
InitializeDatabase(dbPath);

// SqlExecutor REAL - Execução Real de SQL em SQLite
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

        logger.LogInformation("Executing real SQL: {SqlLength} characters", request.Query.Length);

        var sql = request.Query.Trim();
        
        // Validação básica de segurança
        var forbiddenKeywords = new[] { "DROP DATABASE", "DROP SCHEMA", "TRUNCATE", "DELETE FROM sqlite_master" };
        if (forbiddenKeywords.Any(keyword => sql.ToUpper().Contains(keyword)))
        {
            return Results.BadRequest(new { error = "Operação não permitida por segurança" });
        }

        using var connection = new SqliteConnection($"Data Source={dbPath}");
        await connection.OpenAsync();

        using var transaction = (SqliteTransaction)await connection.BeginTransactionAsync();
        
        try
        {
            using var command = connection.CreateCommand();
            command.Transaction = transaction;
            command.CommandText = sql;
            command.CommandTimeout = 30;

            var startTime = DateTime.UtcNow;
            
            if (sql.ToUpper().TrimStart().StartsWith("SELECT") || 
                sql.ToUpper().TrimStart().StartsWith("WITH") ||
                sql.ToUpper().TrimStart().StartsWith("SHOW") ||
                sql.ToUpper().TrimStart().StartsWith("DESCRIBE") ||
                sql.ToUpper().TrimStart().StartsWith("EXPLAIN"))
            {
                // Query que retorna dados
                using var dataReader = await command.ExecuteReaderAsync();
                var results = new List<Dictionary<string, object>>();
                
                while (await dataReader.ReadAsync())
                {
                    var row = new Dictionary<string, object>();
                    for (int i = 0; i < dataReader.FieldCount; i++)
                    {
                        var value = dataReader.GetValue(i);
                        row[dataReader.GetName(i)] = value == DBNull.Value ? null : value;
                    }
                    results.Add(row);
                }
                
                var executionTime = (int)(DateTime.UtcNow - startTime).TotalMilliseconds;
                
                // Commit da transação para SELECTs (caso tenham CTEs ou funções)
                await transaction.CommitAsync();
                
                logger.LogInformation("SQL query executed successfully, returned {Count} rows in {Time}ms", 
                    results.Count, executionTime);
                
                return Results.Ok(new
                {
                    success = true,
                    data = results,
                    rowsAffected = results.Count,
                    message = $"✅ Query executada com sucesso. {results.Count} linha(s) retornada(s).",
                    executionTimeMs = executionTime
                });
            }
            else
            {
                // Comando que modifica dados
                var rowsAffected = await command.ExecuteNonQueryAsync();
                var executionTime = (int)(DateTime.UtcNow - startTime).TotalMilliseconds;
                
                // Commit da transação
                await transaction.CommitAsync();
                
                logger.LogInformation("SQL command executed successfully, {Rows} rows affected in {Time}ms", 
                    rowsAffected, executionTime);
                
                string message;
                if (sql.ToUpper().TrimStart().StartsWith("INSERT"))
                    message = $"✅ INSERT executado com sucesso. {rowsAffected} linha(s) inserida(s).";
                else if (sql.ToUpper().TrimStart().StartsWith("UPDATE"))
                    message = $"✅ UPDATE executado com sucesso. {rowsAffected} linha(s) atualizada(s).";
                else if (sql.ToUpper().TrimStart().StartsWith("DELETE"))
                    message = $"✅ DELETE executado com sucesso. {rowsAffected} linha(s) removida(s).";
                else if (sql.ToUpper().TrimStart().StartsWith("CREATE"))
                    message = "✅ CREATE executado com sucesso. Objeto criado.";
                else if (sql.ToUpper().TrimStart().StartsWith("ALTER"))
                    message = "✅ ALTER executado com sucesso. Objeto alterado.";
                else if (sql.ToUpper().TrimStart().StartsWith("DROP"))
                    message = "✅ DROP executado com sucesso. Objeto removido.";
                else
                    message = $"✅ Comando SQL executado com sucesso. {rowsAffected} linha(s) afetada(s).";
                
                return Results.Ok(new
                {
                    success = true,
                    rowsAffected = rowsAffected,
                    message = message,
                    executionTimeMs = executionTime
                });
            }
        }
        catch (Exception)
        {
            // Rollback em caso de erro
            await transaction.RollbackAsync();
            throw;
        }
    }
    catch (SqliteException ex)
    {
        logger.LogError(ex, "SQL execution error: {Message}", ex.Message);
        return Results.Json(new
        {
            success = false,
            error = $"Erro SQL: {ex.Message}"
        }, statusCode: 400);
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "SQL executor service error: {Message}", ex.Message);
        return Results.Json(new
        {
            success = false,
            error = $"Erro no serviço: {ex.Message}"
        }, statusCode: 500);
    }
});

app.Run();

void InitializeDatabase(string dbPath)
{
    using var connection = new SqliteConnection($"Data Source={dbPath}");
    connection.Open();

    // Criar tabelas de exemplo
    var createTables = @"
        -- Tabela de usuários
        CREATE TABLE IF NOT EXISTS Users (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            Name TEXT NOT NULL,
            Email TEXT UNIQUE NOT NULL,
            Age INTEGER,
            CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP
        );

        -- Tabela de cursos
        CREATE TABLE IF NOT EXISTS Courses (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            Title TEXT NOT NULL,
            Description TEXT,
            Level INTEGER DEFAULT 0,
            CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP
        );

        -- Tabela de matrículas
        CREATE TABLE IF NOT EXISTS Enrollments (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            UserId INTEGER,
            CourseId INTEGER,
            EnrolledAt DATETIME DEFAULT CURRENT_TIMESTAMP,
            Progress INTEGER DEFAULT 0,
            FOREIGN KEY (UserId) REFERENCES Users(Id),
            FOREIGN KEY (CourseId) REFERENCES Courses(Id)
        );

        -- Inserir dados de exemplo se não existirem
        INSERT OR IGNORE INTO Users (Id, Name, Email, Age) VALUES 
        (1, 'Alice Johnson', 'alice@example.com', 25),
        (2, 'Bob Smith', 'bob@example.com', 30),
        (3, 'Carol Davis', 'carol@example.com', 28),
        (4, 'David Wilson', 'david@example.com', 35),
        (5, 'Emma Martinez', 'emma@example.com', 22);

        INSERT OR IGNORE INTO Courses (Id, Title, Description, Level) VALUES 
        (1, 'C# Básico', 'Introdução à programação C#', 0),
        (2, 'C# Intermediário', 'Conceitos avançados de C#', 1),
        (3, 'SQL Fundamentals', 'Aprenda SQL do zero', 0),
        (4, 'Web Development', 'Desenvolvimento web com ASP.NET', 2);

        INSERT OR IGNORE INTO Enrollments (UserId, CourseId, Progress) VALUES 
        (1, 1, 75),
        (1, 3, 50),
        (2, 1, 100),
        (2, 2, 25),
        (3, 3, 80),
        (4, 4, 60),
        (5, 1, 30);
    ";

    using var command = connection.CreateCommand();
    command.CommandText = createTables;
    command.ExecuteNonQuery();
    
    Console.WriteLine($"SQLite database initialized at: {Path.GetFullPath(dbPath)}");
}

public class SqlRequest
{
    public string Query { get; set; } = "";
}