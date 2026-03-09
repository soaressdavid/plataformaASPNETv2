#!/usr/bin/env pwsh
# Deploy Prometheus and Grafana Monitoring Stack
# This script deploys Prometheus, AlertManager, and Grafana to Kubernetes

param(
    [Parameter(Mandatory=$false)]
    [string]$Namespace = "platform-saas",
    
    [Parameter(Mandatory=$false)]
    [switch]$SkipNamespace,
    
    [Parameter(Mandatory=$false)]
    [switch]$DryRun
)

$ErrorActionPreference = "Stop"

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "Prometheus & Grafana Deployment Script" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

# Function to apply Kubernetes manifest
function Apply-Manifest {
    param(
        [string]$File,
        [string]$Description
    )
    
    Write-Host "Deploying $Description..." -ForegroundColor Yellow
    
    if ($DryRun) {
        Write-Host "  [DRY RUN] Would apply: $File" -ForegroundColor Gray
        kubectl apply -f $File --dry-run=client
    } else {
        kubectl apply -f $File
        if ($LASTEXITCODE -eq 0) {
            Write-Host "  ✓ $Description deployed successfully" -ForegroundColor Green
        } else {
            Write-Host "  ✗ Failed to deploy $Description" -ForegroundColor Red
            exit 1
        }
    }
    Write-Host ""
}

# Check if kubectl is available
Write-Host "Checking prerequisites..." -ForegroundColor Yellow
if (-not (Get-Command kubectl -ErrorAction SilentlyContinue)) {
    Write-Host "✗ kubectl is not installed or not in PATH" -ForegroundColor Red
    exit 1
}
Write-Host "✓ kubectl is available" -ForegroundColor Green
Write-Host ""

# Check if namespace exists
if (-not $SkipNamespace) {
    Write-Host "Checking namespace..." -ForegroundColor Yellow
    $namespaceExists = kubectl get namespace $Namespace 2>$null
    if ($LASTEXITCODE -ne 0) {
        Write-Host "  Namespace '$Namespace' does not exist. Creating..." -ForegroundColor Yellow
        kubectl create namespace $Namespace
        if ($LASTEXITCODE -eq 0) {
            Write-Host "  ✓ Namespace created" -ForegroundColor Green
        } else {
            Write-Host "  ✗ Failed to create namespace" -ForegroundColor Red
            exit 1
        }
    } else {
        Write-Host "  ✓ Namespace '$Namespace' exists" -ForegroundColor Green
    }
    Write-Host ""
}

# Deploy Prometheus
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "Deploying Prometheus" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

Apply-Manifest -File "prometheus-config.yaml" -Description "Prometheus Configuration"
Apply-Manifest -File "prometheus-deployment.yaml" -Description "Prometheus Deployment"

# Deploy AlertManager
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "Deploying AlertManager" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

Apply-Manifest -File "alertmanager-config.yaml" -Description "AlertManager Configuration"
Apply-Manifest -File "alertmanager-deployment.yaml" -Description "AlertManager Deployment"

# Deploy Grafana
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "Deploying Grafana" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

Apply-Manifest -File "grafana-config.yaml" -Description "Grafana Configuration"
Apply-Manifest -File "grafana-deployment.yaml" -Description "Grafana Deployment"

# Wait for deployments to be ready
if (-not $DryRun) {
    Write-Host "========================================" -ForegroundColor Cyan
    Write-Host "Waiting for Deployments" -ForegroundColor Cyan
    Write-Host "========================================" -ForegroundColor Cyan
    Write-Host ""
    
    Write-Host "Waiting for Prometheus to be ready..." -ForegroundColor Yellow
    kubectl wait --for=condition=available --timeout=300s deployment/prometheus -n $Namespace
    Write-Host "✓ Prometheus is ready" -ForegroundColor Green
    Write-Host ""
    
    Write-Host "Waiting for AlertManager to be ready..." -ForegroundColor Yellow
    kubectl wait --for=condition=available --timeout=300s deployment/alertmanager -n $Namespace
    Write-Host "✓ AlertManager is ready" -ForegroundColor Green
    Write-Host ""
    
    Write-Host "Waiting for Grafana to be ready..." -ForegroundColor Yellow
    kubectl wait --for=condition=available --timeout=300s deployment/grafana -n $Namespace
    Write-Host "✓ Grafana is ready" -ForegroundColor Green
    Write-Host ""
}

