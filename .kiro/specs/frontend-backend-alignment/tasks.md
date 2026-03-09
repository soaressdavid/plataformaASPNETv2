# Frontend-Backend Alignment - Tasks

## Phase 1: Backend API Implementation

- [x] 1. Create DTOs and Mapping Extensions
  - **Estimated Effort**: 2 hours
  - **Dependencies**: None
  - **Requirements**: FR1.1-FR1.7
  - **Description**: Create all DTO classes and mapping extension methods for transforming entities to DTOs.
  - **Files to Create**:
    - `src/Services/Course/DTOs/LevelDtos.cs`
    - `src/Services/Course/DTOs/CourseDtos.cs`
    - `src/Services/Course/DTOs/LessonDtos.cs`
    - `src/Services/Course/DTOs/LessonContentDtos.cs`
    - `src/Services/Course/Extensions/MappingExtensions.cs`
  - **Acceptance Criteria**:
    - All DTO records defined with correct properties
    - Mapping extensions handle JSON deserialization for Topics, Prerequisites, StructuredContent
    - Null safety handled for optional fields
    - Code compiles without errors

- [x] 2. Create LevelsController
  - **Estimated Effort**: 3 hours
  - **Dependencies**: Task 1
  - **Requirements**: FR1.1, FR1.2, FR1.3
  - **Description**: Implement LevelsController with endpoints for listing levels, getting level details, and getting courses by level.
  - **Files to Create**:
    - `src/Services/Course/Controllers/LevelsController.cs`
  - **Acceptance Criteria**:
    - GET /api/levels returns all curriculum levels ordered by number
    - GET /api/levels/{id} returns level details with courses
    - GET /api/levels/{id}/courses returns courses for specific level
    - All endpoints use EF Core queries (no mock data)
    - Proper error handling for not found cases
    - Returns correct HTTP status codes

