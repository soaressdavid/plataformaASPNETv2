using Execution.Service.Services;

namespace Execution.Tests;

/// <summary>
/// Unit tests for ProhibitedCodeScanner
/// Feature: aspnet-learning-platform
/// </summary>
public class ProhibitedCodeScannerTests
{
    private readonly ProhibitedCodeScanner _scanner;

    public ProhibitedCodeScannerTests()
    {
        _scanner = new ProhibitedCodeScanner();
    }

    [Fact]
    public void ScanCode_SafeCode_ReturnsSafe()
    {
        // Arrange
        var safeCode = @"
            using System;
            
            public class Program
            {
                public static void Main()
                {
                    Console.WriteLine(""Hello World"");
                    var x = 5 + 10;
                    Console.WriteLine(x);
                }
            }";

        // Act
        var result = _scanner.ScanCode(safeCode);

        // Assert
        Assert.True(result.IsSafe);
        Assert.Empty(result.Violations);
    }

    [Fact]
    public void ScanCode_FileIOUsing_DetectsViolation()
    {
        // Arrange
        var dangerousCode = @"
            using System;
            using System.IO;
            
            public class Program
            {
                public static void Main()
                {
                    Console.WriteLine(""Hello"");
                }
            }";

        // Act
        var result = _scanner.ScanCode(dangerousCode);

        // Assert
        Assert.False(result.IsSafe);
        Assert.Contains(result.Violations, v => v.Operation.Contains("System.IO"));
        Assert.Contains(result.Violations, v => v.Reason.Contains("Prohibited namespace"));
    }

    [Fact]
    public void ScanCode_FileReadAllText_DetectsViolation()
    {
        // Arrange
        var dangerousCode = @"
            using System;
            
            public class Program
            {
                public static void Main()
                {
                    var content = File.ReadAllText(""test.txt"");
                    Console.WriteLine(content);
                }
            }";

        // Act
        var result = _scanner.ScanCode(dangerousCode);

        // Assert
        Assert.False(result.IsSafe);
        Assert.Contains(result.Violations, v => v.Operation.Contains("File"));
        Assert.Contains(result.Violations, v => v.Reason.Contains("File system access"));
    }

