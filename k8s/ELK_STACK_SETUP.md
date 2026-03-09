# ELK Stack Setup Guide

## Overview

This guide covers the deployment and configuration of the ELK (Elasticsearch, Logstash, Kibana) stack for centralized log aggregation across all microservices in the Platform Evolution SaaS project.

**Requirements Addressed:**
- Requirement 52.9: Centralized logging using ELK stack
- Requirement 52.13: Log retention for 90 days

## Architecture

The ELK stack consists of three main components:

### 1. Elasticsearch Cluster
- **Purpose**: Distributed search and analytics engine for storing and indexing logs
- **Configuration**: 3-node cluster for high availability
- **Storage**: 50GB per node with persistent volumes
- **Resources**: 3GB RAM, 1-2 CPU cores per node
- **Retention**: 90-day automatic deletion via Index Lifecycle Management (ILM)

### 2. Logstash
- **Purpose**: Log pipeline for ingesting, transforming, and forwarding logs
- **Configuration**: 2 replicas with auto-scaling (2-5 pods)
- **Inputs**: TCP (port 5000), HTTP (port 8080), Beats (port 5044)
- **Resources**: 1.5GB RAM, 500m-1000m CPU per pod
- **Features**: Structured logging, correlation ID tracking, exception parsing

### 3. Kibana
- **Purpose**: Visualization and exploration interface for logs
- **Configuration**: Single replica
- **Resources**: 1GB RAM, 500m-1000m CPU
- **Features**: Pre-configured dashboards, index patterns, saved searches

## Deployment

### Prerequisites

1. Kubernetes cluster with kubectl configured
2. Namespace `platform-saas-prod` created
3. Service account `monitoring-sa` with appropriate RBAC permissions
4. Sufficient cluster resources (minimum 12GB RAM, 6 CPU cores)

### Quick Start

**Linux/macOS:**
```bash
cd k8s
chmod +x deploy-elk-stack.sh
./deploy-elk-stack.sh
```

**Windows (PowerShell):**
```powershell
cd k8s
.\deploy-elk-stack.ps1
```

### Manual Deployment

If you prefer to deploy components individually:

```bash
# 1. Deploy Elasticsearch cluster
kubectl apply -f elasticsearch-cluster.yaml

# 2. Wait for Elasticsearch to be ready
kubectl wait --for=condition=ready pod -l app=elasticsearch -n platform-saas-prod --timeout=300s

# 3. Deploy Logstash
kubectl apply -f logstash-deployment.yaml

# 4. Deploy Kibana
kubectl apply -f kibana-deployment.yaml
```

## Configuration

### Elasticsearch Configuration

**Index Lifecycle Management (ILM):**
- **Hot phase**: Rollover at 50GB or 1 day
- **Warm phase**: After 7 days, shrink to 1 shard and force merge
- **Delete phase**: After 90 days, automatically delete indices

**Index Template:**
- Pattern: `logs-*`
- Shards: 3 primary, 1 replica
- Mappings: Optimized for structured logs with correlation IDs

### Logstash Configuration

**Input Plugins:**
- **TCP (port 5000)**: For structured JSON logs from microservices
- **HTTP (port 8080)**: For REST-based log ingestion
- **Beats (port 5044)**: For Filebeat integration (optional)

**Filter Pipeline:**
- Parse timestamps to `@timestamp` field
- Extract correlation IDs for distributed tracing
- Normalize log levels to uppercase
- Parse exception stack traces
- Add environment tags

**Output:**
- Elasticsearch with daily indices per service: `logs-{service}-{YYYY.MM.dd}`

### Kibana Configuration

**Pre-configured Components:**
- Index pattern: `logs-*` with `@timestamp` as time field
- Saved search: "Error Logs" - filters for ERROR level logs
- Visualization: "Log Levels Distribution" - pie chart of log levels
- Visualization: "Logs by Service" - histogram of logs over time by service
- Dashboard: "Platform Logs Dashboard" - main overview dashboard

