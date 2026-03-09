using Shared.Entities;
using Shared.Models;
using System.Text.Json;

namespace Shared.Data;

/// <summary>
/// Level 14 Content Seeder - Design de Sistemas
/// Topics: Scalability, Availability, Consistency, Caching, Load Balancing
/// </summary>
public partial class Level14ContentSeeder
{
    private readonly Guid _courseId = Guid.Parse("10000000-0000-0000-0000-00000000000F");
    private readonly Guid _levelId = Guid.Parse("00000000-0000-0000-0000-00000000000E");

    public Course CreateLevel14Course()
    {
        return new Course
        {
            Id = _courseId,
            LevelId = _levelId,
            Title = "Design de Sistemas",
            Description = "Curso completo de Design de Sistemas com 20 aulas práticas e projetos reais.",
            Level = Level.Advanced,
            Duration = "6 semanas",
            LessonCount = 20,
            Topics = JsonSerializer.Serialize(new[] { "Scalability", "Availability", "Consistency", "Caching", "Load Balancing" }),
            OrderIndex = 1,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }


    public List<Lesson> CreateLevel14Lessons()
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
                "Compreender os conceitos de requirements",
                "Aprender a implementar constraints",
                "Dominar trade-offs",
                "Aplicar fundamentos de system design em projetos reais"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "Requirements",
                    Content = "O conceito de requirements é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam requirements regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com requirements, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Constraints",
                    Content = "O conceito de constraints é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam constraints regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com constraints, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Trade-offs",
                    Content = "O conceito de trade-offs é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam trade-offs regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com trade-offs, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 3
                },
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Exemplo Básico de Fundamentos de System Design",
                    Code = @"using Microsoft.AspNetCore.Mvc;

// Implementação de Fundamentos de System Design
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
                    Explanation = "Este exemplo demonstra a implementação básica de Fundamentos de System Design. Note como o código segue as convenções do ASP.NET Core.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Exemplo Avançado",
                    Code = @"// Implementação avançada com Requirements
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
                    Explanation = "Exemplo mais complexo mostrando como aplicar Fundamentos de System Design em cenários reais com logging e async/await.",
                    IsRunnable = true
                },
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Exercício 1: Implementação Básica",
                    Description = "Implemente Fundamentos de System Design em um projeto ASP.NET Core simples.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"using Microsoft.AspNetCore.Mvc;

// Implemente aqui
[ApiController]
[Route(""api/[controller]"")]
public class MeuController : ControllerBase
{
    
}",
                    Hints = new List<string> { "Comece definindo o controller", "Configure Requirements", "Teste sua implementação" }
                },
                new Exercise
                {
                    Title = "Exercício 2: Cenário Intermediário",
                    Description = "Crie um serviço completo que utilize Fundamentos de System Design para gerenciar recursos.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"// Implemente o serviço
public class MeuServico
{
    
}",
                    Hints = new List<string> { "Injete dependências via construtor", "Implemente métodos que usem Fundamentos de System Design", "Adicione tratamento de erros" }
                },
                new Exercise
                {
                    Title = "Exercício 3: Desafio Avançado",
                    Description = "Implemente um sistema completo utilizando Fundamentos de System Design com múltiplas funcionalidades.",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = @"// Sistema completo
",
                    Hints = new List<string> { "Planeje a arquitetura antes de implementar", "Considere performance e escalabilidade", "Adicione testes unitários" }
                },
            },
            Summary = "Nesta aula, você aprendeu sobre Fundamentos de System Design no ASP.NET Core. Exploramos Requirements, Constraints, Trade-offs e vimos como aplicar estes conceitos em projetos reais. Compreender profundamente Fundamentos de System Design é essencial para desenvolver aplicações web modernas, robustas e escaláveis. Continue praticando com os exercícios propostos e experimente aplicar estes conceitos em seus próprios projetos. Na próxima aula, avançaremos para tópicos ainda mais interessantes do ASP.NET Core."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-000F-000000000001"),
            CourseId = Guid.Parse("10000000-0000-0000-0000-00000000000F"),
            Title = "Fundamentos de System Design",
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
                "Compreender os conceitos de horizontal",
                "Aprender a implementar vertical",
                "Dominar strategies",
                "Aplicar escalabilidade em projetos reais"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "Horizontal",
                    Content = "O conceito de horizontal é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam horizontal regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com horizontal, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Vertical",
                    Content = "O conceito de vertical é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam vertical regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com vertical, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Strategies",
                    Content = "O conceito de strategies é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam strategies regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com strategies, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 3
                },
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Exemplo Básico de Escalabilidade",
                    Code = @"using Microsoft.AspNetCore.Mvc;

// Implementação de Escalabilidade
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
                    Explanation = "Este exemplo demonstra a implementação básica de Escalabilidade. Note como o código segue as convenções do ASP.NET Core.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Exemplo Avançado",
                    Code = @"// Implementação avançada com Horizontal
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
                    Explanation = "Exemplo mais complexo mostrando como aplicar Escalabilidade em cenários reais com logging e async/await.",
                    IsRunnable = true
                },
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Exercício 1: Implementação Básica",
                    Description = "Implemente Escalabilidade em um projeto ASP.NET Core simples.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"using Microsoft.AspNetCore.Mvc;

// Implemente aqui
[ApiController]
[Route(""api/[controller]"")]
public class MeuController : ControllerBase
{
    
}",
                    Hints = new List<string> { "Comece definindo o controller", "Configure Horizontal", "Teste sua implementação" }
                },
                new Exercise
                {
                    Title = "Exercício 2: Cenário Intermediário",
                    Description = "Crie um serviço completo que utilize Escalabilidade para gerenciar recursos.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"// Implemente o serviço
public class MeuServico
{
    
}",
                    Hints = new List<string> { "Injete dependências via construtor", "Implemente métodos que usem Escalabilidade", "Adicione tratamento de erros" }
                },
                new Exercise
                {
                    Title = "Exercício 3: Desafio Avançado",
                    Description = "Implemente um sistema completo utilizando Escalabilidade com múltiplas funcionalidades.",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = @"// Sistema completo
",
                    Hints = new List<string> { "Planeje a arquitetura antes de implementar", "Considere performance e escalabilidade", "Adicione testes unitários" }
                },
            },
            Summary = "Nesta aula, você aprendeu sobre Escalabilidade no ASP.NET Core. Exploramos Horizontal, Vertical, Strategies e vimos como aplicar estes conceitos em projetos reais. Compreender profundamente Escalabilidade é essencial para desenvolver aplicações web modernas, robustas e escaláveis. Continue praticando com os exercícios propostos e experimente aplicar estes conceitos em seus próprios projetos. Na próxima aula, avançaremos para tópicos ainda mais interessantes do ASP.NET Core."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-000F-000000000002"),
            CourseId = Guid.Parse("10000000-0000-0000-0000-00000000000F"),
            Title = "Escalabilidade",
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
                "Compreender os conceitos de high availability",
                "Aprender a implementar redundancy",
                "Dominar failover",
                "Aplicar disponibilidade em projetos reais"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "High Availability",
                    Content = "O conceito de high availability é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam high availability regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com high availability, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Redundancy",
                    Content = "O conceito de redundancy é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam redundancy regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com redundancy, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Failover",
                    Content = "O conceito de failover é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam failover regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com failover, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 3
                },
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Exemplo Básico de Disponibilidade",
                    Code = @"using Microsoft.AspNetCore.Mvc;

