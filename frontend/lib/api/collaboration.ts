import apiClient from '../api-client';

export interface CollaborationSession {
  id: string;
  name: string;
  description?: string;
  ownerId: string;
  ownerName: string;
  createdAt: string;
  isActive: boolean;
  participantCount: number;
  maxParticipants: number;
  language: string;
  code: string;
}

export interface SessionParticipant {
  id: string;
  userId: string;
  username: string;
  joinedAt: string;
  isActive: boolean;
  cursorPosition?: {
    line: number;
    column: number;
  };
}

export interface CodeChange {
  id: string;
  sessionId: string;
  userId: string;
  username: string;
  operation: 'insert' | 'delete' | 'replace';
  position: {
    line: number;
    column: number;
  };
  content: string;
  timestamp: string;
}

export interface CreateSessionRequest {
  name: string;
  description?: string;
  language: string;
  maxParticipants?: number;
  initialCode?: string;
}

export interface UpdateCodeRequest {
  code: string;
  operation: 'insert' | 'delete' | 'replace';
  position: {
    line: number;
    column: number;
  };
}

export const collaborationApi = {
  /**
   * Get all active collaboration sessions
   */
  getSessions: async (): Promise<CollaborationSession[]> => {
    const response = await apiClient.get<CollaborationSession[]>('/api/collaboration/sessions');
    return response.data;
  },

  /**
   * Get a specific session
   */
  getSession: async (sessionId: string): Promise<CollaborationSession> => {
    const response = await apiClient.get<CollaborationSession>(
      `/api/collaboration/sessions/${sessionId}`
    );
    return response.data;
  },

  /**
   * Create a new collaboration session
   */
  createSession: async (data: CreateSessionRequest): Promise<CollaborationSession> => {
    const response = await apiClient.post<CollaborationSession>(
      '/api/collaboration/sessions',
      data
    );
    return response.data;
  },

  /**
   * Update session details
   */
  updateSession: async (
    sessionId: string,
    data: Partial<CreateSessionRequest>
  ): Promise<CollaborationSession> => {
    const response = await apiClient.put<CollaborationSession>(
      `/api/collaboration/sessions/${sessionId}`,
      data
    );
    return response.data;
  },

  /**
   * Delete a session
   */
  deleteSession: async (sessionId: string): Promise<void> => {
    await apiClient.delete(`/api/collaboration/sessions/${sessionId}`);
  },

  /**
   * Join a collaboration session
   */
  joinSession: async (sessionId: string): Promise<SessionParticipant> => {
    const response = await apiClient.post<SessionParticipant>(
      `/api/collaboration/sessions/${sessionId}/join`
    );
    return response.data;
  },

  /**
   * Leave a collaboration session
   */
  leaveSession: async (sessionId: string): Promise<void> => {
    await apiClient.post(`/api/collaboration/sessions/${sessionId}/leave`);
  },

  /**
   * Get participants in a session
   */
  getParticipants: async (sessionId: string): Promise<SessionParticipant[]> => {
    const response = await apiClient.get<SessionParticipant[]>(
      `/api/collaboration/sessions/${sessionId}/participants`
    );
    return response.data;
  },

  /**
   * Get code change history
   */
  getCodeHistory: async (
    sessionId: string,
    limit: number = 50
  ): Promise<CodeChange[]> => {
    const response = await apiClient.get<CodeChange[]>(
      `/api/collaboration/sessions/${sessionId}/history`,
      { params: { limit } }
    );
    return response.data;
  },

  /**
   * Update code in session
   */
  updateCode: async (sessionId: string, data: UpdateCodeRequest): Promise<void> => {
    await apiClient.post(`/api/collaboration/sessions/${sessionId}/code`, data);
  },

  /**
   * Update cursor position
   */
  updateCursor: async (
    sessionId: string,
    position: { line: number; column: number }
  ): Promise<void> => {
    await apiClient.post(`/api/collaboration/sessions/${sessionId}/cursor`, { position });
  },

  /**
   * Execute code in session
   */
  executeCode: async (sessionId: string): Promise<{
    output: string;
    error?: string;
    executionTime: number;
  }> => {
    const response = await apiClient.post<{
      output: string;
      error?: string;
      executionTime: number;
    }>(`/api/collaboration/sessions/${sessionId}/execute`);
    return response.data;
  },
};