## Accessing the ELK Stack

### Port Forwarding (Development)

**Kibana:**
```bash
kubectl port-forward -n platform-saas-prod svc/kibana 5601:5601
```
Then open: http://localhost:5601

**Elasticsearch:**
```bash
kubectl port-forward -n platform-saas-prod svc/elasticsearch 9200:9200
```
Then open: http://localhost:9200

### Production Access (Ingress)

For production, Kibana is exposed via Ingress at:
- URL: `https://kibana.platform-saas.example.com`
- Update the hostname in `kibana-deployment.yaml` before deploying

## Integrating Microservices

### Serilog Configuration (C#)

All microservices should log to Logstash using Serilog:

```csharp
// Install: Serilog.Sinks.Network
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .Enrich.WithProperty("service", "auth-service")
    .Enrich.WithProperty("environment", "production")
    .Enrich.FromLogContext()
    .WriteTo.TCPSink(
        "logstash.platform-saas-prod.svc.cluster.local",
        5000,
        new JsonFormatter())
    .CreateLogger();
```

### Structured Logging Example

```csharp
Log.Information("User {UserId} logged in successfully", userId);
Log.Error(ex, "Failed to execute code for submission {SubmissionId}", submissionId);

// With correlation ID
using (LogContext.PushProperty("CorrelationId", correlationId))
{
    Log.Information("Processing request");
}
```

### Log Format

All logs should include:
- `@timestamp`: ISO8601 timestamp
- `level`: Log level (INFO, WARN, ERROR, etc.)
- `message`: Log message
- `service`: Service name
- `correlationId`: Request correlation ID for distributed tracing
- `userId`: User ID (if applicable)
- `exception`: Exception details (if applicable)

## Monitoring and Maintenance

### Health Checks

**Elasticsearch Cluster Health:**
```bash
kubectl exec -n platform-saas-prod elasticsearch-0 -- \
  curl -s http://localhost:9200/_cluster/health?pretty
```

**Check Indices:**
```bash
kubectl exec -n platform-saas-prod elasticsearch-0 -- \
  curl -s http://localhost:9200/_cat/indices?v
```

**Logstash Status:**
```bash
kubectl exec -n platform-saas-prod <logstash-pod> -- \
  curl -s http://localhost:9600/?pretty
```

### Viewing Logs

```bash
# Elasticsearch logs
kubectl logs -n platform-saas-prod -l app=elasticsearch --tail=100

# Logstash logs
kubectl logs -n platform-saas-prod -l app=logstash --tail=100

# Kibana logs
kubectl logs -n platform-saas-prod -l app=kibana --tail=100
```

### Scaling

**Logstash Auto-scaling:**
- Configured via HorizontalPodAutoscaler
- Min replicas: 2
- Max replicas: 5
- Triggers: CPU > 70%, Memory > 80%

**Manual Scaling:**
```bash
kubectl scale deployment logstash -n platform-saas-prod --replicas=3
```

## Troubleshooting

### Elasticsearch Pods Not Starting

**Issue**: Pods stuck in Pending or CrashLoopBackOff

**Solutions:**
1. Check if persistent volumes are available:
   ```bash
   kubectl get pv
   ```

2. Verify resource availability:
   ```bash
   kubectl describe pod -n platform-saas-prod elasticsearch-0
   ```

3. Check vm.max_map_count on nodes:
   ```bash
   # Should be at least 262144
   sysctl vm.max_map_count
   ```

### Logstash Connection Issues

**Issue**: Microservices cannot connect to Logstash

**Solutions:**
1. Verify Logstash service is running:
   ```bash
   kubectl get svc -n platform-saas-prod logstash
   ```

2. Test connectivity from a pod:
   ```bash
   kubectl run -it --rm debug --image=busybox --restart=Never -- \
     telnet logstash.platform-saas-prod.svc.cluster.local 5000
   ```

