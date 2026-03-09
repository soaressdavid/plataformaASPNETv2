# Frontend-Backend Alignment - Design

## Architecture Overview

This design implements a three-layer architecture:
1. **Backend API Layer**: ASP.NET Core endpoints querying EF Core
2. **Frontend API Client Layer**: TypeScript API clients with type safety
3. **Frontend UI Layer**: Next.js pages and React components

### High-Level Flow
```
User → Next.js Page → API Client → Backend API → EF Core → Database
                                                              ↓
User ← React Component ← Transformed Data ← JSON Response ← Query Result
```

## Backend Design

### API Endpoints Structure

#### Levels Endpoints
```
GET /api/levels
GET /api/levels/{id}
GET /api/levels/{id}/courses
```

#### Courses Endpoints (Updated)
```
GET /api/courses
GET /api/courses/{id}
GET /api/courses/{id}/lessons
GET /api/courses/{id}/lessons/{lessonId}
POST /api/courses/{courseId}/lessons/{lessonId}/complete
```

### Backend Implementation Strategy


#### 1. Levels Controller (`src/Services/Course/Controllers/LevelsController.cs`)

**Responsibilities:**
- Query CurriculumLevel entities
- Return level information with course counts
- Support filtering and pagination

**Key Methods:**
```csharp
[HttpGet]
public async Task<ActionResult<LevelListResponse>> GetAll()
{
    var levels = await _context.CurriculumLevels
        .Include(l => l.Courses)
        .OrderBy(l => l.Number)
        .ToListAsync();
    
    return Ok(new LevelListResponse
    {
        Levels = levels.Select(MapToLevelDto).ToList()
    });
}

[HttpGet("{id}")]
public async Task<ActionResult<LevelDetailResponse>> GetById(Guid id)
{
    var level = await _context.CurriculumLevels
        .Include(l => l.Courses)
        .Include(l => l.Project)
        .FirstOrDefaultAsync(l => l.Id == id);
    
    if (level == null) return NotFound();
    
    return Ok(MapToLevelDetailDto(level));
}

[HttpGet("{id}/courses")]
public async Task<ActionResult<CourseListResponse>> GetCourses(Guid id)
{
    var courses = await _context.Courses
        .Where(c => c.LevelId == id)
        .OrderBy(c => c.OrderIndex)
        .ToListAsync();
    
    return Ok(new CourseListResponse
    {
        Courses = courses.Select(MapToCourseDto).ToList()
    });
}
```


#### 2. Courses Controller Updates (`src/Services/Course/Controllers/CoursesController.cs`)

**Replace mock endpoints with real database queries:**

```csharp
[HttpGet]
public async Task<ActionResult<CourseListResponse>> GetAll(
    [FromQuery] Guid? levelId = null,
    [FromQuery] string? level = null)
{
    var query = _context.Courses.AsQueryable();
    
    if (levelId.HasValue)
        query = query.Where(c => c.LevelId == levelId);
    
    if (!string.IsNullOrEmpty(level))
        query = query.Where(c => c.Level.ToString() == level);
    
    var courses = await query
        .OrderBy(c => c.OrderIndex)
        .ToListAsync();
    
    return Ok(new CourseListResponse
    {
        Courses = courses.Select(MapToCourseDto).ToList()
    });
}

[HttpGet("{id}")]
public async Task<ActionResult<CourseDetailResponse>> GetById(Guid id)
{
    var course = await _context.Courses
        .Include(c => c.CurriculumLevel)
        .Include(c => c.Lessons.OrderBy(l => l.OrderIndex))
        .FirstOrDefaultAsync(c => c.Id == id);
    
    if (course == null) return NotFound();
    
    return Ok(MapToCourseDetailDto(course));
}

[HttpGet("{id}/lessons")]
public async Task<ActionResult<LessonListResponse>> GetLessons(Guid id)
{
    var lessons = await _context.Lessons
        .Where(l => l.CourseId == id)
        .OrderBy(l => l.OrderIndex)
        .ToListAsync();
    
    return Ok(new LessonListResponse
    {
        Lessons = lessons.Select(MapToLessonDto).ToList()
    });
}

[HttpGet("{courseId}/lessons/{lessonId}")]
public async Task<ActionResult<LessonDetailResponse>> GetLesson(
    Guid courseId, 
    Guid lessonId)
{
    var lesson = await _context.Lessons
        .Include(l => l.Course)
        .FirstOrDefaultAsync(l => l.Id == lessonId && l.CourseId == courseId);
    
    if (lesson == null) return NotFound();
    
    return Ok(MapToLessonDetailDto(lesson));
}
```


#### 3. DTOs and Mapping (`src/Services/Course/DTOs/`)

**Response DTOs:**

