import apiClient from '../api-client';
import {
  ProjectSummary,
  ProjectDetailResponse,
} from '../types';

export interface ProjectListResponse {
  projects: ProjectSummary[];
}

export interface ValidateStepRequest {
  userId: string;
  code: string;
}

export interface ValidateStepResponse {
  success: boolean;
  message: string;
  nextStepUnlocked: boolean;
}

// MOCK DATA - Funciona sem backend
const mockProjects = [
  {
    id: '1',
    title: 'Sistema de E-commerce',
    description: 'Desenvolva um sistema completo de e-commerce com carrinho de compras, pagamentos e gestão de produtos.',
    difficulty: 'Médio',
    estimatedHours: 40,
    currentStep: 1,
    totalSteps: 8,
    isCompleted: false,
    progress: 12.5,
    technologies: ['C#', 'ASP.NET Core', 'Entity Framework', 'SQL Server', 'React'],
    steps: [
      {
        stepNumber: 1,
        title: 'Configuração do Projeto',
        description: 'Configure o projeto ASP.NET Core e Entity Framework',
        isCompleted: false,
        isUnlocked: true,
        estimatedMinutes: 60
      },
      {
        stepNumber: 2,
        title: 'Modelagem de Dados',
        description: 'Crie as entidades Product, Category, User e Order',
        isCompleted: false,
        isUnlocked: false,
        estimatedMinutes: 90
      },
      {
        stepNumber: 3,
        title: 'API de Produtos',
        description: 'Implemente CRUD completo para produtos',
        isCompleted: false,
        isUnlocked: false,
        estimatedMinutes: 120
      },
      {
        stepNumber: 4,
        title: 'Sistema de Autenticação',
        description: 'Adicione JWT authentication e autorização',
        isCompleted: false,
        isUnlocked: false,
        estimatedMinutes: 150
      },
      {
        stepNumber: 5,
        title: 'Carrinho de Compras',
        description: 'Implemente funcionalidade de carrinho',
        isCompleted: false,
        isUnlocked: false,
        estimatedMinutes: 180
      },
      {
        stepNumber: 6,
        title: 'Sistema de Pedidos',
        description: 'Crie o fluxo completo de pedidos',
        isCompleted: false,
        isUnlocked: false,
        estimatedMinutes: 200
      },
      {
        stepNumber: 7,
        title: 'Frontend React',
        description: 'Desenvolva a interface do usuário',
        isCompleted: false,
        isUnlocked: false,
        estimatedMinutes: 300
      },
      {
        stepNumber: 8,
        title: 'Deploy e Testes',
        description: 'Faça o deploy e testes finais',
        isCompleted: false,
        isUnlocked: false,
        estimatedMinutes: 120
      }
    ]
  },
  {
    id: '2',
    title: 'API de Blog',
    description: 'Construa uma API REST completa para um sistema de blog com posts, comentários e categorias.',
    difficulty: 'Fácil',
    estimatedHours: 20,
    currentStep: 1,
    totalSteps: 5,
    isCompleted: false,
    progress: 0,
    technologies: ['C#', 'ASP.NET Core', 'Entity Framework', 'SQLite'],
    steps: [
      {
        stepNumber: 1,
        title: 'Setup do Projeto',
        description: 'Configure o projeto e banco de dados',
        isCompleted: false,
        isUnlocked: true,
        estimatedMinutes: 45
      },
      {
        stepNumber: 2,
        title: 'Modelo de Dados',
        description: 'Crie as entidades Post, Comment, Category',
        isCompleted: false,
        isUnlocked: false,
        estimatedMinutes: 60
      },
      {
        stepNumber: 3,
        title: 'Controllers e Endpoints',
        description: 'Implemente os controllers da API',
        isCompleted: false,
        isUnlocked: false,
        estimatedMinutes: 90
      },
      {
        stepNumber: 4,
        title: 'Validação e Filtros',
        description: 'Adicione validação e filtros de busca',
        isCompleted: false,
        isUnlocked: false,
        estimatedMinutes: 75
      },
      {
        stepNumber: 5,
        title: 'Documentação e Testes',
        description: 'Swagger e testes unitários',
        isCompleted: false,
        isUnlocked: false,
        estimatedMinutes: 90
      }
    ]
  },
  {
    id: '3',
    title: 'Microserviço de Notificações',
    description: 'Desenvolva um microserviço para gerenciar notificações em tempo real com SignalR.',
    difficulty: 'Difícil',
    estimatedHours: 60,
    currentStep: 1,
    totalSteps: 10,
    isCompleted: false,
    progress: 0,
    technologies: ['C#', 'ASP.NET Core', 'SignalR', 'RabbitMQ', 'Docker', 'Redis'],
    steps: [
      {
        stepNumber: 1,
        title: 'Arquitetura do Microserviço',
        description: 'Defina a arquitetura e estrutura do projeto',
        isCompleted: false,
        isUnlocked: true,
        estimatedMinutes: 90
      },
      {
        stepNumber: 2,
        title: 'Setup SignalR',
        description: 'Configure SignalR para comunicação em tempo real',
        isCompleted: false,
        isUnlocked: false,
        estimatedMinutes: 120
      }
      // ... mais steps
    ]
  }
];

