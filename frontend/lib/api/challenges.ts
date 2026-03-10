import apiClient from '../api-client';
import {
  ChallengeListResponse,
  ChallengeDetailResponse,
  SubmitSolutionRequest,
  SubmitSolutionResponse,
  Difficulty,
} from '../types';

// MOCK DATA - Funciona sem backend
const mockChallenges = [
  {
    id: '1',
    title: 'Soma de Dois Números',
    description: 'Implemente uma função que recebe dois números inteiros e retorna a soma deles.',
    difficulty: Difficulty.Easy,
    xpReward: 50,
    supportsTimeAttack: true,
    timeAttackLimitSeconds: 300, // 5 minutos
    starterCode: `using System;

public class Solution 
{
    public static int Sum(int a, int b) 
    {
        // Implemente sua solução aqui
        return 0;
    }
    
    public static void Main() 
    {
        Console.WriteLine(Sum(2, 3)); // Deve imprimir 5
        Console.WriteLine(Sum(-1, 1)); // Deve imprimir 0
        Console.WriteLine(Sum(0, 0)); // Deve imprimir 0
    }
}`,
    testCases: [
      { input: '2, 3', expectedOutput: '5', isHidden: false },
      { input: '-1, 1', expectedOutput: '0', isHidden: false },
      { input: '0, 0', expectedOutput: '0', isHidden: false },
      { input: '100, 200', expectedOutput: '300', isHidden: true },
      { input: '-50, -25', expectedOutput: '-75', isHidden: true }
    ]
  },
  {
    id: '2',
    title: 'Número Par ou Ímpar',
    description: 'Crie uma função que determina se um número é par ou ímpar.',
    difficulty: Difficulty.Easy,
    xpReward: 75,
    supportsTimeAttack: true,
    timeAttackLimitSeconds: 240, // 4 minutos
    starterCode: `using System;

public class Solution 
{
    public static string IsEvenOrOdd(int number) 
    {
        // Retorne "Par" se o número for par, "Ímpar" se for ímpar
        return "";
    }
    
    public static void Main() 
    {
        Console.WriteLine(IsEvenOrOdd(4)); // Deve imprimir "Par"
        Console.WriteLine(IsEvenOrOdd(7)); // Deve imprimir "Ímpar"
        Console.WriteLine(IsEvenOrOdd(0)); // Deve imprimir "Par"
    }
}`,
    testCases: [
      { input: '4', expectedOutput: 'Par', isHidden: false },
      { input: '7', expectedOutput: 'Ímpar', isHidden: false },
      { input: '0', expectedOutput: 'Par', isHidden: false },
      { input: '1', expectedOutput: 'Ímpar', isHidden: true },
      { input: '100', expectedOutput: 'Par', isHidden: true }
    ]
  },
  {
    id: '3',
    title: 'Maior de Três Números',
    description: 'Implemente uma função que encontra o maior entre três números.',
    difficulty: Difficulty.Medium,
    xpReward: 100,
    supportsTimeAttack: true,
    timeAttackLimitSeconds: 420, // 7 minutos
    starterCode: `using System;

public class Solution 
{
    public static int FindMax(int a, int b, int c) 
    {
        // Encontre e retorne o maior dos três números
        return 0;
    }
    
    public static void Main() 
    {
        Console.WriteLine(FindMax(1, 2, 3)); // Deve imprimir 3
        Console.WriteLine(FindMax(5, 1, 4)); // Deve imprimir 5
        Console.WriteLine(FindMax(2, 2, 2)); // Deve imprimir 2
    }
}`,
    testCases: [
      { input: '1, 2, 3', expectedOutput: '3', isHidden: false },
      { input: '5, 1, 4', expectedOutput: '5', isHidden: false },
      { input: '2, 2, 2', expectedOutput: '2', isHidden: false },
      { input: '10, 5, 8', expectedOutput: '10', isHidden: true },
      { input: '-1, -5, -3', expectedOutput: '-1', isHidden: true }
    ]
  },
  {
    id: '4',
    title: 'Inverter String',
    description: 'Crie uma função que inverte uma string sem usar métodos prontos.',
    difficulty: Difficulty.Medium,
    xpReward: 125,
    supportsTimeAttack: true,
    timeAttackLimitSeconds: 600, // 10 minutos
    starterCode: `using System;

public class Solution 
{
    public static string ReverseString(string input) 
    {
        // Inverta a string sem usar métodos como Reverse()
        return "";
    }
    
    public static void Main() 
    {
        Console.WriteLine(ReverseString("hello")); // Deve imprimir "olleh"
        Console.WriteLine(ReverseString("world")); // Deve imprimir "dlrow"
        Console.WriteLine(ReverseString("a")); // Deve imprimir "a"
    }
}`,
    testCases: [
      { input: 'hello', expectedOutput: 'olleh', isHidden: false },
      { input: 'world', expectedOutput: 'dlrow', isHidden: false },
      { input: 'a', expectedOutput: 'a', isHidden: false },
      { input: 'programming', expectedOutput: 'gnimmargorP', isHidden: true },
      { input: '12345', expectedOutput: '54321', isHidden: true }
    ]
  },
  {
    id: '5',
    title: 'Sequência de Fibonacci',
    description: 'Implemente uma função que calcula o n-ésimo número da sequência de Fibonacci.',
    difficulty: Difficulty.Hard,
    xpReward: 200,
    supportsTimeAttack: true,
    timeAttackLimitSeconds: 900, // 15 minutos
    starterCode: `using System;

public class Solution 
{
    public static long Fibonacci(int n) 
    {
        // Calcule o n-ésimo número de Fibonacci
        // F(0) = 0, F(1) = 1, F(n) = F(n-1) + F(n-2)
        return 0;
    }
    
    public static void Main() 
    {
        Console.WriteLine(Fibonacci(0)); // Deve imprimir 0
        Console.WriteLine(Fibonacci(1)); // Deve imprimir 1
        Console.WriteLine(Fibonacci(10)); // Deve imprimir 55
    }
}`,
    testCases: [
      { input: '0', expectedOutput: '0', isHidden: false },
      { input: '1', expectedOutput: '1', isHidden: false },
      { input: '10', expectedOutput: '55', isHidden: false },
      { input: '15', expectedOutput: '610', isHidden: true },
      { input: '20', expectedOutput: '6765', isHidden: true }
    ]
  }
];