```csharp
// LevelDtos.cs
public record LevelDto(
    Guid Id,
    int Number,
    string Title,
    string Description,
    int RequiredXP,
    int CourseCount,
    int EstimatedHours
);

public record LevelDetailDto(
    Guid Id,
    int Number,
    string Title,
    string Description,
    int RequiredXP,
    List<CourseSummaryDto> Courses,
    ProjectSummaryDto? Project
);

public record LevelListResponse(List<LevelDto> Levels);

// CourseDtos.cs
public record CourseSummaryDto(
    Guid Id,
    string Title,
    string Description,
    string Level,
    Guid? LevelId,
    string Duration,
    int LessonCount,
    List<string> Topics,
    int OrderIndex
);

public record CourseDetailDto(
    Guid Id,
    string Title,
    string Description,
    string Level,
    Guid? LevelId,
    string LevelTitle,
    string Duration,
    int LessonCount,
    List<string> Topics,
    List<LessonSummaryDto> Lessons
);

public record CourseListResponse(List<CourseSummaryDto> Courses);

// LessonDtos.cs
public record LessonSummaryDto(
    Guid Id,
    string Title,
    string Duration,
    string Difficulty,
    int EstimatedMinutes,
    int Order,
    bool IsCompleted
);

public record LessonDetailDto(
    Guid Id,
    string Title,
    string? Content,
    LessonContentDto? StructuredContent,
    string Duration,
    string Difficulty,
    int EstimatedMinutes,
    List<string> Prerequisites,
    int Order,
    bool IsCompleted
);

public record LessonListResponse(List<LessonSummaryDto> Lessons);
```


**Structured Content DTOs:**

```csharp
// LessonContentDtos.cs
public record LessonContentDto(
    List<string> Objectives,
    List<TheorySectionDto> Theory,
    List<CodeExampleDto> CodeExamples,
    List<ExerciseDto> Exercises,
    string Summary
);

public record TheorySectionDto(
    string Heading,
    string Content,
    int Order
);

public record CodeExampleDto(
    string Title,
    string Code,
    string Language,
    string Explanation,
    bool IsRunnable
);

public record ExerciseDto(
    string Title,
    string Description,
    string Difficulty,
    string StarterCode,
    List<string> Hints
);
```

**Mapping Extensions:**

```csharp
// MappingExtensions.cs
public static class MappingExtensions
{
    public static LevelDto MapToLevelDto(this CurriculumLevel level)
    {
        return new LevelDto(
            level.Id,
            level.Number,
            level.Title,
            level.Description,
            level.RequiredXP,
            level.Courses.Count,
            EstimateHours(level.Courses)
        );
    }
    
    public static CourseSummaryDto MapToCourseDto(this Course course)
    {
        var topics = JsonSerializer.Deserialize<List<string>>(course.Topics) 
            ?? new List<string>();
        
        return new CourseSummaryDto(
            course.Id,
            course.Title,
            course.Description,
            course.Level.ToString(),
            course.LevelId,
            course.Duration,
            course.LessonCount,
            topics,
            course.OrderIndex
        );
    }
    
    public static LessonDetailDto MapToLessonDetailDto(this Lesson lesson)
    {
        LessonContentDto? structuredContent = null;
        
        if (!string.IsNullOrEmpty(lesson.StructuredContent))
        {
            var content = JsonSerializer.Deserialize<LessonContent>(
                lesson.StructuredContent);
            structuredContent = content?.MapToDto();
        }
        
        var prerequisites = JsonSerializer.Deserialize<List<string>>(
            lesson.Prerequisites) ?? new List<string>();
        
        return new LessonDetailDto(
            lesson.Id,
            lesson.Title,
            lesson.Content,
            structuredContent,
            lesson.Duration,
            lesson.Difficulty,
            lesson.EstimatedMinutes,
            prerequisites,
            lesson.OrderIndex,
            false // TODO: Check user completion status
        );
    }
    
    public static LessonContentDto MapToDto(this LessonContent content)
    {
        return new LessonContentDto(
            content.Objectives,
            content.Theory.Select(t => new TheorySectionDto(
                t.Heading, t.Content, t.Order)).ToList(),
            content.CodeExamples.Select(c => new CodeExampleDto(
                c.Title, c.Code, c.Language, c.Explanation, c.IsRunnable)).ToList(),
            content.Exercises.Select(e => new ExerciseDto(
                e.Title, e.Description, e.Difficulty.ToString(), 
                e.StarterCode, e.Hints)).ToList(),
            content.Summary
        );
    }
}
```


#### 4. Caching Strategy

**Implementation using IMemoryCache:**

```csharp
public class CachedLevelsService
{
    private readonly IMemoryCache _cache;
    private readonly AppDbContext _context;
    private const string LEVELS_CACHE_KEY = "curriculum_levels";
    private static readonly TimeSpan CacheDuration = TimeSpan.FromHours(24);
    
    public async Task<List<CurriculumLevel>> GetAllLevelsAsync()
    {
        if (!_cache.TryGetValue(LEVELS_CACHE_KEY, out List<CurriculumLevel> levels))
        {
            levels = await _context.CurriculumLevels
                .Include(l => l.Courses)
                .OrderBy(l => l.Number)
                .ToListAsync();
            
            _cache.Set(LEVELS_CACHE_KEY, levels, CacheDuration);
        }
        
        return levels;
    }
    
    public void InvalidateCache()
    {
        _cache.Remove(LEVELS_CACHE_KEY);
    }
}
```

**Cache invalidation triggers:**
- Admin updates curriculum levels
- New courses added to levels
- Manual cache clear endpoint (admin only)


## Frontend Design

### Type Definitions (`frontend/lib/types.ts`)

**New Types:**

