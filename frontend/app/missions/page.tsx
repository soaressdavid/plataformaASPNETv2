'use client';

import { useState, useEffect } from 'react';
import { useRouter } from 'next/navigation';
import { ProtectedRoute } from '@/lib/components/ProtectedRoute';
import { Navigation } from '@/lib/components/Navigation';
import { Icons } from '@/lib/components/Icons';
import apiClient from '@/lib/api-client';
import toast from 'react-hot-toast';

interface Mission {
  id: number;
  title: string;
  description: string;
  type: 'daily' | 'weekly';
  xpReward: number;
  progress: number;
  maxProgress: number;
  isCompleted: boolean;
  expiresAt: string;
}

/**
 * Missions Dashboard Page
 * Task 17.5: Display daily and weekly missions, show mission progress, add completion animations
 * Requirements: 16.4, 16.5
 */
export default function MissionsPage() {
  const router = useRouter();
  const [missions, setMissions] = useState<Mission[]>([]);
  const [loading, setLoading] = useState(true);
  const [filter, setFilter] = useState<'all' | 'daily' | 'weekly'>('all');

  useEffect(() => {
    fetchMissions();
  }, []);

  const fetchMissions = async () => {
    try {
      setLoading(true);
      
      // MOCK DATA - Simular missões
      await new Promise(resolve => setTimeout(resolve, 300));
      
      const mockMissions = [
        {
          id: 1,
          title: 'Primeiro Desafio',
          description: 'Complete seu primeiro desafio de programação',
          type: 'challenge',
          targetValue: 1,
          currentProgress: 1,
          xpReward: 100,
          isCompleted: true,
          canClaim: false,
          difficulty: 'Easy',
          category: 'Getting Started',
          deadline: null
        },
        {
          id: 2,
          title: 'Sequência de Estudos',
          description: 'Estude por 7 dias consecutivos',
          type: 'streak',
          targetValue: 7,
          currentProgress: 7,
          xpReward: 200,
          isCompleted: true,
          canClaim: true,
          difficulty: 'Medium',
          category: 'Consistency',
          deadline: null
        },
        {
          id: 3,
          title: 'Mestre dos Algoritmos',
          description: 'Complete 10 desafios de algoritmos',
          type: 'challenge',
          targetValue: 10,
          currentProgress: 6,
          xpReward: 500,
          isCompleted: false,
          canClaim: false,
          difficulty: 'Hard',
          category: 'Algorithms',
          deadline: '2024-03-31'
        },
        {
          id: 4,
          title: 'Explorador de Cursos',
          description: 'Complete 3 cursos diferentes',
          type: 'course',
          targetValue: 3,
          currentProgress: 2,
          xpReward: 300,
          isCompleted: false,
          canClaim: false,
          difficulty: 'Medium',
          category: 'Learning',
          deadline: null
        },
        {
          id: 5,
          title: 'Velocista',
          description: 'Complete um desafio em menos de 2 minutos',
          type: 'time_attack',
          targetValue: 1,
          currentProgress: 0,
          xpReward: 150,
          isCompleted: false,
          canClaim: false,
          difficulty: 'Hard',
          category: 'Speed',
          deadline: null
        }
      ];
      
      setMissions(mockMissions);
    } catch (error) {
      console.error('Failed to fetch missions:', error);
      toast.error('Failed to load missions');
    } finally {
      setLoading(false);
    }
  };

  const handleClaimReward = async (missionId: number) => {
    try {
      // MOCK - Simular claim de recompensa
      await new Promise(resolve => setTimeout(resolve, 500));
      
      const mission = missions.find(m => m.id === missionId);
      if (mission) {
        toast.success(`Reward claimed! +${mission.xpReward} XP`);
        
        // Atualizar a missão localmente
        setMissions(prev => prev.map(m => 
          m.id === missionId 
            ? { ...m, canClaim: false }
            : m
        ));
      }
    } catch (error) {
      console.error('Failed to claim reward:', error);
      toast.error('Failed to claim reward');
    }
  };

  const filteredMissions = missions.filter((mission) => {
    if (filter === 'all') return true;
    return mission.type === filter;
  });

  const dailyMissions = missions.filter((m) => m.type === 'daily');
  const weeklyMissions = missions.filter((m) => m.type === 'weekly');
  const completedDaily = dailyMissions.filter((m) => m.isCompleted).length;
  const completedWeekly = weeklyMissions.filter((m) => m.isCompleted).length;

  const getTimeRemaining = (expiresAt: string) => {
    const now = new Date();
    const expiry = new Date(expiresAt);
    const diff = expiry.getTime() - now.getTime();

    if (diff <= 0) return 'Expired';

    const hours = Math.floor(diff / (1000 * 60 * 60));
    const minutes = Math.floor((diff % (1000 * 60 * 60)) / (1000 * 60));

    if (hours > 24) {
      const days = Math.floor(hours / 24);
      return `${days}d ${hours % 24}h`;
    }

    return `${hours}h ${minutes}m`;
  };

  return (
    <ProtectedRoute>
      <div className="min-h-screen bg-gray-50">
        <Navigation />

        <main className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
          {/* Header */}
          <div className="mb-8">
            <h1 className="text-3xl font-bold text-gray-900 mb-2">
              Daily & Weekly Missions
            </h1>
            <p className="text-gray-600">
              Complete missions to earn bonus XP and level up faster
            </p>
          </div>

          {/* Stats Cards */}
          <div className="grid grid-cols-1 md:grid-cols-2 gap-6 mb-8">
            {/* Daily Missions Card */}
            <div className="bg-linear-to-br from-blue-500 to-blue-600 rounded-lg shadow-lg p-6 text-white">
              <div className="flex items-center justify-between mb-4">
                <div>
                  <h2 className="text-xl font-semibold mb-1">Daily Missions</h2>
                  <p className="text-blue-100 text-sm">Reset in {getTimeRemaining(new Date(Date.now() + 24 * 60 * 60 * 1000).toISOString())}</p>
                </div>
                <Icons.Calendar className="w-12 h-12 text-blue-200" />
              </div>
              <div className="flex items-baseline gap-2">
                <span className="text-4xl font-bold">{completedDaily}</span>
                <span className="text-xl text-blue-100">/ {dailyMissions.length}</span>
              </div>
              <div className="mt-4 w-full bg-blue-400 rounded-full h-2">
                <div
                  className="bg-white h-2 rounded-full transition-all duration-500"
                  style={{
                    width: `${dailyMissions.length > 0 ? (completedDaily / dailyMissions.length) * 100 : 0}%`,
                  }}
                />
              </div>
            </div>

            {/* Weekly Missions Card */}
            <div className="bg-linear-to-br from-purple-500 to-purple-600 rounded-lg shadow-lg p-6 text-white">
              <div className="flex items-center justify-between mb-4">
                <div>
                  <h2 className="text-xl font-semibold mb-1">Weekly Missions</h2>
                  <p className="text-purple-100 text-sm">Reset in {getTimeRemaining(new Date(Date.now() + 7 * 24 * 60 * 60 * 1000).toISOString())}</p>
                </div>
                <Icons.Trophy className="w-12 h-12 text-purple-200" />
              </div>
              <div className="flex items-baseline gap-2">
                <span className="text-4xl font-bold">{completedWeekly}</span>
                <span className="text-xl text-purple-100">/ {weeklyMissions.length}</span>
              </div>
              <div className="mt-4 w-full bg-purple-400 rounded-full h-2">
                <div
                  className="bg-white h-2 rounded-full transition-all duration-500"
                  style={{
                    width: `${weeklyMissions.length > 0 ? (completedWeekly / weeklyMissions.length) * 100 : 0}%`,
                  }}
                />
              </div>
            </div>
          </div>

          {/* Filters */}
          <div className="bg-white rounded-lg shadow-sm p-4 mb-6">
            <div className="flex gap-2">
              <button
                onClick={() => setFilter('all')}
                className={`px-4 py-2 rounded-lg font-medium transition-colors ${
                  filter === 'all'
                    ? 'bg-primary-600 text-white'
                    : 'bg-gray-100 text-gray-700 hover:bg-gray-200'
                }`}
              >
                All Missions
              </button>
              <button
                onClick={() => setFilter('daily')}
                className={`px-4 py-2 rounded-lg font-medium transition-colors ${
                  filter === 'daily'
                    ? 'bg-blue-600 text-white'
                    : 'bg-gray-100 text-gray-700 hover:bg-gray-200'
                }`}
              >
                Daily ({dailyMissions.length})
              </button>
              <button
                onClick={() => setFilter('weekly')}
                className={`px-4 py-2 rounded-lg font-medium transition-colors ${
                  filter === 'weekly'
                    ? 'bg-purple-600 text-white'
                    : 'bg-gray-100 text-gray-700 hover:bg-gray-200'
                }`}
              >
                Weekly ({weeklyMissions.length})
              </button>
            </div>
          </div>

          {/* Loading State */}
          {loading && (
            <div className="space-y-4">
              {[...Array(4)].map((_, i) => (
                <div key={i} className="bg-white rounded-lg shadow-sm p-6 animate-pulse">
                  <div className="h-6 bg-gray-200 rounded mb-2 w-1/3" />
                  <div className="h-4 bg-gray-200 rounded mb-4 w-2/3" />
                  <div className="h-2 bg-gray-200 rounded" />
                </div>
              ))}
            </div>
          )}

          {/* Missions List */}
          {!loading && filteredMissions.length > 0 && (
            <div className="space-y-4">
              {filteredMissions.map((mission) => {
                const progressPercentage = (mission.progress / mission.maxProgress) * 100;
                const isCompleted = mission.isCompleted;

                return (
                  <div
                    key={mission.id}
                    className={`bg-white rounded-lg shadow-sm p-6 transition-all hover:shadow-md ${
                      isCompleted ? 'border-2 border-success-500' : ''
                    }`}
                  >
                    <div className="flex items-start justify-between mb-4">
                      <div className="flex-1">
                        <div className="flex items-center gap-3 mb-2">
                          <span
                            className={`px-3 py-1 rounded-full text-xs font-medium ${
                              mission.type === 'daily'
                                ? 'bg-blue-100 text-blue-700'
                                : 'bg-purple-100 text-purple-700'
                            }`}
                          >
                            {mission.type === 'daily' ? 'Daily' : 'Weekly'}
                          </span>
                          {isCompleted && (
                            <span className="flex items-center gap-1 text-success-600 text-sm font-medium">
                              <Icons.Check className="w-4 h-4" />
                              Completed
                            </span>
                          )}
                        </div>
                        <h3 className="text-lg font-semibold text-gray-900 mb-1">
                          {mission.title}
                        </h3>
                        <p className="text-sm text-gray-600 mb-3">
                          {mission.description}
                        </p>
                        <div className="flex items-center gap-4 text-sm text-gray-500">
                          <div className="flex items-center gap-1">
                            <Icons.Star className="w-4 h-4 text-yellow-500" />
                            <span className="font-medium">+{mission.xpReward} XP</span>
                          </div>
                          <div className="flex items-center gap-1">
                            <Icons.Clock className="w-4 h-4" />
                            <span>{getTimeRemaining(mission.expiresAt)}</span>
                          </div>
                        </div>
                      </div>

                      {isCompleted && (
                        <button
                          onClick={() => handleClaimReward(mission.id)}
                          className="px-4 py-2 bg-success-600 text-white rounded-lg hover:bg-success-700 transition-colors font-medium"
                        >
                          Claim Reward
                        </button>
                      )}
                    </div>

                    {/* Progress Bar */}
                    <div>
                      <div className="flex justify-between text-xs text-gray-600 mb-1">
                        <span>Progress</span>
                        <span>
                          {mission.progress} / {mission.maxProgress}
                        </span>
                      </div>
                      <div className="w-full bg-gray-200 rounded-full h-3">
                        <div
                          className={`h-3 rounded-full transition-all duration-500 ${
                            isCompleted ? 'bg-success-600' : 'bg-primary-600'
                          }`}
                          style={{ width: `${progressPercentage}%` }}
                        />
                      </div>
                    </div>
                  </div>
                );
              })}
            </div>
          )}

          {/* Empty State */}
          {!loading && filteredMissions.length === 0 && (
            <div className="bg-white rounded-lg shadow-sm p-12 text-center">
              <Icons.Target className="w-16 h-16 text-gray-300 mx-auto mb-4" />
              <h3 className="text-lg font-semibold text-gray-900 mb-2">
                No missions available
              </h3>
              <p className="text-gray-600 mb-6">
                Check back later for new missions or start completing challenges!
              </p>
              <button
                onClick={() => router.push('/challenges')}
                className="px-6 py-3 bg-primary-600 text-white rounded-lg hover:bg-primary-700 transition-colors"
              >
                Browse Challenges
              </button>
            </div>
          )}
        </main>
      </div>
    </ProtectedRoute>
  );
}
