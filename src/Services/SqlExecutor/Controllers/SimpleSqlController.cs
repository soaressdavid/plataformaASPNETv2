using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace SqlExecutor.Service.Controllers;

[ApiController]
[Route("api/sql")]
public class SimpleSqlController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<SimpleSqlController> _logger;

    public SimpleSqlController(IConfiguration configuration, ILogger<SimpleSqlController> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    [HttpPost("execute")]
    public async Task<IActionResult> ExecuteQuery([FromBody] ExecuteQueryRequest request)
    {
        var startTime = DateTime.UtcNow;
        
        try
        {
            // Validação básica
            if (string.IsNullOrWhiteSpace(request.Query))
            {
                return BadRequest(new { success = false, error = "Query não pode ser vazia" });
            }

            // Proibir apenas operações perigosas em tabelas do sistema
            var query = request.Query.ToLower();
            
            // Lista de operações permitidas para fins educacionais
            var allowedOperations = new[] { "select", "insert", "update", "delete", "create", "alter", "drop" };
            
            // Proibir operações em tabelas do sistema
            var systemTables = new[] { "__efmigrationshistory", "sysdiagrams", "sys.", "information_schema" };
            
            foreach (var sysTable in systemTables)
            {
                if (query.Contains(sysTable))
                {
                    return BadRequest(new 
                    { 
                        success = false, 
                        error = $"Operações em tabelas do sistema não são permitidas." 
                    });
                }
            }

            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            
            using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();

            using var command = new SqlCommand(request.Query, connection);
            command.CommandTimeout = request.TimeoutSeconds ?? 5;

            // Detectar se é uma query que retorna resultados ou não
            var isSelectQuery = query.TrimStart().StartsWith("select") || 
                               query.TrimStart().StartsWith("with") ||
                               query.Contains("returning");

            if (isSelectQuery)
            {
                // Query que retorna resultados (SELECT)
                using var reader = await command.ExecuteReaderAsync();
                
                var columns = new List<string>();
                var rows = new List<List<object>>();

                // Ler colunas
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    columns.Add(reader.GetName(i));
                }

                // Ler linhas (máximo 100)
                int rowCount = 0;
                while (await reader.ReadAsync() && rowCount < 100)
                {
                    var row = new List<object>();
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        row.Add(reader.IsDBNull(i) ? null : reader.GetValue(i));
                    }
                    rows.Add(row);
                    rowCount++;
                }

                var executionTime = (DateTime.UtcNow - startTime).TotalMilliseconds;

                return Ok(new
                {
                    success = true,
                    columns,
                    rows,
                    rowCount = rows.Count,
                    executionTime = (int)executionTime,
                    message = rows.Count == 0 ? "Consulta executada com sucesso (0 resultados)" : null
                });
            }
            else
            {
                // Comando que não retorna resultados (INSERT, UPDATE, DELETE, CREATE, etc.)
                var rowsAffected = await command.ExecuteNonQueryAsync();
                var executionTime = (DateTime.UtcNow - startTime).TotalMilliseconds;

                return Ok(new
                {
                    success = true,
                    rowsAffected,
                    executionTime = (int)executionTime,
                    message = $"Comando executado com sucesso. {rowsAffected} linha(s) afetada(s)."
                });
            }
        }
        catch (SqlException ex)
        {
            _logger.LogError(ex, "Erro SQL ao executar query");
            var executionTime = (DateTime.UtcNow - startTime).TotalMilliseconds;
            
            return BadRequest(new
            {
                success = false,
                error = ex.Message,
                executionTime = (int)executionTime
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao executar query");
            var executionTime = (DateTime.UtcNow - startTime).TotalMilliseconds;
            
            return StatusCode(500, new
            {
                success = false,
                error = "Erro interno ao executar consulta",
                executionTime = (int)executionTime
            });
        }
    }
}

public class ExecuteQueryRequest
{
    public string Query { get; set; } = "";
    public string? UserId { get; set; }
    public int? TimeoutSeconds { get; set; }
}
