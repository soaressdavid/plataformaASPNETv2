import { useEffect, useState } from 'react';

interface LessonProgress {
  [courseId: string]: {
    [lessonId: string]: boolean;
  };
}

const STORAGE_KEY = 'lesson_progress';

// Helper function to read directly from localStorage
const getProgressFromStorage = (): LessonProgress => {
  if (typeof window === 'undefined') return {};
  
  const stored = localStorage.getItem(STORAGE_KEY);
  if (!stored) return {};
  
  try {
    return JSON.parse(stored);
  } catch (error) {
    console.error('Failed to parse lesson progress:', error);
    return {};
  }
};

export function useLessonProgress() {
  const [, forceUpdate] = useState(0);

  const markLessonComplete = (courseId: string, lessonId: string) => {
    console.log('Marking lesson complete:', { courseId, lessonId });
    
    const current = getProgressFromStorage();
    const updated = {
      ...current,
      [courseId]: {
        ...(current[courseId] || {}),
        [lessonId]: true,
      },
    };
    
    console.log('Updated progress:', updated);
    
    if (typeof window !== 'undefined') {
      localStorage.setItem(STORAGE_KEY, JSON.stringify(updated));
      console.log('Saved to localStorage:', localStorage.getItem(STORAGE_KEY));
      
      // Dispatch custom event to notify other components
      window.dispatchEvent(new Event('lessonProgressUpdated'));
      console.log('Dispatched lessonProgressUpdated event');
    }
    
    forceUpdate(prev => prev + 1);
  };

  const isLessonComplete = (courseId: string, lessonId: string): boolean => {
    const progress = getProgressFromStorage();
    const isComplete = progress[courseId]?.[lessonId] || false;
    console.log(`isLessonComplete(${courseId}, ${lessonId}):`, isComplete);
    return isComplete;
  };

  const getCourseProgress = (courseId: string, totalLessons: number): number => {
    const progress = getProgressFromStorage();
    const completedLessons = Object.values(progress[courseId] || {}).filter(Boolean).length;
    return totalLessons > 0 ? Math.round((completedLessons / totalLessons) * 100) : 0;
  };

  const getCompletedCount = (courseId: string): number => {
    const progress = getProgressFromStorage();
    return Object.values(progress[courseId] || {}).filter(Boolean).length;
  };

  // Listen for updates
  useEffect(() => {
    const handleProgressUpdate = () => {
      console.log('lessonProgressUpdated event received, forcing re-render...');
      forceUpdate(prev => prev + 1);
    };

    window.addEventListener('lessonProgressUpdated', handleProgressUpdate);
    return () => {
      window.removeEventListener('lessonProgressUpdated', handleProgressUpdate);
    };
  }, []);

  return {
    markLessonComplete,
    isLessonComplete,
    getCourseProgress,
    getCompletedCount,
  };
}
