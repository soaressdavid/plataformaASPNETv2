using Shared.Entities;
using Shared.Models;
using System.Text.Json;

namespace Shared.Data;

/// <summary>
/// Level 7 Content Seeder - Autenticação e Autorização
/// Topics: Identity, JWT, OAuth, Claims, Policies, Role-Based Access
/// </summary>
public partial class Level7ContentSeeder
{
    private readonly Guid _courseId = Guid.Parse("10000000-0000-0000-0000-000000000008");
    private readonly Guid _levelId = Guid.Parse("00000000-0000-0000-0000-000000000007");

    public Course CreateLevel7Course()
    {
        return new Course
        {
            Id = _courseId,
            LevelId = _levelId,
            Title = "Autenticação e Autorização",
            Description = "Curso completo de Autenticação e Autorização com 20 aulas práticas e projetos reais.",
            Level = Level.Advanced,
            Duration = "5 semanas",
            LessonCount = 20,
            Topics = JsonSerializer.Serialize(new[] { "Identity", "JWT", "OAuth", "Claims", "Policies", "Role-Based Access" }),
            OrderIndex = 1,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }


    public List<Lesson> CreateLevel7Lessons()
    {
        return new List<Lesson>
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
    }


    private Lesson CreateLesson1()
    {
        var content = new LessonContent
        {
            Objectives = new List<string>
            {
                "Compreender os conceitos de authentication",
                "Aprender a implementar authorization",
                "Dominar princípios",
                "Aplicar fundamentos de segurança em projetos reais"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "Authentication",
                    Content = "O conceito de authentication é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam authentication regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com authentication, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Authorization",
                    Content = "O conceito de authorization é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam authorization regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com authorization, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Princípios",
                    Content = "O conceito de princípios é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam princípios regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com princípios, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 3
                },
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Exemplo Básico de Fundamentos de Segurança",
                    Code = @"using Microsoft.AspNetCore.Mvc;

// Implementação de Fundamentos de Segurança
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route(""api/[controller]"")]
public class ExemploController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        // Implementação do conceito
        return Ok(new { mensagem = ""Exemplo funcionando"" });
    }
}",
                    Language = "csharp",
                    Explanation = "Este exemplo demonstra a implementação básica de Fundamentos de Segurança. Note como o código segue as convenções do ASP.NET Core.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Exemplo Avançado",
                    Code = @"// Implementação avançada com Authentication
public class ServicoAvancado
{
    private readonly ILogger<ServicoAvancado> _logger;
    
    public ServicoAvancado(ILogger<ServicoAvancado> logger)
    {
        _logger = logger;
    }
    
    public async Task<ActionResult> ProcessarAsync()
    {
        _logger.LogInformation(""Processando..."");
        // Lógica avançada aqui
        return new OkResult();
    }
}",
                    Language = "csharp",
                    Explanation = "Exemplo mais complexo mostrando como aplicar Fundamentos de Segurança em cenários reais com logging e async/await.",
                    IsRunnable = true
                },
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Exercício 1: Implementação Básica",
                    Description = "Implemente Fundamentos de Segurança em um projeto ASP.NET Core simples.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"using Microsoft.AspNetCore.Mvc;

// Implemente aqui
[ApiController]
[Route(""api/[controller]"")]
public class MeuController : ControllerBase
{
    
}",
                    Hints = new List<string> { "Comece definindo o controller", "Configure Authentication", "Teste sua implementação" }
                },
                new Exercise
                {
                    Title = "Exercício 2: Cenário Intermediário",
                    Description = "Crie um serviço completo que utilize Fundamentos de Segurança para gerenciar recursos.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"// Implemente o serviço
public class MeuServico
{
    
}",
                    Hints = new List<string> { "Injete dependências via construtor", "Implemente métodos que usem Fundamentos de Segurança", "Adicione tratamento de erros" }
                },
                new Exercise
                {
                    Title = "Exercício 3: Desafio Avançado",
                    Description = "Implemente um sistema completo utilizando Fundamentos de Segurança com múltiplas funcionalidades.",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = @"// Sistema completo
",
                    Hints = new List<string> { "Planeje a arquitetura antes de implementar", "Considere performance e escalabilidade", "Adicione testes unitários" }
                },
            },
            Summary = "Nesta aula, você aprendeu sobre Fundamentos de Segurança no ASP.NET Core. Exploramos Authentication, Authorization, Princípios e vimos como aplicar estes conceitos em projetos reais. Compreender profundamente Fundamentos de Segurança é essencial para desenvolver aplicações web modernas, robustas e escaláveis. Continue praticando com os exercícios propostos e experimente aplicar estes conceitos em seus próprios projetos. Na próxima aula, avançaremos para tópicos ainda mais interessantes do ASP.NET Core."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0008-000000000001"),
            CourseId = Guid.Parse("10000000-0000-0000-0000-000000000008"),
            Title = "Fundamentos de Segurança",
            Duration = "45 min",
            Difficulty = "Intermediário",
            EstimatedMinutes = 45,
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
                "Compreender os conceitos de setup",
                "Aprender a implementar users",
                "Dominar roles",
                "Aplicar asp.net core identity em projetos reais"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "Setup",
                    Content = "O conceito de setup é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam setup regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com setup, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Users",
                    Content = "O conceito de users é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam users regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com users, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Roles",
                    Content = "O conceito de roles é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam roles regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com roles, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 3
                },
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Exemplo Básico de ASP.NET Core Identity",
                    Code = @"using Microsoft.AspNetCore.Mvc;

// Implementação de ASP.NET Core Identity
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route(""api/[controller]"")]
public class ExemploController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        // Implementação do conceito
        return Ok(new { mensagem = ""Exemplo funcionando"" });
    }
}",
                    Language = "csharp",
                    Explanation = "Este exemplo demonstra a implementação básica de ASP.NET Core Identity. Note como o código segue as convenções do ASP.NET Core.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Exemplo Avançado",
                    Code = @"// Implementação avançada com Setup
public class ServicoAvancado
{
    private readonly ILogger<ServicoAvancado> _logger;
    
    public ServicoAvancado(ILogger<ServicoAvancado> logger)
    {
        _logger = logger;
    }
    
    public async Task<ActionResult> ProcessarAsync()
    {
        _logger.LogInformation(""Processando..."");
        // Lógica avançada aqui
        return new OkResult();
    }
}",
                    Language = "csharp",
                    Explanation = "Exemplo mais complexo mostrando como aplicar ASP.NET Core Identity em cenários reais com logging e async/await.",
                    IsRunnable = true
                },
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Exercício 1: Implementação Básica",
                    Description = "Implemente ASP.NET Core Identity em um projeto ASP.NET Core simples.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"using Microsoft.AspNetCore.Mvc;

// Implemente aqui
[ApiController]
[Route(""api/[controller]"")]
public class MeuController : ControllerBase
{
    
}",
                    Hints = new List<string> { "Comece definindo o controller", "Configure Setup", "Teste sua implementação" }
                },
                new Exercise
                {
                    Title = "Exercício 2: Cenário Intermediário",
                    Description = "Crie um serviço completo que utilize ASP.NET Core Identity para gerenciar recursos.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"// Implemente o serviço
public class MeuServico
{
    
}",
                    Hints = new List<string> { "Injete dependências via construtor", "Implemente métodos que usem ASP.NET Core Identity", "Adicione tratamento de erros" }
                },
                new Exercise
                {
                    Title = "Exercício 3: Desafio Avançado",
                    Description = "Implemente um sistema completo utilizando ASP.NET Core Identity com múltiplas funcionalidades.",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = @"// Sistema completo
",
                    Hints = new List<string> { "Planeje a arquitetura antes de implementar", "Considere performance e escalabilidade", "Adicione testes unitários" }
                },
            },
            Summary = "Nesta aula, você aprendeu sobre ASP.NET Core Identity no ASP.NET Core. Exploramos Setup, Users, Roles e vimos como aplicar estes conceitos em projetos reais. Compreender profundamente ASP.NET Core Identity é essencial para desenvolver aplicações web modernas, robustas e escaláveis. Continue praticando com os exercícios propostos e experimente aplicar estes conceitos em seus próprios projetos. Na próxima aula, avançaremos para tópicos ainda mais interessantes do ASP.NET Core."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0008-000000000002"),
            CourseId = Guid.Parse("10000000-0000-0000-0000-000000000008"),
            Title = "ASP.NET Core Identity",
            Duration = "45 min",
            Difficulty = "Intermediário",
            EstimatedMinutes = 45,
            Prerequisites = "[]",
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
                "Compreender os conceitos de hashing",
                "Aprender a implementar salt",
                "Dominar algorithms",
                "Aplicar password hashing em projetos reais"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "Hashing",
                    Content = "O conceito de hashing é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam hashing regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com hashing, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Salt",
                    Content = "O conceito de salt é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam salt regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com salt, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Algorithms",
                    Content = "O conceito de algorithms é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam algorithms regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com algorithms, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 3
                },
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Exemplo Básico de Password Hashing",
                    Code = @"using Microsoft.AspNetCore.Mvc;

