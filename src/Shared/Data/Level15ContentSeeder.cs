using Shared.Entities;
using Shared.Models;
using System.Text.Json;

namespace Shared.Data;

/// <summary>
/// Level 15 Content Seeder - Liderança Técnica
/// Topics: Leadership, Code Review, Mentoring, Documentation, Architecture Decisions
/// </summary>
public partial class Level15ContentSeeder
{
    private readonly Guid _courseId = Guid.Parse("10000000-0000-0000-0000-000000000010");
    private readonly Guid _levelId = Guid.Parse("00000000-0000-0000-0000-00000000000F");

    public Course CreateLevel15Course()
    {
        return new Course
        {
            Id = _courseId,
            LevelId = _levelId,
            Title = "Liderança Técnica",
            Description = "Curso completo de Liderança Técnica com 20 aulas práticas e projetos reais.",
            Level = Level.Advanced,
            Duration = "5 semanas",
            LessonCount = 20,
            Topics = JsonSerializer.Serialize(new[] { "Leadership", "Code Review", "Mentoring", "Documentation", "Architecture Decisions" }),
            OrderIndex = 1,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }


    public List<Lesson> CreateLevel15Lessons()
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
                "Compreender os conceitos de responsibilities",
                "Aprender a implementar skills",
                "Dominar challenges",
                "Aplicar papel do tech lead em projetos reais"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "Responsibilities",
                    Content = "O conceito de responsibilities é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam responsibilities regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com responsibilities, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Skills",
                    Content = "O conceito de skills é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam skills regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com skills, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Challenges",
                    Content = "O conceito de challenges é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam challenges regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com challenges, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 3
                },
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Exemplo Básico de Papel do Tech Lead",
                    Code = @"using Microsoft.AspNetCore.Mvc;

// Implementação de Papel do Tech Lead
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
                    Explanation = "Este exemplo demonstra a implementação básica de Papel do Tech Lead. Note como o código segue as convenções do ASP.NET Core.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Exemplo Avançado",
                    Code = @"// Implementação avançada com Responsibilities
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
                    Explanation = "Exemplo mais complexo mostrando como aplicar Papel do Tech Lead em cenários reais com logging e async/await.",
                    IsRunnable = true
                },
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Exercício 1: Implementação Básica",
                    Description = "Implemente Papel do Tech Lead em um projeto ASP.NET Core simples.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"using Microsoft.AspNetCore.Mvc;

// Implemente aqui
[ApiController]
[Route(""api/[controller]"")]
public class MeuController : ControllerBase
{
    
}",
                    Hints = new List<string> { "Comece definindo o controller", "Configure Responsibilities", "Teste sua implementação" }
                },
                new Exercise
                {
                    Title = "Exercício 2: Cenário Intermediário",
                    Description = "Crie um serviço completo que utilize Papel do Tech Lead para gerenciar recursos.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"// Implemente o serviço
public class MeuServico
{
    
}",
                    Hints = new List<string> { "Injete dependências via construtor", "Implemente métodos que usem Papel do Tech Lead", "Adicione tratamento de erros" }
                },
                new Exercise
                {
                    Title = "Exercício 3: Desafio Avançado",
                    Description = "Implemente um sistema completo utilizando Papel do Tech Lead com múltiplas funcionalidades.",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = @"// Sistema completo
",
                    Hints = new List<string> { "Planeje a arquitetura antes de implementar", "Considere performance e escalabilidade", "Adicione testes unitários" }
                },
            },
            Summary = "Nesta aula, você aprendeu sobre Papel do Tech Lead no ASP.NET Core. Exploramos Responsibilities, Skills, Challenges e vimos como aplicar estes conceitos em projetos reais. Compreender profundamente Papel do Tech Lead é essencial para desenvolver aplicações web modernas, robustas e escaláveis. Continue praticando com os exercícios propostos e experimente aplicar estes conceitos em seus próprios projetos. Na próxima aula, avançaremos para tópicos ainda mais interessantes do ASP.NET Core."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0010-000000000001"),
            CourseId = Guid.Parse("10000000-0000-0000-0000-000000000010"),
            Title = "Papel do Tech Lead",
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
                "Compreender os conceitos de leadership styles",
                "Aprender a implementar communication",
                "Dominar influence",
                "Aplicar liderança técnica em projetos reais"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "Leadership Styles",
                    Content = "O conceito de leadership styles é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam leadership styles regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com leadership styles, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Communication",
                    Content = "O conceito de communication é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam communication regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com communication, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Influence",
                    Content = "O conceito de influence é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam influence regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com influence, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 3
                },
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Exemplo Básico de Liderança Técnica",
                    Code = @"using Microsoft.AspNetCore.Mvc;

// Implementação de Liderança Técnica
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
                    Explanation = "Este exemplo demonstra a implementação básica de Liderança Técnica. Note como o código segue as convenções do ASP.NET Core.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Exemplo Avançado",
                    Code = @"// Implementação avançada com Leadership Styles
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
                    Explanation = "Exemplo mais complexo mostrando como aplicar Liderança Técnica em cenários reais com logging e async/await.",
                    IsRunnable = true
                },
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Exercício 1: Implementação Básica",
                    Description = "Implemente Liderança Técnica em um projeto ASP.NET Core simples.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"using Microsoft.AspNetCore.Mvc;

// Implemente aqui
[ApiController]
[Route(""api/[controller]"")]
public class MeuController : ControllerBase
{
    
}",
                    Hints = new List<string> { "Comece definindo o controller", "Configure Leadership Styles", "Teste sua implementação" }
                },
                new Exercise
                {
                    Title = "Exercício 2: Cenário Intermediário",
                    Description = "Crie um serviço completo que utilize Liderança Técnica para gerenciar recursos.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"// Implemente o serviço
public class MeuServico
{
    
}",
                    Hints = new List<string> { "Injete dependências via construtor", "Implemente métodos que usem Liderança Técnica", "Adicione tratamento de erros" }
                },
                new Exercise
                {
                    Title = "Exercício 3: Desafio Avançado",
                    Description = "Implemente um sistema completo utilizando Liderança Técnica com múltiplas funcionalidades.",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = @"// Sistema completo
",
                    Hints = new List<string> { "Planeje a arquitetura antes de implementar", "Considere performance e escalabilidade", "Adicione testes unitários" }
                },
            },
            Summary = "Nesta aula, você aprendeu sobre Liderança Técnica no ASP.NET Core. Exploramos Leadership Styles, Communication, Influence e vimos como aplicar estes conceitos em projetos reais. Compreender profundamente Liderança Técnica é essencial para desenvolver aplicações web modernas, robustas e escaláveis. Continue praticando com os exercícios propostos e experimente aplicar estes conceitos em seus próprios projetos. Na próxima aula, avançaremos para tópicos ainda mais interessantes do ASP.NET Core."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0010-000000000002"),
            CourseId = Guid.Parse("10000000-0000-0000-0000-000000000010"),
            Title = "Liderança Técnica",
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
                "Compreender os conceitos de best practices",
                "Aprender a implementar feedback",
                "Dominar tools",
                "Aplicar code review em projetos reais"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "Best Practices",
                    Content = "O conceito de best practices é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam best practices regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com best practices, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Feedback",
                    Content = "O conceito de feedback é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam feedback regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com feedback, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Tools",
                    Content = "O conceito de tools é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam tools regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com tools, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 3
                },
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Exemplo Básico de Code Review",
                    Code = @"using Microsoft.AspNetCore.Mvc;

