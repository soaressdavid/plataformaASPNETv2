using Execution.Service.Services;
using FsCheck;
using FsCheck.Xunit;

namespace Execution.Tests;

/// <summary>
/// Property-based tests for prohibited code detection functionality.
/// Feature: aspnet-learning-platform
/// </summary>
public class ProhibitedCodeDetectionPropertiesTests
{
    private readonly ProhibitedCodeScanner _scanner;

    public ProhibitedCodeDetectionPropertiesTests()
    {
        _scanner = new ProhibitedCodeScanner();
    }

    /// <summary>
    /// Property 45: Prohibited Code Detection
    /// **Validates: Requirements 14.4, 14.5**
    /// 
    /// For any submitted code containing prohibited system calls (file I/O, network access, 
    /// process spawning), the platform should detect and reject it before execution.
    /// 
    /// This property verifies that code with prohibited operations is always detected.
    /// </summary>
    [Property(MaxTest = 100)]
    public void Property45_ProhibitedCodeDetection_DetectsFileIO(NonEmptyString fileName)
    {
        // Test various file I/O operations
        var fileOperations = new[]
        {
            "File.ReadAllText",
            "File.WriteAllText",
            "File.ReadAllLines",
            "File.WriteAllLines",
            "File.Delete",
            "File.Create",
            "File.Exists",
            "Directory.GetFiles",
            "Directory.Create",
            "Directory.Delete",
            "new FileStream",
            "new StreamReader",
            "new StreamWriter"
        };

        foreach (var operation in fileOperations)
        {
            // Arrange - Create code with file I/O operation
            var code = $@"
                using System;
                
                public class Program
                {{
                    public static void Main()
                    {{
                        var result = {operation}(""{fileName.Get}"");
                    }}
                }}";

            // Act
            var scanResult = _scanner.ScanCode(code);

            // Assert - Code should be detected as unsafe
            Assert.False(scanResult.IsSafe, $"Operation {operation} should be detected as unsafe");
            Assert.True(scanResult.Violations.Count > 0, $"Operation {operation} should have violations");
            Assert.True(scanResult.Violations.Any(v => 
                v.Reason.Contains("File system access") || 
                v.Reason.Contains("Prohibited")), 
                $"Operation {operation} should have file system violation");
        }
    }

    /// <summary>
    /// Property 45: Prohibited Code Detection - Network Access
    /// **Validates: Requirements 14.4, 14.5**
    /// 
    /// For any code containing network access operations, the scanner should detect it.
    /// </summary>
    [Property(MaxTest = 100)]
    public void Property45_ProhibitedCodeDetection_DetectsNetworkAccess()
    {
        // Test various network operations
        var networkOperations = new[]
        {
            "new HttpClient()",
            "new WebClient()",
            "new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)",
            "new TcpClient()",
            "new UdpClient()"
        };

        foreach (var operation in networkOperations)
        {
            // Arrange - Create code with network operation
            var code = $@"
                using System;
                
                public class Program
                {{
                    public static void Main()
                    {{
                        var client = {operation};
                    }}
                }}";

            // Act
            var scanResult = _scanner.ScanCode(code);

            // Assert - Code should be detected as unsafe
            Assert.False(scanResult.IsSafe, $"Operation {operation} should be detected as unsafe");
            Assert.True(scanResult.Violations.Count > 0, $"Operation {operation} should have violations");
            Assert.True(scanResult.Violations.Any(v => 
                v.Reason.Contains("Network access") || 
                v.Reason.Contains("Prohibited")),
                $"Operation {operation} should have network access violation");
        }
    }

    /// <summary>
    /// Property 45: Prohibited Code Detection - Process Spawning
    /// **Validates: Requirements 14.4, 14.5**
    /// 
    /// For any code containing process spawning operations, the scanner should detect it.
    /// </summary>
    [Property(MaxTest = 100)]
    public void Property45_ProhibitedCodeDetection_DetectsProcessSpawning(NonEmptyString processName)
    {
        // Test various process operations
        var processOperations = new[]
        {
            "Process.Start",
            "Process.Kill",
            "new Process()",
            "new ProcessStartInfo"
        };

        foreach (var operation in processOperations)
        {
            // Arrange - Create code with process operation
            var code = $@"
                using System;
                
                public class Program
                {{
                    public static void Main()
                    {{
                        var proc = {operation}(""{processName.Get}"");
                    }}
                }}";

            // Act
            var scanResult = _scanner.ScanCode(code);

            // Assert - Code should be detected as unsafe
            Assert.False(scanResult.IsSafe, $"Operation {operation} should be detected as unsafe");
            Assert.True(scanResult.Violations.Count > 0, $"Operation {operation} should have violations");
            Assert.True(scanResult.Violations.Any(v => 
                v.Reason.Contains("Process spawning") || 
                v.Reason.Contains("Prohibited")),
                $"Operation {operation} should have process spawning violation");
        }
    }

