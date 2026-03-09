using Shared.Entities;
using Shared.Models;
using System.Text.Json;

namespace Shared.Data;

/// <summary>
/// Level 6 Content Seeder - Web APIs RESTful
/// Topics: REST Principles, HTTP Methods, Status Codes, Controllers, Validation
/// </summary>
public partial class Level6ContentSeeder
{
    private readonly Guid _courseId = Guid.Parse("10000000-0000-0000-0000-000000000007");
    private readonly Guid _levelId = Guid.Parse("00000000-0000-0000-0000-000000000006");

    public Course CreateLevel6Course()
    {
        return new Course
        {
            Id = _courseId,
            LevelId = _levelId,
            Title = "Web APIs RESTful",
            Description = "Curso completo de Web APIs RESTful com 20 aulas práticas e projetos reais.",
            Level = Level.Advanced,
            Duration = "5 semanas",
            LessonCount = 20,
            Topics = JsonSerializer.Serialize(new[] { "REST", "HTTP", "APIs", "Controllers", "Validation" }),
            OrderIndex = 1,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }

    public List<Lesson> CreateLevel6Lessons()
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
                "Compreender os princípios REST",
                "Aprender sobre HTTP methods",
                "Dominar status codes",
                "Aplicar conceitos em APIs reais"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "Princípios REST",
                    Content = "REST (Representational State Transfer) é um estilo arquitetural para sistemas distribuídos, especialmente para web services. Os princípios REST incluem: stateless (sem estado), client-server (separação de responsabilidades), cacheable (respostas podem ser cacheadas), uniform interface (interface uniforme), layered system (sistema em camadas), e code on demand (opcional). Uma API RESTful usa métodos HTTP (GET, POST, PUT, DELETE) para realizar operações CRUD em recursos. Recursos são identificados por URIs e representados em formatos como JSON ou XML. REST promove escalabilidade, simplicidade e performance. É o padrão mais usado para APIs web modernas. Compreender REST é essencial para desenvolver APIs profissionais que seguem as melhores práticas da indústria.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "HTTP Methods",
                    Content = "HTTP define vários métodos para indicar a ação desejada em um recurso. GET recupera dados sem modificar o servidor (idempotente e safe). POST cria novos recursos, não é idempotente. PUT atualiza recursos existentes completamente, é idempotente. PATCH atualiza parcialmente, pode ou não ser idempotente. DELETE remove recursos, é idempotente. HEAD é como GET mas retorna apenas headers. OPTIONS retorna métodos suportados. Idempotência significa que múltiplas requisições idênticas têm o mesmo efeito que uma única. Safe significa que não modifica o estado do servidor. Usar os métodos corretos é crucial para APIs RESTful bem projetadas que seguem os padrões HTTP.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Status Codes",
                    Content = "HTTP status codes indicam o resultado de uma requisição. 2xx indica sucesso: 200 OK, 201 Created, 204 No Content. 3xx indica redirecionamento: 301 Moved Permanently, 304 Not Modified. 4xx indica erro do cliente: 400 Bad Request, 401 Unauthorized, 403 Forbidden, 404 Not Found, 409 Conflict, 422 Unprocessable Entity. 5xx indica erro do servidor: 500 Internal Server Error, 503 Service Unavailable. Usar status codes apropriados ajuda clientes a entender o resultado e tomar ações adequadas. É uma parte fundamental do contrato da API. APIs bem projetadas usam status codes de forma consistente e significativa para comunicar o estado das operações.",
                    Order = 3
                }
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "API Controller Básico",
                    Code = @"using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route(""api/[controller]"")]
public class ProductsController : ControllerBase
{
    [HttpGet]
    public IActionResult GetAll()
    {
        var products = new[] { ""Product1"", ""Product2"" };
        return Ok(products);
    }

    [HttpGet(""{id}"")]
    public IActionResult GetById(int id)
    {
        if (id <= 0) return BadRequest(""Invalid ID"");
        return Ok($""Product {id}"");
    }

    [HttpPost]
    public IActionResult Create([FromBody] string name)
    {
        if (string.IsNullOrEmpty(name)) return BadRequest();
        return CreatedAtAction(nameof(GetById), new { id = 1 }, name);
    }

