using Shared.Entities;
using Shared.Models;
using System.Text.Json;

namespace Shared.Data;

/// <summary>
/// Level 4 Content Seeder - Part 3 (Lessons 7-13)
/// </summary>
public partial class Level4ContentSeeder
{
    private Lesson CreateLesson7()
    {
        var content = new LessonContent
        {
            Objectives = new List<string>
            {
                "Aprender operações CRUD com EF Core",
                "Dominar inserção, atualização e remoção de dados",
                "Entender o Change Tracker",
                "Gerenciar transações e SaveChanges"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "Operações CRUD Básicas",
                    Content = "CRUD (Create, Read, Update, Delete) são as operações fundamentais em qualquer aplicação de banco de dados. No EF Core, Create usa Add() ou AddRange() para adicionar entidades ao contexto. Read usa Find(), FirstOrDefault() ou queries LINQ. Update modifica propriedades de entidades rastreadas. Delete usa Remove() ou RemoveRange(). Todas as mudanças são rastreadas pelo Change Tracker e persistidas quando você chama SaveChanges() ou SaveChangesAsync(). O EF Core detecta automaticamente quais entidades foram modificadas e gera os comandos SQL apropriados (INSERT, UPDATE, DELETE). É importante entender que Add/Remove apenas marcam entidades para inserção/remoção - nada acontece no banco até SaveChanges(). Isso permite agrupar múltiplas operações em uma única transação. O EF Core também suporta operações em lote para melhor performance ao inserir ou atualizar muitos registros.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Change Tracker e Estados de Entidade",
                    Content = "O Change Tracker é o componente do DbContext que monitora todas as entidades carregadas e detecta mudanças. Cada entidade rastreada tem um estado: Unchanged (sem mudanças), Added (será inserida), Modified (será atualizada), Deleted (será removida) ou Detached (não rastreada). Você pode verificar o estado com context.Entry(entidade).State. Quando você carrega uma entidade do banco, ela começa como Unchanged. Ao modificar propriedades, o estado muda para Modified automaticamente. Add() marca como Added, Remove() como Deleted. O Change Tracker usa snapshots para detectar mudanças - ele compara valores atuais com valores originais. Para entidades não rastreadas (por exemplo, de APIs), você pode anexá-las com Attach() ou Update(). Attach() marca como Unchanged, Update() como Modified. Entender estados é crucial para resolver problemas de concorrência e otimizar operações.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Transações e SaveChanges",
                    Content = "SaveChanges() agrupa todas as mudanças pendentes em uma única transação de banco de dados. Se qualquer operação falhar, toda a transação é revertida (rollback), mantendo consistência. Você pode controlar transações manualmente com BeginTransaction() para operações mais complexas que envolvem múltiplos SaveChanges(). Por exemplo, ao processar um pedido, você pode querer atualizar estoque, criar pedido e registrar pagamento em uma transação. Use try-catch para capturar DbUpdateException e tratar erros de concorrência ou violações de constraint. SaveChanges() retorna o número de registros afetados. Para melhor performance em inserções em massa, use AddRange() em vez de múltiplos Add(), e considere desabilitar AutoDetectChanges temporariamente. Em cenários de alta concorrência, implemente controle de concorrência otimista usando tokens de concorrência (RowVersion).",
                    Order = 3
                }
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Operações CRUD Completas",
                    Code = @"// CREATE - Inserir novo produto
var novoProduto = new Produto
{
    Nome = ""Notebook"",
    Preco = 2500.00m,
    Estoque = 10
};
context.Produtos.Add(novoProduto);
await context.SaveChangesAsync();

// READ - Buscar produto
var produto = await context.Produtos.FindAsync(novoProduto.Id);

// UPDATE - Atualizar produto
produto.Preco = 2300.00m;
await context.SaveChangesAsync(); // Detecta mudança automaticamente

// DELETE - Remover produto
context.Produtos.Remove(produto);
await context.SaveChangesAsync();",
                    Language = "csharp",
                    Explanation = "Este exemplo mostra as quatro operações CRUD básicas. Note que Update não precisa ser chamado explicitamente - o Change Tracker detecta mudanças automaticamente em entidades rastreadas.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Inserção em Lote",
                    Code = @"// Inserir múltiplos produtos eficientemente
var produtos = new List<Produto>
{
    new Produto { Nome = ""Produto 1"", Preco = 10 },
    new Produto { Nome = ""Produto 2"", Preco = 20 },
    new Produto { Nome = ""Produto 3"", Preco = 30 }
};

context.Produtos.AddRange(produtos);
await context.SaveChangesAsync();

// Para grandes volumes, desabilite AutoDetectChanges
context.ChangeTracker.AutoDetectChangesEnabled = false;
for (int i = 0; i < 10000; i++)
{
    context.Produtos.Add(new Produto { Nome = $""Produto {i}"", Preco = i });
}
context.ChangeTracker.DetectChanges();
await context.SaveChangesAsync();
context.ChangeTracker.AutoDetectChangesEnabled = true;",
                    Language = "csharp",
                    Explanation = "AddRange() é mais eficiente que múltiplos Add(). Para grandes volumes, desabilitar AutoDetectChanges melhora significativamente a performance.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Transações Manuais",
                    Code = @"using var transaction = await context.Database.BeginTransactionAsync();
try
{
    // Operação 1: Criar pedido
    var pedido = new Pedido { Total = 100 };
    context.Pedidos.Add(pedido);
    await context.SaveChangesAsync();

    // Operação 2: Atualizar estoque
    var produto = await context.Produtos.FindAsync(1);
    produto.Estoque -= 5;
    await context.SaveChangesAsync();

    // Operação 3: Registrar pagamento
    var pagamento = new Pagamento { PedidoId = pedido.Id, Valor = 100 };
    context.Pagamentos.Add(pagamento);
    await context.SaveChangesAsync();

    await transaction.CommitAsync();
}
catch
{
    await transaction.RollbackAsync();
    throw;
}",
                    Language = "csharp",
                    Explanation = "Transações manuais permitem agrupar múltiplos SaveChanges(). Se qualquer operação falhar, todas são revertidas, mantendo consistência dos dados.",
                    IsRunnable = true
                }
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Implementar CRUD Completo",
                    Description = "Crie uma classe CategoriaRepository com métodos para todas as operações CRUD: Criar, ObterPorId, ObterTodos, Atualizar e Remover.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"using Microsoft.EntityFrameworkCore;

public class CategoriaRepository
{
    private readonly AppDbContext _context;

    public CategoriaRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Categoria> Criar(Categoria categoria)
    {
        // Implementar
    }

    // Implementar outros métodos
}",
                    Hints = new List<string> 
                    { 
                        "Use Add() para criar, FindAsync() para buscar",
                        "Remove() para deletar, SaveChangesAsync() para persistir",
                        "Não esqueça de retornar a entidade após criar" 
                    }
                },
                new Exercise
                {
                    Title = "Atualização Parcial",
                    Description = "Implemente um método que atualiza apenas o preço e estoque de um produto sem carregar a entidade completa. Use Attach() e modifique apenas as propriedades necessárias.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"public async Task AtualizarPrecoEstoque(int produtoId, decimal novoPreco, int novoEstoque)
{
    // Criar entidade sem carregar do banco
    var produto = new Produto { Id = produtoId };
    
    // Anexar e marcar propriedades como modificadas
    
    await context.SaveChangesAsync();
}",
                    Hints = new List<string> 
                    { 
                        "Use context.Attach(produto) para anexar",
                        "Use context.Entry(produto).Property(p => p.Preco).IsModified = true",
                        "Isso gera UPDATE apenas das colunas modificadas" 
                    }
                },
                new Exercise
                {
                    Title = "Processamento em Lote com Transação",
                    Description = "Implemente um método que processa uma lista de pedidos: para cada pedido, cria o registro, atualiza estoque dos produtos e registra pagamento. Use transação para garantir atomicidade.",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = @"public async Task ProcessarPedidos(List<PedidoDto> pedidos)
{
    using var transaction = await context.Database.BeginTransactionAsync();
    try
    {
        foreach (var pedidoDto in pedidos)
        {
            // Criar pedido
            // Atualizar estoque
            // Registrar pagamento
        }
        
        await transaction.CommitAsync();
    }
    catch
    {
        await transaction.RollbackAsync();
        throw;
    }
}",
                    Hints = new List<string> 
                    { 
                        "Use SaveChangesAsync() após cada grupo de operações",
                        "Verifique se há estoque suficiente antes de processar",
                        "Rollback reverte todas as mudanças em caso de erro" 
                    }
                }
            },
            Summary = "Nesta aula você aprendeu sobre operações CRUD no EF Core, como o Change Tracker monitora mudanças, e como gerenciar transações. Dominar essas operações é essencial para qualquer aplicação que persiste dados."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0005-000000000007"),
            CourseId = _courseId,
            Title = "Operações CRUD e Change Tracking",
            Duration = "50 min",
            Difficulty = "Intermediário",
            EstimatedMinutes = 50,
            Prerequisites = JsonSerializer.Serialize(new[] { "10000000-0000-0000-0005-000000000006" }),
            OrderIndex = 7,
            Content = "",
            StructuredContent = JsonSerializer.Serialize(content),
            Version = 1,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }

    private Lesson CreateLesson8()
    {
        var content = new LessonContent
        {
            Objectives = new List<string>
            {
                "Compreender índices e sua importância para performance",
                "Aprender a criar índices com Fluent API",
                "Dominar índices únicos e compostos",
                "Entender quando usar índices"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "O que são Índices?",
                    Content = "Índices são estruturas de dados que melhoram drasticamente a velocidade de queries de busca no banco de dados. Funciona como um índice de livro: em vez de ler todas as páginas para encontrar um tópico, você consulta o índice que aponta diretamente para a página correta. Sem índices, o banco precisa fazer um table scan (ler todas as linhas), o que é lento em tabelas grandes. Com índices, buscas podem ser milhares de vezes mais rápidas. No entanto, índices têm custo: ocupam espaço em disco e tornam inserções e atualizações mais lentas, pois o índice precisa ser atualizado. Por isso, crie índices apenas em colunas frequentemente usadas em WHERE, JOIN e ORDER BY. O EF Core cria automaticamente índices para chaves primárias e chaves estrangeiras. Você pode criar índices adicionais usando Fluent API ou Data Annotations. Índices são essenciais para performance em produção.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Tipos de Índices",
                    Content = "Existem vários tipos de índices: índices simples (uma coluna), índices compostos (múltiplas colunas), índices únicos (garantem valores únicos) e índices filtrados (apenas parte dos dados). Índices simples são os mais comuns, criados em colunas frequentemente buscadas como Email ou CPF. Índices compostos são úteis quando você busca por múltiplas colunas juntas, como (CategoriaId, DataCriacao). A ordem das colunas importa: o índice (A, B) ajuda queries que filtram por A ou por A e B, mas não queries que filtram apenas por B. Índices únicos garantem que não haja valores duplicados, útil para Email, CPF, etc. Índices filtrados incluem apenas linhas que atendem uma condição, economizando espaço. No SQL Server, índices podem ser clustered (determina ordem física dos dados) ou non-clustered (estrutura separada). A chave primária é sempre um índice clustered por padrão.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Criando Índices no EF Core",
                    Content = "No EF Core, use HasIndex() na Fluent API para criar índices. Por exemplo, entity.HasIndex(p => p.Email) cria um índice na coluna Email. Para índice único, adicione IsUnique(): HasIndex(p => p.Email).IsUnique(). Para índices compostos, passe múltiplas propriedades: HasIndex(p => new { p.CategoriaId, p.DataCriacao }). Você pode nomear índices com HasDatabaseName(). Para índices filtrados, use HasFilter(): HasIndex(p => p.Email).HasFilter(\"[Email] IS NOT NULL\"). O EF Core gera comandos CREATE INDEX nas migrations. Monitore o plano de execução de queries lentas para identificar onde índices são necessários. Use ferramentas como SQL Server Profiler ou Azure Data Studio para analisar performance. Lembre-se: mais índices não é sempre melhor. Cada índice tem custo de manutenção. Crie índices baseado em análise de queries reais, não especulação.",
                    Order = 3
                }
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Criando Índices Simples e Únicos",
                    Code = @"protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Cliente>(entity =>
    {
        // Índice simples em Nome para buscas
        entity.HasIndex(c => c.Nome);

        // Índice único em Email
        entity.HasIndex(c => c.Email)
            .IsUnique()
            .HasDatabaseName(""IX_Cliente_Email"");

        // Índice único em CPF
        entity.HasIndex(c => c.CPF)
            .IsUnique();
    });
}

