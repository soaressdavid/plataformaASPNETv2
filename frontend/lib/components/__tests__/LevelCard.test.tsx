import React from 'react';
import { render, screen } from '@testing-library/react';
import { LevelCard } from '../LevelCard';
import { CurriculumLevel } from '../../types';

// Mock Next.js Link component
jest.mock('next/link', () => {
  const MockLink = ({ children, href, className }: { children: React.ReactNode; href: string; className?: string }) => {
    return <a href={href} className={className}>{children}</a>;
  };
  MockLink.displayName = 'MockLink';
  return MockLink;
});

describe('LevelCard', () => {
  const mockLevel: CurriculumLevel = {
    id: 'level-1',
    number: 0,
    title: 'Fundamentos de C#',
    description: 'Aprenda os conceitos básicos da linguagem C# e programação orientada a objetos.',
    requiredXP: 0,
    courseCount: 5,
    estimatedHours: 20,
  };

  it('should render level information correctly', () => {
    render(<LevelCard level={mockLevel} />);
    
    expect(screen.getByText('Fundamentos de C#')).toBeInTheDocument();
    expect(screen.getByText(/Aprenda os conceitos básicos/)).toBeInTheDocument();
    expect(screen.getByText('5 cursos')).toBeInTheDocument();
    expect(screen.getByText('20h estimadas')).toBeInTheDocument();
  });

  it('should display level number in badge', () => {
    render(<LevelCard level={mockLevel} />);
    
    expect(screen.getByText('0')).toBeInTheDocument();
  });

  it('should render as a link when not locked', () => {
    const { container } = render(<LevelCard level={mockLevel} />);
    
    const link = container.querySelector('a[href="/levels/level-1"]');
    expect(link).toBeInTheDocument();
  });

  it('should render as div when locked', () => {
    const { container } = render(<LevelCard level={mockLevel} isLocked={true} />);
    
    const link = container.querySelector('a');
    expect(link).not.toBeInTheDocument();
    
    const div = container.querySelector('div.cursor-not-allowed');
    expect(div).toBeInTheDocument();
  });

  it('should show lock overlay when locked', () => {
    render(<LevelCard level={mockLevel} isLocked={true} />);
    
    expect(screen.getByText('Requer 0 XP')).toBeInTheDocument();
  });

  it('should display lock icon when locked', () => {
    const { container } = render(<LevelCard level={mockLevel} isLocked={true} />);
    
    // Check for lock icons (there should be 2: one in header, one in overlay)
    const lockIcons = container.querySelectorAll('svg');
    expect(lockIcons.length).toBeGreaterThan(0);
  });

  it('should show progress bar when progress is provided', () => {
    const { container } = render(<LevelCard level={mockLevel} progress={45} />);
    
    expect(screen.getByText('Progresso')).toBeInTheDocument();
    expect(screen.getByText('45%')).toBeInTheDocument();
    
    const progressBar = container.querySelector('.bg-blue-500');
    expect(progressBar).toHaveStyle({ width: '45%' });
  });

  it('should not show progress bar when locked', () => {
    render(<LevelCard level={mockLevel} isLocked={true} progress={45} />);
    
    expect(screen.queryByText('Progresso')).not.toBeInTheDocument();
  });

  it('should not show progress bar when progress is undefined', () => {
    render(<LevelCard level={mockLevel} />);
    
    expect(screen.queryByText('Progresso')).not.toBeInTheDocument();
  });

  it('should handle 0% progress', () => {
    const { container } = render(<LevelCard level={mockLevel} progress={0} />);
    
    expect(screen.getByText('0%')).toBeInTheDocument();
    
    const progressBar = container.querySelector('.bg-blue-500');
    expect(progressBar).toHaveStyle({ width: '0%' });
  });

  it('should handle 100% progress', () => {
    const { container } = render(<LevelCard level={mockLevel} progress={100} />);
    
    expect(screen.getByText('100%')).toBeInTheDocument();
    
    const progressBar = container.querySelector('.bg-blue-500');
    expect(progressBar).toHaveStyle({ width: '100%' });
  });

  it('should clamp progress above 100%', () => {
    const { container } = render(<LevelCard level={mockLevel} progress={150} />);
    
    const progressBar = container.querySelector('.bg-blue-500');
    expect(progressBar).toHaveStyle({ width: '100%' });
  });

  it('should clamp negative progress to 0%', () => {
    const { container } = render(<LevelCard level={mockLevel} progress={-10} />);
    
    const progressBar = container.querySelector('.bg-blue-500');
    expect(progressBar).toHaveStyle({ width: '0%' });
  });

  it('should display singular "curso" for 1 course', () => {
    const singleCourseLevel = { ...mockLevel, courseCount: 1 };
    render(<LevelCard level={singleCourseLevel} />);
    
    expect(screen.getByText('1 curso')).toBeInTheDocument();
  });

  it('should display plural "cursos" for multiple courses', () => {
    render(<LevelCard level={mockLevel} />);
    
    expect(screen.getByText('5 cursos')).toBeInTheDocument();
  });

  it('should have hover styles when not locked', () => {
    const { container } = render(<LevelCard level={mockLevel} />);
    
    const card = container.querySelector('.hover\\:border-blue-400');
    expect(card).toBeInTheDocument();
  });

  it('should not have hover styles when locked', () => {
    const { container } = render(<LevelCard level={mockLevel} isLocked={true} />);
    
    const card = container.querySelector('.cursor-not-allowed');
    expect(card).toBeInTheDocument();
  });

  it('should render book and clock icons', () => {
    const { container } = render(<LevelCard level={mockLevel} />);
    
    // Check for SVG icons
    const icons = container.querySelectorAll('svg');
    expect(icons.length).toBeGreaterThan(0);
  });

  it('should handle long descriptions with line clamp', () => {
    const longDescLevel = {
      ...mockLevel,
      description: 'Esta é uma descrição muito longa que deve ser truncada com line-clamp-2 para evitar que o card fique muito alto e mantenha um layout consistente entre todos os cards.',
    };
    
    const { container } = render(<LevelCard level={longDescLevel} />);
    
    const description = container.querySelector('.line-clamp-2');
    expect(description).toBeInTheDocument();
  });

  it('should display correct XP requirement in lock overlay', () => {
    const highXPLevel = { ...mockLevel, requiredXP: 500 };
    render(<LevelCard level={highXPLevel} isLocked={true} />);
    
    expect(screen.getByText('Requer 500 XP')).toBeInTheDocument();
  });

  it('should have proper styling classes for unlocked state', () => {
    const { container } = render(<LevelCard level={mockLevel} />);
    
    const card = container.querySelector('.border-blue-200');
    expect(card).toBeInTheDocument();
    
    const bgWhite = container.querySelector('.bg-white');
    expect(bgWhite).toBeInTheDocument();
  });

  it('should have proper styling classes for locked state', () => {
    const { container } = render(<LevelCard level={mockLevel} isLocked={true} />);
    
    const card = container.querySelector('.border-gray-300');
    expect(card).toBeInTheDocument();
    
    const bgGray = container.querySelector('.bg-gray-50');
    expect(bgGray).toBeInTheDocument();
  });

  it('should render level with high number', () => {
    const highLevelNumber = { ...mockLevel, number: 15 };
    render(<LevelCard level={highLevelNumber} />);
    
    expect(screen.getByText('15')).toBeInTheDocument();
  });

  it('should handle level with many courses', () => {
    const manyCourses = { ...mockLevel, courseCount: 25 };
    render(<LevelCard level={manyCourses} />);
    
    expect(screen.getByText('25 cursos')).toBeInTheDocument();
  });

  it('should handle level with many estimated hours', () => {
    const manyHours = { ...mockLevel, estimatedHours: 100 };
    render(<LevelCard level={manyHours} />);
    
    expect(screen.getByText('100h estimadas')).toBeInTheDocument();
  });
});
