# Quick provisioning script for Azure Kubernetes Service (AKS)
# Requirements: 50.1, 50.13 - Support 10,000 concurrent users with auto-scaling

param(
    [string]$ResourceGroup = "platform-saas-prod-rg",
    [string]$Location = "eastus",
    [string]$ClusterName = "platform-saas-prod-aks",
    [string]$SubscriptionId = ""
)

$ErrorActionPreference = "Stop"

Write-Host "========================================" -ForegroundColor Green
Write-Host "AKS Production Cluster Provisioning" -ForegroundColor Green
Write-Host "========================================" -ForegroundColor Green
Write-Host ""

# Check prerequisites
Write-Host "Checking prerequisites..." -ForegroundColor Yellow

if (-not (Get-Command az -ErrorAction SilentlyContinue)) {
    Write-Host "Error: Azure CLI not found. Please install it first." -ForegroundColor Red
    exit 1
}

if (-not (Get-Command kubectl -ErrorAction SilentlyContinue)) {
    Write-Host "Error: kubectl not found. Please install it first." -ForegroundColor Red
    exit 1
}

if (-not (Get-Command helm -ErrorAction SilentlyContinue)) {
    Write-Host "Error: Helm not found. Please install it first." -ForegroundColor Red
    exit 1
}

Write-Host "✓ All prerequisites met" -ForegroundColor Green
Write-Host ""

# Login to Azure
Write-Host "Logging in to Azure..." -ForegroundColor Yellow
az login

if ($SubscriptionId) {
    az account set --subscription $SubscriptionId
}

$currentSubscription = az account show --query name -o tsv
Write-Host "✓ Using subscription: $currentSubscription" -ForegroundColor Green
Write-Host ""

# Create resource group
Write-Host "Creating resource group..." -ForegroundColor Yellow
az group create `
  --name $ResourceGroup `
  --location $Location `
  --output table

Write-Host "✓ Resource group created" -ForegroundColor Green
Write-Host ""

# Create Log Analytics workspace
Write-Host "Creating Log Analytics workspace..." -ForegroundColor Yellow
$workspaceId = az monitor log-analytics workspace create `
  --resource-group $ResourceGroup `
  --workspace-name "platform-saas-logs" `
  --location $Location `
  --query id -o tsv

Write-Host "✓ Log Analytics workspace created" -ForegroundColor Green
Write-Host ""

# Create AKS cluster
Write-Host "Creating AKS cluster (this may take 10-15 minutes)..." -ForegroundColor Yellow
az aks create `
  --resource-group $ResourceGroup `
  --name $ClusterName `
  --location $Location `
  --node-count 3 `
  --min-count 3 `
  --max-count 50 `
  --enable-cluster-autoscaler `
  --node-vm-size Standard_D4s_v3 `
  --zones 1 2 3 `
  --enable-managed-identity `
  --network-plugin azure `
  --network-policy azure `
  --load-balancer-sku standard `
  --enable-addons monitoring `
  --workspace-resource-id $workspaceId `
  --generate-ssh-keys `
  --kubernetes-version 1.28.3 `
  --output table

Write-Host "✓ AKS cluster created" -ForegroundColor Green
Write-Host ""

# Get credentials
Write-Host "Getting cluster credentials..." -ForegroundColor Yellow
az aks get-credentials `
  --resource-group $ResourceGroup `
  --name $ClusterName `
  --overwrite-existing

Write-Host "✓ Credentials configured" -ForegroundColor Green
Write-Host ""

# Create Code Executor node pool
Write-Host "Creating Code Executor node pool..." -ForegroundColor Yellow
az aks nodepool add `
  --resource-group $ResourceGroup `
  --cluster-name $ClusterName `
  --name codeexec `
  --node-count 5 `
  --min-count 5 `
  --max-count 30 `
  --enable-cluster-autoscaler `
  --node-vm-size Standard_F8s_v2 `
  --zones 1 2 3 `
  --labels workload=code-executor `
  --node-taints workload=code-executor:NoSchedule `
  --output table

Write-Host "✓ Code Executor node pool created" -ForegroundColor Green
Write-Host ""

# Create SQL Executor node pool
Write-Host "Creating SQL Executor node pool..." -ForegroundColor Yellow
az aks nodepool add `
  --resource-group $ResourceGroup `
  --cluster-name $ClusterName `
  --name sqlexec `
  --node-count 3 `
  --min-count 3 `
  --max-count 20 `
  --enable-cluster-autoscaler `
  --node-vm-size Standard_D4s_v3 `
  --zones 1 2 3 `
  --labels workload=sql-executor `
  --node-taints workload=sql-executor:NoSchedule `
  --output table

Write-Host "✓ SQL Executor node pool created" -ForegroundColor Green
Write-Host ""

# Create Database node pool
Write-Host "Creating Database node pool..." -ForegroundColor Yellow
az aks nodepool add `
  --resource-group $ResourceGroup `
  --cluster-name $ClusterName `
  --name database `
  --node-count 3 `
  --min-count 3 `
  --max-count 10 `
  --enable-cluster-autoscaler `
  --node-vm-size Standard_E8s_v3 `
  --zones 1 2 3 `
  --labels workload=database `
  --node-taints workload=database:NoSchedule `
  --output table

