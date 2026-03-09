'use client';

import { useEffect, useState } from 'react';
import { progressApi } from '../api/progress';
import { LeaderboardEntry } from '../types';

export function Leaderboard() {
  const [entries, setEntries] = useState<LeaderboardEntry[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [currentUserId, setCurrentUserId] = useState<string | null>(null);

  useEffect(() => {
    // Get current user ID from localStorage
    if (typeof window !== 'undefined') {
      const userId = localStorage.getItem('user_id');
      setCurrentUserId(userId);
    }

    const fetchLeaderboard = async () => {
      try {
        setLoading(true);
        const data = await progressApi.getLeaderboard();
        setEntries(data.entries);
        setError(null);
      } catch (err) {
        console.error('Failed to fetch leaderboard:', err);
        setError('Failed to load leaderboard. Please try again later.');
      } finally {
        setLoading(false);
      }
    };

    fetchLeaderboard();
  }, []);

  if (loading) {
    return (
      <div className="px-4 py-6 sm:px-0">
        <div className="animate-pulse space-y-4">
          {Array.from({ length: 10 }).map((_, index) => (
            <div key={index} className="h-16 bg-gray-200 dark:bg-gray-700 rounded-lg"></div>
          ))}
        </div>
      </div>
    );
  }

  if (error) {
    return (
      <div className="px-4 py-6 sm:px-0">
        <div className="bg-red-50 dark:bg-red-900/20 border border-red-200 dark:border-red-800 rounded-lg p-4">
          <p className="text-red-800 dark:text-red-200">{error}</p>
        </div>
      </div>
    );
  }

  if (entries.length === 0) {
    return (
      <div className="px-4 py-6 sm:px-0">
        <div className="bg-white dark:bg-gray-800 rounded-lg shadow p-8">
          <p className="text-gray-600 dark:text-gray-400 text-center">
            No leaderboard data available yet. Start solving challenges to appear on the leaderboard!
          </p>
        </div>
      </div>
    );
  }

  const getRankBadgeColor = (rank: number) => {
    if (rank === 1) return 'bg-yellow-500 text-white';
    if (rank === 2) return 'bg-gray-400 text-white';
    if (rank === 3) return 'bg-orange-600 text-white';
    return 'bg-gray-200 dark:bg-gray-700 text-gray-700 dark:text-gray-300';
  };

  const getRankIcon = (rank: number) => {
    if (rank === 1) return '🥇';
    if (rank === 2) return '🥈';
    if (rank === 3) return '🥉';
    return null;
  };

  return (
    <div className="px-4 py-6 sm:px-0">
      <div className="bg-white dark:bg-gray-800 rounded-lg shadow overflow-hidden">
        {/* Header */}
        <div className="bg-gradient-to-r from-blue-600 to-purple-600 px-4 sm:px-6 py-4">
          <h2 className="text-xl sm:text-2xl font-bold text-white">Top 100 Students</h2>
          <p className="text-blue-100 text-xs sm:text-sm mt-1">Ranked by total XP earned</p>
        </div>

        {/* Leaderboard Table - Desktop */}
        <div className="hidden sm:block overflow-x-auto">
          <table className="min-w-full divide-y divide-gray-200 dark:divide-gray-700">
            <thead className="bg-gray-50 dark:bg-gray-900">
              <tr>
                <th
                  scope="col"
                  className="px-6 py-3 text-left text-xs font-medium text-gray-500 dark:text-gray-400 uppercase tracking-wider"
                >
                  Rank
                </th>
                <th
                  scope="col"
                  className="px-6 py-3 text-left text-xs font-medium text-gray-500 dark:text-gray-400 uppercase tracking-wider"
                >
                  Name
                </th>
                <th
                  scope="col"
                  className="px-6 py-3 text-left text-xs font-medium text-gray-500 dark:text-gray-400 uppercase tracking-wider"
                >
                  XP
                </th>
                <th
                  scope="col"
                  className="px-6 py-3 text-left text-xs font-medium text-gray-500 dark:text-gray-400 uppercase tracking-wider"
                >
                  Level
                </th>
              </tr>
            </thead>
            <tbody className="bg-white dark:bg-gray-800 divide-y divide-gray-200 dark:divide-gray-700">
              {entries.map((entry) => {
                const isCurrentUser = currentUserId && entry.name === currentUserId;
                const rankIcon = getRankIcon(entry.rank);

                return (
                  <tr
                    key={entry.rank}
                    className={`${
                      isCurrentUser
                        ? 'bg-blue-50 dark:bg-blue-900/20 border-l-4 border-blue-600'
                        : 'hover:bg-gray-50 dark:hover:bg-gray-700/50'
                    } transition-colors`}
                  >
                    <td className="px-6 py-4 whitespace-nowrap">
                      <div className="flex items-center">
                        <span
                          className={`inline-flex items-center justify-center w-10 h-10 rounded-full font-bold text-sm ${getRankBadgeColor(
                            entry.rank
                          )}`}
                        >
                          {rankIcon || entry.rank}
                        </span>
                      </div>
                    </td>
                    <td className="px-6 py-4 whitespace-nowrap">
                      <div className="flex items-center">
                        <div className="text-sm font-medium text-gray-900 dark:text-white">
                          {entry.name}
                          {isCurrentUser && (
                            <span className="ml-2 inline-flex items-center px-2.5 py-0.5 rounded-full text-xs font-medium bg-blue-100 dark:bg-blue-900 text-blue-800 dark:text-blue-200">
                              You
                            </span>
                          )}
                        </div>
                      </div>
                    </td>
                    <td className="px-6 py-4 whitespace-nowrap">
                      <div className="text-sm font-semibold text-gray-900 dark:text-white">
                        {entry.xp.toLocaleString()} XP
                      </div>
                    </td>
                    <td className="px-6 py-4 whitespace-nowrap">
                      <div className="flex items-center">
                        <span className="inline-flex items-center px-3 py-1 rounded-full text-sm font-medium bg-purple-100 dark:bg-purple-900 text-purple-800 dark:text-purple-200">
                          Level {entry.level}
                        </span>
                      </div>
                    </td>
                  </tr>
                );
              })}
            </tbody>
          </table>
        </div>

        {/* Leaderboard Cards - Mobile */}
        <div className="sm:hidden divide-y divide-gray-200 dark:divide-gray-700">
          {entries.map((entry) => {
            const isCurrentUser = currentUserId && entry.name === currentUserId;
            const rankIcon = getRankIcon(entry.rank);

            return (
              <div
                key={entry.rank}
                className={`p-4 ${
                  isCurrentUser
                    ? 'bg-blue-50 dark:bg-blue-900/20 border-l-4 border-blue-600'
                    : ''
                }`}
              >
                <div className="flex items-center justify-between mb-2">
                  <div className="flex items-center space-x-3">
                    <span
                      className={`inline-flex items-center justify-center w-10 h-10 rounded-full font-bold text-sm ${getRankBadgeColor(
                        entry.rank
                      )}`}
                    >
                      {rankIcon || entry.rank}
                    </span>
                    <div>
                      <div className="text-sm font-medium text-gray-900 dark:text-white">
                        {entry.name}
                      </div>
                      {isCurrentUser && (
                        <span className="inline-flex items-center px-2 py-0.5 rounded-full text-xs font-medium bg-blue-100 dark:bg-blue-900 text-blue-800 dark:text-blue-200">
                          You
                        </span>
                      )}
                    </div>
                  </div>
                  <span className="inline-flex items-center px-2.5 py-1 rounded-full text-xs font-medium bg-purple-100 dark:bg-purple-900 text-purple-800 dark:text-purple-200">
                    Level {entry.level}
                  </span>
                </div>
                <div className="text-sm font-semibold text-gray-900 dark:text-white">
                  {entry.xp.toLocaleString()} XP
                </div>
              </div>
            );
          })}
        </div>

        {/* Footer */}
        <div className="bg-gray-50 dark:bg-gray-900 px-4 sm:px-6 py-4">
          <p className="text-xs text-gray-500 dark:text-gray-400 text-center">
            Leaderboard updates within 1 minute of XP changes
          </p>
        </div>
      </div>
    </div>
  );
}
