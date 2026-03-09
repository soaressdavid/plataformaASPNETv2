using FsCheck;
using FsCheck.Xunit;
using Shared.Parsers;
using Shared.ValueObjects;

namespace Challenge.Tests;

/// <summary>
/// Property-based tests for test case parser functionality.
/// Feature: aspnet-learning-platform
/// </summary>
public class TestCaseParserPropertiesTests
{
    /// <summary>
    /// Property 55: TestCase Parsing
    /// **Validates: Requirements 18.1**
    /// 
    /// For any valid test case data string, parsing it should produce a TestCaseData object 
    /// with input, expected output, and visibility flag.
    /// </summary>
    [Property(MaxTest = 20)]
    public void TestCaseParsing_ProducesValidTestCaseData(NonEmptyString input, NonEmptyString expectedOutput, bool isHidden)
    {
        // Arrange - use the printer to create a properly formatted string
        var testCaseData = new TestCaseData(input.Get, expectedOutput.Get, isHidden);
        var testCaseString = TestCasePrinter.Print(testCaseData);
        
        // Act
        var parsed = TestCaseParser.Parse(testCaseString);
        
        // Assert
        Assert.NotNull(parsed);
        Assert.Equal(input.Get, parsed.Input);
        Assert.Equal(expectedOutput.Get, parsed.ExpectedOutput);
        Assert.Equal(isHidden, parsed.IsHidden);
    }

    /// <summary>
    /// Property 56: Invalid TestCase Error
    /// **Validates: Requirements 18.2**
    /// 
    /// For any invalid test case format, the parser should return a descriptive error 
    /// indicating which field is invalid.
    /// </summary>
    [Property(MaxTest = 20)]
    public void TestCaseParsing_ThrowsForMissingInput()
    {
        // Arrange - missing INPUT field
        var testCaseString = "EXPECTED: output\nHIDDEN: false";
        
        // Act & Assert
        var exception = Assert.Throws<FormatException>(() => TestCaseParser.Parse(testCaseString));
        Assert.Contains("INPUT", exception.Message);
    }

    [Property(MaxTest = 20)]
    public void TestCaseParsing_ThrowsForMissingExpected()
    {
        // Arrange - missing EXPECTED field
        var testCaseString = "INPUT: input\nHIDDEN: false";
        
        // Act & Assert
        var exception = Assert.Throws<FormatException>(() => TestCaseParser.Parse(testCaseString));
        Assert.Contains("EXPECTED", exception.Message);
    }

    [Property(MaxTest = 20)]
    public void TestCaseParsing_ThrowsForMissingHidden()
    {
        // Arrange - missing HIDDEN field
        var testCaseString = "INPUT: input\nEXPECTED: output";
        
        // Act & Assert
        var exception = Assert.Throws<FormatException>(() => TestCaseParser.Parse(testCaseString));
        Assert.Contains("HIDDEN", exception.Message);
    }

    [Property(MaxTest = 20)]
    public void TestCaseParsing_ThrowsForInvalidHiddenValue(NonEmptyString invalidValue)
    {
        // Only test with values that are not "true" or "false"
        var value = invalidValue.Get.ToLower();
        if (value == "true" || value == "false")
            return;
        
        // Arrange - invalid HIDDEN field value
        var testCaseString = $"INPUT: input\nEXPECTED: output\nHIDDEN: {invalidValue.Get}";
        
        // Act & Assert
        var exception = Assert.Throws<FormatException>(() => TestCaseParser.Parse(testCaseString));
        Assert.Contains("HIDDEN", exception.Message);
    }

    [Property(MaxTest = 20)]
    public void TestCaseParsing_ThrowsForEmptyString()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => TestCaseParser.Parse(""));
        Assert.Throws<ArgumentException>(() => TestCaseParser.Parse("   "));
    }

    /// <summary>
    /// Property 57: TestCase Round Trip
    /// **Validates: Requirements 18.3, 18.4**
    /// 
    /// For any valid TestCaseData object, parsing then printing then parsing should produce 
    /// an equivalent TestCaseData object (parse(print(parse(x))) == parse(x)).
    /// </summary>
    [Property(MaxTest = 20)]
    public void TestCaseRoundTrip_PreservesStructure(NonEmptyString input, NonEmptyString expectedOutput, bool isHidden)
    {
        // Arrange
        var originalString = $"INPUT: {input.Get}\nEXPECTED: {expectedOutput.Get}\nHIDDEN: {isHidden.ToString().ToLower()}";
        
        // Act
        var parsed1 = TestCaseParser.Parse(originalString);
        var printed = TestCasePrinter.Print(parsed1);
        var parsed2 = TestCaseParser.Parse(printed);
        var reprinted = TestCasePrinter.Print(parsed2);
        var reparsed = TestCaseParser.Parse(reprinted);
        
        // Assert - all parsed versions should be equal
        Assert.Equal(parsed1.Input, parsed2.Input);
        Assert.Equal(parsed1.ExpectedOutput, parsed2.ExpectedOutput);
        Assert.Equal(parsed1.IsHidden, parsed2.IsHidden);
        
        Assert.Equal(parsed2.Input, reparsed.Input);
        Assert.Equal(parsed2.ExpectedOutput, reparsed.ExpectedOutput);
        Assert.Equal(parsed2.IsHidden, reparsed.IsHidden);
    }

    /// <summary>
    /// Property: Printer handles null input
    /// </summary>
    [Property(MaxTest = 20)]
    public void TestCasePrinter_ThrowsForNullInput()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => TestCasePrinter.Print(null!));
    }

    /// <summary>
    /// Property: Parser handles different line endings
    /// </summary>
    [Property(MaxTest = 20)]
    public void TestCaseParsing_HandlesVariousLineEndings(NonEmptyString input, NonEmptyString expectedOutput, bool isHidden)
    {
        // Arrange - test with different line endings
        var testCaseStringLF = $"INPUT: {input.Get}\nEXPECTED: {expectedOutput.Get}\nHIDDEN: {isHidden.ToString().ToLower()}";
        var testCaseStringCRLF = $"INPUT: {input.Get}\r\nEXPECTED: {expectedOutput.Get}\r\nHIDDEN: {isHidden.ToString().ToLower()}";
        var testCaseStringCR = $"INPUT: {input.Get}\rEXPECTED: {expectedOutput.Get}\rHIDDEN: {isHidden.ToString().ToLower()}";
        
        // Act
        var parsedLF = TestCaseParser.Parse(testCaseStringLF);
        var parsedCRLF = TestCaseParser.Parse(testCaseStringCRLF);
        var parsedCR = TestCaseParser.Parse(testCaseStringCR);
        
        // Assert - all should produce the same result
        Assert.Equal(parsedLF.Input, parsedCRLF.Input);
        Assert.Equal(parsedLF.ExpectedOutput, parsedCRLF.ExpectedOutput);
        Assert.Equal(parsedLF.IsHidden, parsedCRLF.IsHidden);
        
        Assert.Equal(parsedLF.Input, parsedCR.Input);
        Assert.Equal(parsedLF.ExpectedOutput, parsedCR.ExpectedOutput);
        Assert.Equal(parsedLF.IsHidden, parsedCR.IsHidden);
    }
}
