using Shared.Interfaces;
using System.Text.RegularExpressions;

namespace Shared.Services;

/// <summary>
/// Scans code for prohibited operations
/// Validates: Requirements 14.4, 14.5
/// </summary>
public class ProhibitedCodeScanner : IProhibitedCodeScanner
{
    private static readonly string[] ProhibitedNamespaces = new[]
    {
        "System.IO",
        "System.Net",
        "System.Net.Http",
        "System.Net.Sockets",
        "System.Diagnostics.Process"
    };

    private static readonly (string Pattern, string Reason)[] ProhibitedOperations = new[]
    {
        // Fully qualified namespace usage
        (@"\bSystem\.IO\.", "Prohibited namespace: System.IO"),
        (@"\bSystem\.Net\.", "Prohibited namespace: System.Net"),
        (@"\bSystem\.Net\.Http\.", "Prohibited namespace: System.Net.Http"),
        (@"\bSystem\.Net\.Sockets\.", "Prohibited namespace: System.Net.Sockets"),
        (@"\bSystem\.Diagnostics\.Process\.", "Prohibited namespace: System.Diagnostics.Process"),
        
        // File I/O operations (case-insensitive)
        (@"(?i)\bFile\.(ReadAllText|WriteAllText|ReadAllLines|WriteAllLines|Delete|Create|Exists|Open|Copy|Move|Replace|AppendAllText|ReadAllBytes|WriteAllBytes)", "File system access is prohibited"),
        (@"(?i)\bDirectory\.(GetFiles|Create|Delete|Exists|Move|GetDirectories|GetFileSystemEntries|EnumerateFiles)", "File system access is prohibited"),
        (@"(?i)\bnew\s+FileStream\s*\(", "File system access is prohibited"),
        (@"(?i)\bnew\s+StreamReader\s*\(", "File system access is prohibited"),
        (@"(?i)\bnew\s+StreamWriter\s*\(", "File system access is prohibited"),
        (@"(?i)\bFileInfo\b", "File system access is prohibited"),
        (@"(?i)\bDirectoryInfo\b", "File system access is prohibited"),
        (@"(?i)\bPath\.(GetFullPath|GetTempPath|GetTempFileName)", "File system access is prohibited"),
        
        // Network operations (case-insensitive)
        (@"(?i)\bnew\s+HttpClient\s*\(", "Network access is prohibited"),
        (@"(?i)\bnew\s+WebClient\s*\(", "Network access is prohibited"),
        (@"(?i)\bnew\s+Socket\s*\(", "Network access is prohibited"),
        (@"(?i)\bnew\s+TcpClient\s*\(", "Network access is prohibited"),
        (@"(?i)\bnew\s+UdpClient\s*\(", "Network access is prohibited"),
        (@"(?i)\bnew\s+TcpListener\s*\(", "Network access is prohibited"),
        (@"(?i)\bHttpClient\b", "Network access is prohibited"),
        (@"(?i)\bWebClient\b", "Network access is prohibited"),
        (@"(?i)\bSocket\b", "Network access is prohibited"),
        (@"(?i)\bTcpClient\b", "Network access is prohibited"),
        (@"(?i)\bUdpClient\b", "Network access is prohibited"),
        (@"(?i)\.GetAsync\s*\(", "Network access is prohibited"),
        (@"(?i)\.PostAsync\s*\(", "Network access is prohibited"),
        (@"(?i)\.PutAsync\s*\(", "Network access is prohibited"),
        (@"(?i)\.DeleteAsync\s*\(", "Network access is prohibited"),
        
        // Process operations (case-insensitive)
        (@"(?i)\bProcess\.(Start|Kill|GetProcesses|GetCurrentProcess)", "Process spawning is prohibited"),
        (@"(?i)\bnew\s+Process\s*\(", "Process spawning is prohibited"),
        (@"(?i)\bnew\s+ProcessStartInfo\s*\(", "Process spawning is prohibited"),
        (@"(?i)\bProcessStartInfo\b", "Process spawning is prohibited"),
        
        // Obfuscation detection - string concatenation of dangerous keywords
        (@"""Process"".*""Start""", "Potential obfuscated process spawning"),
        (@"""File"".*""Delete""", "Potential obfuscated file operation"),
        (@"""Http"".*""Client""", "Potential obfuscated network access")
    };

    public CodeScanResult ScanCode(string code)
    {
        // Handle empty or whitespace code
        if (string.IsNullOrWhiteSpace(code))
        {
            return new CodeScanResult { IsSafe = true };
        }

        var result = new CodeScanResult { IsSafe = true };
        var lines = code.Split('\n');

        // Check for prohibited using directives
        for (int i = 0; i < lines.Length; i++)
        {
            var line = lines[i].Trim();
            
            // Check for prohibited namespaces in using directives
            if (line.StartsWith("using "))
            {
                foreach (var ns in ProhibitedNamespaces)
                {
                    if (line.Contains(ns))
                    {
                        result.IsSafe = false;
                        result.Violations.Add(new CodeViolation
                        {
                            Line = i + 1,
                            Operation = ns,
                            Reason = $"Prohibited namespace: {ns}"
                        });
                    }
                }
            }
        }

        // Check for prohibited operations in code
        for (int i = 0; i < lines.Length; i++)
        {
            var line = lines[i];
            
            foreach (var (pattern, reason) in ProhibitedOperations)
            {
                var matches = Regex.Matches(line, pattern);
                foreach (Match match in matches)
                {
                    result.IsSafe = false;
                    result.Violations.Add(new CodeViolation
                    {
                        Line = i + 1,
                        Operation = match.Value,
                        Reason = reason
                    });
                }
            }
        }

        return result;
    }
}
