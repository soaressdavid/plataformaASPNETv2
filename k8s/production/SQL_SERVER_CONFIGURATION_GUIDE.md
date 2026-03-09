# SQL Server Production Configuration Guide

This guide provides comprehensive instructions for deploying and managing SQL Server in production with read replicas, automated backups, and point-in-time recovery capabilities.

## Overview

The production SQL Server deployment includes:

- **Primary SQL Server**: Handles all write operations
- **2 Read Replicas**: Distribute read operations for improved performance
- **Automated Daily Backups**: Full database backups with 30-day retention
- **Transaction Log Backups**: Every 15 minutes for point-in-time recovery
- **Point-in-Time Recovery (PITR)**: Restore to any point within the last 30 days
- **High Availability**: StatefulSets with persistent storage
- **Monitoring**: Health checks and backup verification

## Requirements Satisfied

- **Requirement 1.7**: Create indexes on Users table for Email and Username fields
- **Requirement 1.8**: Create indexes on Lessons table for CourseId and Order fields
- **Additional**: Automated backups, read replicas, and point-in-time recovery

## Architecture

```
┌─────────────────────────────────────────────────────────────┐
│                     Application Layer                        │
│  ┌──────────────┐              ┌──────────────┐            │
│  │ Write Ops    │              │  Read Ops    │            │
│  │ (INSERT,     │              │  (SELECT)    │            │
│  │  UPDATE,     │              │              │            │
│  │  DELETE)     │              │              │            │
│  └──────┬───────┘              └──────┬───────┘            │
└─────────┼──────────────────────────────┼──────────────────┘
          │                              │
          ▼                              ▼
┌─────────────────┐          ┌─────────────────────────┐
│  Primary SQL    │          │  Read Replicas Service  │
│  Server Service │          │  (Load Balanced)        │
│  (Write)        │          │                         │
└────────┬────────┘          └────────┬────────────────┘
         │                            │
         │                   ┌────────┴────────┐
         │                   │                 │
         ▼                   ▼                 ▼
┌─────────────────┐  ┌─────────────┐  ┌─────────────┐
│ SQL Server      │  │ SQL Server  │  │ SQL Server  │
│ Primary Pod     │  │ Replica 1   │  │ Replica 2   │
│                 │  │ Pod         │  │ Pod         │
│ - Write Ops     │  │ - Read Only │  │ - Read Only │
│ - Backups       │  │             │  │             │
│ - PITR          │  │             │  │             │
└─────────────────┘  └─────────────┘  └─────────────┘
         │
         ▼
┌─────────────────────────────────────────────────────┐
│              Backup Storage                          │
│  - Full Backups (Daily, 30-day retention)          │
│  - Log Backups (Every 15 min, 7-day retention)     │
│  - Cloud Storage (Azure Blob / S3)                 │
└─────────────────────────────────────────────────────┘
```

## Prerequisites

1. **Kubernetes Cluster** (v1.24+)
   - AKS, EKS, or GKE recommended for production
   - Minimum 3 nodes with database workload labels

2. **Storage Classes**
   - `premium-ssd`: For database data (high IOPS)
   - `standard-ssd`: For backups

3. **kubectl** CLI tool installed and configured

4. **Namespace** created:
   ```bash
   kubectl create namespace platform-saas-prod
   ```

5. **Node Pool** for database workloads (recommended):
   - Memory-optimized instances (e.g., E8s_v3, r5.2xlarge)
   - Minimum 8 CPU cores, 32GB RAM per node
   - Labeled with `workload=database`

## Quick Start

### Option 1: Automated Deployment (Recommended)

#### Linux/macOS:
```bash
cd k8s/production
chmod +x configure-sql-server.sh
./configure-sql-server.sh
```

#### Windows (PowerShell):
```powershell
cd k8s\production
.\configure-sql-server.ps1
```

The script will:
1. Validate prerequisites
2. Create secrets
3. Deploy SQL Server primary and replicas
4. Enable point-in-time recovery
5. Configure automated backups
6. Display connection information