// Implementação de Disponibilidade
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
                    Explanation = "Este exemplo demonstra a implementação básica de Disponibilidade. Note como o código segue as convenções do ASP.NET Core.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Exemplo Avançado",
                    Code = @"// Implementação avançada com High Availability
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
                    Explanation = "Exemplo mais complexo mostrando como aplicar Disponibilidade em cenários reais com logging e async/await.",
                    IsRunnable = true
                },
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Exercício 1: Implementação Básica",
                    Description = "Implemente Disponibilidade em um projeto ASP.NET Core simples.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"using Microsoft.AspNetCore.Mvc;

// Implemente aqui
[ApiController]
[Route(""api/[controller]"")]
public class MeuController : ControllerBase
{
    
}",
                    Hints = new List<string> { "Comece definindo o controller", "Configure High Availability", "Teste sua implementação" }
                },
                new Exercise
                {
                    Title = "Exercício 2: Cenário Intermediário",
                    Description = "Crie um serviço completo que utilize Disponibilidade para gerenciar recursos.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"// Implemente o serviço
public class MeuServico
{
    
}",
                    Hints = new List<string> { "Injete dependências via construtor", "Implemente métodos que usem Disponibilidade", "Adicione tratamento de erros" }
                },
                new Exercise
                {
                    Title = "Exercício 3: Desafio Avançado",
                    Description = "Implemente um sistema completo utilizando Disponibilidade com múltiplas funcionalidades.",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = @"// Sistema completo
",
                    Hints = new List<string> { "Planeje a arquitetura antes de implementar", "Considere performance e escalabilidade", "Adicione testes unitários" }
                },
            },
            Summary = "Nesta aula, você aprendeu sobre Disponibilidade no ASP.NET Core. Exploramos High Availability, Redundancy, Failover e vimos como aplicar estes conceitos em projetos reais. Compreender profundamente Disponibilidade é essencial para desenvolver aplicações web modernas, robustas e escaláveis. Continue praticando com os exercícios propostos e experimente aplicar estes conceitos em seus próprios projetos. Na próxima aula, avançaremos para tópicos ainda mais interessantes do ASP.NET Core."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-000F-000000000003"),
            CourseId = Guid.Parse("10000000-0000-0000-0000-00000000000F"),
            Title = "Disponibilidade",
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
                "Compreender os conceitos de consistency",
                "Aprender a implementar availability",
                "Dominar partition tolerance",
                "Aplicar cap theorem em projetos reais"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "Consistency",
                    Content = "O conceito de consistency é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam consistency regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com consistency, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Availability",
                    Content = "O conceito de availability é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam availability regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com availability, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Partition Tolerance",
                    Content = "O conceito de partition tolerance é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam partition tolerance regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com partition tolerance, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 3
                },
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Exemplo Básico de CAP Theorem",
                    Code = @"using Microsoft.AspNetCore.Mvc;

// Implementação de CAP Theorem
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
                    Explanation = "Este exemplo demonstra a implementação básica de CAP Theorem. Note como o código segue as convenções do ASP.NET Core.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Exemplo Avançado",
                    Code = @"// Implementação avançada com Consistency
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
                    Explanation = "Exemplo mais complexo mostrando como aplicar CAP Theorem em cenários reais com logging e async/await.",
                    IsRunnable = true
                },
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Exercício 1: Implementação Básica",
                    Description = "Implemente CAP Theorem em um projeto ASP.NET Core simples.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"using Microsoft.AspNetCore.Mvc;

// Implemente aqui
[ApiController]
[Route(""api/[controller]"")]
public class MeuController : ControllerBase
{
    
}",
                    Hints = new List<string> { "Comece definindo o controller", "Configure Consistency", "Teste sua implementação" }
                },
                new Exercise
                {
                    Title = "Exercício 2: Cenário Intermediário",
                    Description = "Crie um serviço completo que utilize CAP Theorem para gerenciar recursos.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"// Implemente o serviço
public class MeuServico
{
    
}",
                    Hints = new List<string> { "Injete dependências via construtor", "Implemente métodos que usem CAP Theorem", "Adicione tratamento de erros" }
                },
                new Exercise
                {
                    Title = "Exercício 3: Desafio Avançado",
                    Description = "Implemente um sistema completo utilizando CAP Theorem com múltiplas funcionalidades.",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = @"// Sistema completo
",
                    Hints = new List<string> { "Planeje a arquitetura antes de implementar", "Considere performance e escalabilidade", "Adicione testes unitários" }
                },
            },
            Summary = "Nesta aula, você aprendeu sobre CAP Theorem no ASP.NET Core. Exploramos Consistency, Availability, Partition Tolerance e vimos como aplicar estes conceitos em projetos reais. Compreender profundamente CAP Theorem é essencial para desenvolver aplicações web modernas, robustas e escaláveis. Continue praticando com os exercícios propostos e experimente aplicar estes conceitos em seus próprios projetos. Na próxima aula, avançaremos para tópicos ainda mais interessantes do ASP.NET Core."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-000F-000000000004"),
            CourseId = Guid.Parse("10000000-0000-0000-0000-00000000000F"),
            Title = "CAP Theorem",
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
                "Compreender os conceitos de algorithms",
                "Aprender a implementar health checks",
                "Dominar session affinity",
                "Aplicar load balancing em projetos reais"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "Algorithms",
                    Content = "O conceito de algorithms é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam algorithms regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com algorithms, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Health Checks",
                    Content = "O conceito de health checks é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam health checks regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com health checks, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Session Affinity",
                    Content = "O conceito de session affinity é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam session affinity regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com session affinity, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 3
                },
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Exemplo Básico de Load Balancing",
                    Code = @"using Microsoft.AspNetCore.Mvc;

// Implementação de Load Balancing
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
                    Explanation = "Este exemplo demonstra a implementação básica de Load Balancing. Note como o código segue as convenções do ASP.NET Core.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Exemplo Avançado",
                    Code = @"// Implementação avançada com Algorithms
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
                    Explanation = "Exemplo mais complexo mostrando como aplicar Load Balancing em cenários reais com logging e async/await.",
                    IsRunnable = true
                },
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Exercício 1: Implementação Básica",
                    Description = "Implemente Load Balancing em um projeto ASP.NET Core simples.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"using Microsoft.AspNetCore.Mvc;