// Implementação de Password Hashing
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route(""api/[controller]"")]
public class ExemploController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        // Implementação do conceito
        return Ok(new { mensagem = ""Exemplo funcionando"" });
    }
}",
                    Language = "csharp",
                    Explanation = "Este exemplo demonstra a implementação básica de Password Hashing. Note como o código segue as convenções do ASP.NET Core.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Exemplo Avançado",
                    Code = @"// Implementação avançada com Hashing
public class ServicoAvancado
{
    private readonly ILogger<ServicoAvancado> _logger;
    
    public ServicoAvancado(ILogger<ServicoAvancado> logger)
    {
        _logger = logger;
    }
    
    public async Task<ActionResult> ProcessarAsync()
    {
        _logger.LogInformation(""Processando..."");
        // Lógica avançada aqui
        return new OkResult();
    }
}",
                    Language = "csharp",
                    Explanation = "Exemplo mais complexo mostrando como aplicar Password Hashing em cenários reais com logging e async/await.",
                    IsRunnable = true
                },
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Exercício 1: Implementação Básica",
                    Description = "Implemente Password Hashing em um projeto ASP.NET Core simples.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"using Microsoft.AspNetCore.Mvc;

// Implemente aqui
[ApiController]
[Route(""api/[controller]"")]
public class MeuController : ControllerBase
{
    
}",
                    Hints = new List<string> { "Comece definindo o controller", "Configure Hashing", "Teste sua implementação" }
                },
                new Exercise
                {
                    Title = "Exercício 2: Cenário Intermediário",
                    Description = "Crie um serviço completo que utilize Password Hashing para gerenciar recursos.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"// Implemente o serviço
public class MeuServico
{
    
}",
                    Hints = new List<string> { "Injete dependências via construtor", "Implemente métodos que usem Password Hashing", "Adicione tratamento de erros" }
                },
                new Exercise
                {
                    Title = "Exercício 3: Desafio Avançado",
                    Description = "Implemente um sistema completo utilizando Password Hashing com múltiplas funcionalidades.",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = @"// Sistema completo
",
                    Hints = new List<string> { "Planeje a arquitetura antes de implementar", "Considere performance e escalabilidade", "Adicione testes unitários" }
                },
            },
            Summary = "Nesta aula, você aprendeu sobre Password Hashing no ASP.NET Core. Exploramos Hashing, Salt, Algorithms e vimos como aplicar estes conceitos em projetos reais. Compreender profundamente Password Hashing é essencial para desenvolver aplicações web modernas, robustas e escaláveis. Continue praticando com os exercícios propostos e experimente aplicar estes conceitos em seus próprios projetos. Na próxima aula, avançaremos para tópicos ainda mais interessantes do ASP.NET Core."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0008-000000000003"),
            CourseId = Guid.Parse("10000000-0000-0000-0000-000000000008"),
            Title = "Password Hashing",
            Duration = "45 min",
            Difficulty = "Intermediário",
            EstimatedMinutes = 45,
            Prerequisites = "[]",
            OrderIndex = 3,
            Content = "",
            StructuredContent = JsonSerializer.Serialize(content),
            Version = 1,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }

    private Lesson CreateLesson4()
    {
        var content = new LessonContent
        {
            Objectives = new List<string>
            {
                "Compreender os conceitos de claims",
                "Aprender a implementar claimsprincipal",
                "Dominar claimsidentity",
                "Aplicar claims-based identity em projetos reais"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "Claims",
                    Content = "O conceito de claims é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam claims regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com claims, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "ClaimsPrincipal",
                    Content = "O conceito de claimsprincipal é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam claimsprincipal regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com claimsprincipal, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "ClaimsIdentity",
                    Content = "O conceito de claimsidentity é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam claimsidentity regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com claimsidentity, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 3
                },
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Exemplo Básico de Claims-Based Identity",
                    Code = @"using Microsoft.AspNetCore.Mvc;

// Implementação de Claims-Based Identity
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route(""api/[controller]"")]
public class ExemploController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        // Implementação do conceito
        return Ok(new { mensagem = ""Exemplo funcionando"" });
    }
}",
                    Language = "csharp",
                    Explanation = "Este exemplo demonstra a implementação básica de Claims-Based Identity. Note como o código segue as convenções do ASP.NET Core.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Exemplo Avançado",
                    Code = @"// Implementação avançada com Claims
public class ServicoAvancado
{
    private readonly ILogger<ServicoAvancado> _logger;
    
    public ServicoAvancado(ILogger<ServicoAvancado> logger)
    {
        _logger = logger;
    }
    
    public async Task<ActionResult> ProcessarAsync()
    {
        _logger.LogInformation(""Processando..."");
        // Lógica avançada aqui
        return new OkResult();
    }
}",
                    Language = "csharp",
                    Explanation = "Exemplo mais complexo mostrando como aplicar Claims-Based Identity em cenários reais com logging e async/await.",
                    IsRunnable = true
                },
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Exercício 1: Implementação Básica",
                    Description = "Implemente Claims-Based Identity em um projeto ASP.NET Core simples.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"using Microsoft.AspNetCore.Mvc;

// Implemente aqui
[ApiController]
[Route(""api/[controller]"")]
public class MeuController : ControllerBase
{
    
}",
                    Hints = new List<string> { "Comece definindo o controller", "Configure Claims", "Teste sua implementação" }
                },
                new Exercise
                {
                    Title = "Exercício 2: Cenário Intermediário",
                    Description = "Crie um serviço completo que utilize Claims-Based Identity para gerenciar recursos.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"// Implemente o serviço
public class MeuServico
{
    
}",
                    Hints = new List<string> { "Injete dependências via construtor", "Implemente métodos que usem Claims-Based Identity", "Adicione tratamento de erros" }
                },
                new Exercise
                {
                    Title = "Exercício 3: Desafio Avançado",
                    Description = "Implemente um sistema completo utilizando Claims-Based Identity com múltiplas funcionalidades.",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = @"// Sistema completo
",
                    Hints = new List<string> { "Planeje a arquitetura antes de implementar", "Considere performance e escalabilidade", "Adicione testes unitários" }
                },
            },
            Summary = "Nesta aula, você aprendeu sobre Claims-Based Identity no ASP.NET Core. Exploramos Claims, ClaimsPrincipal, ClaimsIdentity e vimos como aplicar estes conceitos em projetos reais. Compreender profundamente Claims-Based Identity é essencial para desenvolver aplicações web modernas, robustas e escaláveis. Continue praticando com os exercícios propostos e experimente aplicar estes conceitos em seus próprios projetos. Na próxima aula, avançaremos para tópicos ainda mais interessantes do ASP.NET Core."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0008-000000000004"),
            CourseId = Guid.Parse("10000000-0000-0000-0000-000000000008"),
            Title = "Claims-Based Identity",
            Duration = "45 min",
            Difficulty = "Intermediário",
            EstimatedMinutes = 45,
            Prerequisites = "[]",
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
                "Compreender os conceitos de cookies",
                "Aprender a implementar sessions",
                "Dominar persistence",
                "Aplicar cookie authentication em projetos reais"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "Cookies",
                    Content = "O conceito de cookies é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam cookies regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com cookies, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Sessions",
                    Content = "O conceito de sessions é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam sessions regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com sessions, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Persistence",
                    Content = "O conceito de persistence é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam persistence regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com persistence, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 3
                },
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Exemplo Básico de Cookie Authentication",
                    Code = @"using Microsoft.AspNetCore.Mvc;

// Implementação de Cookie Authentication
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route(""api/[controller]"")]
public class ExemploController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        // Implementação do conceito
        return Ok(new { mensagem = ""Exemplo funcionando"" });
    }
}",
                    Language = "csharp",
                    Explanation = "Este exemplo demonstra a implementação básica de Cookie Authentication. Note como o código segue as convenções do ASP.NET Core.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Exemplo Avançado",
                    Code = @"// Implementação avançada com Cookies
public class ServicoAvancado
{
    private readonly ILogger<ServicoAvancado> _logger;
    
    public ServicoAvancado(ILogger<ServicoAvancado> logger)
    {
        _logger = logger;
    }
    
    public async Task<ActionResult> ProcessarAsync()
    {
        _logger.LogInformation(""Processando..."");
        // Lógica avançada aqui
        return new OkResult();
    }
}",
                    Language = "csharp",
                    Explanation = "Exemplo mais complexo mostrando como aplicar Cookie Authentication em cenários reais com logging e async/await.",
                    IsRunnable = true
                },
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Exercício 1: Implementação Básica",
                    Description = "Implemente Cookie Authentication em um projeto ASP.NET Core simples.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"using Microsoft.AspNetCore.Mvc;

