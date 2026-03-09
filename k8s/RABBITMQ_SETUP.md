# RabbitMQ Message Queue Setup

This document describes the RabbitMQ message queue infrastructure for the ASP.NET Learning Platform.

## Overview

RabbitMQ is configured as a high-availability cluster with 3 replicas for asynchronous communication between microservices. The setup includes:

- **High Availability Cluster**: 3-node StatefulSet with automatic peer discovery
- **Exchanges**: Topic exchanges for flexible routing
- **Queues**: Durable queues for each microservice with dead letter queue support
- **Dead Letter Queues**: Automatic handling of failed messages
- **Persistent Storage**: 10GB PersistentVolumeClaim per node

## Architecture

### Exchanges

| Exchange Name | Type | Purpose |
|--------------|------|---------|
| `learning.events` | topic | Main event exchange for all domain events |
| `execution.events` | topic | Code and SQL execution events |
| `gamification.events` | topic | XP, achievements, and level-up events |
| `notification.events` | topic | Email, push, and in-app notifications |
| `analytics.events` | topic | Analytics and telemetry events |
| `dlx.events` | topic | Dead letter exchange for failed messages |

### Queues by Microservice

#### Code Executor Service
- `code-executor.requests` - Incoming code execution requests
  - Routing key: `execution.request.*`
  - TTL: 5 minutes
  - Max length: 10,000 messages
  - DLX: `dlx.events` → `code-executor.failed`

- `code-executor.results` - Execution results
  - Routing key: `execution.completed.*`
  - TTL: 5 minutes
  - DLX: `dlx.events` → `code-executor.results.failed`

#### SQL Executor Service
- `sql-executor.requests` - Incoming SQL execution requests
  - Routing key: `sql.request.*`
  - TTL: 5 minutes
  - Max length: 10,000 messages
  - DLX: `dlx.events` → `sql-executor.failed`

- `sql-executor.results` - SQL execution results
  - Routing key: `sql.completed.*`
  - TTL: 5 minutes
  - DLX: `dlx.events` → `sql-executor.results.failed`

#### Gamification Engine
- `gamification.xp-awarded` - XP award events
  - Routing key: `gamification.xp.*`
  - DLX: `dlx.events` → `gamification.xp.failed`

- `gamification.achievements` - Achievement unlock events
  - Routing key: `gamification.achievement.*`
  - DLX: `dlx.events` → `gamification.achievement.failed`

- `gamification.level-up` - Level-up events
  - Routing key: `gamification.levelup.*`
  - DLX: `dlx.events` → `gamification.levelup.failed`

#### Notification Service
- `notification.email` - Email notifications
  - Routing key: `notification.email.*`
  - TTL: 10 minutes
  - DLX: `dlx.events` → `notification.email.failed`

- `notification.push` - Push notifications
  - Routing key: `notification.push.*`
  - TTL: 10 minutes
  - DLX: `dlx.events` → `notification.push.failed`

- `notification.in-app` - In-app notifications
  - Routing key: `notification.inapp.*`
  - DLX: `dlx.events` → `notification.inapp.failed`

#### Analytics Service
- `analytics.telemetry` - Telemetry and metrics
  - Routing key: `analytics.#` (all analytics events)
  - TTL: 1 hour
  - DLX: `dlx.events` → `analytics.telemetry.failed`

#### AI Tutor Service
- `ai-tutor.requests` - AI tutor requests
  - Routing key: `ai.request.*`
  - TTL: 5 minutes
  - Max length: 5,000 messages
  - DLX: `dlx.events` → `ai-tutor.failed`

### Dead Letter Queues

All failed messages are routed to service-specific dead letter queues:

- `dlq.code-executor` - Failed code execution messages
- `dlq.sql-executor` - Failed SQL execution messages
- `dlq.gamification` - Failed gamification messages
- `dlq.notification` - Failed notification messages
- `dlq.analytics` - Failed analytics messages
- `dlq.ai-tutor` - Failed AI tutor messages

## Deployment

### Prerequisites

1. Kubernetes cluster with at least 3 nodes
2. StorageClass configured for PersistentVolumeClaims
3. Platform secrets created with RabbitMQ credentials

### Create Secrets

```bash
# Generate a random Erlang cookie for cluster formation
ERLANG_COOKIE=$(openssl rand -base64 32)

# Create or update secrets
kubectl create secret generic platform-secrets \
  --from-literal=RABBITMQ_USER=platform_user \
  --from-literal=RABBITMQ_PASSWORD=YourSecurePassword123 \
  --from-literal=RABBITMQ_ERLANG_COOKIE=$ERLANG_COOKIE \
  --namespace=aspnet-learning-platform \
  --dry-run=client -o yaml | kubectl apply -f -
```

### Deploy RabbitMQ Cluster

