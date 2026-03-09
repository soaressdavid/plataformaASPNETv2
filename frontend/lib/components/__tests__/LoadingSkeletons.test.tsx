import React from 'react';
import { render, screen } from '@testing-library/react';
import {
  DashboardSkeleton,
  ChallengesSkeleton,
  CoursesSkeleton,
  CodeExecutionLoader,
  AIFeedbackLoader,
  LoadingSpinner,
  ChallengeDetailSkeleton,
  CourseDetailSkeleton,
  PageLoader,
} from '../LoadingSkeletons';

describe('LoadingSkeletons', () => {
  describe('DashboardSkeleton', () => {
    it('should render dashboard loading skeleton', () => {
      const { container } = render(<DashboardSkeleton />);
      
      // Check for animate-pulse class
      expect(container.querySelector('.animate-pulse')).toBeInTheDocument();
      
      // Check for multiple skeleton sections
      const skeletonSections = container.querySelectorAll('.bg-gray-200, .dark\\:bg-gray-700');
      expect(skeletonSections.length).toBeGreaterThan(0);
    });

    it('should display XP/Level section skeleton', () => {
      const { container } = render(<DashboardSkeleton />);
      
      // Should have progress bar skeleton
      const progressBar = container.querySelector('.rounded-full.h-4');
      expect(progressBar).toBeInTheDocument();
    });

    it('should display stats grid skeleton', () => {
      const { container } = render(<DashboardSkeleton />);
      
      // Should have 3 stat cards
      const statCards = container.querySelectorAll('.md\\:grid-cols-3 > div');
      expect(statCards.length).toBe(3);
    });
  });

  describe('ChallengesSkeleton', () => {
    it('should render challenges loading skeleton', () => {
      const { container } = render(<ChallengesSkeleton />);
      
      expect(container.querySelector('.animate-pulse')).toBeInTheDocument();
    });

    it('should display challenge cards skeleton', () => {
      const { container } = render(<ChallengesSkeleton />);
      
      // Should have 6 challenge card skeletons
      const challengeCards = container.querySelectorAll('.lg\\:grid-cols-3 > div');
      expect(challengeCards.length).toBe(6);
    });

    it('should display filter section skeleton', () => {
      const { container } = render(<ChallengesSkeleton />);
      
      // Should have filter buttons
      const filterButtons = container.querySelectorAll('.h-10.w-20');
      expect(filterButtons.length).toBe(4);
    });
  });

  describe('CoursesSkeleton', () => {
    it('should render courses loading skeleton', () => {
      const { container } = render(<CoursesSkeleton />);
      
      expect(container.querySelector('.animate-pulse')).toBeInTheDocument();
    });

    it('should display course cards skeleton', () => {
      const { container } = render(<CoursesSkeleton />);
      
      // Should have 6 course card skeletons
      const courseCards = container.querySelectorAll('.lg\\:grid-cols-3 > div');
      expect(courseCards.length).toBe(6);
    });
  });

  describe('CodeExecutionLoader', () => {
    it('should render code execution loader', () => {
      render(<CodeExecutionLoader />);
      
      expect(screen.getByText('Executing code...')).toBeInTheDocument();
    });

    it('should display spinner animation', () => {
      const { container } = render(<CodeExecutionLoader />);
      
      const spinner = container.querySelector('.animate-spin');
      expect(spinner).toBeInTheDocument();
    });

    it('should have correct styling for dark background', () => {
      const { container } = render(<CodeExecutionLoader />);
      
      const wrapper = container.querySelector('.bg-gray-900');
      expect(wrapper).toBeInTheDocument();
    });
  });

  describe('AIFeedbackLoader', () => {
    it('should render AI feedback loader', () => {
      render(<AIFeedbackLoader />);
      
      expect(screen.getByText('Analyzing code with AI...')).toBeInTheDocument();
    });

    it('should display spinner animation', () => {
      const { container } = render(<AIFeedbackLoader />);
      
      const spinner = container.querySelector('.animate-spin');
      expect(spinner).toBeInTheDocument();
    });

    it('should have blue-themed styling', () => {
      const { container } = render(<AIFeedbackLoader />);
      
      const wrapper = container.querySelector('.bg-blue-50');
      expect(wrapper).toBeInTheDocument();
    });
  });

  describe('LoadingSpinner', () => {
    it('should render loading spinner with default size', () => {
      const { container } = render(<LoadingSpinner />);
      
      const spinner = container.querySelector('.animate-spin');
      expect(spinner).toBeInTheDocument();
      expect(spinner).toHaveClass('h-8', 'w-8');
    });

    it('should render small spinner when size is sm', () => {
      const { container } = render(<LoadingSpinner size="sm" />);
      
      const spinner = container.querySelector('.animate-spin');
      expect(spinner).toHaveClass('h-4', 'w-4');
    });

    it('should render large spinner when size is lg', () => {
      const { container } = render(<LoadingSpinner size="lg" />);
      
      const spinner = container.querySelector('.animate-spin');
      expect(spinner).toHaveClass('h-12', 'w-12');
    });

    it('should display custom message when provided', () => {
      render(<LoadingSpinner message="Custom loading message" />);
      
      expect(screen.getByText('Custom loading message')).toBeInTheDocument();
    });

    it('should not display message when not provided', () => {
      const { container } = render(<LoadingSpinner />);
      
      const message = container.querySelector('p');
      expect(message).not.toBeInTheDocument();
    });
  });

  describe('ChallengeDetailSkeleton', () => {
    it('should render challenge detail skeleton', () => {
      const { container } = render(<ChallengeDetailSkeleton />);
      
      expect(container.querySelector('.animate-pulse')).toBeInTheDocument();
    });

    it('should display split layout for description and editor', () => {
      const { container } = render(<ChallengeDetailSkeleton />);
      
      // Should have left and right panels
      const panels = container.querySelectorAll('.w-1\\/2');
      expect(panels.length).toBe(2);
    });
  });

  describe('CourseDetailSkeleton', () => {
    it('should render course detail skeleton', () => {
      const { container } = render(<CourseDetailSkeleton />);
      
      expect(container.querySelector('.animate-pulse')).toBeInTheDocument();
    });

    it('should display lesson list skeleton', () => {
      const { container } = render(<CourseDetailSkeleton />);
      
      // Should have 5 lesson skeletons
      const lessonSkeletons = container.querySelectorAll('.space-y-3 > div');
      expect(lessonSkeletons.length).toBe(5);
    });
  });

  describe('PageLoader', () => {
    it('should render page loader with default message', () => {
      render(<PageLoader />);
      
      expect(screen.getByText('Loading...')).toBeInTheDocument();
    });

    it('should render page loader with custom message', () => {
      render(<PageLoader message="Please wait..." />);
      
      expect(screen.getByText('Please wait...')).toBeInTheDocument();
    });

    it('should display centered spinner', () => {
      const { container } = render(<PageLoader />);
      
      const spinner = container.querySelector('.animate-spin');
      expect(spinner).toBeInTheDocument();
      
      // Check for centering classes
      const wrapper = container.querySelector('.flex.items-center.justify-center');
      expect(wrapper).toBeInTheDocument();
    });

    it('should have full screen height', () => {
      const { container } = render(<PageLoader />);
      
      const wrapper = container.querySelector('.min-h-screen');
      expect(wrapper).toBeInTheDocument();
    });
  });

  describe('Loading state transitions', () => {
    it('should transition from loading to content', () => {
      const { rerender, container } = render(<LoadingSpinner message="Loading..." />);
      
      // Initially shows loading
      expect(screen.getByText('Loading...')).toBeInTheDocument();
      
      // Simulate transition to content
      rerender(<div>Content loaded</div>);
      
      // Loading should be gone, content should be visible
      expect(screen.queryByText('Loading...')).not.toBeInTheDocument();
      expect(screen.getByText('Content loaded')).toBeInTheDocument();
    });

    it('should handle multiple loading states', () => {
      const { rerender } = render(<CodeExecutionLoader />);
      
      expect(screen.getByText('Executing code...')).toBeInTheDocument();
      
      rerender(<AIFeedbackLoader />);
      
      expect(screen.queryByText('Executing code...')).not.toBeInTheDocument();
      expect(screen.getByText('Analyzing code with AI...')).toBeInTheDocument();
    });
  });
});
