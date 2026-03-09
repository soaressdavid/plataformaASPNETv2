import { useState, useEffect } from 'react';
import { coursesApi } from '../api';
import { LessonDetail } from '../types';

/**
 * Hook to fetch a lesson and automatically detect its content type
 * @param courseId - The course ID containing the lesson
 * @param lessonId - The lesson ID to fetch
 * @returns Object containing lesson data, loading state, error message, and content type
 */
export function useStructuredLesson(courseId: string, lessonId: string) {
  const [lesson, setLesson] = useState<LessonDetail | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [contentType, setContentType] = useState<'structured' | 'html' | null>(null);

  useEffect(() => {
    const fetchLesson = async () => {
      try {
        setLoading(true);
        setError(null);
        const data = await coursesApi.getLesson(courseId, lessonId);
        setLesson(data);
        
        // Determine content type based on available fields
        // Prefer structured content over HTML content
        if (data.structuredContent) {
          setContentType('structured');
        } else if (data.content) {
          setContentType('html');
        } else {
          setContentType(null);
        }
      } catch (err) {
        setError(err instanceof Error ? err.message : 'Failed to load lesson');
      } finally {
        setLoading(false);
      }
    };

    if (courseId && lessonId) {
      fetchLesson();
    }
  }, [courseId, lessonId]);

  return { lesson, loading, error, contentType };
}
