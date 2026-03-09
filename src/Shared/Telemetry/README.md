# Application Insights Telemetry

This directory contains the Application Insights telemetry configuration and custom tracking implementations for the platform.

## Overview

Application Insights provides comprehensive monitoring and telemetry for all microservices in the platform, enabling:

- **Distributed Tracing**: Track requests across multiple services with correlation IDs
- **Performance Monitoring**: Monitor API response times, database queries, and container operations
- **Custom Events**: Track business events like code executions, user activities, and compilation errors
- **Automatic Instrumentation**: Automatic tracking of HTTP requests, dependencies, and exceptions
- **Live Metrics**: Real-time monitoring of application health and performance
- **Application Map**: Visualize service dependencies and communication patterns

## Requirements

This implementation satisfies the following requirements:

- **Requirement 23.1**: Record code execution time for every submission
- **Requirement 23.2**: Record compilation errors and their frequency
- **Requirement 23.3**: Record user activity patterns including login times and session duration
- **Requirement 23.4**: Record API response times for all endpoints
- **Requirement 23.5**: Record container creation and destruction metrics
- **Requirement 23.6**: Record database query performance metrics
- **Requirement 23.7**: Use Application Insights for centralized telemetry collection
- **Requirement 23.8**: Create dashboards showing key performance indicators
- **Requirement 23.9**: Alert administrators when error rate exceeds 5%
- **Requirement 23.10**: Alert administrators when API response time exceeds 2 seconds
- **Requirement 23.11**: Anonymize user data in telemetry to protect privacy

## Setup

### 1. Azure Application Insights Resource

First, create an Application Insights resource in Azure:

```bash
# Using Azure CLI
az monitor app-insights component create \
  --app platform-saas-insights \
  --location eastus \
  --resource-group platform-saas-rg \
  --application-type web

# Get the connection string
az monitor app-insights component show \
  --app platform-saas-insights \
  --resource-group platform-saas-rg \
  --query connectionString -o tsv
```

### 2. Configuration

Add the Application Insights connection string to your service configuration:

**appsettings.json**:
```json
{
  "ApplicationInsights": {
    "ConnectionString": "InstrumentationKey=...;IngestionEndpoint=https://...;LiveEndpoint=https://..."
  }
}
```

**Environment Variables** (recommended for production):
```bash
export ApplicationInsights__ConnectionString="InstrumentationKey=...;IngestionEndpoint=https://...;LiveEndpoint=https://..."
```

**Kubernetes Secret**:
```yaml
apiVersion: v1
kind: Secret
metadata:
  name: appinsights-secret
type: Opaque
stringData:
  connection-string: "InstrumentationKey=...;IngestionEndpoint=https://...;LiveEndpoint=https://..."
```

### 3. Service Integration

Add Application Insights to your service's `Program.cs`:

```csharp
using Shared.Telemetry;

var builder = WebApplication.CreateBuilder(args);

// Add Application Insights telemetry
builder.Services.AddApplicationInsightsTelemetry(
    builder.Configuration,
    "YourServiceName"
);

// Add HTTP context accessor for correlation
builder.Services.AddHttpContextAccessor();

// Add custom telemetry tracker
builder.Services.AddSingleton<ICustomTelemetryTracker, CustomTelemetryTracker>();

var app = builder.Build();

// Add correlation ID middleware (must be early in pipeline)
app.UseMiddleware<CorrelationIdMiddleware>();

// ... rest of your middleware

app.Run();
```

## Custom Telemetry Tracking

### Code Execution Tracking

Track code execution metrics:

```csharp
public class CodeExecutionService
{
    private readonly ICustomTelemetryTracker _telemetry;

    public CodeExecutionService(ICustomTelemetryTracker telemetry)
    {
        _telemetry = telemetry;
    }

    public async Task<ExecutionResult> ExecuteCodeAsync(string userId, string challengeId, string code)
    {
        var sw = Stopwatch.StartNew();
        
        try
        {
            var result = await ExecuteAsync(code);
            sw.Stop();
            
            _telemetry.TrackCodeExecution(
                userId,
                challengeId,
                sw.ElapsedMilliseconds,
                result.Success,
                result.Error
            );
            
            return result;
        }
        catch (Exception ex)
        {
            sw.Stop();
            _telemetry.TrackCodeExecution(userId, challengeId, sw.ElapsedMilliseconds, false, ex.Message);
            throw;
        }
    }
}
```

### SQL Execution Tracking

Track SQL query execution:

```csharp
public class SqlExecutionService
{
    private readonly ICustomTelemetryTracker _telemetry;

    public async Task<QueryResult> ExecuteQueryAsync(string userId, string query)
    {
        var queryHash = ComputeHash(query);
        var sw = Stopwatch.StartNew();
        
        try
        {
            var result = await ExecuteAsync(query);
            sw.Stop();
            
            _telemetry.TrackSqlExecution(
                userId,
                queryHash,
                sw.ElapsedMilliseconds,
                true,
                result.RowCount
            );
            
            return result;
        }
        catch (Exception ex)
        {
            sw.Stop();
            _telemetry.TrackSqlExecution(userId, queryHash, sw.ElapsedMilliseconds, false);
            throw;
        }
    }
}
```

### Compilation Error Tracking

Track compilation errors:

```csharp
public void HandleCompilationError(string userId, string challengeId, CompilationError error)
{
    _telemetry.TrackCompilationError(
        userId,
        challengeId,
        error.Type,
        error.Message
    );
}
```

### User Activity Tracking

Track user activities:

```csharp
public void TrackUserLogin(string userId)
{
    _telemetry.TrackUserActivity(userId, "Login", new Dictionary<string, string>
    {
        ["LoginTime"] = DateTime.UtcNow.ToString("O"),
        ["Source"] = "Web"
    });
}

public void TrackLessonCompleted(string userId, string lessonId)
{
    _telemetry.TrackUserActivity(userId, "LessonCompleted", new Dictionary<string, string>
    {
        ["LessonId"] = lessonId,
        ["CompletionTime"] = DateTime.UtcNow.ToString("O")
    });
}
```

### API Performance Tracking

Track API performance (automatically done by middleware, but can be done manually):

```csharp
public async Task<IResult> HandleRequest(HttpContext context)
{
    var sw = Stopwatch.StartNew();
    
    try
    {
        var result = await ProcessRequest(context);
        sw.Stop();
        
        _telemetry.TrackApiPerformance(
            context.Request.Path,
            context.Request.Method,
            sw.ElapsedMilliseconds,
            context.Response.StatusCode
        );
        
        return result;
    }
    catch (Exception ex)
    {
        sw.Stop();
        _telemetry.TrackApiPerformance(
            context.Request.Path,
            context.Request.Method,
            sw.ElapsedMilliseconds,
            500
        );
        throw;
    }
}
```

### Container Operations Tracking

Track Docker container operations:

```csharp
public async Task<string> CreateContainerAsync()
{
    var sw = Stopwatch.StartNew();
    
    try
    {
        var containerId = await _docker.CreateContainerAsync(...);
        sw.Stop();
        
        _telemetry.TrackContainerOperation("Create", sw.ElapsedMilliseconds, true);
        
        return containerId;
    }
    catch (Exception ex)
    {
        sw.Stop();
        _telemetry.TrackContainerOperation("Create", sw.ElapsedMilliseconds, false);
        throw;
    }
}
```

### Database Query Tracking

Track database queries:

```csharp
public async Task<List<User>> GetUsersAsync()
{
    var sw = Stopwatch.StartNew();
    
    try
    {
        var users = await _context.Users.ToListAsync();
        sw.Stop();
        
        _telemetry.TrackDatabaseQuery("SELECT", "Users", sw.ElapsedMilliseconds, true);
        
        return users;
    }
    catch (Exception ex)
    {
        sw.Stop();
        _telemetry.TrackDatabaseQuery("SELECT", "Users", sw.ElapsedMilliseconds, false);
        throw;
    }
}
```

### Cache Operations Tracking

Track cache hits and misses:

```csharp
public async Task<T?> GetFromCacheAsync<T>(string key)
{
    var value = await _cache.GetAsync<T>(key);
    
    _telemetry.TrackCacheOperation("Get", key, value != null);
    
    return value;
}
```

### Custom Operations

Track custom operations with start/stop:

```csharp
public async Task ProcessComplexOperation()
{
    using var operation = _telemetry.StartOperation("ComplexOperation");
    
    try
    {
        // Step 1
        await Step1();
        
        // Step 2
        await Step2();
        
        operation.Telemetry.Success = true;
    }
    catch (Exception ex)
    {
        operation.Telemetry.Success = false;
        throw;
    }
}
```

## Distributed Tracing with Correlation IDs

The platform uses correlation IDs to trace requests across multiple services:

### How It Works

1. **Request Initiation**: When a request enters the system (e.g., from the frontend), the API Gateway generates a correlation ID
2. **Header Propagation**: The correlation ID is passed in the `X-Correlation-ID` header to all downstream services
3. **Telemetry Enrichment**: All telemetry items (logs, traces, metrics) are tagged with the correlation ID
4. **Cross-Service Tracing**: You can trace a single user request across all microservices using the correlation ID

### Example Flow

