'use client';

import React, { useState, useEffect } from 'react';
import { useCollaboration, ChatMessage } from '@/lib/hooks/useCollaboration';

/**
 * Session management component with chat
 * Validates: Requirements 32.1, 32.4, 32.6, 32.9
 */

interface SessionManagerProps {
  sessionId: string;
  onLeave?: () => void;
  onComplete?: (xpEarned: number) => void;
}

export function SessionManager({ sessionId, onLeave, onComplete }: SessionManagerProps) {
  const [chatMessages, setChatMessages] = useState<ChatMessage[]>([]);
  const [chatInput, setChatInput] = useState('');
  const [showChat, setShowChat] = useState(false);

  const {
    isConnected,
    participants,
    leaveSession,
    sendChatMessage,
    completeSession,
    onReceiveChatMessage,
    onUserJoined,
    onUserLeft,
    onSessionCompleted,
  } = useCollaboration(process.env.NEXT_PUBLIC_COLLABORATION_HUB_URL || 'http://localhost:5007/hubs/collaboration');

  // Setup event listeners
  useEffect(() => {
    const unsubscribeChat = onReceiveChatMessage((message) => {
      setChatMessages((prev) => [...prev, message]);
    });

    const unsubscribeJoined = onUserJoined((participant) => {
      setChatMessages((prev) => [
        ...prev,
        {
          userId: 'system',
          userName: 'System',
          message: `${participant.userName} joined the session`,
          timestamp: participant.timestamp,
        },
      ]);
    });

    const unsubscribeLeft = onUserLeft((data) => {
      setChatMessages((prev) => [
        ...prev,
        {
          userId: 'system',
          userName: 'System',
          message: `User ${data.userId.substring(0, 8)} left the session`,
          timestamp: data.timestamp,
        },
      ]);
    });

    const unsubscribeCompleted = onSessionCompleted((data) => {
      onComplete?.(data.xpEarned);
    });

    return () => {
      unsubscribeChat();
      unsubscribeJoined();
      unsubscribeLeft();
      unsubscribeCompleted();
    };
  }, [onReceiveChatMessage, onUserJoined, onUserLeft, onSessionCompleted, onComplete]);

  const handleSendMessage = () => {
    if (chatInput.trim() && isConnected) {
      sendChatMessage(sessionId, chatInput);
      setChatInput('');
    }
  };

  const handleLeave = async () => {
    await leaveSession(sessionId);
    onLeave?.();
  };

  const handleComplete = async () => {
    // In a real implementation, this would calculate XP based on the challenge
    const xp = 50; // Example XP value
    await completeSession(sessionId, xp);
  };

  return (
    <div className="session-manager flex flex-col h-full">
      {/* Participants Panel */}
      <div className="participants-panel p-4 bg-gray-50 border-b">
        <h3 className="text-sm font-semibold mb-2">Participants ({participants.length}/2)</h3>
        <div className="flex flex-col gap-2">
          {participants.map((userId) => (
            <div key={userId} className="flex items-center gap-2">
              <div className="w-2 h-2 rounded-full bg-green-500" />
              <span className="text-sm">{userId.substring(0, 8)}</span>
            </div>
          ))}
        </div>
      </div>

      {/* Chat Toggle */}
      <button
        onClick={() => setShowChat(!showChat)}
        className="p-2 bg-blue-600 text-white hover:bg-blue-700"
      >
        {showChat ? 'Hide Chat' : 'Show Chat'} ({chatMessages.length})
      </button>

      {/* Chat Panel */}
      {showChat && (
        <div className="chat-panel flex-1 flex flex-col bg-white border-t">
          <div className="messages flex-1 overflow-y-auto p-4 space-y-2">
            {chatMessages.map((msg, index) => (
              <div
                key={index}
                className={`message ${msg.userId === 'system' ? 'text-gray-500 italic' : ''}`}
              >
                <span className="font-semibold text-sm">{msg.userName}: </span>
                <span className="text-sm">{msg.message}</span>
                <span className="text-xs text-gray-400 ml-2">
                  {new Date(msg.timestamp).toLocaleTimeString()}
                </span>
              </div>
            ))}
          </div>
          <div className="chat-input flex gap-2 p-2 border-t">
            <input
              type="text"
              value={chatInput}
              onChange={(e) => setChatInput(e.target.value)}
              onKeyPress={(e) => e.key === 'Enter' && handleSendMessage()}
              placeholder="Type a message..."
              className="flex-1 px-3 py-2 border rounded focus:outline-none focus:ring-2 focus:ring-blue-500"
              disabled={!isConnected}
            />
            <button
              onClick={handleSendMessage}
              disabled={!isConnected || !chatInput.trim()}
              className="px-4 py-2 bg-blue-600 text-white rounded hover:bg-blue-700 disabled:bg-gray-400 disabled:cursor-not-allowed"
            >
              Send
            </button>
          </div>
        </div>
      )}

      {/* Action Buttons */}
      <div className="actions flex gap-2 p-4 bg-gray-50 border-t">
        <button
          onClick={handleLeave}
          className="flex-1 px-4 py-2 bg-gray-600 text-white rounded hover:bg-gray-700"
        >
          Leave Session
        </button>
        <button
          onClick={handleComplete}
          className="flex-1 px-4 py-2 bg-green-600 text-white rounded hover:bg-green-700"
        >
          Complete Session
        </button>
      </div>
    </div>
  );
}
