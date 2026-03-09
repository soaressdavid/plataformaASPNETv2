import React from 'react';
import { render, screen, fireEvent } from '@testing-library/react';
import { PageErrorBoundary } from '../PageErrorBoundary';

// Component that throws an error for testing
const ThrowError = ({ shouldThrow }: { shouldThrow: boolean }) => {
  if (shouldThrow) {
    throw new Error('Page error');
  }
  return <div>Page content</div>;
};

describe('PageErrorBoundary', () => {
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
        <PageErrorBoundary>
          <ThrowError shouldThrow={false} />
        </PageErrorBoundary>
      );

      expect(screen.getByText('Page content')).toBeInTheDocument();
    });

    it('should catch errors and display page-level fallback UI', () => {
      render(
        <PageErrorBoundary>
          <ThrowError shouldThrow={true} />
        </PageErrorBoundary>
      );

      expect(screen.getByText(/Error loading this page/)).toBeInTheDocument();
      expect(screen.getByText(/An unexpected error occurred/)).toBeInTheDocument();
    });

    it('should display custom page name in error message', () => {
      render(
        <PageErrorBoundary pageName="Dashboard">
          <ThrowError shouldThrow={true} />
        </PageErrorBoundary>
      );

      expect(screen.getByText(/Error loading Dashboard/)).toBeInTheDocument();
    });

    it('should use default page name when not provided', () => {
      render(
        <PageErrorBoundary>
          <ThrowError shouldThrow={true} />
        </PageErrorBoundary>
      );

      expect(screen.getByText(/Error loading this page/)).toBeInTheDocument();
    });
  });

  describe('Fallback UI styling', () => {
    it('should have red error styling', () => {
      const { container } = render(
        <PageErrorBoundary>
          <ThrowError shouldThrow={true} />
        </PageErrorBoundary>
      );

      const fallbackContainer = container.querySelector('.bg-red-50');
      expect(fallbackContainer).toBeInTheDocument();
      
      const border = container.querySelector('.border-red-200');
      expect(border).toBeInTheDocument();
    });

    it('should display error icon', () => {
      const { container } = render(
        <PageErrorBoundary>
          <ThrowError shouldThrow={true} />
        </PageErrorBoundary>
      );

      const icon = container.querySelector('svg');
      expect(icon).toBeInTheDocument();
      expect(icon).toHaveClass('text-red-600');
    });

    it('should be centered with max width', () => {
      const { container } = render(
        <PageErrorBoundary>
          <ThrowError shouldThrow={true} />
        </PageErrorBoundary>
      );

      const wrapper = container.querySelector('.flex.items-center.justify-center');
      expect(wrapper).toBeInTheDocument();
      
      const maxWidth = container.querySelector('.max-w-md');
      expect(maxWidth).toBeInTheDocument();
    });
  });

  describe('Refresh functionality', () => {
    it('should have a refresh button', () => {
      render(
        <PageErrorBoundary>
          <ThrowError shouldThrow={true} />
        </PageErrorBoundary>
      );

      const refreshButton = screen.getByText('Refresh page');
      expect(refreshButton).toBeInTheDocument();
    });

    it('should have clickable refresh button', () => {
      render(
        <PageErrorBoundary>
          <ThrowError shouldThrow={true} />
        </PageErrorBoundary>
      );

      const refreshButton = screen.getByText('Refresh page');
      
      // Verify button is clickable (actual reload is a browser function)
      expect(refreshButton).toBeEnabled();
      fireEvent.click(refreshButton);
      
      // Button should still be in the document after click
      expect(refreshButton).toBeInTheDocument();
    });
  });

  describe('Multiple page boundaries', () => {
    it('should isolate errors to specific page sections', () => {
      render(
        <div>
          <PageErrorBoundary pageName="Section 1">
            <ThrowError shouldThrow={true} />
          </PageErrorBoundary>
          <PageErrorBoundary pageName="Section 2">
            <ThrowError shouldThrow={false} />
          </PageErrorBoundary>
        </div>
      );

      // First section should show error
      expect(screen.getByText(/Error loading Section 1/)).toBeInTheDocument();
      
      // Second section should render normally
      expect(screen.getByText('Page content')).toBeInTheDocument();
    });
  });
});