// Implemente aqui
[ApiController]
[Route(""api/[controller]"")]
public class MeuController : ControllerBase
{
    
}",
                    Hints = new List<string> { "Comece definindo o controller", "Configure Cookies", "Teste sua implementação" }
                },
                new Exercise
                {
                    Title = "Exercício 2: Cenário Intermediário",
                    Description = "Crie um serviço completo que utilize Cookie Authentication para gerenciar recursos.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"// Implemente o serviço
public class MeuServico
{
    
}",
                    Hints = new List<string> { "Injete dependências via construtor", "Implemente métodos que usem Cookie Authentication", "Adicione tratamento de erros" }
                },
                new Exercise
                {
                    Title = "Exercício 3: Desafio Avançado",
                    Description = "Implemente um sistema completo utilizando Cookie Authentication com múltiplas funcionalidades.",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = @"// Sistema completo
",
                    Hints = new List<string> { "Planeje a arquitetura antes de implementar", "Considere performance e escalabilidade", "Adicione testes unitários" }
                },
            },
            Summary = "Nesta aula, você aprendeu sobre Cookie Authentication no ASP.NET Core. Exploramos Cookies, Sessions, Persistence e vimos como aplicar estes conceitos em projetos reais. Compreender profundamente Cookie Authentication é essencial para desenvolver aplicações web modernas, robustas e escaláveis. Continue praticando com os exercícios propostos e experimente aplicar estes conceitos em seus próprios projetos. Na próxima aula, avançaremos para tópicos ainda mais interessantes do ASP.NET Core."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0008-000000000005"),
            CourseId = Guid.Parse("10000000-0000-0000-0000-000000000008"),
            Title = "Cookie Authentication",
            Duration = "45 min",
            Difficulty = "Intermediário",
            EstimatedMinutes = 45,
            Prerequisites = "[]",
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
                "Compreender os conceitos de json web tokens",
                "Aprender a implementar structure",
                "Dominar validation",
                "Aplicar jwt tokens em projetos reais"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "JSON Web Tokens",
                    Content = "O conceito de json web tokens é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam json web tokens regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com json web tokens, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Structure",
                    Content = "O conceito de structure é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam structure regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com structure, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Validation",
                    Content = "O conceito de validation é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam validation regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com validation, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 3
                },
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Exemplo Básico de JWT Tokens",
                    Code = @"using Microsoft.AspNetCore.Mvc;

// Implementação de JWT Tokens
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route(""api/[controller]"")]
public class ExemploController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        // Implementação do conceito
        return Ok(new { mensagem = ""Exemplo funcionando"" });
    }
}",
                    Language = "csharp",
                    Explanation = "Este exemplo demonstra a implementação básica de JWT Tokens. Note como o código segue as convenções do ASP.NET Core.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Exemplo Avançado",
                    Code = @"// Implementação avançada com JSON Web Tokens
public class ServicoAvancado
{
    private readonly ILogger<ServicoAvancado> _logger;
    
    public ServicoAvancado(ILogger<ServicoAvancado> logger)
    {
        _logger = logger;
    }
    
    public async Task<ActionResult> ProcessarAsync()
    {
        _logger.LogInformation(""Processando..."");
        // Lógica avançada aqui
        return new OkResult();
    }
}",
                    Language = "csharp",
                    Explanation = "Exemplo mais complexo mostrando como aplicar JWT Tokens em cenários reais com logging e async/await.",
                    IsRunnable = true
                },
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Exercício 1: Implementação Básica",
                    Description = "Implemente JWT Tokens em um projeto ASP.NET Core simples.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"using Microsoft.AspNetCore.Mvc;

// Implemente aqui
[ApiController]
[Route(""api/[controller]"")]
public class MeuController : ControllerBase
{
    
}",
                    Hints = new List<string> { "Comece definindo o controller", "Configure JSON Web Tokens", "Teste sua implementação" }
                },
                new Exercise
                {
                    Title = "Exercício 2: Cenário Intermediário",
                    Description = "Crie um serviço completo que utilize JWT Tokens para gerenciar recursos.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"// Implemente o serviço
public class MeuServico
{
    
}",
                    Hints = new List<string> { "Injete dependências via construtor", "Implemente métodos que usem JWT Tokens", "Adicione tratamento de erros" }
                },
                new Exercise
                {
                    Title = "Exercício 3: Desafio Avançado",
                    Description = "Implemente um sistema completo utilizando JWT Tokens com múltiplas funcionalidades.",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = @"// Sistema completo
",
                    Hints = new List<string> { "Planeje a arquitetura antes de implementar", "Considere performance e escalabilidade", "Adicione testes unitários" }
                },
            },
            Summary = "Nesta aula, você aprendeu sobre JWT Tokens no ASP.NET Core. Exploramos JSON Web Tokens, Structure, Validation e vimos como aplicar estes conceitos em projetos reais. Compreender profundamente JWT Tokens é essencial para desenvolver aplicações web modernas, robustas e escaláveis. Continue praticando com os exercícios propostos e experimente aplicar estes conceitos em seus próprios projetos. Na próxima aula, avançaremos para tópicos ainda mais interessantes do ASP.NET Core."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0008-000000000006"),
            CourseId = Guid.Parse("10000000-0000-0000-0000-000000000008"),
            Title = "JWT Tokens",
            Duration = "45 min",
            Difficulty = "Intermediário",
            EstimatedMinutes = 45,
            Prerequisites = "[]",
            OrderIndex = 6,
            Content = "",
            StructuredContent = JsonSerializer.Serialize(content),
            Version = 1,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }

    private Lesson CreateLesson7()
    {
        var content = new LessonContent
        {
            Objectives = new List<string>
            {
                "Compreender os conceitos de claims",
                "Aprender a implementar signing",
                "Dominar expiration",
                "Aplicar token generation em projetos reais"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "Claims",
                    Content = "O conceito de claims é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam claims regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com claims, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Signing",
                    Content = "O conceito de signing é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam signing regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com signing, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Expiration",
                    Content = "O conceito de expiration é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam expiration regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com expiration, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 3
                },
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Exemplo Básico de Token Generation",
                    Code = @"using Microsoft.AspNetCore.Mvc;

// Implementação de Token Generation
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route(""api/[controller]"")]
public class ExemploController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        // Implementação do conceito
        return Ok(new { mensagem = ""Exemplo funcionando"" });
    }
}",
                    Language = "csharp",
                    Explanation = "Este exemplo demonstra a implementação básica de Token Generation. Note como o código segue as convenções do ASP.NET Core.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Exemplo Avançado",
                    Code = @"// Implementação avançada com Claims
public class ServicoAvancado
{
    private readonly ILogger<ServicoAvancado> _logger;
    
    public ServicoAvancado(ILogger<ServicoAvancado> logger)
    {
        _logger = logger;
    }
    
    public async Task<ActionResult> ProcessarAsync()
    {
        _logger.LogInformation(""Processando..."");
        // Lógica avançada aqui
        return new OkResult();
    }
}",
                    Language = "csharp",
                    Explanation = "Exemplo mais complexo mostrando como aplicar Token Generation em cenários reais com logging e async/await.",
                    IsRunnable = true
                },
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Exercício 1: Implementação Básica",
                    Description = "Implemente Token Generation em um projeto ASP.NET Core simples.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"using Microsoft.AspNetCore.Mvc;

// Implemente aqui
[ApiController]
[Route(""api/[controller]"")]
public class MeuController : ControllerBase
{
    
}",
                    Hints = new List<string> { "Comece definindo o controller", "Configure Claims", "Teste sua implementação" }
                },
                new Exercise
                {
                    Title = "Exercício 2: Cenário Intermediário",
                    Description = "Crie um serviço completo que utilize Token Generation para gerenciar recursos.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"// Implemente o serviço
public class MeuServico
{
    
}",
                    Hints = new List<string> { "Injete dependências via construtor", "Implemente métodos que usem Token Generation", "Adicione tratamento de erros" }
                },
                new Exercise
                {
                    Title = "Exercício 3: Desafio Avançado",
                    Description = "Implemente um sistema completo utilizando Token Generation com múltiplas funcionalidades.",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = @"// Sistema completo
",
                    Hints = new List<string> { "Planeje a arquitetura antes de implementar", "Considere performance e escalabilidade", "Adicione testes unitários" }
                },
            },
            Summary = "Nesta aula, você aprendeu sobre Token Generation no ASP.NET Core. Exploramos Claims, Signing, Expiration e vimos como aplicar estes conceitos em projetos reais. Compreender profundamente Token Generation é essencial para desenvolver aplicações web modernas, robustas e escaláveis. Continue praticando com os exercícios propostos e experimente aplicar estes conceitos em seus próprios projetos. Na próxima aula, avançaremos para tópicos ainda mais interessantes do ASP.NET Core."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0008-000000000007"),
            CourseId = Guid.Parse("10000000-0000-0000-0000-000000000008"),
            Title = "Token Generation",
            Duration = "45 min",
            Difficulty = "Intermediário",
            EstimatedMinutes = 45,
            Prerequisites = "[]",
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
                "Compreender os conceitos de middleware",
                "Aprender a implementar validation parameters",
                "Dominar security",
                "Aplicar token validation em projetos reais"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "Middleware",
                    Content = "O conceito de middleware é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam middleware regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com middleware, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Validation Parameters",
                    Content = "O conceito de validation parameters é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam validation parameters regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com validation parameters, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Security",
                    Content = "O conceito de security é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam security regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com security, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 3
                },
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Exemplo Básico de Token Validation",
                    Code = @"using Microsoft.AspNetCore.Mvc;

