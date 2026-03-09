# Microservices Deployment Guide

This guide provides step-by-step instructions for deploying all microservices to the production Kubernetes cluster.

## Overview

The platform consists of the following microservices:

1. **API Gateway** - Entry point for all client requests
2. **Code Executor Service** - Compiles and executes C# code in isolated containers
3. **SQL Executor Service** - Executes SQL queries in temporary isolated databases
4. **Gamification Engine** - Manages XP, levels, streaks, achievements, and rankings
5. **AI Tutor Service** - Provides AI-powered code feedback and tutoring
6. **Notification Service** - Handles in-app, email, and push notifications
7. **Analytics Service** - Processes telemetry and generates insights

## Prerequisites

### 1. Kubernetes Cluster

Ensure you have a production Kubernetes cluster provisioned:

- **Azure**: Use `provision-aks.sh` or `provision-aks.ps1`
- **AWS**: Use `provision-eks.sh`

Verify cluster connection:
```bash
kubectl cluster-info
kubectl get nodes
```

### 2. Container Images

All microservice Docker images must be built and pushed to a container registry:

```bash
# Build all images
docker build -t aspnet-learning-platform/api-gateway:latest -f src/Services/ApiGateway/Dockerfile .
docker build -t aspnet-learning-platform/auth-service:latest -f src/Services/Auth/Dockerfile .
docker build -t aspnet-learning-platform/course-service:latest -f src/Services/Course/Dockerfile .
docker build -t aspnet-learning-platform/challenge-service:latest -f src/Services/Challenge/Dockerfile .
docker build -t aspnet-learning-platform/execution-service:latest -f src/Services/Execution/Dockerfile .
docker build -t aspnet-learning-platform/sqlexecutor-service:latest -f src/Services/SqlExecutor/Dockerfile .
docker build -t aspnet-learning-platform/progress-service:latest -f src/Services/Progress/Dockerfile .
docker build -t aspnet-learning-platform/aitutor-service:latest -f src/Services/AITutor/Dockerfile .
docker build -t aspnet-learning-platform/notification-service:latest -f src/Services/Notification/Dockerfile .
docker build -t aspnet-learning-platform/analytics-service:latest -f src/Services/Analytics/Dockerfile .

# Tag and push to registry (example for Azure Container Registry)
az acr login --name <your-acr-name>
docker tag aspnet-learning-platform/api-gateway:latest <your-acr-name>.azurecr.io/api-gateway:latest
docker push <your-acr-name>.azurecr.io/api-gateway:latest
# Repeat for all services...
```

### 3. Secrets Configuration

Create the required Kubernetes secrets:

```bash
kubectl create secret generic platform-secrets \
  --from-literal=JWT_SECRET='your-jwt-secret-key-min-32-chars' \
  --from-literal=SQL_CONNECTION_STRING='Server=<sql-server>;Database=aspnet_learning_platform;User Id=<user>;Password=<password>;TrustServerCertificate=True' \
  --from-literal=GROQ_API_KEY='your-groq-api-key' \
  --from-literal=SENDGRID_API_KEY='your-sendgrid-api-key' \
  -n aspnet-learning-platform
```

Or using PowerShell:
```powershell
kubectl create secret generic platform-secrets `
  --from-literal=JWT_SECRET='your-jwt-secret-key-min-32-chars' `
  --from-literal=SQL_CONNECTION_STRING='Server=<sql-server>;Database=aspnet_learning_platform;User Id=<user>;Password=<password>;TrustServerCertificate=True' `
  --from-literal=GROQ_API_KEY='your-groq-api-key' `
  --from-literal=SENDGRID_API_KEY='your-sendgrid-api-key' `
  -n aspnet-learning-platform
