using Shared.Entities;
using Shared.Models;
using System.Text.Json;

namespace Shared.Data;

/// <summary>
/// Seeds Level 4 content: Entity Framework Core (20 lessons)
/// Covers Code First, Migrations, DbContext, Relationships, LINQ, Lazy/Eager Loading, Performance
/// </summary>
public partial class Level4ContentSeeder
{
    private readonly Guid _courseId = Guid.Parse("10000000-0000-0000-0000-000000000005");
    private readonly Guid _levelId = Guid.Parse("00000000-0000-0000-0000-000000000004");

    public Course CreateLevel4Course()
    {
        return new Course
        {
            Id = _courseId,
            LevelId = _levelId,
            Title = "Entity Framework Core",
            Description = "Domine o Entity Framework Core, o ORM mais popular do .NET. Aprenda a trabalhar com bancos de dados usando Code First, migrations, relacionamentos complexos, LINQ avançado e técnicas de otimização de performance para criar aplicações robustas e eficientes.",
            Level = Level.Intermediate,
            Duration = "4 semanas",
            LessonCount = 20,
            Topics = JsonSerializer.Serialize(new[] 
            { 
                "Code First", 
                "Migrations", 
                "DbContext", 
                "Relacionamentos",
                "LINQ",
                "Lazy Loading",
                "Eager Loading",
                "Performance"
            }),
            OrderIndex = 1,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }

    public List<Lesson> CreateLevel4Lessons()
    {
        var lessons = new List<Lesson>
        {
            CreateLesson1(),
            CreateLesson2(),
            CreateLesson3(),
            CreateLesson4(),
            CreateLesson5(),
            CreateLesson6(),
            CreateLesson7(),
            CreateLesson8(),
            CreateLesson9(),
            CreateLesson10(),
            CreateLesson11(),
            CreateLesson12(),
            CreateLesson13(),
            CreateLesson14(),
            CreateLesson15(),
            CreateLesson16(),
            CreateLesson17(),
            CreateLesson18(),
            CreateLesson19(),
            CreateLesson20()
        };

        return lessons;
    }

    private Lesson CreateLesson1()
    {
        var content = new LessonContent
        {
            Objectives = new List<string>
            {
                "Compreender o que é Entity Framework Core e suas vantagens",
                "Conhecer a diferença entre ORM e acesso direto ao banco de dados",
                "Entender os conceitos de Code First e Database First",
                "Configurar o Entity Framework Core em um projeto ASP.NET Core"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "O que é Entity Framework Core?",
                    Content = "Entity Framework Core (EF Core) é um ORM (Object-Relational Mapper) moderno, leve e multiplataforma para .NET. Um ORM é uma ferramenta que permite trabalhar com bancos de dados usando objetos C# em vez de escrever SQL diretamente, criando uma ponte entre o mundo orientado a objetos da aplicação e o mundo relacional do banco de dados. Imagine que você tem uma classe Produto em C# com propriedades como Nome, Preco e Estoque, e uma tabela Produtos no banco de dados com colunas correspondentes. Com EF Core, você pode salvar um objeto Produto simplesmente chamando context.Produtos.Add(produto) e context.SaveChanges(), sem escrever comandos INSERT INTO manualmente. O EF Core traduz suas operações em C# para comandos SQL otimizados automaticamente, gerando queries eficientes baseadas nas suas necessidades. Isso aumenta significativamente a produtividade do desenvolvedor, reduz erros de sintaxe SQL, facilita a manutenção do código e torna as operações de banco de dados mais seguras contra SQL Injection. EF Core suporta múltiplos bancos de dados (SQL Server, PostgreSQL, MySQL, SQLite, Oracle, Cosmos DB) através de providers específicos, permitindo trocar de banco de dados com mínimas alterações no código da aplicação. É a evolução do Entity Framework 6, completamente redesenhado para ser mais leve, mais rápido, extensível e verdadeiramente multiplataforma, funcionando em Windows, Linux e macOS. EF Core também oferece recursos avançados como change tracking (rastreamento de mudanças), lazy loading (carregamento preguiçoso), eager loading (carregamento antecipado), migrations (versionamento de schema), e suporte a queries complexas com LINQ.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Code First vs Database First",
                    Content = "Existem duas abordagens principais para usar EF Core: Code First e Database First, cada uma com suas vantagens e casos de uso específicos. No Code First, você define suas classes C# (entidades) primeiro e o EF Core cria o banco de dados automaticamente baseado nessas classes através de migrations. Por exemplo, você cria uma classe Cliente com propriedades Id, Nome, Email e DataCadastro, e o EF Core gera uma tabela Clientes com colunas correspondentes, inferindo tipos de dados apropriados (string vira NVARCHAR, DateTime vira DATETIME2, etc). Essa abordagem é ideal para projetos novos onde você tem controle total sobre o design do banco de dados e oferece controle completo sobre o modelo de domínio da aplicação. Code First permite usar técnicas avançadas como Domain-Driven Design (DDD), facilita o versionamento do schema através de migrations (que funcionam como um Git para o banco de dados), e mantém o código como fonte única da verdade. No Database First, você tem um banco de dados existente (geralmente legado) e usa ferramentas do EF Core (scaffold-dbcontext) para gerar classes C# automaticamente a partir das tabelas existentes, criando um modelo que espelha a estrutura do banco. Isso é útil ao trabalhar com bancos legados onde você não pode alterar o schema, quando o DBA controla rigidamente a estrutura do banco, ou ao integrar com sistemas existentes. Code First é mais popular em desenvolvimento moderno e ágil porque oferece maior flexibilidade, melhor integração com práticas de desenvolvimento modernas, e facilita testes unitários através de providers in-memory. Neste curso, focaremos principalmente em Code First, que é a abordagem recomendada para novos projetos.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Configurando EF Core",
                    Content = "Para usar EF Core em um projeto ASP.NET Core, você precisa seguir alguns passos essenciais de configuração. Primeiro, instale os pacotes NuGet necessários: Microsoft.EntityFrameworkCore (núcleo do framework), Microsoft.EntityFrameworkCore.SqlServer (provider específico para SQL Server - existem providers para outros bancos), e Microsoft.EntityFrameworkCore.Tools (ferramentas de linha de comando para criar e aplicar migrations). Depois, você cria uma classe DbContext que representa a sessão com o banco de dados e atua como a unidade de trabalho (Unit of Work pattern). O DbContext contém propriedades DbSet<T> para cada entidade que você quer mapear, como DbSet<Produto> Produtos e DbSet<Cliente> Clientes. Cada DbSet representa uma tabela no banco e permite realizar queries e operações CRUD. No Program.cs (ou Startup.cs em versões antigas), você registra o DbContext no container de injeção de dependência usando builder.Services.AddDbContext<MeuDbContext>(options => options.UseSqlServer(connectionString)). A string de conexão é configurada no appsettings.json para facilitar mudanças entre ambientes (desenvolvimento, teste, produção) sem recompilar o código. Com isso configurado, você pode injetar o DbContext em controllers, services e repositories para realizar operações no banco de dados. O EF Core gerencia automaticamente aspectos complexos como abertura e fechamento de conexões (connection pooling), gerenciamento de transações, conversão entre objetos C# e registros do banco (materialização), e rastreamento de mudanças nos objetos para gerar comandos UPDATE eficientes. Essa configuração inicial pode parecer trabalhosa, mas oferece uma base sólida e type-safe para todas as operações de dados da aplicação.",
                    Order = 3
                },
                new TheorySection
                {
                    Heading = "Vantagens do EF Core sobre SQL Direto",
                    Content = "Usar EF Core em vez de escrever SQL diretamente oferece várias vantagens significativas para o desenvolvimento moderno. Primeiro, você ganha type safety (segurança de tipos) - o compilador C# verifica seus queries em tempo de compilação, detectando erros antes da execução, enquanto SQL em strings só falha em runtime. Segundo, EF Core protege automaticamente contra SQL Injection através de queries parametrizadas, eliminando uma das vulnerabilidades mais comuns em aplicações web. Terceiro, você obtém produtividade massiva - operações que exigiriam dezenas de linhas de SQL (com JOINs complexos, paginação, ordenação) podem ser expressas em poucas linhas de LINQ intuitivo. Quarto, EF Core oferece portabilidade entre bancos de dados - trocar de SQL Server para PostgreSQL requer apenas mudar o provider, sem reescrever queries. Quinto, o change tracking automático detecta quais propriedades mudaram e gera UPDATEs otimizados apenas para essas colunas. Sexto, recursos como lazy loading e eager loading permitem controlar quando dados relacionados são carregados, otimizando performance. Sétimo, migrations fornecem versionamento do schema como código, facilitando deploys e rollbacks. Oitavo, EF Core se integra perfeitamente com LINQ, permitindo queries fortemente tipadas e compostas. Por fim, a abstração do ORM facilita testes unitários através de providers in-memory, permitindo testar lógica de dados sem banco real. Essas vantagens tornam EF Core a escolha padrão para a maioria dos projetos .NET modernos.",
                    Order = 4
                }
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Instalando EF Core",
                    Code = @"// Instalar via NuGet Package Manager Console
// Install-Package Microsoft.EntityFrameworkCore
// Install-Package Microsoft.EntityFrameworkCore.SqlServer
// Install-Package Microsoft.EntityFrameworkCore.Tools

// Ou via .NET CLI
// dotnet add package Microsoft.EntityFrameworkCore
// dotnet add package Microsoft.EntityFrameworkCore.SqlServer
// dotnet add package Microsoft.EntityFrameworkCore.Tools",
                    Language = "csharp",
                    Explanation = "Estes são os pacotes essenciais para começar com EF Core. O pacote principal contém as funcionalidades core, o SqlServer é o provider para SQL Server, e Tools fornece comandos para criar migrations e atualizar o banco de dados.",
                    IsRunnable = false
                },
                new CodeExample
                {
                    Title = "Criando DbContext e Entidade",
                    Code = @"using Microsoft.EntityFrameworkCore;

// Entidade - representa uma tabela
public class Produto
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public decimal Preco { get; set; }
    public int Estoque { get; set; }
}

// DbContext - representa a sessão com o banco
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) 
        : base(options)
    {
    }

    public DbSet<Produto> Produtos { get; set; }
}",
                    Language = "csharp",
                    Explanation = "A classe Produto é uma entidade que será mapeada para uma tabela. O AppDbContext herda de DbContext e contém DbSet<Produto> que representa a coleção de produtos no banco. O construtor recebe DbContextOptions para configuração.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Configurando no Program.cs",
                    Code = @"using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Registrar DbContext com SQL Server
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString(""DefaultConnection"")
    )
);

