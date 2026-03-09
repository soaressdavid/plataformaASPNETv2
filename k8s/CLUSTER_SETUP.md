# Kubernetes Cluster Configuration Guide

This document describes the Kubernetes cluster configuration for the Platform Evolution SaaS project, including namespace setup, resource quotas, RBAC policies, and security configurations.

## Overview

The cluster is configured with three environments:
- **Development** (`platform-saas-dev`)
- **Staging** (`platform-saas-staging`)
- **Production** (`platform-saas-prod`)

Each environment has isolated namespaces with appropriate resource quotas, limit ranges, RBAC policies, and network policies.

## Architecture

### Microservices
The platform consists of 9 microservices:
1. **API Gateway** - Entry point with JWT authentication and rate limiting
2. **Code Executor** - Compiles and executes C# code in isolated containers
3. **SQL Executor** - Executes SQL queries in temporary isolated databases
4. **Gamification Engine** - Manages XP, levels, streaks, achievements
5. **AI Tutor** - Provides AI-powered code feedback via Groq API
6. **Notification Service** - Handles in-app, email, and push notifications
7. **Analytics Service** - Processes telemetry and generates insights
8. **Telemetry Service** - Collects metrics and logs
9. **Cache Service** - Redis-based distributed caching

### Infrastructure Components
- **SQL Server 2022** - Primary database with read replicas
- **Redis 7.x** - Distributed cache and session storage
- **RabbitMQ** - Message queue for async communication
- **Prometheus + Grafana** - Metrics and monitoring
- **ELK Stack** - Log aggregation and analysis

## Prerequisites

1. **Kubernetes Cluster** (v1.24+)
   - Minikube (local development)
   - AKS, EKS, or GKE (cloud production)

2. **kubectl** CLI tool installed and configured

3. **Helm** (optional, for installing monitoring stack)

4. **Docker** registry access for container images

## Configuration Files

### Core Configuration
- `namespace.yaml` - Namespace definitions for dev, staging, prod
- `resource-quotas.yaml` - Resource limits per environment
- `limit-ranges.yaml` - Default container resource limits
- `rbac.yaml` - Production RBAC policies and service accounts
- `rbac-dev.yaml` - Development RBAC policies
- `rbac-staging.yaml` - Staging RBAC policies
- `network-policies.yaml` - Network isolation and security policies
- `pod-security-policies.yaml` - Pod security standards

### Resource Quotas by Environment

#### Development
- CPU Requests: 10 cores
- Memory Requests: 20Gi
- CPU Limits: 20 cores
- Memory Limits: 40Gi
- Max Pods: 50
- Max Services: 20

#### Staging
- CPU Requests: 20 cores
- Memory Requests: 40Gi
- CPU Limits: 40 cores
- Memory Limits: 80Gi
- Max Pods: 100
- Max Services: 30

#### Production
- CPU Requests: 100 cores
- Memory Requests: 200Gi
- CPU Limits: 200 cores
- Memory Limits: 400Gi
- Max Pods: 500
- Max Services: 50

### Container Resource Limits

#### Development
- Default CPU: 500m (request: 250m)
- Default Memory: 512Mi (request: 256Mi)
- Max CPU per container: 2 cores
- Max Memory per container: 4Gi

#### Staging
- Default CPU: 1 core (request: 500m)
- Default Memory: 1Gi (request: 512Mi)
- Max CPU per container: 4 cores
- Max Memory per container: 8Gi

#### Production
- Default CPU: 2 cores (request: 1 core)
- Default Memory: 2Gi (request: 1Gi)
- Max CPU per container: 8 cores
- Max Memory per container: 16Gi

## RBAC Configuration

### Service Accounts
Each microservice has a dedicated service account with least-privilege access:

- `api-gateway-sa` - Read-only access to services and endpoints
- `code-executor-sa` - Pod management for container execution
- `sql-executor-sa` - Pod management for SQL containers
- `gamification-sa` - Standard access to secrets and configmaps
- `ai-tutor-sa` - Standard access plus external API calls
- `notification-sa` - Standard access plus external email/push services
- `analytics-sa` - Standard access to secrets and configmaps
- `telemetry-sa` - Standard access to secrets and configmaps
- `monitoring-sa` - Cluster-wide read access for metrics collection