3. Check Logstash logs for errors:
   ```bash
   kubectl logs -n platform-saas-prod -l app=logstash
   ```

### Kibana Not Loading

**Issue**: Kibana UI not accessible or showing errors

**Solutions:**
1. Check if Elasticsearch is healthy:
   ```bash
   kubectl exec -n platform-saas-prod elasticsearch-0 -- \
     curl http://localhost:9200/_cluster/health
   ```

2. Restart Kibana:
   ```bash
   kubectl rollout restart deployment kibana -n platform-saas-prod
   ```

3. Check Kibana logs:
   ```bash
   kubectl logs -n platform-saas-prod -l app=kibana
   ```

### Disk Space Issues

**Issue**: Elasticsearch running out of disk space

**Solutions:**
1. Check disk usage:
   ```bash
   kubectl exec -n platform-saas-prod elasticsearch-0 -- df -h
   ```

2. Manually delete old indices:
   ```bash
   kubectl exec -n platform-saas-prod elasticsearch-0 -- \
     curl -X DELETE http://localhost:9200/logs-*-2024.01.*
   ```

3. Verify ILM policy is working:
   ```bash
   kubectl exec -n platform-saas-prod elasticsearch-0 -- \
     curl http://localhost:9200/_ilm/policy/logs-90day-retention?pretty
   ```

## Performance Tuning

### Elasticsearch Optimization

**Heap Size:**
- Set to 50% of available RAM (max 32GB)
- Configured via `ES_JAVA_OPTS: "-Xms2g -Xmx2g"`

**Shard Strategy:**
- Keep shard size between 10-50GB
- Use 3 primary shards for daily indices
- 1 replica for redundancy

**Index Settings:**
```json
{
  "index.refresh_interval": "30s",
  "index.number_of_replicas": 1,
  "index.translog.durability": "async"
}
```

### Logstash Optimization

**Pipeline Workers:**
- Default: Number of CPU cores
- Adjust via `pipeline.workers` in logstash.yml

**Batch Size:**
- Increase for higher throughput
- Configure via `pipeline.batch.size` (default: 125)

## Security Considerations

**Current Setup:**
- X-Pack security disabled for simplicity
- Internal cluster communication only

**Production Recommendations:**
1. Enable X-Pack security with authentication
2. Configure TLS for Elasticsearch cluster communication
3. Use Kubernetes Network Policies to restrict access
4. Implement role-based access control (RBAC) in Kibana
5. Enable audit logging

## Backup and Recovery

### Elasticsearch Snapshots

Configure snapshot repository for backups:

```bash
# Create snapshot repository
kubectl exec -n platform-saas-prod elasticsearch-0 -- \
  curl -X PUT "http://localhost:9200/_snapshot/backup_repo" \
  -H 'Content-Type: application/json' \
  -d '{
    "type": "fs",
    "settings": {
      "location": "/usr/share/elasticsearch/backup"
    }
  }'

# Create snapshot
kubectl exec -n platform-saas-prod elasticsearch-0 -- \
  curl -X PUT "http://localhost:9200/_snapshot/backup_repo/snapshot_1?wait_for_completion=true"
```

## Next Steps

1. Configure all microservices to send logs to Logstash
2. Create custom Kibana dashboards for specific use cases
3. Set up alerts for critical log patterns (errors, exceptions)
4. Integrate with Application Insights for correlation
5. Configure log sampling for high-volume services

## References

- [Elasticsearch Documentation](https://www.elastic.co/guide/en/elasticsearch/reference/current/index.html)
- [Logstash Documentation](https://www.elastic.co/guide/en/logstash/current/index.html)
- [Kibana Documentation](https://www.elastic.co/guide/en/kibana/current/index.html)
- [Serilog.Sinks.Network](https://github.com/serilog/serilog-sinks-network)
