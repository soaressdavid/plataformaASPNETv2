# Production Infrastructure

This directory contains Kubernetes manifests and provisioning scripts for production deployment.

## Contents

### Kubernetes Manifests

- **cluster-autoscaler.yaml** - Cluster autoscaler configuration for automatic node scaling
- **hpa-all-services.yaml** - Horizontal Pod Autoscalers for all microservices
- **ingress-production.yaml** - Production ingress with load balancer, SSL/TLS, and security headers

### Provisioning Scripts

- **provision-aks.sh** - Automated provisioning script for Azure Kubernetes Service (AKS)
- **provision-eks.sh** - Automated provisioning script for Amazon Elastic Kubernetes Service (EKS)

### Documentation

- **PRODUCTION_INFRASTRUCTURE_GUIDE.md** - Comprehensive guide for provisioning and managing production infrastructure

## Quick Start

### Azure Kubernetes Service (AKS)

```bash
# Set environment variables (optional)
export RESOURCE_GROUP="platform-saas-prod-rg"
export LOCATION="eastus"
export CLUSTER_NAME="platform-saas-prod-aks"

# Run provisioning script
./provision-aks.sh
```

### Amazon Elastic Kubernetes Service (EKS)

```bash
# Set environment variables (optional)
export CLUSTER_NAME="platform-saas-prod-eks"
export REGION="us-east-1"

# Run provisioning script
./provision-eks.sh
```

## Architecture Overview

### Scaling Configuration

The production infrastructure is designed to support **10,000 concurrent users** with the following scaling configuration:

#### Cluster Autoscaler
- **Min Nodes**: 3 (high availability)
- **Max Nodes**: 50 (cost control)
- **Scale Up**: Aggressive (when pods pending)
- **Scale Down**: Conservative (10min delay)

#### Node Pools

| Node Pool | Instance Type | Min | Max | Purpose |
|-----------|---------------|-----|-----|---------|
| General | m5.xlarge / D4s_v3 | 3 | 50 | General workloads |
| Code Executor | c5.2xlarge / F8s_v2 | 5 | 30 | CPU-intensive code execution |
| SQL Executor | m5.xlarge / D4s_v3 | 3 | 20 | SQL query execution |
| Database | r5.2xlarge / E8s_v3 | 3 | 10 | Memory-intensive databases |

#### Horizontal Pod Autoscalers

| Service | Min Replicas | Max Replicas | Metrics |
|---------|--------------|--------------|---------|
| API Gateway | 5 | 50 | CPU 70%, Memory 80% |
| Code Executor | 10 | 100 | CPU 70%, Memory 80%, Queue Length |
| SQL Executor | 5 | 50 | CPU 70%, Memory 80%, Queue Length |
| Gamification | 5 | 30 | CPU 70%, Memory 80% |
| AI Tutor | 3 | 20 | CPU 70%, Memory 80% |
| Notification | 3 | 20 | CPU 70%, Memory 80% |
| Analytics | 3 | 15 | CPU 70%, Memory 80% |
| Auth | 5 | 30 | CPU 70%, Memory 80% |
| Content | 5 | 30 | CPU 70%, Memory 80% |
| Frontend | 5 | 30 | CPU 70%, Memory 80% |

### Load Balancing

- **NGINX Ingress Controller**: 3 replicas with Network Load Balancer
- **Load Balancing Algorithm**: EWMA (Exponentially Weighted Moving Average)
- **Session Affinity**: Cookie-based for WebSocket connections
- **Health Checks**: HTTP health checks on `/healthz` endpoint

### Security Features

- **TLS/SSL**: Automatic certificate management with Let's Encrypt
- **HTTPS Enforcement**: All HTTP traffic redirected to HTTPS
- **HSTS**: HTTP Strict Transport Security enabled
- **Rate Limiting**: 100 requests/minute per user
- **CORS**: Configured for allowed origins
- **Security Headers**: X-Frame-Options, X-Content-Type-Options, X-XSS-Protection
- **Network Policies**: Zero-trust network isolation
- **RBAC**: Least-privilege access control
- **Pod Security**: Non-root containers, read-only root filesystem

### Performance Optimizations

- **Gzip Compression**: Enabled for API responses (Requirement 50.8)
- **Connection Pooling**: Optimized for high concurrency
- **Keep-Alive**: Persistent connections enabled
- **CDN Integration**: Ready for static asset delivery
- **Response Time Target**: <200ms at 95th percentile (Requirement 50.2)

## Requirements Satisfied

This infrastructure configuration satisfies the following requirements:

- **50.1**: Handle 10,000 concurrent users without performance degradation
- **50.2**: API response time <200ms at 95th percentile
- **50.3**: Horizontal scaling for Code Executor based on queue length
- **50.4**: Horizontal scaling for SQL Executor based on queue length
- **50.8**: Compress API responses using gzip
- **50.13**: Auto-scaling policies for all microservices
- **50.14**: Load balancer distributing traffic across multiple API Gateway instances
- **51.4**: HTTPS enforcement for all client-server communication
- **51.5**: Rate limiting to prevent brute force attacks

## Deployment Steps

1. **Provision Cluster**: Run `provision-aks.sh` or `provision-eks.sh`
2. **Configure DNS**: Point domains to Load Balancer IP/hostname
3. **Deploy Infrastructure**: Apply Redis, RabbitMQ, SQL Server manifests
4. **Deploy Microservices**: Apply all service deployment manifests
5. **Apply HPA**: `kubectl apply -f hpa-all-services.yaml`
6. **Apply Ingress**: `kubectl apply -f ingress-production.yaml`
7. **Verify**: Check pods, HPA, and ingress status

## Monitoring

### Dashboards
- **Grafana**: https://monitoring.platform.example.com/grafana
- **Prometheus**: https://monitoring.platform.example.com/prometheus
- **Kibana**: https://monitoring.platform.example.com/kibana

### Key Metrics to Monitor
- API response time (p50, p95, p99)
- Request rate and error rate
- Pod CPU and memory utilization
- HPA scaling events
- Cluster autoscaler events
- Node resource utilization
- Container execution queue length

## Cost Estimates

### Azure (AKS)
- **Base (3 nodes)**: ~$620/month
- **At Scale (50 nodes)**: ~$7,500/month

### AWS (EKS)
- **Base (3 nodes)**: ~$603/month
- **At Scale (50 nodes)**: ~$6,073/month

## Troubleshooting

### Cluster Autoscaler Not Scaling
```bash
kubectl logs -n kube-system deployment/cluster-autoscaler
```

### HPA Not Scaling
```bash
kubectl describe hpa <hpa-name> -n platform-saas-prod
kubectl top pods -n platform-saas-prod
```

### Ingress Not Working
```bash
kubectl logs -n ingress-nginx deployment/ingress-nginx-controller
kubectl describe ingress platform-ingress -n platform-saas-prod
```

### High Latency
```bash
# Check pod resource usage
kubectl top pods -n platform-saas-prod

# Check node resource usage
kubectl top nodes

# Check HPA status
kubectl get hpa -n platform-saas-prod
```

## Support

For detailed instructions and troubleshooting, see [PRODUCTION_INFRASTRUCTURE_GUIDE.md](./PRODUCTION_INFRASTRUCTURE_GUIDE.md).

For issues or questions:
- Platform Team: platform-team@example.com
- Documentation: [Main README](../../README.md)
- Kubernetes Docs: https://kubernetes.io/docs/