### Option 2: Manual Deployment

#### Step 1: Create Secrets

```bash
# Generate a strong SA password
SA_PASSWORD="YourStrong!Passw0rd123"

# Create secret
kubectl create secret generic sql-server-secret \
  --from-literal=SA_PASSWORD="${SA_PASSWORD}" \
  --from-literal=BACKUP_STORAGE_KEY="" \
  --namespace=platform-saas-prod
```

#### Step 2: Deploy SQL Server

```bash
kubectl apply -f sql-server-production.yaml
```

#### Step 3: Wait for Pods to be Ready

```bash
# Wait for primary
kubectl wait --for=condition=ready pod \
  -l app=sql-server,role=primary \
  --namespace=platform-saas-prod \
  --timeout=300s

# Wait for replicas
kubectl wait --for=condition=ready pod \
  -l app=sql-server,role=replica \
  --namespace=platform-saas-prod \
  --timeout=300s
```

#### Step 4: Enable Point-in-Time Recovery

```bash
PRIMARY_POD=$(kubectl get pod -l app=sql-server,role=primary \
  -n platform-saas-prod -o jsonpath='{.items[0].metadata.name}')

kubectl exec -it ${PRIMARY_POD} -n platform-saas-prod -- /scripts/enable-pitr.sh
```

#### Step 5: Deploy Backup Jobs

```bash
kubectl apply -f sql-server-backup-job.yaml
```

## Configuration

### Connection Strings

#### Primary (Write Operations)

```csharp
// C# Connection String
"Server=sql-server-primary.platform-saas-prod.svc.cluster.local,1433;Database=PlatformSaaS;User ID=sa;Password=YourPassword;TrustServerCertificate=True;"
```

```json
// appsettings.json
{
  "ConnectionStrings": {
    "Primary": "Server=sql-server-primary.platform-saas-prod.svc.cluster.local,1433;Database=PlatformSaaS;User ID=sa;Password=YourPassword;TrustServerCertificate=True;"
  }
}
```

#### Read Replicas (Read Operations - Load Balanced)

```csharp
// C# Connection String with ApplicationIntent
"Server=sql-server-replicas.platform-saas-prod.svc.cluster.local,1433;Database=PlatformSaaS;User ID=sa;Password=YourPassword;ApplicationIntent=ReadOnly;TrustServerCertificate=True;"
```

```json
// appsettings.json
{
  "ConnectionStrings": {
    "Primary": "Server=sql-server-primary.platform-saas-prod.svc.cluster.local,1433;...",
    "ReadReplicas": [
      "Server=sql-server-replica-1.platform-saas-prod.svc.cluster.local,1433;...",
      "Server=sql-server-replica-2.platform-saas-prod.svc.cluster.local,1433;..."
    ]
  }
}
```

### Application Configuration

#### Using Read Replicas in C#

```csharp
// Startup.cs or Program.cs
public void ConfigureServices(IServiceCollection services)
{
    // Primary connection for write operations
    services.AddSqlServerDbContext(Configuration);
    
    // OR with read replicas
    services.AddSqlServerDbContextWithReadReplicas(Configuration);
}
```

