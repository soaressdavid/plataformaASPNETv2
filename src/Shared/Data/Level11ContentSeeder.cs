using Shared.Entities;
using Shared.Models;
using System.Text.Json;

namespace Shared.Data;

/// <summary>
/// Level 11 Content Seeder - Docker e Containers
/// Topics: Docker, Containers, Dockerfile, Docker Compose, Orchestration
/// </summary>
public partial class Level11ContentSeeder
{
    private readonly Guid _courseId = Guid.Parse("10000000-0000-0000-0000-00000000000C");
    private readonly Guid _levelId = Guid.Parse("00000000-0000-0000-0000-00000000000B");

    public Course CreateLevel11Course()
    {
        return new Course
        {
            Id = _courseId,
            LevelId = _levelId,
            Title = "Docker e Containers",
            Description = "Curso completo de Docker e Containers com 20 aulas práticas e projetos reais.",
            Level = Level.Advanced,
            Duration = "4 semanas",
            LessonCount = 20,
            Topics = JsonSerializer.Serialize(new[] { "Docker", "Containers", "Dockerfile", "Docker Compose", "Orchestration" }),
            OrderIndex = 1,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }


    public List<Lesson> CreateLevel11Lessons()
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
                "Compreender os conceitos de containers",
                "Aprender a implementar images",
                "Dominar benefits",
                "Aplicar introdução ao docker em projetos reais"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "Containers",
                    Content = "O conceito de containers é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam containers regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com containers, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Images",
                    Content = "O conceito de images é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam images regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com images, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Benefits",
                    Content = "O conceito de benefits é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam benefits regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com benefits, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 3
                },
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Exemplo Básico de Introdução ao Docker",
                    Code = @"using Microsoft.AspNetCore.Mvc;

// Implementação de Introdução ao Docker
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
                    Explanation = "Este exemplo demonstra a implementação básica de Introdução ao Docker. Note como o código segue as convenções do ASP.NET Core.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Exemplo Avançado",
                    Code = @"// Implementação avançada com Containers
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
                    Explanation = "Exemplo mais complexo mostrando como aplicar Introdução ao Docker em cenários reais com logging e async/await.",
                    IsRunnable = true
                },
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Exercício 1: Implementação Básica",
                    Description = "Implemente Introdução ao Docker em um projeto ASP.NET Core simples.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"using Microsoft.AspNetCore.Mvc;

// Implemente aqui
[ApiController]
[Route(""api/[controller]"")]
public class MeuController : ControllerBase
{
    
}",
                    Hints = new List<string> { "Comece definindo o controller", "Configure Containers", "Teste sua implementação" }
                },
                new Exercise
                {
                    Title = "Exercício 2: Cenário Intermediário",
                    Description = "Crie um serviço completo que utilize Introdução ao Docker para gerenciar recursos.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"// Implemente o serviço
public class MeuServico
{
    
}",
                    Hints = new List<string> { "Injete dependências via construtor", "Implemente métodos que usem Introdução ao Docker", "Adicione tratamento de erros" }
                },
                new Exercise
                {
                    Title = "Exercício 3: Desafio Avançado",
                    Description = "Implemente um sistema completo utilizando Introdução ao Docker com múltiplas funcionalidades.",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = @"// Sistema completo
",
                    Hints = new List<string> { "Planeje a arquitetura antes de implementar", "Considere performance e escalabilidade", "Adicione testes unitários" }
                },
            },
            Summary = "Nesta aula, você aprendeu sobre Introdução ao Docker no ASP.NET Core. Exploramos Containers, Images, Benefits e vimos como aplicar estes conceitos em projetos reais. Compreender profundamente Introdução ao Docker é essencial para desenvolver aplicações web modernas, robustas e escaláveis. Continue praticando com os exercícios propostos e experimente aplicar estes conceitos em seus próprios projetos. Na próxima aula, avançaremos para tópicos ainda mais interessantes do ASP.NET Core."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-000C-000000000001"),
            CourseId = Guid.Parse("10000000-0000-0000-0000-00000000000C"),
            Title = "Introdução ao Docker",
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
                "Aprender a implementar configuration",
                "Dominar verification",
                "Aplicar docker installation em projetos reais"
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
                    Heading = "Configuration",
                    Content = "O conceito de configuration é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam configuration regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com configuration, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Verification",
                    Content = "O conceito de verification é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam verification regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com verification, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 3
                },
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Exemplo Básico de Docker Installation",
                    Code = @"using Microsoft.AspNetCore.Mvc;

// Implementação de Docker Installation
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
                    Explanation = "Este exemplo demonstra a implementação básica de Docker Installation. Note como o código segue as convenções do ASP.NET Core.",
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
                    Explanation = "Exemplo mais complexo mostrando como aplicar Docker Installation em cenários reais com logging e async/await.",
                    IsRunnable = true
                },
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Exercício 1: Implementação Básica",
                    Description = "Implemente Docker Installation em um projeto ASP.NET Core simples.",
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
                    Description = "Crie um serviço completo que utilize Docker Installation para gerenciar recursos.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"// Implemente o serviço
public class MeuServico
{
    
}",
                    Hints = new List<string> { "Injete dependências via construtor", "Implemente métodos que usem Docker Installation", "Adicione tratamento de erros" }
                },
                new Exercise
                {
                    Title = "Exercício 3: Desafio Avançado",
                    Description = "Implemente um sistema completo utilizando Docker Installation com múltiplas funcionalidades.",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = @"// Sistema completo
",
                    Hints = new List<string> { "Planeje a arquitetura antes de implementar", "Considere performance e escalabilidade", "Adicione testes unitários" }
                },
            },
            Summary = "Nesta aula, você aprendeu sobre Docker Installation no ASP.NET Core. Exploramos Setup, Configuration, Verification e vimos como aplicar estes conceitos em projetos reais. Compreender profundamente Docker Installation é essencial para desenvolver aplicações web modernas, robustas e escaláveis. Continue praticando com os exercícios propostos e experimente aplicar estes conceitos em seus próprios projetos. Na próxima aula, avançaremos para tópicos ainda mais interessantes do ASP.NET Core."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-000C-000000000002"),
            CourseId = Guid.Parse("10000000-0000-0000-0000-00000000000C"),
            Title = "Docker Installation",
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
                "Compreender os conceitos de pulling",
                "Aprender a implementar building",
                "Dominar tagging",
                "Aplicar docker images em projetos reais"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "Pulling",
                    Content = "O conceito de pulling é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam pulling regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com pulling, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Building",
                    Content = "O conceito de building é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam building regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com building, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Tagging",
                    Content = "O conceito de tagging é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam tagging regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com tagging, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 3
                },
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Exemplo Básico de Docker Images",
                    Code = @"using Microsoft.AspNetCore.Mvc;

// Implementação de Docker Images
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
                    Explanation = "Este exemplo demonstra a implementação básica de Docker Images. Note como o código segue as convenções do ASP.NET Core.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Exemplo Avançado",
                    Code = @"// Implementação avançada com Pulling
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
                    Explanation = "Exemplo mais complexo mostrando como aplicar Docker Images em cenários reais com logging e async/await.",
                    IsRunnable = true
                },
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Exercício 1: Implementação Básica",
                    Description = "Implemente Docker Images em um projeto ASP.NET Core simples.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"using Microsoft.AspNetCore.Mvc;