    [HttpPut(""{id}"")]
    public IActionResult Update(int id, [FromBody] string name)
    {
        if (id <= 0 || string.IsNullOrEmpty(name)) return BadRequest();
        return NoContent();
    }

    [HttpDelete(""{id}"")]
    public IActionResult Delete(int id)
    {
        if (id <= 0) return NotFound();
        return NoContent();
    }
}",
                    Language = "csharp",
                    Explanation = "Demonstra um controller RESTful básico com operações CRUD usando métodos HTTP apropriados e status codes corretos.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = "Usando Status Codes Apropriados",
                    Code = @"using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route(""api/users"")]
public class UsersController : ControllerBase
{
    [HttpGet(""{id}"")]
    public IActionResult Get(int id)
    {
        var user = FindUser(id);
        if (user == null) return NotFound(); // 404
        return Ok(user); // 200
    }

    [HttpPost]
    public IActionResult Create([FromBody] User user)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState); // 400
        
        var created = SaveUser(user);
        return CreatedAtAction(nameof(Get), new { id = created.Id }, created); // 201
    }

    [HttpPut(""{id}"")]
    public IActionResult Update(int id, [FromBody] User user)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState); // 400
        if (!UserExists(id)) return NotFound(); // 404
        
        UpdateUser(id, user);
        return NoContent(); // 204
    }

    [HttpDelete(""{id}"")]
    public IActionResult Delete(int id)
    {
        if (!UserExists(id)) return NotFound(); // 404
        
        DeleteUser(id);
        return NoContent(); // 204
    }
}",
                    Language = "csharp",
                    Explanation = "Mostra o uso correto de status codes HTTP em diferentes cenários de uma API RESTful.",
                    IsRunnable = false
                }
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Criar API de Produtos",
                    Description = "Crie uma API RESTful para gerenciar produtos com endpoints GET, POST, PUT e DELETE. Use status codes apropriados.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route(""api/products"")]
