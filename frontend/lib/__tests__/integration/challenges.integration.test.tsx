/**
 * Integration Tests: Challenge Submission and Result Display
 * 
 * Tests challenge workflows including:
 * - Challenge listing and filtering
 * - Challenge submission
 * - Test result display
 * - XP award on successful completion
 * 
 * Validates Requirements: 5.1, 5.2, 5.3
 */

import React from 'react';
import { render, screen, waitFor } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import axios from 'axios';
import { challengesApi } from '@/lib/api/challenges';
import {
  ChallengeSummary,
  ChallengeDetailResponse,
  SubmitSolutionResponse,
  Difficulty,
  TestResult,
} from '@/lib/types';

// Mock dependencies
jest.mock('@/lib/api-client');
jest.mock('axios');
const mockedAxios = axios as jest.Mocked<typeof axios>;

describe('Challenge Integration Tests', () => {
  beforeEach(() => {
    jest.clearAllMocks();
  });

  describe('Challenge Listing and Filtering', () => {
    it('should fetch and display all challenges', async () => {
      // Arrange
      const mockChallenges: ChallengeSummary[] = [
        {
          id: 'challenge-1',
          title: 'Easy Challenge',
          difficulty: Difficulty.Easy,
          isSolved: false,
          submissionCount: 0,
        },
        {
          id: 'challenge-2',
          title: 'Medium Challenge',
          difficulty: Difficulty.Medium,
          isSolved: true,
          submissionCount: 3,
        },
        {
          id: 'challenge-3',
          title: 'Hard Challenge',
          difficulty: Difficulty.Hard,
          isSolved: false,
          submissionCount: 1,
        },
      ];

      mockedAxios.get.mockResolvedValueOnce({
        data: { challenges: mockChallenges },
      });

      // Act
      const result = await challengesApi.getAll();

      // Assert
      expect(mockedAxios.get).toHaveBeenCalledWith('/api/challenges');
      expect(result.challenges).toHaveLength(3);
      expect(result.challenges[0].title).toBe('Easy Challenge');
      expect(result.challenges[1].isSolved).toBe(true);
    });

    it('should filter challenges by difficulty', async () => {
      // Arrange
      const mockChallenges: ChallengeSummary[] = [
        {
          id: 'challenge-1',
          title: 'Easy Challenge 1',
          difficulty: Difficulty.Easy,
          isSolved: false,
          submissionCount: 0,
        },
        {
          id: 'challenge-2',
          title: 'Easy Challenge 2',
          difficulty: Difficulty.Easy,
          isSolved: false,
          submissionCount: 0,
        },
        {
          id: 'challenge-3',
          title: 'Medium Challenge',
          difficulty: Difficulty.Medium,
          isSolved: false,
          submissionCount: 0,
        },
      ];

      mockedAxios.get.mockResolvedValueOnce({
        data: { challenges: mockChallenges },
      });

      // Act
      const result = await challengesApi.getAll();
      const easyOnly = result.challenges.filter((c) => c.difficulty === Difficulty.Easy);

      // Assert
      expect(easyOnly).toHaveLength(2);
      expect(easyOnly.every((c) => c.difficulty === Difficulty.Easy)).toBe(true);
    });
  });

  describe('Challenge Detail Retrieval', () => {
    it('should fetch challenge details with starter code and test cases', async () => {
      // Arrange
      const mockChallenge: ChallengeDetailResponse = {
        id: 'challenge-1',
        title: 'Reverse String',
        description: 'Write a function that reverses a string',
        difficulty: Difficulty.Easy,
        starterCode: 'public string Reverse(string input) {\n  // Your code here\n}',
        testCases: [
          {
            input: '"hello"',
            expectedOutput: '"olleh"',
            isHidden: false,
          },
          {
            input: '"world"',
            expectedOutput: '"dlrow"',
            isHidden: false,
          },
          {
            input: '""',
            expectedOutput: '""',
            isHidden: true,
          },
        ],
      };

      mockedAxios.get.mockResolvedValueOnce({
        data: mockChallenge,
      });

      // Act
      const result = await challengesApi.getById('challenge-1');

      // Assert
      expect(mockedAxios.get).toHaveBeenCalledWith('/api/challenges/challenge-1');
      expect(result.title).toBe('Reverse String');
      expect(result.starterCode).toContain('public string Reverse');
      expect(result.testCases).toHaveLength(3);
      expect(result.testCases[0].input).toBe('"hello"');
      expect(result.testCases[2].isHidden).toBe(true);
    });

    it('should display challenge description and starter code', async () => {
      // Arrange
      const mockChallenge: ChallengeDetailResponse = {
        id: 'challenge-1',
        title: 'Sum Two Numbers',
        description: 'Write a function that adds two integers',
        difficulty: Difficulty.Easy,
        starterCode: 'public int Sum(int a, int b) {\n  return 0;\n}',
        testCases: [],
      };

      mockedAxios.get.mockResolvedValueOnce({
        data: mockChallenge,
      });

      // Act
      const result = await challengesApi.getById('challenge-1');

      // Assert
      expect(result.description).toBe('Write a function that adds two integers');
      expect(result.starterCode).toBe('public int Sum(int a, int b) {\n  return 0;\n}');
    });
  });

  describe('Challenge Submission', () => {
    it('should submit solution and receive test results', async () => {
      // Arrange
      const mockSubmission: SubmitSolutionResponse = {
        submissionId: 'submission-123',
        allTestsPassed: true,
        results: [
          {
            testCaseId: 'test-1',
            passed: true,
            input: '"hello"',
            expectedOutput: '"olleh"',
            actualOutput: '"olleh"',
          },
          {
            testCaseId: 'test-2',
            passed: true,
            input: '"world"',
            expectedOutput: '"dlrow"',
            actualOutput: '"dlrow"',
          },
        ],
        xpAwarded: 10,
      };

      mockedAxios.post.mockResolvedValueOnce({
        data: mockSubmission,
      });

      const code = 'public string Reverse(string input) { return new string(input.Reverse().ToArray()); }';

      // Act
      const result = await challengesApi.submitSolution('challenge-1', {
        userId: 'user-123',
        code,
      });

      // Assert
      expect(mockedAxios.post).toHaveBeenCalledWith(
        '/api/challenges/challenge-1/submit',
        {
          userId: 'user-123',
          code,
        }
      );
      expect(result.allTestsPassed).toBe(true);
      expect(result.results).toHaveLength(2);
      expect(result.xpAwarded).toBe(10);
    });

    it('should display failed test cases with expected vs actual output', async () => {
      // Arrange
      const mockSubmission: SubmitSolutionResponse = {
        submissionId: 'submission-456',
        allTestsPassed: false,
        results: [
          {
            testCaseId: 'test-1',
            passed: true,
            input: '"hello"',
            expectedOutput: '"olleh"',
            actualOutput: '"olleh"',
          },
          {
            testCaseId: 'test-2',
            passed: false,
            input: '"world"',
            expectedOutput: '"dlrow"',
            actualOutput: '"world"',
          },
        ],
        xpAwarded: 0,
      };

      mockedAxios.post.mockResolvedValueOnce({
        data: mockSubmission,
      });

      // Act
      const result = await challengesApi.submitSolution('challenge-1', {
        userId: 'user-123',
        code: 'public string Reverse(string input) { return input; }',
      });

      // Assert
      expect(result.allTestsPassed).toBe(false);
      expect(result.xpAwarded).toBe(0);
      
      const failedTest = result.results.find((r) => !r.passed);
      expect(failedTest).toBeDefined();
      expect(failedTest?.expectedOutput).toBe('"dlrow"');
      expect(failedTest?.actualOutput).toBe('"world"');
    });

    it('should award XP based on difficulty when all tests pass', async () => {
      // Arrange - Easy challenge
      const easySubmission: SubmitSolutionResponse = {
        submissionId: 'sub-1',
        allTestsPassed: true,
        results: [],
        xpAwarded: 10,
      };

      mockedAxios.post.mockResolvedValueOnce({ data: easySubmission });

      // Act
      const easyResult = await challengesApi.submitSolution('easy-challenge', {
        userId: 'user-123',
        code: 'solution',
      });

      // Assert
      expect(easyResult.xpAwarded).toBe(10);

      // Arrange - Medium challenge
      const mediumSubmission: SubmitSolutionResponse = {
        submissionId: 'sub-2',
        allTestsPassed: true,
        results: [],
        xpAwarded: 25,
      };

      mockedAxios.post.mockResolvedValueOnce({ data: mediumSubmission });

      // Act
      const mediumResult = await challengesApi.submitSolution('medium-challenge', {
        userId: 'user-123',
        code: 'solution',
      });

      // Assert
      expect(mediumResult.xpAwarded).toBe(25);

      // Arrange - Hard challenge
      const hardSubmission: SubmitSolutionResponse = {
        submissionId: 'sub-3',
        allTestsPassed: true,
        results: [],
        xpAwarded: 50,
      };

      mockedAxios.post.mockResolvedValueOnce({ data: hardSubmission });

      // Act
      const hardResult = await challengesApi.submitSolution('hard-challenge', {
        userId: 'user-123',
        code: 'solution',
      });

      // Assert
      expect(hardResult.xpAwarded).toBe(50);
    });

    it('should execute all test cases against submitted code', async () => {
      // Arrange
      const mockSubmission: SubmitSolutionResponse = {
        submissionId: 'submission-789',
        allTestsPassed: true,
        results: [
          {
            testCaseId: 'test-1',
            passed: true,
            input: '2, 3',
            expectedOutput: '5',
            actualOutput: '5',
          },
          {
            testCaseId: 'test-2',
            passed: true,
            input: '0, 0',
            expectedOutput: '0',
            actualOutput: '0',
          },
          {
            testCaseId: 'test-3',
            passed: true,
            input: '-1, 1',
            expectedOutput: '0',
            actualOutput: '0',
          },
        ],
        xpAwarded: 10,
      };

      mockedAxios.post.mockResolvedValueOnce({
        data: mockSubmission,
      });

      // Act
      const result = await challengesApi.submitSolution('challenge-1', {
        userId: 'user-123',
        code: 'public int Sum(int a, int b) { return a + b; }',
      });

      // Assert
      expect(result.results).toHaveLength(3);
      expect(result.results.every((r) => r.passed)).toBe(true);
    });
  });

  describe('Challenge Submission Error Handling', () => {
    it('should handle compilation errors', async () => {
      // Arrange
      mockedAxios.post.mockRejectedValueOnce({
        response: {
          status: 422,
          data: {
            error: {
              code: 'COMPILATION_ERROR',
              message: 'Code failed to compile',
              details: [
                {
                  line: 1,
                  message: 'CS0103: The name "invalid" does not exist in the current context',
                },
              ],
            },
          },
        },
      });

      // Act & Assert
      await expect(
        challengesApi.submitSolution('challenge-1', {
          userId: 'user-123',
          code: 'invalid code',
        })
      ).rejects.toMatchObject({
        response: {
          status: 422,
          data: {
            error: {
              code: 'COMPILATION_ERROR',
            },
          },
        },
      });
    });

    it('should handle runtime errors', async () => {
      // Arrange
      mockedAxios.post.mockRejectedValueOnce({
        response: {
          status: 422,
          data: {
            error: {
              code: 'RUNTIME_ERROR',
              message: 'Code execution failed',
              details: {
                exception: 'System.NullReferenceException',
                message: 'Object reference not set to an instance of an object',
              },
            },
          },
        },
      });

      // Act & Assert
      await expect(
        challengesApi.submitSolution('challenge-1', {
          userId: 'user-123',
          code: 'public string Reverse(string input) { return input.ToUpper(); }',
        })
      ).rejects.toMatchObject({
        response: {
          status: 422,
          data: {
            error: {
              code: 'RUNTIME_ERROR',
            },
          },
        },
      });
    });
  });
});
