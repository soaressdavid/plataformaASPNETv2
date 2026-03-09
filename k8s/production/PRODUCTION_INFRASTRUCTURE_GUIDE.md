# Production Infrastructure Provisioning Guide

This guide provides step-by-step instructions for provisioning production Kubernetes infrastructure on Azure Kubernetes Service (AKS) or Amazon Elastic Kubernetes Service (EKS).

## Overview

The production infrastructure is designed to support **10,000 concurrent users** with auto-scaling, high availability, and comprehensive monitoring.

### Architecture Highlights

- **Auto-scaling**: Cluster autoscaler + HPA for all microservices
- **High Availability**: Multi-zone deployment with 3+ replicas per service
- **Load Balancing**: NGINX Ingress Controller with Network Load Balancer
- **Security**: TLS/SSL, network policies, RBAC, pod security standards
- **Monitoring**: Prometheus, Grafana, ELK Stack, Application Insights
- **Performance**: <200ms API response time (p95), gzip compression, CDN integration

### Requirements Satisfied

- **50.1**: Handle 10,000 concurrent users without performance degradation
- **50.2**: API response time <200ms at 95th percentile
- **50.3**: Horizontal scaling for Code Executor based on queue length
- **50.4**: Horizontal scaling for SQL Executor based on queue length
- **50.13**: Auto-scaling policies for all microservices
- **50.14**: Load balancer distributing traffic across multiple API Gateway instances

---

## Option 1: Azure Kubernetes Service (AKS)

### Prerequisites

1. **Azure CLI** installed and configured
   ```bash
   az --version
   az login
   az account set --subscription "YOUR_SUBSCRIPTION_ID"
   ```

2. **kubectl** installed
   ```bash
   az aks install-cli
   ```

3. **Helm** installed (for cert-manager and ingress-nginx)
   ```bash
   curl https://raw.githubusercontent.com/helm/helm/main/scripts/get-helm-3 | bash
   ```

### Step 1: Create Resource Group

```bash
# Set variables
RESOURCE_GROUP="platform-saas-prod-rg"
LOCATION="eastus"
CLUSTER_NAME="platform-saas-prod-aks"

# Create resource group
az group create \
  --name $RESOURCE_GROUP \
  --location $LOCATION
```

### Step 2: Create AKS Cluster

```bash
# Create AKS cluster with auto-scaling enabled
az aks create \
  --resource-group $RESOURCE_GROUP \
  --name $CLUSTER_NAME \
  --location $LOCATION \
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
  --workspace-resource-id "/subscriptions/YOUR_SUBSCRIPTION_ID/resourceGroups/$RESOURCE_GROUP/providers/Microsoft.OperationalInsights/workspaces/platform-saas-logs" \
  --generate-ssh-keys \
  --kubernetes-version 1.28.3

# Get credentials
az aks get-credentials \
  --resource-group $RESOURCE_GROUP \
  --name $CLUSTER_NAME \
  --overwrite-existing
```

### Step 3: Create Additional Node Pools

```bash
# Node pool for Code Executor (high CPU)
az aks nodepool add \
  --resource-group $RESOURCE_GROUP \
  --cluster-name $CLUSTER_NAME \
  --name codeexec \
  --node-count 5 \
  --min-count 5 \
  --max-count 30 \
  --enable-cluster-autoscaler \
  --node-vm-size Standard_F8s_v2 \
  --zones 1 2 3 \
  --labels workload=code-executor \
  --node-taints workload=code-executor:NoSchedule

# Node pool for SQL Executor
az aks nodepool add \
  --resource-group $RESOURCE_GROUP \
  --cluster-name $CLUSTER_NAME \
  --name sqlexec \
  --node-count 3 \
  --min-count 3 \
  --max-count 20 \
  --enable-cluster-autoscaler \
  --node-vm-size Standard_D4s_v3 \
  --zones 1 2 3 \
  --labels workload=sql-executor \
  --node-taints workload=sql-executor:NoSchedule

# Node pool for databases (memory optimized)
az aks nodepool add \
  --resource-group $RESOURCE_GROUP \
  --cluster-name $CLUSTER_NAME \
  --name database \
  --node-count 3 \
  --min-count 3 \
  --max-count 10 \
  --enable-cluster-autoscaler \
  --node-vm-size Standard_E8s_v3 \
  --zones 1 2 3 \
  --labels workload=database \
  --node-taints workload=database:NoSchedule
```

