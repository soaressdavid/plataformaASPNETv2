# API Gateway Integration Tests

## Overview

The `ApiGatewayIntegrationTests.cs` file contains comprehensive integration tests for the API Gateway that validate:

1. **Routing** - Requests are correctly routed to all microservices (Course, Challenge, Progress, Execution, AI Tutor)
2. **Authentication** - JWT token validation with valid, invalid, expired, and malformed tokens
3. **Rate Limiting** - Request throttling at 100 requests/minute per user with burst handling
4. **Circuit Breaker** - Service unavailability handling and error responses

**Validates: Requirements 11.1, 11.2, 11.3, 11.4, 11.5**

## Prerequisites

### Required Services

The integration tests require the following services to be running:

1. **Redis** (localhost:6379) - Required for rate limiting functionality
   - Install Redis: https://redis.io/download
   - Windows: Use Redis for Windows or WSL
   - Start Redis: `redis-server`

### Optional Services

The following services are optional. Tests will handle their unavailability gracefully:

- Course Service (localhost:5002)
- Challenge Service (localhost:5003)
- Progress Service (localhost:5004)
- AI Tutor Service (localhost:5005)
- Execution Service (localhost:5006)

## Running the Tests

### With Redis Running

```bash
# Start Redis first
redis-server

# Run the integration tests
dotnet test tests/ApiGateway.Tests/ApiGateway.Tests.csproj --filter "FullyQualifiedName~ApiGatewayIntegrationTests"
```

### Without Redis

If Redis is not available, the tests will fail during initialization. To run tests without Redis, you would need to:

1. Modify `src/ApiGateway/Program.cs` to handle Redis connection failures gracefully
2. Use `abortConnect=false` in the Redis connection string
3. Implement a fallback mechanism for rate limiting

## Test Structure

### Routing Tests (5 tests)

- `Routing_CourseRequests_RoutesToCourseService` - Validates routing to Course Service
- `Routing_CodeExecutionRequests_RoutesToExecutionEngine` - Validates routing to Execution Engine
- `Routing_AIFeedbackRequests_RoutesToAITutor` - Validates routing to AI Tutor
- `Routing_ChallengeRequests_RoutesToChallengeService` - Validates routing to Challenge Service
- `Routing_ProgressRequests_RoutesToProgressService` - Validates routing to Progress Service

### Authentication Tests (7 tests)

- `Authentication_ValidToken_AllowsRequest` - Valid JWT tokens are accepted
- `Authentication_MissingToken_Returns401` - Missing tokens are rejected
- `Authentication_ExpiredToken_Returns401` - Expired tokens are rejected
- `Authentication_MalformedToken_Returns401` - Malformed tokens are rejected
- `Authentication_WrongSignature_Returns401` - Tokens with wrong signature are rejected
- `Authentication_PublicEndpoints_AllowWithoutToken` - Public endpoints don't require auth

### Rate Limiting Tests (4 tests)

- `RateLimiting_WithinLimit_AllowsRequests` - Requests within limit are allowed
- `RateLimiting_ExceedsLimit_Returns429` - Requests exceeding limit return 429
- `RateLimiting_DifferentUsers_IndependentLimits` - Each user has independent limits
- `RateLimiting_BurstRequests_EnforcesLimit` - Burst requests are handled correctly

### Circuit Breaker Tests (3 tests)

- `CircuitBreaker_ServiceUnavailable_Returns503` - Unavailable services return 503
- `CircuitBreaker_MultipleFailures_OpensCircuit` - Circuit opens after failures
- `CircuitBreaker_ErrorResponse_HasCorrectStructure` - Error responses are properly formatted

## Test Configuration

The tests use `ApiGatewayTestFactory` which:

1. Generates RSA key pairs for JWT signing/validation
2. Configures mock Redis (when Redis is unavailable)
3. Sets up YARP reverse proxy routes
4. Configures circuit breaker settings
5. Points microservice clusters to test ports (9001-9006)

## Troubleshooting

### Redis Connection Errors

If you see errors like:
```
StackExchange.Redis.RedisConnectionException : It was not possible to connect to the redis server(s)
```

**Solution**: Start Redis before running the tests:
```bash
redis-server
```

### Service Unavailable (503) Responses

This is expected behavior when microservices are not running. The tests are designed to handle this gracefully and validate that the API Gateway correctly returns 503 when services are unavailable.

### All Tests Failing

1. Ensure Redis is running
2. Check that no other process is using the test ports (9001-9006)
3. Verify JWT configuration is correct
4. Check that the API Gateway builds successfully

## Future Improvements

1. **In-Memory Redis Mock** - Implement a complete in-memory Redis mock to eliminate the Redis dependency
2. **Mock Microservices** - Add mock HTTP servers for microservices to test successful routing
3. **Performance Tests** - Add tests for response time and throughput
4. **Load Tests** - Test behavior under high concurrent load
5. **Security Tests** - Add penetration testing for authentication bypass attempts

## Related Files

- `ApiGatewayIntegrationTests.cs` - Main test file
- `ApiGatewayTestFactory.cs` - Test factory for WebApplicationFactory
- `src/ApiGateway/Program.cs` - API Gateway entry point
- `src/ApiGateway/Middleware/` - Middleware implementations
