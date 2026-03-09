# Centralized Logging with Serilog

This directory contains the centralized logging configuration for all microservices in the ASP.NET Core Learning Platform.

## Overview

All services use Serilog for structured logging with:
- **Console sink**: Human-readable logs for development
- **File sink**: JSON-formatted logs for production analysis
- **Structured logging**: Enriched with context (userId, requestId, traceId)
- **Environment-specific log levels**: Different verbosity per environment

## Components

### SerilogConfiguration.cs
Extension methods for configuring Serilog in all services:
- `ConfigureSerilog()`: Sets up Serilog with console and file sinks
- `UseSerilogRequestLogging()`: Adds HTTP request logging middleware
- `GetMinimumLogLevel()`: Returns environment-specific log levels

### LoggingEnrichmentMiddleware.cs
Middleware that enriches logs with:
- **RequestId**: Unique identifier for each request (X-Request-Id header)
- **TraceId**: Distributed tracing identifier (X-Trace-Id header)
- **UserId**: User identifier from authentication context
- **RequestPath**: HTTP request path
- **RequestMethod**: HTTP method (GET, POST, etc.)

## Log Levels by Environment

| Environment | Default Level | Microsoft | System |
|-------------|---------------|-----------|--------|
| Development | Debug         | Information | Warning |
| Staging     | Information   | Warning   | Warning |
| Production  | Warning       | Warning   | Error   |

## Log Sinks

### Console Sink
- **Format**: Human-readable with timestamp, level, service name, message, and properties
- **Template**: `[{Timestamp:HH:mm:ss} {Level:u3}] [{ServiceName}] {Message:lj} {Properties:j}{NewLine}{Exception}`
- **Use case**: Development and debugging

### File Sink
- **Format**: Compact JSON (CompactJsonFormatter)
- **Location**: `logs/{ServiceName}-.log`
- **Rolling**: Daily
- **Retention**: 30 days
- **Size limit**: 100MB per file
- **Use case**: Production log aggregation and analysis

## Structured Logging Properties

All logs are enriched with:
- **ServiceName**: Name of the microservice (e.g., "ApiGateway", "AuthService")
- **Environment**: Environment name (Development, Staging, Production)
- **MachineName**: Host machine name
- **Application**: "ASP.NET Learning Platform"
- **RequestId**: Unique request identifier
- **TraceId**: Distributed tracing identifier
- **UserId**: Authenticated user ID (when available)

## Usage in Services

### Web Applications (API Gateway, Auth, Course, Challenge, Progress, Execution, AITutor)

```csharp
using Shared.Logging;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
builder.ConfigureSerilog("ServiceName");

// ... other service configuration ...

var app = builder.Build();

// Add Serilog request logging
app.UseSerilogRequestLogging();

// Add logging enrichment middleware
app.UseMiddleware<LoggingEnrichmentMiddleware>();

// ... other middleware ...

app.Run();

// Ensure logs are flushed on shutdown
Log.CloseAndFlush();
```

### Background Services (Worker)

```csharp
using Serilog;
using Serilog.Formatting.Compact;

var builder = Host.CreateApplicationBuilder(args);

// Configure Serilog for background service
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .Enrich.WithProperty("ServiceName", "ExecutionWorker")
    .Enrich.WithProperty("Environment", builder.Environment.EnvironmentName)
    .Enrich.WithMachineName()
    .WriteTo.Console(...)
    .WriteTo.File(...)
    .CreateLogger();

builder.Services.AddSerilog();

// ... service configuration ...

var host = builder.Build();

try
{
    Log.Information("Starting service");
    host.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Service terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
```

## Logging Best Practices

### Use Structured Logging
```csharp
// Good - structured
_logger.LogInformation("User {UserId} completed challenge {ChallengeId} with score {Score}", 
    userId, challengeId, score);

// Bad - string interpolation
_logger.LogInformation($"User {userId} completed challenge {challengeId} with score {score}");
```