// Implementação de Code Review
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
                    Explanation = "Este exemplo demonstra a implementação básica de Code Review. Note como o código segue as convenções do ASP.NET Core.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Exemplo Avançado",
                    Code = @"// Implementação avançada com Best Practices
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
                    Explanation = "Exemplo mais complexo mostrando como aplicar Code Review em cenários reais com logging e async/await.",
                    IsRunnable = true
                },
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Exercício 1: Implementação Básica",
                    Description = "Implemente Code Review em um projeto ASP.NET Core simples.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"using Microsoft.AspNetCore.Mvc;

// Implemente aqui
[ApiController]
[Route(""api/[controller]"")]
public class MeuController : ControllerBase
{
    
}",
                    Hints = new List<string> { "Comece definindo o controller", "Configure Best Practices", "Teste sua implementação" }
                },
                new Exercise
                {
                    Title = "Exercício 2: Cenário Intermediário",
                    Description = "Crie um serviço completo que utilize Code Review para gerenciar recursos.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"// Implemente o serviço
public class MeuServico
{
    
}",
                    Hints = new List<string> { "Injete dependências via construtor", "Implemente métodos que usem Code Review", "Adicione tratamento de erros" }
                },
                new Exercise
                {
                    Title = "Exercício 3: Desafio Avançado",
                    Description = "Implemente um sistema completo utilizando Code Review com múltiplas funcionalidades.",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = @"// Sistema completo
",
                    Hints = new List<string> { "Planeje a arquitetura antes de implementar", "Considere performance e escalabilidade", "Adicione testes unitários" }
                },
            },
            Summary = "Nesta aula, você aprendeu sobre Code Review no ASP.NET Core. Exploramos Best Practices, Feedback, Tools e vimos como aplicar estes conceitos em projetos reais. Compreender profundamente Code Review é essencial para desenvolver aplicações web modernas, robustas e escaláveis. Continue praticando com os exercícios propostos e experimente aplicar estes conceitos em seus próprios projetos. Na próxima aula, avançaremos para tópicos ainda mais interessantes do ASP.NET Core."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0010-000000000003"),
            CourseId = Guid.Parse("10000000-0000-0000-0000-000000000010"),
            Title = "Code Review",
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
                "Compreender os conceitos de coaching",
                "Aprender a implementar knowledge transfer",
                "Dominar growth",
                "Aplicar mentoria em projetos reais"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "Coaching",
                    Content = "O conceito de coaching é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam coaching regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com coaching, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Knowledge Transfer",
                    Content = "O conceito de knowledge transfer é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam knowledge transfer regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com knowledge transfer, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Growth",
                    Content = "O conceito de growth é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam growth regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com growth, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 3
                },
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Exemplo Básico de Mentoria",
                    Code = @"using Microsoft.AspNetCore.Mvc;

// Implementação de Mentoria
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
                    Explanation = "Este exemplo demonstra a implementação básica de Mentoria. Note como o código segue as convenções do ASP.NET Core.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Exemplo Avançado",
                    Code = @"// Implementação avançada com Coaching
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
                    Explanation = "Exemplo mais complexo mostrando como aplicar Mentoria em cenários reais com logging e async/await.",
                    IsRunnable = true
                },
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Exercício 1: Implementação Básica",
                    Description = "Implemente Mentoria em um projeto ASP.NET Core simples.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"using Microsoft.AspNetCore.Mvc;

// Implemente aqui
[ApiController]
[Route(""api/[controller]"")]
public class MeuController : ControllerBase
{
    
}",
                    Hints = new List<string> { "Comece definindo o controller", "Configure Coaching", "Teste sua implementação" }
                },
                new Exercise
                {
                    Title = "Exercício 2: Cenário Intermediário",
                    Description = "Crie um serviço completo que utilize Mentoria para gerenciar recursos.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"// Implemente o serviço
public class MeuServico
{
    
}",
                    Hints = new List<string> { "Injete dependências via construtor", "Implemente métodos que usem Mentoria", "Adicione tratamento de erros" }
                },
                new Exercise
                {
                    Title = "Exercício 3: Desafio Avançado",
                    Description = "Implemente um sistema completo utilizando Mentoria com múltiplas funcionalidades.",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = @"// Sistema completo
",
                    Hints = new List<string> { "Planeje a arquitetura antes de implementar", "Considere performance e escalabilidade", "Adicione testes unitários" }
                },
            },
            Summary = "Nesta aula, você aprendeu sobre Mentoria no ASP.NET Core. Exploramos Coaching, Knowledge Transfer, Growth e vimos como aplicar estes conceitos em projetos reais. Compreender profundamente Mentoria é essencial para desenvolver aplicações web modernas, robustas e escaláveis. Continue praticando com os exercícios propostos e experimente aplicar estes conceitos em seus próprios projetos. Na próxima aula, avançaremos para tópicos ainda mais interessantes do ASP.NET Core."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0010-000000000004"),
            CourseId = Guid.Parse("10000000-0000-0000-0000-000000000010"),
            Title = "Mentoria",
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
                "Compreender os conceitos de adrs",
                "Aprender a implementar rfcs",
                "Dominar runbooks",
                "Aplicar documentação técnica em projetos reais"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "ADRs",
                    Content = "O conceito de adrs é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam adrs regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com adrs, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "RFCs",
                    Content = "O conceito de rfcs é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam rfcs regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com rfcs, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Runbooks",
                    Content = "O conceito de runbooks é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam runbooks regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com runbooks, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 3
                },
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Exemplo Básico de Documentação Técnica",
                    Code = @"using Microsoft.AspNetCore.Mvc;

// Implementação de Documentação Técnica
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
                    Explanation = "Este exemplo demonstra a implementação básica de Documentação Técnica. Note como o código segue as convenções do ASP.NET Core.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Exemplo Avançado",
                    Code = @"// Implementação avançada com ADRs
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
                    Explanation = "Exemplo mais complexo mostrando como aplicar Documentação Técnica em cenários reais com logging e async/await.",
                    IsRunnable = true
                },
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Exercício 1: Implementação Básica",
                    Description = "Implemente Documentação Técnica em um projeto ASP.NET Core simples.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"using Microsoft.AspNetCore.Mvc;

