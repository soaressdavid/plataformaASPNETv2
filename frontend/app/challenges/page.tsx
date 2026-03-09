'use client';

import React, { useState, useEffect } from 'react';
import { useRouter } from 'next/navigation';
import { challengesApi } from '@/lib/api/challenges';
import { ChallengeSummary, Difficulty } from '@/lib/types';
import { Navigation, ChallengesSkeleton } from '@/lib/components';

export default function ChallengesPage() {
  const router = useRouter();
  const [challenges, setChallenges] = useState<ChallengeSummary[]>([]);
  const [filteredChallenges, setFilteredChallenges] = useState<ChallengeSummary[]>([]);
  const [selectedDifficulty, setSelectedDifficulty] = useState<Difficulty | 'All'>('All');
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    loadChallenges();
  }, []);

  useEffect(() => {
    filterChallenges();
  }, [challenges, selectedDifficulty]);

  const loadChallenges = async () => {
    try {
      setLoading(true);
      setError(null);
      const response = await challengesApi.getAll();
      setChallenges(response.challenges);
    } catch (err: any) {
      console.warn('Challenges API not available, using mock data');
      
      // Use mock data when API is not available
      const mockChallenges: ChallengeSummary[] = [
        {
          id: '1',
          title: 'Hello World API',
          description: 'Crie sua primeira Web API que retorna "Hello World"',
          difficulty: Difficulty.Easy,
          points: 10,
          tags: ['API', 'Básico'],
          completionRate: 85
        },
        {
          id: '2',
          title: 'CRUD com Entity Framework',
          description: 'Implemente operações CRUD completas usando EF Core',
          difficulty: Difficulty.Medium,
          points: 25,
          tags: ['EF Core', 'Database'],
          completionRate: 60
        },
        {
          id: '3',
          title: 'Autenticação JWT',
          description: 'Implemente autenticação JWT em uma Web API',
          difficulty: Difficulty.Medium,
          points: 30,
          tags: ['Security', 'JWT'],
          completionRate: 45
        },
        {
          id: '4',
          title: 'Microserviço com RabbitMQ',
          description: 'Crie um microserviço que se comunica via RabbitMQ',
          difficulty: Difficulty.Hard,
          points: 50,
          tags: ['Microservices', 'RabbitMQ'],
          completionRate: 25
        }
      ];
      
      setChallenges(mockChallenges);
      setError(null);
    } finally {
      setLoading(false);
    }
  };

  const filterChallenges = () => {
    if (selectedDifficulty === 'All') {
      setFilteredChallenges(challenges);
    } else {
      setFilteredChallenges(
        challenges.filter((challenge) => challenge.difficulty === selectedDifficulty)
      );
    }
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

  const handleChallengeClick = (challengeId: string) => {
    router.push(`/challenges/${challengeId}`);
  };

  if (loading) {
    return (
      <div className="min-h-screen bg-gray-50">
        <Navigation />
        <ChallengesSkeleton />
      </div>
    );
  }

  if (error) {
    return (
      <div className="min-h-screen bg-gray-50 flex items-center justify-center">
        <div className="text-center">
          <p className="text-red-600 mb-4">{error}</p>
          <button
            onClick={loadChallenges}
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
          <h1 className="text-2xl sm:text-3xl font-bold text-gray-900 mb-2">Desafios de Programação</h1>
          <p className="text-sm sm:text-base text-gray-600">
            Pratique suas habilidades em ASP.NET Core com estes desafios de programação
          </p>
        </div>

        {/* Difficulty Filter */}
        <div className="mb-6">
          <span className="text-sm font-medium text-gray-700 mb-2 block">Filtrar por dificuldade:</span>
          <div className="flex flex-wrap gap-2">
            {['All', Difficulty.Easy, Difficulty.Medium, Difficulty.Hard].map((difficulty) => (
              <button
                key={difficulty}
                onClick={() => setSelectedDifficulty(difficulty as Difficulty | 'All')}
                className={`px-3 sm:px-4 py-2 rounded-lg text-xs sm:text-sm font-medium transition-colors ${
                  selectedDifficulty === difficulty
                    ? 'bg-blue-600 text-white'
                    : 'bg-white text-gray-700 hover:bg-gray-100 border border-gray-300'
                }`}
              >
                {difficulty}
              </button>
            ))}
          </div>
        </div>

        {/* Challenge Stats */}
        <div className="mb-6 grid grid-cols-1 sm:grid-cols-4 gap-4">
          <div className="bg-white rounded-lg shadow p-4">
            <p className="text-sm text-gray-600">Total de Desafios</p>
            <p className="text-2xl font-bold text-gray-900">{challenges.length}</p>
          </div>
          <div className="bg-white rounded-lg shadow p-4">
            <p className="text-sm text-gray-600">Resolvidos</p>
            <p className="text-2xl font-bold text-green-600">
              {challenges.filter((c) => c.isSolved).length}
            </p>
          </div>
          <div className="bg-white rounded-lg shadow p-4">
            <p className="text-sm text-gray-600">Em Progresso</p>
            <p className="text-2xl font-bold text-yellow-600">
              {challenges.filter((c) => !c.isSolved && c.submissionCount > 0).length}
            </p>
          </div>
          <div className="bg-white rounded-lg shadow p-4">
            <p className="text-sm text-gray-600">Não Iniciados</p>
            <p className="text-2xl font-bold text-gray-600">
              {challenges.filter((c) => c.submissionCount === 0).length}
            </p>
          </div>
        </div>

        {/* Challenge List */}
        {filteredChallenges.length === 0 ? (
          <div className="bg-white rounded-lg shadow p-8 text-center">
            <p className="text-gray-600">Nenhum desafio encontrado para a dificuldade selecionada.</p>
          </div>
        ) : (
          <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
            {filteredChallenges.map((challenge) => (
              <div
                key={challenge.id}
                onClick={() => handleChallengeClick(challenge.id)}
                className="bg-white rounded-lg shadow hover:shadow-lg transition-shadow cursor-pointer p-6"
              >
                <div className="flex items-start justify-between mb-3">
                  <h3 className="text-lg font-semibold text-gray-900 flex-1">
                    {challenge.title}
                  </h3>
                  {challenge.isSolved && (
                    <span className="ml-2 text-green-600" title="Resolvido">
                      ✓
                    </span>
                  )}
                </div>

                <div className="flex items-center gap-2 mb-4">
                  <span
                    className={`px-3 py-1 rounded-full text-xs font-medium ${getDifficultyColor(
                      challenge.difficulty
                    )}`}
                  >
                    {challenge.difficulty}
                  </span>
                </div>

                <div className="flex items-center justify-between text-sm text-gray-600">
                  <span>
                    {challenge.submissionCount}{' '}
                    {challenge.submissionCount === 1 ? 'submissão' : 'submissões'}
                  </span>
                  <span className="text-blue-600 hover:text-blue-700 font-medium">
                    Iniciar Desafio →
                  </span>
                </div>
              </div>
            ))}
          </div>
        )}
      </div>
    </div>
  );
}