### Roles and Permissions

#### Code Executor Role
- **Permissions**: Create, delete, list, watch pods and pod logs
- **Reason**: Needs to manage Docker containers for code execution
- **Namespace**: Scoped to environment namespace

#### SQL Executor Role
- **Permissions**: Create, delete, list, watch pods and pod logs
- **Reason**: Needs to manage SQL Server containers for query execution
- **Namespace**: Scoped to environment namespace

#### API Gateway Role
- **Permissions**: Read-only access to services, endpoints, secrets, configmaps
- **Reason**: Service discovery and configuration access
- **Namespace**: Scoped to environment namespace

#### Standard Service Role
- **Permissions**: Read-only access to secrets and configmaps
- **Reason**: Access to configuration and credentials
- **Namespace**: Scoped to environment namespace

#### Monitoring ClusterRole
- **Permissions**: Cluster-wide read access to nodes, pods, services, deployments
- **Reason**: Collect metrics across all namespaces
- **Scope**: Cluster-wide

## Network Policies

Network policies enforce zero-trust security by default:

### Default Deny
- All ingress traffic is denied by default in production
- Services must explicitly allow traffic

### Code Executor
- **Ingress**: Only from API Gateway on port 80
- **Egress**: DNS, Redis, RabbitMQ only
- **Blocked**: External internet access (security requirement 21.6)

### SQL Executor
- **Ingress**: Only from API Gateway on port 80
- **Egress**: DNS, Redis, RabbitMQ only
- **Blocked**: External internet access

### API Gateway
- **Ingress**: All traffic on ports 80/443
- **Egress**: All internal services

### Database (SQL Server)
- **Ingress**: Only from specific services (API Gateway, Gamification, Analytics, Notification)
- **Egress**: None

### Redis
- **Ingress**: All microservices on port 6379
- **Egress**: None

### RabbitMQ
- **Ingress**: All microservices on ports 5672, 15672
- **Egress**: None

### AI Tutor
- **Ingress**: Only from API Gateway
- **Egress**: DNS, Redis, external HTTPS (for Groq API)

### Notification Service
- **Ingress**: Only from API Gateway
- **Egress**: DNS, Database, RabbitMQ, external HTTPS/SMTP (for email/push)

## Pod Security Policies

### Restricted PSP (Default)
Applied to: API Gateway, Gamification, AI Tutor, Notification, Analytics, Telemetry

- **Privileged**: Disabled
- **Privilege Escalation**: Disabled
- **Root User**: Disabled (must run as non-root)
- **Read-only Root Filesystem**: Enabled
- **Capabilities**: All dropped

### Code Executor PSP
Applied to: Code Executor service

- **Privileged**: Enabled (required for Docker-in-Docker)
- **Host Path**: `/var/run/docker.sock` (Docker socket access)
- **Justification**: Needs Docker socket to create isolated execution containers

### SQL Executor PSP
Applied to: SQL Executor service

- **Privileged**: Enabled (required for SQL container management)
- **Host Path**: `/var/run/docker.sock` (Docker socket access)
- **Justification**: Needs Docker socket to create isolated SQL containers

## Deployment Instructions

### 1. Create Namespaces
```bash
kubectl apply -f namespace.yaml
```

### 2. Apply Resource Quotas and Limits
```bash
kubectl apply -f resource-quotas.yaml
kubectl apply -f limit-ranges.yaml
```

### 3. Setup RBAC
```bash
# Production
kubectl apply -f rbac.yaml

# Staging
kubectl apply -f rbac-staging.yaml

# Development
kubectl apply -f rbac-dev.yaml
```

### 4. Apply Network Policies
```bash
kubectl apply -f network-policies.yaml
```

### 5. Apply Pod Security Policies
```bash
kubectl apply -f pod-security-policies.yaml
```

### 6. Verify Configuration
```bash
# Check namespaces
kubectl get namespaces -l name=platform-saas

# Check resource quotas
kubectl get resourcequota -n platform-saas-prod
kubectl describe resourcequota prod-resource-quota -n platform-saas-prod

# Check limit ranges
kubectl get limitrange -n platform-saas-prod
kubectl describe limitrange prod-limit-range -n platform-saas-prod

# Check service accounts
kubectl get serviceaccounts -n platform-saas-prod

# Check roles and rolebindings
kubectl get roles -n platform-saas-prod
kubectl get rolebindings -n platform-saas-prod

# Check network policies
kubectl get networkpolicies -n platform-saas-prod

# Check pod security policies
kubectl get psp
```