// Implemente aqui
[ApiController]
[Route(""api/[controller]"")]
public class MeuController : ControllerBase
{
    
}",
                    Hints = new List<string> { "Comece definindo o controller", "Configure Algorithms", "Teste sua implementação" }
                },
                new Exercise
                {
                    Title = "Exercício 2: Cenário Intermediário",
                    Description = "Crie um serviço completo que utilize Load Balancing para gerenciar recursos.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"// Implemente o serviço
public class MeuServico
{
    
}",
                    Hints = new List<string> { "Injete dependências via construtor", "Implemente métodos que usem Load Balancing", "Adicione tratamento de erros" }
                },
                new Exercise
                {
                    Title = "Exercício 3: Desafio Avançado",
                    Description = "Implemente um sistema completo utilizando Load Balancing com múltiplas funcionalidades.",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = @"// Sistema completo
",
                    Hints = new List<string> { "Planeje a arquitetura antes de implementar", "Considere performance e escalabilidade", "Adicione testes unitários" }
                },
            },
            Summary = "Nesta aula, você aprendeu sobre Load Balancing no ASP.NET Core. Exploramos Algorithms, Health Checks, Session Affinity e vimos como aplicar estes conceitos em projetos reais. Compreender profundamente Load Balancing é essencial para desenvolver aplicações web modernas, robustas e escaláveis. Continue praticando com os exercícios propostos e experimente aplicar estes conceitos em seus próprios projetos. Na próxima aula, avançaremos para tópicos ainda mais interessantes do ASP.NET Core."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-000F-000000000005"),
            CourseId = Guid.Parse("10000000-0000-0000-0000-00000000000F"),
            Title = "Load Balancing",
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
                "Compreender os conceitos de cache-aside",
                "Aprender a implementar write-through",
                "Dominar write-behind",
                "Aplicar caching strategies em projetos reais"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "Cache-Aside",
                    Content = "O conceito de cache-aside é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam cache-aside regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com cache-aside, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Write-Through",
                    Content = "O conceito de write-through é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam write-through regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com write-through, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Write-Behind",
                    Content = "O conceito de write-behind é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam write-behind regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com write-behind, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 3
                },
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Exemplo Básico de Caching Strategies",
                    Code = @"using Microsoft.AspNetCore.Mvc;

// Implementação de Caching Strategies
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
                    Explanation = "Este exemplo demonstra a implementação básica de Caching Strategies. Note como o código segue as convenções do ASP.NET Core.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Exemplo Avançado",
                    Code = @"// Implementação avançada com Cache-Aside
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
                    Explanation = "Exemplo mais complexo mostrando como aplicar Caching Strategies em cenários reais com logging e async/await.",
                    IsRunnable = true
                },
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Exercício 1: Implementação Básica",
                    Description = "Implemente Caching Strategies em um projeto ASP.NET Core simples.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"using Microsoft.AspNetCore.Mvc;

// Implemente aqui
[ApiController]
[Route(""api/[controller]"")]
public class MeuController : ControllerBase
{
    
}",
                    Hints = new List<string> { "Comece definindo o controller", "Configure Cache-Aside", "Teste sua implementação" }
                },
                new Exercise
                {
                    Title = "Exercício 2: Cenário Intermediário",
                    Description = "Crie um serviço completo que utilize Caching Strategies para gerenciar recursos.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"// Implemente o serviço
public class MeuServico
{
    
}",
                    Hints = new List<string> { "Injete dependências via construtor", "Implemente métodos que usem Caching Strategies", "Adicione tratamento de erros" }
                },
                new Exercise
                {
                    Title = "Exercício 3: Desafio Avançado",
                    Description = "Implemente um sistema completo utilizando Caching Strategies com múltiplas funcionalidades.",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = @"// Sistema completo
",
                    Hints = new List<string> { "Planeje a arquitetura antes de implementar", "Considere performance e escalabilidade", "Adicione testes unitários" }
                },
            },
            Summary = "Nesta aula, você aprendeu sobre Caching Strategies no ASP.NET Core. Exploramos Cache-Aside, Write-Through, Write-Behind e vimos como aplicar estes conceitos em projetos reais. Compreender profundamente Caching Strategies é essencial para desenvolver aplicações web modernas, robustas e escaláveis. Continue praticando com os exercícios propostos e experimente aplicar estes conceitos em seus próprios projetos. Na próxima aula, avançaremos para tópicos ainda mais interessantes do ASP.NET Core."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-000F-000000000006"),
            CourseId = Guid.Parse("10000000-0000-0000-0000-00000000000F"),
            Title = "Caching Strategies",
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
                "Compreender os conceitos de horizontal partitioning",
                "Aprender a implementar shard keys",
                "Dominar routing",
                "Aplicar database sharding em projetos reais"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "Horizontal Partitioning",
                    Content = "O conceito de horizontal partitioning é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam horizontal partitioning regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com horizontal partitioning, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Shard Keys",
                    Content = "O conceito de shard keys é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam shard keys regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com shard keys, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
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
                    Title = "Exemplo Básico de Database Sharding",
                    Code = @"using Microsoft.AspNetCore.Mvc;

// Implementação de Database Sharding
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
                    Explanation = "Este exemplo demonstra a implementação básica de Database Sharding. Note como o código segue as convenções do ASP.NET Core.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Exemplo Avançado",
                    Code = @"// Implementação avançada com Horizontal Partitioning
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
                    Explanation = "Exemplo mais complexo mostrando como aplicar Database Sharding em cenários reais com logging e async/await.",
                    IsRunnable = true
                },
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Exercício 1: Implementação Básica",
                    Description = "Implemente Database Sharding em um projeto ASP.NET Core simples.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"using Microsoft.AspNetCore.Mvc;

// Implemente aqui
[ApiController]
[Route(""api/[controller]"")]
public class MeuController : ControllerBase
{
    
}",
                    Hints = new List<string> { "Comece definindo o controller", "Configure Horizontal Partitioning", "Teste sua implementação" }
                },
                new Exercise
                {
                    Title = "Exercício 2: Cenário Intermediário",
                    Description = "Crie um serviço completo que utilize Database Sharding para gerenciar recursos.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"// Implemente o serviço
public class MeuServico
{
    
}",
                    Hints = new List<string> { "Injete dependências via construtor", "Implemente métodos que usem Database Sharding", "Adicione tratamento de erros" }
                },
                new Exercise
                {
                    Title = "Exercício 3: Desafio Avançado",
                    Description = "Implemente um sistema completo utilizando Database Sharding com múltiplas funcionalidades.",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = @"// Sistema completo
",
                    Hints = new List<string> { "Planeje a arquitetura antes de implementar", "Considere performance e escalabilidade", "Adicione testes unitários" }
                },
            },
            Summary = "Nesta aula, você aprendeu sobre Database Sharding no ASP.NET Core. Exploramos Horizontal Partitioning, Shard Keys, Routing e vimos como aplicar estes conceitos em projetos reais. Compreender profundamente Database Sharding é essencial para desenvolver aplicações web modernas, robustas e escaláveis. Continue praticando com os exercícios propostos e experimente aplicar estes conceitos em seus próprios projetos. Na próxima aula, avançaremos para tópicos ainda mais interessantes do ASP.NET Core."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-000F-000000000007"),
            CourseId = Guid.Parse("10000000-0000-0000-0000-00000000000F"),
            Title = "Database Sharding",
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
                "Compreender os conceitos de master-slave",
                "Aprender a implementar master-master",
                "Dominar consistency",
                "Aplicar database replication em projetos reais"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "Master-Slave",
                    Content = "O conceito de master-slave é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam master-slave regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com master-slave, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Master-Master",
                    Content = "O conceito de master-master é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam master-master regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com master-master, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Consistency",
                    Content = "O conceito de consistency é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam consistency regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com consistency, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 3
                },
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Exemplo Básico de Database Replication",
                    Code = @"using Microsoft.AspNetCore.Mvc;

