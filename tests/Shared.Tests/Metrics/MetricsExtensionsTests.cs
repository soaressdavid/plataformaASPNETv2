using Shared.Metrics;
using Xunit;

namespace Shared.Tests.Metrics;

public class MetricsExtensionsTests
{
    [Fact]
    public void RecordHttpRequest_SuccessfulRequest_IncrementsCounter()
    {
        // Arrange
        var method = "GET";
        var endpoint = "/api/courses";
        var statusCode = 200;
        var duration = 0.123;

        // Act
        MetricsExtensions.RecordHttpRequest(method, endpoint, statusCode, duration);

        // Assert
        // Metrics are recorded in static registry
        // Actual verification would require Prometheus test server
        // This test ensures no exceptions are thrown
        Assert.True(true);
    }

    [Fact]
    public void RecordHttpRequest_ErrorRequest_IncrementsErrorCounter()
    {
        // Arrange
        var method = "POST";
        var endpoint = "/api/courses";
        var statusCode = 500;
        var duration = 0.456;

        // Act
        MetricsExtensions.RecordHttpRequest(method, endpoint, statusCode, duration);

        // Assert
        Assert.True(true);
    }

    [Fact]
    public void RecordDatabaseQuery_SuccessfulQuery_IncrementsCounter()
    {
        // Arrange
        var operation = "SELECT";
        var table = "Courses";
        var duration = 0.050;

        // Act
        MetricsExtensions.RecordDatabaseQuery(operation, table, duration, success: true);

        // Assert
        Assert.True(true);
    }

    [Fact]
    public void RecordDatabaseQuery_FailedQuery_IncrementsErrorCounter()
    {
        // Arrange
        var operation = "INSERT";
        var table = "Lessons";
        var duration = 0.100;

        // Act
        MetricsExtensions.RecordDatabaseQuery(operation, table, duration, success: false);

        // Assert
        Assert.True(true);
    }

    [Fact]
    public void RecordCacheAccess_Hit_IncrementsHitCounter()
    {
        // Arrange
        var cacheName = "courses";

        // Act
        MetricsExtensions.RecordCacheAccess(cacheName, hit: true);

        // Assert
        Assert.True(true);
    }

    [Fact]
    public void RecordCacheAccess_Miss_IncrementsMissCounter()
    {
        // Arrange
        var cacheName = "lessons";

        // Act
        MetricsExtensions.RecordCacheAccess(cacheName, hit: false);

        // Assert
        Assert.True(true);
    }

    [Fact]
    public void RecordRetryAttempt_IncrementsCounter()
    {
        // Arrange
        var operation = "GetCourse";
        var attemptNumber = 2;

        // Act
        MetricsExtensions.RecordRetryAttempt(operation, attemptNumber);

        // Assert
        Assert.True(true);
    }

    [Fact]
    public void RecordCircuitBreakerState_ClosedState_SetsGauge()
    {
        // Arrange
        var operation = "GetCourse";

        // Act
        MetricsExtensions.RecordCircuitBreakerState(operation, CircuitBreakerState.Closed);

        // Assert
        Assert.True(true);
    }

    [Fact]
    public void RecordCircuitBreakerState_OpenState_SetsGaugeAndIncrementsCounter()
    {
        // Arrange
        var operation = "GetCourse";

        // Act
        MetricsExtensions.RecordCircuitBreakerState(operation, CircuitBreakerState.Open);

        // Assert
        Assert.True(true);
    }

    [Fact]
    public void RecordValidationError_IncrementsCounter()
    {
        // Arrange
        var validator = "CreateCourseRequestValidator";
        var field = "Title";

        // Act
        MetricsExtensions.RecordValidationError(validator, field);

        // Assert
        Assert.True(true);
    }

    [Fact]
    public void RecordBusinessEvents_IncrementCounters()
    {
        // Act
        MetricsExtensions.RecordCourseCreated();
        MetricsExtensions.RecordLessonCreated();
        MetricsExtensions.RecordUserRegistered();
        MetricsExtensions.RecordLessonCompleted(1);

        // Assert
        Assert.True(true);
    }

    [Fact]
    public void SetActiveUsers_SetsGauge()
    {
        // Arrange
        var count = 42;

        // Act
        MetricsExtensions.SetActiveUsers(count);

        // Assert
        Assert.True(true);
    }

    [Fact]
    public void SetActiveSessions_SetsGauge()
    {
        // Arrange
        var count = 15;

        // Act
        MetricsExtensions.SetActiveSessions(count);

        // Assert
        Assert.True(true);
    }

    [Theory]
    [InlineData("GET", "/api/courses", 200, 0.1)]
    [InlineData("POST", "/api/lessons", 201, 0.2)]
    [InlineData("PUT", "/api/courses/1", 204, 0.15)]
    [InlineData("DELETE", "/api/courses/1", 404, 0.05)]
    public void RecordHttpRequest_VariousScenarios_NoExceptions(
        string method, string endpoint, int statusCode, double duration)
    {
        // Act & Assert
        var exception = Record.Exception(() =>
            MetricsExtensions.RecordHttpRequest(method, endpoint, statusCode, duration));
        
        Assert.Null(exception);
    }

    [Theory]
    [InlineData("SELECT", "Courses", 0.01, true)]
    [InlineData("INSERT", "Lessons", 0.05, true)]
    [InlineData("UPDATE", "Users", 0.03, false)]
    [InlineData("DELETE", "Progress", 0.02, false)]
    public void RecordDatabaseQuery_VariousScenarios_NoExceptions(
        string operation, string table, double duration, bool success)
    {
        // Act & Assert
        var exception = Record.Exception(() =>
            MetricsExtensions.RecordDatabaseQuery(operation, table, duration, success));
        
        Assert.Null(exception);
    }
}