export const challengesApi = {
  /**
   * Get all challenges - MOCK VERSION
   */
  getAll: async (): Promise<ChallengeListResponse> => {
    // Simular delay de rede
    await new Promise(resolve => setTimeout(resolve, 300));
    
    return {
      challenges: mockChallenges.map(challenge => ({
        id: challenge.id,
        title: challenge.title,
        description: challenge.description,
        difficulty: challenge.difficulty,
        xpReward: challenge.xpReward,
        supportsTimeAttack: challenge.supportsTimeAttack,
        isCompleted: false,
        completionRate: Math.floor(Math.random() * 100),
        averageTime: Math.floor(Math.random() * 600) + 60
      })),
      total: mockChallenges.length,
      page: 1,
      pageSize: 10,
      totalPages: 1
    };
  },

  /**
   * Get challenge details by ID - MOCK VERSION
   */
  getById: async (challengeId: string): Promise<ChallengeDetailResponse> => {
    await new Promise(resolve => setTimeout(resolve, 200));
    
    const challenge = mockChallenges.find(c => c.id === challengeId);
    if (!challenge) {
      throw new Error('Challenge not found');
    }
    
    return {
      ...challenge,
      isCompleted: false,
      completionRate: Math.floor(Math.random() * 100),
      averageTime: Math.floor(Math.random() * 600) + 60,
      submissions: Math.floor(Math.random() * 1000) + 100
    };
  },

  /**
   * Submit a solution for a challenge - MOCK VERSION
   */
  submitSolution: async (
    challengeId: string,
    data: SubmitSolutionRequest
  ): Promise<SubmitSolutionResponse> => {
    await new Promise(resolve => setTimeout(resolve, 1000));
    
    const challenge = mockChallenges.find(c => c.id === challengeId);
    if (!challenge) {
      throw new Error('Challenge not found');
    }

    // Simular execução de testes
    const results = challenge.testCases.map((testCase, index) => {
      // Simular alguns testes passando e outros falhando baseado no código
      const passed = Math.random() > 0.3; // 70% de chance de passar
      
      return {
        testCaseIndex: index,
        passed,
        input: testCase.input,
        expectedOutput: testCase.expectedOutput,
        actualOutput: passed ? testCase.expectedOutput : 'Saída incorreta',
        executionTimeMs: Math.floor(Math.random() * 100) + 10
      };
    });

    const allTestsPassed = results.every(r => r.passed);
    const xpAwarded = allTestsPassed ? challenge.xpReward : Math.floor(challenge.xpReward * 0.3);
    
    let timeAttackBonusXP = 0;
    if (data.isTimeAttack && allTestsPassed && data.completionTimeSeconds) {
      // Bonus XP baseado no tempo
      const timeBonus = Math.max(0, (challenge.timeAttackLimitSeconds! - data.completionTimeSeconds) / 10);
      timeAttackBonusXP = Math.floor(timeBonus);
    }

    return {
      success: allTestsPassed,
      results,
      allTestsPassed,
      xpAwarded: xpAwarded + timeAttackBonusXP,
      timeAttackBonusXP: data.isTimeAttack ? timeAttackBonusXP : undefined,
      executionTimeMs: results.reduce((sum, r) => sum + r.executionTimeMs, 0)
    };
  },

  /**
   * Submit code review findings - MOCK VERSION
   */
  submitCodeReview: async (
    challengeId: string,
    data: {
      userId: string;
      identifiedIssues: Array<{
        lineNumber: number;
        description: string;
        severity: string;
      }>;
    }
  ): Promise<{
    success: boolean;
    totalExpectedBugs: number;
    correctlyIdentified: number;
    missedBugs: number;
    falsePositives: number;
    accuracyPercentage: number;
    xpAwarded: number;
    bugResults: Array<{
      lineNumber: number;
      expectedDescription: string;
      wasIdentified: boolean;
      userDescription?: string;
    }>;
  }> => {
    await new Promise(resolve => setTimeout(resolve, 800));
    
    // Simular análise de code review
    const totalExpectedBugs = 5;
    const correctlyIdentified = Math.min(data.identifiedIssues.length, Math.floor(Math.random() * totalExpectedBugs) + 1);
    const falsePositives = Math.max(0, data.identifiedIssues.length - correctlyIdentified);
    const missedBugs = totalExpectedBugs - correctlyIdentified;
    const accuracyPercentage = Math.floor((correctlyIdentified / totalExpectedBugs) * 100);
    const xpAwarded = correctlyIdentified * 20;

    const bugResults = Array.from({ length: totalExpectedBugs }, (_, i) => ({
      lineNumber: i + 5,
      expectedDescription: `Bug esperado na linha ${i + 5}`,
      wasIdentified: i < correctlyIdentified,
      userDescription: i < correctlyIdentified ? data.identifiedIssues[i]?.description : undefined
    }));

    return {
      success: accuracyPercentage >= 60,
      totalExpectedBugs,
      correctlyIdentified,
      missedBugs,
      falsePositives,
      accuracyPercentage,
      xpAwarded,
      bugResults
    };
  },
};