    /// <summary>
    /// Property: Safe Code Always Passes
    /// 
    /// For any code that doesn't contain prohibited operations, the scanner should 
    /// mark it as safe.
    /// </summary>
    [Property(MaxTest = 100)]
    public void SafeCode_AlwaysPasses(PositiveInt value)
    {
        // Test various safe operations
        var safeOperations = new[]
        {
            $"Console.WriteLine({value.Get})",
            $"Math.Sqrt({value.Get})",
            $"string.Format(\"{{0}}\", {value.Get})",
            $"int.Parse(\"{value.Get}\")",
            "DateTime.Now",
            "Guid.NewGuid()",
            "new List<int>()",
            "new Dictionary<string, int>()"
        };

        foreach (var operation in safeOperations)
        {
            // Arrange - Create code with safe operation
            var code = $@"
                using System;
                using System.Collections.Generic;
                
                public class Program
                {{
                    public static void Main()
                    {{
                        var result = {operation};
                    }}
                }}";

            // Act
            var scanResult = _scanner.ScanCode(code);

            // Assert - Code should be safe
            Assert.True(scanResult.IsSafe, $"Operation {operation} should be safe");
        }
    }

    /// <summary>
    /// Property: Prohibited Using Directives Are Detected
    /// 
    /// For any code that imports prohibited namespaces, the scanner should detect it.
    /// </summary>
    [Property(MaxTest = 100)]
    public void ProhibitedUsingDirectives_AreDetected()
    {
        // Test various prohibited namespaces
        var prohibitedNamespaces = new[]
        {
            "System.IO",
            "System.Net",
            "System.Net.Http",
            "System.Net.Sockets",
            "System.Diagnostics.Process"
        };

        foreach (var namespaceName in prohibitedNamespaces)
        {
            // Arrange - Create code with prohibited using directive
            var code = $@"
                using System;
                using {namespaceName};
                
                public class Program
                {{
                    public static void Main()
                    {{
                        Console.WriteLine(""Hello"");
                    }}
                }}";

            // Act
            var scanResult = _scanner.ScanCode(code);

            // Assert - Code should be detected as unsafe
            Assert.False(scanResult.IsSafe, $"Namespace {namespaceName} should be detected as unsafe");
            Assert.True(scanResult.Violations.Count > 0, $"Namespace {namespaceName} should have violations");
            Assert.True(scanResult.Violations.Any(v => 
                v.Operation.Contains(namespaceName) &&
                v.Reason.Contains("Prohibited namespace")),
                $"Namespace {namespaceName} should have prohibited namespace violation");
        }
    }

    /// <summary>
    /// Property: Violations Include Line Numbers
    /// 
    /// For any code with violations, each violation should include a valid line number.
    /// </summary>
    [Property(MaxTest = 100)]
    public void Violations_IncludeLineNumbers()
    {
        // Test various prohibited operations
        var prohibitedOps = new[]
        {
            "File.ReadAllText(\"test.txt\")",
            "new HttpClient()",
            "Process.Start(\"cmd\")"
        };

        foreach (var operation in prohibitedOps)
        {
            // Arrange - Create code with prohibited operation
            var code = $@"
                using System;
                
                public class Program
                {{
                    public static void Main()
                    {{
                        var x = {operation};
                    }}
                }}";

            // Act
            var scanResult = _scanner.ScanCode(code);

            // Assert - All violations should have positive line numbers
            Assert.False(scanResult.IsSafe, $"Operation {operation} should be detected as unsafe");
            Assert.All(scanResult.Violations, v => 
                Assert.True(v.Line > 0, $"Line number should be positive for {operation}"));
        }
    }

    /// <summary>
    /// Property: Multiple Violations Are All Detected
    /// 
    /// For any code with multiple prohibited operations, all should be detected.
    /// </summary>
    [Property(MaxTest = 100)]
    public void MultipleViolations_AllDetected(PositiveInt count)
    {
        // Limit count to reasonable range
        var violationCount = Math.Min(count.Get % 4 + 2, 5); // 2-5 violations

        // Arrange - Create code with multiple violations
        var operations = new[]
        {
            "File.ReadAllText(\"test.txt\")",
            "new HttpClient()",
            "Process.Start(\"cmd\")",
            "Directory.GetFiles(\".\")",
            "new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)"
        };

        var selectedOps = operations.Take(violationCount).ToList();
        var codeLines = string.Join("\n                ", 
            selectedOps.Select((op, idx) => $"var x{idx} = {op};"));

        var code = $@"
            using System;
            
            public class Program
            {{
                public static void Main()
                {{
                    {codeLines}
                }}
            }}";

        // Act
        var scanResult = _scanner.ScanCode(code);

        // Assert - Should detect multiple violations
        Assert.False(scanResult.IsSafe);
        Assert.True(scanResult.Violations.Count >= violationCount, 
            $"Should detect at least {violationCount} violations, but found {scanResult.Violations.Count}");
    }

    /// <summary>
    /// Property: Empty or Whitespace Code Is Safe
    /// 
    /// For any empty or whitespace-only code, the scanner should mark it as safe.
    /// </summary>
    [Property(MaxTest = 100)]
    public void EmptyCode_IsSafe()
    {
        // Test various empty/whitespace codes
        var whitespaceCodes = new[] { "", "   ", "\n", "\t", "  \n\t  " };

        foreach (var code in whitespaceCodes)
        {
            // Act
            var scanResult = _scanner.ScanCode(code);

            // Assert - Empty code should be safe
            Assert.True(scanResult.IsSafe, $"Empty/whitespace code should be safe");
            Assert.Empty(scanResult.Violations);
        }
    }
}