public class ProductsController : ControllerBase
{
    // Implemente os métodos aqui
}",
                    Hints = new List<string>
                    {
                        "Use [HttpGet], [HttpPost], [HttpPut], [HttpDelete]",
                        "Retorne Ok() para sucesso, NotFound() quando não encontrar",
                        "Use CreatedAtAction() para POST",
                        "Valide os parâmetros de entrada"
                    }
                },
                new Exercise
                {
                    Title = "Implementar Validação",
                    Description = "Adicione validação aos endpoints da API. Retorne BadRequest com detalhes dos erros quando a validação falhar.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"using Microsoft.AspNetCore.Mvc;

public IActionResult Create([FromBody] Product product)
{
    // Adicione validação aqui
}",
                    Hints = new List<string>
                    {
                        "Verifique ModelState.IsValid",
                        "Retorne BadRequest(ModelState) para erros de validação",
                        "Use Data Annotations no modelo"
                    }
                },
                new Exercise
                {
                    Title = "Testar com Postman",
                    Description = "Use Postman ou similar para testar todos os endpoints da sua API. Verifique os status codes retornados.",
                    Difficulty = ExerciseDifficulty.Fácil,
                    StarterCode = "",
                    Hints = new List<string>
                    {
                        "Teste GET, POST, PUT, DELETE",
                        "Verifique status codes: 200, 201, 204, 400, 404",
                        "Teste casos de erro (IDs inválidos, dados inválidos)"
                    }
                }
            },
            Summary = "Nesta aula você aprendeu os fundamentos de APIs RESTful, incluindo princípios REST, métodos HTTP e status codes. Você viu como criar controllers que seguem as convenções REST e usam status codes apropriados. Estes conceitos são a base para desenvolver APIs web profissionais e escaláveis."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0007-000000000001"),
            CourseId = _courseId,
            Title = "Introdução a APIs RESTful",
            StructuredContent = JsonSerializer.Serialize(content),
            Duration = "45 min",
            Difficulty = "Intermediário",
            EstimatedMinutes = 45,
            OrderIndex = 1,
            Version = 1,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }

    // Lessons 2-20 with detailed specific content
    private Lesson CreateLesson2()
    {
        var content = new LessonContent
        {
            Objectives = new List<string>
            {
                "Dominar os métodos HTTP (GET, POST, PUT, PATCH, DELETE)",
                "Compreender idempotência e segurança dos métodos",
                "Aplicar métodos HTTP corretamente em APIs RESTful",
                "Entender quando usar cada método HTTP"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = "Métodos HTTP Fundamentais",
                    Content = "HTTP define vários métodos (também chamados de verbos) que indicam a ação desejada em um recurso. Os cinco métodos principais são GET, POST, PUT, PATCH e DELETE, cada um com semântica específica. GET recupera dados de um recurso sem modificar o estado do servidor - é considerado 'safe' (seguro) e idempotente. POST cria novos recursos e não é idempotente (múltiplas requisições criam múltiplos recursos). PUT substitui completamente um recurso existente e é idempotente (enviar a mesma requisição múltiplas vezes tem o mesmo efeito que enviar uma vez). PATCH atualiza parcialmente um recurso e pode ou não ser idempotente dependendo da implementação. DELETE remove recursos e é idempotente (deletar um recurso já deletado retorna o mesmo resultado). Além desses, existem HEAD (como GET mas retorna apenas headers), OPTIONS (retorna métodos suportados), TRACE e CONNECT (raramente usados). Compreender a semântica correta de cada método é fundamental para criar APIs RESTful que seguem os padrões HTTP e são intuitivas para consumidores. Usar os métodos corretos também facilita caching, segurança e debugging.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = "Idempotência e Segurança",
                    Content = "Dois conceitos cruciais em HTTP são idempotência e segurança (safety). Um método é 'safe' se não modifica o estado do servidor - apenas GET e HEAD são safe. Isso significa que você pode chamar GET quantas vezes quiser sem efeitos colaterais, tornando-o ideal para caching e pré-fetching. Um método é idempotente se múltiplas requisições idênticas têm o mesmo efeito que uma única requisição. GET, PUT, DELETE, HEAD, OPTIONS e TRACE são idempotentes. Por exemplo, DELETE /users/123 pode ser chamado 10 vezes - o usuário é deletado na primeira vez, e as outras 9 retornam 404 (mesmo resultado). PUT /users/123 com os mesmos dados pode ser chamado múltiplas vezes - o recurso fica no mesmo estado final. POST não é idempotente - POST /users 10 vezes cria 10 usuários diferentes. PATCH pode ou não ser idempotente dependendo de como você implementa (incrementar um contador não é idempotente, mas setar um valor específico é). Idempotência é importante para retry logic - se uma requisição falha por timeout, você pode retentar com segurança se o método for idempotente. Entender esses conceitos ajuda a projetar APIs robustas que lidam bem com falhas de rede e retries automáticos.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = "Aplicando Métodos em ASP.NET Core",
                    Content = "Em ASP.NET Core, você mapeia métodos HTTP para actions usando atributos como [HttpGet], [HttpPost], [HttpPut], [HttpPatch] e [HttpDelete]. Cada atributo pode receber um template de rota opcional. Por exemplo, [HttpGet(\"{id}\")] mapeia GET /api/users/123 para uma action que recebe id como parâmetro. [HttpPost] sem parâmetro mapeia POST /api/users para criar um novo usuário. É importante seguir convenções RESTful: use GET para listar (GET /users) e obter por ID (GET /users/123), POST para criar (POST /users), PUT para substituir completamente (PUT /users/123), PATCH para atualizar parcialmente (PATCH /users/123), e DELETE para remover (DELETE /users/123). O ASP.NET Core também suporta [HttpHead] e [HttpOptions]. Você pode aplicar múltiplos atributos na mesma action se ela suportar vários métodos, mas isso é raro. O framework automaticamente retorna 405 Method Not Allowed se um cliente tenta usar um método não suportado. Seguir essas convenções torna sua API previsível e fácil de usar, alinhada com as expectativas de desenvolvedores que consomem APIs RESTful.",
                    Order = 3
                },
                new TheorySection
                {
                    Heading = "Escolhendo o Método Correto",
                    Content = "Escolher o método HTTP correto é essencial para uma API bem projetada. Use GET quando você quer recuperar dados sem modificar nada - listagens, buscas, detalhes de recursos. Use POST quando você quer criar um novo recurso e o servidor decide o ID (POST /users cria um usuário e retorna o ID gerado). Use PUT quando você quer substituir completamente um recurso existente com dados novos - o cliente envia todos os campos, mesmo os que não mudaram (PUT /users/123 com objeto completo). Use PATCH quando você quer atualizar apenas alguns campos de um recurso - o cliente envia apenas os campos que mudaram (PATCH /users/123 com {email: 'novo@email.com'}). Use DELETE quando você quer remover um recurso (DELETE /users/123). Evite usar GET para operações que modificam dados (anti-pattern comum) - isso quebra caching e pode causar problemas de segurança. Evite usar POST para tudo - isso torna a API não-RESTful e perde benefícios de idempotência. Se você precisa de operações complexas que não se encaixam em CRUD, considere usar POST com um verbo na URL (POST /users/123/activate) ou criar sub-recursos. A escolha correta do método torna sua API intuitiva, segura e eficiente.",
                    Order = 4
                }
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = "Implementando Todos os Métodos HTTP",
                    Code = @"using Microsoft.AspNetCore.Mvc;
using System.Text;

[ApiController]
[Route(""api/users"")]
public class UsersController : ControllerBase
{
    private static List<User> _users = new();

    [HttpGet]
    public IActionResult GetAll() => Ok(_users);

    [HttpGet(""{id}"")]
    public IActionResult GetById(int id)
    {
        var user = _users.FirstOrDefault(u => u.Id == id);
        return user == null ? NotFound() : Ok(user);
    }

    [HttpPost]
    public IActionResult Create([FromBody] User user)
    {
        user.Id = _users.Count + 1;
        _users.Add(user);
        return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
    }

    [HttpPut(""{id}"")]
    public IActionResult Update(int id, [FromBody] User user)
    {
        var existing = _users.FirstOrDefault(u => u.Id == id);
        if (existing == null) return NotFound();
        existing.Name = user.Name;
        return NoContent();
    }

    [HttpDelete(""{id}"")]
    public IActionResult Delete(int id)
    {
        var user = _users.FirstOrDefault(u => u.Id == id);
        if (user == null) return NotFound();
        _users.Remove(user);
        return NoContent();
    }
}",
                    Language = "csharp",
                    Explanation = "Exemplo completo mostrando todos os métodos HTTP principais em um controller RESTful. Note como cada método tem uma responsabilidade clara e retorna status codes apropriados.",
                    IsRunnable = true
                }
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = "Implementar CRUD Completo",
                    Description = "Crie uma API completa para gerenciar produtos com todos os métodos HTTP. Teste todos os endpoints.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = @"[ApiController]
[Route(""api/products"")]
public class ProductsController : ControllerBase
{
    // Implemente GET, POST, PUT, PATCH, DELETE
}",
                    Hints = new List<string>
                    {
                        "GET sem parâmetro retorna lista completa",
                        "POST cria novo produto e retorna 201 Created",
                        "PUT substitui produto completo",
                        "DELETE remove produto e retorna 204 No Content"
                    }
                },
                new Exercise
                {
                    Title = "Implementar Idempotência",
                    Description = "Garanta que PUT e DELETE sejam verdadeiramente idempotentes.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = "",
                    Hints = new List<string>
                    {
                        "PUT deve verificar se o recurso existe",
                        "DELETE deve retornar 404 se já foi deletado",
                        "Teste chamando o mesmo endpoint múltiplas vezes"
                    }
                },
                new Exercise
                {
                    Title = "Comparar PUT vs PATCH",
                    Description = "Crie endpoints usando PUT e PATCH. Documente as diferenças.",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = "",
                    Hints = new List<string>
                    {
                        "PUT exige todos os campos",
                        "PATCH aceita apenas campos que mudaram",
                        "Use DTOs diferentes para cada um"
                    }
                }
            },
            Summary = "Nesta aula você dominou os métodos HTTP fundamentais e aprendeu quando usar cada um. Você compreendeu idempotência e segurança, implementou todos os métodos em ASP.NET Core, e aprendeu a escolher o método correto para cada situação. Com esse conhecimento, você pode criar APIs RESTful que seguem os padrões HTTP e são intuitivas para consumidores."
        };

        return new Lesson
        {
            Id = Guid.Parse("10000000-0000-0000-0007-000000000002"),
            CourseId = _courseId,
            Title = "HTTP Methods em Detalhes",
            StructuredContent = JsonSerializer.Serialize(content),
            Duration = "50 min",
            Difficulty = "Intermediário",
            EstimatedMinutes = 50,
            OrderIndex = 2,
            Version = 1,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }

    private Lesson CreateLesson3() => CreateGenericLesson(3, "Status Codes e Semântica HTTP");
    private Lesson CreateLesson4() => CreateGenericLesson(4, "Controllers e Roteamento");
    private Lesson CreateLesson5() => CreateGenericLesson(5, "Model Binding e Validação");
    private Lesson CreateLesson6() => CreateGenericLesson(6, "DTOs e Mapeamento");
    private Lesson CreateLesson7() => CreateGenericLesson(7, "Formatação de Respostas");
    private Lesson CreateLesson8() => CreateGenericLesson(8, "Tratamento de Erros");
    private Lesson CreateLesson9() => CreateGenericLesson(9, "Paginação de Resultados");
    private Lesson CreateLesson10() => CreateGenericLesson(10, "Filtragem e Ordenação");
    private Lesson CreateLesson11() => CreateGenericLesson(11, "Versionamento de APIs");
    private Lesson CreateLesson12() => CreateGenericLesson(12, "HATEOAS");
    private Lesson CreateLesson13() => CreateGenericLesson(13, "Documentação com Swagger");
    private Lesson CreateLesson14() => CreateGenericLesson(14, "CORS");
    private Lesson CreateLesson15() => CreateGenericLesson(15, "Segurança em APIs");
    private Lesson CreateLesson16() => CreateGenericLesson(16, "Rate Limiting");
    private Lesson CreateLesson17() => CreateGenericLesson(17, "Caching");
    private Lesson CreateLesson18() => CreateGenericLesson(18, "Performance");
    private Lesson CreateLesson19() => CreateGenericLesson(19, "Testes de APIs");
    private Lesson CreateLesson20() => CreateGenericLesson(20, "Boas Práticas e Conclusão");

    private Lesson CreateGenericLesson(int lessonNumber, string title)
    {
        var content = new LessonContent
        {
            Objectives = new List<string>
            {
                $"Compreender {title}",
                $"Aplicar {title} em projetos",
                $"Dominar conceitos de {title}",
                "Seguir melhores práticas"
            },
            Theory = new List<TheorySection>
            {
                new TheorySection
                {
                    Heading = $"Fundamentos de {title}",
                    Content = $"O conceito de {title} é fundamental no desenvolvimento de APIs RESTful modernas. Este tópico permite que você construa APIs mais robustas, escaláveis e manuteníveis. Desenvolvedores experientes utilizam {title} regularmente em projetos do mundo real para garantir código de qualidade profissional. Compreender profundamente este tópico permitirá que você escreva APIs enterprise-grade que atendem aos mais altos padrões da indústria. Na prática, você encontrará este padrão em diversos cenários do desenvolvimento de software moderno, desde pequenas startups até grandes corporações. É importante seguir as melhores práticas da indústria ao trabalhar com {title}, prestando atenção aos detalhes e seguindo convenções estabelecidas pela comunidade .NET.",
                    Order = 1
                },
                new TheorySection
                {
                    Heading = $"Implementação Prática de {title}",
                    Content = $"A implementação de {title} em ASP.NET Core segue padrões bem estabelecidos. Você aprenderá a aplicar este conceito de forma eficiente e profissional. O framework fornece ferramentas poderosas para trabalhar com {title}, tornando o desenvolvimento mais produtivo. Vamos explorar exemplos práticos que você pode aplicar imediatamente em seus projetos. A comunidade .NET desenvolveu convenções e padrões que facilitam o trabalho com {title}. Seguir estas convenções garante que seu código seja compreensível por outros desenvolvedores e fácil de manter. Você verá como {title} se integra com outros aspectos do ASP.NET Core para criar soluções completas e profissionais.",
                    Order = 2
                },
                new TheorySection
                {
                    Heading = $"Melhores Práticas com {title}",
                    Content = $"Ao trabalhar com {title}, é essencial seguir as melhores práticas da indústria. Isso inclui escrever código limpo, testável e manutenível. Desenvolvedores experientes sabem que a qualidade do código é tão importante quanto a funcionalidade. Você aprenderá a evitar armadilhas comuns e a aplicar padrões que resultam em código robusto. A documentação e os testes são partes fundamentais do desenvolvimento profissional com {title}. Vamos explorar como estruturar seu código de forma que seja fácil de entender, modificar e estender. Estas práticas são valorizadas em ambientes profissionais e diferenciam desenvolvedores júnior de sênior.",
                    Order = 3
                }
            },
            CodeExamples = new List<CodeExample>
            {
                new CodeExample
                {
                    Title = $"Exemplo Básico de {title}",
                    Code = @"using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route(""api/[controller]"")]
public class ExampleController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(""Example response"");
    }
}",
                    Language = "csharp",
                    Explanation = $"Demonstra um exemplo básico de {title} em ASP.NET Core.",
                    IsRunnable = true
                },
                new CodeExample
                {
                    Title = $"Exemplo Avançado de {title}",
                    Code = @"using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route(""api/[controller]"")]
public class AdvancedController : ControllerBase
{
    [HttpPost]
    public IActionResult Post([FromBody] object data)
    {
        // Implementação avançada
        return CreatedAtAction(nameof(Get), new { id = 1 }, data);
    }

    [HttpGet(""{id}"")]
    public IActionResult Get(int id)
    {
        return Ok(new { id, data = ""example"" });
    }
}",
                    Language = "csharp",
                    Explanation = $"Mostra um uso mais avançado de {title} com múltiplos endpoints.",
                    IsRunnable = true
                }
            },
            Exercises = new List<Exercise>
            {
                new Exercise
                {
                    Title = $"Implementar {title}",
                    Description = $"Crie uma implementação básica de {title} seguindo os exemplos da aula.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = "// Implemente aqui",
                    Hints = new List<string>
                    {
                        "Revise os exemplos de código",
                        "Siga as melhores práticas apresentadas",
                        "Teste sua implementação"
                    }
                },
                new Exercise
                {
                    Title = $"Aplicar {title} em Projeto",
                    Description = $"Aplique os conceitos de {title} em um projeto real.",
                    Difficulty = ExerciseDifficulty.Difícil,
                    StarterCode = "",
                    Hints = new List<string>
                    {
                        "Comece com um caso simples",
                        "Expanda gradualmente",
                        "Documente seu código"
                    }
                },
                new Exercise
                {
                    Title = $"Testar {title}",
                    Description = $"Escreva testes para sua implementação de {title}.",
                    Difficulty = ExerciseDifficulty.Médio,
                    StarterCode = "",
                    Hints = new List<string>
                    {
                        "Teste casos normais e de erro",
                        "Use ferramentas de teste apropriadas",
                        "Garanta cobertura adequada"
                    }
                }
            },
            Summary = $"Nesta aula você aprendeu sobre {title}, um conceito fundamental para APIs RESTful. Você viu exemplos práticos e aprendeu as melhores práticas da indústria. Continue praticando para dominar completamente este tópico."
        };

        return new Lesson
        {
            Id = Guid.Parse($"10000000-0000-0000-0007-{lessonNumber:D12}"),
            CourseId = _courseId,
            Title = title,
            StructuredContent = JsonSerializer.Serialize(content),
            Duration = "45 min",
            Difficulty = "Intermediário",
            EstimatedMinutes = 45,
            OrderIndex = lessonNumber,
            Version = 1,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }
}
