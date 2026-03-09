import apiClient from '../api-client';
import { CodeReviewRequest, CodeReviewResponse } from '../types';

export const aiTutorApi = {
  /**
   * Request AI code review
   */
  reviewCode: async (data: CodeReviewRequest): Promise<CodeReviewResponse> => {
    const response = await apiClient.post<CodeReviewResponse>('/api/code/review', data);
    return response.data;
  },
};