// Migration gerada criará:
// CREATE INDEX IX_Cliente_Nome ON Clientes (Nome);
// CREATE UNIQUE INDEX IX_Cliente_Email ON Clientes (Email);",
                    Language = "csharp",
                    Explanation = "Índices simples aceleram buscas. Índices únicos garantem valores únicos e também aceleram buscas. HasDatabaseName() permite nomear o índice explicitamente.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Índices Compostos",
                    Code = @"protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Produto>(entity =>
    {
        // Índice composto para queries que filtram por categoria e data
        entity.HasIndex(p => new { p.CategoriaId, p.DataCriacao })
            .HasDatabaseName(""IX_Produto_Categoria_Data"");

        // Útil para queries como:
        // WHERE CategoriaId = 5 AND DataCriacao > '2024-01-01'
        // WHERE CategoriaId = 5 ORDER BY DataCriacao
    });

    modelBuilder.Entity<Pedido>(entity =>
    {
        // Índice composto para buscar pedidos de um cliente por data
        entity.HasIndex(p => new { p.ClienteId, p.Data })
            .IsDescending(false, true); // ClienteId ASC, Data DESC
    });
}",
                    Language = "csharp",
                    Explanation = "Índices compostos otimizam queries que filtram por múltiplas colunas. A ordem das colunas no índice deve corresponder à ordem usada nas queries mais frequentes.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Índices Filtrados",
                    Code = @"protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Produto>(entity =>
    {
        // Índice apenas para produtos ativos
        entity.HasIndex(p => p.Nome)
            .HasFilter(""[Ativo] = 1"")
            .HasDatabaseName(""IX_Produto_Nome_Ativo"");

        // Índice para produtos com estoque
        entity.HasIndex(p => new { p.CategoriaId, p.Preco })
            .HasFilter(""[Estoque] > 0"");
    });
}

