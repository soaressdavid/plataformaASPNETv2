# API Gateway

The API Gateway serves as the single entry point for all client requests to the ASP.NET Core Learning Platform. It handles authentication, rate limiting, and routing to appropriate microservices using YARP (Yet Another Reverse Proxy).

## Features

- **JWT Authentication**: Validates session tokens using RS256 algorithm before processing requests
- **User Context**: Extracts user information from tokens and adds it to the request pipeline
- **Reverse Proxy**: Routes requests to appropriate microservices based on path patterns
- **Public Endpoints**: Allows unauthenticated access to registration and login endpoints

## Authentication Middleware

The `JwtAuthenticationMiddleware` validates JWT tokens from the Authorization header and adds user context to the request pipeline.

### Token Validation

- **Algorithm**: RS256 (RSA with SHA-256)
- **Issuer**: aspnet-learning-platform
- **Audience**: aspnet-learning-platform-users
- **Expiration**: 24 hours
- **Clock Skew**: Zero tolerance

### Authorization Header Format

The middleware supports two formats:

```
Authorization: Bearer <token>
Authorization: <token>
```

### User Context

After successful validation, the middleware adds the following headers for downstream services:

- `X-User-Id`: The user's unique identifier (GUID)
- `X-User-Email`: The user's email address
- `X-User-Name`: The user's name

### Public Endpoints

The following endpoints do not require authentication:

- `/api/auth/register`
- `/api/auth/login`
- `/openapi`
- `/health`

### Error Responses

**Missing Token (401 Unauthorized)**:
```json
{
  "error": {
    "code": "MISSING_TOKEN",
    "message": "Authorization header is required",
    "timestamp": "2024-01-15T10:30:00Z",
    "traceId": "abc123"
  }
}
```

**Invalid Token (401 Unauthorized)**:
```json
{
  "error": {
    "code": "INVALID_TOKEN",
    "message": "Token has expired",
    "timestamp": "2024-01-15T10:30:00Z",
    "traceId": "abc123"
  }
}
```

## Configuration

### JWT Configuration

Configure the RSA public key in `appsettings.json`:

```json
{
  "Jwt": {
    "PublicKeyPem": "-----BEGIN PUBLIC KEY-----\n...\n-----END PUBLIC KEY-----"
  }
}
```

For development, if no public key is configured, the middleware will generate a temporary key pair. **This should not be used in production.**

## YARP Configuration

### Routes

The gateway is configured with the following routes:

| Route Pattern | Target Service | Port | Description |
|--------------|----------------|------|-------------|
| `/api/auth/**` | Auth Service | 5001 | User authentication and registration |
| `/api/courses/**` | Course Service | 5002 | Course and lesson management |
| `/api/challenges/**` | Challenge Service | 5003 | Coding challenges and submissions |
| `/api/progress/**` | Progress Service | 5004 | User progress, XP, and levels |
| `/api/leaderboard/**` | Progress Service | 5004 | Leaderboard rankings |
| `/api/code/execute/**` | Execution Service | 5006 | Code execution requests |
| `/api/code/status/**` | Execution Service | 5006 | Execution status queries |
| `/api/code/review/**` | AI Tutor Service | 5005 | AI-powered code review |

### Clusters

Each route is mapped to a cluster that defines the destination service endpoints:

- **auth-cluster**: Authentication Service (localhost:5001)
- **course-cluster**: Course Service (localhost:5002)
- **challenge-cluster**: Challenge Service (localhost:5003)
- **progress-cluster**: Progress Service (localhost:5004)
- **ai-tutor-cluster**: AI Tutor Service (localhost:5005)
- **execution-cluster**: Code Execution Service (localhost:5006)

### Configuration Files

- `appsettings.json`: Production configuration with service endpoints
- `appsettings.Development.json`: Development configuration with localhost endpoints and enhanced logging

## Running the Gateway

```bash
dotnet run
```

The gateway will start on the default port (5000 for HTTP, 5001 for HTTPS).

## Testing Authentication

### 1. Register a User

```bash
curl -X POST https://localhost:5000/api/auth/register \
  -H "Content-Type: application/json" \
  -d '{"name":"John Doe","email":"john@example.com","password":"SecurePass123"}'
```

### 2. Login to Get Token

```bash
curl -X POST https://localhost:5000/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"email":"john@example.com","password":"SecurePass123"}'
```

Response:
```json
{
  "userId": "123e4567-e89b-12d3-a456-426614174000",
  "name": "John Doe",
  "token": "eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9...",
  "expiresAt": "2024-01-16T10:30:00Z"
}
```

### 3. Access Protected Endpoint

```bash
curl -X GET https://localhost:5000/api/courses \
  -H "Authorization: Bearer eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9..."
```

## Architecture

```
Client Request
    ↓
API Gateway (Port 5000)
    ↓
JwtAuthenticationMiddleware
    ↓ (validates token)
    ↓ (adds user context)
    ↓
YARP Reverse Proxy
    ↓
Microservices (Ports 5001-5006)
```

## Requirements Validation

This implementation validates **Requirement 1.5**:

> WHEN an authenticated user makes a request, THE API_Gateway SHALL validate the session token before processing the request

The middleware:
- Extracts tokens from the Authorization header
- Validates tokens using RS256 algorithm with the Auth Service's public key
- Verifies issuer, audience, and expiration
- Adds user context to the request pipeline for downstream services
- Returns appropriate error responses for invalid or missing tokens

## Future Enhancements

The following features will be added in subsequent tasks:

- Rate limiting (100 req/min per user)
- Circuit breaker for microservice resilience
- Global error handling middleware
- Request/response logging
- Health checks for downstream services