export const projectsApi = {
  /**
   * Get all projects - MOCK VERSION
   */
  getAll: async (): Promise<ProjectListResponse> => {
    await new Promise(resolve => setTimeout(resolve, 300));
    
    return {
      projects: mockProjects.map(project => ({
        id: project.id,
        title: project.title,
        description: project.description,
        difficulty: project.difficulty,
        estimatedHours: project.estimatedHours,
        currentStep: project.currentStep,
        totalSteps: project.totalSteps,
        isCompleted: project.isCompleted,
        progress: project.progress,
        technologies: project.technologies
      }))
    };
  },

  /**
   * Get project details by ID - MOCK VERSION
   */
  getById: async (projectId: string): Promise<ProjectDetailResponse> => {
    await new Promise(resolve => setTimeout(resolve, 200));
    
    const project = mockProjects.find(p => p.id === projectId);
    if (!project) {
      throw new Error('Project not found');
    }
    
    return {
      ...project,
      content: `## ${project.title}

### Visão Geral
${project.description}

### Tecnologias Utilizadas
${project.technologies.map(tech => `- ${tech}`).join('\n')}

### Objetivos de Aprendizagem
- Aplicar conceitos de desenvolvimento full-stack
- Implementar boas práticas de arquitetura
- Trabalhar com tecnologias modernas
- Desenvolver um projeto do mundo real

### Metodologia
Este projeto é dividido em ${project.totalSteps} etapas progressivas. Cada etapa deve ser completada antes de avançar para a próxima.

### Pré-requisitos
- Conhecimento básico de C# e ASP.NET Core
- Familiaridade com Entity Framework
- Conceitos básicos de banco de dados

Comece pela primeira etapa e siga as instruções detalhadas para cada passo.`,
      learningObjectives: [
        'Desenvolver aplicação completa do zero',
        'Aplicar padrões de arquitetura modernos',
        'Implementar boas práticas de desenvolvimento',
        'Trabalhar com tecnologias atuais do mercado'
      ],
      prerequisites: ['ASP.NET Core básico', 'Entity Framework', 'C# intermediário']
    };
  },

  /**
   * Validate a project step - MOCK VERSION
   */
  validateStep: async (
    projectId: string,
    stepNumber: number,
    data: ValidateStepRequest
  ): Promise<ValidateStepResponse> => {
    await new Promise(resolve => setTimeout(resolve, 1000));
    
    // Simular validação
    const success = Math.random() > 0.3; // 70% chance de sucesso
    
    return {
      success,
      message: success 
        ? `✅ Etapa ${stepNumber} concluída com sucesso! Você pode avançar para a próxima etapa.`
        : `❌ Código não atende aos requisitos da etapa ${stepNumber}. Revise e tente novamente.`,
      nextStepUnlocked: success
    };
  },
};
