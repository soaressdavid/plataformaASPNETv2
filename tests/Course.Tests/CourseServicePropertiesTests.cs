using Course.Service.Services;
using FsCheck;
using FsCheck.Xunit;
using Microsoft.EntityFrameworkCore;
using Shared.Data;
using Shared.Entities;
using Shared.Repositories;

namespace Course.Tests;

/// <summary>
/// Property-based tests for course service functionality.
/// Feature: aspnet-learning-platform
/// </summary>
public class CourseServicePropertiesTests : IDisposable
{
    private readonly ApplicationDbContext _context;
    private readonly CourseService _courseService;

    public CourseServicePropertiesTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new ApplicationDbContext(options);
        var courseRepository = new CourseRepository(_context);
        _courseService = new CourseService(courseRepository, _context);
    }

    /// <summary>
    /// Property 23: Course Structure
    /// **Validates: Requirements 7.1, 7.5**
    /// 
    /// For any course in the system, it should have a defined difficulty level 
    /// (Beginner, Intermediate, or Advanced) and contain at least one lesson.
    /// </summary>
    [Property(MaxTest = 20)]
    public void CourseStructure_HasValidLevelAndLessons()
    {
        // Arrange: Create a course with at least one lesson
        var course = new Shared.Entities.Course
        {
            Id = Guid.NewGuid(),
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
            CourseId = course.Id,
            Title = "Test Lesson",
            Content = "Test Content",
            OrderIndex = 1,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Courses.Add(course);
        _context.Lessons.Add(lesson);
        _context.SaveChanges();

        // Act: Retrieve the course
        var retrievedCourse = _courseService.GetCourseByIdAsync(course.Id).Result;
        var lessons = _courseService.GetLessonsAsync(course.Id).Result;

        // Assert: Course has valid level and at least one lesson
        Assert.NotNull(retrievedCourse);
        Assert.True(Enum.IsDefined(typeof(Level), retrievedCourse.Level));
        Assert.NotEmpty(lessons);
    }

    /// <summary>
    /// Property 24: Lesson Ordering
    /// **Validates: Requirements 7.2**
    /// 
    /// For any course, its lessons should have unique sequential order indices with no gaps.
    /// </summary>
    [Property(MaxTest = 20)]
    public void LessonOrdering_HasSequentialIndices(PositiveInt lessonCount)
    {
        // Limit lesson count to reasonable number
        var count = Math.Min(lessonCount.Get, 10);
        
        // Arrange: Create a course with multiple lessons
        var course = new Shared.Entities.Course
        {
            Id = Guid.NewGuid(),
            Title = "Test Course",
            Description = "Test Description",
            Level = Level.Beginner,
            OrderIndex = 1,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Courses.Add(course);

        for (int i = 1; i <= count; i++)
        {
            var lesson = new Lesson
            {
                Id = Guid.NewGuid(),
                CourseId = course.Id,
                Title = $"Lesson {i}",
                Content = $"Content {i}",
                OrderIndex = i,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            _context.Lessons.Add(lesson);
        }

        _context.SaveChanges();

        // Act: Retrieve lessons
        var lessons = _courseService.GetLessonsAsync(course.Id).Result;

        // Assert: Lessons are ordered sequentially
        Assert.Equal(count, lessons.Count);
        for (int i = 0; i < lessons.Count; i++)
        {
            Assert.Equal(i + 1, lessons[i].OrderIndex);
        }
    }

    /// <summary>
    /// Property 25: Enrollment Tracking
    /// **Validates: Requirements 7.3**
    /// 
    /// For any student enrollment in a course, the platform should create an enrollment 
    /// record and track which lessons have been completed.
    /// </summary>
    [Property(MaxTest = 20)]
    public void EnrollmentTracking_CreatesRecordAndTracksCompletions()
    {
        // Arrange: Create a user and course
        var userId = Guid.NewGuid();
        var user = new User
        {
            Id = userId,
            Name = "Test User",
            Email = $"test{userId}@example.com",
            PasswordHash = "hash",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var course = new Shared.Entities.Course
        {
            Id = Guid.NewGuid(),
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
            CourseId = course.Id,
            Title = "Test Lesson",
            Content = "Test Content",
            OrderIndex = 1,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Users.Add(user);
        _context.Courses.Add(course);
        _context.Lessons.Add(lesson);
        _context.SaveChanges();

        // Act: Enroll user and complete a lesson
        var enrollment = _courseService.EnrollAsync(userId, course.Id).Result;
        var (success, _) = _courseService.CompleteLessonAsync(userId, lesson.Id).Result;

        // Assert: Enrollment exists and lesson completion is tracked
        Assert.NotNull(enrollment);
        Assert.Equal(userId, enrollment.UserId);
        Assert.Equal(course.Id, enrollment.CourseId);
        Assert.True(success);

        var completion = _context.LessonCompletions
            .FirstOrDefault(lc => lc.UserId == userId && lc.LessonId == lesson.Id);
        Assert.NotNull(completion);
    }

    /// <summary>
    /// Property 26: Lesson Content Delivery
    /// **Validates: Requirements 7.4**
    /// 
    /// For any lesson opened by a student, the Course Service should return the complete lesson content.
    /// </summary>
    [Property(MaxTest = 20)]
    public void LessonContentDelivery_ReturnsCompleteContent(NonEmptyString content)
    {
        // Sanitize content
        var sanitizedContent = new string(content.Get.Where(c => !char.IsControl(c) || c == ' ' || c == '\n').ToArray()).Trim();
        if (string.IsNullOrWhiteSpace(sanitizedContent))
            sanitizedContent = "Test Content";

        // Arrange: Create a course and lesson
        var course = new Shared.Entities.Course
        {
            Id = Guid.NewGuid(),
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
            CourseId = course.Id,
            Title = "Test Lesson",
            Content = sanitizedContent,
            OrderIndex = 1,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Courses.Add(course);
        _context.Lessons.Add(lesson);
        _context.SaveChanges();

        // Act: Retrieve lessons
        var lessons = _courseService.GetLessonsAsync(course.Id).Result;

        // Assert: Lesson content is complete
        Assert.NotEmpty(lessons);
        var retrievedLesson = lessons.First();
        Assert.Equal(sanitizedContent, retrievedLesson.Content);
        Assert.NotNull(retrievedLesson.Title);
        Assert.NotNull(retrievedLesson.Content);
    }

    /// <summary>
    /// Property 27: Lesson Completion Progression
    /// **Validates: Requirements 7.6**
    /// 
    /// For any lesson completion, the platform should mark it as complete and make 
    /// the next lesson (by order index) accessible.
    /// </summary>
    [Property(MaxTest = 20)]
    public void LessonCompletionProgression_UnlocksNextLesson()
    {
        // Arrange: Create a course with two lessons
        var userId = Guid.NewGuid();
        var user = new User
        {
            Id = userId,
            Name = "Test User",
            Email = $"test{userId}@example.com",
            PasswordHash = "hash",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var course = new Shared.Entities.Course
        {
            Id = Guid.NewGuid(),
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
            CourseId = course.Id,
            Title = "Lesson 1",
            Content = "Content 1",
            OrderIndex = 1,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var lesson2 = new Lesson
        {
            Id = Guid.NewGuid(),
            CourseId = course.Id,
            Title = "Lesson 2",
            Content = "Content 2",
            OrderIndex = 2,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Users.Add(user);
        _context.Courses.Add(course);
        _context.Lessons.AddRange(lesson1, lesson2);
        _context.SaveChanges();

        // Act: Complete first lesson
        var (success, nextLessonId) = _courseService.CompleteLessonAsync(userId, lesson1.Id).Result;

        // Assert: Lesson is marked complete and next lesson is returned
        Assert.True(success);
        Assert.NotNull(nextLessonId);
        Assert.Equal(lesson2.Id, nextLessonId);

        var completion = _context.LessonCompletions
            .FirstOrDefault(lc => lc.UserId == userId && lc.LessonId == lesson1.Id);
        Assert.NotNull(completion);
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
}
