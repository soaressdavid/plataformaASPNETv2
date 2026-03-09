using Polly;
using Polly.CircuitBreaker;
using ApiGateway.Configuration;

namespace ApiGateway.Tests;

/// <summary>
/// Tests for circuit breaker functionality in API Gateway
/// **Validates: Requirement 11.4**
/// </summary>
public class CircuitBreakerTests
{
    [Fact]
    public void CircuitBreaker_OpensAfter5Failures()
    {
        // Arrange
        var failureCount = 0;
        var circuitBreaker = new ResiliencePipelineBuilder()
            .AddCircuitBreaker(new CircuitBreakerStrategyOptions
            {
                FailureRatio = 1.0,
                MinimumThroughput = 5,
                SamplingDuration = TimeSpan.FromSeconds(10),
                BreakDuration = TimeSpan.FromSeconds(30)
            })
            .Build();

        // Act - Make 5 requests that fail
        for (int i = 0; i < 5; i++)
        {
            try
            {
                circuitBreaker.Execute(() =>
                {
                    failureCount++;
                    throw new Exception("Service failure");
                });
            }
            catch (Exception ex) when (ex is not BrokenCircuitException)
            {
                // Expected failures
            }
        }

        // Assert - Circuit should now be open, next request should throw BrokenCircuitException
        Assert.Equal(5, failureCount);
        
        var exception = Assert.Throws<BrokenCircuitException>(() =>
        {
            circuitBreaker.Execute(() =>
            {
                failureCount++;
            });
        });

        Assert.Equal(5, failureCount); // Should not have incremented
    }

    [Fact]
    public async Task CircuitBreaker_HalfOpenAfterBreakDuration()
    {
        // Arrange
        var circuitBreaker = new ResiliencePipelineBuilder()
            .AddCircuitBreaker(new CircuitBreakerStrategyOptions
            {
                FailureRatio = 1.0,
                MinimumThroughput = 5,
                SamplingDuration = TimeSpan.FromSeconds(10),
                BreakDuration = TimeSpan.FromMilliseconds(500) // Minimum allowed duration
            })
            .Build();

        // Act - Trigger circuit breaker
        for (int i = 0; i < 5; i++)
        {
            try
            {
                await circuitBreaker.ExecuteAsync(async _ =>
                {
                    await Task.CompletedTask;
                    throw new Exception("Service failure");
                });
            }
            catch (Exception ex) when (ex is not BrokenCircuitException)
            {
                // Expected failures
            }
        }

        // Circuit should be open
        Assert.Throws<BrokenCircuitException>(() =>
        {
            circuitBreaker.Execute(() => { });
        });

        // Wait for circuit to potentially go half-open
        await Task.Delay(600);

        // Assert - Circuit should allow a test request (half-open state)
        var successfulRequest = false;
        try
        {
            await circuitBreaker.ExecuteAsync(async _ =>
            {
                successfulRequest = true;
                await Task.CompletedTask;
            });
        }
        catch
        {
            // May still throw if timing is off
        }

        Assert.True(successfulRequest);
    }

    [Fact]
    public async Task CircuitBreaker_ClosesAfterSuccessfulRequest()
    {
        // Arrange
        var requestCount = 0;
        var circuitBreaker = new ResiliencePipelineBuilder()
            .AddCircuitBreaker(new CircuitBreakerStrategyOptions
            {
                FailureRatio = 1.0,
                MinimumThroughput = 5,
                SamplingDuration = TimeSpan.FromSeconds(10),
                BreakDuration = TimeSpan.FromMilliseconds(500) // Minimum allowed duration
            })
            .Build();

        // Act - Trigger circuit breaker
        for (int i = 0; i < 5; i++)
        {
            try
            {
                await circuitBreaker.ExecuteAsync(async _ =>
                {
                    requestCount++;
                    await Task.CompletedTask;
                    throw new Exception("Service failure");
                });
            }
            catch (Exception ex) when (ex is not BrokenCircuitException)
            {
                // Expected failures
            }
        }

        // Wait for circuit to go half-open
        await Task.Delay(600);

        // Make a successful request
        await circuitBreaker.ExecuteAsync(async _ =>
        {
            requestCount++;
            await Task.CompletedTask;
            // Success - no exception
        });

        // Assert - Circuit should now be closed, allowing more requests
        await circuitBreaker.ExecuteAsync(async _ =>
        {
            requestCount++;
            await Task.CompletedTask;
        });

        Assert.Equal(7, requestCount); // 5 failures + 2 successes
    }

    [Fact]
    public void CircuitBreaker_ThrowsBrokenCircuitException_WhenOpen()
    {
        // Arrange
        var circuitBreaker = new ResiliencePipelineBuilder()
            .AddCircuitBreaker(new CircuitBreakerStrategyOptions
            {
                FailureRatio = 1.0,
                MinimumThroughput = 5,
                SamplingDuration = TimeSpan.FromSeconds(10),
                BreakDuration = TimeSpan.FromSeconds(30)
            })
            .Build();

        // Act - Trigger circuit breaker
        for (int i = 0; i < 5; i++)
        {
            try
            {
                circuitBreaker.Execute(() =>
                {
                    throw new Exception("Service failure");
                });
            }
            catch (Exception ex) when (ex is not BrokenCircuitException)
            {
                // Expected failures
            }
        }

        // Assert - Subsequent requests should throw BrokenCircuitException
        Assert.Throws<BrokenCircuitException>(() =>
        {
            circuitBreaker.Execute(() => { });
        });
    }

    [Fact]
    public async Task CircuitBreaker_DoesNotOpen_WhenFailuresAreBelowThreshold()
    {
        // Arrange
        var circuitBreaker = new ResiliencePipelineBuilder()
            .AddCircuitBreaker(new CircuitBreakerStrategyOptions
            {
                FailureRatio = 1.0,
                MinimumThroughput = 5,
                SamplingDuration = TimeSpan.FromSeconds(10),
                BreakDuration = TimeSpan.FromSeconds(30)
            })
            .Build();

        var requestCount = 0;

        // Act - Make 4 failing requests (below threshold of 5)
        for (int i = 0; i < 4; i++)
        {
            try
            {
                await circuitBreaker.ExecuteAsync(async _ =>
                {
                    requestCount++;
                    await Task.CompletedTask;
                    throw new Exception("Service failure");
                });
            }
            catch (Exception ex) when (ex is not BrokenCircuitException)
            {
                // Expected failures
            }
        }

        // Assert - Circuit should still be closed
        Assert.Equal(4, requestCount);

        // Make a successful request - should not throw BrokenCircuitException
        await circuitBreaker.ExecuteAsync(async _ =>
        {
            requestCount++;
            await Task.CompletedTask;
            // Success
        });

        Assert.Equal(5, requestCount);
    }

    [Fact]
    public void CircuitBreakerConfiguration_HasCorrectDefaults()
    {
        // Arrange & Act
        var config = new CircuitBreakerConfiguration();

        // Assert
        Assert.Equal(5, config.FailureThreshold);
        Assert.Equal(30, config.DurationOfBreakInSeconds);
    }
}