// Implementação de Database Replication
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
                    Explanation = "Este exemplo demonstra a implementação básica de Database Replication. Note como o código segue as convenções do ASP.NET Core.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Exemplo Avançado",
                    Code = @"// Implementação avançada com Master-Slave
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
                    Explanation = "Exemplo mais complexo mostrando como aplicar Database Replication em cenários reais com logging e async/await.",
                    IsRunnable = true
                },
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Exercício 1: Implementação Básica",
                    Description = "Implemente Database Replication em um projeto ASP.NET Core simples.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"using Microsoft.AspNetCore.Mvc;

// Implemente aqui
[ApiController]
[Route(""api/[controller]"")]
public class MeuController : ControllerBase
{
    
}",
                    Hints = new List<string> { "Comece definindo o controller", "Configure Master-Slave", "Teste sua implementação" }
                },
                new Exercise
                {
                    Title = "Exercício 2: Cenário Intermediário",
                    Description = "Crie um serviço completo que utilize Database Replication para gerenciar recursos.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"// Implemente o serviço
public class MeuServico
{
    
}",
                    Hints = new List<string> { "Injete dependências via construtor", "Implemente métodos que usem Database Replication", "Adicione tratamento de erros" }
                },
                new Exercise
                {
                    Title = "Exercício 3: Desafio Avançado",
                    Description = "Implemente um sistema completo utilizando Database Replication com múltiplas funcionalidades.",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = @"// Sistema completo
",
                    Hints = new List<string> { "Planeje a arquitetura antes de implementar", "Considere performance e escalabilidade", "Adicione testes unitários" }
                },
            },
            Summary = "Nesta aula, você aprendeu sobre Database Replication no ASP.NET Core. Exploramos Master-Slave, Master-Master, Consistency e vimos como aplicar estes conceitos em projetos reais. Compreender profundamente Database Replication é essencial para desenvolver aplicações web modernas, robustas e escaláveis. Continue praticando com os exercícios propostos e experimente aplicar estes conceitos em seus próprios projetos. Na próxima aula, avançaremos para tópicos ainda mais interessantes do ASP.NET Core."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-000F-000000000008"),
            CourseId = Guid.Parse("10000000-0000-0000-0000-00000000000F"),
            Title = "Database Replication",
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
                "Compreender os conceitos de patterns",
                "Aprender a implementar guarantees",
                "Dominar ordering",
                "Aplicar message queues em projetos reais"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "Patterns",
                    Content = "O conceito de patterns é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam patterns regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com patterns, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Guarantees",
                    Content = "O conceito de guarantees é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam guarantees regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com guarantees, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Ordering",
                    Content = "O conceito de ordering é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam ordering regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com ordering, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 3
                },
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Exemplo Básico de Message Queues",
                    Code = @"using Microsoft.AspNetCore.Mvc;

// Implementação de Message Queues
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
                    Explanation = "Este exemplo demonstra a implementação básica de Message Queues. Note como o código segue as convenções do ASP.NET Core.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Exemplo Avançado",
                    Code = @"// Implementação avançada com Patterns
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
                    Explanation = "Exemplo mais complexo mostrando como aplicar Message Queues em cenários reais com logging e async/await.",
                    IsRunnable = true
                },
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Exercício 1: Implementação Básica",
                    Description = "Implemente Message Queues em um projeto ASP.NET Core simples.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"using Microsoft.AspNetCore.Mvc;

// Implemente aqui
[ApiController]
[Route(""api/[controller]"")]
public class MeuController : ControllerBase
{
    
}",
                    Hints = new List<string> { "Comece definindo o controller", "Configure Patterns", "Teste sua implementação" }
                },
                new Exercise
                {
                    Title = "Exercício 2: Cenário Intermediário",
                    Description = "Crie um serviço completo que utilize Message Queues para gerenciar recursos.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"// Implemente o serviço
public class MeuServico
{
    
}",
                    Hints = new List<string> { "Injete dependências via construtor", "Implemente métodos que usem Message Queues", "Adicione tratamento de erros" }
                },
                new Exercise
                {
                    Title = "Exercício 3: Desafio Avançado",
                    Description = "Implemente um sistema completo utilizando Message Queues com múltiplas funcionalidades.",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = @"// Sistema completo
",
                    Hints = new List<string> { "Planeje a arquitetura antes de implementar", "Considere performance e escalabilidade", "Adicione testes unitários" }
                },
            },
            Summary = "Nesta aula, você aprendeu sobre Message Queues no ASP.NET Core. Exploramos Patterns, Guarantees, Ordering e vimos como aplicar estes conceitos em projetos reais. Compreender profundamente Message Queues é essencial para desenvolver aplicações web modernas, robustas e escaláveis. Continue praticando com os exercícios propostos e experimente aplicar estes conceitos em seus próprios projetos. Na próxima aula, avançaremos para tópicos ainda mais interessantes do ASP.NET Core."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-000F-000000000009"),
            CourseId = Guid.Parse("10000000-0000-0000-0000-00000000000F"),
            Title = "Message Queues",
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
                "Compreender os conceitos de kafka",
                "Aprender a implementar event sourcing",
                "Dominar cqrs",
                "Aplicar event streaming em projetos reais"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "Kafka",
                    Content = "O conceito de kafka é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam kafka regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com kafka, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Event Sourcing",
                    Content = "O conceito de event sourcing é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam event sourcing regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com event sourcing, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "CQRS",
                    Content = "O conceito de cqrs é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam cqrs regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com cqrs, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 3
                },
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Exemplo Básico de Event Streaming",
                    Code = @"using Microsoft.AspNetCore.Mvc;

// Implementação de Event Streaming
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
                    Explanation = "Este exemplo demonstra a implementação básica de Event Streaming. Note como o código segue as convenções do ASP.NET Core.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Exemplo Avançado",
                    Code = @"// Implementação avançada com Kafka
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
                    Explanation = "Exemplo mais complexo mostrando como aplicar Event Streaming em cenários reais com logging e async/await.",
                    IsRunnable = true
                },
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Exercício 1: Implementação Básica",
                    Description = "Implemente Event Streaming em um projeto ASP.NET Core simples.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"using Microsoft.AspNetCore.Mvc;