### Step 4: Install NGINX Ingress Controller

```bash
# Add Helm repository
helm repo add ingress-nginx https://kubernetes.github.io/ingress-nginx
helm repo update

# Install NGINX Ingress Controller
helm install ingress-nginx ingress-nginx/ingress-nginx \
  --namespace ingress-nginx \
  --create-namespace \
  --set controller.replicaCount=3 \
  --set controller.nodeSelector."kubernetes\.io/os"=linux \
  --set controller.service.annotations."service\.beta\.kubernetes\.io/azure-load-balancer-health-probe-request-path"=/healthz \
  --set controller.service.externalTrafficPolicy=Local \
  --set controller.metrics.enabled=true \
  --set controller.podAnnotations."prometheus\.io/scrape"=true \
  --set controller.podAnnotations."prometheus\.io/port"=10254
```

### Step 5: Install cert-manager for SSL/TLS

```bash
# Install cert-manager
helm repo add jetstack https://charts.jetstack.io
helm repo update

helm install cert-manager jetstack/cert-manager \
  --namespace cert-manager \
  --create-namespace \
  --version v1.13.0 \
  --set installCRDs=true

# Create ClusterIssuer for Let's Encrypt
kubectl apply -f - <<EOF
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
```

### Step 6: Deploy Application

```bash
# Apply namespace and RBAC
kubectl apply -f ../namespace.yaml
kubectl apply -f ../rbac.yaml

# Apply resource quotas and limits
kubectl apply -f ../resource-quotas.yaml
kubectl apply -f ../limit-ranges.yaml

# Apply network policies
kubectl apply -f ../network-policies.yaml

# Deploy infrastructure (Redis, RabbitMQ, SQL Server)
kubectl apply -f ../redis-cluster.yaml
kubectl apply -f ../rabbitmq-deployment.yaml
kubectl apply -f ../postgres-deployment.yaml  # Or SQL Server

# Deploy microservices
kubectl apply -f ../api-gateway-deployment.yaml
kubectl apply -f ../auth-service-deployment.yaml
kubectl apply -f ../execution-service-deployment.yaml
# ... (all other services)

# Apply HPA configurations
kubectl apply -f hpa-all-services.yaml

# Apply ingress
kubectl apply -f ingress-production.yaml
```

### Step 7: Configure DNS

```bash
# Get Load Balancer IP
LOAD_BALANCER_IP=$(kubectl get service ingress-nginx-controller \
  -n ingress-nginx \
  -o jsonpath='{.status.loadBalancer.ingress[0].ip}')

echo "Load Balancer IP: $LOAD_BALANCER_IP"

# Create DNS A records:
# platform.example.com -> $LOAD_BALANCER_IP
# api.platform.example.com -> $LOAD_BALANCER_IP
# ws.platform.example.com -> $LOAD_BALANCER_IP
```

### Step 8: Verify Deployment

```bash
# Check all pods are running
kubectl get pods -n platform-saas-prod

# Check HPA status
kubectl get hpa -n platform-saas-prod

# Check ingress
kubectl get ingress -n platform-saas-prod

# Test API endpoint
curl https://api.platform.example.com/health
```

---

## Option 2: Amazon Elastic Kubernetes Service (EKS)

### Prerequisites

1. **AWS CLI** installed and configured
   ```bash
   aws --version
   aws configure
   ```

2. **eksctl** installed
   ```bash
   curl --silent --location "https://github.com/weaveworks/eksctl/releases/latest/download/eksctl_$(uname -s)_amd64.tar.gz" | tar xz -C /tmp
   sudo mv /tmp/eksctl /usr/local/bin
   ```

3. **kubectl** installed
   ```bash
   curl -LO "https://dl.k8s.io/release/$(curl -L -s https://dl.k8s.io/release/stable.txt)/bin/linux/amd64/kubectl"
   sudo install -o root -g root -m 0755 kubectl /usr/local/bin/kubectl
   ```

### Step 1: Create EKS Cluster

