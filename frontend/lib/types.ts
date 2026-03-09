// Enums
export enum Level {
  Beginner = 'Beginner',
  Intermediate = 'Intermediate',
  Advanced = 'Advanced',
}

export enum Difficulty {
  Easy = 'Easy',
  Medium = 'Medium',
  Hard = 'Hard',
}

export enum ExecutionStatus {
  Queued = 'Queued',
  Running = 'Running',
  Completed = 'Completed',
  Failed = 'Failed',
  Timeout = 'Timeout',
  MemoryExceeded = 'MemoryExceeded',
}

export enum FeedbackType {
  Security = 'Security',
  Performance = 'Performance',
  BestPractice = 'BestPractice',
  Architecture = 'Architecture',
}

// Auth types
export interface RegisterRequest {
  name: string;
  email: string;
  password: string;
}

export interface RegisterResponse {
  userId: string;
  token: string;
}

export interface LoginRequest {
  email: string;
  password: string;
}

export interface LoginResponse {
  userId: string;
  name: string;
  token: string;
  expiresAt: string;
}

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

// Course types
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

export interface CourseListResponse {
  courses: CourseSummary[];
}

// Lesson types
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
  content?: string; // LEGACY: HTML content (now optional)
  structuredContent?: LessonContent; // NEW: structured content
  prerequisites?: string[]; // NEW
}

export interface LessonListResponse {
  lessons: LessonSummary[];
}

export interface CompleteLessonRequest {
  userId: string;
}

export interface CompleteLessonResponse {
  success: boolean;
  nextLessonId?: string;
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

// Challenge types
export interface ChallengeSummary {
  id: string;
  title: string;
  difficulty: Difficulty;
  isSolved: boolean;
  submissionCount: number;
}

export interface ChallengeListResponse {
  challenges: ChallengeSummary[];
}

export interface TestCasePreview {
  input: string;
  expectedOutput: string;
  isHidden: boolean;
}

export interface ChallengeDetailResponse {
  id: string;
  title: string;
  description: string;
  difficulty: Difficulty;
  starterCode: string;
  testCases: TestCasePreview[];
  supportsTimeAttack?: boolean;
  timeAttackLimitSeconds?: number;
}

export interface TestResult {
  testCaseId: string;
  passed: boolean;
  input: string;
  expectedOutput: string;
  actualOutput: string;
}

export interface SubmitSolutionRequest {
  userId: string;
  code: string;
  isTimeAttack?: boolean;
  completionTimeSeconds?: number;
}

export interface SubmitSolutionResponse {
  submissionId: string;
  allTestsPassed: boolean;
  results: TestResult[];
  xpAwarded: number;
  timeAttackBonusXP?: number;
  completionTimeSeconds?: number;
}

// Progress types
export interface CourseProgress {
  courseId: string;
  title: string;
  completionPercentage: number;
}

export interface SolvedChallengesByDifficulty {
  easy: number;
  medium: number;
  hard: number;
}

export interface DashboardResponse {
  currentXP: number;
  currentLevel: number;
  xpToNextLevel: number;
  solvedChallenges: SolvedChallengesByDifficulty;
  completedProjects: number;
  learningStreak: number;
  coursesInProgress: CourseProgress[];
}

export interface LeaderboardEntry {
  rank: number;
  name: string;
  xp: number;
  level: number;
}

export interface LeaderboardResponse {
  entries: LeaderboardEntry[];
}

// Code execution types
export interface CodeFileData {
  name: string;
  content: string;
}

export interface ExecuteCodeRequest {
  code: string;
  files: CodeFileData[];
  entryPoint: string;
}

export interface ExecuteCodeResponse {
  jobId: string;
  status: string;
}

export interface ExecutionStatusResponse {
  jobId: string;
  status: ExecutionStatus;
  output?: string;
  error?: string;
  exitCode?: number;
  executionTimeMs: number;
}

// AI Tutor types
export interface Feedback {
  type: FeedbackType;
  message: string;
  lineNumber: number;
  codeExample: string;
}

export interface CodeReviewRequest {
  code: string;
  context: string;
}

export interface CodeReviewResponse {
  suggestions: Feedback[];
  overallScore: number;
  securityIssues: string[];
  performanceIssues: string[];
}

// Project types
export interface ProjectStep {
  order: number;
  title: string;
  instructions: string;
  starterCode: string;
  validationCriteria: string;
}

export interface ProjectSummary {
  id: string;
  title: string;
  description: string;
  stepCount: number;
  isCompleted: boolean;
}

export interface ProjectDetailResponse {
  id: string;
  title: string;
  description: string;
  steps: ProjectStep[];
  currentStep: number;
}