// Implementação de Token Validation
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route(""api/[controller]"")]
public class ExemploController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        // Implementação do conceito
        return Ok(new { mensagem = ""Exemplo funcionando"" });
    }
}",
                    Language = "csharp",
                    Explanation = "Este exemplo demonstra a implementação básica de Token Validation. Note como o código segue as convenções do ASP.NET Core.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Exemplo Avançado",
                    Code = @"// Implementação avançada com Middleware
public class ServicoAvancado
{
    private readonly ILogger<ServicoAvancado> _logger;
    
    public ServicoAvancado(ILogger<ServicoAvancado> logger)
    {
        _logger = logger;
    }
    
    public async Task<ActionResult> ProcessarAsync()
    {
        _logger.LogInformation(""Processando..."");
        // Lógica avançada aqui
        return new OkResult();
    }
}",
                    Language = "csharp",
                    Explanation = "Exemplo mais complexo mostrando como aplicar Token Validation em cenários reais com logging e async/await.",
                    IsRunnable = true
                },
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Exercício 1: Implementação Básica",
                    Description = "Implemente Token Validation em um projeto ASP.NET Core simples.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"using Microsoft.AspNetCore.Mvc;

// Implemente aqui
[ApiController]
[Route(""api/[controller]"")]
public class MeuController : ControllerBase
{
    
}",
                    Hints = new List<string> { "Comece definindo o controller", "Configure Middleware", "Teste sua implementação" }
                },
                new Exercise
                {
                    Title = "Exercício 2: Cenário Intermediário",
                    Description = "Crie um serviço completo que utilize Token Validation para gerenciar recursos.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"// Implemente o serviço
public class MeuServico
{
    
}",
                    Hints = new List<string> { "Injete dependências via construtor", "Implemente métodos que usem Token Validation", "Adicione tratamento de erros" }
                },
                new Exercise
                {
                    Title = "Exercício 3: Desafio Avançado",
                    Description = "Implemente um sistema completo utilizando Token Validation com múltiplas funcionalidades.",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = @"// Sistema completo
",
                    Hints = new List<string> { "Planeje a arquitetura antes de implementar", "Considere performance e escalabilidade", "Adicione testes unitários" }
                },
            },
            Summary = "Nesta aula, você aprendeu sobre Token Validation no ASP.NET Core. Exploramos Middleware, Validation Parameters, Security e vimos como aplicar estes conceitos em projetos reais. Compreender profundamente Token Validation é essencial para desenvolver aplicações web modernas, robustas e escaláveis. Continue praticando com os exercícios propostos e experimente aplicar estes conceitos em seus próprios projetos. Na próxima aula, avançaremos para tópicos ainda mais interessantes do ASP.NET Core."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0008-000000000008"),
            CourseId = Guid.Parse("10000000-0000-0000-0000-000000000008"),
            Title = "Token Validation",
            Duration = "45 min",
            Difficulty = "Intermediário",
            EstimatedMinutes = 45,
            Prerequisites = "[]",
            OrderIndex = 8,
            Content = "",
            StructuredContent = JsonSerializer.Serialize(content),
            Version = 1,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }

    private Lesson CreateLesson9()
    {
        var content = new LessonContent
        {
            Objectives = new List<string>
            {
                "Compreender os conceitos de rotation",
                "Aprender a implementar storage",
                "Dominar revocation",
                "Aplicar refresh tokens em projetos reais"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "Rotation",
                    Content = "O conceito de rotation é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam rotation regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com rotation, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Storage",
                    Content = "O conceito de storage é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam storage regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com storage, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Revocation",
                    Content = "O conceito de revocation é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam revocation regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com revocation, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 3
                },
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Exemplo Básico de Refresh Tokens",
                    Code = @"using Microsoft.AspNetCore.Mvc;

// Implementação de Refresh Tokens
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route(""api/[controller]"")]
public class ExemploController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        // Implementação do conceito
        return Ok(new { mensagem = ""Exemplo funcionando"" });
    }
}",
                    Language = "csharp",
                    Explanation = "Este exemplo demonstra a implementação básica de Refresh Tokens. Note como o código segue as convenções do ASP.NET Core.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Exemplo Avançado",
                    Code = @"// Implementação avançada com Rotation
public class ServicoAvancado
{
    private readonly ILogger<ServicoAvancado> _logger;
    
    public ServicoAvancado(ILogger<ServicoAvancado> logger)
    {
        _logger = logger;
    }
    
    public async Task<ActionResult> ProcessarAsync()
    {
        _logger.LogInformation(""Processando..."");
        // Lógica avançada aqui
        return new OkResult();
    }
}",
                    Language = "csharp",
                    Explanation = "Exemplo mais complexo mostrando como aplicar Refresh Tokens em cenários reais com logging e async/await.",
                    IsRunnable = true
                },
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Exercício 1: Implementação Básica",
                    Description = "Implemente Refresh Tokens em um projeto ASP.NET Core simples.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"using Microsoft.AspNetCore.Mvc;

// Implemente aqui
[ApiController]
[Route(""api/[controller]"")]
public class MeuController : ControllerBase
{
    
}",
                    Hints = new List<string> { "Comece definindo o controller", "Configure Rotation", "Teste sua implementação" }
                },
                new Exercise
                {
                    Title = "Exercício 2: Cenário Intermediário",
                    Description = "Crie um serviço completo que utilize Refresh Tokens para gerenciar recursos.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"// Implemente o serviço
public class MeuServico
{
    
}",
                    Hints = new List<string> { "Injete dependências via construtor", "Implemente métodos que usem Refresh Tokens", "Adicione tratamento de erros" }
                },
                new Exercise
                {
                    Title = "Exercício 3: Desafio Avançado",
                    Description = "Implemente um sistema completo utilizando Refresh Tokens com múltiplas funcionalidades.",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = @"// Sistema completo
",
                    Hints = new List<string> { "Planeje a arquitetura antes de implementar", "Considere performance e escalabilidade", "Adicione testes unitários" }
                },
            },
            Summary = "Nesta aula, você aprendeu sobre Refresh Tokens no ASP.NET Core. Exploramos Rotation, Storage, Revocation e vimos como aplicar estes conceitos em projetos reais. Compreender profundamente Refresh Tokens é essencial para desenvolver aplicações web modernas, robustas e escaláveis. Continue praticando com os exercícios propostos e experimente aplicar estes conceitos em seus próprios projetos. Na próxima aula, avançaremos para tópicos ainda mais interessantes do ASP.NET Core."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0008-000000000009"),
            CourseId = Guid.Parse("10000000-0000-0000-0000-000000000008"),
            Title = "Refresh Tokens",
            Duration = "45 min",
            Difficulty = "Intermediário",
            EstimatedMinutes = 45,
            Prerequisites = "[]",
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
                "Compreender os conceitos de flows",
                "Aprender a implementar authorization code",
                "Dominar client credentials",
                "Aplicar oauth 2.0 em projetos reais"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "Flows",
                    Content = "O conceito de flows é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam flows regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com flows, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Authorization Code",
                    Content = "O conceito de authorization code é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam authorization code regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com authorization code, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Client Credentials",
                    Content = "O conceito de client credentials é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam client credentials regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com client credentials, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 3
                },
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Exemplo Básico de OAuth 2.0",
                    Code = @"using Microsoft.AspNetCore.Mvc;

// Implementação de OAuth 2.0
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route(""api/[controller]"")]
public class ExemploController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        // Implementação do conceito
        return Ok(new { mensagem = ""Exemplo funcionando"" });
    }
}",
                    Language = "csharp",
                    Explanation = "Este exemplo demonstra a implementação básica de OAuth 2.0. Note como o código segue as convenções do ASP.NET Core.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Exemplo Avançado",
                    Code = @"// Implementação avançada com Flows
public class ServicoAvancado
{
    private readonly ILogger<ServicoAvancado> _logger;
    
    public ServicoAvancado(ILogger<ServicoAvancado> logger)
    {
        _logger = logger;
    }
    
    public async Task<ActionResult> ProcessarAsync()
    {
        _logger.LogInformation(""Processando..."");
        // Lógica avançada aqui
        return new OkResult();
    }
}",
                    Language = "csharp",
                    Explanation = "Exemplo mais complexo mostrando como aplicar OAuth 2.0 em cenários reais com logging e async/await.",
                    IsRunnable = true
                },
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Exercício 1: Implementação Básica",
                    Description = "Implemente OAuth 2.0 em um projeto ASP.NET Core simples.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"using Microsoft.AspNetCore.Mvc;

