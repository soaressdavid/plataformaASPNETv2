'use client';

import React, { useState, useEffect } from 'react';
import { useRouter, useSearchParams } from 'next/navigation';
import { coursesApi } from '@/lib/api/courses';
import { CourseSummary, Level } from '@/lib/types';
import { Navigation, CoursesSkeleton } from '@/lib/components';
import { Icons } from '@/lib/components/Icons';

export default function CoursesPage() {
  const router = useRouter();
  const searchParams = useSearchParams();
  const [courses, setCourses] = useState<CourseSummary[]>([]);
  const [filteredCourses, setFilteredCourses] = useState<CourseSummary[]>([]);
  const [selectedLevel, setSelectedLevel] = useState<Level | 'All'>('All');
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  // Initialize selected level from URL on mount
  useEffect(() => {
    const levelParam = searchParams.get('level');
    if (levelParam && levelParam !== 'All') {
      // Validate that the level parameter is a valid Level enum value
      if (Object.values(Level).includes(levelParam as Level)) {
        setSelectedLevel(levelParam as Level);
      }
    }
  }, [searchParams]);

  useEffect(() => {
    loadCourses();
  }, []);

  useEffect(() => {
    filterCourses();
  }, [courses, selectedLevel]);

  const loadCourses = async () => {
    try {
      setLoading(true);
      setError(null);
      const response = await coursesApi.getAll();
      setCourses(response.courses || []);
    } catch (err: any) {
      setError('Failed to load courses. Please try again later.');
      console.error('Error loading courses:', err);
      setCourses([]);
    } finally {
      setLoading(false);
    }
  };

  const filterCourses = () => {
    if (selectedLevel === 'All') {
      setFilteredCourses(courses);
    } else {
      setFilteredCourses(
        courses.filter((course) => course.level === selectedLevel)
      );
    }
  };

  const handleLevelChange = (level: Level | 'All') => {
    setSelectedLevel(level);
    
    // Update URL with selected level (persist filter state in URL)
    const params = new URLSearchParams(searchParams.toString());
    if (level === 'All') {
      params.delete('level');
    } else {
      params.set('level', level);
    }
    
    const newUrl = params.toString() ? `/courses?${params.toString()}` : '/courses';
    router.push(newUrl, { scroll: false });
  };

  const getLevelColor = (level: Level) => {
    switch (level) {
      case Level.Beginner:
        return 'text-green-600 bg-green-100';
      case Level.Intermediate:
        return 'text-blue-600 bg-blue-100';
      case Level.Advanced:
        return 'text-purple-600 bg-purple-100';
      default:
        return 'text-gray-600 bg-gray-100';
    }
  };

  const getLevelIcon = (level: Level) => {
    switch (level) {
      case Level.Beginner:
        return <Icons.BookOpen className="w-5 h-5" />;
      case Level.Intermediate:
        return <Icons.Target className="w-5 h-5" />;
      case Level.Advanced:
        return <Icons.Rocket className="w-5 h-5" />;
      default:
        return <Icons.BookOpen className="w-5 h-5" />;
    }
  };

  const handleCourseClick = (courseId: string) => {
    router.push(`/courses/${courseId}`);
  };

  if (loading) {
    return (
      <div className="min-h-screen bg-gray-50">
        <Navigation />
        <CoursesSkeleton />
      </div>
    );
  }

  if (error) {
    return (
      <div className="min-h-screen bg-gray-50 flex items-center justify-center">
        <div className="text-center">
          <p className="text-red-600 mb-4">{error}</p>
          <button
            onClick={loadCourses}
            className="px-4 py-2 bg-blue-600 text-white rounded hover:bg-blue-700"
          >
            Retry
          </button>
        </div>
      </div>
    );
  }

  return (
    <div className="min-h-screen bg-gray-50">
      <Navigation />
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
        {/* Header */}
        <div className="mb-8">
          <h1 className="text-2xl sm:text-3xl font-bold text-gray-900 mb-2">Cursos ASP.NET Core</h1>
          <p className="text-sm sm:text-base text-gray-600">
            Aprenda ASP.NET Core sistematicamente do básico aos tópicos avançados
          </p>
        </div>

        {/* Level Filter */}
        <div className="mb-6">
          <label htmlFor="level-filter" className="text-sm font-medium text-gray-700 mb-2 block">
            Filtrar por nível:
          </label>
          <div className="flex flex-wrap gap-2">
            {['All', Level.Beginner, Level.Intermediate, Level.Advanced].map((level) => (
              <button
                key={level}
                onClick={() => handleLevelChange(level as Level | 'All')}
                className={`px-3 sm:px-4 py-2 rounded-lg text-xs sm:text-sm font-medium transition-colors ${
                  selectedLevel === level
                    ? 'bg-blue-600 text-white shadow-md'
                    : 'bg-white text-gray-700 hover:bg-gray-100 border border-gray-300'
                }`}
                aria-pressed={selectedLevel === level}
              >
                {level}
              </button>
            ))}
          </div>
          {selectedLevel !== 'All' && (
            <p className="text-xs text-gray-500 mt-2">
              Mostrando {filteredCourses.length} curso{filteredCourses.length !== 1 ? 's' : ''} de nível {selectedLevel}
            </p>
          )}
        </div>

        {/* Course Stats */}
        <div className="mb-6 grid grid-cols-1 sm:grid-cols-3 gap-4">
          <div className="bg-white rounded-lg shadow p-4">
            <p className="text-sm text-gray-600">Total de Cursos</p>
            <p className="text-2xl font-bold text-gray-900">{courses.length}</p>
          </div>
          <div className="bg-white rounded-lg shadow p-4">
            <p className="text-sm text-gray-600">Iniciante</p>
            <p className="text-2xl font-bold text-green-600">
              {courses.filter((c) => c.level === Level.Beginner).length}
            </p>
          </div>
          <div className="bg-white rounded-lg shadow p-4">
            <p className="text-sm text-gray-600">Avançado</p>
            <p className="text-2xl font-bold text-purple-600">
              {courses.filter((c) => c.level === Level.Advanced).length}
            </p>
          </div>
        </div>

        {/* Course List */}
        {filteredCourses.length === 0 ? (
          <div className="bg-white rounded-lg shadow p-8 text-center">
            <p className="text-gray-600">Nenhum curso encontrado para o nível selecionado.</p>
          </div>
        ) : (
          <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
            {filteredCourses.map((course) => (
              <div
                key={course.id}
                onClick={() => handleCourseClick(course.id)}
                className="bg-white rounded-lg shadow hover:shadow-lg transition-shadow cursor-pointer overflow-hidden"
              >
                <div className="p-6">
                  <div className="flex items-start justify-between mb-3">
                    <div className="flex items-center gap-2">
                      <span className="text-blue-600">{getLevelIcon(course.level)}</span>
                      <span
                        className={`px-3 py-1 rounded-full text-xs font-medium ${getLevelColor(
                          course.level
                        )}`}
                      >
                        {course.level}
                      </span>
                    </div>
                  </div>

                  <h3 className="text-lg font-semibold text-gray-900 mb-2">
                    {course.title}
                  </h3>

                  <p className="text-sm text-gray-600 mb-4 line-clamp-2">
                    {course.description}
                  </p>

                  <div className="flex items-center justify-between text-sm">
                    <span className="text-gray-600">
                      {course.lessonCount} {course.lessonCount === 1 ? 'aula' : 'aulas'}
                    </span>
                    <span className="text-blue-600 hover:text-blue-700 font-medium">
                      Ver Curso →
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
