'use client';

import React, { useState } from 'react';
import { useRouter } from 'next/navigation';
import { CodeReviewPanel, CodeReviewResults, BugMarker } from '@/lib/components';
import { challengesApi } from '@/lib/api/challenges';

export default function CodeReviewChallengePage() {
  const router = useRouter();
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [results, setResults] = useState<any>(null);

  // Sample buggy code for demonstration
  const buggyCode = `public class UserService
{
    private readonly DbContext _context;
    
    public User GetUserById(int id)
    {
        var user = _context.Users.FirstOrDefault(u => u.Id == id);
        return user.Name; // Bug: Null reference exception
    }
    
    public List<User> GetActiveUsers()
    {
        var users = new List<User>();
        for (int i = 0; i <= users.Count; i++) // Bug: Off-by-one error
        {
            if (users[i].IsActive)
            {
                users.Add(users[i]);
            }
        }
        return users;
    }
    
    public void DeleteUser(string userId)
    {
        var query = "DELETE FROM Users WHERE Id = " + userId; // Bug: SQL injection
        _context.Database.ExecuteSqlRaw(query);
    }
    
    public decimal CalculateDiscount(decimal price, int percentage)
    {
        return price * percentage / 100; // Bug: Integer division issue
    }
}`;

  const handleSubmit = async (identifiedIssues: BugMarker[]) => {
    try {
      setIsSubmitting(true);
      
      // Get user ID from localStorage
      const userId = localStorage.getItem('user_id') || 'demo-user';
      
      // Submit to backend
      const response = await challengesApi.submitCodeReview('code-review-1', {
        userId,
        identifiedIssues: identifiedIssues.map(issue => ({
          lineNumber: issue.lineNumber,
          description: issue.description,
          severity: issue.severity,
        })),
      });
      
      setResults(response);
    } catch (error) {
      console.error('Error submitting code review:', error);
      alert('Failed to submit code review. Please try again.');
    } finally {
      setIsSubmitting(false);
    }
  };

  const handleCloseResults = () => {
    setResults(null);
  };

  return (
    <div className="h-screen flex flex-col bg-gray-50">
      {/* Header */}
      <div className="bg-white border-b border-gray-200 px-6 py-4">
        <div className="flex items-center justify-between">
          <div className="flex items-center gap-4">
            <button
              onClick={() => router.push('/challenges')}
              className="text-gray-600 hover:text-gray-900"
            >
              ← Back
            </button>
            <div>
              <h1 className="text-2xl font-bold text-gray-900">Code Review Challenge</h1>
              <p className="text-sm text-gray-600 mt-1">
                Find all the bugs in the UserService class
              </p>
            </div>
            <span className="px-3 py-1 rounded-full text-xs font-medium bg-yellow-100 text-yellow-700">
              Medium
            </span>
          </div>
        </div>
      </div>

      {/* Instructions */}
      <div className="bg-blue-50 border-b border-blue-200 px-6 py-3">
        <div className="flex items-start gap-3">
          <div className="flex-shrink-0 text-blue-600 text-xl">💡</div>
          <div className="flex-1">
            <h3 className="text-sm font-semibold text-blue-900 mb-1">Instructions</h3>
            <ul className="text-sm text-blue-800 space-y-1">
              <li>• Click on any line to mark an issue</li>
              <li>• Describe the bug and select its severity</li>
              <li>• Look for null reference errors, logic bugs, security issues, and more</li>
              <li>• Submit when you've found all the bugs (aim for 70%+ accuracy)</li>
            </ul>
          </div>
        </div>
      </div>

      {/* Code Review Panel */}
      <div className="flex-1 overflow-hidden">
        <CodeReviewPanel
          code={buggyCode}
          onSubmit={handleSubmit}
          isSubmitting={isSubmitting}
        />
      </div>

      {/* Results Modal */}
      {results && (
        <CodeReviewResults
          totalExpectedBugs={results.totalExpectedBugs}
          correctlyIdentified={results.correctlyIdentified}
          missedBugs={results.missedBugs}
          falsePositives={results.falsePositives}
          accuracyPercentage={results.accuracyPercentage}
          xpAwarded={results.xpAwarded}
          bugResults={results.bugResults}
          onClose={handleCloseResults}
        />
      )}
    </div>
  );
}
