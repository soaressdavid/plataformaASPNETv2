import apiClient from '../api-client';

export interface TelemetryEvent {
  userId: string;
  eventType: string;
  eventName: string;
  properties?: Record<string, any>;
  timestamp?: string;
}

export interface LessonMetrics {
  lessonId: string;
  totalViews: number;
  totalCompletions: number;
  completionRate: number;
  averageTimeSpent: number;
  averageScore: number;
}

export interface RetentionMetrics {
  period: string;
  totalUsers: number;
  activeUsers: number;
  retentionRate: number;
  churnRate: number;
}

export interface DashboardMetrics {
  totalUsers: number;
  activeUsers: number;
  totalLessons: number;
  totalCompletions: number;
  averageCompletionRate: number;
  averageEngagementTime: number;
  topLessons: Array<{
    lessonId: string;
    title: string;
    completions: number;
  }>;
  recentActivity: Array<{
    userId: string;
    eventType: string;
    timestamp: string;
  }>;
}

export const analyticsApi = {
  /**
   * Track a telemetry event
   */
  trackEvent: async (event: TelemetryEvent): Promise<void> => {
    await apiClient.post('/api/analytics/track', event);
  },

  /**
   * Track lesson completion
   */
  trackLessonCompletion: async (data: {
    userId: string;
    lessonId: string;
    courseId: string;
    completedAt: string;
    timeSpent: number;
    score?: number;
  }): Promise<void> => {
    await apiClient.post('/api/analytics/track/lesson-completion', data);
  },

  /**
   * Track challenge completion
   */
  trackChallengeCompletion: async (data: {
    userId: string;
    challengeId: string;
    completedAt: string;
    timeSpent: number;
    score: number;
    attempts: number;
  }): Promise<void> => {
    await apiClient.post('/api/analytics/track/challenge-completion', data);
  },

  /**
   * Track content view
   */
  trackContentView: async (data: {
    userId: string;
    contentId: string;
    contentType: string;
    viewedAt: string;
    duration: number;
  }): Promise<void> => {
    await apiClient.post('/api/analytics/track/content-view', data);
  },

  /**
   * Get lesson metrics
   */
  getLessonMetrics: async (lessonId: string): Promise<LessonMetrics> => {
    const response = await apiClient.get<LessonMetrics>(`/api/analytics/lessons/${lessonId}/metrics`);
    return response.data;
  },

  /**
   * Get retention metrics
   */
  getRetentionMetrics: async (startDate?: string, endDate?: string): Promise<RetentionMetrics[]> => {
    const params = new URLSearchParams();
    if (startDate) params.append('startDate', startDate);
    if (endDate) params.append('endDate', endDate);
    
    const url = params.toString() ? `/api/analytics/retention?${params}` : '/api/analytics/retention';
    const response = await apiClient.get<RetentionMetrics[]>(url);
    return response.data;
  },

  /**
   * Get dashboard metrics - MOCK VERSION
   */
  getDashboardMetrics: async (): Promise<DashboardMetrics> => {
    await new Promise(resolve => setTimeout(resolve, 500));
    
    return {
      totalUsers: 1247,
      activeUsers: 892,
      totalLessons: 178,
      totalCompletions: 3456,
      averageCompletionRate: 78.5,
      averageEngagementTime: 1847, // seconds
      topLessons: [
        { lessonId: '1', title: 'Introdução ao C#', completions: 456 },
        { lessonId: '13', title: 'Introdução ao SQL', completions: 398 },
        { lessonId: '36', title: 'Criando sua Primeira API', completions: 287 },
        { lessonId: '59', title: 'Introdução ao Azure', completions: 234 },
        { lessonId: '64', title: 'Introdução ao DevOps', completions: 198 }
      ],
      recentActivity: [
        { userId: 'user1', eventType: 'lesson_completed', timestamp: new Date(Date.now() - 5 * 60 * 1000).toISOString() },
        { userId: 'user2', eventType: 'challenge_completed', timestamp: new Date(Date.now() - 12 * 60 * 1000).toISOString() },
        { userId: 'user3', eventType: 'course_started', timestamp: new Date(Date.now() - 18 * 60 * 1000).toISOString() },
        { userId: 'user4', eventType: 'lesson_completed', timestamp: new Date(Date.now() - 25 * 60 * 1000).toISOString() },
        { userId: 'user5', eventType: 'project_completed', timestamp: new Date(Date.now() - 32 * 60 * 1000).toISOString() }
      ]
    };
  },

  /**
   * Get user engagement metrics
   */
  getUserEngagement: async (userId: string): Promise<any> => {
    const response = await apiClient.get(`/api/analytics/users/${userId}/engagement`);
    return response.data;
  },
};
