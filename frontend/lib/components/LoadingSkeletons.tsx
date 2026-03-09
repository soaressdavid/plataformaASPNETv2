'use client';

import React from 'react';

/**
 * Loading skeleton for the Dashboard page
 * Shows placeholders for XP/Level, stats cards, and courses in progress
 */
export function DashboardSkeleton() {
  return (
    <div className="px-4 py-6 sm:px-0 space-y-6 animate-pulse">
      {/* XP and Level Section Skeleton */}
      <div className="bg-white dark:bg-gray-800 rounded-lg shadow p-6">
        <div className="flex items-center justify-between mb-4">
          <div>
            <div className="h-8 w-24 bg-gray-200 dark:bg-gray-700 rounded mb-2"></div>
            <div className="h-4 w-16 bg-gray-200 dark:bg-gray-700 rounded"></div>
          </div>
          <div className="text-right">
            <div className="h-4 w-20 bg-gray-200 dark:bg-gray-700 rounded mb-2"></div>
            <div className="h-6 w-16 bg-gray-200 dark:bg-gray-700 rounded"></div>
          </div>
        </div>
        <div className="w-full bg-gray-200 dark:bg-gray-700 rounded-full h-4"></div>
      </div>

      {/* Stats Grid Skeleton */}
      <div className="grid grid-cols-1 md:grid-cols-3 gap-6">
        {[1, 2, 3].map((i) => (
          <div key={i} className="bg-white dark:bg-gray-800 rounded-lg shadow p-6">
            <div className="h-6 w-32 bg-gray-200 dark:bg-gray-700 rounded mb-4"></div>
            <div className="space-y-3">
              <div className="h-4 bg-gray-200 dark:bg-gray-700 rounded"></div>
              <div className="h-4 bg-gray-200 dark:bg-gray-700 rounded"></div>
              <div className="h-4 bg-gray-200 dark:bg-gray-700 rounded"></div>
            </div>
          </div>
        ))}
      </div>

      {/* Courses in Progress Skeleton */}
      <div className="bg-white dark:bg-gray-800 rounded-lg shadow p-6">
        <div className="h-6 w-40 bg-gray-200 dark:bg-gray-700 rounded mb-4"></div>
        <div className="space-y-4">
          {[1, 2].map((i) => (
            <div key={i} className="space-y-2">
              <div className="flex items-center justify-between">
                <div className="h-4 w-48 bg-gray-200 dark:bg-gray-700 rounded"></div>
                <div className="h-4 w-12 bg-gray-200 dark:bg-gray-700 rounded"></div>
              </div>
              <div className="w-full bg-gray-200 dark:bg-gray-700 rounded-full h-2"></div>
            </div>
          ))}
        </div>
      </div>
    </div>
  );
}

/**
 * Loading skeleton for the Challenges page
 * Shows placeholders for challenge cards in a grid layout
 */
export function ChallengesSkeleton() {
  return (
    <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8 animate-pulse">
      {/* Header Skeleton */}
      <div className="mb-8">
        <div className="h-9 w-64 bg-gray-200 rounded mb-2"></div>
        <div className="h-5 w-96 bg-gray-200 rounded"></div>
      </div>

      {/* Filter Skeleton */}
      <div className="mb-6 flex items-center gap-2">
        <div className="h-5 w-32 bg-gray-200 rounded"></div>
        <div className="flex gap-2">
          {[1, 2, 3, 4].map((i) => (
            <div key={i} className="h-10 w-20 bg-gray-200 rounded-lg"></div>
          ))}
        </div>
      </div>

      {/* Stats Skeleton */}
      <div className="mb-6 grid grid-cols-1 sm:grid-cols-4 gap-4">
        {[1, 2, 3, 4].map((i) => (
          <div key={i} className="bg-white rounded-lg shadow p-4">
            <div className="h-4 w-24 bg-gray-200 rounded mb-2"></div>
            <div className="h-8 w-12 bg-gray-200 rounded"></div>
          </div>
        ))}
      </div>

      {/* Challenge Cards Skeleton */}
      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
        {[1, 2, 3, 4, 5, 6].map((i) => (
          <div key={i} className="bg-white rounded-lg shadow p-6">
            <div className="flex items-start justify-between mb-3">
              <div className="h-6 w-48 bg-gray-200 rounded"></div>
            </div>
            <div className="h-6 w-20 bg-gray-200 rounded-full mb-4"></div>
            <div className="flex items-center justify-between">
              <div className="h-4 w-24 bg-gray-200 rounded"></div>
              <div className="h-4 w-32 bg-gray-200 rounded"></div>
            </div>
          </div>
        ))}
      </div>
    </div>
  );
}

