namespace Shared.Interfaces;

/// <summary>
/// Interface for scanning code for prohibited operations
/// </summary>
public interface IProhibitedCodeScanner
{
    /// <summary>
    /// Scans code for prohibited operations
    /// </summary>
    CodeScanResult ScanCode(string code);
}

/// <summary>
/// Result of code scanning
/// </summary>
public class CodeScanResult
{
    public bool IsSafe { get; set; }
    public List<CodeViolation> Violations { get; set; } = new();
}

/// <summary>
/// Represents a code violation
/// </summary>
public class CodeViolation
{
    public int Line { get; set; }
    public string Operation { get; set; } = string.Empty;
    public string Reason { get; set; } = string.Empty;
}
