# PowerShell script to initialize Redis cluster after StatefulSet is deployed

$ErrorActionPreference = "Stop"

$NAMESPACE = "aspnet-learning-platform"
$REPLICAS = 6

Write-Host "Waiting for all Redis pods to be ready..." -ForegroundColor Cyan
kubectl wait --for=condition=ready pod -l app=redis-cluster -n $NAMESPACE --timeout=300s

Write-Host "Getting Redis pod IPs..." -ForegroundColor Cyan
$REDIS_NODES = @()
for ($i = 0; $i -lt $REPLICAS; $i++) {
    $POD_IP = kubectl get pod "redis-cluster-$i" -n $NAMESPACE -o jsonpath='{.status.podIP}'
    $REDIS_NODES += "$POD_IP:6379"
}

$NODES_STRING = $REDIS_NODES -join " "
Write-Host "Redis nodes: $NODES_STRING" -ForegroundColor Yellow

Write-Host "Creating Redis cluster with 3 masters and 3 replicas..." -ForegroundColor Cyan
kubectl exec -it redis-cluster-0 -n $NAMESPACE -- redis-cli --cluster create $NODES_STRING --cluster-replicas 1 --cluster-yes

Write-Host "Verifying cluster status..." -ForegroundColor Cyan
kubectl exec -it redis-cluster-0 -n $NAMESPACE -- redis-cli cluster info

Write-Host "Redis cluster initialized successfully!" -ForegroundColor Green
Write-Host ""
Write-Host "Cluster nodes:" -ForegroundColor Cyan
kubectl exec -it redis-cluster-0 -n $NAMESPACE -- redis-cli cluster nodes
