using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.Data;
using Shared.Entities;

namespace Chat.Service.Controllers;

/// <summary>
/// REST API for chat room management
/// Validates: Requirements 34.2, 34.3, 34.8
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ChatController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<ChatController> _logger;

    public ChatController(ApplicationDbContext context, ILogger<ChatController> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Get all chat rooms for a user
    /// </summary>
    [HttpGet("rooms")]
    public async Task<ActionResult<List<ChatRoomDto>>> GetUserRooms()
    {
        var userId = GetUserId();
        if (userId == null)
        {
            return Unauthorized();
        }

        var userGuid = Guid.Parse(userId);

        var rooms = await _context.ChatRoomMembers
            .Where(m => m.UserId == userGuid)
            .Include(m => m.ChatRoom)
            .ThenInclude(r => r.Messages.OrderByDescending(msg => msg.CreatedAt).Take(1))
            .Select(m => new ChatRoomDto
            {
                Id = m.ChatRoom.Id,
                Name = m.ChatRoom.Name,
                Type = m.ChatRoom.Type.ToString(),
                CourseId = m.ChatRoom.CourseId,
                LastMessage = m.ChatRoom.Messages.FirstOrDefault() != null
                    ? new ChatMessageDto
                    {
                        Id = m.ChatRoom.Messages.First().Id,
                        Content = m.ChatRoom.Messages.First().Content,
                        CreatedAt = m.ChatRoom.Messages.First().CreatedAt
                    }
                    : null,
                UnreadCount = m.ChatRoom.Messages.Count(msg => msg.CreatedAt > (m.LastReadAt ?? DateTime.MinValue))
            })
            .ToListAsync();

        return Ok(rooms);
    }

    /// <summary>
    /// Get messages for a chat room
    /// Validates: Requirement 34.8 (30 days history)
    /// </summary>
    [HttpGet("rooms/{roomId}/messages")]
    public async Task<ActionResult<List<ChatMessageDto>>> GetRoomMessages(Guid roomId, [FromQuery] int limit = 50, [FromQuery] DateTime? before = null)
    {
        var userId = GetUserId();
        if (userId == null)
        {
            return Unauthorized();
        }

        var userGuid = Guid.Parse(userId);

        // Verify user is a member
        var isMember = await _context.ChatRoomMembers
            .AnyAsync(m => m.ChatRoomId == roomId && m.UserId == userGuid);

        if (!isMember)
        {
            return Forbid();
        }

        // Get messages from last 30 days
        var thirtyDaysAgo = DateTime.UtcNow.AddDays(-30);
        var query = _context.ChatMessages
            .Where(m => m.ChatRoomId == roomId && m.CreatedAt >= thirtyDaysAgo);

        if (before.HasValue)
        {
            query = query.Where(m => m.CreatedAt < before.Value);
        }

        var messages = await query
            .OrderByDescending(m => m.CreatedAt)
            .Take(limit)
            .Include(m => m.User)
            .Select(m => new ChatMessageDto
            {
                Id = m.Id,
                UserId = m.UserId,
                UserName = m.User.Name,
                Content = m.IsModerated ? "[Message moderated]" : m.Content,
                Type = m.Type.ToString(),
                CodeLanguage = m.CodeLanguage,
                Reactions = m.Reactions,
                CreatedAt = m.CreatedAt,
                IsModerated = m.IsModerated
            })
            .ToListAsync();

        // Reverse to get chronological order
        messages.Reverse();

        return Ok(messages);
    }

    /// <summary>
    /// Create a new chat room (course or direct message)
    /// Validates: Requirements 34.2, 34.3
    /// </summary>
    [HttpPost("rooms")]
    public async Task<ActionResult<ChatRoomDto>> CreateRoom([FromBody] CreateRoomRequest request)
    {
        var userId = GetUserId();
        if (userId == null)
        {
            return Unauthorized();
        }

        var userGuid = Guid.Parse(userId);

        // For direct messages, check if room already exists
        if (request.Type == "DirectMessage" && request.MemberIds != null && request.MemberIds.Count == 1)
        {
            var otherUserId = Guid.Parse(request.MemberIds[0]);
            var existingRoom = await _context.ChatRooms
                .Where(r => r.Type == ChatRoomType.DirectMessage)
                .Where(r => r.Members.Any(m => m.UserId == userGuid) && r.Members.Any(m => m.UserId == otherUserId))
                .FirstOrDefaultAsync();

            if (existingRoom != null)
            {
                return Ok(new ChatRoomDto
                {
                    Id = existingRoom.Id,
                    Name = existingRoom.Name,
                    Type = existingRoom.Type.ToString(),
                    CourseId = existingRoom.CourseId
                });
            }
        }

        // Create new room
        var room = new ChatRoom
        {
            Name = request.Name,
            Type = Enum.Parse<ChatRoomType>(request.Type),
            CourseId = request.CourseId,
            CreatedAt = DateTime.UtcNow
        };

        _context.ChatRooms.Add(room);

        // Add creator as member
        var creatorMembership = new ChatRoomMember
        {
            ChatRoomId = room.Id,
            UserId = userGuid,
            JoinedAt = DateTime.UtcNow
        };
        _context.ChatRoomMembers.Add(creatorMembership);

        // Add other members
        if (request.MemberIds != null)
        {
            foreach (var memberId in request.MemberIds)
            {
                var memberGuid = Guid.Parse(memberId);
                if (memberGuid != userGuid)
                {
                    var membership = new ChatRoomMember
                    {
                        ChatRoomId = room.Id,
                        UserId = memberGuid,
                        JoinedAt = DateTime.UtcNow
                    };
                    _context.ChatRoomMembers.Add(membership);
                }
            }
        }

        await _context.SaveChangesAsync();

        _logger.LogInformation("Chat room {RoomId} created by user {UserId}", room.Id, userId);

        return CreatedAtAction(nameof(GetUserRooms), new { id = room.Id }, new ChatRoomDto
        {
            Id = room.Id,
            Name = room.Name,
            Type = room.Type.ToString(),
            CourseId = room.CourseId
        });
    }

    /// <summary>
    /// Mark messages as read
    /// </summary>
    [HttpPost("rooms/{roomId}/read")]
    public async Task<IActionResult> MarkAsRead(Guid roomId)
    {
        var userId = GetUserId();
        if (userId == null)
        {
            return Unauthorized();
        }

        var userGuid = Guid.Parse(userId);

        var membership = await _context.ChatRoomMembers
            .FirstOrDefaultAsync(m => m.ChatRoomId == roomId && m.UserId == userGuid);

        if (membership == null)
        {
            return NotFound();
        }

        membership.LastReadAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private string? GetUserId()
    {
        return User.FindFirst("sub")?.Value
            ?? User.FindFirst("userId")?.Value
            ?? User.Identity?.Name;
    }
}

public class ChatRoomDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public Guid? CourseId { get; set; }
    public ChatMessageDto? LastMessage { get; set; }
    public int UnreadCount { get; set; }
}

public class ChatMessageDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string? CodeLanguage { get; set; }
    public string? Reactions { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsModerated { get; set; }
}

public class CreateRoomRequest
{
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public Guid? CourseId { get; set; }
    public List<string>? MemberIds { get; set; }
}
