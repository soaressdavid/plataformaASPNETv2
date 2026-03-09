using Notification.Service.DTOs;
using Notification.Service.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace Notification.Service.Services;

public class NotificationService : INotificationService
{
    private readonly IHubContext<NotificationHub> _hubContext;
    private readonly ILogger<NotificationService> _logger;
    
    // In-memory storage (replace with database in production)
    private static readonly Dictionary<Guid, UserNotificationPreferences> _preferences = new();
    private static readonly List<UserNotification> _notifications = new();

    public NotificationService(
        IHubContext<NotificationHub> hubContext,
        ILogger<NotificationService> logger)
    {
        _hubContext = hubContext;
        _logger = logger;
    }

    public async Task SendNotificationAsync(
        Guid userId,
        string title,
        string message,
        NotificationType type,
        NotificationChannel channels,
        Dictionary<string, object>? data = null)
    {
        var preferences = await GetUserPreferencesAsync(userId);

        // Send in-app notification
        if (channels.HasFlag(NotificationChannel.InApp) && preferences.InAppNotificationsEnabled)
        {
            await SendInAppNotificationAsync(userId, title, message, type, data);
        }

        // Send email notification
        if (channels.HasFlag(NotificationChannel.Email) && preferences.EmailNotificationsEnabled)
        {
            await SendEmailNotificationAsync(userId, title, message, type, data);
        }

        // Send push notification
        if (channels.HasFlag(NotificationChannel.Push) && preferences.PushNotificationsEnabled)
        {
            await SendPushNotificationAsync(userId, title, message, type, data);
        }
    }

    private async Task SendInAppNotificationAsync(
        Guid userId,
        string title,
        string message,
        NotificationType type,
        Dictionary<string, object>? data)
    {
        var notification = new UserNotification
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Title = title,
            Message = message,
            Type = type,
            IsRead = false,
            CreatedAt = DateTime.UtcNow,
            Data = data
        };

        _notifications.Add(notification);

        // Send via SignalR
        try
        {
            await _hubContext.Clients.User(userId.ToString())
                .SendAsync("ReceiveNotification", notification);
            
            _logger.LogInformation("In-app notification sent to user {UserId}", userId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send in-app notification to user {UserId}", userId);
        }
    }

    private async Task SendEmailNotificationAsync(
        Guid userId,
        string title,
        string message,
        NotificationType type,
        Dictionary<string, object>? data)
    {
        // TODO: Implement SendGrid integration
        _logger.LogInformation("Email notification would be sent to user {UserId}: {Title}", userId, title);
        await Task.CompletedTask;
    }

    private async Task SendPushNotificationAsync(
        Guid userId,
        string title,
        string message,
        NotificationType type,
        Dictionary<string, object>? data)
    {
        // TODO: Implement push notification
        _logger.LogInformation("Push notification would be sent to user {UserId}: {Title}", userId, title);
        await Task.CompletedTask;
    }

    public Task<UserNotificationPreferences> GetUserPreferencesAsync(Guid userId)
    {
        if (!_preferences.ContainsKey(userId))
        {
            _preferences[userId] = new UserNotificationPreferences { UserId = userId };
        }

        return Task.FromResult(_preferences[userId]);
    }

    public Task UpdateUserPreferencesAsync(UserNotificationPreferences preferences)
    {
        _preferences[preferences.UserId] = preferences;
        _logger.LogInformation("Updated preferences for user {UserId}", preferences.UserId);
        return Task.CompletedTask;
    }

    public Task<List<UserNotification>> GetUserNotificationsAsync(
        Guid userId,
        int page,
        int limit,
        bool unreadOnly)
    {
        var query = _notifications.Where(n => n.UserId == userId);

        if (unreadOnly)
        {
            query = query.Where(n => !n.IsRead);
        }

        var result = query
            .OrderByDescending(n => n.CreatedAt)
            .Skip((page - 1) * limit)
            .Take(limit)
            .ToList();

        return Task.FromResult(result);
    }

    public Task<int> GetUnreadCountAsync(Guid userId)
    {
        var count = _notifications.Count(n => n.UserId == userId && !n.IsRead);
        return Task.FromResult(count);
    }

    public Task MarkAsReadAsync(Guid notificationId)
    {
        var notification = _notifications.FirstOrDefault(n => n.Id == notificationId);
        if (notification != null)
        {
            notification.IsRead = true;
            notification.ReadAt = DateTime.UtcNow;
            _logger.LogInformation("Marked notification {NotificationId} as read", notificationId);
        }

        return Task.CompletedTask;
    }

    public Task MarkAllAsReadAsync(Guid userId)
    {
        var userNotifications = _notifications.Where(n => n.UserId == userId && !n.IsRead);
        foreach (var notification in userNotifications)
        {
            notification.IsRead = true;
            notification.ReadAt = DateTime.UtcNow;
        }

        _logger.LogInformation("Marked all notifications as read for user {UserId}", userId);
        return Task.CompletedTask;
    }

    public Task DeleteNotificationAsync(Guid notificationId)
    {
        var notification = _notifications.FirstOrDefault(n => n.Id == notificationId);
        if (notification != null)
        {
            _notifications.Remove(notification);
            _logger.LogInformation("Deleted notification {NotificationId}", notificationId);
        }

        return Task.CompletedTask;
    }
}