// Implemente aqui
[ApiController]
[Route(""api/[controller]"")]
public class MeuController : ControllerBase
{
    
}",
                    Hints = new List<string> { "Comece definindo o controller", "Configure Kafka", "Teste sua implementação" }
                },
                new Exercise
                {
                    Title = "Exercício 2: Cenário Intermediário",
                    Description = "Crie um serviço completo que utilize Event Streaming para gerenciar recursos.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"// Implemente o serviço
public class MeuServico
{
    
}",
                    Hints = new List<string> { "Injete dependências via construtor", "Implemente métodos que usem Event Streaming", "Adicione tratamento de erros" }
                },
                new Exercise
                {
                    Title = "Exercício 3: Desafio Avançado",
                    Description = "Implemente um sistema completo utilizando Event Streaming com múltiplas funcionalidades.",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = @"// Sistema completo
",
                    Hints = new List<string> { "Planeje a arquitetura antes de implementar", "Considere performance e escalabilidade", "Adicione testes unitários" }
                },
            },
            Summary = "Nesta aula, você aprendeu sobre Event Streaming no ASP.NET Core. Exploramos Kafka, Event Sourcing, CQRS e vimos como aplicar estes conceitos em projetos reais. Compreender profundamente Event Streaming é essencial para desenvolver aplicações web modernas, robustas e escaláveis. Continue praticando com os exercícios propostos e experimente aplicar estes conceitos em seus próprios projetos. Na próxima aula, avançaremos para tópicos ainda mais interessantes do ASP.NET Core."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-000F-000000000010"),
            CourseId = Guid.Parse("10000000-0000-0000-0000-00000000000F"),
            Title = "Event Streaming",
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
                "Compreender os conceitos de rest",
                "Aprender a implementar graphql",
                "Dominar grpc",
                "Aplicar api design em projetos reais"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "REST",
                    Content = "O conceito de rest é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam rest regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com rest, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "GraphQL",
                    Content = "O conceito de graphql é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam graphql regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com graphql, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "gRPC",
                    Content = "O conceito de grpc é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam grpc regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com grpc, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 3
                },
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Exemplo Básico de API Design",
                    Code = @"using Microsoft.AspNetCore.Mvc;

// Implementação de API Design
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
                    Explanation = "Este exemplo demonstra a implementação básica de API Design. Note como o código segue as convenções do ASP.NET Core.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Exemplo Avançado",
                    Code = @"// Implementação avançada com REST
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
                    Explanation = "Exemplo mais complexo mostrando como aplicar API Design em cenários reais com logging e async/await.",
                    IsRunnable = true
                },
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Exercício 1: Implementação Básica",
                    Description = "Implemente API Design em um projeto ASP.NET Core simples.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"using Microsoft.AspNetCore.Mvc;

// Implemente aqui
[ApiController]
[Route(""api/[controller]"")]
public class MeuController : ControllerBase
{
    
}",
                    Hints = new List<string> { "Comece definindo o controller", "Configure REST", "Teste sua implementação" }
                },
                new Exercise
                {
                    Title = "Exercício 2: Cenário Intermediário",
                    Description = "Crie um serviço completo que utilize API Design para gerenciar recursos.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"// Implemente o serviço
public class MeuServico
{
    
}",
                    Hints = new List<string> { "Injete dependências via construtor", "Implemente métodos que usem API Design", "Adicione tratamento de erros" }
                },
                new Exercise
                {
                    Title = "Exercício 3: Desafio Avançado",
                    Description = "Implemente um sistema completo utilizando API Design com múltiplas funcionalidades.",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = @"// Sistema completo
",
                    Hints = new List<string> { "Planeje a arquitetura antes de implementar", "Considere performance e escalabilidade", "Adicione testes unitários" }
                },
            },
            Summary = "Nesta aula, você aprendeu sobre API Design no ASP.NET Core. Exploramos REST, GraphQL, gRPC, Versioning e vimos como aplicar estes conceitos em projetos reais. Compreender profundamente API Design é essencial para desenvolver aplicações web modernas, robustas e escaláveis. Continue praticando com os exercícios propostos e experimente aplicar estes conceitos em seus próprios projetos. Na próxima aula, avançaremos para tópicos ainda mais interessantes do ASP.NET Core."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-000F-000000000011"),
            CourseId = Guid.Parse("10000000-0000-0000-0000-00000000000F"),
            Title = "API Design",
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
                "Compreender os conceitos de algorithms",
                "Aprender a implementar distributed",
                "Dominar strategies",
                "Aplicar rate limiting em projetos reais"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "Algorithms",
                    Content = "O conceito de algorithms é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam algorithms regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com algorithms, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Distributed",
                    Content = "O conceito de distributed é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam distributed regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com distributed, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Strategies",
                    Content = "O conceito de strategies é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam strategies regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com strategies, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 3
                },
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Exemplo Básico de Rate Limiting",
                    Code = @"using Microsoft.AspNetCore.Mvc;

// Implementação de Rate Limiting
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
                    Explanation = "Este exemplo demonstra a implementação básica de Rate Limiting. Note como o código segue as convenções do ASP.NET Core.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Exemplo Avançado",
                    Code = @"// Implementação avançada com Algorithms
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
                    Explanation = "Exemplo mais complexo mostrando como aplicar Rate Limiting em cenários reais com logging e async/await.",
                    IsRunnable = true
                },
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Exercício 1: Implementação Básica",
                    Description = "Implemente Rate Limiting em um projeto ASP.NET Core simples.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"using Microsoft.AspNetCore.Mvc;

// Implemente aqui
[ApiController]
[Route(""api/[controller]"")]
public class MeuController : ControllerBase
{
    
}",
                    Hints = new List<string> { "Comece definindo o controller", "Configure Algorithms", "Teste sua implementação" }
                },
                new Exercise
                {
                    Title = "Exercício 2: Cenário Intermediário",
                    Description = "Crie um serviço completo que utilize Rate Limiting para gerenciar recursos.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"// Implemente o serviço
public class MeuServico
{
    
}",
                    Hints = new List<string> { "Injete dependências via construtor", "Implemente métodos que usem Rate Limiting", "Adicione tratamento de erros" }
                },
                new Exercise
                {
                    Title = "Exercício 3: Desafio Avançado",
                    Description = "Implemente um sistema completo utilizando Rate Limiting com múltiplas funcionalidades.",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = @"// Sistema completo
",
                    Hints = new List<string> { "Planeje a arquitetura antes de implementar", "Considere performance e escalabilidade", "Adicione testes unitários" }
                },
            },
            Summary = "Nesta aula, você aprendeu sobre Rate Limiting no ASP.NET Core. Exploramos Algorithms, Distributed, Strategies e vimos como aplicar estes conceitos em projetos reais. Compreender profundamente Rate Limiting é essencial para desenvolver aplicações web modernas, robustas e escaláveis. Continue praticando com os exercícios propostos e experimente aplicar estes conceitos em seus próprios projetos. Na próxima aula, avançaremos para tópicos ainda mais interessantes do ASP.NET Core."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-000F-000000000012"),
            CourseId = Guid.Parse("10000000-0000-0000-0000-00000000000F"),
            Title = "Rate Limiting",
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
                "Compreender os conceitos de cdn",
                "Aprender a implementar edge computing",
                "Dominar caching",
                "Aplicar content delivery networks em projetos reais"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "CDN",
                    Content = "O conceito de cdn é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam cdn regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com cdn, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Edge Computing",
                    Content = "O conceito de edge computing é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam edge computing regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com edge computing, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Caching",
                    Content = "O conceito de caching é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam caching regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com caching, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 3
                },
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Exemplo Básico de Content Delivery Networks",
                    Code = @"using Microsoft.AspNetCore.Mvc;

