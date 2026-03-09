'use client';

import React, { useState } from 'react';

export interface BugMarker {
  lineNumber: number;
  description: string;
  severity: 'Low' | 'Medium' | 'High';
}

export interface CodeReviewPanelProps {
  code: string;
  onSubmit: (identifiedIssues: BugMarker[]) => void;
  isSubmitting?: boolean;
}

export const CodeReviewPanel: React.FC<CodeReviewPanelProps> = ({
  code,
  onSubmit,
  isSubmitting = false,
}) => {
  const [identifiedIssues, setIdentifiedIssues] = useState<BugMarker[]>([]);
  const [selectedLine, setSelectedLine] = useState<number | null>(null);
  const [issueDescription, setIssueDescription] = useState('');
  const [issueSeverity, setIssueSeverity] = useState<'Low' | 'Medium' | 'High'>('Medium');
  const [showAddIssueForm, setShowAddIssueForm] = useState(false);

  const codeLines = code.split('\n');

  const handleLineClick = (lineNumber: number) => {
    setSelectedLine(lineNumber);
    setShowAddIssueForm(true);
    setIssueDescription('');
    setIssueSeverity('Medium');
  };

  const handleAddIssue = () => {
    if (selectedLine === null || !issueDescription.trim()) {
      return;
    }

    const newIssue: BugMarker = {
      lineNumber: selectedLine,
      description: issueDescription.trim(),
      severity: issueSeverity,
    };

    setIdentifiedIssues([...identifiedIssues, newIssue]);
    setShowAddIssueForm(false);
    setSelectedLine(null);
    setIssueDescription('');
  };

  const handleRemoveIssue = (index: number) => {
    setIdentifiedIssues(identifiedIssues.filter((_, i) => i !== index));
  };

  const handleSubmit = () => {
    onSubmit(identifiedIssues);
  };

  const getLineIssues = (lineNumber: number) => {
    return identifiedIssues.filter((issue) => issue.lineNumber === lineNumber);
  };

  const getSeverityColor = (severity: 'Low' | 'Medium' | 'High') => {
    switch (severity) {
      case 'High':
        return 'bg-red-100 text-red-800 border-red-300';
      case 'Medium':
        return 'bg-yellow-100 text-yellow-800 border-yellow-300';
      case 'Low':
        return 'bg-blue-100 text-blue-800 border-blue-300';
    }
  };

  return (
    <div className="flex flex-col h-full bg-white">
      {/* Header */}
      <div className="px-4 py-3 border-b border-gray-200 bg-gray-50">
        <div className="flex items-center justify-between">
          <div>
            <h3 className="text-lg font-semibold text-gray-900">Code Review Challenge</h3>
            <p className="text-sm text-gray-600 mt-1">
              Click on a line to mark an issue. Identify all bugs in the code.
            </p>
          </div>
          <div className="text-sm text-gray-600">
            Issues Found: <span className="font-semibold text-gray-900">{identifiedIssues.length}</span>
          </div>
        </div>
      </div>

      {/* Code Display */}
      <div className="flex-1 overflow-auto p-4">
        <div className="bg-gray-900 rounded-lg overflow-hidden">
          <div className="p-4">
            {codeLines.map((line, index) => {
              const lineNumber = index + 1;
              const lineIssues = getLineIssues(lineNumber);
              const hasIssue = lineIssues.length > 0;

              return (
                <div key={lineNumber} className="group">
                  <div
                    className={`flex items-start hover:bg-gray-800 cursor-pointer transition-colors ${
                      hasIssue ? 'bg-red-900 bg-opacity-20' : ''
                    }`}
                    onClick={() => handleLineClick(lineNumber)}
                  >
                    {/* Line Number */}
                    <div
                      className={`flex-shrink-0 w-12 text-right pr-4 py-1 text-gray-500 select-none ${
                        hasIssue ? 'text-red-400 font-semibold' : ''
                      }`}
                    >
                      {lineNumber}
                    </div>

                    {/* Code Line */}
                    <div className="flex-1 py-1 font-mono text-sm text-gray-100 whitespace-pre">
                      {line || ' '}
                    </div>

                    {/* Issue Indicator */}
                    {hasIssue && (
                      <div className="flex-shrink-0 px-2 py-1">
                        <span className="inline-flex items-center justify-center w-6 h-6 bg-red-500 text-white text-xs font-bold rounded-full">
                          {lineIssues.length}
                        </span>
                      </div>
                    )}
                  </div>

                  {/* Issue Tags */}
                  {lineIssues.map((issue, issueIndex) => (
                    <div
                      key={issueIndex}
                      className="ml-12 mb-2 flex items-start gap-2 bg-gray-800 p-2 rounded"
                    >
                      <span
                        className={`px-2 py-1 text-xs font-medium rounded ${getSeverityColor(
                          issue.severity
                        )}`}
                      >
                        {issue.severity}
                      </span>
                      <span className="flex-1 text-sm text-gray-300">{issue.description}</span>
                      <button
                        onClick={(e) => {
                          e.stopPropagation();
                          const globalIndex = identifiedIssues.findIndex(
                            (i) => i.lineNumber === lineNumber && i.description === issue.description
                          );
                          handleRemoveIssue(globalIndex);
                        }}
                        className="text-gray-400 hover:text-red-400 transition-colors"
                      >
                        ✕
                      </button>
                    </div>
                  ))}
                </div>
              );
            })}
          </div>
        </div>
      </div>

      {/* Add Issue Form */}
      {showAddIssueForm && selectedLine !== null && (
        <div className="border-t border-gray-200 bg-gray-50 p-4">
          <h4 className="text-sm font-semibold text-gray-900 mb-3">
            Add Issue at Line {selectedLine}
          </h4>
          <div className="space-y-3">
            <div>
              <label className="block text-sm font-medium text-gray-700 mb-1">
                Severity
              </label>
              <select
                value={issueSeverity}
                onChange={(e) => setIssueSeverity(e.target.value as 'Low' | 'Medium' | 'High')}
                className="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
              >
                <option value="Low">Low</option>
                <option value="Medium">Medium</option>
                <option value="High">High</option>
              </select>
            </div>
            <div>
              <label className="block text-sm font-medium text-gray-700 mb-1">
                Description
              </label>
              <textarea
                value={issueDescription}
                onChange={(e) => setIssueDescription(e.target.value)}
                placeholder="Describe the issue you found..."
                rows={3}
                className="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
              />
            </div>
            <div className="flex gap-2">
              <button
                onClick={handleAddIssue}
                disabled={!issueDescription.trim()}
                className="px-4 py-2 bg-blue-600 text-white rounded-md hover:bg-blue-700 disabled:bg-gray-300 disabled:cursor-not-allowed"
              >
                Add Issue
              </button>
              <button
                onClick={() => {
                  setShowAddIssueForm(false);
                  setSelectedLine(null);
                }}
                className="px-4 py-2 bg-gray-200 text-gray-700 rounded-md hover:bg-gray-300"
              >
                Cancel
              </button>
            </div>
          </div>
        </div>
      )}

      {/* Submit Button */}
      <div className="border-t border-gray-200 p-4 bg-white">
        <button
          onClick={handleSubmit}
          disabled={isSubmitting || identifiedIssues.length === 0}
          className="w-full px-4 py-3 bg-green-600 text-white font-semibold rounded-md hover:bg-green-700 disabled:bg-gray-300 disabled:cursor-not-allowed transition-colors"
        >
          {isSubmitting ? 'Submitting...' : `Submit Review (${identifiedIssues.length} issues)`}
        </button>
      </div>
    </div>
  );
};
