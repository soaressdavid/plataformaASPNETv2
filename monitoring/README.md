# Monitoring and Dashboards

This directory contains Grafana dashboard configurations and alert rules for monitoring the ASP.NET Core Learning Platform.

## Overview

The monitoring setup provides comprehensive visibility into:
- **System Health**: API request rates, response times, and error rates
- **Code Execution**: Queue depth, execution times, success/failure rates
- **Worker Performance**: Utilization, job processing rates
- **Service Health**: Database performance, AI review times, challenge submissions
- **Distributed Tracing**: Request flow across services with OpenTelemetry and Jaeger

## Prerequisites

1. **Prometheus** - Metrics collection and storage
2. **Grafana** - Visualization and alerting
3. **Jaeger** - Distributed tracing and request flow visualization
4. **Application Metrics** - Ensure all services are exposing Prometheus metrics on `/metrics` endpoint
5. **OpenTelemetry** - Ensure all services have distributed tracing configured

## Quick Start

### 1. Set Up Prometheus

Add the following to your `prometheus.yml` configuration:

```yaml
global:
  scrape_interval: 15s
  evaluation_interval: 15s

scrape_configs:
  # API Gateway
  - job_name: 'api-gateway'
    static_configs:
      - targets: ['api-gateway:8080']
    metrics_path: '/metrics'

  # Auth Service
  - job_name: 'auth-service'
    static_configs:
      - targets: ['auth-service:8081']
    metrics_path: '/metrics'

  # Course Service
  - job_name: 'course-service'
    static_configs:
      - targets: ['course-service:8082']
    metrics_path: '/metrics'

  # Challenge Service
  - job_name: 'challenge-service'
    static_configs:
      - targets: ['challenge-service:8083']
    metrics_path: '/metrics'

  # Progress Service
  - job_name: 'progress-service'
    static_configs:
      - targets: ['progress-service:8084']
    metrics_path: '/metrics'

  # AI Tutor Service
  - job_name: 'ai-tutor-service'
    static_configs:
      - targets: ['ai-tutor-service:8085']
    metrics_path: '/metrics'

  # Execution Service
  - job_name: 'execution-service'
    static_configs:
      - targets: ['execution-service:8086']
    metrics_path: '/metrics'

  # Workers
  - job_name: 'workers'
    static_configs:
      - targets: ['worker-1:8087', 'worker-2:8088']
    metrics_path: '/metrics'
```

### 2. Deploy with Docker Compose

Start all monitoring services:

```bash
cd monitoring
docker-compose -f docker-compose.monitoring.yml up -d
```

This will start:
- **Prometheus** on http://localhost:9090
- **Grafana** on http://localhost:3001 (admin/admin)
- **Jaeger UI** on http://localhost:16686

Or start services individually:

```bash
# Start only Prometheus
docker-compose -f docker-compose.monitoring.yml up -d prometheus

# Start only Grafana
docker-compose -f docker-compose.monitoring.yml up -d grafana

# Start only Jaeger
docker-compose -f docker-compose.monitoring.yml up -d jaeger
```

```yaml
services:
  prometheus:
    image: prom/prometheus:latest
    container_name: prometheus
    ports:
      - "9090:9090"
    volumes:
      - ./monitoring/prometheus.yml:/etc/prometheus/prometheus.yml
      - prometheus-data:/prometheus
    command:
      - '--config.file=/etc/prometheus/prometheus.yml'
      - '--storage.tsdb.path=/prometheus'
    networks:
      - aspnet-learning-platform

  grafana:
    image: grafana/grafana:latest
    container_name: grafana
    ports:
      - "3001:3000"
    environment:
      - GF_SECURITY_ADMIN_USER=admin
      - GF_SECURITY_ADMIN_PASSWORD=admin
      - GF_USERS_ALLOW_SIGN_UP=false
    volumes:
      - grafana-data:/var/lib/grafana
      - ./monitoring/grafana-dashboards:/etc/grafana/provisioning/dashboards
      - ./monitoring/grafana-alerts:/etc/grafana/provisioning/alerting
      - ./monitoring/datasources.yaml:/etc/grafana/provisioning/datasources/datasources.yaml
    depends_on:
      - prometheus
    networks:
      - aspnet-learning-platform

volumes:
  prometheus-data:
  grafana-data:

networks:
  aspnet-learning-platform:
    driver: bridge
```

### 3. Configure Grafana Data Source

Create `monitoring/datasources.yaml`:

```yaml
apiVersion: 1

datasources:
  - name: Prometheus
    type: prometheus
    access: proxy
    url: http://prometheus:9090
    isDefault: true
    editable: true
```

### 4. Import Dashboards

#### Option A: Automatic Provisioning (Recommended)

Create `monitoring/grafana-dashboards/dashboard-provider.yaml`:

