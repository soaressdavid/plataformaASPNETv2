import apiClient from '../api-client';
import { LevelListResponse, CurriculumLevelDetail, CourseListResponse } from '../types';

// MOCK DATA - Funciona sem backend
const mockLevels = [
  {
    id: '0',
    title: 'Iniciante',
    description: 'Fundamentos da programação - Construa sua base sólida em desenvolvimento de software',
    order: 0,
    difficulty: 'Fácil',
    estimatedHours: 40,
    coursesCount: 4,
    completedCourses: 0,
    isUnlocked: true,
    progress: 0
  },
  {
    id: '1', 
    title: 'Intermediário',
    description: 'Desenvolvimento web moderno - Crie aplicações completas com tecnologias atuais',
    order: 1,
    difficulty: 'Médio',
    estimatedHours: 60,
    coursesCount: 4,
    completedCourses: 0,
    isUnlocked: true,
    progress: 0
  },
  {
    id: '2',
    title: 'Avançado',
    description: 'Arquitetura e cloud computing - Domine sistemas distribuídos e escaláveis',
    order: 2,
    difficulty: 'Difícil',
    estimatedHours: 80,
    coursesCount: 4,
    completedCourses: 0,
    isUnlocked: false,
    progress: 0
  }
];

export const levelsApi = {
  /**
   * Get all curriculum levels - MOCK VERSION
   */
  getAll: async (): Promise<LevelListResponse> => {
    // Simular delay de rede
    await new Promise(resolve => setTimeout(resolve, 200));
    
    return {
      levels: mockLevels,
      total: mockLevels.length
    };
  },

  /**
   * Get a specific level with its courses - MOCK VERSION
   */
  getById: async (id: string): Promise<CurriculumLevelDetail> => {
    await new Promise(resolve => setTimeout(resolve, 200));
    
    const level = mockLevels.find(l => l.id === id);
    if (!level) {
      throw new Error('Level not found');
    }

    // Conteúdo detalhado específico para cada nível
    let detailedContent = '';
    let prerequisites: string[] = [];
    let learningObjectives: string[] = [];

    switch (id) {
      case '0': // Iniciante
        detailedContent = `
## Nível Iniciante - Fundamentos da Programação

### Visão Geral
Este nível é projetado para pessoas que estão começando sua jornada na programação. Você aprenderá os conceitos fundamentais que são a base de qualquer carreira em desenvolvimento de software.

### O que você vai conquistar
Ao completar este nível, você terá uma base sólida em programação e estará preparado para avançar para conceitos mais complexos. Você será capaz de:

- **Pensar computacionalmente**: Resolver problemas de forma estruturada
- **Programar em C#**: Criar aplicações console funcionais
- **Trabalhar com dados**: Usar SQL para consultar e manipular informações
- **Colaborar em projetos**: Usar Git para controle de versão

### Metodologia de Ensino
- **Aprendizado gradual**: Cada conceito é introduzido de forma progressiva
- **Prática constante**: Exercícios práticos em cada aula
- **Projetos reais**: Aplicações que você pode mostrar no seu portfólio
- **Feedback imediato**: Executores integrados para testar seu código

### Perfil do Estudante
Este nível é ideal para:
- Iniciantes completos em programação
- Estudantes de cursos técnicos ou superiores
- Profissionais de outras áreas querendo migrar para TI
- Pessoas interessadas em desenvolvimento de software

### Tempo de Dedicação
- **Duração estimada**: 2-3 meses (estudando 3-4 horas por semana)
- **Carga horária total**: 40 horas de conteúdo
- **Flexibilidade**: Estude no seu próprio ritmo

### Certificação
Ao completar todos os cursos deste nível, você receberá um certificado de conclusão que comprova seu domínio dos fundamentos da programação.
        `;
        prerequisites = [];
        learningObjectives = [
          'Dominar lógica de programação e algoritmos',
          'Programar fluentemente em C#',
          'Criar e consultar bancos de dados com SQL',
          'Usar Git para controle de versão',
          'Resolver problemas computacionais de forma estruturada'
        ];
        break;

      case '1': // Intermediário
        detailedContent = `
## Nível Intermediário - Desenvolvimento Web Moderno

### Visão Geral
Neste nível, você evoluirá de programador iniciante para desenvolvedor web completo. Aprenderá a criar aplicações web modernas, APIs robustas e interfaces de usuário atrativas.

### O que você vai conquistar
Ao completar este nível, você será um desenvolvedor full-stack capaz de:

- **Criar APIs profissionais**: Desenvolver serviços web escaláveis com ASP.NET Core
- **Trabalhar com bancos de dados**: Usar Entity Framework para acesso a dados
- **Desenvolver interfaces modernas**: Criar UIs responsivas com React
- **Garantir qualidade**: Implementar testes automatizados

### Tecnologias Dominadas
- **Backend**: ASP.NET Core Web API, Entity Framework Core
- **Frontend**: React, TypeScript, HTML5, CSS3
- **Banco de dados**: SQL Server, PostgreSQL, relacionamentos complexos
- **Qualidade**: xUnit, Moq, testes unitários e de integração

### Projetos do Mundo Real
- **E-commerce completo**: Sistema de vendas online
- **Blog pessoal**: Plataforma de conteúdo com CMS
- **API de gestão**: Sistema de gerenciamento empresarial
- **Dashboard analytics**: Interface para visualização de dados

### Mercado de Trabalho
Com as habilidades deste nível, você estará qualificado para posições como:
- Desenvolvedor Full-Stack Júnior
- Desenvolvedor Backend .NET
- Desenvolvedor Frontend React
- Analista de Sistemas

### Preparação para o Próximo Nível
Este nível prepara você para conceitos avançados como:
- Arquitetura de microserviços
- Cloud computing
- DevOps e CI/CD
- Padrões arquiteturais enterprise
        `;
        prerequisites = ['Conclusão do Nível Iniciante'];
        learningObjectives = [
          'Desenvolver APIs REST profissionais',
          'Criar interfaces de usuário modernas e responsivas',
          'Implementar acesso a dados com ORM',
          'Aplicar testes automatizados em projetos',
          'Integrar frontend e backend em aplicações completas'
        ];
        break;

      case '2': // Avançado
        detailedContent = `
## Nível Avançado - Arquitetura e Cloud Computing

### Visão Geral
Este é o nível mais desafiador, projetado para formar arquitetos de software e especialistas em cloud. Você aprenderá a projetar e implementar sistemas distribuídos, escaláveis e resilientes.

### O que você vai conquistar
Ao completar este nível, você será um profissional sênior capaz de:

- **Arquitetar sistemas complexos**: Projetar aplicações enterprise escaláveis
- **Implementar microserviços**: Criar arquiteturas distribuídas modernas
- **Dominar cloud computing**: Usar Azure para soluções na nuvem
- **Liderar equipes técnicas**: Aplicar práticas DevOps e cultura de qualidade

### Competências Avançadas
- **Arquitetura**: Clean Architecture, DDD, SOLID, padrões enterprise
- **Microserviços**: Docker, Kubernetes, service mesh, observabilidade
- **Cloud**: Azure services, serverless, infrastructure as code
- **DevOps**: CI/CD, automação, monitoramento, cultura DevOps

### Projetos Enterprise
- **Plataforma de microserviços**: Sistema distribuído completo
- **Solução cloud-native**: Aplicação serverless no Azure
- **Pipeline DevOps**: Automação completa de deploy
- **Sistema de alta disponibilidade**: Arquitetura resiliente e escalável

### Certificações Preparadas
Este nível prepara você para certificações importantes:
- **Microsoft Azure**: AZ-204 (Developer), AZ-400 (DevOps)
- **Kubernetes**: CKA (Certified Kubernetes Administrator)
- **Docker**: Docker Certified Associate

### Oportunidades de Carreira
Com essas habilidades, você estará qualificado para:
- Arquiteto de Software Sênior
- Tech Lead / Engineering Manager
- DevOps Engineer / SRE
- Cloud Solutions Architect
- Consultor Técnico Especialista

### Impacto no Mercado
Profissionais com essas competências são altamente valorizados e podem:
- Liderar transformações digitais
- Arquitetar soluções para grandes empresas
- Trabalhar remotamente para empresas globais
- Empreender com soluções tecnológicas inovadoras
        `;
        prerequisites = ['Conclusão do Nível Intermediário', 'Experiência prática em projetos'];
        learningObjectives = [
          'Projetar arquiteturas de software escaláveis e maintíveis',
          'Implementar e orquestrar microserviços em produção',
          'Dominar serviços de cloud computing (Azure)',
          'Aplicar práticas DevOps e automação de pipelines',
          'Liderar equipes técnicas e tomar decisões arquiteturais'
        ];
        break;

      default:
        detailedContent = `Este é o nível ${level.title}. Aqui você aprenderá conceitos ${level.difficulty.toLowerCase()} de programação.`;
        prerequisites = level.order > 0 ? [`Nível ${level.order - 1}`] : [];
        learningObjectives = [
          'Dominar conceitos fundamentais',
          'Aplicar boas práticas',
          'Resolver problemas complexos'
        ];
    }
    
    return {
      ...level,
      content: detailedContent,
      prerequisites,
      learningObjectives,
      courses: [] // Será preenchido pela API de courses
    };
  },

  /**
   * Get all courses for a specific level - MOCK VERSION
   */
  getCourses: async (id: string): Promise<CourseListResponse> => {
    await new Promise(resolve => setTimeout(resolve, 200));
    
    // Importar dados mock de courses
    const mockCourses = [
      {
        id: '1',
        title: 'C# Básico',
        description: 'Aprenda os fundamentos da programação C#',
        level: 0,
        levelTitle: 'Iniciante',
        levelId: '0',
        difficulty: 'Fácil',
        duration: '4 semanas',
        estimatedMinutes: 240,
        isCompleted: false,
        progress: 0,
        lessonsCount: 10,
        enrolledCount: 1250
      },
      {
        id: '2', 
        title: 'SQL Fundamentals',
        description: 'Domine consultas SQL e banco de dados',
        level: 0,
        levelTitle: 'Iniciante',
        levelId: '0',
        difficulty: 'Fácil',
        duration: '3 semanas',
        estimatedMinutes: 180,
        isCompleted: false,
        progress: 0,
        lessonsCount: 8,
        enrolledCount: 980
      },
      {
        id: '3',
        title: 'ASP.NET Core Web API',
        description: 'Construa APIs robustas com ASP.NET Core',
        level: 1,
        levelTitle: 'Intermediário',
        levelId: '1',
        difficulty: 'Médio',
        duration: '6 semanas',
        estimatedMinutes: 360,
        isCompleted: false,
        progress: 0,
        lessonsCount: 15,
        enrolledCount: 750
      }
    ];
    
    const levelCourses = mockCourses.filter(course => course.levelId === id);
    
    return {
      courses: levelCourses,
      total: levelCourses.length,
      page: 1,
      pageSize: 10,
      totalPages: 1
    };
  },
};