## Security Considerations

### Container Isolation (Requirements 21.1-21.4)
- All user code executes in isolated Docker containers
- Memory limit: 512MB per container
- CPU limit: 1 core per container
- Disk space limit: 100MB per container
- Execution timeout: 60 seconds

### Network Isolation (Requirement 21.6)
- Code execution containers have no external internet access
- Only whitelisted internal services are accessible
- Network policies enforce zero-trust model

### RBAC Least Privilege
- Each service has minimal required permissions
- Service accounts are namespace-scoped
- Privileged access only for Code/SQL Executor (required for container management)

### Pod Security
- Non-root user enforcement for standard services
- Read-only root filesystem where possible
- All capabilities dropped by default
- Seccomp and AppArmor profiles enabled

## Monitoring and Observability

### Metrics Collection
- Prometheus scrapes metrics from all services
- Grafana dashboards for visualization
- Custom metrics for execution queue length, container pool size

### Logging
- All services log to stdout/stderr
- Logs aggregated by ELK stack
- Structured logging with correlation IDs

### Tracing
- Distributed tracing with Application Insights
- Correlation IDs propagated across service calls
- Performance monitoring for API response times

## Scaling Configuration

### Horizontal Pod Autoscaling (HPA)
Each service has HPA configured based on:
- CPU utilization (70% target)
- Memory utilization (80% target)
- Custom metrics (e.g., execution queue length for Code Executor)

### Cluster Autoscaling
- Node autoscaling based on resource requests
- Min nodes: 3 (high availability)
- Max nodes: 50 (cost control)

## Disaster Recovery

### Backup Strategy
- Database: Daily automated backups with 30-day retention
- Redis: RDB + AOF persistence enabled
- Configuration: All manifests in Git (GitOps)

### High Availability
- Multiple replicas for all services (min 3 for production)
- Pod anti-affinity rules to spread across nodes
- ReadinessProbe and LivenessProbe for health checks

## Troubleshooting

### Check Resource Usage
```bash
kubectl top nodes
kubectl top pods -n platform-saas-prod
```

### Check Pod Status
```bash
kubectl get pods -n platform-saas-prod
kubectl describe pod <pod-name> -n platform-saas-prod
kubectl logs <pod-name> -n platform-saas-prod
```

### Check Network Policies
```bash
kubectl describe networkpolicy <policy-name> -n platform-saas-prod
```

### Check RBAC Permissions
```bash
kubectl auth can-i --list --as=system:serviceaccount:platform-saas-prod:code-executor-sa -n platform-saas-prod
```

### Debug Network Connectivity
```bash
# Run a debug pod
kubectl run debug --image=nicolaka/netshoot -n platform-saas-prod --rm -it -- /bin/bash

# Test connectivity
curl http://api-gateway
nslookup redis
```

## References

- [Kubernetes Documentation](https://kubernetes.io/docs/)
- [RBAC Authorization](https://kubernetes.io/docs/reference/access-authn-authz/rbac/)
- [Network Policies](https://kubernetes.io/docs/concepts/services-networking/network-policies/)
- [Pod Security Standards](https://kubernetes.io/docs/concepts/security/pod-security-standards/)
- [Resource Quotas](https://kubernetes.io/docs/concepts/policy/resource-quotas/)
- [Limit Ranges](https://kubernetes.io/docs/concepts/policy/limit-range/)

## Requirements Mapping

This configuration satisfies the following requirements:

- **21.1**: All user code executes in isolated Docker containers
- **21.2**: Container memory limited to 512MB maximum
- **21.3**: Container CPU limited to 1 core maximum
- **21.4**: Container disk space limited to 100MB maximum
- **21.6**: Network access from containers restricted (network policies)
- **21.7**: File system access restricted to container workspace
- **21.10**: Container creation/destruction logged (via telemetry service)

## Support

For issues or questions, contact the platform team or refer to the main project documentation.
