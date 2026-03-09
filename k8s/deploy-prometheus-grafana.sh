#!/bin/bash
# Deploy Prometheus and Grafana Monitoring Stack
# This script deploys Prometheus, AlertManager, and Grafana to Kubernetes

set -e

NAMESPACE="${1:-platform-saas}"
SKIP_NAMESPACE="${2:-false}"
DRY_RUN="${3:-false}"

# Colors
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
CYAN='\033[0;36m'
NC='\033[0m' # No Color

echo -e "${CYAN}========================================${NC}"
echo -e "${CYAN}Prometheus & Grafana Deployment Script${NC}"
echo -e "${CYAN}========================================${NC}"
echo ""

# Function to apply Kubernetes manifest
apply_manifest() {
    local file=$1
    local description=$2
    
    echo -e "${YELLOW}Deploying $description...${NC}"
    
    if [ "$DRY_RUN" = "true" ]; then
        echo -e "  ${NC}[DRY RUN] Would apply: $file${NC}"
        kubectl apply -f "$file" --dry-run=client
    else
        if kubectl apply -f "$file"; then
            echo -e "  ${GREEN}✓ $description deployed successfully${NC}"
        else
            echo -e "  ${RED}✗ Failed to deploy $description${NC}"
            exit 1
        fi
    fi
    echo ""
}

# Check if kubectl is available
echo -e "${YELLOW}Checking prerequisites...${NC}"
if ! command -v kubectl &> /dev/null; then
    echo -e "${RED}✗ kubectl is not installed or not in PATH${NC}"
    exit 1
fi
echo -e "${GREEN}✓ kubectl is available${NC}"
echo ""

# Check if namespace exists
if [ "$SKIP_NAMESPACE" != "true" ]; then
    echo -e "${YELLOW}Checking namespace...${NC}"
    if ! kubectl get namespace "$NAMESPACE" &> /dev/null; then
        echo -e "  ${YELLOW}Namespace '$NAMESPACE' does not exist. Creating...${NC}"
        if kubectl create namespace "$NAMESPACE"; then
            echo -e "  ${GREEN}✓ Namespace created${NC}"
        else
            echo -e "  ${RED}✗ Failed to create namespace${NC}"
            exit 1
        fi
    else
        echo -e "  ${GREEN}✓ Namespace '$NAMESPACE' exists${NC}"
    fi
    echo ""
fi

# Deploy Prometheus
echo -e "${CYAN}========================================${NC}"
echo -e "${CYAN}Deploying Prometheus${NC}"
echo -e "${CYAN}========================================${NC}"
echo ""

apply_manifest "prometheus-config.yaml" "Prometheus Configuration"
apply_manifest "prometheus-deployment.yaml" "Prometheus Deployment"

# Deploy AlertManager
echo -e "${CYAN}========================================${NC}"
echo -e "${CYAN}Deploying AlertManager${NC}"
echo -e "${CYAN}========================================${NC}"
echo ""

apply_manifest "alertmanager-config.yaml" "AlertManager Configuration"
apply_manifest "alertmanager-deployment.yaml" "AlertManager Deployment"

# Deploy Grafana
echo -e "${CYAN}========================================${NC}"
echo -e "${CYAN}Deploying Grafana${NC}"
echo -e "${CYAN}========================================${NC}"
echo ""

apply_manifest "grafana-config.yaml" "Grafana Configuration"
apply_manifest "grafana-deployment.yaml" "Grafana Deployment"

# Wait for deployments to be ready
if [ "$DRY_RUN" != "true" ]; then
    echo -e "${CYAN}========================================${NC}"
    echo -e "${CYAN}Waiting for Deployments${NC}"
    echo -e "${CYAN}========================================${NC}"
    echo ""
    
    echo -e "${YELLOW}Waiting for Prometheus to be ready...${NC}"
    kubectl wait --for=condition=available --timeout=300s deployment/prometheus -n "$NAMESPACE"
    echo -e "${GREEN}✓ Prometheus is ready${NC}"
    echo ""
    
    echo -e "${YELLOW}Waiting for AlertManager to be ready...${NC}"
    kubectl wait --for=condition=available --timeout=300s deployment/alertmanager -n "$NAMESPACE"
    echo -e "${GREEN}✓ AlertManager is ready${NC}"
    echo ""
    
    echo -e "${YELLOW}Waiting for Grafana to be ready...${NC}"
    kubectl wait --for=condition=available --timeout=300s deployment/grafana -n "$NAMESPACE"
    echo -e "${GREEN}✓ Grafana is ready${NC}"
    echo ""
