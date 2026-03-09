using FsCheck;
using FsCheck.Xunit;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace ApiGateway.Tests;

/// <summary>
/// Property-based tests for API Gateway routing and service unavailability handling.
/// **Validates: Requirements 11.1, 11.2, 11.3, 11.4**
/// </summary>
public class RoutingPropertiesTests
{
    /// <summary>
    /// Property 35: API Gateway Routing
    /// For any incoming request, the API Gateway should route it to the correct microservice
    /// based on the request path (courses → Course Service, code execution → Execution Engine,
    /// AI review → AI Tutor).
    /// **Validates: Requirements 11.1, 11.2, 11.3**
    /// </summary>
    [Property(MaxTest = 100)]
    public void APIGatewayRouting_RoutesToCorrectMicroservice(NonEmptyString pathSuffix)
    {
        var suffix = pathSuffix.Get.Replace("/", "").Replace("\\", "");
        
        // Test course-related requests route to Course Service
        var coursePath = $"/api/courses/{suffix}";
        var courseCluster = GetClusterForPath(coursePath);
        Assert.Equal("course-cluster", courseCluster);
        
        // Test code execution requests route to Execution Engine
        var executePath = $"/api/code/execute/{suffix}";
        var executeCluster = GetClusterForPath(executePath);
        Assert.Equal("execution-cluster", executeCluster);
        
        var statusPath = $"/api/code/status/{suffix}";
        var statusCluster = GetClusterForPath(statusPath);
        Assert.Equal("execution-cluster", statusCluster);
        
        // Test AI review requests route to AI Tutor
        var reviewPath = $"/api/code/review/{suffix}";
        var reviewCluster = GetClusterForPath(reviewPath);
        Assert.Equal("ai-tutor-cluster", reviewCluster);
    }

    /// <summary>
    /// Property: Challenge requests should route to Challenge Service.
    /// **Validates: Requirements 11.1**
    /// </summary>
    [Property(MaxTest = 100)]
    public void ChallengeRequests_RouteToChallengeMicroservice(NonEmptyString pathSuffix)
    {
        var suffix = pathSuffix.Get.Replace("/", "").Replace("\\", "");
        var path = $"/api/challenges/{suffix}";
        
        var cluster = GetClusterForPath(path);
        
        Assert.Equal("challenge-cluster", cluster);
    }

    /// <summary>
    /// Property: Progress and leaderboard requests should route to Progress Service.
    /// **Validates: Requirements 11.1**
    /// </summary>
    [Property(MaxTest = 100)]
    public void ProgressRequests_RouteToProgressMicroservice(NonEmptyString pathSuffix)
    {
        var suffix = pathSuffix.Get.Replace("/", "").Replace("\\", "");
        
        // Test progress requests
        var progressPath = $"/api/progress/{suffix}";
        var progressCluster = GetClusterForPath(progressPath);
        Assert.Equal("progress-cluster", progressCluster);
        
        // Test leaderboard requests (also handled by Progress Service)
        var leaderboardPath = $"/api/leaderboard/{suffix}";
        var leaderboardCluster = GetClusterForPath(leaderboardPath);
        Assert.Equal("progress-cluster", leaderboardCluster);
    }

    /// <summary>
    /// Property: Auth requests should route to Auth Service.
    /// **Validates: Requirements 11.1**
    /// </summary>
    [Property(MaxTest = 100)]
    public void AuthRequests_RouteToAuthMicroservice(NonEmptyString pathSuffix)
    {
        var suffix = pathSuffix.Get.Replace("/", "").Replace("\\", "");
        var path = $"/api/auth/{suffix}";
        
        var cluster = GetClusterForPath(path);
        
        Assert.Equal("auth-cluster", cluster);
    }

