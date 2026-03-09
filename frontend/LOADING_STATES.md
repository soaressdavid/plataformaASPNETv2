# Loading States and Skeletons Implementation

This document describes the loading states and skeleton components implemented for the ASP.NET Core Learning Platform frontend.

## Overview

Loading states provide visual feedback to users while data is being fetched or processed. This implementation includes:

1. **Loading Skeletons** - Placeholder UI that mimics the structure of the actual content
2. **Loading Indicators** - Spinners and progress indicators for async operations
3. **Automatic Output Panel** - Code editor automatically shows output when execution starts

## Components

### Loading Skeletons

All skeleton components are located in `lib/components/LoadingSkeletons.tsx`:

#### 1. DashboardSkeleton
- **Usage**: Dashboard page while loading user progress data
- **Shows**: Placeholders for XP/Level section, stats cards, and courses in progress
- **Location**: `app/dashboard/page.tsx`

#### 2. ChallengesSkeleton
- **Usage**: Challenges list page while loading challenges
- **Shows**: Placeholders for header, filters, stats, and challenge cards in grid layout
- **Location**: `app/challenges/page.tsx`

#### 3. CoursesSkeleton
- **Usage**: Courses list page while loading courses
- **Shows**: Placeholders for header, filters, stats, and course cards in grid layout
- **Location**: `app/courses/page.tsx`

#### 4. ChallengeDetailSkeleton
- **Usage**: Challenge detail page while loading challenge data
- **Shows**: Placeholders for header, description, test cases, and code editor
- **Location**: `app/challenges/[id]/page.tsx`

#### 5. CourseDetailSkeleton
- **Usage**: Course detail page while loading lessons
- **Shows**: Placeholders for course info and lesson list
- **Location**: `app/courses/[id]/page.tsx`

### Loading Indicators

#### 1. CodeExecutionLoader
- **Usage**: Code editor terminal output during code execution
- **Shows**: Spinner with "Executing code..." message
- **Location**: `lib/components/CodeEditor/CodeEditor.tsx`
- **Behavior**: Automatically displays in terminal when `isRunning` prop is true

#### 2. AIFeedbackLoader
- **Usage**: When AI is analyzing code for feedback
- **Shows**: Spinner with "Analyzing code with AI..." message
- **Note**: Ready for integration when AI feedback feature is implemented

#### 3. LoadingSpinner
- **Usage**: Generic loading spinner for any loading state
- **Props**: 
  - `size`: 'sm' | 'md' | 'lg' (default: 'md')
  - `message`: Optional message to display below spinner

#### 4. PageLoader
- **Usage**: Full-page loading state with centered spinner
- **Props**: 
  - `message`: Optional message (default: 'Loading...')

## Implementation Details

### Skeleton Design Principles

1. **Match Layout**: Skeletons closely match the structure of actual content
2. **Animate**: All skeletons use `animate-pulse` for visual feedback
3. **Responsive**: Skeletons adapt to different screen sizes like actual content
4. **Consistent Colors**: Use gray-200 for light mode, gray-700 for dark mode

### Code Editor Enhancements

The CodeEditor component now includes:

1. **Automatic Output Panel**: When code execution starts (`isRunning` becomes true), the terminal output panel automatically opens
2. **Loading Indicator**: Shows `CodeExecutionLoader` in terminal during execution
3. **Visual Feedback**: Run button shows "Running..." text and is disabled during execution

### Usage Example

```tsx
import { DashboardSkeleton, CodeExecutionLoader } from '@/lib/components';

// In a page component
if (loading) {
  return <DashboardSkeleton />;
}

// In code editor terminal
{isRunning ? (
  <CodeExecutionLoader />
) : output ? (
  <pre>{output}</pre>
) : (
  <span>No output yet.</span>
)}
```

## Benefits

1. **Better UX**: Users see immediate feedback that content is loading
2. **Reduced Perceived Wait Time**: Skeleton screens make loading feel faster
3. **Professional Appearance**: Polished loading states improve overall quality
4. **Consistent Experience**: All pages use similar loading patterns

## Future Enhancements

1. **AI Feedback Integration**: Use `AIFeedbackLoader` when AI code review is implemented
2. **Progressive Loading**: Load and display content incrementally as it becomes available
3. **Error States**: Add skeleton variants for error states with retry buttons
4. **Accessibility**: Add ARIA labels and screen reader announcements for loading states

## Testing

To test loading states:

1. **Slow Network**: Use browser DevTools to throttle network speed
2. **Artificial Delay**: Add `setTimeout` in API calls to simulate slow responses
3. **Visual Inspection**: Verify skeletons match actual content layout
4. **Responsive Testing**: Check skeletons on different screen sizes

## Related Files

- `lib/components/LoadingSkeletons.tsx` - All skeleton and loader components
- `lib/components/index.ts` - Component exports
- `lib/components/Dashboard.tsx` - Dashboard with skeleton
- `lib/components/CodeEditor/CodeEditor.tsx` - Code editor with execution loader
- `app/challenges/page.tsx` - Challenges list with skeleton
- `app/courses/page.tsx` - Courses list with skeleton
- `app/challenges/[id]/page.tsx` - Challenge detail with skeleton
- `app/courses/[id]/page.tsx` - Course detail with skeleton
