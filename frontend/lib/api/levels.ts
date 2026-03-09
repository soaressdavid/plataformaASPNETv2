import apiClient from '../api-client';
import { LevelListResponse, CurriculumLevelDetail, CourseListResponse } from '../types';

export const levelsApi = {
  /**
   * Get all curriculum levels
   */
  getAll: async (): Promise<LevelListResponse> => {
    const response = await apiClient.get<LevelListResponse>('/api/levels');
    return response.data;
  },

  /**
   * Get a specific level with its courses
   */
  getById: async (id: string): Promise<CurriculumLevelDetail> => {
    const response = await apiClient.get<CurriculumLevelDetail>(`/api/levels/${id}`);
    return response.data;
  },

  /**
   * Get all courses for a specific level
   */
  getCourses: async (id: string): Promise<CourseListResponse> => {
    const response = await apiClient.get<CourseListResponse>(`/api/levels/${id}/courses`);
    return response.data;
  },
};
