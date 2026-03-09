# Redis Cluster Setup Guide

This guide explains how to deploy and configure the Redis cluster for distributed caching in the ASP.NET Learning Platform.

## Overview

The Redis cluster is configured with:
- **3 master nodes** and **3 replica nodes** (6 total pods)
- **Persistence**: Both RDB snapshots and AOF (Append Only File)
- **Connection pooling**: Implemented in C# using StackExchange.Redis
- **High availability**: Automatic failover with cluster mode

## Architecture

```
Redis Cluster (6 nodes)
├── Master 1 (redis-cluster-0) → Replica 4 (redis-cluster-3)
├── Master 2 (redis-cluster-1) → Replica 5 (redis-cluster-4)
└── Master 3 (redis-cluster-2) → Replica 6 (redis-cluster-5)
```

## Cache TTL Configuration

As per Requirements 22.4, 22.5, 22.6:

| Data Type | TTL | Requirement |
|-----------|-----|-------------|
| Code execution results | 1 hour | 22.4 |
| Leaderboard data | 5 minutes | 22.5 |
| Course content | 24 hours | 22.6 |
| SQL session databases | 30 minutes | Context |
| Rate limiting | 1 hour | Context |

## Deployment Steps

### 1. Deploy Redis Cluster

```bash
# Apply the Redis cluster configuration
kubectl apply -f redis-cluster.yaml

# Wait for all pods to be ready
kubectl wait --for=condition=ready pod -l app=redis-cluster -n aspnet-learning-platform --timeout=300s
```

### 2. Initialize the Cluster

After all pods are running, initialize the cluster:

**Linux/macOS:**
```bash
chmod +x init-redis-cluster.sh
./init-redis-cluster.sh
```

**Windows (PowerShell):**
```powershell
.\init-redis-cluster.ps1
```

This script will:
1. Wait for all Redis pods to be ready
2. Get the IP addresses of all pods
3. Create the cluster with 3 masters and 3 replicas
4. Verify the cluster status

### 3. Verify Cluster Status

```bash
# Check cluster info
kubectl exec -it redis-cluster-0 -n aspnet-learning-platform -- redis-cli cluster info

# Check cluster nodes
kubectl exec -it redis-cluster-0 -n aspnet-learning-platform -- redis-cli cluster nodes

# Check cluster health
kubectl exec -it redis-cluster-0 -n aspnet-learning-platform -- redis-cli --cluster check redis-cluster-0.redis-cluster.aspnet-learning-platform.svc.cluster.local:6379
```

Expected output for `cluster info`:
```
cluster_state:ok
cluster_slots_assigned:16384
cluster_slots_ok:16384
cluster_slots_pfail:0
cluster_slots_fail:0
cluster_known_nodes:6
cluster_size:3
```

## Persistence Configuration

The cluster is configured with both RDB and AOF persistence:

### RDB (Snapshots)
- Save after 900 seconds if at least 1 key changed
- Save after 300 seconds if at least 10 keys changed
- Save after 60 seconds if at least 10000 keys changed
- File: `/data/dump.rdb`

### AOF (Append Only File)
- Enabled with `appendonly yes`
- Fsync every second (`appendfsync everysec`)
- Auto-rewrite when file grows 100% and is at least 64MB
- File: `/data/appendonly.aof`

## C# Connection Pooling

The platform uses StackExchange.Redis with connection pooling configured in `Shared/Configuration/RedisConfiguration.cs`.

### Usage in Services

```csharp
// In Program.cs or Startup.cs
builder.Services.AddRedisCache(config =>
{
    config.ConnectionString = builder.Configuration.GetConnectionString("Redis") 
        ?? "redis-cluster-service.aspnet-learning-platform.svc.cluster.local:6379";
    config.UseCluster = true;
    config.ConnectTimeout = 5000;
    config.SyncTimeout = 5000;
    config.ConnectRetry = 3;
    config.AbortOnConnectFail = false;
});

// Inject and use the cache service
public class MyService
{
    private readonly IRedisCacheService _cache;
    
    public MyService(IRedisCacheService cache)
    {
        _cache = cache;
    }
    
    public async Task<string?> GetDataAsync(string key)
    {
        return await _cache.GetAsync<string>(key);
    }
    
    public async Task SetDataAsync(string key, string value)
    {
        await _cache.SetAsync(key, value, TimeSpan.FromHours(1));
    }
}
```

### Specialized Cache Services

The platform provides specialized cache services for different use cases:

1. **CodeExecutionCacheService**: Caches code execution results (1h TTL)
2. **LeaderboardCacheService**: Manages leaderboard data using sorted sets (5min TTL)
3. **RateLimitCacheService**: Implements rate limiting (10 req/hour free, 50 req/hour premium)

## Connection String Configuration

### Kubernetes (Internal)
```
redis-cluster-service.aspnet-learning-platform.svc.cluster.local:6379
```

### Local Development
```
localhost:6379
```

### Environment Variables

Set in `configmap.yaml` or `secrets.yaml`:
```yaml
REDIS_CONNECTION_STRING: "redis-cluster-service.aspnet-learning-platform.svc.cluster.local:6379"
```

## Monitoring

### Check Redis Metrics

