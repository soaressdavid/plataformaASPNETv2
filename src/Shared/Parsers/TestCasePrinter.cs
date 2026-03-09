using System.Text;
using Shared.ValueObjects;

namespace Shared.Parsers;

/// <summary>
/// Formats TestCaseData objects back into string format.
/// </summary>
public static class TestCasePrinter
{
    /// <summary>
    /// Prints a TestCaseData object as a formatted string.
    /// Special characters are escaped: \n, \r, \t, \\
    /// </summary>
    public static string Print(TestCaseData testCase)
    {
        if (testCase == null)
        {
            throw new ArgumentNullException(nameof(testCase));
        }
        
        var sb = new StringBuilder();
        
        sb.AppendLine($"INPUT: {Escape(testCase.Input)}");
        sb.AppendLine($"EXPECTED: {Escape(testCase.ExpectedOutput)}");
        sb.AppendLine($"HIDDEN: {testCase.IsHidden.ToString().ToLower()}");
        
        return sb.ToString();
    }
    
    private static string Escape(string value)
    {
        var sb = new StringBuilder();
        foreach (var ch in value)
        {
            switch (ch)
            {
                case '\n':
                    sb.Append("\\n");
                    break;
                case '\r':
                    sb.Append("\\r");
                    break;
                case '\t':
                    sb.Append("\\t");
                    break;
                case '\\':
                    sb.Append("\\\\");
                    break;
                default:
                    sb.Append(ch);
                    break;
            }
        }
        return sb.ToString();
    }
}
