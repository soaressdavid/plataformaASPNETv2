using Shared.Exceptions;
using Xunit;

namespace Shared.Tests.Exceptions;

public class NotFoundExceptionTests
{
    [Fact]
    public void Constructor_WithResourceTypeAndId_SetsPropertiesCorrectly()
    {
        // Arrange
        var resourceType = "Course";
        var resourceId = Guid.NewGuid();

        // Act
        var exception = new NotFoundException(resourceType, resourceId);

        // Assert
        Assert.Equal(resourceType, exception.ResourceType);
        Assert.Equal(resourceId, exception.ResourceId);
        Assert.Equal("RESOURCE_NOT_FOUND", exception.ErrorCode);
        Assert.Contains(resourceType, exception.Message);
        Assert.Contains(resourceId.ToString(), exception.Message);
        Assert.Equal(resourceType, exception.Metadata["resourceType"]);
        Assert.Equal(resourceId, exception.Metadata["resourceId"]);
    }

    [Fact]
    public void Constructor_WithCustomMessage_UsesCustomMessage()
    {
        // Arrange
        var customMessage = "Custom not found message";
        var resourceType = "Lesson";
        var resourceId = 123;

        // Act
        var exception = new NotFoundException(customMessage, resourceType, resourceId);

        // Assert
        Assert.Equal(customMessage, exception.Message);
        Assert.Equal(resourceType, exception.ResourceType);
        Assert.Equal(resourceId, exception.ResourceId);
    }
}

public class ValidationExceptionTests
{
    [Fact]
    public void Constructor_WithErrors_SetsPropertiesCorrectly()
    {
        // Arrange
        var errors = new Dictionary<string, string[]>
        {
            ["Title"] = new[] { "Title is required", "Title must be at least 3 characters" },
            ["Description"] = new[] { "Description is required" }
        };

        // Act
        var exception = new ValidationException(errors);

        // Assert
        Assert.Equal("VALIDATION_ERROR", exception.ErrorCode);
        Assert.Equal(errors, exception.Errors);
        Assert.Contains("validation errors", exception.Message.ToLower());
        Assert.Equal(errors, exception.Metadata["errors"]);
    }

    [Fact]
    public void Constructor_WithSingleError_CreatesErrorsDictionary()
    {
        // Arrange
        var propertyName = "Email";
        var errorMessage = "Email is invalid";

        // Act
        var exception = new ValidationException(propertyName, errorMessage);

        // Assert
        Assert.Equal("VALIDATION_ERROR", exception.ErrorCode);
        Assert.Single(exception.Errors);
        Assert.Contains(propertyName, exception.Errors.Keys);
        Assert.Equal(errorMessage, exception.Errors[propertyName][0]);
        Assert.Contains(propertyName, exception.Message);
        Assert.Contains(errorMessage, exception.Message);
    }
}

public class BusinessRuleExceptionTests
{
    [Fact]
    public void Constructor_WithRuleNameAndMessage_SetsPropertiesCorrectly()
    {
        // Arrange
        var ruleName = "MaxCoursesPerLevel";
        var message = "Cannot add more than 20 courses to a level";

        // Act
        var exception = new BusinessRuleException(ruleName, message);

        // Assert
        Assert.Equal(ruleName, exception.RuleName);
        Assert.Equal(message, exception.Message);
        Assert.Equal("BUSINESS_RULE_VIOLATION", exception.ErrorCode);
        Assert.Equal(ruleName, exception.Metadata["ruleName"]);
    }

    [Fact]
    public void Constructor_WithInnerException_PreservesInnerException()
    {
        // Arrange
        var ruleName = "TestRule";
        var message = "Test message";
        var innerException = new InvalidOperationException("Inner error");

        // Act
        var exception = new BusinessRuleException(ruleName, message, innerException);

        // Assert
        Assert.Equal(innerException, exception.InnerException);
        Assert.Equal(ruleName, exception.RuleName);
        Assert.Equal(message, exception.Message);
    }
}

public class DomainExceptionTests
{
    private class TestDomainException : DomainException
    {
        public TestDomainException(string message, string errorCode)
            : base(message, errorCode)
        {
        }
    }

    [Fact]
    public void AddMetadata_AddsKeyValuePair()
    {
        // Arrange
        var exception = new TestDomainException("Test message", "TEST_ERROR");
        var key = "testKey";
        var value = "testValue";

        // Act
        exception.AddMetadata(key, value);

        // Assert
        Assert.Contains(key, exception.Metadata.Keys);
        Assert.Equal(value, exception.Metadata[key]);
    }

    [Fact]
    public void AddMetadata_OverwritesExistingKey()
    {
        // Arrange
        var exception = new TestDomainException("Test message", "TEST_ERROR");
        var key = "testKey";
        var value1 = "value1";
        var value2 = "value2";

        // Act
        exception.AddMetadata(key, value1);
        exception.AddMetadata(key, value2);

        // Assert
        Assert.Equal(value2, exception.Metadata[key]);
    }

    [Fact]
    public void Constructor_InitializesEmptyMetadata()
    {
        // Arrange & Act
        var exception = new TestDomainException("Test message", "TEST_ERROR");

        // Assert
        Assert.NotNull(exception.Metadata);
        Assert.Empty(exception.Metadata);
    }
}
