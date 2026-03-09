# Error Boundary Components

This directory contains error boundary components for graceful error handling in the ASP.NET Core Learning Platform frontend.

## Overview

Error boundaries catch JavaScript errors in React component trees and display user-friendly fallback UI instead of crashing the entire application. This implementation validates **Requirement 16.3**: "WHEN a network error occurs, THE Platform SHALL display a user-friendly error message."

## Components

### 1. ErrorBoundary (Base Component)

The foundational error boundary class component that all other error boundaries build upon.

**Location**: `frontend/lib/components/ErrorBoundary.tsx`

**Features**:
- Catches errors in child component tree
- Displays customizable fallback UI
- Provides error reset functionality
- Logs errors for debugging
- Shows error details in development mode

**Usage**:
```tsx
import { ErrorBoundary } from '@/lib/components';

<ErrorBoundary>
  <YourComponent />
</ErrorBoundary>
```

**With custom fallback**:
```tsx
<ErrorBoundary fallback={<CustomErrorUI />}>
  <YourComponent />
</ErrorBoundary>
```

**With error handler**:
```tsx
<ErrorBoundary onError={(error, errorInfo) => {
  // Log to error tracking service
  console.error('Error:', error, errorInfo);
}}>
  <YourComponent />
</ErrorBoundary>
```

### 2. PageErrorBoundary

A page-level error boundary with a compact fallback UI suitable for full page sections.

**Location**: `frontend/lib/components/PageErrorBoundary.tsx`

**Usage**:
```tsx
import { PageErrorBoundary } from '@/lib/components';

<PageErrorBoundary pageName="challenges">
  <ChallengesList />
</PageErrorBoundary>
```

### 3. ComponentErrorBoundary

A component-level error boundary with minimal inline fallback UI for smaller sections.

**Location**: `frontend/lib/components/ComponentErrorBoundary.tsx`

**Usage**:
```tsx
import { ComponentErrorBoundary } from '@/lib/components';

<ComponentErrorBoundary componentName="challenge card">
  <ChallengeCard challenge={challenge} />
</ComponentErrorBoundary>
```

## Next.js App Router Error Files

### 1. Root Error Boundary

**Location**: `frontend/app/error.tsx`

Catches errors in the root app directory. Automatically used by Next.js.

### 2. Global Error Boundary

**Location**: `frontend/app/global-error.tsx`

Catches errors in the root layout. This is the last resort error handler.

### 3. Route-Specific Error Boundaries

**Locations**:
- `frontend/app/challenges/error.tsx` - Challenges section
- `frontend/app/courses/error.tsx` - Courses section
- `frontend/app/dashboard/error.tsx` - Dashboard section
- `frontend/app/ide/error.tsx` - Code editor section

These provide context-specific error messages for each section of the application.

## Error Handling Strategy

### 1. Granular Error Boundaries

Use multiple error boundaries at different levels:

```tsx
// Page level
<PageErrorBoundary pageName="dashboard">
  <div className="grid grid-cols-2 gap-4">
    {/* Component level */}
    <ComponentErrorBoundary componentName="XP progress">
      <XPProgressCard />
    </ComponentErrorBoundary>
    
    <ComponentErrorBoundary componentName="learning streak">
      <LearningStreakCard />
    </ComponentErrorBoundary>
  </div>
</PageErrorBoundary>
```

This ensures that if one component fails, others continue to work.

### 2. Network Error Handling

For API calls, combine error boundaries with try-catch:

```tsx
async function fetchData() {
  try {
    const response = await apiClient.get('/api/challenges');
    return response.data;
  } catch (error) {
    // Error boundary will catch this if thrown during render
    throw new Error('Failed to load challenges. Please check your connection.');
  }
}
```

### 3. Development vs Production

Error boundaries show different information based on environment:

- **Development**: Shows error message and stack trace
- **Production**: Shows user-friendly message only

## Best Practices

1. **Wrap at appropriate levels**: Don't wrap the entire app in one boundary. Use multiple boundaries for better isolation.

2. **Provide context**: Use descriptive names for `pageName` and `componentName` props.

3. **Log errors**: Use the `onError` callback to send errors to monitoring services:
   ```tsx
   <ErrorBoundary onError={(error, errorInfo) => {
     // Send to Sentry, LogRocket, etc.
     errorTrackingService.log(error, errorInfo);
   }}>
   ```

4. **Test error boundaries**: Simulate errors in development to ensure boundaries work:
   ```tsx
   // Test component that throws an error
   function ErrorTest() {
     throw new Error('Test error');
   }
   ```

5. **Provide recovery options**: Always give users a way to recover (reset, go home, refresh).

## Limitations

Error boundaries do NOT catch:
- Errors in event handlers (use try-catch)
- Asynchronous code (use try-catch)
- Server-side rendering errors
- Errors in the error boundary itself

For these cases, use traditional try-catch blocks.

## Testing

To test error boundaries in development:

1. Create a component that throws an error:
   ```tsx
   function BrokenComponent() {
     throw new Error('This component is broken!');
   }
   ```

2. Wrap it in an error boundary:
   ```tsx
   <ErrorBoundary>
     <BrokenComponent />
   </ErrorBoundary>
   ```

3. Verify the fallback UI appears instead of a blank page.

## Requirements Validation

These error boundary components validate:
- **Requirement 16.3**: "WHEN a network error occurs, THE Platform SHALL display a user-friendly error message"

The implementation ensures that:
- All errors display user-friendly messages
- Technical details are hidden in production
- Users have clear recovery options
- Errors are logged for debugging
