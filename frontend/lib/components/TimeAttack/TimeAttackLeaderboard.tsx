'use client';

import React from 'react';

interface LeaderboardEntry {
  rank: number;
  userName: string;
  completionTimeSeconds: number;
  bonusXP: number;
  submittedAt: Date;
}

interface TimeAttackLeaderboardProps {
  entries: LeaderboardEntry[];
  currentUserId?: string;
  userBestTime?: {
    completionTimeSeconds: number;
    bonusXP: number;
    rank: number;
  };
}

export const TimeAttackLeaderboard: React.FC<TimeAttackLeaderboardProps> = ({
  entries,
  currentUserId,
  userBestTime,
}) => {
  const formatTime = (seconds: number): string => {
    const mins = Math.floor(seconds / 60);
    const secs = seconds % 60;
    return `${mins}:${secs.toString().padStart(2, '0')}`;
  };

  const formatDate = (date: Date): string => {
    return new Date(date).toLocaleDateString('pt-BR', {
      day: '2-digit',
      month: '2-digit',
      year: 'numeric',
    });
  };

  const getRankBadge = (rank: number): string => {
    switch (rank) {
      case 1:
        return '🥇';
      case 2:
        return '🥈';
      case 3:
        return '🥉';
      default:
        return `#${rank}`;
    }
  };

  return (
    <div className="bg-white rounded-lg shadow-md overflow-hidden">
      <div className="bg-gradient-to-r from-purple-600 to-blue-600 px-6 py-4">
        <h2 className="text-2xl font-bold text-white flex items-center gap-2">
          ⚡ Time Attack Leaderboard
        </h2>
        <p className="text-purple-100 text-sm mt-1">
          Top jogadores mais rápidos
        </p>
      </div>

      {/* User's best time */}
      {userBestTime && (
        <div className="bg-blue-50 border-b border-blue-200 px-6 py-4">
          <div className="flex items-center justify-between">
            <div>
              <p className="text-sm text-gray-600">Seu Melhor Tempo</p>
              <p className="text-2xl font-bold text-blue-600">
                {formatTime(userBestTime.completionTimeSeconds)}
              </p>
            </div>
            <div className="text-right">
              <p className="text-sm text-gray-600">Sua Posição</p>
              <p className="text-2xl font-bold text-blue-600">
                {getRankBadge(userBestTime.rank)}
              </p>
            </div>
            <div className="text-right">
              <p className="text-sm text-gray-600">Bonus XP</p>
              <p className="text-2xl font-bold text-green-600">
                +{userBestTime.bonusXP}
              </p>
            </div>
          </div>
        </div>
      )}

      {/* Leaderboard table */}
      <div className="overflow-x-auto">
        <table className="w-full">
          <thead className="bg-gray-50 border-b border-gray-200">
            <tr>
              <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                Posição
              </th>
              <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                Jogador
              </th>
              <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                Tempo
              </th>
              <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                Bonus XP
              </th>
              <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                Data
              </th>
            </tr>
          </thead>
          <tbody className="bg-white divide-y divide-gray-200">
            {entries.length === 0 ? (
              <tr>
                <td colSpan={5} className="px-6 py-8 text-center text-gray-500">
                  Nenhuma submissão ainda. Seja o primeiro! 🚀
                </td>
              </tr>
            ) : (
              entries.map((entry) => (
                <tr
                  key={entry.rank}
                  className={`hover:bg-gray-50 transition-colors ${
                    entry.rank <= 3 ? 'bg-yellow-50' : ''
                  }`}
                >
                  <td className="px-6 py-4 whitespace-nowrap">
                    <span className="text-2xl font-bold">
                      {getRankBadge(entry.rank)}
                    </span>
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap">
                    <div className="flex items-center">
                      <div className="flex-shrink-0 h-10 w-10 bg-gradient-to-br from-purple-400 to-blue-500 rounded-full flex items-center justify-center text-white font-bold">
                        {entry.userName ? entry.userName.charAt(0).toUpperCase() : '?'}
                      </div>
                      <div className="ml-4">
                        <div className="text-sm font-medium text-gray-900">
                          {entry.userName || 'Usuário Anônimo'}
                        </div>
                      </div>
                    </div>
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap">
                    <span className="text-lg font-mono font-semibold text-gray-900">
                      {formatTime(entry.completionTimeSeconds)}
                    </span>
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap">
                    <span className="inline-flex items-center px-2.5 py-0.5 rounded-full text-xs font-medium bg-green-100 text-green-800">
                      +{entry.bonusXP} XP
                    </span>
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500">
                    {formatDate(entry.submittedAt)}
                  </td>
                </tr>
              ))
            )}
          </tbody>
        </table>
      </div>

      {entries.length > 0 && (
        <div className="bg-gray-50 px-6 py-3 text-center text-sm text-gray-600">
          Mostrando top {entries.length} jogadores
        </div>
      )}
    </div>
  );
};
