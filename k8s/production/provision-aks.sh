#!/bin/bash
# Quick provisioning script for Azure Kubernetes Service (AKS)
# Requirements: 50.1, 50.13 - Support 10,000 concurrent users with auto-scaling

set -e

# Configuration
RESOURCE_GROUP="${RESOURCE_GROUP:-platform-saas-prod-rg}"
LOCATION="${LOCATION:-eastus}"
CLUSTER_NAME="${CLUSTER_NAME:-platform-saas-prod-aks}"
SUBSCRIPTION_ID="${SUBSCRIPTION_ID:-}"

# Colors for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
NC='\033[0m' # No Color

echo -e "${GREEN}========================================${NC}"
echo -e "${GREEN}AKS Production Cluster Provisioning${NC}"
echo -e "${GREEN}========================================${NC}"
echo ""

# Check prerequisites
echo -e "${YELLOW}Checking prerequisites...${NC}"

if ! command -v az &> /dev/null; then
    echo -e "${RED}Error: Azure CLI not found. Please install it first.${NC}"
    exit 1
fi

if ! command -v kubectl &> /dev/null; then
    echo -e "${RED}Error: kubectl not found. Please install it first.${NC}"
    exit 1
fi

if ! command -v helm &> /dev/null; then
    echo -e "${RED}Error: Helm not found. Please install it first.${NC}"
    exit 1
fi

echo -e "${GREEN}✓ All prerequisites met${NC}"
echo ""

# Login to Azure
echo -e "${YELLOW}Logging in to Azure...${NC}"
az login

if [ -n "$SUBSCRIPTION_ID" ]; then
    az account set --subscription "$SUBSCRIPTION_ID"
fi

CURRENT_SUBSCRIPTION=$(az account show --query name -o tsv)
echo -e "${GREEN}✓ Using subscription: $CURRENT_SUBSCRIPTION${NC}"
echo ""

# Create resource group
echo -e "${YELLOW}Creating resource group...${NC}"
az group create \
  --name "$RESOURCE_GROUP" \
  --location "$LOCATION" \
  --output table

echo -e "${GREEN}✓ Resource group created${NC}"
echo ""

# Create Log Analytics workspace for monitoring
echo -e "${YELLOW}Creating Log Analytics workspace...${NC}"
WORKSPACE_ID=$(az monitor log-analytics workspace create \
  --resource-group "$RESOURCE_GROUP" \
  --workspace-name "platform-saas-logs" \
  --location "$LOCATION" \
  --query id -o tsv)

echo -e "${GREEN}✓ Log Analytics workspace created${NC}"
echo ""

# Create AKS cluster
echo -e "${YELLOW}Creating AKS cluster (this may take 10-15 minutes)...${NC}"
az aks create \
  --resource-group "$RESOURCE_GROUP" \
  --name "$CLUSTER_NAME" \
  --location "$LOCATION" \
  --node-count 3 \
  --min-count 3 \
  --max-count 50 \
  --enable-cluster-autoscaler \
  --node-vm-size Standard_D4s_v3 \
  --zones 1 2 3 \
  --enable-managed-identity \
  --network-plugin azure \
  --network-policy azure \
  --load-balancer-sku standard \
  --enable-addons monitoring \
  --workspace-resource-id "$WORKSPACE_ID" \
  --generate-ssh-keys \
  --kubernetes-version 1.28.3 \
  --output table

echo -e "${GREEN}✓ AKS cluster created${NC}"
echo ""

# Get credentials
echo -e "${YELLOW}Getting cluster credentials...${NC}"
az aks get-credentials \
  --resource-group "$RESOURCE_GROUP" \
  --name "$CLUSTER_NAME" \
  --overwrite-existing

echo -e "${GREEN}✓ Credentials configured${NC}"
echo ""

# Create Code Executor node pool
echo -e "${YELLOW}Creating Code Executor node pool...${NC}"
az aks nodepool add \
  --resource-group "$RESOURCE_GROUP" \
  --cluster-name "$CLUSTER_NAME" \
  --name codeexec \
  --node-count 5 \
  --min-count 5 \
  --max-count 30 \
  --enable-cluster-autoscaler \
  --node-vm-size Standard_F8s_v2 \
  --zones 1 2 3 \
  --labels workload=code-executor \
  --node-taints workload=code-executor:NoSchedule \
  --output table

echo -e "${GREEN}✓ Code Executor node pool created${NC}"
echo ""

# Create SQL Executor node pool
echo -e "${YELLOW}Creating SQL Executor node pool...${NC}"
az aks nodepool add \
  --resource-group "$RESOURCE_GROUP" \
  --cluster-name "$CLUSTER_NAME" \
  --name sqlexec \
  --node-count 3 \
  --min-count 3 \
  --max-count 20 \
  --enable-cluster-autoscaler \
  --node-vm-size Standard_D4s_v3 \
  --zones 1 2 3 \
  --labels workload=sql-executor \
  --node-taints workload=sql-executor:NoSchedule \
  --output table

echo -e "${GREEN}✓ SQL Executor node pool created${NC}"
echo ""

