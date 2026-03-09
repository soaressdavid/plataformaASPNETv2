# Load Testing Documentation

## Overview

This directory contains load testing scenarios for the ASP.NET Learning Platform. The tests validate system performance under various load conditions, from normal usage to extreme stress scenarios.

## Requirements

### Tool: k6
k6 is a modern load testing tool built for developers.

**Installation:**
- Windows: `choco install k6` or download from https://k6.io/docs/getting-started/installation/
- macOS: `brew install k6`
- Linux: `sudo apt-get install k6` or `sudo yum install k6`

**Documentation:** https://k6.io/docs/

## Test Scenarios

### 1. Smoke Test (1 user, 1 minute)
**Purpose:** Verify basic functionality before running larger tests.

**Checks:**
- Health endpoints respond correctly
- API Gateway is accessible
- Basic services are operational

**Run:**
```bash
k6 run --include-system-env-vars load-test-scenarios.js --env API_GATEWAY_URL=http://localhost:5000
```

### 2. Load Test (100 concurrent users, 14 minutes)
**Purpose:** Test system under normal expected load.

**Profile:**
- Ramp up: 0 → 100 users over 7 minutes
- Sustained: 100 users for 5 minutes
- Ramp down: 100 → 0 users over 2 minutes

**Operations:**
- User authentication
- Browse curriculum
- Browse challenges
- View leaderboard

**SLA Requirements:**
- 95th percentile response time < 200ms
- 99th percentile response time < 500ms
- Error rate < 1%

### 3. Stress Test (1,000 concurrent users, 25 minutes)
**Purpose:** Test system limits and identify breaking points.

**Profile:**
- Ramp up: 0 → 1,000 users over 12 minutes
- Sustained: 1,000 users for 10 minutes
- Ramp down: 1,000 → 0 users over 3 minutes

**Operations:**
- Code execution requests
- SQL execution requests
- Mixed API calls

**Expected Behavior:**
- System should handle load gracefully
- Auto-scaling should trigger
- Circuit breakers may activate
- Some requests may be rate-limited (429)

### 4. Spike Test (2,000 users spike, 3.5 minutes)
**Purpose:** Test system resilience to sudden traffic spikes.

**Profile:**
- Normal: 100 users for 30 seconds
- Spike: 100 → 2,000 users in 30 seconds
- Sustained spike: 2,000 users for 1 minute
- Recovery: 2,000 → 100 users in 30 seconds
- Ramp down: 100 → 0 users in 30 seconds

**Expected Behavior:**
- Rate limiting activates (429 responses)
- Queue system buffers requests
- No complete service failures
- System recovers after spike

### 5. Endurance Test (10,000 concurrent users, 55 minutes)
**Purpose:** Test system stability under sustained extreme load.

**Profile:**
- Ramp up: 0 → 10,000 users over 20 minutes
- Sustained: 10,000 users for 30 minutes
- Ramp down: 10,000 → 0 users over 5 minutes

**Operations:**
- Mixed operations (curriculum, challenges, code execution)
- Random user behavior simulation
- Sustained database queries
- Cache hit/miss patterns

**Monitoring Focus:**
- Memory leaks
- Connection pool exhaustion
- Database performance degradation
- Cache effectiveness
- Auto-scaling behavior

## Running Tests

### Run All Scenarios
```bash
k6 run load-test-scenarios.js
```

### Run Specific Scenario
```bash
# Smoke test only
k6 run --include-system-env-vars load-test-scenarios.js --env SCENARIO=smoke_test

# Load test only
k6 run --include-system-env-vars load-test-scenarios.js --env SCENARIO=load_test_100

# Stress test only
k6 run --include-system-env-vars load-test-scenarios.js --env SCENARIO=stress_test_1000
```

### Run with Custom Configuration
```bash
# Custom API Gateway URL
k6 run --env API_GATEWAY_URL=https://api.example.com load-test-scenarios.js

# Custom duration
k6 run --duration 10m load-test-scenarios.js

# Custom VUs
k6 run --vus 50 --duration 5m load-test-scenarios.js
```

### Run with Cloud Output
```bash
# k6 Cloud (requires account)
k6 cloud load-test-scenarios.js

# InfluxDB output
k6 run --out influxdb=http://localhost:8086/k6 load-test-scenarios.js

# JSON output
k6 run --out json=results.json load-test-scenarios.js
```

## Interpreting Results

### Key Metrics

**http_req_duration:**
- Average response time
- p(95): 95th percentile (SLA: < 200ms)
- p(99): 99th percentile (SLA: < 500ms)