var app = builder.Build();

// appsettings.json:
// {
//   ""ConnectionStrings"": {
//     ""DefaultConnection"": ""Server=localhost;Database=MeuBanco;Trusted_Connection=true;""
//   }
// }",
                    Language = "csharp",
                    Explanation = "Este código registra o DbContext no container de DI e configura a conexão com SQL Server. A string de conexão vem do appsettings.json. Agora você pode injetar AppDbContext em qualquer controller ou service.",
                    IsRunnable = true
                }
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Configurar EF Core em Projeto",
                    Description = "Crie um novo projeto ASP.NET Core Web API, instale os pacotes do EF Core, crie uma entidade Cliente (Id, Nome, Email, Telefone) e configure o DbContext com SQL Server.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"using Microsoft.EntityFrameworkCore;

// 1. Criar projeto: dotnet new webapi -n MeuProjeto
// 2. Instalar pacotes NuGet
// 3. Criar classe Cliente
// 4. Criar AppDbContext
// 5. Configurar no Program.cs",
                    Hints = new List<string> 
                    { 
                        "Use dotnet add package para instalar os pacotes",
                        "A entidade deve ter uma propriedade Id do tipo int",
                        "Não esqueça de adicionar a string de conexão no appsettings.json" 
                    }
                },
                new Exercise
                {
                    Title = "Comparar Code First e Database First",
                    Description = "Pesquise e liste 3 vantagens e 3 desvantagens de cada abordagem (Code First vs Database First). Em que cenários você usaria cada uma?",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"// Code First
// Vantagens:
// 1. 
// 2. 
// 3. 
// Desvantagens:
// 1. 
// 2. 
// 3. 

// Database First
// Vantagens:
// 1. 
// 2. 
// 3. 
// Desvantagens:
// 1. 
// 2. 
// 3. ",
                    Hints = new List<string> 
                    { 
                        "Pense em controle sobre o modelo de dados",
                        "Considere cenários com bancos de dados legados",
                        "Avalie a facilidade de versionamento" 
                    }
                },
                new Exercise
                {
                    Title = "Criar Múltiplas Entidades",
                    Description = "Expanda o projeto anterior criando entidades para Pedido (Id, Data, Total, ClienteId) e ItemPedido (Id, PedidoId, ProdutoId, Quantidade, PrecoUnitario). Configure todas no DbContext.",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = @"using Microsoft.EntityFrameworkCore;

public class Pedido
{
    // Implementar propriedades
}

public class ItemPedido
{
    // Implementar propriedades
}

public class AppDbContext : DbContext
{
    // Adicionar DbSets
}",
                    Hints = new List<string> 
                    { 
                        "Cada entidade precisa de uma propriedade Id",
                        "Use propriedades de navegação para relacionamentos",
                        "Adicione DbSet para cada entidade no DbContext" 
                    }
                },
                new Exercise
                {
                    Title = "Explorar Providers de Banco de Dados",
                    Description = "Pesquise e documente como configurar EF Core com pelo menos 3 bancos de dados diferentes (SQL Server, PostgreSQL, SQLite). Crie um projeto de exemplo que possa trocar entre eles via configuração. Compare as diferenças nas strings de conexão e pacotes NuGet necessários.",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = @"// SQL Server
// Install-Package Microsoft.EntityFrameworkCore.SqlServer
// ConnectionString: Server=localhost;Database=MeuDB;Trusted_Connection=true;

// PostgreSQL
// Install-Package Npgsql.EntityFrameworkCore.PostgreSQL
// ConnectionString: Host=localhost;Database=MeuDB;Username=user;Password=pass;

// SQLite
// Install-Package Microsoft.EntityFrameworkCore.Sqlite
// ConnectionString: Data Source=meudb.db;

// Implemente um método que configure o provider baseado em appsettings.json",
                    Hints = new List<string> 
                    { 
                        "Use um switch ou if/else baseado em uma configuração",
                        "Cada provider tem seu próprio método de extensão (UseSqlServer, UseNpgsql, UseSqlite)",
                        "SQLite é ótimo para desenvolvimento e testes por não precisar de servidor",
                        "Teste a portabilidade criando o mesmo schema em bancos diferentes"
                    }
                }
            },
            Summary = "Nesta aula você aprendeu o que é Entity Framework Core e por que ele é a ferramenta preferida para acesso a dados em .NET moderno. Você compreendeu a diferença fundamental entre usar um ORM como EF Core versus escrever SQL diretamente, entendendo as vantagens de type safety, produtividade, segurança e manutenibilidade. Você explorou as duas abordagens principais - Code First (onde você define classes C# e o EF Core cria o banco) e Database First (onde você gera classes a partir de um banco existente) - e aprendeu quando usar cada uma. Você também configurou o EF Core em um projeto ASP.NET Core do zero, instalando os pacotes necessários, criando um DbContext, registrando-o no container de DI, e configurando a string de conexão. Finalmente, você descobriu as múltiplas vantagens do EF Core sobre SQL direto, incluindo proteção contra SQL Injection, portabilidade entre bancos de dados, e integração perfeita com LINQ. Com esse conhecimento fundamental, você está pronto para começar a trabalhar com bancos de dados de forma produtiva, segura e orientada a objetos, aproveitando todo o poder do ecossistema .NET."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0005-000000000001"),
            CourseId = _courseId,
            Title = "Introdução ao Entity Framework Core",
            Duration = "50 min",
            Difficulty = "Intermediário",
            EstimatedMinutes = 50,
            Prerequisites = "[]",
            OrderIndex = 1,
            Content = "",
            StructuredContent = JsonSerializer.Serialize(content),
            Version = 1,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }

    private Lesson CreateLesson2()
    {
        var content = new LessonContent
        {
            Objectives = new List<string>
            {
                "Compreender o conceito de migrations no EF Core",
                "Aprender a criar e aplicar migrations",
                "Dominar comandos de migration via CLI",
                "Entender como reverter e gerenciar migrations"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "O que são Migrations?",
                    Content = "Migrations são uma forma de versionar o schema do banco de dados junto com o código da aplicação. Quando você cria ou modifica entidades no Code First, o banco de dados precisa ser atualizado para refletir essas mudanças. Migrations geram automaticamente scripts SQL que aplicam essas alterações de forma incremental e controlada. Por exemplo, se você adiciona uma propriedade DataNascimento à classe Cliente, uma migration cria o comando ALTER TABLE para adicionar essa coluna. Cada migration tem um timestamp e um nome descritivo, formando um histórico completo de todas as mudanças no schema. Isso permite que múltiplos desenvolvedores trabalhem no mesmo projeto sem conflitos de banco de dados, facilita deploys em diferentes ambientes (dev, staging, produção) e possibilita reverter mudanças se necessário. Migrations são essenciais para manter o banco de dados sincronizado com o modelo de domínio ao longo do tempo.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Criando e Aplicando Migrations",
                    Content = "Para criar uma migration, use o comando dotnet ef migrations add NomeDaMigration. O EF Core compara o modelo atual com o último snapshot e gera uma classe de migration com métodos Up() (aplicar mudanças) e Down() (reverter mudanças). A migration contém código C# que usa o MigrationBuilder para criar tabelas, adicionar colunas, criar índices, etc. Depois de criar a migration, você aplica ao banco com dotnet ef database update. Isso executa todas as migrations pendentes em ordem cronológica. O EF Core mantém uma tabela __EFMigrationsHistory no banco para rastrear quais migrations já foram aplicadas. Se você trabalha em equipe, é importante sempre criar migrations antes de modificar o banco manualmente e commitar as migrations no controle de versão. Isso garante que todos os desenvolvedores e ambientes tenham o mesmo schema.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Gerenciando Migrations",
                    Content = "Você pode listar todas as migrations com dotnet ef migrations list. Para reverter a última migration aplicada, use dotnet ef database update NomeDaMigrationAnterior. Para remover a última migration não aplicada, use dotnet ef migrations remove. Em produção, é recomendado gerar scripts SQL das migrations com dotnet ef migrations script e revisá-los antes de aplicar. Isso permite que DBAs revisem as mudanças e as executem em janelas de manutenção. Você também pode aplicar migrations programaticamente no startup da aplicação usando context.Database.Migrate(), útil para ambientes containerizados. Evite modificar migrations já aplicadas em produção - sempre crie uma nova migration para corrigir problemas. Se você precisa resetar o banco completamente em desenvolvimento, use dotnet ef database drop seguido de dotnet ef database update.",
                    Order = 3
                }
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Criando a Primeira Migration",
                    Code = @"// 1. Criar migration inicial
// dotnet ef migrations add InitialCreate

// 2. Aplicar ao banco de dados
// dotnet ef database update

// A migration gerada terá algo como:
public partial class InitialCreate : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: ""Produtos"",
            columns: table => new
            {
                Id = table.Column<int>(nullable: false)
                    .Annotation(""SqlServer:Identity"", ""1, 1""),
                Nome = table.Column<string>(nullable: true),
                Preco = table.Column<decimal>(nullable: false),
                Estoque = table.Column<int>(nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey(""PK_Produtos"", x => x.Id);
            });
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(name: ""Produtos"");
    }
}",
                    Language = "csharp",
                    Explanation = "Este exemplo mostra como criar a migration inicial que cria a tabela Produtos. O método Up() cria a tabela e o Down() a remove. O EF Core gera esse código automaticamente baseado nas suas entidades.",
                    IsRunnable = false
                },
                new CodeExample
                {
                    Title = "Adicionando Nova Coluna",
                    Code = @"// 1. Modificar a entidade
public class Produto
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public decimal Preco { get; set; }
    public int Estoque { get; set; }
    public string Descricao { get; set; } // Nova propriedade
}

