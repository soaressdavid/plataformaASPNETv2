# Collaboration Service

## Overview

The Collaboration Service enables real-time collaborative coding sessions where multiple users can edit code together, similar to Google Docs but for code. It implements operational transformation for conflict-free concurrent editing and provides real-time cursor tracking.

**Validates Requirements**: 32.1, 32.2, 32.3, 32.4, 32.6, 32.7, 32.8, 32.9

## Features

### 1. Real-Time Code Synchronization (Requirement 32.2)
- **Operational Transformation (OT)**: Implements OT algorithm to handle concurrent edits from multiple users
- **Conflict Resolution**: Automatically resolves conflicts when users edit the same code simultaneously
- **Version Control**: Tracks operation versions to ensure consistency across all participants

### 2. Cursor Tracking (Requirement 32.3)
- **Real-Time Cursor Positions**: Displays cursor positions of all collaborators in real-time
- **Selection Highlighting**: Shows text selections made by other users
- **Color-Coded Cursors**: Each user gets a unique color for easy identification
- **User Labels**: Displays user identifiers next to their cursors

### 3. Session Management (Requirements 32.1, 32.8, 32.9)
- **Create Sessions**: Users can create new collaborative coding sessions
- **Join Sessions**: Users can join existing sessions via invitation
- **Participant Limit**: Maximum 2 participants per session (Requirement 32.9)
- **Session History**: Tracks all collaborative sessions for users
- **Session Status**: Active, Completed, or Abandoned states

### 4. Integrated Chat (Requirement 32.4)
- **Text Chat**: Real-time text messaging between collaborators
- **System Messages**: Automatic notifications when users join/leave
- **Message History**: Stores chat messages for the session duration

### 5. Code Execution (Requirement 32.7)
- **Shared Execution**: Either collaborator can run the code
- **Execution Notifications**: All participants see when code is being executed
- **Shared Results**: Execution results are visible to all participants

### 6. XP Rewards (Requirement 32.6)
- **Equal Split**: XP is split equally between all active participants
- **Completion Bonus**: Awarded when the session is successfully completed
- **Team Player Achievement**: Unlock after completing 5 collaborative challenges

## Architecture

### Backend Components

#### 1. CollaborationHub (SignalR)
- **Location**: `src/Services/Collaboration/Hubs/CollaborationHub.cs`
- **Purpose**: Real-time WebSocket communication for collaborative features
- **Key Methods**:
  - `CreateSession`: Create a new collaborative session
  - `JoinSession`: Join an existing session
  - `LeaveSession`: Leave a session
  - `SendOperation`: Send code change operations
  - `UpdateCursor`: Update cursor position
  - `SendChatMessage`: Send chat messages
  - `RunCode`: Trigger code execution
  - `CompleteSession`: Complete session and award XP

#### 2. OperationalTransform Service
- **Location**: `src/Services/Collaboration/Services/OperationalTransform.cs`
- **Purpose**: Implements OT algorithm for conflict-free concurrent editing
- **Key Methods**:
  - `Transform`: Transform operation A against operation B
  - `ApplyOperation`: Apply an operation to text
  - `Compose`: Compose two operations into one

#### 3. CollaborationController (REST API)
- **Location**: `src/Services/Collaboration/Controllers/CollaborationController.cs`
- **Purpose**: REST API for session management
- **Endpoints**:
  - `GET /api/collaboration/sessions`: Get user's active sessions
  - `GET /api/collaboration/sessions/{id}`: Get session details
  - `GET /api/collaboration/history`: Get session history
  - `POST /api/collaboration/sessions/{id}/invite`: Invite user to session

### Frontend Components

#### 1. useCollaboration Hook
- **Location**: `frontend/lib/hooks/useCollaboration.ts`
- **Purpose**: React hook for managing SignalR connection and collaboration features
- **Features**:
  - Connection management with automatic reconnection
  - Session creation and joining
  - Operation sending and receiving
  - Cursor position updates
  - Chat messaging
  - Event handlers for all collaboration events

#### 2. CollaborativeEditor Component
- **Location**: `frontend/lib/components/Collaboration/CollaborativeEditor.tsx`
- **Purpose**: Monaco Editor with real-time collaboration features
- **Features**:
  - Real-time code synchronization
  - Remote cursor rendering with colors
  - Selection highlighting
  - Operation transformation
  - Connection status indicator

#### 3. SessionManager Component
- **Location**: `frontend/lib/components/Collaboration/SessionManager.tsx`
- **Purpose**: Session management UI with chat
- **Features**:
  - Participant list
  - Integrated chat
  - Session controls (leave, complete)
  - System notifications

#### 4. Collaborative Session Pages
- **Location**: `frontend/app/collaborate/`
- **Pages**:
  - `/collaborate`: Session list and creation
  - `/collaborate/[sessionId]`: Active collaborative session

### Database Models

#### CollaborativeSession
```csharp
{
  Id: Guid
  ChallengeId: Guid? (optional)
  Name: string
  Code: string (synchronized code)
  Language: string
  Status: SessionStatus (Active, Completed, Abandoned)
  CreatedByUserId: Guid
  CreatedAt: DateTime
  UpdatedAt: DateTime
  EndedAt: DateTime?
  IsDeleted: bool
}
```

#### CollaborativeSessionParticipant
```csharp
{
  Id: Guid
  SessionId: Guid
  UserId: Guid
  Role: ParticipantRole (Owner, Collaborator)
  JoinedAt: DateTime
  LeftAt: DateTime?
  IsActive: bool
  XPEarned: int
}
```

## Operational Transformation Algorithm

The OT algorithm ensures that concurrent edits from multiple users result in the same final state for all participants.

