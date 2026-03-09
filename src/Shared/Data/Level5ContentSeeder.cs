using Shared.Entities;
using Shared.Models;
using System.Text.Json;

namespace Shared.Data;

/// <summary>
/// Level 5 Content Seeder - ASP.NET Core Fundamentos
/// Topics: MVC, Routing, Middleware, Dependency Injection, Configuration, Logging
/// </summary>
public partial class Level5ContentSeeder
{
    private readonly Guid _courseId = Guid.Parse("10000000-0000-0000-0000-000000000006");
    private readonly Guid _levelId = Guid.Parse("00000000-0000-0000-0000-000000000005");

    public Course CreateLevel5Course()
    {
        return new Course
        {
            Id = _courseId,
            LevelId = _levelId,
            Title = "ASP.NET Core Fundamentos",
            Description = "Curso completo de ASP.NET Core Fundamentos com 20 aulas práticas e projetos reais.",
            Level = Level.Intermediate,
            Duration = "4 semanas",
            LessonCount = 20,
            Topics = JsonSerializer.Serialize(new[] { "MVC", "Routing", "Middleware", "Dependency Injection", "Configuration", "Logging" }),
            OrderIndex = 1,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }

    public List<Lesson> CreateLevel5Lessons()
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
                "Compreender os conceitos de introdução",
                "Aprender a implementar ao",
                "Dominar asp.net",
                "Aplicar introdução ao asp.net core em projetos reais"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "Introdução ao ASP.NET Core - Conceitos Fundamentais",
                    Content = "O conceito de introdução ao asp.net core é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam introdução regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com introdução, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o como, mas também o porquê por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Implementação Prática",
                    Content = "A implementação de ao no ASP.NET Core segue padrões bem estabelecidos que facilitam o desenvolvimento e manutenção do código. Ao trabalhar com este conceito, você utilizará recursos nativos do framework que foram projetados especificamente para este propósito. A Microsoft investiu significativamente em tornar esta funcionalidade robusta e fácil de usar. Desenvolvedores ao redor do mundo confiam nesta abordagem para construir aplicações de missão crítica. É essencial entender não apenas a sintaxe, mas também os princípios subjacentes que guiam o design desta funcionalidade. Ao dominar estes conceitos, você estará preparado para enfrentar desafios complexos em projetos reais. A prática constante e a experimentação são fundamentais para consolidar este conhecimento.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Melhores Práticas",
                    Content = "Ao trabalhar com asp.net, é crucial seguir as melhores práticas estabelecidas pela comunidade .NET. Estas práticas foram desenvolvidas ao longo de anos de experiência coletiva e ajudam a evitar armadilhas comuns. Segurança, performance e manutenibilidade devem estar sempre em mente ao implementar esta funcionalidade. Testes automatizados são essenciais para garantir que seu código funcione conforme esperado. Documentação clara ajuda outros desenvolvedores a entender suas decisões de design. Code reviews e pair programming são excelentes formas de compartilhar conhecimento e melhorar a qualidade do código. Lembre-se que código bom é código que outros desenvolvedores conseguem entender e manter facilmente.",
                    Order = 3
                }
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Exemplo Básico de Introdução ao ASP.NET Core",
                    Code = @"using Microsoft.AspNetCore.Mvc;

// Implementação de Introdução ao ASP.NET Core
using Microsoft.AspNetCore.Mvc;

namespace MeuApp.Controllers
{
    public class ExemploController : Controller
    {
        public IActionResult Index()
        {
            // Implementação básica
            return View();
        }
    }
}",
                    Language = "csharp",
                    Explanation = "Este exemplo demonstra a implementação básica de introdução ao asp.net core no ASP.NET Core.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Exemplo Avançado de Introdução ao ASP.NET Core",
                    Code = @"using Microsoft.AspNetCore.Mvc;

// Implementação avançada de Introdução ao ASP.NET Core
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MeuApp.Controllers
{
    [ApiController]
    [Route(""api/[controller]"")]
    public class AvancadoController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetData()
        {
            // Implementação avançada
            await Task.Delay(100);
            return Ok(new { message = ""Sucesso"" });
        }
    }
}",
                    Language = "csharp",
                    Explanation = "Este exemplo mostra uma implementação mais avançada utilizando async/await e API controllers.",
                    IsRunnable = true
                }
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Exercício Básico - Introdução ao ASP.NET Core",
                    Description = "Implemente uma solução básica utilizando os conceitos de introdução ao asp.net core.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"// Implemente sua solução aqui
public class MinhaImplementacao
{
    // Seu código
}",
                    Hints = new List<string> { "Revise os exemplos de código", "Consulte a documentação oficial", "Teste sua implementação" }
                },
                new Exercise
                {
                    Title = "Exercício Intermediário - Introdução ao ASP.NET Core",
                    Description = "Crie uma implementação mais complexa que demonstre domínio dos conceitos.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"// Implemente uma solução intermediária
public class ImplementacaoIntermediaria
{
    // Seu código aqui
}",
                    Hints = new List<string> { "Considere casos de erro", "Implemente validações", "Use async/await quando apropriado" }
                },
                new Exercise
                {
                    Title = "Exercício Avançado - Introdução ao ASP.NET Core",
                    Description = "Desenvolva uma solução completa que integre múltiplos conceitos.",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = @"// Implemente uma solução avançada
public class ImplementacaoAvancada
{
    // Seu código completo aqui
}",
                    Hints = new List<string> { "Integre com outros componentes", "Implemente tratamento de erros robusto", "Considere performance e escalabilidade" }
                }
            },
            Summary = "Nesta aula você aprendeu sobre introdução ao asp.net core, incluindo conceitos fundamentais, implementação prática e melhores práticas. Continue praticando para dominar completamente este tópico essencial do ASP.NET Core."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0006-000000000001"),
            CourseId = _courseId,
            Title = "Introdução ao ASP.NET Core",
            Duration = "45 min",
            Difficulty = "Intermediário",
            EstimatedMinutes = 45,
            Prerequisites = JsonSerializer.Serialize(new[] { "10000000-0000-0000-0006-000000000000" }),
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
                "Compreender os conceitos de program.cs",
                "Aprender a implementar e",
                "Dominar startup",
                "Aplicar program.cs e startup em projetos reais"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "Program.cs e Startup - Conceitos Fundamentais",
                    Content = "O conceito de program.cs e startup é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam program.cs regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com program.cs, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o como, mas também o porquê por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Implementação Prática",
                    Content = "A implementação de e no ASP.NET Core segue padrões bem estabelecidos que facilitam o desenvolvimento e manutenção do código. Ao trabalhar com este conceito, você utilizará recursos nativos do framework que foram projetados especificamente para este propósito. A Microsoft investiu significativamente em tornar esta funcionalidade robusta e fácil de usar. Desenvolvedores ao redor do mundo confiam nesta abordagem para construir aplicações de missão crítica. É essencial entender não apenas a sintaxe, mas também os princípios subjacentes que guiam o design desta funcionalidade. Ao dominar estes conceitos, você estará preparado para enfrentar desafios complexos em projetos reais. A prática constante e a experimentação são fundamentais para consolidar este conhecimento.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Melhores Práticas",
                    Content = "Ao trabalhar com startup, é crucial seguir as melhores práticas estabelecidas pela comunidade .NET. Estas práticas foram desenvolvidas ao longo de anos de experiência coletiva e ajudam a evitar armadilhas comuns. Segurança, performance e manutenibilidade devem estar sempre em mente ao implementar esta funcionalidade. Testes automatizados são essenciais para garantir que seu código funcione conforme esperado. Documentação clara ajuda outros desenvolvedores a entender suas decisões de design. Code reviews e pair programming são excelentes formas de compartilhar conhecimento e melhorar a qualidade do código. Lembre-se que código bom é código que outros desenvolvedores conseguem entender e manter facilmente.",
                    Order = 3
                }
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Exemplo Básico de Program.cs e Startup",
                    Code = @"using Microsoft.AspNetCore.Mvc;

// Implementação de Program.cs e Startup
using Microsoft.AspNetCore.Mvc;

namespace MeuApp.Controllers
{
    public class ExemploController : Controller
    {
        public IActionResult Index()
        {
            // Implementação básica
            return View();
        }
    }
}",
                    Language = "csharp",
                    Explanation = "Este exemplo demonstra a implementação básica de program.cs e startup no ASP.NET Core.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Exemplo Avançado de Program.cs e Startup",
                    Code = @"using Microsoft.AspNetCore.Mvc;

// Implementação avançada de Program.cs e Startup
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MeuApp.Controllers
{
    [ApiController]
    [Route(""api/[controller]"")]
    public class AvancadoController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetData()
        {
            // Implementação avançada
            await Task.Delay(100);
            return Ok(new { message = ""Sucesso"" });
        }
    }
}",
                    Language = "csharp",
                    Explanation = "Este exemplo mostra uma implementação mais avançada utilizando async/await e API controllers.",
                    IsRunnable = true
                }
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Exercício Básico - Program.cs e Startup",
                    Description = "Implemente uma solução básica utilizando os conceitos de program.cs e startup.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"// Implemente sua solução aqui
public class MinhaImplementacao
{
    // Seu código
}",
                    Hints = new List<string> { "Revise os exemplos de código", "Consulte a documentação oficial", "Teste sua implementação" }
                },
                new Exercise
                {
                    Title = "Exercício Intermediário - Program.cs e Startup",
                    Description = "Crie uma implementação mais complexa que demonstre domínio dos conceitos.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"// Implemente uma solução intermediária
public class ImplementacaoIntermediaria
{
    // Seu código aqui
}",
                    Hints = new List<string> { "Considere casos de erro", "Implemente validações", "Use async/await quando apropriado" }
                },
                new Exercise
                {
                    Title = "Exercício Avançado - Program.cs e Startup",
                    Description = "Desenvolva uma solução completa que integre múltiplos conceitos.",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = @"// Implemente uma solução avançada
public class ImplementacaoAvancada
{
    // Seu código completo aqui
}",
                    Hints = new List<string> { "Integre com outros componentes", "Implemente tratamento de erros robusto", "Considere performance e escalabilidade" }
                }
            },
            Summary = "Nesta aula você aprendeu sobre program.cs e startup, incluindo conceitos fundamentais, implementação prática e melhores práticas. Continue praticando para dominar completamente este tópico essencial do ASP.NET Core."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0006-000000000002"),
            CourseId = _courseId,
            Title = "Program.cs e Startup",
            Duration = "45 min",
            Difficulty = "Intermediário",
            EstimatedMinutes = 45,
            Prerequisites = JsonSerializer.Serialize(new[] { "10000000-0000-0000-0006-000000000001" }),
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
                "Compreender os conceitos de middleware",
                "Aprender a implementar pipeline",
                "Dominar aplicação",
                "Aplicar middleware pipeline em projetos reais"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "Middleware Pipeline - Conceitos Fundamentais",
                    Content = "O conceito de middleware pipeline é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam middleware regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com middleware, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o como, mas também o porquê por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Implementação Prática",
                    Content = "A implementação de pipeline no ASP.NET Core segue padrões bem estabelecidos que facilitam o desenvolvimento e manutenção do código. Ao trabalhar com este conceito, você utilizará recursos nativos do framework que foram projetados especificamente para este propósito. A Microsoft investiu significativamente em tornar esta funcionalidade robusta e fácil de usar. Desenvolvedores ao redor do mundo confiam nesta abordagem para construir aplicações de missão crítica. É essencial entender não apenas a sintaxe, mas também os princípios subjacentes que guiam o design desta funcionalidade. Ao dominar estes conceitos, você estará preparado para enfrentar desafios complexos em projetos reais. A prática constante e a experimentação são fundamentais para consolidar este conhecimento.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Melhores Práticas",
                    Content = "Ao trabalhar com aplicação, é crucial seguir as melhores práticas estabelecidas pela comunidade .NET. Estas práticas foram desenvolvidas ao longo de anos de experiência coletiva e ajudam a evitar armadilhas comuns. Segurança, performance e manutenibilidade devem estar sempre em mente ao implementar esta funcionalidade. Testes automatizados são essenciais para garantir que seu código funcione conforme esperado. Documentação clara ajuda outros desenvolvedores a entender suas decisões de design. Code reviews e pair programming são excelentes formas de compartilhar conhecimento e melhorar a qualidade do código. Lembre-se que código bom é código que outros desenvolvedores conseguem entender e manter facilmente.",
                    Order = 3
                }
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Exemplo Básico de Middleware Pipeline",
                    Code = @"using Microsoft.AspNetCore.Mvc;

