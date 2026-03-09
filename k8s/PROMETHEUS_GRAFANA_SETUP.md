# Prometheus and Grafana Setup Guide

## Overview

This guide covers the deployment and configuration of Prometheus and Grafana for comprehensive monitoring and observability of the Platform Evolution SaaS application.

**Requirements Addressed:**
- Requirement 52.1: Health check endpoints for all microservices
- Requirement 52.2: CPU usage monitoring with 80% threshold alerts
- Requirement 52.3: Memory usage monitoring with 80% threshold alerts
- Requirement 52.4: Disk usage monitoring with 80% threshold alerts
- Requirement 52.5: API error rate monitoring with 5% threshold alerts
- Requirement 52.6: API response time monitoring (p95 < 2s)
- Requirement 52.7: Database connection pool monitoring (90% threshold)
- Requirement 52.8: Message queue length monitoring (1000 messages threshold)
- Requirement 52.11: Dashboards showing key metrics for each microservice
- Requirement 52.12: Automated alerting via email and Slack

## Architecture

### Components

1. **Prometheus** (v2.48.0)
   - Metrics collection and storage
   - Service discovery for Kubernetes pods
   - Alert rule evaluation
   - 30-day data retention
   - 50GB persistent storage

2. **AlertManager** (v0.26.0)
   - Alert routing and grouping
   - Email and Slack notifications
   - Alert inhibition rules
   - 10GB persistent storage

3. **Grafana** (v10.2.2)
   - Visualization dashboards
   - Pre-configured datasources
   - Alert management
   - 10GB persistent storage

### Data Flow

```
Microservices → Prometheus → AlertManager → Email/Slack
                    ↓
                 Grafana (Visualization)
```

## Deployment

### Prerequisites

- Kubernetes cluster (v1.24+)
- kubectl configured
- Namespace: `platform-saas`
- Storage class: `standard` (or modify in YAML files)

### Quick Start

**PowerShell (Windows):**
```powershell
cd k8s
./deploy-prometheus-grafana.ps1
```

**Bash (Linux/Mac):**
```bash
cd k8s
chmod +x deploy-prometheus-grafana.sh
./deploy-prometheus-grafana.sh
```

### Manual Deployment

```bash
# Deploy Prometheus
kubectl apply -f prometheus-config.yaml
kubectl apply -f prometheus-deployment.yaml

# Deploy AlertManager
kubectl apply -f alertmanager-config.yaml
kubectl apply -f alertmanager-deployment.yaml

# Deploy Grafana
kubectl apply -f grafana-config.yaml
kubectl apply -f grafana-deployment.yaml

# Verify deployments
kubectl get pods -n platform-saas -l component=monitoring
```

## Configuration

### 1. Prometheus Configuration

**Scrape Interval:** 15 seconds  
**Evaluation Interval:** 15 seconds  
**Retention:** 30 days

**Service Discovery:**
- Kubernetes API servers
- Kubernetes nodes
- Kubernetes pods (with annotations)
- All platform microservices

**Scrape Targets:**
- API Gateway
- Code Executor Service
- Auth Service
- Challenge Service
- Course Service
- Progress Service
- AI Tutor Service
- Redis
- RabbitMQ

### 2. Alert Rules

All alert rules are defined in `prometheus-config.yaml` under `alert-rules.yml`:

| Alert Name | Condition | Threshold | Duration | Severity |
|------------|-----------|-----------|----------|----------|
| HighCPUUsage | CPU usage | > 80% | 5 min | warning |
| HighMemoryUsage | Memory usage | > 80% | 5 min | warning |
| HighDiskUsage | Disk usage | > 80% | 5 min | warning |
| HighAPIErrorRate | API error rate | > 5% | 5 min | critical |
| HighAPIResponseTime | p95 response time | > 2s | 5 min | warning |
| HighDatabaseConnectionPoolUtilization | Connection pool | > 90% | 5 min | critical |
| HighMessageQueueLength | Queue length | > 1000 | 5 min | warning |
| PodRestartingFrequently | Pod restarts | > 5/hour | 5 min | warning |
| ServiceDown | Service availability | down | 2 min | critical |
| ContainerMemoryUsageHigh | Container memory | > 80% | 5 min | warning |