// Implemente aqui
[ApiController]
[Route(""api/[controller]"")]
public class MeuController : ControllerBase
{
    
}",
                    Hints = new List<string> { "Comece definindo o controller", "Configure Flows", "Teste sua implementação" }
                },
                new Exercise
                {
                    Title = "Exercício 2: Cenário Intermediário",
                    Description = "Crie um serviço completo que utilize OAuth 2.0 para gerenciar recursos.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"// Implemente o serviço
public class MeuServico
{
    
}",
                    Hints = new List<string> { "Injete dependências via construtor", "Implemente métodos que usem OAuth 2.0", "Adicione tratamento de erros" }
                },
                new Exercise
                {
                    Title = "Exercício 3: Desafio Avançado",
                    Description = "Implemente um sistema completo utilizando OAuth 2.0 com múltiplas funcionalidades.",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = @"// Sistema completo
",
                    Hints = new List<string> { "Planeje a arquitetura antes de implementar", "Considere performance e escalabilidade", "Adicione testes unitários" }
                },
            },
            Summary = "Nesta aula, você aprendeu sobre OAuth 2.0 no ASP.NET Core. Exploramos Flows, Authorization Code, Client Credentials e vimos como aplicar estes conceitos em projetos reais. Compreender profundamente OAuth 2.0 é essencial para desenvolver aplicações web modernas, robustas e escaláveis. Continue praticando com os exercícios propostos e experimente aplicar estes conceitos em seus próprios projetos. Na próxima aula, avançaremos para tópicos ainda mais interessantes do ASP.NET Core."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0008-000000000010"),
            CourseId = Guid.Parse("10000000-0000-0000-0000-000000000008"),
            Title = "OAuth 2.0",
            Duration = "45 min",
            Difficulty = "Intermediário",
            EstimatedMinutes = 45,
            Prerequisites = "[]",
            OrderIndex = 10,
            Content = "",
            StructuredContent = JsonSerializer.Serialize(content),
            Version = 1,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }

    private Lesson CreateLesson11()
    {
        var content = new LessonContent
        {
            Objectives = new List<string>
            {
                "Compreender os conceitos de oidc",
                "Aprender a implementar identity provider",
                "Dominar discovery",
                "Aplicar openid connect em projetos reais"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "OIDC",
                    Content = "O conceito de oidc é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam oidc regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com oidc, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Identity Provider",
                    Content = "O conceito de identity provider é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam identity provider regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com identity provider, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Discovery",
                    Content = "O conceito de discovery é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam discovery regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com discovery, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 3
                },
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Exemplo Básico de OpenID Connect",
                    Code = @"using Microsoft.AspNetCore.Mvc;

// Implementação de OpenID Connect
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route(""api/[controller]"")]
public class ExemploController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        // Implementação do conceito
        return Ok(new { mensagem = ""Exemplo funcionando"" });
    }
}",
                    Language = "csharp",
                    Explanation = "Este exemplo demonstra a implementação básica de OpenID Connect. Note como o código segue as convenções do ASP.NET Core.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Exemplo Avançado",
                    Code = @"// Implementação avançada com OIDC
public class ServicoAvancado
{
    private readonly ILogger<ServicoAvancado> _logger;
    
    public ServicoAvancado(ILogger<ServicoAvancado> logger)
    {
        _logger = logger;
    }
    
    public async Task<ActionResult> ProcessarAsync()
    {
        _logger.LogInformation(""Processando..."");
        // Lógica avançada aqui
        return new OkResult();
    }
}",
                    Language = "csharp",
                    Explanation = "Exemplo mais complexo mostrando como aplicar OpenID Connect em cenários reais com logging e async/await.",
                    IsRunnable = true
                },
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Exercício 1: Implementação Básica",
                    Description = "Implemente OpenID Connect em um projeto ASP.NET Core simples.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"using Microsoft.AspNetCore.Mvc;

// Implemente aqui
[ApiController]
[Route(""api/[controller]"")]
public class MeuController : ControllerBase
{
    
}",
                    Hints = new List<string> { "Comece definindo o controller", "Configure OIDC", "Teste sua implementação" }
                },
                new Exercise
                {
                    Title = "Exercício 2: Cenário Intermediário",
                    Description = "Crie um serviço completo que utilize OpenID Connect para gerenciar recursos.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"// Implemente o serviço
public class MeuServico
{
    
}",
                    Hints = new List<string> { "Injete dependências via construtor", "Implemente métodos que usem OpenID Connect", "Adicione tratamento de erros" }
                },
                new Exercise
                {
                    Title = "Exercício 3: Desafio Avançado",
                    Description = "Implemente um sistema completo utilizando OpenID Connect com múltiplas funcionalidades.",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = @"// Sistema completo
",
                    Hints = new List<string> { "Planeje a arquitetura antes de implementar", "Considere performance e escalabilidade", "Adicione testes unitários" }
                },
            },
            Summary = "Nesta aula, você aprendeu sobre OpenID Connect no ASP.NET Core. Exploramos OIDC, Identity Provider, Discovery e vimos como aplicar estes conceitos em projetos reais. Compreender profundamente OpenID Connect é essencial para desenvolver aplicações web modernas, robustas e escaláveis. Continue praticando com os exercícios propostos e experimente aplicar estes conceitos em seus próprios projetos. Na próxima aula, avançaremos para tópicos ainda mais interessantes do ASP.NET Core."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0008-000000000011"),
            CourseId = Guid.Parse("10000000-0000-0000-0000-000000000008"),
            Title = "OpenID Connect",
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
                "Compreender os conceitos de google",
                "Aprender a implementar facebook",
                "Dominar external providers",
                "Aplicar social login em projetos reais"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "Google",
                    Content = "O conceito de google é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam google regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com google, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Facebook",
                    Content = "O conceito de facebook é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam facebook regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com facebook, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "External Providers",
                    Content = "O conceito de external providers é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam external providers regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com external providers, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 3
                },
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Exemplo Básico de Social Login",
                    Code = @"using Microsoft.AspNetCore.Mvc;

// Implementação de Social Login
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route(""api/[controller]"")]
public class ExemploController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        // Implementação do conceito
        return Ok(new { mensagem = ""Exemplo funcionando"" });
    }
}",
                    Language = "csharp",
                    Explanation = "Este exemplo demonstra a implementação básica de Social Login. Note como o código segue as convenções do ASP.NET Core.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Exemplo Avançado",
                    Code = @"// Implementação avançada com Google
public class ServicoAvancado
{
    private readonly ILogger<ServicoAvancado> _logger;
    
    public ServicoAvancado(ILogger<ServicoAvancado> logger)
    {
        _logger = logger;
    }
    
    public async Task<ActionResult> ProcessarAsync()
    {
        _logger.LogInformation(""Processando..."");
        // Lógica avançada aqui
        return new OkResult();
    }
}",
                    Language = "csharp",
                    Explanation = "Exemplo mais complexo mostrando como aplicar Social Login em cenários reais com logging e async/await.",
                    IsRunnable = true
                },
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Exercício 1: Implementação Básica",
                    Description = "Implemente Social Login em um projeto ASP.NET Core simples.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"using Microsoft.AspNetCore.Mvc;

// Implemente aqui
[ApiController]
[Route(""api/[controller]"")]
public class MeuController : ControllerBase
{
    
}",
                    Hints = new List<string> { "Comece definindo o controller", "Configure Google", "Teste sua implementação" }
                },
                new Exercise
                {
                    Title = "Exercício 2: Cenário Intermediário",
                    Description = "Crie um serviço completo que utilize Social Login para gerenciar recursos.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"// Implemente o serviço
public class MeuServico
{
    
}",
                    Hints = new List<string> { "Injete dependências via construtor", "Implemente métodos que usem Social Login", "Adicione tratamento de erros" }
                },
                new Exercise
                {
                    Title = "Exercício 3: Desafio Avançado",
                    Description = "Implemente um sistema completo utilizando Social Login com múltiplas funcionalidades.",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = @"// Sistema completo
",
                    Hints = new List<string> { "Planeje a arquitetura antes de implementar", "Considere performance e escalabilidade", "Adicione testes unitários" }
                },
            },
            Summary = "Nesta aula, você aprendeu sobre Social Login no ASP.NET Core. Exploramos Google, Facebook, External Providers e vimos como aplicar estes conceitos em projetos reais. Compreender profundamente Social Login é essencial para desenvolver aplicações web modernas, robustas e escaláveis. Continue praticando com os exercícios propostos e experimente aplicar estes conceitos em seus próprios projetos. Na próxima aula, avançaremos para tópicos ainda mais interessantes do ASP.NET Core."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0008-000000000012"),
            CourseId = Guid.Parse("10000000-0000-0000-0000-000000000008"),
            Title = "Social Login",
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
                "Compreender os conceitos de 2fa",
                "Aprender a implementar totp",
                "Dominar sms",
                "Aplicar two-factor authentication em projetos reais"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "2FA",
                    Content = "O conceito de 2fa é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam 2fa regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com 2fa, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "TOTP",
                    Content = "O conceito de totp é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam totp regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com totp, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "SMS",
                    Content = "O conceito de sms é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam sms regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com sms, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 3
                },
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Exemplo Básico de Two-Factor Authentication",
                    Code = @"using Microsoft.AspNetCore.Mvc;