```
Frontend Request
  └─> API Gateway (generates correlation-id: abc-123)
      ├─> Auth Service (receives correlation-id: abc-123)
      ├─> Code Executor Service (receives correlation-id: abc-123)
      │   └─> Docker Container (tagged with correlation-id: abc-123)
      └─> Gamification Service (receives correlation-id: abc-123)
          └─> Database Query (tagged with correlation-id: abc-123)
```

### Querying by Correlation ID

In Application Insights, you can query all telemetry for a specific request:

```kusto
union requests, dependencies, traces, exceptions
| where customDimensions.CorrelationId == "abc-123"
| order by timestamp asc
| project timestamp, itemType, name, operation_Name, customDimensions
```

### Propagating Correlation IDs

When making HTTP calls to other services:

```csharp
public async Task<HttpResponseMessage> CallDownstreamService(string correlationId)
{
    var request = new HttpRequestMessage(HttpMethod.Get, "https://downstream-service/api/endpoint");
    
    // Propagate correlation ID
    request.Headers.Add("X-Correlation-ID", correlationId);
    
    return await _httpClient.SendAsync(request);
}
```

## Dashboards and Alerts

### Key Performance Indicators (KPIs)

Create dashboards in Application Insights to monitor:

1. **API Performance**
   - Average response time by endpoint
   - P95 and P99 response times
   - Request rate (requests per second)
   - Error rate percentage

2. **Code Execution**
   - Average execution time
   - Success rate
   - Compilation error frequency
   - Container creation time

3. **Database Performance**
   - Query execution time
   - Query failure rate
   - Connection pool usage

4. **User Activity**
   - Active users
   - Session duration
   - Feature usage patterns

### Alert Rules

Configure alerts for critical metrics:

**Slow API Response Time**:
```kusto
requests
| where duration > 2000
| summarize count() by bin(timestamp, 5m)
| where count_ > 10
```

**High Error Rate**:
```kusto
requests
| where success == false
| summarize ErrorRate = (count() * 100.0) / toscalar(requests | count())
| where ErrorRate > 5
```

**Container Operation Failures**:
```kusto
customEvents
| where name == "ContainerOperation"
| where customDimensions.Success == "False"
| summarize count() by bin(timestamp, 5m)
| where count_ > 5
```

## Privacy and Data Anonymization

To comply with GDPR and protect user privacy:

1. **User IDs**: Use hashed or anonymized user IDs in telemetry
2. **PII Filtering**: Never log passwords, tokens, or sensitive personal information
3. **Data Retention**: Configure appropriate data retention policies in Application Insights
4. **Sampling**: Use adaptive sampling to reduce data volume while maintaining statistical accuracy

### Example: Anonymizing User Data

```csharp
public string AnonymizeUserId(string userId)
{
    using var sha256 = SHA256.Create();
    var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(userId));
    return Convert.ToBase64String(hash);
}

public void TrackUserActivity(string userId, string activity)
{
    var anonymizedUserId = AnonymizeUserId(userId);
    _telemetry.TrackUserActivity(anonymizedUserId, activity);
}
```

## Troubleshooting

### No Telemetry Appearing

1. Check connection string is correct
2. Verify network connectivity to Application Insights endpoint
3. Check sampling settings (may be filtering out data)
4. Verify service is calling `AddApplicationInsightsTelemetry()`

### High Telemetry Volume

1. Enable adaptive sampling
2. Filter out health check requests (already done by `HealthCheckTelemetryProcessor`)
3. Reduce custom event frequency
4. Use aggregated metrics instead of individual events

### Missing Correlation IDs

1. Ensure `CorrelationIdMiddleware` is registered early in the pipeline
2. Verify `X-Correlation-ID` header is being propagated to downstream services
3. Check that `HttpContextAccessor` is registered

## Best Practices

1. **Use Structured Logging**: Use structured properties instead of string interpolation
2. **Avoid PII**: Never log sensitive personal information
3. **Use Sampling**: Enable adaptive sampling for high-volume applications
4. **Set Operation Names**: Use meaningful operation names for better grouping
5. **Track Business Events**: Track important business events, not just technical metrics
6. **Monitor Costs**: Application Insights charges by data volume - monitor your usage
7. **Use Dependency Tracking**: Track external dependencies (databases, APIs, caches)
8. **Set Up Alerts**: Configure alerts for critical metrics and error conditions

## References

- [Application Insights Documentation](https://docs.microsoft.com/en-us/azure/azure-monitor/app/app-insights-overview)
- [Distributed Tracing](https://docs.microsoft.com/en-us/azure/azure-monitor/app/distributed-tracing)
- [Telemetry Correlation](https://docs.microsoft.com/en-us/azure/azure-monitor/app/correlation)
- [Sampling](https://docs.microsoft.com/en-us/azure/azure-monitor/app/sampling)
