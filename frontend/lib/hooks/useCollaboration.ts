import { useEffect, useState, useCallback, useRef } from 'react';
import * as signalR from '@microsoft/signalr';

/**
 * Hook for managing collaborative coding sessions
 * Validates: Requirements 32.1, 32.2, 32.3, 32.4, 32.7
 */

export interface Operation {
  type: 'Insert' | 'Delete' | 'Retain';
  position: number;
  text?: string;
  length: number;
  userId: string;
  version: number;
}

export interface CursorPosition {
  userId: string;
  line: number;
  column: number;
  selectionStartLine?: number;
  selectionStartColumn?: number;
  timestamp: string;
}

export interface ChatMessage {
  userId: string;
  userName: string;
  message: string;
  timestamp: string;
}

export interface SessionParticipant {
  userId: string;
  userName: string;
  timestamp: string;
}

export interface SessionJoinResult {
  sessionId: string;
  code: string;
  version: number;
  participants: string[];
}

export function useCollaboration(hubUrl: string) {
  const [connection, setConnection] = useState<signalR.HubConnection | null>(null);
  const [isConnected, setIsConnected] = useState(false);
  const [currentSessionId, setCurrentSessionId] = useState<string | null>(null);
  const [participants, setParticipants] = useState<string[]>([]);
  const [cursorPositions, setCursorPositions] = useState<Map<string, CursorPosition>>(new Map());
  const connectionRef = useRef<signalR.HubConnection | null>(null);

  useEffect(() => {
    // Create connection
    const newConnection = new signalR.HubConnectionBuilder()
      .withUrl(hubUrl, {
        accessTokenFactory: () => {
          return localStorage.getItem('token') || '';
        },
      })
      .withAutomaticReconnect({
        nextRetryDelayInMilliseconds: (retryContext) => {
          if (retryContext.previousRetryCount === 0) return 0;
          if (retryContext.previousRetryCount === 1) return 2000;
          if (retryContext.previousRetryCount === 2) return 10000;
          return 30000;
        },
      })
      .configureLogging(signalR.LogLevel.Information)
      .build();

    connectionRef.current = newConnection;
    setConnection(newConnection);

    // Setup connection event handlers
    newConnection.onreconnecting((error) => {
      console.log('SignalR reconnecting...', error);
      setIsConnected(false);
    });

    newConnection.onreconnected((connectionId) => {
      console.log('SignalR reconnected:', connectionId);
      setIsConnected(true);
    });

    newConnection.onclose((error) => {
      console.log('SignalR connection closed:', error);
      setIsConnected(false);
    });

    // Start connection
    const startConnection = async () => {
      try {
        await newConnection.start();
        console.log('SignalR connected to collaboration hub');
        setIsConnected(true);
      } catch (error) {
        console.error('Error connecting to SignalR:', error);
        setTimeout(startConnection, 5000);
      }
    };

    startConnection();

    // Cleanup on unmount
    return () => {
      if (connectionRef.current) {
        connectionRef.current.stop();
      }
    };
  }, [hubUrl]);

  const createSession = useCallback(
    async (name: string, challengeId?: string, language: string = 'csharp', initialCode: string = ''): Promise<string> => {
      if (!connection || !isConnected) {
        throw new Error('Not connected to collaboration server');
      }

      try {
        const sessionId = await connection.invoke<string>('CreateSession', name, challengeId, language, initialCode);
        setCurrentSessionId(sessionId);
        return sessionId;
      } catch (error) {
        console.error('Error creating session:', error);
        throw error;
      }
    },
    [connection, isConnected]
  );

  const joinSession = useCallback(
    async (sessionId: string): Promise<SessionJoinResult> => {
      if (!connection || !isConnected) {
        throw new Error('Not connected to collaboration server');
      }

      try {
        const result = await connection.invoke<SessionJoinResult>('JoinSession', sessionId);
        setCurrentSessionId(sessionId);
        setParticipants(result.participants);
        return result;
      } catch (error) {
        console.error('Error joining session:', error);
        throw error;
      }
    },
    [connection, isConnected]
  );

  const leaveSession = useCallback(
    async (sessionId: string) => {
      if (!connection || !isConnected) {
        throw new Error('Not connected to collaboration server');
      }

      try {
        await connection.invoke('LeaveSession', sessionId);
        setCurrentSessionId(null);
        setParticipants([]);
        setCursorPositions(new Map());
      } catch (error) {
        console.error('Error leaving session:', error);
        throw error;
      }
    },
    [connection, isConnected]
  );

  const sendOperation = useCallback(
    async (sessionId: string, operation: Operation) => {
      if (!connection || !isConnected) {
        throw new Error('Not connected to collaboration server');
      }

      try {
        await connection.invoke('SendOperation', sessionId, operation);
      } catch (error) {
        console.error('Error sending operation:', error);
        throw error;
      }
    },
    [connection, isConnected]
  );

  const updateCursor = useCallback(
    async (
      sessionId: string,
      line: number,
      column: number,
      selectionStartLine?: number,
      selectionStartColumn?: number
    ) => {
      if (!connection || !isConnected) {
        return;
      }

      try {
        await connection.invoke('UpdateCursor', sessionId, line, column, selectionStartLine, selectionStartColumn);
      } catch (error) {
        console.error('Error updating cursor:', error);
      }
    },
    [connection, isConnected]
  );

  const sendChatMessage = useCallback(
    async (sessionId: string, message: string) => {
      if (!connection || !isConnected) {
        throw new Error('Not connected to collaboration server');
      }

      try {
        await connection.invoke('SendChatMessage', sessionId, message);
      } catch (error) {
        console.error('Error sending chat message:', error);
        throw error;
      }
    },
    [connection, isConnected]
  );

  const runCode = useCallback(
    async (sessionId: string) => {
      if (!connection || !isConnected) {
        throw new Error('Not connected to collaboration server');
      }

      try {
        await connection.invoke('RunCode', sessionId);
      } catch (error) {
        console.error('Error running code:', error);
        throw error;
      }
    },
    [connection, isConnected]
  );

  const completeSession = useCallback(
    async (sessionId: string, totalXP: number) => {
      if (!connection || !isConnected) {
        throw new Error('Not connected to collaboration server');
      }

      try {
        await connection.invoke('CompleteSession', sessionId, totalXP);
        setCurrentSessionId(null);
        setParticipants([]);
        setCursorPositions(new Map());
      } catch (error) {
        console.error('Error completing session:', error);
        throw error;
      }
    },
    [connection, isConnected]
  );

  const onReceiveOperation = useCallback(
    (handler: (operation: Operation) => void) => {
      if (!connection) return () => {};

      connection.on('ReceiveOperation', handler);
      return () => {
        connection.off('ReceiveOperation', handler);
      };
    },
    [connection]
  );

  const onCursorMoved = useCallback(
    (handler: (cursor: CursorPosition) => void) => {
      if (!connection) return () => {};

      connection.on('CursorMoved', (cursor: CursorPosition) => {
        setCursorPositions((prev) => {
          const newMap = new Map(prev);
          newMap.set(cursor.userId, cursor);
          return newMap;
        });
        handler(cursor);
      });
      return () => {
        connection.off('CursorMoved', handler);
      };
    },
    [connection]
  );

  const onReceiveChatMessage = useCallback(
    (handler: (message: ChatMessage) => void) => {
      if (!connection) return () => {};

      connection.on('ReceiveChatMessage', handler);
      return () => {
        connection.off('ReceiveChatMessage', handler);
      };
    },
    [connection]
  );

  const onUserJoined = useCallback(
    (handler: (participant: SessionParticipant) => void) => {
      if (!connection) return () => {};

      connection.on('UserJoined', (participant: SessionParticipant) => {
        setParticipants((prev) => [...prev, participant.userId]);
        handler(participant);
      });
      return () => {
        connection.off('UserJoined', handler);
      };
    },
    [connection]
  );

  const onUserLeft = useCallback(
    (handler: (data: { userId: string; timestamp: string }) => void) => {
      if (!connection) return () => {};

      connection.on('UserLeft', (data: { userId: string; timestamp: string }) => {
        setParticipants((prev) => prev.filter((id) => id !== data.userId));
        setCursorPositions((prev) => {
          const newMap = new Map(prev);
          newMap.delete(data.userId);
          return newMap;
        });
        handler(data);
      });
      return () => {
        connection.off('UserLeft', handler);
      };
    },
    [connection]
  );

  const onCodeExecutionStarted = useCallback(
    (handler: (data: { userId: string; timestamp: string }) => void) => {
      if (!connection) return () => {};

      connection.on('CodeExecutionStarted', handler);
      return () => {
        connection.off('CodeExecutionStarted', handler);
      };
    },
    [connection]
  );

  const onSessionCompleted = useCallback(
    (handler: (data: { sessionId: string; xpEarned: number; timestamp: string }) => void) => {
      if (!connection) return () => {};

      connection.on('SessionCompleted', handler);
      return () => {
        connection.off('SessionCompleted', handler);
      };
    },
    [connection]
  );

  return {
    connection,
    isConnected,
    currentSessionId,
    participants,
    cursorPositions,
    createSession,
    joinSession,
    leaveSession,
    sendOperation,
    updateCursor,
    sendChatMessage,
    runCode,
    completeSession,
    onReceiveOperation,
    onCursorMoved,
    onReceiveChatMessage,
    onUserJoined,
    onUserLeft,
    onCodeExecutionStarted,
    onSessionCompleted,
  };
}
