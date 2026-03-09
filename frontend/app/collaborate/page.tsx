'use client';

import React, { useState, useEffect } from 'react';
import { useRouter } from 'next/navigation';
import { useCollaboration } from '@/lib/hooks/useCollaboration';

/**
 * Collaborative sessions list and creation page
 * Validates: Requirements 32.1, 32.8
 */

interface Session {
  id: string;
  name: string;
  language: string;
  status: string;
  createdAt: string;
  participantCount: number;
  participants: Array<{
    userId: string;
    userName: string;
    role: string;
  }>;
}

export default function CollaboratePage() {
  const router = useRouter();
  const [sessions, setSessions] = useState<Session[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [showCreateModal, setShowCreateModal] = useState(false);
  const [sessionName, setSessionName] = useState('');
  const [language, setLanguage] = useState('csharp');
  const [initialCode, setInitialCode] = useState('');

  const { createSession, isConnected } = useCollaboration(
    process.env.NEXT_PUBLIC_COLLABORATION_HUB_URL || 'http://localhost:5007/hubs/collaboration'
  );

  useEffect(() => {
    loadSessions();
  }, []);

  const loadSessions = async () => {
    try {
      setIsLoading(true);
      // In a real implementation, this would fetch from the API
      // const response = await fetch('/api/collaboration/sessions?userId=current-user-id');
      // const data = await response.json();
      // setSessions(data);
      
      // Mock data for now
      setSessions([]);
      setIsLoading(false);
    } catch (error) {
      console.error('Error loading sessions:', error);
      setIsLoading(false);
    }
  };

  const handleCreateSession = async () => {
    if (!sessionName.trim()) {
      alert('Please enter a session name');
      return;
    }

    try {
      const sessionId = await createSession(sessionName, undefined, language, initialCode);
      setShowCreateModal(false);
      router.push(`/collaborate/${sessionId}`);
    } catch (error) {
      console.error('Error creating session:', error);
      alert('Failed to create session');
    }
  };

  const handleJoinSession = (sessionId: string) => {
    router.push(`/collaborate/${sessionId}`);
  };

  return (
    <div className="collaborate-page min-h-screen bg-gray-50">
      {/* Header */}
      <header className="bg-white border-b">
        <div className="max-w-7xl mx-auto px-6 py-6">
          <div className="flex items-center justify-between">
            <div>
              <h1 className="text-3xl font-bold">Collaborative Coding</h1>
              <p className="text-gray-600 mt-1">Code together in real-time</p>
            </div>
            <button
              onClick={() => setShowCreateModal(true)}
              disabled={!isConnected}
              className="px-6 py-3 bg-blue-600 text-white rounded-lg hover:bg-blue-700 disabled:bg-gray-400 disabled:cursor-not-allowed font-medium"
            >
              Create New Session
            </button>
          </div>
        </div>
      </header>

      {/* Connection Status */}
      <div className="max-w-7xl mx-auto px-6 py-4">
        <div className={`flex items-center gap-2 ${isConnected ? 'text-green-600' : 'text-red-600'}`}>
          <div className={`w-2 h-2 rounded-full ${isConnected ? 'bg-green-600' : 'bg-red-600'}`} />
          <span className="text-sm font-medium">
            {isConnected ? 'Connected to collaboration server' : 'Disconnected from collaboration server'}
          </span>
        </div>
      </div>

      {/* Sessions List */}
      <div className="max-w-7xl mx-auto px-6 py-6">
        <h2 className="text-xl font-semibold mb-4">Active Sessions</h2>
        
        {isLoading ? (
          <div className="text-center py-12">
            <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-blue-600 mx-auto"></div>
          </div>
        ) : sessions.length === 0 ? (
          <div className="text-center py-12 bg-white rounded-lg border">
            <p className="text-gray-600 mb-4">No active sessions</p>
            <button
              onClick={() => setShowCreateModal(true)}
              disabled={!isConnected}
              className="px-6 py-2 bg-blue-600 text-white rounded hover:bg-blue-700 disabled:bg-gray-400 disabled:cursor-not-allowed"
            >
              Create Your First Session
            </button>
          </div>
        ) : (
          <div className="grid gap-4 md:grid-cols-2 lg:grid-cols-3">
            {sessions.map((session) => (
              <div key={session.id} className="bg-white rounded-lg border p-6 hover:shadow-lg transition-shadow">
                <div className="flex items-start justify-between mb-4">
                  <div>
                    <h3 className="font-semibold text-lg">{session.name}</h3>
                    <p className="text-sm text-gray-600">{session.language}</p>
                  </div>
                  <span className={`px-2 py-1 rounded text-xs font-medium ${
                    session.status === 'Active' ? 'bg-green-100 text-green-800' : 'bg-gray-100 text-gray-800'
                  }`}>
                    {session.status}
                  </span>
                </div>
                
                <div className="mb-4">
                  <p className="text-sm text-gray-600">
                    {session.participantCount}/2 participants
                  </p>
                  <div className="mt-2 flex flex-wrap gap-2">
                    {session.participants.map((participant) => (
                      <span key={participant.userId} className="text-xs bg-gray-100 px-2 py-1 rounded">
                        {participant.userName}
                      </span>
                    ))}
                  </div>
                </div>
                
                <button
                  onClick={() => handleJoinSession(session.id)}
                  disabled={session.participantCount >= 2}
                  className="w-full px-4 py-2 bg-blue-600 text-white rounded hover:bg-blue-700 disabled:bg-gray-400 disabled:cursor-not-allowed"
                >
                  {session.participantCount >= 2 ? 'Session Full' : 'Join Session'}
                </button>
              </div>
            ))}
          </div>
        )}
      </div>

      {/* Create Session Modal */}
      {showCreateModal && (
        <div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50">
          <div className="bg-white rounded-lg p-6 max-w-md w-full mx-4">
            <h2 className="text-2xl font-bold mb-4">Create New Session</h2>
            
            <div className="space-y-4">
              <div>
                <label className="block text-sm font-medium mb-1">Session Name</label>
                <input
                  type="text"
                  value={sessionName}
                  onChange={(e) => setSessionName(e.target.value)}
                  placeholder="e.g., Algorithm Practice"
                  className="w-full px-3 py-2 border rounded focus:outline-none focus:ring-2 focus:ring-blue-500"
                />
              </div>
              
              <div>
                <label className="block text-sm font-medium mb-1">Language</label>
                <select
                  value={language}
                  onChange={(e) => setLanguage(e.target.value)}
                  className="w-full px-3 py-2 border rounded focus:outline-none focus:ring-2 focus:ring-blue-500"
                >
                  <option value="csharp">C#</option>
                  <option value="javascript">JavaScript</option>
                  <option value="typescript">TypeScript</option>
                  <option value="python">Python</option>
                </select>
              </div>
              
              <div>
                <label className="block text-sm font-medium mb-1">Initial Code (Optional)</label>
                <textarea
                  value={initialCode}
                  onChange={(e) => setInitialCode(e.target.value)}
                  placeholder="// Start with some code..."
                  rows={4}
                  className="w-full px-3 py-2 border rounded focus:outline-none focus:ring-2 focus:ring-blue-500 font-mono text-sm"
                />
              </div>
            </div>
            
            <div className="flex gap-3 mt-6">
              <button
                onClick={() => setShowCreateModal(false)}
                className="flex-1 px-4 py-2 border rounded hover:bg-gray-50"
              >
                Cancel
              </button>
              <button
                onClick={handleCreateSession}
                className="flex-1 px-4 py-2 bg-blue-600 text-white rounded hover:bg-blue-700"
              >
                Create Session
              </button>
            </div>
          </div>
        </div>
      )}
    </div>
  );
}
