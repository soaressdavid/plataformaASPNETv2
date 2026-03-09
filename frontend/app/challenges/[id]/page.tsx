'use client';

import React, { useState, useEffect } from 'react';
import { useParams, useRouter } from 'next/navigation';
import { challengesApi } from '@/lib/api/challenges';
import { 
  CodeEditor, 
  CodeFile, 
  ChallengeDetailSkeleton, 
  AIFeedback,
  CountdownTimer,
  TimeAttackLeaderboard 
} from '@/lib/components';
import { useAIFeedback } from '@/lib/hooks/useAIFeedback';
import {
  ChallengeDetailResponse,
  TestResult,
  Difficulty,
  SubmitSolutionResponse,
} from '@/lib/types';

export default function ChallengeDetailPage() {
  const params = useParams();
  const router = useRouter();
  const challengeId = params.id as string;

  const [challenge, setChallenge] = useState<ChallengeDetailResponse | null>(null);
  const [files, setFiles] = useState<CodeFile[]>([]);
  const [activeFileIndex, setActiveFileIndex] = useState(0);
  const [output, setOutput] = useState('');
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [testResults, setTestResults] = useState<TestResult[]>([]);
  const [submissionResult, setSubmissionResult] = useState<SubmitSolutionResponse | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [showSubmissionHistory, setShowSubmissionHistory] = useState(false);
  const [showAIFeedback, setShowAIFeedback] = useState(false);
  
  // Time Attack state
  const [isTimeAttackMode, setIsTimeAttackMode] = useState(false);
  const [timeAttackStartTime, setTimeAttackStartTime] = useState<number | null>(null);
  const [timeAttackElapsed, setTimeAttackElapsed] = useState(0);
  const [showLeaderboard, setShowLeaderboard] = useState(false);
  const [leaderboardData, setLeaderboardData] = useState<any[]>([]);
  const [userBestTime, setUserBestTime] = useState<any>(null);

  // Use the AI feedback hook
  const { feedback, isLoading: isLoadingFeedback, getFeedback, clearFeedback } = useAIFeedback();

  useEffect(() => {
    loadChallenge();
    loadLeaderboard();
    loadUserBestTime();
  }, [challengeId]);

  const loadChallenge = async () => {
    try {
      setLoading(true);
      setError(null);
      const response = await challengesApi.getById(challengeId);
      setChallenge(response);

      // Initialize code editor with starter code
      setFiles([
        {
          name: 'Solution.cs',
          content: response.starterCode,
          language: 'csharp',
        },
      ]);
    } catch (err) {
      setError('Falha ao carregar desafio. Tente novamente mais tarde.');
      console.error('Error loading challenge:', err);
    } finally {
      setLoading(false);
    }
  };

  const loadLeaderboard = async () => {
    try {
      const response = await fetch(`http://localhost:5003/api/challenges/${challengeId}/leaderboard`);
      if (response.ok) {
        const data = await response.json();
        setLeaderboardData(data.leaderboard || []);
      }
    } catch (err) {
      console.error('Error loading leaderboard:', err);
    }
  };

  const loadUserBestTime = async () => {
    try {
      const userId = localStorage.getItem('user_id') || '';
      const response = await fetch(`http://localhost:5003/api/challenges/${challengeId}/best-time?userId=${userId}`);
      if (response.ok) {
        const data = await response.json();
        setUserBestTime(data);
      }
    } catch (err) {
      console.error('Error loading user best time:', err);
    }
  };

  const handleStartTimeAttack = () => {
    setIsTimeAttackMode(true);
    setTimeAttackStartTime(Date.now());
    setTimeAttackElapsed(0);
    setTestResults([]);
    setSubmissionResult(null);
    setOutput('');
  };

  const handleStopTimeAttack = () => {
    setIsTimeAttackMode(false);
    setTimeAttackStartTime(null);
    setTimeAttackElapsed(0);
  };

  const handleTimeAttackTick = (remainingSeconds: number) => {
    if (timeAttackStartTime && challenge) {
      const elapsed = Math.floor((Date.now() - timeAttackStartTime) / 1000);
      setTimeAttackElapsed(elapsed);
    }
  };

  const handleTimeUp = () => {
    setOutput('⏰ Tempo esgotado! Tente novamente.\n');
    setIsTimeAttackMode(false);
    setTimeAttackStartTime(null);
  };

  const handleFileChange = (index: number, content: string) => {
    const newFiles = [...files];
    newFiles[index].content = content;
    setFiles(newFiles);
  };

  const handleSubmitSolution = async () => {
    if (!challenge) return;

    try {
      setIsSubmitting(true);
      setOutput('Enviando solução...\n');
      setTestResults([]);
      setSubmissionResult(null);

      // Get user ID from localStorage (set during login)
      const userId = localStorage.getItem('user_id') || '';

      // Calculate completion time for Time Attack mode
      let completionTimeSeconds = null;
      if (isTimeAttackMode && timeAttackStartTime) {
        completionTimeSeconds = Math.floor((Date.now() - timeAttackStartTime) / 1000);
      }

      const response = await challengesApi.submitSolution(challengeId, {
        userId,
        code: files[activeFileIndex].content,
        isTimeAttack: isTimeAttackMode,
        completionTimeSeconds,
      });

      setSubmissionResult(response);
      setTestResults(response.results);

      // Build output message
      let outputMessage = '=== Resultados dos Testes ===\n\n';
      outputMessage += `Total de Testes: ${response.results.length}\n`;
      outputMessage += `Aprovados: ${response.results.filter((r) => r.passed).length}\n`;
      outputMessage += `Reprovados: ${response.results.filter((r) => !r.passed).length}\n\n`;

      if (response.allTestsPassed) {
        outputMessage += '✓ Todos os testes passaram!\n';
        outputMessage += `XP Ganho: ${response.xpAwarded}\n`;
        
        if (isTimeAttackMode && completionTimeSeconds) {
          outputMessage += `\n⚡ TIME ATTACK COMPLETO!\n`;
          outputMessage += `Tempo: ${Math.floor(completionTimeSeconds / 60)}:${(completionTimeSeconds % 60).toString().padStart(2, '0')}\n`;
          if (response.timeAttackBonusXP) {
            outputMessage += `Bonus XP: +${response.timeAttackBonusXP}\n`;
          }
          
          // Stop time attack mode and reload leaderboard
          setIsTimeAttackMode(false);
          setTimeAttackStartTime(null);
          loadLeaderboard();
          loadUserBestTime();
        }
      } else {
        outputMessage += '✗ Alguns testes falharam. Veja os detalhes abaixo.\n';
      }

      setOutput(outputMessage);
    } catch (err) {
      setOutput('Erro ao enviar solução. Tente novamente.\n');
      console.error('Error submitting solution:', err);
    } finally {
      setIsSubmitting(false);
    }
  };

  const handleGetFeedback = async () => {
    if (!challenge) return;
    
    const code = files[activeFileIndex].content;
    const context = `Challenge: ${challenge.title}\nDifficulty: ${challenge.difficulty}\nDescription: ${challenge.description}`;
    
    setShowAIFeedback(true);
    await getFeedback(code, context);
  };

  const handleCloseFeedback = () => {
    setShowAIFeedback(false);
    clearFeedback();
  };

  const getDifficultyColor = (difficulty: Difficulty) => {
    switch (difficulty) {
      case Difficulty.Easy:
        return 'text-green-600 bg-green-100';
      case Difficulty.Medium:
        return 'text-yellow-600 bg-yellow-100';
      case Difficulty.Hard:
        return 'text-red-600 bg-red-100';
      default:
        return 'text-gray-600 bg-gray-100';
    }
  };

  const getDifficultyLabel = (difficulty: Difficulty) => {
    switch (difficulty) {
      case Difficulty.Easy:
        return 'Fácil';
      case Difficulty.Medium:
        return 'Médio';
      case Difficulty.Hard:
        return 'Difícil';
      default:
        return difficulty;
    }
  };

  if (loading) {
    return <ChallengeDetailSkeleton />;
  }

  if (error || !challenge) {
    return (
      <div className="min-h-screen bg-gray-50 flex items-center justify-center">
        <div className="text-center">
          <p className="text-red-600 mb-4">{error || 'Desafio não encontrado'}</p>
          <button
            onClick={() => router.push('/challenges')}
            className="px-4 py-2 bg-blue-600 text-white rounded hover:bg-blue-700"
          >
            Voltar aos Desafios
          </button>
        </div>
      </div>
    );
  }

  return (
    <div className="h-screen flex flex-col bg-gray-50">
      {/* Header */}
      <div className="bg-white border-b border-gray-200 px-6 py-4">
        <div className="flex items-center justify-between">
          <div className="flex items-center gap-4">
            <button
              onClick={() => router.push('/challenges')}
              className="text-gray-600 hover:text-gray-900"
            >
              ← Voltar
            </button>
            <h1 className="text-2xl font-bold text-gray-900">{challenge.title}</h1>
            <span
              className={`px-3 py-1 rounded-full text-xs font-medium ${getDifficultyColor(
                challenge.difficulty
              )}`}
            >
              {getDifficultyLabel(challenge.difficulty)}
            </span>
            {challenge.supportsTimeAttack && (
              <span className="px-3 py-1 rounded-full text-xs font-medium bg-purple-100 text-purple-700">
                ⚡ Time Attack
              </span>
            )}
          </div>
          
          <div className="flex items-center gap-3">
            {/* Time Attack Controls */}
            {challenge.supportsTimeAttack && !isTimeAttackMode && (
              <button
                onClick={handleStartTimeAttack}
                className="px-4 py-2 text-sm font-medium text-white bg-gradient-to-r from-purple-600 to-blue-600 rounded hover:from-purple-700 hover:to-blue-700 transition-all"
              >
                ⚡ Iniciar Time Attack
              </button>
            )}
            
            {isTimeAttackMode && (
              <button
                onClick={handleStopTimeAttack}
                className="px-4 py-2 text-sm font-medium text-white bg-red-600 rounded hover:bg-red-700"
              >
                ✕ Cancelar Time Attack
              </button>
            )}
            
            {challenge.supportsTimeAttack && (
              <button
                onClick={() => setShowLeaderboard(!showLeaderboard)}
                className="px-4 py-2 text-sm text-gray-700 hover:text-gray-900 border border-gray-300 rounded hover:bg-gray-50"
              >
                🏆 Leaderboard
              </button>
            )}
            
            <button
              onClick={() => setShowSubmissionHistory(!showSubmissionHistory)}
              className="px-4 py-2 text-sm text-gray-700 hover:text-gray-900 border border-gray-300 rounded hover:bg-gray-50"
            >
              Histórico de Envios
            </button>
          </div>
        </div>
        
        {/* Time Attack Timer */}
        {isTimeAttackMode && challenge.timeAttackLimitSeconds && (
          <div className="mt-4 flex justify-center">
            <div className="bg-gradient-to-r from-purple-50 to-blue-50 rounded-lg p-4 border-2 border-purple-200">
              <CountdownTimer
                initialSeconds={challenge.timeAttackLimitSeconds}
                onTimeUp={handleTimeUp}
                onTick={handleTimeAttackTick}
                isPaused={isSubmitting}
              />
            </div>
          </div>
        )}
      </div>

      {/* Main Content */}
      <div className="flex-1 flex overflow-hidden">
        {/* Left Panel - Description and Test Cases */}
        <div className="w-1/2 border-r border-gray-200 overflow-y-auto bg-white">
          <div className="p-6">
            {/* Description */}
            <div className="mb-8">
              <h2 className="text-lg font-semibold text-gray-900 mb-3">Descrição</h2>
              <div className="prose prose-sm max-w-none text-gray-700 whitespace-pre-wrap">
                {challenge.description}
              </div>
            </div>

            {/* Test Cases */}
            <div className="mb-8">
              <h2 className="text-lg font-semibold text-gray-900 mb-3">Casos de Teste</h2>
              <div className="space-y-4">
                {challenge.testCases
                  .filter((tc) => !tc.isHidden)
                  .map((testCase, index) => (
                    <div key={index} className="bg-gray-50 rounded-lg p-4 border border-gray-200">
                      <p className="text-sm font-medium text-gray-700 mb-2">
                        Caso de Teste {index + 1}
                      </p>
                      <div className="space-y-2">
                        <div>
                          <p className="text-xs text-gray-600 mb-1">Entrada:</p>
                          <pre className="text-sm bg-white p-2 rounded border border-gray-200 overflow-x-auto">
                            {testCase.input}
                          </pre>
                        </div>
                        <div>
                          <p className="text-xs text-gray-600 mb-1">Saída Esperada:</p>
                          <pre className="text-sm bg-white p-2 rounded border border-gray-200 overflow-x-auto">
                            {testCase.expectedOutput}
                          </pre>
                        </div>
                      </div>
                    </div>
                  ))}
                {challenge.testCases.some((tc) => tc.isHidden) && (
                  <p className="text-sm text-gray-600 italic">
                    + {challenge.testCases.filter((tc) => tc.isHidden).length} caso(s) de teste oculto(s)
                  </p>
                )}
              </div>
            </div>

            {/* Test Results */}
            {testResults.length > 0 && (
              <div className="mb-8">
                <h2 className="text-lg font-semibold text-gray-900 mb-3">Resultados dos Testes</h2>
                <div className="space-y-3">
                  {testResults.map((result, index) => (
                    <div
                      key={index}
                      className={`rounded-lg p-4 border ${
                        result.passed
                          ? 'bg-green-50 border-green-200'
                          : 'bg-red-50 border-red-200'
                      }`}
                    >
                      <div className="flex items-center justify-between mb-2">
                        <p className="text-sm font-medium">Caso de Teste {index + 1}</p>
                        <span
                          className={`text-sm font-semibold ${
                            result.passed ? 'text-green-600' : 'text-red-600'
                          }`}
                        >
                          {result.passed ? '✓ Aprovado' : '✗ Reprovado'}
                        </span>
                      </div>
                      {!result.passed && (
                        <div className="space-y-2 text-sm">
                          <div>
                            <p className="text-xs text-gray-600 mb-1">Entrada:</p>
                            <pre className="bg-white p-2 rounded border border-gray-200 overflow-x-auto">
                              {result.input}
                            </pre>
                          </div>
                          <div>
                            <p className="text-xs text-gray-600 mb-1">Esperado:</p>
                            <pre className="bg-white p-2 rounded border border-gray-200 overflow-x-auto">
                              {result.expectedOutput}
                            </pre>
                          </div>
                          <div>
                            <p className="text-xs text-gray-600 mb-1">Obtido:</p>
                            <pre className="bg-white p-2 rounded border border-red-200 overflow-x-auto">
                              {result.actualOutput}
                            </pre>
                          </div>
                        </div>
                      )}
                    </div>
                  ))}
                </div>

                {submissionResult && submissionResult.allTestsPassed && (
                  <div className="mt-4 bg-green-100 border border-green-300 rounded-lg p-4">
                    <p className="text-green-800 font-semibold">
                      🎉 Parabéns! Todos os testes passaram!
                    </p>
                    <p className="text-green-700 text-sm mt-1">
                      Você ganhou {submissionResult.xpAwarded} XP
                    </p>
                  </div>
                )}
              </div>
            )}
          </div>
        </div>

        {/* Right Panel - Code Editor */}
        <div className="w-1/2 flex flex-col">
          <CodeEditor
            files={files}
            activeFileIndex={activeFileIndex}
            onFileChange={handleFileChange}
            onActiveFileChange={setActiveFileIndex}
            onRun={handleSubmitSolution}
            output={output}
            isRunning={isSubmitting}
            onGetFeedback={handleGetFeedback}
            showAIFeedback={showAIFeedback}
            aiFeedbackPanel={
              <AIFeedback
                suggestions={feedback?.suggestions || []}
                overallScore={feedback?.overallScore || 0}
                securityIssues={feedback?.securityIssues || []}
                performanceIssues={feedback?.performanceIssues || []}
                isLoading={isLoadingFeedback}
                onClose={handleCloseFeedback}
              />
            }
          />
        </div>
      </div>

      {/* Submission History Modal */}
      {showSubmissionHistory && (
        <div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50">
          <div className="bg-white rounded-lg shadow-xl max-w-2xl w-full max-h-[80vh] overflow-hidden">
            <div className="px-6 py-4 border-b border-gray-200 flex items-center justify-between">
              <h2 className="text-xl font-semibold text-gray-900">Histórico de Envios</h2>
              <button
                onClick={() => setShowSubmissionHistory(false)}
                className="text-gray-400 hover:text-gray-600"
              >
                ✕
              </button>
            </div>
            <div className="p-6 overflow-y-auto">
              <p className="text-gray-600 text-center py-8">
                Funcionalidade de histórico de envios em breve...
              </p>
            </div>
          </div>
        </div>
      )}

      {/* Time Attack Leaderboard Modal */}
      {showLeaderboard && (
        <div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50 p-4">
          <div className="bg-white rounded-lg shadow-xl max-w-4xl w-full max-h-[90vh] overflow-hidden">
            <div className="px-6 py-4 border-b border-gray-200 flex items-center justify-between bg-gradient-to-r from-purple-600 to-blue-600">
              <h2 className="text-xl font-semibold text-white">Time Attack Leaderboard</h2>
              <button
                onClick={() => setShowLeaderboard(false)}
                className="text-white hover:text-gray-200"
              >
                ✕
              </button>
            </div>
            <div className="p-6 overflow-y-auto max-h-[calc(90vh-80px)]">
              <TimeAttackLeaderboard
                entries={leaderboardData}
                userBestTime={userBestTime}
              />
            </div>
          </div>
        </div>
      )}
    </div>
  );
}