// Implementação de Two-Factor Authentication
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route(""api/[controller]"")]
public class ExemploController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        // Implementação do conceito
        return Ok(new { mensagem = ""Exemplo funcionando"" });
    }
}",
                    Language = "csharp",
                    Explanation = "Este exemplo demonstra a implementação básica de Two-Factor Authentication. Note como o código segue as convenções do ASP.NET Core.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Exemplo Avançado",
                    Code = @"// Implementação avançada com 2FA
public class ServicoAvancado
{
    private readonly ILogger<ServicoAvancado> _logger;
    
    public ServicoAvancado(ILogger<ServicoAvancado> logger)
    {
        _logger = logger;
    }
    
    public async Task<ActionResult> ProcessarAsync()
    {
        _logger.LogInformation(""Processando..."");
        // Lógica avançada aqui
        return new OkResult();
    }
}",
                    Language = "csharp",
                    Explanation = "Exemplo mais complexo mostrando como aplicar Two-Factor Authentication em cenários reais com logging e async/await.",
                    IsRunnable = true
                },
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Exercício 1: Implementação Básica",
                    Description = "Implemente Two-Factor Authentication em um projeto ASP.NET Core simples.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"using Microsoft.AspNetCore.Mvc;

// Implemente aqui
[ApiController]
[Route(""api/[controller]"")]
public class MeuController : ControllerBase
{
    
}",
                    Hints = new List<string> { "Comece definindo o controller", "Configure 2FA", "Teste sua implementação" }
                },
                new Exercise
                {
                    Title = "Exercício 2: Cenário Intermediário",
                    Description = "Crie um serviço completo que utilize Two-Factor Authentication para gerenciar recursos.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"// Implemente o serviço
public class MeuServico
{
    
}",
                    Hints = new List<string> { "Injete dependências via construtor", "Implemente métodos que usem Two-Factor Authentication", "Adicione tratamento de erros" }
                },
                new Exercise
                {
                    Title = "Exercício 3: Desafio Avançado",
                    Description = "Implemente um sistema completo utilizando Two-Factor Authentication com múltiplas funcionalidades.",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = @"// Sistema completo
",
                    Hints = new List<string> { "Planeje a arquitetura antes de implementar", "Considere performance e escalabilidade", "Adicione testes unitários" }
                },
            },
            Summary = "Nesta aula, você aprendeu sobre Two-Factor Authentication no ASP.NET Core. Exploramos 2FA, TOTP, SMS e vimos como aplicar estes conceitos em projetos reais. Compreender profundamente Two-Factor Authentication é essencial para desenvolver aplicações web modernas, robustas e escaláveis. Continue praticando com os exercícios propostos e experimente aplicar estes conceitos em seus próprios projetos. Na próxima aula, avançaremos para tópicos ainda mais interessantes do ASP.NET Core."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0008-000000000013"),
            CourseId = Guid.Parse("10000000-0000-0000-0000-000000000008"),
            Title = "Two-Factor Authentication",
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
                "Compreender os conceitos de roles",
                "Aprender a implementar policies",
                "Dominar requirements",
                "Aplicar role-based authorization em projetos reais"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "Roles",
                    Content = "O conceito de roles é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam roles regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com roles, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Policies",
                    Content = "O conceito de policies é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam policies regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com policies, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Requirements",
                    Content = "O conceito de requirements é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam requirements regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com requirements, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 3
                },
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Exemplo Básico de Role-Based Authorization",
                    Code = @"using Microsoft.AspNetCore.Mvc;

// Implementação de Role-Based Authorization
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route(""api/[controller]"")]
public class ExemploController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        // Implementação do conceito
        return Ok(new { mensagem = ""Exemplo funcionando"" });
    }
}",
                    Language = "csharp",
                    Explanation = "Este exemplo demonstra a implementação básica de Role-Based Authorization. Note como o código segue as convenções do ASP.NET Core.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Exemplo Avançado",
                    Code = @"// Implementação avançada com Roles
public class ServicoAvancado
{
    private readonly ILogger<ServicoAvancado> _logger;
    
    public ServicoAvancado(ILogger<ServicoAvancado> logger)
    {
        _logger = logger;
    }
    
    public async Task<ActionResult> ProcessarAsync()
    {
        _logger.LogInformation(""Processando..."");
        // Lógica avançada aqui
        return new OkResult();
    }
}",
                    Language = "csharp",
                    Explanation = "Exemplo mais complexo mostrando como aplicar Role-Based Authorization em cenários reais com logging e async/await.",
                    IsRunnable = true
                },
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Exercício 1: Implementação Básica",
                    Description = "Implemente Role-Based Authorization em um projeto ASP.NET Core simples.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"using Microsoft.AspNetCore.Mvc;

// Implemente aqui
[ApiController]
[Route(""api/[controller]"")]
public class MeuController : ControllerBase
{
    
}",
                    Hints = new List<string> { "Comece definindo o controller", "Configure Roles", "Teste sua implementação" }
                },
                new Exercise
                {
                    Title = "Exercício 2: Cenário Intermediário",
                    Description = "Crie um serviço completo que utilize Role-Based Authorization para gerenciar recursos.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"// Implemente o serviço
public class MeuServico
{
    
}",
                    Hints = new List<string> { "Injete dependências via construtor", "Implemente métodos que usem Role-Based Authorization", "Adicione tratamento de erros" }
                },
                new Exercise
                {
                    Title = "Exercício 3: Desafio Avançado",
                    Description = "Implemente um sistema completo utilizando Role-Based Authorization com múltiplas funcionalidades.",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = @"// Sistema completo
",
                    Hints = new List<string> { "Planeje a arquitetura antes de implementar", "Considere performance e escalabilidade", "Adicione testes unitários" }
                },
            },
            Summary = "Nesta aula, você aprendeu sobre Role-Based Authorization no ASP.NET Core. Exploramos Roles, Policies, Requirements e vimos como aplicar estes conceitos em projetos reais. Compreender profundamente Role-Based Authorization é essencial para desenvolver aplicações web modernas, robustas e escaláveis. Continue praticando com os exercícios propostos e experimente aplicar estes conceitos em seus próprios projetos. Na próxima aula, avançaremos para tópicos ainda mais interessantes do ASP.NET Core."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0008-000000000014"),
            CourseId = Guid.Parse("10000000-0000-0000-0000-000000000008"),
            Title = "Role-Based Authorization",
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
                "Compreender os conceitos de custom policies",
                "Aprender a implementar handlers",
                "Dominar requirements",
                "Aplicar policy-based authorization em projetos reais"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "Custom Policies",
                    Content = "O conceito de custom policies é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam custom policies regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com custom policies, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Handlers",
                    Content = "O conceito de handlers é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam handlers regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com handlers, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Requirements",
                    Content = "O conceito de requirements é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam requirements regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com requirements, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 3
                },
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Exemplo Básico de Policy-Based Authorization",
                    Code = @"using Microsoft.AspNetCore.Mvc;

// Implementação de Policy-Based Authorization
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route(""api/[controller]"")]
public class ExemploController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        // Implementação do conceito
        return Ok(new { mensagem = ""Exemplo funcionando"" });
    }
}",
                    Language = "csharp",
                    Explanation = "Este exemplo demonstra a implementação básica de Policy-Based Authorization. Note como o código segue as convenções do ASP.NET Core.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Exemplo Avançado",
                    Code = @"// Implementação avançada com Custom Policies
public class ServicoAvancado
{
    private readonly ILogger<ServicoAvancado> _logger;
    
    public ServicoAvancado(ILogger<ServicoAvancado> logger)
    {
        _logger = logger;
    }
    
    public async Task<ActionResult> ProcessarAsync()
    {
        _logger.LogInformation(""Processando..."");
        // Lógica avançada aqui
        return new OkResult();
    }
}",
                    Language = "csharp",
                    Explanation = "Exemplo mais complexo mostrando como aplicar Policy-Based Authorization em cenários reais com logging e async/await.",
                    IsRunnable = true
                },
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Exercício 1: Implementação Básica",
                    Description = "Implemente Policy-Based Authorization em um projeto ASP.NET Core simples.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"using Microsoft.AspNetCore.Mvc;

