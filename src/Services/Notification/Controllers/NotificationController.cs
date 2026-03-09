using Microsoft.AspNetCore.Mvc;
using Notification.Service.DTOs;
using Notification.Service.Services;

namespace Notification.Service.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NotificationController : ControllerBase
{
    private readonly INotificationService _notificationService;
    private readonly ILogger<NotificationController> _logger;

    public NotificationController(
        INotificationService notificationService,
        ILogger<NotificationController> logger)
    {
        _notificationService = notificationService;
        _logger = logger;
    }

    /// <summary>
    /// Send a notification through specified channels
    /// </summary>
    [HttpPost("send")]
    public async Task<IActionResult> SendNotification([FromBody] SendNotificationRequest request)
    {
        try
        {
            await _notificationService.SendNotificationAsync(
                request.UserId,
                request.Title,
                request.Message,
                request.Type,
                request.Channels,
                request.Data
            );

            return Ok(new { success = true, message = "Notification sent successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending notification to user {UserId}", request.UserId);
            return StatusCode(500, new { success = false, message = "Failed to send notification" });
        }
    }

    /// <summary>
    /// Get user notification preferences
    /// </summary>
    [HttpGet("preferences/{userId}")]
    public async Task<IActionResult> GetPreferences(Guid userId)
    {
        try
        {
            var preferences = await _notificationService.GetUserPreferencesAsync(userId);
            return Ok(preferences);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting preferences for user {UserId}", userId);
            return StatusCode(500, new { message = "Failed to get preferences" });
        }
    }

    /// <summary>
    /// Update user notification preferences
    /// </summary>
    [HttpPut("preferences/{userId}")]
    public async Task<IActionResult> UpdatePreferences(
        Guid userId,
        [FromBody] UserNotificationPreferences preferences)
    {
        try
        {
            preferences.UserId = userId;
            await _notificationService.UpdateUserPreferencesAsync(preferences);
            return Ok(preferences);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating preferences for user {UserId}", userId);
            return StatusCode(500, new { message = "Failed to update preferences" });
        }
    }

    /// <summary>
    /// Get user notifications
    /// </summary>
    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetUserNotifications(
        Guid userId,
        [FromQuery] int page = 1,
        [FromQuery] int limit = 20,
        [FromQuery] bool unreadOnly = false)
    {
        try
        {
            var notifications = await _notificationService.GetUserNotificationsAsync(
                userId,
                page,
                limit,
                unreadOnly
            );

            var unreadCount = await _notificationService.GetUnreadCountAsync(userId);

            return Ok(new
            {
                notifications,
                total = notifications.Count,
                unreadCount
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting notifications for user {UserId}", userId);
            return StatusCode(500, new { message = "Failed to get notifications" });
        }
    }

    /// <summary>
    /// Get unread notification count
    /// </summary>
    [HttpGet("unread/count/{userId}")]
    public async Task<IActionResult> GetUnreadCount(Guid userId)
    {
        try
        {
            var count = await _notificationService.GetUnreadCountAsync(userId);
            return Ok(new { count });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting unread count for user {UserId}", userId);
            return StatusCode(500, new { message = "Failed to get unread count" });
        }
    }

    /// <summary>
    /// Mark notification as read
    /// </summary>
    [HttpPut("{notificationId}/read")]
    public async Task<IActionResult> MarkAsRead(Guid notificationId)
    {
        try
        {
            await _notificationService.MarkAsReadAsync(notificationId);
            return Ok(new { success = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error marking notification {NotificationId} as read", notificationId);
            return StatusCode(500, new { message = "Failed to mark as read" });
        }
    }

    /// <summary>
    /// Mark all notifications as read for a user
    /// </summary>
    [HttpPut("read-all/{userId}")]
    public async Task<IActionResult> MarkAllAsRead(Guid userId)
    {
        try
        {
            await _notificationService.MarkAllAsReadAsync(userId);
            return Ok(new { success = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error marking all notifications as read for user {UserId}", userId);
            return StatusCode(500, new { message = "Failed to mark all as read" });
        }
    }

    /// <summary>
    /// Delete a notification
    /// </summary>
    [HttpDelete("{notificationId}")]
    public async Task<IActionResult> DeleteNotification(Guid notificationId)
    {
        try
        {
            await _notificationService.DeleteNotificationAsync(notificationId);
            return Ok(new { success = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting notification {NotificationId}", notificationId);
            return StatusCode(500, new { message = "Failed to delete notification" });
        }
    }
}
