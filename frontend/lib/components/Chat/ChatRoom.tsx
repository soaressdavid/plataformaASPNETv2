'use client';

import { useState, useEffect, useRef } from 'react';
import { useChatConnection, ChatMessage } from '@/lib/hooks/useChatConnection';
import { Prism as SyntaxHighlighter } from 'react-syntax-highlighter';
import { vscDarkPlus } from 'react-syntax-highlighter/dist/esm/styles/prism';

/**
 * Chat room component with real-time messaging
 * Validates: Requirements 34.1, 34.2, 34.3, 34.4, 34.5, 34.6
 */
interface ChatRoomProps {
  roomId: string;
  roomName: string;
  currentUserId: string;
}

export function ChatRoom({ roomId, roomName, currentUserId }: ChatRoomProps) {
  const [messages, setMessages] = useState<ChatMessage[]>([]);
  const [inputMessage, setInputMessage] = useState('');
  const [isCodeMode, setIsCodeMode] = useState(false);
  const [codeLanguage, setCodeLanguage] = useState('csharp');
  const [onlineUsers, setOnlineUsers] = useState<string[]>([]);
  const messagesEndRef = useRef<HTMLDivElement>(null);

  const chatConnection = useChatConnection(
    process.env.NEXT_PUBLIC_CHAT_HUB_URL || 'http://localhost:5010/hubs/chat'
  );

  useEffect(() => {
    if (!chatConnection.isConnected) return;

    // Join room
    chatConnection.joinRoom(roomId).catch(console.error);

    // Load online users
    chatConnection.getOnlineUsers(roomId).then(setOnlineUsers).catch(console.error);

    // Load message history
    loadMessages();

    // Setup message listener
    const unsubscribe = chatConnection.onReceiveMessage((message) => {
      if (message.roomId === roomId) {
        setMessages((prev) => [...prev, message]);
      }
    });

    // Setup online/offline listeners
    const unsubOnline = chatConnection.onUserOnline((userId) => {
      setOnlineUsers((prev) => [...new Set([...prev, userId])]);
    });

    const unsubOffline = chatConnection.onUserOffline((userId) => {
      setOnlineUsers((prev) => prev.filter((id) => id !== userId));
    });

    return () => {
      unsubscribe();
      unsubOnline();
      unsubOffline();
      chatConnection.leaveRoom(roomId).catch(console.error);
    };
  }, [chatConnection.isConnected, roomId]);

  useEffect(() => {
    // Scroll to bottom when new messages arrive
    messagesEndRef.current?.scrollIntoView({ behavior: 'smooth' });
  }, [messages]);

  const loadMessages = async () => {
    try {
      const response = await fetch(
        `${process.env.NEXT_PUBLIC_API_URL || 'http://localhost:5010'}/api/chat/rooms/${roomId}/messages`,
        {
          headers: {
            Authorization: `Bearer ${localStorage.getItem('token')}`,
          },
        }
      );

      if (response.ok) {
        const data = await response.json();
        setMessages(data);
      }
    } catch (error) {
      console.error('Error loading messages:', error);
    }
  };

  const handleSendMessage = async (e: React.FormEvent) => {
    e.preventDefault();

    if (!inputMessage.trim() || !chatConnection.isConnected) return;

    try {
      await chatConnection.sendMessage(
        roomId,
        inputMessage,
        isCodeMode ? codeLanguage : undefined
      );
      setInputMessage('');
    } catch (error) {
      console.error('Error sending message:', error);
    }
  };

  const handleAddReaction = async (messageId: string, emoji: string) => {
    try {
      await chatConnection.addReaction(messageId, emoji);
    } catch (error) {
      console.error('Error adding reaction:', error);
    }
  };

  const handleReportMessage = async (messageId: string) => {
    const reason = prompt('Why are you reporting this message?');
    if (!reason) return;

    try {
      await chatConnection.reportMessage(messageId, reason);
      alert('Message reported successfully');
    } catch (error) {
      console.error('Error reporting message:', error);
    }
  };

  const formatTime = (dateString: string) => {
    const date = new Date(dateString);
    return date.toLocaleTimeString('en-US', { hour: '2-digit', minute: '2-digit' });
  };

  return (
    <div className="flex flex-col h-full bg-white dark:bg-gray-900 rounded-lg shadow-lg">
      {/* Header */}
      <div className="flex items-center justify-between p-4 border-b border-gray-200 dark:border-gray-700">
        <div>
          <h2 className="text-xl font-bold text-gray-900 dark:text-white">{roomName}</h2>
          <p className="text-sm text-gray-500 dark:text-gray-400">
            {onlineUsers.length} online
          </p>
        </div>
        <div className="flex items-center gap-2">
          <span
            className={`w-3 h-3 rounded-full ${
              chatConnection.isConnected ? 'bg-green-500' : 'bg-red-500'
            }`}
          />
          <span className="text-sm text-gray-600 dark:text-gray-400">
            {chatConnection.isConnected ? 'Connected' : 'Disconnected'}
          </span>
        </div>
      </div>

      {/* Messages */}
      <div className="flex-1 overflow-y-auto p-4 space-y-4">
        {messages.map((message) => (
          <div
            key={message.id}
            className={`flex ${
              message.userId === currentUserId ? 'justify-end' : 'justify-start'
            }`}
          >
            <div
              className={`max-w-[70%] rounded-lg p-3 ${
                message.userId === currentUserId
                  ? 'bg-blue-500 text-white'
                  : 'bg-gray-100 dark:bg-gray-800 text-gray-900 dark:text-white'
              }`}
            >
              {message.userId !== currentUserId && (
                <div className="text-xs font-semibold mb-1 opacity-75">
                  {message.userName}
                </div>
              )}

              {message.type === 'Code' && message.codeLanguage ? (
                <div className="bg-gray-900 rounded overflow-hidden">
                  <div className="text-xs px-2 py-1 bg-gray-800 text-gray-400">
                    {message.codeLanguage}
                  </div>
                  <SyntaxHighlighter
                    language={message.codeLanguage}
                    style={vscDarkPlus}
                    customStyle={{ margin: 0, fontSize: '0.875rem' }}
                  >
                    {message.content}
                  </SyntaxHighlighter>
                </div>
              ) : (
                <div className="whitespace-pre-wrap break-words">{message.content}</div>
              )}

              <div className="flex items-center justify-between mt-2 text-xs opacity-75">
                <span>{formatTime(message.createdAt)}</span>
                <div className="flex items-center gap-2">
                  <button
                    onClick={() => handleAddReaction(message.id, '👍')}
                    className="hover:scale-125 transition-transform"
                  >
                    👍
                  </button>
                  <button
                    onClick={() => handleAddReaction(message.id, '❤️')}
                    className="hover:scale-125 transition-transform"
                  >
                    ❤️
                  </button>
                  {message.userId !== currentUserId && (
                    <button
                      onClick={() => handleReportMessage(message.id)}
                      className="text-red-500 hover:text-red-700"
                      title="Report message"
                    >
                      ⚠️
                    </button>
                  )}
                </div>
              </div>
            </div>
          </div>
        ))}
        <div ref={messagesEndRef} />
      </div>

      {/* Input */}
      <form onSubmit={handleSendMessage} className="p-4 border-t border-gray-200 dark:border-gray-700">
        <div className="flex items-center gap-2 mb-2">
          <label className="flex items-center gap-2 text-sm text-gray-600 dark:text-gray-400">
            <input
              type="checkbox"
              checked={isCodeMode}
              onChange={(e) => setIsCodeMode(e.target.checked)}
              className="rounded"
            />
            Code mode
          </label>
          {isCodeMode && (
            <select
              value={codeLanguage}
              onChange={(e) => setCodeLanguage(e.target.value)}
              className="text-sm border border-gray-300 dark:border-gray-600 rounded px-2 py-1 bg-white dark:bg-gray-800 text-gray-900 dark:text-white"
            >
              <option value="csharp">C#</option>
              <option value="javascript">JavaScript</option>
              <option value="typescript">TypeScript</option>
              <option value="python">Python</option>
              <option value="sql">SQL</option>
            </select>
          )}
        </div>
        <div className="flex gap-2">
          <textarea
            value={inputMessage}
            onChange={(e) => setInputMessage(e.target.value)}
            onKeyDown={(e) => {
              if (e.key === 'Enter' && !e.shiftKey) {
                e.preventDefault();
                handleSendMessage(e);
              }
            }}
            placeholder={isCodeMode ? 'Paste your code here...' : 'Type a message...'}
            className="flex-1 border border-gray-300 dark:border-gray-600 rounded-lg px-4 py-2 bg-white dark:bg-gray-800 text-gray-900 dark:text-white focus:outline-none focus:ring-2 focus:ring-blue-500 resize-none"
            rows={isCodeMode ? 5 : 1}
            disabled={!chatConnection.isConnected}
          />
          <button
            type="submit"
            disabled={!chatConnection.isConnected || !inputMessage.trim()}
            className="px-6 py-2 bg-blue-500 text-white rounded-lg hover:bg-blue-600 disabled:bg-gray-300 disabled:cursor-not-allowed transition-colors"
          >
            Send
          </button>
        </div>
      </form>
    </div>
  );
}
