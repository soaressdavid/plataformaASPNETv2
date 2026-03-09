# Error Boundary Usage Examples

This document provides practical examples of using error boundaries in the ASP.NET Core Learning Platform.

## Example 1: Wrapping the Dashboard Component

The dashboard displays multiple data sections. Wrap each section to isolate failures:

```tsx
'use client';

import { ComponentErrorBoundary } from '@/lib/components';
import { XPProgress, LearningStreak, CourseProgress } from './components';

export function Dashboard() {
  return (
    <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
      <ComponentErrorBoundary componentName="XP progress">
        <XPProgress />
      </ComponentErrorBoundary>

      <ComponentErrorBoundary componentName="learning streak">
        <LearningStreak />
      </ComponentErrorBoundary>

      <ComponentErrorBoundary componentName="course progress">
        <CourseProgress />
      </ComponentErrorBoundary>
    </div>
  );
}
```

## Example 2: Wrapping API Data Fetching

When fetching data that might fail, wrap the component:

```tsx
'use client';

import { useState, useEffect } from 'react';
import { PageErrorBoundary } from '@/lib/components';
import { apiClient } from '@/lib/api-client';

function ChallengesList() {
  const [challenges, setChallenges] = useState([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    async function fetchChallenges() {
      try {
        const response = await apiClient.get('/api/challenges');
        setChallenges(response.data);
      } catch (error) {
        // This will be caught by the error boundary
        throw new Error('Failed to load challenges. Please check your connection.');
      } finally {
        setLoading(false);
      }
    }
    fetchChallenges();
  }, []);

  if (loading) return <div>Loading...</div>;

  return (
    <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
      {challenges.map(challenge => (
        <ChallengeCard key={challenge.id} challenge={challenge} />
      ))}
    </div>
  );
}

// Wrap the component with error boundary
export default function ChallengesPage() {
  return (
    <PageErrorBoundary pageName="challenges">
      <ChallengesList />
    </PageErrorBoundary>
  );
}
```

## Example 3: Monaco Editor with Error Boundary

The code editor might fail to load due to browser issues:

```tsx
'use client';

import { ComponentErrorBoundary } from '@/lib/components';
import { CodeEditor } from '@/lib/components/CodeEditor';

export function IDEPage() {
  return (
    <div className="h-screen flex flex-col">
      <header>
        {/* Header content */}
      </header>
      
      <main className="flex-1">
        <ComponentErrorBoundary componentName="code editor">
          <CodeEditor
            files={files}
            activeFile={activeFile}
            onFileChange={handleFileChange}
            onRun={handleRun}
          />
        </ComponentErrorBoundary>
      </main>
    </div>
  );
}
```

## Example 4: Custom Error Handling with Logging

Send errors to a monitoring service:

```tsx
'use client';

import { ErrorBoundary } from '@/lib/components';

function logErrorToService(error: Error, errorInfo: React.ErrorInfo) {
  // Send to error tracking service (e.g., Sentry, LogRocket)
  console.error('Error logged:', {
    message: error.message,
    stack: error.stack,
    componentStack: errorInfo.componentStack,
    timestamp: new Date().toISOString(),
  });
  
  // In production, send to monitoring service:
  // errorTrackingService.captureException(error, { extra: errorInfo });
}

export function MonitoredComponent({ children }: { children: React.ReactNode }) {
  return (
    <ErrorBoundary onError={logErrorToService}>
      {children}
    </ErrorBoundary>
  );
}
```

## Example 5: Custom Fallback UI

Provide a custom error UI that matches your design:

```tsx
'use client';

import { ErrorBoundary } from '@/lib/components';

const CustomErrorFallback = (
  <div className="bg-red-50 border-l-4 border-red-500 p-4 rounded">
    <div className="flex items-center">
      <div className="flex-shrink-0">
        <svg className="h-5 w-5 text-red-500" viewBox="0 0 20 20" fill="currentColor">
          <path fillRule="evenodd" d="M10 18a8 8 0 100-16 8 8 0 000 16zM8.707 7.293a1 1 0 00-1.414 1.414L8.586 10l-1.293 1.293a1 1 0 101.414 1.414L10 11.414l1.293 1.293a1 1 0 001.414-1.414L11.414 10l1.293-1.293a1 1 0 00-1.414-1.414L10 8.586 8.707 7.293z" clipRule="evenodd" />
        </svg>
      </div>
      <div className="ml-3">
        <h3 className="text-sm font-medium text-red-800">
          Unable to load this section
        </h3>
        <p className="mt-1 text-sm text-red-700">
          Please refresh the page or try again later.
        </p>
      </div>
    </div>
  </div>
);

export function ComponentWithCustomError({ children }: { children: React.ReactNode }) {
  return (
    <ErrorBoundary fallback={CustomErrorFallback}>
      {children}
    </ErrorBoundary>
  );
}
```

## Example 6: Nested Error Boundaries

Use multiple levels for fine-grained error isolation:

```tsx
'use client';

import { PageErrorBoundary, ComponentErrorBoundary } from '@/lib/components';

export function CoursePage() {
  return (
    <PageErrorBoundary pageName="course details">
      <div className="container mx-auto p-6">
        <ComponentErrorBoundary componentName="course header">
          <CourseHeader />
        </ComponentErrorBoundary>

        <div className="grid grid-cols-1 lg:grid-cols-3 gap-6 mt-6">
          <div className="lg:col-span-2">
            <ComponentErrorBoundary componentName="lesson list">
              <LessonList />
            </ComponentErrorBoundary>
          </div>

          <div>
            <ComponentErrorBoundary componentName="course progress">
              <CourseProgressSidebar />
            </ComponentErrorBoundary>
          </div>
        </div>
      </div>
    </PageErrorBoundary>
  );
}
```

## Example 7: Testing Error Boundaries

Create a test component to verify error boundaries work:

```tsx
'use client';

import { useState } from 'react';
import { ErrorBoundary } from '@/lib/components';

function BrokenComponent() {
  throw new Error('This component intentionally throws an error!');
}

export function ErrorBoundaryTest() {
  const [showBroken, setShowBroken] = useState(false);

  return (
    <div className="p-6">
      <button
        onClick={() => setShowBroken(true)}
        className="px-4 py-2 bg-red-600 text-white rounded"
      >
        Trigger Error
      </button>

      <ErrorBoundary>
        {showBroken && <BrokenComponent />}
        {!showBroken && <p>Click the button to test the error boundary</p>}
      </ErrorBoundary>
    </div>
  );
}
```

## Best Practices Summary

1. **Use route-level error boundaries** (automatic via `error.tsx` files)
2. **Wrap data-fetching components** with `PageErrorBoundary`
3. **Wrap individual UI sections** with `ComponentErrorBoundary`
4. **Provide descriptive names** for better error messages
5. **Log errors** to monitoring services in production
6. **Test error boundaries** during development
7. **Don't over-wrap** - find the right granularity

## Common Pitfalls to Avoid

1. **Don't wrap the entire app** - Use multiple boundaries for isolation
2. **Don't forget event handlers** - Use try-catch for async operations
3. **Don't ignore errors** - Always log them for debugging
4. **Don't show technical details** in production - Keep messages user-friendly