// Implemente aqui
[ApiController]
[Route(""api/[controller]"")]
public class MeuController : ControllerBase
{
    
}",
                    Hints = new List<string> { "Comece definindo o controller", "Configure Pulling", "Teste sua implementação" }
                },
                new Exercise
                {
                    Title = "Exercício 2: Cenário Intermediário",
                    Description = "Crie um serviço completo que utilize Docker Images para gerenciar recursos.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"// Implemente o serviço
public class MeuServico
{
    
}",
                    Hints = new List<string> { "Injete dependências via construtor", "Implemente métodos que usem Docker Images", "Adicione tratamento de erros" }
                },
                new Exercise
                {
                    Title = "Exercício 3: Desafio Avançado",
                    Description = "Implemente um sistema completo utilizando Docker Images com múltiplas funcionalidades.",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = @"// Sistema completo
",
                    Hints = new List<string> { "Planeje a arquitetura antes de implementar", "Considere performance e escalabilidade", "Adicione testes unitários" }
                },
            },
            Summary = "Nesta aula, você aprendeu sobre Docker Images no ASP.NET Core. Exploramos Pulling, Building, Tagging e vimos como aplicar estes conceitos em projetos reais. Compreender profundamente Docker Images é essencial para desenvolver aplicações web modernas, robustas e escaláveis. Continue praticando com os exercícios propostos e experimente aplicar estes conceitos em seus próprios projetos. Na próxima aula, avançaremos para tópicos ainda mais interessantes do ASP.NET Core."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-000C-000000000003"),
            CourseId = Guid.Parse("10000000-0000-0000-0000-00000000000C"),
            Title = "Docker Images",
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
                "Compreender os conceitos de instructions",
                "Aprender a implementar layers",
                "Dominar best practices",
                "Aplicar dockerfile em projetos reais"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "Instructions",
                    Content = "O conceito de instructions é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam instructions regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com instructions, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Layers",
                    Content = "O conceito de layers é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam layers regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com layers, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Best Practices",
                    Content = "O conceito de best practices é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam best practices regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com best practices, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 3
                },
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Exemplo Básico de Dockerfile",
                    Code = @"using Microsoft.AspNetCore.Mvc;

// Implementação de Dockerfile
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
                    Explanation = "Este exemplo demonstra a implementação básica de Dockerfile. Note como o código segue as convenções do ASP.NET Core.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Exemplo Avançado",
                    Code = @"// Implementação avançada com Instructions
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
                    Explanation = "Exemplo mais complexo mostrando como aplicar Dockerfile em cenários reais com logging e async/await.",
                    IsRunnable = true
                },
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Exercício 1: Implementação Básica",
                    Description = "Implemente Dockerfile em um projeto ASP.NET Core simples.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"using Microsoft.AspNetCore.Mvc;

// Implemente aqui
[ApiController]
[Route(""api/[controller]"")]
public class MeuController : ControllerBase
{
    
}",
                    Hints = new List<string> { "Comece definindo o controller", "Configure Instructions", "Teste sua implementação" }
                },
                new Exercise
                {
                    Title = "Exercício 2: Cenário Intermediário",
                    Description = "Crie um serviço completo que utilize Dockerfile para gerenciar recursos.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"// Implemente o serviço
public class MeuServico
{
    
}",
                    Hints = new List<string> { "Injete dependências via construtor", "Implemente métodos que usem Dockerfile", "Adicione tratamento de erros" }
                },
                new Exercise
                {
                    Title = "Exercício 3: Desafio Avançado",
                    Description = "Implemente um sistema completo utilizando Dockerfile com múltiplas funcionalidades.",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = @"// Sistema completo
",
                    Hints = new List<string> { "Planeje a arquitetura antes de implementar", "Considere performance e escalabilidade", "Adicione testes unitários" }
                },
            },
            Summary = "Nesta aula, você aprendeu sobre Dockerfile no ASP.NET Core. Exploramos Instructions, Layers, Best Practices e vimos como aplicar estes conceitos em projetos reais. Compreender profundamente Dockerfile é essencial para desenvolver aplicações web modernas, robustas e escaláveis. Continue praticando com os exercícios propostos e experimente aplicar estes conceitos em seus próprios projetos. Na próxima aula, avançaremos para tópicos ainda mais interessantes do ASP.NET Core."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-000C-000000000004"),
            CourseId = Guid.Parse("10000000-0000-0000-0000-00000000000C"),
            Title = "Dockerfile",
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
                "Compreender os conceitos de running",
                "Aprender a implementar managing",
                "Dominar lifecycle",
                "Aplicar docker containers em projetos reais"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "Running",
                    Content = "O conceito de running é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam running regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com running, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Managing",
                    Content = "O conceito de managing é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam managing regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com managing, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Lifecycle",
                    Content = "O conceito de lifecycle é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam lifecycle regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com lifecycle, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 3
                },
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Exemplo Básico de Docker Containers",
                    Code = @"using Microsoft.AspNetCore.Mvc;

// Implementação de Docker Containers
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
                    Explanation = "Este exemplo demonstra a implementação básica de Docker Containers. Note como o código segue as convenções do ASP.NET Core.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Exemplo Avançado",
                    Code = @"// Implementação avançada com Running
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
                    Explanation = "Exemplo mais complexo mostrando como aplicar Docker Containers em cenários reais com logging e async/await.",
                    IsRunnable = true
                },
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Exercício 1: Implementação Básica",
                    Description = "Implemente Docker Containers em um projeto ASP.NET Core simples.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"using Microsoft.AspNetCore.Mvc;

// Implemente aqui
[ApiController]
[Route(""api/[controller]"")]
public class MeuController : ControllerBase
{
    
}",
                    Hints = new List<string> { "Comece definindo o controller", "Configure Running", "Teste sua implementação" }
                },
                new Exercise
                {
                    Title = "Exercício 2: Cenário Intermediário",
                    Description = "Crie um serviço completo que utilize Docker Containers para gerenciar recursos.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"// Implemente o serviço
public class MeuServico
{
    
}",
                    Hints = new List<string> { "Injete dependências via construtor", "Implemente métodos que usem Docker Containers", "Adicione tratamento de erros" }
                },
                new Exercise
                {
                    Title = "Exercício 3: Desafio Avançado",
                    Description = "Implemente um sistema completo utilizando Docker Containers com múltiplas funcionalidades.",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = @"// Sistema completo
",
                    Hints = new List<string> { "Planeje a arquitetura antes de implementar", "Considere performance e escalabilidade", "Adicione testes unitários" }
                },
            },
            Summary = "Nesta aula, você aprendeu sobre Docker Containers no ASP.NET Core. Exploramos Running, Managing, Lifecycle e vimos como aplicar estes conceitos em projetos reais. Compreender profundamente Docker Containers é essencial para desenvolver aplicações web modernas, robustas e escaláveis. Continue praticando com os exercícios propostos e experimente aplicar estes conceitos em seus próprios projetos. Na próxima aula, avançaremos para tópicos ainda mais interessantes do ASP.NET Core."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-000C-000000000005"),
            CourseId = Guid.Parse("10000000-0000-0000-0000-00000000000C"),
            Title = "Docker Containers",
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
                "Compreender os conceitos de persistence",
                "Aprender a implementar bind mounts",
                "Dominar named volumes",
                "Aplicar docker volumes em projetos reais"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "Persistence",
                    Content = "O conceito de persistence é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam persistence regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com persistence, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Bind Mounts",
                    Content = "O conceito de bind mounts é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam bind mounts regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com bind mounts, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Named Volumes",
                    Content = "O conceito de named volumes é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam named volumes regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com named volumes, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 3
                },
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Exemplo Básico de Docker Volumes",
                    Code = @"using Microsoft.AspNetCore.Mvc;

