import apiClient from '../api-client';

export interface Notification {
  id: string;
  userId: string;
  type: 'info' | 'success' | 'warning' | 'error' | 'achievement' | 'message';
  title: string;
  message: string;
  data?: Record<string, any>;
  isRead: boolean;
  createdAt: string;
  readAt?: string;
  actionUrl?: string;
}

export interface NotificationPreferences {
  emailNotifications: boolean;
  pushNotifications: boolean;
  achievementNotifications: boolean;
  messageNotifications: boolean;
  forumNotifications: boolean;
  collaborationNotifications: boolean;
}

export const notificationsApi = {
  /**
   * Get all notifications for current user
   */
  getAll: async (params?: {
    page?: number;
    limit?: number;
    unreadOnly?: boolean;
  }): Promise<{ notifications: Notification[]; total: number; unreadCount: number }> => {
    const response = await apiClient.get<{
      notifications: Notification[];
      total: number;
      unreadCount: number;
    }>('/api/notifications', { params });
    return response.data;
  },

  /**
   * Get unread notification count
   */
  getUnreadCount: async (): Promise<number> => {
    const response = await apiClient.get<{ count: number }>('/api/notifications/unread/count');
    return response.data.count;
  },

  /**
   * Mark a notification as read
   */
  markAsRead: async (notificationId: string): Promise<void> => {
    await apiClient.put(`/api/notifications/${notificationId}/read`);
  },

  /**
   * Mark all notifications as read
   */
  markAllAsRead: async (): Promise<void> => {
    await apiClient.put('/api/notifications/read-all');
  },

  /**
   * Delete a notification
   */
  delete: async (notificationId: string): Promise<void> => {
    await apiClient.delete(`/api/notifications/${notificationId}`);
  },

  /**
   * Delete all read notifications
   */
  deleteAllRead: async (): Promise<void> => {
    await apiClient.delete('/api/notifications/read');
  },

  /**
   * Get notification preferences
   */
  getPreferences: async (): Promise<NotificationPreferences> => {
    const response = await apiClient.get<NotificationPreferences>(
      '/api/notifications/preferences'
    );
    return response.data;
  },

  /**
   * Update notification preferences
   */
  updatePreferences: async (
    preferences: Partial<NotificationPreferences>
  ): Promise<NotificationPreferences> => {
    const response = await apiClient.put<NotificationPreferences>(
      '/api/notifications/preferences',
      preferences
    );
    return response.data;
  },

  /**
   * Get user notifications (compatibility with new backend)
   */
  getUserNotifications: async (
    userId: string,
    page: number = 1,
    limit: number = 20,
    unreadOnly: boolean = false
  ): Promise<{ notifications: Notification[]; total: number; unreadCount: number }> => {
    const response = await apiClient.get<{
      notifications: Notification[];
      total: number;
      unreadCount: number;
    }>(`/api/notification/user/${userId}`, {
      params: { page, limit, unreadOnly }
    });
    return response.data;
  },

  /**
   * Delete notification (compatibility)
   */
  deleteNotification: async (notificationId: string): Promise<void> => {
    await apiClient.delete(`/api/notification/${notificationId}`);
  },
};
