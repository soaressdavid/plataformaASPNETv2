using System.Net;
using System.Net.Http.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Course.Service.DTOs;
using Shared.Data;
using Shared.Entities;

namespace Course.Tests.Integration;

/// <summary>
/// Integration tests for Courses API endpoints
/// </summary>
public class CoursesApiTests : IClassFixture<TestWebApplicationFactory>
{
    private readonly TestWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public CoursesApiTests(TestWebApplicationFactory factory)
    {
        _factory = factory;
        _factory.EnsureSeeded(SeedTestData);
        _client = _factory.CreateClient();
    }

    private static void SeedTestData(ApplicationDbContext context)
    {
        var level0 = new CurriculumLevel
        {
            Id = Guid.NewGuid(),
            Number = 0,
            Title = "Fundamentos",
            Description = "Conceitos básicos",
            RequiredXP = 0
        };

        context.CurriculumLevels.Add(level0);

        var course = new Shared.Entities.Course
        {
            Id = Guid.NewGuid(),
            Title = "Introdução ao C#",
            Description = "Aprenda C#",
            Level = Shared.Entities.Level.Beginner,
            LevelId = level0.Id,
            OrderIndex = 1,
            Duration = "10 horas",
            Topics = "[\"Variáveis\",\"Tipos\"]"
        };

        context.Courses.Add(course);

        var lesson = new Lesson
        {
            Id = Guid.NewGuid(),
            CourseId = course.Id,
            Title = "Variáveis e Tipos",
            OrderIndex = 1,
            Duration = "30 minutos",
            Difficulty = "Fácil",
            EstimatedMinutes = 30,
            Content = "<h1>Variáveis</h1><p>Conteúdo da aula</p>",
            StructuredContent = @"{
                ""Objectives"": [""Entender variáveis"", ""Aprender tipos de dados"", ""Praticar declarações""],
                ""Theory"": [
                    {
                        ""Heading"": ""O que são variáveis?"",
                        ""Content"": ""Variáveis são espaços na memória que armazenam dados. Em C#, toda variável tem um tipo que define que tipo de dado ela pode armazenar. Por exemplo, int para números inteiros, string para texto, bool para valores verdadeiro/falso. A declaração de uma variável segue o padrão: tipo nome = valor. É importante escolher nomes descritivos para suas variáveis, facilitando a leitura e manutenção do código."",
                        ""Order"": 1
                    },
                    {
                        ""Heading"": ""Tipos de Dados Primitivos"",
                        ""Content"": ""C# possui diversos tipos primitivos: int (números inteiros), double (números decimais), bool (verdadeiro/falso), char (caractere único), string (texto). Cada tipo tem um tamanho específico na memória e um intervalo de valores possíveis. Por exemplo, int pode armazenar valores de -2.147.483.648 a 2.147.483.647. Escolher o tipo correto é importante para otimizar o uso de memória."",
                        ""Order"": 2
                    }
                ],
                ""CodeExamples"": [
                    {
                        ""Title"": ""Declaração de Variáveis"",
                        ""Code"": ""int idade = 25;\nstring nome = \""João\"";\nbool ativo = true;\ndouble salario = 3500.50;"",
                        ""Language"": ""csharp"",
                        ""Explanation"": ""Exemplos de declaração de variáveis com diferentes tipos"",
                        ""IsRunnable"": true
                    },
                    {
                        ""Title"": ""Operações com Variáveis"",
                        ""Code"": ""int a = 10;\nint b = 20;\nint soma = a + b;\nConsole.WriteLine(soma);"",
                        ""Language"": ""csharp"",
                        ""Explanation"": ""Realizando operações matemáticas com variáveis"",
                        ""IsRunnable"": true
                    }
                ],
                ""Exercises"": [
                    {
                        ""Title"": ""Declarar Variáveis"",
                        ""Description"": ""Declare três variáveis: uma para idade (int), uma para nome (string) e uma para ativo (bool)"",
                        ""Difficulty"": 0,
                        ""StarterCode"": ""// Declare suas variáveis aqui"",
                        ""Hints"": [""Use int para idade"", ""Use string para nome"", ""Use bool para ativo""]
                    },
                    {
                        ""Title"": ""Calcular Soma"",
                        ""Description"": ""Crie duas variáveis numéricas e calcule a soma delas"",
                        ""Difficulty"": 0,
                        ""StarterCode"": ""int numero1 = 10;\nint numero2 = 20;\n// Calcule a soma"",
                        ""Hints"": [""Use o operador +"", ""Armazene o resultado em uma nova variável""]
                    },
                    {
                        ""Title"": ""Concatenar Strings"",
                        ""Description"": ""Crie duas variáveis string e concatene-as"",
                        ""Difficulty"": 0,
                        ""StarterCode"": ""string primeiro = \""Olá\"";\nstring segundo = \""Mundo\"";\n// Concatene as strings"",
                        ""Hints"": [""Use o operador +"", ""Não esqueça do espaço entre as palavras""]
                    }
                ],
                ""Summary"": ""Você aprendeu sobre variáveis e tipos de dados em C#""
            }",
            Prerequisites = "[\"Nenhum\"]"
        };

