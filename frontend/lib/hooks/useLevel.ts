import { useState, useEffect } from 'react';
import { levelsApi } from '../api';
import { CurriculumLevel, CurriculumLevelDetail } from '../types';

/**
 * Hook to fetch all curriculum levels
 * @returns Object containing levels array, loading state, and error message
 */
export function useLevels() {
  const [levels, setLevels] = useState<CurriculumLevel[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const fetchLevels = async () => {
      try {
        setLoading(true);
        setError(null);
        const response = await levelsApi.getAll();
        setLevels(response.levels);
      } catch (err) {
        setError(err instanceof Error ? err.message : 'Failed to load levels');
      } finally {
        setLoading(false);
      }
    };

    fetchLevels();
  }, []);

  return { levels, loading, error };
}

/**
 * Hook to fetch a single curriculum level with its courses
 * @param id - The level ID to fetch
 * @returns Object containing level detail, loading state, and error message
 */
export function useLevel(id: string) {
  const [level, setLevel] = useState<CurriculumLevelDetail | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const fetchLevel = async () => {
      try {
        setLoading(true);
        setError(null);
        const data = await levelsApi.getById(id);
        setLevel(data);
      } catch (err) {
        setError(err instanceof Error ? err.message : 'Failed to load level');
      } finally {
        setLoading(false);
      }
    };

    if (id) {
      fetchLevel();
    }
  }, [id]);

  return { level, loading, error };
}
