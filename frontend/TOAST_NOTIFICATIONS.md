# Toast Notifications Implementation

This document describes the toast notification system implemented for the ASP.NET Core Learning Platform frontend.

## Overview

Toast notifications provide user feedback for actions throughout the application. The implementation uses `react-hot-toast` library with custom styling to match the platform's design.

## Features

- **Success notifications**: Green toasts for successful actions (e.g., login, challenge completion)
- **Error notifications**: Red toasts for errors with clear messages (e.g., failed submissions, network errors)
- **Info notifications**: Gray toasts for informational messages (e.g., logout confirmation)
- **Loading notifications**: Blue toasts for ongoing operations (e.g., code execution)
- **Automatic dismissal**: Toasts auto-dismiss after 4-5 seconds
- **Manual dismissal**: Users can click to dismiss toasts early
- **Position**: Top-right corner for non-intrusive display

## Usage

### Using the useToast Hook

The recommended way to show toast notifications is using the `useToast` hook:

```typescript
import { useToast } from '@/lib/hooks/useToast';

function MyComponent() {
  const toast = useToast();

  const handleSuccess = () => {
    toast.success('Operation completed successfully!');
  };

  const handleError = () => {
    toast.error('Something went wrong. Please try again.');
  };

  const handleInfo = () => {
    toast.info('This is an informational message.');
  };

  const handleLoading = async () => {
    const loadingToast = toast.loading('Processing...');
    
    try {
      await someAsyncOperation();
      toast.dismiss(loadingToast);
      toast.success('Done!');
    } catch (error) {
      toast.dismiss(loadingToast);
      toast.error('Failed!');
    }
  };

  return (
    <div>
      <button onClick={handleSuccess}>Show Success</button>
      <button onClick={handleError}>Show Error</button>
      <button onClick={handleInfo}>Show Info</button>
      <button onClick={handleLoading}>Show Loading</button>
    </div>
  );
}
```

### Direct Import (Alternative)

You can also import `toast` directly from `react-hot-toast`:

```typescript
import toast from 'react-hot-toast';

function MyComponent() {
  const handleClick = () => {
    toast.success('Success message');
    toast.error('Error message');
    toast('Info message', { icon: 'ℹ️' });
    toast.loading('Loading...');
  };

  return <button onClick={handleClick}>Show Toast</button>;
}
```

## Integration Points

### 1. Authentication (AuthContext)

Toast notifications are integrated into the authentication flow:

- **Login success**: "Welcome back, {name}!"
- **Login failure**: "Login failed. Please check your credentials."
- **Registration success**: "Welcome to the platform, {name}!"
- **Registration failure**: "Registration failed. Please try again."
- **Logout**: "You have been logged out."

### 2. API Client (Global Error Handling)

The API client automatically shows toast notifications for common errors:

- **401 Unauthorized**: "Session expired. Please login again."
- **429 Rate Limit**: "Rate limit exceeded. Please try again later."
- **503 Service Unavailable**: "Service temporarily unavailable. Please try again later."
- **Network Error**: "Network error. Please check your connection."

### 3. Challenge Submissions

When implementing challenge submission UI, use toasts for:

```typescript
const handleSubmit = async () => {
  const loadingToast = toast.loading('Running tests...');
  
  try {
    const result = await challengesApi.submitSolution(challengeId, { code });
    toast.dismiss(loadingToast);
    
    if (result.allTestsPassed) {
      toast.success(`All tests passed! You earned ${result.xpAwarded} XP!`);
    } else {
      toast.error(`${result.results.filter(r => !r.passed).length} test(s) failed.`);
    }
  } catch (error) {
    toast.dismiss(loadingToast);
    toast.error('Submission failed. Please try again.');
  }
};
```

### 4. Course Progress

For lesson completion and course enrollment:

```typescript
const handleCompleteLesson = async () => {
  try {
    await coursesApi.completeLesson(courseId, lessonId);
    toast.success('Lesson completed! Moving to next lesson...');
  } catch (error) {
    toast.error('Failed to mark lesson as complete.');
  }
};
```

### 5. Code Execution

For code execution feedback:

```typescript
const handleRunCode = async () => {
  const loadingToast = toast.loading('Executing code...');
  
  try {
    const result = await codeExecutionApi.execute({ code });
    toast.dismiss(loadingToast);
    
    if (result.status === 'Completed') {
      toast.success('Code executed successfully!');
    } else if (result.status === 'Timeout') {
      toast.error('Execution timeout. Code exceeded 30 second limit.');
    } else if (result.status === 'MemoryExceeded') {
      toast.error('Memory limit exceeded. Code used more than 512MB.');
    }
  } catch (error) {
    toast.dismiss(loadingToast);
    toast.error('Code execution failed.');
  }
};
```

## Styling

The toast notifications use custom styling to match the platform's design:

- **Background colors**:
  - Success: `#10b981` (green)
  - Error: `#ef4444` (red)
  - Loading: `#3b82f6` (blue)
  - Default: `#363636` (dark gray)
- **Text color**: White (`#fff`)
- **Border radius**: `8px`
- **Padding**: `12px 16px`
- **Position**: Top-right corner
- **Duration**: 4 seconds (success/info), 5 seconds (error)

## Best Practices

1. **Be specific**: Provide clear, actionable messages
   - ✅ "Challenge submitted successfully! You earned 25 XP."
   - ❌ "Success"

2. **Use appropriate types**:
   - Success: Completed actions, achievements
   - Error: Failures, validation errors
   - Info: Neutral information, confirmations
   - Loading: Ongoing operations

3. **Dismiss loading toasts**: Always dismiss loading toasts when operations complete

4. **Don't overuse**: Only show toasts for important user actions

5. **Keep messages concise**: Aim for one line of text

6. **Handle errors gracefully**: Provide helpful error messages, not technical jargon

## Requirements Validation

This implementation validates the following requirements:

- **Requirement 16.1**: Compilation errors display with line numbers (via error toasts)
- **Requirement 16.2**: Runtime errors display with exception messages (via error toasts)
- **Requirement 16.3**: Network errors display user-friendly messages (via error toasts)
- **Requirement 16.4**: Timeout errors clearly indicate time limit exceeded (via error toasts)

## Future Enhancements

Potential improvements for the toast system:

1. **Custom toast components**: Rich toasts with buttons, links, or progress bars
2. **Toast queue management**: Limit number of simultaneous toasts
3. **Persistent toasts**: Option for toasts that don't auto-dismiss
4. **Sound notifications**: Optional audio feedback for important toasts
5. **Toast history**: View dismissed toasts in a notification center