# Display service information
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "Deployment Summary" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

if (-not $DryRun) {
    Write-Host "Services:" -ForegroundColor Yellow
    kubectl get services -n $Namespace -l component=monitoring
    Write-Host ""
    
    Write-Host "Pods:" -ForegroundColor Yellow
    kubectl get pods -n $Namespace -l component=monitoring
    Write-Host ""
    
    Write-Host "PersistentVolumeClaims:" -ForegroundColor Yellow
    kubectl get pvc -n $Namespace | Select-String "prometheus|alertmanager|grafana"
    Write-Host ""
}

# Display access information
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "Access Information" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

Write-Host "To access Prometheus:" -ForegroundColor Yellow
Write-Host "  kubectl port-forward -n $Namespace svc/prometheus 9090:9090" -ForegroundColor White
Write-Host "  Then open: http://localhost:9090" -ForegroundColor White
Write-Host ""

Write-Host "To access AlertManager:" -ForegroundColor Yellow
Write-Host "  kubectl port-forward -n $Namespace svc/alertmanager 9093:9093" -ForegroundColor White
Write-Host "  Then open: http://localhost:9093" -ForegroundColor White
Write-Host ""

Write-Host "To access Grafana:" -ForegroundColor Yellow
Write-Host "  kubectl port-forward -n $Namespace svc/grafana 3000:3000" -ForegroundColor White
Write-Host "  Then open: http://localhost:3000" -ForegroundColor White
Write-Host "  Default credentials: admin / (check grafana-secrets)" -ForegroundColor White
Write-Host ""

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "Important Notes" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

Write-Host "1. Update AlertManager secrets:" -ForegroundColor Yellow
Write-Host "   - Edit k8s/alertmanager-config.yaml" -ForegroundColor White
Write-Host "   - Replace SMTP password and Slack webhook URL" -ForegroundColor White
Write-Host "   - Reapply: kubectl apply -f alertmanager-config.yaml" -ForegroundColor White
Write-Host ""

Write-Host "2. Update Grafana admin password:" -ForegroundColor Yellow
Write-Host "   - Edit k8s/grafana-deployment.yaml" -ForegroundColor White
Write-Host "   - Replace REPLACE_WITH_SECURE_PASSWORD" -ForegroundColor White
Write-Host "   - Reapply: kubectl apply -f grafana-deployment.yaml" -ForegroundColor White
Write-Host ""

Write-Host "3. Configure microservice metrics endpoints:" -ForegroundColor Yellow
Write-Host "   - Add prometheus.io/scrape: 'true' annotation to pods" -ForegroundColor White
Write-Host "   - Add prometheus.io/port: '<port>' annotation" -ForegroundColor White
Write-Host "   - Add prometheus.io/path: '/metrics' annotation" -ForegroundColor White
Write-Host ""

Write-Host "4. Pre-configured Grafana dashboards:" -ForegroundColor Yellow
Write-Host "   - System Overview (CPU, Memory, Disk, Network)" -ForegroundColor White
Write-Host "   - API Metrics (Request Rate, Error Rate, Response Time)" -ForegroundColor White
Write-Host "   - Database Metrics (Connection Pool, Query Performance)" -ForegroundColor White
Write-Host "   - Message Queue Metrics (Queue Length, Processing Rate)" -ForegroundColor White
Write-Host "   - Microservices Overview (Pod Status, Resource Usage)" -ForegroundColor White
Write-Host ""

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "Deployment Complete!" -ForegroundColor Green
Write-Host "========================================" -ForegroundColor Cyan
