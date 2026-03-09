import apiClient from '../api-client';
import {
  ProjectSummary,
  ProjectDetailResponse,
} from '../types';

export interface ProjectListResponse {
  projects: ProjectSummary[];
}

export interface ValidateStepRequest {
  userId: string;
  code: string;
}

export interface ValidateStepResponse {
  success: boolean;
  message: string;
  nextStepUnlocked: boolean;
}

export const projectsApi = {
  /**
   * Get all projects
   */
  getAll: async (): Promise<ProjectListResponse> => {
    const response = await apiClient.get<ProjectListResponse>('/api/projects');
    return response.data;
  },

  /**
   * Get project details by ID
   */
  getById: async (projectId: string): Promise<ProjectDetailResponse> => {
    const response = await apiClient.get<ProjectDetailResponse>(`/api/projects/${projectId}`);
    return response.data;
  },

  /**
   * Validate a project step
   */
  validateStep: async (
    projectId: string,
    stepNumber: number,
    data: ValidateStepRequest
  ): Promise<ValidateStepResponse> => {
    const response = await apiClient.post<ValidateStepResponse>(
      `/api/projects/${projectId}/steps/${stepNumber}/validate`,
      data
    );
    return response.data;
  },
};
