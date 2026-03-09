# CI/CD Pipeline Documentation

## Overview

This document describes the Continuous Integration and Continuous Deployment (CI/CD) pipeline for the ASP.NET Learning Platform. The pipeline uses GitHub Actions for CI/CD orchestration and ArgoCD for GitOps-based Kubernetes deployments.

## Architecture

```
┌─────────────┐     ┌──────────────┐     ┌─────────────┐     ┌──────────────┐
│   GitHub    │────▶│GitHub Actions│────▶│  Container  │────▶│   ArgoCD     │
│ Repository  │     │   (CI/CD)    │     │  Registry   │     │  (GitOps)    │
└─────────────┘     └──────────────┘     └─────────────┘     └──────────────┘
                            │                                         │
                            ▼                                         ▼
                    ┌──────────────┐                         ┌──────────────┐
                    │  Test Suite  │                         │  Kubernetes  │
                    │  - Unit      │                         │   Cluster    │
                    │  - Property  │                         │              │
                    │  - Integration│                        │ - Dev        │
                    │  - E2E       │                         │ - Staging    │
                    │  - Load      │                         │ - Production │
                    └──────────────┘                         └──────────────┘
```

## Pipelines

### 1. CI Pipeline (ci-build-test.yml)

**Triggers:**
- Push to `main` or `develop` branches
- Pull requests to `main` or `develop`
- Manual workflow dispatch

**Jobs:**

#### 1.1 Backend Build & Test
- Setup .NET 10.0
- Restore dependencies
- Build solution (Release configuration)
- Run unit tests with code coverage
- Run property-based tests
- Generate coverage reports
- Upload test results

**Success Criteria:**
- Build succeeds
- All tests pass
- Code coverage > 70%

#### 1.2 Frontend Build & Test
- Setup Node.js 20.x
- Install dependencies
- Run ESLint
- Run TypeScript type checking
- Run Jest tests
- Build Next.js application
- Upload build artifacts

**Success Criteria:**
- Build succeeds
- No linting errors
- No type errors
- All tests pass

#### 1.3 Security Scan
- Run Trivy vulnerability scanner
- Run OWASP Dependency Check
- Upload results to GitHub Security
- Generate security reports

**Success Criteria:**
- No critical vulnerabilities
- No high-severity issues in dependencies

#### 1.4 Code Quality Analysis
- Run SonarCloud analysis
- Check code smells
- Check code duplication
- Check maintainability rating

**Success Criteria:**
- Quality gate passes
- Maintainability rating A or B
- Code duplication < 3%

#### 1.5 Integration Tests
- Start SQL Server container
- Start Redis container
- Start RabbitMQ container
- Run integration tests
- Upload test results

**Success Criteria:**
- All integration tests pass
- Services communicate correctly

#### 1.6 Notification
- Send Slack notification with build status
- Include branch, commit, and author information

### 2. CD Pipeline (cd-deploy.yml)

**Triggers:**
- Push to `main` branch (staging deployment)
- Git tags matching `v*` (production deployment)
- Manual workflow dispatch with environment selection

**Jobs:**

#### 2.1 Build Docker Images
- Build images for all 9 services
- Tag with commit SHA and version
- Push to GitHub Container Registry
- Use layer caching for faster builds

**Services:**
- api-gateway
- auth-service
- code-executor
- sql-executor
- gamification-engine
- ai-tutor
- notification-service
- analytics-service
- frontend

#### 2.2 Deploy to Dev
**Trigger:** Push to `develop` branch

**Steps:**
1. Configure kubectl with dev cluster credentials
2. Update image tags in Kustomize manifests
3. Apply Kubernetes manifests
4. Wait for rollout to complete
5. Run smoke tests

**Deployment Strategy:** Rolling update

#### 2.3 Deploy to Staging
**Trigger:** Push to `main` branch

**Steps:**
1. Configure kubectl with staging cluster credentials
2. Deploy to green environment
3. Run health checks on green
4. Switch traffic from blue to green
5. Verify green environment
6. Scale down blue environment
7. Run integration tests
8. Run load tests

**Deployment Strategy:** Blue-Green

**Rollback:** Automatic on failure (switch back to blue)

#### 2.4 Deploy to Production
**Trigger:** Git tag `v*` or manual approval

