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
      // MOCK DATA - Simular leaderboard
      const mockLeaderboard = [
        { userId: 'user1', username: 'CodeMaster', completionTime: 120, rank: 1 },
        { userId: 'user2', username: 'SpeedCoder', completionTime: 135, rank: 2 },
        { userId: 'user3', username: 'QuickSolver', completionTime: 150, rank: 3 },
        { userId: 'user4', username: 'FastDev', completionTime: 180, rank: 4 },
        { userId: 'user5', username: 'RapidCode', completionTime: 200, rank: 5 }
      ];
      setLeaderboardData(mockLeaderboard);
    } catch (err) {
      console.error('Error loading leaderboard:', err);
    }
  };

  const loadUserBestTime = async () => {
    try {
      // MOCK DATA - Simular melhor tempo do usuário
      const mockUserBestTime = {
        userId: localStorage.getItem('user_id') || 'current_user',
        username: 'Você',
        completionTime: 165,
        rank: 6,
        attempts: 3
      };
      setUserBestTime(mockUserBestTime);
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
              <h2 className="text-lg font-semibold text-gray-900 mb-4">Casos de Teste</h2>
              <div className="space-y-6">
                {challenge.testCases
                  .filter((tc) => !tc.isHidden)
                  .map((testCase, index) => (
                    <div key={index} className="bg-gradient-to-r from-blue-50 to-indigo-50 rounded-xl p-5 border-2 border-blue-200 shadow-sm">
                      <div className="flex items-center mb-4">
                        <div className="w-8 h-8 bg-blue-600 text-white rounded-full flex items-center justify-center text-sm font-bold mr-3">
                          {index + 1}
                        </div>
                        <h3 className="text-base font-semibold text-gray-800">
                          Caso de Teste {index + 1}
                        </h3>
                      </div>
                      <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                        <div className="bg-white rounded-lg p-4 border border-gray-300 shadow-sm">
                          <div className="flex items-center mb-2">
                            <div className="w-2 h-2 bg-green-500 rounded-full mr-2"></div>
                            <p className="text-sm font-medium text-gray-700">Entrada</p>
                          </div>
                          <pre className="text-sm bg-gray-50 p-3 rounded-md border border-gray-200 overflow-x-auto font-mono text-gray-800">
                            {testCase.input}
                          </pre>
                        </div>
                        <div className="bg-white rounded-lg p-4 border border-gray-300 shadow-sm">
                          <div className="flex items-center mb-2">
                            <div className="w-2 h-2 bg-blue-500 rounded-full mr-2"></div>
                            <p className="text-sm font-medium text-gray-700">Saída Esperada</p>
                          </div>
                          <pre className="text-sm bg-gray-50 p-3 rounded-md border border-gray-200 overflow-x-auto font-mono text-gray-800">
                            {testCase.expectedOutput}
                          </pre>
                        </div>
                      </div>
                    </div>
                  ))}
                {challenge.testCases.some((tc) => tc.isHidden) && (
                  <div className="bg-yellow-50 border-2 border-yellow-200 rounded-lg p-4 text-center">
                    <div className="flex items-center justify-center mb-2">
                      <div className="w-6 h-6 bg-yellow-500 text-white rounded-full flex items-center justify-center text-xs font-bold mr-2">
                        +
                      </div>
                      <p className="text-sm font-medium text-yellow-800">
                        {challenge.testCases.filter((tc) => tc.isHidden).length} caso(s) de teste oculto(s)
                      </p>
                    </div>
                    <p className="text-xs text-yellow-700">
                      Estes casos serão usados para validar sua solução, mas não são visíveis durante o desenvolvimento.
                    </p>
                  </div>
                )}
              </div>
            </div>

            {/* Test Results */}
            {testResults.length > 0 && (
              <div className="mb-8">
                <h2 className="text-lg font-semibold text-gray-900 mb-4">Resultados dos Testes</h2>
                <div className="space-y-4">
                  {testResults.map((result, index) => (
                    <div
                      key={index}
                      className={`rounded-xl p-5 border-2 shadow-sm ${
                        result.passed
                          ? 'bg-gradient-to-r from-green-50 to-emerald-50 border-green-300'
                          : 'bg-gradient-to-r from-red-50 to-rose-50 border-red-300'
                      }`}
                    >
                      <div className="flex items-center justify-between mb-3">
                        <div className="flex items-center">
                          <div className={`w-8 h-8 rounded-full flex items-center justify-center text-sm font-bold mr-3 ${
                            result.passed 
                              ? 'bg-green-600 text-white' 
                              : 'bg-red-600 text-white'
                          }`}>
                            {index + 1}
                          </div>
                          <h3 className="text-base font-medium text-gray-800">Caso de Teste {index + 1}</h3>
                        </div>
                        <div className={`flex items-center px-3 py-1 rounded-full text-sm font-semibold ${
                          result.passed 
                            ? 'bg-green-100 text-green-700' 
                            : 'bg-red-100 text-red-700'
                        }`}>
                          {result.passed ? (
                            <>
                              <svg className="w-4 h-4 mr-1" fill="currentColor" viewBox="0 0 20 20">
                                <path fillRule="evenodd" d="M16.707 5.293a1 1 0 010 1.414l-8 8a1 1 0 01-1.414 0l-4-4a1 1 0 011.414-1.414L8 12.586l7.293-7.293a1 1 0 011.414 0z" clipRule="evenodd" />
                              </svg>
                              Aprovado
                            </>
                          ) : (
                            <>
                              <svg className="w-4 h-4 mr-1" fill="currentColor" viewBox="0 0 20 20">
                                <path fillRule="evenodd" d="M4.293 4.293a1 1 0 011.414 0L10 8.586l4.293-4.293a1 1 0 111.414 1.414L11.414 10l4.293 4.293a1 1 0 01-1.414 1.414L10 11.414l-4.293 4.293a1 1 0 01-1.414-1.414L8.586 10 4.293 5.707a1 1 0 010-1.414z" clipRule="evenodd" />
                              </svg>
                              Reprovado
                            </>
                          )}
                        </div>
                      </div>
                      {!result.passed && (
                        <div className="grid grid-cols-1 lg:grid-cols-3 gap-4 mt-4">
                          <div className="bg-white rounded-lg p-4 border border-gray-300">
                            <div className="flex items-center mb-2">
                              <div className="w-2 h-2 bg-blue-500 rounded-full mr-2"></div>
                              <p className="text-sm font-medium text-gray-700">Entrada</p>
                            </div>
                            <pre className="text-sm bg-gray-50 p-3 rounded border border-gray-200 overflow-x-auto font-mono">
                              {result.input}
                            </pre>
                          </div>
                          <div className="bg-white rounded-lg p-4 border border-gray-300">
                            <div className="flex items-center mb-2">
                              <div className="w-2 h-2 bg-green-500 rounded-full mr-2"></div>
                              <p className="text-sm font-medium text-gray-700">Esperado</p>
                            </div>
                            <pre className="text-sm bg-gray-50 p-3 rounded border border-gray-200 overflow-x-auto font-mono">
                              {result.expectedOutput}
                            </pre>
                          </div>
                          <div className="bg-white rounded-lg p-4 border border-red-300">
                            <div className="flex items-center mb-2">
                              <div className="w-2 h-2 bg-red-500 rounded-full mr-2"></div>
                              <p className="text-sm font-medium text-gray-700">Obtido</p>
                            </div>
                            <pre className="text-sm bg-red-50 p-3 rounded border border-red-200 overflow-x-auto font-mono">
                              {result.actualOutput}
                            </pre>
                          </div>
                        </div>
                      )}
                    </div>
                  ))}
                </div>

                {submissionResult && submissionResult.allTestsPassed && (
                  <div className="mt-6 bg-gradient-to-r from-green-100 to-emerald-100 border-2 border-green-300 rounded-xl p-6 text-center">
                    <div className="flex items-center justify-center mb-3">
                      <div className="w-12 h-12 bg-green-600 text-white rounded-full flex items-center justify-center mr-4">
                        <svg className="w-6 h-6" fill="currentColor" viewBox="0 0 20 20">
                          <path fillRule="evenodd" d="M16.707 5.293a1 1 0 010 1.414l-8 8a1 1 0 01-1.414 0l-4-4a1 1 0 011.414-1.414L8 12.586l7.293-7.293a1 1 0 011.414 0z" clipRule="evenodd" />
                        </svg>
                      </div>
                      <div>
                        <h3 className="text-xl font-bold text-green-800 mb-1">
                          🎉 Parabéns! Todos os testes passaram!
                        </h3>
                        <p className="text-green-700 font-medium">
                          Você ganhou {submissionResult.xpAwarded} XP
                        </p>
                      </div>
                    </div>
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