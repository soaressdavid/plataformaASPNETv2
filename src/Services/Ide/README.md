# IDE Service

This service handles IDE session persistence, allowing users to save and restore their IDE state across sessions.

## Features

- **Session Persistence**: Save IDE state including open files, active file, file contents, and cursor positions
- **Auto-save**: Automatically saves session every 30 seconds
- **Session Restoration**: Restore IDE state when user returns
- **Per-user Sessions**: Each user has their own isolated session

## API Endpoints

### Save IDE Session
```
POST /api/ide/session
```

Request body:
```json
{
  "userId": "user-id",
  "sessionState": {
    "openFiles": [
      {
        "path": "Program.cs",
        "content": "using System;\n\nclass Program {...}",
        "language": "csharp"
      }
    ],
    "activeFile": "Program.cs",
    "cursorPositions": {
      "Program.cs": {
        "line": 5,
        "column": 10
      }
    }
  }
}
```

### Load IDE Session
```
GET /api/ide/session/{userId}
```

Response:
```json
{
  "sessionId": "session-guid",
  "userId": "user-id",
  "sessionState": {
    "openFiles": [...],
    "activeFile": "Program.cs",
    "cursorPositions": {...}
  },
  "lastSavedAt": "2024-01-15T10:30:00Z"
}
```

### Delete IDE Session
```
DELETE /api/ide/session/{userId}
```

## Frontend Integration

Use the `useIdeSession` hook to integrate session persistence in your IDE component:

```typescript
import { useIdeSession } from '@/lib/hooks';

function MyIDE() {
  const {
    sessionState,
    updateOpenFiles,
    updateActiveFile,
    updateCursorPosition,
    saveNow,
    isLoading,
    isSaving,
    lastSavedAt,
  } = useIdeSession({
    userId: 'current-user-id',
    autoSaveInterval: 30000, // 30 seconds
    onSaveSuccess: () => console.log('Session saved'),
    onSaveError: (error) => console.error('Save failed:', error),
  });

  // Use sessionState to initialize your IDE
  // Call update functions when state changes
  // Auto-save happens automatically every 30 seconds
}
```

## Database Schema

The `IdeSessions` table stores:
- `Id`: Unique session identifier (GUID)
- `UserId`: User identifier (string, indexed, unique)
- `SessionData`: JSON serialized session state
- `LastSavedAt`: Timestamp of last save
- `CreatedAt`, `UpdatedAt`, `IsDeleted`: Audit fields

## Configuration

Configure the database connection in `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=PlatformDb;..."
  }
}
```

## Running the Service

```bash
cd src/Services/Ide
dotnet run
```

The service will be available at `https://localhost:5001` (or configured port).
