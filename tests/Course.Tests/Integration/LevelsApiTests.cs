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
/// Integration tests for Levels API endpoints
/// </summary>
public class LevelsApiTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public LevelsApiTests(WebApplicationFactory<Program> factory)
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
                    options.UseInMemoryDatabase("TestDb_Levels");
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
            Description = "Conceitos básicos de programação",
            RequiredXP = 0
        };

        var level1 = new CurriculumLevel
        {
            Id = Guid.NewGuid(),
            Number = 1,
            Title = "ASP.NET Core Básico",
            Description = "Introdução ao ASP.NET Core",
            RequiredXP = 100
        };

        context.CurriculumLevels.AddRange(level0, level1);

        var course1 = new Shared.Entities.Course
        {
            Id = Guid.NewGuid(),
            Title = "Introdução ao C#",
            Description = "Aprenda os fundamentos do C#",
            Level = Shared.Entities.Level.Beginner,
            LevelId = level0.Id,
            OrderIndex = 1,
            Duration = "10 horas",
            Topics = "[\"Variáveis\",\"Tipos\",\"Operadores\"]"
        };

        var course2 = new Shared.Entities.Course
        {
            Id = Guid.NewGuid(),
            Title = "Web APIs com ASP.NET Core",
            Description = "Construa APIs RESTful",
            Level = Shared.Entities.Level.Intermediate,
            LevelId = level1.Id,
            OrderIndex = 1,
            Duration = "15 horas",
            Topics = "[\"REST\",\"Controllers\",\"Routing\"]"
        };

        context.Courses.AddRange(course1, course2);
        context.SaveChanges();
    }

    [Fact]
    public async Task GetAllLevels_ReturnsOkWithLevels()
    {
        // Act
        var response = await _client.GetAsync("/api/levels");

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var result = await response.Content.ReadFromJsonAsync<LevelListResponse>();
        Assert.NotNull(result);
        Assert.NotNull(result.Levels);
        Assert.Equal(2, result.Levels.Count);
        Assert.Equal(0, result.Levels[0].Number);
        Assert.Equal(1, result.Levels[1].Number);
    }

    [Fact]
    public async Task GetLevelById_ValidId_ReturnsOkWithLevelDetail()
    {
        // Arrange
        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var level = context.CurriculumLevels.First();

        // Act
        var response = await _client.GetAsync($"/api/levels/{level.Id}");

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var result = await response.Content.ReadFromJsonAsync<LevelDetailDto>();
        Assert.NotNull(result);
        Assert.Equal(level.Id, result.Id);
        Assert.Equal(level.Title, result.Title);
        Assert.NotNull(result.Courses);
        Assert.Single(result.Courses);
    }

    [Fact]
    public async Task GetLevelById_InvalidId_ReturnsNotFound()
    {
        // Arrange
        var invalidId = Guid.NewGuid();

        // Act
        var response = await _client.GetAsync($"/api/levels/{invalidId}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetLevelCourses_ValidId_ReturnsOkWithCourses()
    {
        // Arrange
        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var level = context.CurriculumLevels.First();

        // Act
        var response = await _client.GetAsync($"/api/levels/{level.Id}/courses");

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var result = await response.Content.ReadFromJsonAsync<CourseListResponse>();
        Assert.NotNull(result);
        Assert.NotNull(result.Courses);
        Assert.Single(result.Courses);
    }

    [Fact]
    public async Task GetLevelCourses_InvalidId_ReturnsNotFound()
    {
        // Arrange
        var invalidId = Guid.NewGuid();

        // Act
        var response = await _client.GetAsync($"/api/levels/{invalidId}/courses");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetAllLevels_ReturnsLevelsOrderedByNumber()
    {
        // Act
        var response = await _client.GetAsync("/api/levels");
        var result = await response.Content.ReadFromJsonAsync<LevelListResponse>();

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Levels);
        for (int i = 0; i < result.Levels.Count - 1; i++)
        {
            Assert.True(result.Levels[i].Number < result.Levels[i + 1].Number);
        }
    }

    [Fact]
    public async Task GetLevelById_IncludesCoursesOrderedByOrderIndex()
    {
        // Arrange
        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var level = context.CurriculumLevels.First();

        // Act
        var response = await _client.GetAsync($"/api/levels/{level.Id}");
        var result = await response.Content.ReadFromJsonAsync<LevelDetailDto>();

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Courses);
        for (int i = 0; i < result.Courses.Count - 1; i++)
        {
            Assert.True(result.Courses[i].OrderIndex <= result.Courses[i + 1].OrderIndex);
        }
    }
}
