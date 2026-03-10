import apiClient from '../api-client';
import {
  CourseListResponse,
  CourseDetail,
  LessonListResponse,
  LessonDetail,
  CompleteLessonRequest,
  CompleteLessonResponse,
} from '../types';

// MOCK DATA COMPLETO - VERSÃO FACULDADE PROFISSIONAL
const mockCourses = [
  // NÍVEL INICIANTE (0) - Fundamentos
  {
    id: '1',
    title: 'Fundamentos de Programação C#',
    description: 'Aprenda os conceitos básicos da programação orientada a objetos com C#. Ideal para quem está começando na programação.',
    level: 0,
    levelTitle: 'Iniciante',
    levelId: '0',
    difficulty: 'Fácil',
    duration: '4 semanas',
    estimatedMinutes: 240,
    isCompleted: false,
    progress: 0,
    lessonsCount: 12,
    enrolledCount: 1250
  },
  {
    id: '2', 
    title: 'Banco de Dados com SQL',
    description: 'Domine consultas SQL, relacionamentos e modelagem de dados. Essencial para qualquer desenvolvedor.',
    level: 0,
    levelTitle: 'Iniciante',
    levelId: '0',
    difficulty: 'Fácil',
    duration: '3 semanas',
    estimatedMinutes: 180,
    isCompleted: false,
    progress: 0,
    lessonsCount: 10,
    enrolledCount: 980
  },
  {
    id: '3',
    title: 'Lógica de Programação',
    description: 'Desenvolva o raciocínio lógico e aprenda algoritmos fundamentais para resolver problemas computacionais.',
    level: 0,
    levelTitle: 'Iniciante',
    levelId: '0',
    difficulty: 'Fácil',
    duration: '2 semanas',
    estimatedMinutes: 120,
    isCompleted: false,
    progress: 0,
    lessonsCount: 8,
    enrolledCount: 1500
  },
  {
    id: '4',
    title: 'Git e Controle de Versão',
    description: 'Aprenda a usar Git para controlar versões e colaborar em projetos de desenvolvimento.',
    level: 0,
    levelTitle: 'Iniciante',
    levelId: '0',
    difficulty: 'Fácil',
    duration: '1 semana',
    estimatedMinutes: 60,
    isCompleted: false,
    progress: 0,
    lessonsCount: 5,
    enrolledCount: 800
  },

  // NÍVEL INTERMEDIÁRIO (1) - Desenvolvimento Web
  {
    id: '5',
    title: 'ASP.NET Core Web API',
    description: 'Construa APIs robustas e escaláveis com ASP.NET Core. Aprenda sobre controllers, middleware e autenticação.',
    level: 1,
    levelTitle: 'Intermediário',
    levelId: '1',
    difficulty: 'Médio',
    duration: '6 semanas',
    estimatedMinutes: 360,
    isCompleted: false,
    progress: 0,
    lessonsCount: 18,
    enrolledCount: 750
  },
  {
    id: '6',
    title: 'Entity Framework Core',
    description: 'Domine o ORM mais usado no .NET. Aprenda migrations, relacionamentos e otimização de performance.',
    level: 1,
    levelTitle: 'Intermediário',
    levelId: '1',
    difficulty: 'Médio',
    duration: '4 semanas',
    estimatedMinutes: 240,
    isCompleted: false,
    progress: 0,
    lessonsCount: 15,
    enrolledCount: 650
  },
  {
    id: '7',
    title: 'Frontend com React',
    description: 'Crie interfaces modernas e responsivas com React e TypeScript. Aprenda hooks, context e state management.',
    level: 1,
    levelTitle: 'Intermediário',
    levelId: '1',
    difficulty: 'Médio',
    duration: '5 semanas',
    estimatedMinutes: 300,
    isCompleted: false,
    progress: 0,
    lessonsCount: 16,
    enrolledCount: 900
  },
  {
    id: '8',
    title: 'Testes Automatizados',
    description: 'Aprenda TDD, testes unitários e de integração com xUnit e Moq. Garanta qualidade no seu código.',
    level: 1,
    levelTitle: 'Intermediário',
    levelId: '1',
    difficulty: 'Médio',
    duration: '3 semanas',
    estimatedMinutes: 180,
    isCompleted: false,
    progress: 0,
    lessonsCount: 12,
    enrolledCount: 450
  },

  // NÍVEL AVANÇADO (2) - Arquitetura e Cloud
  {
    id: '9',
    title: 'Microserviços com .NET',
    description: 'Arquitetura de microserviços, Docker, Kubernetes e comunicação entre serviços. Aprenda padrões modernos.',
    level: 2,
    levelTitle: 'Avançado',
    levelId: '2',
    difficulty: 'Difícil',
    duration: '8 semanas',
    estimatedMinutes: 480,
    isCompleted: false,
    progress: 0,
    lessonsCount: 24,
    enrolledCount: 300
  },
  {
    id: '10',
    title: 'Cloud Computing com Azure',
    description: 'Deploy, monitoramento e escalabilidade na nuvem Microsoft Azure. Domine serviços PaaS e IaaS.',
    level: 2,
    levelTitle: 'Avançado',
    levelId: '2',
    difficulty: 'Difícil',
    duration: '6 semanas',
    estimatedMinutes: 360,
    isCompleted: false,
    progress: 0,
    lessonsCount: 20,
    enrolledCount: 250
  },
  {
    id: '11',
    title: 'DevOps e CI/CD',
    description: 'Pipelines automatizados, GitHub Actions, monitoramento e deployment contínuo. Acelere entregas.',
    level: 2,
    levelTitle: 'Avançado',
    levelId: '2',
    difficulty: 'Difícil',
    duration: '5 semanas',
    estimatedMinutes: 300,
    isCompleted: false,
    progress: 0,
    lessonsCount: 18,
    enrolledCount: 200
  },
  {
    id: '12',
    title: 'Arquitetura de Software',
    description: 'Padrões arquiteturais, Clean Architecture, DDD e boas práticas. Projete sistemas escaláveis.',
    level: 2,
    levelTitle: 'Avançado',
    levelId: '2',
    difficulty: 'Difícil',
    duration: '7 semanas',
    estimatedMinutes: 420,
    isCompleted: false,
    progress: 0,
    lessonsCount: 22,
    enrolledCount: 180
  }
];

