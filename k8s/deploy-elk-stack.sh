#!/bin/bash
# Deploy ELK Stack for Log Aggregation
# This script deploys Elasticsearch, Logstash, and Kibana to Kubernetes
# Requirements: 52.9, 52.13

set -e

echo "=========================================="
echo "Deploying ELK Stack for Log Aggregation"
echo "=========================================="

# Check if kubectl is available
if ! command -v kubectl &> /dev/null; then
    echo "Error: kubectl is not installed or not in PATH"
    exit 1
fi

# Check if namespace exists
if ! kubectl get namespace platform-saas-prod &> /dev/null; then
    echo "Creating namespace platform-saas-prod..."
    kubectl create namespace platform-saas-prod
fi

# Deploy Elasticsearch cluster
echo ""
echo "Step 1/4: Deploying Elasticsearch cluster (3 nodes)..."
kubectl apply -f elasticsearch-cluster.yaml

echo "Waiting for Elasticsearch pods to be ready..."
kubectl wait --for=condition=ready pod -l app=elasticsearch -n platform-saas-prod --timeout=300s || {
    echo "Warning: Elasticsearch pods took longer than expected to start"
    echo "Checking pod status..."
    kubectl get pods -n platform-saas-prod -l app=elasticsearch
}

# Deploy Logstash
echo ""
echo "Step 2/4: Deploying Logstash..."
kubectl apply -f logstash-deployment.yaml

echo "Waiting for Logstash pods to be ready..."
kubectl wait --for=condition=ready pod -l app=logstash -n platform-saas-prod --timeout=180s || {
    echo "Warning: Logstash pods took longer than expected to start"
    echo "Checking pod status..."
    kubectl get pods -n platform-saas-prod -l app=logstash
}

# Deploy Kibana
echo ""
echo "Step 3/4: Deploying Kibana..."
kubectl apply -f kibana-deployment.yaml

echo "Waiting for Kibana pod to be ready..."
kubectl wait --for=condition=ready pod -l app=kibana -n platform-saas-prod --timeout=180s || {
    echo "Warning: Kibana pod took longer than expected to start"
    echo "Checking pod status..."
    kubectl get pods -n platform-saas-prod -l app=kibana
}

# Verify deployment
echo ""
echo "Step 4/4: Verifying ELK stack deployment..."
echo ""
echo "Elasticsearch cluster status:"
kubectl get pods -n platform-saas-prod -l app=elasticsearch
echo ""
echo "Logstash status:"
kubectl get pods -n platform-saas-prod -l app=logstash
echo ""
echo "Kibana status:"
kubectl get pods -n platform-saas-prod -l app=kibana
echo ""

# Check Elasticsearch cluster health
echo "Checking Elasticsearch cluster health..."
ES_POD=$(kubectl get pod -n platform-saas-prod -l app=elasticsearch -o jsonpath='{.items[0].metadata.name}')
kubectl exec -n platform-saas-prod $ES_POD -- curl -s http://localhost:9200/_cluster/health?pretty || {
    echo "Warning: Could not retrieve Elasticsearch cluster health"
}

echo ""
echo "=========================================="
echo "ELK Stack Deployment Complete!"
echo "=========================================="
echo ""
echo "Access Kibana:"
echo "  kubectl port-forward -n platform-saas-prod svc/kibana 5601:5601"
echo "  Then open: http://localhost:5601"
echo ""
echo "Access Elasticsearch:"
echo "  kubectl port-forward -n platform-saas-prod svc/elasticsearch 9200:9200"
echo "  Then open: http://localhost:9200"
echo ""
echo "Logstash endpoints:"
echo "  TCP: logstash.platform-saas-prod.svc.cluster.local:5000"
echo "  HTTP: logstash.platform-saas-prod.svc.cluster.local:8080"
echo "  Beats: logstash.platform-saas-prod.svc.cluster.local:5044"
echo ""
echo "To view logs:"
echo "  kubectl logs -n platform-saas-prod -l app=elasticsearch"
echo "  kubectl logs -n platform-saas-prod -l app=logstash"
echo "  kubectl logs -n platform-saas-prod -l app=kibana"
echo ""
