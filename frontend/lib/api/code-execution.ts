import apiClient from '../api-client';
import {
  ExecuteCodeRequest,
  ExecuteCodeResponse,
  ExecutionStatusResponse,
} from '../types';

export const codeExecutionApi = {
  /**
   * Execute code and get job ID
   */
  execute: async (data: ExecuteCodeRequest): Promise<ExecuteCodeResponse> => {
    const response = await apiClient.post<ExecuteCodeResponse>('/api/code/execute', data);
    return response.data;
  },

  /**
   * Get execution status by job ID
   */
  getStatus: async (jobId: string): Promise<ExecutionStatusResponse> => {
    const response = await apiClient.get<ExecutionStatusResponse>(`/api/code/status/${jobId}`);
    return response.data;
  },

  /**
   * Poll execution status until completion
   */
  pollStatus: async (
    jobId: string,
    onUpdate?: (status: ExecutionStatusResponse) => void,
    maxAttempts: number = 60,
    intervalMs: number = 1000
  ): Promise<ExecutionStatusResponse> => {
    let attempts = 0;
    
    while (attempts < maxAttempts) {
      const status = await codeExecutionApi.getStatus(jobId);
      
      if (onUpdate) {
        onUpdate(status);
      }
      
      // Check if execution is complete
      if (
        status.status === 'Completed' ||
        status.status === 'Failed' ||
        status.status === 'Timeout' ||
        status.status === 'MemoryExceeded'
      ) {
        return status;
      }
      
      // Wait before next poll
      await new Promise(resolve => setTimeout(resolve, intervalMs));
      attempts++;
    }
    
    throw new Error('Execution polling timeout');
  },
};