// Implementação de Middleware Pipeline
using Microsoft.AspNetCore.Mvc;

namespace MeuApp.Controllers
{
    public class ExemploController : Controller
    {
        public IActionResult Index()
        {
            // Implementação básica
            return View();
        }
    }
}",
                    Language = "csharp",
                    Explanation = "Este exemplo demonstra a implementação básica de middleware pipeline no ASP.NET Core.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Exemplo Avançado de Middleware Pipeline",
                    Code = @"using Microsoft.AspNetCore.Mvc;

// Implementação avançada de Middleware Pipeline
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MeuApp.Controllers
{
    [ApiController]
    [Route(""api/[controller]"")]
    public class AvancadoController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetData()
        {
            // Implementação avançada
            await Task.Delay(100);
            return Ok(new { message = ""Sucesso"" });
        }
    }
}",
                    Language = "csharp",
                    Explanation = "Este exemplo mostra uma implementação mais avançada utilizando async/await e API controllers.",
                    IsRunnable = true
                }
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Exercício Básico - Middleware Pipeline",
                    Description = "Implemente uma solução básica utilizando os conceitos de middleware pipeline.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"// Implemente sua solução aqui
public class MinhaImplementacao
{
    // Seu código
}",
                    Hints = new List<string> { "Revise os exemplos de código", "Consulte a documentação oficial", "Teste sua implementação" }
                },
                new Exercise
                {
                    Title = "Exercício Intermediário - Middleware Pipeline",
                    Description = "Crie uma implementação mais complexa que demonstre domínio dos conceitos.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"// Implemente uma solução intermediária
public class ImplementacaoIntermediaria
{
    // Seu código aqui
}",
                    Hints = new List<string> { "Considere casos de erro", "Implemente validações", "Use async/await quando apropriado" }
                },
                new Exercise
                {
                    Title = "Exercício Avançado - Middleware Pipeline",
                    Description = "Desenvolva uma solução completa que integre múltiplos conceitos.",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = @"// Implemente uma solução avançada
public class ImplementacaoAvancada
{
    // Seu código completo aqui
}",
                    Hints = new List<string> { "Integre com outros componentes", "Implemente tratamento de erros robusto", "Considere performance e escalabilidade" }
                }
            },
            Summary = "Nesta aula você aprendeu sobre middleware pipeline, incluindo conceitos fundamentais, implementação prática e melhores práticas. Continue praticando para dominar completamente este tópico essencial do ASP.NET Core."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0006-000000000003"),
            CourseId = _courseId,
            Title = "Middleware Pipeline",
            Duration = "45 min",
            Difficulty = "Intermediário",
            EstimatedMinutes = 45,
            Prerequisites = JsonSerializer.Serialize(new[] { "10000000-0000-0000-0006-000000000002" }),
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
                "Compreender os conceitos de dependency",
                "Aprender a implementar injection",
                "Dominar aplicação",
                "Aplicar dependency injection em projetos reais"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "Dependency Injection - Conceitos Fundamentais",
                    Content = "O conceito de dependency injection é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam dependency regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com dependency, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o como, mas também o porquê por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Implementação Prática",
                    Content = "A implementação de injection no ASP.NET Core segue padrões bem estabelecidos que facilitam o desenvolvimento e manutenção do código. Ao trabalhar com este conceito, você utilizará recursos nativos do framework que foram projetados especificamente para este propósito. A Microsoft investiu significativamente em tornar esta funcionalidade robusta e fácil de usar. Desenvolvedores ao redor do mundo confiam nesta abordagem para construir aplicações de missão crítica. É essencial entender não apenas a sintaxe, mas também os princípios subjacentes que guiam o design desta funcionalidade. Ao dominar estes conceitos, você estará preparado para enfrentar desafios complexos em projetos reais. A prática constante e a experimentação são fundamentais para consolidar este conhecimento.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Melhores Práticas",
                    Content = "Ao trabalhar com aplicação, é crucial seguir as melhores práticas estabelecidas pela comunidade .NET. Estas práticas foram desenvolvidas ao longo de anos de experiência coletiva e ajudam a evitar armadilhas comuns. Segurança, performance e manutenibilidade devem estar sempre em mente ao implementar esta funcionalidade. Testes automatizados são essenciais para garantir que seu código funcione conforme esperado. Documentação clara ajuda outros desenvolvedores a entender suas decisões de design. Code reviews e pair programming são excelentes formas de compartilhar conhecimento e melhorar a qualidade do código. Lembre-se que código bom é código que outros desenvolvedores conseguem entender e manter facilmente.",
                    Order = 3
                }
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Exemplo Básico de Dependency Injection",
                    Code = @"using Microsoft.AspNetCore.Mvc;

// Implementação de Dependency Injection
using Microsoft.AspNetCore.Mvc;

namespace MeuApp.Controllers
{
    public class ExemploController : Controller
    {
        public IActionResult Index()
        {
            // Implementação básica
            return View();
        }
    }
}",
                    Language = "csharp",
                    Explanation = "Este exemplo demonstra a implementação básica de dependency injection no ASP.NET Core.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Exemplo Avançado de Dependency Injection",
                    Code = @"using Microsoft.AspNetCore.Mvc;

// Implementação avançada de Dependency Injection
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MeuApp.Controllers
{
    [ApiController]
    [Route(""api/[controller]"")]
    public class AvancadoController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetData()
        {
            // Implementação avançada
            await Task.Delay(100);
            return Ok(new { message = ""Sucesso"" });
        }
    }
}",
                    Language = "csharp",
                    Explanation = "Este exemplo mostra uma implementação mais avançada utilizando async/await e API controllers.",
                    IsRunnable = true
                }
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Exercício Básico - Dependency Injection",
                    Description = "Implemente uma solução básica utilizando os conceitos de dependency injection.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"// Implemente sua solução aqui
public class MinhaImplementacao
{
    // Seu código
}",
                    Hints = new List<string> { "Revise os exemplos de código", "Consulte a documentação oficial", "Teste sua implementação" }
                },
                new Exercise
                {
                    Title = "Exercício Intermediário - Dependency Injection",
                    Description = "Crie uma implementação mais complexa que demonstre domínio dos conceitos.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"// Implemente uma solução intermediária
public class ImplementacaoIntermediaria
{
    // Seu código aqui
}",
                    Hints = new List<string> { "Considere casos de erro", "Implemente validações", "Use async/await quando apropriado" }
                },
                new Exercise
                {
                    Title = "Exercício Avançado - Dependency Injection",
                    Description = "Desenvolva uma solução completa que integre múltiplos conceitos.",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = @"// Implemente uma solução avançada
public class ImplementacaoAvancada
{
    // Seu código completo aqui
}",
                    Hints = new List<string> { "Integre com outros componentes", "Implemente tratamento de erros robusto", "Considere performance e escalabilidade" }
                }
            },
            Summary = "Nesta aula você aprendeu sobre dependency injection, incluindo conceitos fundamentais, implementação prática e melhores práticas. Continue praticando para dominar completamente este tópico essencial do ASP.NET Core."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0006-000000000004"),
            CourseId = _courseId,
            Title = "Dependency Injection",
            Duration = "45 min",
            Difficulty = "Intermediário",
            EstimatedMinutes = 45,
            Prerequisites = JsonSerializer.Serialize(new[] { "10000000-0000-0000-0006-000000000003" }),
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
                "Compreender os conceitos de configuration",
                "Aprender a implementar implementação",
                "Dominar aplicação",
                "Aplicar configuration em projetos reais"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "Configuration - Conceitos Fundamentais",
                    Content = "O conceito de configuration é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam configuration regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com configuration, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o como, mas também o porquê por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Implementação Prática",
                    Content = "A implementação de implementação no ASP.NET Core segue padrões bem estabelecidos que facilitam o desenvolvimento e manutenção do código. Ao trabalhar com este conceito, você utilizará recursos nativos do framework que foram projetados especificamente para este propósito. A Microsoft investiu significativamente em tornar esta funcionalidade robusta e fácil de usar. Desenvolvedores ao redor do mundo confiam nesta abordagem para construir aplicações de missão crítica. É essencial entender não apenas a sintaxe, mas também os princípios subjacentes que guiam o design desta funcionalidade. Ao dominar estes conceitos, você estará preparado para enfrentar desafios complexos em projetos reais. A prática constante e a experimentação são fundamentais para consolidar este conhecimento.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Melhores Práticas",
                    Content = "Ao trabalhar com aplicação, é crucial seguir as melhores práticas estabelecidas pela comunidade .NET. Estas práticas foram desenvolvidas ao longo de anos de experiência coletiva e ajudam a evitar armadilhas comuns. Segurança, performance e manutenibilidade devem estar sempre em mente ao implementar esta funcionalidade. Testes automatizados são essenciais para garantir que seu código funcione conforme esperado. Documentação clara ajuda outros desenvolvedores a entender suas decisões de design. Code reviews e pair programming são excelentes formas de compartilhar conhecimento e melhorar a qualidade do código. Lembre-se que código bom é código que outros desenvolvedores conseguem entender e manter facilmente.",
                    Order = 3
                }
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Exemplo Básico de Configuration",
                    Code = @"using Microsoft.AspNetCore.Mvc;

// Implementação de Configuration
using Microsoft.AspNetCore.Mvc;

namespace MeuApp.Controllers
{
    public class ExemploController : Controller
    {
        public IActionResult Index()
        {
            // Implementação básica
            return View();
        }
    }
}",
                    Language = "csharp",
                    Explanation = "Este exemplo demonstra a implementação básica de configuration no ASP.NET Core.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Exemplo Avançado de Configuration",
                    Code = @"using Microsoft.AspNetCore.Mvc;

// Implementação avançada de Configuration
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MeuApp.Controllers
{
    [ApiController]
    [Route(""api/[controller]"")]
    public class AvancadoController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetData()
        {
            // Implementação avançada
            await Task.Delay(100);
            return Ok(new { message = ""Sucesso"" });
        }
    }
}",
                    Language = "csharp",
                    Explanation = "Este exemplo mostra uma implementação mais avançada utilizando async/await e API controllers.",
                    IsRunnable = true
                }
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Exercício Básico - Configuration",
                    Description = "Implemente uma solução básica utilizando os conceitos de configuration.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"// Implemente sua solução aqui
