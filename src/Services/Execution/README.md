# Execution Service - Redis Job Queue

This service implements the Redis-based job queue infrastructure for the code execution system.

## Features Implemented (Task 9.1)

### 1. Redis Connection Configuration
- StackExchange.Redis client configured with connection retry logic
- Connection timeout: 5 seconds
- Retry attempts: 3
- Configurable connection string via appsettings.json

### 2. Job Queue Operations

#### EnqueueJobAsync
- Adds jobs to Redis list using LPUSH
- Serializes ExecutionJob to JSON
- **Validates: Requirements 12.2**

#### DequeueJobAsync
- Dequeues jobs from Redis list using RPOP (non-blocking)
- Deserializes JSON to ExecutionJob
- Returns null if queue is empty
- **Validates: Requirements 12.3**

#### StoreResultAsync
- Caches execution results in Redis with 5-minute TTL
- Uses key pattern: `result:{jobId}`
- Serializes ExecutionResult to JSON
- **Validates: Requirements 12.1, 12.3**

#### GetResultAsync
- Retrieves execution results from Redis
- Returns null if result not found or expired

## Models

### ExecutionJob
```csharp
public class ExecutionJob
{
    public Guid JobId { get; set; }
    public string Code { get; set; }
    public List<string> Files { get; set; }
    public string EntryPoint { get; set; }
    public DateTime EnqueuedAt { get; set; }
}
```

### ExecutionResult
```csharp
public class ExecutionResult
{
    public Guid JobId { get; set; }
    public ExecutionStatus Status { get; set; }
    public string? Output { get; set; }
    public string? Error { get; set; }
    public int? ExitCode { get; set; }
    public long ExecutionTimeMs { get; set; }
    public DateTime CompletedAt { get; set; }
}
```

### ExecutionStatus Enum
- Queued
- Running
- Completed
- Failed
- Timeout
- MemoryExceeded

## API Endpoints

### POST /api/execution/execute
Enqueues a code execution job.

**Request:**
```json
{
  "code": "Console.WriteLine(\"Hello, World!\");",
  "files": ["Program.cs"],
  "entryPoint": "Program.cs"
}
```

**Response (202 Accepted):**
```json
{
  "jobId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "status": "Queued"
}
```

### GET /api/execution/status/{jobId}
Retrieves the status and result of a job.

**Response (200 OK):**
```json
{
  "jobId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "status": "Completed",
  "output": "Hello, World!\n",
  "error": null,
  "exitCode": 0,
  "executionTimeMs": 150,
  "completedAt": "2024-01-15T10:30:00Z"
}
```

## Configuration

### appsettings.json
```json
{
  "Redis": {
    "ConnectionString": "localhost:6379"
  }
}
```

For production, use environment variables:
```bash
export Redis__ConnectionString="redis-server:6379,password=yourpassword"
```

## Testing

Run unit tests:
```bash
dotnet test tests/Execution.Tests/Execution.Tests.csproj
```

All tests validate:
- Job enqueueing and dequeueing
- Result storage with TTL
- Null handling
- JSON serialization/deserialization
- Error handling

## Next Steps (Remaining Task 9 Subtasks)

- 9.2: Write property tests for job queue ✅ COMPLETED
- 9.3: Create Docker container manager ✅ COMPLETED
- 9.4: Write property test for container cleanup
- 9.5: Implement prohibited code scanner
- 9.6: Write property test for prohibited code detection
- 9.7: Implement execution worker
- 9.8: Write property tests for execution service
- 9.9: Create ExecutionService API
- 9.10: Write unit tests for execution service

## Dependencies

- StackExchange.Redis 2.8.16
- Docker.DotNet 3.125.15
- ASP.NET Core 10.0
- System.Text.Json (built-in)

## Docker Container Manager (Task 9.3)

### Features Implemented

#### 1. Container Creation with Resource Limits
- **Memory Limit**: 512MB (no swap)
- **CPU Limit**: 1 CPU core
- **Process Limit**: Maximum 50 PIDs (prevents fork bombs)
- **Network**: Disabled (no network access)
- **Base Image**: mcr.microsoft.com/dotnet/sdk:8.0-alpine
- **Validates: Requirements 3.3, 3.4, 3.5, 14.1, 14.2**

#### 2. Container Execution with Timeout
- **Default Timeout**: 30 seconds
- **Timeout Enforcement**: Automatically stops containers that exceed time limit
- **Output Capture**: Captures both stdout and stderr
- **Memory Monitoring**: Detects OOM (Out of Memory) kills
- **Validates: Requirements 3.7, 3.8, 14.3**

#### 3. Container Cleanup
- **Automatic Cleanup**: Stops and removes containers after execution
- **Force Removal**: Uses force flag to ensure cleanup even if container is stuck
- **Volume Cleanup**: Removes associated volumes
- **Error Handling**: Best-effort cleanup that doesn't throw on missing containers
- **Validates: Requirements 14.6**

### API

#### IDockerContainerManager Interface
```csharp
public interface IDockerContainerManager
{
    Task<string> CreateContainerAsync(string code, List<string> files, string entryPoint);
    Task<ContainerExecutionResult> StartContainerAsync(string containerId, TimeSpan timeout);
    Task StopAndRemoveContainerAsync(string containerId);
}
```

#### ContainerExecutionResult Model
```csharp
public class ContainerExecutionResult
{
    public string Output { get; set; }
    public string Error { get; set; }
    public int ExitCode { get; set; }
    public long ExecutionTimeMs { get; set; }
    public bool TimedOut { get; set; }
    public bool MemoryExceeded { get; set; }
}
```

### Usage Example

```csharp
var dockerClient = new DockerClientConfiguration().CreateClient();
var containerManager = new DockerContainerManager(dockerClient);

// Create container
var containerId = await containerManager.CreateContainerAsync(
    code: "Console.WriteLine(\"Hello, World!\");",
    files: new List<string> { "Program.cs" },
    entryPoint: "Program.cs"
);

// Start and execute
var result = await containerManager.StartContainerAsync(
    containerId,
    timeout: TimeSpan.FromSeconds(30)
);

// Check results
if (result.TimedOut)
{
    Console.WriteLine("Execution timed out");
}
else if (result.MemoryExceeded)
{
    Console.WriteLine("Memory limit exceeded");
}
else
{
    Console.WriteLine($"Output: {result.Output}");
    Console.WriteLine($"Exit Code: {result.ExitCode}");
}

// Cleanup
await containerManager.StopAndRemoveContainerAsync(containerId);
```

### Security Features

1. **Resource Isolation**: Each execution runs in a separate container
2. **Memory Limits**: Prevents memory exhaustion attacks
3. **CPU Limits**: Prevents CPU hogging
4. **Process Limits**: Prevents fork bomb attacks
5. **Network Isolation**: No network access to prevent external communication
6. **Timeout Enforcement**: Prevents infinite loops and long-running attacks

### Testing

Run unit tests:
```bash
dotnet test tests/Execution.Tests/Execution.Tests.csproj --filter "FullyQualifiedName~DockerContainerManagerTests"
```

All tests validate:
- Argument validation (null/empty checks)
- Constructor validation
- ContainerExecutionResult model behavior
