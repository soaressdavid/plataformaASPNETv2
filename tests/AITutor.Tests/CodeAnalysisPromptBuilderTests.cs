using AITutor.Service.Services;

namespace AITutor.Tests;

public class CodeAnalysisPromptBuilderTests
{
    private readonly CodeAnalysisPromptBuilder _builder;

    public CodeAnalysisPromptBuilderTests()
    {
        _builder = new CodeAnalysisPromptBuilder();
    }

    [Fact]
    public void BuildPrompt_WithValidCode_ReturnsFormattedPrompt()
    {
        // Arrange
        var code = "public class MyClass { }";

        // Act
        var result = _builder.BuildPrompt(code);

        // Assert
        Assert.NotNull(result);
        Assert.Contains("Please analyze the following C# code:", result);
        Assert.Contains("```csharp", result);
        Assert.Contains(code, result);
        Assert.Contains("```", result);
        Assert.Contains("Provide your analysis in the JSON format", result);
    }

    [Fact]
    public void BuildPrompt_WithCodeAndContext_IncludesContext()
    {
        // Arrange
        var code = "public class MyClass { }";
        var context = "This is a simple data model class";

        // Act
        var result = _builder.BuildPrompt(code, context);

        // Assert
        Assert.Contains("## Context", result);
        Assert.Contains(context, result);
        Assert.Contains(code, result);
    }

