using Shared.Entities;
using Shared.Repositories;
using Xunit;

namespace Shared.Tests;

public class InMemoryLessonRepositoryTests
{
    private InMemoryLessonRepository CreateRepository()
    {
        return new InMemoryLessonRepository();
    }

    private Lesson CreateTestLesson(Guid? id = null, Guid? courseId = null, string title = "Test Lesson", int orderIndex = 0)
    {
        return new Lesson
        {
            Id = id ?? Guid.NewGuid(),
            CourseId = courseId ?? Guid.NewGuid(),
            Title = title,
            Content = "Test content",
            Duration = "45 min",
            Difficulty = "Iniciante",
            EstimatedMinutes = 45,
            OrderIndex = orderIndex,
            Version = 1
        };
    }

    [Fact]
    public async Task CreateAsync_ShouldAddLesson_WhenValidLessonProvided()
    {
        // Arrange
        var repository = CreateRepository();
        var lesson = CreateTestLesson();

        // Act
        var result = await repository.CreateAsync(lesson);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(lesson.Id, result.Id);
        Assert.Equal(lesson.Title, result.Title);
        Assert.NotEqual(DateTime.MinValue, result.CreatedAt);
        Assert.NotEqual(DateTime.MinValue, result.UpdatedAt);
    }

    [Fact]
    public async Task CreateAsync_ShouldGenerateId_WhenIdIsEmpty()
    {
        // Arrange
        var repository = CreateRepository();
        var lesson = CreateTestLesson(id: Guid.Empty);

        // Act
        var result = await repository.CreateAsync(lesson);

        // Assert
        Assert.NotEqual(Guid.Empty, result.Id);
    }

