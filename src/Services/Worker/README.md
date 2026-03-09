# Execution Worker Service

Background worker service that processes code execution jobs from the Redis queue.

## Features

- **Job Queue Polling**: Uses Redis BRPOP (blocking pop) to efficiently poll for jobs
- **Code Security Scanning**: Scans submitted code for prohibited operations before execution
- **Container Management**: Creates isolated Docker containers with resource limits
- **Timeout Handling**: Enforces 30-second execution timeout
- **Memory Limit Handling**: Enforces 512MB memory limit
- **Result Storage**: Stores execution results in Redis with 5-minute TTL
- **Job Requeue**: Automatically requeues jobs on worker failure
- **Container Cleanup**: Ensures containers are always cleaned up after execution

## Resource Limits

Each code execution runs in an isolated Docker container with:
- **Memory**: 512MB (no swap)
- **CPU**: 1 core
- **PIDs**: 50 max processes (prevents fork bombs)
- **Network**: Disabled (no network access)
- **Timeout**: 30 seconds

## Configuration

### appsettings.json

```json
{
  "ConnectionStrings": {
    "Redis": "localhost:6379"
  },
  "Docker": {
    "Uri": "unix:///var/run/docker.sock"
  }
}
```

### Environment Variables

- `ConnectionStrings__Redis`: Redis connection string
- `Docker__Uri`: Docker daemon URI

## Running the Worker

### Local Development

```bash
dotnet run
```

### Docker

```bash
docker build -t execution-worker .
docker run -d \
  -e ConnectionStrings__Redis=redis:6379 \
  -v /var/run/docker.sock:/var/run/docker.sock \
  execution-worker
```

**Note**: The worker needs access to the Docker daemon socket to create containers.

## Scaling

Multiple worker instances can run concurrently to process jobs in parallel. Each worker:
- Polls the same Redis queue
- Processes jobs independently
- Handles its own container lifecycle
- Stores results independently

To scale horizontally, simply run multiple instances of the worker service.

## Error Handling

### Job Requeue

If a worker fails while processing a job, the job is automatically requeued for retry by another worker, unless:
- The job is older than 5 minutes (considered stale)
- The job has already been retried multiple times

### Container Cleanup

Containers are always cleaned up in the `finally` block, even if:
- The job fails
- The worker crashes
- The execution times out
- Memory limit is exceeded

## Monitoring

The worker logs:
- Job processing start/completion
- Execution status and timing
- Container creation/cleanup
- Errors and warnings
- Requeue operations

Log levels:
- `Information`: Job lifecycle events
- `Debug`: Detailed execution steps
- `Warning`: Prohibited code, requeue operations
- `Error`: Worker failures, container errors

## Requirements Validated

- **3.2**: Worker dequeues jobs and creates containers
- **3.6**: Returns execution output within time limits
- **3.7**: Enforces 30-second timeout
- **3.8**: Enforces 512MB memory limit
- **3.9**: Isolates each execution in separate container
- **12.3**: Polls Redis queue with BRPOP
- **12.5**: Supports multiple workers processing concurrently
- **12.6**: Requeues jobs on worker failure
- **14.4, 14.5**: Scans code for prohibited operations
- **14.6**: Cleans up containers after execution
