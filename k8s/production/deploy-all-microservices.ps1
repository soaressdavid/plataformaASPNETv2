# Deploy All Microservices to Production Kubernetes Cluster
# This script deploys all microservices for the ASP.NET Learning Platform

$ErrorActionPreference = "Stop"

Write-Host "==========================================" -ForegroundColor Cyan
Write-Host "Deploying All Microservices to Production" -ForegroundColor Cyan
Write-Host "==========================================" -ForegroundColor Cyan

# Check if kubectl is installed
if (-not (Get-Command kubectl -ErrorAction SilentlyContinue)) {
    Write-Host "Error: kubectl is not installed" -ForegroundColor Red
    exit 1
}

# Check if we're connected to a cluster
try {
    kubectl cluster-info | Out-Null
} catch {
    Write-Host "Error: Not connected to a Kubernetes cluster" -ForegroundColor Red
    exit 1
}

# Get current context
$context = kubectl config current-context
Write-Host "Current context: $context" -ForegroundColor Yellow
Write-Host ""

# Confirm deployment
$confirm = Read-Host "Are you sure you want to deploy to this cluster? (yes/no)"
if ($confirm -ne "yes") {
    Write-Host "Deployment cancelled"
    exit 0
}

Write-Host ""
Write-Host "==========================================" -ForegroundColor Cyan
Write-Host "Step 1: Ensure namespace exists" -ForegroundColor Cyan
Write-Host "==========================================" -ForegroundColor Cyan
kubectl apply -f ..\namespace.yaml
Write-Host "✓ Namespace configured" -ForegroundColor Green
Write-Host ""

Write-Host "==========================================" -ForegroundColor Cyan
Write-Host "Step 2: Apply ConfigMaps and Secrets" -ForegroundColor Cyan
Write-Host "==========================================" -ForegroundColor Cyan
kubectl apply -f ..\configmap.yaml
Write-Host "✓ ConfigMap applied" -ForegroundColor Green

# Check if secrets exist
try {
    kubectl get secret platform-secrets -n aspnet-learning-platform | Out-Null
    Write-Host "✓ Secrets verified" -ForegroundColor Green
} catch {
    Write-Host "Warning: platform-secrets not found. Please create secrets before proceeding." -ForegroundColor Yellow
    Write-Host "Required secrets:"
    Write-Host "  - JWT_SECRET"
    Write-Host "  - SQL_CONNECTION_STRING"
    Write-Host "  - GROQ_API_KEY"
    Write-Host "  - SENDGRID_API_KEY"
    Write-Host ""
    $continue = Read-Host "Continue anyway? (yes/no)"
    if ($continue -ne "yes") {
        exit 1
    }
}
Write-Host ""

Write-Host "==========================================" -ForegroundColor Cyan
Write-Host "Step 3: Deploy Core Infrastructure" -ForegroundColor Cyan
Write-Host "==========================================" -ForegroundColor Cyan

# Deploy Redis
Write-Host "Deploying Redis cluster..."
kubectl apply -f ..\redis-cluster.yaml
Write-Host "✓ Redis cluster deployed" -ForegroundColor Green

# Deploy RabbitMQ
Write-Host "Deploying RabbitMQ..."
kubectl apply -f ..\rabbitmq-deployment.yaml
Write-Host "✓ RabbitMQ deployed" -ForegroundColor Green

# Wait for infrastructure to be ready
Write-Host "Waiting for infrastructure to be ready..."
kubectl wait --for=condition=ready pod -l app=redis-cluster -n aspnet-learning-platform --timeout=120s
kubectl wait --for=condition=ready pod -l app=rabbitmq -n aspnet-learning-platform --timeout=120s
Write-Host ""

Write-Host "==========================================" -ForegroundColor Cyan
Write-Host "Step 4: Deploy Microservices" -ForegroundColor Cyan
Write-Host "==========================================" -ForegroundColor Cyan

# Array of microservices to deploy
$services = @(
    "api-gateway",
    "auth-service",
    "course-service",
    "challenge-service",
    "execution-service",
    "sqlexecutor-service",
    "gamification-service",
    "aitutor-service",
    "notification-service",
    "analytics-service",
    "progress-service"
)

# Deploy each service
foreach ($service in $services) {
    Write-Host "Deploying $service..."
    kubectl apply -f "..\$service-deployment.yaml"
    Write-Host "✓ $service deployed" -ForegroundColor Green
}

Write-Host ""
Write-Host "==========================================" -ForegroundColor Cyan
Write-Host "Step 5: Deploy Production Configuration" -ForegroundColor Cyan
Write-Host "==========================================" -ForegroundColor Cyan

# Deploy Horizontal Pod Autoscalers
Write-Host "Deploying HPA for all services..."
kubectl apply -f hpa-all-services.yaml
Write-Host "✓ HPA configured" -ForegroundColor Green

# Deploy Cluster Autoscaler
Write-Host "Deploying Cluster Autoscaler..."
kubectl apply -f cluster-autoscaler.yaml
Write-Host "✓ Cluster Autoscaler deployed" -ForegroundColor Green

# Deploy Production Ingress
Write-Host "Deploying Production Ingress..."
kubectl apply -f ingress-production.yaml
Write-Host "✓ Production Ingress configured" -ForegroundColor Green

Write-Host ""
Write-Host "==========================================" -ForegroundColor Cyan
Write-Host "Step 6: Verify Deployments" -ForegroundColor Cyan
Write-Host "==========================================" -ForegroundColor Cyan

Write-Host "Waiting for deployments to be ready..."
foreach ($service in $services) {
    Write-Host "Checking $service..."
    try {
        kubectl rollout status deployment/$service -n aspnet-learning-platform --timeout=180s
    } catch {
        Write-Host "Warning: $service not ready yet" -ForegroundColor Yellow
    }
}

Write-Host ""
Write-Host "==========================================" -ForegroundColor Cyan
Write-Host "Deployment Summary" -ForegroundColor Cyan
Write-Host "==========================================" -ForegroundColor Cyan

# Get deployment status
Write-Host "Deployment Status:"
kubectl get deployments -n aspnet-learning-platform

Write-Host ""
Write-Host "Pod Status:"
kubectl get pods -n aspnet-learning-platform

Write-Host ""
Write-Host "Service Status:"
kubectl get services -n aspnet-learning-platform

Write-Host ""
Write-Host "Ingress Status:"
kubectl get ingress -n aspnet-learning-platform

Write-Host ""
Write-Host "==========================================" -ForegroundColor Cyan
Write-Host "Deployment Complete!" -ForegroundColor Cyan
Write-Host "==========================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "Next steps:"
Write-Host "1. Verify all pods are running: kubectl get pods -n aspnet-learning-platform"
Write-Host "2. Check logs if any issues: kubectl logs -f deployment/<service-name> -n aspnet-learning-platform"
Write-Host "3. Test API Gateway health: curl http://<ingress-ip>/health"
Write-Host "4. Monitor with: kubectl top pods -n aspnet-learning-platform"
Write-Host ""
Write-Host "All microservices deployed successfully!" -ForegroundColor Green