    [Fact]
    public void BuildPrompt_WithNullCode_ThrowsArgumentException()
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => _builder.BuildPrompt(null!));
        Assert.Equal("code", exception.ParamName);
    }

    [Fact]
    public void BuildPrompt_WithEmptyCode_ThrowsArgumentException()
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => _builder.BuildPrompt(""));
        Assert.Equal("code", exception.ParamName);
    }

    [Fact]
    public void BuildPrompt_WithWhitespaceCode_ThrowsArgumentException()
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => _builder.BuildPrompt("   "));
        Assert.Equal("code", exception.ParamName);
    }

    [Fact]
    public void BuildPrompt_WithNullContext_DoesNotIncludeContextSection()
    {
        // Arrange
        var code = "public class MyClass { }";

        // Act
        var result = _builder.BuildPrompt(code, null);

        // Assert
        Assert.DoesNotContain("## Context", result);
        Assert.Contains(code, result);
    }

    [Fact]
    public void BuildPrompt_WithEmptyContext_DoesNotIncludeContextSection()
    {
        // Arrange
        var code = "public class MyClass { }";

        // Act
        var result = _builder.BuildPrompt(code, "");

        // Assert
        Assert.DoesNotContain("## Context", result);
        Assert.Contains(code, result);
    }

    [Fact]
    public void GetSystemPrompt_ReturnsNonEmptyPrompt()
    {
        // Act
        var result = _builder.GetSystemPrompt();

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
    }

    [Fact]
    public void GetSystemPrompt_ContainsSOLIDPrinciples()
    {
        // Act
        var result = _builder.GetSystemPrompt();

        // Assert
        Assert.Contains("SOLID Principles", result);
        Assert.Contains("Single Responsibility Principle", result);
        Assert.Contains("Open/Closed Principle", result);
        Assert.Contains("Liskov Substitution Principle", result);
        Assert.Contains("Interface Segregation Principle", result);
        Assert.Contains("Dependency Inversion Principle", result);
    }

    [Fact]
    public void GetSystemPrompt_ContainsCleanArchitecture()
    {
        // Act
        var result = _builder.GetSystemPrompt();

        // Assert
        Assert.Contains("Clean Architecture", result);
        Assert.Contains("separation of concerns", result);
        Assert.Contains("dependency injection", result);
    }

    [Fact]
    public void GetSystemPrompt_ContainsSecurityGuidelines()
    {
        // Act
        var result = _builder.GetSystemPrompt();

        // Assert
        Assert.Contains("Security Vulnerabilities", result);
        Assert.Contains("SQL Injection", result);
        Assert.Contains("Cross-Site Scripting", result);
        Assert.Contains("Authentication Issues", result);
        Assert.Contains("Sensitive Data Exposure", result);
    }

    [Fact]
    public void GetSystemPrompt_ContainsPerformanceGuidelines()
    {
        // Act
        var result = _builder.GetSystemPrompt();

        // Assert
        Assert.Contains("Performance Issues", result);
        Assert.Contains("N+1 Query Problem", result);
        Assert.Contains("Memory Leaks", result);
        Assert.Contains("async/await", result);
    }

    [Fact]
    public void GetSystemPrompt_ContainsASPNetCoreConventions()
    {
        // Act
        var result = _builder.GetSystemPrompt();

        // Assert
        Assert.Contains("ASP.NET Core Conventions", result);
        Assert.Contains("Async/Await", result);
        Assert.Contains("Dependency Injection", result);
        Assert.Contains("Configuration", result);
        Assert.Contains("Logging", result);
        Assert.Contains("Error Handling", result);
    }

    [Fact]
    public void GetSystemPrompt_ContainsFeedbackFormat()
    {
        // Act
        var result = _builder.GetSystemPrompt();

        // Assert
        Assert.Contains("Feedback Format", result);
        Assert.Contains("suggestions", result);
        Assert.Contains("overallScore", result);
        Assert.Contains("securityIssues", result);
        Assert.Contains("performanceIssues", result);
    }

    [Fact]
    public void BuildMessages_WithValidCode_ReturnsTwoMessages()
    {
        // Arrange
        var code = "public class MyClass { }";

        // Act
        var result = _builder.BuildMessages(code);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public void BuildMessages_WithValidCode_FirstMessageIsSystem()
    {
        // Arrange
        var code = "public class MyClass { }";

        // Act
        var result = _builder.BuildMessages(code);

        // Assert
        Assert.Equal("system", result[0].Role);
        Assert.NotEmpty(result[0].Content);
        Assert.Contains("SOLID Principles", result[0].Content);
    }

    [Fact]
    public void BuildMessages_WithValidCode_SecondMessageIsUser()
    {
        // Arrange
        var code = "public class MyClass { }";

        // Act
        var result = _builder.BuildMessages(code);

        // Assert
        Assert.Equal("user", result[1].Role);
        Assert.Contains(code, result[1].Content);
    }

    [Fact]
    public void BuildMessages_WithCodeAndContext_UserMessageIncludesContext()
    {
        // Arrange
        var code = "public class MyClass { }";
        var context = "This is a data model";

        // Act
        var result = _builder.BuildMessages(code, context);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Contains(context, result[1].Content);
        Assert.Contains(code, result[1].Content);
    }

    [Fact]
    public void BuildMessages_WithNullCode_ThrowsArgumentException()
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => _builder.BuildMessages(null!));
        Assert.Equal("code", exception.ParamName);
    }

    [Fact]
    public void BuildMessages_SystemPromptContainsAllRequiredSections()
    {
        // Arrange
        var code = "public class MyClass { }";

        // Act
        var result = _builder.BuildMessages(code);
        var systemMessage = result[0].Content;

        // Assert - Validates Requirements 4.2, 4.3, 4.4, 4.5
        Assert.Contains("SOLID Principles", systemMessage);
        Assert.Contains("Clean Architecture", systemMessage);
        Assert.Contains("Security Vulnerabilities", systemMessage);
        Assert.Contains("Performance Issues", systemMessage);
        Assert.Contains("ASP.NET Core Conventions", systemMessage);
    }

    [Fact]
    public void BuildPrompt_WithMultilineCode_PreservesFormatting()
    {
        // Arrange
        var code = @"public class MyClass
{
    public int Id { get; set; }
    public string Name { get; set; }
}";

        // Act
        var result = _builder.BuildPrompt(code);

        // Assert
        Assert.Contains(code, result);
        Assert.Contains("```csharp", result);
    }

    [Fact]
    public void BuildPrompt_WithSpecialCharacters_HandlesCorrectly()
    {
        // Arrange
        var code = "var message = \"Hello, World!\"; // Comment with special chars: @#$%";

        // Act
        var result = _builder.BuildPrompt(code);

        // Assert
        Assert.Contains(code, result);
    }

    [Theory]
    [InlineData("public class Test { }")]
    [InlineData("var x = 42;")]
    [InlineData("Console.WriteLine(\"Hello\");")]
    public void BuildPrompt_WithVariousCodeSnippets_ReturnsValidPrompt(string code)
    {
        // Act
        var result = _builder.BuildPrompt(code);

        // Assert
        Assert.NotNull(result);
        Assert.Contains(code, result);
        Assert.Contains("```csharp", result);
    }

    [Fact]
    public void GetSystemPrompt_IsConsistent()
    {
        // Act
        var result1 = _builder.GetSystemPrompt();
        var result2 = _builder.GetSystemPrompt();

        // Assert
        Assert.Equal(result1, result2);
    }

    [Fact]
    public void BuildMessages_SystemPromptMatchesGetSystemPrompt()
    {
        // Arrange
        var code = "public class MyClass { }";

        // Act
        var messages = _builder.BuildMessages(code);
        var systemPrompt = _builder.GetSystemPrompt();

        // Assert
        Assert.Equal(systemPrompt, messages[0].Content);
    }
}