#### appsettings.Production.json

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=sql-server-primary.platform-saas-prod.svc.cluster.local,1433;Database=PlatformSaaS;User ID=sa;Password=${SQL_SA_PASSWORD};TrustServerCertificate=True;",
    "ReadReplicas": [
      "Server=sql-server-replica-1.platform-saas-prod.svc.cluster.local,1433;Database=PlatformSaaS;User ID=sa;Password=${SQL_SA_PASSWORD};ApplicationIntent=ReadOnly;TrustServerCertificate=True;",
      "Server=sql-server-replica-2.platform-saas-prod.svc.cluster.local,1433;Database=PlatformSaaS;User ID=sa;Password=${SQL_SA_PASSWORD};ApplicationIntent=ReadOnly;TrustServerCertificate=True;"
    ]
  },
  "Database": {
    "CommandTimeout": 30,
    "EnableRetryOnFailure": true,
    "MaxRetryCount": 3,
    "MaxRetryDelay": "00:00:30"
  }
}
```

## Backup and Recovery

### Automated Backups

#### Full Backups
- **Schedule**: Daily at 2:00 AM UTC
- **Retention**: 30 days
- **Location**: `/var/opt/mssql/backup` (persistent volume)
- **Cloud Upload**: Optional (Azure Blob Storage or S3)
- **Compression**: Enabled
- **Verification**: Automatic after each backup

#### Transaction Log Backups
- **Schedule**: Every 15 minutes
- **Retention**: 7 days
- **Purpose**: Point-in-time recovery
- **Granularity**: 15-minute intervals

### Manual Backup

```bash
# Get primary pod name
PRIMARY_POD=$(kubectl get pod -l app=sql-server,role=primary \
  -n platform-saas-prod -o jsonpath='{.items[0].metadata.name}')

