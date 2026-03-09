# Deploy ELK Stack for Log Aggregation
# This script deploys Elasticsearch, Logstash, and Kibana to Kubernetes
# Requirements: 52.9, 52.13

$ErrorActionPreference = "Stop"

Write-Host "==========================================" -ForegroundColor Cyan
Write-Host "Deploying ELK Stack for Log Aggregation" -ForegroundColor Cyan
Write-Host "==========================================" -ForegroundColor Cyan

# Check if kubectl is available
if (-not (Get-Command kubectl -ErrorAction SilentlyContinue)) {
    Write-Host "Error: kubectl is not installed or not in PATH" -ForegroundColor Red
    exit 1
}

# Check if namespace exists
$namespaceExists = kubectl get namespace platform-saas-prod 2>$null
if (-not $namespaceExists) {
    Write-Host "Creating namespace platform-saas-prod..." -ForegroundColor Yellow
    kubectl create namespace platform-saas-prod
}

# Deploy Elasticsearch cluster
Write-Host ""
Write-Host "Step 1/4: Deploying Elasticsearch cluster (3 nodes)..." -ForegroundColor Green
kubectl apply -f elasticsearch-cluster.yaml

Write-Host "Waiting for Elasticsearch pods to be ready..." -ForegroundColor Yellow
$esReady = kubectl wait --for=condition=ready pod -l app=elasticsearch -n platform-saas-prod --timeout=300s 2>$null
if (-not $esReady) {
    Write-Host "Warning: Elasticsearch pods took longer than expected to start" -ForegroundColor Yellow
    Write-Host "Checking pod status..." -ForegroundColor Yellow
    kubectl get pods -n platform-saas-prod -l app=elasticsearch
}

# Deploy Logstash
Write-Host ""
Write-Host "Step 2/4: Deploying Logstash..." -ForegroundColor Green
kubectl apply -f logstash-deployment.yaml

Write-Host "Waiting for Logstash pods to be ready..." -ForegroundColor Yellow
$logstashReady = kubectl wait --for=condition=ready pod -l app=logstash -n platform-saas-prod --timeout=180s 2>$null
if (-not $logstashReady) {
    Write-Host "Warning: Logstash pods took longer than expected to start" -ForegroundColor Yellow
    Write-Host "Checking pod status..." -ForegroundColor Yellow
    kubectl get pods -n platform-saas-prod -l app=logstash
}

# Deploy Kibana
Write-Host ""
Write-Host "Step 3/4: Deploying Kibana..." -ForegroundColor Green
kubectl apply -f kibana-deployment.yaml

Write-Host "Waiting for Kibana pod to be ready..." -ForegroundColor Yellow
$kibanaReady = kubectl wait --for=condition=ready pod -l app=kibana -n platform-saas-prod --timeout=180s 2>$null
if (-not $kibanaReady) {
    Write-Host "Warning: Kibana pod took longer than expected to start" -ForegroundColor Yellow
    Write-Host "Checking pod status..." -ForegroundColor Yellow
    kubectl get pods -n platform-saas-prod -l app=kibana
}

# Verify deployment
Write-Host ""
Write-Host "Step 4/4: Verifying ELK stack deployment..." -ForegroundColor Green
Write-Host ""
Write-Host "Elasticsearch cluster status:" -ForegroundColor Cyan
kubectl get pods -n platform-saas-prod -l app=elasticsearch
Write-Host ""
Write-Host "Logstash status:" -ForegroundColor Cyan
kubectl get pods -n platform-saas-prod -l app=logstash
Write-Host ""
Write-Host "Kibana status:" -ForegroundColor Cyan
kubectl get pods -n platform-saas-prod -l app=kibana
Write-Host ""

# Check Elasticsearch cluster health
Write-Host "Checking Elasticsearch cluster health..." -ForegroundColor Yellow
$esPod = kubectl get pod -n platform-saas-prod -l app=elasticsearch -o jsonpath='{.items[0].metadata.name}'
if ($esPod) {
    kubectl exec -n platform-saas-prod $esPod -- curl -s http://localhost:9200/_cluster/health?pretty 2>$null
    if (-not $?) {
        Write-Host "Warning: Could not retrieve Elasticsearch cluster health" -ForegroundColor Yellow
    }
}

Write-Host ""
Write-Host "==========================================" -ForegroundColor Cyan
Write-Host "ELK Stack Deployment Complete!" -ForegroundColor Green
Write-Host "==========================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "Access Kibana:" -ForegroundColor Yellow
Write-Host "  kubectl port-forward -n platform-saas-prod svc/kibana 5601:5601"
Write-Host "  Then open: http://localhost:5601"
Write-Host ""
Write-Host "Access Elasticsearch:" -ForegroundColor Yellow
Write-Host "  kubectl port-forward -n platform-saas-prod svc/elasticsearch 9200:9200"
Write-Host "  Then open: http://localhost:9200"
Write-Host ""
Write-Host "Logstash endpoints:" -ForegroundColor Yellow
Write-Host "  TCP: logstash.platform-saas-prod.svc.cluster.local:5000"
Write-Host "  HTTP: logstash.platform-saas-prod.svc.cluster.local:8080"
Write-Host "  Beats: logstash.platform-saas-prod.svc.cluster.local:5044"
Write-Host ""
Write-Host "To view logs:" -ForegroundColor Yellow
Write-Host "  kubectl logs -n platform-saas-prod -l app=elasticsearch"
Write-Host "  kubectl logs -n platform-saas-prod -l app=logstash"
Write-Host "  kubectl logs -n platform-saas-prod -l app=kibana"
Write-Host ""