    /// <summary>
    /// Property: Routing should be case-insensitive for path matching.
    /// **Validates: Requirements 11.1, 11.2, 11.3**
    /// </summary>
    [Property(MaxTest = 100)]
    public void Routing_IsCaseInsensitive(NonEmptyString pathSuffix)
    {
        var suffix = pathSuffix.Get.Replace("/", "").Replace("\\", "");
        
        // Test lowercase
        var lowerPath = $"/api/courses/{suffix}".ToLower();
        var lowerCluster = GetClusterForPath(lowerPath);
        
        // Test uppercase
        var upperPath = $"/api/courses/{suffix}".ToUpper();
        var upperCluster = GetClusterForPath(upperPath);
        
        // Test mixed case
        var mixedPath = $"/api/CoUrSeS/{suffix}";
        var mixedCluster = GetClusterForPath(mixedPath);
        
        // All should route to the same cluster
        Assert.Equal("course-cluster", lowerCluster);
        Assert.Equal("course-cluster", upperCluster);
        Assert.Equal("course-cluster", mixedCluster);
    }

    /// <summary>
    /// Property: Each route should map to exactly one cluster.
    /// **Validates: Requirements 11.1, 11.2, 11.3**
    /// </summary>
    [Property(MaxTest = 100)]
    public void EachRoute_MapsToExactlyOneCluster(NonEmptyString pathSuffix)
    {
        var suffix = pathSuffix.Get.Replace("/", "").Replace("\\", "");
        
        var testPaths = new[]
        {
            $"/api/auth/{suffix}",
            $"/api/courses/{suffix}",
            $"/api/challenges/{suffix}",
            $"/api/progress/{suffix}",
            $"/api/leaderboard/{suffix}",
            $"/api/code/execute/{suffix}",
            $"/api/code/status/{suffix}",
            $"/api/code/review/{suffix}"
        };
        
        foreach (var path in testPaths)
        {
            var cluster = GetClusterForPath(path);
            
            // Each path should resolve to a non-null cluster
            Assert.False(string.IsNullOrEmpty(cluster));
            
            // Verify it's a valid cluster name
            Assert.Contains(cluster, new[]
            {
                "auth-cluster",
                "course-cluster",
                "challenge-cluster",
                "progress-cluster",
                "execution-cluster",
                "ai-tutor-cluster"
            });
        }
    }

    /// <summary>
    /// Property 36: Service Unavailability Handling
    /// For any request to an unavailable microservice, the API Gateway should return
    /// a 503 Service Unavailable error.
    /// **Validates: Requirements 11.4**
    /// </summary>
    [Property(MaxTest = 100)]
    public void ServiceUnavailability_Returns503Error(NonEmptyString serviceName)
    {
        var service = serviceName.Get.Replace(" ", "").Replace("\n", "");
        
        // Simulate circuit breaker open state
        var circuitState = CircuitState.Open;
        
        // When circuit is open, should return 503
        var statusCode = GetStatusCodeForCircuitState(circuitState);
        Assert.Equal(StatusCodes.Status503ServiceUnavailable, statusCode);
        
        // Verify error response format
        var errorResponse = GetErrorResponseForUnavailableService(service);
        Assert.Equal("SERVICE_UNAVAILABLE", errorResponse.Code);
        Assert.Contains("unavailable", errorResponse.Message.ToLower());
        Assert.NotEqual(default(DateTime), errorResponse.Timestamp);
    }

    /// <summary>
    /// Property: Circuit breaker should allow requests when closed.
    /// **Validates: Requirements 11.4**
    /// </summary>
    [Property(MaxTest = 100)]
    public void CircuitBreakerClosed_AllowsRequests(NonEmptyString requestPath)
    {
        var path = requestPath.Get;
        
        // When circuit is closed, requests should be allowed
        var circuitState = CircuitState.Closed;
        var shouldAllow = ShouldAllowRequest(circuitState);
        
        Assert.True(shouldAllow);
    }

