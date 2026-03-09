import apiClient from '../api-client';

export interface ForumCategory {
  id: string;
  name: string;
  description: string;
  threadCount: number;
  postCount: number;
}

export interface ForumThread {
  id: string;
  categoryId: string;
  categoryName: string;
  title: string;
  content: string;
  authorId: string;
  authorName: string;
  createdAt: string;
  updatedAt: string;
  viewCount: number;
  replyCount: number;
  isPinned: boolean;
  isLocked: boolean;
  tags: string[];
}

export interface ForumPost {
  id: string;
  threadId: string;
  content: string;
  authorId: string;
  authorName: string;
  createdAt: string;
  updatedAt: string;
  isEdited: boolean;
  upvotes: number;
  downvotes: number;
}

export interface CreateThreadRequest {
  categoryId: string;
  title: string;
  content: string;
  tags?: string[];
}

export interface CreatePostRequest {
  content: string;
}

export interface UpdateThreadRequest {
  title?: string;
  content?: string;
  tags?: string[];
}

export const forumApi = {
  /**
   * Get all forum categories
   */
  getCategories: async (): Promise<ForumCategory[]> => {
    const response = await apiClient.get<ForumCategory[]>('/api/forum/categories');
    return response.data;
  },

  /**
   * Get threads in a category
   */
  getThreadsByCategory: async (
    categoryId: string,
    page: number = 1,
    limit: number = 20
  ): Promise<{ threads: ForumThread[]; total: number }> => {
    const response = await apiClient.get<{ threads: ForumThread[]; total: number }>(
      `/api/forum/categories/${categoryId}/threads`,
      { params: { page, limit } }
    );
    return response.data;
  },

  /**
   * Get all threads (with optional filters)
   */
  getThreads: async (params?: {
    page?: number;
    limit?: number;
    search?: string;
    tag?: string;
  }): Promise<{ threads: ForumThread[]; total: number }> => {
    const response = await apiClient.get<{ threads: ForumThread[]; total: number }>(
      '/api/forum/threads',
      { params }
    );
    return response.data;
  },

  /**
   * Get a specific thread
   */
  getThread: async (threadId: string): Promise<ForumThread> => {
    const response = await apiClient.get<ForumThread>(`/api/forum/threads/${threadId}`);
    return response.data;
  },

  /**
   * Create a new thread
   */
  createThread: async (data: CreateThreadRequest): Promise<ForumThread> => {
    const response = await apiClient.post<ForumThread>('/api/forum/threads', data);
    return response.data;
  },

  /**
   * Update a thread
   */
  updateThread: async (threadId: string, data: UpdateThreadRequest): Promise<ForumThread> => {
    const response = await apiClient.put<ForumThread>(
      `/api/forum/threads/${threadId}`,
      data
    );
    return response.data;
  },

  /**
   * Delete a thread
   */
  deleteThread: async (threadId: string): Promise<void> => {
    await apiClient.delete(`/api/forum/threads/${threadId}`);
  },

  /**
   * Get posts in a thread
   */
  getPosts: async (
    threadId: string,
    page: number = 1,
    limit: number = 20
  ): Promise<{ posts: ForumPost[]; total: number }> => {
    const response = await apiClient.get<{ posts: ForumPost[]; total: number }>(
      `/api/forum/threads/${threadId}/posts`,
      { params: { page, limit } }
    );
    return response.data;
  },

  /**
   * Create a post (reply to thread)
   */
  createPost: async (threadId: string, data: CreatePostRequest): Promise<ForumPost> => {
    const response = await apiClient.post<ForumPost>(
      `/api/forum/threads/${threadId}/posts`,
      data
    );
    return response.data;
  },

  /**
   * Update a post
   */
  updatePost: async (
    threadId: string,
    postId: string,
    data: CreatePostRequest
  ): Promise<ForumPost> => {
    const response = await apiClient.put<ForumPost>(
      `/api/forum/threads/${threadId}/posts/${postId}`,
      data
    );
    return response.data;
  },

  /**
   * Delete a post
   */
  deletePost: async (threadId: string, postId: string): Promise<void> => {
    await apiClient.delete(`/api/forum/threads/${threadId}/posts/${postId}`);
  },

  /**
   * Upvote a post
   */
  upvotePost: async (threadId: string, postId: string): Promise<void> => {
    await apiClient.post(`/api/forum/threads/${threadId}/posts/${postId}/upvote`);
  },

  /**
   * Downvote a post
   */
  downvotePost: async (threadId: string, postId: string): Promise<void> => {
    await apiClient.post(`/api/forum/threads/${threadId}/posts/${postId}/downvote`);
  },
};
