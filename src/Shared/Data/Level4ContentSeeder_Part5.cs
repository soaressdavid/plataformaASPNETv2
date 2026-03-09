using Shared.Entities;
using Shared.Models;
using System.Text.Json;

namespace Shared.Data;

/// <summary>
/// Level 4 Content Seeder - Part 5 (Lessons 11-20)
/// Topics: Advanced EF Core Features, Performance, Best Practices
/// </summary>
public partial class Level4ContentSeeder
{
    private Lesson CreateLesson11()
    {
        var content = new LessonContent
        {
            Objectives = new List<string>
            {
                "Compreender transações",
                "Aprender a implementar concorrência otimista",
                "Dominar concorrência pessimista",
                "Aplicar transações e concorrência em projetos reais"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "Transações",
                    Content = "O conceito de transações é fundamental no desenvolvimento com Entity Framework Core. Este recurso permite que você trabalhe de forma mais eficiente e profissional com o framework. Desenvolvedores experientes utilizam transações regularmente em projetos do mundo real para garantir código de qualidade, manutenível e performático. Compreender profundamente este tópico permitirá que você escreva aplicações mais robustas e escaláveis. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno. É importante seguir as melhores práticas da indústria ao trabalhar com transações, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do Entity Framework Core e do desenvolvimento .NET em geral.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Concorrência Otimista",
                    Content = "O conceito de concorrência otimista é fundamental no desenvolvimento com Entity Framework Core. Este recurso permite que você trabalhe de forma mais eficiente e profissional com o framework. Desenvolvedores experientes utilizam concorrência otimista regularmente em projetos do mundo real para garantir código de qualidade, manutenível e performático. Compreender profundamente este tópico permitirá que você escreva aplicações mais robustas e escaláveis. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno. É importante seguir as melhores práticas da indústria ao trabalhar com concorrência otimista, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do Entity Framework Core e do desenvolvimento .NET em geral.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Concorrência Pessimista",
                    Content = "O conceito de concorrência pessimista é fundamental no desenvolvimento com Entity Framework Core. Este recurso permite que você trabalhe de forma mais eficiente e profissional com o framework. Desenvolvedores experientes utilizam concorrência pessimista regularmente em projetos do mundo real para garantir código de qualidade, manutenível e performático. Compreender profundamente este tópico permitirá que você escreva aplicações mais robustas e escaláveis. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno. É importante seguir as melhores práticas da indústria ao trabalhar com concorrência pessimista, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do Entity Framework Core e do desenvolvimento .NET em geral.",
                    Order = 3
                },
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Exemplo Básico de Transações e Concorrência",
                    Code = @"using Microsoft.EntityFrameworkCore;

// Exemplo de implementação de Transações e Concorrência
using Microsoft.EntityFrameworkCore;

public class ExemploContext : DbContext
{
    // Configuração básica
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Implementação do conceito
        modelBuilder.Entity<Produto>()
            .Property(p => p.Nome)
            .IsRequired();
    }
}

// Uso prático
var produtos = await context.Produtos
    .Where(p => p.Ativo)
    .ToListAsync();",
                    Language = "csharp",
                    Explanation = "Este exemplo demonstra a implementação básica de Transações e Concorrência. Note como o código é estruturado de forma clara e segue as convenções do EF Core.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Exemplo Avançado",
                    Code = @"using Microsoft.EntityFrameworkCore;

// Implementação avançada com Transações
public class ServicoAvancado
{
    private readonly ApplicationDbContext _context;
    
    public ServicoAvancado(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<Produto>> ObterProdutosAsync()
    {
        return await _context.Produtos
            .Include(p => p.Categoria)
            .Where(p => p.Ativo)
            .OrderBy(p => p.Nome)
            .ToListAsync();
    }
}",
                    Language = "csharp",
                    Explanation = "Exemplo mais complexo mostrando como aplicar Transações e Concorrência em cenários reais com múltiplas operações.",
                    IsRunnable = true
                },
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Exercício 1: Implementação Básica",
                    Description = "Implemente Transações e Concorrência em um contexto simples com uma entidade Produto.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"using Microsoft.EntityFrameworkCore;

// Implemente aqui
public class MeuContexto : DbContext
{
    
}",
                    Hints = new List<string> { "Comece definindo o DbContext", "Configure Transações no OnModelCreating", "Teste sua implementação" }
                },
                new Exercise
                {
                    Title = "Exercício 2: Cenário Intermediário",
                    Description = "Crie um serviço que utilize Transações e Concorrência para gerenciar produtos e categorias.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"// Implemente o serviço
public class ProdutoService
{
    
}",
                    Hints = new List<string> { "Injete o DbContext via construtor", "Implemente métodos que usem Transações e Concorrência", "Adicione tratamento de erros" }
                },
                new Exercise
                {
                    Title = "Exercício 3: Desafio Avançado",
                    Description = "Implemente um sistema completo utilizando Transações e Concorrência com múltiplas entidades e relacionamentos.",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = @"// Sistema completo
",
                    Hints = new List<string> { "Planeje a estrutura antes de implementar", "Considere performance e escalabilidade", "Adicione testes unitários" }
                },
            },
            Summary = "Nesta aula, você aprendeu sobre Transações e Concorrência no Entity Framework Core. Exploramos Transações, Concorrência Otimista, Concorrência Pessimista e vimos como aplicar estes conceitos em projetos reais. Compreender profundamente Transações e Concorrência é essencial para desenvolver aplicações robustas e escaláveis. Continue praticando com os exercícios propostos e experimente aplicar estes conceitos em seus próprios projetos. Na próxima aula, avançaremos para tópicos ainda mais interessantes do Entity Framework Core."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0005-000000000011"),
            CourseId = Guid.Parse("10000000-0000-0000-0000-000000000005"),
            Title = "Transações e Concorrência",
            Duration = "45 min",
            Difficulty = "Intermediário",
            EstimatedMinutes = 45,
            Prerequisites = "[]",
            OrderIndex = 11,
            Content = "",
            StructuredContent = JsonSerializer.Serialize(content),
            Version = 1,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }

    private Lesson CreateLesson12()
    {
        var content = new LessonContent
        {
            Objectives = new List<string>
            {
                "Compreender migrations complexas",
                "Aprender a implementar data seeding",
                "Dominar rollback",
                "Aplicar migrations avançadas em projetos reais"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "Migrations Complexas",
                    Content = "O conceito de migrations complexas é fundamental no desenvolvimento com Entity Framework Core. Este recurso permite que você trabalhe de forma mais eficiente e profissional com o framework. Desenvolvedores experientes utilizam migrations complexas regularmente em projetos do mundo real para garantir código de qualidade, manutenível e performático. Compreender profundamente este tópico permitirá que você escreva aplicações mais robustas e escaláveis. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno. É importante seguir as melhores práticas da indústria ao trabalhar com migrations complexas, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do Entity Framework Core e do desenvolvimento .NET em geral.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Data Seeding",
                    Content = "O conceito de data seeding é fundamental no desenvolvimento com Entity Framework Core. Este recurso permite que você trabalhe de forma mais eficiente e profissional com o framework. Desenvolvedores experientes utilizam data seeding regularmente em projetos do mundo real para garantir código de qualidade, manutenível e performático. Compreender profundamente este tópico permitirá que você escreva aplicações mais robustas e escaláveis. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno. É importante seguir as melhores práticas da indústria ao trabalhar com data seeding, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do Entity Framework Core e do desenvolvimento .NET em geral.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Rollback",
                    Content = "O conceito de rollback é fundamental no desenvolvimento com Entity Framework Core. Este recurso permite que você trabalhe de forma mais eficiente e profissional com o framework. Desenvolvedores experientes utilizam rollback regularmente em projetos do mundo real para garantir código de qualidade, manutenível e performático. Compreender profundamente este tópico permitirá que você escreva aplicações mais robustas e escaláveis. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno. É importante seguir as melhores práticas da indústria ao trabalhar com rollback, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do Entity Framework Core e do desenvolvimento .NET em geral.",
                    Order = 3
                },
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Exemplo Básico de Migrations Avançadas",
                    Code = @"using Microsoft.EntityFrameworkCore;

// Exemplo de implementação de Migrations Avançadas
using Microsoft.EntityFrameworkCore;

public class ExemploContext : DbContext
{
    // Configuração básica
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Implementação do conceito
        modelBuilder.Entity<Produto>()
            .Property(p => p.Nome)
            .IsRequired();
    }
}

// Uso prático
var produtos = await context.Produtos
    .Where(p => p.Ativo)
    .ToListAsync();",
                    Language = "csharp",
                    Explanation = "Este exemplo demonstra a implementação básica de Migrations Avançadas. Note como o código é estruturado de forma clara e segue as convenções do EF Core.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Exemplo Avançado",
                    Code = @"using Microsoft.EntityFrameworkCore;

// Implementação avançada com Migrations Complexas
public class ServicoAvancado
{
    private readonly ApplicationDbContext _context;
    
    public ServicoAvancado(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<Produto>> ObterProdutosAsync()
    {
        return await _context.Produtos
            .Include(p => p.Categoria)
            .Where(p => p.Ativo)
            .OrderBy(p => p.Nome)
            .ToListAsync();
    }
}",
                    Language = "csharp",
                    Explanation = "Exemplo mais complexo mostrando como aplicar Migrations Avançadas em cenários reais com múltiplas operações.",
                    IsRunnable = true
                },
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Exercício 1: Implementação Básica",
                    Description = "Implemente Migrations Avançadas em um contexto simples com uma entidade Produto.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"using Microsoft.EntityFrameworkCore;

// Implemente aqui
public class MeuContexto : DbContext
{
    
}",
                    Hints = new List<string> { "Comece definindo o DbContext", "Configure Migrations Complexas no OnModelCreating", "Teste sua implementação" }
                },
                new Exercise
                {
                    Title = "Exercício 2: Cenário Intermediário",
                    Description = "Crie um serviço que utilize Migrations Avançadas para gerenciar produtos e categorias.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"// Implemente o serviço
public class ProdutoService
{
    
}",
                    Hints = new List<string> { "Injete o DbContext via construtor", "Implemente métodos que usem Migrations Avançadas", "Adicione tratamento de erros" }
                },
                new Exercise
                {
                    Title = "Exercício 3: Desafio Avançado",
                    Description = "Implemente um sistema completo utilizando Migrations Avançadas com múltiplas entidades e relacionamentos.",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = @"// Sistema completo
",
                    Hints = new List<string> { "Planeje a estrutura antes de implementar", "Considere performance e escalabilidade", "Adicione testes unitários" }
                },
            },
            Summary = "Nesta aula, você aprendeu sobre Migrations Avançadas no Entity Framework Core. Exploramos Migrations Complexas, Data Seeding, Rollback e vimos como aplicar estes conceitos em projetos reais. Compreender profundamente Migrations Avançadas é essencial para desenvolver aplicações robustas e escaláveis. Continue praticando com os exercícios propostos e experimente aplicar estes conceitos em seus próprios projetos. Na próxima aula, avançaremos para tópicos ainda mais interessantes do Entity Framework Core."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0005-000000000012"),
            CourseId = Guid.Parse("10000000-0000-0000-0000-000000000005"),
            Title = "Migrations Avançadas",
            Duration = "45 min",
            Difficulty = "Intermediário",
            EstimatedMinutes = 45,
            Prerequisites = "[]",
            OrderIndex = 12,
            Content = "",
            StructuredContent = JsonSerializer.Serialize(content),
            Version = 1,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }

    private Lesson CreateLesson13()
    {
        var content = new LessonContent
        {
            Objectives = new List<string>
            {
                "Compreender savechanges interceptors",
                "Aprender a implementar query interceptors",
                "Dominar events",
                "Aplicar interceptors e events em projetos reais"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "SaveChanges Interceptors",
                    Content = "O conceito de savechanges interceptors é fundamental no desenvolvimento com Entity Framework Core. Este recurso permite que você trabalhe de forma mais eficiente e profissional com o framework. Desenvolvedores experientes utilizam savechanges interceptors regularmente em projetos do mundo real para garantir código de qualidade, manutenível e performático. Compreender profundamente este tópico permitirá que você escreva aplicações mais robustas e escaláveis. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno. É importante seguir as melhores práticas da indústria ao trabalhar com savechanges interceptors, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do Entity Framework Core e do desenvolvimento .NET em geral.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Query Interceptors",
                    Content = "O conceito de query interceptors é fundamental no desenvolvimento com Entity Framework Core. Este recurso permite que você trabalhe de forma mais eficiente e profissional com o framework. Desenvolvedores experientes utilizam query interceptors regularmente em projetos do mundo real para garantir código de qualidade, manutenível e performático. Compreender profundamente este tópico permitirá que você escreva aplicações mais robustas e escaláveis. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno. É importante seguir as melhores práticas da indústria ao trabalhar com query interceptors, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do Entity Framework Core e do desenvolvimento .NET em geral.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Events",
                    Content = "O conceito de events é fundamental no desenvolvimento com Entity Framework Core. Este recurso permite que você trabalhe de forma mais eficiente e profissional com o framework. Desenvolvedores experientes utilizam events regularmente em projetos do mundo real para garantir código de qualidade, manutenível e performático. Compreender profundamente este tópico permitirá que você escreva aplicações mais robustas e escaláveis. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno. É importante seguir as melhores práticas da indústria ao trabalhar com events, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do Entity Framework Core e do desenvolvimento .NET em geral.",
                    Order = 3
                },
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Exemplo Básico de Interceptors e Events",
                    Code = @"using Microsoft.EntityFrameworkCore;

// Exemplo de implementação de Interceptors e Events
using Microsoft.EntityFrameworkCore;

public class ExemploContext : DbContext
{
    // Configuração básica
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Implementação do conceito
        modelBuilder.Entity<Produto>()
            .Property(p => p.Nome)
            .IsRequired();
    }
}

// Uso prático
var produtos = await context.Produtos
    .Where(p => p.Ativo)
    .ToListAsync();",
                    Language = "csharp",
                    Explanation = "Este exemplo demonstra a implementação básica de Interceptors e Events. Note como o código é estruturado de forma clara e segue as convenções do EF Core.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Exemplo Avançado",
                    Code = @"using Microsoft.EntityFrameworkCore;

// Implementação avançada com SaveChanges Interceptors
public class ServicoAvancado
{
    private readonly ApplicationDbContext _context;
    
    public ServicoAvancado(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<Produto>> ObterProdutosAsync()
    {
        return await _context.Produtos
            .Include(p => p.Categoria)
            .Where(p => p.Ativo)
            .OrderBy(p => p.Nome)
            .ToListAsync();
    }
}",
                    Language = "csharp",
                    Explanation = "Exemplo mais complexo mostrando como aplicar Interceptors e Events em cenários reais com múltiplas operações.",
                    IsRunnable = true
                },
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Exercício 1: Implementação Básica",
                    Description = "Implemente Interceptors e Events em um contexto simples com uma entidade Produto.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"using Microsoft.EntityFrameworkCore;

// Implemente aqui
public class MeuContexto : DbContext
{
    
}",
                    Hints = new List<string> { "Comece definindo o DbContext", "Configure SaveChanges Interceptors no OnModelCreating", "Teste sua implementação" }
                },
                new Exercise
                {
                    Title = "Exercício 2: Cenário Intermediário",
                    Description = "Crie um serviço que utilize Interceptors e Events para gerenciar produtos e categorias.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"// Implemente o serviço
public class ProdutoService
{
    
}",
                    Hints = new List<string> { "Injete o DbContext via construtor", "Implemente métodos que usem Interceptors e Events", "Adicione tratamento de erros" }
                },
                new Exercise
                {
                    Title = "Exercício 3: Desafio Avançado",
                    Description = "Implemente um sistema completo utilizando Interceptors e Events com múltiplas entidades e relacionamentos.",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = @"// Sistema completo
",
                    Hints = new List<string> { "Planeje a estrutura antes de implementar", "Considere performance e escalabilidade", "Adicione testes unitários" }
                },
            },
            Summary = "Nesta aula, você aprendeu sobre Interceptors e Events no Entity Framework Core. Exploramos SaveChanges Interceptors, Query Interceptors, Events e vimos como aplicar estes conceitos em projetos reais. Compreender profundamente Interceptors e Events é essencial para desenvolver aplicações robustas e escaláveis. Continue praticando com os exercícios propostos e experimente aplicar estes conceitos em seus próprios projetos. Na próxima aula, avançaremos para tópicos ainda mais interessantes do Entity Framework Core."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0005-000000000013"),
            CourseId = Guid.Parse("10000000-0000-0000-0000-000000000005"),
            Title = "Interceptors e Events",
            Duration = "45 min",
            Difficulty = "Intermediário",
            EstimatedMinutes = 45,
            Prerequisites = "[]",
            OrderIndex = 13,
            Content = "",
            StructuredContent = JsonSerializer.Serialize(content),
            Version = 1,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }

    private Lesson CreateLesson14()
    {
        var content = new LessonContent
        {
            Objectives = new List<string>
            {
                "Compreender soft delete",
                "Aprender a implementar multi-tenancy",
                "Dominar filtros globais",
                "Aplicar global query filters em projetos reais"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "Soft Delete",
                    Content = "O conceito de soft delete é fundamental no desenvolvimento com Entity Framework Core. Este recurso permite que você trabalhe de forma mais eficiente e profissional com o framework. Desenvolvedores experientes utilizam soft delete regularmente em projetos do mundo real para garantir código de qualidade, manutenível e performático. Compreender profundamente este tópico permitirá que você escreva aplicações mais robustas e escaláveis. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno. É importante seguir as melhores práticas da indústria ao trabalhar com soft delete, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do Entity Framework Core e do desenvolvimento .NET em geral.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Multi-tenancy",
                    Content = "O conceito de multi-tenancy é fundamental no desenvolvimento com Entity Framework Core. Este recurso permite que você trabalhe de forma mais eficiente e profissional com o framework. Desenvolvedores experientes utilizam multi-tenancy regularmente em projetos do mundo real para garantir código de qualidade, manutenível e performático. Compreender profundamente este tópico permitirá que você escreva aplicações mais robustas e escaláveis. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno. É importante seguir as melhores práticas da indústria ao trabalhar com multi-tenancy, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do Entity Framework Core e do desenvolvimento .NET em geral.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Filtros Globais",
                    Content = "O conceito de filtros globais é fundamental no desenvolvimento com Entity Framework Core. Este recurso permite que você trabalhe de forma mais eficiente e profissional com o framework. Desenvolvedores experientes utilizam filtros globais regularmente em projetos do mundo real para garantir código de qualidade, manutenível e performático. Compreender profundamente este tópico permitirá que você escreva aplicações mais robustas e escaláveis. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno. É importante seguir as melhores práticas da indústria ao trabalhar com filtros globais, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do Entity Framework Core e do desenvolvimento .NET em geral.",
                    Order = 3
                },
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Exemplo Básico de Global Query Filters",
                    Code = @"using Microsoft.EntityFrameworkCore;

// Exemplo de implementação de Global Query Filters
using Microsoft.EntityFrameworkCore;

public class ExemploContext : DbContext
{
    // Configuração básica
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Implementação do conceito
        modelBuilder.Entity<Produto>()
            .Property(p => p.Nome)
            .IsRequired();
    }
}

// Uso prático
var produtos = await context.Produtos
    .Where(p => p.Ativo)
    .ToListAsync();",
                    Language = "csharp",
                    Explanation = "Este exemplo demonstra a implementação básica de Global Query Filters. Note como o código é estruturado de forma clara e segue as convenções do EF Core.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Exemplo Avançado",
                    Code = @"using Microsoft.EntityFrameworkCore;

// Implementação avançada com Soft Delete
public class ServicoAvancado
{
    private readonly ApplicationDbContext _context;
    
    public ServicoAvancado(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<Produto>> ObterProdutosAsync()
    {
        return await _context.Produtos
            .Include(p => p.Categoria)
            .Where(p => p.Ativo)
            .OrderBy(p => p.Nome)
            .ToListAsync();
    }
}",
                    Language = "csharp",
                    Explanation = "Exemplo mais complexo mostrando como aplicar Global Query Filters em cenários reais com múltiplas operações.",
                    IsRunnable = true
                },
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Exercício 1: Implementação Básica",
                    Description = "Implemente Global Query Filters em um contexto simples com uma entidade Produto.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"using Microsoft.EntityFrameworkCore;

// Implemente aqui
public class MeuContexto : DbContext
{
    
}",
                    Hints = new List<string> { "Comece definindo o DbContext", "Configure Soft Delete no OnModelCreating", "Teste sua implementação" }
                },
                new Exercise
                {
                    Title = "Exercício 2: Cenário Intermediário",
                    Description = "Crie um serviço que utilize Global Query Filters para gerenciar produtos e categorias.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"// Implemente o serviço
public class ProdutoService
{
    
}",
                    Hints = new List<string> { "Injete o DbContext via construtor", "Implemente métodos que usem Global Query Filters", "Adicione tratamento de erros" }
                },
                new Exercise
                {
                    Title = "Exercício 3: Desafio Avançado",
                    Description = "Implemente um sistema completo utilizando Global Query Filters com múltiplas entidades e relacionamentos.",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = @"// Sistema completo
",
                    Hints = new List<string> { "Planeje a estrutura antes de implementar", "Considere performance e escalabilidade", "Adicione testes unitários" }
                },
            },
            Summary = "Nesta aula, você aprendeu sobre Global Query Filters no Entity Framework Core. Exploramos Soft Delete, Multi-tenancy, Filtros Globais e vimos como aplicar estes conceitos em projetos reais. Compreender profundamente Global Query Filters é essencial para desenvolver aplicações robustas e escaláveis. Continue praticando com os exercícios propostos e experimente aplicar estes conceitos em seus próprios projetos. Na próxima aula, avançaremos para tópicos ainda mais interessantes do Entity Framework Core."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0005-000000000014"),
            CourseId = Guid.Parse("10000000-0000-0000-0000-000000000005"),
            Title = "Global Query Filters",
            Duration = "45 min",
            Difficulty = "Intermediário",
            EstimatedMinutes = 45,
            Prerequisites = "[]",
            OrderIndex = 14,
            Content = "",
            StructuredContent = JsonSerializer.Serialize(content),
            Version = 1,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }

    private Lesson CreateLesson15()
    {
        var content = new LessonContent
        {
            Objectives = new List<string>
            {
                "Compreender conversores personalizados",
                "Aprender a implementar enums",
                "Dominar json columns",
                "Aplicar value conversions em projetos reais"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "Conversores Personalizados",
                    Content = "O conceito de conversores personalizados é fundamental no desenvolvimento com Entity Framework Core. Este recurso permite que você trabalhe de forma mais eficiente e profissional com o framework. Desenvolvedores experientes utilizam conversores personalizados regularmente em projetos do mundo real para garantir código de qualidade, manutenível e performático. Compreender profundamente este tópico permitirá que você escreva aplicações mais robustas e escaláveis. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno. É importante seguir as melhores práticas da indústria ao trabalhar com conversores personalizados, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do Entity Framework Core e do desenvolvimento .NET em geral.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Enums",
                    Content = "O conceito de enums é fundamental no desenvolvimento com Entity Framework Core. Este recurso permite que você trabalhe de forma mais eficiente e profissional com o framework. Desenvolvedores experientes utilizam enums regularmente em projetos do mundo real para garantir código de qualidade, manutenível e performático. Compreender profundamente este tópico permitirá que você escreva aplicações mais robustas e escaláveis. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno. É importante seguir as melhores práticas da indústria ao trabalhar com enums, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do Entity Framework Core e do desenvolvimento .NET em geral.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "JSON Columns",
                    Content = "O conceito de json columns é fundamental no desenvolvimento com Entity Framework Core. Este recurso permite que você trabalhe de forma mais eficiente e profissional com o framework. Desenvolvedores experientes utilizam json columns regularmente em projetos do mundo real para garantir código de qualidade, manutenível e performático. Compreender profundamente este tópico permitirá que você escreva aplicações mais robustas e escaláveis. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno. É importante seguir as melhores práticas da indústria ao trabalhar com json columns, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do Entity Framework Core e do desenvolvimento .NET em geral.",
                    Order = 3
                },
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Exemplo Básico de Value Conversions",
                    Code = @"using Microsoft.EntityFrameworkCore;

// Exemplo de implementação de Value Conversions
using Microsoft.EntityFrameworkCore;

public class ExemploContext : DbContext
{
    // Configuração básica
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Implementação do conceito
        modelBuilder.Entity<Produto>()
            .Property(p => p.Nome)
            .IsRequired();
    }
}

// Uso prático
var produtos = await context.Produtos
    .Where(p => p.Ativo)
    .ToListAsync();",
                    Language = "csharp",
                    Explanation = "Este exemplo demonstra a implementação básica de Value Conversions. Note como o código é estruturado de forma clara e segue as convenções do EF Core.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Exemplo Avançado",
                    Code = @"using Microsoft.EntityFrameworkCore;

// Implementação avançada com Conversores Personalizados
public class ServicoAvancado
{
    private readonly ApplicationDbContext _context;
    
    public ServicoAvancado(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<Produto>> ObterProdutosAsync()
    {
        return await _context.Produtos
            .Include(p => p.Categoria)
            .Where(p => p.Ativo)
            .OrderBy(p => p.Nome)
            .ToListAsync();
    }
}",
                    Language = "csharp",
                    Explanation = "Exemplo mais complexo mostrando como aplicar Value Conversions em cenários reais com múltiplas operações.",
                    IsRunnable = true
                },
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Exercício 1: Implementação Básica",
                    Description = "Implemente Value Conversions em um contexto simples com uma entidade Produto.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"using Microsoft.EntityFrameworkCore;

// Implemente aqui
public class MeuContexto : DbContext
{
    
}",
                    Hints = new List<string> { "Comece definindo o DbContext", "Configure Conversores Personalizados no OnModelCreating", "Teste sua implementação" }
                },
                new Exercise
                {
                    Title = "Exercício 2: Cenário Intermediário",
                    Description = "Crie um serviço que utilize Value Conversions para gerenciar produtos e categorias.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"// Implemente o serviço
public class ProdutoService
{
    
}",
                    Hints = new List<string> { "Injete o DbContext via construtor", "Implemente métodos que usem Value Conversions", "Adicione tratamento de erros" }
                },
                new Exercise
                {
                    Title = "Exercício 3: Desafio Avançado",
                    Description = "Implemente um sistema completo utilizando Value Conversions com múltiplas entidades e relacionamentos.",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = @"// Sistema completo
",
                    Hints = new List<string> { "Planeje a estrutura antes de implementar", "Considere performance e escalabilidade", "Adicione testes unitários" }
                },
            },
            Summary = "Nesta aula, você aprendeu sobre Value Conversions no Entity Framework Core. Exploramos Conversores Personalizados, Enums, JSON Columns e vimos como aplicar estes conceitos em projetos reais. Compreender profundamente Value Conversions é essencial para desenvolver aplicações robustas e escaláveis. Continue praticando com os exercícios propostos e experimente aplicar estes conceitos em seus próprios projetos. Na próxima aula, avançaremos para tópicos ainda mais interessantes do Entity Framework Core."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0005-000000000015"),
            CourseId = Guid.Parse("10000000-0000-0000-0000-000000000005"),
            Title = "Value Conversions",
            Duration = "45 min",
            Difficulty = "Intermediário",
            EstimatedMinutes = 45,
            Prerequisites = "[]",
            OrderIndex = 15,
            Content = "",
            StructuredContent = JsonSerializer.Serialize(content),
            Version = 1,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }

    private Lesson CreateLesson16()
    {
        var content = new LessonContent
        {
            Objectives = new List<string>
            {
                "Compreender propriedades sombra",
                "Aprender a implementar auditoria",
                "Dominar metadados",
                "Aplicar shadow properties em projetos reais"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "Propriedades Sombra",
                    Content = "O conceito de propriedades sombra é fundamental no desenvolvimento com Entity Framework Core. Este recurso permite que você trabalhe de forma mais eficiente e profissional com o framework. Desenvolvedores experientes utilizam propriedades sombra regularmente em projetos do mundo real para garantir código de qualidade, manutenível e performático. Compreender profundamente este tópico permitirá que você escreva aplicações mais robustas e escaláveis. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno. É importante seguir as melhores práticas da indústria ao trabalhar com propriedades sombra, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do Entity Framework Core e do desenvolvimento .NET em geral.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Auditoria",
                    Content = "O conceito de auditoria é fundamental no desenvolvimento com Entity Framework Core. Este recurso permite que você trabalhe de forma mais eficiente e profissional com o framework. Desenvolvedores experientes utilizam auditoria regularmente em projetos do mundo real para garantir código de qualidade, manutenível e performático. Compreender profundamente este tópico permitirá que você escreva aplicações mais robustas e escaláveis. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno. É importante seguir as melhores práticas da indústria ao trabalhar com auditoria, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do Entity Framework Core e do desenvolvimento .NET em geral.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Metadados",
                    Content = "O conceito de metadados é fundamental no desenvolvimento com Entity Framework Core. Este recurso permite que você trabalhe de forma mais eficiente e profissional com o framework. Desenvolvedores experientes utilizam metadados regularmente em projetos do mundo real para garantir código de qualidade, manutenível e performático. Compreender profundamente este tópico permitirá que você escreva aplicações mais robustas e escaláveis. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno. É importante seguir as melhores práticas da indústria ao trabalhar com metadados, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do Entity Framework Core e do desenvolvimento .NET em geral.",
                    Order = 3
                },
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Exemplo Básico de Shadow Properties",
                    Code = @"using Microsoft.EntityFrameworkCore;

// Exemplo de implementação de Shadow Properties
using Microsoft.EntityFrameworkCore;

public class ExemploContext : DbContext
{
    // Configuração básica
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Implementação do conceito
        modelBuilder.Entity<Produto>()
            .Property(p => p.Nome)
            .IsRequired();
    }
}

// Uso prático
var produtos = await context.Produtos
    .Where(p => p.Ativo)
    .ToListAsync();",
                    Language = "csharp",
                    Explanation = "Este exemplo demonstra a implementação básica de Shadow Properties. Note como o código é estruturado de forma clara e segue as convenções do EF Core.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Exemplo Avançado",
                    Code = @"using Microsoft.EntityFrameworkCore;

// Implementação avançada com Propriedades Sombra
public class ServicoAvancado
{
    private readonly ApplicationDbContext _context;
    
    public ServicoAvancado(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<Produto>> ObterProdutosAsync()
    {
        return await _context.Produtos
            .Include(p => p.Categoria)
            .Where(p => p.Ativo)
            .OrderBy(p => p.Nome)
            .ToListAsync();
    }
}",
                    Language = "csharp",
                    Explanation = "Exemplo mais complexo mostrando como aplicar Shadow Properties em cenários reais com múltiplas operações.",
                    IsRunnable = true
                },
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Exercício 1: Implementação Básica",
                    Description = "Implemente Shadow Properties em um contexto simples com uma entidade Produto.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"using Microsoft.EntityFrameworkCore;

// Implemente aqui
public class MeuContexto : DbContext
{
    
}",
                    Hints = new List<string> { "Comece definindo o DbContext", "Configure Propriedades Sombra no OnModelCreating", "Teste sua implementação" }
                },
                new Exercise
                {
                    Title = "Exercício 2: Cenário Intermediário",
                    Description = "Crie um serviço que utilize Shadow Properties para gerenciar produtos e categorias.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"// Implemente o serviço
public class ProdutoService
{
    
}",
                    Hints = new List<string> { "Injete o DbContext via construtor", "Implemente métodos que usem Shadow Properties", "Adicione tratamento de erros" }
                },
                new Exercise
                {
                    Title = "Exercício 3: Desafio Avançado",
                    Description = "Implemente um sistema completo utilizando Shadow Properties com múltiplas entidades e relacionamentos.",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = @"// Sistema completo
",
                    Hints = new List<string> { "Planeje a estrutura antes de implementar", "Considere performance e escalabilidade", "Adicione testes unitários" }
                },
            },
            Summary = "Nesta aula, você aprendeu sobre Shadow Properties no Entity Framework Core. Exploramos Propriedades Sombra, Auditoria, Metadados e vimos como aplicar estes conceitos em projetos reais. Compreender profundamente Shadow Properties é essencial para desenvolver aplicações robustas e escaláveis. Continue praticando com os exercícios propostos e experimente aplicar estes conceitos em seus próprios projetos. Na próxima aula, avançaremos para tópicos ainda mais interessantes do Entity Framework Core."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0005-000000000016"),
            CourseId = Guid.Parse("10000000-0000-0000-0000-000000000005"),
            Title = "Shadow Properties",
            Duration = "45 min",
            Difficulty = "Intermediário",
            EstimatedMinutes = 45,
            Prerequisites = "[]",
            OrderIndex = 16,
            Content = "",
            StructuredContent = JsonSerializer.Serialize(content),
            Version = 1,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }

    private Lesson CreateLesson17()
    {
        var content = new LessonContent
        {
            Objectives = new List<string>
            {
                "Compreender tipos próprios",
                "Aprender a implementar value objects",
                "Dominar agregados",
                "Aplicar owned types em projetos reais"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "Tipos Próprios",
                    Content = "O conceito de tipos próprios é fundamental no desenvolvimento com Entity Framework Core. Este recurso permite que você trabalhe de forma mais eficiente e profissional com o framework. Desenvolvedores experientes utilizam tipos próprios regularmente em projetos do mundo real para garantir código de qualidade, manutenível e performático. Compreender profundamente este tópico permitirá que você escreva aplicações mais robustas e escaláveis. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno. É importante seguir as melhores práticas da indústria ao trabalhar com tipos próprios, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do Entity Framework Core e do desenvolvimento .NET em geral.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Value Objects",
                    Content = "O conceito de value objects é fundamental no desenvolvimento com Entity Framework Core. Este recurso permite que você trabalhe de forma mais eficiente e profissional com o framework. Desenvolvedores experientes utilizam value objects regularmente em projetos do mundo real para garantir código de qualidade, manutenível e performático. Compreender profundamente este tópico permitirá que você escreva aplicações mais robustas e escaláveis. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno. É importante seguir as melhores práticas da indústria ao trabalhar com value objects, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do Entity Framework Core e do desenvolvimento .NET em geral.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Agregados",
                    Content = "O conceito de agregados é fundamental no desenvolvimento com Entity Framework Core. Este recurso permite que você trabalhe de forma mais eficiente e profissional com o framework. Desenvolvedores experientes utilizam agregados regularmente em projetos do mundo real para garantir código de qualidade, manutenível e performático. Compreender profundamente este tópico permitirá que você escreva aplicações mais robustas e escaláveis. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno. É importante seguir as melhores práticas da indústria ao trabalhar com agregados, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do Entity Framework Core e do desenvolvimento .NET em geral.",
                    Order = 3
                },
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Exemplo Básico de Owned Types",
                    Code = @"using Microsoft.EntityFrameworkCore;

// Exemplo de implementação de Owned Types
using Microsoft.EntityFrameworkCore;

public class ExemploContext : DbContext
{
    // Configuração básica
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Implementação do conceito
        modelBuilder.Entity<Produto>()
            .Property(p => p.Nome)
            .IsRequired();
    }
}

// Uso prático
var produtos = await context.Produtos
    .Where(p => p.Ativo)
    .ToListAsync();",
                    Language = "csharp",
                    Explanation = "Este exemplo demonstra a implementação básica de Owned Types. Note como o código é estruturado de forma clara e segue as convenções do EF Core.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Exemplo Avançado",
                    Code = @"using Microsoft.EntityFrameworkCore;

// Implementação avançada com Tipos Próprios
public class ServicoAvancado
{
    private readonly ApplicationDbContext _context;
    
    public ServicoAvancado(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<Produto>> ObterProdutosAsync()
    {
        return await _context.Produtos
            .Include(p => p.Categoria)
            .Where(p => p.Ativo)
            .OrderBy(p => p.Nome)
            .ToListAsync();
    }
}",
                    Language = "csharp",
                    Explanation = "Exemplo mais complexo mostrando como aplicar Owned Types em cenários reais com múltiplas operações.",
                    IsRunnable = true
                },
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Exercício 1: Implementação Básica",
                    Description = "Implemente Owned Types em um contexto simples com uma entidade Produto.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"using Microsoft.EntityFrameworkCore;

// Implemente aqui
public class MeuContexto : DbContext
{
    
}",
                    Hints = new List<string> { "Comece definindo o DbContext", "Configure Tipos Próprios no OnModelCreating", "Teste sua implementação" }
                },
                new Exercise
                {
                    Title = "Exercício 2: Cenário Intermediário",
                    Description = "Crie um serviço que utilize Owned Types para gerenciar produtos e categorias.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"// Implemente o serviço
public class ProdutoService
{
    
}",
                    Hints = new List<string> { "Injete o DbContext via construtor", "Implemente métodos que usem Owned Types", "Adicione tratamento de erros" }
                },
                new Exercise
                {
                    Title = "Exercício 3: Desafio Avançado",
                    Description = "Implemente um sistema completo utilizando Owned Types com múltiplas entidades e relacionamentos.",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = @"// Sistema completo
",
                    Hints = new List<string> { "Planeje a estrutura antes de implementar", "Considere performance e escalabilidade", "Adicione testes unitários" }
                },
            },
            Summary = "Nesta aula, você aprendeu sobre Owned Types no Entity Framework Core. Exploramos Tipos Próprios, Value Objects, Agregados e vimos como aplicar estes conceitos em projetos reais. Compreender profundamente Owned Types é essencial para desenvolver aplicações robustas e escaláveis. Continue praticando com os exercícios propostos e experimente aplicar estes conceitos em seus próprios projetos. Na próxima aula, avançaremos para tópicos ainda mais interessantes do Entity Framework Core."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0005-000000000017"),
            CourseId = Guid.Parse("10000000-0000-0000-0000-000000000005"),
            Title = "Owned Types",
            Duration = "45 min",
            Difficulty = "Intermediário",
            EstimatedMinutes = 45,
            Prerequisites = "[]",
            OrderIndex = 17,
            Content = "",
            StructuredContent = JsonSerializer.Serialize(content),
            Version = 1,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }

    private Lesson CreateLesson18()
    {
        var content = new LessonContent
        {
            Objectives = new List<string>
            {
                "Compreender divisão de tabelas",
                "Aprender a implementar herança tph/tpt",
                "Dominar mapeamento",
                "Aplicar table splitting em projetos reais"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "Divisão de Tabelas",
                    Content = "O conceito de divisão de tabelas é fundamental no desenvolvimento com Entity Framework Core. Este recurso permite que você trabalhe de forma mais eficiente e profissional com o framework. Desenvolvedores experientes utilizam divisão de tabelas regularmente em projetos do mundo real para garantir código de qualidade, manutenível e performático. Compreender profundamente este tópico permitirá que você escreva aplicações mais robustas e escaláveis. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno. É importante seguir as melhores práticas da indústria ao trabalhar com divisão de tabelas, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do Entity Framework Core e do desenvolvimento .NET em geral.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Herança TPH/TPT",
                    Content = "O conceito de herança tph/tpt é fundamental no desenvolvimento com Entity Framework Core. Este recurso permite que você trabalhe de forma mais eficiente e profissional com o framework. Desenvolvedores experientes utilizam herança tph/tpt regularmente em projetos do mundo real para garantir código de qualidade, manutenível e performático. Compreender profundamente este tópico permitirá que você escreva aplicações mais robustas e escaláveis. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno. É importante seguir as melhores práticas da indústria ao trabalhar com herança tph/tpt, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do Entity Framework Core e do desenvolvimento .NET em geral.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Mapeamento",
                    Content = "O conceito de mapeamento é fundamental no desenvolvimento com Entity Framework Core. Este recurso permite que você trabalhe de forma mais eficiente e profissional com o framework. Desenvolvedores experientes utilizam mapeamento regularmente em projetos do mundo real para garantir código de qualidade, manutenível e performático. Compreender profundamente este tópico permitirá que você escreva aplicações mais robustas e escaláveis. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno. É importante seguir as melhores práticas da indústria ao trabalhar com mapeamento, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do Entity Framework Core e do desenvolvimento .NET em geral.",
                    Order = 3
                },
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Exemplo Básico de Table Splitting",
                    Code = @"using Microsoft.EntityFrameworkCore;

// Exemplo de implementação de Table Splitting
using Microsoft.EntityFrameworkCore;

public class ExemploContext : DbContext
{
    // Configuração básica
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Implementação do conceito
        modelBuilder.Entity<Produto>()
            .Property(p => p.Nome)
            .IsRequired();
    }
}

// Uso prático
var produtos = await context.Produtos
    .Where(p => p.Ativo)
    .ToListAsync();",
                    Language = "csharp",
                    Explanation = "Este exemplo demonstra a implementação básica de Table Splitting. Note como o código é estruturado de forma clara e segue as convenções do EF Core.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Exemplo Avançado",
                    Code = @"using Microsoft.EntityFrameworkCore;

// Implementação avançada com Divisão de Tabelas
public class ServicoAvancado
{
    private readonly ApplicationDbContext _context;
    
    public ServicoAvancado(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<Produto>> ObterProdutosAsync()
    {
        return await _context.Produtos
            .Include(p => p.Categoria)
            .Where(p => p.Ativo)
            .OrderBy(p => p.Nome)
            .ToListAsync();
    }
}",
                    Language = "csharp",
                    Explanation = "Exemplo mais complexo mostrando como aplicar Table Splitting em cenários reais com múltiplas operações.",
                    IsRunnable = true
                },
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Exercício 1: Implementação Básica",
                    Description = "Implemente Table Splitting em um contexto simples com uma entidade Produto.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"using Microsoft.EntityFrameworkCore;

// Implemente aqui
public class MeuContexto : DbContext
{
    
}",
                    Hints = new List<string> { "Comece definindo o DbContext", "Configure Divisão de Tabelas no OnModelCreating", "Teste sua implementação" }
                },
                new Exercise
                {
                    Title = "Exercício 2: Cenário Intermediário",
                    Description = "Crie um serviço que utilize Table Splitting para gerenciar produtos e categorias.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"// Implemente o serviço
public class ProdutoService
{
    
}",
                    Hints = new List<string> { "Injete o DbContext via construtor", "Implemente métodos que usem Table Splitting", "Adicione tratamento de erros" }
                },
                new Exercise
                {
                    Title = "Exercício 3: Desafio Avançado",
                    Description = "Implemente um sistema completo utilizando Table Splitting com múltiplas entidades e relacionamentos.",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = @"// Sistema completo
",
                    Hints = new List<string> { "Planeje a estrutura antes de implementar", "Considere performance e escalabilidade", "Adicione testes unitários" }
                },
            },
            Summary = "Nesta aula, você aprendeu sobre Table Splitting no Entity Framework Core. Exploramos Divisão de Tabelas, Herança TPH/TPT, Mapeamento e vimos como aplicar estes conceitos em projetos reais. Compreender profundamente Table Splitting é essencial para desenvolver aplicações robustas e escaláveis. Continue praticando com os exercícios propostos e experimente aplicar estes conceitos em seus próprios projetos. Na próxima aula, avançaremos para tópicos ainda mais interessantes do Entity Framework Core."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0005-000000000018"),
            CourseId = Guid.Parse("10000000-0000-0000-0000-000000000005"),
            Title = "Table Splitting",
            Duration = "45 min",
            Difficulty = "Intermediário",
            EstimatedMinutes = 45,
            Prerequisites = "[]",
            OrderIndex = 18,
            Content = "",
            StructuredContent = JsonSerializer.Serialize(content),
            Version = 1,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }

    private Lesson CreateLesson19()
    {
        var content = new LessonContent
        {
            Objectives = new List<string>
            {
                "Compreender queries compiladas",
                "Aprender a implementar performance",
                "Dominar caching",
                "Aplicar compiled queries em projetos reais"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "Queries Compiladas",
                    Content = "O conceito de queries compiladas é fundamental no desenvolvimento com Entity Framework Core. Este recurso permite que você trabalhe de forma mais eficiente e profissional com o framework. Desenvolvedores experientes utilizam queries compiladas regularmente em projetos do mundo real para garantir código de qualidade, manutenível e performático. Compreender profundamente este tópico permitirá que você escreva aplicações mais robustas e escaláveis. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno. É importante seguir as melhores práticas da indústria ao trabalhar com queries compiladas, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do Entity Framework Core e do desenvolvimento .NET em geral.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Performance",
                    Content = "O conceito de performance é fundamental no desenvolvimento com Entity Framework Core. Este recurso permite que você trabalhe de forma mais eficiente e profissional com o framework. Desenvolvedores experientes utilizam performance regularmente em projetos do mundo real para garantir código de qualidade, manutenível e performático. Compreender profundamente este tópico permitirá que você escreva aplicações mais robustas e escaláveis. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno. É importante seguir as melhores práticas da indústria ao trabalhar com performance, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do Entity Framework Core e do desenvolvimento .NET em geral.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Caching",
                    Content = "O conceito de caching é fundamental no desenvolvimento com Entity Framework Core. Este recurso permite que você trabalhe de forma mais eficiente e profissional com o framework. Desenvolvedores experientes utilizam caching regularmente em projetos do mundo real para garantir código de qualidade, manutenível e performático. Compreender profundamente este tópico permitirá que você escreva aplicações mais robustas e escaláveis. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno. É importante seguir as melhores práticas da indústria ao trabalhar com caching, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do Entity Framework Core e do desenvolvimento .NET em geral.",
                    Order = 3
                },
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Exemplo Básico de Compiled Queries",
                    Code = @"using Microsoft.EntityFrameworkCore;

// Exemplo de implementação de Compiled Queries
using Microsoft.EntityFrameworkCore;

public class ExemploContext : DbContext
{
    // Configuração básica
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Implementação do conceito
        modelBuilder.Entity<Produto>()
            .Property(p => p.Nome)
            .IsRequired();
    }
}

// Uso prático
var produtos = await context.Produtos
    .Where(p => p.Ativo)
    .ToListAsync();",
                    Language = "csharp",
                    Explanation = "Este exemplo demonstra a implementação básica de Compiled Queries. Note como o código é estruturado de forma clara e segue as convenções do EF Core.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Exemplo Avançado",
                    Code = @"using Microsoft.EntityFrameworkCore;

// Implementação avançada com Queries Compiladas
public class ServicoAvancado
{
    private readonly ApplicationDbContext _context;
    
    public ServicoAvancado(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<Produto>> ObterProdutosAsync()
    {
        return await _context.Produtos
            .Include(p => p.Categoria)
            .Where(p => p.Ativo)
            .OrderBy(p => p.Nome)
            .ToListAsync();
    }
}",
                    Language = "csharp",
                    Explanation = "Exemplo mais complexo mostrando como aplicar Compiled Queries em cenários reais com múltiplas operações.",
                    IsRunnable = true
                },
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Exercício 1: Implementação Básica",
                    Description = "Implemente Compiled Queries em um contexto simples com uma entidade Produto.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"using Microsoft.EntityFrameworkCore;

// Implemente aqui
public class MeuContexto : DbContext
{
    
}",
                    Hints = new List<string> { "Comece definindo o DbContext", "Configure Queries Compiladas no OnModelCreating", "Teste sua implementação" }
                },
                new Exercise
                {
                    Title = "Exercício 2: Cenário Intermediário",
                    Description = "Crie um serviço que utilize Compiled Queries para gerenciar produtos e categorias.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"// Implemente o serviço
public class ProdutoService
{
    
}",
                    Hints = new List<string> { "Injete o DbContext via construtor", "Implemente métodos que usem Compiled Queries", "Adicione tratamento de erros" }
                },
                new Exercise
                {
                    Title = "Exercício 3: Desafio Avançado",
                    Description = "Implemente um sistema completo utilizando Compiled Queries com múltiplas entidades e relacionamentos.",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = @"// Sistema completo
",
                    Hints = new List<string> { "Planeje a estrutura antes de implementar", "Considere performance e escalabilidade", "Adicione testes unitários" }
                },
            },
            Summary = "Nesta aula, você aprendeu sobre Compiled Queries no Entity Framework Core. Exploramos Queries Compiladas, Performance, Caching e vimos como aplicar estes conceitos em projetos reais. Compreender profundamente Compiled Queries é essencial para desenvolver aplicações robustas e escaláveis. Continue praticando com os exercícios propostos e experimente aplicar estes conceitos em seus próprios projetos. Na próxima aula, avançaremos para tópicos ainda mais interessantes do Entity Framework Core."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0005-000000000019"),
            CourseId = Guid.Parse("10000000-0000-0000-0000-000000000005"),
            Title = "Compiled Queries",
            Duration = "45 min",
            Difficulty = "Intermediário",
            EstimatedMinutes = 45,
            Prerequisites = "[]",
            OrderIndex = 19,
            Content = "",
            StructuredContent = JsonSerializer.Serialize(content),
            Version = 1,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }

    private Lesson CreateLesson20()
    {
        var content = new LessonContent
        {
            Objectives = new List<string>
            {
                "Compreender padrões",
                "Aprender a implementar anti-padrões",
                "Dominar otimização",
                "Aplicar ef core best practices em projetos reais"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "Padrões",
                    Content = "O conceito de padrões é fundamental no desenvolvimento com Entity Framework Core. Este recurso permite que você trabalhe de forma mais eficiente e profissional com o framework. Desenvolvedores experientes utilizam padrões regularmente em projetos do mundo real para garantir código de qualidade, manutenível e performático. Compreender profundamente este tópico permitirá que você escreva aplicações mais robustas e escaláveis. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno. É importante seguir as melhores práticas da indústria ao trabalhar com padrões, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do Entity Framework Core e do desenvolvimento .NET em geral.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Anti-padrões",
                    Content = "O conceito de anti-padrões é fundamental no desenvolvimento com Entity Framework Core. Este recurso permite que você trabalhe de forma mais eficiente e profissional com o framework. Desenvolvedores experientes utilizam anti-padrões regularmente em projetos do mundo real para garantir código de qualidade, manutenível e performático. Compreender profundamente este tópico permitirá que você escreva aplicações mais robustas e escaláveis. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno. É importante seguir as melhores práticas da indústria ao trabalhar com anti-padrões, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do Entity Framework Core e do desenvolvimento .NET em geral.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Otimização",
                    Content = "O conceito de otimização é fundamental no desenvolvimento com Entity Framework Core. Este recurso permite que você trabalhe de forma mais eficiente e profissional com o framework. Desenvolvedores experientes utilizam otimização regularmente em projetos do mundo real para garantir código de qualidade, manutenível e performático. Compreender profundamente este tópico permitirá que você escreva aplicações mais robustas e escaláveis. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno. É importante seguir as melhores práticas da indústria ao trabalhar com otimização, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do Entity Framework Core e do desenvolvimento .NET em geral.",
                    Order = 3
                },
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Exemplo Básico de EF Core Best Practices",
                    Code = @"using Microsoft.EntityFrameworkCore;

// Exemplo de implementação de EF Core Best Practices
using Microsoft.EntityFrameworkCore;

public class ExemploContext : DbContext
{
    // Configuração básica
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Implementação do conceito
        modelBuilder.Entity<Produto>()
            .Property(p => p.Nome)
            .IsRequired();
    }
}

// Uso prático
var produtos = await context.Produtos
    .Where(p => p.Ativo)
    .ToListAsync();",
                    Language = "csharp",
                    Explanation = "Este exemplo demonstra a implementação básica de EF Core Best Practices. Note como o código é estruturado de forma clara e segue as convenções do EF Core.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Exemplo Avançado",
                    Code = @"using Microsoft.EntityFrameworkCore;

// Implementação avançada com Padrões
public class ServicoAvancado
{
    private readonly ApplicationDbContext _context;
    
    public ServicoAvancado(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<Produto>> ObterProdutosAsync()
    {
        return await _context.Produtos
            .Include(p => p.Categoria)
            .Where(p => p.Ativo)
            .OrderBy(p => p.Nome)
            .ToListAsync();
    }
}",
                    Language = "csharp",
                    Explanation = "Exemplo mais complexo mostrando como aplicar EF Core Best Practices em cenários reais com múltiplas operações.",
                    IsRunnable = true
                },
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Exercício 1: Implementação Básica",
                    Description = "Implemente EF Core Best Practices em um contexto simples com uma entidade Produto.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"using Microsoft.EntityFrameworkCore;

// Implemente aqui
public class MeuContexto : DbContext
{
    
}",
                    Hints = new List<string> { "Comece definindo o DbContext", "Configure Padrões no OnModelCreating", "Teste sua implementação" }
                },
                new Exercise
                {
                    Title = "Exercício 2: Cenário Intermediário",
                    Description = "Crie um serviço que utilize EF Core Best Practices para gerenciar produtos e categorias.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"// Implemente o serviço
public class ProdutoService
{
    
}",
                    Hints = new List<string> { "Injete o DbContext via construtor", "Implemente métodos que usem EF Core Best Practices", "Adicione tratamento de erros" }
                },
                new Exercise
                {
                    Title = "Exercício 3: Desafio Avançado",
                    Description = "Implemente um sistema completo utilizando EF Core Best Practices com múltiplas entidades e relacionamentos.",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = @"// Sistema completo
",
                    Hints = new List<string> { "Planeje a estrutura antes de implementar", "Considere performance e escalabilidade", "Adicione testes unitários" }
                },
            },
            Summary = "Nesta aula, você aprendeu sobre EF Core Best Practices no Entity Framework Core. Exploramos Padrões, Anti-padrões, Otimização e vimos como aplicar estes conceitos em projetos reais. Compreender profundamente EF Core Best Practices é essencial para desenvolver aplicações robustas e escaláveis. Continue praticando com os exercícios propostos e experimente aplicar estes conceitos em seus próprios projetos. Na próxima aula, avançaremos para tópicos ainda mais interessantes do Entity Framework Core."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0005-000000000020"),
            CourseId = Guid.Parse("10000000-0000-0000-0000-000000000005"),
            Title = "EF Core Best Practices",
            Duration = "45 min",
            Difficulty = "Intermediário",
            EstimatedMinutes = 45,
            Prerequisites = "[]",
            OrderIndex = 20,
            Content = "",
            StructuredContent = JsonSerializer.Serialize(content),
            Version = 1,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }

}
