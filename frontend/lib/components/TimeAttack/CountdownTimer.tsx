'use client';

import React, { useEffect, useState, useCallback } from 'react';

interface CountdownTimerProps {
  initialSeconds: number;
  onTimeUp: () => void;
  onTick?: (remainingSeconds: number) => void;
  isPaused?: boolean;
}

export const CountdownTimer: React.FC<CountdownTimerProps> = ({
  initialSeconds,
  onTimeUp,
  onTick,
  isPaused = false,
}) => {
  const [remainingSeconds, setRemainingSeconds] = useState(initialSeconds);

  // Memoize callbacks to prevent unnecessary re-renders
  const handleTimeUp = useCallback(() => {
    onTimeUp();
  }, [onTimeUp]);

  const handleTick = useCallback((newValue: number) => {
    if (onTick) {
      // Use setTimeout to avoid setState during render
      setTimeout(() => onTick(newValue), 0);
    }
  }, [onTick]);

  useEffect(() => {
    if (isPaused || remainingSeconds <= 0) return;

    const interval = setInterval(() => {
      setRemainingSeconds((prev) => {
        const newValue = prev - 1;
        
        if (newValue <= 0) {
          clearInterval(interval);
          setTimeout(() => handleTimeUp(), 0);
          return 0;
        }
        
        // Call onTick after state update
        handleTick(newValue);
        
        return newValue;
      });
    }, 1000);

    return () => clearInterval(interval);
  }, [isPaused, remainingSeconds, handleTimeUp, handleTick]);

  const formatTime = (seconds: number): string => {
    const mins = Math.floor(seconds / 60);
    const secs = seconds % 60;
    return `${mins.toString().padStart(2, '0')}:${secs.toString().padStart(2, '0')}`;
  };

  const getTimerColor = (): string => {
    if (remainingSeconds > 300) return 'text-green-600'; // > 5 minutes
    if (remainingSeconds > 60) return 'text-yellow-600'; // > 1 minute
    return 'text-red-600'; // < 1 minute
  };

  const getProgressPercentage = (): number => {
    return (remainingSeconds / initialSeconds) * 100;
  };

  return (
    <div className="flex flex-col items-center gap-2">
      <div className={`text-4xl font-bold font-mono ${getTimerColor()}`}>
        {formatTime(remainingSeconds)}
      </div>
      
      {/* Progress bar */}
      <div className="w-full bg-gray-200 rounded-full h-2 overflow-hidden">
        <div
          className={`h-full transition-all duration-1000 ${
            remainingSeconds > 300
              ? 'bg-green-500'
              : remainingSeconds > 60
              ? 'bg-yellow-500'
              : 'bg-red-500'
          }`}
          style={{ width: `${getProgressPercentage()}%` }}
        />
      </div>
      
      <div className="text-sm text-gray-600">
        {remainingSeconds > 600 && '🔥 Bonus: 50 XP'}
        {remainingSeconds <= 600 && remainingSeconds > 300 && '⚡ Bonus: 30 XP'}
        {remainingSeconds <= 300 && remainingSeconds > 0 && '💨 Bonus: 10 XP'}
        {remainingSeconds === 0 && '⏰ Tempo Esgotado!'}
      </div>
    </div>
  );
};