```typescript
// Curriculum Level Types
export interface CurriculumLevel {
  id: string;
  number: number;
  title: string;
  description: string;
  requiredXP: number;
  courseCount: number;
  estimatedHours: number;
}

export interface CurriculumLevelDetail extends CurriculumLevel {
  courses: CourseSummary[];
  project?: ProjectSummary;
}

export interface LevelListResponse {
  levels: CurriculumLevel[];
}

// Updated Course Types
export interface CourseSummary {
  id: string;
  title: string;
  description: string;
  level: Level;
  levelId?: string; // NEW
  duration?: string; // NEW
  lessonCount: number;
  topics?: string[]; // NEW
  orderIndex: number; // NEW
}

export interface CourseDetail extends CourseSummary {
  levelTitle?: string; // NEW
  lessons: LessonSummary[];
}

// Updated Lesson Types
export interface LessonSummary {
  id: string;
  title: string;
  duration?: string; // NEW
  difficulty?: string; // NEW
  estimatedMinutes?: number; // NEW
  order: number;
  isCompleted: boolean;
}

export interface LessonDetail extends LessonSummary {
  content?: string; // LEGACY: HTML content
  structuredContent?: LessonContent; // NEW: structured content
  prerequisites?: string[]; // NEW
}

// Structured Content Types
export interface LessonContent {
  objectives: string[];
  theory: TheorySection[];
  codeExamples: CodeExample[];
  exercises: Exercise[];
  summary: string;
}

export interface TheorySection {
  heading: string;
  content: string; // markdown
  order: number;
}

export interface CodeExample {
  title: string;
  code: string;
  language: string;
  explanation: string;
  isRunnable: boolean;
}

export interface Exercise {
  title: string;
  description: string;
  difficulty: 'Fácil' | 'Médio' | 'Difícil';
  starterCode: string;
  hints: string[];
}
```


### API Client Layer (`frontend/lib/api/`)

#### Levels API Client (`frontend/lib/api/levels.ts`)

```typescript
import apiClient from '../api-client';
import { LevelListResponse, CurriculumLevelDetail, CourseListResponse } from '../types';

export const levelsApi = {
  /**
   * Get all curriculum levels
   */
  getAll: async (): Promise<LevelListResponse> => {
    const response = await apiClient.get<LevelListResponse>('/api/levels');
    return response.data;
  },

  /**
   * Get a specific level with its courses
   */
  getById: async (id: string): Promise<CurriculumLevelDetail> => {
    const response = await apiClient.get<CurriculumLevelDetail>(`/api/levels/${id}`);
    return response.data;
  },

  /**
   * Get all courses for a specific level
   */
  getCourses: async (id: string): Promise<CourseListResponse> => {
    const response = await apiClient.get<CourseListResponse>(`/api/levels/${id}/courses`);
    return response.data;
  },
};
```

#### Updated Courses API Client (`frontend/lib/api/courses.ts`)

```typescript
import apiClient from '../api-client';
import {
  CourseListResponse,
  CourseDetail,
  LessonListResponse,
  LessonDetail,
  CompleteLessonRequest,
  CompleteLessonResponse,
} from '../types';

export const coursesApi = {
  /**
   * Get all courses with optional filters
   */
  getAll: async (filters?: {
    levelId?: string;
    level?: string;
  }): Promise<CourseListResponse> => {
    const params = new URLSearchParams();
    if (filters?.levelId) params.append('levelId', filters.levelId);
    if (filters?.level) params.append('level', filters.level);
    
    const response = await apiClient.get<CourseListResponse>(
      `/api/courses?${params.toString()}`
    );
    return response.data;
  },

  /**
   * Get a specific course with details
   */
  getById: async (id: string): Promise<CourseDetail> => {
    const response = await apiClient.get<CourseDetail>(`/api/courses/${id}`);
    return response.data;
  },

  /**
   * Get lessons for a specific course
   */
  getLessons: async (courseId: string): Promise<LessonListResponse> => {
    const response = await apiClient.get<LessonListResponse>(
      `/api/courses/${courseId}/lessons`
    );
    return response.data;
  },

  /**
   * Get a specific lesson with full content
   */
  getLesson: async (courseId: string, lessonId: string): Promise<LessonDetail> => {
    const response = await apiClient.get<LessonDetail>(
      `/api/courses/${courseId}/lessons/${lessonId}`
    );
    return response.data;
  },

  /**
   * Mark a lesson as complete
   */
  completeLesson: async (
    courseId: string,
    lessonId: string,
    data: CompleteLessonRequest
  ): Promise<CompleteLessonResponse> => {
    const response = await apiClient.post<CompleteLessonResponse>(
      `/api/courses/${courseId}/lessons/${lessonId}/complete`,
      data
    );
    return response.data;
  },
};
```


### Custom Hooks (`frontend/lib/hooks/`)

#### useLevel Hook (`frontend/lib/hooks/useLevel.ts`)

```typescript
import { useState, useEffect } from 'react';
import { levelsApi } from '../api';
import { CurriculumLevel, CurriculumLevelDetail } from '../types';

export function useLevels() {
  const [levels, setLevels] = useState<CurriculumLevel[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const fetchLevels = async () => {
      try {
        setLoading(true);
        const response = await levelsApi.getAll();
        setLevels(response.levels);
      } catch (err) {
        setError(err instanceof Error ? err.message : 'Failed to load levels');
      } finally {
        setLoading(false);
      }
    };

    fetchLevels();
  }, []);

  return { levels, loading, error };
}

export function useLevel(id: string) {
  const [level, setLevel] = useState<CurriculumLevelDetail | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const fetchLevel = async () => {
      try {
        setLoading(true);
        const data = await levelsApi.getById(id);
        setLevel(data);
      } catch (err) {
        setError(err instanceof Error ? err.message : 'Failed to load level');
      } finally {
        setLoading(false);
      }
    };

    if (id) fetchLevel();
  }, [id]);

  return { level, loading, error };
}
```

#### useStructuredLesson Hook (`frontend/lib/hooks/useStructuredLesson.ts`)

