# Chat Service

Real-time chat service using SignalR for WebSocket communication.

## Features

- **Real-time messaging**: Send and receive messages instantly using SignalR
- **Online presence**: Track which users are currently online
- **Chat rooms**: Support for global, course-specific, and direct message rooms
- **Message history**: Store and retrieve last 30 days of messages
- **Emoji reactions**: React to messages with emojis
- **Code sharing**: Share code snippets with syntax highlighting
- **Moderation**: Basic profanity filtering and message reporting
- **User blocking**: Block users from seeing their messages

## Requirements Validated

- **34.1**: Real-time chat functionality using SignalR
- **34.2**: Global chat rooms for each course
- **34.3**: Direct messaging between users
- **34.4**: Display online status for users
- **34.5**: Emoji reactions in chat messages
- **34.6**: Code snippet sharing with syntax highlighting
- **34.7**: User muting and blocking
- **34.8**: Chat history storage for 30 days
- **34.9**: Automated profanity filtering
- **34.10**: Message reporting to moderators

## API Endpoints

### REST API

- `GET /api/chat/rooms` - Get all chat rooms for the current user
- `GET /api/chat/rooms/{roomId}/messages` - Get messages for a room
- `POST /api/chat/rooms` - Create a new chat room
- `POST /api/chat/rooms/{roomId}/read` - Mark messages as read

### SignalR Hub Methods

- `SendMessage(roomId, content, codeLanguage?)` - Send a message to a room
- `JoinRoom(roomId)` - Join a chat room
- `LeaveRoom(roomId)` - Leave a chat room
- `AddReaction(messageId, emoji)` - Add emoji reaction to a message
- `GetOnlineUsers(roomId)` - Get list of online users in a room
- `ReportMessage(messageId, reason)` - Report a message
- `BlockUser(roomId, blockedUserId)` - Block a user

### SignalR Client Events

- `ReceiveMessage` - Receive a new message
- `UserOnline` - User came online
- `UserOffline` - User went offline
- `UserJoined` - User joined the room
- `UserLeft` - User left the room
- `ReactionAdded` - Reaction added to a message

## Running the Service

```bash
cd src/Services/Chat
dotnet run
```

The service will be available at `http://localhost:5010` (or configured port).

SignalR hub endpoint: `/hubs/chat`

## Database Tables

- **ChatRooms**: Chat room definitions
- **ChatMessages**: Individual messages
- **ChatRoomMembers**: User memberships in rooms

## Configuration

Configure in `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=PlatformDB;..."
  },
  "Frontend": {
    "Url": "http://localhost:3000"
  }
}
```
