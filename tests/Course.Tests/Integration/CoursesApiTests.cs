using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
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
public class CoursesApiTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public CoursesApiTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                // Remove existing DbContext
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                // Add in-memory database for testing
                services.AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseInMemoryDatabase("TestDb_Courses");
                });

                // Seed test data
                var sp = services.BuildServiceProvider();
                using var scope = sp.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                SeedTestData(context);
            });
        });

        _client = _factory.CreateClient();
    }

    private static void SeedTestData(ApplicationDbContext context)
    {
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

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
                ""objectives"": [""Entender variáveis""],
                ""sections"": [
                    {
                        ""type"": ""theory"",
                        ""heading"": ""O que são variáveis?"",
                        ""content"": ""Variáveis armazenam dados"",
                        ""order"": 1
                    }
                ],
                ""summary"": ""Você aprendeu sobre variáveis""
            }",
            Prerequisites = "[\"Nenhum\"]"
        };

        context.Lessons.Add(lesson);
        context.SaveChanges();
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
        using var scope = _factory.Services.CreateScope();
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
        using var scope = _factory.Services.CreateScope();
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
        using var scope = _factory.Services.CreateScope();
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
        using var scope = _factory.Services.CreateScope();
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
        using var scope = _factory.Services.CreateScope();
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
        using var scope = _factory.Services.CreateScope();
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
        using var scope = _factory.Services.CreateScope();
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
        using var scope = _factory.Services.CreateScope();
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
        using var scope = _factory.Services.CreateScope();
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