public class MinhaImplementacao
{
    // Seu código
}",
                    Hints = new List<string> { "Revise os exemplos de código", "Consulte a documentação oficial", "Teste sua implementação" }
                },
                new Exercise
                {
                    Title = "Exercício Intermediário - Configuration",
                    Description = "Crie uma implementação mais complexa que demonstre domínio dos conceitos.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"// Implemente uma solução intermediária
public class ImplementacaoIntermediaria
{
    // Seu código aqui
}",
                    Hints = new List<string> { "Considere casos de erro", "Implemente validações", "Use async/await quando apropriado" }
                },
                new Exercise
                {
                    Title = "Exercício Avançado - Configuration",
                    Description = "Desenvolva uma solução completa que integre múltiplos conceitos.",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = @"// Implemente uma solução avançada
public class ImplementacaoAvancada
{
    // Seu código completo aqui
}",
                    Hints = new List<string> { "Integre com outros componentes", "Implemente tratamento de erros robusto", "Considere performance e escalabilidade" }
                }
            },
            Summary = "Nesta aula você aprendeu sobre configuration, incluindo conceitos fundamentais, implementação prática e melhores práticas. Continue praticando para dominar completamente este tópico essencial do ASP.NET Core."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0006-000000000005"),
            CourseId = _courseId,
            Title = "Configuration",
            Duration = "45 min",
            Difficulty = "Intermediário",
            EstimatedMinutes = 45,
            Prerequisites = JsonSerializer.Serialize(new[] { "10000000-0000-0000-0006-000000000004" }),
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
                "Compreender os conceitos de logging",
                "Aprender a implementar implementação",
                "Dominar aplicação",
                "Aplicar logging em projetos reais"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "Logging - Conceitos Fundamentais",
                    Content = "O conceito de logging é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam logging regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com logging, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o como, mas também o porquê por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Implementação Prática",
                    Content = "A implementação de implementação no ASP.NET Core segue padrões bem estabelecidos que facilitam o desenvolvimento e manutenção do código. Ao trabalhar com este conceito, você utilizará recursos nativos do framework que foram projetados especificamente para este propósito. A Microsoft investiu significativamente em tornar esta funcionalidade robusta e fácil de usar. Desenvolvedores ao redor do mundo confiam nesta abordagem para construir aplicações de missão crítica. É essencial entender não apenas a sintaxe, mas também os princípios subjacentes que guiam o design desta funcionalidade. Ao dominar estes conceitos, você estará preparado para enfrentar desafios complexos em projetos reais. A prática constante e a experimentação são fundamentais para consolidar este conhecimento.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Melhores Práticas",
                    Content = "Ao trabalhar com aplicação, é crucial seguir as melhores práticas estabelecidas pela comunidade .NET. Estas práticas foram desenvolvidas ao longo de anos de experiência coletiva e ajudam a evitar armadilhas comuns. Segurança, performance e manutenibilidade devem estar sempre em mente ao implementar esta funcionalidade. Testes automatizados são essenciais para garantir que seu código funcione conforme esperado. Documentação clara ajuda outros desenvolvedores a entender suas decisões de design. Code reviews e pair programming são excelentes formas de compartilhar conhecimento e melhorar a qualidade do código. Lembre-se que código bom é código que outros desenvolvedores conseguem entender e manter facilmente.",
                    Order = 3
                }
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Exemplo Básico de Logging",
                    Code = @"using Microsoft.AspNetCore.Mvc;

// Implementação de Logging
using Microsoft.AspNetCore.Mvc;

namespace MeuApp.Controllers
{
    public class ExemploController : Controller
    {
        public IActionResult Index()
        {
            // Implementação básica
            return View();
        }
    }
}",
                    Language = "csharp",
                    Explanation = "Este exemplo demonstra a implementação básica de logging no ASP.NET Core.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Exemplo Avançado de Logging",
                    Code = @"using Microsoft.AspNetCore.Mvc;

// Implementação avançada de Logging
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MeuApp.Controllers
{
    [ApiController]
    [Route(""api/[controller]"")]
    public class AvancadoController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetData()
        {
            // Implementação avançada
            await Task.Delay(100);
            return Ok(new { message = ""Sucesso"" });
        }
    }
}",
                    Language = "csharp",
                    Explanation = "Este exemplo mostra uma implementação mais avançada utilizando async/await e API controllers.",
                    IsRunnable = true
                }
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Exercício Básico - Logging",
                    Description = "Implemente uma solução básica utilizando os conceitos de logging.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"// Implemente sua solução aqui
public class MinhaImplementacao
{
    // Seu código
}",
                    Hints = new List<string> { "Revise os exemplos de código", "Consulte a documentação oficial", "Teste sua implementação" }
                },
                new Exercise
                {
                    Title = "Exercício Intermediário - Logging",
                    Description = "Crie uma implementação mais complexa que demonstre domínio dos conceitos.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"// Implemente uma solução intermediária
public class ImplementacaoIntermediaria
{
    // Seu código aqui
}",
                    Hints = new List<string> { "Considere casos de erro", "Implemente validações", "Use async/await quando apropriado" }
                },
                new Exercise
                {
                    Title = "Exercício Avançado - Logging",
                    Description = "Desenvolva uma solução completa que integre múltiplos conceitos.",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = @"// Implemente uma solução avançada
public class ImplementacaoAvancada
{
    // Seu código completo aqui
}",
                    Hints = new List<string> { "Integre com outros componentes", "Implemente tratamento de erros robusto", "Considere performance e escalabilidade" }
                }
            },
            Summary = "Nesta aula você aprendeu sobre logging, incluindo conceitos fundamentais, implementação prática e melhores práticas. Continue praticando para dominar completamente este tópico essencial do ASP.NET Core."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0006-000000000006"),
            CourseId = _courseId,
            Title = "Logging",
            Duration = "45 min",
            Difficulty = "Intermediário",
            EstimatedMinutes = 45,
            Prerequisites = JsonSerializer.Serialize(new[] { "10000000-0000-0000-0006-000000000005" }),
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
                "Compreender os conceitos de mvc",
                "Aprender a implementar pattern",
                "Dominar aplicação",
                "Aplicar mvc pattern em projetos reais"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "MVC Pattern - Conceitos Fundamentais",
                    Content = "O conceito de mvc pattern é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam mvc regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com mvc, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o como, mas também o porquê por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Implementação Prática",
                    Content = "A implementação de pattern no ASP.NET Core segue padrões bem estabelecidos que facilitam o desenvolvimento e manutenção do código. Ao trabalhar com este conceito, você utilizará recursos nativos do framework que foram projetados especificamente para este propósito. A Microsoft investiu significativamente em tornar esta funcionalidade robusta e fácil de usar. Desenvolvedores ao redor do mundo confiam nesta abordagem para construir aplicações de missão crítica. É essencial entender não apenas a sintaxe, mas também os princípios subjacentes que guiam o design desta funcionalidade. Ao dominar estes conceitos, você estará preparado para enfrentar desafios complexos em projetos reais. A prática constante e a experimentação são fundamentais para consolidar este conhecimento.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Melhores Práticas",
                    Content = "Ao trabalhar com aplicação, é crucial seguir as melhores práticas estabelecidas pela comunidade .NET. Estas práticas foram desenvolvidas ao longo de anos de experiência coletiva e ajudam a evitar armadilhas comuns. Segurança, performance e manutenibilidade devem estar sempre em mente ao implementar esta funcionalidade. Testes automatizados são essenciais para garantir que seu código funcione conforme esperado. Documentação clara ajuda outros desenvolvedores a entender suas decisões de design. Code reviews e pair programming são excelentes formas de compartilhar conhecimento e melhorar a qualidade do código. Lembre-se que código bom é código que outros desenvolvedores conseguem entender e manter facilmente.",
                    Order = 3
                }
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Exemplo Básico de MVC Pattern",
                    Code = @"using Microsoft.AspNetCore.Mvc;

// Implementação de MVC Pattern
using Microsoft.AspNetCore.Mvc;

namespace MeuApp.Controllers
{
    public class ExemploController : Controller
    {
        public IActionResult Index()
        {
            // Implementação básica
            return View();
        }
    }
}",
                    Language = "csharp",
                    Explanation = "Este exemplo demonstra a implementação básica de mvc pattern no ASP.NET Core.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Exemplo Avançado de MVC Pattern",
                    Code = @"using Microsoft.AspNetCore.Mvc;

// Implementação avançada de MVC Pattern
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MeuApp.Controllers
{
    [ApiController]
    [Route(""api/[controller]"")]
    public class AvancadoController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetData()
        {
            // Implementação avançada
            await Task.Delay(100);
            return Ok(new { message = ""Sucesso"" });
        }
    }
}",
                    Language = "csharp",
                    Explanation = "Este exemplo mostra uma implementação mais avançada utilizando async/await e API controllers.",
                    IsRunnable = true
                }
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Exercício Básico - MVC Pattern",
                    Description = "Implemente uma solução básica utilizando os conceitos de mvc pattern.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"// Implemente sua solução aqui
public class MinhaImplementacao
{
    // Seu código
}",
                    Hints = new List<string> { "Revise os exemplos de código", "Consulte a documentação oficial", "Teste sua implementação" }
                },
                new Exercise
                {
                    Title = "Exercício Intermediário - MVC Pattern",
                    Description = "Crie uma implementação mais complexa que demonstre domínio dos conceitos.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"// Implemente uma solução intermediária
public class ImplementacaoIntermediaria
{
    // Seu código aqui
}",
                    Hints = new List<string> { "Considere casos de erro", "Implemente validações", "Use async/await quando apropriado" }
                },
                new Exercise
                {
                    Title = "Exercício Avançado - MVC Pattern",
                    Description = "Desenvolva uma solução completa que integre múltiplos conceitos.",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = @"// Implemente uma solução avançada
public class ImplementacaoAvancada
{
    // Seu código completo aqui
}",
                    Hints = new List<string> { "Integre com outros componentes", "Implemente tratamento de erros robusto", "Considere performance e escalabilidade" }
                }
            },
            Summary = "Nesta aula você aprendeu sobre mvc pattern, incluindo conceitos fundamentais, implementação prática e melhores práticas. Continue praticando para dominar completamente este tópico essencial do ASP.NET Core."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0006-000000000007"),
            CourseId = _courseId,
            Title = "MVC Pattern",
            Duration = "45 min",
            Difficulty = "Intermediário",
            EstimatedMinutes = 45,
            Prerequisites = JsonSerializer.Serialize(new[] { "10000000-0000-0000-0006-000000000006" }),
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
                "Compreender os conceitos de routing",
                "Aprender a implementar implementação",
                "Dominar aplicação",
                "Aplicar routing em projetos reais"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "Routing - Conceitos Fundamentais",
                    Content = "O conceito de routing é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam routing regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com routing, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o como, mas também o porquê por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Implementação Prática",
                    Content = "A implementação de implementação no ASP.NET Core segue padrões bem estabelecidos que facilitam o desenvolvimento e manutenção do código. Ao trabalhar com este conceito, você utilizará recursos nativos do framework que foram projetados especificamente para este propósito. A Microsoft investiu significativamente em tornar esta funcionalidade robusta e fácil de usar. Desenvolvedores ao redor do mundo confiam nesta abordagem para construir aplicações de missão crítica. É essencial entender não apenas a sintaxe, mas também os princípios subjacentes que guiam o design desta funcionalidade. Ao dominar estes conceitos, você estará preparado para enfrentar desafios complexos em projetos reais. A prática constante e a experimentação são fundamentais para consolidar este conhecimento.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Melhores Práticas",
                    Content = "Ao trabalhar com aplicação, é crucial seguir as melhores práticas estabelecidas pela comunidade .NET. Estas práticas foram desenvolvidas ao longo de anos de experiência coletiva e ajudam a evitar armadilhas comuns. Segurança, performance e manutenibilidade devem estar sempre em mente ao implementar esta funcionalidade. Testes automatizados são essenciais para garantir que seu código funcione conforme esperado. Documentação clara ajuda outros desenvolvedores a entender suas decisões de design. Code reviews e pair programming são excelentes formas de compartilhar conhecimento e melhorar a qualidade do código. Lembre-se que código bom é código que outros desenvolvedores conseguem entender e manter facilmente.",
                    Order = 3
                }
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Exemplo Básico de Routing",
                    Code = @"using Microsoft.AspNetCore.Mvc;

