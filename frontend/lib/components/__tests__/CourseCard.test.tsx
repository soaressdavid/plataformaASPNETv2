import { render, screen } from '@testing-library/react';
import { CourseCard } from '../CourseCard';
import { CourseSummary, Level } from '../../types';

describe('CourseCard', () => {
  const mockCourse: CourseSummary = {
    id: 'course-1',
    title: 'Introduction to ASP.NET Core',
    description: 'Learn the fundamentals of ASP.NET Core web development',
    level: Level.Beginner,
    levelId: 'level-0',
    duration: '4 horas',
    lessonCount: 8,
    topics: ['ASP.NET Core', 'Web API', 'MVC'],
    orderIndex: 1,
  };

  it('renders course title', () => {
    render(<CourseCard course={mockCourse} />);
    expect(screen.getByText('Introduction to ASP.NET Core')).toBeInTheDocument();
  });

  it('renders course description', () => {
    render(<CourseCard course={mockCourse} />);
    expect(
      screen.getByText('Learn the fundamentals of ASP.NET Core web development')
    ).toBeInTheDocument();
  });

  it('renders level badge with correct text', () => {
    render(<CourseCard course={mockCourse} />);
    expect(screen.getByText('Iniciante')).toBeInTheDocument();
  });

  it('renders lesson count', () => {
    render(<CourseCard course={mockCourse} />);
    expect(screen.getByText('8 lições')).toBeInTheDocument();
  });

  it('renders singular lesson text when count is 1', () => {
    const singleLessonCourse = { ...mockCourse, lessonCount: 1 };
    render(<CourseCard course={singleLessonCourse} />);
    expect(screen.getByText('1 lição')).toBeInTheDocument();
  });

  it('renders duration when provided', () => {
    render(<CourseCard course={mockCourse} />);
    expect(screen.getByText('4 horas')).toBeInTheDocument();
  });

  it('does not render duration when not provided', () => {
    const courseWithoutDuration = { ...mockCourse, duration: undefined };
    render(<CourseCard course={courseWithoutDuration} />);
    expect(screen.queryByText('4 horas')).not.toBeInTheDocument();
  });

  it('renders up to 3 topic tags', () => {
    render(<CourseCard course={mockCourse} />);
    expect(screen.getByText('ASP.NET Core')).toBeInTheDocument();
    expect(screen.getByText('Web API')).toBeInTheDocument();
    expect(screen.getByText('MVC')).toBeInTheDocument();
  });

  it('shows "+X mais" when more than 3 topics', () => {
    const courseWithManyTopics = {
      ...mockCourse,
      topics: ['Topic 1', 'Topic 2', 'Topic 3', 'Topic 4', 'Topic 5'],
    };
    render(<CourseCard course={courseWithManyTopics} />);
    expect(screen.getByText('+2 mais')).toBeInTheDocument();
  });

  it('does not render topics section when no topics', () => {
    const courseWithoutTopics = { ...mockCourse, topics: undefined };
    render(<CourseCard course={courseWithoutTopics} />);
    expect(screen.queryByText('ASP.NET Core')).not.toBeInTheDocument();
  });

  it('renders as a link to course detail page', () => {
    render(<CourseCard course={mockCourse} />);
    const link = screen.getByRole('link');
    expect(link).toHaveAttribute('href', '/courses/course-1');
  });

  it('applies correct level badge classes for Beginner', () => {
    render(<CourseCard course={mockCourse} />);
    const badge = screen.getByText('Iniciante');
    expect(badge).toHaveClass('bg-green-100', 'text-green-700', 'border-green-200');
  });

  it('applies correct level badge classes for Intermediate', () => {
    const intermediateCourse = { ...mockCourse, level: Level.Intermediate };
    render(<CourseCard course={intermediateCourse} />);
    const badge = screen.getByText('Intermediário');
    expect(badge).toHaveClass('bg-yellow-100', 'text-yellow-700', 'border-yellow-200');
  });

  it('applies correct level badge classes for Advanced', () => {
    const advancedCourse = { ...mockCourse, level: Level.Advanced };
    render(<CourseCard course={advancedCourse} />);
    const badge = screen.getByText('Avançado');
    expect(badge).toHaveClass('bg-red-100', 'text-red-700', 'border-red-200');
  });

  it('handles empty topics array', () => {
    const courseWithEmptyTopics = { ...mockCourse, topics: [] };
    render(<CourseCard course={courseWithEmptyTopics} />);
    expect(screen.queryByText('ASP.NET Core')).not.toBeInTheDocument();
  });
});
