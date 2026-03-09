import apiClient from '../api-client';
import { DashboardResponse, LeaderboardResponse } from '../types';

export const progressApi = {
  /**
   * Get user dashboard data
   */
  getDashboard: async (userId?: string): Promise<DashboardResponse> => {
    const response = await apiClient.get<DashboardResponse>('/api/progress/dashboard', {
      params: userId ? { userId } : undefined,
    });
    return response.data;
  },

  /**
   * Get leaderboard
   */
  getLeaderboard: async (): Promise<LeaderboardResponse> => {
    const response = await apiClient.get<LeaderboardResponse>('/api/leaderboard');
    return response.data;
  },
};
