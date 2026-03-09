import React from 'react';
import { render, screen } from '@testing-library/react';
import { ToastProvider } from '../ToastContext';

// Mock react-hot-toast Toaster component
jest.mock('react-hot-toast', () => ({
  Toaster: ({ children, ...props }: any) => (
    <div data-testid="toaster" {...props}>
      {children}
    </div>
  ),
}));

describe('ToastProvider', () => {
  describe('Provider rendering', () => {
    it('should render children', () => {
      render(
        <ToastProvider>
          <div>Test content</div>
        </ToastProvider>
      );

      expect(screen.getByText('Test content')).toBeInTheDocument();
    });

    it('should render Toaster component', () => {
      render(
        <ToastProvider>
          <div>Test content</div>
        </ToastProvider>
      );

      expect(screen.getByTestId('toaster')).toBeInTheDocument();
    });
  });

  describe('Toaster configuration', () => {
    it('should configure Toaster with correct position', () => {
      const { container } = render(
        <ToastProvider>
          <div>Test content</div>
        </ToastProvider>
      );

      const toaster = container.querySelector('[data-testid="toaster"]');
      expect(toaster).toHaveAttribute('position', 'top-right');
    });

    it('should configure Toaster with gutter spacing', () => {
      const { container } = render(
        <ToastProvider>
          <div>Test content</div>
        </ToastProvider>
      );

      const toaster = container.querySelector('[data-testid="toaster"]');
      expect(toaster).toHaveAttribute('gutter', '8');
    });
  });

  describe('Multiple children', () => {
    it('should render multiple children correctly', () => {
      render(
        <ToastProvider>
          <div>First child</div>
          <div>Second child</div>
          <div>Third child</div>
        </ToastProvider>
      );

      expect(screen.getByText('First child')).toBeInTheDocument();
      expect(screen.getByText('Second child')).toBeInTheDocument();
      expect(screen.getByText('Third child')).toBeInTheDocument();
    });
  });

  describe('Nested providers', () => {
    it('should handle nested content', () => {
      render(
        <ToastProvider>
          <div>
            <div>
              <span>Nested content</span>
            </div>
          </div>
        </ToastProvider>
      );

      expect(screen.getByText('Nested content')).toBeInTheDocument();
    });
  });
});
