using Course.Service.Extensions;
using Shared.Entities;
using Shared.Models;
using System.Text.Json;

namespace Course.Tests.Extensions;

/// <summary>
/// Unit tests for MappingExtensions
/// </summary>
public class MappingExtensionsTests
{
    #region ToLevelDto Tests

    [Fact]
    public void ToLevelDto_MapsAllProperties_Correctly()
    {
        // Arrange
        var level = new CurriculumLevel
        {
            Id = Guid.NewGuid(),
            Number = 5,
            Title = "Level 5",
            Description = "Intermediate level",
            RequiredXP = 1000,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        // Act
        var dto = level.ToLevelDto();

        // Assert
        Assert.Equal(level.Id, dto.Id);
        Assert.Equal(5, dto.Number);
        Assert.Equal("Level 5", dto.Title);
        Assert.Equal("Intermediate level", dto.Description);
        Assert.Equal(1000, dto.RequiredXP);
    }

    [Fact]
    public void ToLevelDto_CalculatesCourseCount_WhenCoursesExist()
    {
        // Arrange
        var level = new CurriculumLevel
        {
            Id = Guid.NewGuid(),
            Number = 0,
            Title = "Level 0",
            Description = "Beginner level",
            RequiredXP = 0,
            Courses = new List<Shared.Entities.Course>
            {
                new() { Id = Guid.NewGuid(), Title = "Course 1", Description = "Desc 1", Level = Level.Beginner, OrderIndex = 1, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new() { Id = Guid.NewGuid(), Title = "Course 2", Description = "Desc 2", Level = Level.Beginner, OrderIndex = 2, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow }
            },
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        // Act
        var dto = level.ToLevelDto();

        // Assert
        Assert.Equal(2, dto.CourseCount);
    }

    [Fact]
    public void ToLevelDto_ReturnsCourseCountZero_WhenNoCoursesExist()
    {
        // Arrange
        var level = new CurriculumLevel
        {
            Id = Guid.NewGuid(),
            Number = 0,
            Title = "Level 0",
            Description = "Beginner level",
            RequiredXP = 0,
            Courses = null,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        // Act
        var dto = level.ToLevelDto();

        // Assert
        Assert.Equal(0, dto.CourseCount);
    }

    [Fact]
    public void ToLevelDto_EstimatesHours_BasedOnLessonCount()
    {
        // Arrange
        var level = new CurriculumLevel
        {
            Id = Guid.NewGuid(),
            Number = 0,
            Title = "Level 0",
            Description = "Beginner level",
            RequiredXP = 0,
            Courses = new List<Shared.Entities.Course>
            {
                new() { Id = Guid.NewGuid(), Title = "Course 1", Description = "Desc 1", Level = Level.Beginner, LessonCount = 10, OrderIndex = 1, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new() { Id = Guid.NewGuid(), Title = "Course 2", Description = "Desc 2", Level = Level.Beginner, LessonCount = 6, OrderIndex = 2, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow }
            },
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        // Act
        var dto = level.ToLevelDto();

        // Assert
        // 16 lessons * 0.75 hours = 12 hours
        Assert.Equal(12, dto.EstimatedHours);
    }

    #endregion

    #region ToLevelDetailDto Tests

    [Fact]
    public void ToLevelDetailDto_MapsAllProperties_Correctly()
    {
        // Arrange
        var level = new CurriculumLevel
        {
            Id = Guid.NewGuid(),
            Number = 3,
            Title = "Level 3",
            Description = "Advanced level",
            RequiredXP = 500,
            Courses = new List<Shared.Entities.Course>
            {
                new() { Id = Guid.NewGuid(), Title = "Course 1", Description = "Desc 1", Level = Level.Advanced, OrderIndex = 1, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow }
            },
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        // Act
        var dto = level.ToLevelDetailDto();

        // Assert
        Assert.Equal(level.Id, dto.Id);
        Assert.Equal(3, dto.Number);
        Assert.Equal("Level 3", dto.Title);
        Assert.Equal("Advanced level", dto.Description);
        Assert.Equal(500, dto.RequiredXP);
        Assert.Single(dto.Courses);
    }

    [Fact]
    public void ToLevelDetailDto_OrdersCourses_ByOrderIndex()
    {
        // Arrange
        var level = new CurriculumLevel
        {
            Id = Guid.NewGuid(),
            Number = 0,
            Title = "Level 0",
            Description = "Test level",
            RequiredXP = 0,
            Courses = new List<Shared.Entities.Course>
            {
                new() { Id = Guid.NewGuid(), Title = "Course B", Description = "Desc B", Level = Level.Beginner, OrderIndex = 2, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new() { Id = Guid.NewGuid(), Title = "Course A", Description = "Desc A", Level = Level.Beginner, OrderIndex = 1, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new() { Id = Guid.NewGuid(), Title = "Course C", Description = "Desc C", Level = Level.Beginner, OrderIndex = 3, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow }
            },
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        // Act
        var dto = level.ToLevelDetailDto();

        // Assert
        Assert.Equal(3, dto.Courses.Count);
        Assert.Equal("Course A", dto.Courses[0].Title);
        Assert.Equal("Course B", dto.Courses[1].Title);
        Assert.Equal("Course C", dto.Courses[2].Title);
    }

    [Fact]
    public void ToLevelDetailDto_IncludesProject_WhenProjectExists()
    {
        // Arrange
        var level = new CurriculumLevel
        {
            Id = Guid.NewGuid(),
            Number = 0,
            Title = "Level 0",
            Description = "Test level",
            RequiredXP = 0,
            Project = new Project
            {
                Id = Guid.NewGuid(),
                Title = "Capstone Project",
                Description = "Final project",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        // Act
        var dto = level.ToLevelDetailDto();

        // Assert
        Assert.NotNull(dto.Project);
        Assert.Equal("Capstone Project", dto.Project.Title);
        Assert.Equal("Final project", dto.Project.Description);
    }

    [Fact]
    public void ToLevelDetailDto_ProjectIsNull_WhenNoProjectExists()
    {
        // Arrange
        var level = new CurriculumLevel
        {
            Id = Guid.NewGuid(),
            Number = 0,
            Title = "Level 0",
            Description = "Test level",
            RequiredXP = 0,
            Project = null,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        // Act
        var dto = level.ToLevelDetailDto();

        // Assert
        Assert.Null(dto.Project);
    }

    #endregion

    #region ToCourseSummaryDto Tests

    [Fact]
    public void ToCourseSummaryDto_MapsAllProperties_Correctly()
    {
        // Arrange
        var course = new Shared.Entities.Course
        {
            Id = Guid.NewGuid(),
            Title = "Test Course",
            Description = "Test Description",
            Level = Level.Intermediate,
            LevelId = Guid.NewGuid(),
            Duration = "2 hours",
            LessonCount = 5,
            Topics = JsonSerializer.Serialize(new[] { "C#", "ASP.NET" }),
            OrderIndex = 3,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        // Act
        var dto = course.ToCourseSummaryDto();

        // Assert
        Assert.Equal(course.Id, dto.Id);
        Assert.Equal("Test Course", dto.Title);
        Assert.Equal("Test Description", dto.Description);
        Assert.Equal("Intermediate", dto.Level);
        Assert.Equal(course.LevelId, dto.LevelId);
        Assert.Equal("2 hours", dto.Duration);
        Assert.Equal(5, dto.LessonCount);
        Assert.Equal(3, dto.OrderIndex);
        Assert.Equal(2, dto.Topics.Count);
        Assert.Contains("C#", dto.Topics);
        Assert.Contains("ASP.NET", dto.Topics);
    }

    [Fact]
    public void ToCourseSummaryDto_HandlesNullTopics()
    {
        // Arrange
        var course = new Shared.Entities.Course
        {
            Id = Guid.NewGuid(),
            Title = "Test Course",
            Description = "Test Description",
            Level = Level.Beginner,
            Topics = null,
            OrderIndex = 1,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        // Act
        var dto = course.ToCourseSummaryDto();

        // Assert
        Assert.Empty(dto.Topics);
    }

    [Fact]
    public void ToCourseSummaryDto_HandlesEmptyTopics()
    {
        // Arrange
        var course = new Shared.Entities.Course
        {
            Id = Guid.NewGuid(),
            Title = "Test Course",
            Description = "Test Description",
            Level = Level.Beginner,
            Topics = "",
            OrderIndex = 1,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        // Act
        var dto = course.ToCourseSummaryDto();

        // Assert
        Assert.Empty(dto.Topics);
    }

    [Fact]
    public void ToCourseSummaryDto_HandlesInvalidJsonTopics()
    {
        // Arrange
        var course = new Shared.Entities.Course
        {
            Id = Guid.NewGuid(),
            Title = "Test Course",
            Description = "Test Description",
            Level = Level.Beginner,
            Topics = "invalid json",
            OrderIndex = 1,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        // Act
        var dto = course.ToCourseSummaryDto();

        // Assert
        Assert.Empty(dto.Topics);
    }

    #endregion

    #region ToCourseDetailDto Tests

    [Fact]
    public void ToCourseDetailDto_MapsAllProperties_Correctly()
    {
        // Arrange
        var levelId = Guid.NewGuid();
        var course = new Shared.Entities.Course
        {
            Id = Guid.NewGuid(),
            Title = "Test Course",
            Description = "Test Description",
            Level = Level.Advanced,
            LevelId = levelId,
            Duration = "3 hours",
            LessonCount = 8,
            Topics = JsonSerializer.Serialize(new[] { "Advanced C#", "Design Patterns" }),
            OrderIndex = 2,
            CurriculumLevel = new CurriculumLevel
            {
                Id = levelId,
                Number = 5,
                Title = "Level 5",
                Description = "Advanced",
                RequiredXP = 1000,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            Lessons = new List<Lesson>
            {
                new() { Id = Guid.NewGuid(), Title = "Lesson 1", Content = "Content 1", OrderIndex = 1, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow }
            },
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        // Act
        var dto = course.ToCourseDetailDto();

        // Assert
        Assert.Equal(course.Id, dto.Id);
        Assert.Equal("Test Course", dto.Title);
        Assert.Equal("Test Description", dto.Description);
        Assert.Equal("Advanced", dto.Level);
        Assert.Equal(levelId, dto.LevelId);
        Assert.Equal("Level 5", dto.LevelTitle);
        Assert.Equal("3 hours", dto.Duration);
        Assert.Equal(8, dto.LessonCount);
        Assert.Equal(2, dto.Topics.Count);
        Assert.Single(dto.Lessons);
    }

    [Fact]
    public void ToCourseDetailDto_OrdersLessons_ByOrderIndex()
    {
        // Arrange
        var course = new Shared.Entities.Course
        {
            Id = Guid.NewGuid(),
            Title = "Test Course",
            Description = "Test Description",
            Level = Level.Beginner,
            OrderIndex = 1,
            Lessons = new List<Lesson>
            {
                new() { Id = Guid.NewGuid(), Title = "Lesson C", Content = "Content C", OrderIndex = 3, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new() { Id = Guid.NewGuid(), Title = "Lesson A", Content = "Content A", OrderIndex = 1, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new() { Id = Guid.NewGuid(), Title = "Lesson B", Content = "Content B", OrderIndex = 2, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow }
            },
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        // Act
        var dto = course.ToCourseDetailDto();

        // Assert
        Assert.Equal(3, dto.Lessons.Count);
        Assert.Equal("Lesson A", dto.Lessons[0].Title);
        Assert.Equal("Lesson B", dto.Lessons[1].Title);
        Assert.Equal("Lesson C", dto.Lessons[2].Title);
    }

    #endregion

    #region ToLessonSummaryDto Tests

    [Fact]
    public void ToLessonSummaryDto_MapsAllProperties_Correctly()
    {
        // Arrange
        var lesson = new Lesson
        {
            Id = Guid.NewGuid(),
            Title = "Test Lesson",
            Duration = "45 min",
            Difficulty = "Medium",
            EstimatedMinutes = 45,
            OrderIndex = 2,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        // Act
        var dto = lesson.ToLessonSummaryDto();

        // Assert
        Assert.Equal(lesson.Id, dto.Id);
        Assert.Equal("Test Lesson", dto.Title);
        Assert.Equal("45 min", dto.Duration);
        Assert.Equal("Medium", dto.Difficulty);
        Assert.Equal(45, dto.EstimatedMinutes);
        Assert.Equal(2, dto.Order);
        Assert.False(dto.IsCompleted); // TODO: Will be updated when user context is implemented
    }

    #endregion

    #region ToLessonDetailDto Tests

    [Fact]
    public void ToLessonDetailDto_MapsAllProperties_Correctly()
    {
        // Arrange
        var lesson = new Lesson
        {
            Id = Guid.NewGuid(),
            Title = "Test Lesson",
            Content = "<p>HTML Content</p>",
            Duration = "30 min",
            Difficulty = "Easy",
            EstimatedMinutes = 30,
            Prerequisites = JsonSerializer.Serialize(new[] { "Lesson 1", "Lesson 2" }),
            OrderIndex = 3,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        // Act
        var dto = lesson.ToLessonDetailDto();

        // Assert
        Assert.Equal(lesson.Id, dto.Id);
        Assert.Equal("Test Lesson", dto.Title);
        Assert.Equal("<p>HTML Content</p>", dto.Content);
        Assert.Equal("30 min", dto.Duration);
        Assert.Equal("Easy", dto.Difficulty);
        Assert.Equal(30, dto.EstimatedMinutes);
        Assert.Equal(2, dto.Prerequisites.Count);
        Assert.Contains("Lesson 1", dto.Prerequisites);
        Assert.Contains("Lesson 2", dto.Prerequisites);
        Assert.Equal(3, dto.Order);
    }

    [Fact]
    public void ToLessonDetailDto_DeserializesStructuredContent_WhenAvailable()
    {
        // Arrange
        var structuredContent = new
        {
            Objectives = new[] { "Learn X", "Practice Y" },
            Theory = new[]
            {
                new { Heading = "Introduction", Content = "Welcome to the lesson", Order = 1 }
            },
            CodeExamples = new[]
            {
                new { Title = "Example 1", Code = "var x = 5;", Language = "csharp", Explanation = "Variable declaration", IsRunnable = true }
            },
            Exercises = new[]
            {
                new { Title = "Exercise 1", Description = "Complete this", Difficulty = "Easy", StarterCode = "// Start", Hints = new[] { "Hint 1" } }
            },
            Summary = "Well done!"
        };

        var lesson = new Lesson
        {
            Id = Guid.NewGuid(),
            Title = "Test Lesson",
            StructuredContent = JsonSerializer.Serialize(structuredContent),
            OrderIndex = 1,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        // Act
        var dto = lesson.ToLessonDetailDto();

        // Assert
        Assert.NotNull(dto.StructuredContent);
        Assert.Equal(2, dto.StructuredContent.Objectives.Count);
        Assert.Single(dto.StructuredContent.Theory);
        Assert.Equal("Introduction", dto.StructuredContent.Theory[0].Heading);
        Assert.Single(dto.StructuredContent.CodeExamples);
        Assert.Equal("Example 1", dto.StructuredContent.CodeExamples[0].Title);
        Assert.Single(dto.StructuredContent.Exercises);
        Assert.Equal("Well done!", dto.StructuredContent.Summary);
    }

    [Fact]
    public void ToLessonDetailDto_HandlesNullStructuredContent()
    {
        // Arrange
        var lesson = new Lesson
        {
            Id = Guid.NewGuid(),
            Title = "Test Lesson",
            Content = "<p>HTML Content</p>",
            StructuredContent = null,
            OrderIndex = 1,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        // Act
        var dto = lesson.ToLessonDetailDto();

        // Assert
        Assert.Null(dto.StructuredContent);
        Assert.NotNull(dto.Content);
    }

    [Fact]
    public void ToLessonDetailDto_HandlesInvalidStructuredContentJson()
    {
        // Arrange
        var lesson = new Lesson
        {
            Id = Guid.NewGuid(),
            Title = "Test Lesson",
            Content = "<p>HTML Content</p>",
            StructuredContent = "invalid json",
            OrderIndex = 1,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        // Act
        var dto = lesson.ToLessonDetailDto();

        // Assert
        Assert.Null(dto.StructuredContent);
        Assert.NotNull(dto.Content);
    }

    [Fact]
    public void ToLessonDetailDto_HandlesNullPrerequisites()
    {
        // Arrange
        var lesson = new Lesson
        {
            Id = Guid.NewGuid(),
            Title = "Test Lesson",
            Prerequisites = null,
            OrderIndex = 1,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        // Act
        var dto = lesson.ToLessonDetailDto();

        // Assert
        Assert.Empty(dto.Prerequisites);
    }

    #endregion

    #region ToLessonContentDto Tests

    [Fact]
    public void ToLessonContentDto_MapsAllProperties_Correctly()
    {
        // Arrange
        var content = new LessonContent
        {
            Objectives = new List<string> { "Objective 1", "Objective 2" },
            Theory = new List<TheorySection>
            {
                new() { Heading = "Section 1", Content = "Content 1", Order = 1 }
            },
            CodeExamples = new List<CodeExample>
            {
                new() { Title = "Example 1", Code = "code", Language = "csharp", Explanation = "Explanation", IsRunnable = true }
            },
            Exercises = new List<Exercise>
            {
                new() { Title = "Exercise 1", Description = "Do this", Difficulty = ExerciseDifficulty.Fácil, StarterCode = "// Start", Hints = new List<string> { "Hint" } }
            },
            Summary = "Summary text"
        };

        // Act
        var dto = content.ToLessonContentDto();

        // Assert
        Assert.Equal(2, dto.Objectives.Count);
        Assert.Single(dto.Theory);
        Assert.Equal("Section 1", dto.Theory[0].Heading);
        Assert.Single(dto.CodeExamples);
        Assert.Equal("Example 1", dto.CodeExamples[0].Title);
        Assert.Single(dto.Exercises);
        Assert.Equal("Easy", dto.Exercises[0].Difficulty);
        Assert.Equal("Summary text", dto.Summary);
    }

    [Fact]
    public void ToLessonContentDto_HandlesNullCollections()
    {
        // Arrange
        var content = new LessonContent
        {
            Objectives = null,
            Theory = null,
            CodeExamples = null,
            Exercises = null,
            Summary = null
        };

        // Act
        var dto = content.ToLessonContentDto();

        // Assert
        Assert.Empty(dto.Objectives);
        Assert.Empty(dto.Theory);
        Assert.Empty(dto.CodeExamples);
        Assert.Empty(dto.Exercises);
        Assert.Equal(string.Empty, dto.Summary);
    }

    [Fact]
    public void ToLessonContentDto_HandlesEmptyCollections()
    {
        // Arrange
        var content = new LessonContent
        {
            Objectives = new List<string>(),
            Theory = new List<TheorySection>(),
            CodeExamples = new List<CodeExample>(),
            Exercises = new List<Exercise>(),
            Summary = ""
        };

        // Act
        var dto = content.ToLessonContentDto();

        // Assert
        Assert.Empty(dto.Objectives);
        Assert.Empty(dto.Theory);
        Assert.Empty(dto.CodeExamples);
        Assert.Empty(dto.Exercises);
        Assert.Equal(string.Empty, dto.Summary);
    }

    #endregion
}
