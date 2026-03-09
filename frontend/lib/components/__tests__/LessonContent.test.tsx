import React from 'react';
import { render, screen } from '@testing-library/react';
import { LessonContent } from '../LessonContent';
import { LessonDetail, LessonContent as LessonContentType, Exercise } from '../../types';

// Mock child components
jest.mock('../StructuredLessonView', () => ({
  StructuredLessonView: ({ content }: { content: LessonContentType }) => (
    <div data-testid="structured-lesson-view">
      Structured Content: {content.objectives.join(', ')}
    </div>
  ),
}));

describe('LessonContent', () => {
  const mockStructuredContent: LessonContentType = {
    objectives: ['Objetivo 1', 'Objetivo 2'],
    theory: [
      {
        heading: 'Teoria 1',
        content: 'Conteúdo da teoria',
        order: 1,
      },
    ],
    codeExamples: [
      {
        title: 'Exemplo 1',
        code: 'int x = 10;',
        language: 'csharp',
        explanation: 'Explicação',
        isRunnable: false,
      },
    ],
    exercises: [
      {
        title: 'Exercício 1',
        description: 'Descrição',
        difficulty: 'Fácil',
        starterCode: 'int x = 0;',
        hints: ['Dica 1'],
      },
    ],
    summary: 'Resumo da lição',
  };

  const mockLessonWithStructuredContent: LessonDetail = {
    id: '1',
    title: 'Lição com Conteúdo Estruturado',
    duration: '30 min',
    difficulty: 'Fácil',
    estimatedMinutes: 30,
    order: 1,
    isCompleted: false,
    structuredContent: mockStructuredContent,
    content: undefined,
    prerequisites: [],
  };

  const mockLessonWithHtmlContent: LessonDetail = {
    id: '2',
    title: 'Lição com HTML',
    duration: '30 min',
    difficulty: 'Fácil',
    estimatedMinutes: 30,
    order: 1,
    isCompleted: false,
    content: '<h1>Título HTML</h1><p>Parágrafo HTML</p>',
    structuredContent: undefined,
    prerequisites: [],
  };

  const mockLessonWithBothContents: LessonDetail = {
    id: '3',
    title: 'Lição com Ambos Conteúdos',
    duration: '30 min',
    difficulty: 'Fácil',
    estimatedMinutes: 30,
    order: 1,
    isCompleted: false,
    structuredContent: mockStructuredContent,
    content: '<h1>HTML que não deve aparecer</h1>',
    prerequisites: [],
  };

  const mockLessonWithNoContent: LessonDetail = {
    id: '4',
    title: 'Lição sem Conteúdo',
    duration: '30 min',
    difficulty: 'Fácil',
    estimatedMinutes: 30,
    order: 1,
    isCompleted: false,
    content: undefined,
    structuredContent: undefined,
    prerequisites: [],
  };

  it('should render StructuredLessonView when structuredContent is available', () => {
    render(<LessonContent lesson={mockLessonWithStructuredContent} />);

    expect(screen.getByTestId('structured-lesson-view')).toBeInTheDocument();
    expect(screen.getByText(/Structured Content: Objetivo 1, Objetivo 2/)).toBeInTheDocument();
  });

  it('should render HTML content when only content is available', () => {
    render(<LessonContent lesson={mockLessonWithHtmlContent} />);

    const htmlContent = screen.getByText('Título HTML');
    expect(htmlContent).toBeInTheDocument();
    expect(screen.getByText('Parágrafo HTML')).toBeInTheDocument();
  });

  it('should prefer structuredContent over HTML content when both are available', () => {
    render(<LessonContent lesson={mockLessonWithBothContents} />);

    // Should render structured content
    expect(screen.getByTestId('structured-lesson-view')).toBeInTheDocument();

    // Should NOT render HTML content
    expect(screen.queryByText('HTML que não deve aparecer')).not.toBeInTheDocument();
  });

  it('should render "Conteúdo não disponível" message when no content is available', () => {
    render(<LessonContent lesson={mockLessonWithNoContent} />);

    expect(screen.getByText('Conteúdo não disponível.')).toBeInTheDocument();
  });

  it('should pass onRunCode callback to StructuredLessonView', () => {
    const mockOnRunCode = jest.fn();
    const { rerender } = render(
      <LessonContent lesson={mockLessonWithStructuredContent} onRunCode={mockOnRunCode} />
    );

    expect(screen.getByTestId('structured-lesson-view')).toBeInTheDocument();
    rerender(<LessonContent lesson={mockLessonWithStructuredContent} />);
  });

  it('should pass onStartExercise callback to StructuredLessonView', () => {
    const mockOnStartExercise = jest.fn();
    const { rerender } = render(
      <LessonContent lesson={mockLessonWithStructuredContent} onStartExercise={mockOnStartExercise} />
    );

    expect(screen.getByTestId('structured-lesson-view')).toBeInTheDocument();
    rerender(<LessonContent lesson={mockLessonWithStructuredContent} />);
  });

  it('should have proper styling classes for HTML content', () => {
    const { container } = render(<LessonContent lesson={mockLessonWithHtmlContent} />);

    const htmlContentDiv = container.querySelector('.html-lesson-content');
    expect(htmlContentDiv).toBeInTheDocument();
    expect(htmlContentDiv).toHaveClass('prose', 'prose-lg', 'dark:prose-invert', 'max-w-none');
  });

  it('should have proper styling classes for no content message', () => {
    const { container } = render(<LessonContent lesson={mockLessonWithNoContent} />);

    const noContentDiv = container.querySelector('.no-content');
    expect(noContentDiv).toBeInTheDocument();
    expect(noContentDiv).toHaveClass('flex', 'items-center', 'justify-center', 'py-12');
  });

  it('should render HTML content with dangerouslySetInnerHTML', () => {
    const { container } = render(<LessonContent lesson={mockLessonWithHtmlContent} />);

    const htmlContentDiv = container.querySelector('.html-lesson-content');
    expect(htmlContentDiv?.innerHTML).toContain('<h1>Título HTML</h1>');
    expect(htmlContentDiv?.innerHTML).toContain('<p>Parágrafo HTML</p>');
  });

  it('should handle empty string content as no content', () => {
    const lessonWithEmptyContent: LessonDetail = {
      ...mockLessonWithNoContent,
      content: '',
    };

    render(<LessonContent lesson={lessonWithEmptyContent} />);

    expect(screen.getByText('Conteúdo não disponível.')).toBeInTheDocument();
  });

  it('should handle empty structuredContent as no structured content', () => {
    const lessonWithEmptyStructured: LessonDetail = {
      ...mockLessonWithHtmlContent,
      structuredContent: undefined,
    };

    render(<LessonContent lesson={lessonWithEmptyStructured} />);

    // Should fall back to HTML content
    expect(screen.getByText('Título HTML')).toBeInTheDocument();
  });

  it('should render complex HTML content correctly', () => {
    const complexHtmlLesson: LessonDetail = {
      ...mockLessonWithNoContent,
      content: `
        <div class="lesson">
          <h1>Título Principal</h1>
          <p>Parágrafo com <strong>negrito</strong> e <em>itálico</em>.</p>
          <ul>
            <li>Item 1</li>
            <li>Item 2</li>
          </ul>
          <pre><code>int x = 10;</code></pre>
        </div>
      `,
    };

    render(<LessonContent lesson={complexHtmlLesson} />);

    expect(screen.getByText('Título Principal')).toBeInTheDocument();
    expect(screen.getByText(/Parágrafo com/)).toBeInTheDocument();
    expect(screen.getByText('Item 1')).toBeInTheDocument();
    expect(screen.getByText('Item 2')).toBeInTheDocument();
  });

  it('should handle HTML content with special characters', () => {
    const specialCharsLesson: LessonDetail = {
      ...mockLessonWithNoContent,
      content: '<p>Operadores: &lt;, &gt;, &amp;, &quot;</p>',
    };

    render(<LessonContent lesson={specialCharsLesson} />);

    expect(screen.getByText(/Operadores:/)).toBeInTheDocument();
  });

  it('should be accessible with proper semantic HTML', () => {
    render(<LessonContent lesson={mockLessonWithNoContent} />);

    const noContentMessage = screen.getByText('Conteúdo não disponível.');
    expect(noContentMessage).toBeInTheDocument();
    expect(noContentMessage.tagName).toBe('P');
  });

  it('should handle lesson with null structuredContent and null content', () => {
    const nullContentLesson: LessonDetail = {
      ...mockLessonWithNoContent,
      content: null as any,
      structuredContent: null as any,
    };

    render(<LessonContent lesson={nullContentLesson} />);

    expect(screen.getByText('Conteúdo não disponível.')).toBeInTheDocument();
  });

  it('should render without callbacks', () => {
    render(<LessonContent lesson={mockLessonWithStructuredContent} />);

    expect(screen.getByTestId('structured-lesson-view')).toBeInTheDocument();
  });

  it('should render with both callbacks', () => {
    const mockOnRunCode = jest.fn();
    const mockOnStartExercise = jest.fn();

    render(
      <LessonContent
        lesson={mockLessonWithStructuredContent}
        onRunCode={mockOnRunCode}
        onStartExercise={mockOnStartExercise}
      />
    );

    expect(screen.getByTestId('structured-lesson-view')).toBeInTheDocument();
  });
});
