# IDE Session Persistence Integration Example

This document shows how to integrate the `useIdeSession` hook into your IDE component to enable automatic session persistence.

## Basic Integration

```typescript
'use client';

import React, { useEffect } from 'react';
import { CodeEditor } from '@/lib/components/CodeEditor';
import { useIdeSession } from '@/lib/hooks';
import { useAuth } from '@/lib/contexts/AuthContext';

export function IDEPage() {
  const { user } = useAuth();
  const [files, setFiles] = React.useState([
    { name: 'Program.cs', content: '', language: 'csharp' }
  ]);
  const [activeFileIndex, setActiveFileIndex] = React.useState(0);

  // Initialize IDE session persistence
  const {
    sessionState,
    updateOpenFiles,
    updateActiveFile,
    updateCursorPosition,
    isLoading,
    isSaving,
    lastSavedAt,
  } = useIdeSession({
    userId: user?.id || 'guest',
    autoSaveInterval: 30000, // Auto-save every 30 seconds
    onSaveSuccess: () => {
      console.log('Session saved successfully');
    },
    onSaveError: (error) => {
      console.error('Failed to save session:', error);
    },
    onLoadSuccess: (state) => {
      console.log('Session loaded successfully');
    },
  });

  // Restore session on load
  useEffect(() => {
    if (!isLoading && sessionState.openFiles.length > 0) {
      // Convert IdeFile to CodeFile format
      const restoredFiles = sessionState.openFiles.map(file => ({
        name: file.path,
        content: file.content,
        language: file.language,
      }));
      setFiles(restoredFiles);

      // Restore active file
      if (sessionState.activeFile) {
        const activeIndex = restoredFiles.findIndex(
          f => f.name === sessionState.activeFile
        );
        if (activeIndex !== -1) {
          setActiveFileIndex(activeIndex);
        }
      }
    }
  }, [isLoading, sessionState]);

  // Update session when files change
  const handleFileChange = (index: number, content: string) => {
    const newFiles = [...files];
    newFiles[index].content = content;
    setFiles(newFiles);

    // Update session state
    const ideFiles = newFiles.map(f => ({
      path: f.name,
      content: f.content,
      language: f.language,
    }));
    updateOpenFiles(ideFiles);
  };

  // Update session when active file changes
  const handleActiveFileChange = (index: number) => {
    setActiveFileIndex(index);
    updateActiveFile(files[index].name);
  };

  // Handle cursor position changes (optional)
  const handleCursorPositionChange = (filePath: string, line: number, column: number) => {
    updateCursorPosition(filePath, { line, column });
  };

  if (isLoading) {
    return <div>Loading session...</div>;
  }

  return (
    <div>
      {/* Save indicator */}
      <div className="flex items-center gap-2 text-sm text-gray-600">
        {isSaving && <span>Saving...</span>}
        {lastSavedAt && !isSaving && (
          <span>Last saved: {lastSavedAt.toLocaleTimeString()}</span>
        )}
      </div>

      {/* IDE Component */}
      <CodeEditor
        files={files}
        activeFileIndex={activeFileIndex}
        onFileChange={handleFileChange}
        onActiveFileChange={handleActiveFileChange}
        onRun={() => {/* ... */}}
        output=""
        isRunning={false}
      />
    </div>
  );
}
```

## Advanced Integration with Monaco Editor

If you want to track cursor positions in Monaco Editor:

```typescript
import Editor from '@monaco-editor/react';
import type { editor } from 'monaco-editor';

function MyEditor() {
  const editorRef = useRef<editor.IStandaloneCodeEditor | null>(null);
  const { updateCursorPosition } = useIdeSession({ userId: 'user-id' });

  const handleEditorDidMount = (editor: editor.IStandaloneCodeEditor) => {
    editorRef.current = editor;

    // Track cursor position changes
    editor.onDidChangeCursorPosition((e) => {
      const position = e.position;
      updateCursorPosition(currentFileName, {
        line: position.lineNumber,
        column: position.column,
      });
    });
  };

  return (
    <Editor
      onMount={handleEditorDidMount}
      // ... other props
    />
  );
}
```

## Restoring Cursor Positions

```typescript
useEffect(() => {
  if (editorRef.current && sessionState.cursorPositions[activeFile]) {
    const position = sessionState.cursorPositions[activeFile];
    editorRef.current.setPosition({
      lineNumber: position.line,
      column: position.column,
    });
    editorRef.current.revealPositionInCenter({
      lineNumber: position.line,
      column: position.column,
    });
  }
}, [activeFile, sessionState.cursorPositions]);
```

## Manual Save

If you need to manually trigger a save (e.g., before navigation):

```typescript
const { saveNow } = useIdeSession({ userId: 'user-id' });

const handleNavigateAway = async () => {
  await saveNow();
  router.push('/other-page');
};
```

## Features

- ✅ Auto-save every 30 seconds (configurable)
- ✅ Save on unmount (when user leaves page)
- ✅ Restore session on page reload
- ✅ Track open files and their contents
- ✅ Track active file
- ✅ Track cursor positions per file
- ✅ Loading and saving states
- ✅ Error handling with callbacks
- ✅ Manual save trigger

## API Configuration

Make sure your API client is configured to point to the IDE service:

```typescript
// In your API client configuration
const IDE_SERVICE_URL = process.env.NEXT_PUBLIC_IDE_SERVICE_URL || 'http://localhost:5001';
```

## Testing

To test the session persistence:

1. Open the IDE and create/edit some files
2. Wait 30 seconds for auto-save (or trigger manual save)
3. Reload the page
4. Verify that all files, content, and cursor positions are restored
