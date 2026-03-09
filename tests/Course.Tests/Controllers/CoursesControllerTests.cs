using Course.Service.Controllers;
using Course.Service.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Shared.Data;
using Shared.Entities;
using System.Text.Json;

namespace Course.Tests.Controllers;

/// <summary>
/// Unit tests for CoursesController
/// </summary>
public class CoursesControllerTests : IDisposable
{
    private readonly ApplicationDbContext _context;
    private readonly Mock<ILogger<CoursesController>> _mockLogger;
    private readonly CoursesController _controller;

    public CoursesControllerTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new ApplicationDbContext(options);
        _mockLogger = new Mock<ILogger<CoursesController>>();
        _controller = new CoursesController(_context, _mockLogger.Object);
    }

    #region GetAll Tests

    [Fact]
    public async Task GetAll_ReturnsAllCourses_OrderedByIndex()
    {
        // Arrange
        var course1 = new Shared.Entities.Course
        {
            Id = Guid.NewGuid(),
            Title = "Course 1",
            Description = "Description 1",
            Level = Level.Beginner,
            OrderIndex = 2,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var course2 = new Shared.Entities.Course
        {
            Id = Guid.NewGuid(),
            Title = "Course 2",
            Description = "Description 2",
            Level = Level.Intermediate,
            OrderIndex = 1,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Courses.AddRange(course1, course2);
        await _context.SaveChangesAsync();

        // Act
        var result = await _controller.GetAll();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var response = Assert.IsType<CourseListResponse>(okResult.Value);
        Assert.Equal(2, response.Courses.Count);
        Assert.Equal("Course 2", response.Courses[0].Title); // OrderIndex 1 first
        Assert.Equal("Course 1", response.Courses[1].Title); // OrderIndex 2 second
    }

    [Fact]
    public async Task GetAll_FiltersBy_LevelId()
    {
        // Arrange
        var levelId = Guid.NewGuid();
        var course1 = new Shared.Entities.Course
        {
            Id = Guid.NewGuid(),
            Title = "Course 1",
            Description = "Description 1",
            Level = Level.Beginner,
            LevelId = levelId,
            OrderIndex = 1,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var course2 = new Shared.Entities.Course
        {
            Id = Guid.NewGuid(),
            Title = "Course 2",
            Description = "Description 2",
            Level = Level.Beginner,
            LevelId = Guid.NewGuid(), // Different level
            OrderIndex = 2,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Courses.AddRange(course1, course2);
        await _context.SaveChangesAsync();

        // Act
        var result = await _controller.GetAll(levelId: levelId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var response = Assert.IsType<CourseListResponse>(okResult.Value);
        Assert.Single(response.Courses);
        Assert.Equal("Course 1", response.Courses[0].Title);
    }

    [Fact]
    public async Task GetAll_FiltersBy_Level()
    {
        // Arrange
        var course1 = new Shared.Entities.Course
        {
            Id = Guid.NewGuid(),
            Title = "Beginner Course",
            Description = "Description 1",
            Level = Level.Beginner,
            OrderIndex = 1,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var course2 = new Shared.Entities.Course
        {
            Id = Guid.NewGuid(),
            Title = "Advanced Course",
            Description = "Description 2",
            Level = Level.Advanced,
            OrderIndex = 2,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Courses.AddRange(course1, course2);
        await _context.SaveChangesAsync();

        // Act
        var result = await _controller.GetAll(level: "Beginner");

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var response = Assert.IsType<CourseListResponse>(okResult.Value);
        Assert.Single(response.Courses);
        Assert.Equal("Beginner Course", response.Courses[0].Title);
    }

    [Fact]
    public async Task GetAll_ReturnsEmptyList_WhenNoCourses()
    {
        // Act
        var result = await _controller.GetAll();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var response = Assert.IsType<CourseListResponse>(okResult.Value);
        Assert.Empty(response.Courses);
    }

    #endregion

    #region GetById Tests

    [Fact]
    public async Task GetById_ReturnsCourseDetails_WhenCourseExists()
    {
        // Arrange
        var courseId = Guid.NewGuid();
        var course = new Shared.Entities.Course
        {
            Id = courseId,
            Title = "Test Course",
            Description = "Test Description",
            Level = Level.Beginner,
            OrderIndex = 1,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var lesson = new Lesson
        {
            Id = Guid.NewGuid(),
            CourseId = courseId,
            Title = "Test Lesson",
            Content = "Test Content",
            OrderIndex = 1,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Courses.Add(course);
        _context.Lessons.Add(lesson);
        await _context.SaveChangesAsync();

        // Act
        var result = await _controller.GetById(courseId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var courseDetail = Assert.IsType<CourseDetailDto>(okResult.Value);
        Assert.Equal(courseId, courseDetail.Id);
        Assert.Equal("Test Course", courseDetail.Title);
        Assert.Single(courseDetail.Lessons);
    }

    [Fact]
    public async Task GetById_ReturnsNotFound_WhenCourseDoesNotExist()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        // Act
        var result = await _controller.GetById(nonExistentId);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetById_IncludesLevelTitle_WhenCurriculumLevelExists()
    {
        // Arrange
        var levelId = Guid.NewGuid();
        var level = new CurriculumLevel
        {
            Id = levelId,
            Number = 0,
            Title = "Level 0",
            Description = "Test level",
            RequiredXP = 0,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var courseId = Guid.NewGuid();
        var course = new Shared.Entities.Course
        {
            Id = courseId,
            Title = "Test Course",
            Description = "Test Description",
            Level = Level.Beginner,
            LevelId = levelId,
            OrderIndex = 1,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Set<CurriculumLevel>().Add(level);
        _context.Courses.Add(course);
        await _context.SaveChangesAsync();

        // Act
        var result = await _controller.GetById(courseId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var courseDetail = Assert.IsType<CourseDetailDto>(okResult.Value);
        Assert.Equal("Level 0", courseDetail.LevelTitle);
    }

    #endregion

    #region GetLessons Tests

    [Fact]
    public async Task GetLessons_ReturnsLessons_OrderedByIndex()
    {
        // Arrange
        var courseId = Guid.NewGuid();
        var course = new Shared.Entities.Course
        {
            Id = courseId,
            Title = "Test Course",
            Description = "Test Description",
            Level = Level.Beginner,
            OrderIndex = 1,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var lesson1 = new Lesson
        {
            Id = Guid.NewGuid(),
            CourseId = courseId,
            Title = "Lesson 1",
            Content = "Content 1",
            OrderIndex = 2,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var lesson2 = new Lesson
        {
            Id = Guid.NewGuid(),
            CourseId = courseId,
            Title = "Lesson 2",
            Content = "Content 2",
            OrderIndex = 1,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Courses.Add(course);
        _context.Lessons.AddRange(lesson1, lesson2);
        await _context.SaveChangesAsync();

        // Act
        var result = await _controller.GetLessons(courseId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var response = Assert.IsType<LessonListResponse>(okResult.Value);
        Assert.Equal(2, response.Lessons.Count);
        Assert.Equal("Lesson 2", response.Lessons[0].Title); // OrderIndex 1 first
        Assert.Equal("Lesson 1", response.Lessons[1].Title); // OrderIndex 2 second
    }

    [Fact]
    public async Task GetLessons_ReturnsNotFound_WhenCourseDoesNotExist()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        // Act
        var result = await _controller.GetLessons(nonExistentId);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetLessons_ReturnsEmptyList_WhenCourseHasNoLessons()
    {
        // Arrange
        var courseId = Guid.NewGuid();
        var course = new Shared.Entities.Course
        {
            Id = courseId,
            Title = "Test Course",
            Description = "Test Description",
            Level = Level.Beginner,
            OrderIndex = 1,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Courses.Add(course);
        await _context.SaveChangesAsync();

        // Act
        var result = await _controller.GetLessons(courseId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var response = Assert.IsType<LessonListResponse>(okResult.Value);
        Assert.Empty(response.Lessons);
    }

    #endregion

    #region GetLesson Tests

    [Fact]
    public async Task GetLesson_ReturnsLessonDetails_WhenLessonExists()
    {
        // Arrange
        var courseId = Guid.NewGuid();
        var lessonId = Guid.NewGuid();

        var course = new Shared.Entities.Course
        {
            Id = courseId,
            Title = "Test Course",
            Description = "Test Description",
            Level = Level.Beginner,
            OrderIndex = 1,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var lesson = new Lesson
        {
            Id = lessonId,
            CourseId = courseId,
            Title = "Test Lesson",
            Content = "Test Content",
            Duration = "30 min",
            Difficulty = "Easy",
            EstimatedMinutes = 30,
            OrderIndex = 1,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Courses.Add(course);
        _context.Lessons.Add(lesson);
        await _context.SaveChangesAsync();

        // Act
        var result = await _controller.GetLesson(courseId, lessonId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var lessonDetail = Assert.IsType<LessonDetailDto>(okResult.Value);
        Assert.Equal(lessonId, lessonDetail.Id);
        Assert.Equal("Test Lesson", lessonDetail.Title);
        Assert.Equal("30 min", lessonDetail.Duration);
        Assert.Equal("Easy", lessonDetail.Difficulty);
        Assert.Equal(30, lessonDetail.EstimatedMinutes);
    }

    [Fact]
    public async Task GetLesson_ReturnsNotFound_WhenLessonDoesNotExist()
    {
        // Arrange
        var courseId = Guid.NewGuid();
        var nonExistentLessonId = Guid.NewGuid();

        var course = new Shared.Entities.Course
        {
            Id = courseId,
            Title = "Test Course",
            Description = "Test Description",
            Level = Level.Beginner,
            OrderIndex = 1,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Courses.Add(course);
        await _context.SaveChangesAsync();

        // Act
        var result = await _controller.GetLesson(courseId, nonExistentLessonId);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetLesson_ReturnsNotFound_WhenLessonBelongsToDifferentCourse()
    {
        // Arrange
        var courseId1 = Guid.NewGuid();
        var courseId2 = Guid.NewGuid();
        var lessonId = Guid.NewGuid();

        var course1 = new Shared.Entities.Course
        {
            Id = courseId1,
            Title = "Course 1",
            Description = "Description 1",
            Level = Level.Beginner,
            OrderIndex = 1,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var course2 = new Shared.Entities.Course
        {
            Id = courseId2,
            Title = "Course 2",
            Description = "Description 2",
            Level = Level.Beginner,
            OrderIndex = 2,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var lesson = new Lesson
        {
            Id = lessonId,
            CourseId = courseId1, // Belongs to course1
            Title = "Test Lesson",
            Content = "Test Content",
            OrderIndex = 1,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Courses.AddRange(course1, course2);
        _context.Lessons.Add(lesson);
        await _context.SaveChangesAsync();

        // Act - Try to get lesson with wrong courseId
        var result = await _controller.GetLesson(courseId2, lessonId);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetLesson_DeserializesStructuredContent_WhenAvailable()
    {
        // Arrange
        var courseId = Guid.NewGuid();
        var lessonId = Guid.NewGuid();

        var structuredContent = new
        {
            Objectives = new[] { "Learn basics", "Practice coding" },
            Theory = new[]
            {
                new { Heading = "Introduction", Content = "Welcome", Order = 1 }
            },
            CodeExamples = new[]
            {
                new { Title = "Hello World", Code = "Console.WriteLine(\"Hello\");", Language = "csharp", Explanation = "Prints hello", IsRunnable = true }
            },
            Exercises = new[]
            {
                new { Title = "Exercise 1", Description = "Do this", Difficulty = "Easy", StarterCode = "// Start here", Hints = new[] { "Hint 1" } }
            },
            Summary = "Great job!"
        };

        var course = new Shared.Entities.Course
        {
            Id = courseId,
            Title = "Test Course",
            Description = "Test Description",
            Level = Level.Beginner,
            OrderIndex = 1,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var lesson = new Lesson
        {
            Id = lessonId,
            CourseId = courseId,
            Title = "Test Lesson",
            StructuredContent = JsonSerializer.Serialize(structuredContent),
            OrderIndex = 1,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Courses.Add(course);
        _context.Lessons.Add(lesson);
        await _context.SaveChangesAsync();

        // Act
        var result = await _controller.GetLesson(courseId, lessonId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var lessonDetail = Assert.IsType<LessonDetailDto>(okResult.Value);
        Assert.NotNull(lessonDetail.StructuredContent);
        Assert.Equal(2, lessonDetail.StructuredContent.Objectives.Count);
        Assert.Single(lessonDetail.StructuredContent.Theory);
        Assert.Single(lessonDetail.StructuredContent.CodeExamples);
        Assert.Single(lessonDetail.StructuredContent.Exercises);
        Assert.Equal("Great job!", lessonDetail.StructuredContent.Summary);
    }

    #endregion

    #region CompleteLesson Tests

    [Fact]
    public async Task CompleteLesson_ReturnsOk_WhenLessonExists()
    {
        // Arrange
        var courseId = Guid.NewGuid();
        var lessonId = Guid.NewGuid();

        var course = new Shared.Entities.Course
        {
            Id = courseId,
            Title = "Test Course",
            Description = "Test Description",
            Level = Level.Beginner,
            OrderIndex = 1,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var lesson = new Lesson
        {
            Id = lessonId,
            CourseId = courseId,
            Title = "Test Lesson",
            Content = "Test Content",
            OrderIndex = 1,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Courses.Add(course);
        _context.Lessons.Add(lesson);
        await _context.SaveChangesAsync();

        // Act
        var result = await _controller.CompleteLesson(courseId, lessonId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.NotNull(okResult.Value);
    }

    [Fact]
    public async Task CompleteLesson_ReturnsNotFound_WhenLessonDoesNotExist()
    {
        // Arrange
        var courseId = Guid.NewGuid();
        var nonExistentLessonId = Guid.NewGuid();

        var course = new Shared.Entities.Course
        {
            Id = courseId,
            Title = "Test Course",
            Description = "Test Description",
            Level = Level.Beginner,
            OrderIndex = 1,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Courses.Add(course);
        await _context.SaveChangesAsync();

        // Act
        var result = await _controller.CompleteLesson(courseId, nonExistentLessonId);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
    }

    #endregion

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
}
