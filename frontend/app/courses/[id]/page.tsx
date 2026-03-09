'use client';

import React, { useState, useEffect } from 'react';
import { useRouter, useParams } from 'next/navigation';
import { coursesApi } from '@/lib/api/courses';
import { CourseDetail, LessonDetail } from '@/lib/types';
import { Navigation, CourseDetailSkeleton, Breadcrumb } from '@/lib/components';
import { Icons } from '@/lib/components/Icons';
import { useLessonProgress } from '@/lib/hooks/useLessonProgress';
import Link from 'next/link';

export default function CourseDetailPage() {
  const router = useRouter();
  const params = useParams();
  const courseId = params.id as string;
  const { isLessonComplete, getCourseProgress } = useLessonProgress();

  const [course, setCourse] = useState<CourseDetail | null>(null);
  const [lessons, setLessons] = useState<LessonDetail[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    if (courseId) {
      loadCourseAndLessons();
    }
  }, [courseId]);

  // Reload lessons when progress changes
  useEffect(() => {
    const handleProgressUpdate = () => {
      if (courseId) {
        loadCourseAndLessons();
      }
    };

    window.addEventListener('lessonProgressUpdated', handleProgressUpdate);
    return () => {
      window.removeEventListener('lessonProgressUpdated', handleProgressUpdate);
    };
  }, [courseId]);

  const loadCourseAndLessons = async () => {
    try {
      setLoading(true);
      setError(null);
      
      // Fetch course details and lessons in parallel
      const [courseData, lessonsResponse] = await Promise.all([
        coursesApi.getById(courseId),
        coursesApi.getLessons(courseId)
      ]);
      
      console.log('Course data:', courseData);
      console.log('Raw lessons from API:', lessonsResponse.lessons);
      console.log('Course ID:', courseId);
      
      setCourse(courseData);
      
      // Merge backend data with local progress
      const lessonsWithProgress = lessonsResponse.lessons.map((lesson: LessonDetail) => {
        const isComplete = isLessonComplete(courseId, lesson.id);
        console.log(`Lesson ${lesson.id} (${lesson.title}): isComplete = ${isComplete}`);
        return {
          ...lesson,
          isCompleted: isComplete,
        };
      });
      
      console.log('Lessons with progress:', lessonsWithProgress);
      setLessons(lessonsWithProgress);
    } catch (err) {
      setError('Failed to load course. Please try again later.');
      console.error('Error loading course:', err);
    } finally {
      setLoading(false);
    }
  };

  const calculateProgress = () => {
    if (lessons.length === 0) return 0;
    const completedCount = lessons.filter((lesson) => lesson.isCompleted).length;
    return Math.round((completedCount / lessons.length) * 100);
  };

  const handleLessonClick = (lessonId: string) => {
    router.push(`/courses/${courseId}/lessons/${lessonId}`);
  };

  const handleBackClick = () => {
    router.push('/courses');
  };

  if (loading) {
    return <CourseDetailSkeleton />;
  }

  if (error) {
    return (
      <div className="min-h-screen bg-gray-50 flex items-center justify-center">
        <div className="text-center">
          <p className="text-red-600 mb-4">{error}</p>
          <button
            onClick={loadCourseAndLessons}
            className="px-4 py-2 bg-blue-600 text-white rounded hover:bg-blue-700 mr-2"
          >
            Retry
          </button>
          <button
            onClick={handleBackClick}
            className="px-4 py-2 bg-gray-600 text-white rounded hover:bg-gray-700"
          >
            Back to Courses
          </button>
        </div>
      </div>
    );
  }

  const progress = calculateProgress();
  const completedCount = lessons.filter((l) => l.isCompleted).length;

  // Build breadcrumb items
  const breadcrumbItems = [
    ...(course?.levelId && course?.levelTitle
      ? [
          { label: 'Níveis', href: '/levels' },
          { label: course.levelTitle, href: `/levels/${course.levelId}` },
        ]
      : []),
    { label: course?.title || 'Curso' },
  ];

  return (
    <div className="min-h-screen bg-gray-50">
      <Navigation />
      <div className="max-w-5xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
        {/* Breadcrumb Navigation */}
        {breadcrumbItems.length > 1 && <Breadcrumb items={breadcrumbItems} />}

        {/* Back Button */}
        <button
          onClick={handleBackClick}
          className="mb-6 flex items-center text-gray-600 hover:text-gray-900 transition-colors"
        >
          <span className="mr-2">←</span>
          Voltar aos Cursos
        </button>

        {/* Course Header */}
        <div className="bg-white rounded-lg shadow p-6 mb-6">
          <div className="flex items-start justify-between mb-4">
            <div className="flex-1">
              <h1 className="text-3xl font-bold text-gray-900 mb-2">
                {course?.title || 'Aulas do Curso'}
              </h1>
              {course?.description && (
                <p className="text-gray-600 mb-3">{course.description}</p>
              )}
              <div className="flex flex-wrap gap-3 items-center">
                {/* Level Badge */}
                {course?.levelId && course?.levelTitle && (
                  <Link
                    href={`/levels/${course.levelId}`}
                    className="inline-flex items-center px-3 py-1 rounded-full text-sm font-medium bg-blue-100 text-blue-800 hover:bg-blue-200 transition-colors"
                  >
                    {course.levelTitle}
                  </Link>
                )}
                {/* Course Level */}
                {course?.level && (
                  <span className="inline-flex items-center px-3 py-1 rounded-full text-sm font-medium bg-gray-100 text-gray-800">
                    {course.level}
                  </span>
                )}
                {/* Duration */}
                {course?.duration && (
                  <span className="text-sm text-gray-600 flex items-center">
                    <Icons.Clock className="w-4 h-4 mr-1" />
                    {course.duration}
                  </span>
                )}
                {/* Lesson Count */}
                <span className="text-sm text-gray-600">
                  {lessons.length} {lessons.length === 1 ? 'aula' : 'aulas'}
                </span>
              </div>
            </div>
          </div>

          {/* Progress Bar */}
          <div className="mb-4">
            <div className="flex items-center justify-between mb-2">
              <span className="text-sm font-medium text-gray-700">Progresso do Curso</span>
              <span className="text-sm font-medium text-gray-700">{progress}%</span>
            </div>
            <div className="w-full bg-gray-200 rounded-full h-3">
              <div
                className="bg-blue-600 h-3 rounded-full transition-all duration-300"
                style={{ width: `${progress}%` }}
              ></div>
            </div>
            <p className="text-sm text-gray-600 mt-2">
              {completedCount} de {lessons.length} aulas concluídas
            </p>
          </div>
        </div>

        {/* Lessons List */}
        {lessons.length === 0 ? (
          <div className="bg-white rounded-lg shadow p-8 text-center">
            <p className="text-gray-600">Nenhuma aula disponível para este curso.</p>
          </div>
        ) : (
          <div className="space-y-4">
            {lessons.map((lesson, index) => (
              <div
                key={lesson.id}
                onClick={() => handleLessonClick(lesson.id)}
                className={`bg-white rounded-lg shadow hover:shadow-lg transition-shadow cursor-pointer p-6 ${
                  lesson.isCompleted ? 'border-l-4 border-green-500' : ''
                }`}
              >
                <div className="flex items-start justify-between">
                  <div className="flex items-start gap-4 flex-1">
                    {/* Lesson Number */}
                    <div
                      className={`shrink-0 w-10 h-10 rounded-full flex items-center justify-center font-bold ${
                        lesson.isCompleted
                          ? 'bg-green-100 text-green-600'
                          : 'bg-gray-100 text-gray-600'
                      }`}
                    >
                      {lesson.order}
                    </div>

                    {/* Lesson Info */}
                    <div className="flex-1">
                      <div className="flex items-center gap-2 mb-1">
                        <h3 className="text-lg font-semibold text-gray-900">
                          {lesson.title}
                        </h3>
                        {lesson.isCompleted && (
                          <span className="text-green-600" title="Concluída">
                            <Icons.CheckCircle className="w-5 h-5" />
                          </span>
                        )}
                      </div>
                      <p className="text-sm text-gray-600">
                        Aula {lesson.order} de {lessons.length}
                      </p>
                    </div>
                  </div>

                  {/* Action */}
                  <div className="shrink-0 ml-4">
                    <span className="text-blue-600 hover:text-blue-700 font-medium text-sm">
                      {lesson.isCompleted ? 'Revisar' : 'Começar'} →
                    </span>
                  </div>
                </div>
              </div>
            ))}
          </div>
        )}
      </div>
    </div>
  );
}
