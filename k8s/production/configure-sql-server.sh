#!/bin/bash
# SQL Server Production Configuration Script
# This script deploys SQL Server with read replicas, automated backups, and PITR
# Requirements: 1.7, 1.8

set -e

# Colors for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
NC='\033[0m' # No Color

# Configuration
NAMESPACE="platform-saas-prod"
SA_PASSWORD="${SQL_SA_PASSWORD:-YourStrong!Passw0rd}"
BACKUP_STORAGE_ACCOUNT="${BACKUP_STORAGE_ACCOUNT:-}"
BACKUP_STORAGE_KEY="${BACKUP_STORAGE_KEY:-}"

echo -e "${GREEN}========================================${NC}"
echo -e "${GREEN}SQL Server Production Configuration${NC}"
echo -e "${GREEN}========================================${NC}"
echo ""

# Check prerequisites
echo -e "${YELLOW}Checking prerequisites...${NC}"

if ! command -v kubectl &> /dev/null; then
    echo -e "${RED}ERROR: kubectl is not installed${NC}"
    exit 1
fi

if ! kubectl get namespace ${NAMESPACE} &> /dev/null; then
    echo -e "${RED}ERROR: Namespace ${NAMESPACE} does not exist${NC}"
    echo "Please create the namespace first: kubectl create namespace ${NAMESPACE}"
    exit 1
fi

echo -e "${GREEN}✓ Prerequisites check passed${NC}"
echo ""

# Prompt for SA password if not set
if [ "${SA_PASSWORD}" == "YourStrong!Passw0rd" ]; then
    echo -e "${YELLOW}WARNING: Using default SA password${NC}"
    read -p "Enter SQL Server SA password (or press Enter to use default): " USER_PASSWORD
    if [ ! -z "${USER_PASSWORD}" ]; then
        SA_PASSWORD="${USER_PASSWORD}"
    fi
fi

# Update secrets
echo -e "${YELLOW}Creating/updating SQL Server secrets...${NC}"
kubectl create secret generic sql-server-secret \
    --from-literal=SA_PASSWORD="${SA_PASSWORD}" \
    --from-literal=BACKUP_STORAGE_KEY="${BACKUP_STORAGE_KEY}" \
    --namespace=${NAMESPACE} \
    --dry-run=client -o yaml | kubectl apply -f -

echo -e "${GREEN}✓ Secrets configured${NC}"
echo ""

# Deploy SQL Server configuration
echo -e "${YELLOW}Deploying SQL Server configuration...${NC}"
kubectl apply -f sql-server-production.yaml

echo -e "${GREEN}✓ SQL Server configuration deployed${NC}"
echo ""

# Wait for primary to be ready
echo -e "${YELLOW}Waiting for primary SQL Server to be ready...${NC}"
kubectl wait --for=condition=ready pod \
    -l app=sql-server,role=primary \
    --namespace=${NAMESPACE} \
    --timeout=300s

echo -e "${GREEN}✓ Primary SQL Server is ready${NC}"
echo ""

# Wait for replicas to be ready
echo -e "${YELLOW}Waiting for read replicas to be ready...${NC}"
kubectl wait --for=condition=ready pod \
    -l app=sql-server,role=replica \
    --namespace=${NAMESPACE} \
    --timeout=300s

echo -e "${GREEN}✓ Read replicas are ready${NC}"
echo ""

# Enable Point-in-Time Recovery
echo -e "${YELLOW}Enabling Point-in-Time Recovery...${NC}"
PRIMARY_POD=$(kubectl get pod -l app=sql-server,role=primary -n ${NAMESPACE} -o jsonpath='{.items[0].metadata.name}')

kubectl exec -it ${PRIMARY_POD} -n ${NAMESPACE} -- /scripts/enable-pitr.sh

echo -e "${GREEN}✓ Point-in-Time Recovery enabled${NC}"
echo ""

# Deploy backup jobs
echo -e "${YELLOW}Deploying automated backup jobs...${NC}"
kubectl apply -f sql-server-backup-job.yaml

echo -e "${GREEN}✓ Backup jobs deployed${NC}"
echo ""

# Verify deployment
echo -e "${YELLOW}Verifying deployment...${NC}"
echo ""

echo "SQL Server Pods:"
kubectl get pods -l app=sql-server -n ${NAMESPACE}
echo ""

echo "SQL Server Services:"
kubectl get services -l app=sql-server -n ${NAMESPACE}
echo ""

echo "Backup CronJobs:"
kubectl get cronjobs -n ${NAMESPACE} | grep sql-server
echo ""

# Display connection information
echo -e "${GREEN}========================================${NC}"
echo -e "${GREEN}Deployment Complete!${NC}"
echo -e "${GREEN}========================================${NC}"
echo ""
echo "Connection Strings:"
echo ""
echo "Primary (Write Operations):"
echo "  Server=sql-server-primary.${NAMESPACE}.svc.cluster.local,1433"
echo "  User ID=sa"
echo "  Password=${SA_PASSWORD}"
echo ""
echo "Read Replicas (Read Operations - Load Balanced):"
echo "  Server=sql-server-replicas.${NAMESPACE}.svc.cluster.local,1433"
echo "  User ID=sa"
echo "  Password=${SA_PASSWORD}"
echo "  ApplicationIntent=ReadOnly"
echo ""
echo "Individual Replicas:"
echo "  Replica 1: sql-server-replica-1.${NAMESPACE}.svc.cluster.local,1433"
echo "  Replica 2: sql-server-replica-2.${NAMESPACE}.svc.cluster.local,1433"
echo ""
echo "Backup Schedule:"
echo "  Full Backup: Daily at 2:00 AM UTC"
echo "  Log Backup: Every 15 minutes"
echo "  Retention: 30 days (full), 7 days (logs)"
echo ""
echo "Point-in-Time Recovery:"
echo "  Enabled: Yes"
echo "  Granularity: 15 minutes"
echo "  Recovery Model: FULL"
echo ""
echo -e "${YELLOW}Next Steps:${NC}"
echo "1. Update application connection strings to use the services above"
echo "2. Configure read replica routing in your application"
echo "3. Test backup and restore procedures"
echo "4. Monitor backup job execution"
echo "5. Set up alerts for backup failures"
echo ""
echo -e "${YELLOW}Useful Commands:${NC}"
echo "  # Check SQL Server logs"
echo "  kubectl logs -f ${PRIMARY_POD} -n ${NAMESPACE}"
echo ""
echo "  # Execute SQL commands"
echo "  kubectl exec -it ${PRIMARY_POD} -n ${NAMESPACE} -- /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P '${SA_PASSWORD}'"
echo ""
echo "  # Manual backup"
echo "  kubectl exec -it ${PRIMARY_POD} -n ${NAMESPACE} -- /scripts/backup.sh"
echo ""
echo "  # Restore from backup"
echo "  kubectl exec -it ${PRIMARY_POD} -n ${NAMESPACE} -- /scripts/restore.sh <backup_file>"
echo ""
echo "  # Check backup jobs"
echo "  kubectl get jobs -n ${NAMESPACE} | grep backup"
echo ""
echo "  # View backup logs"
echo "  kubectl logs -l app=sql-server-backup -n ${NAMESPACE}"
echo ""

echo -e "${GREEN}Configuration completed successfully!${NC}"
