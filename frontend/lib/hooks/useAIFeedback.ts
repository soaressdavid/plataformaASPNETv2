import { useState, useCallback } from 'react';
import { aiTutorApi } from '../api/ai-tutor';
import { CodeReviewResponse } from '../types';
import toast from 'react-hot-toast';

export interface UseAIFeedbackResult {
  feedback: CodeReviewResponse | null;
  isLoading: boolean;
  error: string | null;
  getFeedback: (code: string, context?: string) => Promise<void>;
  clearFeedback: () => void;
}

export const useAIFeedback = (): UseAIFeedbackResult => {
  const [feedback, setFeedback] = useState<CodeReviewResponse | null>(null);
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const getFeedback = useCallback(async (code: string, context: string = '') => {
    setIsLoading(true);
    setError(null);

    try {
      const response = await aiTutorApi.reviewCode({ code, context });
      setFeedback(response);
      toast.success('Feedback da IA recebido!', {
        duration: 3000,
        style: {
          background: '#161b22',
          color: '#fff',
          border: '1px solid #30363d',
        },
      });
    } catch (err: any) {
      const errorMessage = err.response?.data?.error?.message || 'Falha ao obter feedback da IA';
      setError(errorMessage);
      toast.error(errorMessage, {
        duration: 3000,
        style: {
          background: '#161b22',
          color: '#fff',
          border: '1px solid #30363d',
        },
      });
      console.error('AI feedback error:', err);
    } finally {
      setIsLoading(false);
    }
  }, []);

  const clearFeedback = useCallback(() => {
    setFeedback(null);
    setError(null);
  }, []);

  return {
    feedback,
    isLoading,
    error,
    getFeedback,
    clearFeedback,
  };
};