// 2. Criar migration
// dotnet ef migrations add AdicionarDescricaoProduto

// 3. Aplicar ao banco
// dotnet ef database update

// Migration gerada:
protected override void Up(MigrationBuilder migrationBuilder)
{
    migrationBuilder.AddColumn<string>(
        name: ""Descricao"",
        table: ""Produtos"",
        nullable: true);
}",
                    Language = "csharp",
                    Explanation = "Quando você adiciona uma propriedade à entidade e cria uma migration, o EF Core detecta a mudança e gera o comando para adicionar a coluna. Isso mantém o banco sincronizado com o código.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Aplicando Migrations no Startup",
                    Code = @"using Microsoft.EntityFrameworkCore;

// No Program.cs
var app = builder.Build();

// Aplicar migrations automaticamente ao iniciar
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    context.Database.Migrate(); // Aplica migrations pendentes
}

app.Run();",
                    Language = "csharp",
                    Explanation = "Este código aplica automaticamente todas as migrations pendentes quando a aplicação inicia. Útil em ambientes containerizados ou para simplificar deploys, mas use com cuidado em produção.",
                    IsRunnable = true
                }
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Criar Migration Inicial",
                    Description = "No projeto criado na aula anterior, crie a migration inicial e aplique ao banco de dados. Verifique se as tabelas foram criadas corretamente usando SQL Server Management Studio ou Azure Data Studio.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"// Comandos a executar:
