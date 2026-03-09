# ASP.NET Core Learning Platform - Kubernetes Cleanup Script (PowerShell)
# This script removes all deployed components from the Kubernetes cluster

$ErrorActionPreference = "Stop"

$NAMESPACE = "aspnet-learning-platform"

Write-Host "==========================================" -ForegroundColor Cyan
Write-Host "ASP.NET Core Learning Platform Cleanup" -ForegroundColor Cyan
Write-Host "==========================================" -ForegroundColor Cyan
Write-Host ""

function Print-Warning {
    param([string]$Message)
    Write-Host "[!] $Message" -ForegroundColor Yellow
}

function Print-Error {
    param([string]$Message)
    Write-Host "[✗] $Message" -ForegroundColor Red
}

# Warning
Print-Warning "This will delete ALL resources in the $NAMESPACE namespace!"
Print-Warning "This action cannot be undone!"
Write-Host ""
$confirm = Read-Host "Are you sure you want to continue? (type 'yes' to confirm)"

if ($confirm -ne "yes") {
    Write-Host "Cleanup cancelled."
    exit 0
}

Write-Host ""
Write-Host "Starting cleanup..."

# Check if namespace exists
try {
    kubectl get namespace $NAMESPACE | Out-Null
} catch {
    Print-Error "Namespace $NAMESPACE does not exist. Nothing to clean up."
    exit 0
}

# Option 1: Delete entire namespace (fastest)
Write-Host ""
$deleteNamespace = Read-Host "Delete entire namespace? This is the fastest method. (yes/no)"

if ($deleteNamespace -eq "yes") {
    Write-Host "Deleting namespace $NAMESPACE..."
    kubectl delete namespace $NAMESPACE
    Write-Host "Cleanup complete!" -ForegroundColor Green
    exit 0
}

# Option 2: Delete resources individually
Write-Host ""
Write-Host "Deleting resources individually..."

Write-Host "  - Deleting Ingress..."
kubectl delete -f ingress.yaml --ignore-not-found=true

Write-Host "  - Deleting Frontend..."
kubectl delete -f frontend-deployment.yaml --ignore-not-found=true

Write-Host "  - Deleting API Gateway..."
kubectl delete -f api-gateway-deployment.yaml --ignore-not-found=true

Write-Host "  - Deleting Workers..."
kubectl delete -f worker-hpa.yaml --ignore-not-found=true
kubectl delete -f worker-deployment.yaml --ignore-not-found=true

Write-Host "  - Deleting Microservices..."
kubectl delete -f execution-service-deployment.yaml --ignore-not-found=true
kubectl delete -f aitutor-service-deployment.yaml --ignore-not-found=true
kubectl delete -f progress-service-deployment.yaml --ignore-not-found=true
kubectl delete -f challenge-service-deployment.yaml --ignore-not-found=true
kubectl delete -f course-service-deployment.yaml --ignore-not-found=true
kubectl delete -f auth-service-deployment.yaml --ignore-not-found=true

Write-Host "  - Deleting Infrastructure..."
kubectl delete -f rabbitmq-deployment.yaml --ignore-not-found=true
kubectl delete -f redis-deployment.yaml --ignore-not-found=true
kubectl delete -f postgres-deployment.yaml --ignore-not-found=true

Write-Host "  - Deleting Configuration..."
kubectl delete -f configmap.yaml --ignore-not-found=true
kubectl delete -f secrets.yaml --ignore-not-found=true

Write-Host ""
$deleteNsFinal = Read-Host "Delete namespace $NAMESPACE? (yes/no)"
if ($deleteNsFinal -eq "yes") {
    kubectl delete -f namespace.yaml --ignore-not-found=true
    Write-Host "Namespace deleted."
} else {
    Write-Host "Namespace preserved."
}

Write-Host ""
Write-Host "==========================================" -ForegroundColor Cyan
Write-Host "Cleanup complete!" -ForegroundColor Green
Write-Host "==========================================" -ForegroundColor Cyan