Write-Host "✓ Database node pool created" -ForegroundColor Green
Write-Host ""

# Install NGINX Ingress Controller
Write-Host "Installing NGINX Ingress Controller..." -ForegroundColor Yellow
helm repo add ingress-nginx https://kubernetes.github.io/ingress-nginx
helm repo update

helm install ingress-nginx ingress-nginx/ingress-nginx `
  --namespace ingress-nginx `
  --create-namespace `
  --set controller.replicaCount=3 `
  --set controller.nodeSelector."kubernetes\.io/os"=linux `
  --set controller.service.annotations."service\.beta\.kubernetes\.io/azure-load-balancer-health-probe-request-path"=/healthz `
  --set controller.service.externalTrafficPolicy=Local `
  --set controller.metrics.enabled=true `
  --set controller.podAnnotations."prometheus\.io/scrape"=true `
  --set controller.podAnnotations."prometheus\.io/port"=10254 `
  --wait

Write-Host "✓ NGINX Ingress Controller installed" -ForegroundColor Green
Write-Host ""

# Install cert-manager
Write-Host "Installing cert-manager..." -ForegroundColor Yellow
helm repo add jetstack https://charts.jetstack.io
helm repo update

helm install cert-manager jetstack/cert-manager `
  --namespace cert-manager `
  --create-namespace `
  --version v1.13.0 `
  --set installCRDs=true `
  --wait

Write-Host "✓ cert-manager installed" -ForegroundColor Green
Write-Host ""

# Wait for cert-manager to be ready
Write-Host "Waiting for cert-manager to be ready..." -ForegroundColor Yellow
kubectl wait --for=condition=available --timeout=300s deployment/cert-manager -n cert-manager
kubectl wait --for=condition=available --timeout=300s deployment/cert-manager-webhook -n cert-manager

# Create ClusterIssuer
Write-Host "Creating Let's Encrypt ClusterIssuer..." -ForegroundColor Yellow
@"
apiVersion: cert-manager.io/v1
kind: ClusterIssuer
metadata:
  name: letsencrypt-prod
spec:
  acme:
    server: https://acme-v02.api.letsencrypt.org/directory
    email: admin@example.com
    privateKeySecretRef:
      name: letsencrypt-prod
    solvers:
    - http01:
        ingress:
          class: nginx
"@ | kubectl apply -f -

Write-Host "✓ ClusterIssuer created" -ForegroundColor Green
Write-Host ""

# Get Load Balancer IP
Write-Host "Waiting for Load Balancer IP..." -ForegroundColor Yellow
$loadBalancerIp = ""
for ($i = 1; $i -le 30; $i++) {
    $loadBalancerIp = kubectl get service ingress-nginx-controller `
      -n ingress-nginx `
      -o jsonpath='{.status.loadBalancer.ingress[0].ip}' 2>$null
    
    if ($loadBalancerIp) {
        break
    }
    
    Write-Host "Waiting for Load Balancer IP... ($i/30)"
    Start-Sleep -Seconds 10
}

if (-not $loadBalancerIp) {
    Write-Host "Error: Failed to get Load Balancer IP" -ForegroundColor Red
    exit 1
}

Write-Host "✓ Load Balancer IP: $loadBalancerIp" -ForegroundColor Green
Write-Host ""

# Summary
Write-Host "========================================" -ForegroundColor Green
Write-Host "Cluster Provisioning Complete!" -ForegroundColor Green
Write-Host "========================================" -ForegroundColor Green
Write-Host ""
Write-Host "Cluster Information:" -ForegroundColor Yellow
Write-Host "  Resource Group: $ResourceGroup"
Write-Host "  Cluster Name: $ClusterName"
Write-Host "  Location: $Location"
Write-Host "  Load Balancer IP: $loadBalancerIp"
Write-Host ""
Write-Host "Next Steps:" -ForegroundColor Yellow
Write-Host "1. Configure DNS records:"
Write-Host "   platform.example.com -> $loadBalancerIp"
Write-Host "   api.platform.example.com -> $loadBalancerIp"
Write-Host "   ws.platform.example.com -> $loadBalancerIp"
Write-Host ""
Write-Host "2. Deploy application:"
Write-Host "   cd .."
Write-Host "   kubectl apply -f namespace.yaml"
Write-Host "   kubectl apply -f rbac.yaml"
Write-Host "   kubectl apply -f resource-quotas.yaml"
Write-Host "   kubectl apply -f limit-ranges.yaml"
Write-Host "   kubectl apply -f network-policies.yaml"
Write-Host "   kubectl apply -f redis-cluster.yaml"
Write-Host "   kubectl apply -f rabbitmq-deployment.yaml"
Write-Host "   kubectl apply -f production/hpa-all-services.yaml"
Write-Host "   kubectl apply -f production/ingress-production.yaml"
Write-Host ""
Write-Host "3. Verify deployment:"
Write-Host "   kubectl get nodes"
Write-Host "   kubectl get pods -n platform-saas-prod"
Write-Host "   kubectl get hpa -n platform-saas-prod"
Write-Host ""
Write-Host "For detailed instructions, see PRODUCTION_INFRASTRUCTURE_GUIDE.md" -ForegroundColor Green
