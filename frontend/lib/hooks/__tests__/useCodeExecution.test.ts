import { renderHook, act, waitFor } from '@testing-library/react';
import { useCodeExecution } from '../useCodeExecution';
import { codeExecutionApi } from '../../api/code-execution';
import { ExecutionStatus } from '../../types';
import toast from 'react-hot-toast';

// Mock dependencies
jest.mock('../../api/code-execution');
jest.mock('react-hot-toast');

// Mock WebSocket
class MockWebSocket {
  onopen: (() => void) | null = null;
  onmessage: ((event: MessageEvent) => void) | null = null;
  onerror: ((event: Event) => void) | null = null;
  onclose: (() => void) | null = null;

  constructor(public url: string) {
    // Simulate connection opening
    setTimeout(() => {
      if (this.onopen) this.onopen();
    }, 0);
  }

  close() {
    if (this.onclose) this.onclose();
  }

  send(data: string) {
    // Mock send
  }
}

describe('useCodeExecution', () => {
  const mockExecute = codeExecutionApi.execute as jest.MockedFunction<
    typeof codeExecutionApi.execute
  >;
  const mockGetStatus = codeExecutionApi.getStatus as jest.MockedFunction<
    typeof codeExecutionApi.getStatus
  >;

  beforeEach(() => {
    jest.clearAllMocks();
    // Mock WebSocket globally
    (global as any).WebSocket = MockWebSocket;
  });

  afterEach(() => {
    delete (global as any).WebSocket;
  });

  it('should initialize with default state', () => {
    const { result } = renderHook(() => useCodeExecution());

    expect(result.current.status).toBeNull();
    expect(result.current.isRunning).toBe(false);
    expect(result.current.output).toBe('');
    expect(result.current.error).toBeNull();
    expect(result.current.executionTime).toBe(0);
  });

  it('should execute code and update status via WebSocket', async () => {
    const jobId = 'test-job-123';
    mockExecute.mockResolvedValue({ jobId, status: 'Queued' });

    const { result } = renderHook(() => useCodeExecution({ useWebSocket: true }));

    let ws: MockWebSocket | null = null;
    const originalWebSocket = (global as any).WebSocket;
    (global as any).WebSocket = class extends MockWebSocket {
      constructor(url: string) {
        super(url);
        ws = this;
      }
    };

    await act(async () => {
      await result.current.execute({
        code: 'Console.WriteLine("Hello");',
        files: [{ name: 'Program.cs', content: 'Console.WriteLine("Hello");' }],
        entryPoint: 'Program.cs',
      });
    });

    expect(mockExecute).toHaveBeenCalledWith({
      code: 'Console.WriteLine("Hello");',
      files: [{ name: 'Program.cs', content: 'Console.WriteLine("Hello");' }],
      entryPoint: 'Program.cs',
    });

    expect(result.current.isRunning).toBe(true);
    expect(result.current.status?.status).toBe(ExecutionStatus.Queued);

    // Simulate WebSocket message for Running status
    await act(async () => {
      if (ws && ws.onmessage) {
        ws.onmessage(
          new MessageEvent('message', {
            data: JSON.stringify({
              jobId,
              status: ExecutionStatus.Running,
              executionTimeMs: 100,
            }),
          })
        );
      }
    });

    expect(result.current.status?.status).toBe(ExecutionStatus.Running);

    // Simulate WebSocket message for Completed status
    await act(async () => {
      if (ws && ws.onmessage) {
        ws.onmessage(
          new MessageEvent('message', {
            data: JSON.stringify({
              jobId,
              status: ExecutionStatus.Completed,
              output: 'Hello',
              executionTimeMs: 250,
            }),
          })
        );
      }
    });

    expect(result.current.status?.status).toBe(ExecutionStatus.Completed);
    expect(result.current.output).toBe('Hello');
    expect(result.current.executionTime).toBe(250);
    expect(result.current.isRunning).toBe(false);
    expect(toast.success).toHaveBeenCalledWith('Execution completed in 250ms');

    (global as any).WebSocket = originalWebSocket;
  });

  it('should fall back to polling when WebSocket is disabled', async () => {
    const jobId = 'test-job-456';
    mockExecute.mockResolvedValue({ jobId, status: 'Queued' });
    mockGetStatus
      .mockResolvedValueOnce({
        jobId,
        status: ExecutionStatus.Queued,
        executionTimeMs: 0,
      })
      .mockResolvedValueOnce({
        jobId,
        status: ExecutionStatus.Running,
        executionTimeMs: 100,
      })
      .mockResolvedValueOnce({
        jobId,
        status: ExecutionStatus.Completed,
        output: 'Hello from polling',
        executionTimeMs: 300,
      });

    const { result } = renderHook(() => useCodeExecution({ useWebSocket: false }));

    await act(async () => {
      await result.current.execute({
        code: 'Console.WriteLine("Hello");',
        files: [{ name: 'Program.cs', content: 'Console.WriteLine("Hello");' }],
        entryPoint: 'Program.cs',
      });
    });

    expect(result.current.isRunning).toBe(true);

    // Wait for polling to complete
    await waitFor(
      () => {
        expect(result.current.status?.status).toBe(ExecutionStatus.Completed);
      },
      { timeout: 5000 }
    );

    expect(result.current.output).toBe('Hello from polling');
    expect(result.current.executionTime).toBe(300);
    expect(result.current.isRunning).toBe(false);
  });

  it('should handle timeout status', async () => {
    const jobId = 'test-job-timeout';
    mockExecute.mockResolvedValue({ jobId, status: 'Queued' });

    const { result } = renderHook(() => useCodeExecution({ useWebSocket: true }));

    let ws: MockWebSocket | null = null;
    const originalWebSocket = (global as any).WebSocket;
    (global as any).WebSocket = class extends MockWebSocket {
      constructor(url: string) {
        super(url);
        ws = this;
      }
    };

    await act(async () => {
      await result.current.execute({
        code: 'while(true) {}',
        files: [{ name: 'Program.cs', content: 'while(true) {}' }],
        entryPoint: 'Program.cs',
      });
    });

    // Simulate timeout status
    await act(async () => {
      if (ws && ws.onmessage) {
        ws.onmessage(
          new MessageEvent('message', {
            data: JSON.stringify({
              jobId,
              status: ExecutionStatus.Timeout,
              executionTimeMs: 30000,
            }),
          })
        );
      }
    });

    expect(result.current.status?.status).toBe(ExecutionStatus.Timeout);
    expect(result.current.isRunning).toBe(false);
    expect(toast.error).toHaveBeenCalledWith(
      'Execution timeout: Code exceeded 30 second time limit'
    );

    (global as any).WebSocket = originalWebSocket;
  });

  it('should handle memory exceeded status', async () => {
    const jobId = 'test-job-memory';
    mockExecute.mockResolvedValue({ jobId, status: 'Queued' });

    const { result } = renderHook(() => useCodeExecution({ useWebSocket: true }));

    let ws: MockWebSocket | null = null;
    const originalWebSocket = (global as any).WebSocket;
    (global as any).WebSocket = class extends MockWebSocket {
      constructor(url: string) {
        super(url);
        ws = this;
      }
    };

    await act(async () => {
      await result.current.execute({
        code: 'var list = new List<byte[]>();',
        files: [{ name: 'Program.cs', content: 'var list = new List<byte[]>();' }],
        entryPoint: 'Program.cs',
      });
    });

    // Simulate memory exceeded status
    await act(async () => {
      if (ws && ws.onmessage) {
        ws.onmessage(
          new MessageEvent('message', {
            data: JSON.stringify({
              jobId,
              status: ExecutionStatus.MemoryExceeded,
              executionTimeMs: 5000,
            }),
          })
        );
      }
    });

    expect(result.current.status?.status).toBe(ExecutionStatus.MemoryExceeded);
    expect(result.current.isRunning).toBe(false);
    expect(toast.error).toHaveBeenCalledWith(
      'Memory exceeded: Code exceeded 512MB memory limit'
    );

    (global as any).WebSocket = originalWebSocket;
  });

  it('should handle execution errors', async () => {
    const jobId = 'test-job-error';
    mockExecute.mockResolvedValue({ jobId, status: 'Queued' });

    const { result } = renderHook(() => useCodeExecution({ useWebSocket: true }));

    let ws: MockWebSocket | null = null;
    const originalWebSocket = (global as any).WebSocket;
    (global as any).WebSocket = class extends MockWebSocket {
      constructor(url: string) {
        super(url);
        ws = this;
      }
    };

    await act(async () => {
      await result.current.execute({
        code: 'Console.WriteLine(undefinedVar);',
        files: [{ name: 'Program.cs', content: 'Console.WriteLine(undefinedVar);' }],
        entryPoint: 'Program.cs',
      });
    });

    // Simulate failed status with error
    await act(async () => {
      if (ws && ws.onmessage) {
        ws.onmessage(
          new MessageEvent('message', {
            data: JSON.stringify({
              jobId,
              status: ExecutionStatus.Failed,
              error: 'CS0103: The name "undefinedVar" does not exist in the current context',
              executionTimeMs: 100,
            }),
          })
        );
      }
    });

    expect(result.current.status?.status).toBe(ExecutionStatus.Failed);
    expect(result.current.error).toBe(
      'CS0103: The name "undefinedVar" does not exist in the current context'
    );
    expect(result.current.isRunning).toBe(false);
    expect(toast.error).toHaveBeenCalledWith('Execution failed');

    (global as any).WebSocket = originalWebSocket;
  });

  it('should reset state when reset is called', async () => {
    const jobId = 'test-job-reset';
    mockExecute.mockResolvedValue({ jobId, status: 'Queued' });

    const { result } = renderHook(() => useCodeExecution({ useWebSocket: true }));

    let ws: MockWebSocket | null = null;
    const originalWebSocket = (global as any).WebSocket;
    (global as any).WebSocket = class extends MockWebSocket {
      constructor(url: string) {
        super(url);
        ws = this;
      }
    };

    await act(async () => {
      await result.current.execute({
        code: 'Console.WriteLine("Test");',
        files: [{ name: 'Program.cs', content: 'Console.WriteLine("Test");' }],
        entryPoint: 'Program.cs',
      });
    });

    // Simulate completed status
    await act(async () => {
      if (ws && ws.onmessage) {
        ws.onmessage(
          new MessageEvent('message', {
            data: JSON.stringify({
              jobId,
              status: ExecutionStatus.Completed,
              output: 'Test',
              executionTimeMs: 200,
            }),
          })
        );
      }
    });

    expect(result.current.output).toBe('Test');
    expect(result.current.executionTime).toBe(200);

    // Reset
    act(() => {
      result.current.reset();
    });

    expect(result.current.status).toBeNull();
    expect(result.current.isRunning).toBe(false);
    expect(result.current.output).toBe('');
    expect(result.current.error).toBeNull();
    expect(result.current.executionTime).toBe(0);

    (global as any).WebSocket = originalWebSocket;
  });

  it('should call onStatusUpdate callback', async () => {
    const jobId = 'test-job-callback';
    mockExecute.mockResolvedValue({ jobId, status: 'Queued' });

    const onStatusUpdate = jest.fn();
    const { result } = renderHook(() => useCodeExecution({ onStatusUpdate, useWebSocket: true }));

    let ws: MockWebSocket | null = null;
    const originalWebSocket = (global as any).WebSocket;
    (global as any).WebSocket = class extends MockWebSocket {
      constructor(url: string) {
        super(url);
        ws = this;
      }
    };

    await act(async () => {
      await result.current.execute({
        code: 'Console.WriteLine("Callback test");',
        files: [{ name: 'Program.cs', content: 'Console.WriteLine("Callback test");' }],
        entryPoint: 'Program.cs',
      });
    });

    // Should be called with initial queued status
    expect(onStatusUpdate).toHaveBeenCalledWith(
      expect.objectContaining({
        jobId,
        status: ExecutionStatus.Queued,
      })
    );

    // Simulate running status
    await act(async () => {
      if (ws && ws.onmessage) {
        ws.onmessage(
          new MessageEvent('message', {
            data: JSON.stringify({
              jobId,
              status: ExecutionStatus.Running,
              executionTimeMs: 50,
            }),
          })
        );
      }
    });

    expect(onStatusUpdate).toHaveBeenCalledWith(
      expect.objectContaining({
        jobId,
        status: ExecutionStatus.Running,
      })
    );

    (global as any).WebSocket = originalWebSocket;
  });
});
