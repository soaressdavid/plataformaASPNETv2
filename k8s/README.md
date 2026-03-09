# Kubernetes Deployment Guide

This directory contains Kubernetes manifests for deploying the ASP.NET Core Learning Platform to a production Kubernetes cluster.

## Architecture Overview

The platform consists of:
- **Infrastructure**: PostgreSQL, Redis, RabbitMQ
- **Microservices**: API Gateway, Auth, Course, Challenge, Progress, AI Tutor, Execution services
- **Workers**: Horizontally scalable code execution workers with HPA
- **Frontend**: Next.js application

## Prerequisites

1. **Kubernetes Cluster** (v1.24+)
   - Minikube (local testing)
   - GKE, EKS, AKS (cloud production)
   - Self-managed cluster

2. **kubectl** configured to access your cluster

3. **NGINX Ingress Controller** (for external access)
   ```bash
   kubectl apply -f https://raw.githubusercontent.com/kubernetes/ingress-nginx/controller-v1.8.1/deploy/static/provider/cloud/deploy.yaml
   ```

4. **Cert-Manager** (optional, for TLS certificates)
   ```bash
   kubectl apply -f https://github.com/cert-manager/cert-manager/releases/download/v1.13.0/cert-manager.yaml
   ```

5. **Metrics Server** (for HPA)
   ```bash
   kubectl apply -f https://github.com/kubernetes-sigs/metrics-server/releases/latest/download/components.yaml
   ```

6. **Docker Images** built and available
   - Build all images using the `DOCKER_BUILD_GUIDE.md` in the root directory
   - Push images to a container registry (Docker Hub, GCR, ECR, ACR) or use local images

## Configuration

### 1. Update Secrets

**IMPORTANT**: Before deploying, update the secrets in `secrets.yaml`:

```yaml
# Edit k8s/secrets.yaml
- POSTGRES_PASSWORD: Use a strong password
- RABBITMQ_PASSWORD: Use a strong password
- JWT_SECRET: Generate a secure random key (at least 32 characters)
- GROQ_API_KEY: Your actual Groq API key
```

### 2. Update Ingress Hostname

Edit `ingress.yaml` and replace `aspnet-learning-platform.example.com` with your actual domain:

```yaml
spec:
  tls:
  - hosts:
    - your-domain.com  # Change this
  rules:
  - host: your-domain.com  # Change this
```

### 3. Configure Image Pull Policy

If using a private container registry, create an image pull secret:

```bash
kubectl create secret docker-registry regcred \
  --docker-server=<your-registry-server> \
  --docker-username=<your-username> \
  --docker-password=<your-password> \
  --docker-email=<your-email> \
  -n aspnet-learning-platform
```

Then add to each deployment:
```yaml
spec:
  template:
    spec:
      imagePullSecrets:
      - name: regcred
```

## Deployment Steps

### Quick Deploy (All at Once)

```bash
# Deploy everything in order
kubectl apply -f namespace.yaml
kubectl apply -f secrets.yaml
kubectl apply -f configmap.yaml
kubectl apply -f postgres-deployment.yaml
kubectl apply -f redis-deployment.yaml
kubectl apply -f rabbitmq-deployment.yaml

# Wait for infrastructure to be ready
kubectl wait --for=condition=ready pod -l app=postgres -n aspnet-learning-platform --timeout=120s
kubectl wait --for=condition=ready pod -l app=redis -n aspnet-learning-platform --timeout=120s
kubectl wait --for=condition=ready pod -l app=rabbitmq -n aspnet-learning-platform --timeout=120s

# Deploy microservices
kubectl apply -f auth-service-deployment.yaml
kubectl apply -f course-service-deployment.yaml
kubectl apply -f challenge-service-deployment.yaml
kubectl apply -f progress-service-deployment.yaml
kubectl apply -f aitutor-service-deployment.yaml
kubectl apply -f execution-service-deployment.yaml
kubectl apply -f worker-deployment.yaml
kubectl apply -f worker-hpa.yaml

# Wait for services to be ready
kubectl wait --for=condition=ready pod -l app=auth-service -n aspnet-learning-platform --timeout=120s

# Deploy API Gateway and Frontend
kubectl apply -f api-gateway-deployment.yaml
kubectl apply -f frontend-deployment.yaml

# Deploy Ingress
kubectl apply -f ingress.yaml
```

