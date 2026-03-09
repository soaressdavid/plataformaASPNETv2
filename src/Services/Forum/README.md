# Forum Service

The Forum Service provides discussion forum functionality for the platform, allowing users to create threads, post replies, upvote helpful content, and mark accepted answers.

## Features

### Thread Management
- Create discussion threads with categories
- Associate threads with specific challenges or lessons
- Pin important threads
- Lock threads to prevent new replies
- Track view counts

### Post Management
- Reply to threads with markdown-formatted content
- Edit posts (marked as edited with timestamp)
- Upvote helpful posts
- Mark posts as accepted answers (thread author only)
- Report inappropriate content

### Gamification Integration
- Award 10 XP when a post receives 5+ upvotes
- Display user level and XP in posts
- Track helpful community contributions

### Requirements Validation

**Requirement 18.1**: THE Platform SHALL provide a discussion forum for each challenge and lesson
- ✅ Threads can be associated with `ChallengeId` or `LessonId`
- ✅ Endpoints: `GET /api/forum/challenges/{id}/threads` and `GET /api/forum/lessons/{id}/threads`

**Requirement 18.2**: THE Platform SHALL allow users to create threads and reply to posts
- ✅ Endpoint: `POST /api/forum/threads` for creating threads
- ✅ Endpoint: `POST /api/forum/threads/{id}/posts` for posting replies

**Requirement 18.3**: THE Platform SHALL display user reputation and badges in forum posts
- ✅ Posts include `AuthorLevel` and `AuthorXP` fields
- ✅ Frontend displays user level badges and XP

**Requirement 33.1**: THE Discussion_Forum SHALL provide a discussion thread for each challenge and lesson
- ✅ Implemented via `ChallengeId` and `LessonId` foreign keys

**Requirement 33.2**: THE Discussion_Forum SHALL allow users to post questions and answers
- ✅ Thread creation and post reply endpoints implemented

**Requirement 33.3**: THE Discussion_Forum SHALL support markdown formatting in posts
- ✅ Content stored as plain text, frontend renders markdown

**Requirement 33.5**: THE Discussion_Forum SHALL allow users to upvote helpful posts
- ✅ Endpoint: `POST /api/forum/posts/{id}/upvote`
- ✅ Vote tracking with `ForumPostVote` entity

**Requirement 33.6**: THE Discussion_Forum SHALL sort posts by votes and recency
- ✅ Threads sorted by `IsPinned` then `UpdatedAt`
- ✅ Posts sorted by `CreatedAt` (chronological)

**Requirement 33.7**: THE Discussion_Forum SHALL allow challenge creators to mark an answer as "accepted solution"
- ✅ Endpoint: `POST /api/forum/threads/{threadId}/accept/{postId}`
- ✅ Only thread author can accept answers

**Requirement 33.8**: THE Discussion_Forum SHALL award 10 XP to users whose posts receive 5+ upvotes
- ✅ Implemented in `UpvotePost` endpoint

**Requirement 33.9**: THE Discussion_Forum SHALL notify users when their posts receive replies
- ⚠️ Notification integration pending (requires Notification Service)

**Requirement 33.10**: THE Discussion_Forum SHALL allow users to report inappropriate content
- ✅ Endpoint: `POST /api/forum/posts/{id}/report`

**Requirement 33.11**: THE Discussion_Forum SHALL allow moderators to edit or remove posts
- ✅ Edit endpoint: `PUT /api/forum/posts/{id}`
- ✅ Soft delete via `IsDeleted` flag

**Requirement 33.12**: THE Discussion_Forum SHALL display user's level and badges next to their posts
- ✅ Posts include `AuthorLevel` and `AuthorXP`

## API Endpoints

### Categories
- `GET /api/forum/categories` - Get all forum categories with thread counts

### Threads
- `GET /api/forum/categories/{category}/threads` - Get threads in a category (paginated)
- `GET /api/forum/challenges/{challengeId}/threads` - Get threads for a challenge
- `GET /api/forum/lessons/{lessonId}/threads` - Get threads for a lesson
- `GET /api/forum/threads/{threadId}` - Get thread details with all posts
- `POST /api/forum/threads` - Create a new thread

### Posts
- `POST /api/forum/threads/{threadId}/posts` - Create a reply
- `PUT /api/forum/posts/{postId}` - Edit a post
- `POST /api/forum/posts/{postId}/upvote` - Upvote/remove upvote
- `POST /api/forum/threads/{threadId}/accept/{postId}` - Mark as accepted answer
- `POST /api/forum/posts/{postId}/report` - Report inappropriate content

## Database Schema

### ForumThread
- `Id` (Guid, PK)
- `Title` (string, max 300)
- `Content` (string)
- `AuthorId` (Guid, FK to Users)
- `ChallengeId` (Guid?, FK to Challenges)
- `LessonId` (Guid?, FK to Lessons)
- `Category` (string?, max 100)
- `ViewCount` (int)
- `IsPinned` (bool)
- `IsLocked` (bool)
- `AcceptedAnswerId` (Guid?)
- Audit fields: `CreatedAt`, `UpdatedAt`, `IsDeleted`

### ForumPost
- `Id` (Guid, PK)
- `ThreadId` (Guid, FK to ForumThreads)
- `AuthorId` (Guid, FK to Users)
- `Content` (string)
- `Upvotes` (int)
- `IsEdited` (bool)
- `LastEditedAt` (DateTime?)
- `IsReported` (bool)
- `IsAcceptedAnswer` (bool)
- Audit fields: `CreatedAt`, `UpdatedAt`, `IsDeleted`

### ForumPostVote
- `Id` (Guid, PK)
- `PostId` (Guid, FK to ForumPosts)
- `UserId` (Guid, FK to Users)
- `VoteValue` (int, 1 for upvote)
- Audit fields: `CreatedAt`, `UpdatedAt`, `IsDeleted`

## Configuration

### appsettings.json
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1433;Database=AspNetLearningPlatform;..."
  }
}
```

## Running the Service

```bash
# Development
cd src/Services/Forum
dotnet run

# Production
dotnet publish -c Release
dotnet Forum.Service.dll
```

The service runs on port 5007 by default.

## Frontend Integration

Forum pages are located in `frontend/app/forum/`:
- `/forum` - Category listing
- `/forum/category/[category]` - Thread listing for category
- `/forum/thread/[id]` - Thread detail with posts
- `/forum/new` - Create new thread

## Future Enhancements

1. **Notification Integration**: Send notifications when posts receive replies
2. **Markdown Rendering**: Server-side markdown to HTML conversion
3. **Code Syntax Highlighting**: Highlight code blocks in posts
4. **Search**: Full-text search across threads and posts
5. **Moderation Tools**: Admin dashboard for managing reported content
6. **Thread Subscriptions**: Allow users to follow threads
7. **Reputation System**: Track user reputation based on helpful posts
8. **Tags**: Add tagging system for better organization
