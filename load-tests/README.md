# Load Testing Suite

This directory contains load testing scripts for the ASP.NET Core Learning Platform using [k6](https://k6.io/).

## Overview

The load testing suite validates the platform's performance under realistic load conditions across three key scenarios:

1. **Browse Challenges** - 1000 concurrent users browsing challenges
2. **Code Execution** - 100 concurrent code executions
3. **AI Feedback** - 50 concurrent AI feedback requests

## Requirements

### Prerequisites

- **k6** - Load testing tool (v0.40.0 or higher)
- **Running Platform** - The ASP.NET Core Learning Platform must be running and accessible

### Installing k6

#### Windows

```powershell
# Using Chocolatey
choco install k6

# Using Winget
winget install k6
```

#### macOS

```bash
# Using Homebrew
brew install k6
```

#### Linux (Debian/Ubuntu)

```bash
sudo apt-key adv --keyserver hkp://keyserver.ubuntu.com:80 --recv-keys C5AD17C747E3415A3642D57D77C6C491D6AC1D69
echo "deb https://dl.k6.io/deb stable main" | sudo tee /etc/apt/sources.list.d/k6.list
sudo apt-get update
sudo apt-get install k6
```

For other platforms, see: https://k6.io/docs/getting-started/installation/

## Test Scenarios

### 1. Browse Challenges (browse-challenges.js)

**Objective**: Test the platform's ability to handle 1000 concurrent users browsing challenges.

**Load Profile**:
- Ramp up to 100 users over 2 minutes
- Ramp up to 500 users over 3 minutes
- Ramp up to 1000 users over 5 minutes
- Maintain 1000 users for 5 minutes
- Ramp down to 0 users over 2 minutes

**Performance Targets**:
- API response time: p95 < 200ms
- Error rate: < 1%

**User Actions**:
1. Login with credentials
2. Browse challenges list
3. View challenge details
4. View user dashboard

### 2. Code Execution (code-execution.js)

**Objective**: Test the code execution engine's ability to handle 100 concurrent code executions.

**Load Profile**:
- Ramp up to 20 users over 1 minute
- Ramp up to 50 users over 2 minutes
- Ramp up to 100 users over 3 minutes
- Maintain 100 users for 5 minutes
- Ramp down to 0 users over 2 minutes

**Performance Targets**:
- Execution time: p95 < 5 seconds
- Error rate: < 5%
- Success rate: > 90%

**User Actions**:
1. Login with credentials
2. Submit code for execution
3. Poll for execution status
4. Retrieve execution results

### 3. AI Feedback (ai-feedback.js)

**Objective**: Test the AI Tutor service's ability to handle 50 concurrent AI feedback requests.

**Load Profile**:
- Ramp up to 10 users over 1 minute
- Ramp up to 25 users over 2 minutes
- Ramp up to 50 users over 3 minutes
- Maintain 50 users for 5 minutes
- Ramp down to 0 users over 2 minutes

**Performance Targets**:
- AI feedback time: p95 < 10 seconds
- Error rate: < 5%
- Success rate: > 85%

**User Actions**:
1. Login with credentials
2. Submit code for AI review
3. Retrieve AI feedback with suggestions

## Running the Tests

### Quick Start

Run all tests with default settings:

```powershell
./run-all-tests.ps1
```

### Custom Configuration

Run tests against a specific environment:

```powershell
./run-all-tests.ps1 -BaseUrl "https://api.example.com"
```

Skip test user creation:

```powershell
./run-all-tests.ps1 -TestUsers $false
```

### Individual Tests

Run a single test scenario:

```bash
# Browse Challenges
k6 run browse-challenges.js

# Code Execution
k6 run code-execution.js

# AI Feedback
k6 run ai-feedback.js
```

With custom base URL:

```bash
BASE_URL=https://api.example.com k6 run browse-challenges.js
```

## Test Data

The tests use the following test users:

| Email | Password |
|-------|----------|
| test1@example.com | Test123!@# |
| test2@example.com | Test123!@# |
| test3@example.com | Test123!@# |
| test4@example.com | Test123!@# |
| test5@example.com | Test123!@# |

The `run-all-tests.ps1` script automatically creates these users if they don't exist.

## Results

### Console Output

k6 provides real-time metrics during test execution:

```
     ✓ login status is 200
     ✓ challenges list status is 200
     ✓ challenges response time < 200ms

     checks.........................: 98.50% ✓ 2955    ✗ 45
     data_received..................: 1.2 MB 20 kB/s
     data_sent......................: 890 kB 15 kB/s
     http_req_duration..............: avg=145ms min=50ms med=120ms max=850ms p(90)=180ms p(95)=195ms
     http_req_failed................: 0.50%  ✓ 15      ✗ 2985
     http_reqs......................: 3000   50/s
     iteration_duration.............: avg=5.2s  min=4.1s med=5.0s max=8.5s  p(90)=6.1s  p(95)=6.8s
     iterations.....................: 1000   16.67/s
     vus............................: 1000   min=0     max=1000
     vus_max........................: 1000   min=1000  max=1000
```

### JSON Output

Detailed results are saved to `load-tests/results/` directory:

- `browse-challenges-YYYYMMDD_HHMMSS.json`
- `code-execution-YYYYMMDD_HHMMSS.json`
- `ai-feedback-YYYYMMDD_HHMMSS.json`

### Analyzing Results

#### Using k6 Cloud

Upload results to k6 Cloud for detailed analysis:

```bash
k6 cloud browse-challenges.js
```

#### Using JSON Output

Parse JSON results with custom tools or scripts:

```powershell
# Example: Extract p95 response times
$results = Get-Content "results/browse-challenges-*.json" | ConvertFrom-Json
$results | Where-Object { $_.metric -eq "http_req_duration" -and $_.type -eq "Point" } | 
  Measure-Object -Property value -Average -Maximum
```

## Performance Targets

### Requirements Validation

The load tests validate the following requirements:

| Requirement | Target | Test |
|-------------|--------|------|
| 3.6 - Code execution latency | < 5s (p95) | Code Execution |
| 4.6 - AI feedback latency | < 10s | AI Feedback |
| 15.6 - Page load time | < 2s | Browse Challenges |
| API response time | < 200ms (p95) | Browse Challenges |

### Success Criteria

Tests are considered successful when:

1. **Browse Challenges**:
   - p95 response time < 200ms
   - Error rate < 1%
   - All 1000 concurrent users handled successfully

2. **Code Execution**:
   - p95 execution time < 5 seconds
   - Error rate < 5%
   - Success rate > 90%
   - All 100 concurrent executions handled

3. **AI Feedback**:
   - p95 feedback time < 10 seconds
   - Error rate < 5%
   - Success rate > 85%
   - All 50 concurrent requests handled

## Troubleshooting

### Common Issues

#### k6 not found

**Error**: `k6: The term 'k6' is not recognized`

**Solution**: Install k6 using one of the methods in the Prerequisites section.

#### Services not running

**Error**: `Could not reach services at http://localhost:5000`

**Solution**: Start the platform services:

```powershell
docker-compose up -d
```

#### Test users already exist

**Warning**: `User already exists: test1@example.com`

**Solution**: This is normal. The script will skip creating existing users.

#### High error rates

**Issue**: Error rate exceeds threshold

**Possible causes**:
- Insufficient resources (CPU, memory)
- Database connection pool exhausted
- Redis queue full
- Docker container limits reached

**Solutions**:
- Scale up infrastructure
- Increase connection pool sizes
- Add more worker instances
- Adjust Docker resource limits

#### Slow response times

**Issue**: Response times exceed targets

**Possible causes**:
- Database query performance
- Network latency
- Insufficient worker capacity
- External API (Groq) rate limits

**Solutions**:
- Optimize database queries and indexes
- Add database read replicas
- Scale worker instances horizontally
- Implement caching strategies
- Review Groq API rate limits

## Monitoring During Tests

### Recommended Monitoring

While running load tests, monitor:

1. **System Metrics**:
   - CPU usage
   - Memory usage
   - Disk I/O
   - Network throughput

2. **Application Metrics**:
   - Request rate
   - Response times
   - Error rates
   - Queue depth

3. **Database Metrics**:
   - Connection pool usage
   - Query execution times
   - Lock contention

4. **Container Metrics**:
   - Container CPU/memory usage
   - Container creation/destruction rate
   - Docker daemon health

### Using Grafana

Access the monitoring dashboard:

```
http://localhost:3000
```

Default credentials:
- Username: admin
- Password: admin

## Best Practices

1. **Baseline Testing**: Run tests on a clean environment to establish baseline metrics
2. **Incremental Load**: Start with lower load and gradually increase
3. **Monitoring**: Always monitor system resources during tests
4. **Isolation**: Run tests in an isolated environment (not production)
5. **Repeatability**: Run tests multiple times to ensure consistent results
6. **Documentation**: Document any configuration changes or optimizations

## References

- [k6 Documentation](https://k6.io/docs/)
- [k6 Best Practices](https://k6.io/docs/testing-guides/test-types/)
- [ASP.NET Core Performance Best Practices](https://docs.microsoft.com/en-us/aspnet/core/performance/performance-best-practices)
- [Docker Performance Tuning](https://docs.docker.com/config/containers/resource_constraints/)

## Task Validation

This load testing suite validates **Task 20.5** from the implementation plan:

- ✅ Test 1000 concurrent users browsing challenges
- ✅ Test 100 concurrent code executions
- ✅ Test 50 concurrent AI feedback requests
- ✅ Verify performance targets (API < 200ms p95, execution < 5s p95)

**Requirements validated**: 3.6, 4.6, 15.6
