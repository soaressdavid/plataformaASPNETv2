using FsCheck;
using FsCheck.Xunit;
using Xunit;
using Shared.Services;

namespace Execution.Tests;

/// <summary>
/// Property-based tests for malicious code detection
/// Property 21: Malicious Code Detection
/// Validates: Requirements 21.11
/// </summary>
public class MaliciousCodeDetectionPropertyTests
{
    /// <summary>
    /// Property: Code with Process.Start MUST be detected as prohibited
    /// </summary>
    [Theory]
    [InlineData("Process.Start(\"cmd.exe\")")]
    [InlineData("System.Diagnostics.Process.Start(\"notepad\")")]
    [InlineData("var p = Process.Start(\"calc\");")]
    [InlineData("Process.Start(new ProcessStartInfo(\"cmd\"))")]
    public void ProcessStart_MustBeDetectedAsProhibited(string maliciousCode)
    {
        var scanner = new ProhibitedCodeScanner();
        var result = scanner.ScanCode(maliciousCode);

        Assert.False(result.IsSafe, "Process.Start should be prohibited");
        Assert.NotEmpty(result.Violations);
        Assert.Contains(result.Violations, v => v.Reason.Contains("Process") || v.Reason.Contains("process"));
    }

    /// <summary>
    /// Property: Code with File.Delete MUST be detected as prohibited
    /// </summary>
    [Theory]
    [InlineData("File.Delete(\"test.txt\")")]
    [InlineData("System.IO.File.Delete(path)")]
    [InlineData("File.Delete(@\"C:\\temp\\file.txt\")")]
    public void FileDelete_MustBeDetectedAsProhibited(string maliciousCode)
    {
        var scanner = new ProhibitedCodeScanner();
        var result = scanner.ScanCode(maliciousCode);

        Assert.False(result.IsSafe, "File.Delete should be prohibited");
        Assert.NotEmpty(result.Violations);
    }

    /// <summary>
    /// Property: Code with network calls MUST be detected as prohibited
    /// </summary>
    [Theory]
    [InlineData("new HttpClient()")]
    [InlineData("HttpClient client = new HttpClient();")]
    [InlineData("var response = await client.GetAsync(\"http://example.com\");")]
    [InlineData("new TcpClient(\"localhost\", 8080)")]
    [InlineData("new WebClient().DownloadString(\"http://test.com\")")]
    public void NetworkCalls_MustBeDetectedAsProhibited(string maliciousCode)
    {
        var scanner = new ProhibitedCodeScanner();
        var result = scanner.ScanCode(maliciousCode);

        Assert.False(result.IsSafe, "Network calls should be prohibited");
        Assert.NotEmpty(result.Violations);
    }

    /// <summary>
    /// Property: Safe code MUST NOT be flagged as prohibited
    /// </summary>
    [Theory]
    [InlineData("Console.WriteLine(\"Hello World\");")]
    [InlineData("var x = 5 + 10;")]
    [InlineData("string name = \"John\";")]
    [InlineData("int[] numbers = { 1, 2, 3, 4, 5 };")]
    [InlineData("for(int i = 0; i < 10; i++) { }")]
    public void SafeCode_MustNotBeFlaggedAsProhibited(string safeCode)
    {
        var scanner = new ProhibitedCodeScanner();
        var result = scanner.ScanCode(safeCode);

        Assert.True(result.IsSafe, $"Safe code should be allowed: {safeCode}");
        Assert.Empty(result.Violations);
    }

    /// <summary>
    /// Property: Detection MUST be case-insensitive
    /// </summary>
    [Theory]
    [InlineData("process.start(\"cmd\")")]
    [InlineData("PROCESS.START(\"cmd\")")]
    [InlineData("Process.START(\"cmd\")")]
    public void Detection_MustBeCaseInsensitive(string maliciousCode)
    {
        var scanner = new ProhibitedCodeScanner();
        var result = scanner.ScanCode(maliciousCode);

        Assert.False(result.IsSafe, "Detection should be case-insensitive");
    }

    /// <summary>
    /// Property: Multiple violations MUST all be reported
    /// </summary>
    [Fact]
    public void MultipleViolations_MustAllBeReported()
    {
        var codeWithMultipleViolations = @"
using System;
using System.IO;
using System.Diagnostics;
using System.Net.Http;

class Program
{
    static void Main()
    {
        Process.Start(""cmd.exe"");
        File.Delete(""test.txt"");
        var client = new HttpClient();
    }
}";

        var scanner = new ProhibitedCodeScanner();
        var result = scanner.ScanCode(codeWithMultipleViolations);

        Assert.False(result.IsSafe);
        Assert.True(result.Violations.Count >= 3, 
            $"Should detect at least 3 violations, found {result.Violations.Count}");
    }

    /// <summary>
    /// Property: Detection MUST be consistent across multiple scans
    /// </summary>
    // TODO: Fix FsCheck syntax
    /*
    [Property(MaxTest = 50)]
    public Property Detection_MustBeConsistentAcrossScans()
    {
        return Prop.ForAll(
            Arb.Default.String(),
            code =>
            {
                if (string.IsNullOrWhiteSpace(code))
                    return true;

                var scanner = new ProhibitedCodeScanner();
                
                var result1 = scanner.ScanCode(code);
                var result2 = scanner.ScanCode(code);

                return result1.IsSafe == result2.IsSafe && result1.Violations.Count == result2.Violations.Count;
            });
    }
    */

    /// <summary>
    /// Property: Obfuscated malicious code SHOULD be detected
    /// </summary>
    [Theory]
    [InlineData("var cmd = \"Process\"; var method = \"Start\"; // Process.Start")]
    [InlineData("System.Diagnostics.Process.Start(\"cmd\")")]
    [InlineData("using static System.Diagnostics.Process; Start(\"cmd\");")]
    public void ObfuscatedMaliciousCode_ShouldBeDetected(string obfuscatedCode)
    {
        var scanner = new ProhibitedCodeScanner();
        var result = scanner.ScanCode(obfuscatedCode);

        // This is a best-effort check - some obfuscation may bypass detection
        // but common patterns should be caught
        if (obfuscatedCode.Contains("Process") && obfuscatedCode.Contains("Start"))
        {
            Assert.False(result.IsSafe, "Common obfuscation patterns should be detected");
        }
    }

    /// <summary>
    /// Property: Empty or null code MUST be allowed (handled elsewhere)
    /// </summary>
    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void EmptyOrNullCode_MustBeAllowed(string? code)
    {
        var scanner = new ProhibitedCodeScanner();
        var result = scanner.ScanCode(code ?? "");

        // Empty code is technically allowed by scanner (validation happens elsewhere)
        Assert.True(result.IsSafe || code == null);
    }
}


