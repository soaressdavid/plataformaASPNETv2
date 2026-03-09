# Container Pool Manager

## Overview

The Container Pool Manager is a sophisticated system for managing Docker containers used for code execution. It implements warm pooling, auto-scaling, and session-based container reuse to optimize performance and resource utilization.

**Validates Requirements**: 7.1, 7.2, 21.1

## Features

### 1. Warm Pool (Requirement 7.1)
- **10 pre-initialized containers** ready for immediate use
- Containers are created on service startup
- Reduces cold-start latency for code execution
- Containers remain running with `tail -f /dev/null` command

### 2. Auto-Scaling (Requirement 21.1)
- **Dynamic scaling** based on queue length
- **Maximum 100 containers** to prevent resource exhaustion
- **Scale-up**: Adds containers when queue length exceeds available containers
- **Scale-down**: Removes excess idle containers (keeps minimum 2x warm pool size)
- Thread-safe scaling operations with semaphore locking

### 3. Container Reuse (Requirement 7.2)
- **Session-based container association** with 5-minute TTL
- Same user session reuses the same container
- Reduces overhead of container creation/destruction
- Session mappings stored in Redis with automatic expiration

### 4. Resource Limits
- **Memory**: 512MB per container
- **CPU**: 1 core per container
- **Network**: Isolated (no network access)
- **Workspace**: `/workspace` directory for code files

### 5. Health Checks
- **Periodic health checks** every 30 seconds
- Automatically removes unhealthy containers
- Verifies container status via Docker API

### 6. Cleanup
- **Automatic cleanup** every 60 seconds
- Removes expired containers (idle > 5 minutes)
- Cleans up orphaned session associations

## Architecture

```
┌─────────────────────────────────────────────────────────────┐
│                   Container Pool Manager                     │
├─────────────────────────────────────────────────────────────┤
│                                                               │
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐      │
│  │  Available   │  │   In Use     │  │   Session    │      │
│  │  Containers  │  │  Containers  │  │   Mapping    │      │
│  │   (Queue)    │  │  (Dict)      │  │   (Redis)    │      │
│  └──────────────┘  └──────────────┘  └──────────────┘      │
│                                                               │
│  ┌──────────────────────────────────────────────────────┐   │
│  │           Auto-Scaling Logic                         │   │
│  │  • Scale up when queue > available                   │   │
│  │  • Scale down when idle > 2x warm pool               │   │
│  │  • Max 100 containers                                │   │
│  └──────────────────────────────────────────────────────┘   │
│                                                               │
└─────────────────────────────────────────────────────────────┘
         │                    │                    │
         ▼                    ▼                    ▼
   ┌──────────┐        ┌──────────┐        ┌──────────┐
   │  Docker  │        │  Redis   │        │   Job    │
   │  Client  │        │  Cache   │        │  Queue   │
   └──────────┘        └──────────┘        └──────────┘
```

## Usage

### Initialization

The container pool is automatically initialized on service startup:

```csharp
var poolManager = app.Services.GetRequiredService<IContainerPoolManager>();
await poolManager.InitializeWarmPoolAsync();
```

### Acquiring a Container

```csharp
// Without session (new container from pool)
var containerId = await poolManager.AcquireContainerAsync();

// With session (reuses container if exists)
var containerId = await poolManager.AcquireContainerAsync(sessionId: "user-session-123");
```

### Releasing a Container

```csharp
// Release without session (returns to pool immediately)
await poolManager.ReleaseContainerAsync(containerId);

// Release with session (keeps association for reuse)
await poolManager.ReleaseContainerAsync(containerId, sessionId: "user-session-123");
```

### Destroying a Container

```csharp
await poolManager.DestroyContainerAsync(containerId);
```

### Getting Pool Statistics

```csharp
var stats = await poolManager.GetPoolStatsAsync();
Console.WriteLine($"Total: {stats.TotalContainers}");
Console.WriteLine($"Available: {stats.AvailableContainers}");
Console.WriteLine($"In Use: {stats.InUseContainers}");
Console.WriteLine($"Queue Length: {stats.QueueLength}");
```

## API Endpoints

### GET /api/pool/stats

Returns current pool statistics:

```json
{
  "totalContainers": 15,
  "availableContainers": 8,
  "inUseContainers": 7,
  "warmPoolSize": 10,
  "queueLength": 3,
  "lastScalingAction": "2025-03-08T10:30:00Z"
}
```

## Background Services

### ContainerPoolMaintenanceService

Runs two periodic tasks:

1. **Health Checks** (every 30 seconds)
   - Inspects all containers
   - Removes unhealthy containers
   - Logs health status

2. **Cleanup** (every 60 seconds)
   - Removes expired containers
   - Triggers scale-down if needed
   - Logs pool statistics

## Configuration

### Constants (in ContainerPoolManager.cs)

```csharp
private const int WarmPoolSize = 10;              // Initial pool size
private const int MaxPoolSize = 100;              // Maximum containers
private const int SessionTtlMinutes = 5;          // Session TTL
private const int HealthCheckIntervalSeconds = 30; // Health check frequency
private const int CleanupIntervalSeconds = 60;    // Cleanup frequency
```

### Redis Keys

- `execution:session:container:{sessionId}` - Maps session to container
- `execution:container:session:{containerId}` - Maps container to session

## Performance Characteristics

### Warm Pool Benefits
- **Cold start**: ~2-3 seconds (container creation + startup)
- **Warm start**: ~50-100ms (acquire from pool)
- **Session reuse**: ~10-20ms (Redis lookup)

### Scaling Behavior
- **Scale-up trigger**: Queue length > available containers
- **Scale-up amount**: Min(queue - available, max - total)
- **Scale-down trigger**: Available > 2x warm pool size
- **Scale-down amount**: Available - (2x warm pool size)

### Resource Usage
- **Minimum**: 10 containers × 512MB = 5GB RAM
- **Maximum**: 100 containers × 512MB = 50GB RAM
- **Typical**: 15-30 containers × 512MB = 7.5-15GB RAM

## Monitoring

### Metrics to Track
- Pool size (total, available, in-use)
- Queue length
- Container creation/destruction rate
- Session reuse rate
- Health check failures
- Scaling events

### Logs
- Container creation/destruction
- Scaling actions
- Health check results
- Session associations
- Pool statistics (every 60s)

## Error Handling

### Container Creation Failures
- Logs error and releases semaphore
- Does not add to pool
- Retries on next scale-up

### Container Health Failures
- Removes unhealthy container
- Logs warning
- Pool automatically replenishes on next scale-up

### Redis Connection Failures
- Session reuse falls back to new container
- Logs error
- Continues operation without session tracking

## Testing

Unit tests are available in `tests/Execution.Tests/ContainerPoolManagerTests.cs`:

- Warm pool initialization
- Container acquisition and release
- Session-based reuse
- Auto-scaling behavior
- Health checks
- Pool exhaustion handling

Run tests:
```bash
dotnet test tests/Execution.Tests/Execution.Tests.csproj --filter "FullyQualifiedName~ContainerPoolManagerTests"
```

## Future Enhancements

1. **Container Image Caching**: Pre-pull images to reduce creation time
2. **Multi-tier Pooling**: Different pools for different project types
3. **Predictive Scaling**: ML-based scaling based on usage patterns
4. **Container Snapshots**: Save/restore container state for faster reuse
5. **Geographic Distribution**: Pools in multiple regions for lower latency
6. **Cost Optimization**: Spot instances for non-critical workloads
