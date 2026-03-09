# Platform Integration Tests

## Overview

This project contains end-to-end integration tests for the ASP.NET Core Learning Platform. These tests validate complete workflows across all services:

1. **Challenge Submission Flow** - Tests the complete flow from browsing challenges to submitting solutions
2. **Course Enrollment and Lesson Completion** - Tests course enrollment and lesson progression
3. **Code Execution with Queue and Workers** - Tests the distributed code execution system
4. **AI Feedback Integration** - Tests AI-powered code review functionality
5. **Leaderboard Updates** - Tests gamification and progress tracking

**Validates: Requirements 3.1, 3.2, 5.3, 7.3, 7.6, 9.7**

## Prerequisites

### Required Services

The integration tests require the following services to be running:

1. **PostgreSQL** (localhost:5432) - Database
2. **Redis** (localhost:6379) - Job queue and caching
3. **RabbitMQ** (localhost:5672) - Event bus
4. **API Gateway** (localhost:5000) - Main entry point
5. **Auth Service** (localhost:5001) - Authentication
6. **Course Service** (localhost:5002) - Course management
7. **Challenge Service** (localhost:5003) - Challenge management
8. **Progress Service** (localhost:5004) - Progress tracking
9. **AI Tutor Service** (localhost:5005) - AI feedback
10. **Execution Service** (localhost:5006) - Code execution
11. **Worker** - Code execution worker

### Starting Services

#### Option 1: Docker Compose (Recommended)

```bash
# Start infrastructure services
docker-compose up -d postgres redis rabbitmq

# Build and start all services
docker-compose up -d
```

#### Option 2: Manual Start

```bash
# Start infrastructure
docker-compose up -d postgres redis rabbitmq

# Start each service manually
cd src/ApiGateway && dotnet run &
cd src/Services/Auth.Service && dotnet run &
cd src/Services/Course.Service && dotnet run &
cd src/Services/Challenge.Service && dotnet run &
cd src/Services/Progress.Service && dotnet run &
cd src/Services/AITutor.Service && dotnet run &
cd src/Services/Execution.Service && dotnet run &
cd src/Services/Worker.Service && dotnet run &
```

## Running the Tests

### Run All Integration Tests

```bash
dotnet test tests/Integration.Tests/Integration.Tests.csproj
```

### Run Specific Test

```bash
# Challenge submission flow
dotnet test tests/Integration.Tests/Integration.Tests.csproj --filter "FullyQualifiedName~ChallengeSubmissionFlow"

# Course enrollment flow
dotnet test tests/Integration.Tests/Integration.Tests.csproj --filter "FullyQualifiedName~CourseEnrollmentAndLessonCompletion"

# Code execution
dotnet test tests/Integration.Tests/Integration.Tests.csproj --filter "FullyQualifiedName~CodeExecution"

# AI feedback
dotnet test tests/Integration.Tests/Integration.Tests.csproj --filter "FullyQualifiedName~AIFeedback"

# Leaderboard
dotnet test tests/Integration.Tests/Integration.Tests.csproj --filter "FullyQualifiedName~Leaderboard"
```

### Run with Verbose Output

```bash
dotnet test tests/Integration.Tests/Integration.Tests.csproj --logger "console;verbosity=detailed"
```

### Environment Variables

You can configure the API Gateway URL using an environment variable:

```bash
# Windows PowerShell
$env:API_GATEWAY_URL="http://localhost:5000"
dotnet test tests/Integration.Tests/Integration.Tests.csproj

# Linux/Mac
export API_GATEWAY_URL=http://localhost:5000
dotnet test tests/Integration.Tests/Integration.Tests.csproj
```

## Test Behavior

### Graceful Degradation

The tests are designed to handle service unavailability gracefully:

- If authentication fails, tests will be skipped with a warning message
- If specific services are unavailable, tests will log the status code and continue
- Tests will not fail catastrophically if services are not running

### Test Data

Each test run:
- Creates a unique test user with a random email
- Uses the test user's authentication token for all requests
- Does not clean up test data (to allow inspection after test runs)

## Expected Results

### When All Services Are Running

All 5 tests should pass:
- ✅ ChallengeSubmissionFlow_EndToEnd_Success
- ✅ CourseEnrollmentAndLessonCompletion_EndToEnd_Success
- ✅ CodeExecution_WithQueueAndWorkers_Success
- ✅ AIFeedback_Integration_Success
- ✅ Leaderboard_Updates_Success

### When Services Are Not Running

Tests will be skipped or show informational messages:
- ⚠️ "Skipping test: Authentication not available"
- ⚠️ "Challenge submission returned: ServiceUnavailable"

## Troubleshooting

### Connection Refused Errors

If you see connection refused errors, ensure all services are running:

```bash
# Check Docker containers
docker-compose ps

# Check service health
curl http://localhost:5000/health
curl http://localhost:5001/health
curl http://localhost:5002/health
# ... etc
```

### Authentication Failures

If authentication fails:
1. Ensure Auth Service is running on port 5001
2. Check Auth Service logs for errors
3. Verify database migrations have been applied

### Code Execution Timeouts

If code execution tests timeout:
1. Ensure Worker service is running
2. Check Redis connection
3. Verify Docker is running (for container execution)
4. Check Worker service logs

### AI Feedback Failures

If AI feedback tests fail:
1. Ensure AI Tutor Service is running on port 5005
2. Check that GROQ_API_KEY environment variable is set
3. Verify Groq API quota is not exceeded

### Database Errors

If you see database errors:
1. Ensure PostgreSQL is running
2. Apply migrations: `dotnet ef database update`
3. Check connection string in appsettings.json

## CI/CD Integration

### GitHub Actions Example

```yaml
name: Integration Tests

on: [push, pull_request]

jobs:
  integration-tests:
    runs-on: ubuntu-latest
    
    services:
      postgres:
        image: postgres:15-alpine
        env:
          POSTGRES_PASSWORD: postgres
        ports:
          - 5432:5432
      
      redis:
        image: redis:7-alpine
        ports:
          - 6379:6379
      
      rabbitmq:
        image: rabbitmq:3-management-alpine
        ports:
          - 5672:5672
    
    steps:
      - uses: actions/checkout@v3
      
      - name: Setup .NET
        uses: actions/setup-dotnet@v3
        with:
          dotnet-version: '8.0.x'
      
      - name: Start services
        run: |
          docker-compose up -d
          sleep 30  # Wait for services to be ready
      
      - name: Run integration tests
        run: dotnet test tests/Integration.Tests/Integration.Tests.csproj
```

## Notes

- Tests use real HTTP calls to actual services (not mocked)
- Each test is independent and can run in any order
- Tests create real data in the database
- Tests may take 30-60 seconds to complete depending on service response times
- The test suite validates the complete platform functionality end-to-end

## Related Documentation

- [API Gateway Integration Tests](../ApiGateway.Tests/README_INTEGRATION_TESTS.md)
- [Event Bus Integration Tests](../Shared.Tests/README_EVENT_BUS_INTEGRATION_TESTS.md)
- [Execution Service Integration Tests](../Execution.Tests/ExecutionServiceIntegrationTests.cs)
