import React from 'react';
import { render, screen } from '@testing-library/react';
import { TheorySection } from '../TheorySection';

// Mock react-markdown and react-syntax-highlighter
jest.mock('react-markdown', () => {
  return function ReactMarkdown({ children }: { children: string }) {
    return <div data-testid="markdown-content">{children}</div>;
  };
});

jest.mock('react-syntax-highlighter', () => ({
  Prism: function SyntaxHighlighter({ children }: { children: string }) {
    return <pre data-testid="syntax-highlighter">{children}</pre>;
  },
}));

jest.mock('react-syntax-highlighter/dist/cjs/styles/prism', () => ({
  vscDarkPlus: {},
}));

describe('TheorySection', () => {
  const mockHeading = 'Introdução às Variáveis';
  const mockContent = 'Variáveis são contêineres para armazenar valores de dados.';

  it('should render heading and content', () => {
    render(<TheorySection heading={mockHeading} content={mockContent} order={1} />);
    
    expect(screen.getByText(mockHeading)).toBeInTheDocument();
    expect(screen.getByTestId('markdown-content')).toHaveTextContent(mockContent);
  });

  it('should render markdown content correctly', () => {
    const markdownContent = '**Texto em negrito** e *texto em itálico*';
    render(<TheorySection heading={mockHeading} content={markdownContent} order={1} />);
    
    expect(screen.getByTestId('markdown-content')).toHaveTextContent(markdownContent);
  });

  it('should render markdown lists', () => {
    const listContent = `
Lista de itens:
- Item 1
- Item 2
- Item 3
    `;
    render(<TheorySection heading={mockHeading} content={listContent} order={1} />);
    
    expect(screen.getByTestId('markdown-content')).toBeInTheDocument();
  });

  it('should render code blocks with syntax highlighting', () => {
    const codeContent = `
Exemplo de código:

\`\`\`csharp
int x = 10;
Console.WriteLine(x);
\`\`\`
    `;
    render(<TheorySection heading={mockHeading} content={codeContent} order={1} />);
    
    expect(screen.getByTestId('markdown-content')).toBeInTheDocument();
  });

  it('should render inline code correctly', () => {
    const inlineCodeContent = 'Use a palavra-chave `var` para declarar variáveis.';
    render(<TheorySection heading={mockHeading} content={inlineCodeContent} order={1} />);
    
    expect(screen.getByTestId('markdown-content')).toHaveTextContent(inlineCodeContent);
  });

  it('should have proper styling classes', () => {
    const { container } = render(<TheorySection heading={mockHeading} content={mockContent} order={1} />);
    
    const section = container.querySelector('.theory-section');
    expect(section).toBeInTheDocument();
    
    const contentDiv = container.querySelector('.theory-content');
    expect(contentDiv).toBeInTheDocument();
  });

  it('should render nothing when content is empty', () => {
    const { container } = render(<TheorySection heading={mockHeading} content="" order={1} />);
    
    expect(container.firstChild).toBeNull();
  });

  it('should render nothing when content is undefined', () => {
    const { container } = render(<TheorySection heading={mockHeading} content={undefined as any} order={1} />);
    
    expect(container.firstChild).toBeNull();
  });

  it('should have accessible heading', () => {
    render(<TheorySection heading={mockHeading} content={mockContent} order={1} />);
    
    const heading = screen.getByRole('heading', { level: 3 });
    expect(heading).toHaveTextContent(mockHeading);
  });

  it('should render markdown links', () => {
    const linkContent = 'Visite a [documentação oficial](https://docs.microsoft.com) para mais informações.';
    render(<TheorySection heading={mockHeading} content={linkContent} order={1} />);
    
    expect(screen.getByTestId('markdown-content')).toHaveTextContent(linkContent);
  });

  it('should render multiple paragraphs', () => {
    const multiParagraphContent = `
Primeiro parágrafo com informações importantes.

Segundo parágrafo com mais detalhes.

Terceiro parágrafo com conclusão.
    `;
    render(<TheorySection heading={mockHeading} content={multiParagraphContent} order={1} />);
    
    expect(screen.getByTestId('markdown-content')).toBeInTheDocument();
  });

  it('should render headings within markdown content', () => {
    const headingContent = `
## Subtítulo

Conteúdo do subtítulo.

### Sub-subtítulo

Mais conteúdo.
    `;
    render(<TheorySection heading={mockHeading} content={headingContent} order={1} />);
    
    expect(screen.getByTestId('markdown-content')).toBeInTheDocument();
  });

  it('should handle complex markdown with mixed elements', () => {
    const complexContent = `
# Título

Parágrafo com **negrito** e *itálico*.

- Lista item 1
- Lista item 2

\`\`\`javascript
const x = 10;
\`\`\`

[Link](https://example.com)
    `;
    render(<TheorySection heading={mockHeading} content={complexContent} order={1} />);
    
    expect(screen.getByTestId('markdown-content')).toBeInTheDocument();
  });

  it('should render blockquotes', () => {
    const quoteContent = `
> Esta é uma citação importante.
> Ela pode ter múltiplas linhas.
    `;
    render(<TheorySection heading={mockHeading} content={quoteContent} order={1} />);
    
    expect(screen.getByTestId('markdown-content')).toBeInTheDocument();
  });

  it('should handle special characters in content', () => {
    const specialContent = 'Operadores: <, >, &, ", \'';
    render(<TheorySection heading={mockHeading} content={specialContent} order={1} />);
    
    expect(screen.getByTestId('markdown-content')).toBeInTheDocument();
  });

  it('should render ordered lists', () => {
    const orderedListContent = `
Passos:
1. Primeiro passo
2. Segundo passo
3. Terceiro passo
    `;
    render(<TheorySection heading={mockHeading} content={orderedListContent} order={1} />);
    
    expect(screen.getByTestId('markdown-content')).toBeInTheDocument();
  });
});
