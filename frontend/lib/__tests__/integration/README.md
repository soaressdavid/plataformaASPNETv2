# Frontend Integration Tests

This directory contains comprehensive integration tests for the ASP.NET Core Learning Platform frontend application.

## Overview

The integration tests validate complete user workflows by testing the interaction between components, API calls, and state management. These tests mock API responses and verify that the UI correctly handles various scenarios.

## Test Suites

### 1. Authentication Integration Tests (`auth.integration.test.tsx`)

**Validates Requirements:** 1.1, 1.2, 1.3

Tests the complete authentication flow including:

- **User Registration Flow**
  - Successfully register a new user and redirect to dashboard
  - Show error when passwords do not match
  - Show error when password is too short
  - Handle registration API errors

- **User Login Flow**
  - Successfully login and redirect to dashboard
  - Reject invalid credentials
  - Show loading state during login

- **User Logout Flow**
  - Clear token and user state on logout

- **Token Management**
  - Store token in localStorage after successful login
  - Retrieve token from localStorage
  - Return null when no token exists

**Total Tests:** 11

### 2. Challenge Integration Tests (`challenges.integration.test.tsx`)

**Validates Requirements:** 5.1, 5.2, 5.3

Tests challenge submission and result display workflows:

- **Challenge Listing and Filtering**
  - Fetch and display all challenges
  - Filter challenges by difficulty

- **Challenge Detail Retrieval**
  - Fetch challenge details with starter code and test cases
  - Display challenge description and starter code

- **Challenge Submission**
  - Submit solution and receive test results
  - Display failed test cases with expected vs actual output
  - Award XP based on difficulty when all tests pass
  - Execute all test cases against submitted code

- **Challenge Submission Error Handling**
  - Handle compilation errors
  - Handle runtime errors

**Total Tests:** 10

### 3. Course Integration Tests (`courses.integration.test.tsx`)

**Validates Requirements:** 7.1, 7.3

Tests course enrollment and lesson completion workflows:

- **Course Listing**
  - Fetch and display all courses
  - Organize courses by difficulty level

- **Course Enrollment and Lesson Access**
  - Fetch lessons for an enrolled course
  - Maintain lesson order within course

- **Lesson Completion**
  - Mark lesson as complete and unlock next lesson
  - Track progress through lessons when enrolled
  - Return no next lesson when completing final lesson

- **Course Progress Calculation**
  - Calculate completion percentage correctly
  - Show 0% for newly enrolled course
  - Show 100% for completed course

- **Error Handling**
  - Handle course not found error
  - Handle lesson completion failure

**Total Tests:** 13

### 4. Dashboard Integration Tests (`dashboard.integration.test.tsx`)

**Validates Requirements:** 8.1

Tests dashboard data display and real-time updates:

- **Dashboard Data Display**
  - Fetch and display current XP and level
  - Display solved challenges by difficulty
  - Display completed projects count
  - Display learning streak in days
  - Display courses in progress with completion percentage

- **Dashboard Real-time Updates**
  - Update XP when challenge is completed
  - Increment level when XP threshold is reached
  - Update learning streak when activity continues

- **Dashboard for New Users**
  - Display zero state for new user

- **Error Handling**
  - Handle dashboard fetch error
  - Handle unauthorized access

- **XP Calculation Validation**
  - Correctly calculate total XP from multiple challenges
  - Calculate level based on XP formula
  - Calculate XP needed for next level

**Total Tests:** 13

## Running the Tests

### Run all integration tests:
```bash
npm test -- lib/__tests__/integration/ --no-coverage
```

### Run a specific test suite:
```bash
npm test -- lib/__tests__/integration/auth.integration.test.tsx --no-coverage
```

### Run with coverage:
```bash
npm test -- lib/__tests__/integration/ --coverage
```

### Run in watch mode:
```bash
npm test:watch -- lib/__tests__/integration/
```

## Test Structure

Each test suite follows this structure:

1. **Setup**: Mock dependencies (axios, Next.js router, react-hot-toast)
2. **Arrange**: Set up test data and mock API responses
3. **Act**: Simulate user interactions or API calls
4. **Assert**: Verify expected outcomes

