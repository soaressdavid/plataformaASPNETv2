'use client';

import React from 'react';

export interface BugValidationResult {
  lineNumber: number;
  expectedDescription: string;
  wasIdentified: boolean;
  userDescription?: string;
}

export interface CodeReviewResultsProps {
  totalExpectedBugs: number;
  correctlyIdentified: number;
  missedBugs: number;
  falsePositives: number;
  accuracyPercentage: number;
  xpAwarded: number;
  bugResults: BugValidationResult[];
  onClose: () => void;
}

export const CodeReviewResults: React.FC<CodeReviewResultsProps> = ({
  totalExpectedBugs,
  correctlyIdentified,
  missedBugs,
  falsePositives,
  accuracyPercentage,
  xpAwarded,
  bugResults,
  onClose,
}) => {
  const passed = accuracyPercentage >= 70;

  return (
    <div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50 p-4">
      <div className="bg-white rounded-lg shadow-xl max-w-3xl w-full max-h-[90vh] overflow-hidden">
        {/* Header */}
        <div
          className={`px-6 py-4 border-b ${
            passed
              ? 'bg-green-50 border-green-200'
              : 'bg-red-50 border-red-200'
          }`}
        >
          <div className="flex items-center justify-between">
            <div>
              <h2 className="text-2xl font-bold text-gray-900">
                {passed ? '✓ Code Review Complete!' : '✗ Review Incomplete'}
              </h2>
              <p className="text-sm text-gray-600 mt-1">
                {passed
                  ? 'Great job identifying the bugs!'
                  : 'Keep practicing to improve your bug detection skills.'}
              </p>
            </div>
            <button
              onClick={onClose}
              className="text-gray-400 hover:text-gray-600 text-2xl"
            >
              ✕
            </button>
          </div>
        </div>

        {/* Stats */}
        <div className="px-6 py-4 bg-gray-50 border-b border-gray-200">
          <div className="grid grid-cols-2 md:grid-cols-4 gap-4">
            <div className="text-center">
              <div className="text-3xl font-bold text-blue-600">{accuracyPercentage}%</div>
              <div className="text-sm text-gray-600 mt-1">Accuracy</div>
            </div>
            <div className="text-center">
              <div className="text-3xl font-bold text-green-600">{correctlyIdentified}</div>
              <div className="text-sm text-gray-600 mt-1">Correct</div>
            </div>
            <div className="text-center">
              <div className="text-3xl font-bold text-yellow-600">{missedBugs}</div>
              <div className="text-sm text-gray-600 mt-1">Missed</div>
            </div>
            <div className="text-center">
              <div className="text-3xl font-bold text-red-600">{falsePositives}</div>
              <div className="text-sm text-gray-600 mt-1">False Positives</div>
            </div>
          </div>

          {xpAwarded > 0 && (
            <div className="mt-4 text-center">
              <div className="inline-flex items-center gap-2 px-4 py-2 bg-purple-100 text-purple-800 rounded-full">
                <span className="text-lg font-semibold">+{xpAwarded} XP</span>
                <span className="text-sm">earned</span>
              </div>
            </div>
          )}
        </div>

        {/* Bug Results */}
        <div className="px-6 py-4 overflow-y-auto max-h-[50vh]">
          <h3 className="text-lg font-semibold text-gray-900 mb-4">Bug Details</h3>
          <div className="space-y-3">
            {bugResults.map((bug, index) => (
              <div
                key={index}
                className={`p-4 rounded-lg border ${
                  bug.wasIdentified
                    ? 'bg-green-50 border-green-200'
                    : 'bg-red-50 border-red-200'
                }`}
              >
                <div className="flex items-start justify-between mb-2">
                  <div className="flex items-center gap-2">
                    <span
                      className={`text-lg ${
                        bug.wasIdentified ? 'text-green-600' : 'text-red-600'
                      }`}
                    >
                      {bug.wasIdentified ? '✓' : '✗'}
                    </span>
                    <span className="text-sm font-medium text-gray-700">
                      Line {bug.lineNumber}
                    </span>
                  </div>
                  <span
                    className={`px-2 py-1 text-xs font-medium rounded ${
                      bug.wasIdentified
                        ? 'bg-green-100 text-green-800'
                        : 'bg-red-100 text-red-800'
                    }`}
                  >
                    {bug.wasIdentified ? 'Identified' : 'Missed'}
                  </span>
                </div>

                <div className="space-y-2">
                  <div>
                    <p className="text-xs text-gray-600 mb-1">Expected Issue:</p>
                    <p className="text-sm text-gray-900">{bug.expectedDescription}</p>
                  </div>

                  {bug.wasIdentified && bug.userDescription && (
                    <div>
                      <p className="text-xs text-gray-600 mb-1">Your Description:</p>
                      <p className="text-sm text-gray-700">{bug.userDescription}</p>
                    </div>
                  )}

                  {!bug.wasIdentified && (
                    <div className="mt-2 p-2 bg-yellow-50 border border-yellow-200 rounded">
                      <p className="text-xs text-yellow-800">
                        💡 Tip: Look for common issues like null checks, off-by-one errors, and
                        security vulnerabilities.
                      </p>
                    </div>
                  )}
                </div>
              </div>
            ))}
          </div>

          {falsePositives > 0 && (
            <div className="mt-4 p-4 bg-orange-50 border border-orange-200 rounded-lg">
              <p className="text-sm text-orange-800">
                <strong>Note:</strong> You identified {falsePositives} issue(s) that weren't
                actual bugs. Be careful not to over-report issues.
              </p>
            </div>
          )}
        </div>

        {/* Footer */}
        <div className="px-6 py-4 border-t border-gray-200 bg-gray-50">
          <button
            onClick={onClose}
            className="w-full px-4 py-2 bg-blue-600 text-white font-semibold rounded-md hover:bg-blue-700 transition-colors"
          >
            Close
          </button>
        </div>
      </div>
    </div>
  );
};
