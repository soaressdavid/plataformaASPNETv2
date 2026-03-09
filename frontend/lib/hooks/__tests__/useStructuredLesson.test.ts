import { renderHook, waitFor } from '@testing-library/react';
import { useStructuredLesson } from '../useStructuredLesson';
import { coursesApi } from '../../api';
import { LessonDetail, LessonContent } from '../../types';

// Mock the API
jest.mock('../../api', () => ({
  coursesApi: {
    getLesson: jest.fn(),
  },
}));

describe('useStructuredLesson', () => {
  beforeEach(() => {
    jest.clearAllMocks();
  });

  it('should fetch lesson with structured content and detect content type', async () => {
    const mockStructuredContent: LessonContent = {
      objectives: ['Learn TypeScript', 'Understand hooks'],
      theory: [
        {
          heading: 'Introduction',
          content: 'This is the theory content',
          order: 1,
        },
      ],
      codeExamples: [
        {
          title: 'Example 1',
          code: 'const x = 1;',
          language: 'typescript',
          explanation: 'Simple variable declaration',
          isRunnable: true,
        },
      ],
      exercises: [
        {
          title: 'Exercise 1',
          description: 'Practice TypeScript',
          difficulty: 'Fácil',
          starterCode: '// Start here',
          hints: ['Hint 1', 'Hint 2'],
        },
      ],
      summary: 'This is the summary',
    };

    const mockLesson: LessonDetail = {
      id: 'lesson-1',
      title: 'Introduction to TypeScript',
      structuredContent: mockStructuredContent,
      duration: '30 minutes',
      difficulty: 'Easy',
      estimatedMinutes: 30,
      order: 1,
      isCompleted: false,
      prerequisites: ['Basic JavaScript'],
    };

    (coursesApi.getLesson as jest.Mock).mockResolvedValue(mockLesson);

    const { result } = renderHook(() => useStructuredLesson('course-1', 'lesson-1'));

    // Initially loading
    expect(result.current.loading).toBe(true);
    expect(result.current.lesson).toBeNull();
    expect(result.current.error).toBeNull();
    expect(result.current.contentType).toBeNull();

    // Wait for the hook to finish loading
    await waitFor(() => {
      expect(result.current.loading).toBe(false);
    });

    // Check final state
    expect(result.current.lesson).toEqual(mockLesson);
    expect(result.current.error).toBeNull();
    expect(result.current.contentType).toBe('structured');
    expect(coursesApi.getLesson).toHaveBeenCalledWith('course-1', 'lesson-1');
    expect(coursesApi.getLesson).toHaveBeenCalledTimes(1);
  });

  it('should fetch lesson with HTML content and detect content type', async () => {
    const mockLesson: LessonDetail = {
      id: 'lesson-2',
      title: 'Legacy Lesson',
      content: '<h1>HTML Content</h1><p>This is legacy HTML content</p>',
      duration: '20 minutes',
      difficulty: 'Medium',
      estimatedMinutes: 20,
      order: 2,
      isCompleted: true,
    };

    (coursesApi.getLesson as jest.Mock).mockResolvedValue(mockLesson);

    const { result } = renderHook(() => useStructuredLesson('course-1', 'lesson-2'));

    await waitFor(() => {
      expect(result.current.loading).toBe(false);
    });

    expect(result.current.lesson).toEqual(mockLesson);
    expect(result.current.error).toBeNull();
    expect(result.current.contentType).toBe('html');
  });

  it('should prefer structured content over HTML content', async () => {
    const mockStructuredContent: LessonContent = {
      objectives: ['Learn React'],
      theory: [],
      codeExamples: [],
      exercises: [],
      summary: 'Summary',
    };

    const mockLesson: LessonDetail = {
      id: 'lesson-3',
      title: 'Hybrid Lesson',
      content: '<h1>HTML Content</h1>',
      structuredContent: mockStructuredContent,
      duration: '25 minutes',
      difficulty: 'Hard',
      estimatedMinutes: 25,
      order: 3,
      isCompleted: false,
    };

    (coursesApi.getLesson as jest.Mock).mockResolvedValue(mockLesson);

    const { result } = renderHook(() => useStructuredLesson('course-1', 'lesson-3'));

    await waitFor(() => {
      expect(result.current.loading).toBe(false);
    });

    expect(result.current.lesson).toEqual(mockLesson);
    expect(result.current.contentType).toBe('structured');
  });

  it('should set contentType to null when no content is available', async () => {
    const mockLesson: LessonDetail = {
      id: 'lesson-4',
      title: 'Empty Lesson',
      duration: '15 minutes',
      estimatedMinutes: 15,
      order: 4,
      isCompleted: false,
    };

    (coursesApi.getLesson as jest.Mock).mockResolvedValue(mockLesson);

    const { result } = renderHook(() => useStructuredLesson('course-1', 'lesson-4'));

    await waitFor(() => {
      expect(result.current.loading).toBe(false);
    });

    expect(result.current.lesson).toEqual(mockLesson);
    expect(result.current.contentType).toBeNull();
  });

  it('should handle error when fetching lesson fails', async () => {
    const errorMessage = 'Lesson not found';
    (coursesApi.getLesson as jest.Mock).mockRejectedValue(new Error(errorMessage));

    const { result } = renderHook(() => useStructuredLesson('course-1', 'invalid-lesson'));

    await waitFor(() => {
      expect(result.current.loading).toBe(false);
    });

    expect(result.current.lesson).toBeNull();
    expect(result.current.error).toBe(errorMessage);
    expect(result.current.contentType).toBeNull();
  });

  it('should handle non-Error exceptions', async () => {
    (coursesApi.getLesson as jest.Mock).mockRejectedValue('String error');

    const { result } = renderHook(() => useStructuredLesson('course-1', 'lesson-1'));

    await waitFor(() => {
      expect(result.current.loading).toBe(false);
    });

    expect(result.current.error).toBe('Failed to load lesson');
    expect(result.current.contentType).toBeNull();
  });

  it('should refetch when courseId or lessonId changes', async () => {
    const mockLesson1: LessonDetail = {
      id: 'lesson-1',
      title: 'Lesson 1',
      content: '<p>Content 1</p>',
      order: 1,
      isCompleted: false,
    };

    const mockLesson2: LessonDetail = {
      id: 'lesson-2',
      title: 'Lesson 2',
      content: '<p>Content 2</p>',
      order: 2,
      isCompleted: false,
    };

    (coursesApi.getLesson as jest.Mock)
      .mockResolvedValueOnce(mockLesson1)
      .mockResolvedValueOnce(mockLesson2);

    const { result, rerender } = renderHook(
      ({ courseId, lessonId }) => useStructuredLesson(courseId, lessonId),
      {
        initialProps: { courseId: 'course-1', lessonId: 'lesson-1' },
      }
    );

    await waitFor(() => {
      expect(result.current.loading).toBe(false);
    });

    expect(result.current.lesson).toEqual(mockLesson1);
    expect(result.current.contentType).toBe('html');

    // Change the lessonId
    rerender({ courseId: 'course-1', lessonId: 'lesson-2' });

    // Should be loading again
    expect(result.current.loading).toBe(true);

    await waitFor(() => {
      expect(result.current.loading).toBe(false);
    });

    expect(result.current.lesson).toEqual(mockLesson2);
    expect(result.current.contentType).toBe('html');
    expect(coursesApi.getLesson).toHaveBeenCalledTimes(2);
    expect(coursesApi.getLesson).toHaveBeenNthCalledWith(1, 'course-1', 'lesson-1');
    expect(coursesApi.getLesson).toHaveBeenNthCalledWith(2, 'course-1', 'lesson-2');
  });

  it('should not fetch if courseId or lessonId is empty', () => {
    const { result: result1 } = renderHook(() => useStructuredLesson('', 'lesson-1'));
    expect(result1.current.loading).toBe(true);
    expect(coursesApi.getLesson).not.toHaveBeenCalled();

    jest.clearAllMocks();

    const { result: result2 } = renderHook(() => useStructuredLesson('course-1', ''));
    expect(result2.current.loading).toBe(true);
    expect(coursesApi.getLesson).not.toHaveBeenCalled();

    jest.clearAllMocks();

    const { result: result3 } = renderHook(() => useStructuredLesson('', ''));
    expect(result3.current.loading).toBe(true);
    expect(coursesApi.getLesson).not.toHaveBeenCalled();
  });

  it('should reset error state on successful refetch', async () => {
    const errorMessage = 'Network error';
    (coursesApi.getLesson as jest.Mock).mockRejectedValueOnce(new Error(errorMessage));

    const { result, rerender } = renderHook(
      ({ courseId, lessonId }) => useStructuredLesson(courseId, lessonId),
      {
        initialProps: { courseId: 'course-1', lessonId: 'lesson-1' },
      }
    );

    await waitFor(() => {
      expect(result.current.loading).toBe(false);
    });

    expect(result.current.error).toBe(errorMessage);

    // Now mock a successful response
    const mockLesson: LessonDetail = {
      id: 'lesson-1',
      title: 'Lesson 1',
      content: '<p>Content</p>',
      order: 1,
      isCompleted: false,
    };

    (coursesApi.getLesson as jest.Mock).mockResolvedValueOnce(mockLesson);

    // Trigger refetch by changing lessonId
    rerender({ courseId: 'course-1', lessonId: 'lesson-2' });

    await waitFor(() => {
      expect(result.current.loading).toBe(false);
    });

    expect(result.current.error).toBeNull();
    expect(result.current.lesson).toEqual(mockLesson);
  });
});
