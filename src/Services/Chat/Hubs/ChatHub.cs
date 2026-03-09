using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Shared.Data;
using Shared.Entities;
using System.Collections.Concurrent;
using System.Text.Json;

namespace Chat.Service.Hubs;

/// <summary>
/// SignalR hub for real-time chat functionality
/// Validates: Requirements 34.1, 34.2, 34.3, 34.4, 34.5, 34.6
/// </summary>
public class ChatHub : Hub
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<ChatHub> _logger;
    private static readonly ConcurrentDictionary<string, UserPresence> _onlineUsers = new();

    public ChatHub(ApplicationDbContext context, ILogger<ChatHub> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Called when a user connects to the hub
    /// Validates: Requirement 34.4 (online status)
    /// </summary>
    public override async Task OnConnectedAsync()
    {
        var userId = GetUserId();
        if (userId == null)
        {
            _logger.LogWarning("User connected without valid user ID");
            await base.OnConnectedAsync();
            return;
        }

        // Add user to online users
        _onlineUsers.TryAdd(userId, new UserPresence
        {
            UserId = userId,
            ConnectionId = Context.ConnectionId,
            ConnectedAt = DateTime.UtcNow
        });

        // Get user's chat rooms
        var userRooms = await _context.ChatRoomMembers
            .Where(m => m.UserId == Guid.Parse(userId))
            .Select(m => m.ChatRoomId.ToString())
            .ToListAsync();

        // Join all user's rooms
        foreach (var roomId in userRooms)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, roomId);
        }

        // Notify all rooms that user is online
        foreach (var roomId in userRooms)
        {
            await Clients.Group(roomId).SendAsync("UserOnline", userId);
        }

        _logger.LogInformation("User {UserId} connected to chat", userId);
        await base.OnConnectedAsync();
    }

    /// <summary>
    /// Called when a user disconnects from the hub
    /// Validates: Requirement 34.4 (online status)
    /// </summary>
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var userId = GetUserId();
        if (userId != null)
        {
            _onlineUsers.TryRemove(userId, out _);

            // Get user's chat rooms
            var userRooms = await _context.ChatRoomMembers
                .Where(m => m.UserId == Guid.Parse(userId))
                .Select(m => m.ChatRoomId.ToString())
                .ToListAsync();

            // Notify all rooms that user is offline
            foreach (var roomId in userRooms)
            {
                await Clients.Group(roomId).SendAsync("UserOffline", userId);
            }

            _logger.LogInformation("User {UserId} disconnected from chat", userId);
        }

        await base.OnDisconnectedAsync(exception);
    }

    /// <summary>
    /// Send a message to a chat room
    /// Validates: Requirements 34.1, 34.5, 34.6
    /// </summary>
    public async Task SendMessage(string roomId, string content, string? codeLanguage = null)
    {
        var userId = GetUserId();
        if (userId == null)
        {
            throw new HubException("User not authenticated");
        }

        var userGuid = Guid.Parse(userId);
        var roomGuid = Guid.Parse(roomId);

        // Verify user is a member of the room
        var membership = await _context.ChatRoomMembers
            .FirstOrDefaultAsync(m => m.ChatRoomId == roomGuid && m.UserId == userGuid);

        if (membership == null)
        {
            throw new HubException("User is not a member of this room");
        }

        // Check if user is muted
        if (membership.IsMuted && membership.MutedUntil > DateTime.UtcNow)
        {
            throw new HubException("You are muted in this room");
        }

        // Moderate content (basic profanity filter)
        var isModerated = ContainsProfanity(content);

        // Create message
        var message = new ChatMessage
        {
            ChatRoomId = roomGuid,
            UserId = userGuid,
            Content = content,
            Type = string.IsNullOrEmpty(codeLanguage) ? ChatMessageType.Text : ChatMessageType.Code,
            CodeLanguage = codeLanguage,
            IsModerated = isModerated,
            CreatedAt = DateTime.UtcNow
        };

        _context.ChatMessages.Add(message);
        await _context.SaveChangesAsync();

        // Get user info for the message
        var user = await _context.Users.FindAsync(userGuid);

        // Broadcast message to room
        await Clients.Group(roomId).SendAsync("ReceiveMessage", new
        {
            id = message.Id,
            roomId = message.ChatRoomId,
            userId = message.UserId,
            userName = user?.Name ?? "Unknown",
            content = isModerated ? "[Message moderated]" : message.Content,
            type = message.Type.ToString(),
            codeLanguage = message.CodeLanguage,
            createdAt = message.CreatedAt,
            isModerated = message.IsModerated
        });

        _logger.LogInformation("Message sent to room {RoomId} by user {UserId}", roomId, userId);
    }

    /// <summary>
    /// Join a chat room
    /// Validates: Requirement 34.2, 34.3
    /// </summary>
    public async Task JoinRoom(string roomId)
    {
        var userId = GetUserId();
        if (userId == null)
        {
            throw new HubException("User not authenticated");
        }

        var userGuid = Guid.Parse(userId);
        var roomGuid = Guid.Parse(roomId);

        // Check if room exists
        var room = await _context.ChatRooms.FindAsync(roomGuid);
        if (room == null)
        {
            throw new HubException("Room not found");
        }

        // Check if already a member
        var existingMembership = await _context.ChatRoomMembers
            .FirstOrDefaultAsync(m => m.ChatRoomId == roomGuid && m.UserId == userGuid);

        if (existingMembership == null)
        {
            // Add user to room
            var membership = new ChatRoomMember
            {
                ChatRoomId = roomGuid,
                UserId = userGuid,
                JoinedAt = DateTime.UtcNow
            };

            _context.ChatRoomMembers.Add(membership);
            await _context.SaveChangesAsync();
        }

        // Add connection to SignalR group
        await Groups.AddToGroupAsync(Context.ConnectionId, roomId);

        // Notify room that user joined
        await Clients.Group(roomId).SendAsync("UserJoined", userId);

        _logger.LogInformation("User {UserId} joined room {RoomId}", userId, roomId);
    }

    /// <summary>
    /// Leave a chat room
    /// Validates: Requirement 34.3
    /// </summary>
    public async Task LeaveRoom(string roomId)
    {
        var userId = GetUserId();
        if (userId == null)
        {
            throw new HubException("User not authenticated");
        }

        // Remove connection from SignalR group
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomId);

        // Notify room that user left
        await Clients.Group(roomId).SendAsync("UserLeft", userId);

        _logger.LogInformation("User {UserId} left room {RoomId}", userId, roomId);
    }

    /// <summary>
    /// Add emoji reaction to a message
    /// Validates: Requirement 34.5
    /// </summary>
    public async Task AddReaction(string messageId, string emoji)
    {
        var userId = GetUserId();
        if (userId == null)
        {
            throw new HubException("User not authenticated");
        }

        var messageGuid = Guid.Parse(messageId);
        var message = await _context.ChatMessages
            .Include(m => m.ChatRoom)
            .FirstOrDefaultAsync(m => m.Id == messageGuid);

        if (message == null)
        {
            throw new HubException("Message not found");
        }

        // Parse existing reactions
        var reactions = string.IsNullOrEmpty(message.Reactions)
            ? new List<MessageReaction>()
            : JsonSerializer.Deserialize<List<MessageReaction>>(message.Reactions) ?? new List<MessageReaction>();

        // Find or create reaction
        var reaction = reactions.FirstOrDefault(r => r.Emoji == emoji);
        if (reaction == null)
        {
            reaction = new MessageReaction { Emoji = emoji, UserIds = new List<string>() };
            reactions.Add(reaction);
        }

        // Add user to reaction if not already present
        if (!reaction.UserIds.Contains(userId))
        {
            reaction.UserIds.Add(userId);
        }

        // Save reactions
        message.Reactions = JsonSerializer.Serialize(reactions);
        await _context.SaveChangesAsync();

        // Broadcast reaction update
        await Clients.Group(message.ChatRoomId.ToString()).SendAsync("ReactionAdded", new
        {
            messageId = message.Id,
            emoji,
            userId
        });

        _logger.LogInformation("Reaction {Emoji} added to message {MessageId} by user {UserId}", emoji, messageId, userId);
    }

    /// <summary>
    /// Get online users in a room
    /// Validates: Requirement 34.4
    /// </summary>
    public async Task<List<string>> GetOnlineUsers(string roomId)
    {
        var roomGuid = Guid.Parse(roomId);

        // Get room members
        var memberIds = await _context.ChatRoomMembers
            .Where(m => m.ChatRoomId == roomGuid)
            .Select(m => m.UserId.ToString())
            .ToListAsync();

        // Filter to online users
        var onlineUsers = memberIds.Where(id => _onlineUsers.ContainsKey(id)).ToList();

        return onlineUsers;
    }

    /// <summary>
    /// Report a message
    /// Validates: Requirement 34.10
    /// </summary>
    public async Task ReportMessage(string messageId, string reason)
    {
        var userId = GetUserId();
        if (userId == null)
        {
            throw new HubException("User not authenticated");
        }

        var messageGuid = Guid.Parse(messageId);
        var message = await _context.ChatMessages.FindAsync(messageGuid);

        if (message == null)
        {
            throw new HubException("Message not found");
        }

        message.IsReported = true;
        await _context.SaveChangesAsync();

        _logger.LogWarning("Message {MessageId} reported by user {UserId} for reason: {Reason}", messageId, userId, reason);
    }

    /// <summary>
    /// Block a user
    /// Validates: Requirement 34.7
    /// </summary>
    public async Task BlockUser(string roomId, string blockedUserId)
    {
        var userId = GetUserId();
        if (userId == null)
        {
            throw new HubException("User not authenticated");
        }

        var userGuid = Guid.Parse(userId);
        var roomGuid = Guid.Parse(roomId);

        var membership = await _context.ChatRoomMembers
            .FirstOrDefaultAsync(m => m.ChatRoomId == roomGuid && m.UserId == userGuid);

        if (membership == null)
        {
            throw new HubException("User is not a member of this room");
        }

        // Parse blocked users
        var blockedUsers = string.IsNullOrEmpty(membership.BlockedUserIds)
            ? new List<string>()
            : JsonSerializer.Deserialize<List<string>>(membership.BlockedUserIds) ?? new List<string>();

        if (!blockedUsers.Contains(blockedUserId))
        {
            blockedUsers.Add(blockedUserId);
            membership.BlockedUserIds = JsonSerializer.Serialize(blockedUsers);
            await _context.SaveChangesAsync();
        }

        _logger.LogInformation("User {UserId} blocked user {BlockedUserId} in room {RoomId}", userId, blockedUserId, roomId);
    }

    private string? GetUserId()
    {
        return Context.User?.FindFirst("sub")?.Value
            ?? Context.User?.FindFirst("userId")?.Value
            ?? Context.User?.Identity?.Name;
    }

    private bool ContainsProfanity(string content)
    {
        // Basic profanity filter - in production, use a proper service
        var profanityWords = new[] { "badword1", "badword2", "badword3" };
        var lowerContent = content.ToLower();
        return profanityWords.Any(word => lowerContent.Contains(word));
    }
}

public class UserPresence
{
    public string UserId { get; set; } = string.Empty;
    public string ConnectionId { get; set; } = string.Empty;
    public DateTime ConnectedAt { get; set; }
}

public class MessageReaction
{
    public string Emoji { get; set; } = string.Empty;
    public List<string> UserIds { get; set; } = new();
}
