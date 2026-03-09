'use client';

import { useState, useEffect } from 'react';
import { useRouter } from 'next/navigation';
import { ProtectedRoute } from '@/lib/components/ProtectedRoute';
import { Navigation } from '@/lib/components/Navigation';
import { Icons } from '@/lib/components/Icons';
import { apiClient } from '@/lib/api-client';
import toast from 'react-hot-toast';

interface Achievement {
  id: number;
  name: string;
  description: string;
  badgeIcon: string;
  xpReward: number;
  isUnlocked: boolean;
  unlockedAt?: string;
  progress: number;
  maxProgress: number;
  category: string;
}

/**
 * Achievements and Badges Page
 * Task 17.4: Display all available badges, show earned badges with unlock date, add progress bars
 * Requirements: 15.5, 15.6
 */
export default function AchievementsPage() {
  const router = useRouter();
  const [achievements, setAchievements] = useState<Achievement[]>([]);
  const [loading, setLoading] = useState(true);
  const [filter, setFilter] = useState<'all' | 'unlocked' | 'locked'>('all');
  const [categoryFilter, setCategoryFilter] = useState<string>('all');

  useEffect(() => {
    fetchAchievements();
  }, []);

  const fetchAchievements = async () => {
    try {
      setLoading(true);
      const response = await apiClient.get('/api/gamification/achievements');
      setAchievements(response.data);
    } catch (error: any) {
      console.error('Failed to fetch achievements:', error);
      toast.error('Failed to load achievements');
    } finally {
      setLoading(false);
    }
  };

  const filteredAchievements = achievements.filter((achievement) => {
    if (filter === 'unlocked' && !achievement.isUnlocked) return false;
    if (filter === 'locked' && achievement.isUnlocked) return false;
    if (categoryFilter !== 'all' && achievement.category !== categoryFilter) return false;
    return true;
  });

  const categories = ['all', ...Array.from(new Set(achievements.map((a) => a.category)))];
  const unlockedCount = achievements.filter((a) => a.isUnlocked).length;
  const totalCount = achievements.length;
  const completionPercentage = totalCount > 0 ? (unlockedCount / totalCount) * 100 : 0;

  return (
    <ProtectedRoute>
      <div className="min-h-screen bg-gray-50">
        <Navigation />

        <main className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
          {/* Header */}
          <div className="mb-8">
            <h1 className="text-3xl font-bold text-gray-900 mb-2">
              Achievements & Badges
            </h1>
            <p className="text-gray-600">
              Unlock achievements by completing challenges and reaching milestones
            </p>
          </div>

          {/* Stats Card */}
          <div className="bg-white rounded-lg shadow-sm p-6 mb-8">
            <div className="flex items-center justify-between mb-4">
              <div>
                <h2 className="text-xl font-semibold text-gray-900">
                  Your Progress
                </h2>
                <p className="text-gray-600">
                  {unlockedCount} of {totalCount} achievements unlocked
                </p>
              </div>
              <div className="text-4xl font-bold text-primary-600">
                {completionPercentage.toFixed(0)}%
              </div>
            </div>

            {/* Progress Bar */}
            <div className="w-full bg-gray-200 rounded-full h-3">
              <div
                className="bg-primary-600 h-3 rounded-full transition-all duration-500"
                style={{ width: `${completionPercentage}%` }}
              />
            </div>
          </div>

          {/* Filters */}
          <div className="bg-white rounded-lg shadow-sm p-4 mb-6">
            <div className="flex flex-col sm:flex-row gap-4">
              {/* Status Filter */}
              <div className="flex gap-2">
                <button
                  onClick={() => setFilter('all')}
                  className={`px-4 py-2 rounded-lg font-medium transition-colors ${
                    filter === 'all'
                      ? 'bg-primary-600 text-white'
                      : 'bg-gray-100 text-gray-700 hover:bg-gray-200'
                  }`}
                >
                  All
                </button>
                <button
                  onClick={() => setFilter('unlocked')}
                  className={`px-4 py-2 rounded-lg font-medium transition-colors ${
                    filter === 'unlocked'
                      ? 'bg-success-600 text-white'
                      : 'bg-gray-100 text-gray-700 hover:bg-gray-200'
                  }`}
                >
                  Unlocked ({unlockedCount})
                </button>
                <button
                  onClick={() => setFilter('locked')}
                  className={`px-4 py-2 rounded-lg font-medium transition-colors ${
                    filter === 'locked'
                      ? 'bg-gray-600 text-white'
                      : 'bg-gray-100 text-gray-700 hover:bg-gray-200'
                  }`}
                >
                  Locked ({totalCount - unlockedCount})
                </button>
              </div>

              {/* Category Filter */}
              <select
                value={categoryFilter}
                onChange={(e) => setCategoryFilter(e.target.value)}
                className="px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-primary-500 focus:border-transparent"
              >
                {categories.map((category) => (
                  <option key={category} value={category}>
                    {category === 'all' ? 'All Categories' : category}
                  </option>
                ))}
              </select>
            </div>
          </div>

          {/* Loading State */}
          {loading && (
            <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
              {[...Array(6)].map((_, i) => (
                <div key={i} className="bg-white rounded-lg shadow-sm p-6 animate-pulse">
                  <div className="w-16 h-16 bg-gray-200 rounded-full mb-4" />
                  <div className="h-6 bg-gray-200 rounded mb-2" />
                  <div className="h-4 bg-gray-200 rounded mb-4" />
                  <div className="h-2 bg-gray-200 rounded" />
                </div>
              ))}
            </div>
          )}

          {/* Achievements Grid */}
          {!loading && filteredAchievements.length > 0 && (
            <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
              {filteredAchievements.map((achievement) => (
                <div
                  key={achievement.id}
                  className={`bg-white rounded-lg shadow-sm p-6 transition-all hover:shadow-md ${
                    achievement.isUnlocked ? 'border-2 border-success-500' : 'opacity-75'
                  }`}
                >
                  {/* Badge Icon */}
                  <div className="flex items-center justify-between mb-4">
                    <div
                      className={`w-16 h-16 rounded-full flex items-center justify-center text-3xl ${
                        achievement.isUnlocked
                          ? 'bg-success-100 text-success-600'
                          : 'bg-gray-100 text-gray-400'
                      }`}
                    >
                      {achievement.badgeIcon}
                    </div>
                    {achievement.isUnlocked && (
                      <div className="flex items-center gap-1 text-success-600">
                        <Icons.Check className="w-5 h-5" />
                        <span className="text-sm font-medium">Unlocked</span>
                      </div>
                    )}
                  </div>

                  {/* Achievement Info */}
                  <h3 className="text-lg font-semibold text-gray-900 mb-2">
                    {achievement.name}
                  </h3>
                  <p className="text-sm text-gray-600 mb-4">
                    {achievement.description}
                  </p>

                  {/* XP Reward */}
                  <div className="flex items-center gap-2 mb-4">
                    <Icons.Star className="w-5 h-5 text-yellow-500" />
                    <span className="text-sm font-medium text-gray-700">
                      +{achievement.xpReward} XP
                    </span>
                  </div>

                  {/* Progress Bar (for locked achievements) */}
                  {!achievement.isUnlocked && achievement.maxProgress > 0 && (
                    <div>
                      <div className="flex justify-between text-xs text-gray-600 mb-1">
                        <span>Progress</span>
                        <span>
                          {achievement.progress} / {achievement.maxProgress}
                        </span>
                      </div>
                      <div className="w-full bg-gray-200 rounded-full h-2">
                        <div
                          className="bg-primary-600 h-2 rounded-full transition-all"
                          style={{
                            width: `${(achievement.progress / achievement.maxProgress) * 100}%`,
                          }}
                        />
                      </div>
                    </div>
                  )}

                  {/* Unlock Date */}
                  {achievement.isUnlocked && achievement.unlockedAt && (
                    <div className="text-xs text-gray-500 mt-4">
                      Unlocked on {new Date(achievement.unlockedAt).toLocaleDateString()}
                    </div>
                  )}
                </div>
              ))}
            </div>
          )}

          {/* Empty State */}
          {!loading && filteredAchievements.length === 0 && (
            <div className="bg-white rounded-lg shadow-sm p-12 text-center">
              <Icons.Trophy className="w-16 h-16 text-gray-300 mx-auto mb-4" />
              <h3 className="text-lg font-semibold text-gray-900 mb-2">
                No achievements found
              </h3>
              <p className="text-gray-600 mb-6">
                Try adjusting your filters or start completing challenges to unlock achievements!
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