// Implemente aqui
[ApiController]
[Route(""api/[controller]"")]
public class MeuController : ControllerBase
{
    
}",
                    Hints = new List<string> { "Comece definindo o controller", "Configure ADRs", "Teste sua implementação" }
                },
                new Exercise
                {
                    Title = "Exercício 2: Cenário Intermediário",
                    Description = "Crie um serviço completo que utilize Documentação Técnica para gerenciar recursos.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"// Implemente o serviço
public class MeuServico
{
    
}",
                    Hints = new List<string> { "Injete dependências via construtor", "Implemente métodos que usem Documentação Técnica", "Adicione tratamento de erros" }
                },
                new Exercise
                {
                    Title = "Exercício 3: Desafio Avançado",
                    Description = "Implemente um sistema completo utilizando Documentação Técnica com múltiplas funcionalidades.",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = @"// Sistema completo
",
                    Hints = new List<string> { "Planeje a arquitetura antes de implementar", "Considere performance e escalabilidade", "Adicione testes unitários" }
                },
            },
            Summary = "Nesta aula, você aprendeu sobre Documentação Técnica no ASP.NET Core. Exploramos ADRs, RFCs, Runbooks e vimos como aplicar estes conceitos em projetos reais. Compreender profundamente Documentação Técnica é essencial para desenvolver aplicações web modernas, robustas e escaláveis. Continue praticando com os exercícios propostos e experimente aplicar estes conceitos em seus próprios projetos. Na próxima aula, avançaremos para tópicos ainda mais interessantes do ASP.NET Core."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0010-000000000005"),
            CourseId = Guid.Parse("10000000-0000-0000-0000-000000000010"),
            Title = "Documentação Técnica",
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
                "Compreender os conceitos de trade-offs",
                "Aprender a implementar documentation",
                "Dominar communication",
                "Aplicar decisões arquiteturais em projetos reais"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "Trade-offs",
                    Content = "O conceito de trade-offs é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam trade-offs regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com trade-offs, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Documentation",
                    Content = "O conceito de documentation é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam documentation regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com documentation, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Communication",
                    Content = "O conceito de communication é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam communication regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com communication, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 3
                },
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Exemplo Básico de Decisões Arquiteturais",
                    Code = @"using Microsoft.AspNetCore.Mvc;

// Implementação de Decisões Arquiteturais
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
                    Explanation = "Este exemplo demonstra a implementação básica de Decisões Arquiteturais. Note como o código segue as convenções do ASP.NET Core.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Exemplo Avançado",
                    Code = @"// Implementação avançada com Trade-offs
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
                    Explanation = "Exemplo mais complexo mostrando como aplicar Decisões Arquiteturais em cenários reais com logging e async/await.",
                    IsRunnable = true
                },
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Exercício 1: Implementação Básica",
                    Description = "Implemente Decisões Arquiteturais em um projeto ASP.NET Core simples.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"using Microsoft.AspNetCore.Mvc;

// Implemente aqui
[ApiController]
[Route(""api/[controller]"")]
public class MeuController : ControllerBase
{
    
}",
                    Hints = new List<string> { "Comece definindo o controller", "Configure Trade-offs", "Teste sua implementação" }
                },
                new Exercise
                {
                    Title = "Exercício 2: Cenário Intermediário",
                    Description = "Crie um serviço completo que utilize Decisões Arquiteturais para gerenciar recursos.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"// Implemente o serviço
public class MeuServico
{
    
}",
                    Hints = new List<string> { "Injete dependências via construtor", "Implemente métodos que usem Decisões Arquiteturais", "Adicione tratamento de erros" }
                },
                new Exercise
                {
                    Title = "Exercício 3: Desafio Avançado",
                    Description = "Implemente um sistema completo utilizando Decisões Arquiteturais com múltiplas funcionalidades.",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = @"// Sistema completo
",
                    Hints = new List<string> { "Planeje a arquitetura antes de implementar", "Considere performance e escalabilidade", "Adicione testes unitários" }
                },
            },
            Summary = "Nesta aula, você aprendeu sobre Decisões Arquiteturais no ASP.NET Core. Exploramos Trade-offs, Documentation, Communication e vimos como aplicar estes conceitos em projetos reais. Compreender profundamente Decisões Arquiteturais é essencial para desenvolver aplicações web modernas, robustas e escaláveis. Continue praticando com os exercícios propostos e experimente aplicar estes conceitos em seus próprios projetos. Na próxima aula, avançaremos para tópicos ainda mais interessantes do ASP.NET Core."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0010-000000000006"),
            CourseId = Guid.Parse("10000000-0000-0000-0000-000000000010"),
            Title = "Decisões Arquiteturais",
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
                "Compreender os conceitos de identification",
                "Aprender a implementar prioritization",
                "Dominar remediation",
                "Aplicar gestão de débito técnico em projetos reais"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "Identification",
                    Content = "O conceito de identification é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam identification regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com identification, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Prioritization",
                    Content = "O conceito de prioritization é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam prioritization regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com prioritization, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Remediation",
                    Content = "O conceito de remediation é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam remediation regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com remediation, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 3
                },
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Exemplo Básico de Gestão de Débito Técnico",
                    Code = @"using Microsoft.AspNetCore.Mvc;

// Implementação de Gestão de Débito Técnico
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
                    Explanation = "Este exemplo demonstra a implementação básica de Gestão de Débito Técnico. Note como o código segue as convenções do ASP.NET Core.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Exemplo Avançado",
                    Code = @"// Implementação avançada com Identification
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
                    Explanation = "Exemplo mais complexo mostrando como aplicar Gestão de Débito Técnico em cenários reais com logging e async/await.",
                    IsRunnable = true
                },
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Exercício 1: Implementação Básica",
                    Description = "Implemente Gestão de Débito Técnico em um projeto ASP.NET Core simples.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"using Microsoft.AspNetCore.Mvc;

// Implemente aqui
[ApiController]
[Route(""api/[controller]"")]
public class MeuController : ControllerBase
{
    
}",
                    Hints = new List<string> { "Comece definindo o controller", "Configure Identification", "Teste sua implementação" }
                },
                new Exercise
                {
                    Title = "Exercício 2: Cenário Intermediário",
                    Description = "Crie um serviço completo que utilize Gestão de Débito Técnico para gerenciar recursos.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"// Implemente o serviço
public class MeuServico
{
    
}",
                    Hints = new List<string> { "Injete dependências via construtor", "Implemente métodos que usem Gestão de Débito Técnico", "Adicione tratamento de erros" }
                },
                new Exercise
                {
                    Title = "Exercício 3: Desafio Avançado",
                    Description = "Implemente um sistema completo utilizando Gestão de Débito Técnico com múltiplas funcionalidades.",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = @"// Sistema completo
",
                    Hints = new List<string> { "Planeje a arquitetura antes de implementar", "Considere performance e escalabilidade", "Adicione testes unitários" }
                },
            },
            Summary = "Nesta aula, você aprendeu sobre Gestão de Débito Técnico no ASP.NET Core. Exploramos Identification, Prioritization, Remediation e vimos como aplicar estes conceitos em projetos reais. Compreender profundamente Gestão de Débito Técnico é essencial para desenvolver aplicações web modernas, robustas e escaláveis. Continue praticando com os exercícios propostos e experimente aplicar estes conceitos em seus próprios projetos. Na próxima aula, avançaremos para tópicos ainda mais interessantes do ASP.NET Core."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0010-000000000007"),
            CourseId = Guid.Parse("10000000-0000-0000-0000-000000000010"),
            Title = "Gestão de Débito Técnico",
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
                "Compreender os conceitos de techniques",
                "Aprender a implementar uncertainty",
                "Dominar communication",
                "Aplicar estimativas em projetos reais"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "Techniques",
                    Content = "O conceito de techniques é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam techniques regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com techniques, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Uncertainty",
                    Content = "O conceito de uncertainty é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam uncertainty regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com uncertainty, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Communication",
                    Content = "O conceito de communication é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam communication regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com communication, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 3
                },
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Exemplo Básico de Estimativas",
                    Code = @"using Microsoft.AspNetCore.Mvc;

