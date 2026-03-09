using Shared.Logging;
using Shared.HealthChecks;

var builder = WebApplication.CreateBuilder(args);
builder.ConfigureSerilog("ProjectService");

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Add health checks
builder.Services.AddPlatformHealthChecks(builder.Configuration, "ProjectService");

var app = builder.Build();
app.UseCors("AllowAll");

// Map health check endpoint
app.MapPlatformHealthChecks("/health");

// Mock projects data
app.MapGet("/api/projects", () =>
{
    var projects = new[]
    {
        new
        {
            id = "1",
            title = "API de Lista de Tarefas",
            description = "Construa uma API RESTful para gerenciar itens de tarefas com operações CRUD",
            difficulty = "Iniciante",
            estimatedTime = "4 horas",
            points = 200,
            category = "API Web",
            technologies = new[] { "ASP.NET Core", "Entity Framework", "SQL Server" },
            completed = false
        },
        new
        {
            id = "2",
            title = "Backend de E-Commerce",
            description = "Crie um sistema backend completo para uma plataforma de e-commerce com produtos, pedidos e pagamentos",
            difficulty = "Intermediário",
            estimatedTime = "12 horas",
            points = 500,
            category = "Full Stack",
            technologies = new[] { "ASP.NET Core", "PostgreSQL", "Redis", "RabbitMQ" },
            completed = false
        },
        new
        {
            id = "3",
            title = "Aplicação de Chat em Tempo Real",
            description = "Construa um sistema de chat em tempo real usando SignalR com salas e mensagens privadas",
            difficulty = "Intermediário",
            estimatedTime = "8 horas",
            points = 400,
            category = "Tempo Real",
            technologies = new[] { "SignalR", "ASP.NET Core", "MongoDB" },
            completed = false
        },
        new
        {
            id = "4",
            title = "Plataforma de Microsserviços",
            description = "Projete e implemente uma arquitetura de microsserviços com API Gateway, descoberta de serviços e rastreamento distribuído",
            difficulty = "Avançado",
            estimatedTime = "20 horas",
            points = 1000,
            category = "Arquitetura",
            technologies = new[] { "Docker", "Kubernetes", "RabbitMQ", "Redis", "PostgreSQL" },
            completed = false
        }
    };
    
    return Results.Ok(new { projects });
});

app.MapGet("/api/projects/{id}", (string id) =>
{
    var projects = new Dictionary<string, object>
    {
        ["1"] = new
        {
            id = "1",
            title = "API de Lista de Tarefas",
            description = "Construa uma API RESTful para gerenciar itens de tarefas com operações CRUD completas. Implemente autenticação, validação e tratamento adequado de erros.",
            difficulty = "Iniciante",
            estimatedTime = "4 horas",
            points = 200,
            category = "API Web",
            technologies = new[] { "ASP.NET Core", "Entity Framework", "SQL Server" },
            completed = false,
            currentStep = 1,
            steps = new[]
            {
                new
                {
                    stepNumber = 1,
                    title = "Criar o Modelo Todo",
                    instructions = "Crie uma classe TodoItem com as propriedades: Id (int), Title (string), Description (string), IsCompleted (bool), CreatedAt (DateTime).",
                    validationCriteria = "O modelo deve ter todas as propriedades necessárias com os tipos de dados corretos.",
                    starterCode = "using System;\n\nnamespace TodoApi.Models\n{\n    public class TodoItem\n    {\n        // Adicione as propriedades aqui\n    }\n}",
                    isCompleted = false
                },
                new
                {
                    stepNumber = 2,
                    title = "Criar o DbContext",
                    instructions = "Crie uma classe TodoContext que herda de DbContext. Adicione uma propriedade DbSet<TodoItem> chamada TodoItems.",
                    validationCriteria = "O DbContext deve estar configurado corretamente com o DbSet TodoItems.",
                    starterCode = "using Microsoft.EntityFrameworkCore;\nusing TodoApi.Models;\n\nnamespace TodoApi.Data\n{\n    public class TodoContext : DbContext\n    {\n        // Adicione o DbSet aqui\n    }\n}",
                    isCompleted = false
                },
                new
                {
                    stepNumber = 3,
                    title = "Criar o Controller",
                    instructions = "Crie um TodoController com endpoints GET, POST, PUT e DELETE para gerenciar itens de tarefas.",
                    validationCriteria = "Todos os endpoints CRUD devem estar implementados corretamente.",
                    starterCode = "using Microsoft.AspNetCore.Mvc;\nusing TodoApi.Models;\n\nnamespace TodoApi.Controllers\n{\n    [ApiController]\n    [Route(\"api/[controller]\")]\n    public class TodoController : ControllerBase\n    {\n        // Adicione os endpoints aqui\n    }\n}",
                    isCompleted = false
                }
            }
        }
    };

    if (projects.TryGetValue(id, out var project))
    {
        return Results.Ok(project);
    }
    return Results.NotFound(new { message = "Project not found" });
});

app.MapPost("/api/projects/{id}/steps/{stepNumber}/validate", (string id, int stepNumber, HttpContext context) =>
{
    // Read the request body
    using var reader = new StreamReader(context.Request.Body);
    var body = reader.ReadToEndAsync().Result;
    var data = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(body);
    
    var code = data?.ContainsKey("code") == true ? data["code"].ToString() : "";
    
    // Validation logic based on step number
    bool success = false;
    string message = "";
    
    if (stepNumber == 1)
    {
        // Step 1: Validate Todo Model
        var requiredProperties = new[] { "Id", "Title", "Description", "IsCompleted", "CreatedAt" };
        var hasAllProperties = requiredProperties.All(prop => code?.Contains($"{prop} ") == true || code?.Contains($"{prop}{{") == true);
        
        success = hasAllProperties && code.Contains("public class TodoItem");
        message = success 
            ? "Excelente! Seu modelo TodoItem está correto com todas as propriedades necessárias. Você pode prosseguir para o próximo passo." 
            : "Seu código ainda não atende todos os critérios. Certifique-se de que a classe TodoItem tem todas as propriedades: Id (int), Title (string), Description (string), IsCompleted (bool), CreatedAt (DateTime).";
    }
    else if (stepNumber == 2)
    {
        // Step 2: Validate DbContext
        success = code.Contains("DbContext") && code.Contains("DbSet<TodoItem>") && code.Contains("TodoItems");
        message = success 
            ? "Perfeito! Seu DbContext está configurado corretamente. Avance para o próximo passo." 
            : "Seu DbContext precisa herdar de DbContext e ter uma propriedade DbSet<TodoItem> chamada TodoItems.";
    }
    else if (stepNumber == 3)
    {
        // Step 3: Validate Controller
        success = code.Contains("[ApiController]") && code.Contains("TodoController") && code.Contains("ControllerBase");
        message = success 
            ? "Ótimo trabalho! Seu controller está estruturado corretamente. Projeto concluído!" 
            : "Seu controller precisa ter o atributo [ApiController] e herdar de ControllerBase.";
    }
    else
    {
        success = false;
        message = "Passo inválido.";
    }
    
    return Results.Ok(new
    {
        success = success,
        message = message,
        nextStepUnlocked = success
    });
});

app.Run();