## Mocking Strategy

### API Calls
- Uses Jest to mock axios for API requests
- Each test defines specific mock responses for its scenario
- Mocks are cleared between tests to ensure isolation

### Next.js Router
- Mocks `useRouter` from `next/navigation`
- Verifies navigation calls (e.g., redirect to dashboard after login)

### Toast Notifications
- Mocks `react-hot-toast` to prevent actual toast displays during tests
- Allows tests to run without side effects

### LocalStorage
- Custom localStorage mock in `setup.ts`
- Provides full localStorage API for testing token storage

## Key Testing Patterns

### User Interaction Testing
```typescript
const user = userEvent.setup();
await user.type(screen.getByLabelText(/email/i), 'test@example.com');
await user.click(screen.getByRole('button', { name: /sign in/i }));
```

### API Response Mocking
```typescript
mockedAxios.post.mockResolvedValueOnce({
  data: { userId: 'user-123', token: 'test-token' },
});
```

### Async Assertion
```typescript
await waitFor(() => {
  expect(localStorage.getItem('auth_token')).toBe('test-token');
  expect(mockPush).toHaveBeenCalledWith('/dashboard');
});
```

## Test Coverage

The integration tests cover:

- ✅ Authentication flows (login, registration, logout)
- ✅ Challenge submission and result display
- ✅ Course enrollment and lesson completion
- ✅ Dashboard data display and real-time updates
- ✅ Error handling for all workflows
- ✅ Token management and persistence
- ✅ XP calculation and level progression
- ✅ Progress tracking across courses and challenges

## Requirements Validation

| Requirement | Test Suite | Status |
|-------------|------------|--------|
| 1.1 - User account creation | auth.integration.test.tsx | ✅ Passing |
| 1.2 - User authentication | auth.integration.test.tsx | ✅ Passing |
| 1.3 - Invalid credentials rejection | auth.integration.test.tsx | ✅ Passing |
| 5.1 - Challenge storage | challenges.integration.test.tsx | ✅ Passing |
| 5.2 - Challenge display | challenges.integration.test.tsx | ✅ Passing |
| 5.3 - Test case execution | challenges.integration.test.tsx | ✅ Passing |
| 7.1 - Course organization | courses.integration.test.tsx | ✅ Passing |
| 7.3 - Progress tracking | courses.integration.test.tsx | ✅ Passing |
| 8.1 - Dashboard display | dashboard.integration.test.tsx | ✅ Passing |

## Maintenance

### Adding New Tests

1. Create a new test file in `lib/__tests__/integration/`
2. Follow the naming convention: `[feature].integration.test.tsx`
3. Add the mock directive: `jest.mock('@/lib/api-client');`
4. Import necessary testing utilities and components
5. Write tests following the AAA pattern (Arrange, Act, Assert)

### Updating Existing Tests

When updating components or API contracts:

1. Update the corresponding test file
2. Adjust mock responses to match new API contracts
3. Update assertions to match new UI behavior
4. Run tests to ensure they pass

## Troubleshooting

### Tests Failing Due to API Changes

If API contracts change, update the mock responses in the affected test files to match the new structure.

### Tests Timing Out

Increase the timeout in `jest.config.js` or use `jest.setTimeout()` in specific test files.

### Mock Not Working

Ensure the mock is defined before any imports that use it. Use `jest.mock()` at the top of the test file.

## Future Enhancements

Potential additions to the integration test suite:

- [ ] Project submission and step validation tests
- [ ] AI feedback integration tests
- [ ] Leaderboard display and ranking tests
- [ ] Code execution workflow tests
- [ ] Real-time WebSocket communication tests
- [ ] Responsive design tests for mobile/tablet
- [ ] Accessibility (a11y) tests
- [ ] Performance tests for large datasets

## Summary

**Total Integration Tests:** 47  
**Test Suites:** 4  
**Requirements Validated:** 9  
**Status:** ✅ All Passing

These integration tests provide comprehensive coverage of the frontend user workflows, ensuring that the application behaves correctly from the user's perspective and properly integrates with the backend API.
