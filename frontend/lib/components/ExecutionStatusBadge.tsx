'use client';

import React from 'react';
import { ExecutionStatus } from '../types';

interface ExecutionStatusBadgeProps {
  status: ExecutionStatus;
  executionTime?: number;
}

/**
 * Visual badge component for displaying execution status
 */
export const ExecutionStatusBadge: React.FC<ExecutionStatusBadgeProps> = ({
  status,
  executionTime,
}) => {
  const getStatusConfig = () => {
    switch (status) {
      case ExecutionStatus.Queued:
        return {
          icon: '⏳',
          text: 'Queued',
          bgColor: 'bg-yellow-500',
          textColor: 'text-yellow-100',
        };
      case ExecutionStatus.Running:
        return {
          icon: '▶️',
          text: 'Running',
          bgColor: 'bg-blue-500',
          textColor: 'text-blue-100',
        };
      case ExecutionStatus.Completed:
        return {
          icon: '✅',
          text: 'Completed',
          bgColor: 'bg-green-500',
          textColor: 'text-green-100',
        };
      case ExecutionStatus.Failed:
        return {
          icon: '❌',
          text: 'Failed',
          bgColor: 'bg-red-500',
          textColor: 'text-red-100',
        };
      case ExecutionStatus.Timeout:
        return {
          icon: '⏱️',
          text: 'Timeout',
          bgColor: 'bg-orange-500',
          textColor: 'text-orange-100',
        };
      case ExecutionStatus.MemoryExceeded:
        return {
          icon: '💾',
          text: 'Memory Exceeded',
          bgColor: 'bg-purple-500',
          textColor: 'text-purple-100',
        };
      default:
        return {
          icon: '❓',
          text: 'Unknown',
          bgColor: 'bg-gray-500',
          textColor: 'text-gray-100',
        };
    }
  };

  const config = getStatusConfig();

  return (
    <div className="flex items-center gap-2">
      <span
        className={`inline-flex items-center gap-1.5 px-3 py-1 rounded-full text-sm font-medium ${config.bgColor} ${config.textColor}`}
      >
        <span>{config.icon}</span>
        <span>{config.text}</span>
      </span>
      {executionTime !== undefined &&
        (status === ExecutionStatus.Completed || status === ExecutionStatus.Failed) && (
          <span className="text-sm text-gray-400">{executionTime}ms</span>
        )}
    </div>
  );
};