```bash
# Set variables
CLUSTER_NAME="platform-saas-prod-eks"
REGION="us-east-1"
VERSION="1.28"

# Create cluster configuration file
cat > cluster-config.yaml <<EOF
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

# Create cluster
eksctl create cluster -f cluster-config.yaml
```

### Step 2: Install Cluster Autoscaler

```bash
# Update cluster-autoscaler.yaml for EKS
# Change --cloud-provider=aws (already set)
# Update --node-group-auto-discovery tag

kubectl apply -f cluster-autoscaler.yaml
```

### Step 3: Install AWS Load Balancer Controller

```bash
# Download IAM policy
curl -o iam_policy.json https://raw.githubusercontent.com/kubernetes-sigs/aws-load-balancer-controller/v2.6.0/docs/install/iam_policy.json

# Create IAM policy
aws iam create-policy \
  --policy-name AWSLoadBalancerControllerIAMPolicy \
  --policy-document file://iam_policy.json

# Install AWS Load Balancer Controller
helm repo add eks https://aws.github.io/eks-charts
helm repo update

helm install aws-load-balancer-controller eks/aws-load-balancer-controller \
  -n kube-system \
  --set clusterName=$CLUSTER_NAME \
  --set serviceAccount.create=false \
  --set serviceAccount.name=aws-load-balancer-controller
```

### Step 4: Install NGINX Ingress Controller

```bash
helm repo add ingress-nginx https://kubernetes.github.io/ingress-nginx
helm repo update

helm install ingress-nginx ingress-nginx/ingress-nginx \
  --namespace ingress-nginx \
  --create-namespace \
  --set controller.replicaCount=3 \
  --set controller.service.type=LoadBalancer \
  --set controller.service.annotations."service\.beta\.kubernetes\.io/aws-load-balancer-type"="nlb" \
  --set controller.service.annotations."service\.beta\.kubernetes\.io/aws-load-balancer-cross-zone-load-balancing-enabled"="true" \
  --set controller.metrics.enabled=true
```

### Step 5: Install cert-manager

```bash
helm repo add jetstack https://charts.jetstack.io
helm repo update

helm install cert-manager jetstack/cert-manager \
  --namespace cert-manager \
  --create-namespace \
  --version v1.13.0 \
  --set installCRDs=true

# Create ClusterIssuer
kubectl apply -f - <<EOF
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
```

### Step 6: Deploy Application

```bash
# Apply all Kubernetes manifests (same as AKS)
kubectl apply -f ../namespace.yaml
kubectl apply -f ../rbac.yaml
kubectl apply -f ../resource-quotas.yaml
kubectl apply -f ../limit-ranges.yaml
kubectl apply -f ../network-policies.yaml

# Deploy infrastructure
kubectl apply -f ../redis-cluster.yaml
kubectl apply -f ../rabbitmq-deployment.yaml
kubectl apply -f ../postgres-deployment.yaml

# Deploy microservices
kubectl apply -f ../api-gateway-deployment.yaml
kubectl apply -f ../auth-service-deployment.yaml
kubectl apply -f ../execution-service-deployment.yaml
# ... (all other services)

# Apply HPA
kubectl apply -f hpa-all-services.yaml

# Apply ingress
kubectl apply -f ingress-production.yaml
```

### Step 7: Configure DNS

```bash
# Get Load Balancer hostname
LOAD_BALANCER_HOSTNAME=$(kubectl get service ingress-nginx-controller \
  -n ingress-nginx \
  -o jsonpath='{.status.loadBalancer.ingress[0].hostname}')

echo "Load Balancer Hostname: $LOAD_BALANCER_HOSTNAME"

# Create DNS CNAME records in Route 53:
# platform.example.com -> $LOAD_BALANCER_HOSTNAME
# api.platform.example.com -> $LOAD_BALANCER_HOSTNAME
# ws.platform.example.com -> $LOAD_BALANCER_HOSTNAME
```

### Step 8: Verify Deployment

```bash
# Check cluster status
eksctl get cluster --name $CLUSTER_NAME --region $REGION

# Check nodes
kubectl get nodes

# Check all pods
kubectl get pods -n platform-saas-prod

# Check HPA
kubectl get hpa -n platform-saas-prod

# Test API
curl https://api.platform.example.com/health
```

