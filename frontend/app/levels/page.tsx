'use client';

import { useLevels } from '@/lib/hooks/useLevel';
import { LevelCard } from '@/lib/components/LevelCard';

/**
 * Levels List Page
 * Displays all curriculum levels in a responsive grid layout
 */
export default function LevelsPage() {
  const { levels, loading, error } = useLevels();

  // Loading state - show skeleton cards
  if (loading) {
    return (
      <div className="min-h-screen bg-gray-50">
        <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
          {/* Page Header Skeleton */}
          <div className="mb-8 animate-pulse">
            <div className="h-9 w-80 bg-gray-200 rounded mb-2"></div>
            <div className="h-5 w-96 bg-gray-200 rounded"></div>
          </div>

          {/* Level Cards Skeleton */}
          <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
            {[1, 2, 3, 4, 5, 6].map((i) => (
              <div key={i} className="bg-white rounded-lg border-2 border-gray-200 p-6 animate-pulse">
                <div className="flex items-center justify-between mb-3">
                  <div className="w-12 h-12 bg-gray-200 rounded-full"></div>
                </div>
                <div className="h-6 w-48 bg-gray-200 rounded mb-2"></div>
                <div className="h-4 w-full bg-gray-200 rounded mb-1"></div>
                <div className="h-4 w-3/4 bg-gray-200 rounded mb-4"></div>
                <div className="flex items-center gap-4">
                  <div className="h-4 w-20 bg-gray-200 rounded"></div>
                  <div className="h-4 w-24 bg-gray-200 rounded"></div>
                </div>
              </div>
            ))}
          </div>
        </div>
      </div>
    );
  }

  // Error state - show error message
  if (error) {
    return (
      <div className="min-h-screen bg-gray-50 flex items-center justify-center">
        <div className="max-w-md w-full bg-white rounded-lg shadow-lg p-8 text-center">
          <div className="w-16 h-16 bg-red-100 rounded-full flex items-center justify-center mx-auto mb-4">
            <svg
              className="w-8 h-8 text-red-600"
              fill="none"
              stroke="currentColor"
              viewBox="0 0 24 24"
            >
              <path
                strokeLinecap="round"
                strokeLinejoin="round"
                strokeWidth={2}
                d="M12 8v4m0 4h.01M21 12a9 9 0 11-18 0 9 9 0 0118 0z"
              />
            </svg>
          </div>
          <h2 className="text-2xl font-bold text-gray-900 mb-2">
            Erro ao Carregar Níveis
          </h2>
          <p className="text-gray-600 mb-6">{error}</p>
          <button
            onClick={() => window.location.reload()}
            className="px-6 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 transition-colors"
          >
            Tentar Novamente
          </button>
        </div>
      </div>
    );
  }

  // Success state - render levels
  return (
    <div className="min-h-screen bg-gray-50">
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
        {/* Page Header */}
        <header className="mb-8">
          <h1 className="text-3xl font-bold text-gray-900 mb-2">
            Currículo de Aprendizagem
          </h1>
          <p className="text-lg text-gray-600">
            Progrida através de {levels.length} níveis de conhecimento em ASP.NET
          </p>
        </header>

        {/* Levels Grid */}
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
          {levels.map((level) => (
            <LevelCard
              key={level.id}
              level={level}
              isLocked={false} // TODO: Check user XP to determine if locked
              progress={undefined} // TODO: Calculate user progress for this level
            />
          ))}
        </div>

        {/* Empty state - if no levels found */}
        {levels.length === 0 && (
          <div className="text-center py-12">
            <div className="w-16 h-16 bg-gray-100 rounded-full flex items-center justify-center mx-auto mb-4">
              <svg
                className="w-8 h-8 text-gray-400"
                fill="none"
                stroke="currentColor"
                viewBox="0 0 24 24"
              >
                <path
                  strokeLinecap="round"
                  strokeLinejoin="round"
                  strokeWidth={2}
                  d="M12 6.253v13m0-13C10.832 5.477 9.246 5 7.5 5S4.168 5.477 3 6.253v13C4.168 18.477 5.754 18 7.5 18s3.332.477 4.5 1.253m0-13C13.168 5.477 14.754 5 16.5 5c1.747 0 3.332.477 4.5 1.253v13C19.832 18.477 18.247 18 16.5 18c-1.746 0-3.332.477-4.5 1.253"
                />
              </svg>
            </div>
            <h3 className="text-lg font-medium text-gray-900 mb-1">
              Nenhum Nível Disponível
            </h3>
            <p className="text-gray-600">
              Os níveis de currículo ainda não foram configurados.
            </p>
          </div>
        )}
      </div>
    </div>
  );
}