// Implementação de Estimativas
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
                    Explanation = "Este exemplo demonstra a implementação básica de Estimativas. Note como o código segue as convenções do ASP.NET Core.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Exemplo Avançado",
                    Code = @"// Implementação avançada com Techniques
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
                    Explanation = "Exemplo mais complexo mostrando como aplicar Estimativas em cenários reais com logging e async/await.",
                    IsRunnable = true
                },
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Exercício 1: Implementação Básica",
                    Description = "Implemente Estimativas em um projeto ASP.NET Core simples.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"using Microsoft.AspNetCore.Mvc;

// Implemente aqui
[ApiController]
[Route(""api/[controller]"")]
public class MeuController : ControllerBase
{
    
}",
                    Hints = new List<string> { "Comece definindo o controller", "Configure Techniques", "Teste sua implementação" }
                },
                new Exercise
                {
                    Title = "Exercício 2: Cenário Intermediário",
                    Description = "Crie um serviço completo que utilize Estimativas para gerenciar recursos.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"// Implemente o serviço
public class MeuServico
{
    
}",
                    Hints = new List<string> { "Injete dependências via construtor", "Implemente métodos que usem Estimativas", "Adicione tratamento de erros" }
                },
                new Exercise
                {
                    Title = "Exercício 3: Desafio Avançado",
                    Description = "Implemente um sistema completo utilizando Estimativas com múltiplas funcionalidades.",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = @"// Sistema completo
",
                    Hints = new List<string> { "Planeje a arquitetura antes de implementar", "Considere performance e escalabilidade", "Adicione testes unitários" }
                },
            },
            Summary = "Nesta aula, você aprendeu sobre Estimativas no ASP.NET Core. Exploramos Techniques, Uncertainty, Communication e vimos como aplicar estes conceitos em projetos reais. Compreender profundamente Estimativas é essencial para desenvolver aplicações web modernas, robustas e escaláveis. Continue praticando com os exercícios propostos e experimente aplicar estes conceitos em seus próprios projetos. Na próxima aula, avançaremos para tópicos ainda mais interessantes do ASP.NET Core."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0010-000000000008"),
            CourseId = Guid.Parse("10000000-0000-0000-0000-000000000010"),
            Title = "Estimativas",
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
                "Compreender os conceitos de roadmaps",
                "Aprender a implementar milestones",
                "Dominar dependencies",
                "Aplicar planejamento técnico em projetos reais"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "Roadmaps",
                    Content = "O conceito de roadmaps é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam roadmaps regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com roadmaps, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Milestones",
                    Content = "O conceito de milestones é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam milestones regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com milestones, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
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
                    Title = "Exemplo Básico de Planejamento Técnico",
                    Code = @"using Microsoft.AspNetCore.Mvc;

// Implementação de Planejamento Técnico
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
                    Explanation = "Este exemplo demonstra a implementação básica de Planejamento Técnico. Note como o código segue as convenções do ASP.NET Core.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Exemplo Avançado",
                    Code = @"// Implementação avançada com Roadmaps
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
                    Explanation = "Exemplo mais complexo mostrando como aplicar Planejamento Técnico em cenários reais com logging e async/await.",
                    IsRunnable = true
                },
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Exercício 1: Implementação Básica",
                    Description = "Implemente Planejamento Técnico em um projeto ASP.NET Core simples.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"using Microsoft.AspNetCore.Mvc;

// Implemente aqui
[ApiController]
[Route(""api/[controller]"")]
public class MeuController : ControllerBase
{
    
}",
                    Hints = new List<string> { "Comece definindo o controller", "Configure Roadmaps", "Teste sua implementação" }
                },
                new Exercise
                {
                    Title = "Exercício 2: Cenário Intermediário",
                    Description = "Crie um serviço completo que utilize Planejamento Técnico para gerenciar recursos.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"// Implemente o serviço
public class MeuServico
{
    
}",
                    Hints = new List<string> { "Injete dependências via construtor", "Implemente métodos que usem Planejamento Técnico", "Adicione tratamento de erros" }
                },
                new Exercise
                {
                    Title = "Exercício 3: Desafio Avançado",
                    Description = "Implemente um sistema completo utilizando Planejamento Técnico com múltiplas funcionalidades.",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = @"// Sistema completo
",
                    Hints = new List<string> { "Planeje a arquitetura antes de implementar", "Considere performance e escalabilidade", "Adicione testes unitários" }
                },
            },
            Summary = "Nesta aula, você aprendeu sobre Planejamento Técnico no ASP.NET Core. Exploramos Roadmaps, Milestones, Dependencies e vimos como aplicar estes conceitos em projetos reais. Compreender profundamente Planejamento Técnico é essencial para desenvolver aplicações web modernas, robustas e escaláveis. Continue praticando com os exercícios propostos e experimente aplicar estes conceitos em seus próprios projetos. Na próxima aula, avançaremos para tópicos ainda mais interessantes do ASP.NET Core."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0010-000000000009"),
            CourseId = Guid.Parse("10000000-0000-0000-0000-000000000010"),
            Title = "Planejamento Técnico",
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
                "Compreender os conceitos de communication",
                "Aprender a implementar expectations",
                "Dominar alignment",
                "Aplicar gestão de stakeholders em projetos reais"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "Communication",
                    Content = "O conceito de communication é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam communication regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com communication, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Expectations",
                    Content = "O conceito de expectations é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam expectations regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com expectations, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Alignment",
                    Content = "O conceito de alignment é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam alignment regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com alignment, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 3
                },
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Exemplo Básico de Gestão de Stakeholders",
                    Code = @"using Microsoft.AspNetCore.Mvc;

// Implementação de Gestão de Stakeholders
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
                    Explanation = "Este exemplo demonstra a implementação básica de Gestão de Stakeholders. Note como o código segue as convenções do ASP.NET Core.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Exemplo Avançado",
                    Code = @"// Implementação avançada com Communication
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
                    Explanation = "Exemplo mais complexo mostrando como aplicar Gestão de Stakeholders em cenários reais com logging e async/await.",
                    IsRunnable = true
                },
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Exercício 1: Implementação Básica",
                    Description = "Implemente Gestão de Stakeholders em um projeto ASP.NET Core simples.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"using Microsoft.AspNetCore.Mvc;

