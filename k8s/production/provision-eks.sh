#!/bin/bash
# Quick provisioning script for Amazon Elastic Kubernetes Service (EKS)
# Requirements: 50.1, 50.13 - Support 10,000 concurrent users with auto-scaling

set -e

# Configuration
CLUSTER_NAME="${CLUSTER_NAME:-platform-saas-prod-eks}"
REGION="${REGION:-us-east-1}"
VERSION="${VERSION:-1.28}"

# Colors for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
NC='\033[0m' # No Color

echo -e "${GREEN}========================================${NC}"
echo -e "${GREEN}EKS Production Cluster Provisioning${NC}"
echo -e "${GREEN}========================================${NC}"
echo ""

# Check prerequisites
echo -e "${YELLOW}Checking prerequisites...${NC}"

if ! command -v aws &> /dev/null; then
    echo -e "${RED}Error: AWS CLI not found. Please install it first.${NC}"
    exit 1
fi

if ! command -v eksctl &> /dev/null; then
    echo -e "${RED}Error: eksctl not found. Please install it first.${NC}"
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

# Check AWS credentials
echo -e "${YELLOW}Checking AWS credentials...${NC}"
if ! aws sts get-caller-identity &> /dev/null; then
    echo -e "${RED}Error: AWS credentials not configured. Run 'aws configure' first.${NC}"
    exit 1
fi

ACCOUNT_ID=$(aws sts get-caller-identity --query Account --output text)
echo -e "${GREEN}✓ Using AWS Account: $ACCOUNT_ID${NC}"
echo ""

# Create cluster configuration
echo -e "${YELLOW}Creating cluster configuration...${NC}"
cat > /tmp/cluster-config.yaml <<EOF
apiVersion: eksctl.io/v1alpha5
kind: ClusterConfig

metadata:
  name: $CLUSTER_NAME
  region: $REGION
  version: "$VERSION"

availabilityZones:
  - ${REGION}a
  - ${REGION}b
  - ${REGION}c

managedNodeGroups:
  # General purpose node group
  - name: general
    instanceType: m5.xlarge
    minSize: 3
    maxSize: 50
    desiredCapacity: 3
    volumeSize: 100
    volumeType: gp3
    privateNetworking: true
    availabilityZones:
      - ${REGION}a
      - ${REGION}b
      - ${REGION}c
    labels:
      workload: general
    tags:
      k8s.io/cluster-autoscaler/enabled: "true"
      k8s.io/cluster-autoscaler/$CLUSTER_NAME: "owned"
    iam:
      withAddonPolicies:
        autoScaler: true
        cloudWatch: true
        ebs: true
        efs: true
        albIngress: true

  # Code Executor node group (CPU optimized)
  - name: code-executor
    instanceType: c5.2xlarge
    minSize: 5
    maxSize: 30
    desiredCapacity: 5
    volumeSize: 100
    volumeType: gp3
    privateNetworking: true
    availabilityZones:
      - ${REGION}a
      - ${REGION}b
      - ${REGION}c
    labels:
      workload: code-executor
    taints:
      - key: workload
        value: code-executor
        effect: NoSchedule
    tags:
      k8s.io/cluster-autoscaler/enabled: "true"
      k8s.io/cluster-autoscaler/$CLUSTER_NAME: "owned"
    iam:
      withAddonPolicies:
        autoScaler: true

  # SQL Executor node group
  - name: sql-executor
    instanceType: m5.xlarge
    minSize: 3
    maxSize: 20
    desiredCapacity: 3
    volumeSize: 100
    volumeType: gp3
    privateNetworking: true
    availabilityZones:
      - ${REGION}a
      - ${REGION}b
      - ${REGION}c
    labels:
      workload: sql-executor
    taints:
      - key: workload
        value: sql-executor
        effect: NoSchedule
    tags:
      k8s.io/cluster-autoscaler/enabled: "true"
      k8s.io/cluster-autoscaler/$CLUSTER_NAME: "owned"
    iam:
      withAddonPolicies:
        autoScaler: true

  # Database node group (memory optimized)
  - name: database
    instanceType: r5.2xlarge
    minSize: 3
    maxSize: 10
    desiredCapacity: 3
    volumeSize: 200
    volumeType: gp3
    privateNetworking: true
    availabilityZones:
      - ${REGION}a
      - ${REGION}b
      - ${REGION}c
    labels:
      workload: database
    taints:
      - key: workload
        value: database
        effect: NoSchedule
    tags:
      k8s.io/cluster-autoscaler/enabled: "true"
      k8s.io/cluster-autoscaler/$CLUSTER_NAME: "owned"
    iam:
      withAddonPolicies:
        autoScaler: true

cloudWatch:
  clusterLogging:
    enableTypes:
      - api
      - audit
      - authenticator
      - controllerManager
      - scheduler

iam:
  withOIDC: true
  serviceAccounts:
    - metadata:
        name: cluster-autoscaler
        namespace: kube-system
      wellKnownPolicies:
        autoScaler: true
    - metadata:
        name: aws-load-balancer-controller
        namespace: kube-system
      wellKnownPolicies:
        awsLoadBalancerController: true

addons:
  - name: vpc-cni
    version: latest
  - name: coredns
    version: latest
  - name: kube-proxy
    version: latest
  - name: aws-ebs-csi-driver
    version: latest
