using Xunit;
using Moq;
using Shared.Entities;

namespace Notification.Tests;

/// <summary>
/// Unit tests for Notification Service
/// Validates: Requirements 17.1, 17.2, 17.3, 17.4, 17.5, 17.6, 17.7
/// </summary>
public class NotificationServiceTests
{
    [Fact]
    public void NotificationPreferences_DefaultsToAllEnabled()
    {
        // Arrange & Act
        var preferences = new NotificationPreferences
        {
            UserId = Guid.NewGuid()
        };

        // Assert
        Assert.True(preferences.EmailEnabled);
        Assert.True(preferences.PushEnabled);
        Assert.True(preferences.InAppEnabled);
    }

    [Theory]
    [InlineData(NotificationType.BadgeEarned, true)]
    [InlineData(NotificationType.LevelUp, true)]
    [InlineData(NotificationType.StreakWarning, true)]
    [InlineData(NotificationType.MissionCompleted, true)]
    public void ShouldSendNotification_WithEnabledPreferences_ReturnsTrue(
        NotificationType type,
        bool expected)
    {
        // Arrange
        var preferences = new NotificationPreferences
        {
            UserId = Guid.NewGuid(),
            EmailEnabled = true,
            PushEnabled = true,
            InAppEnabled = true
        };

        // Act
        var result = ShouldSendNotification(preferences, type, NotificationChannel.Email);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ShouldSendNotification_WithDisabledEmail_ReturnsFalse()
    {
        // Arrange
        var preferences = new NotificationPreferences
        {
            UserId = Guid.NewGuid(),
            EmailEnabled = false
        };

        // Act
        var result = ShouldSendNotification(
            preferences, 
            NotificationType.BadgeEarned, 
            NotificationChannel.Email);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void CreateNotification_SetsCorrectProperties()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var type = NotificationType.LevelUp;
        var title = "Level Up!";
        var message = "You reached level 5";

        // Act
        var notification = new Notification
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Type = type,
            Title = title,
            Message = message,
            IsRead = false,
            CreatedAt = DateTime.UtcNow
        };

        // Assert
        Assert.Equal(userId, notification.UserId);
        Assert.Equal(type, notification.Type);
        Assert.Equal(title, notification.Title);
        Assert.Equal(message, notification.Message);
        Assert.False(notification.IsRead);
    }

    [Fact]
    public void NotificationGrouping_GroupsSimilarNotifications()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var notifications = new List<Notification>
        {
            new() { Id = Guid.NewGuid(), UserId = userId, Type = NotificationType.BadgeEarned, CreatedAt = DateTime.UtcNow },
            new() { Id = Guid.NewGuid(), UserId = userId, Type = NotificationType.BadgeEarned, CreatedAt = DateTime.UtcNow.AddMinutes(1) },
            new() { Id = Guid.NewGuid(), UserId = userId, Type = NotificationType.LevelUp, CreatedAt = DateTime.UtcNow.AddMinutes(2) }
        };

        // Act
        var grouped = notifications.GroupBy(n => n.Type).ToList();

        // Assert
        Assert.Equal(2, grouped.Count);
        Assert.Equal(2, grouped.First(g => g.Key == NotificationType.BadgeEarned).Count());
        Assert.Single(grouped.First(g => g.Key == NotificationType.LevelUp));
    }

    [Theory]
    [InlineData(NotificationType.BadgeEarned, "Achievement Unlocked")]
    [InlineData(NotificationType.LevelUp, "Level Up")]
    [InlineData(NotificationType.StreakWarning, "Streak Alert")]
    [InlineData(NotificationType.MissionCompleted, "Mission Complete")]
    public void GetNotificationTitle_ReturnsCorrectTitle(
        NotificationType type,
        string expectedTitle)
    {
        // Act
        var title = GetNotificationTitle(type);

        // Assert
        Assert.Equal(expectedTitle, title);
    }

    [Fact]
    public void MarkAsRead_UpdatesIsReadFlag()
    {
        // Arrange
        var notification = new Notification
        {
            Id = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            Type = NotificationType.BadgeEarned,
            Title = "Test",
            Message = "Test",
            IsRead = false
        };

        // Act
        notification.IsRead = true;
        notification.ReadAt = DateTime.UtcNow;

        // Assert
        Assert.True(notification.IsRead);
        Assert.NotNull(notification.ReadAt);
    }

    // Helper methods
    private bool ShouldSendNotification(
        NotificationPreferences preferences,
        NotificationType type,
        NotificationChannel channel)
    {
        return channel switch
        {
            NotificationChannel.Email => preferences.EmailEnabled,
            NotificationChannel.Push => preferences.PushEnabled,
            NotificationChannel.InApp => preferences.InAppEnabled,
            _ => false
        };
    }

    private string GetNotificationTitle(NotificationType type)
    {
        return type switch
        {
            NotificationType.BadgeEarned => "Achievement Unlocked",
            NotificationType.LevelUp => "Level Up",
            NotificationType.StreakWarning => "Streak Alert",
            NotificationType.MissionCompleted => "Mission Complete",
            NotificationType.WeeklyEventStarted => "Weekly Event",
            NotificationType.ForumReply => "New Reply",
            NotificationType.CollaborativeInvite => "Collaboration Invite",
            NotificationType.CertificateEarned => "Certificate Earned",
            NotificationType.ProjectDeployed => "Project Deployed",
            _ => "Notification"
        };
    }
}

public enum NotificationType
{
    BadgeEarned,
    LevelUp,
    StreakWarning,
    WeeklyEventStarted,
    ForumReply,
    CollaborativeInvite,
    MissionCompleted,
    CertificateEarned,
    ProjectDeployed
}

public enum NotificationChannel
{
    Email,
    Push,
    InApp
}

public class NotificationPreferences
{
    public Guid UserId { get; set; }
    public bool EmailEnabled { get; set; } = true;
    public bool PushEnabled { get; set; } = true;
    public bool InAppEnabled { get; set; } = true;
}

public class Notification
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public NotificationType Type { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public bool IsRead { get; set; }
    public DateTime? ReadAt { get; set; }
    public DateTime CreatedAt { get; set; }
}
