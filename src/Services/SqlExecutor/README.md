# SQL Executor Service

Isolated SQL Server execution environment for safe query execution with session management, validation, and resource limits.

## Features

### 1. Isolated Container Execution (Requirement 2.1, 2.2, 2.3)
- Each session gets its own SQL Server container with temporary database
- Complete isolation between sessions
- Automatic cleanup after 30 minutes of inactivity

### 2. Query Validation (Requirement 2.14)
- Whitelist: SELECT, INSERT, UPDATE, DELETE, CREATE TABLE, ALTER TABLE, DROP TABLE
- Blacklist: DROP DATABASE, SHUTDOWN, xp_cmdshell, BACKUP, RESTORE, GRANT, REVOKE
- Prevents SQL injection with single-statement enforcement

### 3. Query Execution with Timeout (Requirement 2.4, 2.8, 2.9)
- Default 5-second timeout (configurable)
- Result set limited to 1000 rows
- Returns results in JSON format
- Execution time tracking

### 4. Session Management (Requirement 2.11)
- Redis-based session tracking
- 30-minute TTL for inactive sessions
- Container reuse for same session
- Automatic cleanup of expired sessions

## API Endpoints

### POST /api/sql/execute
Executes a SQL query in an isolated container.

**Request:**
```json
{
  "query": "SELECT * FROM users WHERE id = 1",
  "sessionId": "optional-session-id",
  "timeoutSeconds": 5
}
```

**Response (Success):**
```json
{
  "success": true,
  "results": [
    { "id": 1, "name": "John", "email": "john@example.com" }
  ],
  "rowCount": 1,
  "rowsAffected": 0,
  "truncated": false,
  "sessionId": "abc-123",
  "executionTimeMs": 45
}
```

**Response (Timeout):**
```json
{
  "error": "QUERY_TIMEOUT",
  "message": "Query execution exceeded 5 second timeout",
  "sessionId": "abc-123",
  "executionTimeMs": 5000
}
```

### POST /api/sql/validate
Validates a SQL query without executing it.

**Request:**
```json
{
  "query": "DROP DATABASE master"
}
```

**Response:**
```json
{
  "isValid": false,
  "errorMessage": "Prohibited operation detected: DROP DATABASE",
  "prohibitedOperation": "DROP DATABASE"
}
```

### DELETE /api/sql/session/{sessionId}
Terminates a session and cleans up resources.

**Response:**
```json
{
  "message": "Session terminated successfully"
}
```

## Property-Based Tests

### Property 3: SQL Execution Isolation
- Each session has isolated database
- Changes in one session don't affect others
- Concurrent sessions don't interfere

### Property 4: SQL Challenge Validation
- Validates user queries against expected results
- All test cases must pass

### Property 5: SQL Query Timeout
- Queries timeout after configured limit (default 5s)
- Fast queries complete before timeout
- Timeout is enforced consistently

### Property 6: SQL Result Set Limit
- Result sets limited to 1000 rows
- Small result sets not truncated
- Truncation flag set when limited

### Property 7: SQL Destructive Operation Prevention
- DROP DATABASE blocked
- SHUTDOWN blocked
- xp_cmdshell blocked
- Safe operations allowed

## Configuration

### appsettings.json
```json
{
  "Redis": {
    "ConnectionString": "localhost:6379"
  },
  "SqlExecutor": {
    "DefaultTimeoutSeconds": 5,
    "MaxResultSetSize": 1000,
    "SessionTtlMinutes": 30
  }
}
```

## Security Features

1. **Query Validation**: Prevents destructive operations
2. **Container Isolation**: Each session in separate container
3. **Resource Limits**: Memory, CPU, and timeout limits
4. **Network Isolation**: No external network access
5. **Session Management**: Automatic cleanup of inactive sessions

## Running the Service

```bash
cd src/Services/SqlExecutor
dotnet run
```

The service will start on port 5007 (configurable).

## Testing

```bash
dotnet test tests/SqlExecutor.Tests/
```

## Docker Support

The service requires Docker to be running for container management.

```bash
docker pull mcr.microsoft.com/mssql/server:2022-latest
```

## Requirements Validation

- ✅ 2.1: SQL Server containers deployed on-demand
- ✅ 2.2: Isolated temporary databases per session
- ✅ 2.3: Automatic cleanup (30 min inactivity)
- ✅ 2.4: Query execution with timeout
- ✅ 2.6: SQL challenge validation
- ✅ 2.8: 5 second query timeout
- ✅ 2.9: 1000 row result set limit
- ✅ 2.11: Session management with Redis
- ✅ 2.14: Query validation (whitelist/blacklist)
