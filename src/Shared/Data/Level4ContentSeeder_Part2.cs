using Shared.Entities;
using Shared.Models;
using System.Text.Json;

namespace Shared.Data;

/// <summary>
/// Level 4 Content Seeder - Part 2 (Lessons 4-20)
/// </summary>
public partial class Level4ContentSeeder
{
    private Lesson CreateLesson4()
    {
        var content = new LessonContent
        {
            Objectives = new List<string>
            {
                "Compreender os tipos de relacionamentos no EF Core",
                "Aprender a configurar relacionamentos um-para-muitos",
                "Dominar relacionamentos um-para-um",
                "Entender relacionamentos muitos-para-muitos"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "Tipos de Relacionamentos",
                    Content = "No EF Core, existem três tipos principais de relacionamentos entre entidades: um-para-muitos (1:N), um-para-um (1:1) e muitos-para-muitos (N:M). Um relacionamento um-para-muitos é o mais comum, como Cliente tem muitos Pedidos. Um relacionamento um-para-um conecta duas entidades onde cada instância de uma está associada a exatamente uma instância da outra, como Usuario tem um Perfil. Relacionamentos muitos-para-muitos conectam múltiplas instâncias de ambos os lados, como Aluno tem muitos Cursos e Curso tem muitos Alunos. O EF Core usa propriedades de navegação para representar relacionamentos: uma propriedade de navegação de coleção (List<T>) para o lado 'muitos' e uma propriedade de navegação de referência (T) para o lado 'um'. Além disso, você geralmente tem uma propriedade de chave estrangeira (foreign key) que armazena o ID da entidade relacionada.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Configurando Relacionamentos",
                    Content = "O EF Core pode detectar relacionamentos automaticamente por convenção, mas é recomendado configurá-los explicitamente usando Fluent API para maior clareza e controle. Para um relacionamento um-para-muitos, use HasOne() e WithMany(). Por exemplo, modelBuilder.Entity<Pedido>().HasOne(p => p.Cliente).WithMany(c => c.Pedidos).HasForeignKey(p => p.ClienteId). Isso define que Pedido tem um Cliente e Cliente tem muitos Pedidos. Para um-para-um, use HasOne() e WithOne(). Para muitos-para-muitos no EF Core 5+, você pode usar HasMany() e WithMany() sem precisar criar uma entidade de junção explícita - o EF Core cria automaticamente. Você também pode configurar comportamentos de deleção em cascata com OnDelete(DeleteBehavior.Cascade) ou restringir com DeleteBehavior.Restrict. Configurar relacionamentos corretamente é crucial para integridade referencial e performance.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Propriedades de Navegação",
                    Content = "Propriedades de navegação permitem navegar entre entidades relacionadas no código C#. Por exemplo, se você tem um Pedido, pode acessar pedido.Cliente para obter o cliente associado, e cliente.Pedidos para obter todos os pedidos daquele cliente. Existem dois tipos: navegação de referência (propriedade única) e navegação de coleção (List, ICollection). As propriedades de navegação não são armazenadas no banco - elas são materializadas pelo EF Core quando você carrega dados. Você pode ter navegações em ambos os lados (bidirecional) ou apenas em um lado (unidirecional). Navegações bidirecionais facilitam a navegação mas requerem mais cuidado para manter sincronizadas. O EF Core usa essas propriedades para gerar JOINs automaticamente em queries. É importante entender que carregar uma navegação pode resultar em queries adicionais ao banco, dependendo da estratégia de carregamento (lazy, eager ou explicit).",
                    Order = 3
                }
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Relacionamento Um-para-Muitos",
                    Code = @"// Entidades
public class Cliente
{
    public int Id { get; set; }
    public string Nome { get; set; }
    
    // Navegação de coleção
    public List<Pedido> Pedidos { get; set; }
}

public class Pedido
{
    public int Id { get; set; }
    public DateTime Data { get; set; }
    public decimal Total { get; set; }
    
    // Chave estrangeira
    public int ClienteId { get; set; }
    
    // Navegação de referência
    public Cliente Cliente { get; set; }
}

// Configuração Fluent API
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Pedido>()
        .HasOne(p => p.Cliente)
        .WithMany(c => c.Pedidos)
        .HasForeignKey(p => p.ClienteId)
        .OnDelete(DeleteBehavior.Cascade);
}",
                    Language = "csharp",
                    Explanation = "Este exemplo mostra um relacionamento um-para-muitos completo. Cliente tem uma coleção de Pedidos, e Pedido tem uma referência ao Cliente. A configuração Fluent API define explicitamente o relacionamento e comportamento de deleção em cascata.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Relacionamento Muitos-para-Muitos",
                    Code = @"// Entidades
public class Aluno
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public List<Curso> Cursos { get; set; }
}

public class Curso
{
    public int Id { get; set; }
    public string Titulo { get; set; }
    public List<Aluno> Alunos { get; set; }
}

// Configuração (EF Core 5+)
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Aluno>()
        .HasMany(a => a.Cursos)
        .WithMany(c => c.Alunos)
        .UsingEntity(j => j.ToTable(""AlunosCursos""));
}