EOF

echo -e "${GREEN}✓ Cluster configuration created${NC}"
echo ""

# Create EKS cluster
echo -e "${YELLOW}Creating EKS cluster (this may take 20-30 minutes)...${NC}"
eksctl create cluster -f /tmp/cluster-config.yaml

echo -e "${GREEN}✓ EKS cluster created${NC}"
echo ""

# Update kubeconfig
echo -e "${YELLOW}Updating kubeconfig...${NC}"
aws eks update-kubeconfig --name "$CLUSTER_NAME" --region "$REGION"

echo -e "${GREEN}✓ Kubeconfig updated${NC}"
echo ""

# Install Cluster Autoscaler
echo -e "${YELLOW}Installing Cluster Autoscaler...${NC}"

# Update cluster-autoscaler.yaml for EKS
sed -i.bak "s/--cloud-provider=aws/--cloud-provider=aws/g" cluster-autoscaler.yaml
sed -i.bak "s/k8s.io\/cluster-autoscaler\/platform-saas-prod/k8s.io\/cluster-autoscaler\/$CLUSTER_NAME/g" cluster-autoscaler.yaml

kubectl apply -f cluster-autoscaler.yaml

echo -e "${GREEN}✓ Cluster Autoscaler installed${NC}"
echo ""

# Install AWS Load Balancer Controller
echo -e "${YELLOW}Installing AWS Load Balancer Controller...${NC}"

# Download IAM policy
curl -o /tmp/iam_policy.json https://raw.githubusercontent.com/kubernetes-sigs/aws-load-balancer-controller/v2.6.0/docs/install/iam_policy.json

# Create IAM policy
POLICY_ARN=$(aws iam create-policy \
  --policy-name AWSLoadBalancerControllerIAMPolicy-$CLUSTER_NAME \
  --policy-document file:///tmp/iam_policy.json \
  --query 'Policy.Arn' \
  --output text 2>/dev/null || \
  aws iam list-policies --query 'Policies[?PolicyName==`AWSLoadBalancerControllerIAMPolicy-'$CLUSTER_NAME'`].Arn' --output text)

echo "Policy ARN: $POLICY_ARN"

# Install AWS Load Balancer Controller
helm repo add eks https://aws.github.io/eks-charts
helm repo update

helm install aws-load-balancer-controller eks/aws-load-balancer-controller \
  -n kube-system \
  --set clusterName=$CLUSTER_NAME \
  --set serviceAccount.create=false \
  --set serviceAccount.name=aws-load-balancer-controller \
  --wait

echo -e "${GREEN}✓ AWS Load Balancer Controller installed${NC}"
echo ""

# Install NGINX Ingress Controller
echo -e "${YELLOW}Installing NGINX Ingress Controller...${NC}"
helm repo add ingress-nginx https://kubernetes.github.io/ingress-nginx
helm repo update

helm install ingress-nginx ingress-nginx/ingress-nginx \
  --namespace ingress-nginx \
  --create-namespace \
  --set controller.replicaCount=3 \
  --set controller.service.type=LoadBalancer \
  --set controller.service.annotations."service\.beta\.kubernetes\.io/aws-load-balancer-type"="nlb" \
  --set controller.service.annotations."service\.beta\.kubernetes\.io/aws-load-balancer-cross-zone-load-balancing-enabled"="true" \
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

# Get Load Balancer hostname
echo -e "${YELLOW}Waiting for Load Balancer hostname...${NC}"
LOAD_BALANCER_HOSTNAME=""
for i in {1..30}; do
    LOAD_BALANCER_HOSTNAME=$(kubectl get service ingress-nginx-controller \
      -n ingress-nginx \
      -o jsonpath='{.status.loadBalancer.ingress[0].hostname}' 2>/dev/null || echo "")
    
    if [ -n "$LOAD_BALANCER_HOSTNAME" ]; then
        break
    fi
    
    echo "Waiting for Load Balancer hostname... ($i/30)"
    sleep 10
done

if [ -z "$LOAD_BALANCER_HOSTNAME" ]; then
    echo -e "${RED}Error: Failed to get Load Balancer hostname${NC}"
    exit 1
fi

echo -e "${GREEN}✓ Load Balancer Hostname: $LOAD_BALANCER_HOSTNAME${NC}"
echo ""

# Summary
echo -e "${GREEN}========================================${NC}"
echo -e "${GREEN}Cluster Provisioning Complete!${NC}"
echo -e "${GREEN}========================================${NC}"
echo ""
echo -e "${YELLOW}Cluster Information:${NC}"
echo "  Cluster Name: $CLUSTER_NAME"
echo "  Region: $REGION"
echo "  Load Balancer Hostname: $LOAD_BALANCER_HOSTNAME"
echo ""
echo -e "${YELLOW}Next Steps:${NC}"
echo "1. Configure DNS records in Route 53:"
echo "   platform.example.com -> $LOAD_BALANCER_HOSTNAME (CNAME)"
echo "   api.platform.example.com -> $LOAD_BALANCER_HOSTNAME (CNAME)"
echo "   ws.platform.example.com -> $LOAD_BALANCER_HOSTNAME (CNAME)"
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