const mockLessons = [
  // Curso 1: Fundamentos C# (12 aulas)
  { id: '1', courseId: '1', title: 'Introdução ao C# e .NET', content: 'História e conceitos fundamentais do C# e .NET Framework', order: 1, difficulty: 'Fácil', duration: '30 min', estimatedMinutes: 30, isCompleted: false },
  { id: '2', courseId: '1', title: 'Variáveis e Tipos de Dados', content: 'int, string, bool, var e conversões de tipos', order: 2, difficulty: 'Fácil', duration: '45 min', estimatedMinutes: 45, isCompleted: false },
  { id: '3', courseId: '1', title: 'Estruturas de Controle', content: 'if, else, switch, loops (for, while, foreach)', order: 3, difficulty: 'Fácil', duration: '50 min', estimatedMinutes: 50, isCompleted: false },
  { id: '4', courseId: '1', title: 'Métodos e Funções', content: 'Criação e uso de métodos, parâmetros e retorno', order: 4, difficulty: 'Fácil', duration: '40 min', estimatedMinutes: 40, isCompleted: false },
  { id: '5', courseId: '1', title: 'Classes e Objetos', content: 'POO básica em C#, construtores e propriedades', order: 5, difficulty: 'Médio', duration: '60 min', estimatedMinutes: 60, isCompleted: false },
  { id: '6', courseId: '1', title: 'Herança e Polimorfismo', content: 'Conceitos avançados de POO', order: 6, difficulty: 'Médio', duration: '55 min', estimatedMinutes: 55, isCompleted: false },
  { id: '7', courseId: '1', title: 'Interfaces e Abstrações', content: 'Contratos e classes abstratas', order: 7, difficulty: 'Médio', duration: '50 min', estimatedMinutes: 50, isCompleted: false },
  { id: '8', courseId: '1', title: 'Tratamento de Exceções', content: 'try-catch, finally e exceções customizadas', order: 8, difficulty: 'Médio', duration: '45 min', estimatedMinutes: 45, isCompleted: false },
  { id: '9', courseId: '1', title: 'Collections e Generics', content: 'List, Dictionary, Array e tipos genéricos', order: 9, difficulty: 'Médio', duration: '50 min', estimatedMinutes: 50, isCompleted: false },
  { id: '10', courseId: '1', title: 'LINQ e Lambda', content: 'Consultas em coleções com LINQ', order: 10, difficulty: 'Difícil', duration: '60 min', estimatedMinutes: 60, isCompleted: false },
  { id: '11', courseId: '1', title: 'Async/Await', content: 'Programação assíncrona em C#', order: 11, difficulty: 'Difícil', duration: '55 min', estimatedMinutes: 55, isCompleted: false },
  { id: '12', courseId: '1', title: 'Projeto Final C#', content: 'Aplicação console completa', order: 12, difficulty: 'Difícil', duration: '90 min', estimatedMinutes: 90, isCompleted: false },

  // Curso 2: SQL (10 aulas)
  { id: '13', courseId: '2', title: 'Introdução ao SQL', content: 'Conceitos de banco de dados relacionais', order: 1, difficulty: 'Fácil', duration: '30 min', estimatedMinutes: 30, isCompleted: false },
  { id: '14', courseId: '2', title: 'SELECT e Consultas Básicas', content: 'WHERE, ORDER BY, LIMIT e filtros', order: 2, difficulty: 'Fácil', duration: '45 min', estimatedMinutes: 45, isCompleted: false },
  { id: '15', courseId: '2', title: 'JOINs e Relacionamentos', content: 'INNER, LEFT, RIGHT JOIN entre tabelas', order: 3, difficulty: 'Médio', duration: '60 min', estimatedMinutes: 60, isCompleted: false },
  { id: '16', courseId: '2', title: 'Funções de Agregação', content: 'COUNT, SUM, AVG, GROUP BY, HAVING', order: 4, difficulty: 'Médio', duration: '50 min', estimatedMinutes: 50, isCompleted: false },
  { id: '17', courseId: '2', title: 'INSERT, UPDATE, DELETE', content: 'Manipulação de dados (DML)', order: 5, difficulty: 'Fácil', duration: '40 min', estimatedMinutes: 40, isCompleted: false },
  { id: '18', courseId: '2', title: 'CREATE TABLE e DDL', content: 'Definição de estruturas de dados', order: 6, difficulty: 'Médio', duration: '45 min', estimatedMinutes: 45, isCompleted: false },
  { id: '19', courseId: '2', title: 'Índices e Performance', content: 'Otimização de consultas SQL', order: 7, difficulty: 'Difícil', duration: '55 min', estimatedMinutes: 55, isCompleted: false },
  { id: '20', courseId: '2', title: 'Subconsultas e CTEs', content: 'Consultas aninhadas e Common Table Expressions', order: 8, difficulty: 'Difícil', duration: '60 min', estimatedMinutes: 60, isCompleted: false },
  { id: '21', courseId: '2', title: 'Stored Procedures', content: 'Procedimentos armazenados e funções', order: 9, difficulty: 'Difícil', duration: '50 min', estimatedMinutes: 50, isCompleted: false },
  { id: '22', courseId: '2', title: 'Projeto Final SQL', content: 'Modelagem completa de banco de dados', order: 10, difficulty: 'Difícil', duration: '90 min', estimatedMinutes: 90, isCompleted: false },

  // Curso 3: Lógica de Programação (8 aulas)
  { id: '23', courseId: '3', title: 'Algoritmos e Fluxogramas', content: 'Conceitos básicos de algoritmos', order: 1, difficulty: 'Fácil', duration: '30 min', estimatedMinutes: 30, isCompleted: false },
  { id: '24', courseId: '3', title: 'Estruturas Sequenciais', content: 'Entrada, processamento e saída', order: 2, difficulty: 'Fácil', duration: '25 min', estimatedMinutes: 25, isCompleted: false },
  { id: '25', courseId: '3', title: 'Estruturas Condicionais', content: 'Tomada de decisões em algoritmos', order: 3, difficulty: 'Fácil', duration: '35 min', estimatedMinutes: 35, isCompleted: false },
  { id: '26', courseId: '3', title: 'Estruturas de Repetição', content: 'Loops e iterações', order: 4, difficulty: 'Médio', duration: '40 min', estimatedMinutes: 40, isCompleted: false },
  { id: '27', courseId: '3', title: 'Arrays e Matrizes', content: 'Estruturas de dados básicas', order: 5, difficulty: 'Médio', duration: '45 min', estimatedMinutes: 45, isCompleted: false },
  { id: '28', courseId: '3', title: 'Algoritmos de Ordenação', content: 'Bubble Sort, Selection Sort', order: 6, difficulty: 'Difícil', duration: '50 min', estimatedMinutes: 50, isCompleted: false },
  { id: '29', courseId: '3', title: 'Algoritmos de Busca', content: 'Busca linear e binária', order: 7, difficulty: 'Difícil', duration: '45 min', estimatedMinutes: 45, isCompleted: false },
  { id: '30', courseId: '3', title: 'Projeto Final Lógica', content: 'Sistema completo com algoritmos', order: 8, difficulty: 'Difícil', duration: '60 min', estimatedMinutes: 60, isCompleted: false },

  // Curso 4: Git (5 aulas)
  { id: '31', courseId: '4', title: 'Introdução ao Git', content: 'Conceitos de controle de versão', order: 1, difficulty: 'Fácil', duration: '20 min', estimatedMinutes: 20, isCompleted: false },
  { id: '32', courseId: '4', title: 'Comandos Básicos', content: 'init, add, commit, status', order: 2, difficulty: 'Fácil', duration: '25 min', estimatedMinutes: 25, isCompleted: false },
  { id: '33', courseId: '4', title: 'Branches e Merge', content: 'Trabalhando com ramificações', order: 3, difficulty: 'Médio', duration: '30 min', estimatedMinutes: 30, isCompleted: false },
  { id: '34', courseId: '4', title: 'GitHub e Repositórios Remotos', content: 'push, pull, clone, fork', order: 4, difficulty: 'Médio', duration: '35 min', estimatedMinutes: 35, isCompleted: false },
  { id: '35', courseId: '4', title: 'Colaboração e Pull Requests', content: 'Trabalhando em equipe', order: 5, difficulty: 'Médio', duration: '40 min', estimatedMinutes: 40, isCompleted: false },

  // Curso 5: ASP.NET Core Web API (18 aulas)
  { id: '36', courseId: '5', title: 'Criando sua Primeira API', content: 'Controllers e Actions básicos', order: 1, difficulty: 'Médio', duration: '45 min', estimatedMinutes: 45, isCompleted: false },
  { id: '37', courseId: '5', title: 'Roteamento e Parâmetros', content: 'Route attributes e model binding', order: 2, difficulty: 'Médio', duration: '50 min', estimatedMinutes: 50, isCompleted: false },
  { id: '38', courseId: '5', title: 'Middleware e Pipeline', content: 'Request pipeline customizado', order: 3, difficulty: 'Difícil', duration: '60 min', estimatedMinutes: 60, isCompleted: false },
  { id: '39', courseId: '5', title: 'Dependency Injection', content: 'Injeção de dependência no .NET', order: 4, difficulty: 'Difícil', duration: '55 min', estimatedMinutes: 55, isCompleted: false },
  { id: '40', courseId: '5', title: 'Entity Framework Integration', content: 'Conectando com banco de dados', order: 5, difficulty: 'Médio', duration: '60 min', estimatedMinutes: 60, isCompleted: false },
  { id: '41', courseId: '5', title: 'Autenticação JWT', content: 'Implementando segurança', order: 6, difficulty: 'Difícil', duration: '70 min', estimatedMinutes: 70, isCompleted: false },
  { id: '42', courseId: '5', title: 'Autorização e Roles', content: 'Controle de acesso', order: 7, difficulty: 'Difícil', duration: '60 min', estimatedMinutes: 60, isCompleted: false },
  { id: '43', courseId: '5', title: 'Validação de Dados', content: 'Data Annotations e FluentValidation', order: 8, difficulty: 'Médio', duration: '45 min', estimatedMinutes: 45, isCompleted: false },
  { id: '44', courseId: '5', title: 'Tratamento de Erros', content: 'Exception handling e logging', order: 9, difficulty: 'Médio', duration: '50 min', estimatedMinutes: 50, isCompleted: false },
  { id: '45', courseId: '5', title: 'Swagger e Documentação', content: 'Documentando APIs', order: 10, difficulty: 'Fácil', duration: '30 min', estimatedMinutes: 30, isCompleted: false },
  { id: '46', courseId: '5', title: 'Versionamento de API', content: 'Gerenciando versões', order: 11, difficulty: 'Médio', duration: '40 min', estimatedMinutes: 40, isCompleted: false },
  { id: '47', courseId: '5', title: 'CORS e Segurança', content: 'Cross-Origin Resource Sharing', order: 12, difficulty: 'Médio', duration: '35 min', estimatedMinutes: 35, isCompleted: false },
  { id: '48', courseId: '5', title: 'Caching e Performance', content: 'Otimização de APIs', order: 13, difficulty: 'Difícil', duration: '55 min', estimatedMinutes: 55, isCompleted: false },
  { id: '49', courseId: '5', title: 'Rate Limiting', content: 'Controle de taxa de requisições', order: 14, difficulty: 'Difícil', duration: '45 min', estimatedMinutes: 45, isCompleted: false },
  { id: '50', courseId: '5', title: 'Health Checks', content: 'Monitoramento de saúde', order: 15, difficulty: 'Médio', duration: '40 min', estimatedMinutes: 40, isCompleted: false },
  { id: '51', courseId: '5', title: 'Background Services', content: 'Tarefas em segundo plano', order: 16, difficulty: 'Difícil', duration: '60 min', estimatedMinutes: 60, isCompleted: false },
  { id: '52', courseId: '5', title: 'Testing APIs', content: 'Testes de integração', order: 17, difficulty: 'Difícil', duration: '65 min', estimatedMinutes: 65, isCompleted: false },
  { id: '53', courseId: '5', title: 'Projeto Final API', content: 'API completa com todas as funcionalidades', order: 18, difficulty: 'Difícil', duration: '120 min', estimatedMinutes: 120, isCompleted: false },

  // Curso 9: Microserviços (24 aulas)
  { id: '54', courseId: '9', title: 'Arquitetura de Microserviços', content: 'Conceitos e padrões fundamentais', order: 1, difficulty: 'Difícil', duration: '60 min', estimatedMinutes: 60, isCompleted: false },
  { id: '55', courseId: '9', title: 'Docker e Containerização', content: 'Dockerfile e Docker Compose', order: 2, difficulty: 'Difícil', duration: '80 min', estimatedMinutes: 80, isCompleted: false },
  { id: '56', courseId: '9', title: 'API Gateway', content: 'Roteamento e load balancing', order: 3, difficulty: 'Difícil', duration: '70 min', estimatedMinutes: 70, isCompleted: false },
  { id: '57', courseId: '9', title: 'Service Discovery', content: 'Descoberta de serviços', order: 4, difficulty: 'Difícil', duration: '65 min', estimatedMinutes: 65, isCompleted: false },
  { id: '58', courseId: '9', title: 'Message Brokers', content: 'RabbitMQ e comunicação assíncrona', order: 5, difficulty: 'Difícil', duration: '75 min', estimatedMinutes: 75, isCompleted: false },

  // Curso 10: Azure (20 aulas)
  { id: '59', courseId: '10', title: 'Introdução ao Azure', content: 'Portal e conceitos básicos da nuvem', order: 1, difficulty: 'Médio', duration: '40 min', estimatedMinutes: 40, isCompleted: false },
  { id: '60', courseId: '10', title: 'App Services', content: 'Deploy de aplicações web', order: 2, difficulty: 'Médio', duration: '50 min', estimatedMinutes: 50, isCompleted: false },
  { id: '61', courseId: '10', title: 'Azure SQL Database', content: 'Banco de dados na nuvem', order: 3, difficulty: 'Médio', duration: '60 min', estimatedMinutes: 60, isCompleted: false },
  { id: '62', courseId: '10', title: 'Azure Functions', content: 'Computação serverless', order: 4, difficulty: 'Difícil', duration: '55 min', estimatedMinutes: 55, isCompleted: false },
  { id: '63', courseId: '10', title: 'Azure Storage', content: 'Armazenamento de arquivos e blobs', order: 5, difficulty: 'Médio', duration: '45 min', estimatedMinutes: 45, isCompleted: false },

  // Curso 6: Entity Framework Core (15 aulas)
  { id: '69', courseId: '6', title: 'Introdução ao Entity Framework', content: 'ORM e mapeamento objeto-relacional', order: 1, difficulty: 'Médio', duration: '45 min', estimatedMinutes: 45, isCompleted: false },
  { id: '70', courseId: '6', title: 'DbContext e Configuração', content: 'Contexto de dados e setup inicial', order: 2, difficulty: 'Médio', duration: '50 min', estimatedMinutes: 50, isCompleted: false },
  { id: '71', courseId: '6', title: 'Code First Approach', content: 'Criando banco a partir de classes', order: 3, difficulty: 'Médio', duration: '60 min', estimatedMinutes: 60, isCompleted: false },
  { id: '72', courseId: '6', title: 'Migrations', content: 'Versionamento de schema de banco', order: 4, difficulty: 'Médio', duration: '55 min', estimatedMinutes: 55, isCompleted: false },
  { id: '73', courseId: '6', title: 'Relacionamentos One-to-Many', content: 'Mapeando relacionamentos 1:N', order: 5, difficulty: 'Difícil', duration: '65 min', estimatedMinutes: 65, isCompleted: false },
  { id: '74', courseId: '6', title: 'Relacionamentos Many-to-Many', content: 'Mapeando relacionamentos N:N', order: 6, difficulty: 'Difícil', duration: '70 min', estimatedMinutes: 70, isCompleted: false },
  { id: '75', courseId: '6', title: 'LINQ to Entities', content: 'Consultas tipadas com LINQ', order: 7, difficulty: 'Médio', duration: '60 min', estimatedMinutes: 60, isCompleted: false },
  { id: '76', courseId: '6', title: 'Lazy Loading vs Eager Loading', content: 'Estratégias de carregamento', order: 8, difficulty: 'Difícil', duration: '55 min', estimatedMinutes: 55, isCompleted: false },
  { id: '77', courseId: '6', title: 'Fluent API', content: 'Configuração avançada de mapeamento', order: 9, difficulty: 'Difícil', duration: '65 min', estimatedMinutes: 65, isCompleted: false },
  { id: '78', courseId: '6', title: 'Repository Pattern', content: 'Padrão de acesso a dados', order: 10, difficulty: 'Difícil', duration: '70 min', estimatedMinutes: 70, isCompleted: false },
  { id: '79', courseId: '6', title: 'Unit of Work', content: 'Gerenciamento de transações', order: 11, difficulty: 'Difícil', duration: '60 min', estimatedMinutes: 60, isCompleted: false },
  { id: '80', courseId: '6', title: 'Performance e Otimização', content: 'Melhorando performance de consultas', order: 12, difficulty: 'Difícil', duration: '75 min', estimatedMinutes: 75, isCompleted: false },
  { id: '81', courseId: '6', title: 'Interceptors e Eventos', content: 'Customizando comportamento do EF', order: 13, difficulty: 'Difícil', duration: '65 min', estimatedMinutes: 65, isCompleted: false },
  { id: '82', courseId: '6', title: 'Testes com EF Core', content: 'Testando código com Entity Framework', order: 14, difficulty: 'Difícil', duration: '70 min', estimatedMinutes: 70, isCompleted: false },
  { id: '83', courseId: '6', title: 'Projeto Final EF Core', content: 'Sistema completo com Entity Framework', order: 15, difficulty: 'Difícil', duration: '120 min', estimatedMinutes: 120, isCompleted: false },

  // Curso 7: Frontend com React (16 aulas)
  { id: '84', courseId: '7', title: 'Introdução ao React', content: 'Conceitos fundamentais e JSX', order: 1, difficulty: 'Médio', duration: '45 min', estimatedMinutes: 45, isCompleted: false },
  { id: '85', courseId: '7', title: 'Componentes e Props', content: 'Criando componentes reutilizáveis', order: 2, difficulty: 'Médio', duration: '50 min', estimatedMinutes: 50, isCompleted: false },
  { id: '86', courseId: '7', title: 'State e useState', content: 'Gerenciamento de estado local', order: 3, difficulty: 'Médio', duration: '55 min', estimatedMinutes: 55, isCompleted: false },
  { id: '87', courseId: '7', title: 'Event Handling', content: 'Manipulando eventos do usuário', order: 4, difficulty: 'Médio', duration: '50 min', estimatedMinutes: 50, isCompleted: false },
  { id: '88', courseId: '7', title: 'useEffect Hook', content: 'Efeitos colaterais e ciclo de vida', order: 5, difficulty: 'Difícil', duration: '60 min', estimatedMinutes: 60, isCompleted: false },
  { id: '89', courseId: '7', title: 'Conditional Rendering', content: 'Renderização condicional', order: 6, difficulty: 'Médio', duration: '45 min', estimatedMinutes: 45, isCompleted: false },
  { id: '90', courseId: '7', title: 'Lists e Keys', content: 'Renderizando listas de dados', order: 7, difficulty: 'Médio', duration: '50 min', estimatedMinutes: 50, isCompleted: false },
  { id: '91', courseId: '7', title: 'Forms e Controlled Components', content: 'Formulários controlados', order: 8, difficulty: 'Médio', duration: '55 min', estimatedMinutes: 55, isCompleted: false },
  { id: '92', courseId: '7', title: 'useContext Hook', content: 'Compartilhamento de estado global', order: 9, difficulty: 'Difícil', duration: '60 min', estimatedMinutes: 60, isCompleted: false },
  { id: '93', courseId: '7', title: 'Custom Hooks', content: 'Criando hooks personalizados', order: 10, difficulty: 'Difícil', duration: '65 min', estimatedMinutes: 65, isCompleted: false },
  { id: '94', courseId: '7', title: 'React Router', content: 'Navegação em Single Page Applications', order: 11, difficulty: 'Difícil', duration: '70 min', estimatedMinutes: 70, isCompleted: false },
  { id: '95', courseId: '7', title: 'API Integration', content: 'Consumindo APIs REST', order: 12, difficulty: 'Difícil', duration: '65 min', estimatedMinutes: 65, isCompleted: false },
  { id: '96', courseId: '7', title: 'Styled Components', content: 'CSS-in-JS e estilização', order: 13, difficulty: 'Médio', duration: '55 min', estimatedMinutes: 55, isCompleted: false },
  { id: '97', courseId: '7', title: 'Testing com Jest', content: 'Testes unitários em React', order: 14, difficulty: 'Difícil', duration: '70 min', estimatedMinutes: 70, isCompleted: false },
  { id: '98', courseId: '7', title: 'Performance Optimization', content: 'Otimizando aplicações React', order: 15, difficulty: 'Difícil', duration: '65 min', estimatedMinutes: 65, isCompleted: false },
  { id: '99', courseId: '7', title: 'Projeto Final React', content: 'Aplicação completa com React', order: 16, difficulty: 'Difícil', duration: '120 min', estimatedMinutes: 120, isCompleted: false },

  // Curso 8: Testes Automatizados (12 aulas)
  { id: '100', courseId: '8', title: 'Fundamentos de Testes', content: 'Tipos de testes e importância', order: 1, difficulty: 'Médio', duration: '40 min', estimatedMinutes: 40, isCompleted: false },
  { id: '101', courseId: '8', title: 'xUnit Framework', content: 'Configuração e primeiros testes', order: 2, difficulty: 'Médio', duration: '45 min', estimatedMinutes: 45, isCompleted: false },
  { id: '102', courseId: '8', title: 'AAA Pattern', content: 'Arrange, Act, Assert', order: 3, difficulty: 'Médio', duration: '50 min', estimatedMinutes: 50, isCompleted: false },
  { id: '103', courseId: '8', title: 'Test-Driven Development', content: 'TDD na prática', order: 4, difficulty: 'Difícil', duration: '60 min', estimatedMinutes: 60, isCompleted: false },
  { id: '104', courseId: '8', title: 'Mocking com Moq', content: 'Isolamento de dependências', order: 5, difficulty: 'Difícil', duration: '65 min', estimatedMinutes: 65, isCompleted: false },
  { id: '105', courseId: '8', title: 'Testes de Integração', content: 'Testando componentes integrados', order: 6, difficulty: 'Difícil', duration: '70 min', estimatedMinutes: 70, isCompleted: false },
  { id: '106', courseId: '8', title: 'FluentAssertions', content: 'Assertions mais expressivas', order: 7, difficulty: 'Médio', duration: '45 min', estimatedMinutes: 45, isCompleted: false },
  { id: '107', courseId: '8', title: 'AutoFixture', content: 'Geração automática de dados de teste', order: 8, difficulty: 'Médio', duration: '50 min', estimatedMinutes: 50, isCompleted: false },
  { id: '108', courseId: '8', title: 'Testes de API', content: 'Testando Web APIs', order: 9, difficulty: 'Difícil', duration: '65 min', estimatedMinutes: 65, isCompleted: false },
  { id: '109', courseId: '8', title: 'Code Coverage', content: 'Medindo cobertura de código', order: 10, difficulty: 'Médio', duration: '55 min', estimatedMinutes: 55, isCompleted: false },
  { id: '110', courseId: '8', title: 'Testes em CI/CD', content: 'Automação de testes', order: 11, difficulty: 'Difícil', duration: '60 min', estimatedMinutes: 60, isCompleted: false },
  { id: '111', courseId: '8', title: 'Projeto Final Testes', content: 'Suite completa de testes', order: 12, difficulty: 'Difícil', duration: '90 min', estimatedMinutes: 90, isCompleted: false },

  // Curso 10: Cloud Computing com Azure (20 aulas)
  { id: '112', courseId: '10', title: 'Introdução ao Azure', content: 'Portal e conceitos básicos da nuvem', order: 1, difficulty: 'Médio', duration: '40 min', estimatedMinutes: 40, isCompleted: false },
  { id: '113', courseId: '10', title: 'App Services', content: 'Deploy de aplicações web', order: 2, difficulty: 'Médio', duration: '50 min', estimatedMinutes: 50, isCompleted: false },
  { id: '114', courseId: '10', title: 'Azure SQL Database', content: 'Banco de dados na nuvem', order: 3, difficulty: 'Médio', duration: '60 min', estimatedMinutes: 60, isCompleted: false },
  { id: '115', courseId: '10', title: 'Azure Functions', content: 'Computação serverless', order: 4, difficulty: 'Difícil', duration: '55 min', estimatedMinutes: 55, isCompleted: false },
  { id: '116', courseId: '10', title: 'Azure Storage', content: 'Armazenamento de arquivos e blobs', order: 5, difficulty: 'Médio', duration: '45 min', estimatedMinutes: 45, isCompleted: false },
  { id: '117', courseId: '10', title: 'Azure Key Vault', content: 'Gerenciamento de segredos', order: 6, difficulty: 'Médio', duration: '50 min', estimatedMinutes: 50, isCompleted: false },
  { id: '118', courseId: '10', title: 'Azure Active Directory', content: 'Identidade e acesso', order: 7, difficulty: 'Difícil', duration: '65 min', estimatedMinutes: 65, isCompleted: false },
  { id: '119', courseId: '10', title: 'Application Insights', content: 'Monitoramento e telemetria', order: 8, difficulty: 'Médio', duration: '55 min', estimatedMinutes: 55, isCompleted: false },
  { id: '120', courseId: '10', title: 'Azure DevOps', content: 'CI/CD na nuvem', order: 9, difficulty: 'Difícil', duration: '70 min', estimatedMinutes: 70, isCompleted: false },
  { id: '121', courseId: '10', title: 'ARM Templates', content: 'Infrastructure as Code', order: 10, difficulty: 'Difícil', duration: '75 min', estimatedMinutes: 75, isCompleted: false },
  { id: '122', courseId: '10', title: 'Azure Monitor', content: 'Observabilidade completa', order: 11, difficulty: 'Médio', duration: '60 min', estimatedMinutes: 60, isCompleted: false },
  { id: '123', courseId: '10', title: 'Azure Service Bus', content: 'Mensageria enterprise', order: 12, difficulty: 'Difícil', duration: '65 min', estimatedMinutes: 65, isCompleted: false },
  { id: '124', courseId: '10', title: 'Azure Cosmos DB', content: 'Banco NoSQL global', order: 13, difficulty: 'Difícil', duration: '70 min', estimatedMinutes: 70, isCompleted: false },
  { id: '125', courseId: '10', title: 'Azure Kubernetes Service', content: 'Orquestração de containers', order: 14, difficulty: 'Difícil', duration: '80 min', estimatedMinutes: 80, isCompleted: false },
  { id: '126', courseId: '10', title: 'Azure API Management', content: 'Gerenciamento de APIs', order: 15, difficulty: 'Difícil', duration: '65 min', estimatedMinutes: 65, isCompleted: false },
  { id: '127', courseId: '10', title: 'Azure Logic Apps', content: 'Integração e automação', order: 16, difficulty: 'Médio', duration: '55 min', estimatedMinutes: 55, isCompleted: false },
  { id: '128', courseId: '10', title: 'Segurança no Azure', content: 'Boas práticas de segurança', order: 17, difficulty: 'Difícil', duration: '70 min', estimatedMinutes: 70, isCompleted: false },
  { id: '129', courseId: '10', title: 'Otimização de Custos', content: 'Gerenciamento de custos na nuvem', order: 18, difficulty: 'Médio', duration: '50 min', estimatedMinutes: 50, isCompleted: false },
  { id: '130', courseId: '10', title: 'Preparação AZ-204', content: 'Certificação Azure Developer', order: 19, difficulty: 'Difícil', duration: '90 min', estimatedMinutes: 90, isCompleted: false },
  { id: '131', courseId: '10', title: 'Projeto Final Azure', content: 'Aplicação completa na nuvem', order: 20, difficulty: 'Difícil', duration: '120 min', estimatedMinutes: 120, isCompleted: false },

  // Curso 11: DevOps e CI/CD (18 aulas)
  { id: '132', courseId: '11', title: 'Introdução ao DevOps', content: 'Cultura e práticas DevOps', order: 1, difficulty: 'Médio', duration: '40 min', estimatedMinutes: 40, isCompleted: false },
  { id: '133', courseId: '11', title: 'Git Workflows', content: 'GitFlow e GitHub Flow', order: 2, difficulty: 'Médio', duration: '50 min', estimatedMinutes: 50, isCompleted: false },
  { id: '134', courseId: '11', title: 'GitHub Actions Básico', content: 'Primeiros workflows de CI', order: 3, difficulty: 'Difícil', duration: '60 min', estimatedMinutes: 60, isCompleted: false },
  { id: '135', courseId: '11', title: 'GitHub Actions Avançado', content: 'Workflows complexos e reutilizáveis', order: 4, difficulty: 'Difícil', duration: '70 min', estimatedMinutes: 70, isCompleted: false },
  { id: '136', courseId: '11', title: 'Docker para DevOps', content: 'Containerização em pipelines', order: 5, difficulty: 'Difícil', duration: '65 min', estimatedMinutes: 65, isCompleted: false },
  { id: '137', courseId: '11', title: 'Docker Compose', content: 'Orquestração de múltiplos containers', order: 6, difficulty: 'Difícil', duration: '60 min', estimatedMinutes: 60, isCompleted: false },
  { id: '138', courseId: '11', title: 'Kubernetes Fundamentos', content: 'Conceitos básicos de K8s', order: 7, difficulty: 'Difícil', duration: '80 min', estimatedMinutes: 80, isCompleted: false },
  { id: '139', courseId: '11', title: 'Kubernetes Deploy', content: 'Deploy de aplicações no K8s', order: 8, difficulty: 'Difícil', duration: '75 min', estimatedMinutes: 75, isCompleted: false },
  { id: '140', courseId: '11', title: 'Helm Charts', content: 'Gerenciamento de aplicações K8s', order: 9, difficulty: 'Difícil', duration: '70 min', estimatedMinutes: 70, isCompleted: false },
  { id: '141', courseId: '11', title: 'Infrastructure as Code', content: 'Terraform e ARM Templates', order: 10, difficulty: 'Difícil', duration: '75 min', estimatedMinutes: 75, isCompleted: false },
  { id: '142', courseId: '11', title: 'Monitoring com Prometheus', content: 'Métricas e alertas', order: 11, difficulty: 'Difícil', duration: '65 min', estimatedMinutes: 65, isCompleted: false },
  { id: '143', courseId: '11', title: 'Logging com ELK Stack', content: 'Elasticsearch, Logstash, Kibana', order: 12, difficulty: 'Difícil', duration: '70 min', estimatedMinutes: 70, isCompleted: false },
  { id: '144', courseId: '11', title: 'Security em DevOps', content: 'DevSecOps e SAST/DAST', order: 13, difficulty: 'Difícil', duration: '65 min', estimatedMinutes: 65, isCompleted: false },
  { id: '145', courseId: '11', title: 'Testing em Pipelines', content: 'Automação de testes', order: 14, difficulty: 'Difícil', duration: '60 min', estimatedMinutes: 60, isCompleted: false },
  { id: '146', courseId: '11', title: 'Deployment Strategies', content: 'Blue-Green, Canary, Rolling', order: 15, difficulty: 'Difícil', duration: '70 min', estimatedMinutes: 70, isCompleted: false },
  { id: '147', courseId: '11', title: 'Métricas DevOps', content: 'DORA metrics e KPIs', order: 16, difficulty: 'Médio', duration: '50 min', estimatedMinutes: 50, isCompleted: false },
  { id: '148', courseId: '11', title: 'GitOps', content: 'Git como fonte da verdade', order: 17, difficulty: 'Difícil', duration: '65 min', estimatedMinutes: 65, isCompleted: false },
  { id: '149', courseId: '11', title: 'Projeto Final DevOps', content: 'Pipeline completo de CI/CD', order: 18, difficulty: 'Difícil', duration: '120 min', estimatedMinutes: 120, isCompleted: false },

  // Curso 12: Arquitetura de Software (22 aulas)
  { id: '150', courseId: '12', title: 'Princípios SOLID', content: 'Fundamentos de design de software', order: 1, difficulty: 'Difícil', duration: '60 min', estimatedMinutes: 60, isCompleted: false },
  { id: '151', courseId: '12', title: 'Clean Code', content: 'Código limpo e legível', order: 2, difficulty: 'Médio', duration: '50 min', estimatedMinutes: 50, isCompleted: false },
  { id: '152', courseId: '12', title: 'Design Patterns - Creational', content: 'Singleton, Factory, Builder', order: 3, difficulty: 'Difícil', duration: '70 min', estimatedMinutes: 70, isCompleted: false },
  { id: '153', courseId: '12', title: 'Design Patterns - Structural', content: 'Adapter, Decorator, Facade', order: 4, difficulty: 'Difícil', duration: '70 min', estimatedMinutes: 70, isCompleted: false },
  { id: '154', courseId: '12', title: 'Design Patterns - Behavioral', content: 'Observer, Strategy, Command', order: 5, difficulty: 'Difícil', duration: '70 min', estimatedMinutes: 70, isCompleted: false },
  { id: '155', courseId: '12', title: 'Arquitetura em Camadas', content: 'Separação de responsabilidades', order: 6, difficulty: 'Difícil', duration: '65 min', estimatedMinutes: 65, isCompleted: false },
  { id: '156', courseId: '12', title: 'Clean Architecture', content: 'Arquitetura limpa e testável', order: 7, difficulty: 'Difícil', duration: '75 min', estimatedMinutes: 75, isCompleted: false },
  { id: '157', courseId: '12', title: 'Hexagonal Architecture', content: 'Ports and Adapters', order: 8, difficulty: 'Difícil', duration: '70 min', estimatedMinutes: 70, isCompleted: false },
  { id: '158', courseId: '12', title: 'Domain-Driven Design', content: 'Modelagem orientada ao domínio', order: 9, difficulty: 'Difícil', duration: '80 min', estimatedMinutes: 80, isCompleted: false },
  { id: '159', courseId: '12', title: 'CQRS Pattern', content: 'Command Query Responsibility Segregation', order: 10, difficulty: 'Difícil', duration: '75 min', estimatedMinutes: 75, isCompleted: false },
  { id: '160', courseId: '12', title: 'Event Sourcing', content: 'Histórico de eventos como fonte da verdade', order: 11, difficulty: 'Difícil', duration: '80 min', estimatedMinutes: 80, isCompleted: false },
  { id: '161', courseId: '12', title: 'Event-Driven Architecture', content: 'Arquitetura orientada a eventos', order: 12, difficulty: 'Difícil', duration: '75 min', estimatedMinutes: 75, isCompleted: false },
  { id: '162', courseId: '12', title: 'Microserviços vs Monolito', content: 'Quando usar cada abordagem', order: 13, difficulty: 'Difícil', duration: '65 min', estimatedMinutes: 65, isCompleted: false },
  { id: '163', courseId: '12', title: 'API Design', content: 'REST, GraphQL, gRPC', order: 14, difficulty: 'Difícil', duration: '70 min', estimatedMinutes: 70, isCompleted: false },
  { id: '164', courseId: '12', title: 'Caching Strategies', content: 'Estratégias de cache', order: 15, difficulty: 'Difícil', duration: '60 min', estimatedMinutes: 60, isCompleted: false },
  { id: '165', courseId: '12', title: 'Performance e Escalabilidade', content: 'Otimização de sistemas', order: 16, difficulty: 'Difícil', duration: '70 min', estimatedMinutes: 70, isCompleted: false },
  { id: '166', courseId: '12', title: 'Security by Design', content: 'Segurança na arquitetura', order: 17, difficulty: 'Difícil', duration: '65 min', estimatedMinutes: 65, isCompleted: false },
  { id: '167', courseId: '12', title: 'Documentação Arquitetural', content: 'C4 Model e ADRs', order: 18, difficulty: 'Médio', duration: '55 min', estimatedMinutes: 55, isCompleted: false },
  { id: '168', courseId: '12', title: 'Refactoring Strategies', content: 'Melhoria contínua de código', order: 19, difficulty: 'Difícil', duration: '65 min', estimatedMinutes: 65, isCompleted: false },
  { id: '169', courseId: '12', title: 'Technical Debt', content: 'Gerenciamento de débito técnico', order: 20, difficulty: 'Médio', duration: '50 min', estimatedMinutes: 50, isCompleted: false },
  { id: '170', courseId: '12', title: 'Architecture Review', content: 'Revisão e avaliação arquitetural', order: 21, difficulty: 'Difícil', duration: '60 min', estimatedMinutes: 60, isCompleted: false },
  { id: '171', courseId: '12', title: 'Projeto Final Arquitetura', content: 'Sistema enterprise completo', order: 22, difficulty: 'Difícil', duration: '150 min', estimatedMinutes: 150, isCompleted: false }
];

