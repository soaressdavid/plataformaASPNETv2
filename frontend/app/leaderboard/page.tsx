'use client';

import { ProtectedRoute } from '@/lib/components/ProtectedRoute';
import { Leaderboard } from '@/lib/components/Leaderboard';
import { Navigation } from '@/lib/components';

export default function LeaderboardPage() {
  return (
    <ProtectedRoute>
      <div className="min-h-screen bg-gray-50">
        <Navigation />
        <main className="max-w-7xl mx-auto py-6 sm:px-6 lg:px-8">
          <Leaderboard />
        </main>
      </div>
    </ProtectedRoute>
  );
}
