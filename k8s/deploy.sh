#!/bin/bash

# ASP.NET Core Learning Platform - Kubernetes Deployment Script
# This script deploys all components to a Kubernetes cluster

set -e

NAMESPACE="aspnet-learning-platform"
TIMEOUT="120s"

echo "=========================================="
echo "ASP.NET Core Learning Platform Deployment"
echo "=========================================="
echo ""

# Color codes for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
NC='\033[0m' # No Color

# Function to print colored output
print_status() {
    echo -e "${GREEN}[✓]${NC} $1"
}

print_error() {
    echo -e "${RED}[✗]${NC} $1"
}

print_warning() {
    echo -e "${YELLOW}[!]${NC} $1"
}

# Check if kubectl is installed
if ! command -v kubectl &> /dev/null; then
    print_error "kubectl is not installed. Please install kubectl first."
    exit 1
fi

# Check if cluster is accessible
if ! kubectl cluster-info &> /dev/null; then
    print_error "Cannot connect to Kubernetes cluster. Please check your kubeconfig."
    exit 1
fi

print_status "Connected to Kubernetes cluster"

# Step 1: Create Namespace
echo ""
echo "Step 1: Creating namespace..."
kubectl apply -f namespace.yaml
print_status "Namespace created"

# Step 2: Create Secrets and ConfigMap
echo ""
echo "Step 2: Creating secrets and configuration..."
print_warning "Make sure you've updated secrets.yaml with production values!"
read -p "Have you updated the secrets? (yes/no): " confirm
if [ "$confirm" != "yes" ]; then
    print_error "Please update secrets.yaml before deploying"
    exit 1
fi

kubectl apply -f secrets.yaml
kubectl apply -f configmap.yaml
print_status "Secrets and ConfigMap created"

# Step 3: Deploy Infrastructure
echo ""
echo "Step 3: Deploying infrastructure (PostgreSQL, Redis, RabbitMQ)..."

echo "  - Deploying PostgreSQL..."
kubectl apply -f postgres-deployment.yaml
kubectl wait --for=condition=ready pod -l app=postgres -n $NAMESPACE --timeout=$TIMEOUT
print_status "PostgreSQL is ready"

echo "  - Deploying Redis..."
kubectl apply -f redis-deployment.yaml
kubectl wait --for=condition=ready pod -l app=redis -n $NAMESPACE --timeout=$TIMEOUT
print_status "Redis is ready"

echo "  - Deploying RabbitMQ..."
kubectl apply -f rabbitmq-deployment.yaml
kubectl wait --for=condition=ready pod -l app=rabbitmq -n $NAMESPACE --timeout=$TIMEOUT
print_status "RabbitMQ is ready"

# Step 4: Run Database Migrations
echo ""
echo "Step 4: Database migrations..."
print_warning "You may need to run database migrations manually:"
echo "  kubectl exec -it deployment/auth-service -n $NAMESPACE -- dotnet ef database update"
read -p "Press Enter to continue..."

# Step 5: Deploy Microservices
echo ""
echo "Step 5: Deploying microservices..."

services=("auth-service" "course-service" "challenge-service" "progress-service" "aitutor-service" "execution-service")

for service in "${services[@]}"; do
    echo "  - Deploying $service..."
    kubectl apply -f ${service}-deployment.yaml
done

# Wait for services to be ready
echo "  - Waiting for services to be ready..."
for service in "${services[@]}"; do
    kubectl wait --for=condition=ready pod -l app=$service -n $NAMESPACE --timeout=$TIMEOUT || true
done
print_status "Microservices deployed"

# Step 6: Deploy Workers
echo ""
echo "Step 6: Deploying workers with autoscaling..."
kubectl apply -f worker-deployment.yaml
kubectl apply -f worker-hpa.yaml
kubectl wait --for=condition=ready pod -l app=worker -n $NAMESPACE --timeout=$TIMEOUT || true
print_status "Workers deployed with HPA"

# Step 7: Deploy API Gateway
echo ""
echo "Step 7: Deploying API Gateway..."
kubectl apply -f api-gateway-deployment.yaml
kubectl wait --for=condition=ready pod -l app=api-gateway -n $NAMESPACE --timeout=$TIMEOUT
print_status "API Gateway deployed"

# Step 8: Deploy Frontend
echo ""
echo "Step 8: Deploying Frontend..."
kubectl apply -f frontend-deployment.yaml
kubectl wait --for=condition=ready pod -l app=frontend -n $NAMESPACE --timeout=$TIMEOUT
print_status "Frontend deployed"

# Step 9: Deploy Ingress
echo ""
echo "Step 9: Deploying Ingress..."
print_warning "Make sure you've updated ingress.yaml with your domain name!"
read -p "Have you updated the ingress hostname? (yes/no): " confirm_ingress
if [ "$confirm_ingress" == "yes" ]; then
    kubectl apply -f ingress.yaml
    print_status "Ingress deployed"
else
    print_warning "Skipping ingress deployment. You can deploy it later with: kubectl apply -f ingress.yaml"
fi

# Summary
echo ""
echo "=========================================="
echo "Deployment Summary"
echo "=========================================="
echo ""

echo "Checking pod status..."
kubectl get pods -n $NAMESPACE

echo ""
echo "Checking services..."
kubectl get services -n $NAMESPACE

echo ""
echo "Checking HPA..."
kubectl get hpa -n $NAMESPACE

if [ "$confirm_ingress" == "yes" ]; then
    echo ""
    echo "Checking ingress..."
    kubectl get ingress -n $NAMESPACE
fi

echo ""
echo "=========================================="
print_status "Deployment completed!"
echo "=========================================="
echo ""
echo "Next steps:"
echo "1. Run database migrations if not done already"
echo "2. Configure DNS to point to your ingress controller"
echo "3. Monitor logs: kubectl logs -f deployment/api-gateway -n $NAMESPACE"
echo "4. Access via port-forward for testing:"
echo "   kubectl port-forward service/frontend-service 3000:3000 -n $NAMESPACE"
echo ""