# Create Database node pool
echo -e "${YELLOW}Creating Database node pool...${NC}"
az aks nodepool add \
  --resource-group "$RESOURCE_GROUP" \
  --cluster-name "$CLUSTER_NAME" \
  --name database \
  --node-count 3 \
  --min-count 3 \
  --max-count 10 \
  --enable-cluster-autoscaler \
  --node-vm-size Standard_E8s_v3 \
  --zones 1 2 3 \
  --labels workload=database \
  --node-taints workload=database:NoSchedule \
  --output table

echo -e "${GREEN}✓ Database node pool created${NC}"
echo ""

# Install NGINX Ingress Controller
echo -e "${YELLOW}Installing NGINX Ingress Controller...${NC}"
helm repo add ingress-nginx https://kubernetes.github.io/ingress-nginx
helm repo update

helm install ingress-nginx ingress-nginx/ingress-nginx \
  --namespace ingress-nginx \
  --create-namespace \
  --set controller.replicaCount=3 \
  --set controller.nodeSelector."kubernetes\.io/os"=linux \
  --set controller.service.annotations."service\.beta\.kubernetes\.io/azure-load-balancer-health-probe-request-path"=/healthz \
  --set controller.service.externalTrafficPolicy=Local \
  --set controller.metrics.enabled=true \
  --set controller.podAnnotations."prometheus\.io/scrape"=true \
  --set controller.podAnnotations."prometheus\.io/port"=10254 \
  --wait

echo -e "${GREEN}✓ NGINX Ingress Controller installed${NC}"
echo ""

# Install cert-manager
echo -e "${YELLOW}Installing cert-manager...${NC}"
helm repo add jetstack https://charts.jetstack.io
helm repo update

helm install cert-manager jetstack/cert-manager \
  --namespace cert-manager \
  --create-namespace \
  --version v1.13.0 \
  --set installCRDs=true \
  --wait

echo -e "${GREEN}✓ cert-manager installed${NC}"
echo ""

# Wait for cert-manager to be ready
echo -e "${YELLOW}Waiting for cert-manager to be ready...${NC}"
kubectl wait --for=condition=available --timeout=300s deployment/cert-manager -n cert-manager
kubectl wait --for=condition=available --timeout=300s deployment/cert-manager-webhook -n cert-manager

# Create ClusterIssuer
echo -e "${YELLOW}Creating Let's Encrypt ClusterIssuer...${NC}"
cat <<EOF | kubectl apply -f -
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
EOF

echo -e "${GREEN}✓ ClusterIssuer created${NC}"
echo ""

# Get Load Balancer IP
echo -e "${YELLOW}Waiting for Load Balancer IP...${NC}"
LOAD_BALANCER_IP=""
for i in {1..30}; do
    LOAD_BALANCER_IP=$(kubectl get service ingress-nginx-controller \
      -n ingress-nginx \
      -o jsonpath='{.status.loadBalancer.ingress[0].ip}' 2>/dev/null || echo "")
    
    if [ -n "$LOAD_BALANCER_IP" ]; then
        break
    fi
    
    echo "Waiting for Load Balancer IP... ($i/30)"
    sleep 10
done

if [ -z "$LOAD_BALANCER_IP" ]; then
    echo -e "${RED}Error: Failed to get Load Balancer IP${NC}"
    exit 1
fi

echo -e "${GREEN}✓ Load Balancer IP: $LOAD_BALANCER_IP${NC}"
echo ""

# Summary
echo -e "${GREEN}========================================${NC}"
echo -e "${GREEN}Cluster Provisioning Complete!${NC}"
echo -e "${GREEN}========================================${NC}"
echo ""
echo -e "${YELLOW}Cluster Information:${NC}"
echo "  Resource Group: $RESOURCE_GROUP"
echo "  Cluster Name: $CLUSTER_NAME"
echo "  Location: $LOCATION"
echo "  Load Balancer IP: $LOAD_BALANCER_IP"
echo ""
echo -e "${YELLOW}Next Steps:${NC}"
echo "1. Configure DNS records:"
echo "   platform.example.com -> $LOAD_BALANCER_IP"
echo "   api.platform.example.com -> $LOAD_BALANCER_IP"
echo "   ws.platform.example.com -> $LOAD_BALANCER_IP"
echo ""
echo "2. Deploy application:"
echo "   cd .."
echo "   kubectl apply -f namespace.yaml"
echo "   kubectl apply -f rbac.yaml"
echo "   kubectl apply -f resource-quotas.yaml"
echo "   kubectl apply -f limit-ranges.yaml"
echo "   kubectl apply -f network-policies.yaml"
echo "   kubectl apply -f redis-cluster.yaml"
echo "   kubectl apply -f rabbitmq-deployment.yaml"
echo "   kubectl apply -f production/hpa-all-services.yaml"
echo "   kubectl apply -f production/ingress-production.yaml"
echo ""
echo "3. Verify deployment:"
echo "   kubectl get nodes"
echo "   kubectl get pods -n platform-saas-prod"
echo "   kubectl get hpa -n platform-saas-prod"
echo ""
echo -e "${GREEN}For detailed instructions, see PRODUCTION_INFRASTRUCTURE_GUIDE.md${NC}"
