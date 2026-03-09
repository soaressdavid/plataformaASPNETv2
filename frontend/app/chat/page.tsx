'use client';

import { useState } from 'react';
import { ChatRoom } from '@/lib/components/Chat/ChatRoom';
import { ChatRoomList } from '@/lib/components/Chat/ChatRoomList';
import { ChatRoom as ChatRoomType } from '@/lib/hooks/useChatConnection';

/**
 * Chat page with room list and active chat
 * Validates: Requirements 34.1, 34.2, 34.3, 34.4
 */
export default function ChatPage() {
  const [selectedRoom, setSelectedRoom] = useState<ChatRoomType | null>(null);
  const [currentUserId] = useState(() => {
    // Get current user ID from auth context or localStorage
    return localStorage.getItem('userId') || 'unknown';
  });

  return (
    <div className="h-screen flex">
      {/* Room List Sidebar */}
      <div className="w-80 flex-shrink-0">
        <ChatRoomList
          onSelectRoom={setSelectedRoom}
          selectedRoomId={selectedRoom?.id}
        />
      </div>

      {/* Chat Room */}
      <div className="flex-1">
        {selectedRoom ? (
          <ChatRoom
            roomId={selectedRoom.id}
            roomName={selectedRoom.name}
            currentUserId={currentUserId}
          />
        ) : (
          <div className="flex items-center justify-center h-full bg-gray-50 dark:bg-gray-900">
            <div className="text-center">
              <h2 className="text-2xl font-bold text-gray-900 dark:text-white mb-2">
                Welcome to Chat
              </h2>
              <p className="text-gray-500 dark:text-gray-400">
                Select a room to start chatting
              </p>
            </div>
          </div>
        )}
      </div>
    </div>
  );
}