```bash
# Memory usage
kubectl exec -it redis-cluster-0 -n aspnet-learning-platform -- redis-cli info memory

# Stats
kubectl exec -it redis-cluster-0 -n aspnet-learning-platform -- redis-cli info stats

# Replication
kubectl exec -it redis-cluster-0 -n aspnet-learning-platform -- redis-cli info replication
```

### Key Metrics to Monitor

- **Memory usage**: Should stay below 1GB per node (maxmemory configured)
- **Hit rate**: Should be above 70% (Requirement 22.10)
- **Connected clients**: Monitor for connection leaks
- **Evicted keys**: Should be minimal with proper TTL configuration
- **Replication lag**: Should be near zero

## Troubleshooting

### Cluster Not Forming

If the cluster doesn't form automatically:

```bash
# Get pod IPs
kubectl get pods -n aspnet-learning-platform -l app=redis-cluster -o wide

# Manually create cluster
kubectl exec -it redis-cluster-0 -n aspnet-learning-platform -- redis-cli --cluster create \
  <pod-0-ip>:6379 <pod-1-ip>:6379 <pod-2-ip>:6379 \
  <pod-3-ip>:6379 <pod-4-ip>:6379 <pod-5-ip>:6379 \
  --cluster-replicas 1 --cluster-yes
```

### Connection Issues

```bash
# Test connection from a pod
kubectl run redis-test --rm -it --image=redis:7-alpine -n aspnet-learning-platform -- \
  redis-cli -h redis-cluster-service -p 6379 ping

# Should return: PONG
```

### Data Persistence Issues

```bash
# Check if data directory is mounted
kubectl exec -it redis-cluster-0 -n aspnet-learning-platform -- ls -la /data

# Check RDB file
kubectl exec -it redis-cluster-0 -n aspnet-learning-platform -- ls -lh /data/dump.rdb

# Check AOF file
kubectl exec -it redis-cluster-0 -n aspnet-learning-platform -- ls -lh /data/appendonly.aof
```

### Performance Issues

```bash
# Check slow log
kubectl exec -it redis-cluster-0 -n aspnet-learning-platform -- redis-cli slowlog get 10

# Check latency
kubectl exec -it redis-cluster-0 -n aspnet-learning-platform -- redis-cli --latency

# Check memory fragmentation
kubectl exec -it redis-cluster-0 -n aspnet-learning-platform -- redis-cli info memory | grep fragmentation
```

## Scaling

To scale the cluster (add more master-replica pairs):

1. Update `replicas` in `redis-cluster.yaml`
2. Apply the changes: `kubectl apply -f redis-cluster.yaml`
3. Add new nodes to cluster:
   ```bash
   kubectl exec -it redis-cluster-0 -n aspnet-learning-platform -- redis-cli --cluster add-node <new-node-ip>:6379 <existing-node-ip>:6379
   ```
4. Rebalance the cluster:
   ```bash
   kubectl exec -it redis-cluster-0 -n aspnet-learning-platform -- redis-cli --cluster rebalance <any-node-ip>:6379
   ```

## Backup and Recovery

### Manual Backup

```bash
# Trigger RDB snapshot
kubectl exec -it redis-cluster-0 -n aspnet-learning-platform -- redis-cli BGSAVE

# Copy RDB file from pod
kubectl cp aspnet-learning-platform/redis-cluster-0:/data/dump.rdb ./backup-dump.rdb
```

### Restore from Backup

```bash
# Copy RDB file to pod
kubectl cp ./backup-dump.rdb aspnet-learning-platform/redis-cluster-0:/data/dump.rdb

# Restart pod to load data
kubectl delete pod redis-cluster-0 -n aspnet-learning-platform
```

## Security Considerations

1. **Network Policies**: Redis cluster is only accessible within the Kubernetes cluster
2. **No External Access**: ClusterIP service type prevents external connections
3. **Disabled Commands**: Dangerous commands (FLUSHDB, FLUSHALL, KEYS, SHUTDOWN) are disabled
4. **Protected Mode**: Disabled for internal cluster communication
5. **Authentication**: Can be enabled by setting password in ConfigMap

## Requirements Validation

This Redis cluster setup validates the following requirements:

- ✅ **22.1**: Cache code execution results keyed by code hash and input parameters
- ✅ **22.2**: Cache SQL query results keyed by query hash and database state
- ✅ **22.3**: Use Redis for distributed caching across microservices
- ✅ **22.4**: Set cache TTL to 1 hour for execution results
- ✅ **22.5**: Set cache TTL to 5 minutes for leaderboard data
- ✅ **22.6**: Set cache TTL to 24 hours for course content
- ✅ **22.7**: Return cached result without re-executing code
- ✅ **22.8**: Invalidate cache entries when related content is updated
- ✅ **22.9**: Implement cache warming for frequently accessed content
- ✅ **22.10**: Monitor cache hit rate and alert if it drops below 70%

## Additional Resources

- [Redis Cluster Tutorial](https://redis.io/docs/manual/scaling/)
- [StackExchange.Redis Documentation](https://stackexchange.github.io/StackExchange.Redis/)
- [Redis Persistence](https://redis.io/docs/manual/persistence/)
