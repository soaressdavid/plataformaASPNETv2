import { useState, useCallback, useRef, useEffect } from 'react';
import { codeExecutionApi } from '../api/code-execution';
import {
  ExecuteCodeRequest,
  ExecutionStatus,
  ExecutionStatusResponse,
} from '../types';
import toast from 'react-hot-toast';

interface UseCodeExecutionOptions {
  onStatusUpdate?: (status: ExecutionStatusResponse) => void;
  useWebSocket?: boolean;
}

interface UseCodeExecutionReturn {
  execute: (request: ExecuteCodeRequest) => Promise<void>;
  status: ExecutionStatusResponse | null;
  isRunning: boolean;
  output: string;
  error: string | null;
  executionTime: number;
  reset: () => void;
}

/**
 * Hook for code execution with WebSocket support for real-time updates
 * Falls back to polling if WebSocket is not available
 */
export const useCodeExecution = (
  options: UseCodeExecutionOptions = {}
): UseCodeExecutionReturn => {
  const { onStatusUpdate, useWebSocket = true } = options;

  const [status, setStatus] = useState<ExecutionStatusResponse | null>(null);
  const [isRunning, setIsRunning] = useState(false);
  const [output, setOutput] = useState('');
  const [error, setError] = useState<string | null>(null);
  const [executionTime, setExecutionTime] = useState(0);

  const wsRef = useRef<WebSocket | null>(null);
  const pollingIntervalRef = useRef<NodeJS.Timeout | null>(null);

  // Clean up WebSocket and polling on unmount
  useEffect(() => {
    return () => {
      if (wsRef.current) {
        wsRef.current.close();
      }
      if (pollingIntervalRef.current) {
        clearInterval(pollingIntervalRef.current);
      }
    };
  }, []);

  const handleStatusUpdate = useCallback(
    (newStatus: ExecutionStatusResponse) => {
      setStatus(newStatus);
      setExecutionTime(newStatus.executionTimeMs);

      // Update output and error based on status
      if (newStatus.output) {
        setOutput(newStatus.output);
      }
      if (newStatus.error) {
        setError(newStatus.error);
      }

      // Check if execution is complete
      const isComplete =
        newStatus.status === ExecutionStatus.Completed ||
        newStatus.status === ExecutionStatus.Failed ||
        newStatus.status === ExecutionStatus.Timeout ||
        newStatus.status === ExecutionStatus.MemoryExceeded;

      if (isComplete) {
        setIsRunning(false);

        // Close WebSocket if open
        if (wsRef.current) {
          wsRef.current.close();
          wsRef.current = null;
        }

        // Clear polling interval
        if (pollingIntervalRef.current) {
          clearInterval(pollingIntervalRef.current);
          pollingIntervalRef.current = null;
        }

        // Show appropriate toast notification
        if (newStatus.status === ExecutionStatus.Completed) {
          toast.success(`Execution completed in ${newStatus.executionTimeMs}ms`);
        } else if (newStatus.status === ExecutionStatus.Timeout) {
          toast.error('Execution timeout: Code exceeded 30 second time limit');
        } else if (newStatus.status === ExecutionStatus.MemoryExceeded) {
          toast.error('Memory exceeded: Code exceeded 512MB memory limit');
        } else if (newStatus.status === ExecutionStatus.Failed) {
          toast.error('Execution failed');
        }
      }

      // Call user-provided callback
      if (onStatusUpdate) {
        onStatusUpdate(newStatus);
      }
    },
    [onStatusUpdate]
  );

  const connectWebSocket = useCallback(
    (jobId: string) => {
      const wsUrl = process.env.NEXT_PUBLIC_WS_URL || 'ws://localhost:5000';
      const ws = new WebSocket(`${wsUrl}/ws/code/status/${jobId}`);

      ws.onopen = () => {
        console.log('WebSocket connected for job:', jobId);
      };

      ws.onmessage = (event) => {
        try {
          const statusUpdate: ExecutionStatusResponse = JSON.parse(event.data);
          handleStatusUpdate(statusUpdate);
        } catch (err) {
          console.error('Failed to parse WebSocket message:', err);
        }
      };

      ws.onerror = (event) => {
        console.error('WebSocket error:', event);
        // Fall back to polling on WebSocket error
        startPolling(jobId);
      };

      ws.onclose = () => {
        console.log('WebSocket closed for job:', jobId);
      };

      wsRef.current = ws;
    },
    [handleStatusUpdate]
  );

  const startPolling = useCallback(
    (jobId: string) => {
      // Clear any existing polling interval
      if (pollingIntervalRef.current) {
        clearInterval(pollingIntervalRef.current);
      }

      const poll = async () => {
        try {
          const statusUpdate = await codeExecutionApi.getStatus(jobId);
          handleStatusUpdate(statusUpdate);
        } catch (err) {
          console.error('Polling error:', err);
          toast.error('Failed to get execution status');
          setIsRunning(false);
          if (pollingIntervalRef.current) {
            clearInterval(pollingIntervalRef.current);
            pollingIntervalRef.current = null;
          }
        }
      };

      // Poll immediately
      poll();

      // Then poll every second
      pollingIntervalRef.current = setInterval(poll, 1000);
    },
    [handleStatusUpdate]
  );

  const execute = useCallback(
    async (request: ExecuteCodeRequest) => {
      try {
        // Reset state
        setIsRunning(true);
        setOutput('');
        setError(null);
        setExecutionTime(0);
        setStatus(null);

        // Submit code for execution
        const response = await codeExecutionApi.execute(request);

        // Check if response already contains execution results (mock mode)
        if ('output' in response || 'executionTimeMs' in response) {
          // Response contains full execution results
          const fullResponse = response as any;
          const finalStatus: ExecutionStatusResponse = {
            jobId: fullResponse.jobId,
            status: fullResponse.status as ExecutionStatus,
            output: fullResponse.output,
            error: fullResponse.error,
            executionTimeMs: fullResponse.executionTimeMs || 0,
          };
          handleStatusUpdate(finalStatus);
          return;
        }

        // Set initial status for async execution
        const initialStatus: ExecutionStatusResponse = {
          jobId: response.jobId,
          status: ExecutionStatus.Queued,
          executionTimeMs: 0,
        };
        handleStatusUpdate(initialStatus);

        // Use WebSocket if available and enabled, otherwise fall back to polling
        if (useWebSocket && typeof WebSocket !== 'undefined') {
          try {
            connectWebSocket(response.jobId);
          } catch (err) {
            console.warn('WebSocket connection failed, falling back to polling:', err);
            startPolling(response.jobId);
          }
        } else {
          startPolling(response.jobId);
        }
      } catch (err: any) {
        console.error('Code execution error:', err);
        setIsRunning(false);
        
        // Handle specific error cases
        if (err.response?.status === 422) {
          // Prohibited code or validation error
          const errorMessage = err.response?.data?.error?.message || 'Code validation failed';
          setError(errorMessage);
          toast.error(errorMessage);
        } else {
          setError('Failed to execute code');
          toast.error('Failed to execute code');
        }
      }
    },
    [useWebSocket, connectWebSocket, startPolling, handleStatusUpdate]
  );

  const reset = useCallback(() => {
    setStatus(null);
    setIsRunning(false);
    setOutput('');
    setError(null);
    setExecutionTime(0);

    // Clean up connections
    if (wsRef.current) {
      wsRef.current.close();
      wsRef.current = null;
    }
    if (pollingIntervalRef.current) {
      clearInterval(pollingIntervalRef.current);
      pollingIntervalRef.current = null;
    }
  }, []);

  return {
    execute,
    status,
    isRunning,
    output,
    error,
    executionTime,
    reset,
  };
};
