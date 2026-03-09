# Notification Service

Multi-channel notification system with SignalR real-time delivery, SendGrid email integration, and user preference management.

## Features

### 1. Multi-Channel Notification Dispatch (Requirements 17.1, 17.2, 17.3)
- **In-App**: Real-time notifications via SignalR
- **Email**: Transactional and marketing emails via SendGrid
- **Push**: Placeholder for mobile push notifications
- User preference checking before sending

### 2. SignalR Real-Time Hub (Requirement 17.4)
- WebSocket connections for instant notifications
- User-specific message delivery
- Connection lifecycle management
- Broadcast capabilities

### 3. SendGrid Email Integration (Requirement 17.5)
- Transactional email sending
- Template-based emails
- Bulk email support
- Retry logic with error handling

### 4. Daily Digest System (Requirement 17.6)
- Background service runs at 8 AM UTC
- Aggregates previous day's activities
- Respects user timezone preferences
- HTML email templates

### 5. User Preferences Management (Requirement 17.7)
- Enable/disable per notification type
- Channel-specific preferences
- Daily digest opt-in/out
- Granular control (achievements, level-ups, streaks, etc.)

## API Endpoints

### POST /api/notifications/send
Sends a notification through specified channels.

**Request:**
```json
{
  "userId": "user-guid",
  "title": "Achievement Unlocked!",
  "message": "You've completed 10 challenges",
  "type": "Achievement",
  "channels": 3,
  "data": {
    "achievementId": "achievement-guid"
  }
}
```

**Response:**
```json
{
  "success": true,
  "message": "Notification sent successfully"
}
```

### GET /api/notifications/preferences/{userId}
Gets user notification preferences.

**Response:**
```json
{
  "userId": "user-guid",
  "inAppNotificationsEnabled": true,
  "emailNotificationsEnabled": true,
  "pushNotificationsEnabled": false,
  "achievementEmailsEnabled": true,
  "levelUpEmailsEnabled": true,
  "streakReminderEmailsEnabled": true,
  "courseUpdateEmailsEnabled": true,
  "dailyDigestEnabled": true
}
```

### PUT /api/notifications/preferences/{userId}
Updates user notification preferences.

## SignalR Hub

### Connection
```javascript
const connection = new signalR.HubConnectionBuilder()
    .withUrl("/notificationHub")
    .build();

connection.on("ReceiveNotification", (notification) => {
    console.log("Received:", notification);
    // Display notification in UI
});

await connection.start();
```

### Events
- `ReceiveNotification`: Receives real-time notifications

## Notification Types

- `Info`: General information
- `Success`: Success messages
- `Warning`: Warning messages
- `Error`: Error messages
- `Achievement`: Achievement unlocked
- `LevelUp`: Level progression
- `MissionComplete`: Mission completed
- `StreakReminder`: Streak about to break
- `CourseUpdate`: New course content

## Configuration

### appsettings.json
```json
{
  "SendGrid": {
    "ApiKey": "your-sendgrid-api-key",
    "FromEmail": "noreply@aspnetlearning.com",
    "FromName": "ASP.NET Learning Platform"
  },
  "DailyDigest": {
    "SendTimeUtc": "08:00:00"
  }
}
```

## Email Templates

### Achievement Unlocked
```html
<h1>🎉 Achievement Unlocked!</h1>
<p>Congratulations! You've earned: <strong>{achievementName}</strong></p>
<p>XP Reward: +{xpReward}</p>
```

### Level Up
```html
<h1>🚀 Level Up!</h1>
<p>You've reached Level {level}!</p>
<p>Keep up the great work!</p>
```

### Daily Digest
```html
<h1>Your Daily Learning Summary</h1>
<ul>
  <li>Lessons completed: {lessonCount}</li>
  <li>Challenges solved: {challengeCount}</li>
  <li>XP gained: {xpGained}</li>
  <li>Current streak: {streak} days</li>
</ul>
```

## Background Services

### DailyDigestService
- Runs daily at 8 AM UTC
- Queries user activity from previous day
- Generates personalized HTML digest
- Sends to users with digest enabled
- Respects user timezone preferences

## Integration

### From Other Services
```csharp
// Send notification via message queue
await _messageBus.PublishAsync(new NotificationEvent
{
    UserId = userId,
    Title = "Achievement Unlocked",
    Message = "You've completed 10 challenges!",
    Type = NotificationType.Achievement,
    Channels = NotificationChannel.All
});
```

## Requirements Validation

- ✅ 17.1: In-app notifications
- ✅ 17.2: Email notifications
- ✅ 17.3: Push notifications (placeholder)
- ✅ 17.4: SignalR real-time hub
- ✅ 17.5: SendGrid integration
- ✅ 17.6: Daily digest system
- ✅ 17.7: User preferences management

## Running the Service

```bash
cd src/Services/Notification
dotnet run
```

The service will start on port 5008 (configurable).

## Testing

```bash
dotnet test tests/Notification.Tests/
```

## Dependencies

- SignalR for real-time communication
- SendGrid for email delivery
- RabbitMQ for event-driven notifications
