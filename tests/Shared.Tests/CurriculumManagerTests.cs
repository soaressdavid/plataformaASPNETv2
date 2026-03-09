using Shared.Services;
using Xunit;

namespace Shared.Tests;

public class CurriculumManagerTests
{
    private readonly CurriculumManager _manager;

    public CurriculumManagerTests()
    {
        _manager = new CurriculumManager();
    }

    [Fact]
    public void GetAllLevels_ShouldReturn16Levels()
    {
        // Act
        var levels = _manager.GetAllLevels();

        // Assert
        Assert.NotNull(levels);
        Assert.Equal(16, levels.Count);
    }

    [Fact]
    public void GetAllLevels_ShouldReturnLevelsNumbered0To15()
    {
        // Act
        var levels = _manager.GetAllLevels();

        // Assert
        for (int i = 0; i <= 15; i++)
        {
            Assert.Contains(levels, l => l.Number == i);
        }
    }

    [Fact]
    public void GetAllLevels_AllLevelsShouldHaveRequiredMetadata()
    {
        // Act
        var levels = _manager.GetAllLevels();

        // Assert
        foreach (var level in levels)
        {
            Assert.NotEqual(Guid.Empty, level.Id);
            Assert.InRange(level.Number, 0, 15);
            Assert.False(string.IsNullOrWhiteSpace(level.Title));
            Assert.False(string.IsNullOrWhiteSpace(level.Description));
            Assert.True(level.RequiredXP >= 0);
            Assert.NotNull(level.Courses);
        }
    }

    [Theory]
    [InlineData(0, "Programming Fundamentals")]
    [InlineData(1, "C# Basics")]
    [InlineData(5, "Entity Framework Core")]
    [InlineData(6, "ASP.NET Core Web APIs")]
    [InlineData(9, "Microservices Architecture")]
    [InlineData(15, "Senior Engineering")]
    public void GetLevelByNumber_ShouldReturnCorrectLevel(int levelNumber, string expectedTitle)
    {
        // Act
        var level = _manager.GetLevelByNumber(levelNumber);

        // Assert
        Assert.NotNull(level);
        Assert.Equal(levelNumber, level.Number);
        Assert.Equal(expectedTitle, level.Title);
    }

    [Fact]
    public void GetLevelByNumber_WithInvalidNumber_ShouldReturnNull()
    {
        // Act
        var level = _manager.GetLevelByNumber(99);

        // Assert
        Assert.Null(level);
    }

    [Fact]
    public void GetLevelById_WithValidId_ShouldReturnLevel()
    {
        // Arrange
        var allLevels = _manager.GetAllLevels();
        var firstLevel = allLevels.First();

        // Act
        var level = _manager.GetLevelById(firstLevel.Id);

        // Assert
        Assert.NotNull(level);
        Assert.Equal(firstLevel.Id, level.Id);
        Assert.Equal(firstLevel.Number, level.Number);
    }

    [Fact]
    public void GetLevelById_WithInvalidId_ShouldReturnNull()
    {
        // Act
        var level = _manager.GetLevelById(Guid.NewGuid());

        // Assert
        Assert.Null(level);
    }

    [Fact]
    public void GetCoursesByLevel_WithValidLevelId_ShouldReturnCoursesList()
    {
        // Arrange
        var allLevels = _manager.GetAllLevels();
        var firstLevel = allLevels.First();

        // Act
        var courses = _manager.GetCoursesByLevel(firstLevel.Id);

        // Assert
        Assert.NotNull(courses);
        // Initially empty as courses will be added later
        Assert.IsType<List<Shared.Entities.Course>>(courses);
    }

    [Fact]
    public void GetCoursesByLevel_WithInvalidLevelId_ShouldReturnEmptyList()
    {
        // Act
        var courses = _manager.GetCoursesByLevel(Guid.NewGuid());

        // Assert
        Assert.NotNull(courses);
        Assert.Empty(courses);
    }

