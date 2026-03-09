'use client';

import React, { useState, useEffect } from 'react';
import { analyticsApi, DashboardMetrics } from '@/lib/api/analytics';
import { Navigation, Icons } from '@/lib/components';
import toast from 'react-hot-toast';

export default function AnalyticsPage() {
  const [metrics, setMetrics] = useState<DashboardMetrics | null>(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    loadMetrics();
  }, []);

  const loadMetrics = async () => {
    try {
      setLoading(true);
      const data = await analyticsApi.getDashboardMetrics();
      setMetrics(data);
    } catch (error) {
      console.error('Error loading metrics:', error);
      toast.error('Erro ao carregar métricas');
    } finally {
      setLoading(false);
    }
  };

  if (loading) {
    return (
      <div className="min-h-screen bg-gray-50">
        <Navigation />
        <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
          <div className="text-center py-12">
            <div className="inline-block animate-spin rounded-full h-8 w-8 border-b-2 border-blue-600"></div>
            <p className="mt-2 text-gray-600">Carregando métricas...</p>
          </div>
        </div>
      </div>
    );
  }

  if (!metrics) {
    return (
      <div className="min-h-screen bg-gray-50">
        <Navigation />
        <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
          <div className="bg-white rounded-lg shadow p-8 text-center">
            <p className="text-gray-600">Erro ao carregar métricas</p>
            <button
              onClick={loadMetrics}
              className="mt-4 px-4 py-2 bg-blue-600 text-white rounded hover:bg-blue-700"
            >
              Tentar novamente
            </button>
          </div>
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
          <h1 className="text-3xl font-bold text-gray-900 mb-2">Analytics Dashboard</h1>
          <p className="text-gray-600">Métricas e estatísticas da plataforma</p>
        </div>

        {/* Key Metrics */}
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6 mb-8">
          <div className="bg-white rounded-lg shadow p-6">
            <div className="flex items-center justify-between mb-2">
              <h3 className="text-sm font-medium text-gray-600">Total de Usuários</h3>
              <Icons.Users className="w-8 h-8 text-gray-400" />
            </div>
            <p className="text-3xl font-bold text-gray-900">{Number(metrics.totalUsers || 0).toLocaleString()}</p>
            <p className="text-sm text-gray-500 mt-1">Usuários registrados</p>
          </div>

          <div className="bg-white rounded-lg shadow p-6">
            <div className="flex items-center justify-between mb-2">
              <h3 className="text-sm font-medium text-gray-600">Usuários Ativos</h3>
              <Icons.Activity className="w-8 h-8 text-green-500" />
            </div>
            <p className="text-3xl font-bold text-green-600">{Number(metrics.activeUsers || 0).toLocaleString()}</p>
            <p className="text-sm text-gray-500 mt-1">
              {(metrics.totalUsers || 0) > 0 ? (((metrics.activeUsers || 0) / (metrics.totalUsers || 1)) * 100).toFixed(1) : '0.0'}% do total
            </p>
          </div>

          <div className="bg-white rounded-lg shadow p-6">
            <div className="flex items-center justify-between mb-2">
              <h3 className="text-sm font-medium text-gray-600">Total de Aulas</h3>
              <Icons.BookOpen className="w-8 h-8 text-blue-500" />
            </div>
            <p className="text-3xl font-bold text-blue-600">{Number(metrics.totalLessons || 0).toLocaleString()}</p>
            <p className="text-sm text-gray-500 mt-1">Aulas disponíveis</p>
          </div>

          <div className="bg-white rounded-lg shadow p-6">
            <div className="flex items-center justify-between mb-2">
              <h3 className="text-sm font-medium text-gray-600">Conclusões</h3>
              <Icons.CheckCircle className="w-8 h-8 text-purple-500" />
            </div>
            <p className="text-3xl font-bold text-purple-600">{Number(metrics.totalCompletions || 0).toLocaleString()}</p>
            <p className="text-sm text-gray-500 mt-1">Aulas concluídas</p>
          </div>
        </div>

        {/* Performance Metrics */}
        <div className="grid grid-cols-1 md:grid-cols-2 gap-6 mb-8">
          <div className="bg-white rounded-lg shadow p-6">
            <h3 className="text-lg font-semibold text-gray-900 mb-4">Taxa de Conclusão</h3>
            <div className="flex items-center justify-center">
              <div className="relative w-32 h-32">
                <svg className="transform -rotate-90 w-32 h-32">
                  <circle
                    cx="64"
                    cy="64"
                    r="56"
                    stroke="#e5e7eb"
                    strokeWidth="12"
                    fill="none"
                  />
                  <circle
                    cx="64"
                    cy="64"
                    r="56"
                    stroke="#3b82f6"
                    strokeWidth="12"
                    fill="none"
                    strokeDasharray={`${2 * Math.PI * 56}`}
                    strokeDashoffset={`${2 * Math.PI * 56 * (1 - (metrics.averageCompletionRate || 0))}`}
                    strokeLinecap="round"
                  />
                </svg>
                <div className="absolute inset-0 flex items-center justify-center">
                  <span className="text-2xl font-bold text-gray-900">
                    {((metrics.averageCompletionRate || 0) * 100).toFixed(1)}%
                  </span>
                </div>
              </div>
            </div>
            <p className="text-center text-sm text-gray-600 mt-4">
              Taxa média de conclusão de aulas
            </p>
          </div>

          <div className="bg-white rounded-lg shadow p-6">
            <h3 className="text-lg font-semibold text-gray-900 mb-4">Tempo de Engajamento</h3>
            <div className="flex items-center justify-center">
              <div className="text-center">
                <p className="text-5xl font-bold text-blue-600">
                  {Math.floor((metrics.averageEngagementTime || 0) / 60)}
                </p>
                <p className="text-lg text-gray-600 mt-2">minutos</p>
              </div>
            </div>
            <p className="text-center text-sm text-gray-600 mt-4">
              Tempo médio de engajamento por sessão
            </p>
          </div>
        </div>

        {/* Top Lessons */}
        <div className="bg-white rounded-lg shadow p-6 mb-8">
          <h3 className="text-lg font-semibold text-gray-900 mb-4">Aulas Mais Populares</h3>
          {metrics.topLessons && metrics.topLessons.length > 0 ? (
            <div className="space-y-3">
              {metrics.topLessons.map((lesson, index) => (
                <div key={lesson.lessonId} className="flex items-center justify-between p-3 bg-gray-50 rounded-lg">
                  <div className="flex items-center gap-3">
                    <span className="text-2xl font-bold text-gray-400 flex items-center gap-1">
                      <Icons.Award className="w-5 h-5" />
                      {index + 1}
                    </span>
                    <div>
                      <p className="font-medium text-gray-900">{lesson.title}</p>
                      <p className="text-sm text-gray-600">{lesson.completions} conclusões</p>
                    </div>
                  </div>
                  <div className="text-right">
                    <div className="w-24 bg-gray-200 rounded-full h-2">
                      <div
                        className="bg-blue-600 h-2 rounded-full"
                        style={{
                          width: `${metrics.topLessons[0]?.completions ? (lesson.completions / metrics.topLessons[0].completions) * 100 : 0}%`
                        }}
                      ></div>
                    </div>
                  </div>
                </div>
              ))}
            </div>
          ) : (
            <p className="text-gray-600 text-center py-4">Nenhum dado disponível</p>
          )}
        </div>

        {/* Recent Activity */}
        <div className="bg-white rounded-lg shadow p-6">
          <h3 className="text-lg font-semibold text-gray-900 mb-4">Atividade Recente</h3>
          {metrics.recentActivity && metrics.recentActivity.length > 0 ? (
            <div className="space-y-2">
              {metrics.recentActivity.slice(0, 10).map((activity, index) => (
                <div key={index} className="flex items-center justify-between p-2 hover:bg-gray-50 rounded">
                  <div className="flex items-center gap-3">
                    <span className="text-sm text-gray-600">
                      {new Date(activity.timestamp).toLocaleString('pt-BR')}
                    </span>
                    <span className="text-sm font-medium text-gray-900">{activity.eventType}</span>
                  </div>
                  <span className="text-xs text-gray-500">{activity.userId.substring(0, 8)}...</span>
                </div>
              ))}
            </div>
          ) : (
            <p className="text-gray-600 text-center py-4">Nenhuma atividade recente</p>
          )}
        </div>
      </div>
    </div>
  );
}