// Implementação de Routing
using Microsoft.AspNetCore.Mvc;

namespace MeuApp.Controllers
{
    public class ExemploController : Controller
    {
        public IActionResult Index()
        {
            // Implementação básica
            return View();
        }
    }
}",
                    Language = "csharp",
                    Explanation = "Este exemplo demonstra a implementação básica de routing no ASP.NET Core.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Exemplo Avançado de Routing",
                    Code = @"using Microsoft.AspNetCore.Mvc;

// Implementação avançada de Routing
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MeuApp.Controllers
{
    [ApiController]
    [Route(""api/[controller]"")]
    public class AvancadoController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetData()
        {
            // Implementação avançada
            await Task.Delay(100);
            return Ok(new { message = ""Sucesso"" });
        }
    }
}",
                    Language = "csharp",
                    Explanation = "Este exemplo mostra uma implementação mais avançada utilizando async/await e API controllers.",
                    IsRunnable = true
                }
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Exercício Básico - Routing",
                    Description = "Implemente uma solução básica utilizando os conceitos de routing.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"// Implemente sua solução aqui
public class MinhaImplementacao
{
    // Seu código
}",
                    Hints = new List<string> { "Revise os exemplos de código", "Consulte a documentação oficial", "Teste sua implementação" }
                },
                new Exercise
                {
                    Title = "Exercício Intermediário - Routing",
                    Description = "Crie uma implementação mais complexa que demonstre domínio dos conceitos.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"// Implemente uma solução intermediária
public class ImplementacaoIntermediaria
{
    // Seu código aqui
}",
                    Hints = new List<string> { "Considere casos de erro", "Implemente validações", "Use async/await quando apropriado" }
                },
                new Exercise
                {
                    Title = "Exercício Avançado - Routing",
                    Description = "Desenvolva uma solução completa que integre múltiplos conceitos.",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = @"// Implemente uma solução avançada
public class ImplementacaoAvancada
{
    // Seu código completo aqui
}",
                    Hints = new List<string> { "Integre com outros componentes", "Implemente tratamento de erros robusto", "Considere performance e escalabilidade" }
                }
            },
            Summary = "Nesta aula você aprendeu sobre routing, incluindo conceitos fundamentais, implementação prática e melhores práticas. Continue praticando para dominar completamente este tópico essencial do ASP.NET Core."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0006-000000000008"),
            CourseId = _courseId,
            Title = "Routing",
            Duration = "45 min",
            Difficulty = "Intermediário",
            EstimatedMinutes = 45,
            Prerequisites = JsonSerializer.Serialize(new[] { "10000000-0000-0000-0006-000000000007" }),
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
                "Compreender os conceitos de controllers",
                "Aprender a implementar e",
                "Dominar actions",
                "Aplicar controllers e actions em projetos reais"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "Controllers e Actions - Conceitos Fundamentais",
                    Content = "O conceito de controllers e actions é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam controllers regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com controllers, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o como, mas também o porquê por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Implementação Prática",
                    Content = "A implementação de e no ASP.NET Core segue padrões bem estabelecidos que facilitam o desenvolvimento e manutenção do código. Ao trabalhar com este conceito, você utilizará recursos nativos do framework que foram projetados especificamente para este propósito. A Microsoft investiu significativamente em tornar esta funcionalidade robusta e fácil de usar. Desenvolvedores ao redor do mundo confiam nesta abordagem para construir aplicações de missão crítica. É essencial entender não apenas a sintaxe, mas também os princípios subjacentes que guiam o design desta funcionalidade. Ao dominar estes conceitos, você estará preparado para enfrentar desafios complexos em projetos reais. A prática constante e a experimentação são fundamentais para consolidar este conhecimento.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Melhores Práticas",
                    Content = "Ao trabalhar com actions, é crucial seguir as melhores práticas estabelecidas pela comunidade .NET. Estas práticas foram desenvolvidas ao longo de anos de experiência coletiva e ajudam a evitar armadilhas comuns. Segurança, performance e manutenibilidade devem estar sempre em mente ao implementar esta funcionalidade. Testes automatizados são essenciais para garantir que seu código funcione conforme esperado. Documentação clara ajuda outros desenvolvedores a entender suas decisões de design. Code reviews e pair programming são excelentes formas de compartilhar conhecimento e melhorar a qualidade do código. Lembre-se que código bom é código que outros desenvolvedores conseguem entender e manter facilmente.",
                    Order = 3
                }
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Exemplo Básico de Controllers e Actions",
                    Code = @"using Microsoft.AspNetCore.Mvc;

// Implementação de Controllers e Actions
using Microsoft.AspNetCore.Mvc;

namespace MeuApp.Controllers
{
    public class ExemploController : Controller
    {
        public IActionResult Index()
        {
            // Implementação básica
            return View();
        }
    }
}",
                    Language = "csharp",
                    Explanation = "Este exemplo demonstra a implementação básica de controllers e actions no ASP.NET Core.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Exemplo Avançado de Controllers e Actions",
                    Code = @"using Microsoft.AspNetCore.Mvc;

// Implementação avançada de Controllers e Actions
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MeuApp.Controllers
{
    [ApiController]
    [Route(""api/[controller]"")]
    public class AvancadoController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetData()
        {
            // Implementação avançada
            await Task.Delay(100);
            return Ok(new { message = ""Sucesso"" });
        }
    }
}",
                    Language = "csharp",
                    Explanation = "Este exemplo mostra uma implementação mais avançada utilizando async/await e API controllers.",
                    IsRunnable = true
                }
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Exercício Básico - Controllers e Actions",
                    Description = "Implemente uma solução básica utilizando os conceitos de controllers e actions.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"// Implemente sua solução aqui
public class MinhaImplementacao
{
    // Seu código
}",
                    Hints = new List<string> { "Revise os exemplos de código", "Consulte a documentação oficial", "Teste sua implementação" }
                },
                new Exercise
                {
                    Title = "Exercício Intermediário - Controllers e Actions",
                    Description = "Crie uma implementação mais complexa que demonstre domínio dos conceitos.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"// Implemente uma solução intermediária
public class ImplementacaoIntermediaria
{
    // Seu código aqui
}",
                    Hints = new List<string> { "Considere casos de erro", "Implemente validações", "Use async/await quando apropriado" }
                },
                new Exercise
                {
                    Title = "Exercício Avançado - Controllers e Actions",
                    Description = "Desenvolva uma solução completa que integre múltiplos conceitos.",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = @"// Implemente uma solução avançada
public class ImplementacaoAvancada
{
    // Seu código completo aqui
}",
                    Hints = new List<string> { "Integre com outros componentes", "Implemente tratamento de erros robusto", "Considere performance e escalabilidade" }
                }
            },
            Summary = "Nesta aula você aprendeu sobre controllers e actions, incluindo conceitos fundamentais, implementação prática e melhores práticas. Continue praticando para dominar completamente este tópico essencial do ASP.NET Core."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0006-000000000009"),
            CourseId = _courseId,
            Title = "Controllers e Actions",
            Duration = "45 min",
            Difficulty = "Intermediário",
            EstimatedMinutes = 45,
            Prerequisites = JsonSerializer.Serialize(new[] { "10000000-0000-0000-0006-000000000008" }),
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
                "Compreender os conceitos de model",
                "Aprender a implementar binding",
                "Dominar aplicação",
                "Aplicar model binding em projetos reais"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "Model Binding - Conceitos Fundamentais",
                    Content = "O conceito de model binding é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam model regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com model, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o como, mas também o porquê por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Implementação Prática",
                    Content = "A implementação de binding no ASP.NET Core segue padrões bem estabelecidos que facilitam o desenvolvimento e manutenção do código. Ao trabalhar com este conceito, você utilizará recursos nativos do framework que foram projetados especificamente para este propósito. A Microsoft investiu significativamente em tornar esta funcionalidade robusta e fácil de usar. Desenvolvedores ao redor do mundo confiam nesta abordagem para construir aplicações de missão crítica. É essencial entender não apenas a sintaxe, mas também os princípios subjacentes que guiam o design desta funcionalidade. Ao dominar estes conceitos, você estará preparado para enfrentar desafios complexos em projetos reais. A prática constante e a experimentação são fundamentais para consolidar este conhecimento.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Melhores Práticas",
                    Content = "Ao trabalhar com aplicação, é crucial seguir as melhores práticas estabelecidas pela comunidade .NET. Estas práticas foram desenvolvidas ao longo de anos de experiência coletiva e ajudam a evitar armadilhas comuns. Segurança, performance e manutenibilidade devem estar sempre em mente ao implementar esta funcionalidade. Testes automatizados são essenciais para garantir que seu código funcione conforme esperado. Documentação clara ajuda outros desenvolvedores a entender suas decisões de design. Code reviews e pair programming são excelentes formas de compartilhar conhecimento e melhorar a qualidade do código. Lembre-se que código bom é código que outros desenvolvedores conseguem entender e manter facilmente.",
                    Order = 3
                }
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Exemplo Básico de Model Binding",
                    Code = @"using Microsoft.AspNetCore.Mvc;

// Implementação de Model Binding
using Microsoft.AspNetCore.Mvc;

namespace MeuApp.Controllers
{
    public class ExemploController : Controller
    {
        public IActionResult Index()
        {
            // Implementação básica
            return View();
        }
    }
}",
                    Language = "csharp",
                    Explanation = "Este exemplo demonstra a implementação básica de model binding no ASP.NET Core.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Exemplo Avançado de Model Binding",
                    Code = @"using Microsoft.AspNetCore.Mvc;

// Implementação avançada de Model Binding
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MeuApp.Controllers
{
    [ApiController]
    [Route(""api/[controller]"")]
    public class AvancadoController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetData()
        {
            // Implementação avançada
            await Task.Delay(100);
            return Ok(new { message = ""Sucesso"" });
        }
    }
}",
                    Language = "csharp",
                    Explanation = "Este exemplo mostra uma implementação mais avançada utilizando async/await e API controllers.",
                    IsRunnable = true
                }
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Exercício Básico - Model Binding",
                    Description = "Implemente uma solução básica utilizando os conceitos de model binding.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"// Implemente sua solução aqui