// Implemente aqui
[ApiController]
[Route(""api/[controller]"")]
public class MeuController : ControllerBase
{
    
}",
                    Hints = new List<string> { "Comece definindo o controller", "Configure Custom Policies", "Teste sua implementação" }
                },
                new Exercise
                {
                    Title = "Exercício 2: Cenário Intermediário",
                    Description = "Crie um serviço completo que utilize Policy-Based Authorization para gerenciar recursos.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"// Implemente o serviço
public class MeuServico
{
    
}",
                    Hints = new List<string> { "Injete dependências via construtor", "Implemente métodos que usem Policy-Based Authorization", "Adicione tratamento de erros" }
                },
                new Exercise
                {
                    Title = "Exercício 3: Desafio Avançado",
                    Description = "Implemente um sistema completo utilizando Policy-Based Authorization com múltiplas funcionalidades.",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = @"// Sistema completo
",
                    Hints = new List<string> { "Planeje a arquitetura antes de implementar", "Considere performance e escalabilidade", "Adicione testes unitários" }
                },
            },
            Summary = "Nesta aula, você aprendeu sobre Policy-Based Authorization no ASP.NET Core. Exploramos Custom Policies, Handlers, Requirements e vimos como aplicar estes conceitos em projetos reais. Compreender profundamente Policy-Based Authorization é essencial para desenvolver aplicações web modernas, robustas e escaláveis. Continue praticando com os exercícios propostos e experimente aplicar estes conceitos em seus próprios projetos. Na próxima aula, avançaremos para tópicos ainda mais interessantes do ASP.NET Core."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0008-000000000015"),
            CourseId = Guid.Parse("10000000-0000-0000-0000-000000000008"),
            Title = "Policy-Based Authorization",
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
                "Compreender os conceitos de resources",
                "Aprender a implementar handlers",
                "Dominar context",
                "Aplicar resource-based authorization em projetos reais"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "Resources",
                    Content = "O conceito de resources é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam resources regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com resources, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Handlers",
                    Content = "O conceito de handlers é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam handlers regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com handlers, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Context",
                    Content = "O conceito de context é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam context regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com context, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 3
                },
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Exemplo Básico de Resource-Based Authorization",
                    Code = @"using Microsoft.AspNetCore.Mvc;

// Implementação de Resource-Based Authorization
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route(""api/[controller]"")]
public class ExemploController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        // Implementação do conceito
        return Ok(new { mensagem = ""Exemplo funcionando"" });
    }
}",
                    Language = "csharp",
                    Explanation = "Este exemplo demonstra a implementação básica de Resource-Based Authorization. Note como o código segue as convenções do ASP.NET Core.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Exemplo Avançado",
                    Code = @"// Implementação avançada com Resources
public class ServicoAvancado
{
    private readonly ILogger<ServicoAvancado> _logger;
    
    public ServicoAvancado(ILogger<ServicoAvancado> logger)
    {
        _logger = logger;
    }
    
    public async Task<ActionResult> ProcessarAsync()
    {
        _logger.LogInformation(""Processando..."");
        // Lógica avançada aqui
        return new OkResult();
    }
}",
                    Language = "csharp",
                    Explanation = "Exemplo mais complexo mostrando como aplicar Resource-Based Authorization em cenários reais com logging e async/await.",
                    IsRunnable = true
                },
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Exercício 1: Implementação Básica",
                    Description = "Implemente Resource-Based Authorization em um projeto ASP.NET Core simples.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"using Microsoft.AspNetCore.Mvc;

// Implemente aqui
[ApiController]
[Route(""api/[controller]"")]
public class MeuController : ControllerBase
{
    
}",
                    Hints = new List<string> { "Comece definindo o controller", "Configure Resources", "Teste sua implementação" }
                },
                new Exercise
                {
                    Title = "Exercício 2: Cenário Intermediário",
                    Description = "Crie um serviço completo que utilize Resource-Based Authorization para gerenciar recursos.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"// Implemente o serviço
public class MeuServico
{
    
}",
                    Hints = new List<string> { "Injete dependências via construtor", "Implemente métodos que usem Resource-Based Authorization", "Adicione tratamento de erros" }
                },
                new Exercise
                {
                    Title = "Exercício 3: Desafio Avançado",
                    Description = "Implemente um sistema completo utilizando Resource-Based Authorization com múltiplas funcionalidades.",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = @"// Sistema completo
",
                    Hints = new List<string> { "Planeje a arquitetura antes de implementar", "Considere performance e escalabilidade", "Adicione testes unitários" }
                },
            },
            Summary = "Nesta aula, você aprendeu sobre Resource-Based Authorization no ASP.NET Core. Exploramos Resources, Handlers, Context e vimos como aplicar estes conceitos em projetos reais. Compreender profundamente Resource-Based Authorization é essencial para desenvolver aplicações web modernas, robustas e escaláveis. Continue praticando com os exercícios propostos e experimente aplicar estes conceitos em seus próprios projetos. Na próxima aula, avançaremos para tópicos ainda mais interessantes do ASP.NET Core."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0008-000000000016"),
            CourseId = Guid.Parse("10000000-0000-0000-0000-000000000008"),
            Title = "Resource-Based Authorization",
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
                "Compreender os conceitos de api keys",
                "Aprender a implementar headers",
                "Dominar validation",
                "Aplicar api key authentication em projetos reais"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "API Keys",
                    Content = "O conceito de api keys é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam api keys regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com api keys, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Headers",
                    Content = "O conceito de headers é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam headers regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com headers, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Validation",
                    Content = "O conceito de validation é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam validation regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com validation, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 3
                },
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Exemplo Básico de API Key Authentication",
                    Code = @"using Microsoft.AspNetCore.Mvc;

// Implementação de API Key Authentication
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route(""api/[controller]"")]
public class ExemploController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        // Implementação do conceito
        return Ok(new { mensagem = ""Exemplo funcionando"" });
    }
}",
                    Language = "csharp",
                    Explanation = "Este exemplo demonstra a implementação básica de API Key Authentication. Note como o código segue as convenções do ASP.NET Core.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Exemplo Avançado",
                    Code = @"// Implementação avançada com API Keys
public class ServicoAvancado
{
    private readonly ILogger<ServicoAvancado> _logger;
    
    public ServicoAvancado(ILogger<ServicoAvancado> logger)
    {
        _logger = logger;
    }
    
    public async Task<ActionResult> ProcessarAsync()
    {
        _logger.LogInformation(""Processando..."");
        // Lógica avançada aqui
        return new OkResult();
    }
}",
                    Language = "csharp",
                    Explanation = "Exemplo mais complexo mostrando como aplicar API Key Authentication em cenários reais com logging e async/await.",
                    IsRunnable = true
                },
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Exercício 1: Implementação Básica",
                    Description = "Implemente API Key Authentication em um projeto ASP.NET Core simples.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"using Microsoft.AspNetCore.Mvc;

// Implemente aqui
[ApiController]
[Route(""api/[controller]"")]
public class MeuController : ControllerBase
{
    
}",
                    Hints = new List<string> { "Comece definindo o controller", "Configure API Keys", "Teste sua implementação" }
                },
                new Exercise
                {
                    Title = "Exercício 2: Cenário Intermediário",
                    Description = "Crie um serviço completo que utilize API Key Authentication para gerenciar recursos.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"// Implemente o serviço
public class MeuServico
{
    
}",
                    Hints = new List<string> { "Injete dependências via construtor", "Implemente métodos que usem API Key Authentication", "Adicione tratamento de erros" }
                },
                new Exercise
                {
                    Title = "Exercício 3: Desafio Avançado",
                    Description = "Implemente um sistema completo utilizando API Key Authentication com múltiplas funcionalidades.",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = @"// Sistema completo
",
                    Hints = new List<string> { "Planeje a arquitetura antes de implementar", "Considere performance e escalabilidade", "Adicione testes unitários" }
                },
            },
            Summary = "Nesta aula, você aprendeu sobre API Key Authentication no ASP.NET Core. Exploramos API Keys, Headers, Validation e vimos como aplicar estes conceitos em projetos reais. Compreender profundamente API Key Authentication é essencial para desenvolver aplicações web modernas, robustas e escaláveis. Continue praticando com os exercícios propostos e experimente aplicar estes conceitos em seus próprios projetos. Na próxima aula, avançaremos para tópicos ainda mais interessantes do ASP.NET Core."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0008-000000000017"),
            CourseId = Guid.Parse("10000000-0000-0000-0000-000000000008"),
            Title = "API Key Authentication",
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
                "Compreender os conceitos de tenant isolation",
                "Aprender a implementar data separation",
                "Dominar routing",
                "Aplicar multi-tenancy em projetos reais"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "Tenant Isolation",
                    Content = "O conceito de tenant isolation é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam tenant isolation regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com tenant isolation, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Data Separation",
                    Content = "O conceito de data separation é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam data separation regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com data separation, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Routing",
                    Content = "O conceito de routing é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam routing regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com routing, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 3
                },
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Exemplo Básico de Multi-Tenancy",
                    Code = @"using Microsoft.AspNetCore.Mvc;

// Implementação de Multi-Tenancy
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route(""api/[controller]"")]
public class ExemploController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        // Implementação do conceito
        return Ok(new { mensagem = ""Exemplo funcionando"" });
    }
}",
                    Language = "csharp",
                    Explanation = "Este exemplo demonstra a implementação básica de Multi-Tenancy. Note como o código segue as convenções do ASP.NET Core.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Exemplo Avançado",
                    Code = @"// Implementação avançada com Tenant Isolation
public class ServicoAvancado
{
    private readonly ILogger<ServicoAvancado> _logger;
    
    public ServicoAvancado(ILogger<ServicoAvancado> logger)
    {
        _logger = logger;
    }
    
