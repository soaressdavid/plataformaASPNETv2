using Course.Service.Controllers;
using Course.Service.DTOs;
using Course.Service.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Shared.Data;
using Shared.Entities;

namespace Course.Tests.Controllers;

/// <summary>
/// Unit tests for LevelsController
/// </summary>
public class LevelsControllerTests : IDisposable
{
    private readonly ApplicationDbContext _context;
    private readonly Mock<ILogger<LevelsController>> _mockLogger;
    private readonly CachedLevelsService _cachedLevelsService;
    private readonly LevelsController _controller;

    public LevelsControllerTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new ApplicationDbContext(options);
        _mockLogger = new Mock<ILogger<LevelsController>>();
        
        // Create CachedLevelsService with in-memory cache
        var cacheLogger = new Mock<ILogger<CachedLevelsService>>();
        var cache = new Microsoft.Extensions.Caching.Memory.MemoryCache(
            new Microsoft.Extensions.Caching.Memory.MemoryCacheOptions());
        _cachedLevelsService = new CachedLevelsService(cache, _context, cacheLogger.Object);
        
        _controller = new LevelsController(_context, _cachedLevelsService, _mockLogger.Object);
    }

    [Fact]
    public async Task GetAll_ReturnsAllLevels_OrderedByNumber()
    {
        // Arrange
        var level1 = new CurriculumLevel
        {
            Id = Guid.NewGuid(),
            Number = 1,
            Title = "Level 1",
            Description = "First level",
            RequiredXP = 0,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var level2 = new CurriculumLevel
        {
            Id = Guid.NewGuid(),
            Number = 0,
            Title = "Level 0",
            Description = "Beginner level",
            RequiredXP = 0,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Set<CurriculumLevel>().AddRange(level2, level1); // Add out of order
        await _context.SaveChangesAsync();

        // Act
        var result = await _controller.GetAll();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var response = Assert.IsType<LevelListResponse>(okResult.Value);
        Assert.Equal(2, response.Levels.Count);
        Assert.Equal("Level 0", response.Levels[0].Title);
        Assert.Equal("Level 1", response.Levels[1].Title);
    }

    [Fact]
    public async Task GetAll_ReturnsEmptyList_WhenNoLevels()
    {
        // Act
        var result = await _controller.GetAll();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var response = Assert.IsType<LevelListResponse>(okResult.Value);
        Assert.Empty(response.Levels);
    }

    [Fact]
    public async Task GetAll_IncludesCourseCount()
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
            LevelId = levelId,
            OrderIndex = 2,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Set<CurriculumLevel>().Add(level);
        _context.Courses.AddRange(course1, course2);
        await _context.SaveChangesAsync();

        // Act
        var result = await _controller.GetAll();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var response = Assert.IsType<LevelListResponse>(okResult.Value);
        Assert.Single(response.Levels);
        Assert.Equal(2, response.Levels[0].CourseCount);
    }

    [Fact]
    public async Task GetById_ReturnsLevelDetails_WhenLevelExists()
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

        var course = new Shared.Entities.Course
        {
            Id = Guid.NewGuid(),
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
        var result = await _controller.GetById(levelId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var levelDetail = Assert.IsType<LevelDetailDto>(okResult.Value);
        Assert.Equal(levelId, levelDetail.Id);
        Assert.Equal("Level 0", levelDetail.Title);
        Assert.Single(levelDetail.Courses);
        Assert.Equal("Test Course", levelDetail.Courses[0].Title);
    }

    [Fact]
    public async Task GetById_ReturnsNotFound_WhenLevelDoesNotExist()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        // Act
        var result = await _controller.GetById(nonExistentId);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetById_IncludesProject_WhenProjectExists()
    {
        // Arrange
        var levelId = Guid.NewGuid();
        var projectId = Guid.NewGuid();
        
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

        var project = new Project
        {
            Id = projectId,
            Title = "Capstone Project",
            Description = "Final project",
            LevelId = levelId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Set<CurriculumLevel>().Add(level);
        _context.Projects.Add(project);
        await _context.SaveChangesAsync();

        // Act
        var result = await _controller.GetById(levelId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var levelDetail = Assert.IsType<LevelDetailDto>(okResult.Value);
        Assert.NotNull(levelDetail.Project);
        Assert.Equal("Capstone Project", levelDetail.Project.Title);
    }

    [Fact]
    public async Task GetCourses_ReturnsCoursesForLevel_OrderedByIndex()
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

        var course1 = new Shared.Entities.Course
        {
            Id = Guid.NewGuid(),
            Title = "Course 1",
            Description = "Description 1",
            Level = Level.Beginner,
            LevelId = levelId,
            OrderIndex = 2,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var course2 = new Shared.Entities.Course
        {
            Id = Guid.NewGuid(),
            Title = "Course 2",
            Description = "Description 2",
            Level = Level.Beginner,
            LevelId = levelId,
            OrderIndex = 1,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Set<CurriculumLevel>().Add(level);
        _context.Courses.AddRange(course1, course2);
        await _context.SaveChangesAsync();

        // Act
        var result = await _controller.GetCourses(levelId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var response = Assert.IsType<CourseListResponse>(okResult.Value);
        Assert.Equal(2, response.Courses.Count);
        Assert.Equal("Course 2", response.Courses[0].Title); // OrderIndex 1 comes first
        Assert.Equal("Course 1", response.Courses[1].Title); // OrderIndex 2 comes second
    }

    [Fact]
    public async Task GetCourses_ReturnsNotFound_WhenLevelDoesNotExist()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        // Act
        var result = await _controller.GetCourses(nonExistentId);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetCourses_ReturnsEmptyList_WhenLevelHasNoCourses()
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

        _context.Set<CurriculumLevel>().Add(level);
        await _context.SaveChangesAsync();

        // Act
        var result = await _controller.GetCourses(levelId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var response = Assert.IsType<CourseListResponse>(okResult.Value);
        Assert.Empty(response.Courses);
    }

    [Fact]
    public async Task InvalidateCache_ReturnsOk()
    {
        // Act
        var result = _controller.InvalidateCache();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.NotNull(okResult.Value);
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
}