```typescript
import { useState, useEffect } from 'react';
import { coursesApi } from '../api';
import { LessonDetail } from '../types';

export function useStructuredLesson(courseId: string, lessonId: string) {
  const [lesson, setLesson] = useState<LessonDetail | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [contentType, setContentType] = useState<'structured' | 'html' | null>(null);

  useEffect(() => {
    const fetchLesson = async () => {
      try {
        setLoading(true);
        const data = await coursesApi.getLesson(courseId, lessonId);
        setLesson(data);
        
        // Determine content type
        if (data.structuredContent) {
          setContentType('structured');
        } else if (data.content) {
          setContentType('html');
        }
      } catch (err) {
        setError(err instanceof Error ? err.message : 'Failed to load lesson');
      } finally {
        setLoading(false);
      }
    };

    if (courseId && lessonId) fetchLesson();
  }, [courseId, lessonId]);

  return { lesson, loading, error, contentType };
}
```


### Component Architecture

#### Component Hierarchy

```
App
├── Levels Page (/levels)
│   └── LevelCard (multiple)
│       └── LevelProgress
│
├── Level Detail Page (/levels/[id])
│   ├── LevelHeader
│   ├── CourseGrid
│   │   └── CourseCard (multiple)
│   └── CapstoneProject
│
├── Course Detail Page (/courses/[id])
│   ├── CourseHeader (updated with level info)
│   ├── LessonList
│   │   └── LessonCard (multiple)
│   └── Breadcrumb (Level > Course)
│
└── Lesson Page (/courses/[id]/lessons/[lessonId])
    ├── Breadcrumb (Level > Course > Lesson)
    ├── LessonContent (smart component)
    │   ├── StructuredLessonView (if structured content)
    │   │   ├── LessonObjectives
    │   │   ├── TheorySection (multiple)
    │   │   ├── CodeExample (multiple)
    │   │   ├── ExerciseList
    │   │   └── LessonSummary
    │   └── HtmlLessonView (if legacy HTML)
    └── LessonNavigation
```


### Component Specifications

#### 1. Structured Content Components

**LessonObjectives Component** (`frontend/lib/components/LessonObjectives.tsx`)

```typescript
interface LessonObjectivesProps {
  objectives: string[];
}

export function LessonObjectives({ objectives }: LessonObjectivesProps) {
  return (
    <div className="objectives-section">
      <h2>Objetivos de Aprendizagem</h2>
      <ul className="objectives-list">
        {objectives.map((objective, index) => (
          <li key={index} className="objective-item">
            <CheckIcon />
            <span>{objective}</span>
          </li>
        ))}
      </ul>
    </div>
  );
}
```

**TheorySection Component** (`frontend/lib/components/TheorySection.tsx`)

```typescript
import ReactMarkdown from 'react-markdown';

interface TheorySectionProps {
  heading: string;
  content: string;
  order: number;
}

export function TheorySection({ heading, content }: TheorySectionProps) {
  return (
    <section className="theory-section">
      <h3>{heading}</h3>
      <div className="theory-content">
        <ReactMarkdown>{content}</ReactMarkdown>
      </div>
    </section>
  );
}
```

**CodeExample Component** (`frontend/lib/components/CodeExample.tsx`)

```typescript
import { Prism as SyntaxHighlighter } from 'react-syntax-highlighter';
import { vscDarkPlus } from 'react-syntax-highlighter/dist/cjs/styles/prism';

interface CodeExampleProps {
  title: string;
  code: string;
  language: string;
  explanation: string;
  isRunnable: boolean;
  onRun?: (code: string) => void;
}

export function CodeExample({
  title,
  code,
  language,
  explanation,
  isRunnable,
  onRun,
}: CodeExampleProps) {
  return (
    <div className="code-example">
      <div className="code-example-header">
        <h4>{title}</h4>
        {isRunnable && onRun && (
          <button onClick={() => onRun(code)} className="run-button">
            Executar
          </button>
        )}
      </div>
      
      <SyntaxHighlighter
        language={language}
        style={vscDarkPlus}
        showLineNumbers
      >
        {code}
      </SyntaxHighlighter>
      
      <p className="code-explanation">{explanation}</p>
    </div>
  );
}
```

**ExerciseList Component** (`frontend/lib/components/ExerciseList.tsx`)

```typescript
interface ExerciseListProps {
  exercises: Exercise[];
  onStartExercise?: (exercise: Exercise) => void;
}

export function ExerciseList({ exercises, onStartExercise }: ExerciseListProps) {
  return (
    <div className="exercises-section">
      <h2>Exercícios Práticos</h2>
      <div className="exercises-grid">
        {exercises.map((exercise, index) => (
          <ExerciseCard
            key={index}
            exercise={exercise}
            onStart={onStartExercise}
          />
        ))}
      </div>
    </div>
  );
}

function ExerciseCard({ exercise, onStart }: {
  exercise: Exercise;
  onStart?: (exercise: Exercise) => void;
}) {
  return (
    <div className="exercise-card">
      <div className="exercise-header">
        <h4>{exercise.title}</h4>
        <span className={`difficulty-badge ${exercise.difficulty.toLowerCase()}`}>
          {exercise.difficulty}
        </span>
      </div>
      <p>{exercise.description}</p>
      {exercise.hints.length > 0 && (
        <details className="hints">
          <summary>Dicas ({exercise.hints.length})</summary>
          <ul>
            {exercise.hints.map((hint, i) => (
              <li key={i}>{hint}</li>
            ))}
          </ul>
        </details>
      )}
      {onStart && (
        <button onClick={() => onStart(exercise)} className="start-button">
          Começar Exercício
        </button>
      )}
    </div>
  );
}
```


