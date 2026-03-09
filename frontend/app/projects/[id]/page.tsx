'use client';

import { useState, useEffect } from 'react';
import { useParams, useRouter } from 'next/navigation';
import { projectsApi } from '@/lib/api/projects';
import { ProjectDetailResponse, ProjectStep } from '@/lib/types';
import { Navigation, CodeEditor } from '@/lib/components';
import { Icons } from '@/lib/components/Icons';

export default function ProjectDetailPage() {
  const params = useParams();
  const router = useRouter();
  const projectId = params.id as string;

  const [project, setProject] = useState<ProjectDetailResponse | null>(null);
  const [currentStepIndex, setCurrentStepIndex] = useState(0);
  const [stepCodes, setStepCodes] = useState<Record<number, string>>({});
  const [validating, setValidating] = useState(false);
  const [validationResult, setValidationResult] = useState<{
    success: boolean;
    message: string;
  } | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    loadProject();
  }, [projectId]);

  useEffect(() => {
    if (project && project.steps.length > 0) {
      // Initialize step codes if not already set
      if (!stepCodes[currentStepIndex]) {
        const step = project.steps[currentStepIndex];
        setStepCodes(prev => ({
          ...prev,
          [currentStepIndex]: step.starterCode
        }));
      }
      setValidationResult(null);
    }
  }, [currentStepIndex, project]);

  const loadProject = async () => {
    try {
      setLoading(true);
      setError(null);
      const response = await projectsApi.getById(projectId);
      setProject(response);
      setCurrentStepIndex(response.currentStep - 1);
    } catch (err) {
      setError('Failed to load project. Please try again later.');
      console.error('Error loading project:', err);
    } finally {
      setLoading(false);
    }
  };

  const handleValidateStep = async () => {
    if (!project) return;

    const code = stepCodes[currentStepIndex] || '';

    try {
      setValidating(true);
      setValidationResult(null);

      const userId = localStorage.getItem('userId') || '';
      const response = await projectsApi.validateStep(
        projectId,
        currentStepIndex + 1,
        { userId, code }
      );

      setValidationResult({
        success: response.success,
        message: response.message,
      });

      if (response.success && response.nextStepUnlocked) {
        // Update project state without reloading
        setProject({
          ...project,
          currentStep: project.currentStep + 1
        });
        
        // Automatically advance to next step after a short delay
        setTimeout(() => {
          if (currentStepIndex < project.steps.length - 1) {
            setCurrentStepIndex(currentStepIndex + 1);
          }
        }, 1500); // 1.5 second delay to show success message
      }
    } catch (err) {
      setValidationResult({
        success: false,
        message: 'Failed to validate step. Please try again.',
      });
      console.error('Error validating step:', err);
    } finally {
      setValidating(false);
    }
  };

  const handleNextStep = () => {
    if (project && currentStepIndex < project.steps.length - 1) {
      setCurrentStepIndex(currentStepIndex + 1);
    }
  };

  const handlePreviousStep = () => {
    if (currentStepIndex > 0) {
      setCurrentStepIndex(currentStepIndex - 1);
    }
  };

  if (loading) {
    return (
      <div className="min-h-screen bg-gray-50">
        <Navigation />
        <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
          <div className="animate-pulse">
            <div className="h-8 bg-gray-300 rounded w-1/3 mb-4"></div>
            <div className="h-4 bg-gray-300 rounded w-1/2 mb-8"></div>
            <div className="grid grid-cols-1 lg:grid-cols-2 gap-6">
              <div className="bg-white rounded-lg shadow p-6">
                <div className="h-6 bg-gray-300 rounded mb-4"></div>
                <div className="h-4 bg-gray-300 rounded mb-2"></div>
                <div className="h-4 bg-gray-300 rounded w-3/4"></div>
              </div>
              <div className="bg-white rounded-lg shadow p-6">
                <div className="h-96 bg-gray-300 rounded"></div>
              </div>
            </div>
          </div>
        </div>
      </div>
    );
  }

  if (error || !project) {
    return (
      <div className="min-h-screen bg-gray-50 flex items-center justify-center">
        <div className="text-center">
          <p className="text-red-600 mb-4">{error || 'Project not found'}</p>
          <button
            onClick={() => router.push('/projects')}
            className="px-4 py-2 bg-blue-600 text-white rounded hover:bg-blue-700"
          >
            Back to Projects
          </button>
        </div>
      </div>
    );
  }

  const currentStep = project.steps[currentStepIndex];
  const isLastStep = currentStepIndex === project.steps.length - 1;
  const isStepUnlocked = currentStepIndex < project.currentStep;
  const completionPercentage = Math.round(
    ((project.currentStep - 1) / project.steps.length) * 100
  );

  return (
    <div className="min-h-screen bg-gray-50">
      <Navigation />
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
        {/* Header */}
        <div className="mb-6">
          <button
            onClick={() => router.push('/projects')}
            className="text-blue-600 hover:text-blue-700 mb-4 flex items-center gap-2 font-medium transition-colors"
          >
            <Icons.ArrowLeft className="w-5 h-5" />
            <span>Voltar aos Projetos</span>
          </button>
          <h1 className="text-4xl font-bold text-gray-900 mb-3">{project.title}</h1>
          <p className="text-lg text-gray-600 mb-6">{project.description}</p>

          {/* Progress Bar */}
          <div className="bg-white rounded-lg shadow p-6">
            <div className="flex items-center justify-between mb-3">
              <span className="text-sm font-semibold text-gray-700">
                Etapa {currentStepIndex + 1} de {project.steps.length}
              </span>
              <span className="text-sm font-semibold text-blue-600">
                {completionPercentage}% Completo
              </span>
            </div>
            <div className="w-full bg-gray-200 rounded-full h-3">
              <div
                className="bg-gradient-to-r from-blue-500 to-blue-600 h-3 rounded-full transition-all duration-500 shadow-sm"
                style={{ width: `${completionPercentage}%` }}
              ></div>
            </div>
          </div>
        </div>

        {/* Step Navigation */}
        <div className="mb-6 bg-white rounded-lg shadow p-4">
          <div className="flex items-center gap-2 overflow-x-auto pb-2">
            {project.steps.map((step, index) => (
              <button
                key={index}
                onClick={() => index < project.currentStep && setCurrentStepIndex(index)}
                disabled={index >= project.currentStep}
                className={`shrink-0 px-5 py-3 rounded-lg text-sm font-semibold transition-all ${
                  index === currentStepIndex
                    ? 'bg-gradient-to-r from-blue-600 to-blue-700 text-white shadow-lg scale-105'
                    : index < project.currentStep
                    ? 'bg-green-100 text-green-700 hover:bg-green-200 hover:shadow-md'
                    : 'bg-gray-100 text-gray-400 cursor-not-allowed'
                }`}
              >
                <div className="flex items-center gap-2">
                  {index < project.currentStep - 1 && <Icons.CheckCircle className="w-4 h-4" />}
                  <span>Etapa {index + 1}</span>
                </div>
              </button>
            ))}
          </div>
        </div>

        {/* Main Content */}
        <div className="grid grid-cols-1 lg:grid-cols-2 gap-6">
          {/* Instructions Panel */}
          <div className="bg-white rounded-lg shadow p-6">
            <h2 className="text-2xl font-bold text-gray-900 mb-6">
              {currentStep.title}
            </h2>

            <div className="mb-8">
              <h3 className="text-sm font-semibold text-gray-700 uppercase tracking-wide mb-3 flex items-center gap-2">
                <Icons.ClipboardList className="w-5 h-5 text-blue-600" />
                Instruções
              </h3>
              <div className="bg-blue-50 border-l-4 border-blue-500 p-4 rounded-r-lg">
                <p className="text-gray-700 leading-relaxed whitespace-pre-wrap">
                  {currentStep.instructions}
                </p>
              </div>
            </div>

            <div className="mb-8">
              <h3 className="text-sm font-semibold text-gray-700 uppercase tracking-wide mb-3 flex items-center gap-2">
                <Icons.CheckBadge className="w-5 h-5 text-green-600" />
                Critérios de Validação
              </h3>
              <div className="bg-green-50 border-l-4 border-green-500 p-4 rounded-r-lg">
                <p className="text-gray-700 leading-relaxed whitespace-pre-wrap">
                  {currentStep.validationCriteria}
                </p>
              </div>
            </div>

            {/* Validation Result */}
            {validationResult && (
              <div
                className={`p-4 rounded-lg mb-6 border-l-4 ${
                  validationResult.success
                    ? 'bg-green-50 border-green-500'
                    : 'bg-red-50 border-red-500'
                }`}
              >
                <div className="flex items-start gap-3">
                  {validationResult.success ? (
                    <Icons.CheckCircle className="w-6 h-6 text-green-600 flex-shrink-0" />
                  ) : (
                    <Icons.XCircle className="w-6 h-6 text-red-600 flex-shrink-0" />
                  )}
                  <p
                    className={`text-sm flex-1 ${
                      validationResult.success ? 'text-green-800' : 'text-red-800'
                    }`}
                  >
                    {validationResult.message}
                  </p>
                </div>
              </div>
            )}

            {/* Action Buttons */}
            <div className="flex items-center gap-3">
              <button
                onClick={handleValidateStep}
                disabled={validating || !isStepUnlocked}
                className={`flex-1 px-6 py-3 rounded-lg font-semibold transition-all ${
                  validating || !isStepUnlocked
                    ? 'bg-gray-300 text-gray-500 cursor-not-allowed'
                    : 'bg-blue-600 text-white hover:bg-blue-700 hover:shadow-lg'
                }`}
              >
                {validating ? 'Validando...' : 'Validar Etapa'}
              </button>
            </div>

            {!isStepUnlocked && (
              <div className="mt-4 flex items-center gap-2 text-sm text-yellow-700 bg-yellow-50 border border-yellow-200 rounded-lg p-3">
                <Icons.LockClosed className="w-5 h-5 flex-shrink-0" />
                <span>Complete as etapas anteriores para desbloquear esta etapa</span>
              </div>
            )}
          </div>

          {/* Code Editor Panel */}
          <div className="bg-white rounded-lg shadow p-6 flex flex-col" style={{ minHeight: '700px' }}>
            <div className="flex items-center justify-between mb-4">
              <h3 className="text-xl font-bold text-gray-900 flex items-center gap-2">
                <Icons.Code className="w-6 h-6 text-purple-600" />
                Editor de Código
              </h3>
              <div className="flex items-center gap-2">
                <button
                  onClick={handlePreviousStep}
                  disabled={currentStepIndex === 0}
                  className={`px-4 py-2 rounded-lg text-sm font-medium transition-all flex items-center gap-2 ${
                    currentStepIndex === 0
                      ? 'bg-gray-100 text-gray-400 cursor-not-allowed'
                      : 'bg-gray-200 text-gray-700 hover:bg-gray-300 hover:shadow-md'
                  }`}
                >
                  <Icons.ArrowLeft className="w-4 h-4" />
                  Anterior
                </button>
                <button
                  onClick={handleNextStep}
                  disabled={currentStepIndex >= project.currentStep - 1}
                  className={`px-4 py-2 rounded-lg text-sm font-medium transition-all flex items-center gap-2 ${
                    currentStepIndex >= project.currentStep - 1
                      ? 'bg-gray-100 text-gray-400 cursor-not-allowed'
                      : 'bg-gray-200 text-gray-700 hover:bg-gray-300 hover:shadow-md'
                  }`}
                >
                  Próxima
                  <Icons.ArrowRight className="w-4 h-4" />
                </button>
              </div>
            </div>

            <div className="border-2 border-gray-200 rounded-lg overflow-hidden flex-1 shadow-inner">
              {stepCodes[currentStepIndex] !== undefined && (
                <CodeEditor
                  files={[
                    {
                      name: 'Program.cs',
                      content: stepCodes[currentStepIndex],
                      language: 'csharp',
                    },
                  ]}
                  activeFileIndex={0}
                  onFileChange={(index, content) => {
                    setStepCodes(prev => ({
                      ...prev,
                      [currentStepIndex]: content
                    }));
                  }}
                  onActiveFileChange={() => {}}
                  onAddFile={() => {}}
                  onRemoveFile={() => {}}
                  onRun={() => {}}
                  output=""
                  isRunning={false}
                />
              )}
            </div>
          </div>
        </div>

        {/* Completion Status */}
        {project.currentStep > project.steps.length && (
          <div className="mt-6 bg-green-50 border border-green-200 rounded-lg p-6 text-center">
            <div className="mb-4 flex justify-center">
              <Icons.Celebration className="w-16 h-16 text-green-600" />
            </div>
            <h3 className="text-xl font-semibold text-green-900 mb-2">
              Projeto Concluído!
            </h3>
            <p className="text-green-700 mb-4">
              Congratulations! You've completed all steps and earned 100 XP.
            </p>
            <button
              onClick={() => router.push('/projects')}
              className="px-6 py-2 bg-green-600 text-white rounded-lg font-medium hover:bg-green-700"
            >
              Back to Projects
            </button>
          </div>
        )}
      </div>
    </div>
  );
}
