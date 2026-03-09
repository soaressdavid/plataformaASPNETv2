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
   * Get dashboard metrics
   */
  getDashboardMetrics: async (): Promise<DashboardMetrics> => {
    const response = await apiClient.get<DashboardMetrics>('/api/analytics/dashboard');
    return response.data;
  },

  /**
   * Get user engagement metrics
   */
  getUserEngagement: async (userId: string): Promise<any> => {
    const response = await apiClient.get(`/api/analytics/users/${userId}/engagement`);
    return response.data;
  },
};
