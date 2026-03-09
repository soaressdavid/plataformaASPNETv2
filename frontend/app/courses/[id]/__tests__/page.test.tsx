import React from 'react';
import { render, screen, waitFor } from '@testing-library/react';
import { useRouter, useParams } from 'next/navigation';
import CourseDetailPage from '../page';
import { coursesApi } from '@/lib/api/courses';
import { useLessonProgress } from '@/lib/hooks/useLessonProgress';

// Mock dependencies
jest.mock('next/navigation', () => ({
  useRouter: jest.fn(),
  useParams: jest.fn(),
}));

jest.mock('@/lib/api/courses');
jest.mock('@/lib/hooks/useLessonProgress');
jest.mock('@/lib/components', () => ({
  Navigation: () => <div data-testid="navigation">Navigation</div>,
  CourseDetailSkeleton: () => <div data-testid="skeleton">Loading...</div>,
  Breadcrumb: ({ items }: { items: Array<{ label: string; href?: string }> }) => (
    <nav data-testid="breadcrumb">
      {items.map((item, i) => (
        <span key={i}>{item.label}</span>
      ))}
    </nav>
  ),
}));

jest.mock('@/lib/components/Icons', () => ({
  Icons: {
    CheckCircle: () => <span>✓</span>,
    Clock: ({ className }: { className?: string }) => <span className={className}>🕐</span>,
    ChevronRight: ({ className }: { className?: string }) => <span className={className}>›</span>,
  },
}));

describe('CourseDetailPage', () => {
  const mockRouter = {
    push: jest.fn(),
  };

  const mockCourseData = {
    id: 'course-1',
    title: 'Test Course',
    description: 'Test course description',
    level: 'Beginner' as const,
    levelId: 'level-1',
    levelTitle: 'Level 1: Basics',
    duration: '4 hours',
    lessonCount: 2,
    topics: ['Topic 1', 'Topic 2'],
    orderIndex: 1,
    lessons: [],
  };

  const mockLessons = [
    {
      id: 'lesson-1',
      title: 'Lesson 1',
      order: 1,
      isCompleted: false,
      duration: '30 min',
      difficulty: 'Easy',
      estimatedMinutes: 30,
    },
    {
      id: 'lesson-2',
      title: 'Lesson 2',
      order: 2,
      isCompleted: true,
      duration: '45 min',
      difficulty: 'Medium',
      estimatedMinutes: 45,
    },
  ];

  beforeEach(() => {
    jest.clearAllMocks();
    (useRouter as jest.Mock).mockReturnValue(mockRouter);
    (useParams as jest.Mock).mockReturnValue({ id: 'course-1' });
    (useLessonProgress as jest.Mock).mockReturnValue({
      isLessonComplete: jest.fn((courseId: string, lessonId: string) => lessonId === 'lesson-2'),
      getCourseProgress: jest.fn(),
    });
  });

  it('renders loading skeleton initially', () => {
    (coursesApi.getById as jest.Mock).mockImplementation(() => new Promise(() => {}));
    (coursesApi.getLessons as jest.Mock).mockImplementation(() => new Promise(() => {}));

    render(<CourseDetailPage />);
    expect(screen.getByTestId('skeleton')).toBeInTheDocument();
  });

  it('renders course details with level information', async () => {
    (coursesApi.getById as jest.Mock).mockResolvedValue(mockCourseData);
    (coursesApi.getLessons as jest.Mock).mockResolvedValue({ lessons: mockLessons });

    render(<CourseDetailPage />);

    await waitFor(() => {
      expect(screen.getByRole('heading', { name: 'Test Course' })).toBeInTheDocument();
    });

    expect(screen.getByText('Test course description')).toBeInTheDocument();
    expect(screen.getAllByText('Level 1: Basics').length).toBeGreaterThan(0);
    expect(screen.getByText('Beginner')).toBeInTheDocument();
    expect(screen.getByText('4 hours')).toBeInTheDocument();
  });

  it('renders breadcrumb with level navigation', async () => {
    (coursesApi.getById as jest.Mock).mockResolvedValue(mockCourseData);
    (coursesApi.getLessons as jest.Mock).mockResolvedValue({ lessons: mockLessons });

    render(<CourseDetailPage />);

    await waitFor(() => {
      expect(screen.getByTestId('breadcrumb')).toBeInTheDocument();
    });

    const breadcrumb = screen.getByTestId('breadcrumb');
    expect(breadcrumb).toHaveTextContent('Níveis');
    expect(breadcrumb).toHaveTextContent('Level 1: Basics');
    expect(breadcrumb).toHaveTextContent('Test Course');
  });

  it('does not render breadcrumb when level information is missing', async () => {
    const courseWithoutLevel = {
      ...mockCourseData,
      levelId: undefined,
      levelTitle: undefined,
    };
    (coursesApi.getById as jest.Mock).mockResolvedValue(courseWithoutLevel);
    (coursesApi.getLessons as jest.Mock).mockResolvedValue({ lessons: mockLessons });

    render(<CourseDetailPage />);

    await waitFor(() => {
      expect(screen.getByText('Test Course')).toBeInTheDocument();
    });

    expect(screen.queryByTestId('breadcrumb')).not.toBeInTheDocument();
  });

  it('renders lessons list with progress', async () => {
    (coursesApi.getById as jest.Mock).mockResolvedValue(mockCourseData);
    (coursesApi.getLessons as jest.Mock).mockResolvedValue({ lessons: mockLessons });

    render(<CourseDetailPage />);

    await waitFor(() => {
      expect(screen.getByText('Lesson 1')).toBeInTheDocument();
    });

    expect(screen.getByText('Lesson 2')).toBeInTheDocument();
    expect(screen.getByText('50%')).toBeInTheDocument(); // 1 of 2 completed
    expect(screen.getByText('1 de 2 aulas concluídas')).toBeInTheDocument();
  });

  it('level badge links to level page', async () => {
    (coursesApi.getById as jest.Mock).mockResolvedValue(mockCourseData);
    (coursesApi.getLessons as jest.Mock).mockResolvedValue({ lessons: mockLessons });

    render(<CourseDetailPage />);

    await waitFor(() => {
      expect(screen.getByRole('heading', { name: 'Test Course' })).toBeInTheDocument();
    });

    const levelBadges = screen.getAllByText('Level 1: Basics');
    // Find the one that's a link (not in breadcrumb)
    const levelBadge = levelBadges.find(el => el.closest('a')?.getAttribute('href') === '/levels/level-1');
    expect(levelBadge).toBeDefined();
    expect(levelBadge?.closest('a')).toHaveAttribute('href', '/levels/level-1');
  });

  it('handles error state', async () => {
    (coursesApi.getById as jest.Mock).mockRejectedValue(new Error('API Error'));
    (coursesApi.getLessons as jest.Mock).mockRejectedValue(new Error('API Error'));

    render(<CourseDetailPage />);

    await waitFor(() => {
      expect(screen.getByText('Failed to load course. Please try again later.')).toBeInTheDocument();
    });
  });

  it('displays course metadata correctly', async () => {
    (coursesApi.getById as jest.Mock).mockResolvedValue(mockCourseData);
    (coursesApi.getLessons as jest.Mock).mockResolvedValue({ lessons: mockLessons });

    render(<CourseDetailPage />);

    await waitFor(() => {
      expect(screen.getByRole('heading', { name: 'Test Course' })).toBeInTheDocument();
    });

    // Check for duration with clock icon
    expect(screen.getByText('4 hours')).toBeInTheDocument();
    
    // Check for lesson count
    expect(screen.getByText('2 aulas')).toBeInTheDocument();
  });
});
