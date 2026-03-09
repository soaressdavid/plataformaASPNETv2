using Xunit;
using Shared.Data;
using Shared.Services;
using System.Text.Json;
using Shared.Models;

namespace Shared.Tests;

public class Level0ContentValidationTests
{
    private readonly LessonValidator _validator;
    private readonly Level0ContentSeeder _seeder;

    public Level0ContentValidationTests()
    {
        _validator = new LessonValidator();
        _seeder = new Level0ContentSeeder();
    }

    [Fact]
    public void Level0Course_ShouldBeValid()
    {
        // Arrange
        var course = _seeder.CreateLevel0Course();

        // Assert
        Assert.NotNull(course);
        Assert.Equal("Fundamentos de Programação", course.Title);
        Assert.Equal(20, course.LessonCount);
    }

    [Fact]
    public void Level0Lessons_ShouldHave20Lessons()
    {
        // Arrange & Act
        var lessons = _seeder.CreateLevel0Lessons();

        // Assert
        Assert.NotNull(lessons);
        Assert.Equal(20, lessons.Count);
    }

    [Fact]
    public void AllLevel0Lessons_ShouldHaveValidStructuredContent()
    {
        // Arrange
        var lessons = _seeder.CreateLevel0Lessons();

        // Act & Assert
        foreach (var lesson in lessons)
        {
            Assert.NotNull(lesson.StructuredContent);
            Assert.NotEmpty(lesson.StructuredContent);
            
            // Should be valid JSON
            var content = JsonSerializer.Deserialize<LessonContent>(lesson.StructuredContent);
            Assert.NotNull(content);
        }
    }

    [Fact]
    public void FirstLesson_ShouldPassValidation()
    {
        // Arrange
        var lessons = _seeder.CreateLevel0Lessons();
        var firstLesson = lessons[0];
        var content = JsonSerializer.Deserialize<LessonContent>(firstLesson.StructuredContent!);

        // Act
        var result = _validator.Validate(content!);

        // Assert
        Assert.True(result.IsValid, $"Validation failed: {string.Join(", ", result.Errors)}");
    }
}