# Execute manual backup
kubectl exec -it ${PRIMARY_POD} -n platform-saas-prod -- /scripts/backup.sh
```

### Restore from Backup

#### Full Restore

```bash
# List available backups
kubectl exec -it ${PRIMARY_POD} -n platform-saas-prod -- \
  ls -lh /var/opt/mssql/backup/*.bak

# Restore from specific backup
kubectl exec -it ${PRIMARY_POD} -n platform-saas-prod -- \
  /scripts/restore.sh PlatformSaaS_Full_20240116_020000.bak
```

#### Point-in-Time Restore

```bash
# Restore to specific date/time
kubectl exec -it ${PRIMARY_POD} -n platform-saas-prod -- \
  /scripts/restore.sh PlatformSaaS_Full_20240116_020000.bak "2024-01-16 14:30:00"
```

### Disaster Recovery Procedure

1. **Identify Recovery Point**
   ```bash
   # List available backups
   kubectl exec -it ${PRIMARY_POD} -n platform-saas-prod -- \
     ls -lh /var/opt/mssql/backup/
   ```

2. **Stop Application Services**
   ```bash
   kubectl scale deployment api-gateway --replicas=0 -n platform-saas-prod
   # Scale down other services...
   ```

3. **Restore Database**
   ```bash
   kubectl exec -it ${PRIMARY_POD} -n platform-saas-prod -- \
     /scripts/restore.sh <backup_file> [point_in_time]
   ```

4. **Verify Data Integrity**
   ```bash
   kubectl exec -it ${PRIMARY_POD} -n platform-saas-prod -- \
     /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P "${SA_PASSWORD}" \
     -Q "SELECT COUNT(*) FROM Users; SELECT COUNT(*) FROM Lessons;"
   ```

5. **Restart Application Services**
   ```bash
   kubectl scale deployment api-gateway --replicas=5 -n platform-saas-prod
   # Scale up other services...
   ```

## Monitoring and Maintenance

### Check SQL Server Status

```bash
# Check pods
kubectl get pods -l app=sql-server -n platform-saas-prod

# Check services
kubectl get services -l app=sql-server -n platform-saas-prod

# Check persistent volumes
kubectl get pvc -n platform-saas-prod | grep sql
```

### View Logs

```bash
# Primary SQL Server logs
kubectl logs -f sql-server-primary-0 -n platform-saas-prod

# Replica logs
kubectl logs -f sql-server-replica-1-0 -n platform-saas-prod

# Backup job logs
kubectl logs -l app=sql-server-backup -n platform-saas-prod --tail=100
```

### Execute SQL Commands

```bash
# Interactive SQL session
kubectl exec -it ${PRIMARY_POD} -n platform-saas-prod -- \
  /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P "${SA_PASSWORD}"

# Execute single query
kubectl exec -it ${PRIMARY_POD} -n platform-saas-prod -- \
  /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P "${SA_PASSWORD}" \
  -Q "SELECT name, state_desc FROM sys.databases;"
```

### Monitor Backup Jobs

```bash
# List backup jobs
kubectl get jobs -n platform-saas-prod | grep backup

# Check CronJob schedule
kubectl get cronjobs -n platform-saas-prod

# View recent backup logs
kubectl logs -l app=sql-server-backup -n platform-saas-prod --tail=50
```

### Database Performance Monitoring

```sql
-- Check database size
SELECT 
    name AS DatabaseName,
    size * 8 / 1024 AS SizeMB,
    max_size * 8 / 1024 AS MaxSizeMB
FROM sys.master_files
WHERE database_id = DB_ID('PlatformSaaS');

-- Check index fragmentation
SELECT 
    OBJECT_NAME(ips.object_id) AS TableName,
    i.name AS IndexName,
    ips.avg_fragmentation_in_percent
FROM sys.dm_db_index_physical_stats(DB_ID('PlatformSaaS'), NULL, NULL, NULL, 'LIMITED') ips
JOIN sys.indexes i ON ips.object_id = i.object_id AND ips.index_id = i.index_id
WHERE ips.avg_fragmentation_in_percent > 10
ORDER BY ips.avg_fragmentation_in_percent DESC;

-- Check backup history
SELECT TOP 10
    BackupDate,
    BackupType,
    BackupFile,
    BackupSize,
    Status
FROM [PlatformSaaS].[dbo].[BackupLog]
ORDER BY BackupDate DESC;
```

## Scaling

### Vertical Scaling (Increase Resources)

Edit `sql-server-production.yaml` and update resource limits:

```yaml
resources:
  requests:
    memory: "16Gi"  # Increased from 8Gi
    cpu: "8"        # Increased from 4
  limits:
    memory: "32Gi"  # Increased from 16Gi
    cpu: "16"       # Increased from 8
```

Apply changes:
```bash
kubectl apply -f sql-server-production.yaml
```

### Horizontal Scaling (Add More Replicas)

Add additional read replica:

```bash
# Scale replica StatefulSet
kubectl scale statefulset sql-server-replica-1 --replicas=2 -n platform-saas-prod
```

Or create a new replica StatefulSet (sql-server-replica-3) by copying the replica-2 configuration.

## Security Best Practices

### 1. Change Default SA Password

```bash
# Generate strong password
NEW_PASSWORD=$(openssl rand -base64 32)

# Update secret
kubectl create secret generic sql-server-secret \
  --from-literal=SA_PASSWORD="${NEW_PASSWORD}" \
  --namespace=platform-saas-prod \
  --dry-run=client -o yaml | kubectl apply -f -

# Restart pods to apply new password
kubectl rollout restart statefulset sql-server-primary -n platform-saas-prod
```

### 2. Enable TLS/SSL

```sql
-- Generate certificate
CREATE CERTIFICATE SQLServerCert
WITH SUBJECT = 'SQL Server Certificate',
EXPIRY_DATE = '2025-12-31';

-- Enable encryption
ALTER DATABASE [PlatformSaaS] SET ENCRYPTION ON;
```

### 3. Implement Network Policies

```yaml
apiVersion: networking.k8s.io/v1
kind: NetworkPolicy
metadata:
  name: sql-server-network-policy
  namespace: platform-saas-prod
spec:
  podSelector:
    matchLabels:
      app: sql-server
  policyTypes:
    - Ingress
  ingress:
    - from:
        - podSelector:
            matchLabels:
              app: api-gateway
        - podSelector:
            matchLabels:
              app: gamification
      ports:
        - protocol: TCP
          port: 1433
```

### 4. Regular Security Audits

```sql
-- Check failed login attempts
SELECT 
    event_time,
    server_principal_name,
    client_ip
FROM sys.fn_get_audit_file('/var/opt/mssql/audit/*.sqlaudit', DEFAULT, DEFAULT)
WHERE action_id = 'LGIF'  -- Login Failed
ORDER BY event_time DESC;

-- Review user permissions
SELECT 
    dp.name AS UserName,
    dp.type_desc AS UserType,
    o.name AS ObjectName,
    p.permission_name,
    p.state_desc
FROM sys.database_permissions p
JOIN sys.database_principals dp ON p.grantee_principal_id = dp.principal_id
LEFT JOIN sys.objects o ON p.major_id = o.object_id
WHERE dp.name NOT IN ('public', 'guest');
```

## Troubleshooting

### Pod Not Starting

```bash
# Check pod status
kubectl describe pod sql-server-primary-0 -n platform-saas-prod

# Check events
kubectl get events -n platform-saas-prod --sort-by='.lastTimestamp'

# Check logs
kubectl logs sql-server-primary-0 -n platform-saas-prod
```

### Connection Issues

```bash
# Test connectivity from another pod
kubectl run -it --rm debug --image=mcr.microsoft.com/mssql-tools \
  --restart=Never -n platform-saas-prod -- \
  /opt/mssql-tools/bin/sqlcmd -S sql-server-primary -U sa -P "${SA_PASSWORD}" -Q "SELECT 1"
```

### Backup Failures

```bash
# Check backup job logs
kubectl logs -l app=sql-server-backup -n platform-saas-prod

# Check disk space
kubectl exec -it ${PRIMARY_POD} -n platform-saas-prod -- df -h

# Manually trigger backup
kubectl create job --from=cronjob/sql-server-backup manual-backup-$(date +%s) -n platform-saas-prod
```

### Performance Issues

```sql
-- Check active connections
SELECT 
    DB_NAME(dbid) AS DatabaseName,
    COUNT(dbid) AS NumberOfConnections,
    loginame AS LoginName
FROM sys.sysprocesses
WHERE dbid > 0
GROUP BY dbid, loginame;

-- Check blocking queries
SELECT 
    blocking_session_id,
    session_id,
    wait_type,
    wait_time,
    wait_resource
FROM sys.dm_exec_requests
WHERE blocking_session_id <> 0;

-- Check long-running queries
SELECT 
    r.session_id,
    r.status,
    r.command,
    r.cpu_time,
    r.total_elapsed_time,
    t.text AS query_text
FROM sys.dm_exec_requests r
CROSS APPLY sys.dm_exec_sql_text(r.sql_handle) t
WHERE r.total_elapsed_time > 10000  -- 10 seconds
ORDER BY r.total_elapsed_time DESC;
```

## Cost Optimization

### Storage Costs

- **Premium SSD**: ~$0.135/GB/month (data)
- **Standard SSD**: ~$0.05/GB/month (backups)
- **Total**: ~$120/month for 500GB data + 1TB backups

### Compute Costs

- **3 Pods** (8 CPU, 16GB RAM each): ~$600/month
- **Spot Instances**: Save up to 70% for non-primary replicas

### Optimization Tips

1. **Use Standard SSD for backups** instead of Premium
2. **Enable compression** for backups (already configured)
3. **Archive old backups** to cheaper storage (Azure Cool/Archive tier)
4. **Use spot instances** for read replicas
5. **Right-size resources** based on actual usage

## References

- [SQL Server on Kubernetes](https://docs.microsoft.com/en-us/sql/linux/sql-server-linux-kubernetes-deploy)
- [SQL Server Backup and Restore](https://docs.microsoft.com/en-us/sql/relational-databases/backup-restore/back-up-and-restore-of-sql-server-databases)
- [Point-in-Time Recovery](https://docs.microsoft.com/en-us/sql/relational-databases/backup-restore/restore-a-sql-server-database-to-a-point-in-time-full-recovery-model)
- [SQL Server Always On](https://docs.microsoft.com/en-us/sql/database-engine/availability-groups/windows/overview-of-always-on-availability-groups-sql-server)

## Support

For issues or questions:
- Platform Team: platform-team@example.com
- Documentation: [Main README](../../README.md)
- SQL Server Docs: https://docs.microsoft.com/en-us/sql/

---

**Last Updated**: January 2024
**Version**: 1.0
**Requirements**: 1.7, 1.8