public class MinhaImplementacao
{
    // Seu código
}",
                    Hints = new List<string> { "Revise os exemplos de código", "Consulte a documentação oficial", "Teste sua implementação" }
                },
                new Exercise
                {
                    Title = "Exercício Intermediário - Model Binding",
                    Description = "Crie uma implementação mais complexa que demonstre domínio dos conceitos.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"// Implemente uma solução intermediária
public class ImplementacaoIntermediaria
{
    // Seu código aqui
}",
                    Hints = new List<string> { "Considere casos de erro", "Implemente validações", "Use async/await quando apropriado" }
                },
                new Exercise
                {
                    Title = "Exercício Avançado - Model Binding",
                    Description = "Desenvolva uma solução completa que integre múltiplos conceitos.",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = @"// Implemente uma solução avançada
public class ImplementacaoAvancada
{
    // Seu código completo aqui
}",
                    Hints = new List<string> { "Integre com outros componentes", "Implemente tratamento de erros robusto", "Considere performance e escalabilidade" }
                }
            },
            Summary = "Nesta aula você aprendeu sobre model binding, incluindo conceitos fundamentais, implementação prática e melhores práticas. Continue praticando para dominar completamente este tópico essencial do ASP.NET Core."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0006-000000000010"),
            CourseId = _courseId,
            Title = "Model Binding",
            Duration = "45 min",
            Difficulty = "Intermediário",
            EstimatedMinutes = 45,
            Prerequisites = JsonSerializer.Serialize(new[] { "10000000-0000-0000-0006-000000000009" }),
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
                "Compreender os conceitos de views",
                "Aprender a implementar e",
                "Dominar razor",
                "Aplicar views e razor em projetos reais"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "Views e Razor - Conceitos Fundamentais",
                    Content = "O conceito de views e razor é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam views regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com views, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o como, mas também o porquê por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Implementação Prática",
                    Content = "A implementação de e no ASP.NET Core segue padrões bem estabelecidos que facilitam o desenvolvimento e manutenção do código. Ao trabalhar com este conceito, você utilizará recursos nativos do framework que foram projetados especificamente para este propósito. A Microsoft investiu significativamente em tornar esta funcionalidade robusta e fácil de usar. Desenvolvedores ao redor do mundo confiam nesta abordagem para construir aplicações de missão crítica. É essencial entender não apenas a sintaxe, mas também os princípios subjacentes que guiam o design desta funcionalidade. Ao dominar estes conceitos, você estará preparado para enfrentar desafios complexos em projetos reais. A prática constante e a experimentação são fundamentais para consolidar este conhecimento.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Melhores Práticas",
                    Content = "Ao trabalhar com razor, é crucial seguir as melhores práticas estabelecidas pela comunidade .NET. Estas práticas foram desenvolvidas ao longo de anos de experiência coletiva e ajudam a evitar armadilhas comuns. Segurança, performance e manutenibilidade devem estar sempre em mente ao implementar esta funcionalidade. Testes automatizados são essenciais para garantir que seu código funcione conforme esperado. Documentação clara ajuda outros desenvolvedores a entender suas decisões de design. Code reviews e pair programming são excelentes formas de compartilhar conhecimento e melhorar a qualidade do código. Lembre-se que código bom é código que outros desenvolvedores conseguem entender e manter facilmente.",
                    Order = 3
                }
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Exemplo Básico de Views e Razor",
                    Code = @"using Microsoft.AspNetCore.Mvc;

// Implementação de Views e Razor
using Microsoft.AspNetCore.Mvc;

namespace MeuApp.Controllers
{
    public class ExemploController : Controller
    {
        public IActionResult Index()
        {
            // Implementação básica
            return View();
        }
    }
}",
                    Language = "csharp",
                    Explanation = "Este exemplo demonstra a implementação básica de views e razor no ASP.NET Core.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Exemplo Avançado de Views e Razor",
                    Code = @"using Microsoft.AspNetCore.Mvc;

// Implementação avançada de Views e Razor
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MeuApp.Controllers
{
    [ApiController]
    [Route(""api/[controller]"")]
    public class AvancadoController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetData()
        {
            // Implementação avançada
            await Task.Delay(100);
            return Ok(new { message = ""Sucesso"" });
        }
    }
}",
                    Language = "csharp",
                    Explanation = "Este exemplo mostra uma implementação mais avançada utilizando async/await e API controllers.",
                    IsRunnable = true
                }
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Exercício Básico - Views e Razor",
                    Description = "Implemente uma solução básica utilizando os conceitos de views e razor.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"// Implemente sua solução aqui
public class MinhaImplementacao
{
    // Seu código
}",
                    Hints = new List<string> { "Revise os exemplos de código", "Consulte a documentação oficial", "Teste sua implementação" }
                },
                new Exercise
                {
                    Title = "Exercício Intermediário - Views e Razor",
                    Description = "Crie uma implementação mais complexa que demonstre domínio dos conceitos.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"// Implemente uma solução intermediária
public class ImplementacaoIntermediaria
{
    // Seu código aqui
}",
                    Hints = new List<string> { "Considere casos de erro", "Implemente validações", "Use async/await quando apropriado" }
                },
                new Exercise
                {
                    Title = "Exercício Avançado - Views e Razor",
                    Description = "Desenvolva uma solução completa que integre múltiplos conceitos.",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = @"// Implemente uma solução avançada
public class ImplementacaoAvancada
{
    // Seu código completo aqui
}",
                    Hints = new List<string> { "Integre com outros componentes", "Implemente tratamento de erros robusto", "Considere performance e escalabilidade" }
                }
            },
            Summary = "Nesta aula você aprendeu sobre views e razor, incluindo conceitos fundamentais, implementação prática e melhores práticas. Continue praticando para dominar completamente este tópico essencial do ASP.NET Core."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0006-000000000011"),
            CourseId = _courseId,
            Title = "Views e Razor",
            Duration = "45 min",
            Difficulty = "Intermediário",
            EstimatedMinutes = 45,
            Prerequisites = JsonSerializer.Serialize(new[] { "10000000-0000-0000-0006-000000000010" }),
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
                "Compreender os conceitos de tag",
                "Aprender a implementar helpers",
                "Dominar aplicação",
                "Aplicar tag helpers em projetos reais"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "Tag Helpers - Conceitos Fundamentais",
                    Content = "O conceito de tag helpers é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam tag regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com tag, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o como, mas também o porquê por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Implementação Prática",
                    Content = "A implementação de helpers no ASP.NET Core segue padrões bem estabelecidos que facilitam o desenvolvimento e manutenção do código. Ao trabalhar com este conceito, você utilizará recursos nativos do framework que foram projetados especificamente para este propósito. A Microsoft investiu significativamente em tornar esta funcionalidade robusta e fácil de usar. Desenvolvedores ao redor do mundo confiam nesta abordagem para construir aplicações de missão crítica. É essencial entender não apenas a sintaxe, mas também os princípios subjacentes que guiam o design desta funcionalidade. Ao dominar estes conceitos, você estará preparado para enfrentar desafios complexos em projetos reais. A prática constante e a experimentação são fundamentais para consolidar este conhecimento.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Melhores Práticas",
                    Content = "Ao trabalhar com aplicação, é crucial seguir as melhores práticas estabelecidas pela comunidade .NET. Estas práticas foram desenvolvidas ao longo de anos de experiência coletiva e ajudam a evitar armadilhas comuns. Segurança, performance e manutenibilidade devem estar sempre em mente ao implementar esta funcionalidade. Testes automatizados são essenciais para garantir que seu código funcione conforme esperado. Documentação clara ajuda outros desenvolvedores a entender suas decisões de design. Code reviews e pair programming são excelentes formas de compartilhar conhecimento e melhorar a qualidade do código. Lembre-se que código bom é código que outros desenvolvedores conseguem entender e manter facilmente.",
                    Order = 3
                }
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Exemplo Básico de Tag Helpers",
                    Code = @"using Microsoft.AspNetCore.Mvc;

// Implementação de Tag Helpers
using Microsoft.AspNetCore.Mvc;

namespace MeuApp.Controllers
{
    public class ExemploController : Controller
    {
        public IActionResult Index()
        {
            // Implementação básica
            return View();
        }
    }
}",
                    Language = "csharp",
                    Explanation = "Este exemplo demonstra a implementação básica de tag helpers no ASP.NET Core.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Exemplo Avançado de Tag Helpers",
                    Code = @"using Microsoft.AspNetCore.Mvc;

// Implementação avançada de Tag Helpers
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MeuApp.Controllers
{
    [ApiController]
    [Route(""api/[controller]"")]
    public class AvancadoController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetData()
        {
            // Implementação avançada
            await Task.Delay(100);
            return Ok(new { message = ""Sucesso"" });
        }
    }
}",
                    Language = "csharp",
                    Explanation = "Este exemplo mostra uma implementação mais avançada utilizando async/await e API controllers.",
                    IsRunnable = true
                }
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Exercício Básico - Tag Helpers",
                    Description = "Implemente uma solução básica utilizando os conceitos de tag helpers.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"// Implemente sua solução aqui
public class MinhaImplementacao
{
    // Seu código
}",
                    Hints = new List<string> { "Revise os exemplos de código", "Consulte a documentação oficial", "Teste sua implementação" }
                },
                new Exercise
                {
                    Title = "Exercício Intermediário - Tag Helpers",
                    Description = "Crie uma implementação mais complexa que demonstre domínio dos conceitos.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"// Implemente uma solução intermediária
public class ImplementacaoIntermediaria
{
    // Seu código aqui
}",
                    Hints = new List<string> { "Considere casos de erro", "Implemente validações", "Use async/await quando apropriado" }
                },
                new Exercise
                {
                    Title = "Exercício Avançado - Tag Helpers",
                    Description = "Desenvolva uma solução completa que integre múltiplos conceitos.",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = @"// Implemente uma solução avançada
public class ImplementacaoAvancada
{
    // Seu código completo aqui
}",
                    Hints = new List<string> { "Integre com outros componentes", "Implemente tratamento de erros robusto", "Considere performance e escalabilidade" }
                }
            },
            Summary = "Nesta aula você aprendeu sobre tag helpers, incluindo conceitos fundamentais, implementação prática e melhores práticas. Continue praticando para dominar completamente este tópico essencial do ASP.NET Core."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0006-000000000012"),
            CourseId = _courseId,
            Title = "Tag Helpers",
            Duration = "45 min",
            Difficulty = "Intermediário",
            EstimatedMinutes = 45,
            Prerequisites = JsonSerializer.Serialize(new[] { "10000000-0000-0000-0006-000000000011" }),
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
                "Compreender os conceitos de view",
                "Aprender a implementar components",
                "Dominar aplicação",
                "Aplicar view components em projetos reais"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "View Components - Conceitos Fundamentais",
                    Content = "O conceito de view components é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam view regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com view, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o como, mas também o porquê por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Implementação Prática",
                    Content = "A implementação de components no ASP.NET Core segue padrões bem estabelecidos que facilitam o desenvolvimento e manutenção do código. Ao trabalhar com este conceito, você utilizará recursos nativos do framework que foram projetados especificamente para este propósito. A Microsoft investiu significativamente em tornar esta funcionalidade robusta e fácil de usar. Desenvolvedores ao redor do mundo confiam nesta abordagem para construir aplicações de missão crítica. É essencial entender não apenas a sintaxe, mas também os princípios subjacentes que guiam o design desta funcionalidade. Ao dominar estes conceitos, você estará preparado para enfrentar desafios complexos em projetos reais. A prática constante e a experimentação são fundamentais para consolidar este conhecimento.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Melhores Práticas",
                    Content = "Ao trabalhar com aplicação, é crucial seguir as melhores práticas estabelecidas pela comunidade .NET. Estas práticas foram desenvolvidas ao longo de anos de experiência coletiva e ajudam a evitar armadilhas comuns. Segurança, performance e manutenibilidade devem estar sempre em mente ao implementar esta funcionalidade. Testes automatizados são essenciais para garantir que seu código funcione conforme esperado. Documentação clara ajuda outros desenvolvedores a entender suas decisões de design. Code reviews e pair programming são excelentes formas de compartilhar conhecimento e melhorar a qualidade do código. Lembre-se que código bom é código que outros desenvolvedores conseguem entender e manter facilmente.",
                    Order = 3
                }
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Exemplo Básico de View Components",
                    Code = @"using Microsoft.AspNetCore.Mvc;

