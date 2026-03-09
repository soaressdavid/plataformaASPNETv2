import { useEffect, useState, useCallback, useRef } from 'react';
import * as signalR from '@microsoft/signalr';

/**
 * Hook for managing SignalR chat connection
 * Validates: Requirements 34.1, 34.4
 */
export interface ChatMessage {
  id: string;
  roomId: string;
  userId: string;
  userName: string;
  content: string;
  type: 'Text' | 'Code' | 'System';
  codeLanguage?: string;
  reactions?: string;
  createdAt: string;
  isModerated: boolean;
}

export interface ChatRoom {
  id: string;
  name: string;
  type: 'Global' | 'Course' | 'DirectMessage';
  courseId?: string;
  lastMessage?: ChatMessage;
  unreadCount: number;
}

export function useChatConnection(hubUrl: string) {
  const [connection, setConnection] = useState<signalR.HubConnection | null>(null);
  const [isConnected, setIsConnected] = useState(false);
  const [onlineUsers, setOnlineUsers] = useState<string[]>([]);
  const connectionRef = useRef<signalR.HubConnection | null>(null);

  useEffect(() => {
    // Create connection
    const newConnection = new signalR.HubConnectionBuilder()
      .withUrl(hubUrl, {
        accessTokenFactory: () => {
          // Get JWT token from localStorage or your auth provider
          return localStorage.getItem('token') || '';
        },
      })
      .withAutomaticReconnect({
        nextRetryDelayInMilliseconds: (retryContext) => {
          // Exponential backoff: 0s, 2s, 10s, 30s, then 30s
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
        console.log('SignalR connected');
        setIsConnected(true);
      } catch (error) {
        console.error('Error connecting to SignalR:', error);
        // Retry after 5 seconds
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

  const sendMessage = useCallback(
    async (roomId: string, content: string, codeLanguage?: string) => {
      if (!connection || !isConnected) {
        throw new Error('Not connected to chat server');
      }

      try {
        await connection.invoke('SendMessage', roomId, content, codeLanguage);
      } catch (error) {
        console.error('Error sending message:', error);
        throw error;
      }
    },
    [connection, isConnected]
  );

  const joinRoom = useCallback(
    async (roomId: string) => {
      if (!connection || !isConnected) {
        throw new Error('Not connected to chat server');
      }

      try {
        await connection.invoke('JoinRoom', roomId);
      } catch (error) {
        console.error('Error joining room:', error);
        throw error;
      }
    },
    [connection, isConnected]
  );

  const leaveRoom = useCallback(
    async (roomId: string) => {
      if (!connection || !isConnected) {
        throw new Error('Not connected to chat server');
      }

      try {
        await connection.invoke('LeaveRoom', roomId);
      } catch (error) {
        console.error('Error leaving room:', error);
        throw error;
      }
    },
    [connection, isConnected]
  );

  const addReaction = useCallback(
    async (messageId: string, emoji: string) => {
      if (!connection || !isConnected) {
        throw new Error('Not connected to chat server');
      }

      try {
        await connection.invoke('AddReaction', messageId, emoji);
      } catch (error) {
        console.error('Error adding reaction:', error);
        throw error;
      }
    },
    [connection, isConnected]
  );

  const getOnlineUsers = useCallback(
    async (roomId: string): Promise<string[]> => {
      if (!connection || !isConnected) {
        throw new Error('Not connected to chat server');
      }

      try {
        const users = await connection.invoke<string[]>('GetOnlineUsers', roomId);
        return users;
      } catch (error) {
        console.error('Error getting online users:', error);
        throw error;
      }
    },
    [connection, isConnected]
  );

  const reportMessage = useCallback(
    async (messageId: string, reason: string) => {
      if (!connection || !isConnected) {
        throw new Error('Not connected to chat server');
      }

      try {
        await connection.invoke('ReportMessage', messageId, reason);
      } catch (error) {
        console.error('Error reporting message:', error);
        throw error;
      }
    },
    [connection, isConnected]
  );

  const blockUser = useCallback(
    async (roomId: string, blockedUserId: string) => {
      if (!connection || !isConnected) {
        throw new Error('Not connected to chat server');
      }

      try {
        await connection.invoke('BlockUser', roomId, blockedUserId);
      } catch (error) {
        console.error('Error blocking user:', error);
        throw error;
      }
    },
    [connection, isConnected]
  );

  const onReceiveMessage = useCallback(
    (handler: (message: ChatMessage) => void) => {
      if (!connection) return () => {};

      connection.on('ReceiveMessage', handler);
      return () => {
        connection.off('ReceiveMessage', handler);
      };
    },
    [connection]
  );

  const onUserOnline = useCallback(
    (handler: (userId: string) => void) => {
      if (!connection) return () => {};

      connection.on('UserOnline', (userId: string) => {
        setOnlineUsers((prev) => [...new Set([...prev, userId])]);
        handler(userId);
      });
      return () => {
        connection.off('UserOnline', handler);
      };
    },
    [connection]
  );

  const onUserOffline = useCallback(
    (handler: (userId: string) => void) => {
      if (!connection) return () => {};

      connection.on('UserOffline', (userId: string) => {
        setOnlineUsers((prev) => prev.filter((id) => id !== userId));
        handler(userId);
      });
      return () => {
        connection.off('UserOffline', handler);
      };
    },
    [connection]
  );

  const onUserJoined = useCallback(
    (handler: (userId: string) => void) => {
      if (!connection) return () => {};

      connection.on('UserJoined', handler);
      return () => {
        connection.off('UserJoined', handler);
      };
    },
    [connection]
  );

  const onUserLeft = useCallback(
    (handler: (userId: string) => void) => {
      if (!connection) return () => {};

      connection.on('UserLeft', handler);
      return () => {
        connection.off('UserLeft', handler);
      };
    },
    [connection]
  );

  const onReactionAdded = useCallback(
    (handler: (data: { messageId: string; emoji: string; userId: string }) => void) => {
      if (!connection) return () => {};

      connection.on('ReactionAdded', handler);
      return () => {
        connection.off('ReactionAdded', handler);
      };
    },
    [connection]
  );

  return {
    connection,
    isConnected,
    onlineUsers,
    sendMessage,
    joinRoom,
    leaveRoom,
    addReaction,
    getOnlineUsers,
    reportMessage,
    blockUser,
    onReceiveMessage,
    onUserOnline,
    onUserOffline,
    onUserJoined,
    onUserLeft,
    onReactionAdded,
  };
}
