# Application Insights Setup Guide

This guide walks you through setting up Application Insights for the platform's microservices.

## Prerequisites

- Azure subscription
- Azure CLI installed
- kubectl configured for your Kubernetes cluster
- Access to the platform's source code

## Step 1: Create Application Insights Resource

### Option A: Using Azure Portal

1. Navigate to [Azure Portal](https://portal.azure.com)
2. Click "Create a resource"
3. Search for "Application Insights"
4. Click "Create"
5. Fill in the details:
   - **Subscription**: Select your subscription
   - **Resource Group**: `platform-saas-rg` (or create new)
   - **Name**: `platform-saas-insights`
   - **Region**: `East US` (or your preferred region)
   - **Resource Mode**: `Workspace-based`
6. Click "Review + Create"
7. Click "Create"

### Option B: Using Azure CLI

```bash
# Create resource group if it doesn't exist
az group create \
  --name platform-saas-rg \
  --location eastus

# Create Log Analytics workspace (required for workspace-based Application Insights)
az monitor log-analytics workspace create \
  --resource-group platform-saas-rg \
  --workspace-name platform-saas-logs \
  --location eastus

# Get workspace ID
WORKSPACE_ID=$(az monitor log-analytics workspace show \
  --resource-group platform-saas-rg \
  --workspace-name platform-saas-logs \
  --query id -o tsv)

# Create Application Insights resource
az monitor app-insights component create \
  --app platform-saas-insights \
  --location eastus \
  --resource-group platform-saas-rg \
  --application-type web \
  --workspace $WORKSPACE_ID

# Get connection string
CONNECTION_STRING=$(az monitor app-insights component show \
  --app platform-saas-insights \
  --resource-group platform-saas-rg \
  --query connectionString -o tsv)

echo "Connection String: $CONNECTION_STRING"
```

## Step 2: Configure Kubernetes Secrets

### Create the Secret

```bash
# Replace with your actual connection string
CONNECTION_STRING="InstrumentationKey=...;IngestionEndpoint=https://...;LiveEndpoint=https://..."

# Create namespace if it doesn't exist
kubectl create namespace platform-saas

# Create secret
kubectl create secret generic appinsights-secret \
  --from-literal=connection-string="$CONNECTION_STRING" \
  --namespace=platform-saas
```

### Verify the Secret

```bash
kubectl get secret appinsights-secret -n platform-saas
kubectl describe secret appinsights-secret -n platform-saas
```

## Step 3: Apply Kubernetes Configuration

```bash
# Apply Application Insights configuration
kubectl apply -f k8s/appinsights-config.yaml

# Verify ConfigMap
kubectl get configmap appinsights-config -n platform-saas
kubectl describe configmap appinsights-config -n platform-saas
```

## Step 4: Update Service Deployments

Each service needs to be updated to use Application Insights. Here's an example for the Auth Service:

### Update Program.cs

```csharp
using Shared.Telemetry;
using Shared.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add Application Insights telemetry
builder.Services.AddApplicationInsightsTelemetry(
    builder.Configuration,
    "AuthService"
);

// Add HTTP context accessor for correlation
builder.Services.AddHttpContextAccessor();

// Add custom telemetry tracker
builder.Services.AddSingleton<ICustomTelemetryTracker, CustomTelemetryTracker>();

// ... other service configuration

var app = builder.Build();

// Add correlation ID middleware (must be early in pipeline)
app.UseMiddleware<CorrelationIdMiddleware>();

// ... other middleware

app.Run();
```

### Update Deployment YAML

Ensure your deployment includes the Application Insights environment variables:

```yaml
env:
- name: ApplicationInsights__ConnectionString
  valueFrom:
    secretKeyRef:
      name: appinsights-secret
      key: connection-string

- name: SERVICE_NAME
  value: "AuthService"

envFrom:
- configMapRef:
    name: appinsights-config
```

## Step 5: Deploy Services

```bash
# Deploy all services
kubectl apply -f k8s/

# Check deployment status
kubectl get deployments -n platform-saas
kubectl get pods -n platform-saas

# Check logs to verify Application Insights is working
kubectl logs -n platform-saas -l app=auth-service --tail=50
```

## Step 6: Verify Telemetry

### Check Application Insights Portal

1. Navigate to your Application Insights resource in Azure Portal
2. Click on "Live Metrics" to see real-time telemetry
3. Click on "Transaction search" to see individual requests
4. Click on "Application map" to see service dependencies

### Query Telemetry Data

Use Kusto Query Language (KQL) to query telemetry:

```kusto
// View all requests in the last hour
requests
| where timestamp > ago(1h)
| project timestamp, name, url, duration, resultCode, customDimensions
| order by timestamp desc

// View requests by service
requests
| where timestamp > ago(1h)
| summarize count() by tostring(customDimensions.ServiceName)

// View slow requests (>2 seconds)
requests
| where timestamp > ago(1h)
| where duration > 2000
| project timestamp, name, url, duration, customDimensions.ServiceName
| order by duration desc

// View requests by correlation ID
union requests, dependencies, traces
| where customDimensions.CorrelationId == "your-correlation-id"
| order by timestamp asc
| project timestamp, itemType, name, operation_Name

// View custom events
customEvents
| where timestamp > ago(1h)
| where name == "CodeExecution"
| project timestamp, name, customDimensions, customMeasurements
| order by timestamp desc

// View compilation errors
customEvents
| where timestamp > ago(1h)
| where name == "CompilationError"
| summarize count() by tostring(customDimensions.ErrorType)
| order by count_ desc
```

## Step 7: Create Dashboards

### Create Performance Dashboard

1. In Application Insights, click "Dashboards"
2. Click "New dashboard"
3. Add tiles for:
   - Average response time
   - Request rate
   - Failed requests
   - Server response time
   - Dependency calls
   - Custom metrics (code execution time, etc.)

### Example Dashboard Queries

**Average Response Time by Endpoint**:
```kusto
requests
| where timestamp > ago(1h)
| summarize avg(duration) by name
| order by avg_duration desc
| render barchart
```

**Request Rate Over Time**:
```kusto
requests
| where timestamp > ago(24h)
| summarize count() by bin(timestamp, 5m)
| render timechart
```

**Error Rate**:
```kusto
requests
| where timestamp > ago(1h)
| summarize 
    Total = count(),
    Errors = countif(success == false)
| extend ErrorRate = (Errors * 100.0) / Total
| project ErrorRate
```

## Step 8: Configure Alerts

### Create Alert for High Error Rate

```bash
# Using Azure CLI
az monitor metrics alert create \
  --name "High Error Rate" \
  --resource-group platform-saas-rg \
  --scopes "/subscriptions/{subscription-id}/resourceGroups/platform-saas-rg/providers/microsoft.insights/components/platform-saas-insights" \
  --condition "count requests/failed > 5" \
  --window-size 5m \
  --evaluation-frequency 1m \
  --action-group-ids "/subscriptions/{subscription-id}/resourceGroups/platform-saas-rg/providers/microsoft.insights/actionGroups/platform-alerts"
```

### Create Alert for Slow Response Time

```bash
az monitor metrics alert create \
  --name "Slow Response Time" \
  --resource-group platform-saas-rg \
  --scopes "/subscriptions/{subscription-id}/resourceGroups/platform-saas-rg/providers/microsoft.insights/components/platform-saas-insights" \
  --condition "avg requests/duration > 2000" \
  --window-size 5m \
  --evaluation-frequency 1m \
  --action-group-ids "/subscriptions/{subscription-id}/resourceGroups/platform-saas-rg/providers/microsoft.insights/actionGroups/platform-alerts"
```

### Create Action Group for Notifications

```bash
az monitor action-group create \
  --name platform-alerts \
  --resource-group platform-saas-rg \
  --short-name PlatformAlerts \
  --email-receiver name=admin email=admin@example.com
```

## Step 9: Configure Continuous Export (Optional)

For long-term storage and advanced analytics:

```bash
# Create storage account
az storage account create \
  --name platformsaastelemetry \
  --resource-group platform-saas-rg \
  --location eastus \
  --sku Standard_LRS

# Get storage account key
STORAGE_KEY=$(az storage account keys list \
  --account-name platformsaastelemetry \
  --resource-group platform-saas-rg \
  --query '[0].value' -o tsv)

# Configure continuous export (via Portal or ARM template)
# This exports telemetry to blob storage for long-term retention
```

## Step 10: Monitor Costs

Application Insights charges based on data ingestion volume:

### Check Current Usage

```bash
# View data ingestion in the last 30 days
az monitor app-insights component billing show \
  --app platform-saas-insights \
  --resource-group platform-saas-rg
```

### Set Daily Cap

1. In Azure Portal, navigate to your Application Insights resource
2. Click "Usage and estimated costs"
3. Click "Daily cap"
4. Set a daily cap (e.g., 10 GB/day)
5. Configure alert when 90% of cap is reached

### Optimize Costs

- Enable adaptive sampling (already configured)
- Filter out health check requests (already configured)
- Use aggregated metrics instead of individual events
- Set appropriate data retention (default is 90 days)

## Troubleshooting

### No Telemetry Appearing

1. **Check connection string**:
   ```bash
   kubectl get secret appinsights-secret -n platform-saas -o jsonpath='{.data.connection-string}' | base64 -d
   ```

2. **Check pod logs**:
   ```bash
   kubectl logs -n platform-saas -l app=auth-service --tail=100
   ```

3. **Verify network connectivity**:
   ```bash
   kubectl exec -n platform-saas -it <pod-name> -- curl -v https://dc.services.visualstudio.com/v2/track
   ```

4. **Check sampling settings**: Sampling may be filtering out data

### High Latency in Telemetry

- Application Insights uses batching and may have a delay of 1-2 minutes
- Check "Live Metrics" for real-time data
- Verify network connectivity and bandwidth

### Missing Correlation IDs

1. Ensure `CorrelationIdMiddleware` is registered in all services
2. Verify `HttpContextAccessor` is registered
3. Check that `X-Correlation-ID` header is being propagated

## Best Practices

1. **Use Structured Logging**: Always use structured properties
2. **Avoid PII**: Never log sensitive personal information
3. **Monitor Costs**: Set up billing alerts
4. **Use Sampling**: Enable adaptive sampling for high-volume services
5. **Create Dashboards**: Create dashboards for key metrics
6. **Set Up Alerts**: Configure alerts for critical conditions
7. **Regular Review**: Review telemetry data regularly to identify issues

## Next Steps

- Set up custom dashboards for your specific KPIs
- Configure alerts for critical metrics
- Integrate with Azure DevOps or GitHub Actions for deployment tracking
- Set up availability tests for critical endpoints
- Configure smart detection for anomaly detection

## References

- [Application Insights Documentation](https://docs.microsoft.com/en-us/azure/azure-monitor/app/app-insights-overview)
- [Kusto Query Language (KQL)](https://docs.microsoft.com/en-us/azure/data-explorer/kusto/query/)
- [Application Insights Pricing](https://azure.microsoft.com/en-us/pricing/details/monitor/)
- [Distributed Tracing](https://docs.microsoft.com/en-us/azure/azure-monitor/app/distributed-tracing)