// Implementação de View Components
using Microsoft.AspNetCore.Mvc;

namespace MeuApp.Controllers
{
    public class ExemploController : Controller
    {
        public IActionResult Index()
        {
            // Implementação básica
            return View();
        }
    }
}",
                    Language = "csharp",
                    Explanation = "Este exemplo demonstra a implementação básica de view components no ASP.NET Core.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Exemplo Avançado de View Components",
                    Code = @"using Microsoft.AspNetCore.Mvc;

// Implementação avançada de View Components
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MeuApp.Controllers
{
    [ApiController]
    [Route(""api/[controller]"")]
    public class AvancadoController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetData()
        {
            // Implementação avançada
            await Task.Delay(100);
            return Ok(new { message = ""Sucesso"" });
        }
    }
}",
                    Language = "csharp",
                    Explanation = "Este exemplo mostra uma implementação mais avançada utilizando async/await e API controllers.",
                    IsRunnable = true
                }
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Exercício Básico - View Components",
                    Description = "Implemente uma solução básica utilizando os conceitos de view components.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"// Implemente sua solução aqui
public class MinhaImplementacao
{
    // Seu código
}",
                    Hints = new List<string> { "Revise os exemplos de código", "Consulte a documentação oficial", "Teste sua implementação" }
                },
                new Exercise
                {
                    Title = "Exercício Intermediário - View Components",
                    Description = "Crie uma implementação mais complexa que demonstre domínio dos conceitos.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"// Implemente uma solução intermediária
public class ImplementacaoIntermediaria
{
    // Seu código aqui
}",
                    Hints = new List<string> { "Considere casos de erro", "Implemente validações", "Use async/await quando apropriado" }
                },
                new Exercise
                {
                    Title = "Exercício Avançado - View Components",
                    Description = "Desenvolva uma solução completa que integre múltiplos conceitos.",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = @"// Implemente uma solução avançada
public class ImplementacaoAvancada
{
    // Seu código completo aqui
}",
                    Hints = new List<string> { "Integre com outros componentes", "Implemente tratamento de erros robusto", "Considere performance e escalabilidade" }
                }
            },
            Summary = "Nesta aula você aprendeu sobre view components, incluindo conceitos fundamentais, implementação prática e melhores práticas. Continue praticando para dominar completamente este tópico essencial do ASP.NET Core."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0006-000000000013"),
            CourseId = _courseId,
            Title = "View Components",
            Duration = "45 min",
            Difficulty = "Intermediário",
            EstimatedMinutes = 45,
            Prerequisites = JsonSerializer.Serialize(new[] { "10000000-0000-0000-0006-000000000012" }),
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
                "Compreender os conceitos de filters",
                "Aprender a implementar implementação",
                "Dominar aplicação",
                "Aplicar filters em projetos reais"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "Filters - Conceitos Fundamentais",
                    Content = "O conceito de filters é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam filters regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com filters, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o como, mas também o porquê por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Implementação Prática",
                    Content = "A implementação de implementação no ASP.NET Core segue padrões bem estabelecidos que facilitam o desenvolvimento e manutenção do código. Ao trabalhar com este conceito, você utilizará recursos nativos do framework que foram projetados especificamente para este propósito. A Microsoft investiu significativamente em tornar esta funcionalidade robusta e fácil de usar. Desenvolvedores ao redor do mundo confiam nesta abordagem para construir aplicações de missão crítica. É essencial entender não apenas a sintaxe, mas também os princípios subjacentes que guiam o design desta funcionalidade. Ao dominar estes conceitos, você estará preparado para enfrentar desafios complexos em projetos reais. A prática constante e a experimentação são fundamentais para consolidar este conhecimento.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Melhores Práticas",
                    Content = "Ao trabalhar com aplicação, é crucial seguir as melhores práticas estabelecidas pela comunidade .NET. Estas práticas foram desenvolvidas ao longo de anos de experiência coletiva e ajudam a evitar armadilhas comuns. Segurança, performance e manutenibilidade devem estar sempre em mente ao implementar esta funcionalidade. Testes automatizados são essenciais para garantir que seu código funcione conforme esperado. Documentação clara ajuda outros desenvolvedores a entender suas decisões de design. Code reviews e pair programming são excelentes formas de compartilhar conhecimento e melhorar a qualidade do código. Lembre-se que código bom é código que outros desenvolvedores conseguem entender e manter facilmente.",
                    Order = 3
                }
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Exemplo Básico de Filters",
                    Code = @"using Microsoft.AspNetCore.Mvc;

// Implementação de Filters
using Microsoft.AspNetCore.Mvc;

namespace MeuApp.Controllers
{
    public class ExemploController : Controller
    {
        public IActionResult Index()
        {
            // Implementação básica
            return View();
        }
    }
}",
                    Language = "csharp",
                    Explanation = "Este exemplo demonstra a implementação básica de filters no ASP.NET Core.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Exemplo Avançado de Filters",
                    Code = @"using Microsoft.AspNetCore.Mvc;

// Implementação avançada de Filters
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MeuApp.Controllers
{
    [ApiController]
    [Route(""api/[controller]"")]
    public class AvancadoController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetData()
        {
            // Implementação avançada
            await Task.Delay(100);
            return Ok(new { message = ""Sucesso"" });
        }
    }
}",
                    Language = "csharp",
                    Explanation = "Este exemplo mostra uma implementação mais avançada utilizando async/await e API controllers.",
                    IsRunnable = true
                }
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Exercício Básico - Filters",
                    Description = "Implemente uma solução básica utilizando os conceitos de filters.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"// Implemente sua solução aqui
public class MinhaImplementacao
{
    // Seu código
}",
                    Hints = new List<string> { "Revise os exemplos de código", "Consulte a documentação oficial", "Teste sua implementação" }
                },
                new Exercise
                {
                    Title = "Exercício Intermediário - Filters",
                    Description = "Crie uma implementação mais complexa que demonstre domínio dos conceitos.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"// Implemente uma solução intermediária
public class ImplementacaoIntermediaria
{
    // Seu código aqui
}",
                    Hints = new List<string> { "Considere casos de erro", "Implemente validações", "Use async/await quando apropriado" }
                },
                new Exercise
                {
                    Title = "Exercício Avançado - Filters",
                    Description = "Desenvolva uma solução completa que integre múltiplos conceitos.",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = @"// Implemente uma solução avançada
public class ImplementacaoAvancada
{
    // Seu código completo aqui
}",
                    Hints = new List<string> { "Integre com outros componentes", "Implemente tratamento de erros robusto", "Considere performance e escalabilidade" }
                }
            },
            Summary = "Nesta aula você aprendeu sobre filters, incluindo conceitos fundamentais, implementação prática e melhores práticas. Continue praticando para dominar completamente este tópico essencial do ASP.NET Core."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0006-000000000014"),
            CourseId = _courseId,
            Title = "Filters",
            Duration = "45 min",
            Difficulty = "Intermediário",
            EstimatedMinutes = 45,
            Prerequisites = JsonSerializer.Serialize(new[] { "10000000-0000-0000-0006-000000000013" }),
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
                "Compreender os conceitos de error",
                "Aprender a implementar handling",
                "Dominar aplicação",
                "Aplicar error handling em projetos reais"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "Error Handling - Conceitos Fundamentais",
                    Content = "O conceito de error handling é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam error regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com error, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o como, mas também o porquê por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Implementação Prática",
                    Content = "A implementação de handling no ASP.NET Core segue padrões bem estabelecidos que facilitam o desenvolvimento e manutenção do código. Ao trabalhar com este conceito, você utilizará recursos nativos do framework que foram projetados especificamente para este propósito. A Microsoft investiu significativamente em tornar esta funcionalidade robusta e fácil de usar. Desenvolvedores ao redor do mundo confiam nesta abordagem para construir aplicações de missão crítica. É essencial entender não apenas a sintaxe, mas também os princípios subjacentes que guiam o design desta funcionalidade. Ao dominar estes conceitos, você estará preparado para enfrentar desafios complexos em projetos reais. A prática constante e a experimentação são fundamentais para consolidar este conhecimento.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Melhores Práticas",
                    Content = "Ao trabalhar com aplicação, é crucial seguir as melhores práticas estabelecidas pela comunidade .NET. Estas práticas foram desenvolvidas ao longo de anos de experiência coletiva e ajudam a evitar armadilhas comuns. Segurança, performance e manutenibilidade devem estar sempre em mente ao implementar esta funcionalidade. Testes automatizados são essenciais para garantir que seu código funcione conforme esperado. Documentação clara ajuda outros desenvolvedores a entender suas decisões de design. Code reviews e pair programming são excelentes formas de compartilhar conhecimento e melhorar a qualidade do código. Lembre-se que código bom é código que outros desenvolvedores conseguem entender e manter facilmente.",
                    Order = 3
                }
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Exemplo Básico de Error Handling",
                    Code = @"using Microsoft.AspNetCore.Mvc;

// Implementação de Error Handling
using Microsoft.AspNetCore.Mvc;

namespace MeuApp.Controllers
{
    public class ExemploController : Controller
    {
        public IActionResult Index()
        {
            // Implementação básica
            return View();
        }
    }
}",
                    Language = "csharp",
                    Explanation = "Este exemplo demonstra a implementação básica de error handling no ASP.NET Core.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Exemplo Avançado de Error Handling",
                    Code = @"using Microsoft.AspNetCore.Mvc;

// Implementação avançada de Error Handling
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MeuApp.Controllers
{
    [ApiController]
    [Route(""api/[controller]"")]
    public class AvancadoController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetData()
        {
            // Implementação avançada
            await Task.Delay(100);
            return Ok(new { message = ""Sucesso"" });
        }
    }
}",
                    Language = "csharp",
                    Explanation = "Este exemplo mostra uma implementação mais avançada utilizando async/await e API controllers.",
                    IsRunnable = true
                }
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Exercício Básico - Error Handling",
                    Description = "Implemente uma solução básica utilizando os conceitos de error handling.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"// Implemente sua solução aqui
