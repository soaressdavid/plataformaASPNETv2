# API Gateway Middleware

This directory contains middleware components for the API Gateway.

## Rate Limiting Middleware

The `RateLimitingMiddleware` implements rate limiting using a token bucket algorithm with Redis as the backend storage.

### Features

- **Token Bucket Algorithm**: Smooth rate limiting that allows bursts while maintaining average rate
- **Redis Backend**: Distributed rate limiting across multiple gateway instances
- **Per-User Limits**: Different limits for authenticated and unauthenticated users
- **Graceful Degradation**: Fails open if Redis is unavailable (allows requests)
- **Retry-After Header**: Informs clients when they can retry

### Configuration

Rate limits are configured in the middleware:
- **Authenticated users**: 100 requests per minute
- **Unauthenticated users**: 10 requests per minute

Redis connection is configured in `appsettings.json`:
```json
{
  "Redis": {
    "ConnectionString": "localhost:6379"
  }
}
```

### How It Works

1. **Client Identification**:
   - Authenticated users: Identified by user ID from JWT token
   - Unauthenticated users: Identified by IP address

2. **Token Bucket**:
   - Each client has a bucket with a maximum number of tokens (limit)
   - Each request consumes one token
   - Tokens refill continuously over time at a rate of (limit / 60 seconds)
   - If no tokens available, request is rejected with 429 status

3. **Redis Storage**:
   - `ratelimit:{identifier}:tokens` - Current token count
   - `ratelimit:{identifier}:lastRefill` - Last refill timestamp
   - Keys expire after 2 minutes of inactivity

### Response Format

When rate limit is exceeded:
```json
{
  "error": {
    "code": "RATE_LIMIT_EXCEEDED",
    "message": "Rate limit exceeded. Maximum 100 requests per minute allowed.",
    "retryAfter": 30,
    "timestamp": "2024-01-15T10:30:00Z",
    "traceId": "abc123-def456"
  }
}
```

HTTP Headers:
- Status: `429 Too Many Requests`
- `Retry-After`: Seconds until next token is available

### Testing

Unit tests: `tests/ApiGateway.Tests/RateLimitingMiddlewareTests.cs`
Property tests: `tests/ApiGateway.Tests/RateLimitingPropertiesTests.cs`

Run tests:
```bash
dotnet test tests/ApiGateway.Tests/ApiGateway.Tests.csproj
```

### Requirements

Validates: Requirements 11.5
