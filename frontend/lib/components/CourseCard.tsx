import Link from 'next/link';
import { CourseSummary } from '../types';
import { Icons } from './Icons';

export interface CourseCardProps {
  course: CourseSummary;
}

export function CourseCard({ course }: CourseCardProps) {
  // Map level enum to display text and color
  const getLevelBadgeClasses = (level: string) => {
    switch (level) {
      case 'Beginner':
        return 'bg-green-100 text-green-700 border-green-200';
      case 'Intermediate':
        return 'bg-yellow-100 text-yellow-700 border-yellow-200';
      case 'Advanced':
        return 'bg-red-100 text-red-700 border-red-200';
      default:
        return 'bg-gray-100 text-gray-700 border-gray-200';
    }
  };

  const getLevelText = (level: string) => {
    switch (level) {
      case 'Beginner':
        return 'Iniciante';
      case 'Intermediate':
        return 'Intermediário';
      case 'Advanced':
        return 'Avançado';
      default:
        return level;
    }
  };

  return (
    <Link
      href={`/courses/${course.id}`}
      className="block rounded-lg border-2 border-blue-200 bg-white hover:border-blue-400 hover:shadow-lg transition-all duration-200 p-6 cursor-pointer"
    >
      {/* Header with level badge */}
      <div className="flex items-start justify-between mb-3">
        <h3 className="text-xl font-semibold text-gray-900 flex-1 pr-2">
          {course.title}
        </h3>
        <span
          className={`inline-flex items-center px-2.5 py-0.5 rounded-full text-xs font-medium border ${getLevelBadgeClasses(
            course.level
          )}`}
        >
          {getLevelText(course.level)}
        </span>
      </div>

      {/* Description */}
      <p className="text-gray-600 text-sm mb-4 line-clamp-2">
        {course.description}
      </p>

      {/* Course metadata */}
      <div className="flex items-center gap-4 text-sm text-gray-500 mb-4">
        <div className="flex items-center gap-1">
          <Icons.BookOpen className="w-4 h-4" />
          <span>
            {course.lessonCount} {course.lessonCount === 1 ? 'lição' : 'lições'}
          </span>
        </div>
        {course.duration && (
          <div className="flex items-center gap-1">
            <Icons.Clock className="w-4 h-4" />
            <span>{course.duration}</span>
          </div>
        )}
      </div>

      {/* Topics as tags */}
      {course.topics && course.topics.length > 0 && (
        <div className="flex flex-wrap gap-2">
          {course.topics.slice(0, 3).map((topic, index) => (
            <span
              key={index}
              className="inline-flex items-center px-2 py-1 rounded-md bg-blue-50 text-blue-700 text-xs font-medium"
            >
              {topic}
            </span>
          ))}
          {course.topics.length > 3 && (
            <span className="inline-flex items-center px-2 py-1 rounded-md bg-gray-50 text-gray-600 text-xs font-medium">
              +{course.topics.length - 3} mais
            </span>
          )}
        </div>
      )}
    </Link>
  );
}