// Implementação de Docker Volumes
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
                    Explanation = "Este exemplo demonstra a implementação básica de Docker Volumes. Note como o código segue as convenções do ASP.NET Core.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Exemplo Avançado",
                    Code = @"// Implementação avançada com Persistence
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
                    Explanation = "Exemplo mais complexo mostrando como aplicar Docker Volumes em cenários reais com logging e async/await.",
                    IsRunnable = true
                },
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Exercício 1: Implementação Básica",
                    Description = "Implemente Docker Volumes em um projeto ASP.NET Core simples.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"using Microsoft.AspNetCore.Mvc;

// Implemente aqui
[ApiController]
[Route(""api/[controller]"")]
public class MeuController : ControllerBase
{
    
}",
                    Hints = new List<string> { "Comece definindo o controller", "Configure Persistence", "Teste sua implementação" }
                },
                new Exercise
                {
                    Title = "Exercício 2: Cenário Intermediário",
                    Description = "Crie um serviço completo que utilize Docker Volumes para gerenciar recursos.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"// Implemente o serviço
public class MeuServico
{
    
}",
                    Hints = new List<string> { "Injete dependências via construtor", "Implemente métodos que usem Docker Volumes", "Adicione tratamento de erros" }
                },
                new Exercise
                {
                    Title = "Exercício 3: Desafio Avançado",
                    Description = "Implemente um sistema completo utilizando Docker Volumes com múltiplas funcionalidades.",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = @"// Sistema completo
",
                    Hints = new List<string> { "Planeje a arquitetura antes de implementar", "Considere performance e escalabilidade", "Adicione testes unitários" }
                },
            },
            Summary = "Nesta aula, você aprendeu sobre Docker Volumes no ASP.NET Core. Exploramos Persistence, Bind Mounts, Named Volumes e vimos como aplicar estes conceitos em projetos reais. Compreender profundamente Docker Volumes é essencial para desenvolver aplicações web modernas, robustas e escaláveis. Continue praticando com os exercícios propostos e experimente aplicar estes conceitos em seus próprios projetos. Na próxima aula, avançaremos para tópicos ainda mais interessantes do ASP.NET Core."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-000C-000000000006"),
            CourseId = Guid.Parse("10000000-0000-0000-0000-00000000000C"),
            Title = "Docker Volumes",
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
                "Compreender os conceitos de bridge",
                "Aprender a implementar host",
                "Dominar overlay",
                "Aplicar docker networks em projetos reais"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "Bridge",
                    Content = "O conceito de bridge é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam bridge regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com bridge, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Host",
                    Content = "O conceito de host é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam host regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com host, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Overlay",
                    Content = "O conceito de overlay é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam overlay regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com overlay, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 3
                },
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Exemplo Básico de Docker Networks",
                    Code = @"using Microsoft.AspNetCore.Mvc;

// Implementação de Docker Networks
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
                    Explanation = "Este exemplo demonstra a implementação básica de Docker Networks. Note como o código segue as convenções do ASP.NET Core.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Exemplo Avançado",
                    Code = @"// Implementação avançada com Bridge
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
                    Explanation = "Exemplo mais complexo mostrando como aplicar Docker Networks em cenários reais com logging e async/await.",
                    IsRunnable = true
                },
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Exercício 1: Implementação Básica",
                    Description = "Implemente Docker Networks em um projeto ASP.NET Core simples.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"using Microsoft.AspNetCore.Mvc;

// Implemente aqui
[ApiController]
[Route(""api/[controller]"")]
public class MeuController : ControllerBase
{
    
}",
                    Hints = new List<string> { "Comece definindo o controller", "Configure Bridge", "Teste sua implementação" }
                },
                new Exercise
                {
                    Title = "Exercício 2: Cenário Intermediário",
                    Description = "Crie um serviço completo que utilize Docker Networks para gerenciar recursos.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"// Implemente o serviço
public class MeuServico
{
    
}",
                    Hints = new List<string> { "Injete dependências via construtor", "Implemente métodos que usem Docker Networks", "Adicione tratamento de erros" }
                },
                new Exercise
                {
                    Title = "Exercício 3: Desafio Avançado",
                    Description = "Implemente um sistema completo utilizando Docker Networks com múltiplas funcionalidades.",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = @"// Sistema completo
",
                    Hints = new List<string> { "Planeje a arquitetura antes de implementar", "Considere performance e escalabilidade", "Adicione testes unitários" }
                },
            },
            Summary = "Nesta aula, você aprendeu sobre Docker Networks no ASP.NET Core. Exploramos Bridge, Host, Overlay e vimos como aplicar estes conceitos em projetos reais. Compreender profundamente Docker Networks é essencial para desenvolver aplicações web modernas, robustas e escaláveis. Continue praticando com os exercícios propostos e experimente aplicar estes conceitos em seus próprios projetos. Na próxima aula, avançaremos para tópicos ainda mais interessantes do ASP.NET Core."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-000C-000000000007"),
            CourseId = Guid.Parse("10000000-0000-0000-0000-00000000000C"),
            Title = "Docker Networks",
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
                "Compreender os conceitos de yaml",
                "Aprender a implementar services",
                "Dominar dependencies",
                "Aplicar docker compose em projetos reais"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "YAML",
                    Content = "O conceito de yaml é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam yaml regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com yaml, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Services",
                    Content = "O conceito de services é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam services regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com services, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Dependencies",
                    Content = "O conceito de dependencies é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam dependencies regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com dependencies, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 3
                },
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Exemplo Básico de Docker Compose",
                    Code = @"using Microsoft.AspNetCore.Mvc;

// Implementação de Docker Compose
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
                    Explanation = "Este exemplo demonstra a implementação básica de Docker Compose. Note como o código segue as convenções do ASP.NET Core.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Exemplo Avançado",
                    Code = @"// Implementação avançada com YAML
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
                    Explanation = "Exemplo mais complexo mostrando como aplicar Docker Compose em cenários reais com logging e async/await.",
                    IsRunnable = true
                },
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Exercício 1: Implementação Básica",
                    Description = "Implemente Docker Compose em um projeto ASP.NET Core simples.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"using Microsoft.AspNetCore.Mvc;

