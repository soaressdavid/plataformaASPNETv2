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

// MOCK DATA - Notificações simuladas
const mockNotifications: Notification[] = [
  {
    id: '1',
    userId: 'user1',
    type: 'achievement',
    title: 'Parabéns! Primeira aula concluída',
    message: 'Você completou sua primeira aula de C#. Continue assim!',
    isRead: false,
    createdAt: new Date(Date.now() - 2 * 60 * 60 * 1000).toISOString(), // 2 horas atrás
    actionUrl: '/courses/1'
  },
  {
    id: '2',
    userId: 'user1',
    type: 'info',
    title: 'Nova aula disponível',
    message: 'A aula "Variáveis e Tipos de Dados" está disponível no curso de C#.',
    isRead: false,
    createdAt: new Date(Date.now() - 4 * 60 * 60 * 1000).toISOString(), // 4 horas atrás
    actionUrl: '/courses/1/lessons/2'
  },
  {
    id: '3',
    userId: 'user1',
    type: 'success',
    title: 'Exercício SQL completado',
    message: 'Você executou com sucesso sua primeira consulta SQL!',
    isRead: true,
    createdAt: new Date(Date.now() - 24 * 60 * 60 * 1000).toISOString(), // 1 dia atrás
    readAt: new Date(Date.now() - 20 * 60 * 60 * 1000).toISOString(),
    actionUrl: '/courses/2'
  },
  {
    id: '4',
    userId: 'user1',
    type: 'message',
    title: 'Bem-vindo à plataforma!',
    message: 'Explore nossos 12 cursos e comece sua jornada de aprendizado.',
    isRead: true,
    createdAt: new Date(Date.now() - 48 * 60 * 60 * 1000).toISOString(), // 2 dias atrás
    readAt: new Date(Date.now() - 24 * 60 * 60 * 1000).toISOString(),
    actionUrl: '/courses'
  }
];

const mockPreferences: NotificationPreferences = {
  emailNotifications: true,
  pushNotifications: false,
  achievementNotifications: true,
  messageNotifications: true,
  forumNotifications: false,
  collaborationNotifications: true
};

export const notificationsApi = {
  /**
   * Get all notifications for current user - MOCK VERSION
   */
  getAll: async (params?: {
    page?: number;
    limit?: number;
    unreadOnly?: boolean;
  }): Promise<{ notifications: Notification[]; total: number; unreadCount: number }> => {
    // Simular delay de rede
    await new Promise(resolve => setTimeout(resolve, 300));
    
    let filteredNotifications = [...mockNotifications];
    
    if (params?.unreadOnly) {
      filteredNotifications = filteredNotifications.filter(n => !n.isRead);
    }
    
    const unreadCount = mockNotifications.filter(n => !n.isRead).length;
    
    return {
      notifications: filteredNotifications,
      total: filteredNotifications.length,
      unreadCount
    };
  },

  /**
   * Get unread notification count - MOCK VERSION
   */
  getUnreadCount: async (): Promise<number> => {
    await new Promise(resolve => setTimeout(resolve, 200));
    return mockNotifications.filter(n => !n.isRead).length;
  },

  /**
   * Mark a notification as read - MOCK VERSION
   */
  markAsRead: async (notificationId: string): Promise<void> => {
    await new Promise(resolve => setTimeout(resolve, 200));
    const notification = mockNotifications.find(n => n.id === notificationId);
    if (notification) {
      notification.isRead = true;
      notification.readAt = new Date().toISOString();
    }
  },

  /**
   * Mark all notifications as read - MOCK VERSION
   */
  markAllAsRead: async (): Promise<void> => {
    await new Promise(resolve => setTimeout(resolve, 300));
    mockNotifications.forEach(n => {
      if (!n.isRead) {
        n.isRead = true;
        n.readAt = new Date().toISOString();
      }
    });
  },

  /**
   * Delete a notification - MOCK VERSION
   */
  delete: async (notificationId: string): Promise<void> => {
    await new Promise(resolve => setTimeout(resolve, 200));
    const index = mockNotifications.findIndex(n => n.id === notificationId);
    if (index > -1) {
      mockNotifications.splice(index, 1);
    }
  },

  /**
   * Delete all read notifications - MOCK VERSION
   */
  deleteAllRead: async (): Promise<void> => {
    await new Promise(resolve => setTimeout(resolve, 300));
    for (let i = mockNotifications.length - 1; i >= 0; i--) {
      if (mockNotifications[i].isRead) {
        mockNotifications.splice(i, 1);
      }
    }
  },

  /**
   * Get notification preferences - MOCK VERSION
   */
  getPreferences: async (): Promise<NotificationPreferences> => {
    await new Promise(resolve => setTimeout(resolve, 200));
    return { ...mockPreferences };
  },

  /**
   * Update notification preferences - MOCK VERSION
   */
  updatePreferences: async (
    preferences: Partial<NotificationPreferences>
  ): Promise<NotificationPreferences> => {
    await new Promise(resolve => setTimeout(resolve, 300));
    Object.assign(mockPreferences, preferences);
    return { ...mockPreferences };
  },

  /**
   * Get user notifications (compatibility with new backend) - MOCK VERSION
   */
  getUserNotifications: async (
    userId: string,
    page: number = 1,
    limit: number = 20,
    unreadOnly: boolean = false
  ): Promise<{ notifications: Notification[]; total: number; unreadCount: number }> => {
    await new Promise(resolve => setTimeout(resolve, 300));
    
    let filteredNotifications = mockNotifications.filter(n => n.userId === userId || n.userId === 'user1');
    
    if (unreadOnly) {
      filteredNotifications = filteredNotifications.filter(n => !n.isRead);
    }
    
    const unreadCount = filteredNotifications.filter(n => !n.isRead).length;
    
    return {
      notifications: filteredNotifications,
      total: filteredNotifications.length,
      unreadCount
    };
  },

  /**
   * Delete notification (compatibility) - MOCK VERSION
   */
  deleteNotification: async (notificationId: string): Promise<void> => {
    await new Promise(resolve => setTimeout(resolve, 200));
    const index = mockNotifications.findIndex(n => n.id === notificationId);
    if (index > -1) {
      mockNotifications.splice(index, 1);
    }
  },
};
