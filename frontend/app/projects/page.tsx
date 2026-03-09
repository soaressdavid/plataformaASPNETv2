'use client';

import { useState, useEffect } from 'react';
import { useRouter } from 'next/navigation';
import { projectsApi } from '@/lib/api/projects';
import { ProjectSummary } from '@/lib/types';
import { Navigation } from '@/lib/components';
import { Icons } from '@/lib/components/Icons';

export default function ProjectsPage() {
  const router = useRouter();
  const [projects, setProjects] = useState<ProjectSummary[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    loadProjects();
  }, []);

  const loadProjects = async () => {
    try {
      setLoading(true);
      setError(null);
      const response = await projectsApi.getAll();
      setProjects(response.projects);
    } catch (err: any) {
      console.warn('Projects API not available, using mock data');
      
      // Use mock data when API is not available
      const mockProjects: any[] = [
        {
          id: '1',
          title: 'Blog API',
          description: 'Construa uma API RESTful completa para um sistema de blog com posts, comentários e autenticação.',
          difficulty: 'Medium',
          estimatedHours: 8,
          tags: ['API', 'CRUD', 'Authentication']
        },
        {
          id: '2',
          title: 'E-commerce Backend',
          description: 'Desenvolva o backend de um e-commerce com carrinho, pagamentos e gestão de produtos.',
          difficulty: 'Hard',
          estimatedHours: 20,
          tags: ['E-commerce', 'Payments', 'Complex']
        },
        {
          id: '3',
          title: 'Task Manager',
          description: 'Crie um gerenciador de tarefas com categorias, prioridades e notificações.',
          difficulty: 'Easy',
          estimatedHours: 5,
          tags: ['CRUD', 'Beginner']
        }
      ];
      
      setProjects(mockProjects);
      setError(null);
    } finally {
      setLoading(false);
    }
  };

  const handleProjectClick = (projectId: string) => {
    router.push(`/projects/${projectId}`);
  };

  if (loading) {
    return (
      <div className="min-h-screen bg-gray-50">
        <Navigation />
        <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
          <div className="animate-pulse">
            <div className="h-8 bg-gray-300 rounded w-1/4 mb-4"></div>
            <div className="h-4 bg-gray-300 rounded w-1/2 mb-8"></div>
            <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
              {[1, 2, 3].map((i) => (
                <div key={i} className="bg-white rounded-lg shadow p-6">
                  <div className="h-6 bg-gray-300 rounded mb-4"></div>
                  <div className="h-4 bg-gray-300 rounded mb-2"></div>
                  <div className="h-4 bg-gray-300 rounded w-3/4"></div>
                </div>
              ))}
            </div>
          </div>
        </div>
      </div>
    );
  }

  if (error) {
    return (
      <div className="min-h-screen bg-gray-50 flex items-center justify-center">
        <div className="text-center">
          <p className="text-red-600 mb-4">{error}</p>
          <button
            onClick={loadProjects}
            className="px-4 py-2 bg-blue-600 text-white rounded hover:bg-blue-700"
          >
            Retry
          </button>
        </div>
      </div>
    );
  }

  return (
    <div className="min-h-screen bg-gray-50">
      <Navigation />
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
        {/* Header */}
        <div className="mb-8">
          <h1 className="text-2xl sm:text-3xl font-bold text-gray-900 mb-2">Projetos Guiados</h1>
          <p className="text-sm sm:text-base text-gray-600">
            Construa aplicações completas em ASP.NET Core com orientação passo a passo
          </p>
        </div>

        {/* Project Stats */}
        <div className="mb-6 grid grid-cols-1 sm:grid-cols-3 gap-4">
          <div className="bg-white rounded-lg shadow p-4">
            <p className="text-sm text-gray-600">Total de Projetos</p>
            <p className="text-2xl font-bold text-gray-900">{projects.length}</p>
          </div>
          <div className="bg-white rounded-lg shadow p-4">
            <p className="text-sm text-gray-600">Concluídos</p>
            <p className="text-2xl font-bold text-green-600">
              {projects.filter((p) => p.isCompleted).length}
            </p>
          </div>
          <div className="bg-white rounded-lg shadow p-4">
            <p className="text-sm text-gray-600">Em Progresso</p>
            <p className="text-2xl font-bold text-blue-600">
              {projects.filter((p) => !p.isCompleted).length}
            </p>
          </div>
        </div>

        {/* Project List */}
        {projects.length === 0 ? (
          <div className="bg-white rounded-lg shadow p-8 text-center">
            <p className="text-gray-600">Nenhum projeto disponível ainda.</p>
          </div>
        ) : (
          <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
            {projects.map((project) => (
              <div
                key={project.id}
                onClick={() => handleProjectClick(project.id)}
                className="bg-white rounded-lg shadow hover:shadow-lg transition-shadow cursor-pointer overflow-hidden"
              >
                <div className="p-6">
                  <div className="flex items-start justify-between mb-3">
                    <h3 className="text-lg font-semibold text-gray-900 flex-1">
                      {project.title}
                    </h3>
                    {project.isCompleted && (
                      <span className="ml-2 text-green-600" title="Concluído">
                        <Icons.CheckCircle className="w-6 h-6" />
                      </span>
                    )}
                  </div>

                  <p className="text-sm text-gray-600 mb-4 line-clamp-2">
                    {project.description}
                  </p>

                  <div className="flex items-center justify-between">
                    <div className="flex items-center gap-2">
                      <span className="text-sm text-gray-600">
                        {project.stepCount} {project.stepCount === 1 ? 'etapa' : 'etapas'}
                      </span>
                      <span className="text-sm text-gray-400">•</span>
                      <span className="text-sm text-purple-600 font-medium">100 XP</span>
                    </div>
                    <span className="text-blue-600 hover:text-blue-700 font-medium text-sm">
                      {project.isCompleted ? 'Revisar' : 'Iniciar'} →
                    </span>
                  </div>
                </div>
              </div>
            ))}
          </div>
        )}
      </div>
    </div>
  );
}