/**
 * Loading skeleton for the Courses page
 * Shows placeholders for course cards in a grid layout
 */
export function CoursesSkeleton() {
  return (
    <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8 animate-pulse">
      {/* Header Skeleton */}
      <div className="mb-8">
        <div className="h-9 w-72 bg-gray-200 rounded mb-2"></div>
        <div className="h-5 w-96 bg-gray-200 rounded"></div>
      </div>

      {/* Filter Skeleton */}
      <div className="mb-6 flex items-center gap-2">
        <div className="h-5 w-28 bg-gray-200 rounded"></div>
        <div className="flex gap-2">
          {[1, 2, 3, 4].map((i) => (
            <div key={i} className="h-10 w-24 bg-gray-200 rounded-lg"></div>
          ))}
        </div>
      </div>

      {/* Stats Skeleton */}
      <div className="mb-6 grid grid-cols-1 sm:grid-cols-3 gap-4">
        {[1, 2, 3].map((i) => (
          <div key={i} className="bg-white rounded-lg shadow p-4">
            <div className="h-4 w-24 bg-gray-200 rounded mb-2"></div>
            <div className="h-8 w-12 bg-gray-200 rounded"></div>
          </div>
        ))}
      </div>

      {/* Course Cards Skeleton */}
      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
        {[1, 2, 3, 4, 5, 6].map((i) => (
          <div key={i} className="bg-white rounded-lg shadow overflow-hidden">
            <div className="p-6">
              <div className="flex items-start justify-between mb-3">
                <div className="flex items-center gap-2">
                  <div className="h-8 w-8 bg-gray-200 rounded"></div>
                  <div className="h-6 w-20 bg-gray-200 rounded-full"></div>
                </div>
              </div>
              <div className="h-6 w-full bg-gray-200 rounded mb-2"></div>
              <div className="h-4 w-full bg-gray-200 rounded mb-1"></div>
              <div className="h-4 w-3/4 bg-gray-200 rounded mb-4"></div>
              <div className="flex items-center justify-between">
                <div className="h-4 w-20 bg-gray-200 rounded"></div>
                <div className="h-4 w-28 bg-gray-200 rounded"></div>
              </div>
            </div>
          </div>
        ))}
      </div>
    </div>
  );
}

/**
 * Loading indicator for code execution
 * Shows a spinner with "Executing code..." message
 */
export function CodeExecutionLoader() {
  return (
    <div className="flex items-center gap-3 p-4 bg-gray-900 text-gray-100">
      <div className="animate-spin rounded-full h-5 w-5 border-2 border-gray-400 border-t-green-500"></div>
      <span className="text-sm font-medium">Executing code...</span>
    </div>
  );
}

/**
 * Loading indicator for AI feedback
 * Shows a spinner with "Analyzing code..." message
 */
export function AIFeedbackLoader() {
  return (
    <div className="flex items-center gap-3 p-4 bg-blue-50 dark:bg-blue-900/20 border border-blue-200 dark:border-blue-800 rounded-lg">
      <div className="animate-spin rounded-full h-5 w-5 border-2 border-blue-400 border-t-blue-600"></div>
      <span className="text-sm font-medium text-blue-900 dark:text-blue-100">
        Analyzing code with AI...
      </span>
    </div>
  );
}

/**
 * Generic loading spinner component
 * Can be used for any loading state
 */
export function LoadingSpinner({ size = 'md', message }: { size?: 'sm' | 'md' | 'lg'; message?: string }) {
  const sizeClasses = {
    sm: 'h-4 w-4',
    md: 'h-8 w-8',
    lg: 'h-12 w-12',
  };

  return (
    <div className="flex flex-col items-center justify-center gap-3">
      <div
        className={`animate-spin rounded-full border-2 border-gray-300 border-t-blue-600 ${sizeClasses[size]}`}
      ></div>
      {message && <p className="text-sm text-gray-600 dark:text-gray-400">{message}</p>}
    </div>
  );
}

/**
 * Loading skeleton for the Challenge Detail page
 * Shows placeholders for challenge description, test cases, and code editor
 */
