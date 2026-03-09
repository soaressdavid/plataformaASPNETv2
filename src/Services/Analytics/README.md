# Analytics Service

Real-time analytics and metrics processing service for tracking user behavior, content performance, and platform health.

## Features

### 1. Telemetry Event Processing (Requirements 31.1, 31.2)
- Subscribes to analytics events from RabbitMQ
- Processes events asynchronously
- Stores aggregated metrics in data warehouse
- Event types: LessonCompleted, ChallengeCompleted, UserActivity, ContentDropOff

### 2. Lesson Completion Metrics (Requirement 31.8)
- Tracks completion count per lesson
- Calculates average completion time
- Identifies lessons with low completion rate (<50%)
- Provides insights for content improvement

### 3. Retention Analysis (Requirement 31.9)
- Calculates 7-day, 30-day, 90-day retention by cohort
- Tracks active users per day/week/month
- Generates retention reports
- Identifies at-risk users

### 4. Drop-Off Point Detection (Requirement 31.10)
- Tracks where users abandon lessons/challenges
- Calculates drop-off percentage per content item
- Flags problematic content for review (>50% drop-off)
- Provides detailed drop-off analysis

## API Endpoints

### GET /api/analytics/lessons/{lessonId}/metrics
Gets completion metrics for a lesson.

**Response:**
```json
{
  "lessonId": "lesson-guid",
  "completionCount": 1250,
  "averageCompletionTimeMs": 180000,
  "completionRate": 0.75
}
```

### GET /api/analytics/lessons/low-completion
Gets lessons with low completion rate.

**Response:**
```json
[
  {
    "lessonId": "lesson-guid",
    "completionCount": 50,
    "completionRate": 0.35
  }
]
```

### GET /api/analytics/retention
Gets retention metrics for cohorts.

**Response:**
```json
[
  {
    "cohortStartDate": "2025-01-01",
    "cohortSize": 1000,
    "day7Retention": 0.65,
    "day30Retention": 0.45,
    "day90Retention": 0.30
  }
]
```

### GET /api/analytics/active-users
Gets active users metrics.

**Query Parameters:**
- `startDate`: Start date (ISO 8601)
- `endDate`: End date (ISO 8601)

**Response:**
```json
{
  "startDate": "2025-01-01",
  "endDate": "2025-01-31",
  "dailyActiveUsers": 5000,
  "weeklyActiveUsers": 15000,
  "monthlyActiveUsers": 30000
}
```

### GET /api/analytics/content/{contentId}/dropoff
Gets drop-off analysis for content.

**Response:**
```json
{
  "contentId": "content-guid",
  "totalViews": 1000,
  "totalDropOffs": 350,
  "dropOffRate": 0.35,
  "dropOffPoints": [
    {
      "location": "Section 3",
      "dropOffCount": 150,
      "dropOffPercentage": 0.15
    }
  ]
}
```

### GET /api/analytics/content/high-dropoff
Gets content with high drop-off rate.

**Response:**
```json
[
  {
    "contentId": "content-guid",
    "dropOffRate": 0.65,
    "totalViews": 500
  }
]
```

## Event Processing

### Telemetry Events

**LessonCompleted:**
```json
{
  "eventType": "LessonCompleted",
  "userId": "user-guid",
  "contentId": "lesson-guid",
  "timestamp": "2025-03-09T10:30:00Z",
  "properties": {
    "completionTimeMs": 180000
  }
}
```

**UserActivity:**
```json
{
  "eventType": "UserActivity",
  "userId": "user-guid",
  "timestamp": "2025-03-09T10:30:00Z",
  "properties": {
    "activityType": "PageView",
    "page": "/courses/123"
  }
}
```

**ContentDropOff:**
```json
{
  "eventType": "ContentDropOff",
  "userId": "user-guid",
  "contentId": "lesson-guid",
  "timestamp": "2025-03-09T10:30:00Z",
  "properties": {
    "dropOffPoint": "Section 3",
    "timeSpentMs": 120000
  }
}
```

## Metrics Calculated

### Lesson Metrics
- **Completion Count**: Total number of completions
- **Average Completion Time**: Mean time to complete
- **Completion Rate**: (Completions / Views) * 100

### Retention Metrics
- **Day 7 Retention**: % of cohort active after 7 days
- **Day 30 Retention**: % of cohort active after 30 days
- **Day 90 Retention**: % of cohort active after 90 days
- **DAU/WAU/MAU**: Daily/Weekly/Monthly Active Users

### Drop-Off Metrics
- **Drop-Off Rate**: (Drop-offs / Views) * 100
- **Drop-Off Points**: Specific locations where users leave
- **Drop-Off Percentage**: % of users dropping at each point

## Data Warehouse

Analytics data is stored in a separate data warehouse optimized for analytical queries:

- **Time-series data**: User activities, events
- **Aggregated metrics**: Pre-calculated summaries
- **Dimensional data**: User cohorts, content metadata

## Background Services

### TelemetryEventProcessor
- Consumes events from RabbitMQ
- Processes events asynchronously
- Stores metrics in data warehouse
- Handles retries and error recovery

## Configuration

### appsettings.json
```json
{
  "RabbitMQ": {
    "HostName": "localhost",
    "QueueName": "analytics.telemetry"
  },
  "DataWarehouse": {
    "ConnectionString": "connection-string"
  }
}
```

## Requirements Validation

- ✅ 31.1: Telemetry event processing
- ✅ 31.2: Asynchronous processing
- ✅ 31.8: Lesson completion metrics
- ✅ 31.9: Retention analysis
- ✅ 31.10: Drop-off point detection

## Running the Service

```bash
cd src/Services/Analytics
dotnet run
```

The service will start on port 5009 (configurable).

## Testing

```bash
dotnet test tests/Analytics.Tests/
```

## Integration

### Publishing Events
```csharp
// From other services
await _messageBus.PublishAsync("analytics.telemetry", new TelemetryEvent
{
    EventType = "LessonCompleted",
    UserId = userId,
    ContentId = lessonId,
    Timestamp = DateTime.UtcNow,
    Properties = new Dictionary<string, object>
    {
        ["completionTimeMs"] = 180000
    }
});
```

## Dashboards

Analytics data can be visualized in:
- Grafana dashboards
- Custom admin panels
- Kibana for log analysis
- Application Insights for real-time monitoring
