using Shared.Entities;
using Shared.Models;
using System.Text.Json;

namespace Shared.Data;

/// <summary>
/// Level 4 Content Seeder - Part 4 (Lessons 9-20)
/// </summary>
public partial class Level4ContentSeeder
{
    private Lesson CreateLesson9()
    {
        var content = new LessonContent
        {
            Objectives = new List<string>
            {
                "Aprender a executar SQL bruto no EF Core",
                "Dominar FromSqlRaw e FromSqlInterpolated",
                "Entender quando usar SQL bruto",
                "Executar stored procedures"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "SQL Bruto no EF Core",
                    Content = "Embora o EF Core traduza LINQ para SQL automaticamente, às vezes você precisa executar SQL bruto diretamente. Isso é útil para queries complexas que o LINQ não suporta bem, para aproveitar recursos específicos do banco de dados, ou para otimizar queries críticas de performance. O EF Core oferece FromSqlRaw() e FromSqlInterpolated() para executar SELECT queries que retornam entidades. Para comandos que não retornam dados (INSERT, UPDATE, DELETE), use ExecuteSqlRaw() ou ExecuteSqlInterpolated(). A diferença entre Raw e Interpolated é a forma de passar parâmetros: Raw usa placeholders e array de parâmetros, Interpolated usa string interpolation do C# e cria parâmetros automaticamente, protegendo contra SQL injection. Sempre use parametrização - nunca concatene valores diretamente na string SQL. SQL bruto deve ser usado com moderação, apenas quando LINQ não é suficiente.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Stored Procedures",
                    Content = "Stored procedures são blocos de código SQL armazenados no banco de dados. Eles podem aceitar parâmetros, executar lógica complexa e retornar resultados. Stored procedures oferecem vantagens: melhor performance (pré-compilados), segurança (controle de acesso granular), e encapsulamento de lógica de negócio no banco. No EF Core, você executa stored procedures usando FromSqlRaw() para procedures que retornam entidades, ou ExecuteSqlRaw() para procedures que não retornam dados. Você pode passar parâmetros de entrada e saída. Para procedures que retornam múltiplos result sets ou dados não mapeados para entidades, use ADO.NET diretamente através de context.Database.GetDbConnection(). Stored procedures são comuns em sistemas legados e em cenários onde DBAs controlam o schema. Em aplicações modernas, prefira manter lógica no código C# para melhor testabilidade e manutenibilidade.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Quando Usar SQL Bruto",
                    Content = "Use SQL bruto apenas quando necessário. Situações apropriadas incluem: queries muito complexas com múltiplos JOINs e subqueries que são difíceis de expressar em LINQ, uso de recursos específicos do banco como window functions ou CTEs recursivas, otimização de queries críticas de performance onde você precisa controle total sobre o SQL gerado, e integração com stored procedures existentes. Vantagens do SQL bruto: controle total, acesso a recursos avançados do banco, e às vezes melhor performance. Desvantagens: perde type-safety, dificulta mudanças de banco de dados, mais difícil de testar, e pode introduzir vulnerabilidades de SQL injection se não parametrizado corretamente. Como regra geral, comece com LINQ e só use SQL bruto quando LINQ não atende ou quando performance é crítica e você mediu que SQL customizado é significativamente mais rápido.",
                    Order = 3
                }
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Executando SQL Bruto",
                    Code = @"// FromSqlRaw com parâmetros (protegido contra SQL injection)
var produtos = await context.Produtos
    .FromSqlRaw(""SELECT * FROM Produtos WHERE CategoriaId = {0} AND Preco > {1}"", 
                categoriaId, precoMinimo)
    .ToListAsync();

// FromSqlInterpolated (recomendado - mais seguro e legível)
var produtosInterpolated = await context.Produtos
    .FromSqlInterpolated($""SELECT * FROM Produtos WHERE CategoriaId = {categoriaId} AND Preco > {precoMinimo}"")
    .ToListAsync();

// Você pode combinar com LINQ
var produtosFiltrados = await context.Produtos
    .FromSqlInterpolated($""SELECT * FROM Produtos WHERE Ativo = 1"")
    .Where(p => p.Estoque > 0)
    .OrderBy(p => p.Nome)
    .ToListAsync();",
                    Language = "csharp",
                    Explanation = "FromSqlInterpolated é mais seguro e legível que FromSqlRaw. Você pode combinar SQL bruto com operadores LINQ para filtrar e ordenar os resultados.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Executando Comandos SQL",
                    Code = @"// ExecuteSqlRaw para comandos que não retornam dados
int linhasAfetadas = await context.Database
    .ExecuteSqlRawAsync(""UPDATE Produtos SET Preco = Preco * 1.1 WHERE CategoriaId = {0}"", 
                        categoriaId);

// ExecuteSqlInterpolated (recomendado)
await context.Database
    .ExecuteSqlInterpolatedAsync($""DELETE FROM Produtos WHERE Estoque = 0 AND DataCriacao < {dataLimite}"");

// Executar múltiplos comandos em uma transação
using var transaction = await context.Database.BeginTransactionAsync();
try
{
    await context.Database.ExecuteSqlInterpolatedAsync($""UPDATE Produtos SET Ativo = 0 WHERE Estoque = 0"");
    await context.Database.ExecuteSqlInterpolatedAsync($""INSERT INTO Log (Mensagem) VALUES ('Produtos desativados')"");
    await transaction.CommitAsync();
}
catch
{
    await transaction.RollbackAsync();
    throw;
}",
                    Language = "csharp",
                    Explanation = "ExecuteSqlInterpolated executa comandos SQL que não retornam entidades. Retorna o número de linhas afetadas. Use transações para garantir atomicidade.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Chamando Stored Procedures",
                    Code = @"// Stored Procedure que retorna entidades
var produtos = await context.Produtos
    .FromSqlInterpolated($""EXEC BuscarProdutosPorCategoria {categoriaId}"")
    .ToListAsync();

// Stored Procedure com múltiplos parâmetros
var produtosComFiltro = await context.Produtos
    .FromSqlInterpolated($""EXEC BuscarProdutos @CategoriaId={categoriaId}, @PrecoMin={precoMin}, @PrecoMax={precoMax}"")
    .ToListAsync();

// Stored Procedure que não retorna dados
await context.Database
    .ExecuteSqlInterpolatedAsync($""EXEC AtualizarEstoque @ProdutoId={produtoId}, @Quantidade={quantidade}"");

// Stored Procedure com parâmetro de saída (requer ADO.NET)
var connection = context.Database.GetDbConnection();
using var command = connection.CreateCommand();
command.CommandText = ""EXEC CalcularTotal @PedidoId, @Total OUTPUT"";
command.Parameters.Add(new SqlParameter(""@PedidoId"", pedidoId));
var totalParam = new SqlParameter(""@Total"", SqlDbType.Decimal) { Direction = ParameterDirection.Output };
command.Parameters.Add(totalParam);
await connection.OpenAsync();
await command.ExecuteNonQueryAsync();
decimal total = (decimal)totalParam.Value;",
                    Language = "csharp",
                    Explanation = "Stored procedures são chamadas com EXEC. Para parâmetros de saída, use ADO.NET diretamente através de GetDbConnection().",
                    IsRunnable = true
                }
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Executar Query SQL Bruta",
                    Description = "Escreva uma query SQL bruta que busca produtos de uma categoria específica com estoque maior que zero, ordenados por preço. Use FromSqlInterpolated.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"public async Task<List<Produto>> BuscarProdutosSQL(int categoriaId)
{
    var produtos = await context.Produtos
        .FromSqlInterpolated($""..."")
        .ToListAsync();
    
    return produtos;
}",
                    Hints = new List<string> 
                    { 
                        "Use SELECT * FROM Produtos WHERE...",
                        "Adicione condições para CategoriaId e Estoque",
                        "Use ORDER BY Preco para ordenar" 
                    }
                },
                new Exercise
                {
                    Title = "Atualização em Lote com SQL",
                    Description = "Implemente um método que aumenta o preço de todos os produtos de uma categoria em 10% usando ExecuteSqlInterpolated. Retorne o número de produtos atualizados.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"public async Task<int> AumentarPrecoCategoria(int categoriaId, decimal percentual)
{
    int linhasAfetadas = await context.Database
        .ExecuteSqlInterpolatedAsync($""..."");
    
    return linhasAfetadas;
}",
                    Hints = new List<string> 
                    { 
                        "Use UPDATE Produtos SET Preco = Preco * (1 + percentual/100)",
                        "Adicione WHERE CategoriaId = {categoriaId}",
                        "ExecuteSqlInterpolated retorna número de linhas afetadas" 
                    }
                },
                new Exercise
                {
                    Title = "Criar e Chamar Stored Procedure",
                    Description = "Crie uma stored procedure no SQL Server que busca os N produtos mais vendidos. Depois, chame essa procedure do EF Core passando N como parâmetro.",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = @"// SQL para criar a procedure:
// CREATE PROCEDURE BuscarProdutosMaisVendidos
//     @Quantidade INT
// AS
// BEGIN
//     SELECT TOP (@Quantidade) p.*
//     FROM Produtos p
//     INNER JOIN ItensPedido ip ON p.Id = ip.ProdutoId
//     GROUP BY p.Id, p.Nome, p.Preco, p.Estoque
//     ORDER BY SUM(ip.Quantidade) DESC
// END

// Código C# para chamar:
public async Task<List<Produto>> ObterMaisVendidos(int quantidade)
{
    // Implementar chamada à procedure
}",
                    Hints = new List<string> 
                    { 
                        "Execute o SQL CREATE PROCEDURE no banco primeiro",
                        "Use FromSqlInterpolated com EXEC BuscarProdutosMaisVendidos",
                        "Passe o parâmetro @Quantidade={quantidade}" 
                    }
                }
            },
            Summary = "Nesta aula você aprendeu a executar SQL bruto no EF Core usando FromSqlInterpolated e ExecuteSqlInterpolated, como chamar stored procedures, e quando usar SQL bruto em vez de LINQ. Use SQL bruto com moderação e sempre parametrize para evitar SQL injection."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0005-000000000009"),
            CourseId = _courseId,
            Title = "SQL Bruto e Stored Procedures",
            Duration = "50 min",
            Difficulty = "Intermediário",
            EstimatedMinutes = 50,
            Prerequisites = JsonSerializer.Serialize(new[] { "10000000-0000-0000-0005-000000000008" }),
            OrderIndex = 9,
            Content = "",
            StructuredContent = JsonSerializer.Serialize(content),
            Version = 1,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }

    private Lesson CreateLesson10()
    {
        var content = new LessonContent
        {
            Objectives = new List<string>
            {
                "Compreender concorrência otimista e pessimista",
                "Aprender a usar tokens de concorrência",
                "Dominar tratamento de conflitos de concorrência",
                "Implementar RowVersion para controle de concorrência"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "Concorrência em Bancos de Dados",
                    Content = "Concorrência ocorre quando múltiplos usuários ou processos tentam modificar os mesmos dados simultaneamente. Sem controle de concorrência, podem ocorrer problemas como lost updates (uma atualização sobrescreve outra) e dirty reads (ler dados não commitados). Existem duas abordagens: concorrência pessimista e otimista. Concorrência pessimista usa locks no banco de dados para prevenir acesso simultâneo - quando um usuário edita um registro, outros são bloqueados até que ele termine. Isso garante consistência mas reduz concorrência e pode causar deadlocks. Concorrência otimista assume que conflitos são raros e permite acesso simultâneo, mas detecta conflitos no momento do save e permite que a aplicação decida como resolvê-los. O EF Core suporta principalmente concorrência otimista através de tokens de concorrência. É a abordagem recomendada para aplicações web onde locks de longa duração não são práticos.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Tokens de Concorrência",
                    Content = "Um token de concorrência é uma propriedade que o EF Core verifica antes de atualizar ou deletar um registro. O token mais comum é RowVersion (timestamp no SQL Server), um valor que muda automaticamente a cada atualização. Quando você carrega uma entidade, o EF Core armazena o valor atual do RowVersion. Ao salvar, ele gera UPDATE ... WHERE Id = @id AND RowVersion = @originalRowVersion. Se outro usuário modificou o registro entre o load e o save, o RowVersion será diferente e o UPDATE afetará 0 linhas, causando DbUpdateConcurrencyException. Você pode então recarregar os dados atuais, mostrar ao usuário as mudanças conflitantes, e deixá-lo decidir como resolver. Alternativamente, você pode usar qualquer propriedade como token de concorrência com IsConcurrencyToken(). Por exemplo, usar LastModified permite detectar se o registro foi modificado desde que foi carregado.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Tratando Conflitos de Concorrência",
                    Content = "Quando ocorre DbUpdateConcurrencyException, você tem várias opções: Client Wins (sobrescrever com valores do cliente), Database Wins (descartar mudanças do cliente e usar valores do banco), ou Merge (combinar mudanças de forma inteligente). Para Client Wins, recarregue a entidade, copie os valores do cliente e salve novamente. Para Database Wins, simplesmente descarte as mudanças do cliente. Para Merge, compare propriedade por propriedade e decida qual valor manter. Em aplicações web, é comum mostrar ao usuário os valores conflitantes e deixá-lo escolher. Use entry.OriginalValues para ver valores quando foi carregado, entry.CurrentValues para valores que o cliente quer salvar, e entry.GetDatabaseValues() para valores atuais no banco. Implemente uma estratégia de retry com backoff exponencial para conflitos temporários. Em sistemas críticos, considere usar filas de mensagens para serializar operações conflitantes.",
                    Order = 3
                }
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Configurando RowVersion",
                    Code = @"public class Produto
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public decimal Preco { get; set; }
    
    // Token de concorrência
    [Timestamp]
    public byte[] RowVersion { get; set; }
}

// Ou usando Fluent API
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Produto>(entity =>
    {
        entity.Property(p => p.RowVersion)
            .IsRowVersion();
    });
}

