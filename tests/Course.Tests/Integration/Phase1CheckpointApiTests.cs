using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shared.Data;
using Course.Service.DTOs;
using Xunit;
using Xunit.Abstractions;

namespace Course.Tests.Integration;

/// <summary>
/// Phase 1 Checkpoint - API Integration Tests for Levels 0-3
/// Validates: Requirements 9.1, 9.3, 9.6
/// Tests GET /api/courses returns 4 courses
/// Tests GET /api/courses?levelId={id} filters correctly
/// Tests lesson retrieval for all 80 lessons
/// </summary>
public class Phase1CheckpointApiTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly WebApplicationFactory<Program> _factory;
    private readonly ITestOutputHelper _output;

    public Phase1CheckpointApiTests(WebApplicationFactory<Program> factory, ITestOutputHelper output)
    {
        _output = output;
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
                    options.UseInMemoryDatabase("TestDb_Phase1Checkpoint_" + Guid.NewGuid());
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
    public async Task Phase1_GetAllCourses_Returns4Courses()
    {
        // Act
        var response = await _client.GetAsync("/api/courses");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var result = await response.Content.ReadFromJsonAsync<CourseListResponse>();
        Assert.NotNull(result);
        Assert.Equal(4, result.Courses.Count);

        _output.WriteLine($"✓ GET /api/courses returns {result.Courses.Count} courses");
        
        foreach (var course in result.Courses)
        {
            _output.WriteLine($"  - {course.Title} ({course.LessonCount} lessons)");
        }
    }

    [Fact]
    public async Task Phase1_GetCoursesByLevelId_FiltersCorrectly()
    {
        // Arrange
        var levelIds = new Dictionary<string, Guid>
        {
            { "Level 0", Guid.Parse("00000000-0000-0000-0000-000000000000") },
            { "Level 1", Guid.Parse("00000001-0000-0000-0000-000000000000") },
            { "Level 2", Guid.Parse("00000002-0000-0000-0000-000000000000") },
            { "Level 3", Guid.Parse("00000003-0000-0000-0000-000000000000") }
        };

        var expectedTitles = new Dictionary<string, string>
        {
            { "Level 0", "Fundamentos de Programação" },
            { "Level 1", "Programação Orientada a Objetos" },
            { "Level 2", "Estruturas de Dados e Algoritmos" },
            { "Level 3", "Banco de Dados e SQL" }
        };

        // Act & Assert
        foreach (var (levelName, levelId) in levelIds)
        {
            var response = await _client.GetAsync($"/api/courses?levelId={levelId}");
            
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            
            var result = await response.Content.ReadFromJsonAsync<CourseListResponse>();
            Assert.NotNull(result);
            Assert.Single(result.Courses);
            Assert.Equal(expectedTitles[levelName], result.Courses[0].Title);
            
            _output.WriteLine($"✓ {levelName} filter returns: {result.Courses[0].Title}");
        }
    }

    [Fact]
    public async Task Phase1_AllCourses_Return20LessonsEach()
    {
        // Arrange
        var courseIds = new Dictionary<string, Guid>
        {
            { "Level 0", Guid.Parse("10000000-0000-0000-0000-000000000001") },
            { "Level 1", Guid.Parse("10000000-0000-0000-0000-000000000002") },
            { "Level 2", Guid.Parse("10000000-0000-0000-0000-000000000003") },
            { "Level 3", Guid.Parse("10000000-0000-0000-0000-000000000004") }
        };

        // Act & Assert
        foreach (var (levelName, courseId) in courseIds)
        {
            var response = await _client.GetAsync($"/api/courses/{courseId}/lessons");
            
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            
            var result = await response.Content.ReadFromJsonAsync<LessonListResponse>();
            Assert.NotNull(result);
            Assert.Equal(20, result.Lessons.Count);
            
            _output.WriteLine($"✓ {levelName} course returns {result.Lessons.Count} lessons");
        }
    }

    [Fact]
    public async Task Phase1_All80Lessons_CanBeRetrievedById()
    {
        // Arrange
        var courseLessonPairs = new List<(string level, Guid courseId, Guid[] lessonIds)>
        {
            ("Level 0", 
             Guid.Parse("10000000-0000-0000-0000-000000000001"),
             GenerateLessonIds("10000000-0000-0000-0001-")),
            
            ("Level 1", 
             Guid.Parse("10000000-0000-0000-0000-000000000002"),
             GenerateLessonIds("10000000-0000-0000-0002-")),
            
            ("Level 2", 
             Guid.Parse("10000000-0000-0000-0000-000000000003"),
             GenerateLessonIds("10000000-0000-0000-0003-")),
            
            ("Level 3", 
             Guid.Parse("10000000-0000-0000-0000-000000000004"),
             GenerateLessonIds("20000000-0000-0000-0004-"))
        };

        int totalLessonsRetrieved = 0;
        var failures = new List<string>();

        // Act
        foreach (var (level, courseId, lessonIds) in courseLessonPairs)
        {
            for (int i = 0; i < lessonIds.Length; i++)
            {
                var lessonId = lessonIds[i];
                var response = await _client.GetAsync($"/api/courses/{courseId}/lessons/{lessonId}");

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var result = await response.Content.ReadFromJsonAsync<LessonDetailDto>();
                    
                    if (result != null && result.StructuredContent != null)
                    {
                        totalLessonsRetrieved++;
                    }
                    else
                    {
                        failures.Add($"{level} Lesson {i + 1}: Missing structured content");
                    }
                }
                else
                {
                    failures.Add($"{level} Lesson {i + 1}: HTTP {response.StatusCode}");
                }
            }
            
            _output.WriteLine($"✓ {level}: Retrieved all 20 lessons");
        }

        // Assert
        Assert.Empty(failures);
        Assert.Equal(80, totalLessonsRetrieved);
        
        _output.WriteLine($"\n✓✓✓ ALL 80 LESSONS RETRIEVED SUCCESSFULLY ✓✓✓");
    }

    [Fact]
    public async Task Phase1_AllLessons_HaveStructuredContent()
    {
        // Arrange
        var courseIds = new Dictionary<string, Guid>
        {
            { "Level 0", Guid.Parse("10000000-0000-0000-0000-000000000001") },
            { "Level 1", Guid.Parse("10000000-0000-0000-0000-000000000002") },
            { "Level 2", Guid.Parse("10000000-0000-0000-0000-000000000003") },
            { "Level 3", Guid.Parse("10000000-0000-0000-0000-000000000004") }
        };

        var violations = new List<string>();

        // Act
        foreach (var (levelName, courseId) in courseIds)
        {
            var response = await _client.GetAsync($"/api/courses/{courseId}/lessons");
            var result = await response.Content.ReadFromJsonAsync<LessonListResponse>();

            foreach (var lesson in result.Lessons)
            {
                var detailResponse = await _client.GetAsync($"/api/courses/{courseId}/lessons/{lesson.Id}");
                var detail = await detailResponse.Content.ReadFromJsonAsync<LessonDetailDto>();

                if (detail.StructuredContent == null)
                {
                    violations.Add($"{levelName} - {lesson.Title}: Missing StructuredContent");
                    continue;
                }

                if (detail.StructuredContent.Objectives == null || !detail.StructuredContent.Objectives.Any())
                    violations.Add($"{levelName} - {lesson.Title}: Missing Objectives");

                if (detail.StructuredContent.Theory == null || !detail.StructuredContent.Theory.Any())
                    violations.Add($"{levelName} - {lesson.Title}: Missing Theory");

                if (detail.StructuredContent.CodeExamples == null || !detail.StructuredContent.CodeExamples.Any())
                    violations.Add($"{levelName} - {lesson.Title}: Missing CodeExamples");

                if (detail.StructuredContent.Exercises == null || !detail.StructuredContent.Exercises.Any())
                    violations.Add($"{levelName} - {lesson.Title}: Missing Exercises");
            }
        }

        // Assert
        if (violations.Any())
        {
            _output.WriteLine("Structured content violations:");
            foreach (var v in violations)
            {
                _output.WriteLine($"  - {v}");
            }
        }
        Assert.Empty(violations);

        _output.WriteLine($"✓ All 80 lessons have complete structured content");
    }

    [Fact]
    public async Task Phase1_AllCourses_HaveCorrectMetadata()
    {
        // Act
        var response = await _client.GetAsync("/api/courses");
        var result = await response.Content.ReadFromJsonAsync<CourseListResponse>();

        // Assert
        Assert.NotNull(result);
        
        var expectedCourses = new Dictionary<string, (string title, int lessonCount)>
        {
            { "Level 0", ("Fundamentos de Programação", 20) },
            { "Level 1", ("Programação Orientada a Objetos", 20) },
            { "Level 2", ("Estruturas de Dados e Algoritmos", 20) },
            { "Level 3", ("Banco de Dados e SQL", 20) }
        };

        foreach (var (level, (expectedTitle, expectedCount)) in expectedCourses)
        {
            var course = result.Courses.FirstOrDefault(c => c.Title == expectedTitle);
            Assert.NotNull(course);
            Assert.Equal(expectedCount, course.LessonCount);
            
            _output.WriteLine($"✓ {level}: {course.Title} - {course.LessonCount} lessons");
        }
    }

    // Helper method
    private Guid[] GenerateLessonIds(string prefix)
    {
        var ids = new Guid[20];
        for (int i = 0; i < 20; i++)
        {
            var lessonNum = (i + 1).ToString("X").PadLeft(12, '0');
            ids[i] = Guid.Parse($"{prefix}{lessonNum}");
        }
        return ids;
    }
}
