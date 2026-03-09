/**
 * Integration Tests: Course Enrollment and Lesson Completion
 * 
 * Tests course workflows including:
 * - Course listing
 * - Course enrollment
 * - Lesson completion
 * - Progress tracking
 * 
 * Validates Requirements: 7.1, 7.3
 */

import React from 'react';
import { render, screen, waitFor } from '@testing-library/react';
import axios from 'axios';
import { coursesApi } from '@/lib/api/courses';
import {
  CourseSummary,
  LessonDetail,
  Level,
  CompleteLessonResponse,
} from '@/lib/types';

// Mock dependencies
jest.mock('@/lib/api-client');
jest.mock('axios');
const mockedAxios = axios as jest.Mocked<typeof axios>;

describe('Course Integration Tests', () => {
  beforeEach(() => {
    jest.clearAllMocks();
  });

  describe('Course Listing', () => {
    it('should fetch and display all courses', async () => {
      // Arrange
      const mockCourses: CourseSummary[] = [
        {
          id: 'course-1',
          title: 'ASP.NET Core Basics',
          description: 'Learn the fundamentals of ASP.NET Core',
          level: Level.Beginner,
          lessonCount: 10,
        },
        {
          id: 'course-2',
          title: 'Advanced Web APIs',
          description: 'Build scalable REST APIs',
          level: Level.Advanced,
          lessonCount: 15,
        },
      ];

      mockedAxios.get.mockResolvedValueOnce({
        data: { courses: mockCourses },
      });

      // Act
      const result = await coursesApi.getAll();

      // Assert
      expect(mockedAxios.get).toHaveBeenCalledWith('/api/courses');
      expect(result.courses).toHaveLength(2);
      expect(result.courses[0].title).toBe('ASP.NET Core Basics');
      expect(result.courses[0].level).toBe(Level.Beginner);
      expect(result.courses[1].lessonCount).toBe(15);
    });

    it('should organize courses by difficulty level', async () => {
      // Arrange
      const mockCourses: CourseSummary[] = [
        {
          id: 'course-1',
          title: 'Beginner Course',
          description: 'Start here',
          level: Level.Beginner,
          lessonCount: 5,
        },
        {
          id: 'course-2',
          title: 'Intermediate Course',
          description: 'Next step',
          level: Level.Intermediate,
          lessonCount: 8,
        },
        {
          id: 'course-3',
          title: 'Advanced Course',
          description: 'Expert level',
          level: Level.Advanced,
          lessonCount: 12,
        },
      ];

      mockedAxios.get.mockResolvedValueOnce({
        data: { courses: mockCourses },
      });

      // Act
      const result = await coursesApi.getAll();
      const beginnerCourses = result.courses.filter((c) => c.level === Level.Beginner);
      const intermediateCourses = result.courses.filter((c) => c.level === Level.Intermediate);
      const advancedCourses = result.courses.filter((c) => c.level === Level.Advanced);

      // Assert
      expect(beginnerCourses).toHaveLength(1);
      expect(intermediateCourses).toHaveLength(1);
      expect(advancedCourses).toHaveLength(1);
    });
  });

  describe('Course Enrollment and Lesson Access', () => {
    it('should fetch lessons for an enrolled course', async () => {
      // Arrange
      const mockLessons: LessonDetail[] = [
        {
          id: 'lesson-1',
          title: 'Introduction to ASP.NET Core',
          content: 'Welcome to the course...',
          order: 1,
          isCompleted: false,
        },
        {
          id: 'lesson-2',
          title: 'Setting Up Your Environment',
          content: 'Install the .NET SDK...',
          order: 2,
          isCompleted: false,
        },
        {
          id: 'lesson-3',
          title: 'Your First Web API',
          content: 'Create a new project...',
          order: 3,
          isCompleted: false,
        },
      ];

      mockedAxios.get.mockResolvedValueOnce({
        data: { lessons: mockLessons },
      });

      // Act
      const result = await coursesApi.getLessons('course-1');

      // Assert
      expect(mockedAxios.get).toHaveBeenCalledWith('/api/courses/course-1/lessons');
      expect(result.lessons).toHaveLength(3);
      expect(result.lessons[0].order).toBe(1);
      expect(result.lessons[1].order).toBe(2);
      expect(result.lessons[2].order).toBe(3);
    });

    it('should maintain lesson order within course', async () => {
      // Arrange
      const mockLessons: LessonDetail[] = [
        {
          id: 'lesson-3',
          title: 'Lesson 3',
          content: 'Content 3',
          order: 3,
          isCompleted: false,
        },
        {
          id: 'lesson-1',
          title: 'Lesson 1',
          content: 'Content 1',
          order: 1,
          isCompleted: false,
        },
        {
          id: 'lesson-2',
          title: 'Lesson 2',
          content: 'Content 2',
          order: 2,
          isCompleted: false,
        },
      ];

      mockedAxios.get.mockResolvedValueOnce({
        data: { lessons: mockLessons },
      });

      // Act
      const result = await coursesApi.getLessons('course-1');
      const sortedLessons = [...result.lessons].sort((a, b) => a.order - b.order);

      // Assert
      expect(sortedLessons[0].order).toBe(1);
      expect(sortedLessons[1].order).toBe(2);
      expect(sortedLessons[2].order).toBe(3);
      expect(sortedLessons[0].title).toBe('Lesson 1');
    });
  });

  describe('Lesson Completion', () => {
    it('should mark lesson as complete and unlock next lesson', async () => {
      // Arrange
      const mockResponse: CompleteLessonResponse = {
        success: true,
        nextLessonId: 'lesson-2',
      };

      mockedAxios.post.mockResolvedValueOnce({
        data: mockResponse,
      });

      // Act
      const result = await coursesApi.completeLesson('course-1', 'lesson-1', {
        userId: 'user-123',
      });

      // Assert
      expect(mockedAxios.post).toHaveBeenCalledWith(
        '/api/courses/course-1/lessons/lesson-1/complete',
        { userId: 'user-123' }
      );
      expect(result.success).toBe(true);
      expect(result.nextLessonId).toBe('lesson-2');
    });

    it('should track progress through lessons when enrolled', async () => {
      // Arrange
      const mockLessons: LessonDetail[] = [
        {
          id: 'lesson-1',
          title: 'Lesson 1',
          content: 'Content 1',
          order: 1,
          isCompleted: true,
        },
        {
          id: 'lesson-2',
          title: 'Lesson 2',
          content: 'Content 2',
          order: 2,
          isCompleted: true,
        },
        {
          id: 'lesson-3',
          title: 'Lesson 3',
          content: 'Content 3',
          order: 3,
          isCompleted: false,
        },
      ];

      mockedAxios.get.mockResolvedValueOnce({
        data: { lessons: mockLessons },
      });

      // Act
      const result = await coursesApi.getLessons('course-1');
      const completedCount = result.lessons.filter((l) => l.isCompleted).length;
      const totalCount = result.lessons.length;
      const progressPercentage = (completedCount / totalCount) * 100;

      // Assert
      expect(completedCount).toBe(2);
      expect(totalCount).toBe(3);
      expect(progressPercentage).toBeCloseTo(66.67, 1);
    });

    it('should return no next lesson when completing final lesson', async () => {
      // Arrange
      const mockResponse: CompleteLessonResponse = {
        success: true,
        nextLessonId: undefined,
      };

      mockedAxios.post.mockResolvedValueOnce({
        data: mockResponse,
      });

      // Act
      const result = await coursesApi.completeLesson('course-1', 'lesson-final', {
        userId: 'user-123',
      });

      // Assert
      expect(result.success).toBe(true);
      expect(result.nextLessonId).toBeUndefined();
    });
  });

  describe('Course Progress Calculation', () => {
    it('should calculate completion percentage correctly', async () => {
      // Arrange
      const mockLessons: LessonDetail[] = [
        { id: '1', title: 'L1', content: '', order: 1, isCompleted: true },
        { id: '2', title: 'L2', content: '', order: 2, isCompleted: true },
        { id: '3', title: 'L3', content: '', order: 3, isCompleted: true },
        { id: '4', title: 'L4', content: '', order: 4, isCompleted: false },
        { id: '5', title: 'L5', content: '', order: 5, isCompleted: false },
      ];

      mockedAxios.get.mockResolvedValueOnce({
        data: { lessons: mockLessons },
      });

      // Act
      const result = await coursesApi.getLessons('course-1');
      const completed = result.lessons.filter((l) => l.isCompleted).length;
      const total = result.lessons.length;
      const percentage = (completed / total) * 100;

      // Assert
      expect(percentage).toBe(60);
    });

    it('should show 0% for newly enrolled course', async () => {
      // Arrange
      const mockLessons: LessonDetail[] = [
        { id: '1', title: 'L1', content: '', order: 1, isCompleted: false },
        { id: '2', title: 'L2', content: '', order: 2, isCompleted: false },
        { id: '3', title: 'L3', content: '', order: 3, isCompleted: false },
      ];

      mockedAxios.get.mockResolvedValueOnce({
        data: { lessons: mockLessons },
      });

      // Act
      const result = await coursesApi.getLessons('course-1');
      const completed = result.lessons.filter((l) => l.isCompleted).length;
      const total = result.lessons.length;
      const percentage = (completed / total) * 100;

      // Assert
      expect(percentage).toBe(0);
    });

    it('should show 100% for completed course', async () => {
      // Arrange
      const mockLessons: LessonDetail[] = [
        { id: '1', title: 'L1', content: '', order: 1, isCompleted: true },
        { id: '2', title: 'L2', content: '', order: 2, isCompleted: true },
        { id: '3', title: 'L3', content: '', order: 3, isCompleted: true },
      ];

      mockedAxios.get.mockResolvedValueOnce({
        data: { lessons: mockLessons },
      });

      // Act
      const result = await coursesApi.getLessons('course-1');
      const completed = result.lessons.filter((l) => l.isCompleted).length;
      const total = result.lessons.length;
      const percentage = (completed / total) * 100;

      // Assert
      expect(percentage).toBe(100);
    });
  });

  describe('Error Handling', () => {
    it('should handle course not found error', async () => {
      // Arrange
      mockedAxios.get.mockRejectedValueOnce({
        response: {
          status: 404,
          data: {
            error: {
              code: 'NOT_FOUND',
              message: 'Course not found',
            },
          },
        },
      });

      // Act & Assert
      await expect(coursesApi.getLessons('invalid-course-id')).rejects.toMatchObject({
        response: {
          status: 404,
        },
      });
    });

    it('should handle lesson completion failure', async () => {
      // Arrange
      mockedAxios.post.mockRejectedValueOnce({
        response: {
          status: 400,
          data: {
            error: {
              code: 'VALIDATION_ERROR',
              message: 'Invalid lesson completion request',
            },
          },
        },
      });

      // Act & Assert
      await expect(
        coursesApi.completeLesson('course-1', 'lesson-1', { userId: 'user-123' })
      ).rejects.toMatchObject({
        response: {
          status: 400,
        },
      });
    });
  });
});