// SQL Server criará coluna ROWVERSION que atualiza automaticamente",
                    Language = "csharp",
                    Explanation = "RowVersion é um token de concorrência que muda automaticamente a cada UPDATE. O atributo [Timestamp] ou IsRowVersion() configura isso. SQL Server gerencia o valor automaticamente.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Tratando Conflitos de Concorrência",
                    Code = @"try
{
    // Usuário 1 carrega produto
    var produto = await context.Produtos.FindAsync(1);
    produto.Preco = 100;
    
    // Usuário 2 modifica o mesmo produto (em outro contexto)
    // produto.Preco = 150;
    
    // Usuário 1 tenta salvar
    await context.SaveChangesAsync();
}
catch (DbUpdateConcurrencyException ex)
{
    var entry = ex.Entries.Single();
    var databaseValues = await entry.GetDatabaseValuesAsync();
    
    if (databaseValues == null)
    {
        // Registro foi deletado
        Console.WriteLine(""O produto foi deletado por outro usuário."");
    }
    else
    {
        var databaseEntity = (Produto)databaseValues.ToObject();
        
        Console.WriteLine($""Conflito detectado!"");
        Console.WriteLine($""Seu valor: {((Produto)entry.Entity).Preco}"");
        Console.WriteLine($""Valor atual no banco: {databaseEntity.Preco}"");
        
        // Opção 1: Client Wins - forçar valores do cliente
        entry.OriginalValues.SetValues(databaseValues);
        await context.SaveChangesAsync();
        
        // Opção 2: Database Wins - descartar mudanças do cliente
        // entry.CurrentValues.SetValues(databaseValues);
        
        // Opção 3: Merge - combinar valores
        // Decidir propriedade por propriedade qual valor manter
    }
}",
                    Language = "csharp",
                    Explanation = "DbUpdateConcurrencyException é lançada quando há conflito. GetDatabaseValuesAsync() obtém valores atuais do banco. Você pode então decidir como resolver o conflito.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Usando Propriedade como Token",
                    Code = @"public class Pedido
{
    public int Id { get; set; }
    public decimal Total { get; set; }
    
    // Usar LastModified como token de concorrência
    public DateTime LastModified { get; set; }
}

protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Pedido>(entity =>
    {
        entity.Property(p => p.LastModified)
            .IsConcurrencyToken()
            .ValueGeneratedOnAddOrUpdate();
    });
}

// Ao salvar, EF Core verifica se LastModified mudou
// UPDATE Pedidos SET Total = @total, LastModified = GETDATE()
// WHERE Id = @id AND LastModified = @originalLastModified",
                    Language = "csharp",
                    Explanation = "Qualquer propriedade pode ser usada como token de concorrência com IsConcurrencyToken(). LastModified é comum em sistemas que rastreiam quando registros foram modificados.",
                    IsRunnable = true
                }
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Adicionar RowVersion",
                    Description = "Adicione uma propriedade RowVersion à entidade Produto e configure-a como token de concorrência. Crie uma migration e aplique ao banco.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"public class Produto
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public decimal Preco { get; set; }
    
    // Adicionar RowVersion
}

protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    // Configurar RowVersion
}",
                    Hints = new List<string> 
                    { 
                        "Use byte[] para o tipo de RowVersion",
                        "Adicione [Timestamp] ou use IsRowVersion()",
                        "Crie migration com dotnet ef migrations add" 
                    }
                },
                new Exercise
                {
                    Title = "Simular e Tratar Conflito",
                    Description = "Crie um teste que simula dois usuários modificando o mesmo produto simultaneamente. Implemente tratamento de DbUpdateConcurrencyException com estratégia Client Wins.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"using Microsoft.EntityFrameworkCore;

// Usuário 1
using var context1 = new AppDbContext(options);
var produto1 = await context1.Produtos.FindAsync(1);
produto1.Preco = 100;

// Usuário 2
using var context2 = new AppDbContext(options);
var produto2 = await context2.Produtos.FindAsync(1);
produto2.Preco = 150;

// Salvar em ordem e tratar conflito
try
{
    await context2.SaveChangesAsync(); // Sucesso
    await context1.SaveChangesAsync(); // Conflito!
}
catch (DbUpdateConcurrencyException ex)
{
    // Implementar Client Wins
}",
                    Hints = new List<string> 
                    { 
                        "Use GetDatabaseValuesAsync() para obter valores atuais",
                        "SetValues(databaseValues) em OriginalValues para Client Wins",
                        "Chame SaveChangesAsync() novamente após resolver" 
                    }
                },
                new Exercise
                {
                    Title = "Implementar Merge Strategy",
                    Description = "Implemente uma estratégia de merge que, em caso de conflito, mantém o maior preço entre o valor do cliente e o valor do banco, mas sempre mantém o nome do banco.",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = @"catch (DbUpdateConcurrencyException ex)
{
    var entry = ex.Entries.Single();
    var databaseValues = await entry.GetDatabaseValuesAsync();
    var databaseEntity = (Produto)databaseValues.ToObject();
    var clientEntity = (Produto)entry.Entity;
    
    // Implementar lógica de merge
    // Preço: manter o maior
    // Nome: sempre do banco
    
    entry.OriginalValues.SetValues(databaseValues);
    await context.SaveChangesAsync();
}",
                    Hints = new List<string> 
                    { 
                        "Compare clientEntity.Preco com databaseEntity.Preco",
                        "Use Math.Max() para escolher o maior preço",
                        "Atribua databaseEntity.Nome ao clientEntity.Nome" 
                    }
                }
            },
            Summary = "Nesta aula você aprendeu sobre controle de concorrência no EF Core, como usar tokens de concorrência (especialmente RowVersion), e como tratar conflitos de concorrência com diferentes estratégias. Controle de concorrência é essencial em aplicações multi-usuário."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0005-00000000000A"),
            CourseId = _courseId,
            Title = "Controle de Concorrência",
            Duration = "50 min",
            Difficulty = "Intermediário",
            EstimatedMinutes = 50,
            Prerequisites = JsonSerializer.Serialize(new[] { "10000000-0000-0000-0005-000000000009" }),
            OrderIndex = 10,
            Content = "",
            StructuredContent = JsonSerializer.Serialize(content),
            Version = 1,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }
}