public class MinhaImplementacao
{
    // Seu código
}",
                    Hints = new List<string> { "Revise os exemplos de código", "Consulte a documentação oficial", "Teste sua implementação" }
                },
                new Exercise
                {
                    Title = "Exercício Intermediário - Error Handling",
                    Description = "Crie uma implementação mais complexa que demonstre domínio dos conceitos.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"// Implemente uma solução intermediária
public class ImplementacaoIntermediaria
{
    // Seu código aqui
}",
                    Hints = new List<string> { "Considere casos de erro", "Implemente validações", "Use async/await quando apropriado" }
                },
                new Exercise
                {
                    Title = "Exercício Avançado - Error Handling",
                    Description = "Desenvolva uma solução completa que integre múltiplos conceitos.",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = @"// Implemente uma solução avançada
public class ImplementacaoAvancada
{
    // Seu código completo aqui
}",
                    Hints = new List<string> { "Integre com outros componentes", "Implemente tratamento de erros robusto", "Considere performance e escalabilidade" }
                }
            },
            Summary = "Nesta aula você aprendeu sobre error handling, incluindo conceitos fundamentais, implementação prática e melhores práticas. Continue praticando para dominar completamente este tópico essencial do ASP.NET Core."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0006-000000000015"),
            CourseId = _courseId,
            Title = "Error Handling",
            Duration = "45 min",
            Difficulty = "Intermediário",
            EstimatedMinutes = 45,
            Prerequisites = JsonSerializer.Serialize(new[] { "10000000-0000-0000-0006-000000000014" }),
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
                "Compreender os conceitos de static",
                "Aprender a implementar files",
                "Dominar aplicação",
                "Aplicar static files em projetos reais"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "Static Files - Conceitos Fundamentais",
                    Content = "O conceito de static files é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam static regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com static, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o como, mas também o porquê por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Implementação Prática",
                    Content = "A implementação de files no ASP.NET Core segue padrões bem estabelecidos que facilitam o desenvolvimento e manutenção do código. Ao trabalhar com este conceito, você utilizará recursos nativos do framework que foram projetados especificamente para este propósito. A Microsoft investiu significativamente em tornar esta funcionalidade robusta e fácil de usar. Desenvolvedores ao redor do mundo confiam nesta abordagem para construir aplicações de missão crítica. É essencial entender não apenas a sintaxe, mas também os princípios subjacentes que guiam o design desta funcionalidade. Ao dominar estes conceitos, você estará preparado para enfrentar desafios complexos em projetos reais. A prática constante e a experimentação são fundamentais para consolidar este conhecimento.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Melhores Práticas",
                    Content = "Ao trabalhar com aplicação, é crucial seguir as melhores práticas estabelecidas pela comunidade .NET. Estas práticas foram desenvolvidas ao longo de anos de experiência coletiva e ajudam a evitar armadilhas comuns. Segurança, performance e manutenibilidade devem estar sempre em mente ao implementar esta funcionalidade. Testes automatizados são essenciais para garantir que seu código funcione conforme esperado. Documentação clara ajuda outros desenvolvedores a entender suas decisões de design. Code reviews e pair programming são excelentes formas de compartilhar conhecimento e melhorar a qualidade do código. Lembre-se que código bom é código que outros desenvolvedores conseguem entender e manter facilmente.",
                    Order = 3
                }
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Exemplo Básico de Static Files",
                    Code = @"using Microsoft.AspNetCore.Mvc;

// Implementação de Static Files
using Microsoft.AspNetCore.Mvc;

namespace MeuApp.Controllers
{
    public class ExemploController : Controller
    {
        public IActionResult Index()
        {
            // Implementação básica
            return View();
        }
    }
}",
                    Language = "csharp",
                    Explanation = "Este exemplo demonstra a implementação básica de static files no ASP.NET Core.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Exemplo Avançado de Static Files",
                    Code = @"using Microsoft.AspNetCore.Mvc;

// Implementação avançada de Static Files
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MeuApp.Controllers
{
    [ApiController]
    [Route(""api/[controller]"")]
    public class AvancadoController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetData()
        {
            // Implementação avançada
            await Task.Delay(100);
            return Ok(new { message = ""Sucesso"" });
        }
    }
}",
                    Language = "csharp",
                    Explanation = "Este exemplo mostra uma implementação mais avançada utilizando async/await e API controllers.",
                    IsRunnable = true
                }
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Exercício Básico - Static Files",
                    Description = "Implemente uma solução básica utilizando os conceitos de static files.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"// Implemente sua solução aqui
public class MinhaImplementacao
{
    // Seu código
}",
                    Hints = new List<string> { "Revise os exemplos de código", "Consulte a documentação oficial", "Teste sua implementação" }
                },
                new Exercise
                {
                    Title = "Exercício Intermediário - Static Files",
                    Description = "Crie uma implementação mais complexa que demonstre domínio dos conceitos.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"// Implemente uma solução intermediária
public class ImplementacaoIntermediaria
{
    // Seu código aqui
}",
                    Hints = new List<string> { "Considere casos de erro", "Implemente validações", "Use async/await quando apropriado" }
                },
                new Exercise
                {
                    Title = "Exercício Avançado - Static Files",
                    Description = "Desenvolva uma solução completa que integre múltiplos conceitos.",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = @"// Implemente uma solução avançada
public class ImplementacaoAvancada
{
    // Seu código completo aqui
}",
                    Hints = new List<string> { "Integre com outros componentes", "Implemente tratamento de erros robusto", "Considere performance e escalabilidade" }
                }
            },
            Summary = "Nesta aula você aprendeu sobre static files, incluindo conceitos fundamentais, implementação prática e melhores práticas. Continue praticando para dominar completamente este tópico essencial do ASP.NET Core."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0006-000000000016"),
            CourseId = _courseId,
            Title = "Static Files",
            Duration = "45 min",
            Difficulty = "Intermediário",
            EstimatedMinutes = 45,
            Prerequisites = JsonSerializer.Serialize(new[] { "10000000-0000-0000-0006-000000000015" }),
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
                "Compreender os conceitos de session",
                "Aprender a implementar e",
                "Dominar state",
                "Aplicar session e state em projetos reais"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "Session e State - Conceitos Fundamentais",
                    Content = "O conceito de session e state é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam session regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com session, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o como, mas também o porquê por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Implementação Prática",
                    Content = "A implementação de e no ASP.NET Core segue padrões bem estabelecidos que facilitam o desenvolvimento e manutenção do código. Ao trabalhar com este conceito, você utilizará recursos nativos do framework que foram projetados especificamente para este propósito. A Microsoft investiu significativamente em tornar esta funcionalidade robusta e fácil de usar. Desenvolvedores ao redor do mundo confiam nesta abordagem para construir aplicações de missão crítica. É essencial entender não apenas a sintaxe, mas também os princípios subjacentes que guiam o design desta funcionalidade. Ao dominar estes conceitos, você estará preparado para enfrentar desafios complexos em projetos reais. A prática constante e a experimentação são fundamentais para consolidar este conhecimento.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Melhores Práticas",
                    Content = "Ao trabalhar com state, é crucial seguir as melhores práticas estabelecidas pela comunidade .NET. Estas práticas foram desenvolvidas ao longo de anos de experiência coletiva e ajudam a evitar armadilhas comuns. Segurança, performance e manutenibilidade devem estar sempre em mente ao implementar esta funcionalidade. Testes automatizados são essenciais para garantir que seu código funcione conforme esperado. Documentação clara ajuda outros desenvolvedores a entender suas decisões de design. Code reviews e pair programming são excelentes formas de compartilhar conhecimento e melhorar a qualidade do código. Lembre-se que código bom é código que outros desenvolvedores conseguem entender e manter facilmente.",
                    Order = 3
                }
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Exemplo Básico de Session e State",
                    Code = @"using Microsoft.AspNetCore.Mvc;

// Implementação de Session e State
using Microsoft.AspNetCore.Mvc;

namespace MeuApp.Controllers
{
    public class ExemploController : Controller
    {
        public IActionResult Index()
        {
            // Implementação básica
            return View();
        }
    }
}",
                    Language = "csharp",
                    Explanation = "Este exemplo demonstra a implementação básica de session e state no ASP.NET Core.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Exemplo Avançado de Session e State",
                    Code = @"using Microsoft.AspNetCore.Mvc;

// Implementação avançada de Session e State
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MeuApp.Controllers
{
    [ApiController]
    [Route(""api/[controller]"")]
    public class AvancadoController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetData()
        {
            // Implementação avançada
            await Task.Delay(100);
            return Ok(new { message = ""Sucesso"" });
        }
    }
}",
                    Language = "csharp",
                    Explanation = "Este exemplo mostra uma implementação mais avançada utilizando async/await e API controllers.",
                    IsRunnable = true
                }
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Exercício Básico - Session e State",
                    Description = "Implemente uma solução básica utilizando os conceitos de session e state.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"// Implemente sua solução aqui
public class MinhaImplementacao
{
    // Seu código
}",
                    Hints = new List<string> { "Revise os exemplos de código", "Consulte a documentação oficial", "Teste sua implementação" }
                },
                new Exercise
                {
                    Title = "Exercício Intermediário - Session e State",
                    Description = "Crie uma implementação mais complexa que demonstre domínio dos conceitos.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"// Implemente uma solução intermediária
public class ImplementacaoIntermediaria
{
    // Seu código aqui
}",
                    Hints = new List<string> { "Considere casos de erro", "Implemente validações", "Use async/await quando apropriado" }
                },
                new Exercise
                {
                    Title = "Exercício Avançado - Session e State",
                    Description = "Desenvolva uma solução completa que integre múltiplos conceitos.",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = @"// Implemente uma solução avançada
public class ImplementacaoAvancada
{
    // Seu código completo aqui
}",
                    Hints = new List<string> { "Integre com outros componentes", "Implemente tratamento de erros robusto", "Considere performance e escalabilidade" }
                }
            },
            Summary = "Nesta aula você aprendeu sobre session e state, incluindo conceitos fundamentais, implementação prática e melhores práticas. Continue praticando para dominar completamente este tópico essencial do ASP.NET Core."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0006-000000000017"),
            CourseId = _courseId,
            Title = "Session e State",
            Duration = "45 min",
            Difficulty = "Intermediário",
            EstimatedMinutes = 45,
            Prerequisites = JsonSerializer.Serialize(new[] { "10000000-0000-0000-0006-000000000016" }),
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
                "Compreender os conceitos de areas",
                "Aprender a implementar implementação",
                "Dominar aplicação",
                "Aplicar areas em projetos reais"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "Areas - Conceitos Fundamentais",
                    Content = "O conceito de areas é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam areas regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com areas, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o como, mas também o porquê por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Implementação Prática",
                    Content = "A implementação de implementação no ASP.NET Core segue padrões bem estabelecidos que facilitam o desenvolvimento e manutenção do código. Ao trabalhar com este conceito, você utilizará recursos nativos do framework que foram projetados especificamente para este propósito. A Microsoft investiu significativamente em tornar esta funcionalidade robusta e fácil de usar. Desenvolvedores ao redor do mundo confiam nesta abordagem para construir aplicações de missão crítica. É essencial entender não apenas a sintaxe, mas também os princípios subjacentes que guiam o design desta funcionalidade. Ao dominar estes conceitos, você estará preparado para enfrentar desafios complexos em projetos reais. A prática constante e a experimentação são fundamentais para consolidar este conhecimento.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Melhores Práticas",
                    Content = "Ao trabalhar com aplicação, é crucial seguir as melhores práticas estabelecidas pela comunidade .NET. Estas práticas foram desenvolvidas ao longo de anos de experiência coletiva e ajudam a evitar armadilhas comuns. Segurança, performance e manutenibilidade devem estar sempre em mente ao implementar esta funcionalidade. Testes automatizados são essenciais para garantir que seu código funcione conforme esperado. Documentação clara ajuda outros desenvolvedores a entender suas decisões de design. Code reviews e pair programming são excelentes formas de compartilhar conhecimento e melhorar a qualidade do código. Lembre-se que código bom é código que outros desenvolvedores conseguem entender e manter facilmente.",
                    Order = 3
                }
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Exemplo Básico de Areas",
                    Code = @"using Microsoft.AspNetCore.Mvc;

// Implementação de Areas
using Microsoft.AspNetCore.Mvc;

namespace MeuApp.Controllers
{
    public class ExemploController : Controller
    {
        public IActionResult Index()
        {
            // Implementação básica
            return View();
        }
    }
}",
                    Language = "csharp",
                    Explanation = "Este exemplo demonstra a implementação básica de areas no ASP.NET Core.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Exemplo Avançado de Areas",
                    Code = @"using Microsoft.AspNetCore.Mvc;