---

## Post-Deployment Configuration

### 1. Configure Monitoring

```bash
# Deploy Prometheus and Grafana
kubectl apply -f ../prometheus-deployment.yaml
kubectl apply -f ../grafana-deployment.yaml

# Deploy ELK Stack
kubectl apply -f ../elasticsearch-cluster.yaml
kubectl apply -f ../logstash-deployment.yaml
kubectl apply -f ../kibana-deployment.yaml

# Configure Application Insights (Azure) or CloudWatch (AWS)
# Update configmaps with connection strings
```

### 2. Setup Backup Strategy

```bash
# Install Velero for cluster backups
# For AWS:
velero install \
  --provider aws \
  --plugins velero/velero-plugin-for-aws:v1.8.0 \
  --bucket platform-saas-backups \
  --backup-location-config region=$REGION \
  --snapshot-location-config region=$REGION \
  --secret-file ./credentials-velero

# For Azure:
velero install \
  --provider azure \
  --plugins velero/velero-plugin-for-microsoft-azure:v1.8.0 \
  --bucket platform-saas-backups \
  --secret-file ./credentials-velero \
  --backup-location-config resourceGroup=$RESOURCE_GROUP,storageAccount=platformsaasbackups \
  --snapshot-location-config apiTimeout=5m,resourceGroup=$RESOURCE_GROUP

# Create backup schedule
velero schedule create daily-backup --schedule="0 2 * * *"
```

### 3. Configure Secrets Management

```bash
# For AWS: Use AWS Secrets Manager
# Install External Secrets Operator
helm repo add external-secrets https://charts.external-secrets.io
helm install external-secrets external-secrets/external-secrets \
  -n external-secrets-system \
  --create-namespace

# For Azure: Use Azure Key Vault
# Install CSI Secret Store Driver
helm repo add csi-secrets-store-provider-azure https://azure.github.io/secrets-store-csi-driver-provider-azure/charts
helm install csi-secrets-store-provider-azure/csi-secrets-store-provider-azure \
  --namespace kube-system
```

### 4. Enable Pod Disruption Budgets

```bash
# Apply PDBs for all critical services
kubectl apply -f - <<EOF
apiVersion: policy/v1
kind: PodDisruptionBudget
metadata:
  name: api-gateway-pdb
  namespace: platform-saas-prod
spec:
  minAvailable: 2
  selector:
    matchLabels:
      app: api-gateway
---
apiVersion: policy/v1
kind: PodDisruptionBudget
metadata:
  name: code-executor-pdb
  namespace: platform-saas-prod
spec:
  minAvailable: 5
  selector:
    matchLabels:
      app: code-executor
# ... (add for all services)
EOF
```

---

## Load Testing

### Run Load Tests to Verify 10,000 Concurrent Users

```bash
# Install k6
curl https://github.com/grafana/k6/releases/download/v0.47.0/k6-v0.47.0-linux-amd64.tar.gz -L | tar xvz
sudo mv k6-v0.47.0-linux-amd64/k6 /usr/local/bin/

# Run endurance test (from tests/LoadTests/)
cd ../../tests/LoadTests
k6 run --vus 10000 --duration 55m load-test-scenarios.js

# Monitor during test:
# - Prometheus metrics
# - Grafana dashboards
# - kubectl top nodes
# - kubectl top pods -n platform-saas-prod
```

---

## Scaling Configuration Summary

### Cluster Autoscaler
- **Min Nodes**: 3 (high availability)
- **Max Nodes**: 50 (cost control)
- **Scale Up**: Aggressive (when pods pending)
- **Scale Down**: Conservative (10min delay, 50% utilization threshold)

### Horizontal Pod Autoscalers
| Service | Min Replicas | Max Replicas | Target CPU | Target Memory |
|---------|--------------|--------------|------------|---------------|
| API Gateway | 5 | 50 | 70% | 80% |
| Code Executor | 10 | 100 | 70% | 80% |
| SQL Executor | 5 | 50 | 70% | 80% |
| Gamification | 5 | 30 | 70% | 80% |
| AI Tutor | 3 | 20 | 70% | 80% |
| Notification | 3 | 20 | 70% | 80% |
| Analytics | 3 | 15 | 70% | 80% |
| Auth | 5 | 30 | 70% | 80% |
| Content | 5 | 30 | 70% | 80% |
| Frontend | 5 | 30 | 70% | 80% |

