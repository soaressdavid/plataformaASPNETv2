'use client';

import { useState, useEffect } from 'react';
import { useRouter, useParams } from 'next/navigation';
import { coursesApi } from '@/lib/api';
import type { LessonDetail, CourseDetail, Exercise } from '@/lib/types';
import { Icons } from '@/lib/components/Icons';
import { CodeEditor, CodeFile, AIFeedback, LessonContent, Breadcrumb } from '@/lib/components';
import { useAuth } from '@/lib/contexts/AuthContext';
import { useLessonProgress } from '@/lib/hooks/useLessonProgress';
import { useAIFeedback } from '@/lib/hooks/useAIFeedback';
import { useStructuredLesson } from '@/lib/hooks/useStructuredLesson';
import toast from 'react-hot-toast';

export default function LessonPage() {
  const router = useRouter();
  const params = useParams();
  const { user } = useAuth();
  const { markLessonComplete, isLessonComplete } = useLessonProgress();
  const courseId = params.id as string;
  const lessonId = params.lessonId as string;

  // Use the new useStructuredLesson hook
  const { lesson: structuredLesson, loading: lessonLoading, error: lessonError, contentType } = useStructuredLesson(courseId, lessonId);
  
  const [lesson, setLesson] = useState<LessonDetail | null>(null);
  const [course, setCourse] = useState<CourseDetail | null>(null);
  const [allLessons, setAllLessons] = useState<LessonDetail[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [completing, setCompleting] = useState(false);
  const [files, setFiles] = useState<CodeFile[]>([
    {
      name: 'Program.cs',
      content: '// Escreva seu código aqui\nusing System;\n\nclass Program\n{\n    static void Main()\n    {\n        Console.WriteLine("Olá, Mundo!");\n    }\n}',
      language: 'csharp',
    },
  ]);
  const [activeFileIndex, setActiveFileIndex] = useState(0);
  const [output, setOutput] = useState('');
  const [isRunning, setIsRunning] = useState(false);
  const [showAIFeedback, setShowAIFeedback] = useState(false);
  const { feedback, isLoading: isLoadingFeedback, getFeedback, clearFeedback } = useAIFeedback();

  useEffect(() => {
    if (courseId && lessonId) {
      loadData();
    }
  }, [courseId, lessonId]);

  useEffect(() => {
    // Update lesson state when structuredLesson is loaded
    if (structuredLesson) {
      const isComplete = isLessonComplete(courseId, lessonId);
      setLesson({ ...structuredLesson, isCompleted: isComplete });
    }
  }, [structuredLesson, courseId, lessonId]);

  const loadData = async () => {
    try {
      setLoading(true);
      setError(null);
      
      // Load course details for breadcrumb
      const courseData = await coursesApi.getById(courseId);
      setCourse(courseData);
      
      // Load all lessons for navigation
      const response = await coursesApi.getLessons(courseId);
      setAllLessons(response.lessons);
    } catch (err) {
      setError('Failed to load data');
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  const handleCompleteLesson = async () => {
    try {
      setCompleting(true);
      
      if (!user?.userId) {
        toast.error('Você precisa estar logado para completar aulas');
        return;
      }

      console.log('Completing lesson:', { courseId, lessonId, userId: user.userId });
      
      try {
        const response = await coursesApi.completeLesson(courseId, lessonId, { userId: user.userId });
        console.log('Complete lesson response:', response);
        
        if (response.success) {
          // Mark lesson as complete in local storage
          markLessonComplete(courseId, lessonId);
          
          // Update lesson completion status
          setLesson(prev => prev ? { ...prev, isCompleted: true } : null);
          
          // Show success message
          toast.success('Aula concluída! XP concedido.', {
            duration: 3000,
            style: {
              background: '#161b22',
              color: '#fff',
              border: '1px solid #30363d',
            },
          });
          
          // Navigate to next lesson if available
          if (response.nextLessonId) {
            setTimeout(() => {
              router.push(`/courses/${courseId}/lessons/${response.nextLessonId}`);
            }, 1500);
          }
          // Don't redirect if no next lesson - let user stay on current page
        }
      } catch (apiError: any) {
        // If API fails (503 or other error), simulate success for demo purposes
        console.warn('API not available, simulating success:', apiError);
        
        // Mark lesson as complete in local storage
        markLessonComplete(courseId, lessonId);
        
        // Update lesson completion status locally
        setLesson(prev => prev ? { ...prev, isCompleted: true } : null);
        
        // Show success message
        toast.success('Aula concluída (modo demonstração)', {
          duration: 3000,
          style: {
            background: '#161b22',
            color: '#fff',
            border: '1px solid #30363d',
          },
        });
        
        // Find next lesson
        const currentIndex = allLessons.findIndex(l => l.id === lessonId);
        const nextLesson = currentIndex < allLessons.length - 1 ? allLessons[currentIndex + 1] : null;
        
        if (nextLesson) {
          setTimeout(() => {
            router.push(`/courses/${courseId}/lessons/${nextLesson.id}`);
          }, 1500);
        }
        // Don't redirect if no next lesson - let user stay on current page
      }
    } catch (err: any) {
      console.error('Failed to complete lesson:', err);
      toast.error('Erro inesperado ao completar aula');
    } finally {
      setCompleting(false);
    }
  };

  const handleBackClick = () => {
    router.push(`/courses/${courseId}`);
  };

  const handleRunCode = async (code: string) => {
    // Copy code to the practice editor
    const newFiles = [...files];
    newFiles[activeFileIndex].content = code;
    setFiles(newFiles);
    
    // Show toast notification
    toast.success('Código copiado para o editor!', {
      duration: 2000,
      style: {
        background: '#161b22',
        color: '#fff',
        border: '1px solid #30363d',
      },
    });
    
    // Execute the code
    setIsRunning(true);
    setOutput('Compilando e executando código...\n');
    
    try {
      // Call the actual execution service
      const response = await fetch('http://localhost:5006/api/code/execute', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({
          code: code,
          language: 'csharp',
          timeoutSeconds: 10
        }),
      });
      
      console.log('Response status:', response.status);
      console.log('Response headers:', response.headers);
      
      // Check if response is ok
      if (!response.ok) {
        const errorText = await response.text();
        console.error('Error response:', errorText);
        setOutput(`Erro HTTP ${response.status}:\n${errorText || 'Erro desconhecido'}`);
        return;
      }
      
      // Try to parse JSON
      const contentType = response.headers.get('content-type');
      if (!contentType || !contentType.includes('application/json')) {
        const text = await response.text();
        console.error('Non-JSON response:', text);
        setOutput(`Erro: Resposta não é JSON\n${text}`);
        return;
      }
      
      const result = await response.json();
      console.log('Execution result:', result);
      
      if (result.success) {
        setOutput(result.output || 'Execução concluída sem output.');
      } else {
        setOutput(`Erro:\n${result.error || 'Erro desconhecido'}`);
      }
    } catch (error: any) {
      console.error('Execution error:', error);
      setOutput(`Erro ao executar código:\n${error.message || 'Erro de conexão com o serviço de execução'}`);
    } finally {
      setIsRunning(false);
    }
  };

  const handleStartExercise = (exercise: Exercise) => {
    // Set the starter code in the editor
    if (exercise.starterCode) {
      const newFiles = [...files];
      newFiles[activeFileIndex].content = exercise.starterCode;
      setFiles(newFiles);
      toast.success(`Exercício "${exercise.title}" carregado no editor!`);
    }
  };

  if (loading || lessonLoading) {
    return (
      <div className="min-h-screen bg-gradient-to-br from-blue-50 via-white to-purple-50">
        {/* Header Skeleton */}
        <div className="bg-white/80 backdrop-blur-sm border-b border-gray-200 sticky top-0 z-10 shadow-sm">
          <div className="px-6 py-4">
            <div className="h-5 w-32 bg-gray-200 rounded animate-pulse"></div>
          </div>
        </div>

        {/* Content Skeleton */}
        <div className="max-w-[1800px] mx-auto px-6 py-12">
          {/* Breadcrumb Skeleton */}
          <div className="mb-6 animate-pulse">
            <div className="h-4 w-64 bg-gray-200 rounded"></div>
          </div>

          {/* Lesson Header Skeleton */}
          <header className="mb-8">
            <div className="flex items-center gap-3 mb-6 flex-wrap animate-pulse">
              <div className="h-8 w-24 bg-gray-200 rounded-full"></div>
              <div className="h-8 w-20 bg-gray-200 rounded-full"></div>
              <div className="h-8 w-28 bg-gray-200 rounded-full"></div>
            </div>
            <div className="animate-pulse">
              <div className="h-12 w-3/4 bg-gray-200 rounded mb-6"></div>
              <div className="h-1 w-24 bg-gray-200 rounded-full"></div>
            </div>
          </header>

          {/* Two Column Layout Skeleton */}
          <div className="grid grid-cols-1 lg:grid-cols-2 gap-6 mb-8">
            {/* Left Column - Content Skeleton */}
            <div className="flex flex-col gap-6">
              <div className="bg-white rounded-2xl shadow-lg border border-gray-200 p-8 animate-pulse">
                <div className="space-y-4">
                  <div className="h-6 w-48 bg-gray-200 rounded"></div>
                  <div className="h-4 w-full bg-gray-200 rounded"></div>
                  <div className="h-4 w-full bg-gray-200 rounded"></div>
                  <div className="h-4 w-3/4 bg-gray-200 rounded"></div>
                  <div className="h-32 w-full bg-gray-200 rounded mt-6"></div>
                  <div className="h-4 w-full bg-gray-200 rounded"></div>
                  <div className="h-4 w-5/6 bg-gray-200 rounded"></div>
                </div>
              </div>
            </div>

            {/* Right Column - Editor Skeleton */}
            <div className="lg:sticky lg:top-20 lg:self-start">
              <div className="bg-white rounded-2xl shadow-lg border border-gray-200 overflow-hidden" style={{ height: 'calc(100vh - 7rem)' }}>
                <div className="bg-gradient-to-r from-blue-600 to-purple-600 px-6 py-4">
                  <div className="h-6 w-40 bg-white/20 rounded animate-pulse"></div>
                </div>
                <div className="p-6 animate-pulse">
                  <div className="space-y-3">
                    <div className="h-4 w-full bg-gray-200 rounded"></div>
                    <div className="h-4 w-full bg-gray-200 rounded"></div>
                    <div className="h-4 w-3/4 bg-gray-200 rounded"></div>
                    <div className="h-4 w-full bg-gray-200 rounded"></div>
                    <div className="h-4 w-5/6 bg-gray-200 rounded"></div>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    );
  }

  if (error || lessonError || !lesson) {
    return (
      <div className="min-h-screen bg-gradient-to-br from-blue-50 via-white to-purple-50 flex items-center justify-center">
        <div className="text-center">
          <div className="inline-flex items-center justify-center w-16 h-16 bg-red-100 rounded-full mb-4">
            <Icons.XCircle className="w-8 h-8 text-red-600" />
          </div>
          <h2 className="text-2xl font-bold text-gray-900 mb-4">Erro</h2>
          <p className="text-gray-600 mb-6">{error || lessonError || 'Aula não encontrada'}</p>
          <button
            onClick={handleBackClick}
            className="inline-flex items-center gap-2 px-6 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 transition-colors"
          >
            <Icons.ArrowLeft className="w-4 h-4" />
            Voltar ao Curso
          </button>
        </div>
      </div>
    );
  }

  // Find previous and next lessons
  const currentIndex = allLessons.findIndex(l => l.id === lessonId);
  const previousLesson = currentIndex > 0 ? allLessons[currentIndex - 1] : null;
  const nextLesson = currentIndex < allLessons.length - 1 ? allLessons[currentIndex + 1] : null;

  return (
    <div className="min-h-screen bg-gradient-to-br from-blue-50 via-white to-purple-50">
      {/* Header with gradient */}
      <div className="bg-white/80 backdrop-blur-sm border-b border-gray-200 sticky top-0 z-10 shadow-sm">
        <div className="px-6 py-4">
          <button
            onClick={handleBackClick}
            className="inline-flex items-center gap-2 text-sm font-medium text-gray-600 hover:text-blue-600 transition-colors"
          >
            <Icons.ArrowLeft className="w-4 h-4" />
            Voltar ao Curso
          </button>
        </div>
      </div>

      {/* Content - Two Column Layout */}
      <div className="max-w-[1800px] mx-auto px-6 py-12">
        {/* Breadcrumb Navigation */}
        {course && (
          <Breadcrumb
            items={[
              { label: 'Níveis', href: '/levels' },
              ...(course.levelId && course.levelTitle 
                ? [{ label: course.levelTitle, href: `/levels/${course.levelId}` }]
                : []
              ),
              { label: course.title, href: `/courses/${courseId}` },
              { label: lesson.title },
            ]}
          />
        )}

        {/* Lesson Header */}
        <header className="mb-8">
          <div className="flex items-center gap-3 mb-6 flex-wrap">
            <span className="inline-flex items-center gap-2 px-3 py-1.5 bg-blue-100 text-blue-700 rounded-full text-sm font-semibold">
              <Icons.BookOpen className="w-4 h-4" />
              Aula {lesson.order}
            </span>
            {lesson.isCompleted && (
              <span className="inline-flex items-center gap-2 px-3 py-1.5 bg-green-100 text-green-700 rounded-full text-sm font-semibold">
                <Icons.CheckCircle className="w-4 h-4" />
                Concluída
              </span>
            )}
            {lesson.difficulty && (
              <span className={`inline-flex items-center gap-2 px-3 py-1.5 rounded-full text-sm font-semibold ${
                lesson.difficulty === 'Fácil' ? 'bg-green-100 text-green-700' :
                lesson.difficulty === 'Médio' ? 'bg-yellow-100 text-yellow-700' :
                'bg-red-100 text-red-700'
              }`}>
                {lesson.difficulty}
              </span>
            )}
            {lesson.duration && (
              <span className="inline-flex items-center gap-2 px-3 py-1.5 bg-gray-100 text-gray-700 rounded-full text-sm font-semibold">
                <Icons.Clock className="w-4 h-4" />
                {lesson.duration}
              </span>
            )}
            {lesson.estimatedMinutes && (
              <span className="inline-flex items-center gap-2 px-3 py-1.5 bg-gray-100 text-gray-700 rounded-full text-sm font-semibold">
                {lesson.estimatedMinutes} minutos
              </span>
            )}
          </div>
          <h1 className="text-4xl md:text-5xl font-bold bg-gradient-to-r from-gray-900 via-blue-900 to-purple-900 bg-clip-text text-transparent leading-tight mb-6">
            {lesson.title}
          </h1>
          <div className="h-1 w-24 bg-gradient-to-r from-blue-600 to-purple-600 rounded-full"></div>
        </header>

        {/* Two Column Layout: Content Left, Editor Right */}
        <div className="grid grid-cols-1 lg:grid-cols-2 gap-6 mb-8">
          {/* Left Column - Lesson Content */}
          <div className="flex flex-col gap-6">
            <div className="bg-white rounded-2xl shadow-lg border border-gray-200 p-8 flex-1">
              <LessonContent
                lesson={lesson}
                onRunCode={handleRunCode}
                onStartExercise={handleStartExercise}
              />
            </div>

            {/* Complete Button */}
            {!lesson.isCompleted && (
              <div className="flex justify-center">
                <button
                  onClick={handleCompleteLesson}
                  disabled={completing}
                  className="inline-flex items-center gap-3 px-8 py-4 bg-gradient-to-r from-blue-600 to-purple-600 text-white rounded-xl hover:from-blue-700 hover:to-purple-700 transition-all disabled:opacity-50 disabled:cursor-not-allowed font-semibold text-lg shadow-lg hover:shadow-xl transform hover:-translate-y-0.5"
                >
                  {completing ? (
                    <>
                      <svg className="animate-spin h-5 w-5" fill="none" viewBox="0 0 24 24">
                        <circle className="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" strokeWidth="4"></circle>
                        <path className="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"></path>
                      </svg>
                      Concluindo...
                    </>
                  ) : (
                    <>
                      <Icons.CheckCircle className="w-5 h-5" />
                      Marcar como Concluída
                    </>
                  )}
                </button>
              </div>
            )}
          </div>

          {/* Right Column - Code Editor (Sticky) */}
          <div className="lg:sticky lg:top-20 lg:self-start">
            <div className="bg-white rounded-2xl shadow-lg border border-gray-200 overflow-hidden flex flex-col" style={{ height: 'calc(100vh - 7rem)' }}>
              <div className="bg-gradient-to-r from-blue-600 to-purple-600 px-6 py-4 flex-shrink-0">
                <div className="flex items-center gap-3">
                  <Icons.Code className="w-6 h-6 text-white" />
                  <h2 className="text-xl font-bold text-white">Praticar Código</h2>
                </div>
              </div>
              
              <div className="flex-1 min-h-0">
                <CodeEditor
                  files={files}
                  activeFileIndex={activeFileIndex}
                  onFileChange={(index, content) => {
                    const newFiles = [...files];
                    newFiles[index].content = content;
                    setFiles(newFiles);
                  }}
                  onActiveFileChange={setActiveFileIndex}
                  onGetFeedback={async () => {
                    setShowAIFeedback(true);
                    const context = `Aula: ${lesson?.title}\nConteúdo: ${lesson?.content?.substring(0, 200) || 'Conteúdo estruturado'}...`;
                    await getFeedback(files[activeFileIndex].content, context);
                  }}
                  showAIFeedback={showAIFeedback}
                  aiFeedbackPanel={
                    <AIFeedback
                      suggestions={feedback?.suggestions || []}
                      overallScore={feedback?.overallScore || 0}
                      securityIssues={feedback?.securityIssues || []}
                      performanceIssues={feedback?.performanceIssues || []}
                      isLoading={isLoadingFeedback}
                      onClose={() => {
                        setShowAIFeedback(false);
                        clearFeedback();
                      }}
                    />
                  }
                  onRun={async () => {
                    await handleRunCode(files[activeFileIndex].content);
                  }}
                  output={output}
                  isRunning={isRunning}
                />
              </div>
            </div>
          </div>
        </div>

        {/* Navigation with enhanced cards */}
        <nav className="mt-12">
          <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
            {previousLesson ? (
              <button
                onClick={() => router.push(`/courses/${courseId}/lessons/${previousLesson.id}`)}
                className="group text-left p-6 rounded-xl bg-[#161b22] border border-[#30363d] hover:border-blue-500 hover:shadow-lg transition-all transform hover:-translate-y-1"
              >
                <div className="flex items-center gap-3 mb-3">
                  <div className="w-8 h-8 bg-[#21262d] border border-[#30363d] rounded-lg flex items-center justify-center group-hover:bg-blue-600 group-hover:border-blue-600 transition-colors">
                    <Icons.ArrowLeft className="w-4 h-4 text-gray-400 group-hover:text-white transition-colors" />
                  </div>
                  <span className="text-xs font-semibold text-gray-400 uppercase tracking-wider">
                    Aula Anterior
                  </span>
                </div>
                <div className="font-semibold text-gray-100 group-hover:text-blue-400 transition-colors line-clamp-2 text-lg">
                  {previousLesson.title}
                </div>
              </button>
            ) : (
              <div></div>
            )}
            
            {nextLesson && (
              <button
                onClick={() => router.push(`/courses/${courseId}/lessons/${nextLesson.id}`)}
                className="group text-right p-6 rounded-xl bg-gradient-to-br from-blue-600 to-blue-700 border border-blue-500 hover:from-blue-500 hover:to-blue-600 hover:shadow-xl transition-all transform hover:-translate-y-1 md:col-start-2"
              >
                <div className="flex items-center justify-end gap-3 mb-3">
                  <span className="text-xs font-semibold text-blue-100 uppercase tracking-wider">
                    Próxima Aula
                  </span>
                  <div className="w-8 h-8 bg-white/20 rounded-lg flex items-center justify-center group-hover:bg-white/30 transition-colors">
                    <Icons.ArrowRight className="w-4 h-4 text-white" />
                  </div>
                </div>
                <div className="font-semibold text-white group-hover:text-blue-50 transition-colors line-clamp-2 text-lg">
                  {nextLesson.title}
                </div>
              </button>
            )}
          </div>
        </nav>
      </div>
      
      <style jsx global>{`
        /* Preserve button and icon colors */
        .lesson-content-wrapper button {
          color: inherit !important;
        }
        
        .lesson-content-wrapper button.bg-white,
        .lesson-content-wrapper button.bg-gray-50 {
          color: #374151 !important;
        }
        
        .lesson-content-wrapper button.bg-purple-600,
        .lesson-content-wrapper button.bg-green-600,
        .lesson-content-wrapper button.bg-blue-600 {
          color: #ffffff !important;
        }
        
        .lesson-content {
          font-size: 1.125rem;
          line-height: 1.8;
        }
        
        .lesson-content h2 {
          font-size: 2rem;
          font-weight: 700;
          color: #111827;
          margin-top: 3rem;
          margin-bottom: 1.5rem;
          letter-spacing: -0.025em;
          padding-bottom: 0.75rem;
          border-bottom: 2px solid #e5e7eb;
        }
        
        .lesson-content h2:first-child {
          margin-top: 0;
        }
        
        .lesson-content h3 {
          font-size: 1.5rem;
          font-weight: 600;
          color: #1f2937;
          margin-top: 2.5rem;
          margin-bottom: 1rem;
          letter-spacing: -0.025em;
        }
        
        .lesson-content p {
          margin-bottom: 1.5rem;
          color: #374151;
          line-height: 1.8;
        }
        
        .lesson-content ul {
          margin: 1.5rem 0;
          padding-left: 0;
          list-style: none;
        }
        
        .lesson-content li {
          margin-bottom: 1rem;
          padding-left: 2rem;
          position: relative;
          color: #374151;
          line-height: 1.7;
        }
        
        .lesson-content li::before {
          content: "";
          position: absolute;
          left: 0;
          top: 0.65em;
          width: 8px;
          height: 8px;
          background: linear-gradient(135deg, #3b82f6 0%, #8b5cf6 100%);
          border-radius: 50%;
        }
        
        .lesson-content strong {
          font-weight: 600;
          color: #111827;
        }
        
        .lesson-content pre {
          background: linear-gradient(135deg, #0f172a 0%, #1e293b 100%);
          color: #e2e8f0;
          padding: 1.75rem;
          border-radius: 1rem;
          overflow-x: auto;
          margin: 2.5rem 0;
          font-size: 0.9375rem;
          line-height: 1.7;
          box-shadow: 0 10px 15px -3px rgba(0, 0, 0, 0.1), 0 4px 6px -2px rgba(0, 0, 0, 0.05);
          border: 1px solid rgba(255, 255, 255, 0.1);
        }
        
        .lesson-content code {
          font-family: 'SF Mono', 'Monaco', 'Inconsolata', 'Fira Code', 'Consolas', monospace;
        }
        
        .lesson-content pre code {
          background: none;
          padding: 0;
          color: inherit;
        }
        
        .lesson-content p code {
          background: linear-gradient(135deg, #ede9fe 0%, #dbeafe 100%);
          color: #5b21b6;
          padding: 0.2em 0.5em;
          border-radius: 0.375rem;
          font-size: 0.9em;
          font-weight: 500;
        }
      `}</style>
    </div>
  );
}
