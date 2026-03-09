using System.Text;
using Shared.ValueObjects;

namespace Shared.Parsers;

/// <summary>
/// Parses test case data from string format into structured TestCaseData objects.
/// Uses a simple escape mechanism to handle special characters in values.
/// </summary>
public static class TestCaseParser
{
    /// <summary>
    /// Parses test case data string into a TestCaseData object.
    /// Expected format:
    /// INPUT: input_value
    /// EXPECTED: expected_output_value
    /// HIDDEN: true|false
    /// 
    /// Special characters in values are escaped: \n, \r, \t, \\
    /// </summary>
    public static TestCaseData Parse(string testCaseString)
    {
        if (string.IsNullOrWhiteSpace(testCaseString))
        {
            throw new ArgumentException("Test case data cannot be empty", nameof(testCaseString));
        }

        // Normalize line endings to \n for parsing
        testCaseString = testCaseString.Replace("\r\n", "\n").Replace("\r", "\n");
        var lines = testCaseString.Split('\n');
        
        string? input = null;
        string? expectedOutput = null;
        bool? isHidden = null;
        
        foreach (var line in lines)
        {
            // Skip empty lines
            if (string.IsNullOrEmpty(line))
                continue;
            
            // Check for INPUT: (case insensitive)
            var inputIndex = line.IndexOf("INPUT:", StringComparison.OrdinalIgnoreCase);
            if (inputIndex >= 0)
            {
                // Extract everything after "INPUT:"
                var value = line.Substring(inputIndex + 6);
                // Only remove a single leading space if present
                value = value.StartsWith(" ") ? value.Substring(1) : value;
                // Unescape special characters
                input = Unescape(value);
                continue;
            }
            
            // Check for EXPECTED: (case insensitive)
            var expectedIndex = line.IndexOf("EXPECTED:", StringComparison.OrdinalIgnoreCase);
            if (expectedIndex >= 0)
            {
                // Extract everything after "EXPECTED:"
                var value = line.Substring(expectedIndex + 9);
                // Only remove a single leading space if present
                value = value.StartsWith(" ") ? value.Substring(1) : value;
                // Unescape special characters
                expectedOutput = Unescape(value);
                continue;
            }
            
            // Check for HIDDEN: (case insensitive)
            var hiddenIndex = line.IndexOf("HIDDEN:", StringComparison.OrdinalIgnoreCase);
            if (hiddenIndex >= 0)
            {
                var hiddenValue = line.Substring(hiddenIndex + 7).Trim();
                if (!bool.TryParse(hiddenValue, out var parsedHidden))
                {
                    throw new FormatException($"Invalid HIDDEN field value: '{hiddenValue}'. Expected 'true' or 'false'.");
                }
                isHidden = parsedHidden;
                continue;
            }
        }
        
        // Validate required fields
        if (input == null)
        {
            throw new FormatException("Test case data must have an INPUT field");
        }
        
        if (expectedOutput == null)
        {
            throw new FormatException("Test case data must have an EXPECTED field");
        }
        
        if (isHidden == null)
        {
            throw new FormatException("Test case data must have a HIDDEN field");
        }
        
        return new TestCaseData(input, expectedOutput, isHidden.Value);
    }
    
    private static string Unescape(string value)
    {
        var sb = new StringBuilder();
        for (int i = 0; i < value.Length; i++)
        {
            if (value[i] == '\\' && i + 1 < value.Length)
            {
                switch (value[i + 1])
                {
                    case 'n':
                        sb.Append('\n');
                        i++;
                        break;
                    case 'r':
                        sb.Append('\r');
                        i++;
                        break;
                    case 't':
                        sb.Append('\t');
                        i++;
                        break;
                    case '\\':
                        sb.Append('\\');
                        i++;
                        break;
                    default:
                        sb.Append(value[i]);
                        break;
                }
            }
            else
            {
                sb.Append(value[i]);
            }
        }
        return sb.ToString();
    }
}
