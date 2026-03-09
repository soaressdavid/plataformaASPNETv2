using Course.Service.Services;
using Microsoft.EntityFrameworkCore;
using Shared.Data;
using Shared.Entities;
using Shared.Repositories;

namespace Course.Tests;

/// <summary>
/// Unit tests for CourseService.
/// </summary>
public class CourseServiceTests : IDisposable
{
    private readonly ApplicationDbContext _context;
    private readonly CourseService _courseService;

    public CourseServiceTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new ApplicationDbContext(options);
        var courseRepository = new CourseRepository(_context);
        _courseService = new CourseService(courseRepository, _context);
    }

    [Fact]
    public async Task GetAllCoursesAsync_ReturnsAllCourses()
    {
        // Arrange
        var course1 = new Shared.Entities.Course
        {
            Id = Guid.NewGuid(),
            Title = "Course 1",
            Description = "Description 1",
            Level = Level.Beginner,
            OrderIndex = 1,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var course2 = new Shared.Entities.Course
        {
            Id = Guid.NewGuid(),
            Title = "Course 2",
            Description = "Description 2",
            Level = Level.Intermediate,
            OrderIndex = 2,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Courses.AddRange(course1, course2);
        await _context.SaveChangesAsync();

        // Act
        var courses = await _courseService.GetAllCoursesAsync();

        // Assert
        Assert.Equal(2, courses.Count);
        Assert.Contains(courses, c => c.Title == "Course 1");
        Assert.Contains(courses, c => c.Title == "Course 2");
    }

    [Fact]
    public async Task GetCourseByIdAsync_ReturnsCourseWithLessons()
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
        var retrievedCourse = await _courseService.GetCourseByIdAsync(courseId);

        // Assert
        Assert.NotNull(retrievedCourse);
        Assert.Equal("Test Course", retrievedCourse.Title);
        Assert.NotEmpty(retrievedCourse.Lessons);
    }

    [Fact]
    public async Task GetLessonsAsync_ReturnsLessonsOrderedByIndex()
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
            OrderIndex = 1,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var lesson2 = new Lesson
        {
            Id = Guid.NewGuid(),
            CourseId = courseId,
            Title = "Lesson 2",
            Content = "Content 2",
            OrderIndex = 2,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var lesson3 = new Lesson
        {
            Id = Guid.NewGuid(),
            CourseId = courseId,
            Title = "Lesson 3",
            Content = "Content 3",
            OrderIndex = 3,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Courses.Add(course);
        _context.Lessons.AddRange(lesson3, lesson1, lesson2); // Add out of order
        await _context.SaveChangesAsync();

        // Act
        var lessons = await _courseService.GetLessonsAsync(courseId);

        // Assert
        Assert.Equal(3, lessons.Count);
        Assert.Equal("Lesson 1", lessons[0].Title);
        Assert.Equal("Lesson 2", lessons[1].Title);
        Assert.Equal("Lesson 3", lessons[2].Title);
    }

    [Fact]
    public async Task EnrollAsync_CreatesNewEnrollment()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var courseId = Guid.NewGuid();

        var user = new User
        {
            Id = userId,
            Name = "Test User",
            Email = "test@example.com",
            PasswordHash = "hash",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
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

        _context.Users.Add(user);
        _context.Courses.Add(course);
        await _context.SaveChangesAsync();

        // Act
        var enrollment = await _courseService.EnrollAsync(userId, courseId);

        // Assert
        Assert.NotNull(enrollment);
        Assert.Equal(userId, enrollment.UserId);
        Assert.Equal(courseId, enrollment.CourseId);
        Assert.NotEqual(Guid.Empty, enrollment.Id);
    }

    [Fact]
    public async Task EnrollAsync_ReturnsExistingEnrollmentIfAlreadyEnrolled()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var courseId = Guid.NewGuid();

        var user = new User
        {
            Id = userId,
            Name = "Test User",
            Email = "test@example.com",
            PasswordHash = "hash",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
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

        _context.Users.Add(user);
        _context.Courses.Add(course);
        await _context.SaveChangesAsync();

        // Act
        var enrollment1 = await _courseService.EnrollAsync(userId, courseId);
        var enrollment2 = await _courseService.EnrollAsync(userId, courseId);

        // Assert
        Assert.Equal(enrollment1.Id, enrollment2.Id);
        Assert.Single(_context.Enrollments.Where(e => e.UserId == userId && e.CourseId == courseId));
    }

    [Fact]
    public async Task CompleteLessonAsync_MarksLessonAsComplete()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var courseId = Guid.NewGuid();
        var lessonId = Guid.NewGuid();

        var user = new User
        {
            Id = userId,
            Name = "Test User",
            Email = "test@example.com",
            PasswordHash = "hash",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
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
            Content = "Test Content",
            OrderIndex = 1,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Users.Add(user);
        _context.Courses.Add(course);
        _context.Lessons.Add(lesson);
        await _context.SaveChangesAsync();

        // Act
        var (success, nextLessonId) = await _courseService.CompleteLessonAsync(userId, lessonId);

        // Assert
        Assert.True(success);
        Assert.Null(nextLessonId); // No next lesson

        var completion = await _context.LessonCompletions
            .FirstOrDefaultAsync(lc => lc.UserId == userId && lc.LessonId == lessonId);
        Assert.NotNull(completion);
    }

    [Fact]
    public async Task CompleteLessonAsync_ReturnsNextLessonId()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var courseId = Guid.NewGuid();

        var user = new User
        {
            Id = userId,
            Name = "Test User",
            Email = "test@example.com",
            PasswordHash = "hash",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
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

        var lesson1 = new Lesson
        {
            Id = Guid.NewGuid(),
            CourseId = courseId,
            Title = "Lesson 1",
            Content = "Content 1",
            OrderIndex = 1,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var lesson2 = new Lesson
        {
            Id = Guid.NewGuid(),
            CourseId = courseId,
            Title = "Lesson 2",
            Content = "Content 2",
            OrderIndex = 2,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Users.Add(user);
        _context.Courses.Add(course);
        _context.Lessons.AddRange(lesson1, lesson2);
        await _context.SaveChangesAsync();

        // Act
        var (success, nextLessonId) = await _courseService.CompleteLessonAsync(userId, lesson1.Id);

        // Assert
        Assert.True(success);
        Assert.NotNull(nextLessonId);
        Assert.Equal(lesson2.Id, nextLessonId);
    }

    [Fact]
    public async Task CompleteLessonAsync_DoesNotDuplicateCompletion()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var courseId = Guid.NewGuid();
        var lessonId = Guid.NewGuid();

        var user = new User
        {
            Id = userId,
            Name = "Test User",
            Email = "test@example.com",
            PasswordHash = "hash",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
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
            Content = "Test Content",
            OrderIndex = 1,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Users.Add(user);
        _context.Courses.Add(course);
        _context.Lessons.Add(lesson);
        await _context.SaveChangesAsync();

        // Act
        await _courseService.CompleteLessonAsync(userId, lessonId);
        await _courseService.CompleteLessonAsync(userId, lessonId);

        // Assert
        var completions = _context.LessonCompletions
            .Where(lc => lc.UserId == userId && lc.LessonId == lessonId)
            .ToList();
        Assert.Single(completions);
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
}
