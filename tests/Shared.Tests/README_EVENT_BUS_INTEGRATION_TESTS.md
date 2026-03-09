# Event Bus Integration Tests

## Overview

The `EventBusIntegrationTests.cs` file contains integration tests for the RabbitMQ event bus implementation. These tests validate:

- Event publishing and consumption
- Event ordering and delivery guarantees
- Message persistence
- Concurrent publishing
- Routing with different routing keys
- Message requeue on Nack (delivery guarantees)

**Validates: Requirement 11.6 - RabbitMQ for asynchronous communication**

## Prerequisites

These tests require a running RabbitMQ instance. The tests are marked with `Skip` attributes by default to prevent failures in environments where RabbitMQ is not available.

### Option 1: Run RabbitMQ with Docker

```bash
docker run -d --name rabbitmq-test \
  -p 5672:5672 \
  -p 15672:15672 \
  -e RABBITMQ_DEFAULT_USER=guest \
  -e RABBITMQ_DEFAULT_PASS=guest \
  rabbitmq:3-management
```

### Option 2: Use Docker Compose

If you have a `docker-compose.yml` file in the project root:

```bash
docker-compose up -d rabbitmq
```

## Running the Tests

### Run All Tests (Including Skipped)

To run the integration tests, you need to remove the `Skip` attribute or use the `--filter` option:

```bash
# Run all tests in the EventBusIntegrationTests class
dotnet test tests/Shared.Tests/Shared.Tests.csproj --filter "FullyQualifiedName~EventBusIntegrationTests"
```

### Run Specific Test

```bash
dotnet test tests/Shared.Tests/Shared.Tests.csproj --filter "FullyQualifiedName~EventBusIntegrationTests.PublishAsync_WithChallengeCompletedEvent_PublishesSuccessfully"
```

### Environment Variables

You can configure the RabbitMQ connection using environment variables:

- `RABBITMQ_HOST` - RabbitMQ hostname (default: `localhost`)
- `RABBITMQ_PORT` - RabbitMQ port (default: `5672`)
- `RABBITMQ_USER` - RabbitMQ username (default: `guest`)
- `RABBITMQ_PASSWORD` - RabbitMQ password (default: `guest`)

Example:

```bash
export RABBITMQ_HOST=localhost
export RABBITMQ_PORT=5672
export RABBITMQ_USER=guest
export RABBITMQ_PASSWORD=guest

dotnet test tests/Shared.Tests/Shared.Tests.csproj --filter "FullyQualifiedName~EventBusIntegrationTests"
```

## Test Coverage

### 1. PublishAsync_WithChallengeCompletedEvent_PublishesSuccessfully
- **Purpose**: Verifies that a ChallengeCompletedEvent can be published successfully
- **Validates**: Basic event publishing functionality

### 2. PublishAsync_WithMultipleEvents_MaintainsOrder
- **Purpose**: Verifies that multiple events published in sequence maintain their order
- **Validates**: Event ordering guarantee

### 3. PublishAsync_WithPersistentMessages_SurvivesRestart
- **Purpose**: Verifies that messages are marked as persistent and can survive broker restarts
- **Validates**: Message durability

### 4. EventBus_EndToEnd_PublishAndConsume
- **Purpose**: Tests the complete flow from publishing to potential consumption
- **Validates**: End-to-end event bus functionality

### 5. PublishAsync_WithDifferentRoutingKeys_RoutesCorrectly
- **Purpose**: Verifies that events with different routing keys are routed correctly
- **Validates**: Topic-based routing

### 6. PublishAsync_WithLessonCompletedEvent_PublishesSuccessfully
- **Purpose**: Verifies that LessonCompletedEvent can be published successfully
- **Validates**: Support for different event types

### 7. PublishAsync_WithConcurrentPublishes_HandlesCorrectly
- **Purpose**: Verifies that the event bus can handle concurrent publishing
- **Validates**: Thread safety and concurrent access

### 8. PublishAsync_WithRetry_RecoversFromTransientFailure
- **Purpose**: Verifies that the retry mechanism works correctly
- **Validates**: Resilience and retry logic

### 9. EventBus_DeliveryGuarantee_MessageNotLostOnNack
- **Purpose**: Verifies that messages are requeued when Nack'd
- **Validates**: Delivery guarantees and message requeue

## Troubleshooting

### Connection Refused Error

If you see a connection refused error, ensure RabbitMQ is running:

```bash
docker ps | grep rabbitmq
```

### Tests Timing Out

If tests are timing out, check RabbitMQ logs:

```bash
docker logs rabbitmq-test
```

### Queue Already Exists Error

If you see queue already exists errors, you may need to clean up test queues:

```bash
# Access RabbitMQ management UI
# http://localhost:15672 (guest/guest)
# Or use rabbitmqadmin CLI tool
```

## CI/CD Integration

For CI/CD pipelines, you can use a service container:

### GitHub Actions Example

```yaml
services:
  rabbitmq:
    image: rabbitmq:3-management
    ports:
      - 5672:5672
      - 15672:15672
    env:
      RABBITMQ_DEFAULT_USER: guest
      RABBITMQ_DEFAULT_PASS: guest
```

### Azure DevOps Example

```yaml
resources:
  containers:
  - container: rabbitmq
    image: rabbitmq:3-management
    ports:
    - 5672:5672
    - 15672:15672
```

## Notes

- These tests use a separate test exchange (`test.learning.events`) and test queues to avoid interfering with production data
- Test queues and exchanges are cleaned up in the `DisposeAsync` method
- Tests are designed to be idempotent and can be run multiple times
- The in-memory database is used for entity storage to avoid database dependencies
