import React from 'react';
import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import { CodeExample } from '../CodeExample';

// Mock react-syntax-highlighter
jest.mock('react-syntax-highlighter', () => ({
  Prism: function SyntaxHighlighter({ children, language }: { children: string; language: string }) {
    return (
      <pre data-testid="syntax-highlighter" data-language={language}>
        {children}
      </pre>
    );
  },
}));

jest.mock('react-syntax-highlighter/dist/cjs/styles/prism', () => ({
  vscDarkPlus: {},
}));

// Mock clipboard API
Object.assign(navigator, {
  clipboard: {
    writeText: jest.fn(() => Promise.resolve()),
  },
});

describe('CodeExample', () => {
  const mockCode = 'int x = 10;\nConsole.WriteLine(x);';
  const mockTitle = 'Exemplo de Variável';
  const mockLanguage = 'csharp';
  const mockExplanation = 'Este código declara uma variável e imprime seu valor.';

  beforeEach(() => {
    jest.clearAllMocks();
  });

  it('should render title, code, and explanation', () => {
    render(
      <CodeExample
        title={mockTitle}
        code={mockCode}
        language={mockLanguage}
        explanation={mockExplanation}
        isRunnable={false}
      />
    );

    expect(screen.getByText(mockTitle)).toBeInTheDocument();
    expect(screen.getByTestId('syntax-highlighter')).toBeInTheDocument();
    expect(screen.getByText(mockExplanation)).toBeInTheDocument();
  });

  it('should render syntax highlighter with correct language', () => {
    render(
      <CodeExample
        title={mockTitle}
        code={mockCode}
        language={mockLanguage}
        explanation={mockExplanation}
        isRunnable={false}
      />
    );

    const highlighter = screen.getByTestId('syntax-highlighter');
    expect(highlighter).toHaveAttribute('data-language', mockLanguage);
  });

  it('should render copy button', () => {
    render(
      <CodeExample
        title={mockTitle}
        code={mockCode}
        language={mockLanguage}
        explanation={mockExplanation}
        isRunnable={false}
      />
    );

    expect(screen.getByRole('button', { name: /copy code to clipboard/i })).toBeInTheDocument();
  });

  it('should copy code to clipboard when copy button is clicked', async () => {
    render(
      <CodeExample
        title={mockTitle}
        code={mockCode}
        language={mockLanguage}
        explanation={mockExplanation}
        isRunnable={false}
      />
    );

    const copyButton = screen.getByRole('button', { name: /copy code to clipboard/i });
    fireEvent.click(copyButton);

    await waitFor(() => {
      expect(navigator.clipboard.writeText).toHaveBeenCalledWith(mockCode);
    });
  });

  it('should show "Copiado!" message after copying', async () => {
    render(
      <CodeExample
        title={mockTitle}
        code={mockCode}
        language={mockLanguage}
        explanation={mockExplanation}
        isRunnable={false}
      />
    );

    const copyButton = screen.getByRole('button', { name: /copy code to clipboard/i });
    fireEvent.click(copyButton);

    await waitFor(() => {
      expect(screen.getByText('Copiado!')).toBeInTheDocument();
    });
  });

  it('should render run button when isRunnable is true', () => {
    render(
      <CodeExample
        title={mockTitle}
        code={mockCode}
        language={mockLanguage}
        explanation={mockExplanation}
        isRunnable={true}
        onRun={jest.fn()}
      />
    );

    expect(screen.getByRole('button', { name: /run code/i })).toBeInTheDocument();
  });

  it('should not render run button when isRunnable is false', () => {
    render(
      <CodeExample
        title={mockTitle}
        code={mockCode}
        language={mockLanguage}
        explanation={mockExplanation}
        isRunnable={false}
      />
    );

    expect(screen.queryByRole('button', { name: /run code/i })).not.toBeInTheDocument();
  });

  it('should not render run button when onRun is not provided', () => {
    render(
      <CodeExample
        title={mockTitle}
        code={mockCode}
        language={mockLanguage}
        explanation={mockExplanation}
        isRunnable={true}
      />
    );

    expect(screen.queryByRole('button', { name: /run code/i })).not.toBeInTheDocument();
  });

  it('should call onRun callback when run button is clicked', () => {
    const mockOnRun = jest.fn();
    render(
      <CodeExample
        title={mockTitle}
        code={mockCode}
        language={mockLanguage}
        explanation={mockExplanation}
        isRunnable={true}
        onRun={mockOnRun}
      />
    );

    const runButton = screen.getByRole('button', { name: /run code/i });
    fireEvent.click(runButton);

    expect(mockOnRun).toHaveBeenCalledWith(mockCode);
  });

  it('should render without explanation when not provided', () => {
    render(
      <CodeExample
        title={mockTitle}
        code={mockCode}
        language={mockLanguage}
        explanation=""
        isRunnable={false}
      />
    );

    expect(screen.queryByText(mockExplanation)).not.toBeInTheDocument();
  });

  it('should render nothing when code is empty', () => {
    const { container } = render(
      <CodeExample
        title={mockTitle}
        code=""
        language={mockLanguage}
        explanation={mockExplanation}
        isRunnable={false}
      />
    );

    expect(container.firstChild).toBeNull();
  });

  it('should have proper styling classes', () => {
    const { container } = render(
      <CodeExample
        title={mockTitle}
        code={mockCode}
        language={mockLanguage}
        explanation={mockExplanation}
        isRunnable={false}
      />
    );

    expect(container.querySelector('.code-example')).toBeInTheDocument();
    expect(container.querySelector('.code-example-header')).toBeInTheDocument();
    expect(container.querySelector('.code-example-body')).toBeInTheDocument();
    expect(container.querySelector('.code-explanation')).toBeInTheDocument();
  });

  it('should handle copy error gracefully', async () => {
    const consoleErrorSpy = jest.spyOn(console, 'error').mockImplementation(() => {});
    (navigator.clipboard.writeText as jest.Mock).mockRejectedValueOnce(new Error('Copy failed'));

    render(
      <CodeExample
        title={mockTitle}
        code={mockCode}
        language={mockLanguage}
        explanation={mockExplanation}
        isRunnable={false}
      />
    );

    const copyButton = screen.getByRole('button', { name: /copy code to clipboard/i });
    fireEvent.click(copyButton);

    await waitFor(() => {
      expect(consoleErrorSpy).toHaveBeenCalledWith('Failed to copy code:', expect.any(Error));
    });

    consoleErrorSpy.mockRestore();
  });

  it('should support multiple programming languages', () => {
    const languages = ['javascript', 'python', 'java', 'typescript'];

    languages.forEach((lang) => {
      const { unmount } = render(
        <CodeExample
          title={mockTitle}
          code={mockCode}
          language={lang}
          explanation={mockExplanation}
          isRunnable={false}
        />
      );

      const highlighter = screen.getByTestId('syntax-highlighter');
      expect(highlighter).toHaveAttribute('data-language', lang);

      unmount();
    });
  });

  it('should render multiline code correctly', () => {
    const multilineCode = `using System;

namespace HelloWorld
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }
}`;

    render(
      <CodeExample
        title={mockTitle}
        code={multilineCode}
        language={mockLanguage}
        explanation={mockExplanation}
        isRunnable={false}
      />
    );

    const highlighter = screen.getByTestId('syntax-highlighter');
    expect(highlighter).toBeInTheDocument();
    expect(highlighter.textContent).toContain('using System');
    expect(highlighter.textContent).toContain('namespace HelloWorld');
  });

  it('should render code with special characters', () => {
    const specialCode = 'string text = "Hello <World> & \'Friends\'";';

    render(
      <CodeExample
        title={mockTitle}
        code={specialCode}
        language={mockLanguage}
        explanation={mockExplanation}
        isRunnable={false}
      />
    );

    expect(screen.getByTestId('syntax-highlighter')).toHaveTextContent(specialCode);
  });

  it('should have accessible buttons', () => {
    render(
      <CodeExample
        title={mockTitle}
        code={mockCode}
        language={mockLanguage}
        explanation={mockExplanation}
        isRunnable={true}
        onRun={jest.fn()}
      />
    );

    const copyButton = screen.getByRole('button', { name: /copy code to clipboard/i });
    const runButton = screen.getByRole('button', { name: /run code/i });

    expect(copyButton).toHaveAttribute('aria-label', 'Copy code to clipboard');
    expect(runButton).toHaveAttribute('aria-label', 'Run code');
  });
});