**StructuredLessonView Component** (`frontend/lib/components/StructuredLessonView.tsx`)

```typescript
interface StructuredLessonViewProps {
  content: LessonContent;
  onRunCode?: (code: string) => void;
  onStartExercise?: (exercise: Exercise) => void;
}

export function StructuredLessonView({
  content,
  onRunCode,
  onStartExercise,
}: StructuredLessonViewProps) {
  return (
    <div className="structured-lesson">
      <LessonObjectives objectives={content.objectives} />
      
      <div className="theory-sections">
        {content.theory
          .sort((a, b) => a.order - b.order)
          .map((section, index) => (
            <TheorySection
              key={index}
              heading={section.heading}
              content={section.content}
              order={section.order}
            />
          ))}
      </div>
      
      <div className="code-examples">
        <h2>Exemplos de Código</h2>
        {content.codeExamples.map((example, index) => (
          <CodeExample
            key={index}
            {...example}
            onRun={onRunCode}
          />
        ))}
      </div>
      
      <ExerciseList
        exercises={content.exercises}
        onStartExercise={onStartExercise}
      />
      
      {content.summary && (
        <div className="lesson-summary">
          <h2>Resumo</h2>
          <p>{content.summary}</p>
        </div>
      )}
    </div>
  );
}
```

**LessonContent Smart Component** (`frontend/lib/components/LessonContent.tsx`)

```typescript
interface LessonContentProps {
  lesson: LessonDetail;
  onRunCode?: (code: string) => void;
  onStartExercise?: (exercise: Exercise) => void;
}

export function LessonContent({
  lesson,
  onRunCode,
  onStartExercise,
}: LessonContentProps) {
  // Prefer structured content over HTML
  if (lesson.structuredContent) {
    return (
      <StructuredLessonView
        content={lesson.structuredContent}
        onRunCode={onRunCode}
        onStartExercise={onStartExercise}
      />
    );
  }
  
  // Fallback to HTML content
  if (lesson.content) {
    return (
      <div
        className="html-lesson-content"
        dangerouslySetInnerHTML={{ __html: lesson.content }}
      />
    );
  }
  
  // No content available
  return (
    <div className="no-content">
      <p>Conteúdo não disponível.</p>
    </div>
  );
}
```


#### 2. Level Navigation Components

**LevelCard Component** (`frontend/lib/components/LevelCard.tsx`)

```typescript
interface LevelCardProps {
  level: CurriculumLevel;
  isLocked?: boolean;
  progress?: number;
}

export function LevelCard({ level, isLocked, progress }: LevelCardProps) {
  return (
    <Link
      href={isLocked ? '#' : `/levels/${level.id}`}
      className={`level-card ${isLocked ? 'locked' : ''}`}
    >
      <div className="level-number">Nível {level.number}</div>
      <h3>{level.title}</h3>
      <p>{level.description}</p>
      
      <div className="level-stats">
        <span>{level.courseCount} cursos</span>
        <span>{level.estimatedHours}h estimadas</span>
      </div>
      
      {isLocked && (
        <div className="lock-overlay">
          <LockIcon />
          <span>Requer {level.requiredXP} XP</span>
        </div>
      )}
      
      {progress !== undefined && !isLocked && (
        <div className="progress-bar">
          <div
            className="progress-fill"
            style={{ width: `${progress}%` }}
          />
        </div>
      )}
    </Link>
  );
}
```

**Breadcrumb Component** (`frontend/lib/components/Breadcrumb.tsx`)

```typescript
interface BreadcrumbItem {
  label: string;
  href?: string;
}

interface BreadcrumbProps {
  items: BreadcrumbItem[];
}

export function Breadcrumb({ items }: BreadcrumbProps) {
  return (
    <nav className="breadcrumb">
      {items.map((item, index) => (
        <span key={index} className="breadcrumb-item">
          {item.href ? (
            <Link href={item.href}>{item.label}</Link>
          ) : (
            <span>{item.label}</span>
          )}
          {index < items.length - 1 && (
            <ChevronRightIcon className="separator" />
          )}
        </span>
      ))}
    </nav>
  );
}
```


### Page Implementations

#### Levels Page (`frontend/app/levels/page.tsx`)

```typescript
'use client';

import { useLevels } from '@/lib/hooks/useLevel';
import { LevelCard } from '@/lib/components/LevelCard';
import { LoadingSkeletons } from '@/lib/components/LoadingSkeletons';

export default function LevelsPage() {
  const { levels, loading, error } = useLevels();

  if (loading) return <LoadingSkeletons count={6} />;
  if (error) return <div>Erro: {error}</div>;

  return (
    <div className="levels-page">
      <header className="page-header">
        <h1>Currículo de Aprendizagem</h1>
        <p>Progrida através de 16 níveis de conhecimento em ASP.NET</p>
      </header>

      <div className="levels-grid">
        {levels.map((level) => (
          <LevelCard
            key={level.id}
            level={level}
            isLocked={false} // TODO: Check user XP
            progress={0} // TODO: Calculate progress
          />
        ))}
      </div>
    </div>
  );
}
```

#### Level Detail Page (`frontend/app/levels/[id]/page.tsx`)