    [Fact]
    public void ScanCode_NetworkAccess_DetectsViolation()
    {
        // Arrange
        var dangerousCode = @"
            using System;
            
            public class Program
            {
                public static void Main()
                {
                    var client = new HttpClient();
                    var response = client.GetAsync(""http://example.com"");
                }
            }";

        // Act
        var result = _scanner.ScanCode(dangerousCode);

        // Assert
        Assert.False(result.IsSafe);
        Assert.Contains(result.Violations, v => 
            v.Operation.Contains("HttpClient") || v.Operation.Contains("GetAsync"));
        Assert.Contains(result.Violations, v => v.Reason.Contains("Network access"));
    }

    [Fact]
    public void ScanCode_ProcessStart_DetectsViolation()
    {
        // Arrange
        var dangerousCode = @"
            using System;
            
            public class Program
            {
                public static void Main()
                {
                    Process.Start(""cmd.exe"");
                }
            }";

        // Act
        var result = _scanner.ScanCode(dangerousCode);

        // Assert
        Assert.False(result.IsSafe);
        Assert.Contains(result.Violations, v => v.Operation.Contains("Process"));
        Assert.Contains(result.Violations, v => v.Reason.Contains("Process spawning"));
    }

    [Fact]
    public void ScanCode_FileStreamCreation_DetectsViolation()
    {
        // Arrange
        var dangerousCode = @"
            using System;
            
            public class Program
            {
                public static void Main()
                {
                    var stream = new FileStream(""test.txt"", FileMode.Open);
                }
            }";

        // Act
        var result = _scanner.ScanCode(dangerousCode);

        // Assert
        Assert.False(result.IsSafe);
        Assert.Contains(result.Violations, v => v.Operation.Contains("FileStream"));
        Assert.Contains(result.Violations, v => v.Reason.Contains("File system access"));
    }

    [Fact]
    public void ScanCode_DirectoryOperations_DetectsViolation()
    {
        // Arrange
        var dangerousCode = @"
            using System;
            
            public class Program
            {
                public static void Main()
                {
                    var files = Directory.GetFiles(""C:\\"");
                }
            }";

        // Act
        var result = _scanner.ScanCode(dangerousCode);

        // Assert
        Assert.False(result.IsSafe);
        Assert.Contains(result.Violations, v => v.Operation.Contains("Directory"));
        Assert.Contains(result.Violations, v => v.Reason.Contains("File system access"));
    }

    [Fact]
    public void ScanCode_SocketCreation_DetectsViolation()
    {
        // Arrange
        var dangerousCode = @"
            using System;
            
            public class Program
            {
                public static void Main()
                {
                    var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                }
            }";

        // Act
        var result = _scanner.ScanCode(dangerousCode);

        // Assert
        Assert.False(result.IsSafe);
        Assert.Contains(result.Violations, v => v.Operation.Contains("Socket"));
        Assert.Contains(result.Violations, v => v.Reason.Contains("Network access"));
    }

    [Fact]
    public void ScanCode_FullyQualifiedFileAccess_DetectsViolation()
    {
        // Arrange
        var dangerousCode = @"
            public class Program
            {
                public static void Main()
                {
                    var content = System.IO.File.ReadAllText(""test.txt"");
                }
            }";

        // Act
        var result = _scanner.ScanCode(dangerousCode);

        // Assert
        Assert.False(result.IsSafe);
        Assert.Contains(result.Violations, v => v.Operation.Contains("System.IO"));
    }

    [Fact]
    public void ScanCode_MultipleViolations_DetectsAll()
    {
        // Arrange
        var dangerousCode = @"
            using System.IO;
            using System.Net;
            
            public class Program
            {
                public static void Main()
                {
                    File.ReadAllText(""test.txt"");
                    var client = new HttpClient();
                    Process.Start(""cmd.exe"");
                }
            }";

        // Act
        var result = _scanner.ScanCode(dangerousCode);

        // Assert
        Assert.False(result.IsSafe);
        Assert.True(result.Violations.Count >= 3, "Should detect multiple violations");
    }

    [Fact]
    public void ScanCode_EmptyCode_ReturnsSafe()
    {
        // Arrange
        var emptyCode = "";

        // Act
        var result = _scanner.ScanCode(emptyCode);

        // Assert
        Assert.True(result.IsSafe);
        Assert.Empty(result.Violations);
    }

    [Fact]
    public void ScanCode_WhitespaceOnly_ReturnsSafe()
    {
        // Arrange
        var whitespaceCode = "   \n\t  \n  ";

        // Act
        var result = _scanner.ScanCode(whitespaceCode);

        // Assert
        Assert.True(result.IsSafe);
        Assert.Empty(result.Violations);
    }

    [Fact]
    public void ScanCode_ViolationIncludesLineNumber()
    {
        // Arrange
        var dangerousCode = @"
            using System;
            
            public class Program
            {
                public static void Main()
                {
                    File.ReadAllText(""test.txt"");
                }
            }";

        // Act
        var result = _scanner.ScanCode(dangerousCode);

        // Assert
        Assert.False(result.IsSafe);
        Assert.All(result.Violations, v => Assert.True(v.Line > 0, "Line number should be positive"));
    }

    [Fact]
    public void ScanCode_StreamReaderCreation_DetectsViolation()
    {
        // Arrange
        var dangerousCode = @"
            using System;
            
            public class Program
            {
                public static void Main()
                {
                    var reader = new StreamReader(""test.txt"");
                }
            }";

        // Act
        var result = _scanner.ScanCode(dangerousCode);

        // Assert
        Assert.False(result.IsSafe);
        Assert.Contains(result.Violations, v => v.Operation.Contains("StreamReader"));
        Assert.Contains(result.Violations, v => v.Reason.Contains("File system access"));
    }

    [Fact]
    public void ScanCode_WebClientUsage_DetectsViolation()
    {
        // Arrange
        var dangerousCode = @"
            using System;
            
            public class Program
            {
                public static void Main()
                {
                    var client = new WebClient();
                    client.DownloadString(""http://example.com"");
                }
            }";

        // Act
        var result = _scanner.ScanCode(dangerousCode);

        // Assert
        Assert.False(result.IsSafe);
        Assert.Contains(result.Violations, v => v.Operation.Contains("WebClient"));
        Assert.Contains(result.Violations, v => v.Reason.Contains("Network access"));
    }

    [Fact]
    public void ScanCode_SafeMathOperations_ReturnsSafe()
    {
        // Arrange
        var safeCode = @"
            using System;
            using System.Linq;
            using System.Collections.Generic;
            
            public class Program
            {
                public static void Main()
                {
                    var numbers = new List<int> { 1, 2, 3, 4, 5 };
                    var sum = numbers.Sum();
                    var average = numbers.Average();
                    Console.WriteLine($""Sum: {sum}, Average: {average}"");
                }
            }";

        // Act
        var result = _scanner.ScanCode(safeCode);

        // Assert
        Assert.True(result.IsSafe);
        Assert.Empty(result.Violations);
    }

    [Fact]
    public void ScanCode_SafeStringOperations_ReturnsSafe()
    {
        // Arrange
        var safeCode = @"
            using System;
            
            public class Program
            {
                public static void Main()
                {
                    var text = ""Hello World"";
                    var upper = text.ToUpper();
                    var lower = text.ToLower();
                    var length = text.Length;
                    Console.WriteLine($""{upper} - {lower} - {length}"");
                }
            }";

        // Act
        var result = _scanner.ScanCode(safeCode);

        // Assert
        Assert.True(result.IsSafe);
        Assert.Empty(result.Violations);
    }
}
