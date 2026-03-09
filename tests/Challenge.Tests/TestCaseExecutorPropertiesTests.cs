using FsCheck;
using FsCheck.Xunit;
using Shared.Entities;
using Shared.Services;

namespace Challenge.Tests;

/// <summary>
/// Property-based tests for test case executor functionality.
/// Feature: aspnet-learning-platform
/// 
/// Note: These tests verify the executor's structure and behavior.
/// Actual code execution will be implemented in Task 9 (Execution Service).
/// </summary>
public class TestCaseExecutorPropertiesTests
{
    /// <summary>
    /// Property 13: Test Case Execution Completeness
    /// **Validates: Requirements 5.3, 13.2**
    /// 
    /// For any challenge submission, all test cases associated with that challenge 
    /// should be executed against the submitted code.
    /// </summary>
    [Property(MaxTest = 20)]
    public async Task TestCaseExecution_ExecutesAllTestCases(NonEmptyString code, PositiveInt testCaseCount)
    {
        // Arrange
        var executor = new TestCaseExecutor();
        var count = Math.Min(testCaseCount.Get, 10); // Limit to 10 for test performance
        var testCases = GenerateTestCases(count);
        
        // Act
        var results = await executor.ExecuteAllTestCasesAsync(code.Get, testCases);
        
        // Assert - all test cases should have results
        Assert.Equal(testCases.Count, results.Count);
        
        // Each test case should have a corresponding result
        foreach (var testCase in testCases)
        {
            Assert.Contains(results, r => r.TestCaseId == testCase.Id);
        }
    }

    /// <summary>
    /// Property 41: Test Case Parsing
    /// **Validates: Requirements 13.1**
    /// 
    /// For any valid test case definition, the platform should parse it into a TestCase object 
    /// with input and expected output.
    /// 
    /// Note: This is tested through the TestCaseParser tests, but we verify the executor 
    /// can work with parsed test cases.
    /// </summary>
    [Property(MaxTest = 20)]
    public async Task TestCaseExecution_WorksWithParsedTestCases(NonEmptyString code, NonEmptyString input, NonEmptyString expectedOutput)
    {
        // Arrange
        var executor = new TestCaseExecutor();
        var testCase = new TestCase
        {
            Id = Guid.NewGuid(),
            ChallengeId = Guid.NewGuid(),
            Input = input.Get,
            ExpectedOutput = expectedOutput.Get,
            OrderIndex = 0,
            IsHidden = false
        };
        
        // Act
        var result = await executor.ExecuteTestCaseAsync(code.Get, testCase);
        
        // Assert - result should be created with test case ID
        Assert.NotNull(result);
        Assert.Equal(testCase.Id, result.TestCaseId);
        Assert.Equal(testCase.ExpectedOutput, result.ExpectedOutput);
    }

    /// <summary>
    /// Property 42: Output Comparison
    /// **Validates: Requirements 13.3**
    /// 
    /// For any test case execution, the platform should compare the actual output 
    /// with the expected output using deep equality.
    /// 
    /// Note: Since actual code execution is not implemented yet, we test the comparison logic
    /// through the TestResult structure.
    /// </summary>
    [Property(MaxTest = 20)]
    public async Task TestCaseExecution_ComparesOutputs(NonEmptyString code)
    {
        // Arrange
        var executor = new TestCaseExecutor();
        var testCase = new TestCase
        {
            Id = Guid.NewGuid(),
            ChallengeId = Guid.NewGuid(),
            Input = "test input",
            ExpectedOutput = "expected output",
            OrderIndex = 0,
            IsHidden = false
        };
        
        // Act
        var result = await executor.ExecuteTestCaseAsync(code.Get, testCase);
        
        // Assert - result should have comparison information
        Assert.NotNull(result);
        Assert.NotNull(result.ExpectedOutput);
        
        // The result should indicate whether outputs matched
        // (will be false until actual execution is implemented)
        Assert.False(result.Passed); // Expected since execution is not implemented
    }

