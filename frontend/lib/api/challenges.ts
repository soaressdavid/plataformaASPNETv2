import apiClient from '../api-client';
import {
  ChallengeListResponse,
  ChallengeDetailResponse,
  SubmitSolutionRequest,
  SubmitSolutionResponse,
} from '../types';

export const challengesApi = {
  /**
   * Get all challenges
   */
  getAll: async (): Promise<ChallengeListResponse> => {
    const response = await apiClient.get<ChallengeListResponse>('/api/challenges');
    return response.data;
  },

  /**
   * Get challenge details by ID
   */
  getById: async (challengeId: string): Promise<ChallengeDetailResponse> => {
    const response = await apiClient.get<ChallengeDetailResponse>(`/api/challenges/${challengeId}`);
    return response.data;
  },

  /**
   * Submit a solution for a challenge
   */
  submitSolution: async (
    challengeId: string,
    data: SubmitSolutionRequest
  ): Promise<SubmitSolutionResponse> => {
    const response = await apiClient.post<SubmitSolutionResponse>(
      `/api/challenges/${challengeId}/submit`,
      data
    );
    return response.data;
  },

  /**
   * Submit code review findings
   */
  submitCodeReview: async (
    challengeId: string,
    data: {
      userId: string;
      identifiedIssues: Array<{
        lineNumber: number;
        description: string;
        severity: string;
      }>;
    }
  ): Promise<{
    success: boolean;
    totalExpectedBugs: number;
    correctlyIdentified: number;
    missedBugs: number;
    falsePositives: number;
    accuracyPercentage: number;
    xpAwarded: number;
    bugResults: Array<{
      lineNumber: number;
      expectedDescription: string;
      wasIdentified: boolean;
      userDescription?: string;
    }>;
  }> => {
    const response = await apiClient.post(
      `/api/challenges/${challengeId}/code-review`,
      data
    );
    return response.data;
  },
};
