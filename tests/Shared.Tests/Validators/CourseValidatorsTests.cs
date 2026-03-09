using FluentValidation.TestHelper;
using Shared.Validators;
using Xunit;

namespace Shared.Tests.Validators;

public class CourseValidatorsTests
{
    [Fact]
    public void CreateCourseRequestValidator_ValidData_ShouldPass()
    {
        // Arrange
        var validator = new CreateCourseRequestValidator();
        var request = new
        {
            Title = "Introduction to C#",
            Description = "Learn the basics of C# programming language",
            LevelId = Guid.NewGuid(),
            Order = 1
        };

        // Act
        var result = validator.TestValidate(request);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Theory]
    [InlineData("")]
    [InlineData("AB")]
    [InlineData(null)]
    public void CreateCourseRequestValidator_InvalidTitle_ShouldFail(string? title)
    {
        // Arrange
        var validator = new CreateCourseRequestValidator();
        var request = new
        {
            Title = title!,
            Description = "Valid description here",
            LevelId = Guid.NewGuid(),
            Order = 1
        };

        // Act
        var result = validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.GetType().GetProperty("Title")!.GetValue(x));
    }

    [Theory]
    [InlineData("")]
    [InlineData("Short")]
    [InlineData(null)]
    public void CreateCourseRequestValidator_InvalidDescription_ShouldFail(string? description)
    {
        // Arrange
        var validator = new CreateCourseRequestValidator();
        var request = new
        {
            Title = "Valid Title",
            Description = description!,
            LevelId = Guid.NewGuid(),
            Order = 1
        };

        // Act
        var result = validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.GetType().GetProperty("Description")!.GetValue(x));
    }

    [Fact]
    public void CreateCourseRequestValidator_EmptyLevelId_ShouldFail()
    {
        // Arrange
        var validator = new CreateCourseRequestValidator();
        var request = new
        {
            Title = "Valid Title",
            Description = "Valid description here",
            LevelId = Guid.Empty,
            Order = 1
        };

        // Act
        var result = validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.GetType().GetProperty("LevelId")!.GetValue(x));
    }

    [Fact]
    public void CreateCourseRequestValidator_NegativeOrder_ShouldFail()
    {
        // Arrange
        var validator = new CreateCourseRequestValidator();
        var request = new
        {
            Title = "Valid Title",
            Description = "Valid description here",
            LevelId = Guid.NewGuid(),
            Order = -1
        };

        // Act
        var result = validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.GetType().GetProperty("Order")!.GetValue(x));
    }
}

public class LessonValidatorsTests
{
    [Fact]
    public void CreateLessonRequestValidator_ValidData_ShouldPass()
    {
        // Arrange
        var validator = new CreateLessonRequestValidator();
        var request = new
        {
            Title = "Variables and Data Types",
            Description = "Learn about variables and data types in C#",
            CourseId = Guid.NewGuid(),
            Order = 1,
            Difficulty = "Beginner"
        };

        // Act
        var result = validator.TestValidate(request);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Theory]
    [InlineData("Easy")]
    [InlineData("Hard")]
    [InlineData("")]
    [InlineData(null)]
    public void CreateLessonRequestValidator_InvalidDifficulty_ShouldFail(string? difficulty)
    {
        // Arrange
        var validator = new CreateLessonRequestValidator();
        var request = new
        {
            Title = "Valid Title",
            Description = "Valid description here",
            CourseId = Guid.NewGuid(),
            Order = 1,
            Difficulty = difficulty!
        };

        // Act
        var result = validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.GetType().GetProperty("Difficulty")!.GetValue(x));
    }

    [Theory]
    [InlineData("Beginner")]
    [InlineData("Intermediate")]
    [InlineData("Advanced")]
    public void CreateLessonRequestValidator_ValidDifficulty_ShouldPass(string difficulty)
    {
        // Arrange
        var validator = new CreateLessonRequestValidator();
        var request = new
        {
            Title = "Valid Title",
            Description = "Valid description here",
            CourseId = Guid.NewGuid(),
            Order = 1,
            Difficulty = difficulty
        };

        // Act
        var result = validator.TestValidate(request);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}

public class LevelValidatorsTests
{
    [Fact]
    public void CreateLevelRequestValidator_ValidData_ShouldPass()
    {
        // Arrange
        var validator = new CreateLevelRequestValidator();
        var request = new
        {
            Number = 1,
            Title = "Beginner Level",
            Description = "Start your programming journey"
        };

        // Act
        var result = validator.TestValidate(request);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(21)]
    [InlineData(100)]
    public void CreateLevelRequestValidator_InvalidNumber_ShouldFail(int number)
    {
        // Arrange
        var validator = new CreateLevelRequestValidator();
        var request = new
        {
            Number = number,
            Title = "Valid Title",
            Description = "Valid description here"
        };

        // Act
        var result = validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.GetType().GetProperty("Number")!.GetValue(x));
    }

    [Theory]
    [InlineData(1)]
    [InlineData(10)]
    [InlineData(20)]
    public void CreateLevelRequestValidator_ValidNumber_ShouldPass(int number)
    {
        // Arrange
        var validator = new CreateLevelRequestValidator();
        var request = new
        {
            Number = number,
            Title = "Valid Title",
            Description = "Valid description here"
        };

        // Act
        var result = validator.TestValidate(request);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}
