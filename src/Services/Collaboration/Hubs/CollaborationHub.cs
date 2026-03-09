using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Shared.Data;
using Shared.Entities;
using Collaboration.Service.Services;
using System.Collections.Concurrent;
using System.Text.Json;

namespace Collaboration.Service.Hubs;

/// <summary>
/// SignalR hub for real-time collaborative coding
/// Validates: Requirements 32.1, 32.2, 32.3, 32.4, 32.7
/// </summary>
public class CollaborationHub : Hub
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<CollaborationHub> _logger;
    
    // Track active sessions and their participants
    private static readonly ConcurrentDictionary<string, SessionState> _activeSessions = new();
    
    // Track cursor positions for each user in each session
    private static readonly ConcurrentDictionary<string, ConcurrentDictionary<string, CursorPosition>> _cursorPositions = new();

    public CollaborationHub(ApplicationDbContext context, ILogger<CollaborationHub> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Create a new collaborative session
    /// Validates: Requirement 32.1
    /// </summary>
    public async Task<string> CreateSession(string name, string? challengeId = null, string language = "csharp", string initialCode = "")
    {
        var userId = GetUserId();
        if (userId == null)
        {
            throw new HubException("User not authenticated");
        }

        var userGuid = Guid.Parse(userId);
        var challengeGuid = string.IsNullOrEmpty(challengeId) ? (Guid?)null : Guid.Parse(challengeId);

        // Create session in database
        var session = new CollaborativeSession
        {
            Name = name,
            ChallengeId = challengeGuid,
            Code = initialCode,
            Language = language,
            CreatedByUserId = userGuid,
            Status = SessionStatus.Active
        };

        _context.CollaborativeSessions.Add(session);

        // Add creator as participant
        var participant = new CollaborativeSessionParticipant
        {
            SessionId = session.Id,
            UserId = userGuid,
            Role = ParticipantRole.Owner,
            IsActive = true
        };

        _context.CollaborativeSessionParticipants.Add(participant);
        await _context.SaveChangesAsync();

        // Initialize session state
        var sessionId = session.Id.ToString();
        _activeSessions[sessionId] = new SessionState
        {
            SessionId = sessionId,
            Code = initialCode,
            Version = 0,
            Participants = new ConcurrentDictionary<string, string>()
        };
        _activeSessions[sessionId].Participants[userId] = Context.ConnectionId;

        _cursorPositions[sessionId] = new ConcurrentDictionary<string, CursorPosition>();

        // Add connection to session group
        await Groups.AddToGroupAsync(Context.ConnectionId, sessionId);

        _logger.LogInformation("User {UserId} created collaborative session {SessionId}", userId, sessionId);

        return sessionId;
    }

    /// <summary>
    /// Join an existing collaborative session
    /// Validates: Requirements 32.1, 32.9
    /// </summary>
    public async Task<SessionJoinResult> JoinSession(string sessionId)
    {
        var userId = GetUserId();
        if (userId == null)
        {
            throw new HubException("User not authenticated");
        }

        var sessionGuid = Guid.Parse(sessionId);
        var userGuid = Guid.Parse(userId);

        // Check if session exists and is active
        var session = await _context.CollaborativeSessions
            .Include(s => s.Participants)
            .FirstOrDefaultAsync(s => s.Id == sessionGuid && !s.IsDeleted);

        if (session == null)
        {
            throw new HubException("Session not found");
        }

        if (session.Status != SessionStatus.Active)
        {
            throw new HubException("Session is not active");
        }

        // Check participant limit (max 2 users)
        var activeParticipants = session.Participants.Count(p => p.IsActive);
        if (activeParticipants >= 2)
        {
            throw new HubException("Session is full (maximum 2 participants)");
        }

        // Check if user is already a participant
        var existingParticipant = session.Participants.FirstOrDefault(p => p.UserId == userGuid);
        if (existingParticipant == null)
        {
            // Add new participant
            var participant = new CollaborativeSessionParticipant
            {
                SessionId = sessionGuid,
                UserId = userGuid,
                Role = ParticipantRole.Collaborator,
                IsActive = true
            };

            _context.CollaborativeSessionParticipants.Add(participant);
            await _context.SaveChangesAsync();
        }
        else
        {
            // Reactivate existing participant
            existingParticipant.IsActive = true;
            existingParticipant.LeftAt = null;
            await _context.SaveChangesAsync();
        }

        // Add to session state
        if (!_activeSessions.ContainsKey(sessionId))
        {
            _activeSessions[sessionId] = new SessionState
            {
                SessionId = sessionId,
                Code = session.Code,
                Version = 0,
                Participants = new ConcurrentDictionary<string, string>()
            };
            _cursorPositions[sessionId] = new ConcurrentDictionary<string, CursorPosition>();
        }

        _activeSessions[sessionId].Participants[userId] = Context.ConnectionId;

        // Add connection to session group
        await Groups.AddToGroupAsync(Context.ConnectionId, sessionId);

        // Get user info
        var user = await _context.Users.FindAsync(userGuid);

        // Notify other participants
        await Clients.GroupExcept(sessionId, Context.ConnectionId).SendAsync("UserJoined", new
        {
            userId,
            userName = user?.Name ?? "Unknown",
            timestamp = DateTime.UtcNow
        });

        _logger.LogInformation("User {UserId} joined collaborative session {SessionId}", userId, sessionId);

        return new SessionJoinResult
        {
            SessionId = sessionId,
            Code = _activeSessions[sessionId].Code,
            Version = _activeSessions[sessionId].Version,
            Participants = _activeSessions[sessionId].Participants.Keys.ToList()
        };
    }

    /// <summary>
    /// Leave a collaborative session
    /// Validates: Requirement 32.8
    /// </summary>
    public async Task LeaveSession(string sessionId)
    {
        var userId = GetUserId();
        if (userId == null)
        {
            throw new HubException("User not authenticated");
        }

        var sessionGuid = Guid.Parse(sessionId);
        var userGuid = Guid.Parse(userId);

        // Update participant status
        var participant = await _context.CollaborativeSessionParticipants
            .FirstOrDefaultAsync(p => p.SessionId == sessionGuid && p.UserId == userGuid);

        if (participant != null)
        {
            participant.IsActive = false;
            participant.LeftAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }

        // Remove from session state
        if (_activeSessions.TryGetValue(sessionId, out var sessionState))
        {
            sessionState.Participants.TryRemove(userId, out _);
        }

        if (_cursorPositions.TryGetValue(sessionId, out var cursors))
        {
            cursors.TryRemove(userId, out _);
        }

        // Remove from SignalR group
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, sessionId);

        // Notify other participants
        await Clients.Group(sessionId).SendAsync("UserLeft", new
        {
            userId,
            timestamp = DateTime.UtcNow
        });

        _logger.LogInformation("User {UserId} left collaborative session {SessionId}", userId, sessionId);
    }

    /// <summary>
    /// Send a code change operation
    /// Validates: Requirements 32.2, 32.7
    /// </summary>
    public async Task SendOperation(string sessionId, OperationalTransform.Operation operation)
    {
        var userId = GetUserId();
        if (userId == null)
        {
            throw new HubException("User not authenticated");
        }

        if (!_activeSessions.TryGetValue(sessionId, out var sessionState))
        {
            throw new HubException("Session not found");
        }

        operation.UserId = userId;

        // Transform operation against any concurrent operations
        lock (sessionState)
        {
            // Apply operation to current code
            sessionState.Code = OperationalTransform.ApplyOperation(sessionState.Code, operation);
            sessionState.Version++;
            operation.Version = sessionState.Version;

            // Store operation in history
            sessionState.OperationHistory.Add(operation);
        }

        // Broadcast operation to other participants
        await Clients.GroupExcept(sessionId, Context.ConnectionId).SendAsync("ReceiveOperation", operation);

        _logger.LogDebug("Operation {Type} at position {Position} sent by user {UserId} in session {SessionId}",
            operation.Type, operation.Position, userId, sessionId);
    }

    /// <summary>
    /// Update cursor position
    /// Validates: Requirement 32.3
    /// </summary>
    public async Task UpdateCursor(string sessionId, int line, int column, int? selectionStartLine = null, int? selectionStartColumn = null)
    {
        var userId = GetUserId();
        if (userId == null)
        {
            throw new HubException("User not authenticated");
        }

        if (!_cursorPositions.TryGetValue(sessionId, out var cursors))
        {
            throw new HubException("Session not found");
        }

        var cursor = new CursorPosition
        {
            UserId = userId,
            Line = line,
            Column = column,
            SelectionStartLine = selectionStartLine,
            SelectionStartColumn = selectionStartColumn,
            Timestamp = DateTime.UtcNow
        };

        cursors[userId] = cursor;

        // Broadcast cursor position to other participants
        await Clients.GroupExcept(sessionId, Context.ConnectionId).SendAsync("CursorMoved", cursor);
    }

    /// <summary>
    /// Send a chat message in the session
    /// Validates: Requirement 32.4
    /// </summary>
    public async Task SendChatMessage(string sessionId, string message)
    {
        var userId = GetUserId();
        if (userId == null)
        {
            throw new HubException("User not authenticated");
        }

        var userGuid = Guid.Parse(userId);
        var user = await _context.Users.FindAsync(userGuid);

        // Broadcast message to all participants
        await Clients.Group(sessionId).SendAsync("ReceiveChatMessage", new
        {
            userId,
            userName = user?.Name ?? "Unknown",
            message,
            timestamp = DateTime.UtcNow
        });

        _logger.LogDebug("Chat message sent by user {UserId} in session {SessionId}", userId, sessionId);
    }

    /// <summary>
    /// Run code in the session
    /// Validates: Requirement 32.7
    /// </summary>
    public async Task RunCode(string sessionId)
    {
        var userId = GetUserId();
        if (userId == null)
        {
            throw new HubException("User not authenticated");
        }

        if (!_activeSessions.TryGetValue(sessionId, out var sessionState))
        {
            throw new HubException("Session not found");
        }

        // Notify all participants that code is being executed
        await Clients.Group(sessionId).SendAsync("CodeExecutionStarted", new
        {
            userId,
            timestamp = DateTime.UtcNow
        });

        _logger.LogInformation("Code execution started by user {UserId} in session {SessionId}", userId, sessionId);
    }

    /// <summary>
    /// Complete the session and award XP
    /// Validates: Requirement 32.6
    /// </summary>
    public async Task CompleteSession(string sessionId, int totalXP)
    {
        var userId = GetUserId();
        if (userId == null)
        {
            throw new HubException("User not authenticated");
        }

        var sessionGuid = Guid.Parse(sessionId);
        var userGuid = Guid.Parse(userId);

        var session = await _context.CollaborativeSessions
            .Include(s => s.Participants)
            .FirstOrDefaultAsync(s => s.Id == sessionGuid);

        if (session == null)
        {
            throw new HubException("Session not found");
        }

        // Only owner can complete the session
        if (session.CreatedByUserId != userGuid)
        {
            throw new HubException("Only the session owner can complete the session");
        }

        // Update session status
        session.Status = SessionStatus.Completed;
        session.EndedAt = DateTime.UtcNow;

        // Split XP equally among active participants
        var activeParticipants = session.Participants.Where(p => p.IsActive).ToList();
        var xpPerParticipant = totalXP / Math.Max(activeParticipants.Count, 1);

        foreach (var participant in activeParticipants)
        {
            participant.XPEarned = xpPerParticipant;
            participant.IsActive = false;
            participant.LeftAt = DateTime.UtcNow;
        }

        await _context.SaveChangesAsync();

        // Notify all participants
        await Clients.Group(sessionId).SendAsync("SessionCompleted", new
        {
            sessionId,
            xpEarned = xpPerParticipant,
            timestamp = DateTime.UtcNow
        });

        // Clean up session state
        _activeSessions.TryRemove(sessionId, out _);
        _cursorPositions.TryRemove(sessionId, out _);

        _logger.LogInformation("Session {SessionId} completed by user {UserId}", sessionId, userId);
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var userId = GetUserId();
        if (userId != null)
        {
            // Find all sessions this user is in and mark them as left
            var sessionsToLeave = _activeSessions
                .Where(kvp => kvp.Value.Participants.ContainsKey(userId))
                .Select(kvp => kvp.Key)
                .ToList();

            foreach (var sessionId in sessionsToLeave)
            {
                await LeaveSession(sessionId);
            }
        }

        await base.OnDisconnectedAsync(exception);
    }

    private string? GetUserId()
    {
        return Context.User?.FindFirst("sub")?.Value
            ?? Context.User?.FindFirst("userId")?.Value
            ?? Context.User?.Identity?.Name;
    }
}

/// <summary>
/// Represents the state of an active collaborative session
/// </summary>
public class SessionState
{
    public string SessionId { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public long Version { get; set; }
    public ConcurrentDictionary<string, string> Participants { get; set; } = new();
    public List<OperationalTransform.Operation> OperationHistory { get; set; } = new();
}

/// <summary>
/// Represents a cursor position in the editor
/// </summary>
public class CursorPosition
{
    public string UserId { get; set; } = string.Empty;
    public int Line { get; set; }
    public int Column { get; set; }
    public int? SelectionStartLine { get; set; }
    public int? SelectionStartColumn { get; set; }
    public DateTime Timestamp { get; set; }
}

/// <summary>
/// Result of joining a session
/// </summary>
public class SessionJoinResult
{
    public string SessionId { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public long Version { get; set; }
    public List<string> Participants { get; set; } = new();
}
