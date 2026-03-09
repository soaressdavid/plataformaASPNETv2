#!/bin/bash

# ASP.NET Core Learning Platform - Kubernetes Cleanup Script
# This script removes all deployed components from the Kubernetes cluster

set -e

NAMESPACE="aspnet-learning-platform"

echo "=========================================="
echo "ASP.NET Core Learning Platform Cleanup"
echo "=========================================="
echo ""

# Color codes for output
RED='\033[0;31m'
YELLOW='\033[1;33m'
NC='\033[0m' # No Color

print_warning() {
    echo -e "${YELLOW}[!]${NC} $1"
}

print_error() {
    echo -e "${RED}[✗]${NC} $1"
}

# Warning
print_warning "This will delete ALL resources in the $NAMESPACE namespace!"
print_warning "This action cannot be undone!"
echo ""
read -p "Are you sure you want to continue? (type 'yes' to confirm): " confirm

if [ "$confirm" != "yes" ]; then
    echo "Cleanup cancelled."
    exit 0
fi

echo ""
echo "Starting cleanup..."

# Check if namespace exists
if ! kubectl get namespace $NAMESPACE &> /dev/null; then
    print_error "Namespace $NAMESPACE does not exist. Nothing to clean up."
    exit 0
fi

# Option 1: Delete entire namespace (fastest)
echo ""
read -p "Delete entire namespace? This is the fastest method. (yes/no): " delete_namespace

if [ "$delete_namespace" == "yes" ]; then
    echo "Deleting namespace $NAMESPACE..."
    kubectl delete namespace $NAMESPACE
    echo "Cleanup complete!"
    exit 0
fi

# Option 2: Delete resources individually
echo ""
echo "Deleting resources individually..."

echo "  - Deleting Ingress..."
kubectl delete -f ingress.yaml --ignore-not-found=true

echo "  - Deleting Frontend..."
kubectl delete -f frontend-deployment.yaml --ignore-not-found=true

echo "  - Deleting API Gateway..."
kubectl delete -f api-gateway-deployment.yaml --ignore-not-found=true

echo "  - Deleting Workers..."
kubectl delete -f worker-hpa.yaml --ignore-not-found=true
kubectl delete -f worker-deployment.yaml --ignore-not-found=true

echo "  - Deleting Microservices..."
kubectl delete -f execution-service-deployment.yaml --ignore-not-found=true
kubectl delete -f aitutor-service-deployment.yaml --ignore-not-found=true
kubectl delete -f progress-service-deployment.yaml --ignore-not-found=true
kubectl delete -f challenge-service-deployment.yaml --ignore-not-found=true
kubectl delete -f course-service-deployment.yaml --ignore-not-found=true
kubectl delete -f auth-service-deployment.yaml --ignore-not-found=true

echo "  - Deleting Infrastructure..."
kubectl delete -f rabbitmq-deployment.yaml --ignore-not-found=true
kubectl delete -f redis-deployment.yaml --ignore-not-found=true
kubectl delete -f postgres-deployment.yaml --ignore-not-found=true

echo "  - Deleting Configuration..."
kubectl delete -f configmap.yaml --ignore-not-found=true
kubectl delete -f secrets.yaml --ignore-not-found=true

echo ""
read -p "Delete namespace $NAMESPACE? (yes/no): " delete_ns_final
if [ "$delete_ns_final" == "yes" ]; then
    kubectl delete -f namespace.yaml --ignore-not-found=true
    echo "Namespace deleted."
else
    echo "Namespace preserved."
fi

echo ""
echo "=========================================="
echo "Cleanup complete!"
echo "=========================================="