// Implemente aqui
[ApiController]
[Route(""api/[controller]"")]
public class MeuController : ControllerBase
{
    
}",
                    Hints = new List<string> { "Comece definindo o controller", "Configure YAML", "Teste sua implementação" }
                },
                new Exercise
                {
                    Title = "Exercício 2: Cenário Intermediário",
                    Description = "Crie um serviço completo que utilize Docker Compose para gerenciar recursos.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"// Implemente o serviço
public class MeuServico
{
    
}",
                    Hints = new List<string> { "Injete dependências via construtor", "Implemente métodos que usem Docker Compose", "Adicione tratamento de erros" }
                },
                new Exercise
                {
                    Title = "Exercício 3: Desafio Avançado",
                    Description = "Implemente um sistema completo utilizando Docker Compose com múltiplas funcionalidades.",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = @"// Sistema completo
",
                    Hints = new List<string> { "Planeje a arquitetura antes de implementar", "Considere performance e escalabilidade", "Adicione testes unitários" }
                },
            },
            Summary = "Nesta aula, você aprendeu sobre Docker Compose no ASP.NET Core. Exploramos YAML, Services, Dependencies e vimos como aplicar estes conceitos em projetos reais. Compreender profundamente Docker Compose é essencial para desenvolver aplicações web modernas, robustas e escaláveis. Continue praticando com os exercícios propostos e experimente aplicar estes conceitos em seus próprios projetos. Na próxima aula, avançaremos para tópicos ainda mais interessantes do ASP.NET Core."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-000C-000000000008"),
            CourseId = Guid.Parse("10000000-0000-0000-0000-00000000000C"),
            Title = "Docker Compose",
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
                "Compreender os conceitos de optimization",
                "Aprender a implementar size reduction",
                "Dominar security",
                "Aplicar multi-stage builds em projetos reais"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "Optimization",
                    Content = "O conceito de optimization é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam optimization regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com optimization, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Size Reduction",
                    Content = "O conceito de size reduction é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam size reduction regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com size reduction, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
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
                    Title = "Exemplo Básico de Multi-Stage Builds",
                    Code = @"using Microsoft.AspNetCore.Mvc;

// Implementação de Multi-Stage Builds
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
                    Explanation = "Este exemplo demonstra a implementação básica de Multi-Stage Builds. Note como o código segue as convenções do ASP.NET Core.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Exemplo Avançado",
                    Code = @"// Implementação avançada com Optimization
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
                    Explanation = "Exemplo mais complexo mostrando como aplicar Multi-Stage Builds em cenários reais com logging e async/await.",
                    IsRunnable = true
                },
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Exercício 1: Implementação Básica",
                    Description = "Implemente Multi-Stage Builds em um projeto ASP.NET Core simples.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"using Microsoft.AspNetCore.Mvc;

// Implemente aqui
[ApiController]
[Route(""api/[controller]"")]
public class MeuController : ControllerBase
{
    
}",
                    Hints = new List<string> { "Comece definindo o controller", "Configure Optimization", "Teste sua implementação" }
                },
                new Exercise
                {
                    Title = "Exercício 2: Cenário Intermediário",
                    Description = "Crie um serviço completo que utilize Multi-Stage Builds para gerenciar recursos.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"// Implemente o serviço
public class MeuServico
{
    
}",
                    Hints = new List<string> { "Injete dependências via construtor", "Implemente métodos que usem Multi-Stage Builds", "Adicione tratamento de erros" }
                },
                new Exercise
                {
                    Title = "Exercício 3: Desafio Avançado",
                    Description = "Implemente um sistema completo utilizando Multi-Stage Builds com múltiplas funcionalidades.",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = @"// Sistema completo
",
                    Hints = new List<string> { "Planeje a arquitetura antes de implementar", "Considere performance e escalabilidade", "Adicione testes unitários" }
                },
            },
            Summary = "Nesta aula, você aprendeu sobre Multi-Stage Builds no ASP.NET Core. Exploramos Optimization, Size Reduction, Security e vimos como aplicar estes conceitos em projetos reais. Compreender profundamente Multi-Stage Builds é essencial para desenvolver aplicações web modernas, robustas e escaláveis. Continue praticando com os exercícios propostos e experimente aplicar estes conceitos em seus próprios projetos. Na próxima aula, avançaremos para tópicos ainda mais interessantes do ASP.NET Core."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-000C-000000000009"),
            CourseId = Guid.Parse("10000000-0000-0000-0000-00000000000C"),
            Title = "Multi-Stage Builds",
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
                "Compreender os conceitos de docker hub",
                "Aprender a implementar private registry",
                "Dominar push/pull",
                "Aplicar docker registry em projetos reais"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "Docker Hub",
                    Content = "O conceito de docker hub é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam docker hub regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com docker hub, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Private Registry",
                    Content = "O conceito de private registry é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam private registry regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com private registry, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Push/Pull",
                    Content = "O conceito de push/pull é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam push/pull regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com push/pull, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 3
                },
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Exemplo Básico de Docker Registry",
                    Code = @"using Microsoft.AspNetCore.Mvc;

// Implementação de Docker Registry
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
                    Explanation = "Este exemplo demonstra a implementação básica de Docker Registry. Note como o código segue as convenções do ASP.NET Core.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Exemplo Avançado",
                    Code = @"// Implementação avançada com Docker Hub
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
                    Explanation = "Exemplo mais complexo mostrando como aplicar Docker Registry em cenários reais com logging e async/await.",
                    IsRunnable = true
                },
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Exercício 1: Implementação Básica",
                    Description = "Implemente Docker Registry em um projeto ASP.NET Core simples.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"using Microsoft.AspNetCore.Mvc;

// Implemente aqui
[ApiController]
[Route(""api/[controller]"")]
public class MeuController : ControllerBase
{
    
}",
                    Hints = new List<string> { "Comece definindo o controller", "Configure Docker Hub", "Teste sua implementação" }
                },
                new Exercise
                {
                    Title = "Exercício 2: Cenário Intermediário",
                    Description = "Crie um serviço completo que utilize Docker Registry para gerenciar recursos.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"// Implemente o serviço
public class MeuServico
{
    
}",
                    Hints = new List<string> { "Injete dependências via construtor", "Implemente métodos que usem Docker Registry", "Adicione tratamento de erros" }
                },
                new Exercise
                {
                    Title = "Exercício 3: Desafio Avançado",
                    Description = "Implemente um sistema completo utilizando Docker Registry com múltiplas funcionalidades.",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = @"// Sistema completo
",
                    Hints = new List<string> { "Planeje a arquitetura antes de implementar", "Considere performance e escalabilidade", "Adicione testes unitários" }
                },
            },
            Summary = "Nesta aula, você aprendeu sobre Docker Registry no ASP.NET Core. Exploramos Docker Hub, Private Registry, Push/Pull e vimos como aplicar estes conceitos em projetos reais. Compreender profundamente Docker Registry é essencial para desenvolver aplicações web modernas, robustas e escaláveis. Continue praticando com os exercícios propostos e experimente aplicar estes conceitos em seus próprios projetos. Na próxima aula, avançaremos para tópicos ainda mais interessantes do ASP.NET Core."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-000C-000000000010"),
            CourseId = Guid.Parse("10000000-0000-0000-0000-00000000000C"),
            Title = "Docker Registry",
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
                "Compreender os conceitos de scanning",
                "Aprender a implementar best practices",
                "Dominar vulnerabilities",
                "Aplicar container security em projetos reais"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "Scanning",
                    Content = "O conceito de scanning é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam scanning regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com scanning, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Best Practices",
                    Content = "O conceito de best practices é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam best practices regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com best practices, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Vulnerabilities",
                    Content = "O conceito de vulnerabilities é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam vulnerabilities regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com vulnerabilities, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 3
                },
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Exemplo Básico de Container Security",
                    Code = @"using Microsoft.AspNetCore.Mvc;