```

## Deployment Methods

### Method 1: Automated Deployment Script (Recommended)

Use the provided deployment script to deploy all microservices at once:

**Linux/Mac:**
```bash
cd k8s/production
chmod +x deploy-all-microservices.sh
./deploy-all-microservices.sh
```

**Windows:**
```powershell
cd k8s\production
.\deploy-all-microservices.ps1
```

The script will:
1. Verify cluster connection
2. Create namespace and apply ConfigMaps
3. Deploy Redis and RabbitMQ infrastructure
4. Deploy all microservices
5. Configure Horizontal Pod Autoscalers
6. Deploy Cluster Autoscaler
7. Configure Production Ingress
8. Verify all deployments

### Method 2: Manual Deployment

Deploy services individually for more control:

#### Step 1: Create Namespace
```bash
kubectl apply -f ../namespace.yaml
```

#### Step 2: Apply Configuration
```bash
kubectl apply -f ../configmap.yaml
```

#### Step 3: Deploy Infrastructure
```bash
kubectl apply -f ../redis-cluster.yaml
kubectl apply -f ../rabbitmq-deployment.yaml
```

Wait for infrastructure to be ready:
```bash
kubectl wait --for=condition=ready pod -l app=redis-cluster -n aspnet-learning-platform --timeout=120s
kubectl wait --for=condition=ready pod -l app=rabbitmq -n aspnet-learning-platform --timeout=120s
```

#### Step 4: Deploy Microservices

Deploy each service:
```bash
kubectl apply -f ../api-gateway-deployment.yaml
kubectl apply -f ../auth-service-deployment.yaml
kubectl apply -f ../course-service-deployment.yaml
kubectl apply -f ../challenge-service-deployment.yaml
kubectl apply -f ../execution-service-deployment.yaml
kubectl apply -f ../sqlexecutor-service-deployment.yaml
kubectl apply -f ../gamification-service-deployment.yaml
kubectl apply -f ../aitutor-service-deployment.yaml
kubectl apply -f ../notification-service-deployment.yaml
kubectl apply -f ../analytics-service-deployment.yaml
kubectl apply -f ../progress-service-deployment.yaml
```

#### Step 5: Configure Auto-scaling
```bash
kubectl apply -f hpa-all-services.yaml
kubectl apply -f cluster-autoscaler.yaml
```

#### Step 6: Configure Ingress
```bash
kubectl apply -f ingress-production.yaml
```

### Method 3: GitOps with ArgoCD

For continuous deployment, use ArgoCD:

```bash
# Apply ArgoCD application
kubectl apply -f ../argocd/application-production.yaml

# Sync application
argocd app sync aspnet-learning-platform-production
```

## Verification

### Check Deployment Status

```bash
# View all deployments
kubectl get deployments -n aspnet-learning-platform

# View all pods
kubectl get pods -n aspnet-learning-platform

# View all services
kubectl get services -n aspnet-learning-platform

# View ingress
kubectl get ingress -n aspnet-learning-platform
```

### Check Individual Service Health

```bash
# Check API Gateway
kubectl logs -f deployment/api-gateway -n aspnet-learning-platform

# Check Code Executor
kubectl logs -f deployment/execution-service -n aspnet-learning-platform

# Check SQL Executor
kubectl logs -f deployment/sqlexecutor-service -n aspnet-learning-platform

# Check Gamification Engine
kubectl logs -f deployment/gamification-service -n aspnet-learning-platform

# Check AI Tutor
kubectl logs -f deployment/aitutor-service -n aspnet-learning-platform

# Check Notification Service
kubectl logs -f deployment/notification-service -n aspnet-learning-platform

# Check Analytics Service
kubectl logs -f deployment/analytics-service -n aspnet-learning-platform
```

### Test API Gateway

Get the external IP:
```bash
kubectl get ingress -n aspnet-learning-platform
```

Test health endpoint:
```bash
curl http://<EXTERNAL-IP>/health
```

### Monitor Resource Usage

```bash
# View pod resource usage
kubectl top pods -n aspnet-learning-platform

# View node resource usage
kubectl top nodes
```

## Scaling

### Manual Scaling

Scale a specific service:
```bash
kubectl scale deployment/api-gateway --replicas=5 -n aspnet-learning-platform
```

### Auto-scaling

Horizontal Pod Autoscalers (HPA) are configured for all services:

```bash
# View HPA status
kubectl get hpa -n aspnet-learning-platform