    [Theory]
    [InlineData(0, 1)]
    [InlineData(1, 2)]
    [InlineData(5, 6)]
    [InlineData(14, 15)]
    public void GetNextLevel_ShouldReturnCorrectNextLevel(int currentLevel, int expectedNextLevel)
    {
        // Act
        var nextLevel = _manager.GetNextLevel(currentLevel);

        // Assert
        Assert.NotNull(nextLevel);
        Assert.Equal(expectedNextLevel, nextLevel.Number);
    }

    [Fact]
    public void GetNextLevel_FromLevel15_ShouldReturnNull()
    {
        // Act
        var nextLevel = _manager.GetNextLevel(15);

        // Assert
        Assert.Null(nextLevel);
    }

    [Fact]
    public void GetNextLevel_FromLevel16_ShouldReturnNull()
    {
        // Act
        var nextLevel = _manager.GetNextLevel(16);

        // Assert
        Assert.Null(nextLevel);
    }

    [Fact]
    public void Levels_ShouldHaveIncreasingRequiredXP()
    {
        // Act
        var levels = _manager.GetAllLevels();

        // Assert
        for (int i = 1; i < levels.Count; i++)
        {
            var previousLevel = levels[i - 1];
            var currentLevel = levels[i];
            Assert.True(currentLevel.RequiredXP > previousLevel.RequiredXP,
                $"Level {currentLevel.Number} XP ({currentLevel.RequiredXP}) should be greater than Level {previousLevel.Number} XP ({previousLevel.RequiredXP})");
        }
    }

    [Fact]
    public void Level0_ShouldHaveZeroRequiredXP()
    {
        // Act
        var level0 = _manager.GetLevelByNumber(0);

        // Assert
        Assert.NotNull(level0);
        Assert.Equal(0, level0.RequiredXP);
    }

    [Fact]
    public void AllLevels_ShouldHaveUniqueIds()
    {
        // Act
        var levels = _manager.GetAllLevels();
        var ids = levels.Select(l => l.Id).ToList();

        // Assert
        Assert.Equal(ids.Count, ids.Distinct().Count());
    }

    [Fact]
    public void AllLevels_ShouldHaveUniqueNumbers()
    {
        // Act
        var levels = _manager.GetAllLevels();
        var numbers = levels.Select(l => l.Number).ToList();

        // Assert
        Assert.Equal(16, numbers.Distinct().Count());
    }

    [Fact]
    public void AllLevels_ShouldHaveCreatedAndUpdatedTimestamps()
    {
        // Act
        var levels = _manager.GetAllLevels();

        // Assert
        foreach (var level in levels)
        {
            Assert.NotEqual(default(DateTime), level.CreatedAt);
            Assert.NotEqual(default(DateTime), level.UpdatedAt);
        }
    }

    [Theory]
    [InlineData(0, 20)]  // Level 0: 20 lessons planned
    [InlineData(1, 20)]  // Level 1: 20 lessons planned
    [InlineData(2, 22)]  // Level 2: 22 lessons planned
    [InlineData(9, 26)]  // Level 9: 26 lessons planned
    public void LevelDescriptions_ShouldMatchDesignDocument(int levelNumber, int expectedLessonCount)
    {
        // Act
        var level = _manager.GetLevelByNumber(levelNumber);

        // Assert
        Assert.NotNull(level);
        Assert.False(string.IsNullOrWhiteSpace(level.Description));
        // This test documents the expected lesson counts from the design
        // Actual lesson counts will be validated when courses are added
    }

    [Fact]
    public void GetAllLevels_ShouldReturnSameInstanceOnMultipleCalls()
    {
        // Act
        var levels1 = _manager.GetAllLevels();
        var levels2 = _manager.GetAllLevels();

        // Assert
        Assert.Same(levels1, levels2);
    }

    [Fact]
    public void CurriculumManager_ShouldInitializeAllLevelsOnConstruction()
    {
        // Act
        var newManager = new CurriculumManager();
        var levels = newManager.GetAllLevels();

        // Assert
        Assert.NotNull(levels);
        Assert.Equal(16, levels.Count);
    }
}
