# Kubernetes Cluster Setup Script (PowerShell)
# This script sets up the complete Kubernetes cluster configuration
# for the Platform Evolution SaaS project

$ErrorActionPreference = "Stop"

Write-Host "==========================================" -ForegroundColor Cyan
Write-Host "Platform SaaS - Kubernetes Cluster Setup" -ForegroundColor Cyan
Write-Host "==========================================" -ForegroundColor Cyan
Write-Host ""

function Write-Success {
    param([string]$Message)
    Write-Host "✓ $Message" -ForegroundColor Green
}

function Write-Error-Custom {
    param([string]$Message)
    Write-Host "✗ $Message" -ForegroundColor Red
}

function Write-Info {
    param([string]$Message)
    Write-Host "→ $Message" -ForegroundColor Yellow
}

# Check if kubectl is installed
try {
    $null = kubectl version --client 2>$null
    Write-Success "kubectl is installed"
} catch {
    Write-Error-Custom "kubectl is not installed. Please install kubectl first."
    exit 1
}

# Check if cluster is accessible
try {
    $null = kubectl cluster-info 2>$null
    Write-Success "Connected to Kubernetes cluster"
} catch {
    Write-Error-Custom "Cannot connect to Kubernetes cluster. Please configure kubectl."
    exit 1
}

Write-Host ""

# Prompt for environment
Write-Host "Select environment to setup:"
Write-Host "1) Development"
Write-Host "2) Staging"
Write-Host "3) Production"
Write-Host "4) All environments"
$envChoice = Read-Host "Enter choice [1-4]"

$environments = @()
switch ($envChoice) {
    "1" { $environments = @("dev") }
    "2" { $environments = @("staging") }
    "3" { $environments = @("prod") }
    "4" { $environments = @("dev", "staging", "prod") }
    default {
        Write-Error-Custom "Invalid choice"
        exit 1
    }
}

Write-Host ""
Write-Info "Setting up environments: $($environments -join ', ')"
Write-Host ""

# Step 1: Create Namespaces
Write-Info "Step 1: Creating namespaces..."
kubectl apply -f namespace.yaml
Write-Success "Namespaces created"
Write-Host ""

# Step 2: Apply Resource Quotas
Write-Info "Step 2: Applying resource quotas..."
kubectl apply -f resource-quotas.yaml
Write-Success "Resource quotas applied"
Write-Host ""

# Step 3: Apply Limit Ranges
Write-Info "Step 3: Applying limit ranges..."
kubectl apply -f limit-ranges.yaml
Write-Success "Limit ranges applied"
Write-Host ""

# Step 4: Setup RBAC
Write-Info "Step 4: Setting up RBAC policies..."
foreach ($env in $environments) {
    if ($env -eq "prod") {
        kubectl apply -f rbac.yaml
        Write-Success "Production RBAC policies applied"
    } elseif ($env -eq "staging") {
        kubectl apply -f rbac-staging.yaml
        Write-Success "Staging RBAC policies applied"
    } elseif ($env -eq "dev") {
        kubectl apply -f rbac-dev.yaml
        Write-Success "Development RBAC policies applied"
    }
}
Write-Host ""

# Step 5: Apply Network Policies (production only)
if ($environments -contains "prod") {
    Write-Info "Step 5: Applying network policies (production)..."
    kubectl apply -f network-policies.yaml
    Write-Success "Network policies applied"
    Write-Host ""
}

# Step 6: Apply Pod Security Policies (production only)
if ($environments -contains "prod") {
    Write-Info "Step 6: Applying pod security policies (production)..."
    kubectl apply -f pod-security-policies.yaml
    Write-Success "Pod security policies applied"
    Write-Host ""
}

# Verification
Write-Host ""
Write-Info "Verifying configuration..."
Write-Host ""

foreach ($env in $environments) {
    $namespace = "platform-saas-$env"
    
    Write-Host "Environment: $env ($namespace)" -ForegroundColor Cyan
    Write-Host "-----------------------------------"
    
    # Check namespace
    try {
        $null = kubectl get namespace $namespace 2>$null
        Write-Success "Namespace exists"
    } catch {
        Write-Error-Custom "Namespace not found"
    }
    
    # Check resource quota
    try {
        $quotaCount = (kubectl get resourcequota -n $namespace --no-headers 2>$null | Measure-Object).Count
        Write-Success "Resource quota configured ($quotaCount)"
    } catch {
        Write-Error-Custom "Resource quota not found"
    }
    
    # Check limit range
    try {
        $limitCount = (kubectl get limitrange -n $namespace --no-headers 2>$null | Measure-Object).Count
        Write-Success "Limit range configured ($limitCount)"
    } catch {
        Write-Error-Custom "Limit range not found"
    }
    
    # Check service accounts
    $saCount = (kubectl get serviceaccounts -n $namespace --no-headers 2>$null | Measure-Object).Count
    Write-Success "Service accounts created ($saCount)"
    
    # Check roles
    $roleCount = (kubectl get roles -n $namespace --no-headers 2>$null | Measure-Object).Count
    Write-Success "Roles configured ($roleCount)"
    
    # Check rolebindings
    $rbCount = (kubectl get rolebindings -n $namespace --no-headers 2>$null | Measure-Object).Count
    Write-Success "RoleBindings configured ($rbCount)"
    
    if ($env -eq "prod") {
        # Check network policies
        $npCount = (kubectl get networkpolicies -n $namespace --no-headers 2>$null | Measure-Object).Count
        Write-Success "Network policies configured ($npCount)"
    }
    
    Write-Host ""
}

# Display resource quotas
Write-Host ""
Write-Info "Resource Quota Summary:"
Write-Host ""
foreach ($env in $environments) {
    $namespace = "platform-saas-$env"
    Write-Host "=== $env ===" -ForegroundColor Cyan
    kubectl describe resourcequota -n $namespace 2>$null
    Write-Host ""
}

# Display cluster info
Write-Host ""
Write-Info "Cluster Information:"
kubectl cluster-info
Write-Host ""

# Success message
Write-Host ""
Write-Host "==========================================" -ForegroundColor Cyan
Write-Success "Cluster setup completed successfully!"
Write-Host "==========================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "Next steps:"
Write-Host "1. Deploy infrastructure components (Redis, RabbitMQ, SQL Server)"
Write-Host "2. Deploy microservices"
Write-Host "3. Configure monitoring and logging"
Write-Host "4. Run smoke tests"
Write-Host ""
Write-Host "For detailed information, see CLUSTER_SETUP.md"
Write-Host ""
