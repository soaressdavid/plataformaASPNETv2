export interface LessonSummaryProps {
  summary: string;
}

/**
 * Component to render lesson summary at the end of a lesson.
 * 
 * @param summary - Summary text for the lesson
 */
export function LessonSummary({ summary }: LessonSummaryProps) {
  if (!summary || summary.trim().length === 0) {
    return null;
  }

  return (
    <section className="lesson-summary mt-8 p-8 bg-gradient-to-br from-blue-50 via-indigo-50 to-purple-50 rounded-2xl border-2 border-blue-200 shadow-lg">
      <div className="flex items-center gap-3 mb-5">
        <div className="w-12 h-12 bg-gradient-to-br from-blue-500 to-purple-600 rounded-xl flex items-center justify-center">
          <svg className="w-7 h-7 text-white" fill="none" viewBox="0 0 24 24" stroke="currentColor">
            <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M9 12h6m-6 4h6m2 5H7a2 2 0 01-2-2V5a2 2 0 012-2h5.586a1 1 0 01.707.293l5.414 5.414a1 1 0 01.293.707V19a2 2 0 01-2 2z" />
          </svg>
        </div>
        <h2 className="text-3xl font-bold bg-gradient-to-r from-blue-600 to-purple-600 bg-clip-text text-transparent">
          Resumo da Aula
        </h2>
      </div>
      <div className="bg-white rounded-xl p-6 shadow-sm">
        <p className="leading-relaxed text-lg text-gray-700">
          {summary}
        </p>
      </div>
    </section>
  );
}