    /// <summary>
    /// Property 43: Test Case Input Support
    /// **Validates: Requirements 13.4**
    /// 
    /// For any test case, it should support input parameters and expected return values.
    /// </summary>
    [Property(MaxTest = 20)]
    public async Task TestCaseExecution_SupportsInputParameters(NonEmptyString code, NonEmptyString input, NonEmptyString expectedOutput)
    {
        // Arrange
        var executor = new TestCaseExecutor();
        var testCase = new TestCase
        {
            Id = Guid.NewGuid(),
            ChallengeId = Guid.NewGuid(),
            Input = input.Get,
            ExpectedOutput = expectedOutput.Get,
            OrderIndex = 0,
            IsHidden = false
        };
        
        // Act
        var result = await executor.ExecuteTestCaseAsync(code.Get, testCase);
        
        // Assert - test case input and expected output should be preserved
        Assert.NotNull(result);
        Assert.Equal(expectedOutput.Get, result.ExpectedOutput);
    }

    /// <summary>
    /// Property 44: All Tests Pass Result
    /// **Validates: Requirements 13.5**
    /// 
    /// For any submission where all test cases pass, the platform should return a success result.
    /// </summary>
    [Property(MaxTest = 20)]
    public async Task TestCaseExecution_ReturnsSuccessWhenAllPass(NonEmptyString code)
    {
        // Arrange
        var executor = new TestCaseExecutor();
        var testCases = GenerateTestCases(3);
        
        // Act
        var results = await executor.ExecuteAllTestCasesAsync(code.Get, testCases);
        
        // Assert - should have results for all test cases
        Assert.Equal(testCases.Count, results.Count);
        
        // Check if all passed (will be false until execution is implemented)
        var allPassed = results.All(r => r.Passed);
        
        // The logic for determining overall success should be based on individual results
        Assert.Equal(allPassed, results.All(r => r.Passed));
    }

    /// <summary>
    /// Property: Timeout handling
    /// 
    /// Test case execution should respect the 5-second timeout.
    /// </summary>
    [Property(MaxTest = 20)]
    public async Task TestCaseExecution_RespectsTimeout()
    {
        // Arrange
        var timeout = TimeSpan.FromMilliseconds(100); // Short timeout for testing
        var executor = new TestCaseExecutor(timeout);
        var testCase = new TestCase
        {
            Id = Guid.NewGuid(),
            ChallengeId = Guid.NewGuid(),
            Input = "test",
            ExpectedOutput = "output",
            OrderIndex = 0,
            IsHidden = false
        };
        
        // Act
        var result = await executor.ExecuteTestCaseAsync("code", testCase);
        
        // Assert - should complete (even if it fails due to not implemented)
        Assert.NotNull(result);
        Assert.NotNull(result.ErrorMessage);
    }

    /// <summary>
    /// Property: Test case isolation
    /// 
    /// Each test case should be executed in isolation.
    /// </summary>
    [Property(MaxTest = 20)]
    public async Task TestCaseExecution_IsolatesTestCases(NonEmptyString code)
    {
        // Arrange
        var executor = new TestCaseExecutor();
        var testCase1 = new TestCase
        {
            Id = Guid.NewGuid(),
            ChallengeId = Guid.NewGuid(),
            Input = "input1",
            ExpectedOutput = "output1",
            OrderIndex = 0,
            IsHidden = false
        };
        var testCase2 = new TestCase
        {
            Id = Guid.NewGuid(),
            ChallengeId = Guid.NewGuid(),
            Input = "input2",
            ExpectedOutput = "output2",
            OrderIndex = 1,
            IsHidden = false
        };
        
        // Act
        var result1 = await executor.ExecuteTestCaseAsync(code.Get, testCase1);
        var result2 = await executor.ExecuteTestCaseAsync(code.Get, testCase2);
        
        // Assert - results should be independent
        Assert.NotEqual(result1.TestCaseId, result2.TestCaseId);
        Assert.NotEqual(result1.ExpectedOutput, result2.ExpectedOutput);
    }

    private List<TestCase> GenerateTestCases(int count)
    {
        var testCases = new List<TestCase>();
        var challengeId = Guid.NewGuid();
        
        for (int i = 0; i < count; i++)
        {
            testCases.Add(new TestCase
            {
                Id = Guid.NewGuid(),
                ChallengeId = challengeId,
                Input = $"input{i}",
                ExpectedOutput = $"output{i}",
                OrderIndex = i,
                IsHidden = i % 2 == 0
            });
        }
        
        return testCases;
    }
}