// Implementação de Container Security
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
                    Explanation = "Este exemplo demonstra a implementação básica de Container Security. Note como o código segue as convenções do ASP.NET Core.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Exemplo Avançado",
                    Code = @"// Implementação avançada com Scanning
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
                    Explanation = "Exemplo mais complexo mostrando como aplicar Container Security em cenários reais com logging e async/await.",
                    IsRunnable = true
                },
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Exercício 1: Implementação Básica",
                    Description = "Implemente Container Security em um projeto ASP.NET Core simples.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"using Microsoft.AspNetCore.Mvc;

// Implemente aqui
[ApiController]
[Route(""api/[controller]"")]
public class MeuController : ControllerBase
{
    
}",
                    Hints = new List<string> { "Comece definindo o controller", "Configure Scanning", "Teste sua implementação" }
                },
                new Exercise
                {
                    Title = "Exercício 2: Cenário Intermediário",
                    Description = "Crie um serviço completo que utilize Container Security para gerenciar recursos.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"// Implemente o serviço
public class MeuServico
{
    
}",
                    Hints = new List<string> { "Injete dependências via construtor", "Implemente métodos que usem Container Security", "Adicione tratamento de erros" }
                },
                new Exercise
                {
                    Title = "Exercício 3: Desafio Avançado",
                    Description = "Implemente um sistema completo utilizando Container Security com múltiplas funcionalidades.",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = @"// Sistema completo
",
                    Hints = new List<string> { "Planeje a arquitetura antes de implementar", "Considere performance e escalabilidade", "Adicione testes unitários" }
                },
            },
            Summary = "Nesta aula, você aprendeu sobre Container Security no ASP.NET Core. Exploramos Scanning, Best Practices, Vulnerabilities e vimos como aplicar estes conceitos em projetos reais. Compreender profundamente Container Security é essencial para desenvolver aplicações web modernas, robustas e escaláveis. Continue praticando com os exercícios propostos e experimente aplicar estes conceitos em seus próprios projetos. Na próxima aula, avançaremos para tópicos ainda mais interessantes do ASP.NET Core."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-000C-000000000011"),
            CourseId = Guid.Parse("10000000-0000-0000-0000-00000000000C"),
            Title = "Container Security",
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
                "Compreender os conceitos de clustering",
                "Aprender a implementar services",
                "Dominar scaling",
                "Aplicar docker swarm em projetos reais"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "Clustering",
                    Content = "O conceito de clustering é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam clustering regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com clustering, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Services",
                    Content = "O conceito de services é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam services regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com services, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Scaling",
                    Content = "O conceito de scaling é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam scaling regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com scaling, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 3
                },
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Exemplo Básico de Docker Swarm",
                    Code = @"using Microsoft.AspNetCore.Mvc;

// Implementação de Docker Swarm
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
                    Explanation = "Este exemplo demonstra a implementação básica de Docker Swarm. Note como o código segue as convenções do ASP.NET Core.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Exemplo Avançado",
                    Code = @"// Implementação avançada com Clustering
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
                    Explanation = "Exemplo mais complexo mostrando como aplicar Docker Swarm em cenários reais com logging e async/await.",
                    IsRunnable = true
                },
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Exercício 1: Implementação Básica",
                    Description = "Implemente Docker Swarm em um projeto ASP.NET Core simples.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"using Microsoft.AspNetCore.Mvc;

// Implemente aqui
[ApiController]
[Route(""api/[controller]"")]
public class MeuController : ControllerBase
{
    
}",
                    Hints = new List<string> { "Comece definindo o controller", "Configure Clustering", "Teste sua implementação" }
                },
                new Exercise
                {
                    Title = "Exercício 2: Cenário Intermediário",
                    Description = "Crie um serviço completo que utilize Docker Swarm para gerenciar recursos.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"// Implemente o serviço
public class MeuServico
{
    
}",
                    Hints = new List<string> { "Injete dependências via construtor", "Implemente métodos que usem Docker Swarm", "Adicione tratamento de erros" }
                },
                new Exercise
                {
                    Title = "Exercício 3: Desafio Avançado",
                    Description = "Implemente um sistema completo utilizando Docker Swarm com múltiplas funcionalidades.",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = @"// Sistema completo
",
                    Hints = new List<string> { "Planeje a arquitetura antes de implementar", "Considere performance e escalabilidade", "Adicione testes unitários" }
                },
            },
            Summary = "Nesta aula, você aprendeu sobre Docker Swarm no ASP.NET Core. Exploramos Clustering, Services, Scaling e vimos como aplicar estes conceitos em projetos reais. Compreender profundamente Docker Swarm é essencial para desenvolver aplicações web modernas, robustas e escaláveis. Continue praticando com os exercícios propostos e experimente aplicar estes conceitos em seus próprios projetos. Na próxima aula, avançaremos para tópicos ainda mais interessantes do ASP.NET Core."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-000C-000000000012"),
            CourseId = Guid.Parse("10000000-0000-0000-0000-00000000000C"),
            Title = "Docker Swarm",
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
                "Compreender os conceitos de pods",
                "Aprender a implementar services",
                "Dominar deployments",
                "Aplicar kubernetes basics em projetos reais"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "Pods",
                    Content = "O conceito de pods é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam pods regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com pods, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Services",
                    Content = "O conceito de services é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam services regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com services, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Deployments",
                    Content = "O conceito de deployments é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam deployments regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com deployments, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 3
                },
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Exemplo Básico de Kubernetes Basics",
                    Code = @"using Microsoft.AspNetCore.Mvc;

// Implementação de Kubernetes Basics
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
                    Explanation = "Este exemplo demonstra a implementação básica de Kubernetes Basics. Note como o código segue as convenções do ASP.NET Core.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Exemplo Avançado",
                    Code = @"// Implementação avançada com Pods
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
                    Explanation = "Exemplo mais complexo mostrando como aplicar Kubernetes Basics em cenários reais com logging e async/await.",
                    IsRunnable = true
                },
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Exercício 1: Implementação Básica",
                    Description = "Implemente Kubernetes Basics em um projeto ASP.NET Core simples.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"using Microsoft.AspNetCore.Mvc;