```bash
# Apply RabbitMQ deployment
kubectl apply -f k8s/rabbitmq-deployment.yaml

# Apply topology configuration
kubectl apply -f k8s/rabbitmq-topology.yaml

# Wait for RabbitMQ pods to be ready
kubectl wait --for=condition=ready pod -l app=rabbitmq \
  --namespace=aspnet-learning-platform \
  --timeout=300s

# Initialize topology (exchanges, queues, bindings)
kubectl apply -f k8s/rabbitmq-init-job.yaml

# Check initialization job status
kubectl logs -f job/rabbitmq-init --namespace=aspnet-learning-platform
```

### Verify Deployment

```bash
# Check RabbitMQ pods
kubectl get pods -l app=rabbitmq --namespace=aspnet-learning-platform

# Check RabbitMQ cluster status
kubectl exec -it rabbitmq-0 --namespace=aspnet-learning-platform -- \
  rabbitmq-diagnostics cluster_status

# Port-forward to access management UI
kubectl port-forward svc/rabbitmq-service 15672:15672 \
  --namespace=aspnet-learning-platform

# Access management UI at http://localhost:15672
# Login with credentials from secrets
```

## Application Configuration

### appsettings.json

```json
{
  "RabbitMQ": {
    "HostName": "rabbitmq-service",
    "Port": 5672,
    "UserName": "platform_user",
    "Password": "YourSecurePassword123",
    "VirtualHost": "/",
    "MaxRetryAttempts": 5,
    "RetryDelayMs": 1000,
    "ConnectionTimeoutSeconds": 30,
    "RequestedHeartbeat": 60
  }
}
```

### Service Registration

```csharp
// In Program.cs or Startup.cs
using Shared.Extensions;

// Add RabbitMQ services with automatic topology initialization
builder.Services.AddRabbitMQ(builder.Configuration, initializeTopology: true);

// Or with custom options
builder.Services.AddRabbitMQ(options =>
{
    options.HostName = "rabbitmq-service";
    options.Port = 5672;
    options.UserName = "platform_user";
    options.Password = Environment.GetEnvironmentVariable("RABBITMQ_PASSWORD");
}, initializeTopology: true);
```

### Publishing Events

```csharp
using Shared.Interfaces;
using Shared.Models;

public class ChallengeService
{
    private readonly IEventPublisher _eventPublisher;

    public ChallengeService(IEventPublisher eventPublisher)
    {
        _eventPublisher = eventPublisher;
    }

    public async Task CompleteChallengeAsync(string userId, string challengeId)
    {
        // ... business logic ...

        // Publish event
        var @event = new ChallengeCompletedEvent
        {
            EventId = Guid.NewGuid(),
            EventType = "challenge.completed",
            OccurredAt = DateTime.UtcNow,
            UserId = userId,
            ChallengeId = challengeId,
            Difficulty = "Medium",
            XPAwarded = 25
        };

        await _eventPublisher.PublishAsync(@event);
    }
}
```

### Consuming Messages

```csharp
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Shared.Interfaces;

public class GamificationConsumer : BackgroundService
{
    private readonly IRabbitMQConnectionManager _connectionManager;
    private readonly ILogger<GamificationConsumer> _logger;
    private IModel? _channel;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _channel = _connectionManager.CreateChannel();
        
        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.Received += async (sender, ea) =>
        {
            try
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                
                // Process message
                await ProcessMessageAsync(message);
                
                // Acknowledge message
                _channel.BasicAck(ea.DeliveryTag, false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing message");
                
                // Reject and requeue (or send to DLQ if max retries exceeded)
                _channel.BasicNack(ea.DeliveryTag, false, false);
            }
        };

        _channel.BasicConsume(
            queue: "gamification.xp-awarded",
            autoAck: false,
            consumer: consumer);

        await Task.Delay(Timeout.Infinite, stoppingToken);
    }
}
```

## Monitoring

### Health Checks

RabbitMQ health checks are configured in the deployment:

- **Liveness Probe**: `rabbitmq-diagnostics -q ping`
  - Initial delay: 60 seconds
  - Period: 30 seconds

- **Readiness Probe**: `rabbitmq-diagnostics -q check_running`
  - Initial delay: 20 seconds
  - Period: 10 seconds

### Management UI

Access the RabbitMQ Management UI:

```bash
kubectl port-forward svc/rabbitmq-service 15672:15672 \
  --namespace=aspnet-learning-platform
```

Navigate to http://localhost:15672 and login with your credentials.

### Metrics

Monitor key metrics:

- Queue length (alert if > 1000 messages)
- Message rate (messages/second)
- Consumer count per queue
- Memory usage per node
- Disk space usage
- Connection count

### Alerts

Configure alerts for:

- Queue length exceeds threshold
- Dead letter queue has messages
- Node memory usage > 80%
- Node disk usage > 80%
- Cluster node down
- High message rejection rate

