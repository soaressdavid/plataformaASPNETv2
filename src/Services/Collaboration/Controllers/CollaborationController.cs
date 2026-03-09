using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.Data;
using Shared.Entities;

namespace Collaboration.Service.Controllers;

/// <summary>
/// REST API for collaborative session management
/// Validates: Requirements 32.1, 32.8
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class CollaborationController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<CollaborationController> _logger;

    public CollaborationController(ApplicationDbContext context, ILogger<CollaborationController> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Get all active sessions for a user
    /// </summary>
    [HttpGet("sessions")]
    public async Task<ActionResult<List<SessionDto>>> GetUserSessions([FromQuery] string userId)
    {
        var userGuid = Guid.Parse(userId);

        var sessions = await _context.CollaborativeSessionParticipants
            .Where(p => p.UserId == userGuid && p.IsActive)
            .Include(p => p.Session)
                .ThenInclude(s => s.Participants)
                    .ThenInclude(p => p.User)
            .Select(p => p.Session)
            .ToListAsync();

        var sessionDtos = sessions.Select(s => new SessionDto
        {
            Id = s.Id.ToString(),
            Name = s.Name,
            Language = s.Language,
            Status = s.Status.ToString(),
            CreatedAt = s.CreatedAt,
            ParticipantCount = s.Participants.Count(p => p.IsActive),
            Participants = s.Participants
                .Where(p => p.IsActive)
                .Select(p => new ParticipantDto
                {
                    UserId = p.UserId.ToString(),
                    UserName = p.User.Name,
                    Role = p.Role.ToString(),
                    JoinedAt = p.JoinedAt
                })
                .ToList()
        }).ToList();

        return Ok(sessionDtos);
    }

    /// <summary>
    /// Get session details
    /// </summary>
    [HttpGet("sessions/{sessionId}")]
    public async Task<ActionResult<SessionDto>> GetSession(string sessionId)
    {
        var sessionGuid = Guid.Parse(sessionId);

        var session = await _context.CollaborativeSessions
            .Include(s => s.Participants)
                .ThenInclude(p => p.User)
            .Include(s => s.Challenge)
            .FirstOrDefaultAsync(s => s.Id == sessionGuid && !s.IsDeleted);

        if (session == null)
        {
            return NotFound("Session not found");
        }

        var sessionDto = new SessionDto
        {
            Id = session.Id.ToString(),
            Name = session.Name,
            Language = session.Language,
            Status = session.Status.ToString(),
            Code = session.Code,
            ChallengeId = session.ChallengeId?.ToString(),
            ChallengeName = session.Challenge?.Title,
            CreatedAt = session.CreatedAt,
            EndedAt = session.EndedAt,
            ParticipantCount = session.Participants.Count(p => p.IsActive),
            Participants = session.Participants
                .Select(p => new ParticipantDto
                {
                    UserId = p.UserId.ToString(),
                    UserName = p.User.Name,
                    Role = p.Role.ToString(),
                    JoinedAt = p.JoinedAt,
                    LeftAt = p.LeftAt,
                    IsActive = p.IsActive,
                    XPEarned = p.XPEarned
                })
                .ToList()
        };

        return Ok(sessionDto);
    }

    /// <summary>
    /// Get session history for a user
    /// </summary>
    [HttpGet("history")]
    public async Task<ActionResult<List<SessionDto>>> GetSessionHistory([FromQuery] string userId)
    {
        var userGuid = Guid.Parse(userId);

        var sessions = await _context.CollaborativeSessionParticipants
            .Where(p => p.UserId == userGuid)
            .Include(p => p.Session)
                .ThenInclude(s => s.Participants)
                    .ThenInclude(p => p.User)
            .OrderByDescending(p => p.JoinedAt)
            .Take(50)
            .Select(p => p.Session)
            .ToListAsync();

        var sessionDtos = sessions.Select(s => new SessionDto
        {
            Id = s.Id.ToString(),
            Name = s.Name,
            Language = s.Language,
            Status = s.Status.ToString(),
            CreatedAt = s.CreatedAt,
            EndedAt = s.EndedAt,
            ParticipantCount = s.Participants.Count,
            Participants = s.Participants
                .Select(p => new ParticipantDto
                {
                    UserId = p.UserId.ToString(),
                    UserName = p.User.Name,
                    Role = p.Role.ToString(),
                    JoinedAt = p.JoinedAt,
                    LeftAt = p.LeftAt,
                    IsActive = p.IsActive,
                    XPEarned = p.XPEarned
                })
                .ToList()
        }).ToList();

        return Ok(sessionDtos);
    }

    /// <summary>
    /// Invite a user to a session
    /// </summary>
    [HttpPost("sessions/{sessionId}/invite")]
    public async Task<ActionResult> InviteUser(string sessionId, [FromBody] InviteRequest request)
    {
        var sessionGuid = Guid.Parse(sessionId);
        var invitedUserGuid = Guid.Parse(request.UserId);

        var session = await _context.CollaborativeSessions
            .Include(s => s.Participants)
            .FirstOrDefaultAsync(s => s.Id == sessionGuid && !s.IsDeleted);

        if (session == null)
        {
            return NotFound("Session not found");
        }

        if (session.Status != SessionStatus.Active)
        {
            return BadRequest("Session is not active");
        }

        // Check participant limit
        var activeParticipants = session.Participants.Count(p => p.IsActive);
        if (activeParticipants >= 2)
        {
            return BadRequest("Session is full (maximum 2 participants)");
        }

        // Check if user is already invited
        var existingParticipant = session.Participants.FirstOrDefault(p => p.UserId == invitedUserGuid);
        if (existingParticipant != null)
        {
            return BadRequest("User is already a participant");
        }

        // TODO: Send notification to invited user
        // This would integrate with the Notification Service

        _logger.LogInformation("User {InvitedUserId} invited to session {SessionId}", request.UserId, sessionId);

        return Ok(new { message = "Invitation sent successfully" });
    }
}

public class SessionDto
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Language { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string? Code { get; set; }
    public string? ChallengeId { get; set; }
    public string? ChallengeName { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? EndedAt { get; set; }
    public int ParticipantCount { get; set; }
    public List<ParticipantDto> Participants { get; set; } = new();
}

public class ParticipantDto
{
    public string UserId { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public DateTime JoinedAt { get; set; }
    public DateTime? LeftAt { get; set; }
    public bool IsActive { get; set; }
    public int XPEarned { get; set; }
}

public class InviteRequest
{
    public string UserId { get; set; } = string.Empty;
}
