import apiClient from '../api-client';

export interface ChatRoom {
  id: string;
  name: string;
  description?: string;
  participantCount: number;
  lastMessageAt?: string;
  isPrivate: boolean;
}

export interface ChatMessage {
  id: string;
  roomId: string;
  userId: string;
  username: string;
  content: string;
  timestamp: string;
  isEdited: boolean;
}

export interface SendMessageRequest {
  content: string;
}

export interface CreateRoomRequest {
  name: string;
  description?: string;
  isPrivate: boolean;
  participantIds?: string[];
}

export const chatApi = {
  /**
   * Get all available chat rooms
   */
  getRooms: async (): Promise<ChatRoom[]> => {
    const response = await apiClient.get<ChatRoom[]>('/api/chat/rooms');
    return response.data;
  },

  /**
   * Get a specific chat room
   */
  getRoom: async (roomId: string): Promise<ChatRoom> => {
    const response = await apiClient.get<ChatRoom>(`/api/chat/rooms/${roomId}`);
    return response.data;
  },

  /**
   * Create a new chat room
   */
  createRoom: async (data: CreateRoomRequest): Promise<ChatRoom> => {
    const response = await apiClient.post<ChatRoom>('/api/chat/rooms', data);
    return response.data;
  },

  /**
   * Get messages for a specific room
   */
  getMessages: async (roomId: string, limit: number = 50): Promise<ChatMessage[]> => {
    const response = await apiClient.get<ChatMessage[]>(
      `/api/chat/rooms/${roomId}/messages`,
      { params: { limit } }
    );
    return response.data;
  },

  /**
   * Send a message to a room
   */
  sendMessage: async (roomId: string, data: SendMessageRequest): Promise<ChatMessage> => {
    const response = await apiClient.post<ChatMessage>(
      `/api/chat/rooms/${roomId}/messages`,
      data
    );
    return response.data;
  },

  /**
   * Delete a message
   */
  deleteMessage: async (roomId: string, messageId: string): Promise<void> => {
    await apiClient.delete(`/api/chat/rooms/${roomId}/messages/${messageId}`);
  },

  /**
   * Join a chat room
   */
  joinRoom: async (roomId: string): Promise<void> => {
    await apiClient.post(`/api/chat/rooms/${roomId}/join`);
  },

  /**
   * Leave a chat room
   */
  leaveRoom: async (roomId: string): Promise<void> => {
    await apiClient.post(`/api/chat/rooms/${roomId}/leave`);
  },
};