### 3. AlertManager Configuration

**Notification Channels:**
- Email (SMTP)
- Slack webhooks

**Alert Routing:**
- Critical alerts → Email + Slack
- Warning alerts → Slack only
- Component-specific routing (infrastructure, api, database)

**Inhibition Rules:**
- Warning alerts inhibited when critical alerts fire
- Service down alerts inhibited when pod is restarting

### 4. Grafana Dashboards

Five pre-configured dashboards are automatically provisioned:

#### System Overview Dashboard
- CPU usage by instance
- Memory usage by instance
- Disk usage by instance and mount point
- Network traffic (RX/TX)

#### API Metrics Dashboard
- Request rate by service
- Error rate percentage
- Response time (p95) by service
- Request count by status code

#### Database Metrics Dashboard
- Connection pool utilization
- Query performance (p95, p99)
- Active vs idle connections
- Query rate

#### Message Queue Metrics Dashboard
- Queue length by queue
- Message processing rate (published vs delivered)
- Consumer count
- Message acknowledgement rate

#### Microservices Overview Dashboard
- Pod status (running count)
- Pod restarts (last hour)
- Container CPU usage by pod
- Container memory usage by pod

## Access

### Port Forwarding

**Prometheus:**
```bash
kubectl port-forward -n platform-saas svc/prometheus 9090:9090
# Access: http://localhost:9090
```

**AlertManager:**
```bash
kubectl port-forward -n platform-saas svc/alertmanager 9093:9093
# Access: http://localhost:9093
```

**Grafana:**
```bash
kubectl port-forward -n platform-saas svc/grafana 3000:3000
# Access: http://localhost:3000
# Default credentials: admin / (see grafana-secrets)
```

### Ingress (Production)

For production deployments, configure ingress rules:

```yaml
apiVersion: networking.k8s.io/v1
kind: Ingress
metadata:
  name: monitoring-ingress
  namespace: platform-saas
spec:
  rules:
    - host: prometheus.platform-saas.com
      http:
        paths:
          - path: /
            pathType: Prefix
            backend:
              service:
                name: prometheus
                port:
                  number: 9090
    - host: grafana.platform-saas.com
      http:
        paths:
          - path: /
            pathType: Prefix
            backend:
              service:
                name: grafana
                port:
                  number: 3000
```

## Microservice Integration

### Adding Metrics to Microservices

#### 1. Add Prometheus Annotations to Pods

Edit your microservice deployment YAML:

```yaml
apiVersion: apps/v1
kind: Deployment
metadata:
  name: your-service
spec:
  template:
    metadata:
      annotations:
        prometheus.io/scrape: "true"
        prometheus.io/port: "8080"
        prometheus.io/path: "/metrics"
```

#### 2. Implement Metrics Endpoint in ASP.NET Core

**Install NuGet packages:**
```bash
dotnet add package prometheus-net.AspNetCore
```

**Configure in Program.cs:**
```csharp
using Prometheus;

var builder = WebApplication.CreateBuilder(args);

// ... other configurations

var app = builder.Build();

// Enable metrics endpoint
app.UseMetricServer();  // Exposes /metrics endpoint
app.UseHttpMetrics();   // Tracks HTTP request metrics

app.Run();
```

#### 3. Custom Metrics Example

```csharp
using Prometheus;

public class CodeExecutorService
{
    private static readonly Counter ExecutionCount = Metrics
        .CreateCounter("code_executions_total", "Total number of code executions");
    
    private static readonly Histogram ExecutionDuration = Metrics
        .CreateHistogram("code_execution_duration_seconds", "Code execution duration");
    
    private static readonly Gauge ActiveContainers = Metrics
        .CreateGauge("active_containers", "Number of active Docker containers");
    
    public async Task<ExecutionResult> ExecuteAsync(string code)
    {
        ExecutionCount.Inc();
        ActiveContainers.Inc();
        
        using (ExecutionDuration.NewTimer())
        {
            try
            {
                // Execute code
                var result = await DoExecuteAsync(code);
                return result;
            }
            finally
            {
                ActiveContainers.Dec();
            }
        }
    }
}
```

