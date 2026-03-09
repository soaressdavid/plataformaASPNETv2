import { render, screen, fireEvent } from '@testing-library/react';
import { ExerciseList } from '../ExerciseList';
import { Exercise } from '../../types';

describe('ExerciseList', () => {
  const mockExercises: Exercise[] = [
    {
      title: 'Variáveis e Tipos',
      description: 'Pratique declaração de variáveis e tipos de dados em C#.',
      difficulty: 'Fácil',
      starterCode: 'int x = 0;',
      hints: ['Use int para números inteiros', 'Use string para texto'],
    },
    {
      title: 'Loops e Condicionais',
      description: 'Implemente loops e estruturas condicionais.',
      difficulty: 'Médio',
      starterCode: 'for (int i = 0; i < 10; i++) {}',
      hints: ['Use for para loops', 'Use if/else para condições'],
    },
    {
      title: 'Classes e Objetos',
      description: 'Crie classes e instancie objetos.',
      difficulty: 'Difícil',
      starterCode: 'public class MyClass {}',
      hints: [],
    },
  ];

  it('renders nothing when exercises array is empty', () => {
    const { container } = render(<ExerciseList exercises={[]} />);
    expect(container.firstChild).toBeNull();
  });

  it('renders nothing when exercises is null/undefined', () => {
    const { container } = render(<ExerciseList exercises={null as any} />);
    expect(container.firstChild).toBeNull();
  });

  it('renders section title', () => {
    render(<ExerciseList exercises={mockExercises} />);
    expect(screen.getByText('Exercícios Práticos')).toBeInTheDocument();
  });

  it('renders all exercises', () => {
    render(<ExerciseList exercises={mockExercises} />);
    expect(screen.getByText('Variáveis e Tipos')).toBeInTheDocument();
    expect(screen.getByText('Loops e Condicionais')).toBeInTheDocument();
    expect(screen.getByText('Classes e Objetos')).toBeInTheDocument();
  });

  it('renders exercise descriptions', () => {
    render(<ExerciseList exercises={mockExercises} />);
    expect(
      screen.getByText('Pratique declaração de variáveis e tipos de dados em C#.')
    ).toBeInTheDocument();
  });

  it('renders difficulty badges with correct text', () => {
    render(<ExerciseList exercises={mockExercises} />);
    expect(screen.getByText('Fácil')).toBeInTheDocument();
    expect(screen.getByText('Médio')).toBeInTheDocument();
    expect(screen.getByText('Difícil')).toBeInTheDocument();
  });

  it('applies correct CSS classes for difficulty badges', () => {
    render(<ExerciseList exercises={mockExercises} />);
    const facilBadge = screen.getByText('Fácil');
    const medioBadge = screen.getByText('Médio');
    const dificilBadge = screen.getByText('Difícil');

    expect(facilBadge).toHaveClass('bg-green-100', 'text-green-800');
    expect(medioBadge).toHaveClass('bg-yellow-100', 'text-yellow-800');
    expect(dificilBadge).toHaveClass('bg-red-100', 'text-red-800');
  });

  it('renders hints in collapsible details element', () => {
    render(<ExerciseList exercises={mockExercises} />);
    const hintsElements = screen.getAllByText(/Dicas \(\d+\)/);
    expect(hintsElements).toHaveLength(2); // Only 2 exercises have hints
  });

  it('shows correct hint count', () => {
    render(<ExerciseList exercises={mockExercises} />);
    const hintsElements = screen.getAllByText('Dicas (2)');
    expect(hintsElements).toHaveLength(2); // Both first and second exercises have 2 hints
  });

  it('does not render hints section when hints array is empty', () => {
    render(<ExerciseList exercises={mockExercises} />);
    const allCards = screen.getAllByText(/Variáveis e Tipos|Loops e Condicionais|Classes e Objetos/);
    // The third exercise (Classes e Objetos) has no hints
    const thirdExerciseTitle = screen.getByText('Classes e Objetos');
    const thirdCard = thirdExerciseTitle.closest('.exercise-card');
    expect(thirdCard?.querySelector('details')).not.toBeInTheDocument();
  });

  it('renders start button when onStartExercise callback is provided', () => {
    const mockOnStart = jest.fn();
    render(<ExerciseList exercises={mockExercises} onStartExercise={mockOnStart} />);
    const startButtons = screen.getAllByText('Começar Exercício');
    expect(startButtons).toHaveLength(3);
  });

  it('does not render start button when onStartExercise is not provided', () => {
    render(<ExerciseList exercises={mockExercises} />);
    const startButtons = screen.queryAllByText('Começar Exercício');
    expect(startButtons).toHaveLength(0);
  });

  it('calls onStartExercise with correct exercise when start button is clicked', () => {
    const mockOnStart = jest.fn();
    render(<ExerciseList exercises={mockExercises} onStartExercise={mockOnStart} />);
    
    const startButtons = screen.getAllByText('Começar Exercício');
    fireEvent.click(startButtons[0]);
    
    expect(mockOnStart).toHaveBeenCalledTimes(1);
    expect(mockOnStart).toHaveBeenCalledWith(mockExercises[0]);
  });

  it('renders exercises in a grid layout', () => {
    render(<ExerciseList exercises={mockExercises} />);
    const grid = screen.getByText('Variáveis e Tipos').closest('.exercises-grid');
    expect(grid).toHaveClass('grid', 'grid-cols-1', 'md:grid-cols-2');
  });

  it('has accessible aria-label on start buttons', () => {
    const mockOnStart = jest.fn();
    render(<ExerciseList exercises={mockExercises} onStartExercise={mockOnStart} />);
    
    expect(
      screen.getByLabelText('Começar exercício: Variáveis e Tipos')
    ).toBeInTheDocument();
  });

  it('renders hints list items correctly', () => {
    render(<ExerciseList exercises={mockExercises} />);
    
    // Open the first details element
    const detailsElements = screen.getAllByText(/Dicas \(\d+\)/);
    fireEvent.click(detailsElements[0]);
    
    expect(screen.getByText('Use int para números inteiros')).toBeInTheDocument();
    expect(screen.getByText('Use string para texto')).toBeInTheDocument();
  });
});
