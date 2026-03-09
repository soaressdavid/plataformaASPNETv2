using System.Text;
using Shared.ValueObjects;

namespace Shared.Parsers;

/// <summary>
/// Formats Content objects back into markdown format.
/// </summary>
public static class ContentPrinter
{
    /// <summary>
    /// Prints a Content object as markdown.
    /// </summary>
    public static string Print(Content content)
    {
        if (content == null)
        {
            throw new ArgumentNullException(nameof(content));
        }
        
        var sb = new StringBuilder();
        
        // Print title
        sb.AppendLine($"# {content.Title}");
        
        // Print description
        sb.AppendLine(content.Description);
        
        // Print code blocks
        foreach (var codeBlock in content.CodeBlocks)
        {
            sb.AppendLine();
            sb.AppendLine($"```{codeBlock.Language}");
            sb.AppendLine(codeBlock.Code);
            sb.AppendLine("```");
            
            if (!string.IsNullOrEmpty(codeBlock.Caption))
            {
                sb.AppendLine(codeBlock.Caption);
            }
        }
        
        // Print key points
        if (content.KeyPoints.Count > 0)
        {
            sb.AppendLine();
            sb.AppendLine("## Key Points");
            foreach (var point in content.KeyPoints)
            {
                sb.AppendLine($"- {point}");
            }
        }
        
        return sb.ToString();
    }
}