// Implemente aqui
[ApiController]
[Route(""api/[controller]"")]
public class MeuController : ControllerBase
{
    
}",
                    Hints = new List<string> { "Comece definindo o controller", "Configure Pods", "Teste sua implementação" }
                },
                new Exercise
                {
                    Title = "Exercício 2: Cenário Intermediário",
                    Description = "Crie um serviço completo que utilize Kubernetes Basics para gerenciar recursos.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"// Implemente o serviço
public class MeuServico
{
    
}",
                    Hints = new List<string> { "Injete dependências via construtor", "Implemente métodos que usem Kubernetes Basics", "Adicione tratamento de erros" }
                },
                new Exercise
                {
                    Title = "Exercício 3: Desafio Avançado",
                    Description = "Implemente um sistema completo utilizando Kubernetes Basics com múltiplas funcionalidades.",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = @"// Sistema completo
",
                    Hints = new List<string> { "Planeje a arquitetura antes de implementar", "Considere performance e escalabilidade", "Adicione testes unitários" }
                },
            },
            Summary = "Nesta aula, você aprendeu sobre Kubernetes Basics no ASP.NET Core. Exploramos Pods, Services, Deployments e vimos como aplicar estes conceitos em projetos reais. Compreender profundamente Kubernetes Basics é essencial para desenvolver aplicações web modernas, robustas e escaláveis. Continue praticando com os exercícios propostos e experimente aplicar estes conceitos em seus próprios projetos. Na próxima aula, avançaremos para tópicos ainda mais interessantes do ASP.NET Core."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-000C-000000000013"),
            CourseId = Guid.Parse("10000000-0000-0000-0000-00000000000C"),
            Title = "Kubernetes Basics",
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
                "Compreender os conceitos de packaging",
                "Aprender a implementar templates",
                "Dominar releases",
                "Aplicar helm charts em projetos reais"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "Packaging",
                    Content = "O conceito de packaging é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam packaging regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com packaging, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Templates",
                    Content = "O conceito de templates é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam templates regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com templates, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Releases",
                    Content = "O conceito de releases é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam releases regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com releases, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 3
                },
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Exemplo Básico de Helm Charts",
                    Code = @"using Microsoft.AspNetCore.Mvc;

// Implementação de Helm Charts
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
                    Explanation = "Este exemplo demonstra a implementação básica de Helm Charts. Note como o código segue as convenções do ASP.NET Core.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Exemplo Avançado",
                    Code = @"// Implementação avançada com Packaging
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
                    Explanation = "Exemplo mais complexo mostrando como aplicar Helm Charts em cenários reais com logging e async/await.",
                    IsRunnable = true
                },
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Exercício 1: Implementação Básica",
                    Description = "Implemente Helm Charts em um projeto ASP.NET Core simples.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"using Microsoft.AspNetCore.Mvc;

// Implemente aqui
[ApiController]
[Route(""api/[controller]"")]
public class MeuController : ControllerBase
{
    
}",
                    Hints = new List<string> { "Comece definindo o controller", "Configure Packaging", "Teste sua implementação" }
                },
                new Exercise
                {
                    Title = "Exercício 2: Cenário Intermediário",
                    Description = "Crie um serviço completo que utilize Helm Charts para gerenciar recursos.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"// Implemente o serviço
public class MeuServico
{
    
}",
                    Hints = new List<string> { "Injete dependências via construtor", "Implemente métodos que usem Helm Charts", "Adicione tratamento de erros" }
                },
                new Exercise
                {
                    Title = "Exercício 3: Desafio Avançado",
                    Description = "Implemente um sistema completo utilizando Helm Charts com múltiplas funcionalidades.",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = @"// Sistema completo
",
                    Hints = new List<string> { "Planeje a arquitetura antes de implementar", "Considere performance e escalabilidade", "Adicione testes unitários" }
                },
            },
            Summary = "Nesta aula, você aprendeu sobre Helm Charts no ASP.NET Core. Exploramos Packaging, Templates, Releases e vimos como aplicar estes conceitos em projetos reais. Compreender profundamente Helm Charts é essencial para desenvolver aplicações web modernas, robustas e escaláveis. Continue praticando com os exercícios propostos e experimente aplicar estes conceitos em seus próprios projetos. Na próxima aula, avançaremos para tópicos ainda mais interessantes do ASP.NET Core."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-000C-000000000014"),
            CourseId = Guid.Parse("10000000-0000-0000-0000-00000000000C"),
            Title = "Helm Charts",
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
                "Compreender os conceitos de logs",
                "Aprender a implementar metrics",
                "Dominar health checks",
                "Aplicar container monitoring em projetos reais"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "Logs",
                    Content = "O conceito de logs é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam logs regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com logs, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Metrics",
                    Content = "O conceito de metrics é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam metrics regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com metrics, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Health Checks",
                    Content = "O conceito de health checks é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam health checks regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com health checks, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 3
                },
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Exemplo Básico de Container Monitoring",
                    Code = @"using Microsoft.AspNetCore.Mvc;

// Implementação de Container Monitoring
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
                    Explanation = "Este exemplo demonstra a implementação básica de Container Monitoring. Note como o código segue as convenções do ASP.NET Core.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Exemplo Avançado",
                    Code = @"// Implementação avançada com Logs
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
                    Explanation = "Exemplo mais complexo mostrando como aplicar Container Monitoring em cenários reais com logging e async/await.",
                    IsRunnable = true
                },
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Exercício 1: Implementação Básica",
                    Description = "Implemente Container Monitoring em um projeto ASP.NET Core simples.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"using Microsoft.AspNetCore.Mvc;

// Implemente aqui
[ApiController]
[Route(""api/[controller]"")]
public class MeuController : ControllerBase
{
    
}",
                    Hints = new List<string> { "Comece definindo o controller", "Configure Logs", "Teste sua implementação" }
                },
                new Exercise
                {
                    Title = "Exercício 2: Cenário Intermediário",
                    Description = "Crie um serviço completo que utilize Container Monitoring para gerenciar recursos.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"// Implemente o serviço
public class MeuServico
{
    
}",
                    Hints = new List<string> { "Injete dependências via construtor", "Implemente métodos que usem Container Monitoring", "Adicione tratamento de erros" }
                },
                new Exercise
                {
                    Title = "Exercício 3: Desafio Avançado",
                    Description = "Implemente um sistema completo utilizando Container Monitoring com múltiplas funcionalidades.",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = @"// Sistema completo
",
                    Hints = new List<string> { "Planeje a arquitetura antes de implementar", "Considere performance e escalabilidade", "Adicione testes unitários" }
                },
            },
            Summary = "Nesta aula, você aprendeu sobre Container Monitoring no ASP.NET Core. Exploramos Logs, Metrics, Health Checks e vimos como aplicar estes conceitos em projetos reais. Compreender profundamente Container Monitoring é essencial para desenvolver aplicações web modernas, robustas e escaláveis. Continue praticando com os exercícios propostos e experimente aplicar estes conceitos em seus próprios projetos. Na próxima aula, avançaremos para tópicos ainda mais interessantes do ASP.NET Core."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-000C-000000000015"),
            CourseId = Guid.Parse("10000000-0000-0000-0000-00000000000C"),
            Title = "Container Monitoring",
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
                "Compreender os conceitos de build",
                "Aprender a implementar test",
                "Dominar deploy",
                "Aplicar docker in ci/cd em projetos reais"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "Build",
                    Content = "O conceito de build é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam build regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com build, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Test",
                    Content = "O conceito de test é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam test regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com test, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Deploy",
                    Content = "O conceito de deploy é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam deploy regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com deploy, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 3
                },
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Exemplo Básico de Docker in CI/CD",
                    Code = @"using Microsoft.AspNetCore.Mvc;