```typescript
'use client';

import { useLevel } from '@/lib/hooks/useLevel';
import { CourseCard } from '@/lib/components/CourseCard';
import { Breadcrumb } from '@/lib/components/Breadcrumb';

export default function LevelDetailPage({ params }: { params: { id: string } }) {
  const { level, loading, error } = useLevel(params.id);

  if (loading) return <LoadingSkeletons count={4} />;
  if (error) return <div>Erro: {error}</div>;
  if (!level) return <div>Nível não encontrado</div>;

  return (
    <div className="level-detail-page">
      <Breadcrumb
        items={[
          { label: 'Níveis', href: '/levels' },
          { label: level.title },
        ]}
      />

      <header className="level-header">
        <div className="level-badge">Nível {level.number}</div>
        <h1>{level.title}</h1>
        <p>{level.description}</p>
        <div className="level-meta">
          <span>{level.courseCount} cursos</span>
          <span>{level.estimatedHours}h estimadas</span>
          <span>{level.requiredXP} XP necessário</span>
        </div>
      </header>

      <section className="courses-section">
        <h2>Cursos</h2>
        <div className="courses-grid">
          {level.courses.map((course) => (
            <CourseCard key={course.id} course={course} />
          ))}
        </div>
      </section>

      {level.project && (
        <section className="capstone-section">
          <h2>Projeto Capstone</h2>
          <ProjectCard project={level.project} />
        </section>
      )}
    </div>
  );
}
```


#### Updated Lesson Page (`frontend/app/courses/[id]/lessons/[lessonId]/page.tsx`)

```typescript
'use client';

import { useStructuredLesson } from '@/lib/hooks/useStructuredLesson';
import { LessonContent } from '@/lib/components/LessonContent';
import { Breadcrumb } from '@/lib/components/Breadcrumb';
import { useCodeExecution } from '@/lib/hooks/useCodeExecution';

export default function LessonPage({
  params,
}: {
  params: { id: string; lessonId: string };
}) {
  const { lesson, loading, error, contentType } = useStructuredLesson(
    params.id,
    params.lessonId
  );
  const { executeCode } = useCodeExecution();

  if (loading) return <LoadingSkeletons count={3} />;
  if (error) return <div>Erro: {error}</div>;
  if (!lesson) return <div>Lição não encontrada</div>;

  const handleRunCode = async (code: string) => {
    await executeCode({
      code,
      files: [],
      entryPoint: 'Program.cs',
    });
  };

  const handleStartExercise = (exercise: Exercise) => {
    // Navigate to IDE with starter code
    router.push(`/ide?exercise=${encodeURIComponent(exercise.title)}`);
  };

  return (
    <div className="lesson-page">
      <Breadcrumb
        items={[
          { label: 'Níveis', href: '/levels' },
          { label: 'Curso', href: `/courses/${params.id}` }, // TODO: Get course title
          { label: lesson.title },
        ]}
      />

      <header className="lesson-header">
        <h1>{lesson.title}</h1>
        <div className="lesson-meta">
          {lesson.duration && <span>{lesson.duration}</span>}
          {lesson.difficulty && (
            <span className={`difficulty ${lesson.difficulty.toLowerCase()}`}>
              {lesson.difficulty}
            </span>
          )}
          {lesson.estimatedMinutes && (
            <span>{lesson.estimatedMinutes} minutos</span>
          )}
        </div>
      </header>

      <LessonContent
        lesson={lesson}
        onRunCode={handleRunCode}
        onStartExercise={handleStartExercise}
      />

      <footer className="lesson-footer">
        <button className="complete-button">
          Marcar como Concluída
        </button>
        <button className="next-button">
          Próxima Lição
        </button>
      </footer>
    </div>
  );
}
```


## Data Flow Diagrams

### Level Navigation Flow

```
User clicks "Níveis" in nav
    ↓
LevelsPage component mounts
    ↓
useLevels hook calls levelsApi.getAll()
    ↓
API Client: GET /api/levels
    ↓
Backend: LevelsController.GetAll()
    ↓
EF Core queries CurriculumLevels table
    ↓
Backend maps entities to DTOs
    ↓
JSON response: { levels: [...] }
    ↓
Frontend updates state
    ↓
Renders LevelCard components
```

### Structured Lesson Rendering Flow

```
User navigates to lesson page
    ↓
LessonPage component mounts
    ↓
useStructuredLesson hook calls coursesApi.getLesson()
    ↓
API Client: GET /api/courses/{id}/lessons/{lessonId}
    ↓
Backend: CoursesController.GetLesson()
    ↓
EF Core queries Lessons table
    ↓
Backend deserializes StructuredContent JSON
    ↓
Backend maps to LessonDetailDto
    ↓
JSON response with structuredContent
    ↓
Frontend detects content type
    ↓
LessonContent component chooses renderer
    ↓
StructuredLessonView renders components:
    - LessonObjectives
    - TheorySection (with markdown)
    - CodeExample (with syntax highlighting)
    - ExerciseList
    - LessonSummary
```


## Error Handling Strategy

### Backend Error Responses

```csharp
// Global exception handler
app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        var error = context.Features.Get<IExceptionHandlerFeature>();
        var statusCode = error?.Error switch
        {
            NotFoundException => 404,
            ValidationException => 400,
            UnauthorizedException => 401,
            _ => 500
        };
        
        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";
        
        await context.Response.WriteAsJsonAsync(new
        {
            error = error?.Error.Message ?? "An error occurred",
            statusCode
        });
    });
});
```

### Frontend Error Handling

