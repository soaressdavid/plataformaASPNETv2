import React from 'react';
import { render, screen } from '@testing-library/react';
import { LessonObjectives } from '../LessonObjectives';

describe('LessonObjectives', () => {
  const mockObjectives = [
    'Entender os conceitos básicos de variáveis',
    'Aprender sobre tipos de dados primitivos',
    'Praticar declaração e inicialização de variáveis',
  ];

  it('should render all objectives', () => {
    render(<LessonObjectives objectives={mockObjectives} />);
    
    expect(screen.getByText('Objetivos de Aprendizagem')).toBeInTheDocument();
    expect(screen.getByText('Entender os conceitos básicos de variáveis')).toBeInTheDocument();
    expect(screen.getByText('Aprender sobre tipos de dados primitivos')).toBeInTheDocument();
    expect(screen.getByText('Praticar declaração e inicialização de variáveis')).toBeInTheDocument();
  });

  it('should render check icons for each objective', () => {
    const { container } = render(<LessonObjectives objectives={mockObjectives} />);
    
    // Check for SVG icons (CheckCircle icons)
    const icons = container.querySelectorAll('svg');
    expect(icons.length).toBe(mockObjectives.length);
  });

  it('should render objectives as a list', () => {
    const { container } = render(<LessonObjectives objectives={mockObjectives} />);
    
    const list = container.querySelector('ul[role="list"]');
    expect(list).toBeInTheDocument();
    
    const listItems = container.querySelectorAll('li.objective-item');
    expect(listItems.length).toBe(mockObjectives.length);
  });

  it('should have proper styling classes', () => {
    const { container } = render(<LessonObjectives objectives={mockObjectives} />);
    
    const section = container.querySelector('.objectives-section');
    expect(section).toBeInTheDocument();
    
    const list = container.querySelector('.objectives-list');
    expect(list).toBeInTheDocument();
  });

  it('should render nothing when objectives array is empty', () => {
    const { container } = render(<LessonObjectives objectives={[]} />);
    
    expect(container.firstChild).toBeNull();
  });

  it('should render nothing when objectives is undefined', () => {
    const { container } = render(<LessonObjectives objectives={undefined as any} />);
    
    expect(container.firstChild).toBeNull();
  });

  it('should have accessible markup', () => {
    const { container } = render(<LessonObjectives objectives={mockObjectives} />);
    
    // Check for semantic HTML
    const heading = screen.getByRole('heading', { level: 2 });
    expect(heading).toHaveTextContent('Objetivos de Aprendizagem');
    
    const list = screen.getByRole('list');
    expect(list).toBeInTheDocument();
  });

  it('should render single objective correctly', () => {
    const singleObjective = ['Aprender sobre loops'];
    render(<LessonObjectives objectives={singleObjective} />);
    
    expect(screen.getByText('Aprender sobre loops')).toBeInTheDocument();
    
    const { container } = render(<LessonObjectives objectives={singleObjective} />);
    const listItems = container.querySelectorAll('li.objective-item');
    expect(listItems.length).toBe(1);
  });

  it('should handle long objective text', () => {
    const longObjectives = [
      'Compreender profundamente os conceitos avançados de programação orientada a objetos, incluindo herança, polimorfismo, encapsulamento e abstração',
    ];
    
    render(<LessonObjectives objectives={longObjectives} />);
    
    expect(screen.getByText(/Compreender profundamente os conceitos avançados/)).toBeInTheDocument();
  });

  it('should render objectives with special characters', () => {
    const specialObjectives = [
      'Aprender sobre C# & .NET',
      'Entender "strings" e \'caracteres\'',
      'Usar operadores: +, -, *, /',
    ];
    
    render(<LessonObjectives objectives={specialObjectives} />);
    
    expect(screen.getByText('Aprender sobre C# & .NET')).toBeInTheDocument();
    expect(screen.getByText('Entender "strings" e \'caracteres\'')).toBeInTheDocument();
    expect(screen.getByText('Usar operadores: +, -, *, /')).toBeInTheDocument();
  });
});
