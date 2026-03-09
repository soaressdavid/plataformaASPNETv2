'use client';

import React from 'react';
import { ErrorBoundary } from './ErrorBoundary';

interface ComponentErrorBoundaryProps {
  children: React.ReactNode;
  componentName?: string;
}

/**
 * Component-level error boundary with a minimal inline fallback UI
 * suitable for use within smaller UI sections like cards or panels.
 * 
 * Validates: Requirements 16.3
 */
export function ComponentErrorBoundary({ 
  children, 
  componentName = 'component' 
}: ComponentErrorBoundaryProps) {
  const fallback = (
    <div className="bg-yellow-50 border border-yellow-200 rounded-md p-4">
      <div className="flex">
        <div className="flex-shrink-0">
          <svg
            className="w-5 h-5 text-yellow-600"
            fill="none"
            stroke="currentColor"
            viewBox="0 0 24 24"
          >
            <path
              strokeLinecap="round"
              strokeLinejoin="round"
              strokeWidth={2}
              d="M12 9v2m0 4h.01m-6.938 4h13.856c1.54 0 2.502-1.667 1.732-3L13.732 4c-.77-1.333-2.694-1.333-3.464 0L3.34 16c-.77 1.333.192 3 1.732 3z"
            />
          </svg>
        </div>
        <div className="ml-3">
          <p className="text-sm text-yellow-800">
            Unable to load {componentName}. Please try refreshing the page.
          </p>
        </div>
      </div>
    </div>
  );

  return (
    <ErrorBoundary fallback={fallback}>
      {children}
    </ErrorBoundary>
  );
}