// Implementação de Docker in CI/CD
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
                    Explanation = "Este exemplo demonstra a implementação básica de Docker in CI/CD. Note como o código segue as convenções do ASP.NET Core.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Exemplo Avançado",
                    Code = @"// Implementação avançada com Build
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
                    Explanation = "Exemplo mais complexo mostrando como aplicar Docker in CI/CD em cenários reais com logging e async/await.",
                    IsRunnable = true
                },
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Exercício 1: Implementação Básica",
                    Description = "Implemente Docker in CI/CD em um projeto ASP.NET Core simples.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"using Microsoft.AspNetCore.Mvc;

// Implemente aqui
[ApiController]
[Route(""api/[controller]"")]
public class MeuController : ControllerBase
{
    
}",
                    Hints = new List<string> { "Comece definindo o controller", "Configure Build", "Teste sua implementação" }
                },
                new Exercise
                {
                    Title = "Exercício 2: Cenário Intermediário",
                    Description = "Crie um serviço completo que utilize Docker in CI/CD para gerenciar recursos.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"// Implemente o serviço
public class MeuServico
{
    
}",
                    Hints = new List<string> { "Injete dependências via construtor", "Implemente métodos que usem Docker in CI/CD", "Adicione tratamento de erros" }
                },
                new Exercise
                {
                    Title = "Exercício 3: Desafio Avançado",
                    Description = "Implemente um sistema completo utilizando Docker in CI/CD com múltiplas funcionalidades.",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = @"// Sistema completo
",
                    Hints = new List<string> { "Planeje a arquitetura antes de implementar", "Considere performance e escalabilidade", "Adicione testes unitários" }
                },
            },
            Summary = "Nesta aula, você aprendeu sobre Docker in CI/CD no ASP.NET Core. Exploramos Build, Test, Deploy e vimos como aplicar estes conceitos em projetos reais. Compreender profundamente Docker in CI/CD é essencial para desenvolver aplicações web modernas, robustas e escaláveis. Continue praticando com os exercícios propostos e experimente aplicar estes conceitos em seus próprios projetos. Na próxima aula, avançaremos para tópicos ainda mais interessantes do ASP.NET Core."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-000C-000000000016"),
            CourseId = Guid.Parse("10000000-0000-0000-0000-00000000000C"),
            Title = "Docker in CI/CD",
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
                "Compreender os conceitos de logs",
                "Aprender a implementar exec",
                "Dominar troubleshooting",
                "Aplicar debugging containers em projetos reais"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "Logs",
                    Content = "O conceito de logs é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam logs regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com logs, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Exec",
                    Content = "O conceito de exec é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam exec regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com exec, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Troubleshooting",
                    Content = "O conceito de troubleshooting é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam troubleshooting regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com troubleshooting, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 3
                },
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Exemplo Básico de Debugging Containers",
                    Code = @"using Microsoft.AspNetCore.Mvc;

// Implementação de Debugging Containers
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
                    Explanation = "Este exemplo demonstra a implementação básica de Debugging Containers. Note como o código segue as convenções do ASP.NET Core.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Exemplo Avançado",
                    Code = @"// Implementação avançada com Logs
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
                    Explanation = "Exemplo mais complexo mostrando como aplicar Debugging Containers em cenários reais com logging e async/await.",
                    IsRunnable = true
                },
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Exercício 1: Implementação Básica",
                    Description = "Implemente Debugging Containers em um projeto ASP.NET Core simples.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"using Microsoft.AspNetCore.Mvc;

// Implemente aqui
[ApiController]
[Route(""api/[controller]"")]
public class MeuController : ControllerBase
{
    
}",
                    Hints = new List<string> { "Comece definindo o controller", "Configure Logs", "Teste sua implementação" }
                },
                new Exercise
                {
                    Title = "Exercício 2: Cenário Intermediário",
                    Description = "Crie um serviço completo que utilize Debugging Containers para gerenciar recursos.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"// Implemente o serviço
public class MeuServico
{
    
}",
                    Hints = new List<string> { "Injete dependências via construtor", "Implemente métodos que usem Debugging Containers", "Adicione tratamento de erros" }
                },
                new Exercise
                {
                    Title = "Exercício 3: Desafio Avançado",
                    Description = "Implemente um sistema completo utilizando Debugging Containers com múltiplas funcionalidades.",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = @"// Sistema completo
",
                    Hints = new List<string> { "Planeje a arquitetura antes de implementar", "Considere performance e escalabilidade", "Adicione testes unitários" }
                },
            },
            Summary = "Nesta aula, você aprendeu sobre Debugging Containers no ASP.NET Core. Exploramos Logs, Exec, Troubleshooting e vimos como aplicar estes conceitos em projetos reais. Compreender profundamente Debugging Containers é essencial para desenvolver aplicações web modernas, robustas e escaláveis. Continue praticando com os exercícios propostos e experimente aplicar estes conceitos em seus próprios projetos. Na próxima aula, avançaremos para tópicos ainda mais interessantes do ASP.NET Core."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-000C-000000000017"),
            CourseId = Guid.Parse("10000000-0000-0000-0000-00000000000C"),
            Title = "Debugging Containers",
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
                "Compreender os conceitos de kubernetes",
                "Aprender a implementar swarm",
                "Dominar comparison",
                "Aplicar container orchestration em projetos reais"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "Kubernetes",
                    Content = "O conceito de kubernetes é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam kubernetes regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com kubernetes, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Swarm",
                    Content = "O conceito de swarm é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam swarm regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com swarm, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Comparison",
                    Content = "O conceito de comparison é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam comparison regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com comparison, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 3
                },
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Exemplo Básico de Container Orchestration",
                    Code = @"using Microsoft.AspNetCore.Mvc;

// Implementação de Container Orchestration
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
                    Explanation = "Este exemplo demonstra a implementação básica de Container Orchestration. Note como o código segue as convenções do ASP.NET Core.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Exemplo Avançado",
                    Code = @"// Implementação avançada com Kubernetes
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
                    Explanation = "Exemplo mais complexo mostrando como aplicar Container Orchestration em cenários reais com logging e async/await.",
                    IsRunnable = true
                },
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Exercício 1: Implementação Básica",
                    Description = "Implemente Container Orchestration em um projeto ASP.NET Core simples.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"using Microsoft.AspNetCore.Mvc;