### Use Appropriate Log Levels
- **Debug**: Detailed information for diagnosing issues (development only)
- **Information**: General informational messages (important events)
- **Warning**: Unexpected situations that don't prevent operation
- **Error**: Errors that prevent specific operations
- **Fatal**: Critical errors that cause application shutdown

### Add Context to Logs
```csharp
using (LogContext.PushProperty("ChallengeId", challengeId))
{
    _logger.LogInformation("Processing challenge submission");
    // All logs in this scope will include ChallengeId
}
```

### Log Exceptions Properly
```csharp
try
{
    // operation
}
catch (Exception ex)
{
    _logger.LogError(ex, "Failed to process submission for user {UserId}", userId);
    throw;
}
```

## Configuration Files

Each service has its own appsettings files:
- `appsettings.json`: Production configuration (Warning level)
- `appsettings.Development.json`: Development configuration (Debug level)
- `appsettings.Staging.json`: Staging configuration (Information level)

## Log File Structure

Logs are written to the `logs/` directory in each service:
```
logs/
├── ApiGateway-20240115.log
├── AuthService-20240115.log
├── CourseService-20240115.log
├── ChallengeService-20240115.log
├── ProgressService-20240115.log
├── ExecutionService-20240115.log
├── AITutorService-20240115.log
└── ExecutionWorker-20240115.log
```

Each log file contains JSON-formatted entries:
```json
{
  "@t": "2024-01-15T10:30:00.123Z",
  "@l": "Information",
  "@mt": "User {UserId} completed challenge {ChallengeId}",
  "UserId": "123e4567-e89b-12d3-a456-426614174000",
  "ChallengeId": "789e4567-e89b-12d3-a456-426614174000",
  "ServiceName": "ChallengeService",
  "Environment": "Production",
  "MachineName": "web-server-01",
  "RequestId": "abc123-def456",
  "TraceId": "xyz789-uvw012"
}
```

## Distributed Tracing

The logging system supports distributed tracing across microservices:

1. **API Gateway** generates a TraceId for each incoming request
2. **TraceId** is propagated via X-Trace-Id header to downstream services
3. All services log with the same TraceId
4. Logs can be correlated across services using TraceId

Example flow:
```
Client Request → API Gateway (TraceId: abc123)
                 ↓
                 Auth Service (TraceId: abc123)
                 ↓
                 Challenge Service (TraceId: abc123)
                 ↓
                 Progress Service (TraceId: abc123)
```

## Monitoring and Alerting

For production deployments, consider integrating with:
- **Seq**: Structured log server (https://datalust.co/seq)
- **ELK Stack**: Elasticsearch, Logstash, Kibana
- **Splunk**: Enterprise log management
- **Azure Application Insights**: Cloud-native monitoring
- **Grafana Loki**: Log aggregation system

## NuGet Packages Required

All services require these packages (already included in Shared.csproj):
- `Serilog.AspNetCore` (10.0.0)
- `Serilog.Enrichers.Environment` (3.1.0)
- `Serilog.Formatting.Compact` (3.0.0)
- `Serilog.Sinks.Console` (6.0.0)
- `Serilog.Sinks.File` (6.0.0)

## Testing Logging

To test logging in development:

1. Run a service:
   ```bash
   cd src/Services/Auth
   dotnet run
   ```

2. Check console output for formatted logs
3. Check `logs/` directory for JSON log files
4. Verify structured properties are included

## Troubleshooting

### Logs not appearing
- Check that `builder.ConfigureSerilog()` is called before building the app
- Verify appsettings.json has correct Serilog configuration
- Ensure log directory is writable

### Missing context properties
- Verify `LoggingEnrichmentMiddleware` is registered in middleware pipeline
- Check that authentication middleware sets UserId in HttpContext.Items
- Ensure X-Trace-Id header is propagated from API Gateway

### Log files too large
- Adjust `fileSizeLimitBytes` in appsettings.json
- Reduce `retainedFileCountLimit` for shorter retention
- Increase log level to reduce verbosity
