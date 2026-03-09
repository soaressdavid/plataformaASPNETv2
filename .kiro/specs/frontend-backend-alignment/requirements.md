# Frontend-Backend Alignment - Requirements

## Overview
Alinhar o frontend Next.js com a nova estrutura de backend que implementa currículo baseado em níveis e conteúdo estruturado de lições. Atualmente, o frontend consome dados mock com HTML simples, mas o backend possui entidades com LevelId e StructuredContent (JSON).

## Problem Statement
1. **Mock API vs Real Data**: Backend serve dados mock com HTML, mas entidades reais usam StructuredContent JSON
2. **Missing LevelId**: Frontend não conhece o conceito de níveis de currículo (Level 0-15)
3. **Content Structure**: Frontend espera string HTML, mas backend tem conteúdo estruturado (objetivos, teoria, exemplos, exercícios)
4. **New Curriculum Inaccessible**: 40 lições criadas (Level0, Level1) não são acessíveis via API
5. **Missing Fields**: Frontend types não incluem campos como `duration`, `levelId`, `structuredContent`

## Goals
1. Substituir mock API por endpoints reais que consultam o banco de dados
2. Expor estrutura de níveis de currículo no frontend
3. Renderizar conteúdo estruturado de lições (objetivos, teoria, exemplos, exercícios)
4. Adicionar navegação baseada em níveis
5. Manter compatibilidade com dados existentes durante migração

## Functional Requirements

### FR1: Backend API Endpoints
- **FR1.1**: Criar endpoint `GET /api/levels` que retorna todos os níveis de currículo
- **FR1.2**: Criar endpoint `GET /api/levels/{id}` que retorna detalhes de um nível específico
- **FR1.3**: Criar endpoint `GET /api/levels/{id}/courses` que retorna cursos de um nível
- **FR1.4**: Atualizar endpoint `GET /api/courses` para incluir `levelId` e `duration`
- **FR1.5**: Atualizar endpoint `GET /api/courses/{id}` para incluir informações completas do curso
- **FR1.6**: Atualizar endpoint `GET /api/courses/{id}/lessons` para retornar `structuredContent`
- **FR1.7**: Criar endpoint `GET /api/courses/{id}/lessons/{lessonId}` para detalhes de lição individual
- **FR1.8**: Todos os endpoints devem consultar banco de dados real, não mock data

### FR2: Frontend Type Definitions
- **FR2.1**: Criar type `CurriculumLevel` com campos: `id`, `name`, `description`, `order`, `courseCount`
- **FR2.2**: Atualizar `CourseSummary` para incluir: `levelId`, `duration`, `topics`
- **FR2.3**: Atualizar `LessonDetail` para incluir: `duration`, `difficulty`, `estimatedMinutes`, `structuredContent`
- **FR2.4**: Criar type `LessonContent` com: `objectives`, `theory`, `codeExamples`, `exercises`, `summary`
- **FR2.5**: Criar types para `TheorySection`, `CodeExample`, `Exercise`

### FR3: Frontend API Client
- **FR3.1**: Adicionar `levelsApi.getAll()` para buscar todos os níveis
- **FR3.2**: Adicionar `levelsApi.getById(id)` para buscar nível específico
- **FR3.3**: Adicionar `levelsApi.getCourses(id)` para buscar cursos de um nível
- **FR3.4**: Atualizar `coursesApi.getAll()` para retornar novos campos
- **FR3.5**: Adicionar `coursesApi.getById(id)` para detalhes de curso
- **FR3.6**: Atualizar `coursesApi.getLessons(courseId)` para retornar structured content
- **FR3.7**: Adicionar `coursesApi.getLesson(courseId, lessonId)` para lição individual

### FR4: Frontend Components - Structured Content Rendering
- **FR4.1**: Criar componente `LessonObjectives` para renderizar lista de objetivos
- **FR4.2**: Criar componente `TheorySection` para renderizar seções de teoria com markdown
- **FR4.3**: Criar componente `CodeExample` para renderizar exemplos de código com syntax highlighting
- **FR4.4**: Criar componente `ExerciseList` para renderizar exercícios práticos
- **FR4.5**: Criar componente `LessonSummary` para renderizar resumo da lição
- **FR4.6**: Atualizar página de lição para usar componentes de conteúdo estruturado