export function ChallengeDetailSkeleton() {
  return (
    <div className="h-screen flex flex-col bg-gray-50">
      {/* Header Skeleton */}
      <div className="bg-white border-b border-gray-200 px-6 py-4">
        <div className="flex items-center justify-between animate-pulse">
          <div className="flex items-center gap-4">
            <div className="h-6 w-16 bg-gray-200 rounded"></div>
            <div className="h-8 w-64 bg-gray-200 rounded"></div>
            <div className="h-6 w-20 bg-gray-200 rounded-full"></div>
          </div>
          <div className="h-10 w-40 bg-gray-200 rounded"></div>
        </div>
      </div>

      {/* Main Content Skeleton */}
      <div className="flex-1 flex overflow-hidden">
        {/* Left Panel Skeleton */}
        <div className="w-1/2 border-r border-gray-200 overflow-y-auto bg-white">
          <div className="p-6 animate-pulse space-y-8">
            {/* Description Skeleton */}
            <div>
              <div className="h-6 w-32 bg-gray-200 rounded mb-3"></div>
              <div className="space-y-2">
                <div className="h-4 w-full bg-gray-200 rounded"></div>
                <div className="h-4 w-full bg-gray-200 rounded"></div>
                <div className="h-4 w-3/4 bg-gray-200 rounded"></div>
              </div>
            </div>

            {/* Test Cases Skeleton */}
            <div>
              <div className="h-6 w-28 bg-gray-200 rounded mb-3"></div>
              <div className="space-y-4">
                {[1, 2].map((i) => (
                  <div key={i} className="bg-gray-50 rounded-lg p-4 border border-gray-200">
                    <div className="h-4 w-24 bg-gray-200 rounded mb-2"></div>
                    <div className="space-y-2">
                      <div className="h-16 bg-white rounded border border-gray-200"></div>
                      <div className="h-16 bg-white rounded border border-gray-200"></div>
                    </div>
                  </div>
                ))}
              </div>
            </div>
          </div>
        </div>

        {/* Right Panel Skeleton - Code Editor */}
        <div className="w-1/2 flex flex-col bg-gray-900">
          <div className="flex items-center bg-gray-800 border-b border-gray-700 px-4 py-2 animate-pulse">
            <div className="h-6 w-32 bg-gray-700 rounded"></div>
            <div className="ml-auto h-8 w-20 bg-gray-700 rounded"></div>
          </div>
          <div className="flex-1 bg-gray-900"></div>
        </div>
      </div>
    </div>
  );
}

/**
 * Loading skeleton for the Course Detail page
 * Shows placeholders for course info and lesson list
 */
export function CourseDetailSkeleton() {
  return (
    <div className="min-h-screen bg-gray-50">
      <div className="max-w-5xl mx-auto px-4 sm:px-6 lg:px-8 py-8 animate-pulse">
        {/* Back Button Skeleton */}
        <div className="h-6 w-32 bg-gray-200 rounded mb-6"></div>

        {/* Course Header Skeleton */}
        <div className="bg-white rounded-lg shadow p-6 mb-6">
          <div className="h-8 w-64 bg-gray-200 rounded mb-4"></div>
          <div className="flex items-center gap-4 mb-4">
            <div className="h-4 w-32 bg-gray-200 rounded"></div>
            <div className="h-4 w-24 bg-gray-200 rounded"></div>
          </div>
          <div className="w-full bg-gray-200 rounded-full h-3"></div>
        </div>

        {/* Lessons List Skeleton */}
        <div className="space-y-3">
          {[1, 2, 3, 4, 5].map((i) => (
            <div key={i} className="bg-white rounded-lg shadow p-4">
              <div className="flex items-center justify-between">
                <div className="flex items-center gap-3 flex-1">
                  <div className="h-10 w-10 bg-gray-200 rounded-full"></div>
                  <div className="flex-1">
                    <div className="h-5 w-48 bg-gray-200 rounded mb-2"></div>
                    <div className="h-4 w-32 bg-gray-200 rounded"></div>
                  </div>
                </div>
                <div className="h-8 w-8 bg-gray-200 rounded"></div>
              </div>
            </div>
          ))}
        </div>
      </div>
    </div>
  );
}

/**
 * Full page loading skeleton
 * Shows a centered spinner with optional message
 */
export function PageLoader({ message = 'Loading...' }: { message?: string }) {
  return (
    <div className="min-h-screen bg-gray-50 dark:bg-gray-900 flex items-center justify-center">
      <div className="text-center">
        <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-blue-600 mx-auto"></div>
        <p className="mt-4 text-gray-600 dark:text-gray-400">{message}</p>
      </div>
    </div>
  );
}