### Step-by-Step Deploy

#### 1. Create Namespace
```bash
kubectl apply -f namespace.yaml
```

#### 2. Create Secrets and ConfigMap
```bash
kubectl apply -f secrets.yaml
kubectl apply -f configmap.yaml
```

#### 3. Deploy Infrastructure
```bash
# PostgreSQL
kubectl apply -f postgres-deployment.yaml
kubectl wait --for=condition=ready pod -l app=postgres -n aspnet-learning-platform --timeout=120s

# Redis
kubectl apply -f redis-deployment.yaml
kubectl wait --for=condition=ready pod -l app=redis -n aspnet-learning-platform --timeout=120s

# RabbitMQ
kubectl apply -f rabbitmq-deployment.yaml
kubectl wait --for=condition=ready pod -l app=rabbitmq -n aspnet-learning-platform --timeout=120s
```

#### 4. Run Database Migrations

Before deploying services, run database migrations:

```bash
# Option 1: Run as a Kubernetes Job
kubectl run migration-job \
  --image=aspnet-learning-platform/auth-service:latest \
  --restart=Never \
  --namespace=aspnet-learning-platform \
  --env="ConnectionStrings__DefaultConnection=Host=postgres-service;Port=5432;Database=aspnet_learning_platform;Username=platform_user;Password=platform_pass" \
  --command -- dotnet ef database update

# Option 2: Run from a service pod (after deployment)
kubectl exec -it deployment/auth-service -n aspnet-learning-platform -- dotnet ef database update
```

#### 5. Deploy Microservices
```bash
kubectl apply -f auth-service-deployment.yaml
kubectl apply -f course-service-deployment.yaml
kubectl apply -f challenge-service-deployment.yaml
kubectl apply -f progress-service-deployment.yaml
kubectl apply -f aitutor-service-deployment.yaml
kubectl apply -f execution-service-deployment.yaml
```

#### 6. Deploy Workers with Autoscaling
```bash
kubectl apply -f worker-deployment.yaml
kubectl apply -f worker-hpa.yaml
```

#### 7. Deploy API Gateway
```bash
kubectl apply -f api-gateway-deployment.yaml
```

#### 8. Deploy Frontend
```bash
kubectl apply -f frontend-deployment.yaml
```

#### 9. Deploy Ingress
```bash
kubectl apply -f ingress.yaml
```

## Verification

### Check All Pods
```bash
kubectl get pods -n aspnet-learning-platform
```

Expected output: All pods should be in `Running` state with `READY 1/1` or `2/2`.

### Check Services
```bash
kubectl get services -n aspnet-learning-platform
```

### Check Ingress
```bash
kubectl get ingress -n aspnet-learning-platform
```

### Check HPA Status
```bash
kubectl get hpa -n aspnet-learning-platform
```

### View Logs
```bash
# API Gateway logs
kubectl logs -f deployment/api-gateway -n aspnet-learning-platform

# Worker logs
kubectl logs -f deployment/worker -n aspnet-learning-platform

# Auth service logs
kubectl logs -f deployment/auth-service -n aspnet-learning-platform
```

## Accessing the Application

### Via Ingress (Production)
Once DNS is configured and pointing to your ingress controller's external IP:
```
https://your-domain.com
```

### Via Port Forward (Testing)
```bash
# Frontend
kubectl port-forward service/frontend-service 3000:3000 -n aspnet-learning-platform

# API Gateway
kubectl port-forward service/api-gateway-service 8080:8080 -n aspnet-learning-platform

# RabbitMQ Management
kubectl port-forward service/rabbitmq-service 15672:15672 -n aspnet-learning-platform
```

Then access:
- Frontend: http://localhost:3000
- API: http://localhost:8080
- RabbitMQ: http://localhost:15672

## Scaling

### Manual Scaling
```bash
# Scale a specific service
kubectl scale deployment auth-service --replicas=5 -n aspnet-learning-platform

# Scale workers
kubectl scale deployment worker --replicas=10 -n aspnet-learning-platform
```

### Horizontal Pod Autoscaler (HPA)

Workers are configured with HPA to automatically scale based on CPU and memory usage:
- **Min replicas**: 3
- **Max replicas**: 10
- **CPU target**: 70%
- **Memory target**: 80%