export const coursesApi = {
  /**
   * Get all courses with optional filters - MOCK VERSION
   */
  getAll: async (filters?: {
    levelId?: string;
    level?: string;
  }): Promise<CourseListResponse> => {
    // Simular delay de rede
    await new Promise(resolve => setTimeout(resolve, 300));
    
    let filteredCourses = [...mockCourses];
    
    if (filters?.levelId) {
      filteredCourses = filteredCourses.filter(course => course.levelId === filters.levelId);
    }
    
    if (filters?.level) {
      filteredCourses = filteredCourses.filter(course => course.level.toString() === filters.level);
    }
    
    return {
      courses: filteredCourses,
      total: filteredCourses.length,
      page: 1,
      pageSize: 10,
      totalPages: 1
    };
  },

  /**
   * Get a specific course with details - MOCK VERSION
   */
  getById: async (id: string): Promise<CourseDetail> => {
    await new Promise(resolve => setTimeout(resolve, 200));
    
    const course = mockCourses.find(c => c.id === id);
    if (!course) {
      throw new Error('Course not found');
    }

    // Conteúdo detalhado específico para cada curso
    let detailedContent = '';
    let prerequisites: string[] = [];
    let learningObjectives: string[] = [];
    let tags: string[] = [];
    let instructor = { id: '1', name: 'Professor Expert', bio: 'Especialista em desenvolvimento' };

    switch (id) {
      case '1': // Fundamentos C#
        detailedContent = `
## Fundamentos de Programação C#

### Sobre o Curso
Este curso é sua porta de entrada para o mundo da programação com C#. Desenvolvido para iniciantes, você aprenderá desde os conceitos mais básicos até programação orientada a objetos, construindo uma base sólida para sua carreira em desenvolvimento.

### O que você vai aprender
- **Sintaxe básica do C#**: Variáveis, tipos de dados, operadores
- **Estruturas de controle**: Condicionais (if/else/switch) e loops (for/while/foreach)
- **Programação Orientada a Objetos**: Classes, objetos, herança, polimorfismo
- **Tratamento de exceções**: Como lidar com erros de forma elegante
- **Collections e Generics**: Listas, dicionários e tipos genéricos
- **LINQ**: Consultas poderosas em coleções de dados
- **Programação assíncrona**: Async/await para aplicações responsivas

### Por que C#?
C# é uma das linguagens mais demandadas no mercado, especialmente para:
- Desenvolvimento web com ASP.NET
- Aplicações desktop com WPF/WinUI
- Jogos com Unity
- Aplicações mobile com Xamarin/.NET MAUI
- APIs e microserviços

### Metodologia
- **Teoria + Prática**: Cada conceito é seguido de exercícios práticos
- **Projetos reais**: Construa aplicações do mundo real
- **Executor integrado**: Teste seu código diretamente na plataforma
- **Progressão gradual**: Do básico ao avançado de forma estruturada
        `;
        prerequisites = [];
        learningObjectives = [
          'Dominar a sintaxe básica do C#',
          'Compreender programação orientada a objetos',
          'Desenvolver aplicações console completas',
          'Aplicar boas práticas de programação',
          'Preparar-se para frameworks avançados'
        ];
        tags = ['c#', 'programação', 'orientação-objetos', 'iniciante'];
        instructor = {
          id: '1',
          name: 'Prof. Carlos Silva',
          bio: 'Microsoft MVP, 15 anos de experiência em .NET, autor de 3 livros sobre C#'
        };
        break;

      case '2': // SQL
        detailedContent = `
## Banco de Dados com SQL

### Sobre o Curso
Domine a linguagem SQL e torne-se capaz de trabalhar com qualquer banco de dados relacional. Este curso abrange desde consultas básicas até otimização de performance, preparando você para trabalhar com dados em qualquer aplicação.

### O que você vai aprender
- **Fundamentos de bancos relacionais**: Tabelas, relacionamentos, normalização
- **Consultas SELECT**: Filtros, ordenação, agrupamento
- **JOINs**: Relacionar dados entre múltiplas tabelas
- **Funções de agregação**: COUNT, SUM, AVG, MIN, MAX
- **Manipulação de dados**: INSERT, UPDATE, DELETE
- **Definição de estruturas**: CREATE TABLE, ALTER, DROP
- **Subconsultas e CTEs**: Consultas complexas e reutilizáveis
- **Índices e performance**: Otimização de consultas
- **Stored procedures**: Lógica no banco de dados

### Banco de Dados Real
Você praticará com um banco SQLite real contendo:
- **Tabela Clientes**: Dados de clientes com relacionamentos
- **Tabela Pedidos**: Histórico de pedidos com valores
- **Tabela Produtos**: Catálogo de produtos
- **Relacionamentos funcionais**: Foreign keys e integridade referencial

### Aplicações Práticas
- Relatórios gerenciais
- Análise de dados
- Integração com aplicações
- Business Intelligence
- Data Science
        `;
        prerequisites = [];
        learningObjectives = [
          'Escrever consultas SQL eficientes',
          'Modelar bancos de dados relacionais',
          'Otimizar performance de consultas',
          'Trabalhar com relacionamentos complexos',
          'Aplicar boas práticas de banco de dados'
        ];
        tags = ['sql', 'banco-dados', 'consultas', 'relacionamentos'];
        instructor = {
          id: '2',
          name: 'Profa. Ana Database',
          bio: 'DBA Senior, especialista em SQL Server e PostgreSQL, 12 anos de experiência'
        };
        break;

      case '3': // Lógica
        detailedContent = `
## Lógica de Programação

### Sobre o Curso
Desenvolva o pensamento computacional e aprenda a resolver problemas de forma estruturada. Este curso é fundamental para qualquer pessoa que deseja programar, independente da linguagem escolhida.

### O que você vai aprender
- **Algoritmos**: Como pensar computacionalmente
- **Fluxogramas**: Representação visual de algoritmos
- **Estruturas sequenciais**: Entrada, processamento, saída
- **Estruturas condicionais**: Tomada de decisões
- **Estruturas de repetição**: Loops e iterações
- **Arrays e matrizes**: Estruturas de dados básicas
- **Algoritmos de ordenação**: Bubble Sort, Selection Sort
- **Algoritmos de busca**: Linear e binária
- **Complexidade**: Análise de eficiência

### Metodologia Prática
- **Pseudocódigo**: Antes do código real
- **Visualização**: Diagramas e fluxogramas
- **Exercícios graduais**: Do simples ao complexo
- **Problemas reais**: Situações do dia a dia
- **Implementação em C#**: Transformar lógica em código
        `;
        prerequisites = [];
        learningObjectives = [
          'Desenvolver pensamento computacional',
          'Criar algoritmos eficientes',
          'Resolver problemas complexos',
          'Compreender estruturas de dados',
          'Analisar complexidade de algoritmos'
        ];
        tags = ['lógica', 'algoritmos', 'estruturas-dados', 'resolução-problemas'];
        instructor = {
          id: '3',
          name: 'Prof. João Algoritmo',
          bio: 'PhD em Ciência da Computação, especialista em algoritmos e estruturas de dados'
        };
        break;

      case '4': // Git
        detailedContent = `
## Git e Controle de Versão

### Sobre o Curso
Aprenda a ferramenta mais importante para qualquer desenvolvedor. Git é essencial para trabalhar em equipe, manter histórico de código e colaborar em projetos open source.

### O que você vai aprender
- **Conceitos fundamentais**: Repositórios, commits, branches
- **Comandos básicos**: init, add, commit, status, log
- **Branches**: Criação, merge, resolução de conflitos
- **Repositórios remotos**: GitHub, push, pull, clone
- **Colaboração**: Pull requests, code review
- **Workflows**: Git Flow, GitHub Flow
- **Boas práticas**: Mensagens de commit, organização

### Ferramentas Abordadas
- **Git CLI**: Linha de comando
- **GitHub**: Plataforma de colaboração
- **Visual Studio**: Integração com IDE
- **GitKraken**: Interface gráfica

### Por que é Importante?
- **Colaboração em equipe**: Trabalhe com outros desenvolvedores
- **Histórico completo**: Nunca perca código
- **Backup automático**: Código seguro na nuvem
- **Portfolio**: Mostre seus projetos no GitHub
        `;
        prerequisites = [];
        learningObjectives = [
          'Dominar comandos Git essenciais',
          'Trabalhar com branches e merges',
          'Colaborar em projetos de equipe',
          'Usar GitHub profissionalmente',
          'Aplicar workflows de desenvolvimento'
        ];
        tags = ['git', 'controle-versão', 'github', 'colaboração'];
        instructor = {
          id: '4',
          name: 'Prof. Pedro Version',
          bio: 'DevOps Engineer, contribuidor open source, especialista em Git workflows'
        };
        break;

      case '5': // ASP.NET Core
        detailedContent = `
## ASP.NET Core Web API

### Sobre o Curso
Construa APIs robustas e escaláveis com ASP.NET Core, o framework web mais moderno da Microsoft. Aprenda a criar serviços que podem ser consumidos por aplicações web, mobile e desktop.

### O que você vai aprender
- **Fundamentos de APIs REST**: HTTP, verbos, status codes
- **Controllers e Actions**: Estrutura básica de uma API
- **Roteamento**: URLs amigáveis e parâmetros
- **Model Binding**: Conversão automática de dados
- **Dependency Injection**: Inversão de controle
- **Entity Framework**: Integração com banco de dados
- **Autenticação JWT**: Segurança moderna
- **Autorização**: Controle de acesso baseado em roles
- **Middleware**: Pipeline de requisições customizado
- **Swagger**: Documentação automática
- **Testes**: Testes unitários e de integração
- **Deploy**: Publicação em produção

### Projetos Práticos
- **API de E-commerce**: Sistema completo de vendas
- **Sistema de Usuários**: Autenticação e autorização
- **API de Blog**: CRUD completo com relacionamentos
- **Microserviço**: Serviço independente e escalável

### Tecnologias Integradas
- **Entity Framework Core**: ORM moderno
- **AutoMapper**: Mapeamento de objetos
- **FluentValidation**: Validação robusta
- **Serilog**: Logging estruturado
        `;
        prerequisites = ['Fundamentos de C#', 'Banco de Dados SQL'];
        learningObjectives = [
          'Criar APIs REST profissionais',
          'Implementar autenticação e autorização',
          'Integrar com bancos de dados',
          'Aplicar padrões de arquitetura',
          'Preparar APIs para produção'
        ];
        tags = ['asp.net-core', 'web-api', 'rest', 'backend'];
        instructor = {
          id: '5',
          name: 'Prof. Maria WebAPI',
          bio: 'Arquiteta de Software, Microsoft MVP, especialista em ASP.NET Core'
        };
        break;

      case '6': // Entity Framework
        detailedContent = `
## Entity Framework Core

### Sobre o Curso
Domine o ORM (Object-Relational Mapping) mais poderoso do ecossistema .NET. Aprenda a trabalhar com bancos de dados de forma orientada a objetos, aumentando produtividade e mantendo performance.

### O que você vai aprender
- **Code First**: Criar banco a partir de classes
- **Database First**: Gerar classes a partir do banco
- **DbContext**: Contexto de dados e configuração
- **Entities**: Mapeamento objeto-relacional
- **Relationships**: One-to-One, One-to-Many, Many-to-Many
- **Migrations**: Versionamento de schema
- **LINQ to Entities**: Consultas tipadas
- **Performance**: Lazy loading, eager loading, tracking
- **Configurações avançadas**: Fluent API, Data Annotations
- **Interceptors**: Customização de comportamento

### Padrões Abordados
- **Repository Pattern**: Abstração de acesso a dados
- **Unit of Work**: Transações e consistência
- **Specification Pattern**: Consultas reutilizáveis
- **CQRS**: Separação de comandos e consultas

### Bancos Suportados
- SQL Server
- PostgreSQL
- MySQL
- SQLite
- Oracle
- In-Memory (testes)
        `;
        prerequisites = ['Fundamentos de C#', 'Banco de Dados SQL'];
        learningObjectives = [
          'Modelar dados com Code First',
          'Otimizar performance de consultas',
          'Implementar relacionamentos complexos',
          'Aplicar padrões de acesso a dados',
          'Gerenciar migrations em produção'
        ];
        tags = ['entity-framework', 'orm', 'banco-dados', 'linq'];
        instructor = {
          id: '6',
          name: 'Prof. Roberto ORM',
          bio: 'Especialista em Entity Framework, autor de curso oficial Microsoft'
        };
        break;

      case '7': // React
        detailedContent = `
## Frontend com React

### Sobre o Curso
Crie interfaces modernas e interativas com React, a biblioteca JavaScript mais popular para desenvolvimento frontend. Aprenda desde os conceitos básicos até padrões avançados.

### O que você vai aprender
- **Componentes**: Funcionais e de classe
- **JSX**: Sintaxe declarativa
- **Props e State**: Gerenciamento de dados
- **Hooks**: useState, useEffect, useContext, custom hooks
- **Event Handling**: Interações do usuário
- **Conditional Rendering**: Renderização condicional
- **Lists e Keys**: Renderização de listas
- **Forms**: Formulários controlados
- **Context API**: Estado global
- **React Router**: Navegação SPA
- **Styled Components**: CSS-in-JS
- **Testing**: Jest e React Testing Library

### TypeScript Integration
- **Tipagem forte**: Props, state, eventos
- **Interfaces**: Contratos de dados
- **Generics**: Componentes reutilizáveis
- **Type Guards**: Validação de tipos

### Ferramentas Modernas
- **Vite**: Build tool rápido
- **ESLint**: Qualidade de código
- **Prettier**: Formatação automática
- **Storybook**: Documentação de componentes
        `;
        prerequisites = ['Lógica de Programação', 'JavaScript básico'];
        learningObjectives = [
          'Criar componentes React reutilizáveis',
          'Gerenciar estado de aplicações',
          'Implementar navegação SPA',
          'Integrar com APIs REST',
          'Aplicar boas práticas de frontend'
        ];
        tags = ['react', 'frontend', 'javascript', 'typescript'];
        instructor = {
          id: '7',
          name: 'Profa. Julia Frontend',
          bio: 'Frontend Architect, especialista em React e ecossistema JavaScript'
        };
        break;

      case '8': // Testes
        detailedContent = `
## Testes Automatizados

### Sobre o Curso
Aprenda a garantir qualidade de software através de testes automatizados. Domine TDD (Test-Driven Development) e construa aplicações mais confiáveis e maintíveis.

### O que você vai aprender
- **Fundamentos de testes**: Unitários, integração, E2E
- **xUnit**: Framework de testes para .NET
- **Moq**: Mocking e isolamento
- **TDD**: Test-Driven Development
- **BDD**: Behavior-Driven Development
- **AAA Pattern**: Arrange, Act, Assert
- **Test Doubles**: Mocks, stubs, fakes
- **Code Coverage**: Cobertura de código
- **Testes de API**: Testes de integração
- **Testes de UI**: Selenium, Playwright

### Ferramentas e Frameworks
- **xUnit**: Testes unitários
- **NUnit**: Framework alternativo
- **MSTest**: Framework da Microsoft
- **FluentAssertions**: Assertions expressivas
- **AutoFixture**: Geração de dados de teste
- **TestContainers**: Testes com containers

### Boas Práticas
- **FIRST**: Fast, Independent, Repeatable, Self-validating, Timely
- **Pirâmide de testes**: Proporção ideal de tipos de teste
- **Testes como documentação**: Especificação viva
- **CI/CD**: Integração contínua com testes
        `;
        prerequisites = ['Fundamentos de C#', 'ASP.NET Core Web API'];
        learningObjectives = [
          'Implementar TDD em projetos reais',
          'Criar testes unitários eficazes',
          'Configurar pipelines de CI/CD',
          'Aplicar mocking e isolamento',
          'Medir e melhorar cobertura de código'
        ];
        tags = ['testes', 'tdd', 'qualidade', 'xunit'];
        instructor = {
          id: '8',
          name: 'Prof. Carlos Quality',
          bio: 'QA Engineer, especialista em testes automatizados e TDD'
        };
        break;

      case '9': // Microserviços
        detailedContent = `
## Microserviços com .NET

### Sobre o Curso
Aprenda a arquitetura de microserviços, o padrão mais usado em aplicações enterprise modernas. Construa sistemas distribuídos escaláveis e resilientes.

### O que você vai aprender
- **Arquitetura de microserviços**: Princípios e padrões
- **Domain-Driven Design**: Modelagem orientada ao domínio
- **API Gateway**: Ponto único de entrada
- **Service Discovery**: Descoberta de serviços
- **Load Balancing**: Distribuição de carga
- **Circuit Breaker**: Resiliência e tolerância a falhas
- **Message Brokers**: RabbitMQ, Apache Kafka
- **Event Sourcing**: Histórico de eventos
- **CQRS**: Separação de comandos e consultas
- **Docker**: Containerização de serviços
- **Kubernetes**: Orquestração de containers
- **Monitoring**: Observabilidade e métricas

### Padrões Implementados
- **Saga Pattern**: Transações distribuídas
- **Outbox Pattern**: Consistência eventual
- **Strangler Fig**: Migração gradual
- **Database per Service**: Isolamento de dados

### Tecnologias Utilizadas
- **ASP.NET Core**: APIs de microserviços
- **Docker**: Containerização
- **RabbitMQ**: Mensageria assíncrona
- **Redis**: Cache distribuído
- **Consul**: Service discovery
- **Ocelot**: API Gateway
        `;
        prerequisites = ['ASP.NET Core Web API', 'Docker básico'];
        learningObjectives = [
          'Projetar arquiteturas de microserviços',
          'Implementar comunicação entre serviços',
          'Aplicar padrões de resiliência',
          'Configurar observabilidade',
          'Orquestrar com Kubernetes'
        ];
        tags = ['microserviços', 'arquitetura', 'docker', 'kubernetes'];
        instructor = {
          id: '9',
          name: 'Prof. André Micro',
          bio: 'Solutions Architect, especialista em sistemas distribuídos e microserviços'
        };
        break;

      case '10': // Azure
        detailedContent = `
## Cloud Computing com Azure

### Sobre o Curso
Domine a nuvem Microsoft Azure e aprenda a construir, deployar e gerenciar aplicações escaláveis na cloud. Prepare-se para certificações Azure.

### O que você vai aprender
- **Fundamentos de Cloud**: IaaS, PaaS, SaaS
- **Azure Portal**: Interface de gerenciamento
- **App Services**: Hospedagem de aplicações web
- **Azure Functions**: Computação serverless
- **Azure SQL Database**: Banco de dados na nuvem
- **Azure Storage**: Armazenamento de arquivos e blobs
- **Azure Key Vault**: Gerenciamento de segredos
- **Azure Active Directory**: Identidade e acesso
- **Application Insights**: Monitoramento e telemetria
- **Azure DevOps**: CI/CD na nuvem
- **ARM Templates**: Infrastructure as Code
- **Azure Monitor**: Observabilidade completa

### Serviços Avançados
- **Azure Service Bus**: Mensageria enterprise
- **Azure Cosmos DB**: Banco NoSQL global
- **Azure Kubernetes Service**: Orquestração de containers
- **Azure API Management**: Gerenciamento de APIs
- **Azure Logic Apps**: Integração e automação

### Certificações Preparadas
- **AZ-900**: Azure Fundamentals
- **AZ-204**: Azure Developer Associate
- **AZ-400**: DevOps Engineer Expert

### Projetos Práticos
- **Deploy de Web App**: Aplicação completa na nuvem
- **API com Azure Functions**: Serverless architecture
- **Sistema de monitoramento**: Observabilidade completa
        `;
        prerequisites = ['ASP.NET Core Web API', 'Conceitos de cloud'];
        learningObjectives = [
          'Deployar aplicações no Azure',
          'Configurar monitoramento e alertas',
          'Implementar segurança na nuvem',
          'Otimizar custos e performance',
          'Preparar para certificações Azure'
        ];
        tags = ['azure', 'cloud', 'devops', 'serverless'];
        instructor = {
          id: '10',
          name: 'Profa. Carla Cloud',
          bio: 'Azure MVP, Solutions Architect, especialista em cloud computing'
        };
        break;

      case '11': // DevOps
        detailedContent = `
## DevOps e CI/CD

### Sobre o Curso
Aprenda a cultura DevOps e implemente pipelines de CI/CD para acelerar entregas e aumentar a qualidade do software. Domine ferramentas modernas de automação.

### O que você vai aprender
- **Cultura DevOps**: Colaboração entre Dev e Ops
- **Git Workflows**: GitFlow, GitHub Flow
- **GitHub Actions**: Automação de workflows
- **Docker**: Containerização de aplicações
- **Kubernetes**: Orquestração de containers
- **Infrastructure as Code**: Terraform, ARM Templates
- **Monitoring**: Prometheus, Grafana, ELK Stack
- **Security**: DevSecOps, SAST, DAST
- **Testing**: Testes automatizados em pipelines
- **Deployment Strategies**: Blue-Green, Canary, Rolling

### Ferramentas Abordadas
- **GitHub Actions**: CI/CD nativo do GitHub
- **Azure DevOps**: Plataforma completa da Microsoft
- **Jenkins**: Automação open source
- **Docker**: Containerização
- **Kubernetes**: Orquestração
- **Helm**: Gerenciamento de aplicações K8s
- **Terraform**: Infrastructure as Code

### Pipelines Implementados
- **Build Pipeline**: Compilação e testes
- **Release Pipeline**: Deploy automatizado
- **Security Pipeline**: Análise de segurança
- **Monitoring Pipeline**: Observabilidade

### Métricas e KPIs
- **Lead Time**: Tempo de entrega
- **Deployment Frequency**: Frequência de deploy
- **MTTR**: Tempo médio de recuperação
- **Change Failure Rate**: Taxa de falha em mudanças
        `;
        prerequisites = ['Git', 'Docker básico', 'Conceitos de cloud'];
        learningObjectives = [
          'Implementar pipelines de CI/CD',
          'Automatizar deployments',
          'Configurar monitoramento',
          'Aplicar Infrastructure as Code',
          'Medir e melhorar métricas DevOps'
        ];
        tags = ['devops', 'ci-cd', 'automação', 'kubernetes'];
        instructor = {
          id: '11',
          name: 'Prof. Felipe DevOps',
          bio: 'DevOps Engineer, especialista em automação e cultura DevOps'
        };
        break;

      case '12': // Arquitetura
        detailedContent = `
## Arquitetura de Software

### Sobre o Curso
Aprenda a projetar sistemas de software escaláveis, maintíveis e robustos. Domine padrões arquiteturais e princípios de design para construir aplicações enterprise.

### O que você vai aprender
- **Princípios SOLID**: Fundamentos de design
- **Clean Architecture**: Arquitetura limpa e testável
- **Domain-Driven Design**: Modelagem orientada ao domínio
- **Padrões de Design**: GoF, Enterprise Patterns
- **Arquiteturas em Camadas**: Separação de responsabilidades
- **Event-Driven Architecture**: Arquitetura orientada a eventos
- **CQRS**: Command Query Responsibility Segregation
- **Event Sourcing**: Histórico de eventos como fonte da verdade
- **Microserviços**: Arquitetura distribuída
- **Monolito Modular**: Alternativa aos microserviços
- **API Design**: REST, GraphQL, gRPC
- **Performance**: Caching, otimização, escalabilidade

### Padrões Arquiteturais
- **MVC**: Model-View-Controller
- **MVP**: Model-View-Presenter
- **MVVM**: Model-View-ViewModel
- **Hexagonal**: Ports and Adapters
- **Onion**: Arquitetura em cebola
- **Clean**: Arquitetura limpa

### Qualidade de Software
- **Code Review**: Revisão de código
- **Refactoring**: Melhoria contínua
- **Technical Debt**: Gerenciamento de débito técnico
- **Documentation**: Documentação arquitetural
- **Testing**: Estratégias de teste

### Ferramentas de Design
- **C4 Model**: Documentação arquitetural
- **UML**: Modelagem unificada
- **Event Storming**: Descoberta de domínio
- **Architecture Decision Records**: Registro de decisões
        `;
        prerequisites = ['ASP.NET Core Web API', 'Testes Automatizados', 'Padrões de Design'];
        learningObjectives = [
          'Projetar arquiteturas escaláveis',
          'Aplicar princípios SOLID e Clean Code',
          'Implementar Domain-Driven Design',
          'Documentar decisões arquiteturais',
          'Avaliar trade-offs arquiteturais'
        ];
        tags = ['arquitetura', 'design-patterns', 'clean-architecture', 'ddd'];
        instructor = {
          id: '12',
          name: 'Prof. Ricardo Architect',
          bio: 'Software Architect, especialista em arquitetura enterprise e DDD'
        };
        break;

      default:
        detailedContent = `Este é o conteúdo detalhado do curso "${course.title}". Aqui você aprenderá todos os conceitos fundamentais.`;
        prerequisites = [];
        learningObjectives = [
          'Dominar os conceitos básicos',
          'Aplicar conhecimentos práticos',
          'Resolver problemas reais'
        ];
        tags = ['programação', 'desenvolvimento'];
    }
    
    return {
      ...course,
      content: detailedContent,
      prerequisites,
      learningObjectives,
      tags,
      instructor
    };
  },

  /**
   * Get lessons for a specific course - MOCK VERSION
   */
  getLessons: async (courseId: string): Promise<LessonListResponse> => {
    await new Promise(resolve => setTimeout(resolve, 200));
    
    const courseLessons = mockLessons.filter(lesson => lesson.courseId === courseId);
    
    return {
      lessons: courseLessons,
      total: courseLessons.length,
      courseId: courseId
    };
  },

  /**
   * Get a specific lesson with full content - MOCK VERSION
   */
  getLesson: async (courseId: string, lessonId: string): Promise<LessonDetail> => {
    await new Promise(resolve => setTimeout(resolve, 200));
    
    const lesson = mockLessons.find(l => l.id === lessonId && l.courseId === courseId);
    if (!lesson) {
      throw new Error('Lesson not found');
    }

    // Conteúdo educacional específico baseado no tipo de aula
    let educationalContent = '';
    
    // Conteúdo para aulas de C#
    if (courseId === '1') {
      switch (lessonId) {
        case '1':
          educationalContent = `
## Introdução ao C# e .NET

### O que é C#?
C# (pronuncia-se "C Sharp") é uma linguagem de programação moderna, orientada a objetos e type-safe desenvolvida pela Microsoft. É uma das linguagens mais populares para desenvolvimento de aplicações empresariais.

### História e Evolução
- **2000**: Primeira versão do C# lançada com .NET Framework 1.0
- **2002**: C# 1.0 - Linguagem orientada a objetos básica
- **2023**: C# 12 - Recursos modernos e performance otimizada

### Características Principais
- **Type Safety**: Previne erros comuns de programação
- **Garbage Collection**: Gerenciamento automático de memória
- **Cross-Platform**: Roda em Windows, Linux e macOS
- **Rich Ecosystem**: Vasta biblioteca de classes e frameworks

### Exemplo Prático

\`\`\`csharp
using System;

namespace MeuPrimeiroProjeto
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Bem-vindo ao C#!");
            Console.WriteLine("Sua jornada de programação começa aqui.");
            
            // Exemplo de variável
            string nome = "Desenvolvedor";
            int idade = 25;
            
            Console.WriteLine($"Olá, {nome}! Você tem {idade} anos.");
        }
    }
}
\`\`\`

### Exercício Prático
Modifique o código acima para:
1. Solicitar o nome do usuário
2. Calcular o ano de nascimento
3. Exibir uma mensagem personalizada
          `;
          break;
        case '2':
          educationalContent = `
## Variáveis e Tipos de Dados

### Tipos Fundamentais em C#
C# é uma linguagem fortemente tipada, o que significa que toda variável deve ter um tipo específico.

### Tipos Numéricos
\`\`\`csharp
// Números inteiros
int idade = 25;
long populacao = 7800000000L;
short temperatura = -10;
byte nivel = 100;

// Números decimais
float preco = 19.99f;
double pi = 3.14159265359;
decimal salario = 5000.50m;
\`\`\`

### Tipos de Texto e Caracteres
\`\`\`csharp
char inicial = 'A';
string nome = "João Silva";
string endereco = @"C:\Users\João\Documents"; // String literal
\`\`\`

### Tipos Lógicos
\`\`\`csharp
bool ativo = true;
bool concluido = false;
\`\`\`

### Inferência de Tipos com var
\`\`\`csharp
var numero = 42;        // int
var texto = "Hello";    // string
var ativo = true;       // bool
var preco = 19.99;      // double
\`\`\`

### Conversões de Tipos
\`\`\`csharp
// Conversão implícita (segura)
int inteiro = 100;
double decimal = inteiro;

// Conversão explícita (cast)
double valor = 19.99;
int parteInteira = (int)valor; // 19

// Conversão usando métodos
string numeroTexto = "123";
int numero = Convert.ToInt32(numeroTexto);
int.TryParse(numeroTexto, out int resultado);
\`\`\`

### Exercício Prático
Crie um programa que:
1. Declare variáveis de diferentes tipos
2. Realize conversões entre tipos
3. Exiba os resultados formatados
          `;
          break;
        default:
          educationalContent = `
## ${lesson.title}

Este é o conteúdo da aula "${lesson.title}".

### Objetivos de Aprendizagem
- Compreender os conceitos fundamentais
- Aplicar conhecimentos na prática
- Desenvolver habilidades de programação

### Exemplo de Código

\`\`\`csharp
using System;

class Program 
{
    static void Main()
    {
        Console.WriteLine("Exemplo prático da aula");
        
        // Código específico da aula
        var exemplo = "Aprendendo C#";
        Console.WriteLine($"Conteúdo: {exemplo}");
    }
}
\`\`\`

### Exercício Prático
Complete o código seguindo as instruções da aula.
          `;
      }
    }
    
    // Conteúdo para aulas de SQL
    else if (courseId === '2') {
      switch (lessonId) {
        case '13':
          educationalContent = `
## Introdução ao SQL

### O que é SQL?
SQL (Structured Query Language) é a linguagem padrão para gerenciar e manipular bancos de dados relacionais. É essencial para qualquer desenvolvedor que trabalha com dados.

### Conceitos Fundamentais
- **Banco de Dados**: Coleção organizada de dados
- **Tabela**: Estrutura que armazena dados em linhas e colunas
- **Registro (Row)**: Uma linha de dados na tabela
- **Campo (Column)**: Uma coluna que representa um atributo

### Tipos de Comandos SQL
1. **DDL (Data Definition Language)**: CREATE, ALTER, DROP
2. **DML (Data Manipulation Language)**: INSERT, UPDATE, DELETE
3. **DQL (Data Query Language)**: SELECT
4. **DCL (Data Control Language)**: GRANT, REVOKE

### Exemplo de Estrutura de Tabela

\`\`\`sql
-- Visualizar estrutura das tabelas disponíveis
SELECT name FROM sqlite_master WHERE type='table';

-- Ver dados da tabela Clientes
SELECT * FROM Clientes;
\`\`\`

### Exercício Prático
Execute as consultas acima para explorar o banco de dados.
          `;
          break;
        case '14':
          educationalContent = `
## SELECT e Consultas Básicas

### Sintaxe Básica do SELECT
\`\`\`sql
SELECT coluna1, coluna2
FROM tabela
WHERE condição
ORDER BY coluna
LIMIT quantidade;
\`\`\`

### Exemplos Práticos

\`\`\`sql
-- Selecionar todos os clientes
SELECT * FROM Clientes;

-- Selecionar apenas nome e email
SELECT Nome, Email FROM Clientes;

-- Filtrar clientes específicos
SELECT * FROM Clientes WHERE Nome LIKE '%Silva%';

-- Ordenar por nome
SELECT * FROM Clientes ORDER BY Nome;

-- Limitar resultados
SELECT * FROM Clientes LIMIT 3;
\`\`\`

### Operadores de Comparação
\`\`\`sql
-- Igualdade
SELECT * FROM Pedidos WHERE Status = 'Concluído';

-- Maior que
SELECT * FROM Pedidos WHERE Valor > 100;

-- Entre valores
SELECT * FROM Pedidos WHERE Valor BETWEEN 50 AND 200;

-- Lista de valores
SELECT * FROM Pedidos WHERE Status IN ('Pendente', 'Enviado');
\`\`\`

### Exercício Prático
Pratique diferentes tipos de consultas SELECT com as tabelas disponíveis.
          `;
          break;
        default:
          educationalContent = `
## ${lesson.title}

### Conceitos da Aula
Esta aula aborda conceitos importantes de SQL para ${lesson.content}.

### Exemplo SQL

\`\`\`sql
-- Exemplo prático
SELECT * FROM Clientes;

-- Consulta com JOIN
SELECT c.Nome, p.Valor 
FROM Clientes c
INNER JOIN Pedidos p ON c.ClienteID = p.ClienteID;
\`\`\`

### Exercício
Execute as consultas e observe os resultados.
          `;
      }
    }
    
    // Conteúdo para Lógica de Programação (Curso 3)
    else if (courseId === '3') {
      educationalContent = `
## ${lesson.title}

### Conceitos Fundamentais
${lesson.content}

### Aplicação Prática
A lógica de programação é a base para resolver qualquer problema computacional. Nesta aula, você aprenderá a pensar de forma estruturada.

### Exemplo de Algoritmo

\`\`\`
ALGORITMO: ${lesson.title}
INÍCIO
  // Entrada de dados
  LEIA variável
  
  // Processamento
  PROCESSE lógica
  
  // Saída
  ESCREVA resultado
FIM
\`\`\`

### Exercício
Desenvolva um algoritmo seguindo os conceitos apresentados.
      `;
    }
    
    // Conteúdo para Git (Curso 4)
    else if (courseId === '4') {
      educationalContent = `
## ${lesson.title}

### Conceitos do Git
${lesson.content}

### Comandos Essenciais
Esta aula aborda comandos fundamentais do Git para controle de versão.

### Exemplo Prático

\`\`\`bash
# Comandos básicos do Git
git init
git add .
git commit -m "Mensagem do commit"
git status
git log
\`\`\`

### Exercício
Pratique os comandos Git em um repositório de teste.
      `;
    }
    
    // Conteúdo para ASP.NET Core (Curso 5)
    else if (courseId === '5') {
      educationalContent = `
## ${lesson.title}

### Desenvolvimento Web com ASP.NET Core
${lesson.content}

### Conceitos da Aula
Aprenda a construir APIs robustas e escaláveis com ASP.NET Core.

### Exemplo de Controller

\`\`\`csharp
[ApiController]
[Route("api/[controller]")]
public class ExemploController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok("API funcionando!");
    }
    
    [HttpPost]
    public IActionResult Post([FromBody] object data)
    {
        return Ok($"Dados recebidos: {data}");
    }
}
\`\`\`

### Exercício
Implemente um controller seguindo os padrões apresentados.
      `;
    }
    
    // Conteúdo para Entity Framework (Curso 6)
    else if (courseId === '6') {
      educationalContent = `
## ${lesson.title}

### Entity Framework Core
${lesson.content}

### ORM e Mapeamento
Aprenda a trabalhar com bancos de dados de forma orientada a objetos.

### Exemplo de Entity

\`\`\`csharp
public class Cliente
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Email { get; set; }
    public List<Pedido> Pedidos { get; set; }
}

public class AppDbContext : DbContext
{
    public DbSet<Cliente> Clientes { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("connection-string");
    }
}
\`\`\`

### Exercício
Configure um DbContext e crie suas primeiras entidades.
      `;
    }
    
    // Conteúdo para React (Curso 7)
    else if (courseId === '7') {
      educationalContent = `
## ${lesson.title}

### Desenvolvimento Frontend com React
${lesson.content}

### Componentes React
Aprenda a criar interfaces modernas e interativas.

### Exemplo de Componente

\`\`\`jsx
import React, { useState } from 'react';

function ExemploComponente() {
    const [contador, setContador] = useState(0);
    
    return (
        <div>
            <h1>Contador: {contador}</h1>
            <button onClick={() => setContador(contador + 1)}>
                Incrementar
            </button>
        </div>
    );
}

export default ExemploComponente;
\`\`\`

### Exercício
Crie um componente React funcional com estado.
      `;
    }
    
    // Conteúdo para Testes (Curso 8)
    else if (courseId === '8') {
      educationalContent = `
## ${lesson.title}

### Testes Automatizados
${lesson.content}

### Qualidade de Software
Aprenda a garantir qualidade através de testes automatizados.

### Exemplo de Teste

\`\`\`csharp
[Fact]
public void DeveCalcularSomaCorretamente()
{
    // Arrange
    var calculadora = new Calculadora();
    
    // Act
    var resultado = calculadora.Somar(2, 3);
    
    // Assert
    Assert.Equal(5, resultado);
}
\`\`\`

### Exercício
Escreva testes unitários seguindo o padrão AAA.
      `;
    }
    
    // Conteúdo para Microserviços (Curso 9)
    else if (courseId === '9') {
      educationalContent = `
## ${lesson.title}

### Arquitetura de Microserviços
${lesson.content}

### Sistemas Distribuídos
Aprenda a construir aplicações escaláveis e resilientes.

### Exemplo de Microserviço

\`\`\`csharp
[ApiController]
[Route("api/[controller]")]
public class ProdutoController : ControllerBase
{
    private readonly IProdutoService _produtoService;
    
    public ProdutoController(IProdutoService produtoService)
    {
        _produtoService = produtoService;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetProdutos()
    {
        var produtos = await _produtoService.GetAllAsync();
        return Ok(produtos);
    }
}
\`\`\`

### Exercício
Implemente um microserviço seguindo os padrões apresentados.
      `;
    }
    
    // Conteúdo para Azure (Curso 10)
    else if (courseId === '10') {
      educationalContent = `
## ${lesson.title}

### Cloud Computing com Azure
${lesson.content}

### Computação na Nuvem
Aprenda a construir e deployar aplicações na nuvem Microsoft Azure.

### Exemplo de Configuração

\`\`\`json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=azure-server;Database=mydb;Trusted_Connection=true;"
  },
  "AzureSettings": {
    "StorageAccount": "mystorageaccount",
    "KeyVault": "mykeyvault"
  }
}
\`\`\`

### Exercício
Configure recursos Azure seguindo as boas práticas.
      `;
    }
    
    // Conteúdo para DevOps (Curso 11)
    else if (courseId === '11') {
      educationalContent = `
## ${lesson.title}

### DevOps e CI/CD
${lesson.content}

### Automação e Entrega Contínua
Aprenda a automatizar pipelines e acelerar entregas.

### Exemplo de Pipeline

\`\`\`yaml
name: CI/CD Pipeline

on:
  push:
    branches: [ main ]

jobs:
  build:
    runs-on: ubuntu-latest
    
    steps:
    - uses: actions/checkout@v2
    
    - name: Setup .NET
      uses: actions/setup-dotnet@v1
      with:
        dotnet-version: 8.0.x
        
    - name: Build
      run: dotnet build
      
    - name: Test
      run: dotnet test
\`\`\`

### Exercício
Configure um pipeline de CI/CD básico.
      `;
    }
    
    // Conteúdo para Arquitetura (Curso 12)
    else if (courseId === '12') {
      educationalContent = `
## ${lesson.title}

### Arquitetura de Software
${lesson.content}

### Design de Sistemas
Aprenda a projetar sistemas escaláveis e maintíveis.

### Exemplo de Padrão

\`\`\`csharp
// Exemplo de aplicação dos princípios SOLID
public interface INotificationService
{
    Task SendAsync(string message, string recipient);
}

public class EmailService : INotificationService
{
    public async Task SendAsync(string message, string recipient)
    {
        // Implementação de envio de email
        await Task.CompletedTask;
    }
}

public class NotificationManager
{
    private readonly INotificationService _notificationService;
    
    public NotificationManager(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }
    
    public async Task NotifyUserAsync(string message, string user)
    {
        await _notificationService.SendAsync(message, user);
    }
}
\`\`\`

### Exercício
Implemente um padrão arquitetural seguindo os princípios apresentados.
      `;
    }
    
    // Conteúdo padrão para outras aulas
    else {
      educationalContent = `
## ${lesson.title}

### Visão Geral
${lesson.content}

### Objetivos de Aprendizagem
- Dominar os conceitos fundamentais
- Aplicar conhecimentos práticos
- Resolver problemas reais

### Exemplo Prático

\`\`\`csharp
using System;

class Program 
{
    static void Main()
    {
        Console.WriteLine("Exemplo da aula: ${lesson.title}");
        
        // Código específico da aula
        var exemplo = "Aprendendo programação";
        Console.WriteLine($"Conteúdo: {exemplo}");
    }
}
\`\`\`

### Exercício
Complete o código seguindo as instruções da aula.
      `;
    }
    
    return {
      ...lesson,
      content: educationalContent,
      videoUrl: null,
      exercises: [
        {
          id: '1',
          title: 'Exercício Prático',
          description: 'Complete o código seguindo as instruções da aula',
          starterCode: courseId === '2' ? 'SELECT * FROM Clientes;' : 'Console.WriteLine("Seu código aqui");',
          solution: courseId === '2' ? 'SELECT * FROM Clientes ORDER BY Nome;' : 'Console.WriteLine("Hello, World!");'
        }
      ]
    };
  },

  /**
   * Mark a lesson as complete - MOCK VERSION
   */
  completeLesson: async (
    courseId: string,
    lessonId: string,
    data: CompleteLessonRequest
  ): Promise<CompleteLessonResponse> => {
    await new Promise(resolve => setTimeout(resolve, 300));
    
    // Simular sucesso
    return {
      success: true,
      message: 'Aula concluída com sucesso!',
      xpGained: 50,
      nextLessonId: (parseInt(lessonId) + 1).toString(),
      courseProgress: 25
    };
  },
};
