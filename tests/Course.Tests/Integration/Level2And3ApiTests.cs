using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shared.Data;
using Course.Service.DTOs;
using Xunit;

namespace Course.Tests.Integration;

/// <summary>
/// Integration tests for Level 2 and Level 3 API endpoints
/// Validates: Requirements 4.7, 6.3, 6.6
/// </summary>
public class Level2And3ApiTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly WebApplicationFactory<Program> _factory;

    public Level2And3ApiTests(WebApplicationFactory<Program> factory)
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

                // Add in-memory database
                services.AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseInMemoryDatabase("TestDb_Level2And3_" + Guid.NewGuid());
                });

                // Seed data
                var sp = services.BuildServiceProvider();
                using var scope = sp.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                DbSeeder.SeedData(context);
            });
        });

        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task GetAllCourses_ReturnsLevel2Course()
    {
        // Act
        var response = await _client.GetAsync("/api/courses");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        
        var result = await response.Content.ReadFromJsonAsync<CourseListResponse>();
        Assert.NotNull(result);
        
        var level2Course = result.Courses.FirstOrDefault(c => c.Title == "Estruturas de Dados e Algoritmos");
        Assert.NotNull(level2Course);
        Assert.Equal("Intermediate", level2Course.Level);
        Assert.Equal(20, level2Course.LessonCount);
    }

    [Fact]
    public async Task GetAllCourses_ReturnsLevel3Course()
    {
        // Act
        var response = await _client.GetAsync("/api/courses");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        
        var result = await response.Content.ReadFromJsonAsync<CourseListResponse>();
        Assert.NotNull(result);
        
        var level3Course = result.Courses.FirstOrDefault(c => c.Title == "Banco de Dados e SQL");
        Assert.NotNull(level3Course);
        Assert.Equal("Intermediate", level3Course.Level);
        Assert.Equal(20, level3Course.LessonCount);
    }

    [Fact]
    public async Task GetAllCourses_ReturnsFourCourses()
    {
        // Act
        var response = await _client.GetAsync("/api/courses");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        
        var result = await response.Content.ReadFromJsonAsync<CourseListResponse>();
        Assert.NotNull(result);
        Assert.Equal(4, result.Courses.Count); // Levels 0, 1, 2, 3
    }

    [Fact]
    public async Task GetLevel2Lessons_Returns20Lessons()
    {
        // Arrange
        var level2CourseId = Guid.Parse("10000000-0000-0000-0000-000000000003");

        // Act
        var response = await _client.GetAsync($"/api/courses/{level2CourseId}/lessons");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        
        var result = await response.Content.ReadFromJsonAsync<LessonListResponse>();
        Assert.NotNull(result);
        Assert.Equal(20, result.Lessons.Count);
    }

    [Fact]
    public async Task GetLevel3Lessons_Returns20Lessons()
    {
        // Arrange
        var level3CourseId = Guid.Parse("10000000-0000-0000-0000-000000000004");

        // Act
        var response = await _client.GetAsync($"/api/courses/{level3CourseId}/lessons");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        
        var result = await response.Content.ReadFromJsonAsync<LessonListResponse>();
        Assert.NotNull(result);
        Assert.Equal(20, result.Lessons.Count);
    }

    [Fact]
    public async Task GetLevel2LessonById_ReturnsStructuredContent()
    {
        // Arrange
        var level2CourseId = Guid.Parse("10000000-0000-0000-0000-000000000003");
        var level2Lesson1Id = Guid.Parse("10000000-0000-0000-0003-000000000001");

        // Act
        var response = await _client.GetAsync($"/api/courses/{level2CourseId}/lessons/{level2Lesson1Id}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        
        var result = await response.Content.ReadFromJsonAsync<LessonDetailDto>();
        Assert.NotNull(result);
        Assert.NotNull(result.StructuredContent);
        Assert.NotEmpty(result.StructuredContent.Objectives);
        Assert.NotEmpty(result.StructuredContent.Theory);
        Assert.NotEmpty(result.StructuredContent.CodeExamples);
        Assert.NotEmpty(result.StructuredContent.Exercises);
    }

    [Fact]
    public async Task GetLevel3LessonById_ReturnsStructuredContent()
    {
        // Arrange
        var level3CourseId = Guid.Parse("10000000-0000-0000-0000-000000000004");
        var level3Lesson1Id = Guid.Parse("10000000-0000-0000-0004-000000000001");

        // Act
        var response = await _client.GetAsync($"/api/courses/{level3CourseId}/lessons/{level3Lesson1Id}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        
        var result = await response.Content.ReadFromJsonAsync<LessonDetailDto>();
        Assert.NotNull(result);
        Assert.NotNull(result.StructuredContent);
        Assert.NotEmpty(result.StructuredContent.Objectives);
        Assert.NotEmpty(result.StructuredContent.Theory);
        Assert.NotEmpty(result.StructuredContent.CodeExamples);
        Assert.NotEmpty(result.StructuredContent.Exercises);
    }

    [Fact]
    public async Task GetCoursesByLevelId_ReturnsLevel2Course()
    {
        // Arrange
        var level2Id = Guid.Parse("00000000-0000-0000-0000-000000000002");

        // Act
        var response = await _client.GetAsync($"/api/courses?levelId={level2Id}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        
        var result = await response.Content.ReadFromJsonAsync<CourseListResponse>();
        Assert.NotNull(result);
        Assert.Single(result.Courses);
        Assert.Equal("Estruturas de Dados e Algoritmos", result.Courses[0].Title);
    }

    [Fact]
    public async Task GetCoursesByLevelId_ReturnsLevel3Course()
    {
        // Arrange
        var level3Id = Guid.Parse("00000000-0000-0000-0000-000000000003");

        // Act
        var response = await _client.GetAsync($"/api/courses?levelId={level3Id}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        
        var result = await response.Content.ReadFromJsonAsync<CourseListResponse>();
        Assert.NotNull(result);
        Assert.Single(result.Courses);
        Assert.Equal("Banco de Dados e SQL", result.Courses[0].Title);
    }
}
