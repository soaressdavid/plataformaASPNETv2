'use client';

import React from 'react';
import { Feedback, FeedbackType } from '../../types';
import { Icons } from '../Icons';

export interface AIFeedbackProps {
  suggestions: Feedback[];
  overallScore: number;
  securityIssues: string[];
  performanceIssues: string[];
  isLoading: boolean;
  onClose: () => void;
}

const feedbackTypeColors: Record<FeedbackType, { bg: string; text: string; border: string }> = {
  [FeedbackType.Security]: {
    bg: 'bg-[#1c1917]',
    text: 'text-red-400',
    border: 'border-red-900',
  },
  [FeedbackType.Performance]: {
    bg: 'bg-[#1c1917]',
    text: 'text-yellow-400',
    border: 'border-yellow-900',
  },
  [FeedbackType.BestPractice]: {
    bg: 'bg-[#1c1917]',
    text: 'text-blue-400',
    border: 'border-blue-900',
  },
  [FeedbackType.Architecture]: {
    bg: 'bg-[#1c1917]',
    text: 'text-purple-400',
    border: 'border-purple-900',
  },
};

const feedbackTypeLabels: Record<FeedbackType, string> = {
  [FeedbackType.Security]: 'Segurança',
  [FeedbackType.Performance]: 'Performance',
  [FeedbackType.BestPractice]: 'Boas Práticas',
  [FeedbackType.Architecture]: 'Arquitetura',
};

export const AIFeedback: React.FC<AIFeedbackProps> = ({
  suggestions,
  overallScore,
  securityIssues,
  performanceIssues,
  isLoading,
  onClose,
}) => {
  // Group suggestions by type
  const groupedSuggestions = suggestions.reduce((acc, suggestion) => {
    if (!acc[suggestion.type]) {
      acc[suggestion.type] = [];
    }
    acc[suggestion.type].push(suggestion);
    return acc;
  }, {} as Record<FeedbackType, Feedback[]>);

  const getScoreColor = (score: number) => {
    if (score >= 80) return 'text-green-400';
    if (score >= 60) return 'text-yellow-400';
    return 'text-red-400';
  };

  const getScoreLabel = (score: number) => {
    if (score >= 80) return 'Excelente';
    if (score >= 60) return 'Bom';
    if (score >= 40) return 'Regular';
    return 'Precisa Melhorar';
  };

  if (isLoading) {
    return (
      <div className="bg-[#0d1117] border-l border-[#30363d] p-6 overflow-y-auto h-full">
        <div className="flex items-center justify-between mb-6">
          <h2 className="text-xl font-bold text-gray-100">Análise de Código IA</h2>
          <button
            onClick={onClose}
            className="text-gray-400 hover:text-gray-200"
            aria-label="Fechar feedback"
          >
            <Icons.XCircle className="w-5 h-5" />
          </button>
        </div>
        <div className="flex flex-col items-center justify-center py-12">
          <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-blue-500 mb-4"></div>
          <p className="text-gray-300">Analisando seu código...</p>
          <p className="text-sm text-gray-500 mt-2">Isso pode levar alguns segundos</p>
        </div>
      </div>
    );
  }

  return (
    <div className="bg-[#0d1117] border-l border-[#30363d] p-6 overflow-y-auto h-full">
      {/* Header */}
      <div className="flex items-center justify-between mb-6">
        <h2 className="text-xl font-bold text-gray-100">Análise de Código IA</h2>
        <button
          onClick={onClose}
          className="text-gray-400 hover:text-gray-200"
          aria-label="Fechar feedback"
        >
          <Icons.XCircle className="w-5 h-5" />
        </button>
      </div>

      {/* Overall Score */}
      <div className="mb-6 p-4 bg-[#161b22] rounded-lg border border-[#30363d]">
        <div className="flex items-center justify-between">
          <div>
            <p className="text-sm text-gray-400 mb-1">Pontuação Geral</p>
            <p className={`text-3xl font-bold ${getScoreColor(overallScore)}`}>
              {overallScore}/100
            </p>
            <p className="text-sm text-gray-400 mt-1">{getScoreLabel(overallScore)}</p>
          </div>
          <div className="text-right">
            <p className="text-sm text-gray-400">Problemas Encontrados</p>
            <p className="text-2xl font-bold text-gray-100">{suggestions.length}</p>
          </div>
        </div>
      </div>

      {/* Summary Badges */}
      {(securityIssues.length > 0 || performanceIssues.length > 0) && (
        <div className="mb-6 flex gap-2 flex-wrap">
          {securityIssues.length > 0 && (
            <span className="px-3 py-1 bg-red-900/30 text-red-400 text-sm font-medium rounded-full border border-red-900">
              {securityIssues.length} {securityIssues.length === 1 ? 'Problema' : 'Problemas'} de Segurança
            </span>
          )}
          {performanceIssues.length > 0 && (
            <span className="px-3 py-1 bg-yellow-900/30 text-yellow-400 text-sm font-medium rounded-full border border-yellow-900">
              {performanceIssues.length} {performanceIssues.length === 1 ? 'Problema' : 'Problemas'} de Performance
            </span>
          )}
        </div>
      )}

      {/* Suggestions by Category */}
      {suggestions.length === 0 ? (
        <div className="text-center py-12">
          <Icons.CheckCircle className="w-12 h-12 text-green-400 mx-auto mb-4" />
          <p className="text-gray-300 mb-2">Ótimo trabalho!</p>
          <p className="text-sm text-gray-500">Nenhum problema encontrado no seu código.</p>
        </div>
      ) : (
        <div className="space-y-6">
          {Object.entries(groupedSuggestions).map(([type, items]) => {
            const feedbackType = type as FeedbackType;
            const colors = feedbackTypeColors[feedbackType];
            const label = feedbackTypeLabels[feedbackType];

            return (
              <div key={type} className="space-y-3">
                <h3 className="text-lg font-semibold text-gray-100 flex items-center gap-2">
                  <span>{label}</span>
                  <span className="text-sm font-normal text-gray-500">({items.length})</span>
                </h3>
                {items.map((suggestion, index) => (
                  <div
                    key={index}
                    className={`p-4 rounded-lg border ${colors.border} ${colors.bg}`}
                  >
                    <div className="flex items-start gap-3">
                      <div className="flex-1">
                        <div className="flex items-center gap-2 mb-2">
                          <span className={`text-xs font-medium ${colors.text}`}>
                            Linha {suggestion.lineNumber}
                          </span>
                        </div>
                        <p className="text-sm text-gray-300 mb-3">{suggestion.message}</p>
                        {suggestion.codeExample && (
                          <div className="mt-2">
                            <p className="text-xs text-gray-400 mb-1 font-medium">
                              Código Sugerido:
                            </p>
                            <pre className="bg-[#0d1117] text-gray-100 p-3 rounded text-xs overflow-x-auto border border-[#30363d]">
                              <code>{suggestion.codeExample}</code>
                            </pre>
                          </div>
                        )}
                      </div>
                    </div>
                  </div>
                ))}
              </div>
            );
          })}
        </div>
      )}

      {/* Footer */}
      <div className="mt-6 pt-6 border-t border-[#30363d]">
        <p className="text-xs text-gray-500 text-center">
          Powered by Groq AI • Análise baseada em princípios SOLID, Clean Architecture e boas práticas ASP.NET Core
        </p>
      </div>
    </div>
  );
};
