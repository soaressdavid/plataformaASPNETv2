import { Icons } from './Icons';

export interface LessonObjectivesProps {
  objectives: string[];
}

/**
 * Component to render lesson learning objectives as a list with check icons.
 * 
 * @param objectives - Array of learning objective strings
 */
export function LessonObjectives({ objectives }: LessonObjectivesProps) {
  if (!objectives || objectives.length === 0) {
    return null;
  }

  return (
    <section className="objectives-section mb-8 bg-gradient-to-br from-green-50 to-emerald-50 rounded-xl p-6 border-2 border-green-200">
      <div className="flex items-center gap-3 mb-5">
        <div className="w-10 h-10 bg-gradient-to-br from-green-500 to-emerald-600 rounded-lg flex items-center justify-center">
          <Icons.CheckCircle className="w-6 h-6 text-white" />
        </div>
        <h2 className="text-2xl font-bold bg-gradient-to-r from-green-600 to-emerald-600 bg-clip-text text-transparent">
          Objetivos de Aprendizagem
        </h2>
      </div>
      <ul className="objectives-list space-y-4" role="list">
        {objectives.map((objective, index) => (
          <li 
            key={index} 
            className="objective-item flex items-start gap-4 bg-white rounded-lg p-4 shadow-sm hover:shadow-md transition-shadow"
          >
            <div className="w-8 h-8 bg-gradient-to-br from-green-500 to-emerald-600 rounded-full flex items-center justify-center shrink-0">
              <Icons.CheckCircle className="w-5 h-5 text-white" />
            </div>
            <span className="flex-1 text-gray-700 leading-relaxed text-base pt-1">{objective}</span>
          </li>
        ))}
      </ul>
    </section>
  );
}