// Uso
var aluno = new Aluno { Nome = ""João"" };
var curso = new Curso { Titulo = ""C# Avançado"" };
aluno.Cursos = new List<Curso> { curso };
context.Alunos.Add(aluno);
await context.SaveChanges();",
                    Language = "csharp",
                    Explanation = "No EF Core 5+, relacionamentos muitos-para-muitos são simples. O EF Core cria automaticamente a tabela de junção. Você pode personalizar o nome da tabela com UsingEntity().",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Relacionamento Um-para-Um",
                    Code = @"// Entidades
public class Usuario
{
    public int Id { get; set; }
    public string Email { get; set; }
    public Perfil Perfil { get; set; }
}

public class Perfil
{
    public int Id { get; set; }
    public string Bio { get; set; }
    public string Foto { get; set; }
    
    public int UsuarioId { get; set; }
    public Usuario Usuario { get; set; }
}

// Configuração
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Usuario>()
        .HasOne(u => u.Perfil)
        .WithOne(p => p.Usuario)
        .HasForeignKey<Perfil>(p => p.UsuarioId);
}",
                    Language = "csharp",
                    Explanation = "Relacionamento um-para-um requer especificar qual entidade contém a chave estrangeira usando HasForeignKey<T>(). Neste caso, Perfil contém UsuarioId.",
                    IsRunnable = true
                }
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Criar Relacionamento Categoria-Produto",
                    Description = "Crie entidades Categoria e Produto com relacionamento um-para-muitos (uma categoria tem muitos produtos). Configure usando Fluent API e crie uma migration.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"public class Categoria
{
    public int Id { get; set; }
    public string Nome { get; set; }
    // Adicionar navegação
}

public class Produto
{
    public int Id { get; set; }
    public string Nome { get; set; }
    // Adicionar FK e navegação
}",
                    Hints = new List<string> 
                    { 
                        "Categoria deve ter List<Produto> Produtos",
                        "Produto deve ter int CategoriaId e Categoria Categoria",
                        "Use HasOne().WithMany().HasForeignKey()" 
                    }
                },
                new Exercise
                {
                    Title = "Implementar Relacionamento Muitos-para-Muitos",
                    Description = "Crie um sistema de Tags para Produtos onde um produto pode ter múltiplas tags e uma tag pode estar em múltiplos produtos. Implemente as entidades e configuração.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"public class Produto
{
    public int Id { get; set; }
    public string Nome { get; set; }
    // Adicionar navegação para Tags
}

public class Tag
{
    public int Id { get; set; }
    public string Nome { get; set; }
    // Adicionar navegação para Produtos
}

// Configurar no OnModelCreating",
                    Hints = new List<string> 
                    { 
                        "Use List<Tag> e List<Produto> para navegações",
                        "Configure com HasMany().WithMany()",
                        "Opcionalmente nomeie a tabela de junção com UsingEntity()" 
                    }
                },
                new Exercise
                {
                    Title = "Configurar Deleção em Cascata",
                    Description = "No relacionamento Cliente-Pedido, configure para que ao deletar um Cliente, todos os seus Pedidos sejam deletados automaticamente. Teste criando um cliente com pedidos e deletando o cliente.",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = @"// Configuração
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    // Configurar deleção em cascata
}