## High Availability

### Cluster Configuration

- **3 replicas** for high availability
- **Automatic peer discovery** using Kubernetes plugin
- **Queue mirroring** across all nodes (ha-mode: all)
- **Automatic synchronization** of mirrored queues
- **Partition handling**: autoheal mode

### Failure Scenarios

1. **Single node failure**: Cluster continues operating with 2 nodes
2. **Network partition**: Automatic healing when partition resolves
3. **Pod restart**: Automatic rejoin to cluster
4. **Message loss prevention**: Persistent messages and durable queues

### Backup and Recovery

```bash
# Export definitions (exchanges, queues, bindings)
kubectl exec -it rabbitmq-0 --namespace=aspnet-learning-platform -- \
  rabbitmqadmin export /tmp/definitions.json

kubectl cp aspnet-learning-platform/rabbitmq-0:/tmp/definitions.json \
  ./rabbitmq-definitions-backup.json

# Import definitions
kubectl cp ./rabbitmq-definitions-backup.json \
  aspnet-learning-platform/rabbitmq-0:/tmp/definitions.json

kubectl exec -it rabbitmq-0 --namespace=aspnet-learning-platform -- \
  rabbitmqadmin import /tmp/definitions.json
```

## Scaling

### Horizontal Scaling

```bash
# Scale to 5 nodes
kubectl scale statefulset rabbitmq --replicas=5 \
  --namespace=aspnet-learning-platform

# Scale down to 3 nodes
kubectl scale statefulset rabbitmq --replicas=3 \
  --namespace=aspnet-learning-platform
```

### Resource Limits

Current configuration:
- **Requests**: 512Mi memory, 500m CPU
- **Limits**: 2Gi memory, 2000m CPU

Adjust based on load:

```yaml
resources:
  requests:
    memory: "1Gi"
    cpu: "1000m"
  limits:
    memory: "4Gi"
    cpu: "4000m"
```

## Troubleshooting

### Check Cluster Status

```bash
kubectl exec -it rabbitmq-0 --namespace=aspnet-learning-platform -- \
  rabbitmq-diagnostics cluster_status
```

### Check Queue Status

```bash
kubectl exec -it rabbitmq-0 --namespace=aspnet-learning-platform -- \
  rabbitmqctl list_queues name messages consumers
```

### View Logs

```bash
# View logs for specific pod
kubectl logs rabbitmq-0 --namespace=aspnet-learning-platform

# Follow logs
kubectl logs -f rabbitmq-0 --namespace=aspnet-learning-platform

# View logs for all pods
kubectl logs -l app=rabbitmq --namespace=aspnet-learning-platform
```

### Common Issues

1. **Pods not starting**: Check secrets are created correctly
2. **Cluster not forming**: Verify Erlang cookie is the same across all nodes
3. **Connection refused**: Ensure service is created and pods are ready
4. **High memory usage**: Increase resource limits or scale horizontally
5. **Messages not routing**: Verify exchange and queue bindings

## Security

### Authentication

- Username/password authentication via secrets
- Separate credentials per environment (dev, staging, production)

### Network Policies

RabbitMQ is only accessible within the cluster:

```yaml
apiVersion: networking.k8s.io/v1
kind: NetworkPolicy
metadata:
  name: rabbitmq-network-policy
spec:
  podSelector:
    matchLabels:
      app: rabbitmq
  policyTypes:
  - Ingress
  ingress:
  - from:
    - podSelector: {}
    ports:
    - protocol: TCP
      port: 5672
    - protocol: TCP
      port: 15672
```

### TLS/SSL

For production, enable TLS:

```yaml
env:
- name: RABBITMQ_SSL_CERTFILE
  value: /etc/rabbitmq/certs/tls.crt
- name: RABBITMQ_SSL_KEYFILE
  value: /etc/rabbitmq/certs/tls.key
- name: RABBITMQ_SSL_CACERTFILE
  value: /etc/rabbitmq/certs/ca.crt
```

## Performance Tuning

### Connection Pooling

Use connection pooling in application code to reduce connection overhead.

### Prefetch Count

Set appropriate prefetch count for consumers:

```csharp
channel.BasicQos(prefetchSize: 0, prefetchCount: 10, global: false);
```

### Message Batching

Batch messages when possible to reduce network overhead.

### Queue Length Monitoring

Monitor queue lengths and scale consumers accordingly.

## References

- [RabbitMQ Documentation](https://www.rabbitmq.com/documentation.html)
- [RabbitMQ Kubernetes Operator](https://www.rabbitmq.com/kubernetes/operator/operator-overview.html)
- [RabbitMQ Clustering Guide](https://www.rabbitmq.com/clustering.html)
- [RabbitMQ High Availability](https://www.rabbitmq.com/ha.html)