// 1. dotnet ef migrations add InitialCreate
// 2. dotnet ef database update
// 3. Verificar tabelas no banco",
                    Hints = new List<string> 
                    { 
                        "Certifique-se de que a string de conexão está correta",
                        "O comando deve ser executado na pasta do projeto",
                        "Verifique se os pacotes EF Core Tools estão instalados" 
                    }
                },
                new Exercise
                {
                    Title = "Adicionar e Remover Coluna",
                    Description = "Adicione uma propriedade Ativo (bool) à entidade Cliente, crie uma migration e aplique. Depois, remova a propriedade, crie outra migration e aplique. Observe o código gerado em cada migration.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"// Passo 1: Adicionar propriedade
public class Cliente
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public bool Ativo { get; set; } // Adicionar
}

// Passo 2: Criar migration
// dotnet ef migrations add AdicionarAtivoCliente

// Passo 3: Aplicar
// dotnet ef database update

// Passo 4: Remover propriedade e repetir",
                    Hints = new List<string> 
                    { 
                        "Use nomes descritivos para as migrations",
                        "Observe os métodos Up() e Down() gerados",
                        "Você pode reverter com dotnet ef database update NomeMigrationAnterior" 
                    }
                },
                new Exercise
                {
                    Title = "Gerar Script SQL",
                    Description = "Crie uma migration que adiciona uma tabela Categoria (Id, Nome, Descricao) e gere o script SQL correspondente sem aplicar ao banco. Analise o SQL gerado.",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = @"using Microsoft.EntityFrameworkCore;

// 1. Criar entidade Categoria
public class Categoria
{
    // Implementar
}

// 2. Adicionar DbSet no DbContext
public DbSet<Categoria> Categorias { get; set; }

// 3. Criar migration
// dotnet ef migrations add AdicionarCategoria

// 4. Gerar script SQL
// dotnet ef migrations script > migration.sql",
                    Hints = new List<string> 
                    { 
                        "O comando script gera SQL sem aplicar ao banco",
                        "Você pode especificar um range de migrations",
                        "Útil para revisar mudanças antes de aplicar em produção" 
                    }
                }
            },
            Summary = "Nesta aula você aprendeu sobre migrations, como criar e aplicar mudanças no schema do banco de dados de forma versionada e controlada. Migrations são essenciais para manter o banco sincronizado com o código e trabalhar em equipe de forma eficiente."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0005-000000000002"),
            CourseId = _courseId,
            Title = "Migrations - Versionando o Banco de Dados",
            Duration = "45 min",
            Difficulty = "Intermediário",
            EstimatedMinutes = 45,
            Prerequisites = JsonSerializer.Serialize(new[] { "10000000-0000-0000-0005-000000000001" }),
            OrderIndex = 2,
            Content = "",
            StructuredContent = JsonSerializer.Serialize(content),
            Version = 1,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }

    private Lesson CreateLesson3()
    {
        var content = new LessonContent
        {
            Objectives = new List<string>
            {
                "Compreender o papel do DbContext no EF Core",
                "Aprender a configurar entidades usando Fluent API",
                "Dominar o método OnModelCreating",
                "Entender o ciclo de vida do DbContext"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "O Papel do DbContext",
                    Content = "O DbContext é o coração do Entity Framework Core. Ele representa uma sessão com o banco de dados e coordena todas as operações de leitura e escrita. O DbContext mantém um rastreamento de mudanças (change tracking) de todas as entidades carregadas, detectando automaticamente quais objetos foram modificados, adicionados ou removidos. Quando você chama SaveChanges(), o DbContext gera os comandos SQL necessários para persistir essas mudanças. O DbContext também gerencia conexões com o banco, abrindo e fechando automaticamente conforme necessário. Ele implementa o padrão Unit of Work, agrupando múltiplas operações em uma única transação. É importante entender que o DbContext não é thread-safe e deve ter um escopo limitado - geralmente uma instância por requisição web. Criar um DbContext é relativamente barato, então não há problema em criar e descartar frequentemente.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Configurando Entidades com Fluent API",
                    Content = "Existem duas formas de configurar entidades no EF Core: Data Annotations (atributos nas classes) e Fluent API (código no OnModelCreating). A Fluent API é mais poderosa e flexível, permitindo configurações que não são possíveis com atributos. Por exemplo, você pode definir chaves compostas, índices únicos, valores padrão, conversões de tipo e muito mais. A Fluent API também mantém suas entidades limpas, sem poluí-las com atributos de infraestrutura. Use modelBuilder.Entity<Produto>() para configurar uma entidade específica. Você pode definir o nome da tabela com ToTable(), configurar propriedades com Property(), definir relacionamentos com HasOne/HasMany, e criar índices com HasIndex(). A Fluent API é executada uma vez quando o modelo é criado e cached, então não há impacto em performance. É a abordagem recomendada para configurações complexas.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Ciclo de Vida e Boas Práticas",
                    Content = "O DbContext deve ter um ciclo de vida curto. Em aplicações web ASP.NET Core, use AddDbContext() para registrar o DbContext com escopo por requisição - uma nova instância é criada para cada requisição HTTP e descartada ao final. Nunca compartilhe uma instância de DbContext entre threads ou requisições. O DbContext mantém um cache de primeiro nível (identity map) que garante que múltiplas queries para a mesma entidade retornem a mesma instância de objeto. Isso evita inconsistências mas pode causar problemas de memória em operações de longa duração. Para queries somente leitura, use AsNoTracking() para desabilitar o change tracking e melhorar performance. Sempre use using ou injete via DI para garantir que o DbContext seja descartado corretamente, liberando conexões e recursos. Em cenários de alta concorrência, considere usar connection pooling que já vem habilitado por padrão.",
                    Order = 3
                }
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Configurando Entidades com Fluent API",
                    Code = @"using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public DbSet<Produto> Produtos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Produto>(entity =>
        {
            // Nome da tabela
            entity.ToTable(""Produtos"");

            // Chave primária
            entity.HasKey(p => p.Id);

            // Configurar propriedades
            entity.Property(p => p.Nome)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(p => p.Preco)
                .HasColumnType(""decimal(18,2)"");

            // Índice único
            entity.HasIndex(p => p.Nome)
                .IsUnique();

            // Valor padrão
            entity.Property(p => p.Estoque)
                .HasDefaultValue(0);
        });
    }
}",
                    Language = "csharp",
                    Explanation = "Este exemplo mostra como usar Fluent API para configurar uma entidade. Definimos o nome da tabela, chave primária, restrições de propriedades, índices e valores padrão. Isso oferece controle total sobre o mapeamento.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Usando DbContext Corretamente",
                    Code = @"using Microsoft.EntityFrameworkCore;

