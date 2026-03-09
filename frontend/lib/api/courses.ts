import apiClient from '../api-client';
import {
  CourseListResponse,
  CourseDetail,
  LessonListResponse,
  LessonDetail,
  CompleteLessonRequest,
  CompleteLessonResponse,
} from '../types';

export const coursesApi = {
  /**
   * Get all courses with optional filters
   */
  getAll: async (filters?: {
    levelId?: string;
    level?: string;
  }): Promise<CourseListResponse> => {
    const params = new URLSearchParams();
    if (filters?.levelId) params.append('levelId', filters.levelId);
    if (filters?.level) params.append('level', filters.level);
    
    const queryString = params.toString();
    const url = queryString ? `/api/courses?${queryString}` : '/api/courses';
    
    const response = await apiClient.get<CourseListResponse>(url);
    return response.data;
  },

  /**
   * Get a specific course with details
   */
  getById: async (id: string): Promise<CourseDetail> => {
    const response = await apiClient.get<CourseDetail>(`/api/courses/${id}`);
    return response.data;
  },

  /**
   * Get lessons for a specific course
   */
  getLessons: async (courseId: string): Promise<LessonListResponse> => {
    const response = await apiClient.get<LessonListResponse>(`/api/courses/${courseId}/lessons`);
    return response.data;
  },

  /**
   * Get a specific lesson with full content
   */
  getLesson: async (courseId: string, lessonId: string): Promise<LessonDetail> => {
    const response = await apiClient.get<LessonDetail>(
      `/api/courses/${courseId}/lessons/${lessonId}`
    );
    return response.data;
  },

  /**
   * Mark a lesson as complete
   */
  completeLesson: async (
    courseId: string,
    lessonId: string,
    data: CompleteLessonRequest
  ): Promise<CompleteLessonResponse> => {
    const response = await apiClient.post<CompleteLessonResponse>(
      `/api/courses/${courseId}/lessons/${lessonId}/complete`,
      data
    );
    return response.data;
  },
};