fi

# Display service information
echo -e "${CYAN}========================================${NC}"
echo -e "${CYAN}Deployment Summary${NC}"
echo -e "${CYAN}========================================${NC}"
echo ""

if [ "$DRY_RUN" != "true" ]; then
    echo -e "${YELLOW}Services:${NC}"
    kubectl get services -n "$NAMESPACE" -l component=monitoring
    echo ""
    
    echo -e "${YELLOW}Pods:${NC}"
    kubectl get pods -n "$NAMESPACE" -l component=monitoring
    echo ""
    
    echo -e "${YELLOW}PersistentVolumeClaims:${NC}"
    kubectl get pvc -n "$NAMESPACE" | grep -E "prometheus|alertmanager|grafana"
    echo ""
fi

# Display access information
echo -e "${CYAN}========================================${NC}"
echo -e "${CYAN}Access Information${NC}"
echo -e "${CYAN}========================================${NC}"
echo ""

echo -e "${YELLOW}To access Prometheus:${NC}"
echo -e "  kubectl port-forward -n $NAMESPACE svc/prometheus 9090:9090"
echo -e "  Then open: http://localhost:9090"
echo ""

echo -e "${YELLOW}To access AlertManager:${NC}"
echo -e "  kubectl port-forward -n $NAMESPACE svc/alertmanager 9093:9093"
echo -e "  Then open: http://localhost:9093"
echo ""

echo -e "${YELLOW}To access Grafana:${NC}"
echo -e "  kubectl port-forward -n $NAMESPACE svc/grafana 3000:3000"
echo -e "  Then open: http://localhost:3000"
echo -e "  Default credentials: admin / (check grafana-secrets)"
echo ""

echo -e "${CYAN}========================================${NC}"
echo -e "${CYAN}Important Notes${NC}"
echo -e "${CYAN}========================================${NC}"
echo ""

echo -e "${YELLOW}1. Update AlertManager secrets:${NC}"
echo -e "   - Edit k8s/alertmanager-config.yaml"
echo -e "   - Replace SMTP password and Slack webhook URL"
echo -e "   - Reapply: kubectl apply -f alertmanager-config.yaml"
echo ""

echo -e "${YELLOW}2. Update Grafana admin password:${NC}"
echo -e "   - Edit k8s/grafana-deployment.yaml"
echo -e "   - Replace REPLACE_WITH_SECURE_PASSWORD"
echo -e "   - Reapply: kubectl apply -f grafana-deployment.yaml"
echo ""

echo -e "${YELLOW}3. Configure microservice metrics endpoints:${NC}"
echo -e "   - Add prometheus.io/scrape: 'true' annotation to pods"
echo -e "   - Add prometheus.io/port: '<port>' annotation"
echo -e "   - Add prometheus.io/path: '/metrics' annotation"
echo ""

echo -e "${YELLOW}4. Pre-configured Grafana dashboards:${NC}"
echo -e "   - System Overview (CPU, Memory, Disk, Network)"
echo -e "   - API Metrics (Request Rate, Error Rate, Response Time)"
echo -e "   - Database Metrics (Connection Pool, Query Performance)"
echo -e "   - Message Queue Metrics (Queue Length, Processing Rate)"
echo -e "   - Microservices Overview (Pod Status, Resource Usage)"
echo ""

echo -e "${CYAN}========================================${NC}"
echo -e "${GREEN}Deployment Complete!${NC}"
echo -e "${CYAN}========================================${NC}"