```typescript
// API Client error interceptor
apiClient.interceptors.response.use(
  (response) => response,
  (error) => {
    if (error.response?.status === 404) {
      // Handle not found
      return Promise.reject(new Error('Recurso não encontrado'));
    }
    if (error.response?.status === 401) {
      // Redirect to login
      window.location.href = '/login';
    }
    return Promise.reject(error);
  }
);

// Component error boundaries
export class LessonErrorBoundary extends React.Component {
  state = { hasError: false, error: null };
  
  static getDerivedStateFromError(error: Error) {
    return { hasError: true, error };
  }
  
  render() {
    if (this.state.hasError) {
      return (
        <div className="error-state">
          <h2>Erro ao carregar lição</h2>
          <p>{this.state.error?.message}</p>
          <button onClick={() => window.location.reload()}>
            Tentar Novamente
          </button>
        </div>
      );
    }
    
    return this.props.children;
  }
}
```


## Performance Optimizations

### Backend Optimizations

1. **Database Indexing**
```sql
CREATE INDEX IX_Courses_LevelId ON Courses(LevelId);
CREATE INDEX IX_Lessons_CourseId ON Lessons(CourseId);
CREATE INDEX IX_CurriculumLevels_Number ON CurriculumLevels(Number);
```

2. **Query Optimization**
```csharp
// Use AsNoTracking for read-only queries
var levels = await _context.CurriculumLevels
    .AsNoTracking()
    .Include(l => l.Courses)
    .ToListAsync();

// Select only needed fields
var lessons = await _context.Lessons
    .Where(l => l.CourseId == courseId)
    .Select(l => new LessonSummaryDto(
        l.Id, l.Title, l.Duration, l.Difficulty, 
        l.EstimatedMinutes, l.OrderIndex, false
    ))
    .ToListAsync();
```

3. **Response Compression**
```csharp
builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
    options.Providers.Add<GzipCompressionProvider>();
});
```

### Frontend Optimizations

1. **Code Splitting**
```typescript
// Lazy load syntax highlighter
const SyntaxHighlighter = dynamic(
  () => import('react-syntax-highlighter').then(mod => mod.Prism),
  { ssr: false }
);

// Lazy load markdown renderer
const ReactMarkdown = dynamic(() => import('react-markdown'));
```

2. **Memoization**
```typescript
export const LessonObjectives = React.memo(({ objectives }: Props) => {
  // Component implementation
});

export const CodeExample = React.memo(({ code, language }: Props) => {
  // Component implementation
});
```

3. **Virtual Scrolling for Long Lists**
```typescript
import { FixedSizeList } from 'react-window';

export function LessonList({ lessons }: { lessons: LessonSummary[] }) {
  return (
    <FixedSizeList
      height={600}
      itemCount={lessons.length}
      itemSize={80}
      width="100%"
    >
      {({ index, style }) => (
        <div style={style}>
          <LessonCard lesson={lessons[index]} />
        </div>
      )}
    </FixedSizeList>
  );
}
```


## Testing Strategy

### Backend Tests

**Unit Tests for Controllers**
```csharp
public class LevelsControllerTests
{
    [Fact]
    public async Task GetAll_ReturnsAllLevels()
    {
        // Arrange
        var mockContext = CreateMockContext();
        var controller = new LevelsController(mockContext);
        
        // Act
        var result = await controller.GetAll();
        
        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var response = Assert.IsType<LevelListResponse>(okResult.Value);
        Assert.Equal(16, response.Levels.Count);
    }
}
```

**Integration Tests**
```csharp
public class LevelsApiTests : IClassFixture<WebApplicationFactory<Program>>
{
    [Fact]
    public async Task GetLevels_ReturnsSuccessAndCorrectContentType()
    {
        var response = await _client.GetAsync("/api/levels");
        
        response.EnsureSuccessStatusCode();
        Assert.Equal("application/json", 
            response.Content.Headers.ContentType?.MediaType);
    }
}
```

### Frontend Tests

**Component Tests**
```typescript
describe('LessonObjectives', () => {
  it('renders all objectives', () => {
    const objectives = ['Learn X', 'Understand Y', 'Practice Z'];
    render(<LessonObjectives objectives={objectives} />);
    
    expect(screen.getByText('Learn X')).toBeInTheDocument();
    expect(screen.getByText('Understand Y')).toBeInTheDocument();
    expect(screen.getByText('Practice Z')).toBeInTheDocument();
  });
});

describe('LessonContent', () => {
  it('renders structured content when available', () => {
    const lesson = {
      id: '1',
      title: 'Test',
      structuredContent: {
        objectives: ['Test objective'],
        theory: [],
        codeExamples: [],
        exercises: [],
        summary: 'Test summary',
      },
    };
    
    render(<LessonContent lesson={lesson} />);
    expect(screen.getByText('Test objective')).toBeInTheDocument();
  });
  
  it('falls back to HTML content when structured content unavailable', () => {
    const lesson = {
      id: '1',
      title: 'Test',
      content: '<p>HTML content</p>',
    };
    
    render(<LessonContent lesson={lesson} />);
    expect(screen.getByText('HTML content')).toBeInTheDocument();
  });
});
```

**Hook Tests**
```typescript
describe('useStructuredLesson', () => {
  it('fetches lesson and determines content type', async () => {
    const mockLesson = {
      id: '1',
      title: 'Test',
      structuredContent: { /* ... */ },
    };
    
    jest.spyOn(coursesApi, 'getLesson').mockResolvedValue(mockLesson);
    
    const { result, waitForNextUpdate } = renderHook(() =>
      useStructuredLesson('course-1', 'lesson-1')
    );
    
    await waitForNextUpdate();
    
    expect(result.current.lesson).toEqual(mockLesson);
    expect(result.current.contentType).toBe('structured');
  });
});
```


