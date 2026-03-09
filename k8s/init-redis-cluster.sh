#!/bin/bash
# Script to initialize Redis cluster after StatefulSet is deployed

set -e

NAMESPACE="aspnet-learning-platform"
REPLICAS=6

echo "Waiting for all Redis pods to be ready..."
kubectl wait --for=condition=ready pod -l app=redis-cluster -n $NAMESPACE --timeout=300s

echo "Getting Redis pod IPs..."
REDIS_NODES=""
for i in $(seq 0 $((REPLICAS-1))); do
    POD_IP=$(kubectl get pod redis-cluster-$i -n $NAMESPACE -o jsonpath='{.status.podIP}')
    REDIS_NODES="$REDIS_NODES $POD_IP:6379"
done

echo "Redis nodes: $REDIS_NODES"

echo "Creating Redis cluster with 3 masters and 3 replicas..."
kubectl exec -it redis-cluster-0 -n $NAMESPACE -- redis-cli --cluster create $REDIS_NODES --cluster-replicas 1 --cluster-yes

echo "Verifying cluster status..."
kubectl exec -it redis-cluster-0 -n $NAMESPACE -- redis-cli cluster info

echo "Redis cluster initialized successfully!"
echo ""
echo "Cluster nodes:"
kubectl exec -it redis-cluster-0 -n $NAMESPACE -- redis-cli cluster nodes
