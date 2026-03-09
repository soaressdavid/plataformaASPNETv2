using Shared.Logging;
using Shared.Metrics;
using Shared.HealthChecks;

var builder = WebApplication.CreateBuilder(args);
builder.ConfigureSerilog("ChallengeService");

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
builder.Services.AddPlatformHealthChecks(builder.Configuration, "ChallengeService");

var app = builder.Build();
app.UseCors("AllowAll");

// Add Prometheus metrics
app.UsePrometheusMetrics();

// Map health check endpoint
app.MapPlatformHealthChecks("/health");

// Mock challenges data
app.MapGet("/api/challenges", () =>
{
    var challenges = new[]
    {
        new
        {
            id = "1",
            title = "Desafio FizzBuzz",
            description = "Escreva um programa que imprime números de 1 a 100, mas para múltiplos de 3 imprima Fizz, para múltiplos de 5 imprima Buzz",
            difficulty = "Fácil",
            points = 50,
            completedCount = 3420,
            category = "Lógica",
            timeLimit = "30 min",
            completed = false
        },
        new
        {
            id = "2",
            title = "Verificador de Palíndromo",
            description = "Crie uma função que verifica se uma string é um palíndromo",
            difficulty = "Fácil",
            points = 75,
            completedCount = 2890,
            category = "Strings",
            timeLimit = "20 min",
            completed = false
        },
        new
        {
            id = "3",
            title = "Árvore Binária de Busca",
            description = "Implemente uma árvore binária de busca com operações de inserção, busca e exclusão",
            difficulty = "Médio",
            points = 150,
            completedCount = 1250,
            category = "Estruturas de Dados",
            timeLimit = "60 min",
            completed = false
        },
        new
        {
            id = "4",
            title = "Limitador de Taxa de API",
            description = "Projete e implemente um sistema de limitação de taxa para uma API",
            difficulty = "Difícil",
            points = 300,
            completedCount = 450,
            category = "Design de Sistemas",
            timeLimit = "90 min",
            completed = false
        }
    };
    
    return Results.Ok(new { challenges });
});

app.MapGet("/api/challenges/{id}", (string id) =>
{
    var challenges = new Dictionary<string, object>
    {
        ["1"] = new
        {
            id = "1",
            title = "Desafio FizzBuzz",
            description = "Escreva um programa que imprime números de 1 a 100. Para múltiplos de 3, imprima 'Fizz' ao invés do número. Para múltiplos de 5, imprima 'Buzz'. Para números que são múltiplos de 3 e 5, imprima 'FizzBuzz'.",
            difficulty = "Fácil",
            points = 50,
            completedCount = 3420,
            category = "Lógica",
            timeLimit = "30 min",
            completed = false,
            supportsTimeAttack = true,
            timeAttackLimitSeconds = 900,
            starterCode = "public class Solution\n{\n    public void FizzBuzz(int n)\n    {\n        // Seu código aqui\n    }\n}",
            testCases = new[]
            {
                new { input = "15", expectedOutput = "1, 2, Fizz, 4, Buzz, Fizz, 7, 8, Fizz, Buzz, 11, Fizz, 13, 14, FizzBuzz" },
                new { input = "5", expectedOutput = "1, 2, Fizz, 4, Buzz" }
            }
        },
        ["2"] = new
        {
            id = "2",
            title = "Verificador de Palíndromo",
            description = "Crie uma função que verifica se uma string é um palíndromo (lê-se igual de frente para trás e de trás para frente). Ignore espaços e maiúsculas/minúsculas.",
            difficulty = "Fácil",
            points = 75,
            completedCount = 2890,
            category = "Strings",
            timeLimit = "20 min",
            completed = false,
            supportsTimeAttack = true,
            timeAttackLimitSeconds = 900,
            starterCode = "public class Solution\n{\n    public bool IsPalindrome(string s)\n    {\n        // Seu código aqui\n        return false;\n    }\n}",
            testCases = new[]
            {
                new { input = "racecar", expectedOutput = "true" },
                new { input = "hello", expectedOutput = "false" }
            }
        }
    };

    if (challenges.TryGetValue(id, out var challenge))
    {
        return Results.Ok(challenge);
    }
    return Results.NotFound(new { message = "Challenge not found" });
});

// Time Attack leaderboard endpoint
app.MapGet("/api/challenges/{id}/leaderboard", (string id) =>
{
    // Mock leaderboard data
    var leaderboard = new[]
    {
        new { rank = 1, userName = "CodeMaster", completionTimeSeconds = 180, bonusXP = 50, submittedAt = DateTime.UtcNow.AddHours(-2) },
        new { rank = 2, userName = "SpeedCoder", completionTimeSeconds = 240, bonusXP = 30, submittedAt = DateTime.UtcNow.AddHours(-5) },
        new { rank = 3, userName = "QuickDev", completionTimeSeconds = 290, bonusXP = 10, submittedAt = DateTime.UtcNow.AddHours(-8) },
    };
    
    return Results.Ok(new { leaderboard });
});

// User's best time endpoint
app.MapGet("/api/challenges/{id}/best-time", (string id, string userId) =>
{
    // Mock best time data
    var bestTime = new
    {
        completionTimeSeconds = 250,
        bonusXP = 30,
        submittedAt = DateTime.UtcNow.AddDays(-1),
        rank = 15
    };
    
    return Results.Ok(bestTime);
});

// Code Review submission endpoint
app.MapPost("/api/challenges/{id}/code-review", (string id, Shared.DTOs.SubmitCodeReviewRequest request) =>
{
    // Mock challenge data with code review
    var mockChallenge = new Shared.Entities.Challenge
    {
        Id = Guid.TryParse(id, out var guid) ? guid : Guid.NewGuid(),
        Title = "Code Review Challenge",
        Difficulty = Shared.Entities.Difficulty.Medium,
        IsCodeReviewChallenge = true,
        ExpectedIssues = System.Text.Json.JsonSerializer.Serialize(new[]
        {
            new Shared.DTOs.ExpectedBug
            {
                LineNumber = 5,
                Description = "Null reference exception - missing null check",
                Category = "NullReference",
                Severity = "High"
            },
            new Shared.DTOs.ExpectedBug
            {
                LineNumber = 12,
                Description = "Off-by-one error in loop condition",
                Category = "LogicError",
                Severity = "Medium"
            },
            new Shared.DTOs.ExpectedBug
            {
                LineNumber = 18,
                Description = "SQL injection vulnerability",
                Category = "SecurityIssue",
                Severity = "High"
            }
        })
    };

    var validationService = new Challenge.Services.CodeReviewValidationService();
    var result = validationService.ValidateCodeReview(mockChallenge, request.IdentifiedIssues);
    
    return Results.Ok(result);
});

app.Run();