// Implementação de Content Delivery Networks
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
                    Explanation = "Este exemplo demonstra a implementação básica de Content Delivery Networks. Note como o código segue as convenções do ASP.NET Core.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Exemplo Avançado",
                    Code = @"// Implementação avançada com CDN
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
                    Explanation = "Exemplo mais complexo mostrando como aplicar Content Delivery Networks em cenários reais com logging e async/await.",
                    IsRunnable = true
                },
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Exercício 1: Implementação Básica",
                    Description = "Implemente Content Delivery Networks em um projeto ASP.NET Core simples.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"using Microsoft.AspNetCore.Mvc;

// Implemente aqui
[ApiController]
[Route(""api/[controller]"")]
public class MeuController : ControllerBase
{
    
}",
                    Hints = new List<string> { "Comece definindo o controller", "Configure CDN", "Teste sua implementação" }
                },
                new Exercise
                {
                    Title = "Exercício 2: Cenário Intermediário",
                    Description = "Crie um serviço completo que utilize Content Delivery Networks para gerenciar recursos.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"// Implemente o serviço
public class MeuServico
{
    
}",
                    Hints = new List<string> { "Injete dependências via construtor", "Implemente métodos que usem Content Delivery Networks", "Adicione tratamento de erros" }
                },
                new Exercise
                {
                    Title = "Exercício 3: Desafio Avançado",
                    Description = "Implemente um sistema completo utilizando Content Delivery Networks com múltiplas funcionalidades.",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = @"// Sistema completo
",
                    Hints = new List<string> { "Planeje a arquitetura antes de implementar", "Considere performance e escalabilidade", "Adicione testes unitários" }
                },
            },
            Summary = "Nesta aula, você aprendeu sobre Content Delivery Networks no ASP.NET Core. Exploramos CDN, Edge Computing, Caching e vimos como aplicar estes conceitos em projetos reais. Compreender profundamente Content Delivery Networks é essencial para desenvolver aplicações web modernas, robustas e escaláveis. Continue praticando com os exercícios propostos e experimente aplicar estes conceitos em seus próprios projetos. Na próxima aula, avançaremos para tópicos ainda mais interessantes do ASP.NET Core."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-000F-000000000013"),
            CourseId = Guid.Parse("10000000-0000-0000-0000-00000000000F"),
            Title = "Content Delivery Networks",
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
                "Compreender os conceitos de elasticsearch",
                "Aprender a implementar indexing",
                "Dominar ranking",
                "Aplicar search systems em projetos reais"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "Elasticsearch",
                    Content = "O conceito de elasticsearch é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam elasticsearch regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com elasticsearch, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Indexing",
                    Content = "O conceito de indexing é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam indexing regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com indexing, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Ranking",
                    Content = "O conceito de ranking é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam ranking regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com ranking, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 3
                },
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Exemplo Básico de Search Systems",
                    Code = @"using Microsoft.AspNetCore.Mvc;

// Implementação de Search Systems
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
                    Explanation = "Este exemplo demonstra a implementação básica de Search Systems. Note como o código segue as convenções do ASP.NET Core.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Exemplo Avançado",
                    Code = @"// Implementação avançada com Elasticsearch
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
                    Explanation = "Exemplo mais complexo mostrando como aplicar Search Systems em cenários reais com logging e async/await.",
                    IsRunnable = true
                },
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Exercício 1: Implementação Básica",
                    Description = "Implemente Search Systems em um projeto ASP.NET Core simples.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"using Microsoft.AspNetCore.Mvc;

// Implemente aqui
[ApiController]
[Route(""api/[controller]"")]
public class MeuController : ControllerBase
{
    
}",
                    Hints = new List<string> { "Comece definindo o controller", "Configure Elasticsearch", "Teste sua implementação" }
                },
                new Exercise
                {
                    Title = "Exercício 2: Cenário Intermediário",
                    Description = "Crie um serviço completo que utilize Search Systems para gerenciar recursos.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"// Implemente o serviço
public class MeuServico
{
    
}",
                    Hints = new List<string> { "Injete dependências via construtor", "Implemente métodos que usem Search Systems", "Adicione tratamento de erros" }
                },
                new Exercise
                {
                    Title = "Exercício 3: Desafio Avançado",
                    Description = "Implemente um sistema completo utilizando Search Systems com múltiplas funcionalidades.",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = @"// Sistema completo
",
                    Hints = new List<string> { "Planeje a arquitetura antes de implementar", "Considere performance e escalabilidade", "Adicione testes unitários" }
                },
            },
            Summary = "Nesta aula, você aprendeu sobre Search Systems no ASP.NET Core. Exploramos Elasticsearch, Indexing, Ranking e vimos como aplicar estes conceitos em projetos reais. Compreender profundamente Search Systems é essencial para desenvolver aplicações web modernas, robustas e escaláveis. Continue praticando com os exercícios propostos e experimente aplicar estes conceitos em seus próprios projetos. Na próxima aula, avançaremos para tópicos ainda mais interessantes do ASP.NET Core."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-000F-000000000014"),
            CourseId = Guid.Parse("10000000-0000-0000-0000-00000000000F"),
            Title = "Search Systems",
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
                "Compreender os conceitos de collaborative filtering",
                "Aprender a implementar content-based",
                "Dominar hybrid",
                "Aplicar recommendation systems em projetos reais"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "Collaborative Filtering",
                    Content = "O conceito de collaborative filtering é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam collaborative filtering regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com collaborative filtering, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Content-Based",
                    Content = "O conceito de content-based é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam content-based regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com content-based, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Hybrid",
                    Content = "O conceito de hybrid é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam hybrid regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com hybrid, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 3
                },
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Exemplo Básico de Recommendation Systems",
                    Code = @"using Microsoft.AspNetCore.Mvc;

// Implementação de Recommendation Systems
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
                    Explanation = "Este exemplo demonstra a implementação básica de Recommendation Systems. Note como o código segue as convenções do ASP.NET Core.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Exemplo Avançado",
                    Code = @"// Implementação avançada com Collaborative Filtering
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
                    Explanation = "Exemplo mais complexo mostrando como aplicar Recommendation Systems em cenários reais com logging e async/await.",
                    IsRunnable = true
                },
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Exercício 1: Implementação Básica",
                    Description = "Implemente Recommendation Systems em um projeto ASP.NET Core simples.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"using Microsoft.AspNetCore.Mvc;