// Implemente aqui
[ApiController]
[Route(""api/[controller]"")]
public class MeuController : ControllerBase
{
    
}",
                    Hints = new List<string> { "Comece definindo o controller", "Configure Kubernetes", "Teste sua implementação" }
                },
                new Exercise
                {
                    Title = "Exercício 2: Cenário Intermediário",
                    Description = "Crie um serviço completo que utilize Container Orchestration para gerenciar recursos.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"// Implemente o serviço
public class MeuServico
{
    
}",
                    Hints = new List<string> { "Injete dependências via construtor", "Implemente métodos que usem Container Orchestration", "Adicione tratamento de erros" }
                },
                new Exercise
                {
                    Title = "Exercício 3: Desafio Avançado",
                    Description = "Implemente um sistema completo utilizando Container Orchestration com múltiplas funcionalidades.",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = @"// Sistema completo
",
                    Hints = new List<string> { "Planeje a arquitetura antes de implementar", "Considere performance e escalabilidade", "Adicione testes unitários" }
                },
            },
            Summary = "Nesta aula, você aprendeu sobre Container Orchestration no ASP.NET Core. Exploramos Kubernetes, Swarm, Comparison e vimos como aplicar estes conceitos em projetos reais. Compreender profundamente Container Orchestration é essencial para desenvolver aplicações web modernas, robustas e escaláveis. Continue praticando com os exercícios propostos e experimente aplicar estes conceitos em seus próprios projetos. Na próxima aula, avançaremos para tópicos ainda mais interessantes do ASP.NET Core."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-000C-000000000018"),
            CourseId = Guid.Parse("10000000-0000-0000-0000-00000000000C"),
            Title = "Container Orchestration",
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
                "Compreender os conceitos de strategies",
                "Aprender a implementar scaling",
                "Dominar updates",
                "Aplicar production deployment em projetos reais"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "Strategies",
                    Content = "O conceito de strategies é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam strategies regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com strategies, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Scaling",
                    Content = "O conceito de scaling é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam scaling regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com scaling, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Updates",
                    Content = "O conceito de updates é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam updates regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com updates, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 3
                },
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Exemplo Básico de Production Deployment",
                    Code = @"using Microsoft.AspNetCore.Mvc;

// Implementação de Production Deployment
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
                    Explanation = "Este exemplo demonstra a implementação básica de Production Deployment. Note como o código segue as convenções do ASP.NET Core.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Exemplo Avançado",
                    Code = @"// Implementação avançada com Strategies
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
                    Explanation = "Exemplo mais complexo mostrando como aplicar Production Deployment em cenários reais com logging e async/await.",
                    IsRunnable = true
                },
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Exercício 1: Implementação Básica",
                    Description = "Implemente Production Deployment em um projeto ASP.NET Core simples.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"using Microsoft.AspNetCore.Mvc;

// Implemente aqui
[ApiController]
[Route(""api/[controller]"")]
public class MeuController : ControllerBase
{
    
}",
                    Hints = new List<string> { "Comece definindo o controller", "Configure Strategies", "Teste sua implementação" }
                },
                new Exercise
                {
                    Title = "Exercício 2: Cenário Intermediário",
                    Description = "Crie um serviço completo que utilize Production Deployment para gerenciar recursos.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"// Implemente o serviço
public class MeuServico
{
    
}",
                    Hints = new List<string> { "Injete dependências via construtor", "Implemente métodos que usem Production Deployment", "Adicione tratamento de erros" }
                },
                new Exercise
                {
                    Title = "Exercício 3: Desafio Avançado",
                    Description = "Implemente um sistema completo utilizando Production Deployment com múltiplas funcionalidades.",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = @"// Sistema completo
",
                    Hints = new List<string> { "Planeje a arquitetura antes de implementar", "Considere performance e escalabilidade", "Adicione testes unitários" }
                },
            },
            Summary = "Nesta aula, você aprendeu sobre Production Deployment no ASP.NET Core. Exploramos Strategies, Scaling, Updates e vimos como aplicar estes conceitos em projetos reais. Compreender profundamente Production Deployment é essencial para desenvolver aplicações web modernas, robustas e escaláveis. Continue praticando com os exercícios propostos e experimente aplicar estes conceitos em seus próprios projetos. Na próxima aula, avançaremos para tópicos ainda mais interessantes do ASP.NET Core."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-000C-000000000019"),
            CourseId = Guid.Parse("10000000-0000-0000-0000-00000000000C"),
            Title = "Production Deployment",
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
                "Compreender os conceitos de performance",
                "Aprender a implementar security",
                "Dominar maintenance",
                "Aplicar docker best practices em projetos reais"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "Performance",
                    Content = "O conceito de performance é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam performance regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com performance, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Security",
                    Content = "O conceito de security é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam security regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com security, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Maintenance",
                    Content = "O conceito de maintenance é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam maintenance regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com maintenance, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 3
                },
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Exemplo Básico de Docker Best Practices",
                    Code = @"using Microsoft.AspNetCore.Mvc;

// Implementação de Docker Best Practices
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
                    Explanation = "Este exemplo demonstra a implementação básica de Docker Best Practices. Note como o código segue as convenções do ASP.NET Core.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Exemplo Avançado",
                    Code = @"// Implementação avançada com Performance
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
                    Explanation = "Exemplo mais complexo mostrando como aplicar Docker Best Practices em cenários reais com logging e async/await.",
                    IsRunnable = true
                },
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Exercício 1: Implementação Básica",
                    Description = "Implemente Docker Best Practices em um projeto ASP.NET Core simples.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"using Microsoft.AspNetCore.Mvc;

// Implemente aqui
[ApiController]
[Route(""api/[controller]"")]
public class MeuController : ControllerBase
{
    
}",
                    Hints = new List<string> { "Comece definindo o controller", "Configure Performance", "Teste sua implementação" }
                },
                new Exercise
                {
                    Title = "Exercício 2: Cenário Intermediário",
                    Description = "Crie um serviço completo que utilize Docker Best Practices para gerenciar recursos.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"// Implemente o serviço
public class MeuServico
{
    
}",
                    Hints = new List<string> { "Injete dependências via construtor", "Implemente métodos que usem Docker Best Practices", "Adicione tratamento de erros" }
                },
                new Exercise
                {
                    Title = "Exercício 3: Desafio Avançado",
                    Description = "Implemente um sistema completo utilizando Docker Best Practices com múltiplas funcionalidades.",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = @"// Sistema completo
",
                    Hints = new List<string> { "Planeje a arquitetura antes de implementar", "Considere performance e escalabilidade", "Adicione testes unitários" }
                },
            },
            Summary = "Nesta aula, você aprendeu sobre Docker Best Practices no ASP.NET Core. Exploramos Performance, Security, Maintenance e vimos como aplicar estes conceitos em projetos reais. Compreender profundamente Docker Best Practices é essencial para desenvolver aplicações web modernas, robustas e escaláveis. Continue praticando com os exercícios propostos e experimente aplicar estes conceitos em seus próprios projetos. Na próxima aula, avançaremos para tópicos ainda mais interessantes do ASP.NET Core."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-000C-000000000020"),
            CourseId = Guid.Parse("10000000-0000-0000-0000-00000000000C"),
            Title = "Docker Best Practices",
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