**http_req_failed:**
- Percentage of failed requests
- SLA: < 1%

**http_reqs:**
- Total requests per second
- Throughput indicator

**vus:**
- Number of virtual users
- Concurrent load

**errors:**
- Custom error rate
- Application-level errors

### Success Criteria

✅ **PASS:**
- p(95) response time < 200ms
- p(99) response time < 500ms
- Error rate < 1%
- No service crashes
- Auto-scaling works correctly

⚠️ **WARNING:**
- p(95) response time 200-300ms
- Error rate 1-5%
- Some rate limiting (expected under stress)

❌ **FAIL:**
- p(95) response time > 300ms
- Error rate > 5%
- Service crashes or becomes unresponsive
- Data corruption

## Performance Targets

### Normal Load (100 users)
- Response time p(95): < 100ms
- Response time p(99): < 200ms
- Throughput: > 1,000 req/s
- Error rate: < 0.1%

### High Load (1,000 users)
- Response time p(95): < 200ms
- Response time p(99): < 500ms
- Throughput: > 5,000 req/s
- Error rate: < 1%

### Extreme Load (10,000 users)
- Response time p(95): < 500ms
- Response time p(99): < 1,000ms
- Throughput: > 20,000 req/s
- Error rate: < 5%
- System remains stable for 30+ minutes

## Monitoring During Tests

### Application Insights
- Monitor real-time metrics
- Check for exceptions
- Verify distributed tracing

### Kubernetes Dashboard
- Watch pod scaling
- Monitor resource usage (CPU, memory)
- Check pod health

### Database Metrics
- Query performance
- Connection pool usage
- Lock contention

### Redis Metrics
- Cache hit rate
- Memory usage
- Connection count

### RabbitMQ Metrics
- Queue depth
- Message rate
- Consumer lag

## Troubleshooting

### High Response Times
1. Check database query performance
2. Verify cache hit rate
3. Check for N+1 queries
4. Review auto-scaling configuration

### High Error Rates
1. Check application logs
2. Verify database connections
3. Check rate limiting configuration
4. Review circuit breaker status

### Service Crashes
1. Check memory usage (potential leaks)
2. Review connection pool settings
3. Check for deadlocks
4. Verify resource limits

### Auto-Scaling Issues
1. Check HPA (Horizontal Pod Autoscaler) configuration
2. Verify metrics server is running
3. Review scaling thresholds
4. Check node capacity

## Best Practices

1. **Run smoke test first** - Verify basic functionality
2. **Monitor during tests** - Watch metrics in real-time
3. **Start small** - Begin with lower load and increase gradually
4. **Test in staging** - Never run load tests in production
5. **Clean up after** - Remove test data and reset state
6. **Document results** - Keep records of test runs
7. **Compare over time** - Track performance trends
8. **Test regularly** - Include in CI/CD pipeline

## CI/CD Integration

### GitHub Actions Example
```yaml
name: Load Tests

on:
  schedule:
    - cron: '0 2 * * 0'  # Weekly on Sunday at 2 AM
  workflow_dispatch:

jobs:
  load-test:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v3
      
      - name: Install k6
        run: |
          sudo apt-key adv --keyserver hkp://keyserver.ubuntu.com:80 --recv-keys C5AD17C747E3415A3642D57D77C6C491D6AC1D69
          echo "deb https://dl.k6.io/deb stable main" | sudo tee /etc/apt/sources.list.d/k6.list
          sudo apt-get update
          sudo apt-get install k6
      
      - name: Run load tests
        run: |
          k6 run --env API_GATEWAY_URL=${{ secrets.STAGING_API_URL }} tests/LoadTests/load-test-scenarios.js
      
      - name: Upload results
        uses: actions/upload-artifact@v3
        with:
          name: load-test-results
          path: results.json
```

## Additional Resources

- [k6 Documentation](https://k6.io/docs/)
- [k6 Examples](https://k6.io/docs/examples/)
- [Performance Testing Best Practices](https://k6.io/docs/testing-guides/test-types/)
- [Grafana k6 Cloud](https://k6.io/cloud/)

## Task 22.4 Completion Checklist

- [x] Create load test scenarios
- [x] Configure 100 concurrent users test
- [x] Configure 1,000 concurrent users test
- [x] Configure 10,000 concurrent users test
- [x] Define SLA thresholds (p95 < 200ms)
- [x] Document test execution
- [x] Document performance targets
- [ ] Execute tests in staging environment
- [ ] Verify auto-scaling behavior
- [ ] Identify and document bottlenecks
- [ ] Create performance optimization plan

**Status:** Implementation complete, execution pending staging environment deployment.