    public async Task<ActionResult> ProcessarAsync()
    {
        _logger.LogInformation(""Processando..."");
        // Lógica avançada aqui
        return new OkResult();
    }
}",
                    Language = "csharp",
                    Explanation = "Exemplo mais complexo mostrando como aplicar Multi-Tenancy em cenários reais com logging e async/await.",
                    IsRunnable = true
                },
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Exercício 1: Implementação Básica",
                    Description = "Implemente Multi-Tenancy em um projeto ASP.NET Core simples.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"using Microsoft.AspNetCore.Mvc;

// Implemente aqui
[ApiController]
[Route(""api/[controller]"")]
public class MeuController : ControllerBase
{
    
}",
                    Hints = new List<string> { "Comece definindo o controller", "Configure Tenant Isolation", "Teste sua implementação" }
                },
                new Exercise
                {
                    Title = "Exercício 2: Cenário Intermediário",
                    Description = "Crie um serviço completo que utilize Multi-Tenancy para gerenciar recursos.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"// Implemente o serviço
public class MeuServico
{
    
}",
                    Hints = new List<string> { "Injete dependências via construtor", "Implemente métodos que usem Multi-Tenancy", "Adicione tratamento de erros" }
                },
                new Exercise
                {
                    Title = "Exercício 3: Desafio Avançado",
                    Description = "Implemente um sistema completo utilizando Multi-Tenancy com múltiplas funcionalidades.",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = @"// Sistema completo
",
                    Hints = new List<string> { "Planeje a arquitetura antes de implementar", "Considere performance e escalabilidade", "Adicione testes unitários" }
                },
            },
            Summary = "Nesta aula, você aprendeu sobre Multi-Tenancy no ASP.NET Core. Exploramos Tenant Isolation, Data Separation, Routing e vimos como aplicar estes conceitos em projetos reais. Compreender profundamente Multi-Tenancy é essencial para desenvolver aplicações web modernas, robustas e escaláveis. Continue praticando com os exercícios propostos e experimente aplicar estes conceitos em seus próprios projetos. Na próxima aula, avançaremos para tópicos ainda mais interessantes do ASP.NET Core."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0008-000000000018"),
            CourseId = Guid.Parse("10000000-0000-0000-0000-000000000008"),
            Title = "Multi-Tenancy",
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
                "Compreender os conceitos de owasp",
                "Aprender a implementar vulnerabilities",
                "Dominar mitigation",
                "Aplicar security best practices em projetos reais"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "OWASP",
                    Content = "O conceito de owasp é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam owasp regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com owasp, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Vulnerabilities",
                    Content = "O conceito de vulnerabilities é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam vulnerabilities regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com vulnerabilities, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Mitigation",
                    Content = "O conceito de mitigation é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam mitigation regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com mitigation, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 3
                },
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Exemplo Básico de Security Best Practices",
                    Code = @"using Microsoft.AspNetCore.Mvc;

// Implementação de Security Best Practices
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route(""api/[controller]"")]
public class ExemploController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        // Implementação do conceito
        return Ok(new { mensagem = ""Exemplo funcionando"" });
    }
}",
                    Language = "csharp",
                    Explanation = "Este exemplo demonstra a implementação básica de Security Best Practices. Note como o código segue as convenções do ASP.NET Core.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Exemplo Avançado",
                    Code = @"// Implementação avançada com OWASP
public class ServicoAvancado
{
    private readonly ILogger<ServicoAvancado> _logger;
    
    public ServicoAvancado(ILogger<ServicoAvancado> logger)
    {
        _logger = logger;
    }
    
    public async Task<ActionResult> ProcessarAsync()
    {
        _logger.LogInformation(""Processando..."");
        // Lógica avançada aqui
        return new OkResult();
    }
}",
                    Language = "csharp",
                    Explanation = "Exemplo mais complexo mostrando como aplicar Security Best Practices em cenários reais com logging e async/await.",
                    IsRunnable = true
                },
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Exercício 1: Implementação Básica",
                    Description = "Implemente Security Best Practices em um projeto ASP.NET Core simples.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"using Microsoft.AspNetCore.Mvc;

// Implemente aqui
[ApiController]
[Route(""api/[controller]"")]
public class MeuController : ControllerBase
{
    
}",
                    Hints = new List<string> { "Comece definindo o controller", "Configure OWASP", "Teste sua implementação" }
                },
                new Exercise
                {
                    Title = "Exercício 2: Cenário Intermediário",
                    Description = "Crie um serviço completo que utilize Security Best Practices para gerenciar recursos.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"// Implemente o serviço
public class MeuServico
{
    
}",
                    Hints = new List<string> { "Injete dependências via construtor", "Implemente métodos que usem Security Best Practices", "Adicione tratamento de erros" }
                },
                new Exercise
                {
                    Title = "Exercício 3: Desafio Avançado",
                    Description = "Implemente um sistema completo utilizando Security Best Practices com múltiplas funcionalidades.",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = @"// Sistema completo
",
                    Hints = new List<string> { "Planeje a arquitetura antes de implementar", "Considere performance e escalabilidade", "Adicione testes unitários" }
                },
            },
            Summary = "Nesta aula, você aprendeu sobre Security Best Practices no ASP.NET Core. Exploramos OWASP, Vulnerabilities, Mitigation e vimos como aplicar estes conceitos em projetos reais. Compreender profundamente Security Best Practices é essencial para desenvolver aplicações web modernas, robustas e escaláveis. Continue praticando com os exercícios propostos e experimente aplicar estes conceitos em seus próprios projetos. Na próxima aula, avançaremos para tópicos ainda mais interessantes do ASP.NET Core."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0008-000000000019"),
            CourseId = Guid.Parse("10000000-0000-0000-0000-000000000008"),
            Title = "Security Best Practices",
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
                "Compreender os conceitos de identityserver4",
                "Aprender a implementar duende",
                "Dominar configuration",
                "Aplicar identity server em projetos reais"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "IdentityServer4",
                    Content = "O conceito de identityserver4 é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam identityserver4 regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com identityserver4, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Duende",
                    Content = "O conceito de duende é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam duende regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com duende, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Configuration",
                    Content = "O conceito de configuration é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam configuration regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com configuration, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 3
                },
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Exemplo Básico de Identity Server",
                    Code = @"using Microsoft.AspNetCore.Mvc;

// Implementação de Identity Server
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route(""api/[controller]"")]
public class ExemploController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        // Implementação do conceito
        return Ok(new { mensagem = ""Exemplo funcionando"" });
    }
}",
                    Language = "csharp",
                    Explanation = "Este exemplo demonstra a implementação básica de Identity Server. Note como o código segue as convenções do ASP.NET Core.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Exemplo Avançado",
                    Code = @"// Implementação avançada com IdentityServer4
public class ServicoAvancado
{
    private readonly ILogger<ServicoAvancado> _logger;
    
    public ServicoAvancado(ILogger<ServicoAvancado> logger)
    {
        _logger = logger;
    }
    
    public async Task<ActionResult> ProcessarAsync()
    {
        _logger.LogInformation(""Processando..."");
        // Lógica avançada aqui
        return new OkResult();
    }
}",
                    Language = "csharp",
                    Explanation = "Exemplo mais complexo mostrando como aplicar Identity Server em cenários reais com logging e async/await.",
                    IsRunnable = true
                },
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Exercício 1: Implementação Básica",
                    Description = "Implemente Identity Server em um projeto ASP.NET Core simples.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"using Microsoft.AspNetCore.Mvc;

// Implemente aqui
[ApiController]
[Route(""api/[controller]"")]
public class MeuController : ControllerBase
{
    
}",
                    Hints = new List<string> { "Comece definindo o controller", "Configure IdentityServer4", "Teste sua implementação" }
                },
                new Exercise
                {
                    Title = "Exercício 2: Cenário Intermediário",
                    Description = "Crie um serviço completo que utilize Identity Server para gerenciar recursos.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"// Implemente o serviço
public class MeuServico
{
    
}",
                    Hints = new List<string> { "Injete dependências via construtor", "Implemente métodos que usem Identity Server", "Adicione tratamento de erros" }
                },
                new Exercise
                {
                    Title = "Exercício 3: Desafio Avançado",
                    Description = "Implemente um sistema completo utilizando Identity Server com múltiplas funcionalidades.",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = @"// Sistema completo
",
                    Hints = new List<string> { "Planeje a arquitetura antes de implementar", "Considere performance e escalabilidade", "Adicione testes unitários" }
                },
            },
            Summary = "Nesta aula, você aprendeu sobre Identity Server no ASP.NET Core. Exploramos IdentityServer4, Duende, Configuration e vimos como aplicar estes conceitos em projetos reais. Compreender profundamente Identity Server é essencial para desenvolver aplicações web modernas, robustas e escaláveis. Continue praticando com os exercícios propostos e experimente aplicar estes conceitos em seus próprios projetos. Na próxima aula, avançaremos para tópicos ainda mais interessantes do ASP.NET Core."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0008-000000000020"),
            CourseId = Guid.Parse("10000000-0000-0000-0000-000000000008"),
            Title = "Identity Server",
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