// Teste
var cliente = new Cliente { Nome = ""Teste"" };
cliente.Pedidos = new List<Pedido>
{
    new Pedido { Total = 100 },
    new Pedido { Total = 200 }
};
context.Clientes.Add(cliente);
await context.SaveChanges();

// Deletar cliente e verificar se pedidos foram deletados
context.Clientes.Remove(cliente);
await context.SaveChanges();",
                    Hints = new List<string> 
                    { 
                        "Use OnDelete(DeleteBehavior.Cascade)",
                        "Verifique no banco se os pedidos foram removidos",
                        "Cuidado: deleção em cascata pode remover muitos dados" 
                    }
                }
            },
            Summary = "Nesta aula você aprendeu sobre os três tipos de relacionamentos no EF Core (um-para-muitos, um-para-um, muitos-para-muitos), como configurá-los usando Fluent API e como usar propriedades de navegação para navegar entre entidades relacionadas."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0005-000000000004"),
            CourseId = _courseId,
            Title = "Relacionamentos entre Entidades",
            Duration = "55 min",
            Difficulty = "Intermediário",
            EstimatedMinutes = 55,
            Prerequisites = JsonSerializer.Serialize(new[] { "10000000-0000-0000-0005-000000000003" }),
            OrderIndex = 4,
            Content = "",
            StructuredContent = JsonSerializer.Serialize(content),
            Version = 1,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }

    private Lesson CreateLesson5()
    {
        var content = new LessonContent
        {
            Objectives = new List<string>
            {
                "Compreender LINQ e sua integração com EF Core",
                "Aprender operadores LINQ básicos para queries",
                "Dominar filtragem, ordenação e projeção",
                "Entender como LINQ é traduzido para SQL"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "LINQ e Entity Framework Core",
                    Content = "LINQ (Language Integrated Query) é uma funcionalidade do C# que permite escrever queries de forma declarativa e type-safe. No EF Core, LINQ é traduzido automaticamente para SQL e executado no banco de dados. Isso significa que você escreve código C# mas a execução acontece no servidor de banco de dados, não na memória da aplicação. Por exemplo, context.Produtos.Where(p => p.Preco > 100) gera SELECT * FROM Produtos WHERE Preco > 100. O EF Core suporta a maioria dos operadores LINQ: Where (filtrar), Select (projetar), OrderBy (ordenar), GroupBy (agrupar), Join (juntar), Count, Sum, Average, etc. É importante entender que queries LINQ são executadas de forma lazy - a query só é enviada ao banco quando você materializa os resultados com ToList(), FirstOrDefault(), Count(), etc. Isso permite compor queries complexas antes de executá-las.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Operadores LINQ Essenciais",
                    Content = "Os operadores LINQ mais usados são: Where() para filtrar registros baseado em condições, Select() para projetar apenas as colunas necessárias, OrderBy()/OrderByDescending() para ordenar resultados, Take()/Skip() para paginação, First()/FirstOrDefault() para obter o primeiro registro, Any() para verificar existência, e Count() para contar registros. Você pode encadear múltiplos operadores: context.Produtos.Where(p => p.Estoque > 0).OrderBy(p => p.Nome).Take(10). O EF Core otimiza a query resultante. Use FirstOrDefault() em vez de First() para evitar exceções quando nenhum registro é encontrado. Use Any() em vez de Count() > 0 para verificar existência - é mais eficiente pois para na primeira ocorrência. Select() é crucial para performance: projete apenas as colunas que você precisa em vez de carregar entidades completas.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Tradução para SQL",
                    Content = "O EF Core usa um provider de banco de dados para traduzir expressões LINQ em SQL específico do banco. Você pode ver o SQL gerado usando ToQueryString() ou habilitando logging. Nem todas as expressões C# podem ser traduzidas para SQL - se você usar um método que o EF Core não conhece, ele tentará avaliar no cliente (client evaluation), o que pode ser ineficiente. No EF Core 3.0+, client evaluation é restrito e lança exceção por padrão. Para forçar avaliação no cliente, use AsEnumerable() antes do operador. Por exemplo, context.Produtos.AsEnumerable().Where(p => MetodoCustomizado(p)). Entender a tradução SQL ajuda a escrever queries eficientes. Use Include() para carregar relacionamentos em uma única query (eager loading) em vez de múltiplas queries (N+1 problem). Sempre teste queries complexas e verifique o SQL gerado para garantir performance adequada.",
                    Order = 3
                }
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Operadores LINQ Básicos",
                    Code = @"// Filtrar produtos com estoque
var produtosDisponiveis = await context.Produtos
    .Where(p => p.Estoque > 0)
    .ToListAsync();

// Ordenar por preço
var produtosOrdenados = await context.Produtos
    .OrderBy(p => p.Preco)
    .ToListAsync();

// Projetar apenas nome e preço
var produtosSimples = await context.Produtos
    .Select(p => new { p.Nome, p.Preco })
    .ToListAsync();

// Paginação
var produtosPaginados = await context.Produtos
    .Skip(20)
    .Take(10)
    .ToListAsync();

// Verificar existência
bool existeProdutoCaro = await context.Produtos
    .AnyAsync(p => p.Preco > 1000);",
                    Language = "csharp",
                    Explanation = "Este exemplo demonstra os operadores LINQ mais comuns. Cada operação é traduzida para SQL eficiente. Note o uso de async/await para operações assíncronas.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Queries Complexas",
                    Code = @"// Buscar produtos de uma categoria específica com estoque
var produtos = await context.Produtos
    .Where(p => p.CategoriaId == 5)
    .Where(p => p.Estoque > 0)
    .OrderByDescending(p => p.DataCadastro)
    .Select(p => new
    {
        p.Id,
        p.Nome,
        p.Preco,
        Categoria = p.Categoria.Nome
    })
    .Take(20)
    .ToListAsync();

// Agrupar e contar
var produtosPorCategoria = await context.Produtos
    .GroupBy(p => p.CategoriaId)
    .Select(g => new
    {
        CategoriaId = g.Key,
        Quantidade = g.Count(),
        PrecoMedio = g.Average(p => p.Preco)
    })
    .ToListAsync();",
                    Language = "csharp",
                    Explanation = "Queries complexas podem ser construídas encadeando operadores. O EF Core otimiza tudo em uma única query SQL. GroupBy permite agregações como Count, Sum, Average.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Visualizando SQL Gerado",
                    Code = @"using Microsoft.EntityFrameworkCore;

// Ver SQL gerado
var query = context.Produtos
    .Where(p => p.Preco > 100)
    .OrderBy(p => p.Nome);

string sql = query.ToQueryString();
Console.WriteLine(sql);

// Habilitar logging no Program.cs
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString)
           .LogTo(Console.WriteLine, LogLevel.Information)
           .EnableSensitiveDataLogging()
);

