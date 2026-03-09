'use client';

import { useLevel } from '@/lib/hooks/useLevel';
import { CourseCard } from '@/lib/components/CourseCard';
import { Breadcrumb } from '@/lib/components/Breadcrumb';
import { Icons } from '@/lib/components/Icons';

/**
 * Level Detail Page
 * Displays courses within a specific curriculum level
 */
export default function LevelDetailPage({ params }: { params: { id: string } }) {
  const { level, loading, error } = useLevel(params.id);

  // Loading state - show skeleton
  if (loading) {
    return (
      <div className="min-h-screen bg-gray-50">
        <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
          {/* Breadcrumb Skeleton */}
          <div className="mb-6 animate-pulse">
            <div className="h-4 w-48 bg-gray-200 rounded"></div>
          </div>

          {/* Level Header Skeleton */}
          <div className="mb-8 animate-pulse">
            <div className="h-6 w-24 bg-gray-200 rounded mb-3"></div>
            <div className="h-9 w-96 bg-gray-200 rounded mb-3"></div>
            <div className="h-5 w-full max-w-2xl bg-gray-200 rounded mb-2"></div>
            <div className="h-5 w-3/4 bg-gray-200 rounded mb-4"></div>
            <div className="flex items-center gap-6">
              <div className="h-4 w-24 bg-gray-200 rounded"></div>
              <div className="h-4 w-32 bg-gray-200 rounded"></div>
              <div className="h-4 w-28 bg-gray-200 rounded"></div>
            </div>
          </div>

          {/* Courses Section Skeleton */}
          <div className="mb-8">
            <div className="h-7 w-32 bg-gray-200 rounded mb-4 animate-pulse"></div>
            <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
              {[1, 2, 3].map((i) => (
                <div key={i} className="bg-white rounded-lg border-2 border-gray-200 p-6 animate-pulse">
                  <div className="h-6 w-48 bg-gray-200 rounded mb-3"></div>
                  <div className="h-4 w-full bg-gray-200 rounded mb-2"></div>
                  <div className="h-4 w-3/4 bg-gray-200 rounded mb-4"></div>
                  <div className="flex items-center gap-4 mb-4">
                    <div className="h-4 w-20 bg-gray-200 rounded"></div>
                    <div className="h-4 w-16 bg-gray-200 rounded"></div>
                  </div>
                  <div className="flex gap-2">
                    <div className="h-6 w-16 bg-gray-200 rounded"></div>
                    <div className="h-6 w-20 bg-gray-200 rounded"></div>
                  </div>
                </div>
              ))}
            </div>
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
            Erro ao Carregar Nível
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

  // Not found state
  if (!level) {
    return (
      <div className="min-h-screen bg-gray-50 flex items-center justify-center">
        <div className="max-w-md w-full bg-white rounded-lg shadow-lg p-8 text-center">
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
                d="M9.172 16.172a4 4 0 015.656 0M9 10h.01M15 10h.01M21 12a9 9 0 11-18 0 9 9 0 0118 0z"
              />
            </svg>
          </div>
          <h2 className="text-2xl font-bold text-gray-900 mb-2">
            Nível Não Encontrado
          </h2>
          <p className="text-gray-600 mb-6">
            O nível solicitado não existe ou foi removido.
          </p>
          <a
            href="/levels"
            className="inline-block px-6 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 transition-colors"
          >
            Voltar para Níveis
          </a>
        </div>
      </div>
    );
  }

  // Success state - render level detail
  return (
    <div className="min-h-screen bg-gray-50">
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
        {/* Breadcrumb Navigation */}
        <Breadcrumb
          items={[
            { label: 'Níveis', href: '/levels' },
            { label: level.title },
          ]}
        />

        {/* Level Header */}
        <header className="mb-8">
          {/* Level Badge */}
          <div className="inline-flex items-center px-3 py-1 rounded-full bg-blue-100 text-blue-700 text-sm font-medium mb-3">
            Nível {level.number}
          </div>

          {/* Level Title */}
          <h1 className="text-3xl font-bold text-gray-900 mb-3">
            {level.title}
          </h1>

          {/* Level Description */}
          <p className="text-lg text-gray-600 mb-4 max-w-3xl">
            {level.description}
          </p>

          {/* Level Metadata */}
          <div className="flex flex-wrap items-center gap-6 text-sm text-gray-600">
            <div className="flex items-center gap-2">
              <Icons.BookOpen className="w-5 h-5 text-gray-400" />
              <span>
                {level.courseCount} {level.courseCount === 1 ? 'curso' : 'cursos'}
              </span>
            </div>
            <div className="flex items-center gap-2">
              <Icons.Clock className="w-5 h-5 text-gray-400" />
              <span>{level.estimatedHours}h estimadas</span>
            </div>
            <div className="flex items-center gap-2">
              <Icons.Trophy className="w-5 h-5 text-gray-400" />
              <span>{level.requiredXP} XP necessário</span>
            </div>
          </div>
        </header>

        {/* Courses Section */}
        <section className="mb-8">
          <h2 className="text-2xl font-bold text-gray-900 mb-4">Cursos</h2>
          
          {level.courses.length > 0 ? (
            <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
              {level.courses.map((course) => (
                <CourseCard key={course.id} course={course} />
              ))}
            </div>
          ) : (
            <div className="text-center py-12 bg-white rounded-lg border-2 border-gray-200">
              <div className="w-16 h-16 bg-gray-100 rounded-full flex items-center justify-center mx-auto mb-4">
                <Icons.BookOpen className="w-8 h-8 text-gray-400" />
              </div>
              <h3 className="text-lg font-medium text-gray-900 mb-1">
                Nenhum Curso Disponível
              </h3>
              <p className="text-gray-600">
                Os cursos para este nível ainda não foram adicionados.
              </p>
            </div>
          )}
        </section>

        {/* Capstone Project Section */}
        {level.project && (
          <section className="mb-8">
            <h2 className="text-2xl font-bold text-gray-900 mb-4">
              Projeto Capstone
            </h2>
            <div className="bg-linear-to-r from-purple-50 to-blue-50 rounded-lg border-2 border-purple-200 p-6">
              <div className="flex items-start gap-4">
                <div className="w-12 h-12 bg-purple-100 rounded-lg flex items-center justify-center shrink-0">
                  <Icons.Trophy className="w-6 h-6 text-purple-600" />
                </div>
                <div className="flex-1">
                  <h3 className="text-xl font-semibold text-gray-900 mb-2">
                    {level.project.title}
                  </h3>
                  <p className="text-gray-600 mb-4">
                    {level.project.description}
                  </p>
                  <div className="flex items-center gap-4 text-sm text-gray-600 mb-4">
                    <div className="flex items-center gap-1">
                      <Icons.CheckCircle className="w-4 h-4" />
                      <span>
                        {level.project.stepCount}{' '}
                        {level.project.stepCount === 1 ? 'etapa' : 'etapas'}
                      </span>
                    </div>
                    {level.project.isCompleted && (
                      <span className="inline-flex items-center px-2 py-1 rounded-full bg-green-100 text-green-700 text-xs font-medium">
                        Concluído
                      </span>
                    )}
                  </div>
                  <a
                    href={`/projects/${level.project.id}`}
                    className="inline-flex items-center px-4 py-2 bg-purple-600 text-white rounded-lg hover:bg-purple-700 transition-colors"
                  >
                    {level.project.isCompleted ? 'Revisar Projeto' : 'Iniciar Projeto'}
                    <Icons.ArrowRight className="w-4 h-4 ml-2" />
                  </a>
                </div>
              </div>
            </div>
          </section>
        )}
      </div>
    </div>
  );
}
