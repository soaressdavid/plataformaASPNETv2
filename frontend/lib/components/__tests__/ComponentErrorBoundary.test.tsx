import React from 'react';
import { render, screen } from '@testing-library/react';
import { ComponentErrorBoundary } from '../ComponentErrorBoundary';

// Component that throws an error for testing
const ThrowError = ({ shouldThrow }: { shouldThrow: boolean }) => {
  if (shouldThrow) {
    throw new Error('Component error');
  }
  return <div>Component content</div>;
};

describe('ComponentErrorBoundary', () => {
  // Suppress console.error for cleaner test output
  const originalError = console.error;
  beforeAll(() => {
    console.error = jest.fn();
  });

  afterAll(() => {
    console.error = originalError;
  });

  describe('Error boundary behavior', () => {
    it('should render children when there is no error', () => {
      render(
        <ComponentErrorBoundary>
          <ThrowError shouldThrow={false} />
        </ComponentErrorBoundary>
      );

      expect(screen.getByText('Component content')).toBeInTheDocument();
    });

    it('should catch errors and display inline fallback UI', () => {
      render(
        <ComponentErrorBoundary>
          <ThrowError shouldThrow={true} />
        </ComponentErrorBoundary>
      );

      expect(screen.getByText(/Unable to load component/)).toBeInTheDocument();
      expect(screen.getByText(/Please try refreshing the page/)).toBeInTheDocument();
    });

    it('should display custom component name in error message', () => {
      render(
        <ComponentErrorBoundary componentName="Dashboard Widget">
          <ThrowError shouldThrow={true} />
        </ComponentErrorBoundary>
      );

      expect(screen.getByText(/Unable to load Dashboard Widget/)).toBeInTheDocument();
    });

    it('should use default component name when not provided', () => {
      render(
        <ComponentErrorBoundary>
          <ThrowError shouldThrow={true} />
        </ComponentErrorBoundary>
      );

      expect(screen.getByText(/Unable to load component/)).toBeInTheDocument();
    });
  });

  describe('Fallback UI styling', () => {
    it('should have yellow warning styling', () => {
      const { container } = render(
        <ComponentErrorBoundary>
          <ThrowError shouldThrow={true} />
        </ComponentErrorBoundary>
      );

      const fallbackContainer = container.querySelector('.bg-yellow-50');
      expect(fallbackContainer).toBeInTheDocument();
      
      const border = container.querySelector('.border-yellow-200');
      expect(border).toBeInTheDocument();
    });

    it('should display warning icon', () => {
      const { container } = render(
        <ComponentErrorBoundary>
          <ThrowError shouldThrow={true} />
        </ComponentErrorBoundary>
      );

      const icon = container.querySelector('svg');
      expect(icon).toBeInTheDocument();
      expect(icon).toHaveClass('text-yellow-600');
    });

    it('should be compact and suitable for inline use', () => {
      const { container } = render(
        <ComponentErrorBoundary>
          <ThrowError shouldThrow={true} />
        </ComponentErrorBoundary>
      );

      // Should have padding but not full screen
      const fallback = container.querySelector('.p-4');
      expect(fallback).toBeInTheDocument();
      
      // Should not have min-h-screen class
      const fullScreen = container.querySelector('.min-h-screen');
      expect(fullScreen).not.toBeInTheDocument();
    });
  });

  describe('Multiple component boundaries', () => {
    it('should isolate errors to specific components', () => {
      render(
        <div>
          <ComponentErrorBoundary componentName="Widget 1">
            <ThrowError shouldThrow={true} />
          </ComponentErrorBoundary>
          <ComponentErrorBoundary componentName="Widget 2">
            <ThrowError shouldThrow={false} />
          </ComponentErrorBoundary>
        </div>
      );

      // First component should show error
      expect(screen.getByText(/Unable to load Widget 1/)).toBeInTheDocument();
      
      // Second component should render normally
      expect(screen.getByText('Component content')).toBeInTheDocument();
    });
  });
});