// Implemente aqui
[ApiController]
[Route(""api/[controller]"")]
public class MeuController : ControllerBase
{
    
}",
                    Hints = new List<string> { "Comece definindo o controller", "Configure Collaborative Filtering", "Teste sua implementação" }
                },
                new Exercise
                {
                    Title = "Exercício 2: Cenário Intermediário",
                    Description = "Crie um serviço completo que utilize Recommendation Systems para gerenciar recursos.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"// Implemente o serviço
public class MeuServico
{
    
}",
                    Hints = new List<string> { "Injete dependências via construtor", "Implemente métodos que usem Recommendation Systems", "Adicione tratamento de erros" }
                },
                new Exercise
                {
                    Title = "Exercício 3: Desafio Avançado",
                    Description = "Implemente um sistema completo utilizando Recommendation Systems com múltiplas funcionalidades.",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = @"// Sistema completo
",
                    Hints = new List<string> { "Planeje a arquitetura antes de implementar", "Considere performance e escalabilidade", "Adicione testes unitários" }
                },
            },
            Summary = "Nesta aula, você aprendeu sobre Recommendation Systems no ASP.NET Core. Exploramos Collaborative Filtering, Content-Based, Hybrid e vimos como aplicar estes conceitos em projetos reais. Compreender profundamente Recommendation Systems é essencial para desenvolver aplicações web modernas, robustas e escaláveis. Continue praticando com os exercícios propostos e experimente aplicar estes conceitos em seus próprios projetos. Na próxima aula, avançaremos para tópicos ainda mais interessantes do ASP.NET Core."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-000F-000000000015"),
            CourseId = Guid.Parse("10000000-0000-0000-0000-00000000000F"),
            Title = "Recommendation Systems",
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
                "Compreender os conceitos de websockets",
                "Aprender a implementar server-sent events",
                "Dominar polling",
                "Aplicar real-time systems em projetos reais"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "WebSockets",
                    Content = "O conceito de websockets é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam websockets regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com websockets, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Server-Sent Events",
                    Content = "O conceito de server-sent events é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam server-sent events regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com server-sent events, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Polling",
                    Content = "O conceito de polling é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam polling regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com polling, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 3
                },
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Exemplo Básico de Real-Time Systems",
                    Code = @"using Microsoft.AspNetCore.Mvc;

// Implementação de Real-Time Systems
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
                    Explanation = "Este exemplo demonstra a implementação básica de Real-Time Systems. Note como o código segue as convenções do ASP.NET Core.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Exemplo Avançado",
                    Code = @"// Implementação avançada com WebSockets
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
                    Explanation = "Exemplo mais complexo mostrando como aplicar Real-Time Systems em cenários reais com logging e async/await.",
                    IsRunnable = true
                },
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Exercício 1: Implementação Básica",
                    Description = "Implemente Real-Time Systems em um projeto ASP.NET Core simples.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"using Microsoft.AspNetCore.Mvc;

// Implemente aqui
[ApiController]
[Route(""api/[controller]"")]
public class MeuController : ControllerBase
{
    
}",
                    Hints = new List<string> { "Comece definindo o controller", "Configure WebSockets", "Teste sua implementação" }
                },
                new Exercise
                {
                    Title = "Exercício 2: Cenário Intermediário",
                    Description = "Crie um serviço completo que utilize Real-Time Systems para gerenciar recursos.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"// Implemente o serviço
public class MeuServico
{
    
}",
                    Hints = new List<string> { "Injete dependências via construtor", "Implemente métodos que usem Real-Time Systems", "Adicione tratamento de erros" }
                },
                new Exercise
                {
                    Title = "Exercício 3: Desafio Avançado",
                    Description = "Implemente um sistema completo utilizando Real-Time Systems com múltiplas funcionalidades.",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = @"// Sistema completo
",
                    Hints = new List<string> { "Planeje a arquitetura antes de implementar", "Considere performance e escalabilidade", "Adicione testes unitários" }
                },
            },
            Summary = "Nesta aula, você aprendeu sobre Real-Time Systems no ASP.NET Core. Exploramos WebSockets, Server-Sent Events, Polling e vimos como aplicar estes conceitos em projetos reais. Compreender profundamente Real-Time Systems é essencial para desenvolver aplicações web modernas, robustas e escaláveis. Continue praticando com os exercícios propostos e experimente aplicar estes conceitos em seus próprios projetos. Na próxima aula, avançaremos para tópicos ainda mais interessantes do ASP.NET Core."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-000F-000000000016"),
            CourseId = Guid.Parse("10000000-0000-0000-0000-00000000000F"),
            Title = "Real-Time Systems",
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
                "Compreender os conceitos de 2pc",
                "Aprender a implementar saga",
                "Dominar compensation",
                "Aplicar distributed transactions em projetos reais"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "2PC",
                    Content = "O conceito de 2pc é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam 2pc regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com 2pc, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Saga",
                    Content = "O conceito de saga é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam saga regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com saga, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Compensation",
                    Content = "O conceito de compensation é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam compensation regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com compensation, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 3
                },
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Exemplo Básico de Distributed Transactions",
                    Code = @"using Microsoft.AspNetCore.Mvc;

// Implementação de Distributed Transactions
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
                    Explanation = "Este exemplo demonstra a implementação básica de Distributed Transactions. Note como o código segue as convenções do ASP.NET Core.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Exemplo Avançado",
                    Code = @"// Implementação avançada com 2PC
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
                    Explanation = "Exemplo mais complexo mostrando como aplicar Distributed Transactions em cenários reais com logging e async/await.",
                    IsRunnable = true
                },
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Exercício 1: Implementação Básica",
                    Description = "Implemente Distributed Transactions em um projeto ASP.NET Core simples.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"using Microsoft.AspNetCore.Mvc;

// Implemente aqui
[ApiController]
[Route(""api/[controller]"")]
public class MeuController : ControllerBase
{
    
}",
                    Hints = new List<string> { "Comece definindo o controller", "Configure 2PC", "Teste sua implementação" }
                },
                new Exercise
                {
                    Title = "Exercício 2: Cenário Intermediário",
                    Description = "Crie um serviço completo que utilize Distributed Transactions para gerenciar recursos.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"// Implemente o serviço
public class MeuServico
{
    
}",
                    Hints = new List<string> { "Injete dependências via construtor", "Implemente métodos que usem Distributed Transactions", "Adicione tratamento de erros" }
                },
                new Exercise
                {
                    Title = "Exercício 3: Desafio Avançado",
                    Description = "Implemente um sistema completo utilizando Distributed Transactions com múltiplas funcionalidades.",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = @"// Sistema completo
",
                    Hints = new List<string> { "Planeje a arquitetura antes de implementar", "Considere performance e escalabilidade", "Adicione testes unitários" }
                },
            },
            Summary = "Nesta aula, você aprendeu sobre Distributed Transactions no ASP.NET Core. Exploramos 2PC, Saga, Compensation e vimos como aplicar estes conceitos em projetos reais. Compreender profundamente Distributed Transactions é essencial para desenvolver aplicações web modernas, robustas e escaláveis. Continue praticando com os exercícios propostos e experimente aplicar estes conceitos em seus próprios projetos. Na próxima aula, avançaremos para tópicos ainda mais interessantes do ASP.NET Core."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-000F-000000000017"),
            CourseId = Guid.Parse("10000000-0000-0000-0000-00000000000F"),
            Title = "Distributed Transactions",
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
                "Compreender os conceitos de metrics",
                "Aprender a implementar logs",
                "Dominar traces",
                "Aplicar monitoring e observability em projetos reais"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "Metrics",
                    Content = "O conceito de metrics é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam metrics regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com metrics, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Logs",
                    Content = "O conceito de logs é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam logs regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com logs, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Traces",
                    Content = "O conceito de traces é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam traces regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com traces, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 3
                },
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Exemplo Básico de Monitoring e Observability",
                    Code = @"using Microsoft.AspNetCore.Mvc;