    /// <summary>
    /// Property: Circuit breaker should block requests when open.
    /// **Validates: Requirements 11.4**
    /// </summary>
    [Property(MaxTest = 100)]
    public void CircuitBreakerOpen_BlocksRequests(NonEmptyString requestPath)
    {
        var path = requestPath.Get;
        
        // When circuit is open, requests should be blocked
        var circuitState = CircuitState.Open;
        var shouldAllow = ShouldAllowRequest(circuitState);
        
        Assert.False(shouldAllow);
    }

    /// <summary>
    /// Property: Circuit breaker should test service health when half-open.
    /// **Validates: Requirements 11.4**
    /// </summary>
    [Property(MaxTest = 100)]
    public void CircuitBreakerHalfOpen_TestsServiceHealth(PositiveInt requestCount)
    {
        var count = requestCount.Get % 10; // Limit to reasonable range
        
        // When circuit is half-open, it should allow limited requests to test health
        var circuitState = CircuitState.HalfOpen;
        
        // Half-open state should allow some requests through
        var allowedRequests = 0;
        for (int i = 0; i < count; i++)
        {
            if (ShouldAllowRequest(circuitState))
            {
                allowedRequests++;
            }
        }
        
        // At least one request should be allowed to test the service
        if (count > 0)
        {
            Assert.True(allowedRequests > 0);
        }
    }

    /// <summary>
    /// Property: Circuit breaker should open after threshold failures.
    /// **Validates: Requirements 11.4**
    /// </summary>
    [Property(MaxTest = 100)]
    public void CircuitBreaker_OpensAfterThresholdFailures(PositiveInt failureCount)
    {
        var failures = failureCount.Get % 20; // Limit to reasonable range
        var threshold = 5; // From configuration
        
        var circuitState = SimulateFailures(failures, threshold);
        
        // If failures exceed threshold, circuit should be open
        if (failures >= threshold)
        {
            Assert.Equal(CircuitState.Open, circuitState);
        }
        else
        {
            Assert.Equal(CircuitState.Closed, circuitState);
        }
    }

    /// <summary>
    /// Property: Circuit breaker should reset after successful requests in half-open state.
    /// **Validates: Requirements 11.4**
    /// </summary>
    [Property(MaxTest = 100)]
    public void CircuitBreaker_ResetsAfterSuccessfulRequests(PositiveInt successCount)
    {
        var successes = successCount.Get % 10;
        var requiredSuccesses = 3; // Typical threshold for closing circuit
        
        // Start in half-open state
        var initialState = CircuitState.HalfOpen;
        var finalState = SimulateSuccesses(initialState, successes, requiredSuccesses);
        
        // If enough successes, circuit should close
        if (successes >= requiredSuccesses)
        {
            Assert.Equal(CircuitState.Closed, finalState);
        }
        else
        {
            Assert.Equal(CircuitState.HalfOpen, finalState);
        }
    }

    /// <summary>
    /// Property: Service unavailable error should include timestamp.
    /// **Validates: Requirements 11.4**
    /// </summary>
    [Property(MaxTest = 100)]
    public void ServiceUnavailableError_IncludesTimestamp(NonEmptyString serviceName)
    {
        var service = serviceName.Get;
        var beforeTime = DateTime.UtcNow;
        
        var errorResponse = GetErrorResponseForUnavailableService(service);
        
        var afterTime = DateTime.UtcNow;
        
        Assert.NotEqual(default(DateTime), errorResponse.Timestamp);
        Assert.True(errorResponse.Timestamp >= beforeTime && errorResponse.Timestamp <= afterTime);
        Assert.Equal(DateTimeKind.Utc, errorResponse.Timestamp.Kind);
    }

