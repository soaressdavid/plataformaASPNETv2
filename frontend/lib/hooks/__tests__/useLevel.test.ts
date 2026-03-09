import { renderHook, waitFor } from '@testing-library/react';
import { useLevels, useLevel } from '../useLevel';
import { levelsApi } from '../../api';
import { CurriculumLevel, CurriculumLevelDetail, Level } from '../../types';

// Mock the API
jest.mock('../../api', () => ({
  levelsApi: {
    getAll: jest.fn(),
    getById: jest.fn(),
  },
}));

describe('useLevels', () => {
  beforeEach(() => {
    jest.clearAllMocks();
  });

  it('should fetch all levels successfully', async () => {
    const mockLevels: CurriculumLevel[] = [
      {
        id: '1',
        number: 0,
        title: 'Level 0',
        description: 'Beginner level',
        requiredXP: 0,
        courseCount: 5,
        estimatedHours: 10,
      },
      {
        id: '2',
        number: 1,
        title: 'Level 1',
        description: 'Intermediate level',
        requiredXP: 100,
        courseCount: 4,
        estimatedHours: 12,
      },
    ];

    (levelsApi.getAll as jest.Mock).mockResolvedValue({ levels: mockLevels });

    const { result } = renderHook(() => useLevels());

    // Initially loading
    expect(result.current.loading).toBe(true);
    expect(result.current.levels).toEqual([]);
    expect(result.current.error).toBeNull();

    // Wait for the hook to finish loading
    await waitFor(() => {
      expect(result.current.loading).toBe(false);
    });

    // Check final state
    expect(result.current.levels).toEqual(mockLevels);
    expect(result.current.error).toBeNull();
    expect(levelsApi.getAll).toHaveBeenCalledTimes(1);
  });

  it('should handle error when fetching levels fails', async () => {
    const errorMessage = 'Network error';
    (levelsApi.getAll as jest.Mock).mockRejectedValue(new Error(errorMessage));

    const { result } = renderHook(() => useLevels());

    // Wait for the hook to finish loading
    await waitFor(() => {
      expect(result.current.loading).toBe(false);
    });

    // Check error state
    expect(result.current.levels).toEqual([]);
    expect(result.current.error).toBe(errorMessage);
    expect(levelsApi.getAll).toHaveBeenCalledTimes(1);
  });

  it('should handle non-Error exceptions', async () => {
    (levelsApi.getAll as jest.Mock).mockRejectedValue('String error');

    const { result } = renderHook(() => useLevels());

    await waitFor(() => {
      expect(result.current.loading).toBe(false);
    });

    expect(result.current.error).toBe('Failed to load levels');
  });
});

describe('useLevel', () => {
  beforeEach(() => {
    jest.clearAllMocks();
  });

  it('should fetch a single level successfully', async () => {
    const mockLevel: CurriculumLevelDetail = {
      id: '1',
      number: 0,
      title: 'Level 0',
      description: 'Beginner level',
      requiredXP: 0,
      courseCount: 2,
      estimatedHours: 10,
      courses: [
        {
          id: 'c1',
          title: 'Course 1',
          description: 'First course',
          level: Level.Beginner,
          lessonCount: 5,
          orderIndex: 0,
        },
        {
          id: 'c2',
          title: 'Course 2',
          description: 'Second course',
          level: Level.Beginner,
          lessonCount: 4,
          orderIndex: 1,
        },
      ],
    };

    (levelsApi.getById as jest.Mock).mockResolvedValue(mockLevel);

    const { result } = renderHook(() => useLevel('1'));

    // Initially loading
    expect(result.current.loading).toBe(true);
    expect(result.current.level).toBeNull();
    expect(result.current.error).toBeNull();

    // Wait for the hook to finish loading
    await waitFor(() => {
      expect(result.current.loading).toBe(false);
    });

    // Check final state
    expect(result.current.level).toEqual(mockLevel);
    expect(result.current.error).toBeNull();
    expect(levelsApi.getById).toHaveBeenCalledWith('1');
    expect(levelsApi.getById).toHaveBeenCalledTimes(1);
  });

  it('should handle error when fetching level fails', async () => {
    const errorMessage = 'Level not found';
    (levelsApi.getById as jest.Mock).mockRejectedValue(new Error(errorMessage));

    const { result } = renderHook(() => useLevel('999'));

    await waitFor(() => {
      expect(result.current.loading).toBe(false);
    });

    expect(result.current.level).toBeNull();
    expect(result.current.error).toBe(errorMessage);
    expect(levelsApi.getById).toHaveBeenCalledWith('999');
  });

  it('should refetch when id changes', async () => {
    const mockLevel1: CurriculumLevelDetail = {
      id: '1',
      number: 0,
      title: 'Level 0',
      description: 'Beginner level',
      requiredXP: 0,
      courseCount: 2,
      estimatedHours: 10,
      courses: [],
    };

    const mockLevel2: CurriculumLevelDetail = {
      id: '2',
      number: 1,
      title: 'Level 1',
      description: 'Intermediate level',
      requiredXP: 100,
      courseCount: 3,
      estimatedHours: 15,
      courses: [],
    };

    (levelsApi.getById as jest.Mock)
      .mockResolvedValueOnce(mockLevel1)
      .mockResolvedValueOnce(mockLevel2);

    const { result, rerender } = renderHook(({ id }) => useLevel(id), {
      initialProps: { id: '1' },
    });

    await waitFor(() => {
      expect(result.current.loading).toBe(false);
    });

    expect(result.current.level).toEqual(mockLevel1);

    // Change the id
    rerender({ id: '2' });

    // Should be loading again
    expect(result.current.loading).toBe(true);

    await waitFor(() => {
      expect(result.current.loading).toBe(false);
    });

    expect(result.current.level).toEqual(mockLevel2);
    expect(levelsApi.getById).toHaveBeenCalledTimes(2);
    expect(levelsApi.getById).toHaveBeenNthCalledWith(1, '1');
    expect(levelsApi.getById).toHaveBeenNthCalledWith(2, '2');
  });

  it('should not fetch if id is empty', () => {
    const { result } = renderHook(() => useLevel(''));

    expect(result.current.loading).toBe(true);
    expect(result.current.level).toBeNull();
    expect(levelsApi.getById).not.toHaveBeenCalled();
  });

  it('should handle non-Error exceptions', async () => {
    (levelsApi.getById as jest.Mock).mockRejectedValue('String error');

    const { result } = renderHook(() => useLevel('1'));

    await waitFor(() => {
      expect(result.current.loading).toBe(false);
    });

    expect(result.current.error).toBe('Failed to load level');
  });
});
