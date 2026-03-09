using FsCheck;
using FsCheck.Xunit;
using Shared.Parsers;
using Shared.ValueObjects;

namespace Course.Tests;

/// <summary>
/// Property-based tests for content parser functionality.
/// Feature: aspnet-learning-platform
/// </summary>
public class ContentParserPropertiesTests
{
    /// <summary>
    /// Property 52: Content Parsing
    /// **Validates: Requirements 17.1**
    /// 
    /// For any valid lesson content string, parsing it should produce a Content object 
    /// with title, description, code blocks, and key points.
    /// </summary>
    [Property(MaxTest = 20)]
    public void ContentParsing_ProducesValidContentObject(NonEmptyString title, NonEmptyString description)
    {
        // Sanitize inputs to remove newlines and control characters
        var sanitizedTitle = SanitizeString(title.Get);
        var sanitizedDescription = SanitizeString(description.Get);
        
        // Arrange: Create valid markdown content
        var markdown = $"# {sanitizedTitle}\n{sanitizedDescription}\n\n## Key Points\n- Point 1\n- Point 2";
        
        // Act: Parse the content
        var content = ContentParser.Parse(markdown);
        
        // Assert: Verify all required fields are present
        Assert.NotNull(content);
        Assert.Equal(sanitizedTitle, content.Title);
        Assert.Equal(sanitizedDescription, content.Description);
        Assert.NotNull(content.CodeBlocks);
        Assert.NotNull(content.KeyPoints);
        Assert.Equal(2, content.KeyPoints.Count);
    }
    
    private static string SanitizeString(string input)
    {
        // Remove newlines and control characters that would break markdown structure
        var sanitized = new string(input.Where(c => !char.IsControl(c) || c == ' ').ToArray()).Trim();
        // If sanitization results in empty string, return a default value
        return string.IsNullOrWhiteSpace(sanitized) ? "a" : sanitized;
    }

    /// <summary>
    /// Property 53: Invalid Content Error
    /// **Validates: Requirements 17.2**
    /// 
    /// For any invalid lesson content format, the parser should return a descriptive 
    /// error indicating what is invalid.
    /// </summary>
    [Property(MaxTest = 20)]
    public void ContentParsing_ThrowsForInvalidFormat()
    {
        // Test 1: Empty content
        Assert.Throws<ArgumentException>(() => ContentParser.Parse(""));
        Assert.Throws<ArgumentException>(() => ContentParser.Parse("   "));
        
        // Test 2: Missing title
        Assert.Throws<FormatException>(() => ContentParser.Parse("Some description without title"));
        
        // Test 3: Missing description
        Assert.Throws<FormatException>(() => ContentParser.Parse("# Title Only"));
    }

    /// <summary>
    /// Property 54: Content Round Trip
    /// **Validates: Requirements 17.3, 17.4**
    /// 
    /// For any valid Content object, parsing then printing then parsing should produce 
    /// an equivalent Content object (parse(print(parse(x))) == parse(x)).
    /// </summary>
    [Property(MaxTest = 20)]
    public void ContentRoundTrip_PreservesStructure(NonEmptyString title, NonEmptyString description)
    {
        // Sanitize inputs to remove newlines and control characters
        var sanitizedTitle = SanitizeString(title.Get);
        var sanitizedDescription = SanitizeString(description.Get);
        
        // Arrange: Create valid markdown content
        var markdown = $"# {sanitizedTitle}\n{sanitizedDescription}\n\n```csharp\nvar x = 1;\n```\nExample code\n\n## Key Points\n- Point 1\n- Point 2";
        
        // Act: Parse, print, and parse again
        var parsed1 = ContentParser.Parse(markdown);
        var printed = ContentPrinter.Print(parsed1);
        var parsed2 = ContentParser.Parse(printed);
        
        // Assert: Both parsed objects should be equivalent
        Assert.Equal(parsed1.Title, parsed2.Title);
        Assert.Equal(parsed1.Description, parsed2.Description);
        Assert.Equal(parsed1.CodeBlocks.Count, parsed2.CodeBlocks.Count);
        Assert.Equal(parsed1.KeyPoints.Count, parsed2.KeyPoints.Count);
        
        // Verify code blocks are equivalent
        for (int i = 0; i < parsed1.CodeBlocks.Count; i++)
        {
            Assert.Equal(parsed1.CodeBlocks[i].Language, parsed2.CodeBlocks[i].Language);
            Assert.Equal(parsed1.CodeBlocks[i].Code, parsed2.CodeBlocks[i].Code);
            Assert.Equal(parsed1.CodeBlocks[i].Caption, parsed2.CodeBlocks[i].Caption);
        }
        
        // Verify key points are equivalent
        for (int i = 0; i < parsed1.KeyPoints.Count; i++)
        {
            Assert.Equal(parsed1.KeyPoints[i], parsed2.KeyPoints[i]);
        }
    }

    /// <summary>
    /// Additional test: Content with multiple code blocks
    /// </summary>
    [Fact]
    public void ContentParsing_HandlesMultipleCodeBlocks()
    {
        var markdown = @"# Test Title
This is a description.

```csharp
var x = 1;
```
First example

```javascript
const y = 2;
```
Second example

## Key Points
- Point 1
- Point 2";

        var content = ContentParser.Parse(markdown);
        
        Assert.Equal("Test Title", content.Title);
        Assert.Equal("This is a description.", content.Description);
        Assert.Equal(2, content.CodeBlocks.Count);
        Assert.Equal("csharp", content.CodeBlocks[0].Language);
        Assert.Equal("First example", content.CodeBlocks[0].Caption);
        Assert.Equal("javascript", content.CodeBlocks[1].Language);
        Assert.Equal("Second example", content.CodeBlocks[1].Caption);
        Assert.Equal(2, content.KeyPoints.Count);
    }

    /// <summary>
    /// Additional test: Content without code blocks
    /// </summary>
    [Fact]
    public void ContentParsing_HandlesContentWithoutCodeBlocks()
    {
        var markdown = @"# Test Title
This is a description.

## Key Points
- Point 1
- Point 2";

        var content = ContentParser.Parse(markdown);
        
        Assert.Equal("Test Title", content.Title);
        Assert.Equal("This is a description.", content.Description);
        Assert.Empty(content.CodeBlocks);
        Assert.Equal(2, content.KeyPoints.Count);
    }

    /// <summary>
    /// Additional test: Content without key points
    /// </summary>
    [Fact]
    public void ContentParsing_HandlesContentWithoutKeyPoints()
    {
        var markdown = @"# Test Title
This is a description.

```csharp
var x = 1;
```";

        var content = ContentParser.Parse(markdown);
        
        Assert.Equal("Test Title", content.Title);
        Assert.Equal("This is a description.", content.Description);
        Assert.Single(content.CodeBlocks);
        Assert.Empty(content.KeyPoints);
    }
}
