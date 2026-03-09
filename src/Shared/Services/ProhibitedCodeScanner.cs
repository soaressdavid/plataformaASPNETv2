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
        
        // File I/O operations
        (@"\bFile\.(ReadAllText|WriteAllText|ReadAllLines|WriteAllLines|Delete|Create|Exists|Open|Copy|Move|Replace|AppendAllText|ReadAllBytes|WriteAllBytes)", "File system access is prohibited"),
        (@"\bDirectory\.(GetFiles|Create|Delete|Exists|Move|GetDirectories|GetFileSystemEntries|EnumerateFiles)", "File system access is prohibited"),
        (@"\bnew\s+FileStream\s*\(", "File system access is prohibited"),
        (@"\bnew\s+StreamReader\s*\(", "File system access is prohibited"),
        (@"\bnew\s+StreamWriter\s*\(", "File system access is prohibited"),
        (@"\bFileInfo\b", "File system access is prohibited"),
        (@"\bDirectoryInfo\b", "File system access is prohibited"),
        (@"\bPath\.(GetFullPath|GetTempPath|GetTempFileName)", "File system access is prohibited"),
        
        // Network operations
        (@"\bnew\s+HttpClient\s*\(", "Network access is prohibited"),
        (@"\bnew\s+WebClient\s*\(", "Network access is prohibited"),
        (@"\bnew\s+Socket\s*\(", "Network access is prohibited"),
        (@"\bnew\s+TcpClient\s*\(", "Network access is prohibited"),
        (@"\bnew\s+UdpClient\s*\(", "Network access is prohibited"),
        (@"\bnew\s+TcpListener\s*\(", "Network access is prohibited"),
        (@"\bHttpClient\b", "Network access is prohibited"),
        (@"\bWebClient\b", "Network access is prohibited"),
        (@"\bSocket\b", "Network access is prohibited"),
        (@"\bTcpClient\b", "Network access is prohibited"),
        (@"\bUdpClient\b", "Network access is prohibited"),
        
        // Process operations
        (@"\bProcess\.(Start|Kill|GetProcesses|GetCurrentProcess)", "Process spawning is prohibited"),
        (@"\bnew\s+Process\s*\(", "Process spawning is prohibited"),
        (@"\bnew\s+ProcessStartInfo\s*\(", "Process spawning is prohibited"),
        (@"\bProcessStartInfo\b", "Process spawning is prohibited")
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
