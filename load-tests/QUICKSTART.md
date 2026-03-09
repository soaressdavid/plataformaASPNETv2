# Load Testing Quick Start Guide

## 1. Install k6

### Windows
```powershell
# Using Chocolatey
choco install k6

# Using Winget
winget install k6
```

### macOS
```bash
brew install k6
```

### Linux
```bash
# Debian/Ubuntu
sudo apt-key adv --keyserver hkp://keyserver.ubuntu.com:80 --recv-keys C5AD17C747E3415A3642D57D77C6C491D6AC1D69
echo "deb https://dl.k6.io/deb stable main" | sudo tee /etc/apt/sources.list.d/k6.list
sudo apt-get update
sudo apt-get install k6
```

## 2. Start the Platform

```powershell
# From project root
docker-compose up -d
```

Wait for all services to be healthy:
```powershell
docker-compose ps
```

## 3. Run Load Tests

```powershell
# From project root
cd load-tests
./run-all-tests.ps1
```

## 4. View Results

Results are saved to `load-tests/results/` with timestamps.

Console output shows real-time metrics:
- ✓ Passed checks
- ✗ Failed checks
- Response times (avg, min, max, p95)
- Error rates
- Request rates

## Test Scenarios

| Test | Concurrent Users | Duration | Target |
|------|-----------------|----------|--------|
| Browse Challenges | 1000 | 17 min | p95 < 200ms |
| Code Execution | 100 | 13 min | p95 < 5s |
| AI Feedback | 50 | 13 min | p95 < 10s |

## Success Criteria

✅ **Browse Challenges**: p95 < 200ms, error rate < 1%
✅ **Code Execution**: p95 < 5s, error rate < 5%, success rate > 90%
✅ **AI Feedback**: p95 < 10s, error rate < 5%, success rate > 85%

## Troubleshooting

**k6 not found?**
→ Install k6 (see step 1)

**Services not running?**
→ Run `docker-compose up -d`

**High error rates?**
→ Check Docker resources, scale workers

**Slow response times?**
→ Monitor Grafana (http://localhost:3000)

## Advanced Usage

### Run Individual Tests

```bash
# Browse challenges only
k6 run browse-challenges.js

# Code execution only
k6 run code-execution.js

# AI feedback only
k6 run ai-feedback.js
```

### Custom Environment

```powershell
./run-all-tests.ps1 -BaseUrl "https://api.example.com"
```

### Skip User Creation

```powershell
./run-all-tests.ps1 -TestUsers $false
```

## Monitoring

Access Grafana dashboard during tests:
```
http://localhost:3000
Username: admin
Password: admin
```

## More Information

See [README.md](README.md) for comprehensive documentation.