- [x] 3. Update CoursesController - Replace Mock Endpoints
  - **Estimated Effort**: 4 hours
  - **Dependencies**: Task 1
  - **Requirements**: FR1.4, FR1.5, FR1.6, FR1.7, FR1.8
  - **Description**: Replace all mock data endpoints in CoursesController with real database queries using EF Core.
  - **Files to Modify**:
    - `src/Services/Course/Program.cs` (remove mock endpoints)
    - `src/Services/Course/Controllers/CoursesController.cs` (create if doesn't exist)
  - **Acceptance Criteria**:
    - GET /api/courses queries database with optional levelId and level filters
    - GET /api/courses/{id} returns course with lessons
    - GET /api/courses/{id}/lessons returns lessons with structured content
    - GET /api/courses/{id}/lessons/{lessonId} returns single lesson detail
    - POST /api/courses/{courseId}/lessons/{lessonId}/complete works correctly
    - All mock data removed from Program.cs
    - Structured content properly deserialized from JSON
    - Topics and Prerequisites arrays properly parsed

- [x] 4. Implement Caching Service
  - **Estimated Effort**: 2 hours
  - **Dependencies**: Task 2
  - **Requirements**: NFR1.2
  - **Description**: Create caching service for curriculum levels to improve performance.
  - **Files to Create**:
    - `src/Services/Course/Services/CachedLevelsService.cs`
  - **Files to Modify**:
    - `src/Services/Course/Program.cs` (register service)
    - `src/Services/Course/Controllers/LevelsController.cs` (use cached service)
  - **Acceptance Criteria**:
    - Levels cached for 24 hours
    - Cache invalidation method available
    - LevelsController uses cached service
    - Memory cache properly configured in DI container

- [x] 5. Add Database Indexes
  - **Estimated Effort**: 1 hour
  - **Dependencies**: None
  - **Requirements**: NFR1.1
  - **Description**: Create database indexes to optimize query performance.
  - **Files to Create**:
    - `src/Services/Course/Migrations/AddPerformanceIndexes.cs`
  - **Acceptance Criteria**:
    - Index on Courses.LevelId
    - Index on Lessons.CourseId
    - Index on CurriculumLevels.Number
    - Migration runs successfully
    - Query performance improved

- [x] 6. Add Response Compression
  - **Estimated Effort**: 30 minutes
  - **Dependencies**: None
  - **Requirements**: NFR1.1
  - **Description**: Enable response compression for API endpoints.
  - **Files to Modify**:
    - `src/Services/Course/Program.cs`
  - **Acceptance Criteria**:
    - Response compression middleware configured
    - Gzip compression enabled
    - HTTPS compression enabled
    - Responses are compressed in production

- [x] 7. Backend Unit Tests
  - **Estimated Effort**: 4 hours
  - **Dependencies**: Tasks 1-3
  - **Requirements**: NFR3.2
  - **Description**: Write unit tests for controllers and mapping extensions.
  - **Files to Create**:
    - `src/Services/Course.Tests/Controllers/LevelsControllerTests.cs`
    - `src/Services/Course.Tests/Controllers/CoursesControllerTests.cs`
    - `src/Services/Course.Tests/Extensions/MappingExtensionsTests.cs`
  - **Acceptance Criteria**:
    - Test coverage > 80% for controllers
    - All mapping scenarios tested
    - Edge cases handled (null values, empty lists)
    - Tests pass successfully

- [x] 8. Backend Integration Tests
  - **Estimated Effort**: 3 hours
  - **Dependencies**: Tasks 2-3
  - **Requirements**: NFR3.2
  - **Description**: Write integration tests for API endpoints.
  - **Files to Create**:
    - `tests/Course.Tests/Integration/LevelsApiTests.cs`
    - `tests/Course.Tests/Integration/CoursesApiTests.cs`
  - **Acceptance Criteria**:
    - All endpoints tested end-to-end
    - Database seeded with test data
    - Response formats validated
    - HTTP status codes verified

## Phase 2: Frontend Type Definitions

- [x] 9. Update Frontend Types
  - **Estimated Effort**: 2 hours
  - **Dependencies**: Task 1
  - **Requirements**: FR2.1, FR2.2, FR2.3, FR2.4, FR2.5
  - **Description**: Add new types and update existing types in frontend to match backend DTOs.
  - **Files to Modify**:
    - `frontend/lib/types.ts`
  - **Acceptance Criteria**:
    - CurriculumLevel and CurriculumLevelDetail types defined
    - CourseSummary updated with levelId, duration, topics, orderIndex
    - LessonDetail updated with duration, difficulty, estimatedMinutes, structuredContent, prerequisites
    - LessonContent type matches backend structure
    - TheorySection, CodeExample, Exercise types defined
    - All types have correct TypeScript syntax
    - No compilation errors

- [x] 10. Create Levels API Client
  - **Estimated Effort**: 1 hour
  - **Dependencies**: Task 9
  - **Requirements**: FR3.1, FR3.2, FR3.3
  - **Description**: Create API client for levels endpoints.
  - **Files to Create**:
    - `frontend/lib/api/levels.ts`
  - **Files to Modify**:
    - `frontend/lib/api/index.ts` (export levelsApi)
  - **Acceptance Criteria**:
    - levelsApi.getAll() implemented
    - levelsApi.getById(id) implemented
    - levelsApi.getCourses(id) implemented
    - Proper TypeScript types used
    - Error handling included

- [x] 11. Update Courses API Client
  - **Estimated Effort**: 1.5 hours
  - **Dependencies**: Task 9
  - **Requirements**: FR3.4, FR3.5, FR3.6, FR3.7
  - **Description**: Update courses API client with new methods and parameters.
  - **Files to Modify**:
    - `frontend/lib/api/courses.ts`
  - **Acceptance Criteria**:
    - coursesApi.getAll() supports levelId and level filters
    - coursesApi.getById(id) implemented
    - coursesApi.getLessons() returns updated lesson structure
    - coursesApi.getLesson(courseId, lessonId) implemented
    - All methods properly typed

## Phase 3: Frontend Hooks

- [x] 12. Create useLevel Hooks
  - **Estimated Effort**: 2 hours
  - **Dependencies**: Task 10
  - **Requirements**: FR3.1, FR3.2, FR3.3
  - **Description**: Create custom hooks for fetching and managing level data.
  - **Files to Create**:
    - `frontend/lib/hooks/useLevel.ts`
  - **Files to Modify**:
    - `frontend/lib/hooks/index.ts` (export hooks)
  - **Acceptance Criteria**:
    - useLevels() hook fetches all levels
    - useLevel(id) hook fetches single level
    - Loading and error states managed
    - Proper TypeScript types
    - Hooks tested

- [x] 13. Create useStructuredLesson Hook
  - **Estimated Effort**: 2 hours
  - **Dependencies**: Task 11
  - **Requirements**: FR3.7, FR6.2
  - **Description**: Create hook for fetching lessons and detecting content type.
  - **Files to Create**:
    - `frontend/lib/hooks/useStructuredLesson.ts`
  - **Files to Modify**:
    - `frontend/lib/hooks/index.ts` (export hook)
  - **Acceptance Criteria**:
    - useStructuredLesson(courseId, lessonId) fetches lesson
    - Automatically detects content type (structured vs HTML)
    - Returns contentType: 'structured' | 'html' | null
    - Loading and error states managed
    - Hook tested

## Phase 4: Frontend Components - Structured Content

- [x] 14. Install Frontend Dependencies
  - **Estimated Effort**: 15 minutes
  - **Dependencies**: None
  - **Requirements**: NFR1.4
  - **Description**: Install required npm packages for markdown and syntax highlighting.
  - **Commands to Run**:
    ```bash
    cd frontend
    npm install react-markdown react-syntax-highlighter react-window
    npm install -D @types/react-syntax-highlighter @types/react-window
    ```
  - **Acceptance Criteria**:
    - All packages installed successfully
    - package.json updated
    - No dependency conflicts

- [x] 15. Create LessonObjectives Component
  - **Estimated Effort**: 1 hour
  - **Dependencies**: Task 9, Task 14
  - **Requirements**: FR4.1
  - **Description**: Create component to render lesson objectives list.
  - **Files to Create**:
    - `frontend/lib/components/LessonObjectives.tsx`
  - **Acceptance Criteria**:
    - Renders list of objectives with icons
    - Proper styling
    - Accessible markup
    - Component exported from index.ts

- [x] 16. Create TheorySection Component
  - **Estimated Effort**: 1.5 hours
  - **Dependencies**: Task 9, Task 14
  - **Requirements**: FR4.2
  - **Description**: Create component to render theory sections with markdown support.
  - **Files to Create**:
    - `frontend/lib/components/TheorySection.tsx`
  - **Acceptance Criteria**:
    - Renders heading and markdown content
    - react-markdown properly configured
    - Code blocks in markdown styled correctly
    - Sections ordered by order property

- [x] 17. Create CodeExample Component
  - **Estimated Effort**: 2 hours
  - **Dependencies**: Task 9, Task 14
  - **Requirements**: FR4.3
  - **Description**: Create component to render code examples with syntax highlighting.
  - **Files to Create**:
    - `frontend/lib/components/CodeExample.tsx`
  - **Acceptance Criteria**:
    - Syntax highlighting works for multiple languages
    - Line numbers displayed
    - Copy button included
    - Run button shown for runnable examples
    - Explanation text rendered below code

- [x] 18. Create ExerciseList Component
  - **Estimated Effort**: 2 hours
  - **Dependencies**: Task 9
  - **Requirements**: FR4.4
  - **Description**: Create component to render practice exercises.
  - **Files to Create**:
    - `frontend/lib/components/ExerciseList.tsx`
  - **Acceptance Criteria**:
    - Exercises displayed in grid layout
    - Difficulty badges styled correctly
    - Hints shown in collapsible section
    - Start button triggers callback
    - Responsive design

- [x] 19. Create LessonSummary Component
  - **Estimated Effort**: 30 minutes
  - **Dependencies**: Task 9
  - **Requirements**: FR4.5
  - **Description**: Create component to render lesson summary.
  - **Files to Create**:
    - `frontend/lib/components/LessonSummary.tsx`
  - **Acceptance Criteria**:
    - Summary text rendered
    - Proper styling
    - Positioned at end of lesson

- [x] 20. Create StructuredLessonView Component
  - **Estimated Effort**: 2 hours
  - **Dependencies**: Tasks 15-19
  - **Requirements**: FR4.6
  - **Description**: Create main component that orchestrates all structured content components.
  - **Files to Create**:
    - `frontend/lib/components/StructuredLessonView.tsx`
  - **Acceptance Criteria**:
    - Renders all sub-components in correct order
    - Handles callbacks for code execution and exercises
    - Proper spacing and layout
    - Responsive design

- [x] 21. Create LessonContent Smart Component
  - **Estimated Effort**: 1.5 hours
  - **Dependencies**: Task 20
  - **Requirements**: FR6.1, FR6.2, FR6.4
  - **Description**: Create smart component that detects content type and renders appropriately.
  - **Files to Create**:
    - `frontend/lib/components/LessonContent.tsx`
  - **Acceptance Criteria**:
    - Prefers structuredContent over HTML content
    - Falls back to HTML if structured content unavailable
    - Shows message if no content available
    - Backward compatible with legacy lessons

## Phase 5: Frontend Components - Navigation

- [x] 22. Create LevelCard Component
  - **Estimated Effort**: 2 hours
  - **Dependencies**: Task 9
  - **Requirements**: FR5.1
  - **Description**: Create card component for displaying curriculum levels.
  - **Files to Create**:
    - `frontend/lib/components/LevelCard.tsx`
  - **Acceptance Criteria**:
    - Shows level number, title, description
    - Displays course count and estimated hours
    - Lock overlay for locked levels
    - Progress bar for in-progress levels
    - Clickable link to level detail page
    - Responsive design

- [x] 23. Create Breadcrumb Component
  - **Estimated Effort**: 1 hour
  - **Dependencies**: None
  - **Requirements**: FR5.5
  - **Description**: Create breadcrumb navigation component.
  - **Files to Create**:
    - `frontend/lib/components/Breadcrumb.tsx`
  - **Acceptance Criteria**:
    - Renders breadcrumb items with separators
    - Links work correctly
    - Last item not clickable
    - Accessible markup (aria-label)
    - Responsive design

- [x] 24. Create CourseCard Component (if not exists)
  - **Estimated Effort**: 1.5 hours
  - **Dependencies**: Task 9
  - **Requirements**: FR5.2
  - **Description**: Create or update course card component to show level information.
  - **Files to Create/Modify**:
    - `frontend/lib/components/CourseCard.tsx`
  - **Acceptance Criteria**:
    - Shows course title, description, level
    - Displays duration and lesson count
    - Shows topics as tags
    - Clickable link to course detail
    - Responsive design

## Phase 6: Frontend Pages

- [x] 25. Create Levels List Page
  - **Estimated Effort**: 2 hours
  - **Dependencies**: Tasks 12, 22
  - **Requirements**: FR5.1, US1
  - **Description**: Create page to display all curriculum levels.
  - **Files to Create**:
    - `frontend/app/levels/page.tsx`
  - **Acceptance Criteria**:
    - Uses useLevels hook
    - Renders LevelCard for each level
    - Loading state shown while fetching
    - Error state handled gracefully
    - Grid layout responsive
    - Page title and description

- [x] 26. Create Level Detail Page
  - **Estimated Effort**: 3 hours
  - **Dependencies**: Tasks 12, 23, 24
  - **Requirements**: FR5.2, US2
  - **Description**: Create page to show courses within a specific level.
  - **Files to Create**:
    - `frontend/app/levels/[id]/page.tsx`
  - **Acceptance Criteria**:
    - Uses useLevel(id) hook
    - Shows level header with metadata
    - Renders courses in grid
    - Shows capstone project if available
    - Breadcrumb navigation included
    - Loading and error states
    - Responsive design

- [x] 27. Update Lesson Page
  - **Estimated Effort**: 3 hours
  - **Dependencies**: Tasks 13, 21, 23
  - **Requirements**: FR4.6, FR6.1, US3, US4
  - **Description**: Update lesson page to use new LessonContent component with breadcrumb.
  - **Files to Modify**:
    - `frontend/app/courses/[id]/lessons/[lessonId]/page.tsx`
  - **Acceptance Criteria**:
    - Uses useStructuredLesson hook
    - Renders LessonContent component
    - Breadcrumb shows Level > Course > Lesson
    - Code execution callback integrated
    - Exercise start callback integrated
    - Lesson metadata displayed (duration, difficulty)
    - Complete and next buttons work
    - Works with both structured and HTML content

- [x] 28. Update Courses List Page (Optional)
  - **Estimated Effort**: 2 hours
  - **Dependencies**: Task 11
  - **Requirements**: FR5.3
  - **Description**: Add level filter to courses list page.
  - **Files to Modify**:
    - `frontend/app/courses/page.tsx`
  - **Acceptance Criteria**:
    - Level filter dropdown added ✅
    - Filters courses by selected level ✅
    - Shows all courses when no filter ✅
    - Filter state persists in URL ✅

- [x] 29. Update Course Detail Page
  - **Estimated Effort**: 1.5 hours
  - **Dependencies**: Task 23
  - **Requirements**: FR5.4
  - **Description**: Add level information and breadcrumb to course detail page.
  - **Files to Modify**:
    - `frontend/app/courses/[id]/page.tsx`
  - **Acceptance Criteria**:
    - Shows level badge/link
    - Breadcrumb shows Level > Course
    - Level title displayed
    - Links to level page work

## Phase 7: Testing and Polish

- [x] 30. Frontend Component Tests
  - **Estimated Effort**: 4 hours
  - **Dependencies**: Tasks 15-24
  - **Requirements**: NFR3.2
  - **Description**: Write unit tests for all new components.
  - **Files to Create**:
    - `frontend/lib/components/__tests__/LessonObjectives.test.tsx`
    - `frontend/lib/components/__tests__/TheorySection.test.tsx`
    - `frontend/lib/components/__tests__/CodeExample.test.tsx`
    - `frontend/lib/components/__tests__/ExerciseList.test.tsx`
    - `frontend/lib/components/__tests__/LessonContent.test.tsx`
    - `frontend/lib/components/__tests__/LevelCard.test.tsx`
    - `frontend/lib/components/__tests__/Breadcrumb.test.tsx`
  - **Acceptance Criteria**:
    - All components have tests
    - Test coverage > 80%
    - Edge cases tested
    - Tests pass successfully

- [x] 31. Frontend Hook Tests
  - **Estimated Effort**: 2 hours
  - **Dependencies**: Tasks 12-13
  - **Requirements**: NFR3.2
  - **Description**: Write tests for custom hooks.
  - **Files to Create**:
    - `frontend/lib/hooks/__tests__/useLevel.test.ts`
    - `frontend/lib/hooks/__tests__/useStructuredLesson.test.ts`
  - **Acceptance Criteria**:
    - All hooks tested
    - Loading states tested
    - Error states tested
    - API mocking works correctly

- [x] 32. Add Loading Skeletons
  - **Estimated Effort**: 2 hours
  - **Dependencies**: Tasks 25-27
  - **Requirements**: NFR2.2
  - **Description**: Add skeleton loading states for all pages.
  - **Files to Modify**:
    - `frontend/app/levels/page.tsx`
    - `frontend/app/levels/[id]/page.tsx`
    - `frontend/app/courses/[id]/lessons/[lessonId]/page.tsx`
  - **Acceptance Criteria**:
    - Skeleton screens match final layout
    - Smooth transition from skeleton to content
    - Loading states feel responsive

- [x] 33. Add Error Boundaries
  - **Estimated Effort**: 1.5 hours
  - **Dependencies**: Task 21
  - **Requirements**: NFR2.3
  - **Description**: Add error boundaries for structured content rendering.
  - **Files to Create**:
    - `frontend/lib/components/LessonErrorBoundary.tsx`
  - **Files to Modify**:
    - `frontend/app/courses/[id]/lessons/[lessonId]/page.tsx`
  - **Acceptance Criteria**:
    - Errors caught and displayed gracefully
    - Retry button works
    - Error logged to console
    - User-friendly error messages

- [x] 34. Performance Optimization
  - **Estimated Effort**: 3 hours
  - **Dependencies**: Tasks 15-21
  - **Requirements**: NFR1.3, NFR1.4
  - **Description**: Optimize component performance with memoization and code splitting.
  - **Files to Modify**:
    - All component files (add React.memo where appropriate)
    - `frontend/lib/components/CodeExample.tsx` (lazy load syntax highlighter)
    - `frontend/lib/components/TheorySection.tsx` (lazy load markdown)
  - **Acceptance Criteria**:
    - Components memoized appropriately
    - Syntax highlighter lazy loaded
    - Markdown renderer lazy loaded
    - No unnecessary re-renders
    - Lighthouse performance score > 90

- [x] 35. Styling and Responsive Design
  - **Estimated Effort**: 4 hours
  - **Dependencies**: Tasks 15-24
  - **Requirements**: NFR2.4
  - **Description**: Add CSS styling and ensure responsive design for all components.
  - **Files to Create/Modify**:
    - Component-specific CSS files or Tailwind classes
    - `frontend/app/globals.css` (add global styles)
  - **Acceptance Criteria**:
    - All components styled consistently
    - Responsive on mobile, tablet, desktop
    - Dark mode support (if applicable)
    - Accessibility standards met
    - Visual design matches mockups

- [x] 36. End-to-End Testing
  - **Estimated Effort**: 4 hours
  - **Dependencies**: Tasks 25-27
  - **Requirements**: NFR3.2
  - **Description**: Write E2E tests for critical user flows.
  - **Files to Create**:
    - `frontend/e2e/levels-navigation.spec.ts`
    - `frontend/e2e/lesson-viewing.spec.ts`
    - `frontend/e2e/content-rendering.spec.ts`
  - **Acceptance Criteria**:
    - User can navigate from levels to lessons
    - Structured content renders correctly
    - HTML content fallback works
    - Code examples can be executed
    - Tests pass in CI/CD

- [x] 37. Documentation
  - **Estimated Effort**: 3 hours
  - **Dependencies**: All previous tasks
  - **Requirements**: NFR3.4
  - **Description**: Create documentation for new features and components.
  - **Files to Create**:
    - `frontend/lib/components/STRUCTURED_CONTENT.md`
    - `docs/API_ENDPOINTS.md`
    - `docs/MIGRATION_GUIDE.md`
  - **Acceptance Criteria**:
    - Component usage documented
    - API endpoints documented
    - Migration guide for developers
    - Code examples included
    - README updated

- [x] 38. Final Integration Testing
  - **Estimated Effort**: 4 hours
  - **Dependencies**: All previous tasks
  - **Requirements**: All success criteria
  - **Description**: Perform comprehensive integration testing of entire system.
  - **Acceptance Criteria**:
    - All 40 lessons (Level0 + Level1) accessible via frontend
    - Structured content renders correctly
    - Legacy HTML lessons still work
    - Navigation flows work end-to-end
    - Performance metrics met (< 200ms API, < 2s page load)
    - No console errors
    - All user stories validated

## Summary

**Total Tasks**: 38 (37 required, 1 optional)
**Estimated Total Effort**: 80-85 hours
**Phases**: 7
**Status**: ✅ Database migrated from PostgreSQL to SQL Server successfully!

**Migration Completed**:
- ✅ Stopped all running services
- ✅ Updated Shared.csproj to use Microsoft.EntityFrameworkCore.SqlServer
- ✅ Updated connection strings to SQL Server
- ✅ Created SqlServerHealthCheck to replace PostgresHealthCheck
- ✅ Updated all service references to use compatible package versions
- ✅ Created and applied InitialCreate migration to SQL Server
- ✅ Verified all tables created successfully (13 tables including CurriculumLevels)
- ✅ Course.Service running successfully on port 5002
- ✅ API endpoints responding correctly (GET /api/levels, GET /api/courses)

**Critical Path**:
1. Backend DTOs and Controllers (Tasks 1-3) - ✅ COMPLETED
2. Frontend Types and API Clients (Tasks 9-11) - 🔄 READY TO START
3. Structured Content Components (Tasks 15-21) - 🔄 PENDING
4. Pages Implementation (Tasks 25-27) - 🔄 PENDING
5. Testing and Polish (Tasks 30-38) - 🔄 PENDING
