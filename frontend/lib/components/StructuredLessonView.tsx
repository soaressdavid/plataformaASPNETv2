'use client';

import { LessonContent, Exercise } from '../types';
import { LessonObjectives } from './LessonObjectives';
import { TheorySection } from './TheorySection';
import { CodeExample } from './CodeExample';
import { ExerciseList } from './ExerciseList';
import { LessonSummary } from './LessonSummary';

export interface StructuredLessonViewProps {
  content: LessonContent;
  onRunCode?: (code: string) => void;
  onStartExercise?: (exercise: Exercise) => void;
}

/**
 * Main component that orchestrates all structured content components.
 * Renders lesson content in the correct order: objectives, theory, code examples, exercises, and summary.
 * 
 * @param content - Structured lesson content object
 * @param onRunCode - Optional callback function when code execution is triggered
 * @param onStartExercise - Optional callback function when an exercise is started
 */
export function StructuredLessonView({
  content,
  onRunCode,
  onStartExercise,
}: StructuredLessonViewProps) {
  if (!content) {
    return null;
  }

  // Sort theory sections by order property
  const sortedTheory = content.theory
    ? [...content.theory].sort((a, b) => a.order - b.order)
    : [];

  return (
    <div className="structured-lesson space-y-10">
      {/* Lesson Objectives */}
      {content.objectives && content.objectives.length > 0 && (
        <LessonObjectives objectives={content.objectives} />
      )}

      {/* Theory Sections */}
      {sortedTheory.length > 0 && (
        <div className="theory-sections space-y-8">
          {sortedTheory.map((section, index) => (
            <TheorySection
              key={index}
              heading={section.heading}
              content={section.content}
              order={section.order}
            />
          ))}
        </div>
      )}

      {/* Code Examples */}
      {content.codeExamples && content.codeExamples.length > 0 && (
        <div className="code-examples space-y-6 mt-10">
          <div className="flex items-center gap-3 mb-6">
            <div className="w-10 h-10 bg-gradient-to-br from-indigo-500 to-purple-600 rounded-lg flex items-center justify-center">
              <svg className="w-6 h-6 text-white" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M10 20l4-16m4 4l4 4-4 4M6 16l-4-4 4-4" />
              </svg>
            </div>
            <h2 className="text-2xl font-bold bg-gradient-to-r from-indigo-600 to-purple-600 bg-clip-text text-transparent">
              Exemplos de Código
            </h2>
          </div>
          {content.codeExamples.map((example, index) => (
            <CodeExample
              key={index}
              title={example.title}
              code={example.code}
              language={example.language}
              explanation={example.explanation}
              isRunnable={example.isRunnable}
              onRun={onRunCode}
            />
          ))}
        </div>
      )}

      {/* Exercises */}
      {content.exercises && content.exercises.length > 0 && (
        <ExerciseList
          exercises={content.exercises}
          onStartExercise={onStartExercise}
        />
      )}

      {/* Lesson Summary */}
      {content.summary && <LessonSummary summary={content.summary} />}
    </div>
  );
}
