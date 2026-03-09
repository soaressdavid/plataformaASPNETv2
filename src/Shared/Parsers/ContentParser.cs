using System.Text;
using System.Text.RegularExpressions;
using Shared.ValueObjects;

namespace Shared.Parsers;

/// <summary>
/// Parses lesson content from markdown format into structured Content objects.
/// </summary>
public static class ContentParser
{
    /// <summary>
    /// Parses markdown content into a structured Content object.
    /// Expected format:
    /// # Title
    /// Description text
    /// 
    /// ```language
    /// code
    /// ```
    /// Caption (optional)
    /// 
    /// ## Key Points
    /// - Point 1
    /// - Point 2
    /// </summary>
    public static Content Parse(string markdown)
    {
        if (string.IsNullOrWhiteSpace(markdown))
        {
            throw new ArgumentException("Content cannot be empty", nameof(markdown));
        }

        // Normalize line endings to \n
        markdown = markdown.Replace("\r\n", "\n").Replace("\r", "\n");
        var lines = markdown.Split('\n', StringSplitOptions.None);
        
        string? title = null;
        var descriptionLines = new List<string>();
        var codeBlocks = new List<CodeBlock>();
        var keyPoints = new List<string>();
        
        int i = 0;
        
        // Parse title (first line starting with #)
        while (i < lines.Length)
        {
            var line = lines[i].Trim();
            if (line.StartsWith("# "))
            {
                title = line.Substring(2).Trim();
                i++;
                break;
            }
            i++;
        }
        
        if (string.IsNullOrEmpty(title))
        {
            throw new FormatException("Content must have a title starting with '# '");
        }
        
        // Parse description (until first code block or key points section)
        while (i < lines.Length)
        {
            var line = lines[i];
            var trimmed = line.Trim();
            
            if (trimmed.StartsWith("```") || trimmed.StartsWith("## Key Points"))
            {
                break;
            }
            
            if (!string.IsNullOrWhiteSpace(trimmed) || descriptionLines.Count > 0)
            {
                descriptionLines.Add(line);
            }
            i++;
        }
        
        var description = string.Join("\n", descriptionLines).Trim();
        if (string.IsNullOrEmpty(description))
        {
            throw new FormatException("Content must have a description");
        }
        
        // Parse code blocks and key points
        while (i < lines.Length)
        {
            var line = lines[i].Trim();
            
            // Parse code block
            if (line.StartsWith("```"))
            {
                var language = line.Substring(3).Trim();
                if (string.IsNullOrEmpty(language))
                {
                    language = "text";
                }
                
                i++;
                var codeLines = new List<string>();
                
                while (i < lines.Length && !lines[i].Trim().StartsWith("```"))
                {
                    codeLines.Add(lines[i]);
                    i++;
                }
                
                if (i >= lines.Length)
                {
                    throw new FormatException("Unclosed code block");
                }
                
                var code = string.Join("\n", codeLines);
                i++; // Skip closing ```
                
                // Check for optional caption on next line
                string? caption = null;
                if (i < lines.Length)
                {
                    var nextLine = lines[i].Trim();
                    if (!string.IsNullOrEmpty(nextLine) && 
                        !nextLine.StartsWith("```") && 
                        !nextLine.StartsWith("## Key Points") &&
                        !nextLine.StartsWith("-"))
                    {
                        caption = nextLine;
                        i++;
                    }
                }
                
                codeBlocks.Add(new CodeBlock(language, code, caption));
            }
            // Parse key points section
            else if (line.StartsWith("## Key Points"))
            {
                i++;
                while (i < lines.Length)
                {
                    var pointLine = lines[i].Trim();
                    if (pointLine.StartsWith("- "))
                    {
                        keyPoints.Add(pointLine.Substring(2).Trim());
                    }
                    else if (!string.IsNullOrWhiteSpace(pointLine))
                    {
                        // Non-empty line that's not a bullet point - might be another section
                        break;
                    }
                    i++;
                }
            }
            else
            {
                i++;
            }
        }
        
        return new Content(title, description, codeBlocks, keyPoints);
    }
}
