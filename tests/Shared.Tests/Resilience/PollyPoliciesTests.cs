using Microsoft.Extensions.Logging;
using Moq;
using Polly.CircuitBreaker;
using Shared.Resilience;
using Xunit;

namespace Shared.Tests.Resilience;

public class PollyPoliciesTests
{
    private readonly Mock<ILogger> _loggerMock;

    public PollyPoliciesTests()
    {
        _loggerMock = new Mock<ILogger>();
    }

    [Fact]
    public async Task GetHttpRetryPolicy_SuccessfulRequest_NoRetry()
    {
        // Arrange
        var policy = PollyPolicies.GetHttpRetryPolicy(_loggerMock.Object);
        var executionCount = 0;

        // Act
        var result = await policy.ExecuteAsync(async () =>
        {
            executionCount++;
            await Task.CompletedTask;
            return new HttpResponseMessage(System.Net.HttpStatusCode.OK);
        });

        // Assert
        Assert.Equal(1, executionCount);
        Assert.True(result.IsSuccessStatusCode);
    }

    [Fact]
    public async Task GetHttpRetryPolicy_TransientFailure_RetriesThreeTimes()
    {
        // Arrange
        var policy = PollyPolicies.GetHttpRetryPolicy(_loggerMock.Object);
        var executionCount = 0;

        // Act & Assert
        await Assert.ThrowsAsync<HttpRequestException>(async () =>
        {
            await policy.ExecuteAsync(async () =>
            {
                executionCount++;
                await Task.CompletedTask;
                throw new HttpRequestException("Network error");
            });
        });

        Assert.Equal(4, executionCount); // 1 initial + 3 retries
    }

    [Fact]
    public async Task GetHttpRetryPolicy_FailsThenSucceeds_StopsRetrying()
    {
        // Arrange
        var policy = PollyPolicies.GetHttpRetryPolicy(_loggerMock.Object);
        var executionCount = 0;

        // Act
        var result = await policy.ExecuteAsync(async () =>
        {
            executionCount++;
            await Task.CompletedTask;
            
            if (executionCount < 3)
            {
                throw new HttpRequestException("Temporary error");
            }
            
            return new HttpResponseMessage(System.Net.HttpStatusCode.OK);
        });

        // Assert
        Assert.Equal(3, executionCount);
        Assert.True(result.IsSuccessStatusCode);
    }

    [Fact]
    public async Task GetDatabaseRetryPolicy_SuccessfulQuery_NoRetry()
    {
        // Arrange
        var policy = PollyPolicies.GetDatabaseRetryPolicy(_loggerMock.Object);
        var executionCount = 0;

        // Act
        var result = await policy.ExecuteAsync(async () =>
        {
            executionCount++;
            await Task.CompletedTask;
            return "Success";
        });

        // Assert
        Assert.Equal(1, executionCount);
        Assert.Equal("Success", result);
    }

    [Fact]
    public async Task GetDatabaseRetryPolicy_TransientFailure_RetriesThreeTimes()
    {
        // Arrange
        var policy = PollyPolicies.GetDatabaseRetryPolicy(_loggerMock.Object);
        var executionCount = 0;

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(async () =>
        {
            await policy.ExecuteAsync(async () =>
            {
                executionCount++;
                await Task.CompletedTask;
                throw new InvalidOperationException("Database timeout");
            });
        });

        Assert.Equal(4, executionCount); // 1 initial + 3 retries
    }

    [Fact]
    public async Task GetHttpCircuitBreakerPolicy_ConsecutiveFailures_OpensCircuit()
    {
        // Arrange
        var policy = PollyPolicies.GetHttpCircuitBreakerPolicy(_loggerMock.Object);
        var executionCount = 0;

        // Act - Cause 5 consecutive failures to open circuit
        for (int i = 0; i < 5; i++)
        {
            try
            {
                await policy.ExecuteAsync(async () =>
                {
                    executionCount++;
                    await Task.CompletedTask;
                    throw new HttpRequestException("Service unavailable");
                });
            }
            catch (HttpRequestException)
            {
                // Expected
            }
        }

        // Assert - Circuit should be open, next call should throw BrokenCircuitException
        await Assert.ThrowsAsync<BrokenCircuitException>(async () =>
        {
            await policy.ExecuteAsync(async () =>
            {
                await Task.CompletedTask;
                return new HttpResponseMessage(System.Net.HttpStatusCode.OK);
            });
        });

        Assert.Equal(5, executionCount); // Circuit opened after 5 failures
    }

    [Fact]
    public async Task GetCombinedHttpPolicy_SuccessfulRequest_NoRetryOrTimeout()
    {
        // Arrange
        var policy = PollyPolicies.GetCombinedHttpPolicy(_loggerMock.Object, 30);
        var executionCount = 0;

        // Act
        var result = await policy.ExecuteAsync(async () =>
        {
            executionCount++;
            await Task.Delay(100);
            return new HttpResponseMessage(System.Net.HttpStatusCode.OK);
        });

        // Assert
        Assert.Equal(1, executionCount);
        Assert.True(result.IsSuccessStatusCode);
    }

    [Fact]
    public async Task GetCombinedDatabasePolicy_SuccessfulQuery_NoRetryOrTimeout()
    {
        // Arrange
        var policy = PollyPolicies.GetCombinedDatabasePolicy(_loggerMock.Object, 30);
        var executionCount = 0;

        // Act
        var result = await policy.ExecuteAsync(async () =>
        {
            executionCount++;
            await Task.Delay(100);
            return "Success";
        });

        // Assert
        Assert.Equal(1, executionCount);
        Assert.Equal("Success", result);
    }

    [Fact]
    public async Task GetHttpRetryPolicy_LogsRetryAttempts()
    {
        // Arrange
        var policy = PollyPolicies.GetHttpRetryPolicy(_loggerMock.Object);
        var executionCount = 0;

        // Act
        try
        {
            await policy.ExecuteAsync(async () =>
            {
                executionCount++;
                await Task.CompletedTask;
                throw new HttpRequestException("Test error");
            });
        }
        catch
        {
            // Expected
        }

        // Assert - Verify logging occurred (at least once for retry)
        _loggerMock.Verify(
            x => x.Log(
                It.IsAny<LogLevel>(),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => true),
                It.IsAny<Exception>(),
                It.Is<Func<It.IsAnyType, Exception?, string>>((v, t) => true)),
            Times.AtLeastOnce);
    }
}