**Steps:**
1. Create backup of current deployment
2. Configure kubectl with production cluster credentials
3. Deploy canary (10% traffic)
4. Monitor canary for 5 minutes
5. Check error rate (must be < 1%)
6. Gradually increase traffic: 10% → 25% → 50% → 100%
7. Monitor at each stage
8. Promote canary to stable
9. Run production smoke tests
10. Create GitHub release

**Deployment Strategy:** Canary

**Traffic Split:**
- Phase 1: 10% canary, 90% stable (5 min)
- Phase 2: 25% canary, 75% stable (5 min)
- Phase 3: 50% canary, 50% stable (5 min)
- Phase 4: 100% canary (promote to stable)

**Rollback:** Automatic on failure or high error rate

#### 2.5 Rollback on Failure
**Trigger:** Failure in production deployment

**Steps:**
1. Rollback all deployments to previous version
2. Wait for rollout to complete
3. Verify health checks
4. Send Slack notification

## ArgoCD GitOps

### Setup

1. **Install ArgoCD:**
```bash
kubectl create namespace argocd
kubectl apply -n argocd -f https://raw.githubusercontent.com/argoproj/argo-cd/stable/manifests/install.yaml
```

2. **Access ArgoCD UI:**
```bash
kubectl port-forward svc/argocd-server -n argocd 8080:443
```

3. **Get admin password:**
```bash
kubectl -n argocd get secret argocd-initial-admin-secret -o jsonpath="{.data.password}" | base64 -d
```

4. **Apply application manifests:**
```bash
kubectl apply -f k8s/argocd/application-dev.yaml
kubectl apply -f k8s/argocd/application-staging.yaml
kubectl apply -f k8s/argocd/application-production.yaml
```

### Applications

#### Dev Application
- **Source:** `develop` branch
- **Sync Policy:** Automated (auto-sync, auto-prune, self-heal)
- **Namespace:** `dev`
- **Purpose:** Development testing

#### Staging Application
- **Source:** `main` branch
- **Sync Policy:** Automated (auto-sync, auto-prune, self-heal)
- **Namespace:** `staging`
- **Purpose:** Pre-production validation

#### Production Application
- **Source:** Git tags (e.g., `v1.0.0`)
- **Sync Policy:** Manual (requires approval)
- **Namespace:** `production`
- **Purpose:** Production deployment
- **RBAC:** Restricted to production-admins and production-deployers

### Sync Strategies

**Automated Sync (Dev, Staging):**
- Changes in Git automatically deployed
- Failed resources automatically pruned
- Self-healing enabled (reverts manual changes)

**Manual Sync (Production):**
- Requires explicit approval
- Changes reviewed before deployment
- Rollback available through ArgoCD UI

## Environments

### Development (dev)
- **URL:** https://dev.aspnet-learning.com
- **Branch:** `develop`
- **Deployment:** Automatic on push
- **Strategy:** Rolling update
- **Replicas:** 1 per service
- **Resources:** Minimal (0.5 CPU, 512Mi RAM per pod)

### Staging (staging)
- **URL:** https://staging.aspnet-learning.com
- **Branch:** `main`
- **Deployment:** Automatic on push
- **Strategy:** Blue-Green
- **Replicas:** 2 per service
- **Resources:** Medium (1 CPU, 1Gi RAM per pod)
- **Tests:** Integration + Load tests

### Production (production)
- **URL:** https://aspnet-learning.com
- **Branch:** Git tags (`v*`)
- **Deployment:** Manual approval required
- **Strategy:** Canary (10% → 25% → 50% → 100%)
- **Replicas:** 3-10 per service (auto-scaling)
- **Resources:** High (2 CPU, 2Gi RAM per pod)
- **Monitoring:** Full observability stack

## Secrets Management

### GitHub Secrets

**Required secrets:**
```
GITHUB_TOKEN              # Automatic (GitHub provides)
SONAR_TOKEN              # SonarCloud authentication
SLACK_WEBHOOK            # Slack notifications
KUBE_CONFIG_DEV          # Kubernetes config for dev (base64)
KUBE_CONFIG_STAGING      # Kubernetes config for staging (base64)
KUBE_CONFIG_PRODUCTION   # Kubernetes config for production (base64)
```

**Setting secrets:**
```bash
# Encode kubeconfig
cat ~/.kube/config | base64 -w 0

# Add to GitHub: Settings → Secrets → Actions → New repository secret
```

### Kubernetes Secrets

**Managed by:**
- External Secrets Operator (recommended)
- Sealed Secrets
- Manual kubectl apply (not recommended)