# Describe specific HPA
kubectl describe hpa api-gateway-hpa -n aspnet-learning-platform
```

HPA Configuration:
- **Min Replicas**: 2
- **Max Replicas**: 10
- **Target CPU**: 70%
- **Target Memory**: 80%

## Troubleshooting

### Pod Not Starting

Check pod events:
```bash
kubectl describe pod <pod-name> -n aspnet-learning-platform
```

Check pod logs:
```bash
kubectl logs <pod-name> -n aspnet-learning-platform
```

### Service Not Accessible

Check service endpoints:
```bash
kubectl get endpoints -n aspnet-learning-platform
```

Check service configuration:
```bash
kubectl describe service <service-name> -n aspnet-learning-platform
```

### Database Connection Issues

Verify secrets:
```bash
kubectl get secret platform-secrets -n aspnet-learning-platform -o yaml
```

Test database connectivity from a pod:
```bash
kubectl run -it --rm debug --image=mcr.microsoft.com/mssql-tools --restart=Never -n aspnet-learning-platform -- /bin/bash
sqlcmd -S <sql-server> -U <user> -P <password>
```

### Redis Connection Issues

Check Redis cluster status:
```bash
kubectl exec -it redis-cluster-0 -n aspnet-learning-platform -- redis-cli cluster info
```

### RabbitMQ Connection Issues

Check RabbitMQ status:
```bash
kubectl exec -it rabbitmq-0 -n aspnet-learning-platform -- rabbitmqctl status
```

## Rollback

If deployment fails, rollback to previous version:

```bash
# Rollback specific deployment
kubectl rollout undo deployment/api-gateway -n aspnet-learning-platform

# Rollback to specific revision
kubectl rollout undo deployment/api-gateway --to-revision=2 -n aspnet-learning-platform

# View rollout history
kubectl rollout history deployment/api-gateway -n aspnet-learning-platform
```

## Update Strategy

All deployments use **RollingUpdate** strategy:
- **Max Surge**: 1 (one extra pod during update)
- **Max Unavailable**: 0 (zero downtime)

Update a service:
```bash
# Update image
kubectl set image deployment/api-gateway api-gateway=aspnet-learning-platform/api-gateway:v2.0 -n aspnet-learning-platform

# Watch rollout
kubectl rollout status deployment/api-gateway -n aspnet-learning-platform
```

## Production Checklist

Before deploying to production, ensure:

- [ ] Kubernetes cluster is provisioned and healthy
- [ ] All Docker images are built and pushed to registry
- [ ] Secrets are created with production values
- [ ] Database is provisioned with read replicas
- [ ] Redis cluster is configured with persistence
- [ ] RabbitMQ is configured with high availability
- [ ] Monitoring is configured (Application Insights, ELK, Prometheus)
- [ ] Ingress is configured with SSL/TLS certificates
- [ ] Auto-scaling is configured (HPA and Cluster Autoscaler)
- [ ] Backup strategy is in place
- [ ] Disaster recovery plan is documented
- [ ] Load testing has been performed
- [ ] Security scanning has been completed

## Monitoring and Alerts

### Application Insights

View metrics in Azure Portal:
- Navigate to Application Insights resource
- View Live Metrics, Performance, Failures

### Prometheus + Grafana

Access Grafana dashboard:
```bash
kubectl port-forward svc/grafana 3000:3000 -n aspnet-learning-platform
```

Open browser: http://localhost:3000

### ELK Stack

Access Kibana:
```bash
kubectl port-forward svc/kibana 5601:5601 -n aspnet-learning-platform
```

Open browser: http://localhost:5601

## Support

For issues or questions:
1. Check logs: `kubectl logs -f deployment/<service-name> -n aspnet-learning-platform`
2. Check events: `kubectl get events -n aspnet-learning-platform --sort-by='.lastTimestamp'`
3. Review monitoring dashboards
4. Contact DevOps team

## Additional Resources

- [Kubernetes Documentation](https://kubernetes.io/docs/)
- [Azure Kubernetes Service (AKS)](https://docs.microsoft.com/en-us/azure/aks/)
- [Amazon Elastic Kubernetes Service (EKS)](https://docs.aws.amazon.com/eks/)
- [ArgoCD Documentation](https://argo-cd.readthedocs.io/)