### Standard Metrics to Implement

Each microservice should expose:

1. **HTTP Metrics** (automatic with `UseHttpMetrics()`)
   - `http_requests_total` - Total HTTP requests
   - `http_request_duration_seconds` - Request duration histogram

2. **Custom Business Metrics**
   - Operation counters (e.g., `code_executions_total`)
   - Duration histograms (e.g., `compilation_duration_seconds`)
   - Current state gauges (e.g., `active_sessions`)

3. **Database Metrics**
   - `database_connections_active` - Active connections
   - `database_connections_max` - Max connections
   - `database_query_duration_seconds` - Query duration

4. **Cache Metrics**
   - `cache_hits_total` - Cache hits
   - `cache_misses_total` - Cache misses
   - `cache_size_bytes` - Cache size

## Security Configuration

### 1. Update AlertManager Secrets

Edit `k8s/alertmanager-config.yaml`:

```yaml
apiVersion: v1
kind: Secret
metadata:
  name: alertmanager-secrets
stringData:
  smtp-password: "your-actual-smtp-password"
  slack-webhook-url: "https://hooks.slack.com/services/YOUR/WEBHOOK/URL"
```

Apply changes:
```bash
kubectl apply -f alertmanager-config.yaml
kubectl rollout restart deployment/alertmanager -n platform-saas
```

### 2. Update Grafana Admin Password

Edit `k8s/grafana-deployment.yaml`:

```yaml
apiVersion: v1
kind: Secret
metadata:
  name: grafana-secrets
stringData:
  admin-user: admin
  admin-password: "your-secure-password-here"
```

Apply changes:
```bash
kubectl apply -f grafana-deployment.yaml
kubectl rollout restart deployment/grafana -n platform-saas
```

### 3. Configure SMTP for Grafana

Edit environment variables in `grafana-deployment.yaml`:

```yaml
- name: GF_SMTP_ENABLED
  value: "true"
- name: GF_SMTP_HOST
  value: "smtp.gmail.com:587"
- name: GF_SMTP_USER
  value: "your-email@domain.com"
- name: GF_SMTP_PASSWORD
  value: "your-smtp-password"
```

## Monitoring Best Practices

### 1. Alert Fatigue Prevention

- Set appropriate thresholds (not too sensitive)
- Use alert grouping and inhibition rules
- Implement escalation policies
- Regular review and tuning of alerts

### 2. Dashboard Organization

- Create role-specific dashboards (DevOps, Developers, Business)
- Use consistent naming conventions
- Add documentation panels to dashboards
- Set appropriate time ranges and refresh intervals

### 3. Metric Naming Conventions

Follow Prometheus naming best practices:
- Use base unit (seconds, bytes, not milliseconds or megabytes)
- Suffix with unit (`_seconds`, `_bytes`, `_total`)
- Use snake_case
- Include namespace prefix (`platform_`)

Example:
```
platform_code_execution_duration_seconds
platform_api_requests_total
platform_database_connections_active
```

### 4. Data Retention

- Prometheus: 30 days (configurable via `--storage.tsdb.retention.time`)
- For longer retention, consider:
  - Thanos for long-term storage
  - Cortex for multi-tenant metrics
  - VictoriaMetrics for high-performance storage

## Troubleshooting

### Prometheus Not Scraping Targets

**Check target status:**
```bash
# Port forward Prometheus
kubectl port-forward -n platform-saas svc/prometheus 9090:9090

# Open http://localhost:9090/targets
# Look for targets in "DOWN" state
```

**Common issues:**
1. Missing annotations on pods
2. Incorrect port in annotation
3. Metrics endpoint not implemented
4. Network policies blocking access

**Fix:**
```bash
# Check pod annotations
kubectl get pod <pod-name> -n platform-saas -o yaml | grep prometheus

# Check if metrics endpoint is accessible
kubectl exec -it <pod-name> -n platform-saas -- curl localhost:8080/metrics
```

### Alerts Not Firing