### FR5: Frontend Pages - Level Navigation
- **FR5.1**: Criar página `/levels` que lista todos os níveis de currículo
- **FR5.2**: Criar página `/levels/[id]` que mostra cursos de um nível específico
- **FR5.3**: Atualizar página `/courses` para incluir filtro por nível
- **FR5.4**: Atualizar página `/courses/[id]` para mostrar informações de nível
- **FR5.5**: Adicionar breadcrumb navigation: Level > Course > Lesson

### FR6: Backward Compatibility
- **FR6.1**: Suportar lições com HTML content (legacy) e structured content (novo)
- **FR6.2**: Componente de lição deve detectar tipo de conteúdo e renderizar apropriadamente
- **FR6.3**: API deve retornar ambos `content` (HTML) e `structuredContent` (JSON) durante migração
- **FR6.4**: Frontend deve preferir `structuredContent` quando disponível, fallback para `content`

## Non-Functional Requirements

### NFR1: Performance
- **NFR1.1**: Endpoints de API devem responder em menos de 200ms para queries simples
- **NFR1.2**: Implementar caching de níveis e cursos (dados raramente mudam)
- **NFR1.3**: Lazy loading de lições (não carregar todas de uma vez)
- **NFR1.4**: Code syntax highlighting deve ser client-side para não bloquear rendering

### NFR2: User Experience
- **NFR2.1**: Transição suave entre HTML content e structured content (sem quebrar UI)
- **NFR2.2**: Loading states para todas as operações de fetch
- **NFR2.3**: Error boundaries para falhas de rendering de conteúdo
- **NFR2.4**: Responsive design para todos os novos componentes

### NFR3: Code Quality
- **NFR3.1**: Todos os novos componentes devem ter TypeScript types completos
- **NFR3.2**: Componentes devem ser testáveis (separar lógica de apresentação)
- **NFR3.3**: Seguir padrões de código existentes no projeto
- **NFR3.4**: Documentar componentes complexos com comentários JSDoc

### NFR4: Maintainability
- **NFR4.1**: Separar lógica de transformação de dados em utils/transformers
- **NFR4.2**: Criar hooks customizados para operações comuns (useLevel, useStructuredLesson)
- **NFR4.3**: Centralizar configuração de syntax highlighting
- **NFR4.4**: Manter backward compatibility por pelo menos 2 releases

## Data Models

### Backend Response Models

```typescript
// Level Response
interface LevelResponse {
  id: string;
  name: string;
  description: string;
  order: number;
  courseCount: number;
  estimatedHours: number;
}

// Course Response (updated)
interface CourseResponse {
  id: string;
  title: string;
  description: string;
  level: string; // enum: Beginner, Intermediate, Advanced
  levelId?: string; // NEW: link to curriculum level
  duration?: string; // NEW
  lessonCount: number;
  topics?: string[]; // NEW: parsed from JSON
  orderIndex: number; // NEW
}

// Lesson Response (updated)
interface LessonResponse {
  id: string;
  title: string;
  content?: string; // LEGACY: HTML content
  structuredContent?: LessonContentResponse; // NEW: structured content
  order: number;
  isCompleted: boolean;
  duration?: string; // NEW
  difficulty?: string; // NEW: Easy, Medium, Hard
  estimatedMinutes?: number; // NEW
  prerequisites?: string[]; // NEW
}

// Structured Content
interface LessonContentResponse {
  objectives: string[];
  theory: TheorySectionResponse[];
  codeExamples: CodeExampleResponse[];
  exercises: ExerciseResponse[];
  summary: string;
}

interface TheorySectionResponse {
  title: string;
  content: string; // markdown
  order: number;
}

interface CodeExampleResponse {
  title: string;
  code: string;
  language: string;
  explanation: string;
  order: number;
}

interface ExerciseResponse {
  title: string;
  description: string;
  difficulty: string;
  starterCode?: string;
  solution?: string;
  hints?: string[];
  order: number;
}
```

## User Stories

