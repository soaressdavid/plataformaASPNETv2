import React from 'react';
import { render, screen, fireEvent } from '@testing-library/react';
import '@testing-library/jest-dom';
import { AIFeedback } from '../AIFeedback';
import { FeedbackType } from '../../../types';

describe('AIFeedback Component', () => {
  const mockOnClose = jest.fn();

  const mockSuggestions = [
    {
      type: FeedbackType.Security,
      message: 'Potential SQL injection vulnerability',
      lineNumber: 15,
      codeExample: 'Use parameterized queries instead',
    },
    {
      type: FeedbackType.Performance,
      message: 'Consider using async/await for better performance',
      lineNumber: 23,
      codeExample: 'public async Task<Result> GetDataAsync()',
    },
    {
      type: FeedbackType.BestPractice,
      message: 'Follow naming conventions for methods',
      lineNumber: 8,
      codeExample: 'public void ProcessData() // PascalCase',
    },
    {
      type: FeedbackType.Architecture,
      message: 'Consider using dependency injection',
      lineNumber: 42,
      codeExample: 'public MyService(IRepository repository)',
    },
  ];

  const mockSecurityIssues = ['SQL injection vulnerability'];
  const mockPerformanceIssues = ['Synchronous database calls'];

  beforeEach(() => {
    mockOnClose.mockClear();
  });

  it('renders loading state correctly', () => {
    render(
      <AIFeedback
        suggestions={[]}
        overallScore={0}
        securityIssues={[]}
        performanceIssues={[]}
        isLoading={true}
        onClose={mockOnClose}
      />
    );

    expect(screen.getByText('Analyzing your code...')).toBeInTheDocument();
  });

  it('renders feedback with overall score', () => {
    render(
      <AIFeedback
        suggestions={mockSuggestions}
        overallScore={75}
        securityIssues={mockSecurityIssues}
        performanceIssues={mockPerformanceIssues}
        isLoading={false}
        onClose={mockOnClose}
      />
    );

    expect(screen.getByText('75/100')).toBeInTheDocument();
    expect(screen.getByText('Good')).toBeInTheDocument();
  });

  it('displays categorized suggestions', () => {
    render(
      <AIFeedback
        suggestions={mockSuggestions}
        overallScore={75}
        securityIssues={mockSecurityIssues}
        performanceIssues={mockPerformanceIssues}
        isLoading={false}
        onClose={mockOnClose}
      />
    );

    expect(screen.getByText('Security')).toBeInTheDocument();
    expect(screen.getByText('Performance')).toBeInTheDocument();
    expect(screen.getByText('BestPractice')).toBeInTheDocument();
    expect(screen.getByText('Architecture')).toBeInTheDocument();
  });

  it('displays line numbers for each suggestion', () => {
    render(
      <AIFeedback
        suggestions={mockSuggestions}
        overallScore={75}
        securityIssues={mockSecurityIssues}
        performanceIssues={mockPerformanceIssues}
        isLoading={false}
        onClose={mockOnClose}
      />
    );

    expect(screen.getByText('Line 15')).toBeInTheDocument();
    expect(screen.getByText('Line 23')).toBeInTheDocument();
    expect(screen.getByText('Line 8')).toBeInTheDocument();
    expect(screen.getByText('Line 42')).toBeInTheDocument();
  });

  it('displays code examples for suggestions', () => {
    render(
      <AIFeedback
        suggestions={mockSuggestions}
        overallScore={75}
        securityIssues={mockSecurityIssues}
        performanceIssues={mockPerformanceIssues}
        isLoading={false}
        onClose={mockOnClose}
      />
    );

    expect(screen.getByText('Use parameterized queries instead')).toBeInTheDocument();
    expect(screen.getByText('public async Task<Result> GetDataAsync()')).toBeInTheDocument();
  });

  it('displays security and performance issue badges', () => {
    render(
      <AIFeedback
        suggestions={mockSuggestions}
        overallScore={75}
        securityIssues={mockSecurityIssues}
        performanceIssues={mockPerformanceIssues}
        isLoading={false}
        onClose={mockOnClose}
      />
    );

    expect(screen.getByText('1 Security Issue')).toBeInTheDocument();
    expect(screen.getByText('1 Performance Issue')).toBeInTheDocument();
  });

  it('calls onClose when close button is clicked', () => {
    render(
      <AIFeedback
        suggestions={mockSuggestions}
        overallScore={75}
        securityIssues={mockSecurityIssues}
        performanceIssues={mockPerformanceIssues}
        isLoading={false}
        onClose={mockOnClose}
      />
    );

    const closeButton = screen.getByLabelText('Close feedback');
    fireEvent.click(closeButton);

    expect(mockOnClose).toHaveBeenCalledTimes(1);
  });

  it('displays success message when no issues found', () => {
    render(
      <AIFeedback
        suggestions={[]}
        overallScore={100}
        securityIssues={[]}
        performanceIssues={[]}
        isLoading={false}
        onClose={mockOnClose}
      />
    );

    expect(screen.getByText('🎉 Great job!')).toBeInTheDocument();
    expect(screen.getByText('No issues found in your code.')).toBeInTheDocument();
  });

  it('displays correct score color for excellent score', () => {
    const { container } = render(
      <AIFeedback
        suggestions={[]}
        overallScore={85}
        securityIssues={[]}
        performanceIssues={[]}
        isLoading={false}
        onClose={mockOnClose}
      />
    );

    const scoreElement = screen.getByText('85/100');
    expect(scoreElement).toHaveClass('text-green-600');
  });

  it('displays correct score color for good score', () => {
    const { container } = render(
      <AIFeedback
        suggestions={[]}
        overallScore={65}
        securityIssues={[]}
        performanceIssues={[]}
        isLoading={false}
        onClose={mockOnClose}
      />
    );

    const scoreElement = screen.getByText('65/100');
    expect(scoreElement).toHaveClass('text-yellow-600');
  });

  it('displays correct score color for poor score', () => {
    const { container } = render(
      <AIFeedback
        suggestions={[]}
        overallScore={45}
        securityIssues={[]}
        performanceIssues={[]}
        isLoading={false}
        onClose={mockOnClose}
      />
    );

    const scoreElement = screen.getByText('45/100');
    expect(scoreElement).toHaveClass('text-red-600');
  });
});
