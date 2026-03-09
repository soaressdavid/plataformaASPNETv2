'use client';

import React, { useEffect, useState } from 'react';
import { useParams, useRouter } from 'next/navigation';
import { CollaborativeEditor } from '@/lib/components/Collaboration/CollaborativeEditor';
import { SessionManager } from '@/lib/components/Collaboration/SessionManager';
import { useCollaboration } from '@/lib/hooks/useCollaboration';

/**
 * Collaborative coding session page
 * Validates: Requirements 32.1, 32.2, 32.3, 32.4, 32.6, 32.7
 */

export default function CollaborativeSessionPage() {
  const params = useParams();
  const router = useRouter();
  const sessionId = params.sessionId as string;
  
  const [sessionData, setSessionData] = useState<any>(null);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [code, setCode] = useState('');

  const { joinSession, isConnected } = useCollaboration(
    process.env.NEXT_PUBLIC_COLLABORATION_HUB_URL || 'http://localhost:5007/hubs/collaboration'
  );

  useEffect(() => {
    const loadSession = async () => {
      try {
        setIsLoading(true);
        
        // Join the session
        const result = await joinSession(sessionId);
        setSessionData(result);
        setCode(result.code);
        
        setIsLoading(false);
      } catch (err) {
        console.error('Error loading session:', err);
        setError(err instanceof Error ? err.message : 'Failed to load session');
        setIsLoading(false);
      }
    };

    if (isConnected) {
      loadSession();
    }
  }, [sessionId, isConnected, joinSession]);

  const handleCodeChange = (newCode: string) => {
    setCode(newCode);
  };

  const handleRunCode = async () => {
    // In a real implementation, this would call the code execution service
    console.log('Running code:', code);
    alert('Code execution would happen here. This would integrate with the Code Executor service.');
  };

  const handleLeave = () => {
    router.push('/collaborate');
  };

  const handleComplete = (xpEarned: number) => {
    alert(`Session completed! You earned ${xpEarned} XP`);
    router.push('/collaborate');
  };

  if (isLoading) {
    return (
      <div className="flex items-center justify-center h-screen">
        <div className="text-center">
          <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-blue-600 mx-auto mb-4"></div>
          <p className="text-gray-600">Connecting to session...</p>
        </div>
      </div>
    );
  }

  if (error) {
    return (
      <div className="flex items-center justify-center h-screen">
        <div className="text-center">
          <div className="text-red-600 text-xl mb-4">⚠️</div>
          <h2 className="text-xl font-semibold mb-2">Error Loading Session</h2>
          <p className="text-gray-600 mb-4">{error}</p>
          <button
            onClick={() => router.push('/collaborate')}
            className="px-4 py-2 bg-blue-600 text-white rounded hover:bg-blue-700"
          >
            Back to Sessions
          </button>
        </div>
      </div>
    );
  }

  return (
    <div className="collaborative-session h-screen flex flex-col">
      {/* Header */}
      <header className="bg-white border-b px-6 py-4">
        <div className="flex items-center justify-between">
          <div>
            <h1 className="text-2xl font-bold">Collaborative Coding Session</h1>
            <p className="text-gray-600 text-sm">Session ID: {sessionId.substring(0, 8)}</p>
          </div>
          <div className="flex items-center gap-4">
            <div className={`flex items-center gap-2 ${isConnected ? 'text-green-600' : 'text-red-600'}`}>
              <div className={`w-3 h-3 rounded-full ${isConnected ? 'bg-green-600' : 'bg-red-600'}`} />
              <span className="text-sm font-medium">
                {isConnected ? 'Connected' : 'Disconnected'}
              </span>
            </div>
          </div>
        </div>
      </header>

      {/* Main Content */}
      <div className="flex-1 flex overflow-hidden">
        {/* Editor Panel */}
        <div className="flex-1 flex flex-col">
          <CollaborativeEditor
            sessionId={sessionId}
            initialCode={code}
            language="csharp"
            onCodeChange={handleCodeChange}
            onRunCode={handleRunCode}
          />
        </div>

        {/* Sidebar */}
        <div className="w-80 border-l bg-white">
          <SessionManager
            sessionId={sessionId}
            onLeave={handleLeave}
            onComplete={handleComplete}
          />
        </div>
      </div>
    </div>
  );
}
