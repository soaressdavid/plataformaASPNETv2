using Shared.Entities;

namespace Shared.Services;

/// <summary>
/// Executes code against test cases with timeout handling.
/// </summary>
public class TestCaseExecutor
{
    private readonly TimeSpan _timeout;

    public TestCaseExecutor(TimeSpan? timeout = null)
    {
        _timeout = timeout ?? TimeSpan.FromSeconds(5);
    }

    /// <summary>
    /// Executes a single test case against the provided code.
    /// </summary>
    /// <param name="code">The code to execute</param>
    /// <param name="testCase">The test case to run</param>
    /// <returns>The test result</returns>
    public async Task<TestResult> ExecuteTestCaseAsync(string code, TestCase testCase)
    {
        try
        {
            // Create a cancellation token with timeout
            using var cts = new CancellationTokenSource(_timeout);
            
            // Execute the code with the test case input
            var result = await Task.Run(() => ExecuteCode(code, testCase.Input), cts.Token);
            
            // Compare actual output with expected output
            var passed = CompareOutputs(result, testCase.ExpectedOutput);
            
            return new TestResult
            {
                TestCaseId = testCase.Id,
                Passed = passed,
                ActualOutput = result,
                ExpectedOutput = testCase.ExpectedOutput,
                ErrorMessage = passed ? null : "Output mismatch"
            };
        }
        catch (OperationCanceledException)
        {
            return new TestResult
            {
                TestCaseId = testCase.Id,
                Passed = false,
                ActualOutput = null,
                ExpectedOutput = testCase.ExpectedOutput,
                ErrorMessage = $"Test case execution exceeded timeout of {_timeout.TotalSeconds} seconds"
            };
        }
        catch (Exception ex)
        {
            return new TestResult
            {
                TestCaseId = testCase.Id,
                Passed = false,
                ActualOutput = null,
                ExpectedOutput = testCase.ExpectedOutput,
                ErrorMessage = $"Execution error: {ex.Message}"
            };
        }
    }

    /// <summary>
    /// Executes all test cases against the provided code.
    /// </summary>
    /// <param name="code">The code to execute</param>
    /// <param name="testCases">The test cases to run</param>
    /// <returns>List of test results</returns>
    public async Task<List<TestResult>> ExecuteAllTestCasesAsync(string code, List<TestCase> testCases)
    {
        var results = new List<TestResult>();
        
        foreach (var testCase in testCases)
        {
            var result = await ExecuteTestCaseAsync(code, testCase);
            results.Add(result);
        }
        
        return results;
    }

    /// <summary>
    /// Executes the code with the given input.
    /// This is a placeholder implementation - in a real system, this would use
    /// the code execution engine (Docker containers, etc.)
    /// </summary>
    private string ExecuteCode(string code, string input)
    {
        // TODO: This is a simplified implementation for testing purposes
        // In the real system, this would:
        // 1. Send the code to the execution engine
        // 2. Pass the input to the code
        // 3. Capture and return the output
        
        // For now, we'll just return a placeholder
        // This will be replaced with actual code execution in Task 9
        throw new NotImplementedException("Code execution will be implemented in the Execution Service (Task 9)");
    }

    /// <summary>
    /// Compares actual output with expected output using deep equality.
    /// </summary>
    private bool CompareOutputs(string actual, string expected)
    {
        if (actual == null && expected == null)
            return true;
        
        if (actual == null || expected == null)
            return false;
        
        // Normalize line endings for comparison
        var normalizedActual = actual.Replace("\r\n", "\n").Replace("\r", "\n").Trim();
        var normalizedExpected = expected.Replace("\r\n", "\n").Replace("\r", "\n").Trim();
        
        return normalizedActual == normalizedExpected;
    }
}

/// <summary>
/// Represents the result of executing a test case.
/// </summary>
public class TestResult
{
    public Guid TestCaseId { get; set; }
    public bool Passed { get; set; }
    public string? ActualOutput { get; set; }
    public string ExpectedOutput { get; set; } = string.Empty;
    public string? ErrorMessage { get; set; }
}
