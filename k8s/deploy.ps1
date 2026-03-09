# ASP.NET Core Learning Platform - Kubernetes Deployment Script (PowerShell)
# This script deploys all components to a Kubernetes cluster

$ErrorActionPreference = "Stop"

$NAMESPACE = "aspnet-learning-platform"
$TIMEOUT = "120s"

Write-Host "==========================================" -ForegroundColor Cyan
Write-Host "ASP.NET Core Learning Platform Deployment" -ForegroundColor Cyan
Write-Host "==========================================" -ForegroundColor Cyan
Write-Host ""

function Print-Status {
    param([string]$Message)
    Write-Host "[✓] $Message" -ForegroundColor Green
}

function Print-Error {
    param([string]$Message)
    Write-Host "[✗] $Message" -ForegroundColor Red
}

function Print-Warning {
    param([string]$Message)
    Write-Host "[!] $Message" -ForegroundColor Yellow
}

# Check if kubectl is installed
try {
    kubectl version --client | Out-Null
} catch {
    Print-Error "kubectl is not installed. Please install kubectl first."
    exit 1
}

# Check if cluster is accessible
try {
    kubectl cluster-info | Out-Null
    Print-Status "Connected to Kubernetes cluster"
} catch {
    Print-Error "Cannot connect to Kubernetes cluster. Please check your kubeconfig."
    exit 1
}

# Step 1: Create Namespace
Write-Host ""
Write-Host "Step 1: Creating namespace..." -ForegroundColor Cyan
kubectl apply -f namespace.yaml
Print-Status "Namespace created"

# Step 2: Create Secrets and ConfigMap
Write-Host ""
Write-Host "Step 2: Creating secrets and configuration..." -ForegroundColor Cyan
Print-Warning "Make sure you've updated secrets.yaml with production values!"
$confirm = Read-Host "Have you updated the secrets? (yes/no)"
if ($confirm -ne "yes") {
    Print-Error "Please update secrets.yaml before deploying"
    exit 1
}

kubectl apply -f secrets.yaml
kubectl apply -f configmap.yaml
Print-Status "Secrets and ConfigMap created"

# Step 3: Deploy Infrastructure
Write-Host ""
Write-Host "Step 3: Deploying infrastructure (PostgreSQL, Redis, RabbitMQ)..." -ForegroundColor Cyan

Write-Host "  - Deploying PostgreSQL..."
kubectl apply -f postgres-deployment.yaml
kubectl wait --for=condition=ready pod -l app=postgres -n $NAMESPACE --timeout=$TIMEOUT
Print-Status "PostgreSQL is ready"

Write-Host "  - Deploying Redis Cluster..."
kubectl apply -f redis-cluster.yaml
kubectl wait --for=condition=ready pod -l app=redis-cluster -n $NAMESPACE --timeout=$TIMEOUT
Print-Status "Redis Cluster pods are ready"

Write-Host "  - Initializing Redis Cluster..."
Print-Warning "Initializing Redis cluster with 3 masters and 3 replicas..."
try {
    .\init-redis-cluster.ps1
    Print-Status "Redis Cluster initialized"
} catch {
    Print-Warning "Redis cluster initialization may need manual intervention"
}

Write-Host "  - Deploying RabbitMQ..."
kubectl apply -f rabbitmq-deployment.yaml
kubectl wait --for=condition=ready pod -l app=rabbitmq -n $NAMESPACE --timeout=$TIMEOUT
Print-Status "RabbitMQ is ready"

# Step 4: Run Database Migrations
Write-Host ""
Write-Host "Step 4: Database migrations..." -ForegroundColor Cyan
Print-Warning "You may need to run database migrations manually:"
Write-Host "  kubectl exec -it deployment/auth-service -n $NAMESPACE -- dotnet ef database update"
Read-Host "Press Enter to continue"

# Step 5: Deploy Microservices
Write-Host ""
Write-Host "Step 5: Deploying microservices..." -ForegroundColor Cyan

$services = @("auth-service", "course-service", "challenge-service", "progress-service", "aitutor-service", "execution-service")

foreach ($service in $services) {
    Write-Host "  - Deploying $service..."
    kubectl apply -f "$service-deployment.yaml"
}

# Wait for services to be ready
Write-Host "  - Waiting for services to be ready..."
foreach ($service in $services) {
    try {
        kubectl wait --for=condition=ready pod -l app=$service -n $NAMESPACE --timeout=$TIMEOUT
    } catch {
        Print-Warning "$service may not be ready yet"
    }
}
Print-Status "Microservices deployed"

# Step 6: Deploy Workers
Write-Host ""
Write-Host "Step 6: Deploying workers with autoscaling..." -ForegroundColor Cyan
kubectl apply -f worker-deployment.yaml
kubectl apply -f worker-hpa.yaml
try {
    kubectl wait --for=condition=ready pod -l app=worker -n $NAMESPACE --timeout=$TIMEOUT
} catch {
    Print-Warning "Workers may not be ready yet"
}
Print-Status "Workers deployed with HPA"

# Step 7: Deploy API Gateway
Write-Host ""
Write-Host "Step 7: Deploying API Gateway..." -ForegroundColor Cyan
kubectl apply -f api-gateway-deployment.yaml
kubectl wait --for=condition=ready pod -l app=api-gateway -n $NAMESPACE --timeout=$TIMEOUT
Print-Status "API Gateway deployed"

# Step 8: Deploy Frontend
Write-Host ""
Write-Host "Step 8: Deploying Frontend..." -ForegroundColor Cyan
kubectl apply -f frontend-deployment.yaml
kubectl wait --for=condition=ready pod -l app=frontend -n $NAMESPACE --timeout=$TIMEOUT
Print-Status "Frontend deployed"

# Step 9: Deploy Ingress
Write-Host ""
Write-Host "Step 9: Deploying Ingress..." -ForegroundColor Cyan
Print-Warning "Make sure you've updated ingress.yaml with your domain name!"
$confirmIngress = Read-Host "Have you updated the ingress hostname? (yes/no)"
if ($confirmIngress -eq "yes") {
    kubectl apply -f ingress.yaml
    Print-Status "Ingress deployed"
} else {
    Print-Warning "Skipping ingress deployment. You can deploy it later with: kubectl apply -f ingress.yaml"
}

# Summary
Write-Host ""
Write-Host "==========================================" -ForegroundColor Cyan
Write-Host "Deployment Summary" -ForegroundColor Cyan
Write-Host "==========================================" -ForegroundColor Cyan
Write-Host ""

Write-Host "Checking pod status..."
kubectl get pods -n $NAMESPACE

Write-Host ""
Write-Host "Checking services..."
kubectl get services -n $NAMESPACE

Write-Host ""
Write-Host "Checking HPA..."
kubectl get hpa -n $NAMESPACE

if ($confirmIngress -eq "yes") {
    Write-Host ""
    Write-Host "Checking ingress..."
    kubectl get ingress -n $NAMESPACE
}

Write-Host ""
Write-Host "==========================================" -ForegroundColor Cyan
Print-Status "Deployment completed!"
Write-Host "==========================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "Next steps:"
Write-Host "1. Run database migrations if not done already"
Write-Host "2. Configure DNS to point to your ingress controller"
Write-Host "3. Monitor logs: kubectl logs -f deployment/api-gateway -n $NAMESPACE"
Write-Host "4. Access via port-forward for testing:"
Write-Host "   kubectl port-forward service/frontend-service 3000:3000 -n $NAMESPACE"
Write-Host ""