// ❌ Incorreto - DbContext compartilhado
public class ProdutoService
{
    private readonly AppDbContext _context; // Perigoso!

    public ProdutoService(AppDbContext context)
    {
        _context = context; // Não faça isso em singletons
    }
}

// ✅ Correto - DbContext com escopo
public class ProdutoService
{
    private readonly IDbContextFactory<AppDbContext> _factory;

    public ProdutoService(IDbContextFactory<AppDbContext> factory)
    {
        _factory = factory;
    }

    public async Task<Produto> ObterProdutoAsync(int id)
    {
        using var context = await _factory.CreateDbContextAsync();
        return await context.Produtos.FindAsync(id);
    }
}",
                    Language = "csharp",
                    Explanation = "O primeiro exemplo mostra um erro comum: manter uma referência ao DbContext em um serviço singleton. O segundo exemplo usa IDbContextFactory para criar instâncias sob demanda, garantindo ciclo de vida correto.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Queries com e sem Tracking",
                    Code = @"// Com tracking (padrão) - para operações de escrita
var produto = await context.Produtos.FindAsync(1);
produto.Preco = 99.90m;
await context.SaveChanges(); // Detecta mudança automaticamente

// Sem tracking - para queries somente leitura
var produtos = await context.Produtos
    .AsNoTracking()
    .Where(p => p.Estoque > 0)
    .ToListAsync();

