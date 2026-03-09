import Link from 'next/link';
import { CurriculumLevel } from '../types';
import { Icons } from './Icons';

export interface LevelCardProps {
  level: CurriculumLevel;
  isLocked?: boolean;
  progress?: number;
}

export function LevelCard({ level, isLocked = false, progress }: LevelCardProps) {
  const cardClasses = `
    relative block rounded-lg border-2 transition-all duration-200
    ${isLocked 
      ? 'border-gray-300 bg-gray-50 cursor-not-allowed opacity-60' 
      : 'border-blue-200 bg-white hover:border-blue-400 hover:shadow-lg cursor-pointer'
    }
    p-6
  `.trim();

  const content = (
    <>
      <div className="flex items-center justify-between mb-3">
        <div className="inline-flex items-center justify-center w-12 h-12 rounded-full bg-blue-100 text-blue-600 font-bold text-lg">
          {level.number}
        </div>
        {isLocked && (
          <Icons.LockClosed className="w-6 h-6 text-gray-400" />
        )}
      </div>

      <h3 className="text-xl font-semibold text-gray-900 mb-2">
        {level.title}
      </h3>
      
      <p className="text-gray-600 text-sm mb-4 line-clamp-2">
        {level.description}
      </p>

      <div className="flex items-center gap-4 text-sm text-gray-500 mb-3">
        <div className="flex items-center gap-1">
          <Icons.BookOpen className="w-4 h-4" />
          <span>{level.courseCount} {level.courseCount === 1 ? 'curso' : 'cursos'}</span>
        </div>
        <div className="flex items-center gap-1">
          <Icons.Clock className="w-4 h-4" />
          <span>{level.estimatedHours}h estimadas</span>
        </div>
      </div>

      {/* Progress bar for in-progress levels */}
      {progress !== undefined && !isLocked && (
        <div className="mt-4">
          <div className="flex items-center justify-between text-xs text-gray-600 mb-1">
            <span>Progresso</span>
            <span>{Math.round(progress)}%</span>
          </div>
          <div className="w-full bg-gray-200 rounded-full h-2 overflow-hidden">
            <div
              className="bg-blue-500 h-full rounded-full transition-all duration-300"
              style={{ width: `${Math.min(100, Math.max(0, progress))}%` }}
            />
          </div>
        </div>
      )}

      {/* Lock overlay for locked levels */}
      {isLocked && (
        <div className="absolute inset-0 flex flex-col items-center justify-center bg-white/80 rounded-lg">
          <Icons.LockClosed className="w-12 h-12 text-gray-400 mb-2" />
          <span className="text-sm font-medium text-gray-600">
            Requer {level.requiredXP} XP
          </span>
        </div>
      )}
    </>
  );

  // If locked, render as div instead of Link
  if (isLocked) {
    return (
      <div className={cardClasses}>
        {content}
      </div>
    );
  }

  // Otherwise, render as clickable Link
  return (
    <Link href={`/levels/${level.id}`} className={cardClasses}>
      {content}
    </Link>
  );
}
