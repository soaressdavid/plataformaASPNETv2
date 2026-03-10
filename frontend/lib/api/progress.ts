import { DashboardResponse, LeaderboardResponse } from '../types';

// MOCK DATA - Funciona sem backend
const mockLeaderboardEntries = [
  { rank: 1, name: 'CodeMaster', xp: 15420, level: 12 },
  { rank: 2, name: 'DevNinja', xp: 14850, level: 11 },
  { rank: 3, name: 'AlgoExpert', xp: 13920, level: 11 },
  { rank: 4, name: 'ByteWizard', xp: 12750, level: 10 },
  { rank: 5, name: 'LogicLord', xp: 11680, level: 10 },
  { rank: 6, name: 'CodeCrusher', xp: 10950, level: 9 },
  { rank: 7, name: 'DataDragon', xp: 10200, level: 9 },
  { rank: 8, name: 'ScriptSage', xp: 9850, level: 8 },
  { rank: 9, name: 'BugHunter', xp: 9320, level: 8 },
  { rank: 10, name: 'CodeCrafter', xp: 8750, level: 8 },
  { rank: 11, name: 'DevDynamo', xp: 8200, level: 7 },
  { rank: 12, name: 'TechTitan', xp: 7850, level: 7 },
  { rank: 13, name: 'PixelPro', xp: 7320, level: 7 },
  { rank: 14, name: 'WebWarrior', xp: 6950, level: 6 },
  { rank: 15, name: 'AppAce', xp: 6420, level: 6 },
  { rank: 16, name: 'CloudChamp', xp: 5980, level: 6 },
  { rank: 17, name: 'DataDiver', xp: 5650, level: 5 },
  { rank: 18, name: 'CodeCadet', xp: 5200, level: 5 },
  { rank: 19, name: 'DevDreamer', xp: 4850, level: 5 },
  { rank: 20, name: 'TechTrainee', xp: 4320, level: 4 }
];

const mockDashboardData: DashboardResponse = {
  currentXP: 8750,
  currentLevel: 8,
  xpToNextLevel: 1250,
  solvedChallenges: {
    easy: 12,
    medium: 6,
    hard: 2
  },
  completedProjects: 3,
  learningStreak: 7,
  coursesInProgress: [
    {
      courseId: '1',
      title: 'Fundamentos de C#',
      completionPercentage: 85.5
    },
    {
      courseId: '5',
      title: 'ASP.NET Core Web API',
      completionPercentage: 42.3
    }
  ]
};

export const progressApi = {
  /**
   * Get user dashboard data - MOCK VERSION
   */
  getDashboard: async (userId?: string): Promise<DashboardResponse> => {
    // Simular delay de rede
    await new Promise(resolve => setTimeout(resolve, 400));
    
    return mockDashboardData;
  },

  /**
   * Get leaderboard - MOCK VERSION
   */
  getLeaderboard: async (): Promise<LeaderboardResponse> => {
    // Simular delay de rede
    await new Promise(resolve => setTimeout(resolve, 300));
    
    return {
      entries: mockLeaderboardEntries,
      userRank: 16,
      totalUsers: 1247
    };
  },
};