### US1: Como estudante, quero ver todos os níveis de currículo
**Acceptance Criteria:**
- Posso acessar página `/levels`
- Vejo lista de níveis ordenados (Level 0 a Level 15)
- Cada nível mostra: nome, descrição, número de cursos, horas estimadas
- Posso clicar em um nível para ver seus cursos

### US2: Como estudante, quero navegar cursos por nível
**Acceptance Criteria:**
- Na página `/levels/[id]`, vejo todos os cursos daquele nível
- Cursos mostram: título, descrição, duração, número de lições
- Posso clicar em um curso para ver suas lições
- Breadcrumb mostra: Níveis > [Nome do Nível] > Cursos

### US3: Como estudante, quero ver conteúdo estruturado de lições
**Acceptance Criteria:**
- Lição mostra seção de objetivos claramente
- Teoria é renderizada com formatação markdown
- Exemplos de código têm syntax highlighting
- Exercícios são listados com dificuldade e descrição
- Resumo aparece ao final da lição

### US4: Como estudante, quero que lições antigas ainda funcionem
**Acceptance Criteria:**
- Lições com HTML content (legacy) ainda são exibidas corretamente
- Não há erro ou quebra de UI ao acessar lições antigas
- Sistema detecta automaticamente tipo de conteúdo

### US5: Como desenvolvedor, quero APIs que consultam dados reais
**Acceptance Criteria:**
- Endpoints retornam dados do banco de dados
- Dados incluem lições criadas pelos seeders (Level0, Level1)
- Performance é aceitável (< 200ms)
- Erros são tratados apropriadamente

## Constraints
1. **Technology Stack**: Backend ASP.NET Core, Frontend Next.js 14+ com TypeScript
2. **Database**: Usar Entity Framework Core existente
3. **No Breaking Changes**: Não quebrar funcionalidade existente durante migração
4. **Portuguese Content**: Todo conteúdo de lições está em português
5. **Existing Seeders**: Usar Level0ContentSeeder e Level1ContentSeeder já criados

## Assumptions
1. Banco de dados já tem tabelas Courses, Lessons, Levels configuradas
2. Entity Framework migrations já foram aplicadas
3. Seeders Level0 e Level1 já foram executados
4. Frontend tem biblioteca de syntax highlighting disponível (ou será adicionada)
5. Markdown rendering library está disponível no frontend

## Success Criteria
1. ✅ Todos os endpoints mock substituídos por queries reais ao banco
2. ✅ Frontend consegue listar e navegar por níveis de currículo
3. ✅ Lições com structured content são renderizadas corretamente
4. ✅ Lições legacy (HTML) continuam funcionando
5. ✅ 40 lições criadas (Level0 + Level1) são acessíveis via frontend
6. ✅ Performance de APIs está dentro do esperado (< 200ms)
7. ✅ Todos os tipos TypeScript estão corretos e completos
8. ✅ Componentes são reutilizáveis e testáveis

## Out of Scope
1. Migração de lições HTML existentes para structured content (será feito depois)
2. Editor de conteúdo estruturado (admin interface)
3. Versionamento de conteúdo de lições
4. Tradução de conteúdo para outros idiomas
5. Implementação de busca/filtro avançado
6. Analytics de progresso por nível
7. Gamificação específica por nível

## Dependencies
1. **curriculum-expansion spec**: Depende dos seeders Level0-Level15 estarem completos
2. **Database schema**: Tabelas Levels, Courses, Lessons devem existir
3. **Frontend libraries**: Syntax highlighting (ex: Prism.js, highlight.js) e markdown (ex: react-markdown)

## Risks
1. **Performance**: Queries complexas com joins podem ser lentas - mitigar com caching e indexação
2. **Data Migration**: Lições existentes podem ter formato inconsistente - validar durante migração
3. **Breaking Changes**: Mudanças em types podem quebrar código existente - fazer migração gradual
4. **Content Quality**: Structured content pode ter erros de formatação - validar no seeder
5. **Browser Compatibility**: Syntax highlighting pode não funcionar em browsers antigos - usar polyfills

## Next Steps
Após aprovação dos requisitos:
1. Criar documento de design (design.md)
2. Definir arquitetura de componentes
3. Especificar estrutura de pastas
4. Criar tasks detalhadas (tasks.md)
