#!/bin/bash

# Kubernetes Cluster Setup Script
# This script sets up the complete Kubernetes cluster configuration
# for the Platform Evolution SaaS project

set -e

echo "=========================================="
echo "Platform SaaS - Kubernetes Cluster Setup"
echo "=========================================="
echo ""

# Color codes for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
NC='\033[0m' # No Color

# Function to print colored output
print_success() {
    echo -e "${GREEN}✓ $1${NC}"
}

print_error() {
    echo -e "${RED}✗ $1${NC}"
}

print_info() {
    echo -e "${YELLOW}→ $1${NC}"
}

# Check if kubectl is installed
if ! command -v kubectl &> /dev/null; then
    print_error "kubectl is not installed. Please install kubectl first."
    exit 1
fi

print_success "kubectl is installed"

# Check if cluster is accessible
if ! kubectl cluster-info &> /dev/null; then
    print_error "Cannot connect to Kubernetes cluster. Please configure kubectl."
    exit 1
fi

print_success "Connected to Kubernetes cluster"
echo ""

# Prompt for environment
echo "Select environment to setup:"
echo "1) Development"
echo "2) Staging"
echo "3) Production"
echo "4) All environments"
read -p "Enter choice [1-4]: " env_choice

case $env_choice in
    1)
        ENVIRONMENTS=("dev")
        ;;
    2)
        ENVIRONMENTS=("staging")
        ;;
    3)
        ENVIRONMENTS=("prod")
        ;;
    4)
        ENVIRONMENTS=("dev" "staging" "prod")
        ;;
    *)
        print_error "Invalid choice"
        exit 1
        ;;
esac

echo ""
print_info "Setting up environments: ${ENVIRONMENTS[*]}"
echo ""

# Step 1: Create Namespaces
print_info "Step 1: Creating namespaces..."
kubectl apply -f namespace.yaml
print_success "Namespaces created"
echo ""

# Step 2: Apply Resource Quotas
print_info "Step 2: Applying resource quotas..."
kubectl apply -f resource-quotas.yaml
print_success "Resource quotas applied"
echo ""

# Step 3: Apply Limit Ranges
print_info "Step 3: Applying limit ranges..."
kubectl apply -f limit-ranges.yaml
print_success "Limit ranges applied"
echo ""

# Step 4: Setup RBAC
print_info "Step 4: Setting up RBAC policies..."
for env in "${ENVIRONMENTS[@]}"; do
    if [ "$env" = "prod" ]; then
        kubectl apply -f rbac.yaml
        print_success "Production RBAC policies applied"
    elif [ "$env" = "staging" ]; then
        kubectl apply -f rbac-staging.yaml
        print_success "Staging RBAC policies applied"
    elif [ "$env" = "dev" ]; then
        kubectl apply -f rbac-dev.yaml
        print_success "Development RBAC policies applied"
    fi
done
echo ""

# Step 5: Apply Network Policies (production only)
if [[ " ${ENVIRONMENTS[@]} " =~ " prod " ]]; then
    print_info "Step 5: Applying network policies (production)..."
    kubectl apply -f network-policies.yaml
    print_success "Network policies applied"
    echo ""
fi

# Step 6: Apply Pod Security Policies (production only)
if [[ " ${ENVIRONMENTS[@]} " =~ " prod " ]]; then
    print_info "Step 6: Applying pod security policies (production)..."
    kubectl apply -f pod-security-policies.yaml
    print_success "Pod security policies applied"
    echo ""
fi

# Verification
echo ""
print_info "Verifying configuration..."
echo ""

for env in "${ENVIRONMENTS[@]}"; do
    namespace="platform-saas-$env"
    
    echo "Environment: $env ($namespace)"
    echo "-----------------------------------"
    
    # Check namespace
    if kubectl get namespace $namespace &> /dev/null; then
        print_success "Namespace exists"
    else
        print_error "Namespace not found"
    fi
    
    # Check resource quota
    if kubectl get resourcequota -n $namespace &> /dev/null; then
        quota_count=$(kubectl get resourcequota -n $namespace --no-headers | wc -l)
        print_success "Resource quota configured ($quota_count)"
    else
        print_error "Resource quota not found"
    fi
    
    # Check limit range
    if kubectl get limitrange -n $namespace &> /dev/null; then
        limit_count=$(kubectl get limitrange -n $namespace --no-headers | wc -l)
        print_success "Limit range configured ($limit_count)"
    else
        print_error "Limit range not found"
    fi
    
    # Check service accounts
    sa_count=$(kubectl get serviceaccounts -n $namespace --no-headers | wc -l)
    print_success "Service accounts created ($sa_count)"
    
    # Check roles
    role_count=$(kubectl get roles -n $namespace --no-headers 2>/dev/null | wc -l)
    print_success "Roles configured ($role_count)"
    
    # Check rolebindings
    rb_count=$(kubectl get rolebindings -n $namespace --no-headers 2>/dev/null | wc -l)
    print_success "RoleBindings configured ($rb_count)"
    
    if [ "$env" = "prod" ]; then
        # Check network policies
        np_count=$(kubectl get networkpolicies -n $namespace --no-headers 2>/dev/null | wc -l)
        print_success "Network policies configured ($np_count)"
    fi
    
    echo ""
done

# Display resource quotas
echo ""
print_info "Resource Quota Summary:"
echo ""
for env in "${ENVIRONMENTS[@]}"; do
    namespace="platform-saas-$env"
    echo "=== $env ==="
    kubectl describe resourcequota -n $namespace 2>/dev/null | grep -A 10 "Resource" || echo "No quota found"
    echo ""
done

# Display cluster info
echo ""
print_info "Cluster Information:"
kubectl cluster-info
echo ""

# Success message
echo ""
echo "=========================================="
print_success "Cluster setup completed successfully!"
echo "=========================================="
echo ""
echo "Next steps:"
echo "1. Deploy infrastructure components (Redis, RabbitMQ, SQL Server)"
echo "2. Deploy microservices"
echo "3. Configure monitoring and logging"
echo "4. Run smoke tests"
echo ""
echo "For detailed information, see CLUSTER_SETUP.md"
echo ""