**Example:**
```bash
kubectl create secret generic db-credentials \
  --from-literal=username=admin \
  --from-literal=password=secure-password \
  -n production
```

## Monitoring & Observability

### Metrics

**Application Insights:**
- Request rates
- Response times
- Error rates
- Custom metrics

**Prometheus:**
- Pod CPU/memory usage
- Container metrics
- Custom application metrics

**Grafana Dashboards:**
- Service health overview
- Performance metrics
- Error tracking
- Resource utilization

### Logs

**ELK Stack:**
- Centralized log aggregation
- Log search and analysis
- Log-based alerts

**Kubernetes Logs:**
```bash
# View logs
kubectl logs -f deployment/api-gateway -n production

# View logs from all pods
kubectl logs -l app=api-gateway -n production --tail=100
```

### Alerts

**Alert Channels:**
- Slack
- Email
- PagerDuty (production only)

**Alert Rules:**
- Error rate > 1%
- Response time p95 > 200ms
- Pod crash loop
- High memory usage (> 80%)
- High CPU usage (> 80%)
- Deployment failure

## Rollback Procedures

### Automatic Rollback

**Triggers:**
- Deployment failure
- Health check failure
- High error rate (> 1%)
- Failed smoke tests

**Process:**
1. Detect failure
2. Execute rollback command
3. Wait for rollout
4. Verify health
5. Notify team

### Manual Rollback

**Using kubectl:**
```bash
# Rollback to previous version
kubectl rollout undo deployment/api-gateway -n production

# Rollback to specific revision
kubectl rollout undo deployment/api-gateway -n production --to-revision=3

# Check rollout status
kubectl rollout status deployment/api-gateway -n production
```

**Using ArgoCD:**
1. Open ArgoCD UI
2. Select application
3. Click "History and Rollback"
4. Select previous revision
5. Click "Rollback"

## Best Practices

### 1. Branch Strategy
- `develop` → Dev environment
- `main` → Staging environment
- `v*` tags → Production environment

### 2. Commit Messages
Follow Conventional Commits:
```
feat: add user authentication
fix: resolve memory leak in code executor
docs: update deployment guide
test: add integration tests for AI tutor
```

### 3. Pull Requests
- Require code review
- Require CI checks to pass
- Require up-to-date branch
- Use PR templates

### 4. Testing
- Write tests before merging
- Maintain > 80% code coverage
- Run tests locally before pushing
- Fix failing tests immediately

### 5. Deployments
- Deploy to dev first
- Test in staging thoroughly
- Deploy to production during low-traffic hours
- Monitor closely after deployment
- Have rollback plan ready

### 6. Security
- Scan for vulnerabilities regularly
- Keep dependencies updated
- Use least privilege access
- Rotate secrets periodically
- Enable audit logging

## Troubleshooting

### Build Failures

**Problem:** Build fails in CI
**Solution:**
1. Check build logs in GitHub Actions
2. Reproduce locally: `dotnet build`
3. Fix compilation errors
4. Push fix

### Test Failures

**Problem:** Tests fail in CI
**Solution:**
1. Check test logs
2. Reproduce locally: `dotnet test`
3. Fix failing tests
4. Verify all tests pass locally
5. Push fix

### Deployment Failures

**Problem:** Deployment fails in Kubernetes
**Solution:**
1. Check pod status: `kubectl get pods -n <namespace>`
2. Check pod logs: `kubectl logs <pod-name> -n <namespace>`
3. Check events: `kubectl get events -n <namespace>`
4. Fix issue and redeploy

### ArgoCD Sync Issues

**Problem:** ArgoCD shows "OutOfSync"
**Solution:**
1. Check ArgoCD UI for details
2. Review diff between Git and cluster
3. Sync manually if needed
4. Check for manual changes in cluster

## Task 23 Completion Checklist

- [x] Create GitHub Actions CI workflow
- [x] Create GitHub Actions CD workflow
- [x] Configure automated testing on PR
- [x] Setup Docker image building
- [x] Create ArgoCD application manifests
- [x] Configure dev environment deployment
- [x] Configure staging environment deployment
- [x] Configure production environment deployment
- [x] Implement blue-green deployment for staging
- [x] Implement canary deployment for production
- [x] Setup automatic rollback on failure
- [x] Document CI/CD pipeline
- [x] Document deployment strategies
- [x] Document rollback procedures

**Status:** Complete - CI/CD pipeline fully configured and documented.
