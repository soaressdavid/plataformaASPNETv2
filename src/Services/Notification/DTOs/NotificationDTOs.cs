namespace Notification.Service.DTOs;

public class SendNotificationRequest
{
    public Guid UserId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public NotificationType Type { get; set; }
    public NotificationChannel Channels { get; set; }
    public Dictionary<string, object>? Data { get; set; }
}

public class UserNotificationPreferences
{
    public Guid UserId { get; set; }
    public bool InAppNotificationsEnabled { get; set; } = true;
    public bool EmailNotificationsEnabled { get; set; } = true;
    public bool PushNotificationsEnabled { get; set; } = false;
    public bool AchievementEmailsEnabled { get; set; } = true;
    public bool LevelUpEmailsEnabled { get; set; } = true;
    public bool StreakReminderEmailsEnabled { get; set; } = true;
    public bool CourseUpdateEmailsEnabled { get; set; } = true;
    public bool DailyDigestEnabled { get; set; } = true;
}

public class UserNotification
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public NotificationType Type { get; set; }
    public bool IsRead { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ReadAt { get; set; }
    public Dictionary<string, object>? Data { get; set; }
}

public enum NotificationType
{
    Info,
    Success,
    Warning,
    Error,
    Achievement,
    LevelUp,
    MissionComplete,
    StreakReminder,
    CourseUpdate
}

[Flags]
public enum NotificationChannel
{
    None = 0,
    InApp = 1,
    Email = 2,
    Push = 4,
    All = InApp | Email | Push
}