        context.Lessons.Add(lesson);
    }

    [Fact]
    public async Task GetAllCourses_ReturnsOkWithCourses()
    {
        // Act
        var response = await _client.GetAsync("/api/courses");

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var result = await response.Content.ReadFromJsonAsync<CourseListResponse>();
        Assert.NotNull(result);
        Assert.NotNull(result.Courses);
        Assert.Single(result.Courses);
    }

    [Fact]
    public async Task GetAllCourses_WithLevelIdFilter_ReturnsFilteredCourses()
    {
        // Arrange
        using var scope = _factory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var level = context.CurriculumLevels.First();

        // Act
        var response = await _client.GetAsync($"/api/courses?levelId={level.Id}");

        // Assert
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<CourseListResponse>();
        Assert.NotNull(result);
        Assert.NotNull(result.Courses);
        Assert.All(result.Courses, c => Assert.Equal(level.Id, c.LevelId));
    }

    [Fact]
    public async Task GetAllCourses_WithLevelFilter_ReturnsFilteredCourses()
    {
        // Act
        var response = await _client.GetAsync("/api/courses?level=Fácil");

        // Assert
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<CourseListResponse>();
        Assert.NotNull(result);
        Assert.NotNull(result.Courses);
        Assert.All(result.Courses, c => Assert.Equal("Fácil", c.Level));
    }

    [Fact]
    public async Task GetCourseById_ValidId_ReturnsOkWithCourseDetail()
    {
        // Arrange
        using var scope = _factory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var course = context.Courses.First();

        // Act
        var response = await _client.GetAsync($"/api/courses/{course.Id}");

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var result = await response.Content.ReadFromJsonAsync<CourseDetailDto>();
        Assert.NotNull(result);
        Assert.Equal(course.Id, result.Id);
        Assert.Equal(course.Title, result.Title);
        Assert.NotNull(result.Lessons);
        Assert.Single(result.Lessons);
    }

    [Fact]
    public async Task GetCourseById_InvalidId_ReturnsNotFound()
    {
        // Arrange
        var invalidId = Guid.NewGuid();

        // Act
        var response = await _client.GetAsync($"/api/courses/{invalidId}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetCourseLessons_ValidId_ReturnsOkWithLessons()
    {
        // Arrange
        using var scope = _factory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var course = context.Courses.First();

        // Act
        var response = await _client.GetAsync($"/api/courses/{course.Id}/lessons");

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var result = await response.Content.ReadFromJsonAsync<LessonListResponse>();
        Assert.NotNull(result);
        Assert.NotNull(result.Lessons);
        Assert.Single(result.Lessons);
    }

    [Fact]
    public async Task GetCourseLessons_InvalidCourseId_ReturnsNotFound()
    {
        // Arrange
        var invalidId = Guid.NewGuid();

        // Act
        var response = await _client.GetAsync($"/api/courses/{invalidId}/lessons");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetLesson_ValidIds_ReturnsOkWithLessonDetail()
    {
        // Arrange
        using var scope = _factory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var lesson = context.Lessons.Include(l => l.Course).First();

        // Act
        var response = await _client.GetAsync($"/api/courses/{lesson.CourseId}/lessons/{lesson.Id}");

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var result = await response.Content.ReadFromJsonAsync<LessonDetailDto>();
        Assert.NotNull(result);
        Assert.Equal(lesson.Id, result.Id);
        Assert.Equal(lesson.Title, result.Title);
        Assert.NotNull(result.StructuredContent);
        Assert.NotEmpty(result.Prerequisites);
    }

    [Fact]
    public async Task GetLesson_InvalidLessonId_ReturnsNotFound()
    {
        // Arrange
        using var scope = _factory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var course = context.Courses.First();
        var invalidLessonId = Guid.NewGuid();

        // Act
        var response = await _client.GetAsync($"/api/courses/{course.Id}/lessons/{invalidLessonId}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task CompleteLesson_ValidIds_ReturnsOk()
    {
        // Arrange
        using var scope = _factory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var lesson = context.Lessons.First();

        // Act
        var response = await _client.PostAsync(
            $"/api/courses/{lesson.CourseId}/lessons/{lesson.Id}/complete",
            null);

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task CompleteLesson_InvalidLessonId_ReturnsNotFound()
    {
        // Arrange
        using var scope = _factory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var course = context.Courses.First();
        var invalidLessonId = Guid.NewGuid();

        // Act
        var response = await _client.PostAsync(
            $"/api/courses/{course.Id}/lessons/{invalidLessonId}/complete",
            null);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetAllCourses_ReturnsCoursesOrderedByOrderIndex()
    {
        // Act
        var response = await _client.GetAsync("/api/courses");
        var result = await response.Content.ReadFromJsonAsync<CourseListResponse>();

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Courses);
        for (int i = 0; i < result.Courses.Count - 1; i++)
        {
            Assert.True(result.Courses[i].OrderIndex <= result.Courses[i + 1].OrderIndex);
        }
    }

    [Fact]
    public async Task GetCourseLessons_ReturnsLessonsOrderedByOrderIndex()
    {
        // Arrange
        using var scope = _factory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var course = context.Courses.First();

        // Act
        var response = await _client.GetAsync($"/api/courses/{course.Id}/lessons");
        var result = await response.Content.ReadFromJsonAsync<LessonListResponse>();

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Lessons);
        for (int i = 0; i < result.Lessons.Count - 1; i++)
        {
            Assert.True(result.Lessons[i].Order <= result.Lessons[i + 1].Order);
        }
    }

    [Fact]
    public async Task GetLesson_StructuredContentParsedCorrectly()
    {
        // Arrange
        using var scope = _factory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var lesson = context.Lessons.First();

        // Act
        var response = await _client.GetAsync($"/api/courses/{lesson.CourseId}/lessons/{lesson.Id}");
        var result = await response.Content.ReadFromJsonAsync<LessonDetailDto>();

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.StructuredContent);
        Assert.NotNull(result.StructuredContent.Objectives);
        Assert.NotEmpty(result.StructuredContent.Objectives);
        Assert.NotNull(result.StructuredContent.Theory);
        Assert.NotEmpty(result.StructuredContent.Theory);
    }
}