    [Fact]
    public async Task CreateAsync_ShouldThrowException_WhenLessonIsNull()
    {
        // Arrange
        var repository = CreateRepository();

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => repository.CreateAsync(null!));
    }

    [Fact]
    public async Task CreateAsync_ShouldThrowException_WhenDuplicateIdExists()
    {
        // Arrange
        var repository = CreateRepository();
        var lessonId = Guid.NewGuid();
        var lesson1 = CreateTestLesson(id: lessonId);
        var lesson2 = CreateTestLesson(id: lessonId);

        // Act
        await repository.CreateAsync(lesson1);

        // Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => repository.CreateAsync(lesson2));
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnLesson_WhenLessonExists()
    {
        // Arrange
        var repository = CreateRepository();
        var lesson = CreateTestLesson();
        await repository.CreateAsync(lesson);

        // Act
        var result = await repository.GetByIdAsync(lesson.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(lesson.Id, result.Id);
        Assert.Equal(lesson.Title, result.Title);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenLessonDoesNotExist()
    {
        // Arrange
        var repository = CreateRepository();
        var nonExistentId = Guid.NewGuid();

        // Act
        var result = await repository.GetByIdAsync(nonExistentId);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllLessons()
    {
        // Arrange
        var repository = CreateRepository();
        var lesson1 = CreateTestLesson(title: "Lesson 1");
        var lesson2 = CreateTestLesson(title: "Lesson 2");
        var lesson3 = CreateTestLesson(title: "Lesson 3");

        await repository.CreateAsync(lesson1);
        await repository.CreateAsync(lesson2);
        await repository.CreateAsync(lesson3);

        // Act
        var result = await repository.GetAllAsync();

        // Assert
        Assert.Equal(3, result.Count());
        Assert.Contains(result, l => l.Id == lesson1.Id);
        Assert.Contains(result, l => l.Id == lesson2.Id);
        Assert.Contains(result, l => l.Id == lesson3.Id);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnEmptyList_WhenNoLessonsExist()
    {
        // Arrange
        var repository = CreateRepository();

        // Act
        var result = await repository.GetAllAsync();

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetByCourseIdAsync_ShouldReturnLessonsForCourse_OrderedByOrderIndex()
    {
        // Arrange
        var repository = CreateRepository();
        var courseId = Guid.NewGuid();
        var otherCourseId = Guid.NewGuid();

        var lesson1 = CreateTestLesson(courseId: courseId, title: "Lesson 1", orderIndex: 2);
        var lesson2 = CreateTestLesson(courseId: courseId, title: "Lesson 2", orderIndex: 1);
        var lesson3 = CreateTestLesson(courseId: courseId, title: "Lesson 3", orderIndex: 3);
        var lesson4 = CreateTestLesson(courseId: otherCourseId, title: "Other Course Lesson", orderIndex: 1);

        await repository.CreateAsync(lesson1);
        await repository.CreateAsync(lesson2);
        await repository.CreateAsync(lesson3);
        await repository.CreateAsync(lesson4);

        // Act
        var result = await repository.GetByCourseIdAsync(courseId);

        // Assert
        Assert.Equal(3, result.Count);
        Assert.Equal(lesson2.Id, result[0].Id); // OrderIndex 1
        Assert.Equal(lesson1.Id, result[1].Id); // OrderIndex 2
        Assert.Equal(lesson3.Id, result[2].Id); // OrderIndex 3
    }

    [Fact]
    public async Task GetByCourseIdAsync_ShouldReturnEmptyList_WhenNoCourseMatchesId()
    {
        // Arrange
        var repository = CreateRepository();
        var courseId = Guid.NewGuid();
        var lesson = CreateTestLesson(courseId: Guid.NewGuid());
        await repository.CreateAsync(lesson);

        // Act
        var result = await repository.GetByCourseIdAsync(courseId);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task SearchAsync_ShouldReturnLessons_WhenTitleMatches()
    {
        // Arrange
        var repository = CreateRepository();
        var lesson1 = CreateTestLesson(title: "Introduction to C#");
        var lesson2 = CreateTestLesson(title: "Advanced C# Concepts");
        var lesson3 = CreateTestLesson(title: "Python Basics");

        await repository.CreateAsync(lesson1);
        await repository.CreateAsync(lesson2);
        await repository.CreateAsync(lesson3);

        // Act
        var result = await repository.SearchAsync("C#");

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Contains(result, l => l.Id == lesson1.Id);
        Assert.Contains(result, l => l.Id == lesson2.Id);
    }

    [Fact]
    public async Task SearchAsync_ShouldReturnLessons_WhenStructuredContentMatches()
    {
        // Arrange
        var repository = CreateRepository();
        var lesson1 = CreateTestLesson(title: "Lesson 1");
        lesson1.StructuredContent = "{\"objectives\": [\"Learn variables\", \"Understand data types\"]}";
        
        var lesson2 = CreateTestLesson(title: "Lesson 2");
        lesson2.StructuredContent = "{\"objectives\": [\"Master loops\", \"Practice conditionals\"]}";

        await repository.CreateAsync(lesson1);
        await repository.CreateAsync(lesson2);

        // Act
        var result = await repository.SearchAsync("variables");

        // Assert
        Assert.Single(result);
        Assert.Equal(lesson1.Id, result[0].Id);
    }

    [Fact]
    public async Task SearchAsync_ShouldBeCaseInsensitive()
    {
        // Arrange
        var repository = CreateRepository();
        var lesson = CreateTestLesson(title: "Introduction to Programming");
        await repository.CreateAsync(lesson);

        // Act
        var result1 = await repository.SearchAsync("PROGRAMMING");
        var result2 = await repository.SearchAsync("programming");
        var result3 = await repository.SearchAsync("Programming");

        // Assert
        Assert.Single(result1);
        Assert.Single(result2);
        Assert.Single(result3);
    }

    [Fact]
    public async Task SearchAsync_ShouldReturnEmptyList_WhenQueryIsEmpty()
    {
        // Arrange
        var repository = CreateRepository();
        var lesson = CreateTestLesson();
        await repository.CreateAsync(lesson);

        // Act
        var result = await repository.SearchAsync("");

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task SearchAsync_ShouldReturnEmptyList_WhenQueryIsWhitespace()
    {
        // Arrange
        var repository = CreateRepository();
        var lesson = CreateTestLesson();
        await repository.CreateAsync(lesson);

        // Act
        var result = await repository.SearchAsync("   ");

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task SearchAsync_ShouldReturnEmptyList_WhenNoMatchesFound()
    {
        // Arrange
        var repository = CreateRepository();
        var lesson = CreateTestLesson(title: "C# Basics");
        await repository.CreateAsync(lesson);

        // Act
        var result = await repository.SearchAsync("Python");

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateLesson_WhenLessonExists()
    {
        // Arrange
        var repository = CreateRepository();
        var lesson = CreateTestLesson(title: "Original Title");
        await repository.CreateAsync(lesson);

        // Act
        lesson.Title = "Updated Title";
        lesson.Duration = "60 min";
        await repository.UpdateAsync(lesson);

        // Act - Retrieve updated lesson
        var result = await repository.GetByIdAsync(lesson.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Updated Title", result.Title);
        Assert.Equal("60 min", result.Duration);
        Assert.True(result.UpdatedAt > result.CreatedAt);
    }

    [Fact]
    public async Task UpdateAsync_ShouldThrowException_WhenLessonIsNull()
    {
        // Arrange
        var repository = CreateRepository();

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => repository.UpdateAsync(null!));
    }

    [Fact]
    public async Task UpdateAsync_ShouldThrowException_WhenLessonDoesNotExist()
    {
        // Arrange
        var repository = CreateRepository();
        var lesson = CreateTestLesson();

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => repository.UpdateAsync(lesson));
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveLesson_WhenLessonExists()
    {
        // Arrange
        var repository = CreateRepository();
        var lesson = CreateTestLesson();
        await repository.CreateAsync(lesson);

        // Act
        await repository.DeleteAsync(lesson.Id);

        // Assert
        var result = await repository.GetByIdAsync(lesson.Id);
        Assert.Null(result);
    }

    [Fact]
    public async Task DeleteAsync_ShouldNotThrowException_WhenLessonDoesNotExist()
    {
        // Arrange
        var repository = CreateRepository();
        var nonExistentId = Guid.NewGuid();

        // Act & Assert - Should not throw
        await repository.DeleteAsync(nonExistentId);
    }

    [Fact]
    public async Task Repository_ShouldBeThreadSafe_WhenAccessedConcurrently()
    {
        // Arrange
        var repository = CreateRepository();
        var courseId = Guid.NewGuid();
        var tasks = new List<Task>();

        // Act - Create 100 lessons concurrently
        for (int i = 0; i < 100; i++)
        {
            var index = i;
            tasks.Add(Task.Run(async () =>
            {
                var lesson = CreateTestLesson(courseId: courseId, title: $"Lesson {index}", orderIndex: index);
                await repository.CreateAsync(lesson);
            }));
        }

        await Task.WhenAll(tasks);

        // Assert
        var allLessons = await repository.GetAllAsync();
        Assert.Equal(100, allLessons.Count());

        var courseLessons = await repository.GetByCourseIdAsync(courseId);
        Assert.Equal(100, courseLessons.Count);
    }
}