**Check alert rules:**
```bash
# Port forward Prometheus
kubectl port-forward -n platform-saas svc/prometheus 9090:9090

# Open http://localhost:9090/alerts
# Check if alerts are in "Pending" or "Firing" state
```

**Check AlertManager:**
```bash
# Port forward AlertManager
kubectl port-forward -n platform-saas svc/alertmanager 9093:9093

# Open http://localhost:9093
# Check if alerts are being received
```

### Grafana Dashboards Not Loading

**Check datasource connection:**
1. Login to Grafana
2. Go to Configuration → Data Sources
3. Click on Prometheus datasource
4. Click "Test" button

**Check logs:**
```bash
kubectl logs -n platform-saas deployment/grafana
```

### High Memory Usage

**Prometheus memory usage:**
- Depends on number of time series
- Typical: 1-3 bytes per sample
- Reduce retention time if needed
- Increase memory limits in deployment

```yaml
resources:
  limits:
    memory: 8Gi  # Increase if needed
```

## Maintenance

### Backup

**Prometheus data:**
```bash
# Create snapshot
kubectl exec -n platform-saas deployment/prometheus -- \
  curl -XPOST http://localhost:9090/api/v1/admin/tsdb/snapshot

# Copy snapshot
kubectl cp platform-saas/prometheus-pod:/prometheus/snapshots/snapshot-name ./backup/
```

**Grafana dashboards:**
```bash
# Export all dashboards via API
curl -H "Authorization: Bearer <api-key>" \
  http://localhost:3000/api/search?type=dash-db | \
  jq -r '.[] | .uid' | \
  xargs -I {} curl -H "Authorization: Bearer <api-key>" \
  http://localhost:3000/api/dashboards/uid/{} > dashboard-{}.json
```

### Upgrade

```bash
# Update image versions in deployment files
# Then apply changes
kubectl apply -f prometheus-deployment.yaml
kubectl apply -f alertmanager-deployment.yaml
kubectl apply -f grafana-deployment.yaml

# Monitor rollout
kubectl rollout status deployment/prometheus -n platform-saas
kubectl rollout status deployment/alertmanager -n platform-saas
kubectl rollout status deployment/grafana -n platform-saas
```

## Performance Tuning

### Prometheus

**Reduce cardinality:**
- Avoid high-cardinality labels (user IDs, request IDs)
- Use label dropping/relabeling
- Aggregate metrics before scraping

**Optimize queries:**
- Use recording rules for expensive queries
- Limit time range in dashboards
- Use appropriate step intervals

**Example recording rule:**
```yaml
groups:
  - name: aggregations
    interval: 30s
    rules:
      - record: job:http_requests:rate5m
        expr: sum(rate(http_requests_total[5m])) by (job)
```

### Grafana

**Dashboard performance:**
- Limit number of panels per dashboard
- Use appropriate refresh intervals
- Cache query results
- Use query variables for filtering

## Integration with Existing Monitoring

This Prometheus/Grafana stack complements the existing monitoring infrastructure:

- **Application Insights**: Application-level telemetry and distributed tracing
- **ELK Stack**: Centralized logging and log analysis
- **Prometheus/Grafana**: Infrastructure and business metrics

### Unified Monitoring Strategy

1. **Logs** → ELK Stack (Elasticsearch, Logstash, Kibana)
2. **Traces** → Application Insights
3. **Metrics** → Prometheus + Grafana
4. **Alerts** → AlertManager → Email/Slack

## Support

For issues or questions:
- Check Prometheus documentation: https://prometheus.io/docs/
- Check Grafana documentation: https://grafana.com/docs/
- Review alert rules in `prometheus-config.yaml`
- Check pod logs: `kubectl logs -n platform-saas <pod-name>`

## Summary

This Prometheus and Grafana setup provides:

✅ Comprehensive metrics collection from all microservices  
✅ Real-time monitoring dashboards  
✅ Automated alerting for critical thresholds  
✅ Email and Slack notifications  
✅ 30-day metric retention  
✅ Service discovery for dynamic environments  
✅ Pre-configured dashboards for common use cases  
✅ Integration with existing monitoring stack  

The monitoring stack is production-ready and addresses all requirements from Requirement 52 (Monitoring and Observability).
