# Code Execution Integration

This document describes the implementation of real-time code execution integration for the ASP.NET Core Learning Platform frontend.

## Overview

The code execution integration provides real-time feedback for code execution using WebSocket connections with automatic fallback to polling. It handles all execution statuses and displays output, errors, and execution time in the terminal panel.

## Architecture

### Components

1. **useCodeExecution Hook** (`lib/hooks/useCodeExecution.ts`)
   - Custom React hook for managing code execution state
   - Supports WebSocket for real-time updates
   - Falls back to polling if WebSocket is unavailable
   - Handles all execution statuses
   - Provides toast notifications for execution events

2. **CodeEditor Component** (`lib/components/CodeEditor/CodeEditor.tsx`)
   - Monaco Editor-based code editor
   - Multi-file support
   - Terminal output panel
   - Run button for code execution

3. **IDE Page** (`app/ide/page.tsx`)
   - Main page for browser-based IDE
   - Integrates CodeEditor with useCodeExecution hook
   - Formats output with status indicators

## Features

### Real-Time Execution Updates

The system uses WebSocket connections to receive real-time updates about code execution:

```typescript
const { execute, isRunning, output, error, status, executionTime } = useCodeExecution({
  useWebSocket: true,
  onStatusUpdate: (status) => {
    console.log('Status updated:', status);
  },
});
```

### Execution Status Handling

The integration handles all execution statuses defined in the requirements:

- **Queued**: Code is waiting in the job queue
- **Running**: Code is currently executing
- **Completed**: Execution finished successfully
- **Failed**: Execution failed with an error
- **Timeout**: Execution exceeded 30-second time limit
- **MemoryExceeded**: Execution exceeded 512MB memory limit

### Status Indicators

The terminal output includes visual indicators for each status:

- ⏳ Queued - Waiting for execution...
- ▶️  Running - Executing your code...
- ✅ Completed
- ❌ Failed
- ⏱️  Timeout - Execution exceeded 30 second time limit
- 💾 Memory Exceeded - Execution exceeded 512MB memory limit

### Automatic Fallback

If WebSocket connection fails or is unavailable, the system automatically falls back to polling:

```typescript
// WebSocket connection attempt
try {
  connectWebSocket(jobId);
} catch (err) {
  console.warn('WebSocket connection failed, falling back to polling:', err);
  startPolling(jobId);
}
```

### Toast Notifications

The integration provides user-friendly toast notifications:

- Success: "Execution completed in {time}ms"
- Timeout: "Execution timeout: Code exceeded 30 second time limit"
- Memory: "Memory exceeded: Code exceeded 512MB memory limit"
- Failed: "Execution failed"

## Usage

### Basic Usage

```typescript
import { useCodeExecution } from '@/lib/hooks/useCodeExecution';

function MyComponent() {
  const { execute, isRunning, output, error, status } = useCodeExecution();

  const handleRun = async () => {
    await execute({
      code: 'Console.WriteLine("Hello, World!");',
      files: [
        { name: 'Program.cs', content: 'Console.WriteLine("Hello, World!");' }
      ],
      entryPoint: 'Program.cs',
    });
  };

  return (
    <div>
      <button onClick={handleRun} disabled={isRunning}>
        {isRunning ? 'Running...' : 'Run'}
      </button>
      <pre>{output}</pre>
      {error && <div className="error">{error}</div>}
    </div>
  );
}
```

### With Status Updates

```typescript
const { execute, status } = useCodeExecution({
  onStatusUpdate: (status) => {
    console.log('Current status:', status.status);
    console.log('Execution time:', status.executionTimeMs);
  },
});
```

### Disable WebSocket (Use Polling Only)

```typescript
const { execute } = useCodeExecution({
  useWebSocket: false, // Force polling
});
```

## Configuration

### Environment Variables

- `NEXT_PUBLIC_API_URL`: Base URL for the API (default: `http://localhost:5000`)
- `NEXT_PUBLIC_WS_URL`: WebSocket URL (default: `ws://localhost:5000`)

### WebSocket Endpoint

The WebSocket connection is established at:
```
ws://{host}/ws/code/status/{jobId}
```

### Polling Configuration

- **Interval**: 1000ms (1 second)
- **Max Attempts**: 60 (60 seconds total)
- **Timeout**: Execution polling timeout after 60 attempts

## Error Handling

### Network Errors

Network errors are caught and displayed with user-friendly messages:

```typescript
if (!error.response) {
  toast.error('Network error. Please check your connection.');
}
```

### Validation Errors (422)

Code validation errors (prohibited code) are handled specifically:

```typescript
if (err.response?.status === 422) {
  const errorMessage = err.response?.data?.error?.message || 'Code validation failed';
  setError(errorMessage);
  toast.error(errorMessage);
}
```

### WebSocket Errors

WebSocket errors automatically trigger fallback to polling:

```typescript
ws.onerror = (event) => {
  console.error('WebSocket error:', event);
  startPolling(jobId);
};
```

## Testing

The integration includes comprehensive tests:

- Initial state verification
- WebSocket message handling
- Polling fallback
- Status transitions
- Error handling
- Timeout and memory exceeded scenarios
- Reset functionality
- Callback invocation

Run tests:
```bash
npm test -- useCodeExecution.test.ts
```

## Requirements Validation

This implementation validates the following requirements:

- **Requirement 3.1**: Code execution requests are enqueued via POST to `/api/code/execute`
- **Requirement 3.6**: Execution results are returned within 5 seconds (via WebSocket or polling)
- **Requirement 3.7**: Timeout errors are displayed when execution exceeds time limits
- **Requirement 3.8**: Memory errors are displayed when execution exceeds memory limits

## Future Enhancements

Potential improvements for future iterations:

1. **Streaming Output**: Stream output as it's generated instead of waiting for completion
2. **Execution History**: Store and display previous execution results
3. **Collaborative Editing**: Multiple users editing and executing code together
4. **Execution Metrics**: Display CPU usage, memory usage, and other metrics
5. **Breakpoint Debugging**: Add debugging capabilities with breakpoints
6. **Code Snippets**: Save and load code snippets for quick testing

## Troubleshooting

### WebSocket Connection Issues

If WebSocket connections fail:

1. Check that the backend WebSocket endpoint is running
2. Verify `NEXT_PUBLIC_WS_URL` environment variable
3. Check browser console for WebSocket errors
4. The system will automatically fall back to polling

### Polling Not Working

If polling fails:

1. Check that the API endpoint `/api/code/status/{jobId}` is accessible
2. Verify `NEXT_PUBLIC_API_URL` environment variable
3. Check network tab for failed requests
4. Verify authentication token is valid

### No Output Displayed

If execution completes but no output is shown:

1. Check that the backend is returning output in the response
2. Verify the `output` field in `ExecutionStatusResponse`
3. Check browser console for parsing errors
4. Ensure the terminal panel is expanded

## Related Files

- `frontend/lib/hooks/useCodeExecution.ts` - Main hook implementation
- `frontend/lib/hooks/__tests__/useCodeExecution.test.ts` - Hook tests
- `frontend/lib/api/code-execution.ts` - API client for code execution
- `frontend/lib/components/CodeEditor/CodeEditor.tsx` - Code editor component
- `frontend/app/ide/page.tsx` - IDE page using the integration
- `frontend/lib/types.ts` - TypeScript type definitions