// Implemente aqui
[ApiController]
[Route(""api/[controller]"")]
public class MeuController : ControllerBase
{
    
}",
                    Hints = new List<string> { "Comece definindo o controller", "Configure Communication", "Teste sua implementação" }
                },
                new Exercise
                {
                    Title = "Exercício 2: Cenário Intermediário",
                    Description = "Crie um serviço completo que utilize Gestão de Stakeholders para gerenciar recursos.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"// Implemente o serviço
public class MeuServico
{
    
}",
                    Hints = new List<string> { "Injete dependências via construtor", "Implemente métodos que usem Gestão de Stakeholders", "Adicione tratamento de erros" }
                },
                new Exercise
                {
                    Title = "Exercício 3: Desafio Avançado",
                    Description = "Implemente um sistema completo utilizando Gestão de Stakeholders com múltiplas funcionalidades.",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = @"// Sistema completo
",
                    Hints = new List<string> { "Planeje a arquitetura antes de implementar", "Considere performance e escalabilidade", "Adicione testes unitários" }
                },
            },
            Summary = "Nesta aula, você aprendeu sobre Gestão de Stakeholders no ASP.NET Core. Exploramos Communication, Expectations, Alignment e vimos como aplicar estes conceitos em projetos reais. Compreender profundamente Gestão de Stakeholders é essencial para desenvolver aplicações web modernas, robustas e escaláveis. Continue praticando com os exercícios propostos e experimente aplicar estes conceitos em seus próprios projetos. Na próxima aula, avançaremos para tópicos ainda mais interessantes do ASP.NET Core."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0010-000000000010"),
            CourseId = Guid.Parse("10000000-0000-0000-0000-000000000010"),
            Title = "Gestão de Stakeholders",
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
                "Compreender os conceitos de interviews",
                "Aprender a implementar assessment",
                "Dominar hiring",
                "Aplicar recrutamento técnico em projetos reais"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "Interviews",
                    Content = "O conceito de interviews é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam interviews regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com interviews, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Assessment",
                    Content = "O conceito de assessment é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam assessment regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com assessment, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Hiring",
                    Content = "O conceito de hiring é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam hiring regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com hiring, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 3
                },
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Exemplo Básico de Recrutamento Técnico",
                    Code = @"using Microsoft.AspNetCore.Mvc;

// Implementação de Recrutamento Técnico
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
                    Explanation = "Este exemplo demonstra a implementação básica de Recrutamento Técnico. Note como o código segue as convenções do ASP.NET Core.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Exemplo Avançado",
                    Code = @"// Implementação avançada com Interviews
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
                    Explanation = "Exemplo mais complexo mostrando como aplicar Recrutamento Técnico em cenários reais com logging e async/await.",
                    IsRunnable = true
                },
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Exercício 1: Implementação Básica",
                    Description = "Implemente Recrutamento Técnico em um projeto ASP.NET Core simples.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"using Microsoft.AspNetCore.Mvc;

// Implemente aqui
[ApiController]
[Route(""api/[controller]"")]
public class MeuController : ControllerBase
{
    
}",
                    Hints = new List<string> { "Comece definindo o controller", "Configure Interviews", "Teste sua implementação" }
                },
                new Exercise
                {
                    Title = "Exercício 2: Cenário Intermediário",
                    Description = "Crie um serviço completo que utilize Recrutamento Técnico para gerenciar recursos.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"// Implemente o serviço
public class MeuServico
{
    
}",
                    Hints = new List<string> { "Injete dependências via construtor", "Implemente métodos que usem Recrutamento Técnico", "Adicione tratamento de erros" }
                },
                new Exercise
                {
                    Title = "Exercício 3: Desafio Avançado",
                    Description = "Implemente um sistema completo utilizando Recrutamento Técnico com múltiplas funcionalidades.",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = @"// Sistema completo
",
                    Hints = new List<string> { "Planeje a arquitetura antes de implementar", "Considere performance e escalabilidade", "Adicione testes unitários" }
                },
            },
            Summary = "Nesta aula, você aprendeu sobre Recrutamento Técnico no ASP.NET Core. Exploramos Interviews, Assessment, Hiring e vimos como aplicar estes conceitos em projetos reais. Compreender profundamente Recrutamento Técnico é essencial para desenvolver aplicações web modernas, robustas e escaláveis. Continue praticando com os exercícios propostos e experimente aplicar estes conceitos em seus próprios projetos. Na próxima aula, avançaremos para tópicos ainda mais interessantes do ASP.NET Core."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0010-000000000011"),
            CourseId = Guid.Parse("10000000-0000-0000-0000-000000000010"),
            Title = "Recrutamento Técnico",
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
                "Compreender os conceitos de process",
                "Aprender a implementar documentation",
                "Dominar support",
                "Aplicar onboarding em projetos reais"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "Process",
                    Content = "O conceito de process é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam process regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com process, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Documentation",
                    Content = "O conceito de documentation é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam documentation regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com documentation, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Support",
                    Content = "O conceito de support é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam support regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com support, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 3
                },
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Exemplo Básico de Onboarding",
                    Code = @"using Microsoft.AspNetCore.Mvc;

// Implementação de Onboarding
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
                    Explanation = "Este exemplo demonstra a implementação básica de Onboarding. Note como o código segue as convenções do ASP.NET Core.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Exemplo Avançado",
                    Code = @"// Implementação avançada com Process
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
                    Explanation = "Exemplo mais complexo mostrando como aplicar Onboarding em cenários reais com logging e async/await.",
                    IsRunnable = true
                },
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Exercício 1: Implementação Básica",
                    Description = "Implemente Onboarding em um projeto ASP.NET Core simples.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"using Microsoft.AspNetCore.Mvc;

// Implemente aqui
[ApiController]
[Route(""api/[controller]"")]
public class MeuController : ControllerBase
{
    
}",
                    Hints = new List<string> { "Comece definindo o controller", "Configure Process", "Teste sua implementação" }
                },
                new Exercise
                {
                    Title = "Exercício 2: Cenário Intermediário",
                    Description = "Crie um serviço completo que utilize Onboarding para gerenciar recursos.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"// Implemente o serviço
public class MeuServico
{
    
}",
                    Hints = new List<string> { "Injete dependências via construtor", "Implemente métodos que usem Onboarding", "Adicione tratamento de erros" }
                },
                new Exercise
                {
                    Title = "Exercício 3: Desafio Avançado",
                    Description = "Implemente um sistema completo utilizando Onboarding com múltiplas funcionalidades.",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = @"// Sistema completo
",
                    Hints = new List<string> { "Planeje a arquitetura antes de implementar", "Considere performance e escalabilidade", "Adicione testes unitários" }
                },
            },
            Summary = "Nesta aula, você aprendeu sobre Onboarding no ASP.NET Core. Exploramos Process, Documentation, Support e vimos como aplicar estes conceitos em projetos reais. Compreender profundamente Onboarding é essencial para desenvolver aplicações web modernas, robustas e escaláveis. Continue praticando com os exercícios propostos e experimente aplicar estes conceitos em seus próprios projetos. Na próxima aula, avançaremos para tópicos ainda mais interessantes do ASP.NET Core."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0010-000000000012"),
            CourseId = Guid.Parse("10000000-0000-0000-0000-000000000010"),
            Title = "Onboarding",
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
                "Compreender os conceitos de feedback",
                "Aprender a implementar goals",
                "Dominar development",
                "Aplicar performance reviews em projetos reais"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "Feedback",
                    Content = "O conceito de feedback é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam feedback regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com feedback, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Goals",
                    Content = "O conceito de goals é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam goals regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com goals, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Development",
                    Content = "O conceito de development é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam development regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com development, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 3
                },
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Exemplo Básico de Performance Reviews",
                    Code = @"using Microsoft.AspNetCore.Mvc;

// Implementação de Performance Reviews
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
                    Explanation = "Este exemplo demonstra a implementação básica de Performance Reviews. Note como o código segue as convenções do ASP.NET Core.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Exemplo Avançado",
                    Code = @"// Implementação avançada com Feedback
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
                    Explanation = "Exemplo mais complexo mostrando como aplicar Performance Reviews em cenários reais com logging e async/await.",
                    IsRunnable = true
                },
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Exercício 1: Implementação Básica",
                    Description = "Implemente Performance Reviews em um projeto ASP.NET Core simples.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"using Microsoft.AspNetCore.Mvc;