### Operation Types
1. **Insert**: Insert text at a position
2. **Delete**: Delete text from a position
3. **Retain**: Keep text unchanged (used for cursor positioning)

### Transformation Rules

When two operations A and B are concurrent:

1. **Insert vs Insert**:
   - If B inserts before A's position, shift A's position right by B's length
   - If B inserts at or after A's position, A remains unchanged

2. **Insert vs Delete**:
   - If B deletes before A's position, shift A's position left by B's length
   - If B deletes at A's position, adjust A's position to deletion point

3. **Delete vs Delete**:
   - Calculate overlap between deletions
   - Adjust deletion length based on overlap

### Example

```
Initial: "Hello World"
User A: Insert "Beautiful " at position 6
User B: Delete "World" at position 6

After transformation:
User A's operation: Insert "Beautiful " at position 6
User B's operation: Delete "World" at position 16 (shifted by "Beautiful ".length)

Final: "Hello Beautiful "
```

## Usage Examples

### Creating a Session

```typescript
const { createSession } = useCollaboration(hubUrl);

const sessionId = await createSession(
  'Algorithm Practice',
  undefined, // no challenge
  'csharp',
  '// Start coding here'
);
```

### Joining a Session

```typescript
const { joinSession } = useCollaboration(hubUrl);

const result = await joinSession(sessionId);
console.log('Current code:', result.code);
console.log('Participants:', result.participants);
```

### Sending Code Changes

```typescript
const { sendOperation } = useCollaboration(hubUrl);

const operation = {
  type: 'Insert',
  position: 10,
  text: 'new code',
  length: 8,
  userId: currentUserId,
  version: currentVersion
};

await sendOperation(sessionId, operation);
```

### Updating Cursor Position

```typescript
const { updateCursor } = useCollaboration(hubUrl);

await updateCursor(sessionId, lineNumber, columnNumber);
```

## Configuration

### Backend Configuration

Add to `appsettings.json`:

```json
{
  "AllowedOrigins": [
    "http://localhost:3000",
    "https://your-frontend-domain.com"
  ],
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=PlatformDB;..."
  }
}
```

### Frontend Configuration

Add to `.env.local`:

```
NEXT_PUBLIC_COLLABORATION_HUB_URL=http://localhost:5007/hubs/collaboration
```

## Testing

### Manual Testing

1. **Create a Session**:
   - Navigate to `/collaborate`
   - Click "Create New Session"
   - Enter session details
   - Click "Create Session"

2. **Join from Another Browser**:
   - Open the same URL in a different browser/incognito window
   - Copy the session ID
   - Navigate to `/collaborate/[sessionId]`

3. **Test Real-Time Editing**:
   - Type in one browser
   - Observe changes appear in the other browser
   - Move cursor and see it reflected

4. **Test Chat**:
   - Click "Show Chat"
   - Send messages from both browsers
   - Verify messages appear in real-time

### Integration Testing

```csharp
[Fact]
public async Task TwoUsersCanEditConcurrently()
{
    // Arrange
    var hub = new CollaborationHub(context, logger);
    var sessionId = await hub.CreateSession("Test", null, "csharp", "");
    
    // Act
    var op1 = new Operation { Type = OperationType.Insert, Position = 0, Text = "Hello" };
    var op2 = new Operation { Type = OperationType.Insert, Position = 0, Text = "World" };
    
    await hub.SendOperation(sessionId, op1);
    await hub.SendOperation(sessionId, op2);
    
    // Assert
    var session = await context.CollaborativeSessions.FindAsync(sessionId);
    Assert.Contains("Hello", session.Code);
    Assert.Contains("World", session.Code);
}
```

## Performance Considerations

1. **Operation Batching**: Operations are sent immediately but can be batched for better performance
2. **Cursor Throttling**: Cursor updates are throttled to avoid overwhelming the network
3. **Message Size Limits**: SignalR message size is limited to 1MB
4. **Connection Pooling**: SignalR uses connection pooling for scalability

## Security

1. **Authentication**: All SignalR connections require JWT authentication
2. **Authorization**: Users can only join sessions they're invited to
3. **Input Validation**: All operations are validated before being applied
4. **Rate Limiting**: Operations are rate-limited to prevent abuse
5. **Session Isolation**: Each session is isolated from others

## Limitations

1. **Participant Limit**: Maximum 2 participants per session (Requirement 32.9)
2. **Session Duration**: Sessions are automatically closed after 2 hours of inactivity
3. **Code Size**: Maximum 1MB of code per session
4. **Operation History**: Only last 1000 operations are kept in memory

## Future Enhancements

1. **Voice Chat**: Add WebRTC voice chat (Requirement 32.5)
2. **Video Chat**: Add WebRTC video chat
3. **Screen Sharing**: Share screen during collaboration
4. **More Participants**: Support more than 2 participants
5. **Persistent History**: Store all operations in database for replay
6. **Conflict Visualization**: Show conflicts visually before resolution
7. **Code Review**: Add inline comments and code review features

## Troubleshooting

### Connection Issues

**Problem**: SignalR connection fails
**Solution**: 
- Check CORS configuration
- Verify JWT token is valid
- Check network connectivity
- Review browser console for errors

### Operations Not Syncing

**Problem**: Code changes don't appear for other users
**Solution**:
- Check if both users are in the same session
- Verify SignalR connection is active
- Check operation version numbers
- Review server logs for errors

### Cursor Not Showing

**Problem**: Remote cursors don't appear
**Solution**:
- Verify cursor update events are being sent
- Check CSS styles are loaded
- Ensure Monaco Editor is properly initialized
- Review browser console for errors

## Support

For issues or questions:
- Check the logs in `src/Services/Collaboration/Logs/`
- Review SignalR connection status
- Contact the development team
