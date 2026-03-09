import { render, screen, waitFor } from '@testing-library/react';
import { useRouter, useParams } from 'next/navigation';
import LessonPage from '../page';
import { coursesApi } from '@/lib/api';
import { useAuth } from '@/lib/contexts/AuthContext';
import { useLessonProgress } from '@/lib/hooks/useLessonProgress';
import { useStructuredLesson } from '@/lib/hooks/useStructuredLesson';

// Mock dependencies
jest.mock('next/navigation', () => ({
  useRouter: jest.fn(),
  useParams: jest.fn(),
}));

jest.mock('@/lib/api', () => ({
  coursesApi: {
    getById: jest.fn(),
    getLessons: jest.fn(),
  },
}));

jest.mock('@/lib/contexts/AuthContext', () => ({
  useAuth: jest.fn(),
}));

jest.mock('@/lib/hooks/useLessonProgress', () => ({
  useLessonProgress: jest.fn(),
}));

jest.mock('@/lib/hooks/useStructuredLesson', () => ({
  useStructuredLesson: jest.fn(),
}));

jest.mock('@/lib/hooks/useAIFeedback', () => ({
  useAIFeedback: jest.fn(() => ({
    feedback: null,
    isLoading: false,
    getFeedback: jest.fn(),
    clearFeedback: jest.fn(),
  })),
}));

describe('LessonPage', () => {
  const mockRouter = {
    push: jest.fn(),
  };

  const mockCourse = {
    id: 'course-1',
    title: 'Test Course',
    description: 'Test Description',
    level: 'Beginner',
    levelId: 'level-1',
    levelTitle: 'Level 1',
    lessonCount: 5,
    lessons: [],
    orderIndex: 1,
  };

  const mockLesson = {
    id: 'lesson-1',
    title: 'Test Lesson',
    content: '<p>Test content</p>',
    order: 1,
    isCompleted: false,
    duration: '30 min',
    difficulty: 'Fácil',
    estimatedMinutes: 30,
  };

  beforeEach(() => {
    jest.clearAllMocks();
    (useRouter as jest.Mock).mockReturnValue(mockRouter);
    (useParams as jest.Mock).mockReturnValue({ id: 'course-1', lessonId: 'lesson-1' });
    (useAuth as jest.Mock).mockReturnValue({ user: { userId: 'user-1' } });
    (useLessonProgress as jest.Mock).mockReturnValue({
      markLessonComplete: jest.fn(),
      isLessonComplete: jest.fn(() => false),
    });
    (useStructuredLesson as jest.Mock).mockReturnValue({
      lesson: mockLesson,
      loading: false,
      error: null,
      contentType: 'html',
    });
    (coursesApi.getById as jest.Mock).mockResolvedValue(mockCourse);
    (coursesApi.getLessons as jest.Mock).mockResolvedValue({
      lessons: [mockLesson],
    });
  });

  it('renders lesson page with breadcrumb', async () => {
    render(<LessonPage />);

    await waitFor(() => {
      expect(screen.getByText('Test Lesson')).toBeInTheDocument();
    });

    // Check breadcrumb items
    expect(screen.getByText('Níveis')).toBeInTheDocument();
    expect(screen.getByText('Level 1')).toBeInTheDocument();
    expect(screen.getByText('Test Course')).toBeInTheDocument();
  });

  it('displays lesson metadata', async () => {
    render(<LessonPage />);

    await waitFor(() => {
      expect(screen.getByText('Test Lesson')).toBeInTheDocument();
    });

    expect(screen.getByText('Aula 1')).toBeInTheDocument();
    expect(screen.getByText('Fácil')).toBeInTheDocument();
    expect(screen.getByText('30 min')).toBeInTheDocument();
    expect(screen.getByText('30 minutos')).toBeInTheDocument();
  });

  it('uses LessonContent component', async () => {
    render(<LessonPage />);

    await waitFor(() => {
      expect(screen.getByText('Test Lesson')).toBeInTheDocument();
    });

    // The LessonContent component should render the HTML content
    expect(screen.getByText('Test content')).toBeInTheDocument();
  });

  it('shows loading state', () => {
    (useStructuredLesson as jest.Mock).mockReturnValue({
      lesson: null,
      loading: true,
      error: null,
      contentType: null,
    });

    render(<LessonPage />);

    expect(screen.getByText('Carregando aula...')).toBeInTheDocument();
  });

  it('shows error state', () => {
    (useStructuredLesson as jest.Mock).mockReturnValue({
      lesson: null,
      loading: false,
      error: 'Failed to load lesson',
      contentType: null,
    });

    render(<LessonPage />);

    expect(screen.getByText('Failed to load lesson')).toBeInTheDocument();
  });

  it('renders structured content when available', async () => {
    const structuredLesson = {
      ...mockLesson,
      content: undefined,
      structuredContent: {
        objectives: ['Learn basics', 'Practice coding'],
        theory: [],
        codeExamples: [],
        exercises: [],
        summary: 'Test summary',
      },
    };

    (useStructuredLesson as jest.Mock).mockReturnValue({
      lesson: structuredLesson,
      loading: false,
      error: null,
      contentType: 'structured',
    });

    render(<LessonPage />);

    await waitFor(() => {
      expect(screen.getByText('Test Lesson')).toBeInTheDocument();
    });

    // The StructuredLessonView should render objectives
    expect(screen.getByText('Learn basics')).toBeInTheDocument();
    expect(screen.getByText('Practice coding')).toBeInTheDocument();
  });
});