// Implemente aqui
[ApiController]
[Route(""api/[controller]"")]
public class MeuController : ControllerBase
{
    
}",
                    Hints = new List<string> { "Comece definindo o controller", "Configure Feedback", "Teste sua implementação" }
                },
                new Exercise
                {
                    Title = "Exercício 2: Cenário Intermediário",
                    Description = "Crie um serviço completo que utilize Performance Reviews para gerenciar recursos.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"// Implemente o serviço
public class MeuServico
{
    
}",
                    Hints = new List<string> { "Injete dependências via construtor", "Implemente métodos que usem Performance Reviews", "Adicione tratamento de erros" }
                },
                new Exercise
                {
                    Title = "Exercício 3: Desafio Avançado",
                    Description = "Implemente um sistema completo utilizando Performance Reviews com múltiplas funcionalidades.",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = @"// Sistema completo
",
                    Hints = new List<string> { "Planeje a arquitetura antes de implementar", "Considere performance e escalabilidade", "Adicione testes unitários" }
                },
            },
            Summary = "Nesta aula, você aprendeu sobre Performance Reviews no ASP.NET Core. Exploramos Feedback, Goals, Development e vimos como aplicar estes conceitos em projetos reais. Compreender profundamente Performance Reviews é essencial para desenvolver aplicações web modernas, robustas e escaláveis. Continue praticando com os exercícios propostos e experimente aplicar estes conceitos em seus próprios projetos. Na próxima aula, avançaremos para tópicos ainda mais interessantes do ASP.NET Core."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0010-000000000013"),
            CourseId = Guid.Parse("10000000-0000-0000-0000-000000000010"),
            Title = "Performance Reviews",
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
                "Compreender os conceitos de mediation",
                "Aprender a implementar communication",
                "Dominar solutions",
                "Aplicar resolução de conflitos em projetos reais"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "Mediation",
                    Content = "O conceito de mediation é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam mediation regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com mediation, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Communication",
                    Content = "O conceito de communication é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam communication regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com communication, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Solutions",
                    Content = "O conceito de solutions é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam solutions regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com solutions, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 3
                },
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Exemplo Básico de Resolução de Conflitos",
                    Code = @"using Microsoft.AspNetCore.Mvc;

// Implementação de Resolução de Conflitos
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
                    Explanation = "Este exemplo demonstra a implementação básica de Resolução de Conflitos. Note como o código segue as convenções do ASP.NET Core.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Exemplo Avançado",
                    Code = @"// Implementação avançada com Mediation
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
                    Explanation = "Exemplo mais complexo mostrando como aplicar Resolução de Conflitos em cenários reais com logging e async/await.",
                    IsRunnable = true
                },
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Exercício 1: Implementação Básica",
                    Description = "Implemente Resolução de Conflitos em um projeto ASP.NET Core simples.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"using Microsoft.AspNetCore.Mvc;

// Implemente aqui
[ApiController]
[Route(""api/[controller]"")]
public class MeuController : ControllerBase
{
    
}",
                    Hints = new List<string> { "Comece definindo o controller", "Configure Mediation", "Teste sua implementação" }
                },
                new Exercise
                {
                    Title = "Exercício 2: Cenário Intermediário",
                    Description = "Crie um serviço completo que utilize Resolução de Conflitos para gerenciar recursos.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"// Implemente o serviço
public class MeuServico
{
    
}",
                    Hints = new List<string> { "Injete dependências via construtor", "Implemente métodos que usem Resolução de Conflitos", "Adicione tratamento de erros" }
                },
                new Exercise
                {
                    Title = "Exercício 3: Desafio Avançado",
                    Description = "Implemente um sistema completo utilizando Resolução de Conflitos com múltiplas funcionalidades.",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = @"// Sistema completo
",
                    Hints = new List<string> { "Planeje a arquitetura antes de implementar", "Considere performance e escalabilidade", "Adicione testes unitários" }
                },
            },
            Summary = "Nesta aula, você aprendeu sobre Resolução de Conflitos no ASP.NET Core. Exploramos Mediation, Communication, Solutions e vimos como aplicar estes conceitos em projetos reais. Compreender profundamente Resolução de Conflitos é essencial para desenvolver aplicações web modernas, robustas e escaláveis. Continue praticando com os exercícios propostos e experimente aplicar estes conceitos em seus próprios projetos. Na próxima aula, avançaremos para tópicos ainda mais interessantes do ASP.NET Core."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0010-000000000014"),
            CourseId = Guid.Parse("10000000-0000-0000-0000-000000000010"),
            Title = "Resolução de Conflitos",
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
                "Compreender os conceitos de values",
                "Aprender a implementar practices",
                "Dominar improvement",
                "Aplicar cultura de engenharia em projetos reais"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "Values",
                    Content = "O conceito de values é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam values regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com values, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Practices",
                    Content = "O conceito de practices é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam practices regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com practices, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Improvement",
                    Content = "O conceito de improvement é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam improvement regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com improvement, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 3
                },
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Exemplo Básico de Cultura de Engenharia",
                    Code = @"using Microsoft.AspNetCore.Mvc;

// Implementação de Cultura de Engenharia
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
                    Explanation = "Este exemplo demonstra a implementação básica de Cultura de Engenharia. Note como o código segue as convenções do ASP.NET Core.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Exemplo Avançado",
                    Code = @"// Implementação avançada com Values
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
                    Explanation = "Exemplo mais complexo mostrando como aplicar Cultura de Engenharia em cenários reais com logging e async/await.",
                    IsRunnable = true
                },
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Exercício 1: Implementação Básica",
                    Description = "Implemente Cultura de Engenharia em um projeto ASP.NET Core simples.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"using Microsoft.AspNetCore.Mvc;

// Implemente aqui
[ApiController]
[Route(""api/[controller]"")]
public class MeuController : ControllerBase
{
    
}",
                    Hints = new List<string> { "Comece definindo o controller", "Configure Values", "Teste sua implementação" }
                },
                new Exercise
                {
                    Title = "Exercício 2: Cenário Intermediário",
                    Description = "Crie um serviço completo que utilize Cultura de Engenharia para gerenciar recursos.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"// Implemente o serviço
public class MeuServico
{
    
}",
                    Hints = new List<string> { "Injete dependências via construtor", "Implemente métodos que usem Cultura de Engenharia", "Adicione tratamento de erros" }
                },
                new Exercise
                {
                    Title = "Exercício 3: Desafio Avançado",
                    Description = "Implemente um sistema completo utilizando Cultura de Engenharia com múltiplas funcionalidades.",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = @"// Sistema completo
",
                    Hints = new List<string> { "Planeje a arquitetura antes de implementar", "Considere performance e escalabilidade", "Adicione testes unitários" }
                },
            },
            Summary = "Nesta aula, você aprendeu sobre Cultura de Engenharia no ASP.NET Core. Exploramos Values, Practices, Improvement e vimos como aplicar estes conceitos em projetos reais. Compreender profundamente Cultura de Engenharia é essencial para desenvolver aplicações web modernas, robustas e escaláveis. Continue praticando com os exercícios propostos e experimente aplicar estes conceitos em seus próprios projetos. Na próxima aula, avançaremos para tópicos ainda mais interessantes do ASP.NET Core."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0010-000000000015"),
            CourseId = Guid.Parse("10000000-0000-0000-0000-000000000010"),
            Title = "Cultura de Engenharia",
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
                "Compreender os conceitos de research",
                "Aprender a implementar experimentation",
                "Dominar adoption",
                "Aplicar inovação técnica em projetos reais"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "Research",
                    Content = "O conceito de research é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam research regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com research, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Experimentation",
                    Content = "O conceito de experimentation é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam experimentation regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com experimentation, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Adoption",
                    Content = "O conceito de adoption é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam adoption regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com adoption, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 3
                },
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Exemplo Básico de Inovação Técnica",
                    Code = @"using Microsoft.AspNetCore.Mvc;

