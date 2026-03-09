# SQL Server Production Configuration Script (PowerShell)
# This script deploys SQL Server with read replicas, automated backups, and PITR
# Requirements: 1.7, 1.8

param(
    [string]$Namespace = "platform-saas-prod",
    [string]$SAPassword = "",
    [string]$BackupStorageAccount = "",
    [string]$BackupStorageKey = ""
)

$ErrorActionPreference = "Stop"

Write-Host "========================================" -ForegroundColor Green
Write-Host "SQL Server Production Configuration" -ForegroundColor Green
Write-Host "========================================" -ForegroundColor Green
Write-Host ""

# Check prerequisites
Write-Host "Checking prerequisites..." -ForegroundColor Yellow

if (-not (Get-Command kubectl -ErrorAction SilentlyContinue)) {
    Write-Host "ERROR: kubectl is not installed" -ForegroundColor Red
    exit 1
}

try {
    kubectl get namespace $Namespace | Out-Null
} catch {
    Write-Host "ERROR: Namespace $Namespace does not exist" -ForegroundColor Red
    Write-Host "Please create the namespace first: kubectl create namespace $Namespace"
    exit 1
}

Write-Host "✓ Prerequisites check passed" -ForegroundColor Green
Write-Host ""

# Prompt for SA password if not provided
if ([string]::IsNullOrEmpty($SAPassword)) {
    $SecurePassword = Read-Host "Enter SQL Server SA password" -AsSecureString
    $BSTR = [System.Runtime.InteropServices.Marshal]::SecureStringToBSTR($SecurePassword)
    $SAPassword = [System.Runtime.InteropServices.Marshal]::PtrToStringAuto($BSTR)
    [System.Runtime.InteropServices.Marshal]::ZeroFreeBSTR($BSTR)
}

# Create/update secrets
Write-Host "Creating/updating SQL Server secrets..." -ForegroundColor Yellow

$secretYaml = @"
apiVersion: v1
kind: Secret
metadata:
  name: sql-server-secret
  namespace: $Namespace
type: Opaque
stringData:
  SA_PASSWORD: "$SAPassword"
  BACKUP_STORAGE_KEY: "$BackupStorageKey"
"@

$secretYaml | kubectl apply -f -

Write-Host "✓ Secrets configured" -ForegroundColor Green
Write-Host ""

# Deploy SQL Server configuration
Write-Host "Deploying SQL Server configuration..." -ForegroundColor Yellow
kubectl apply -f sql-server-production.yaml

Write-Host "✓ SQL Server configuration deployed" -ForegroundColor Green
Write-Host ""

# Wait for primary to be ready
Write-Host "Waiting for primary SQL Server to be ready..." -ForegroundColor Yellow
kubectl wait --for=condition=ready pod `
    -l app=sql-server,role=primary `
    --namespace=$Namespace `
    --timeout=300s

Write-Host "✓ Primary SQL Server is ready" -ForegroundColor Green
Write-Host ""

# Wait for replicas to be ready
Write-Host "Waiting for read replicas to be ready..." -ForegroundColor Yellow
kubectl wait --for=condition=ready pod `
    -l app=sql-server,role=replica `
    --namespace=$Namespace `
    --timeout=300s

Write-Host "✓ Read replicas are ready" -ForegroundColor Green
Write-Host ""

# Enable Point-in-Time Recovery
Write-Host "Enabling Point-in-Time Recovery..." -ForegroundColor Yellow
$primaryPod = kubectl get pod -l app=sql-server,role=primary -n $Namespace -o jsonpath='{.items[0].metadata.name}'

kubectl exec -it $primaryPod -n $Namespace -- /scripts/enable-pitr.sh

Write-Host "✓ Point-in-Time Recovery enabled" -ForegroundColor Green
Write-Host ""

# Deploy backup jobs
Write-Host "Deploying automated backup jobs..." -ForegroundColor Yellow
kubectl apply -f sql-server-backup-job.yaml

Write-Host "✓ Backup jobs deployed" -ForegroundColor Green
Write-Host ""

# Verify deployment
Write-Host "Verifying deployment..." -ForegroundColor Yellow
Write-Host ""

Write-Host "SQL Server Pods:"
kubectl get pods -l app=sql-server -n $Namespace
Write-Host ""

Write-Host "SQL Server Services:"
kubectl get services -l app=sql-server -n $Namespace
Write-Host ""

Write-Host "Backup CronJobs:"
kubectl get cronjobs -n $Namespace | Select-String "sql-server"
Write-Host ""

# Display connection information
Write-Host "========================================" -ForegroundColor Green
Write-Host "Deployment Complete!" -ForegroundColor Green
Write-Host "========================================" -ForegroundColor Green
Write-Host ""
Write-Host "Connection Strings:"
Write-Host ""
Write-Host "Primary (Write Operations):"
Write-Host "  Server=sql-server-primary.$Namespace.svc.cluster.local,1433"
Write-Host "  User ID=sa"
Write-Host "  Password=$SAPassword"
Write-Host ""
Write-Host "Read Replicas (Read Operations - Load Balanced):"
Write-Host "  Server=sql-server-replicas.$Namespace.svc.cluster.local,1433"
Write-Host "  User ID=sa"
Write-Host "  Password=$SAPassword"
Write-Host "  ApplicationIntent=ReadOnly"
Write-Host ""
Write-Host "Individual Replicas:"
Write-Host "  Replica 1: sql-server-replica-1.$Namespace.svc.cluster.local,1433"
Write-Host "  Replica 2: sql-server-replica-2.$Namespace.svc.cluster.local,1433"
Write-Host ""
Write-Host "Backup Schedule:"
Write-Host "  Full Backup: Daily at 2:00 AM UTC"
Write-Host "  Log Backup: Every 15 minutes"
Write-Host "  Retention: 30 days (full), 7 days (logs)"
Write-Host ""
Write-Host "Point-in-Time Recovery:"
Write-Host "  Enabled: Yes"
Write-Host "  Granularity: 15 minutes"
Write-Host "  Recovery Model: FULL"
Write-Host ""
Write-Host "Next Steps:" -ForegroundColor Yellow
Write-Host "1. Update application connection strings to use the services above"
Write-Host "2. Configure read replica routing in your application"
Write-Host "3. Test backup and restore procedures"
Write-Host "4. Monitor backup job execution"
Write-Host "5. Set up alerts for backup failures"
Write-Host ""
Write-Host "Useful Commands:" -ForegroundColor Yellow
Write-Host "  # Check SQL Server logs"
Write-Host "  kubectl logs -f $primaryPod -n $Namespace"
Write-Host ""
Write-Host "  # Execute SQL commands"
Write-Host "  kubectl exec -it $primaryPod -n $Namespace -- /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P '$SAPassword'"
Write-Host ""
Write-Host "  # Manual backup"
Write-Host "  kubectl exec -it $primaryPod -n $Namespace -- /scripts/backup.sh"
Write-Host ""
Write-Host "  # Restore from backup"
Write-Host "  kubectl exec -it $primaryPod -n $Namespace -- /scripts/restore.sh <backup_file>"
Write-Host ""
Write-Host "  # Check backup jobs"
Write-Host "  kubectl get jobs -n $Namespace | Select-String 'backup'"
Write-Host ""
Write-Host "  # View backup logs"
Write-Host "  kubectl logs -l app=sql-server-backup -n $Namespace"
Write-Host ""

Write-Host "Configuration completed successfully!" -ForegroundColor Green