View HPA status:
```bash
kubectl get hpa worker-hpa -n aspnet-learning-platform --watch
```

## Monitoring

### Resource Usage
```bash
# Pod resource usage
kubectl top pods -n aspnet-learning-platform

# Node resource usage
kubectl top nodes
```

### Events
```bash
kubectl get events -n aspnet-learning-platform --sort-by='.lastTimestamp'
```

## Troubleshooting

### Pod Not Starting
```bash
# Describe pod to see events
kubectl describe pod <pod-name> -n aspnet-learning-platform

# Check logs
kubectl logs <pod-name> -n aspnet-learning-platform

# Check previous logs if pod restarted
kubectl logs <pod-name> -n aspnet-learning-platform --previous
```

### Service Connection Issues
```bash
# Test service connectivity from a pod
kubectl run test-pod --image=busybox --rm -it --restart=Never -n aspnet-learning-platform -- sh

# Inside the pod:
wget -O- http://auth-service:8080/health
wget -O- http://postgres-service:5432
```

### Database Connection Issues
```bash
# Connect to PostgreSQL
kubectl exec -it deployment/postgres -n aspnet-learning-platform -- psql -U platform_user -d aspnet_learning_platform

# Check database tables
\dt
```

### Worker Not Processing Jobs
```bash
# Check Redis connection
kubectl exec -it deployment/redis -n aspnet-learning-platform -- redis-cli

# Check queue length
LLEN execution_queue

# Check worker logs
kubectl logs -f deployment/worker -n aspnet-learning-platform
```

### HPA Not Scaling
```bash
# Check metrics server
kubectl get apiservice v1beta1.metrics.k8s.io -o yaml

# Check HPA status
kubectl describe hpa worker-hpa -n aspnet-learning-platform

# Manually check metrics
kubectl top pods -n aspnet-learning-platform
```

## Updating Deployments

### Rolling Update
```bash
# Update image version
kubectl set image deployment/auth-service auth-service=aspnet-learning-platform/auth-service:v2.0 -n aspnet-learning-platform

# Check rollout status
kubectl rollout status deployment/auth-service -n aspnet-learning-platform

# Rollback if needed
kubectl rollout undo deployment/auth-service -n aspnet-learning-platform
```

### Update ConfigMap or Secret
```bash
# Update the file
kubectl apply -f configmap.yaml

# Restart deployments to pick up changes
kubectl rollout restart deployment/auth-service -n aspnet-learning-platform
```

## Cleanup

### Delete Everything
```bash
kubectl delete namespace aspnet-learning-platform
```

### Delete Specific Resources
```bash
kubectl delete -f ingress.yaml
kubectl delete -f frontend-deployment.yaml
kubectl delete -f api-gateway-deployment.yaml
# ... etc
```

## Production Considerations

### 1. Persistent Storage
- Use persistent volumes for PostgreSQL instead of `emptyDir`
- Consider managed database services (Cloud SQL, RDS, Azure Database)

### 2. High Availability
- Run multiple replicas of all services
- Use pod anti-affinity to spread pods across nodes
- Configure pod disruption budgets

### 3. Security
- Use network policies to restrict pod-to-pod communication
- Enable RBAC and use least-privilege service accounts
- Scan images for vulnerabilities
- Use secrets management (Vault, AWS Secrets Manager)
- Enable pod security policies/standards

### 4. Monitoring & Logging
- Deploy Prometheus and Grafana for metrics
- Use ELK/EFK stack or cloud logging for centralized logs
- Set up alerts for critical issues

### 5. Backup & Disaster Recovery
- Regular database backups
- Backup persistent volumes
- Document recovery procedures

### 6. Performance
- Configure resource requests and limits appropriately
- Use horizontal pod autoscaling for all services
- Consider vertical pod autoscaling for right-sizing
- Use caching strategies (Redis)

### 7. CI/CD Integration
- Automate deployments with GitOps (ArgoCD, Flux)
- Use Helm charts for templating
- Implement blue-green or canary deployments

## Additional Resources

- [Kubernetes Documentation](https://kubernetes.io/docs/)
- [NGINX Ingress Controller](https://kubernetes.github.io/ingress-nginx/)
- [Cert-Manager](https://cert-manager.io/)
- [Horizontal Pod Autoscaler](https://kubernetes.io/docs/tasks/run-application/horizontal-pod-autoscale/)