// Útil quando a maioria das queries filtra por uma condição específica
// Economiza espaço e melhora performance",
                    Language = "csharp",
                    Explanation = "Índices filtrados incluem apenas linhas que atendem uma condição. Isso economiza espaço e melhora performance quando você sempre filtra pela mesma condição.",
                    IsRunnable = true
                }
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Criar Índices Básicos",
                    Description = "Na entidade Produto, crie índices para: Nome (simples), Código (único), e um índice composto em (CategoriaId, Preco). Gere a migration e verifique os comandos SQL.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Produto>(entity =>
    {
        // Criar índices aqui
    });
}",
                    Hints = new List<string> 
                    { 
                        "Use HasIndex(p => p.Nome) para índice simples",
                        "Adicione .IsUnique() para índice único",
                        "Use new { p.CategoriaId, p.Preco } para composto" 
                    }
                },
                new Exercise
                {
                    Title = "Analisar Performance com Índices",
                    Description = "Crie uma tabela com 100.000 produtos. Execute uma busca por nome sem índice e meça o tempo. Adicione um índice e meça novamente. Compare os resultados.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"// Inserir 100.000 produtos
for (int i = 0; i < 100000; i++)
{
    context.Produtos.Add(new Produto { Nome = $""Produto {i}"" });
}
await context.SaveChangesAsync();

// Buscar sem índice
var sw = Stopwatch.StartNew();
var produto = await context.Produtos
    .FirstOrDefaultAsync(p => p.Nome == ""Produto 50000"");
sw.Stop();
Console.WriteLine($""Sem índice: {sw.ElapsedMilliseconds}ms"");

// Adicionar índice e testar novamente",
                    Hints = new List<string> 
                    { 
                        "Use Stopwatch para medir tempo",
                        "Crie migration com índice em Nome",
                        "A diferença deve ser significativa (10x-100x mais rápido)" 
                    }
                },
                new Exercise
                {
                    Title = "Otimizar Query Lenta",
                    Description = "Dada uma query que busca pedidos de um cliente em um período específico, identifique qual índice composto seria mais eficiente e implemente-o.",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = @"// Query lenta
var pedidos = await context.Pedidos
    .Where(p => p.ClienteId == clienteId)
    .Where(p => p.Data >= dataInicio && p.Data <= dataFim)
    .OrderBy(p => p.Data)
    .ToListAsync();

// Qual índice composto otimizaria esta query?
// Implementar no OnModelCreating",
                    Hints = new List<string> 
                    { 
                        "A query filtra por ClienteId e Data, e ordena por Data",
                        "Índice composto (ClienteId, Data) seria ideal",
                        "Use ToQueryString() para ver o plano de execução" 
                    }
                }
            },
            Summary = "Nesta aula você aprendeu sobre índices de banco de dados, os diferentes tipos (simples, compostos, únicos, filtrados), e como criá-los no EF Core. Índices são cruciais para performance de queries em produção."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0005-000000000008"),
            CourseId = _courseId,
            Title = "Índices e Otimização de Queries",
            Duration = "50 min",
            Difficulty = "Intermediário",
            EstimatedMinutes = 50,
            Prerequisites = JsonSerializer.Serialize(new[] { "10000000-0000-0000-0005-000000000007" }),
            OrderIndex = 8,
            Content = "",
            StructuredContent = JsonSerializer.Serialize(content),
            Version = 1,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }
}