// Implementação de Inovação Técnica
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
                    Explanation = "Este exemplo demonstra a implementação básica de Inovação Técnica. Note como o código segue as convenções do ASP.NET Core.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Exemplo Avançado",
                    Code = @"// Implementação avançada com Research
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
                    Explanation = "Exemplo mais complexo mostrando como aplicar Inovação Técnica em cenários reais com logging e async/await.",
                    IsRunnable = true
                },
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Exercício 1: Implementação Básica",
                    Description = "Implemente Inovação Técnica em um projeto ASP.NET Core simples.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"using Microsoft.AspNetCore.Mvc;

// Implemente aqui
[ApiController]
[Route(""api/[controller]"")]
public class MeuController : ControllerBase
{
    
}",
                    Hints = new List<string> { "Comece definindo o controller", "Configure Research", "Teste sua implementação" }
                },
                new Exercise
                {
                    Title = "Exercício 2: Cenário Intermediário",
                    Description = "Crie um serviço completo que utilize Inovação Técnica para gerenciar recursos.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"// Implemente o serviço
public class MeuServico
{
    
}",
                    Hints = new List<string> { "Injete dependências via construtor", "Implemente métodos que usem Inovação Técnica", "Adicione tratamento de erros" }
                },
                new Exercise
                {
                    Title = "Exercício 3: Desafio Avançado",
                    Description = "Implemente um sistema completo utilizando Inovação Técnica com múltiplas funcionalidades.",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = @"// Sistema completo
",
                    Hints = new List<string> { "Planeje a arquitetura antes de implementar", "Considere performance e escalabilidade", "Adicione testes unitários" }
                },
            },
            Summary = "Nesta aula, você aprendeu sobre Inovação Técnica no ASP.NET Core. Exploramos Research, Experimentation, Adoption e vimos como aplicar estes conceitos em projetos reais. Compreender profundamente Inovação Técnica é essencial para desenvolver aplicações web modernas, robustas e escaláveis. Continue praticando com os exercícios propostos e experimente aplicar estes conceitos em seus próprios projetos. Na próxima aula, avançaremos para tópicos ainda mais interessantes do ASP.NET Core."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0010-000000000016"),
            CourseId = Guid.Parse("10000000-0000-0000-0000-000000000010"),
            Title = "Inovação Técnica",
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
                "Compreender os conceitos de presentations",
                "Aprender a implementar writing",
                "Dominar meetings",
                "Aplicar comunicação efetiva em projetos reais"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "Presentations",
                    Content = "O conceito de presentations é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam presentations regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com presentations, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Writing",
                    Content = "O conceito de writing é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam writing regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com writing, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Meetings",
                    Content = "O conceito de meetings é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam meetings regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com meetings, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 3
                },
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Exemplo Básico de Comunicação Efetiva",
                    Code = @"using Microsoft.AspNetCore.Mvc;

// Implementação de Comunicação Efetiva
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
                    Explanation = "Este exemplo demonstra a implementação básica de Comunicação Efetiva. Note como o código segue as convenções do ASP.NET Core.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Exemplo Avançado",
                    Code = @"// Implementação avançada com Presentations
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
                    Explanation = "Exemplo mais complexo mostrando como aplicar Comunicação Efetiva em cenários reais com logging e async/await.",
                    IsRunnable = true
                },
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Exercício 1: Implementação Básica",
                    Description = "Implemente Comunicação Efetiva em um projeto ASP.NET Core simples.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"using Microsoft.AspNetCore.Mvc;

// Implemente aqui
[ApiController]
[Route(""api/[controller]"")]
public class MeuController : ControllerBase
{
    
}",
                    Hints = new List<string> { "Comece definindo o controller", "Configure Presentations", "Teste sua implementação" }
                },
                new Exercise
                {
                    Title = "Exercício 2: Cenário Intermediário",
                    Description = "Crie um serviço completo que utilize Comunicação Efetiva para gerenciar recursos.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"// Implemente o serviço
public class MeuServico
{
    
}",
                    Hints = new List<string> { "Injete dependências via construtor", "Implemente métodos que usem Comunicação Efetiva", "Adicione tratamento de erros" }
                },
                new Exercise
                {
                    Title = "Exercício 3: Desafio Avançado",
                    Description = "Implemente um sistema completo utilizando Comunicação Efetiva com múltiplas funcionalidades.",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = @"// Sistema completo
",
                    Hints = new List<string> { "Planeje a arquitetura antes de implementar", "Considere performance e escalabilidade", "Adicione testes unitários" }
                },
            },
            Summary = "Nesta aula, você aprendeu sobre Comunicação Efetiva no ASP.NET Core. Exploramos Presentations, Writing, Meetings e vimos como aplicar estes conceitos em projetos reais. Compreender profundamente Comunicação Efetiva é essencial para desenvolver aplicações web modernas, robustas e escaláveis. Continue praticando com os exercícios propostos e experimente aplicar estes conceitos em seus próprios projetos. Na próxima aula, avançaremos para tópicos ainda mais interessantes do ASP.NET Core."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0010-000000000017"),
            CourseId = Guid.Parse("10000000-0000-0000-0000-000000000010"),
            Title = "Comunicação Efetiva",
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
                "Compreender os conceitos de incident response",
                "Aprender a implementar communication",
                "Dominar post-mortems",
                "Aplicar gestão de crises em projetos reais"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "Incident Response",
                    Content = "O conceito de incident response é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam incident response regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com incident response, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Communication",
                    Content = "O conceito de communication é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam communication regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com communication, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Post-Mortems",
                    Content = "O conceito de post-mortems é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam post-mortems regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com post-mortems, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 3
                },
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Exemplo Básico de Gestão de Crises",
                    Code = @"using Microsoft.AspNetCore.Mvc;

// Implementação de Gestão de Crises
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
                    Explanation = "Este exemplo demonstra a implementação básica de Gestão de Crises. Note como o código segue as convenções do ASP.NET Core.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Exemplo Avançado",
                    Code = @"// Implementação avançada com Incident Response
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
                    Explanation = "Exemplo mais complexo mostrando como aplicar Gestão de Crises em cenários reais com logging e async/await.",
                    IsRunnable = true
                },
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Exercício 1: Implementação Básica",
                    Description = "Implemente Gestão de Crises em um projeto ASP.NET Core simples.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"using Microsoft.AspNetCore.Mvc;

