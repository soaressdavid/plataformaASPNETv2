# Monitoring Quick Start Guide

This guide will help you quickly set up Prometheus and Grafana monitoring for the ASP.NET Core Learning Platform.

## Prerequisites

- Docker and Docker Compose installed
- ASP.NET Learning Platform services running
- Services exposing Prometheus metrics on `/metrics` endpoint

## Step 1: Start Monitoring Stack

From the `monitoring` directory, run:

```bash
docker-compose -f docker-compose.monitoring.yml up -d
```

This will start:
- **Prometheus** on `http://localhost:9090`
- **Grafana** on `http://localhost:3001`

## Step 2: Verify Prometheus

1. Open `http://localhost:9090` in your browser
2. Navigate to **Status** → **Targets**
3. Verify all services are showing as "UP"
4. If services are "DOWN", check:
   - Services are running
   - Services are exposing `/metrics` endpoint
   - Network connectivity between Prometheus and services

## Step 3: Access Grafana

1. Open `http://localhost:3001` in your browser
2. Login with:
   - **Username**: `admin`
   - **Password**: `admin`
3. (Optional) Change the password when prompted

## Step 4: Verify Dashboard

1. Navigate to **Dashboards** → **Browse**
2. Open the **Learning Platform** folder
3. Click on **ASP.NET Learning Platform - System Overview**
4. You should see metrics populating in all panels

## Step 5: Configure Alerts (Optional)

1. Navigate to **Alerting** → **Alert rules**
2. Verify alert rules are loaded
3. Navigate to **Alerting** → **Contact points**
4. Add a notification channel (Slack, Email, etc.)
5. Navigate to **Alerting** → **Notification policies**
6. Configure routing for your alerts

## Troubleshooting

### No Data in Grafana

**Problem**: Dashboard panels show "No data"

**Solutions**:
1. Check Prometheus targets: `http://localhost:9090/targets`
2. Verify services are exposing metrics:
   ```bash
   curl http://localhost:8080/metrics  # API Gateway
   curl http://localhost:8081/metrics  # Auth Service
   # etc.
   ```
3. Check time range in Grafana (top-right corner)
4. Verify Prometheus data source in Grafana: **Configuration** → **Data sources**

### Prometheus Targets Down

**Problem**: Services show as "DOWN" in Prometheus targets

**Solutions**:
1. Check service is running:
   ```bash
   docker ps | grep api-gateway
   ```
2. Check service logs:
   ```bash
   docker logs api-gateway
   ```
3. Verify network connectivity:
   ```bash
   docker network inspect aspnet-learning-platform
   ```
4. Update `prometheus.yml` with correct service names/ports

### Grafana Can't Connect to Prometheus

**Problem**: Grafana shows "Data source error"

**Solutions**:
1. Verify Prometheus is running:
   ```bash
   docker ps | grep prometheus
   ```
2. Check Prometheus URL in Grafana data source (should be `http://prometheus:9090`)
3. Verify both containers are on the same network:
   ```bash
   docker network inspect aspnet-learning-platform
   ```

## Next Steps

1. **Customize Dashboards**: Modify panels to fit your needs
2. **Set Up Alerts**: Configure notification channels for critical alerts
3. **Add Custom Metrics**: Instrument your code with additional metrics
4. **Create New Dashboards**: Build dashboards for specific services or features

## Useful Commands

```bash
# View Prometheus logs
docker logs prometheus

# View Grafana logs
docker logs grafana

# Restart monitoring stack
docker-compose -f docker-compose.monitoring.yml restart

# Stop monitoring stack
docker-compose -f docker-compose.monitoring.yml down

# Stop and remove volumes (WARNING: deletes all metrics data)
docker-compose -f docker-compose.monitoring.yml down -v

# Check Prometheus configuration
docker exec prometheus promtool check config /etc/prometheus/prometheus.yml

# Reload Prometheus configuration (without restart)
curl -X POST http://localhost:9090/-/reload
```

## Metrics Endpoints

Verify each service is exposing metrics:

```bash
# API Gateway
curl http://localhost:8080/metrics

# Auth Service
curl http://localhost:8081/metrics

# Course Service
curl http://localhost:8082/metrics

# Challenge Service
curl http://localhost:8083/metrics

# Progress Service
curl http://localhost:8084/metrics

# AI Tutor Service
curl http://localhost:8085/metrics

# Execution Service
curl http://localhost:8086/metrics

# Workers
curl http://localhost:8087/metrics  # Worker 1
curl http://localhost:8088/metrics  # Worker 2
```

## Default Credentials

- **Grafana**:
  - Username: `admin`
  - Password: `admin`
  - URL: `http://localhost:3001`

- **Prometheus**:
  - No authentication required
  - URL: `http://localhost:9090`

## Security Recommendations

For production deployments:

1. **Change Default Passwords**: Update Grafana admin password
2. **Enable Authentication**: Configure Prometheus authentication
3. **Use HTTPS**: Set up TLS certificates
4. **Restrict Access**: Use firewall rules to limit access
5. **Secure Secrets**: Use Docker secrets or environment variables for sensitive data

## Support

For more detailed information, see:
- [README.md](./README.md) - Comprehensive documentation
- [Prometheus Documentation](https://prometheus.io/docs/)
- [Grafana Documentation](https://grafana.com/docs/)