// Implementação avançada de Areas
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MeuApp.Controllers
{
    [ApiController]
    [Route(""api/[controller]"")]
    public class AvancadoController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetData()
        {
            // Implementação avançada
            await Task.Delay(100);
            return Ok(new { message = ""Sucesso"" });
        }
    }
}",
                    Language = "csharp",
                    Explanation = "Este exemplo mostra uma implementação mais avançada utilizando async/await e API controllers.",
                    IsRunnable = true
                }
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Exercício Básico - Areas",
                    Description = "Implemente uma solução básica utilizando os conceitos de areas.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"// Implemente sua solução aqui
public class MinhaImplementacao
{
    // Seu código
}",
                    Hints = new List<string> { "Revise os exemplos de código", "Consulte a documentação oficial", "Teste sua implementação" }
                },
                new Exercise
                {
                    Title = "Exercício Intermediário - Areas",
                    Description = "Crie uma implementação mais complexa que demonstre domínio dos conceitos.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"// Implemente uma solução intermediária
public class ImplementacaoIntermediaria
{
    // Seu código aqui
}",
                    Hints = new List<string> { "Considere casos de erro", "Implemente validações", "Use async/await quando apropriado" }
                },
                new Exercise
                {
                    Title = "Exercício Avançado - Areas",
                    Description = "Desenvolva uma solução completa que integre múltiplos conceitos.",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = @"// Implemente uma solução avançada
public class ImplementacaoAvancada
{
    // Seu código completo aqui
}",
                    Hints = new List<string> { "Integre com outros componentes", "Implemente tratamento de erros robusto", "Considere performance e escalabilidade" }
                }
            },
            Summary = "Nesta aula você aprendeu sobre areas, incluindo conceitos fundamentais, implementação prática e melhores práticas. Continue praticando para dominar completamente este tópico essencial do ASP.NET Core."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0006-000000000018"),
            CourseId = _courseId,
            Title = "Areas",
            Duration = "45 min",
            Difficulty = "Intermediário",
            EstimatedMinutes = 45,
            Prerequisites = JsonSerializer.Serialize(new[] { "10000000-0000-0000-0006-000000000017" }),
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
                "Compreender os conceitos de localization",
                "Aprender a implementar implementação",
                "Dominar aplicação",
                "Aplicar localization em projetos reais"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "Localization - Conceitos Fundamentais",
                    Content = "O conceito de localization é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam localization regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com localization, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o como, mas também o porquê por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Implementação Prática",
                    Content = "A implementação de implementação no ASP.NET Core segue padrões bem estabelecidos que facilitam o desenvolvimento e manutenção do código. Ao trabalhar com este conceito, você utilizará recursos nativos do framework que foram projetados especificamente para este propósito. A Microsoft investiu significativamente em tornar esta funcionalidade robusta e fácil de usar. Desenvolvedores ao redor do mundo confiam nesta abordagem para construir aplicações de missão crítica. É essencial entender não apenas a sintaxe, mas também os princípios subjacentes que guiam o design desta funcionalidade. Ao dominar estes conceitos, você estará preparado para enfrentar desafios complexos em projetos reais. A prática constante e a experimentação são fundamentais para consolidar este conhecimento.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Melhores Práticas",
                    Content = "Ao trabalhar com aplicação, é crucial seguir as melhores práticas estabelecidas pela comunidade .NET. Estas práticas foram desenvolvidas ao longo de anos de experiência coletiva e ajudam a evitar armadilhas comuns. Segurança, performance e manutenibilidade devem estar sempre em mente ao implementar esta funcionalidade. Testes automatizados são essenciais para garantir que seu código funcione conforme esperado. Documentação clara ajuda outros desenvolvedores a entender suas decisões de design. Code reviews e pair programming são excelentes formas de compartilhar conhecimento e melhorar a qualidade do código. Lembre-se que código bom é código que outros desenvolvedores conseguem entender e manter facilmente.",
                    Order = 3
                }
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Exemplo Básico de Localization",
                    Code = @"using Microsoft.AspNetCore.Mvc;

// Implementação de Localization
using Microsoft.AspNetCore.Mvc;

namespace MeuApp.Controllers
{
    public class ExemploController : Controller
    {
        public IActionResult Index()
        {
            // Implementação básica
            return View();
        }
    }
}",
                    Language = "csharp",
                    Explanation = "Este exemplo demonstra a implementação básica de localization no ASP.NET Core.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Exemplo Avançado de Localization",
                    Code = @"using Microsoft.AspNetCore.Mvc;

// Implementação avançada de Localization
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MeuApp.Controllers
{
    [ApiController]
    [Route(""api/[controller]"")]
    public class AvancadoController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetData()
        {
            // Implementação avançada
            await Task.Delay(100);
            return Ok(new { message = ""Sucesso"" });
        }
    }
}",
                    Language = "csharp",
                    Explanation = "Este exemplo mostra uma implementação mais avançada utilizando async/await e API controllers.",
                    IsRunnable = true
                }
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Exercício Básico - Localization",
                    Description = "Implemente uma solução básica utilizando os conceitos de localization.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"// Implemente sua solução aqui
public class MinhaImplementacao
{
    // Seu código
}",
                    Hints = new List<string> { "Revise os exemplos de código", "Consulte a documentação oficial", "Teste sua implementação" }
                },
                new Exercise
                {
                    Title = "Exercício Intermediário - Localization",
                    Description = "Crie uma implementação mais complexa que demonstre domínio dos conceitos.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"// Implemente uma solução intermediária
public class ImplementacaoIntermediaria
{
    // Seu código aqui
}",
                    Hints = new List<string> { "Considere casos de erro", "Implemente validações", "Use async/await quando apropriado" }
                },
                new Exercise
                {
                    Title = "Exercício Avançado - Localization",
                    Description = "Desenvolva uma solução completa que integre múltiplos conceitos.",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = @"// Implemente uma solução avançada
public class ImplementacaoAvancada
{
    // Seu código completo aqui
}",
                    Hints = new List<string> { "Integre com outros componentes", "Implemente tratamento de erros robusto", "Considere performance e escalabilidade" }
                }
            },
            Summary = "Nesta aula você aprendeu sobre localization, incluindo conceitos fundamentais, implementação prática e melhores práticas. Continue praticando para dominar completamente este tópico essencial do ASP.NET Core."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0006-000000000019"),
            CourseId = _courseId,
            Title = "Localization",
            Duration = "45 min",
            Difficulty = "Intermediário",
            EstimatedMinutes = 45,
            Prerequisites = JsonSerializer.Serialize(new[] { "10000000-0000-0000-0006-000000000018" }),
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
                "Aprender a implementar e",
                "Dominar caching",
                "Aplicar performance e caching em projetos reais"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "Performance e Caching - Conceitos Fundamentais",
                    Content = "O conceito de performance e caching é fundamental no desenvolvimento moderno com ASP.NET Core. Este recurso permite que você construa aplicações mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam performance regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva aplicações enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com performance, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET. Vamos explorar não apenas o como, mas também o porquê por trás deste conceito, para que você possa tomar decisões arquiteturais informadas em seus projetos.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Implementação Prática",
                    Content = "A implementação de e no ASP.NET Core segue padrões bem estabelecidos que facilitam o desenvolvimento e manutenção do código. Ao trabalhar com este conceito, você utilizará recursos nativos do framework que foram projetados especificamente para este propósito. A Microsoft investiu significativamente em tornar esta funcionalidade robusta e fácil de usar. Desenvolvedores ao redor do mundo confiam nesta abordagem para construir aplicações de missão crítica. É essencial entender não apenas a sintaxe, mas também os princípios subjacentes que guiam o design desta funcionalidade. Ao dominar estes conceitos, você estará preparado para enfrentar desafios complexos em projetos reais. A prática constante e a experimentação são fundamentais para consolidar este conhecimento.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Melhores Práticas",
                    Content = "Ao trabalhar com caching, é crucial seguir as melhores práticas estabelecidas pela comunidade .NET. Estas práticas foram desenvolvidas ao longo de anos de experiência coletiva e ajudam a evitar armadilhas comuns. Segurança, performance e manutenibilidade devem estar sempre em mente ao implementar esta funcionalidade. Testes automatizados são essenciais para garantir que seu código funcione conforme esperado. Documentação clara ajuda outros desenvolvedores a entender suas decisões de design. Code reviews e pair programming são excelentes formas de compartilhar conhecimento e melhorar a qualidade do código. Lembre-se que código bom é código que outros desenvolvedores conseguem entender e manter facilmente.",
                    Order = 3
                }
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Exemplo Básico de Performance e Caching",
                    Code = @"using Microsoft.AspNetCore.Mvc;

// Implementação de Performance e Caching
using Microsoft.AspNetCore.Mvc;

namespace MeuApp.Controllers
{
    public class ExemploController : Controller
    {
        public IActionResult Index()
        {
            // Implementação básica
            return View();
        }
    }
}",
                    Language = "csharp",
                    Explanation = "Este exemplo demonstra a implementação básica de performance e caching no ASP.NET Core.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Exemplo Avançado de Performance e Caching",
                    Code = @"using Microsoft.AspNetCore.Mvc;

// Implementação avançada de Performance e Caching
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MeuApp.Controllers
{
    [ApiController]
    [Route(""api/[controller]"")]
    public class AvancadoController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetData()
        {
            // Implementação avançada
            await Task.Delay(100);
            return Ok(new { message = ""Sucesso"" });
        }
    }
}",
                    Language = "csharp",
                    Explanation = "Este exemplo mostra uma implementação mais avançada utilizando async/await e API controllers.",
                    IsRunnable = true
                }
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Exercício Básico - Performance e Caching",
                    Description = "Implemente uma solução básica utilizando os conceitos de performance e caching.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = @"// Implemente sua solução aqui
public class MinhaImplementacao
{
    // Seu código
}",
                    Hints = new List<string> { "Revise os exemplos de código", "Consulte a documentação oficial", "Teste sua implementação" }
                },
                new Exercise
                {
                    Title = "Exercício Intermediário - Performance e Caching",
                    Description = "Crie uma implementação mais complexa que demonstre domínio dos conceitos.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"// Implemente uma solução intermediária
public class ImplementacaoIntermediaria
{
    // Seu código aqui
}",
                    Hints = new List<string> { "Considere casos de erro", "Implemente validações", "Use async/await quando apropriado" }
                },
                new Exercise
                {
                    Title = "Exercício Avançado - Performance e Caching",
                    Description = "Desenvolva uma solução completa que integre múltiplos conceitos.",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = @"// Implemente uma solução avançada
public class ImplementacaoAvancada
{
    // Seu código completo aqui
}",
                    Hints = new List<string> { "Integre com outros componentes", "Implemente tratamento de erros robusto", "Considere performance e escalabilidade" }
                }
            },
            Summary = "Nesta aula você aprendeu sobre performance e caching, incluindo conceitos fundamentais, implementação prática e melhores práticas. Continue praticando para dominar completamente este tópico essencial do ASP.NET Core."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0006-000000000020"),
            CourseId = _courseId,
            Title = "Performance e Caching",
            Duration = "45 min",
            Difficulty = "Intermediário",
            EstimatedMinutes = 45,
            Prerequisites = JsonSerializer.Serialize(new[] { "10000000-0000-0000-0006-000000000019" }),
            OrderIndex = 20,
            Content = "",
            StructuredContent = JsonSerializer.Serialize(content),
            Version = 1,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }

}