```yaml
apiVersion: 1

providers:
  - name: 'ASP.NET Learning Platform'
    orgId: 1
    folder: 'Learning Platform'
    type: file
    disableDeletion: false
    updateIntervalSeconds: 10
    allowUiUpdates: true
    options:
      path: /etc/grafana/provisioning/dashboards
```

The dashboard will be automatically loaded when Grafana starts.

#### Option B: Manual Import

1. Open Grafana at `http://localhost:3001`
2. Login with username: `admin`, password: `admin`
3. Navigate to **Dashboards** → **Import**
4. Click **Upload JSON file**
5. Select `monitoring/grafana-dashboards/aspnet-learning-platform-overview.json`
6. Select the Prometheus data source
7. Click **Import**

## Dashboard Panels

### System Overview Dashboard

The main dashboard (`aspnet-learning-platform-overview.json`) includes:

#### 1. API Request Rate
- **Metric**: `rate(api_requests_total[5m])`
- **Description**: Requests per second by service, method, and endpoint
- **Use Case**: Monitor traffic patterns and identify high-load endpoints

#### 2. API Response Time (p95)
- **Metric**: `histogram_quantile(0.95, rate(api_request_duration_seconds_bucket[5m]))`
- **Description**: 95th percentile response time
- **Threshold**: Yellow at 0.1s, Red at 0.2s
- **Use Case**: Detect performance degradation

#### 3. Error Rate by Service
- **Metric**: `rate(api_errors_total[5m])`
- **Description**: Errors per second by service and error type
- **Use Case**: Identify failing services and error patterns

#### 4. Execution Queue Depth
- **Metric**: `execution_queue_depth`
- **Description**: Number of jobs waiting in the execution queue
- **Threshold**: Yellow at 10, Red at 50
- **Use Case**: Monitor queue backlog and worker capacity

#### 5. Code Execution Rate by Status
- **Metric**: `rate(code_executions_total[5m])`
- **Description**: Executions per second by status (Completed, Failed, Timeout, etc.)
- **Use Case**: Track execution success rates

#### 6. Code Execution Duration
- **Metric**: `histogram_quantile(0.50|0.95|0.99, rate(code_execution_duration_seconds_bucket[5m]))`
- **Description**: Execution time percentiles (p50, p95, p99)
- **Use Case**: Monitor execution performance

#### 7. Worker Utilization
- **Metric**: `worker_utilization`
- **Description**: Current utilization (0-1) per worker
- **Threshold**: Red < 0.5, Yellow 0.5-0.8, Green > 0.8
- **Use Case**: Identify underutilized or overloaded workers

#### 8. Worker Job Processing Rate
- **Metric**: `rate(worker_jobs_processed_total[5m])`
- **Description**: Jobs processed per second by worker and status
- **Use Case**: Monitor worker throughput

#### 9. Challenge Submission Rate
- **Metric**: `rate(challenge_submissions_total[5m])`
- **Description**: Submissions per second by difficulty and result
- **Use Case**: Track student activity and success rates

#### 10. AI Review Duration
- **Metric**: `histogram_quantile(0.50|0.95|0.99, rate(ai_review_duration_seconds_bucket[5m]))`
- **Description**: AI review time percentiles
- **Use Case**: Monitor AI service performance

#### 11. Database Query Duration (p95)
- **Metric**: `histogram_quantile(0.95, rate(database_query_duration_seconds_bucket[5m]))`
- **Description**: 95th percentile query time by operation and entity
- **Use Case**: Identify slow database operations

#### 12. Database Error Rate
- **Metric**: `rate(database_errors_total[5m])`
- **Description**: Database errors per second by operation and entity
- **Use Case**: Monitor database health

## Alerts

The alert rules in `monitoring/grafana-alerts/alerts.yaml` include:

### Critical Alerts

1. **High API Error Rate**
   - Condition: Error rate > 0.1 errors/sec for 5 minutes
   - Action: Investigate failing service immediately

2. **High Code Execution Failure Rate**
   - Condition: Failure rate > 10% for 5 minutes
   - Action: Check worker logs and container health

3. **High Database Error Rate**
   - Condition: Error rate > 0.05 errors/sec for 5 minutes
   - Action: Check database connectivity and query logs

### Warning Alerts

4. **High Execution Queue Depth**
   - Condition: Queue depth > 50 jobs for 5 minutes
   - Action: Consider scaling up workers

5. **Slow Code Execution**
   - Condition: p95 execution time > 5s for 5 minutes
   - Action: Investigate slow executions and resource limits

6. **Slow API Response Time**
   - Condition: p95 response time > 0.2s for 5 minutes
   - Action: Profile slow endpoints

7. **Slow Database Queries**
   - Condition: p95 query time > 0.1s for 5 minutes
   - Action: Review query plans and add indexes

8. **Slow AI Code Reviews**
   - Condition: p95 review time > 10s for 5 minutes
   - Action: Check Groq API status and rate limits

9. **Worker Overload**
   - Condition: Average utilization > 90% for 5 minutes
   - Action: Scale up worker instances

### Info Alerts

