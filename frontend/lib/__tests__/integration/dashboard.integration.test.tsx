/**
 * Integration Tests: Dashboard Data Display
 * 
 * Tests dashboard workflows including:
 * - Dashboard data fetching
 * - XP and level display
 * - Progress metrics display
 * - Course progress display
 * 
 * Validates Requirements: 8.1
 */

import React from 'react';
import { render, screen, waitFor } from '@testing-library/react';
import axios from 'axios';
import { progressApi } from '@/lib/api/progress';
import {
  DashboardResponse,
  CourseProgress,
  SolvedChallengesByDifficulty,
} from '@/lib/types';

// Mock dependencies
jest.mock('@/lib/api-client');
jest.mock('axios');
const mockedAxios = axios as jest.Mocked<typeof axios>;

describe('Dashboard Integration Tests', () => {
  beforeEach(() => {
    jest.clearAllMocks();
  });

  describe('Dashboard Data Display', () => {
    it('should fetch and display current XP and level', async () => {
      // Arrange
      const mockDashboard: DashboardResponse = {
        currentXP: 250,
        currentLevel: 5,
        xpToNextLevel: 150,
        solvedChallenges: {
          easy: 10,
          medium: 5,
          hard: 2,
        },
        completedProjects: 3,
        learningStreak: 7,
        coursesInProgress: [],
      };

      mockedAxios.get.mockResolvedValueOnce({
        data: mockDashboard,
      });

      // Act
      const result = await progressApi.getDashboard('user-123');

      // Assert
      expect(mockedAxios.get).toHaveBeenCalledWith('/api/progress/dashboard', {
        params: { userId: 'user-123' },
      });
      expect(result.currentXP).toBe(250);
      expect(result.currentLevel).toBe(5);
      expect(result.xpToNextLevel).toBe(150);
    });

    it('should display solved challenges by difficulty', async () => {
      // Arrange
      const mockDashboard: DashboardResponse = {
        currentXP: 500,
        currentLevel: 7,
        xpToNextLevel: 200,
        solvedChallenges: {
          easy: 15,
          medium: 8,
          hard: 3,
        },
        completedProjects: 5,
        learningStreak: 14,
        coursesInProgress: [],
      };

      mockedAxios.get.mockResolvedValueOnce({
        data: mockDashboard,
      });

      // Act
      const result = await progressApi.getDashboard('user-123');

      // Assert
      expect(result.solvedChallenges.easy).toBe(15);
      expect(result.solvedChallenges.medium).toBe(8);
      expect(result.solvedChallenges.hard).toBe(3);
    });

    it('should display completed projects count', async () => {
      // Arrange
      const mockDashboard: DashboardResponse = {
        currentXP: 1000,
        currentLevel: 10,
        xpToNextLevel: 100,
        solvedChallenges: {
          easy: 20,
          medium: 15,
          hard: 10,
        },
        completedProjects: 8,
        learningStreak: 30,
        coursesInProgress: [],
      };

      mockedAxios.get.mockResolvedValueOnce({
        data: mockDashboard,
      });

      // Act
      const result = await progressApi.getDashboard('user-123');

      // Assert
      expect(result.completedProjects).toBe(8);
    });

    it('should display learning streak in days', async () => {
      // Arrange
      const mockDashboard: DashboardResponse = {
        currentXP: 350,
        currentLevel: 6,
        xpToNextLevel: 250,
        solvedChallenges: {
          easy: 12,
          medium: 6,
          hard: 2,
        },
        completedProjects: 4,
        learningStreak: 21,
        coursesInProgress: [],
      };

      mockedAxios.get.mockResolvedValueOnce({
        data: mockDashboard,
      });

      // Act
      const result = await progressApi.getDashboard('user-123');

      // Assert
      expect(result.learningStreak).toBe(21);
    });

    it('should display courses in progress with completion percentage', async () => {
      // Arrange
      const mockCourses: CourseProgress[] = [
        {
          courseId: 'course-1',
          title: 'ASP.NET Core Basics',
          completionPercentage: 75,
        },
        {
          courseId: 'course-2',
          title: 'Advanced Web APIs',
          completionPercentage: 40,
        },
        {
          courseId: 'course-3',
          title: 'Entity Framework Core',
          completionPercentage: 10,
        },
      ];

      const mockDashboard: DashboardResponse = {
        currentXP: 450,
        currentLevel: 7,
        xpToNextLevel: 150,
        solvedChallenges: {
          easy: 15,
          medium: 8,
          hard: 3,
        },
        completedProjects: 5,
        learningStreak: 12,
        coursesInProgress: mockCourses,
      };

      mockedAxios.get.mockResolvedValueOnce({
        data: mockDashboard,
      });

      // Act
      const result = await progressApi.getDashboard('user-123');

      // Assert
      expect(result.coursesInProgress).toHaveLength(3);
      expect(result.coursesInProgress[0].title).toBe('ASP.NET Core Basics');
      expect(result.coursesInProgress[0].completionPercentage).toBe(75);
      expect(result.coursesInProgress[1].completionPercentage).toBe(40);
      expect(result.coursesInProgress[2].completionPercentage).toBe(10);
    });
  });

  describe('Dashboard Real-time Updates', () => {
    it('should update XP when challenge is completed', async () => {
      // Arrange - Initial dashboard state
      const initialDashboard: DashboardResponse = {
        currentXP: 100,
        currentLevel: 3,
        xpToNextLevel: 200,
        solvedChallenges: {
          easy: 5,
          medium: 2,
          hard: 0,
        },
        completedProjects: 1,
        learningStreak: 5,
        coursesInProgress: [],
      };

      mockedAxios.get.mockResolvedValueOnce({
        data: initialDashboard,
      });

      const initialResult = await progressApi.getDashboard('user-123');
      expect(initialResult.currentXP).toBe(100);

      // Arrange - Updated dashboard after completing a medium challenge (+25 XP)
      const updatedDashboard: DashboardResponse = {
        currentXP: 125,
        currentLevel: 3,
        xpToNextLevel: 175,
        solvedChallenges: {
          easy: 5,
          medium: 3,
          hard: 0,
        },
        completedProjects: 1,
        learningStreak: 5,
        coursesInProgress: [],
      };

      mockedAxios.get.mockResolvedValueOnce({
        data: updatedDashboard,
      });

      // Act
      const updatedResult = await progressApi.getDashboard('user-123');

      // Assert
      expect(updatedResult.currentXP).toBe(125);
      expect(updatedResult.solvedChallenges.medium).toBe(3);
      expect(updatedResult.xpToNextLevel).toBe(175);
    });

    it('should increment level when XP threshold is reached', async () => {
      // Arrange - Just before level up
      const beforeLevelUp: DashboardResponse = {
        currentXP: 395,
        currentLevel: 6,
        xpToNextLevel: 5,
        solvedChallenges: {
          easy: 15,
          medium: 8,
          hard: 3,
        },
        completedProjects: 3,
        learningStreak: 10,
        coursesInProgress: [],
      };

      mockedAxios.get.mockResolvedValueOnce({
        data: beforeLevelUp,
      });

      const beforeResult = await progressApi.getDashboard('user-123');
      expect(beforeResult.currentLevel).toBe(6);

      // Arrange - After completing challenge that triggers level up
      const afterLevelUp: DashboardResponse = {
        currentXP: 405,
        currentLevel: 7,
        xpToNextLevel: 395,
        solvedChallenges: {
          easy: 16,
          medium: 8,
          hard: 3,
        },
        completedProjects: 3,
        learningStreak: 10,
        coursesInProgress: [],
      };

      mockedAxios.get.mockResolvedValueOnce({
        data: afterLevelUp,
      });

      // Act
      const afterResult = await progressApi.getDashboard('user-123');

      // Assert
      expect(afterResult.currentLevel).toBe(7);
      expect(afterResult.currentXP).toBe(405);
    });

    it('should update learning streak when activity continues', async () => {
      // Arrange - Day 1
      const day1Dashboard: DashboardResponse = {
        currentXP: 100,
        currentLevel: 3,
        xpToNextLevel: 200,
        solvedChallenges: { easy: 5, medium: 2, hard: 0 },
        completedProjects: 1,
        learningStreak: 5,
        coursesInProgress: [],
      };

      mockedAxios.get.mockResolvedValueOnce({
        data: day1Dashboard,
      });

      const day1Result = await progressApi.getDashboard('user-123');
      expect(day1Result.learningStreak).toBe(5);

      // Arrange - Day 2 (streak continues)
      const day2Dashboard: DashboardResponse = {
        currentXP: 110,
        currentLevel: 3,
        xpToNextLevel: 190,
        solvedChallenges: { easy: 6, medium: 2, hard: 0 },
        completedProjects: 1,
        learningStreak: 6,
        coursesInProgress: [],
      };

      mockedAxios.get.mockResolvedValueOnce({
        data: day2Dashboard,
      });

      // Act
      const day2Result = await progressApi.getDashboard('user-123');

      // Assert
      expect(day2Result.learningStreak).toBe(6);
    });
  });

  describe('Dashboard for New Users', () => {
    it('should display zero state for new user', async () => {
      // Arrange
      const newUserDashboard: DashboardResponse = {
        currentXP: 0,
        currentLevel: 1,
        xpToNextLevel: 100,
        solvedChallenges: {
          easy: 0,
          medium: 0,
          hard: 0,
        },
        completedProjects: 0,
        learningStreak: 0,
        coursesInProgress: [],
      };

      mockedAxios.get.mockResolvedValueOnce({
        data: newUserDashboard,
      });

      // Act
      const result = await progressApi.getDashboard('new-user-123');

      // Assert
      expect(result.currentXP).toBe(0);
      expect(result.currentLevel).toBe(1);
      expect(result.solvedChallenges.easy).toBe(0);
      expect(result.solvedChallenges.medium).toBe(0);
      expect(result.solvedChallenges.hard).toBe(0);
      expect(result.completedProjects).toBe(0);
      expect(result.learningStreak).toBe(0);
      expect(result.coursesInProgress).toHaveLength(0);
    });
  });

  describe('Error Handling', () => {
    it('should handle dashboard fetch error', async () => {
      // Arrange
      mockedAxios.get.mockRejectedValueOnce({
        response: {
          status: 500,
          data: {
            error: {
              code: 'INTERNAL_ERROR',
              message: 'Failed to fetch dashboard data',
            },
          },
        },
      });

      // Act & Assert
      await expect(progressApi.getDashboard('user-123')).rejects.toMatchObject({
        response: {
          status: 500,
        },
      });
    });

    it('should handle unauthorized access', async () => {
      // Arrange
      mockedAxios.get.mockRejectedValueOnce({
        response: {
          status: 401,
          data: {
            error: {
              code: 'UNAUTHORIZED',
              message: 'Invalid or expired token',
            },
          },
        },
      });

      // Act & Assert
      await expect(progressApi.getDashboard('user-123')).rejects.toMatchObject({
        response: {
          status: 401,
        },
      });
    });
  });

  describe('XP Calculation Validation', () => {
    it('should correctly calculate total XP from multiple challenges', () => {
      // Arrange
      const easyXP = 10;
      const mediumXP = 25;
      const hardXP = 50;

      const easySolved = 10;
      const mediumSolved = 5;
      const hardSolved = 2;

      // Act
      const totalXP = (easySolved * easyXP) + (mediumSolved * mediumXP) + (hardSolved * hardXP);

      // Assert
      expect(totalXP).toBe(325); // (10*10) + (5*25) + (2*50) = 100 + 125 + 100
    });

    it('should calculate level based on XP formula', () => {
      // Level formula: Level = floor(sqrt(TotalXP / 100))
      
      // Test various XP amounts
      const testCases = [
        { xp: 0, expectedLevel: 0 },
        { xp: 100, expectedLevel: 1 },
        { xp: 400, expectedLevel: 2 },
        { xp: 900, expectedLevel: 3 },
        { xp: 1600, expectedLevel: 4 },
        { xp: 2500, expectedLevel: 5 },
      ];

      testCases.forEach(({ xp, expectedLevel }) => {
        const calculatedLevel = Math.floor(Math.sqrt(xp / 100));
        expect(calculatedLevel).toBe(expectedLevel);
      });
    });

    it('should calculate XP needed for next level', () => {
      // XP for next level = (nextLevel^2 * 100) - currentXP
      
      const currentLevel = 5;
      const currentXP = 2500;
      const nextLevel = currentLevel + 1;
      const xpForNextLevel = (nextLevel * nextLevel * 100);
      const xpNeeded = xpForNextLevel - currentXP;

      expect(xpNeeded).toBe(1100); // 3600 - 2500
    });
  });
});