### Custom Metrics
- **Code Executor**: Scales based on `execution_queue_length` (target: 10 per pod)
- **SQL Executor**: Scales based on `sql_queue_length` (target: 10 per pod)

---

## Cost Optimization

### AKS Cost Estimates (Monthly)
- **Cluster Management**: Free
- **Nodes (3x Standard_D4s_v3)**: ~$450
- **Load Balancer**: ~$20
- **Storage**: ~$50
- **Monitoring**: ~$100
- **Total Base**: ~$620/month
- **At Scale (50 nodes)**: ~$7,500/month

### EKS Cost Estimates (Monthly)
- **Cluster Management**: $73
- **Nodes (3x m5.xlarge)**: ~$360
- **Load Balancer (NLB)**: ~$20
- **Storage (EBS)**: ~$50
- **CloudWatch**: ~$100
- **Total Base**: ~$603/month
- **At Scale (50 nodes)**: ~$6,073/month

### Cost Optimization Tips
1. Use spot instances for non-critical workloads
2. Enable cluster autoscaler to scale down during low traffic
3. Use reserved instances for baseline capacity
4. Implement pod resource requests/limits accurately
5. Use horizontal pod autoscaling to avoid over-provisioning

---

## Troubleshooting

### Cluster Autoscaler Not Scaling
```bash
# Check autoscaler logs
kubectl logs -n kube-system deployment/cluster-autoscaler

# Check node group tags (AWS)
aws autoscaling describe-auto-scaling-groups \
  --query "AutoScalingGroups[?Tags[?Key=='k8s.io/cluster-autoscaler/enabled']]"
```

### HPA Not Scaling
```bash
# Check HPA status
kubectl describe hpa <hpa-name> -n platform-saas-prod

# Check metrics server
kubectl top nodes
kubectl top pods -n platform-saas-prod

# Check custom metrics
kubectl get --raw /apis/custom.metrics.k8s.io/v1beta1
```

### Ingress Not Working
```bash
# Check ingress controller logs
kubectl logs -n ingress-nginx deployment/ingress-nginx-controller

# Check ingress status
kubectl describe ingress platform-ingress -n platform-saas-prod

# Test from inside cluster
kubectl run debug --image=curlimages/curl -it --rm -- curl http://api-gateway-service.platform-saas-prod.svc.cluster.local
```

---

## Security Checklist

- [ ] TLS/SSL certificates configured (Let's Encrypt)
- [ ] Network policies applied
- [ ] RBAC configured with least privilege
- [ ] Pod security standards enforced
- [ ] Secrets encrypted at rest
- [ ] Container images scanned for vulnerabilities
- [ ] API rate limiting enabled
- [ ] CORS configured properly
- [ ] Security headers configured in ingress
- [ ] Monitoring and alerting configured
- [ ] Backup and disaster recovery tested
- [ ] Penetration testing completed

---

## Support and Maintenance

### Regular Maintenance Tasks
1. **Weekly**: Review monitoring dashboards and alerts
2. **Monthly**: Update Kubernetes version and node images
3. **Quarterly**: Review and optimize resource quotas
4. **Annually**: Conduct security audit and penetration testing

### Monitoring Dashboards
- **Grafana**: https://monitoring.platform.example.com/grafana
- **Prometheus**: https://monitoring.platform.example.com/prometheus
- **Kibana**: https://monitoring.platform.example.com/kibana

### Emergency Contacts
- Platform Team: platform-team@example.com
- On-Call: +1-XXX-XXX-XXXX
- Slack: #platform-incidents

---

## References

- [Kubernetes Documentation](https://kubernetes.io/docs/)
- [AKS Documentation](https://docs.microsoft.com/en-us/azure/aks/)
- [EKS Documentation](https://docs.aws.amazon.com/eks/)
- [NGINX Ingress Controller](https://kubernetes.github.io/ingress-nginx/)
- [cert-manager Documentation](https://cert-manager.io/docs/)
- [Cluster Autoscaler](https://github.com/kubernetes/autoscaler/tree/master/cluster-autoscaler)