10. **Low Worker Utilization**
    - Condition: Average utilization < 20% for 10 minutes
    - Action: Consider scaling down workers to save resources

## Alert Notification Channels

Configure notification channels in Grafana:

1. Navigate to **Alerting** → **Contact points**
2. Click **New contact point**
3. Choose your notification method:
   - **Email**: Configure SMTP settings
   - **Slack**: Add webhook URL
   - **PagerDuty**: Add integration key
   - **Webhook**: Custom HTTP endpoint

Example Slack configuration:

```yaml
apiVersion: 1

contactPoints:
  - orgId: 1
    name: slack-alerts
    receivers:
      - uid: slack-receiver
        type: slack
        settings:
          url: https://hooks.slack.com/services/YOUR/WEBHOOK/URL
          text: |
            {{ range .Alerts }}
            *Alert:* {{ .Labels.alertname }}
            *Severity:* {{ .Labels.severity }}
            *Component:* {{ .Labels.component }}
            *Description:* {{ .Annotations.description }}
            {{ end }}
```

## Metrics Reference

### API Gateway Metrics

| Metric | Type | Labels | Description |
|--------|------|--------|-------------|
| `api_requests_total` | Counter | service, method, endpoint, status_code | Total API requests |
| `api_request_duration_seconds` | Histogram | service, method, endpoint | Request duration |
| `api_errors_total` | Counter | service, error_type | Total API errors |

### Code Execution Metrics

| Metric | Type | Labels | Description |
|--------|------|--------|-------------|
| `execution_queue_depth` | Gauge | - | Jobs in queue |
| `code_executions_total` | Counter | status | Total executions |
| `code_execution_duration_seconds` | Histogram | status | Execution duration |
| `code_executions_success_total` | Counter | - | Successful executions |
| `code_executions_failure_total` | Counter | failure_type | Failed executions |

### Worker Metrics

| Metric | Type | Labels | Description |
|--------|------|--------|-------------|
| `worker_utilization` | Gauge | worker_id | Worker utilization (0-1) |
| `worker_jobs_processed_total` | Counter | worker_id, status | Jobs processed |
| `worker_job_processing_duration_seconds` | Histogram | worker_id | Job processing time |

### Challenge Service Metrics

| Metric | Type | Labels | Description |
|--------|------|--------|-------------|
| `challenge_submissions_total` | Counter | difficulty, result | Challenge submissions |
| `test_case_execution_duration_seconds` | Histogram | - | Test case execution time |

### AI Tutor Metrics

| Metric | Type | Labels | Description |
|--------|------|--------|-------------|
| `ai_review_requests_total` | Counter | status | AI review requests |
| `ai_review_duration_seconds` | Histogram | - | AI review duration |

### Database Metrics

| Metric | Type | Labels | Description |
|--------|------|--------|-------------|
| `database_queries_total` | Counter | operation, entity | Database queries |
| `database_query_duration_seconds` | Histogram | operation, entity | Query duration |
| `database_errors_total` | Counter | operation, entity | Database errors |

## Troubleshooting

### Dashboard Not Loading

1. Check Prometheus is running: `curl http://localhost:9090/-/healthy`
2. Check Grafana is running: `curl http://localhost:3001/api/health`
3. Verify data source connection in Grafana UI
4. Check Prometheus targets: `http://localhost:9090/targets`

### No Data in Panels

1. Verify services are exposing metrics: `curl http://api-gateway:8080/metrics`
2. Check Prometheus is scraping: `http://localhost:9090/targets`
3. Verify metric names match in queries
4. Check time range in dashboard (default: last 1 hour)

### Alerts Not Firing

1. Check alert rules are loaded: Grafana → Alerting → Alert rules
2. Verify notification channels are configured
3. Check alert evaluation: Grafana → Alerting → Alert rules → View details
4. Review Grafana logs: `docker logs grafana`

## Best Practices

1. **Set Appropriate Thresholds**: Adjust alert thresholds based on your baseline metrics
2. **Use Labels**: Add labels to metrics for better filtering and grouping
3. **Monitor Cardinality**: Avoid high-cardinality labels (e.g., user IDs, request IDs)
4. **Regular Review**: Review dashboards weekly to identify trends
5. **Alert Fatigue**: Tune alerts to reduce false positives
6. **Retention**: Configure Prometheus retention based on storage capacity

## Additional Resources

- [Prometheus Documentation](https://prometheus.io/docs/)
- [Grafana Documentation](https://grafana.com/docs/)
- [PromQL Query Examples](https://prometheus.io/docs/prometheus/latest/querying/examples/)
- [Grafana Dashboard Best Practices](https://grafana.com/docs/grafana/latest/best-practices/)

## Support

For issues or questions:
1. Check the troubleshooting section above
2. Review service logs: `docker logs <service-name>`
3. Check Prometheus metrics: `http://localhost:9090/graph`
4. Verify Grafana configuration: `http://localhost:3001/datasources`