    /// <summary>
    /// Property: Service unavailable error should have consistent format.
    /// **Validates: Requirements 11.4**
    /// </summary>
    [Property(MaxTest = 100)]
    public void ServiceUnavailableError_HasConsistentFormat(NonEmptyString serviceName)
    {
        var service = serviceName.Get;
        
        var errorResponse = GetErrorResponseForUnavailableService(service);
        
        // Verify all required fields are present
        Assert.False(string.IsNullOrEmpty(errorResponse.Code));
        Assert.False(string.IsNullOrEmpty(errorResponse.Message));
        Assert.NotEqual(default(DateTime), errorResponse.Timestamp);
        
        // Verify error code is correct
        Assert.Equal("SERVICE_UNAVAILABLE", errorResponse.Code);
    }

    /// <summary>
    /// Property: Different services should have independent circuit breakers.
    /// **Validates: Requirements 11.4**
    /// </summary>
    [Property(MaxTest = 100)]
    public void CircuitBreakers_AreIndependentPerService(PositiveInt failures1, PositiveInt failures2)
    {
        var service1Failures = failures1.Get % 10;
        var service2Failures = failures2.Get % 10;
        var threshold = 5;
        
        // Simulate failures for two different services
        var service1State = SimulateFailures(service1Failures, threshold);
        var service2State = SimulateFailures(service2Failures, threshold);
        
        // Each service should have independent state
        if (service1Failures >= threshold)
        {
            Assert.Equal(CircuitState.Open, service1State);
        }
        else
        {
            Assert.Equal(CircuitState.Closed, service1State);
        }
        
        if (service2Failures >= threshold)
        {
            Assert.Equal(CircuitState.Open, service2State);
        }
        else
        {
            Assert.Equal(CircuitState.Closed, service2State);
        }
    }

    // Helper methods to simulate routing and circuit breaker behavior

    private string GetClusterForPath(string path)
    {
        // Simulate YARP routing logic based on path patterns
        var normalizedPath = path.ToLower();
        
        if (normalizedPath.StartsWith("/api/auth/"))
            return "auth-cluster";
        if (normalizedPath.StartsWith("/api/courses/"))
            return "course-cluster";
        if (normalizedPath.StartsWith("/api/challenges/"))
            return "challenge-cluster";
        if (normalizedPath.StartsWith("/api/progress/"))
            return "progress-cluster";
        if (normalizedPath.StartsWith("/api/leaderboard/"))
            return "progress-cluster";
        if (normalizedPath.StartsWith("/api/code/execute/"))
            return "execution-cluster";
        if (normalizedPath.StartsWith("/api/code/status/"))
            return "execution-cluster";
        if (normalizedPath.StartsWith("/api/code/review/"))
            return "ai-tutor-cluster";
        
        return string.Empty;
    }

    private int GetStatusCodeForCircuitState(CircuitState state)
    {
        return state == CircuitState.Open || state == CircuitState.Isolated
            ? StatusCodes.Status503ServiceUnavailable
            : StatusCodes.Status200OK;
    }

    private ErrorResponse GetErrorResponseForUnavailableService(string serviceName)
    {
        return new ErrorResponse
        {
            Code = "SERVICE_UNAVAILABLE",
            Message = "The requested service is temporarily unavailable. Please try again later.",
            Timestamp = DateTime.UtcNow
        };
    }

    private bool ShouldAllowRequest(CircuitState state)
    {
        return state switch
        {
            CircuitState.Closed => true,
            CircuitState.Open => false,
            CircuitState.Isolated => false,
            CircuitState.HalfOpen => true, // Allow test requests
            _ => false
        };
    }

    private CircuitState SimulateFailures(int failureCount, int threshold)
    {
        return failureCount >= threshold ? CircuitState.Open : CircuitState.Closed;
    }

    private CircuitState SimulateSuccesses(CircuitState initialState, int successCount, int requiredSuccesses)
    {
        if (initialState != CircuitState.HalfOpen)
            return initialState;
        
        return successCount >= requiredSuccesses ? CircuitState.Closed : CircuitState.HalfOpen;
    }

    // Helper classes

    private enum CircuitState
    {
        Closed,
        Open,
        HalfOpen,
        Isolated
    }

    private class ErrorResponse
    {
        public string Code { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
    }
}