// Sem tracking é mais rápido e usa menos memória
// Use quando não for modificar os objetos",
                    Language = "csharp",
                    Explanation = "AsNoTracking() desabilita o change tracking, melhorando performance em queries somente leitura. Use tracking apenas quando for modificar e salvar entidades.",
                    IsRunnable = true
                }
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Configurar Entidade com Fluent API",
                    Description = "Configure a entidade Cliente usando Fluent API: Nome obrigatório com máximo 200 caracteres, Email obrigatório e único, Telefone opcional com máximo 20 caracteres, e DataCadastro com valor padrão GETDATE().",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Cliente>(entity =>
    {
        // Implementar configurações aqui
    });
}",
                    Hints = new List<string> 
                    { 
                        "Use Property() para configurar cada propriedade",
                        "IsRequired() torna a propriedade obrigatória",
                        "HasIndex().IsUnique() cria índice único" 
                    }
                },
                new Exercise
                {
                    Title = "Implementar Repository Pattern",
                    Description = "Crie uma classe ProdutoRepository que recebe AppDbContext via construtor e implementa métodos ObterTodos(), ObterPorId(int id), Adicionar(Produto produto), Atualizar(Produto produto) e Remover(int id).",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"using Microsoft.EntityFrameworkCore;

public class ProdutoRepository
{
    private readonly AppDbContext _context;

    public ProdutoRepository(AppDbContext context)
    {
        _context = context;
    }

    // Implementar métodos
}",
                    Hints = new List<string> 
                    { 
                        "Use _context.Produtos para acessar o DbSet",
                        "Não esqueça de chamar SaveChanges() após modificações",
                        "Use async/await para operações assíncronas" 
                    }
                },
                new Exercise
                {
                    Title = "Comparar Performance com e sem Tracking",
                    Description = "Crie um teste que carrega 1000 produtos com tracking e sem tracking. Meça o tempo e uso de memória de cada abordagem usando Stopwatch e GC.GetTotalMemory(). Analise os resultados.",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = @"using System.Diagnostics;

// Teste com tracking
var sw1 = Stopwatch.StartNew();
var mem1 = GC.GetTotalMemory(true);
var comTracking = await context.Produtos.ToListAsync();
sw1.Stop();
var mem2 = GC.GetTotalMemory(false);

// Teste sem tracking
// Implementar

Console.WriteLine($""Com tracking: {sw1.ElapsedMilliseconds}ms, Memória: {mem2 - mem1} bytes"");",
                    Hints = new List<string> 
                    { 
                        "Use AsNoTracking() para desabilitar tracking",
                        "Crie produtos de teste antes de medir",
                        "Execute múltiplas vezes para obter média confiável" 
                    }
                }
            },
            Summary = "Nesta aula você aprendeu sobre o DbContext, como configurar entidades usando Fluent API, e boas práticas para gerenciar o ciclo de vida do DbContext. Compreender esses conceitos é fundamental para usar o EF Core de forma eficiente e evitar problemas comuns."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0005-000000000003"),
            CourseId = _courseId,
            Title = "DbContext e Configuração de Entidades",
            Duration = "50 min",
            Difficulty = "Intermediário",
            EstimatedMinutes = 50,
            Prerequisites = JsonSerializer.Serialize(new[] { "10000000-0000-0000-0005-000000000002" }),
            OrderIndex = 3,
            Content = "",
            StructuredContent = JsonSerializer.Serialize(content),
            Version = 1,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }
}
