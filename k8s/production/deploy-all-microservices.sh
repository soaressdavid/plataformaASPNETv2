#!/bin/bash

# Deploy All Microservices to Production Kubernetes Cluster
# This script deploys all microservices for the ASP.NET Learning Platform

set -e

echo "=========================================="
echo "Deploying All Microservices to Production"
echo "=========================================="

# Color codes for output
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
RED='\033[0;31m'
NC='\033[0m' # No Color

# Check if kubectl is installed
if ! command -v kubectl &> /dev/null; then
    echo -e "${RED}Error: kubectl is not installed${NC}"
    exit 1
fi

# Check if we're connected to a cluster
if ! kubectl cluster-info &> /dev/null; then
    echo -e "${RED}Error: Not connected to a Kubernetes cluster${NC}"
    exit 1
fi

# Get current context
CONTEXT=$(kubectl config current-context)
echo -e "${YELLOW}Current context: ${CONTEXT}${NC}"
echo ""

# Confirm deployment
read -p "Are you sure you want to deploy to this cluster? (yes/no): " CONFIRM
if [ "$CONFIRM" != "yes" ]; then
    echo "Deployment cancelled"
    exit 0
fi

echo ""
echo "=========================================="
echo "Step 1: Ensure namespace exists"
echo "=========================================="
kubectl apply -f ../namespace.yaml
echo -e "${GREEN}✓ Namespace configured${NC}"
echo ""

echo "=========================================="
echo "Step 2: Apply ConfigMaps and Secrets"
echo "=========================================="
kubectl apply -f ../configmap.yaml
echo -e "${GREEN}✓ ConfigMap applied${NC}"

# Check if secrets exist, if not warn user
if ! kubectl get secret platform-secrets -n aspnet-learning-platform &> /dev/null; then
    echo -e "${YELLOW}Warning: platform-secrets not found. Please create secrets before proceeding.${NC}"
    echo "Required secrets:"
    echo "  - JWT_SECRET"
    echo "  - SQL_CONNECTION_STRING"
    echo "  - GROQ_API_KEY"
    echo "  - SENDGRID_API_KEY"
    echo ""
    read -p "Continue anyway? (yes/no): " CONTINUE
    if [ "$CONTINUE" != "yes" ]; then
        exit 1
    fi
else
    echo -e "${GREEN}✓ Secrets verified${NC}"
fi
echo ""

echo "=========================================="
echo "Step 3: Deploy Core Infrastructure"
echo "=========================================="

# Deploy Redis
echo "Deploying Redis cluster..."
kubectl apply -f ../redis-cluster.yaml
echo -e "${GREEN}✓ Redis cluster deployed${NC}"

# Deploy RabbitMQ
echo "Deploying RabbitMQ..."
kubectl apply -f ../rabbitmq-deployment.yaml
echo -e "${GREEN}✓ RabbitMQ deployed${NC}"

# Wait for infrastructure to be ready
echo "Waiting for infrastructure to be ready..."
kubectl wait --for=condition=ready pod -l app=redis-cluster -n aspnet-learning-platform --timeout=120s || true
kubectl wait --for=condition=ready pod -l app=rabbitmq -n aspnet-learning-platform --timeout=120s || true
echo ""

echo "=========================================="
echo "Step 4: Deploy Microservices"
echo "=========================================="

# Array of microservices to deploy
SERVICES=(
    "api-gateway"
    "auth-service"
    "course-service"
    "challenge-service"
    "execution-service"
    "sqlexecutor-service"
    "gamification-service"
    "aitutor-service"
    "notification-service"
    "analytics-service"
    "progress-service"
)

# Deploy each service
for SERVICE in "${SERVICES[@]}"; do
    echo "Deploying ${SERVICE}..."
    kubectl apply -f ../${SERVICE}-deployment.yaml
    echo -e "${GREEN}✓ ${SERVICE} deployed${NC}"
done

echo ""
echo "=========================================="
echo "Step 5: Deploy Production Configuration"
echo "=========================================="

# Deploy Horizontal Pod Autoscalers
echo "Deploying HPA for all services..."
kubectl apply -f hpa-all-services.yaml
echo -e "${GREEN}✓ HPA configured${NC}"

# Deploy Cluster Autoscaler
echo "Deploying Cluster Autoscaler..."
kubectl apply -f cluster-autoscaler.yaml
echo -e "${GREEN}✓ Cluster Autoscaler deployed${NC}"

# Deploy Production Ingress
echo "Deploying Production Ingress..."
kubectl apply -f ingress-production.yaml
echo -e "${GREEN}✓ Production Ingress configured${NC}"

echo ""
echo "=========================================="
echo "Step 6: Verify Deployments"
echo "=========================================="

echo "Waiting for deployments to be ready..."
for SERVICE in "${SERVICES[@]}"; do
    echo "Checking ${SERVICE}..."
    kubectl rollout status deployment/${SERVICE} -n aspnet-learning-platform --timeout=180s || echo -e "${YELLOW}Warning: ${SERVICE} not ready yet${NC}"
done

echo ""
echo "=========================================="
echo "Deployment Summary"
echo "=========================================="

# Get deployment status
echo "Deployment Status:"
kubectl get deployments -n aspnet-learning-platform

echo ""
echo "Pod Status:"
kubectl get pods -n aspnet-learning-platform

echo ""
echo "Service Status:"
kubectl get services -n aspnet-learning-platform

echo ""
echo "Ingress Status:"
kubectl get ingress -n aspnet-learning-platform

echo ""
echo "=========================================="
echo "Deployment Complete!"
echo "=========================================="
echo ""
echo "Next steps:"
echo "1. Verify all pods are running: kubectl get pods -n aspnet-learning-platform"
echo "2. Check logs if any issues: kubectl logs -f deployment/<service-name> -n aspnet-learning-platform"
echo "3. Test API Gateway health: curl http://<ingress-ip>/health"
echo "4. Monitor with: kubectl top pods -n aspnet-learning-platform"
echo ""
echo -e "${GREEN}All microservices deployed successfully!${NC}"