// Implemente aqui
[ApiController]
[Route(""api/[controller]"")]
public class MeuController : ControllerBase
{
    
}",
                    Hints = new List<string> { "Comece definindo o controller", "Configure Incident Response", "Teste sua implementação" }
                },
                new Exercise
                {
                    Title = "Exercício 2: Cenário Intermediário",
                    Description = "Crie um serviço completo que utilize Gestão de Crises para gerenciar recursos.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"// Implemente o serviço
public class MeuServico
{
    
}",
                    Hints = new List<string> { "Injete dependências via construtor", "Implemente métodos que usem Gestão de Crises", "Adicione tratamento de erros" }
                },
                new Exercise
                {
                    Title = "Exercício 3: Desafio Avançado",
                    Description = "Implemente um sistema completo utilizando Gestão de Crises com múltiplas funcionalidades.",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = @"// Sistema completo
",
                    Hints = new List<string> { "Planeje a arquitetura antes de implementar", "Considere performance e escalabilidade", "Adicione testes unitários" }
                },
            },
            Summary = "Nesta aula, você aprendeu sobre Gestão de Crises no ASP.NET Core. Exploramos Incident Response, Communication, Post-Mortems e vimos como aplicar estes conceitos em projetos reais. Compreender profundamente Gestão de Crises é essencial para desenvolver aplicações web modernas, robustas e escaláveis. Continue praticando com os exercícios propostos e experimente aplicar estes conceitos em seus próprios projetos. Na próxima aula, avançaremos para tópicos ainda mais interessantes do ASP.NET Core."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0010-000000000018"),
            CourseId = Guid.Parse("10000000-0000-0000-0000-000000000010"),
            Title = "Gestão de Crises",
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
                "Compreender os conceitos de paths",
                "Aprender a implementar growth",
                "Dominar transitions",
                "Aplicar carreira em tecnologia em projetos reais"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "Paths",
                    Content = "O conceito de paths é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam paths regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com paths, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Growth",
                    Content = "O conceito de growth é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam growth regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com growth, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Transitions",
                    Content = "O conceito de transitions é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam transitions regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com transitions, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 3
                },
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Exemplo Básico de Carreira em Tecnologia",
                    Code = @"using Microsoft.AspNetCore.Mvc;

// Implementação de Carreira em Tecnologia
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
                    Explanation = "Este exemplo demonstra a implementação básica de Carreira em Tecnologia. Note como o código segue as convenções do ASP.NET Core.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Exemplo Avançado",
                    Code = @"// Implementação avançada com Paths
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
                    Explanation = "Exemplo mais complexo mostrando como aplicar Carreira em Tecnologia em cenários reais com logging e async/await.",
                    IsRunnable = true
                },
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Exercício 1: Implementação Básica",
                    Description = "Implemente Carreira em Tecnologia em um projeto ASP.NET Core simples.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"using Microsoft.AspNetCore.Mvc;

// Implemente aqui
[ApiController]
[Route(""api/[controller]"")]
public class MeuController : ControllerBase
{
    
}",
                    Hints = new List<string> { "Comece definindo o controller", "Configure Paths", "Teste sua implementação" }
                },
                new Exercise
                {
                    Title = "Exercício 2: Cenário Intermediário",
                    Description = "Crie um serviço completo que utilize Carreira em Tecnologia para gerenciar recursos.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"// Implemente o serviço
public class MeuServico
{
    
}",
                    Hints = new List<string> { "Injete dependências via construtor", "Implemente métodos que usem Carreira em Tecnologia", "Adicione tratamento de erros" }
                },
                new Exercise
                {
                    Title = "Exercício 3: Desafio Avançado",
                    Description = "Implemente um sistema completo utilizando Carreira em Tecnologia com múltiplas funcionalidades.",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = @"// Sistema completo
",
                    Hints = new List<string> { "Planeje a arquitetura antes de implementar", "Considere performance e escalabilidade", "Adicione testes unitários" }
                },
            },
            Summary = "Nesta aula, você aprendeu sobre Carreira em Tecnologia no ASP.NET Core. Exploramos Paths, Growth, Transitions e vimos como aplicar estes conceitos em projetos reais. Compreender profundamente Carreira em Tecnologia é essencial para desenvolver aplicações web modernas, robustas e escaláveis. Continue praticando com os exercícios propostos e experimente aplicar estes conceitos em seus próprios projetos. Na próxima aula, avançaremos para tópicos ainda mais interessantes do ASP.NET Core."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0010-000000000019"),
            CourseId = Guid.Parse("10000000-0000-0000-0000-000000000010"),
            Title = "Carreira em Tecnologia",
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
                "Compreender os conceitos de balance",
                "Aprender a implementar delegation",
                "Dominar continuous learning",
                "Aplicar tech lead best practices em projetos reais"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "Balance",
                    Content = "O conceito de balance é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam balance regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com balance, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Delegation",
                    Content = "O conceito de delegation é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam delegation regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com delegation, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Continuous Learning",
                    Content = "O conceito de continuous learning é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam continuous learning regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com continuous learning, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 3
                },
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Exemplo Básico de Tech Lead Best Practices",
                    Code = @"using Microsoft.AspNetCore.Mvc;

// Implementação de Tech Lead Best Practices
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
                    Explanation = "Este exemplo demonstra a implementação básica de Tech Lead Best Practices. Note como o código segue as convenções do ASP.NET Core.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Exemplo Avançado",
                    Code = @"// Implementação avançada com Balance
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
                    Explanation = "Exemplo mais complexo mostrando como aplicar Tech Lead Best Practices em cenários reais com logging e async/await.",
                    IsRunnable = true
                },
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Exercício 1: Implementação Básica",
                    Description = "Implemente Tech Lead Best Practices em um projeto ASP.NET Core simples.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"using Microsoft.AspNetCore.Mvc;

// Implemente aqui
[ApiController]
[Route(""api/[controller]"")]
public class MeuController : ControllerBase
{
    
}",
                    Hints = new List<string> { "Comece definindo o controller", "Configure Balance", "Teste sua implementação" }
                },
                new Exercise
                {
                    Title = "Exercício 2: Cenário Intermediário",
                    Description = "Crie um serviço completo que utilize Tech Lead Best Practices para gerenciar recursos.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"// Implemente o serviço
public class MeuServico
{
    
}",
                    Hints = new List<string> { "Injete dependências via construtor", "Implemente métodos que usem Tech Lead Best Practices", "Adicione tratamento de erros" }
                },
                new Exercise
                {
                    Title = "Exercício 3: Desafio Avançado",
                    Description = "Implemente um sistema completo utilizando Tech Lead Best Practices com múltiplas funcionalidades.",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = @"// Sistema completo
",
                    Hints = new List<string> { "Planeje a arquitetura antes de implementar", "Considere performance e escalabilidade", "Adicione testes unitários" }
                },
            },
            Summary = "Nesta aula, você aprendeu sobre Tech Lead Best Practices no ASP.NET Core. Exploramos Balance, Delegation, Continuous Learning e vimos como aplicar estes conceitos em projetos reais. Compreender profundamente Tech Lead Best Practices é essencial para desenvolver aplicações web modernas, robustas e escaláveis. Continue praticando com os exercícios propostos e experimente aplicar estes conceitos em seus próprios projetos. Na próxima aula, avançaremos para tópicos ainda mais interessantes do ASP.NET Core."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0010-000000000020"),
            CourseId = Guid.Parse("10000000-0000-0000-0000-000000000010"),
            Title = "Tech Lead Best Practices",
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
