'use client';

import { Exercise } from '../types';
import { Icons } from './Icons';

export interface ExerciseListProps {
  exercises: Exercise[];
  onStartExercise?: (exercise: Exercise) => void;
}

/**
 * Component to render a list of practice exercises in a grid layout.
 * Each exercise displays title, description, difficulty badge, and collapsible hints.
 * 
 * @param exercises - Array of exercise objects to display
 * @param onStartExercise - Optional callback function when start button is clicked
 */
export function ExerciseList({ exercises, onStartExercise }: ExerciseListProps) {
  if (!exercises || exercises.length === 0) {
    return null;
  }

  return (
    <section className="exercises-section mb-8 mt-8">
      <div className="flex items-center gap-3 mb-6">
        <div className="w-10 h-10 bg-gradient-to-br from-purple-500 to-pink-600 rounded-lg flex items-center justify-center">
          <Icons.Code className="w-6 h-6 text-white" />
        </div>
        <h2 className="text-2xl font-bold bg-gradient-to-r from-purple-600 to-pink-600 bg-clip-text text-transparent">
          Exercícios Práticos
        </h2>
      </div>
      <div className="exercises-grid grid grid-cols-1 md:grid-cols-2 gap-5">
        {exercises.map((exercise, index) => (
          <ExerciseCard
            key={index}
            exercise={exercise}
            onStart={onStartExercise}
          />
        ))}
      </div>
    </section>
  );
}

interface ExerciseCardProps {
  exercise: Exercise;
  onStart?: (exercise: Exercise) => void;
}

function ExerciseCard({ exercise, onStart }: ExerciseCardProps) {
  const getDifficultyColor = (difficulty: string) => {
    switch (difficulty) {
      case 'Fácil':
        return 'bg-green-100 text-green-800';
      case 'Médio':
        return 'bg-yellow-100 text-yellow-800';
      case 'Difícil':
        return 'bg-red-100 text-red-800';
      default:
        return 'bg-gray-100 text-gray-800';
    }
  };

  return (
    <div className="exercise-card border-2 border-gray-200 rounded-xl p-6 bg-gradient-to-br from-white to-purple-50 hover:shadow-xl hover:border-purple-300 transition-all">
      <div className="exercise-header flex items-start justify-between mb-4">
        <h4 className="text-lg font-bold flex-1 pr-2 text-gray-800">
          {exercise.title}
        </h4>
        <span
          className={`difficulty-badge px-3 py-1.5 text-xs font-bold rounded-full shrink-0 shadow-sm ${getDifficultyColor(
            exercise.difficulty
          )}`}
        >
          {exercise.difficulty}
        </span>
      </div>
      
      <p className="text-base mb-5 leading-relaxed text-gray-700">
        {exercise.description}
      </p>
      
      {exercise.hints && exercise.hints.length > 0 && (
        <details className="hints mb-4 group">
          <summary className="cursor-pointer text-sm font-medium text-blue-600 hover:text-blue-700 flex items-center gap-1.5 select-none">
            <Icons.Info className="w-4 h-4" />
            <span>Dicas ({exercise.hints.length})</span>
            <Icons.ChevronDown className="w-4 h-4 transition-transform group-open:rotate-180" />
          </summary>
          <ul className="mt-3 space-y-2 pl-5 list-disc text-sm" style={{ color: '#4b5563' }}>
            {exercise.hints.map((hint, i) => (
              <li key={i} className="leading-relaxed">
                {hint}
              </li>
            ))}
          </ul>
        </details>
      )}
      
      {onStart && (
        <button
          onClick={() => onStart(exercise)}
          className="start-button w-full flex items-center justify-center gap-2 px-5 py-3 text-sm font-bold text-white bg-gradient-to-r from-purple-600 to-pink-600 hover:from-purple-700 hover:to-pink-700 rounded-lg transition-all shadow-md hover:shadow-lg transform hover:-translate-y-0.5"
          aria-label={`Começar exercício: ${exercise.title}`}
        >
          <Icons.Play className="w-5 h-5" />
          <span>Começar Exercício</span>
        </button>
      )}
    </div>
  );
}