// Agora todas as queries serão logadas no console",
                    Language = "csharp",
                    Explanation = "ToQueryString() mostra o SQL que será executado. Habilitar logging é útil para debug e otimização. EnableSensitiveDataLogging() mostra valores de parâmetros (não use em produção).",
                    IsRunnable = true
                }
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Queries Básicas com LINQ",
                    Description = "Escreva queries LINQ para: (1) Buscar todos os produtos com preço entre 50 e 200, (2) Buscar os 5 produtos mais caros, (3) Contar quantos produtos têm estoque zero.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"// Query 1: Produtos entre 50 e 200
var query1 = await context.Produtos
    // Completar

// Query 2: 5 produtos mais caros
var query2 = await context.Produtos
    // Completar

// Query 3: Contar produtos sem estoque
var query3 = await context.Produtos
    // Completar",
                    Hints = new List<string> 
                    { 
                        "Use Where() com && para múltiplas condições",
                        "OrderByDescending() + Take() para top N",
                        "CountAsync() com Where() para contar com filtro" 
                    }
                },
                new Exercise
                {
                    Title = "Projeção e Performance",
                    Description = "Crie uma query que retorna apenas Id, Nome e NomeCategoria de produtos. Compare a performance com carregar entidades completas. Use ToQueryString() para ver a diferença no SQL.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"// Query com projeção
var produtosProjetados = await context.Produtos
    .Select(p => new
    {
        // Completar
    })
    .ToListAsync();

// Query sem projeção
var produtosCompletos = await context.Produtos
    .Include(p => p.Categoria)
    .ToListAsync();

// Comparar SQL gerado",
                    Hints = new List<string> 
                    { 
                        "Use Select() para projetar apenas colunas necessárias",
                        "Acesse p.Categoria.Nome na projeção",
                        "ToQueryString() mostra o SQL antes de executar" 
                    }
                },
                new Exercise
                {
                    Title = "Implementar Busca Avançada",
                    Description = "Crie um método BuscarProdutos que aceita parâmetros opcionais (nome, categoriaId, precoMin, precoMax) e retorna produtos filtrados. Use LINQ para construir a query dinamicamente.",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = @"public async Task<List<Produto>> BuscarProdutos(
    string nome = null,
    int? categoriaId = null,
    decimal? precoMin = null,
    decimal? precoMax = null)
{
    var query = context.Produtos.AsQueryable();

    // Adicionar filtros condicionalmente
    
    return await query.ToListAsync();
}",
                    Hints = new List<string> 
                    { 
                        "Use if para adicionar Where() condicionalmente",
                        "Contains() para busca parcial de nome",
                        "AsQueryable() permite compor a query antes de executar" 
                    }
                }
            },
            Summary = "Nesta aula você aprendeu sobre LINQ e sua integração com EF Core, os operadores LINQ essenciais para queries, e como o EF Core traduz LINQ para SQL. Dominar LINQ é fundamental para escrever queries eficientes e expressivas."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0005-000000000005"),
            CourseId = _courseId,
            Title = "LINQ - Consultando Dados",
            Duration = "50 min",
            Difficulty = "Intermediário",
            EstimatedMinutes = 50,
            Prerequisites = JsonSerializer.Serialize(new[] { "10000000-0000-0000-0005-000000000004" }),
            OrderIndex = 5,
            Content = "",
            StructuredContent = JsonSerializer.Serialize(content),
            Version = 1,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }

    private Lesson CreateLesson6()
    {
        var content = new LessonContent
        {
            Objectives = new List<string>
            {
                "Compreender estratégias de carregamento no EF Core",
                "Aprender sobre Lazy Loading e suas implicações",
                "Dominar Eager Loading com Include",
                "Entender Explicit Loading e quando usá-lo"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "Estratégias de Carregamento",
                    Content = "O EF Core oferece três estratégias para carregar dados relacionados: Lazy Loading (carregamento preguiçoso), Eager Loading (carregamento antecipado) e Explicit Loading (carregamento explícito). Lazy Loading carrega relacionamentos automaticamente quando você acessa a propriedade de navegação pela primeira vez. Eager Loading carrega relacionamentos junto com a entidade principal em uma única query usando Include(). Explicit Loading permite carregar relacionamentos sob demanda usando métodos como Entry().Collection().Load(). Cada estratégia tem vantagens e desvantagens. Lazy Loading é conveniente mas pode causar o problema N+1 (múltiplas queries ao banco). Eager Loading é eficiente mas pode carregar dados desnecessários. Explicit Loading oferece controle fino mas requer mais código. A escolha depende do cenário: use Eager Loading para dados que você sabe que vai precisar, Explicit Loading para carregamento condicional, e evite Lazy Loading em produção devido a problemas de performance.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Eager Loading com Include",
                    Content = "Eager Loading usa o método Include() para carregar relacionamentos em uma única query com JOINs. Por exemplo, context.Pedidos.Include(p => p.Cliente) gera um LEFT JOIN e carrega pedidos com seus clientes. Você pode encadear múltiplos Include() para carregar vários relacionamentos: Include(p => p.Cliente).Include(p => p.Itens). Para relacionamentos aninhados, use ThenInclude(): Include(p => p.Itens).ThenInclude(i => i.Produto). Isso carrega pedidos, seus itens e os produtos de cada item. Eager Loading é a estratégia mais eficiente quando você sabe que vai acessar os relacionamentos. No entanto, cuidado para não carregar dados demais - isso aumenta o tamanho da query e o uso de memória. Use projeções com Select() quando precisar apenas de algumas propriedades dos relacionamentos. O EF Core otimiza queries com Include() usando split queries quando necessário para evitar explosão cartesiana.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Lazy Loading e Seus Problemas",
                    Content = "Lazy Loading carrega relacionamentos automaticamente quando acessados, mas requer que as propriedades de navegação sejam virtual e que você instale o pacote Microsoft.EntityFrameworkCore.Proxies. Quando habilitado, o EF Core cria proxies dinâmicos que interceptam acessos às propriedades. O problema principal do Lazy Loading é o N+1: se você carrega 100 pedidos e acessa pedido.Cliente para cada um, são executadas 101 queries (1 para pedidos + 100 para clientes). Isso é extremamente ineficiente. Lazy Loading também não funciona após o DbContext ser descartado, causando exceções. Por esses motivos, muitos desenvolvedores desabilitam Lazy Loading completamente e usam Eager ou Explicit Loading. Se você usar Lazy Loading, monitore as queries geradas e converta para Eager Loading onde necessário. Em APIs web, Lazy Loading pode causar problemas de serialização JSON.",
                    Order = 3
                }
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Eager Loading com Include",
                    Code = @"// Carregar pedidos com clientes
var pedidos = await context.Pedidos
    .Include(p => p.Cliente)
    .ToListAsync();

// Carregar múltiplos relacionamentos
var pedidosCompletos = await context.Pedidos
    .Include(p => p.Cliente)
    .Include(p => p.Itens)
    .ToListAsync();

// Relacionamentos aninhados
var pedidosDetalhados = await context.Pedidos
    .Include(p => p.Itens)
        .ThenInclude(i => i.Produto)
    .Include(p => p.Cliente)
    .ToListAsync();

// Agora você pode acessar sem queries adicionais
foreach (var pedido in pedidosDetalhados)
{
    Console.WriteLine($""Cliente: {pedido.Cliente.Nome}"");
    foreach (var item in pedido.Itens)
    {
        Console.WriteLine($""  Produto: {item.Produto.Nome}"");
    }
}",
                    Language = "csharp",
                    Explanation = "Include() carrega relacionamentos em uma única query eficiente. ThenInclude() permite carregar relacionamentos aninhados. Todos os dados são carregados antecipadamente, evitando queries adicionais.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Explicit Loading",
                    Code = @"// Carregar pedido sem relacionamentos
var pedido = await context.Pedidos.FindAsync(1);

// Carregar cliente explicitamente se necessário
if (precisaCliente)
{
    await context.Entry(pedido)
        .Reference(p => p.Cliente)
        .LoadAsync();
}

// Carregar coleção de itens
await context.Entry(pedido)
    .Collection(p => p.Itens)
    .LoadAsync();

// Carregar com filtro
await context.Entry(pedido)
    .Collection(p => p.Itens)
    .Query()
    .Where(i => i.Quantidade > 1)
    .LoadAsync();",
                    Language = "csharp",
                    Explanation = "Explicit Loading permite carregar relacionamentos sob demanda. Use Reference() para navegações únicas e Collection() para coleções. Query() permite filtrar antes de carregar.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Comparando Estratégias",
                    Code = @"// ❌ Problema N+1 (sem Include)
var pedidos = await context.Pedidos.ToListAsync();
foreach (var pedido in pedidos)
{
    // Cada acesso gera uma query!
    Console.WriteLine(pedido.Cliente.Nome);
}

// ✅ Solução com Eager Loading
var pedidosComCliente = await context.Pedidos
    .Include(p => p.Cliente)
    .ToListAsync();
foreach (var pedido in pedidosComCliente)
{
    // Sem queries adicionais
    Console.WriteLine(pedido.Cliente.Nome);
}

// ✅ Alternativa com projeção
var pedidosProjetados = await context.Pedidos
    .Select(p => new
    {
        p.Id,
        p.Total,
        ClienteNome = p.Cliente.Nome
    })
    .ToListAsync();",
                    Language = "csharp",
                    Explanation = "O primeiro exemplo causa N+1 queries. O segundo usa Include() para carregar tudo em uma query. O terceiro usa projeção para carregar apenas os dados necessários - a abordagem mais eficiente.",
                    IsRunnable = true
                }
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Implementar Eager Loading",
                    Description = "Crie uma query que carrega Pedidos com Cliente, Itens e Produto de cada item. Verifique o SQL gerado e conte quantas queries são executadas.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"var pedidos = await context.Pedidos
    // Adicionar Include e ThenInclude
    .ToListAsync();

// Verificar SQL
var query = context.Pedidos
    // Mesmos includes
    ;
Console.WriteLine(query.ToQueryString());",
                    Hints = new List<string> 
                    { 
                        "Use Include() para Cliente",
                        "Use Include().ThenInclude() para Itens e Produtos",
                        "Deve resultar em 1 ou 2 queries (split query)" 
                    }
                },
                new Exercise
                {
                    Title = "Detectar e Corrigir N+1",
                    Description = "Dado o código que lista produtos e suas categorias, identifique o problema N+1 e corrija usando Eager Loading. Meça a diferença de performance.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"// Código com problema
var produtos = await context.Produtos.ToListAsync();
foreach (var produto in produtos)
{
    Console.WriteLine($""{produto.Nome} - {produto.Categoria.Nome}"");
}

// Corrigir aqui
var produtosCorrigidos = await context.Produtos
    // Adicionar Include
    .ToListAsync();",
                    Hints = new List<string> 
                    { 
                        "Habilite logging para ver quantas queries são executadas",
                        "Use Include(p => p.Categoria)",
                        "Compare o número de queries antes e depois" 
                    }
                },
                new Exercise
                {
                    Title = "Carregamento Condicional",
                    Description = "Implemente um método que carrega um Pedido e, baseado em um parâmetro, carrega ou não os Itens. Use Explicit Loading para carregar condicionalmente.",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = @"public async Task<Pedido> ObterPedido(int id, bool incluirItens)
{
    var pedido = await context.Pedidos.FindAsync(id);
    
    if (incluirItens)
    {
        // Carregar itens explicitamente
    }
    
    return pedido;
}",
                    Hints = new List<string> 
                    { 
                        "Use context.Entry(pedido).Collection(p => p.Itens).LoadAsync()",
                        "Explicit Loading permite controle fino",
                        "Útil quando o carregamento depende de lógica de negócio" 
                    }
                }
            },
            Summary = "Nesta aula você aprendeu sobre as três estratégias de carregamento no EF Core: Lazy Loading, Eager Loading e Explicit Loading. Compreender quando usar cada estratégia é crucial para evitar problemas de performance como N+1 queries."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0005-000000000006"),
            CourseId = _courseId,
            Title = "Estratégias de Carregamento de Dados",
            Duration = "50 min",
            Difficulty = "Intermediário",
            EstimatedMinutes = 50,
            Prerequisites = JsonSerializer.Serialize(new[] { "10000000-0000-0000-0005-000000000005" }),
            OrderIndex = 6,
            Content = "",
            StructuredContent = JsonSerializer.Serialize(content),
            Version = 1,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }
}
