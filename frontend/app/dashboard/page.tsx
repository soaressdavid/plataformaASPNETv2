'use client';

import { useAuth } from '@/lib/contexts/AuthContext';
import { useEffect, useState } from 'react';
import { progressApi } from '@/lib/api/progress';
import { DashboardResponse } from '@/lib/types';
import Link from 'next/link';
import { Icons } from '@/lib/components/Icons';
import { Navigation } from '@/lib/components/Navigation';

export default function DashboardPage() {
  const { isAuthenticated, isLoading, user } = useAuth();
  const [dashboardData, setDashboardData] = useState<DashboardResponse | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  // Debug: Log auth state
  useEffect(() => {
    console.log('Dashboard: Auth state changed', {
      isAuthenticated,
      isLoading,
      hasUser: !!user,
      userName: user?.name
    });
  }, [isAuthenticated, isLoading, user]);

  useEffect(() => {
    const fetchDashboardData = async () => {
      if (!isAuthenticated) {
        setLoading(false);
        return;
      }

      try {
        console.log('Dashboard: Fetching dashboard data...');
        setLoading(true);
        
        try {
          const data = await progressApi.getDashboard();
          console.log('Dashboard: Data received:', data);
          setDashboardData(data);
          setError(null);
        } catch (apiError: any) {
          console.warn('Dashboard: API not available, using mock data');
          
          // Usar dados mockados se a API não estiver disponível
          const mockData: DashboardResponse = {
            currentLevel: 1,
            currentXP: 150,
            xpToNextLevel: 350,
            solvedChallenges: {
              easy: 3,
              medium: 1,
              hard: 0
            },
            completedProjects: 0,
            learningStreak: 1,
            coursesInProgress: []
          };
          
          console.log('Dashboard: Using mock data:', mockData);
          setDashboardData(mockData);
          setError(null);
        }
      } catch (err: any) {
        console.error('Dashboard: Failed to fetch dashboard data:', err);
        setError('Falha ao carregar dados do painel.');
      } finally {
        setLoading(false);
      }
    };

    fetchDashboardData();
  }, [isAuthenticated]);

  // Show loading while checking auth
  if (isLoading) {
    return (
      <div className="min-h-screen flex items-center justify-center bg-[#0d1117]">
        <div className="text-center">
          <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-blue-500 mx-auto"></div>
          <p className="mt-4 text-gray-300">Verificando autenticação...</p>
        </div>
      </div>
    );
  }

  // Show message if not authenticated (NO REDIRECT)
  if (!isAuthenticated) {
    return (
      <div className="min-h-screen flex items-center justify-center bg-[#0d1117] p-4">
        <div className="max-w-md w-full bg-gray-800 rounded-lg shadow-lg p-6 border border-gray-700">
          <h2 className="text-2xl font-bold text-red-400 mb-4">⚠️ Não Autenticado</h2>
          <p className="text-gray-300 mb-4">Você precisa fazer login para acessar o dashboard.</p>
          
          <div className="bg-gray-900 p-4 rounded mb-4 border border-gray-700">
            <p className="text-sm font-mono text-gray-400 mb-2">Status:</p>
            <p className="text-xs font-mono text-gray-300">isAuthenticated: {String(isAuthenticated)}</p>
            <p className="text-xs font-mono text-gray-300">hasUser: {String(!!user)}</p>
          </div>
          
          <Link 
            href="/login"
            className="block w-full bg-blue-600 text-white py-2 px-4 rounded hover:bg-blue-700 transition-colors text-center"
          >
            Ir para Login
          </Link>
        </div>
      </div>
    );
  }

  // Show loading while fetching dashboard data
  if (loading) {
    return (
      <div className="min-h-screen bg-[#0d1117] px-4 py-8">
        <div className="max-w-7xl mx-auto">
          <div className="animate-pulse space-y-8">
            <div className="h-8 bg-gray-800 rounded w-1/3"></div>
            <div className="h-32 bg-gray-800 rounded"></div>
            <div className="grid grid-cols-1 md:grid-cols-3 gap-6">
              <div className="h-48 bg-gray-800 rounded"></div>
              <div className="h-48 bg-gray-800 rounded"></div>
              <div className="h-48 bg-gray-800 rounded"></div>
            </div>
          </div>
        </div>
      </div>
    );
  }

  // Show error if failed to load
  if (error || !dashboardData) {
    return (
      <div className="min-h-screen bg-[#0d1117] px-4 py-8">
        <div className="max-w-2xl mx-auto">
          <div className="bg-red-900/20 border border-red-500 rounded-lg p-6">
            <h2 className="text-xl font-bold text-red-400 mb-2">Erro ao Carregar Dashboard</h2>
            <p className="text-red-300 mb-4">{error || 'Erro desconhecido'}</p>
            <button
              onClick={() => window.location.reload()}
              className="bg-red-600 text-white px-4 py-2 rounded hover:bg-red-700"
            >
              Tentar Novamente
            </button>
          </div>
        </div>
      </div>
    );
  }

  const progressPercentage = (dashboardData.xpToNextLevel || 0) > 0
    ? ((dashboardData.currentXP || 0) / ((dashboardData.currentXP || 0) + (dashboardData.xpToNextLevel || 0))) * 100
    : 100;

  // Render dashboard
  return (
    <div className="min-h-screen bg-[#0d1117]">
      {/* Navigation */}
      <Navigation />

      {/* Main Content */}
      <div className="px-4 py-8">
        <div className="max-w-7xl mx-auto space-y-8">
          {/* Header */}
          <div className="mb-8">
            <h1 className="text-4xl font-bold text-gray-100 mb-2">Painel de Controle</h1>
            <p className="text-gray-400 text-lg">Acompanhe seu progresso e conquistas</p>
          </div>

          {/* XP and Level Section */}
          <div className="bg-[#161b22] border border-[#30363d] rounded-xl p-8 shadow-lg">
            <div className="flex flex-col md:flex-row md:items-center md:justify-between gap-6 mb-6">
              <div className="flex items-center gap-4">
                <div className="w-16 h-16 bg-gradient-to-br from-blue-500 to-blue-600 rounded-xl flex items-center justify-center shadow-lg">
                  <Icons.Trophy className="w-8 h-8 text-white" />
                </div>
                <div>
                  <h2 className="text-3xl font-bold text-gray-100">
                    Nível {dashboardData.currentLevel}
                  </h2>
                  <p className="text-gray-400 mt-1">
                    {(dashboardData.currentXP || 0).toLocaleString()} XP acumulados
                  </p>
                </div>
              </div>
              <div className="bg-[#21262d] border border-[#30363d] rounded-xl px-6 py-4">
                <p className="text-xs text-gray-400 uppercase tracking-wider mb-1">
                  Próximo Nível
                </p>
                <p className="text-2xl font-bold text-blue-400">
                  {(dashboardData.xpToNextLevel || 0).toLocaleString()} XP
                </p>
              </div>
            </div>
            
            {/* Progress Bar */}
            <div className="space-y-2">
              <div className="flex items-center justify-between text-sm">
                <span className="text-gray-400">Progresso para o próximo nível</span>
                <span className="text-gray-100 font-semibold">{progressPercentage.toFixed(1)}%</span>
              </div>
              <div className="w-full bg-[#21262d] rounded-full h-4 overflow-hidden">
                <div
                  className="bg-gradient-to-r from-blue-500 to-blue-600 h-4 rounded-full transition-all duration-500 shadow-lg"
                  style={{ width: `${progressPercentage}%` }}
                ></div>
              </div>
            </div>
          </div>

          {/* Stats Grid */}
          <div className="grid grid-cols-1 md:grid-cols-3 gap-6">
            {/* Solved Challenges */}
            <div className="bg-[#161b22] border border-[#30363d] rounded-xl p-6 shadow-lg">
              <div className="flex items-center gap-3 mb-6">
                <div className="w-12 h-12 bg-gradient-to-br from-green-500 to-green-600 rounded-xl flex items-center justify-center shadow-lg">
                  <Icons.CheckCircle className="w-6 h-6 text-white" />
                </div>
                <h3 className="text-lg font-semibold text-gray-100">
                  Desafios Resolvidos
                </h3>
              </div>
              <div className="space-y-3">
                <div className="flex items-center justify-between p-4 bg-[#21262d] rounded-lg border border-[#30363d]">
                  <div className="flex items-center gap-2">
                    <div className="w-2 h-2 bg-green-500 rounded-full"></div>
                    <span className="text-sm text-gray-300 font-medium">Fácil</span>
                  </div>
                  <span className="text-2xl font-bold text-green-400">
                    {dashboardData.solvedChallenges?.easy || 0}
                  </span>
                </div>
                <div className="flex items-center justify-between p-4 bg-[#21262d] rounded-lg border border-[#30363d]">
                  <div className="flex items-center gap-2">
                    <div className="w-2 h-2 bg-yellow-500 rounded-full"></div>
                    <span className="text-sm text-gray-300 font-medium">Médio</span>
                  </div>
                  <span className="text-2xl font-bold text-yellow-400">
                    {dashboardData.solvedChallenges?.medium || 0}
                  </span>
                </div>
                <div className="flex items-center justify-between p-4 bg-[#21262d] rounded-lg border border-[#30363d]">
                  <div className="flex items-center gap-2">
                    <div className="w-2 h-2 bg-red-500 rounded-full"></div>
                    <span className="text-sm text-gray-300 font-medium">Difícil</span>
                  </div>
                  <span className="text-2xl font-bold text-red-400">
                    {dashboardData.solvedChallenges?.hard || 0}
                  </span>
                </div>
                <div className="pt-4 border-t border-[#30363d]">
                  <div className="flex items-center justify-between">
                    <span className="text-sm font-semibold text-gray-100 uppercase tracking-wider">Total</span>
                    <span className="text-3xl font-bold text-blue-400">
                      {(dashboardData.solvedChallenges?.easy || 0) + 
                       (dashboardData.solvedChallenges?.medium || 0) + 
                       (dashboardData.solvedChallenges?.hard || 0)}
                    </span>
                  </div>
                </div>
              </div>
            </div>

            {/* Completed Projects */}
            <div className="bg-[#161b22] border border-[#30363d] rounded-xl p-6 shadow-lg">
              <div className="flex items-center gap-3 mb-6">
                <div className="w-12 h-12 bg-gradient-to-br from-purple-500 to-purple-600 rounded-xl flex items-center justify-center shadow-lg">
                  <Icons.Rocket className="w-6 h-6 text-white" />
                </div>
                <h3 className="text-lg font-semibold text-gray-100">
                  Projetos Concluídos
                </h3>
              </div>
              <div className="flex flex-col items-center justify-center py-8">
                <div className="relative">
                  <div className="absolute inset-0 bg-purple-500/20 blur-2xl rounded-full"></div>
                  <span className="relative text-6xl font-bold text-purple-400">
                    {dashboardData.completedProjects || 0}
                  </span>
                </div>
                <p className="text-gray-400 mt-4 text-center">
                  Projetos completos
                </p>
              </div>
            </div>

            {/* Learning Streak */}
            <div className="bg-[#161b22] border border-[#30363d] rounded-xl p-6 shadow-lg">
              <div className="flex items-center gap-3 mb-6">
                <div className="w-12 h-12 bg-gradient-to-br from-orange-500 to-orange-600 rounded-xl flex items-center justify-center shadow-lg">
                  <Icons.Fire className="w-6 h-6 text-white" />
                </div>
                <h3 className="text-lg font-semibold text-gray-100">
                  Sequência de Dias
                </h3>
              </div>
              <div className="flex items-center justify-center mb-4">
                <div className="text-center">
                  <div className="relative">
                    <div className="absolute inset-0 bg-orange-500/20 blur-2xl rounded-full"></div>
                    <span className="relative text-6xl font-bold text-orange-400">
                      {dashboardData.learningStreak || 0}
                    </span>
                  </div>
                </div>
              </div>
              <p className="text-gray-400 text-center mb-6">
                {(dashboardData.learningStreak || 0) === 1 ? 'dia seguido' : 'dias seguidos'}
              </p>
              
              {/* Calendar Visualization */}
              <div className="flex justify-center gap-2">
                {Array.from({ length: 7 }).map((_, index) => {
                  const isActive = index < Math.min(dashboardData.learningStreak || 0, 7);
                  return (
                    <div
                      key={index}
                      className={`w-8 h-8 rounded-lg transition-all ${
                        isActive
                          ? 'bg-gradient-to-br from-orange-500 to-orange-600 shadow-lg'
                          : 'bg-[#21262d] border border-[#30363d]'
                      }`}
                      title={isActive ? 'Dia ativo' : 'Dia inativo'}
                    ></div>
                  );
                })}
              </div>
              <p className="text-xs text-gray-500 text-center mt-4 uppercase tracking-wider">
                Últimos 7 dias
              </p>
            </div>
          </div>

          {/* Courses in Progress */}
          {(dashboardData.coursesInProgress && dashboardData.coursesInProgress.length > 0) ? (
            <div className="bg-[#161b22] border border-[#30363d] rounded-xl p-8 shadow-lg">
              <div className="flex items-center gap-3 mb-6">
                <div className="w-12 h-12 bg-gradient-to-br from-blue-500 to-blue-600 rounded-xl flex items-center justify-center shadow-lg">
                  <Icons.BookOpen className="w-6 h-6 text-white" />
                </div>
                <h3 className="text-2xl font-semibold text-gray-100">
                  Cursos em Progresso
                </h3>
              </div>
              <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                {dashboardData.coursesInProgress.map((course) => (
                  <div key={course.courseId} className="space-y-3 p-5 bg-[#21262d] rounded-xl border border-[#30363d]">
                    <div className="flex items-center justify-between">
                      <span className="text-base font-semibold text-gray-100">
                        {course.title}
                      </span>
                      <span className="text-2xl font-bold text-blue-400">
                        {(course.completionPercentage || 0).toFixed(0)}%
                      </span>
                    </div>
                    <div className="w-full bg-[#30363d] rounded-full h-3 overflow-hidden">
                      <div
                        className="bg-gradient-to-r from-blue-500 to-blue-600 h-3 rounded-full transition-all duration-500 shadow-lg"
                        style={{ width: `${course.completionPercentage || 0}%` }}
                      ></div>
                    </div>
                  </div>
                ))}
              </div>
            </div>
          ) : (
            <div className="bg-[#161b22] border border-[#30363d] rounded-xl p-12 text-center shadow-lg">
              <div className="w-20 h-20 bg-[#21262d] border border-[#30363d] rounded-xl flex items-center justify-center mx-auto mb-4">
                <Icons.BookOpen className="w-10 h-10 text-gray-500" />
              </div>
              <h3 className="text-xl font-semibold text-gray-100 mb-2">
                Nenhum Curso em Progresso
              </h3>
              <p className="text-gray-400 text-lg">
                Comece um curso para iniciar sua jornada de aprendizado!
              </p>
            </div>
          )}
        </div>
      </div>
    </div>
  );
}