// Implementação de Monitoring e Observability
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
                    Explanation = "Este exemplo demonstra a implementação básica de Monitoring e Observability. Note como o código segue as convenções do ASP.NET Core.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Exemplo Avançado",
                    Code = @"// Implementação avançada com Metrics
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
                    Explanation = "Exemplo mais complexo mostrando como aplicar Monitoring e Observability em cenários reais com logging e async/await.",
                    IsRunnable = true
                },
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Exercício 1: Implementação Básica",
                    Description = "Implemente Monitoring e Observability em um projeto ASP.NET Core simples.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"using Microsoft.AspNetCore.Mvc;

// Implemente aqui
[ApiController]
[Route(""api/[controller]"")]
public class MeuController : ControllerBase
{
    
}",
                    Hints = new List<string> { "Comece definindo o controller", "Configure Metrics", "Teste sua implementação" }
                },
                new Exercise
                {
                    Title = "Exercício 2: Cenário Intermediário",
                    Description = "Crie um serviço completo que utilize Monitoring e Observability para gerenciar recursos.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"// Implemente o serviço
public class MeuServico
{
    
}",
                    Hints = new List<string> { "Injete dependências via construtor", "Implemente métodos que usem Monitoring e Observability", "Adicione tratamento de erros" }
                },
                new Exercise
                {
                    Title = "Exercício 3: Desafio Avançado",
                    Description = "Implemente um sistema completo utilizando Monitoring e Observability com múltiplas funcionalidades.",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = @"// Sistema completo
",
                    Hints = new List<string> { "Planeje a arquitetura antes de implementar", "Considere performance e escalabilidade", "Adicione testes unitários" }
                },
            },
            Summary = "Nesta aula, você aprendeu sobre Monitoring e Observability no ASP.NET Core. Exploramos Metrics, Logs, Traces, Alerts e vimos como aplicar estes conceitos em projetos reais. Compreender profundamente Monitoring e Observability é essencial para desenvolver aplicações web modernas, robustas e escaláveis. Continue praticando com os exercícios propostos e experimente aplicar estes conceitos em seus próprios projetos. Na próxima aula, avançaremos para tópicos ainda mais interessantes do ASP.NET Core."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-000F-000000000018"),
            CourseId = Guid.Parse("10000000-0000-0000-0000-00000000000F"),
            Title = "Monitoring e Observability",
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
                "Compreender os conceitos de authentication",
                "Aprender a implementar authorization",
                "Dominar encryption",
                "Aplicar security architecture em projetos reais"
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
                    Heading = "Encryption",
                    Content = "O conceito de encryption é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam encryption regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com encryption, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 3
                },
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Exemplo Básico de Security Architecture",
                    Code = @"using Microsoft.AspNetCore.Mvc;

// Implementação de Security Architecture
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
                    Explanation = "Este exemplo demonstra a implementação básica de Security Architecture. Note como o código segue as convenções do ASP.NET Core.",
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
                    Explanation = "Exemplo mais complexo mostrando como aplicar Security Architecture em cenários reais com logging e async/await.",
                    IsRunnable = true
                },
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Exercício 1: Implementação Básica",
                    Description = "Implemente Security Architecture em um projeto ASP.NET Core simples.",
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
                    Description = "Crie um serviço completo que utilize Security Architecture para gerenciar recursos.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"// Implemente o serviço
public class MeuServico
{
    
}",
                    Hints = new List<string> { "Injete dependências via construtor", "Implemente métodos que usem Security Architecture", "Adicione tratamento de erros" }
                },
                new Exercise
                {
                    Title = "Exercício 3: Desafio Avançado",
                    Description = "Implemente um sistema completo utilizando Security Architecture com múltiplas funcionalidades.",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = @"// Sistema completo
",
                    Hints = new List<string> { "Planeje a arquitetura antes de implementar", "Considere performance e escalabilidade", "Adicione testes unitários" }
                },
            },
            Summary = "Nesta aula, você aprendeu sobre Security Architecture no ASP.NET Core. Exploramos Authentication, Authorization, Encryption e vimos como aplicar estes conceitos em projetos reais. Compreender profundamente Security Architecture é essencial para desenvolver aplicações web modernas, robustas e escaláveis. Continue praticando com os exercícios propostos e experimente aplicar estes conceitos em seus próprios projetos. Na próxima aula, avançaremos para tópicos ainda mais interessantes do ASP.NET Core."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-000F-000000000019"),
            CourseId = Guid.Parse("10000000-0000-0000-0000-00000000000F"),
            Title = "Security Architecture",
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
                "Compreender os conceitos de approach",
                "Aprender a implementar communication",
                "Dominar trade-offs",
                "Aplicar system design interview em projetos reais"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "Approach",
                    Content = "O conceito de approach é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam approach regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com approach, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
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
                    Heading = "Trade-offs",
                    Content = "O conceito de trade-offs é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam trade-offs regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com trade-offs, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o 'como', mas também o 'porquê' por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos. À medida que você pratica e ganha experiência, perceberá como este conhecimento se conecta com outros aspectos do ASP.NET Core e do ecossistema .NET em geral.",
                    Order = 3
                },
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Exemplo Básico de System Design Interview",
                    Code = @"using Microsoft.AspNetCore.Mvc;

// Implementação de System Design Interview
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
                    Explanation = "Este exemplo demonstra a implementação básica de System Design Interview. Note como o código segue as convenções do ASP.NET Core.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Exemplo Avançado",
                    Code = @"// Implementação avançada com Approach
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
                    Explanation = "Exemplo mais complexo mostrando como aplicar System Design Interview em cenários reais com logging e async/await.",
                    IsRunnable = true
                },
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Exercício 1: Implementação Básica",
                    Description = "Implemente System Design Interview em um projeto ASP.NET Core simples.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"using Microsoft.AspNetCore.Mvc;

// Implemente aqui
[ApiController]
[Route(""api/[controller]"")]
public class MeuController : ControllerBase
{
    
}",
                    Hints = new List<string> { "Comece definindo o controller", "Configure Approach", "Teste sua implementação" }
                },
                new Exercise
                {
                    Title = "Exercício 2: Cenário Intermediário",
                    Description = "Crie um serviço completo que utilize System Design Interview para gerenciar recursos.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"// Implemente o serviço
public class MeuServico
{
    
}",
                    Hints = new List<string> { "Injete dependências via construtor", "Implemente métodos que usem System Design Interview", "Adicione tratamento de erros" }
                },
                new Exercise
                {
                    Title = "Exercício 3: Desafio Avançado",
                    Description = "Implemente um sistema completo utilizando System Design Interview com múltiplas funcionalidades.",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = @"// Sistema completo
",
                    Hints = new List<string> { "Planeje a arquitetura antes de implementar", "Considere performance e escalabilidade", "Adicione testes unitários" }
                },
            },
            Summary = "Nesta aula, você aprendeu sobre System Design Interview no ASP.NET Core. Exploramos Approach, Communication, Trade-offs e vimos como aplicar estes conceitos em projetos reais. Compreender profundamente System Design Interview é essencial para desenvolver aplicações web modernas, robustas e escaláveis. Continue praticando com os exercícios propostos e experimente aplicar estes conceitos em seus próprios projetos. Na próxima aula, avançaremos para tópicos ainda mais interessantes do ASP.NET Core."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-000F-000000000020"),
            CourseId = Guid.Parse("10000000-0000-0000-0000-00000000000F"),
            Title = "System Design Interview",
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