## Migration Strategy

### Phase 1: Backend API Implementation (No Breaking Changes)
1. Create new controllers (LevelsController)
2. Add new endpoints to existing controllers
3. Keep mock endpoints running in parallel
4. Add feature flag to switch between mock and real data

### Phase 2: Frontend Type Updates (Backward Compatible)
1. Add new optional fields to existing types
2. Create new types for structured content
3. Update API clients with new methods
4. Keep existing API methods working

### Phase 3: Component Development (Isolated)
1. Create new components for structured content
2. Build smart LessonContent component with fallback
3. Test with both content types
4. No changes to existing pages yet

### Phase 4: Page Updates (Gradual Rollout)
1. Add new /levels routes
2. Update lesson page to use LessonContent component
3. Add breadcrumb navigation
4. Test thoroughly with both content types

### Phase 5: Mock Data Removal (After Validation)
1. Verify all new endpoints working correctly
2. Remove mock data endpoints
3. Update documentation
4. Monitor for issues

### Rollback Plan
- Keep mock endpoints for 2 releases
- Feature flag to switch back to mock data
- Database backup before seeder execution
- Frontend can render both content types indefinitely


## Dependencies and Libraries

### Backend Dependencies
```xml
<!-- Already in project -->
<PackageReference Include="Microsoft.EntityFrameworkCore" />
<PackageReference Include="Microsoft.AspNetCore.OpenApi" />

<!-- May need to add -->
<PackageReference Include="Microsoft.Extensions.Caching.Memory" />
<PackageReference Include="Microsoft.AspNetCore.ResponseCompression" />
```

### Frontend Dependencies
```json
{
  "dependencies": {
    "react-markdown": "^9.0.0",
    "react-syntax-highlighter": "^15.5.0",
    "react-window": "^1.8.10"
  },
  "devDependencies": {
    "@types/react-syntax-highlighter": "^15.5.11",
    "@types/react-window": "^1.8.8"
  }
}
```

### Installation Commands
```bash
# Frontend
cd frontend
npm install react-markdown react-syntax-highlighter react-window
npm install -D @types/react-syntax-highlighter @types/react-window
```

## File Structure

### Backend Files to Create/Modify
```
src/Services/Course/
├── Controllers/
│   ├── LevelsController.cs (NEW)
│   └── CoursesController.cs (MODIFY - replace mock endpoints)
├── DTOs/
│   ├── LevelDtos.cs (NEW)
│   ├── CourseDtos.cs (NEW)
│   ├── LessonDtos.cs (NEW)
│   └── LessonContentDtos.cs (NEW)
├── Extensions/
│   └── MappingExtensions.cs (NEW)
└── Services/
    └── CachedLevelsService.cs (NEW)
```

### Frontend Files to Create/Modify
```
frontend/
├── app/
│   ├── levels/
│   │   ├── page.tsx (NEW)
│   │   └── [id]/
│   │       └── page.tsx (NEW)
│   └── courses/
│       └── [id]/
│           └── lessons/
│               └── [lessonId]/
│                   └── page.tsx (MODIFY)
├── lib/
│   ├── api/
│   │   ├── levels.ts (NEW)
│   │   └── courses.ts (MODIFY)
│   ├── components/
│   │   ├── LessonObjectives.tsx (NEW)
│   │   ├── TheorySection.tsx (NEW)
│   │   ├── CodeExample.tsx (NEW)
│   │   ├── ExerciseList.tsx (NEW)
│   │   ├── StructuredLessonView.tsx (NEW)
│   │   ├── LessonContent.tsx (NEW)
│   │   ├── LevelCard.tsx (NEW)
│   │   └── Breadcrumb.tsx (NEW)
│   ├── hooks/
│   │   ├── useLevel.ts (NEW)
│   │   └── useStructuredLesson.ts (NEW)
│   └── types.ts (MODIFY - add new types)
```

## Security Considerations

1. **Input Validation**: Validate all GUID parameters in controllers
2. **SQL Injection**: Use parameterized queries (EF Core handles this)
3. **XSS Prevention**: Sanitize HTML content before rendering
4. **Rate Limiting**: Apply rate limits to API endpoints
5. **Authentication**: Verify user authentication for completion endpoints
6. **Authorization**: Check user permissions for accessing content

## Monitoring and Logging

```csharp
// Backend logging
_logger.LogInformation("Fetching level {LevelId}", id);
_logger.LogWarning("Level {LevelId} not found", id);
_logger.LogError(ex, "Error fetching lessons for course {CourseId}", courseId);

// Frontend error tracking
useEffect(() => {
  if (error) {
    console.error('Failed to load lesson:', error);
    // Send to error tracking service (e.g., Sentry)
  }
}, [error]);
```

## Success Metrics

1. **API Performance**: < 200ms response time for 95% of requests
2. **Frontend Load Time**: < 2s for lesson page initial load
3. **Error Rate**: < 1% of API requests fail
4. **User Engagement**: Increased time on lesson pages
5. **Content Accessibility**: 100% of structured lessons render correctly
6. **Backward Compatibility**: 0 errors for legacy HTML lessons

## Next Steps

After design approval:
1. Review and validate design decisions
2. Create detailed task breakdown (tasks.md)
3. Estimate effort for each task
4. Begin implementation in phases
