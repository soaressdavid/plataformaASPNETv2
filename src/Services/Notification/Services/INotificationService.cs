using Notification.Service.DTOs;

namespace Notification.Service.Services;

public interface INotificationService
{
    Task SendNotificationAsync(
        Guid userId,
        string title,
        string message,
        NotificationType type,
        NotificationChannel channels,
        Dictionary<string, object>? data = null);

    Task<UserNotificationPreferences> GetUserPreferencesAsync(Guid userId);
    Task UpdateUserPreferencesAsync(UserNotificationPreferences preferences);
    Task<List<UserNotification>> GetUserNotificationsAsync(Guid userId, int page, int limit, bool unreadOnly);
    Task<int> GetUnreadCountAsync(Guid userId);
    Task MarkAsReadAsync(Guid notificationId);
    Task MarkAllAsReadAsync(Guid userId);
    Task DeleteNotificationAsync(Guid notificationId);
}
