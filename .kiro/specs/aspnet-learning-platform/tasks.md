# Implementation Plan: ASP.NET Core Learning Platform
# Implementation Plan: ASP.NET Core Learning Platform

## Overview

This implementation plan breaks down the ASP.NET Core Learning Platform into discrete coding tasks organized by component. The platform uses a microservices architecture with ASP.NET Core services, PostgreSQL database, Redis job queue, Docker-based code execution, and a Next.js frontend. Tasks are structured to build incrementally, with early validation through testing and checkpoints.

## Tasks

- [x] 1. Set up project structure and shared infrastructure
  - Create solution structure with projects for API Gateway, Auth Service, Course Service, Challenge Service, Progress Service, AI Tutor Service, and Execution Service
  - Set up shared libraries for common models, interfaces, and utilities
  - Configure Docker Compose for local development with PostgreSQL, Redis, and RabbitMQ
  - Set up Entity Framework Core with PostgreSQL provider
  - Configure Serilog for centralized logging
  - _Requirements: 10.6, 11.1, 12.1_

- [x] 2. Implement database schema and Entity Framework models
  - [x] 2.1 Create entity classes for User, Course, Lesson, Challenge, TestCase, Submission, Enrollment, Progress, and LessonCompletion
    - Define all entity properties with appropriate data types and navigation properties
    - Add data annotations for validation and constraints
    - _Requirements: 10.1, 10.2, 10.3, 10.4, 10.5_
  
  - [x] 2.2 Create DbContext with entity configurations
    - Configure entity relationships and foreign keys
    - Set up indexes for performance (email, userId, challengeId)
    - Configure cascade delete behaviors
    - _Requirements: 10.6_
  
  - [x] 2.3 Create database migrations
    - Generate initial migration for all entities
    - Add seed data for sample courses, lessons, and challenges
    - _Requirements: 10.1, 10.2, 10.3, 10.4, 10.5_
  
  - [x] 2.4 Write property test for entity data completeness
    - **Property 33: Entity Data Completeness**
    - **Validates: Requirements 10.1, 10.2, 10.3, 10.4, 10.5**


- [x] 3. Implement repository layer with retry logic
  - [x] 3.1 Create repository interfaces (IUserRepository, ICourseRepository, IChallengeRepository, ISubmissionRepository, IProgressRepository)
    - Define all CRUD methods with async signatures
    - _Requirements: 10.1, 10.2, 10.3, 10.4, 10.5_
  
  - [x] 3.2 Implement repository classes with Entity Framework
    - Implement all repository methods using DbContext
    - Add retry logic with exponential backoff for transient failures (3 retries)
    - _Requirements: 10.7_
  
  - [x] 3.3 Write property test for database retry logic
    - **Property 34: Database Retry Logic**
    - **Validates: Requirements 10.7**
  
  - [x] 3.4 Write unit tests for repository methods
    - Test CRUD operations with in-memory database
    - Test retry logic with simulated failures
    - _Requirements: 10.1, 10.2, 10.3, 10.4, 10.5, 10.7_


- [x] 4. Implement Authentication Service
  - [x] 4.1 Create password hashing utility with BCrypt
    - Implement HashPassword and VerifyPassword methods with salt (work factor: 12)
    - _Requirements: 1.4_
  
  - [x] 4.2 Write property tests for password hashing
    - **Property 1: Password Hashing with Salt**
    - **Validates: Requirements 1.1, 1.4**
  
  - [x] 4.3 Create JWT token service
    - Implement token generation with RS256 algorithm
    - Implement token validation with expiration checking (24-hour expiration)
    - Configure token signing keys
    - _Requirements: 1.2, 1.5_
  
  - [x] 4.4 Write property test for token validation
    - **Property 4: Token Validation**
    - **Validates: Requirements 1.5**
  
  - [x] 4.5 Implement AuthService with registration and login
    - Create RegisterAsync method with user creation and password hashing
    - Create LoginAsync method with credential validation and token issuance
    - _Requirements: 1.1, 1.2, 1.3_
  
  - [x] 4.6 Write property tests for authentication
    - **Property 2: Authentication Round Trip**
    - **Property 3: Invalid Credentials Rejection**
    - **Validates: Requirements 1.2, 1.3**
  
  - [x] 4.7 Write unit tests for AuthService
    - Test registration with valid and invalid data
    - Test login with correct and incorrect credentials
    - Test token expiration handling
    - _Requirements: 1.1, 1.2, 1.3_


- [x] 5. Implement Course Service
  - [x] 5.1 Create content parser for lesson content
    - Implement ContentParser.Parse to extract title, description, code blocks, and key points from markdown
    - Implement ContentPrinter.Print to format Content objects back to markdown
    - _Requirements: 17.1, 17.3_
  
  - [x] 5.2 Write property tests for content parser
    - **Property 52: Content Parsing**
    - **Property 53: Invalid Content Error**
    - **Property 54: Content Round Trip**
    - **Validates: Requirements 17.1, 17.2, 17.3, 17.4**
  
  - [x] 5.3 Implement CourseService with CRUD operations
    - Create GetAllCoursesAsync method
    - Create GetCourseByIdAsync method
    - Create GetLessonsAsync method with ordering
    - Create EnrollAsync method to create enrollment records
    - Create CompleteLessonAsync method to mark lessons complete and unlock next lesson
    - _Requirements: 7.1, 7.2, 7.3, 7.4, 7.5, 7.6_
  
  - [x] 5.4 Write property tests for course service
    - **Property 23: Course Structure**
    - **Property 24: Lesson Ordering**
    - **Property 25: Enrollment Tracking**
    - **Property 26: Lesson Content Delivery**
    - **Property 27: Lesson Completion Progression**
    - **Validates: Requirements 7.1, 7.2, 7.3, 7.4, 7.5, 7.6**
  
  - [x] 5.5 Write unit tests for CourseService
    - Test course retrieval and filtering by level
    - Test lesson ordering and completion tracking
    - Test enrollment creation and progress tracking
    - _Requirements: 7.1, 7.2, 7.3, 7.4, 7.5, 7.6_


- [x] 6. Implement Challenge Service with test case execution
  - [x] 6.1 Create test case parser
    - Implement TestCaseParser.Parse to extract input, expected output, and visibility from test case data
    - Implement TestCasePrinter.Print to format TestCase objects back to string format
    - _Requirements: 18.1, 18.3_
  
  - [x] 6.2 Write property tests for test case parser
    - **Property 55: TestCase Parsing**
    - **Property 56: Invalid TestCase Error**
    - **Property 57: TestCase Round Trip**
    - **Validates: Requirements 18.1, 18.2, 18.3, 18.4**
  
  - [x] 6.3 Implement test case executor
    - Create TestCaseExecutor to run code against test cases in isolation
    - Implement output comparison with deep equality
    - Add timeout handling per test case (5 seconds)
    - _Requirements: 13.2, 13.3, 13.4_
  
  - [x] 6.4 Write property tests for test case execution
    - **Property 13: Test Case Execution Completeness**
    - **Property 41: Test Case Parsing**
    - **Property 42: Output Comparison**
    - **Property 43: Test Case Input Support**
    - **Property 44: All Tests Pass Result**
    - **Validates: Requirements 5.3, 13.1, 13.2, 13.3, 13.4, 13.5**
  
  - [x] 6.5 Implement ChallengeService with submission handling
    - Create GetAllChallengesAsync method with filtering by difficulty
    - Create GetChallengeByIdAsync method returning challenge details and test cases
    - Create SubmitSolutionAsync method to execute test cases and store submission
    - Implement XP award logic based on difficulty (Easy: 10, Medium: 25, Hard: 50)
    - _Requirements: 5.1, 5.2, 5.3, 5.4, 5.5, 5.6, 5.7_
  
  - [x] 6.6 Write property tests for challenge service
    - **Property 11: Challenge Data Completeness**
    - **Property 12: Challenge Retrieval**
    - **Property 14: XP Award on Success**
    - **Property 15: Test Failure Reporting**
    - **Property 16: Challenge Difficulty Categorization**
    - **Property 17: Submission Persistence**
    - **Validates: Requirements 5.1, 5.2, 5.3, 5.4, 5.5, 5.6, 5.7**
  
  - [x] 6.7 Write unit tests for ChallengeService
    - Test challenge retrieval and filtering
    - Test submission with passing and failing test cases
    - Test XP calculation for different difficulties
    - Test submission history tracking
    - _Requirements: 5.1, 5.2, 5.3, 5.4, 5.5, 5.6, 5.7_


- [x] 7. Implement Progress Service with gamification
  - [x] 7.1 Create XP and level calculator
    - Implement CalculateLevel using formula: floor(sqrt(TotalXP / 100))
    - Implement CalculateXPToNextLevel
    - _Requirements: 9.5_
  
  - [x] 7.2 Write property test for level calculation
    - **Property 30: Level Calculation**
    - **Validates: Requirements 9.5**
  
  - [x] 7.3 Create learning streak calculator
    - Implement CalculateStreak to count consecutive days with activity
    - Add timezone-aware date comparison
    - _Requirements: 9.6_
  
  - [x] 7.4 Write property test for streak calculation
    - **Property 31: Streak Calculation**
    - **Validates: Requirements 9.6**
  
  - [x] 7.5 Implement ProgressService with dashboard and leaderboard
    - Create GetDashboardAsync method returning XP, level, solved challenges, completed projects, streak, and courses in progress
    - Create AwardXPAsync method to add XP, update level, and update last activity
    - Create GetLeaderboardAsync method returning top 100 students ranked by XP
    - Implement course progress calculation (completed lessons / total lessons * 100)
    - _Requirements: 8.1, 8.2, 8.3, 8.4, 8.5, 8.6, 9.1, 9.2, 9.3, 9.4, 9.5, 9.6, 9.7, 9.8_
  
  - [x] 7.6 Write property tests for progress service
    - **Property 28: Dashboard Data Completeness**
    - **Property 29: Course Progress Calculation**
    - **Property 32: Leaderboard Ranking**
    - **Validates: Requirements 8.1, 8.2, 8.3, 8.4, 8.5, 9.7**
  
  - [x] 7.7 Write unit tests for ProgressService
    - Test XP award and level increment
    - Test streak calculation with various activity patterns
    - Test leaderboard ranking and ordering
    - Test dashboard data aggregation
    - _Requirements: 8.1, 8.2, 8.3, 8.4, 8.5, 8.6, 9.1, 9.2, 9.3, 9.4, 9.5, 9.6, 9.7, 9.8_


- [x] 8. Checkpoint - Ensure all service layer tests pass
  - Ensure all tests pass, ask the user if questions arise.

- [x] 9. Implement Code Execution Service with Redis job queue
  - [x] 9.1 Set up Redis connection and job queue operations
    - Configure StackExchange.Redis client
    - Implement EnqueueJobAsync to add jobs to Redis list
    - Implement DequeueJobAsync with BRPOP for blocking dequeue
    - Implement StoreResultAsync to cache execution results with 5-minute TTL
    - _Requirements: 12.1, 12.2, 12.3_
  
  - [x] 9.2 Write property tests for job queue
    - **Property 6: Code Execution Enqueueing**
    - **Property 7: Job Processing**
    - **Validates: Requirements 3.1, 3.2, 12.2, 12.3**
  
  - [x] 9.3 Create Docker container manager
    - Implement CreateContainerAsync with resource limits (512MB memory, 1 CPU, 50 PIDs, no network)
    - Implement StartContainerAsync with timeout enforcement (30 seconds)
    - Implement StopAndRemoveContainerAsync for cleanup
    - Configure container with dotnet/sdk:8.0-alpine base image
    - _Requirements: 3.3, 3.4, 3.5, 3.7, 3.8, 14.1, 14.2, 14.3, 14.6_
  
  - [x] 9.4 Write property test for container cleanup
    - **Property 46: Container Cleanup**
    - **Validates: Requirements 14.6**
  
  - [x] 9.5 Implement prohibited code scanner
    - Create code analyzer to detect file I/O, network access, and process spawning
    - Use Roslyn syntax analysis to scan for prohibited namespaces and methods
    - _Requirements: 14.4, 14.5_
  
  - [x] 9.6 Write property test for prohibited code detection
    - **Property 45: Prohibited Code Detection**
    - **Validates: Requirements 14.4, 14.5**
  
  - [x] 9.7 Implement execution worker
    - Create Worker service that polls Redis queue with BRPOP
    - Implement job processing: scan code, create container, execute, capture output, cleanup
    - Add timeout handling (30 seconds) and memory limit handling (512MB)
    - Store execution results in Redis with job ID
    - Implement job requeue on worker failure
    - _Requirements: 3.2, 3.6, 3.7, 3.8, 3.9, 12.3, 12.5, 12.6_
  
  - [x] 9.8 Write property tests for execution service
    - **Property 8: Execution Isolation**
    - **Property 39: Concurrent Worker Processing**
    - **Property 40: Job Retry on Worker Failure**
    - **Validates: Requirements 3.9, 12.5, 12.6**
  
  - [x] 9.9 Create ExecutionService API
    - Create ExecuteCodeAsync endpoint to enqueue jobs and return job ID
    - Create GetExecutionStatusAsync endpoint to retrieve results from Redis
    - Implement job timeout detection (mark as failed after 60 seconds in queue)
    - _Requirements: 3.1, 3.6, 12.2, 12.4_
  
  - [x] 9.10 Write unit tests for execution service
    - Test job enqueueing and status retrieval
    - Test timeout and memory limit error handling
    - Test container isolation with concurrent executions
    - Test prohibited code rejection
    - _Requirements: 3.1, 3.2, 3.3, 3.4, 3.5, 3.6, 3.7, 3.8, 3.9, 12.2, 12.3, 12.4, 12.5, 12.6_


- [x] 10. Implement AI Tutor Service with Groq integration
  - [x] 10.1 Set up Groq API client
    - Configure HTTP client for Groq API with llama-3.1-70b-versatile model
    - Set temperature to 0.3 and max tokens to 2000
    - Add timeout handling (10 seconds)
    - _Requirements: 4.1, 4.6_
  
  - [x] 10.2 Create code analysis prompt builder
    - Build system prompt with ASP.NET Core best practices, SOLID principles, and security guidelines
    - Format user code with context for analysis
    - _Requirements: 4.2, 4.3, 4.4, 4.5_
  
  - [x] 10.3 Implement AITutorService with code review
    - Create ReviewCodeAsync method to call Groq API
    - Parse API response into structured feedback (type, message, line number, code example)
    - Categorize feedback by type (Security, Performance, BestPractice, Architecture)
    - Add retry logic (2 retries with 1-second delay)
    - _Requirements: 4.1, 4.2, 4.3, 4.4, 4.5, 4.6, 4.7_
  
  - [x] 10.4 Write property tests for AI tutor service
    - **Property 9: AI Code Review Integration**
    - **Property 10: AI Feedback Structure**
    - **Validates: Requirements 4.1, 4.7**
  
  - [x] 10.5 Write unit tests for AITutorService
    - Test API integration with mock responses
    - Test feedback parsing and categorization
    - Test timeout and retry handling
    - _Requirements: 4.1, 4.2, 4.3, 4.4, 4.5, 4.6, 4.7_


- [x] 11. Implement Guided Project System
  - [x] 11.1 Create Project entity and repository
    - Define Project entity with Id, Title, Description, Steps (JSON), and XP reward
    - Define ProjectStep value object with Order, Title, Instructions, StarterCode, and ValidationCriteria
    - Implement IProjectRepository with CRUD methods
    - _Requirements: 6.1, 6.2_
  
  - [x] 11.2 Implement ProjectService with step progression
    - Create StartProjectAsync method to return first step
    - Create CompleteStepAsync method to validate implementation and unlock next step
    - Create GetProjectProgressAsync method to track completed steps
    - Award 100 XP on project completion
    - _Requirements: 6.1, 6.2, 6.3, 6.4, 6.5_
  
  - [x] 11.3 Write property tests for project service
    - **Property 18: Project Step Ordering**
    - **Property 19: Project Start**
    - **Property 20: Sequential Step Unlocking**
    - **Property 21: Project Step Examples**
    - **Property 22: Project Completion XP**
    - **Validates: Requirements 6.1, 6.2, 6.3, 6.4, 6.5**
  
  - [x] 11.4 Write unit tests for ProjectService
    - Test project start and first step retrieval
    - Test step validation and progression
    - Test project completion and XP award
    - _Requirements: 6.1, 6.2, 6.3, 6.4, 6.5, 6.6_


- [x] 12. Implement API Gateway with routing and middleware
  - [x] 12.1 Set up YARP reverse proxy configuration
    - Configure routes for Auth, Course, Challenge, Progress, AI Tutor, and Execution services
    - Set up service discovery or static endpoint configuration
    - _Requirements: 11.1, 11.2, 11.3_
  
  - [x] 12.2 Implement authentication middleware
    - Create JWT validation middleware to extract and validate tokens
    - Add user context to request pipeline
    - _Requirements: 1.5_
  
  - [x] 12.3 Implement rate limiting middleware
    - Create token bucket rate limiter with Redis backend
    - Set limits: 100 req/min for authenticated users, 10 req/min for unauthenticated
    - Return 429 Too Many Requests with Retry-After header
    - _Requirements: 11.5_
  
  - [x] 12.4 Write property test for rate limiting
    - **Property 37: Rate Limiting**
    - **Validates: Requirements 11.5**
  
  - [x] 12.5 Implement error handling middleware
    - Create global exception handler to catch and format errors
    - Map exceptions to appropriate HTTP status codes
    - Return consistent error response format with code, message, details, timestamp, and traceId
    - _Requirements: 16.1, 16.2, 16.3, 16.4, 16.5_
  
  - [x] 12.6 Write property tests for error handling
    - **Property 47: Compilation Error Formatting**
    - **Property 48: Runtime Error Formatting**
    - **Property 49: Network Error Handling**
    - **Property 50: Timeout Error Messaging**
    - **Property 51: Error Logging**
    - **Validates: Requirements 16.1, 16.2, 16.3, 16.4, 16.5**
  
  - [x] 12.7 Implement circuit breaker for microservice calls
    - Add Polly circuit breaker policy (opens after 5 failures, half-open after 30s)
    - Return 503 Service Unavailable when circuit is open
    - _Requirements: 11.4_
  
  - [x] 12.8 Write property tests for API gateway
    - **Property 35: API Gateway Routing**
    - **Property 36: Service Unavailability Handling**
    - **Validates: Requirements 11.1, 11.2, 11.3, 11.4**
  
  - [x] 12.9 Write integration tests for API Gateway
    - Test routing to all microservices
    - Test authentication middleware with valid and invalid tokens
    - Test rate limiting with burst requests
    - Test circuit breaker behavior
    - _Requirements: 11.1, 11.2, 11.3, 11.4, 11.5_


- [x] 13. Checkpoint - Ensure all backend services are integrated
  - Ensure all tests pass, ask the user if questions arise.

- [x] 14. Set up RabbitMQ event bus for inter-service communication
  - [x] 14.1 Configure RabbitMQ connection and channel management
    - Set up connection factory with retry logic
    - Create exchange and queue declarations
    - _Requirements: 11.6_
  
  - [x] 14.2 Implement event publisher
    - Create IEventPublisher interface with PublishAsync method
    - Implement RabbitMQEventPublisher to publish domain events
    - _Requirements: 11.6_
  
  - [x] 14.3 Implement event consumers
    - Create ChallengeCompletedEventConsumer to trigger XP award in Progress Service
    - Create LessonCompletedEventConsumer to update course progress
    - _Requirements: 11.6_
  
  - [x] 14.4 Write integration tests for event bus
    - Test event publishing and consumption
    - Test event ordering and delivery guarantees
    - _Requirements: 11.6_


- [x] 15. Implement Next.js frontend application
  - [x] 15.1 Set up Next.js project with TypeScript and Tailwind CSS
    - Initialize Next.js project with App Router
    - Configure Tailwind CSS with custom theme
    - Set up API client with axios for backend communication
    - _Requirements: 15.1, 15.2_
  
  - [x] 15.2 Create authentication pages and context
    - Create login page with email and password form
    - Create registration page with name, email, and password form
    - Implement AuthContext to manage user session and token
    - Add protected route wrapper for authenticated pages
    - _Requirements: 1.1, 1.2, 1.3_
  
  - [x] 15.3 Implement Monaco Editor integration for Browser IDE
    - Install @monaco-editor/react package
    - Create CodeEditor component with C# syntax highlighting
    - Add IntelliSense configuration for .NET APIs
    - Implement multi-file tab interface
    - Add terminal output panel
    - Add run button to trigger code execution
    - _Requirements: 2.1, 2.2, 2.3, 2.4, 2.5, 2.6_
  
  - [x] 15.4 Write property test for multi-file editor state
    - **Property 5: Multi-File Editor State**
    - **Validates: Requirements 2.3**
  
  - [x] 15.5 Create Dashboard component
    - Display XP, level, and XP to next level with progress bar
    - Show solved challenges count by difficulty (Easy, Medium, Hard)
    - Show completed projects count
    - Display learning streak with calendar visualization
    - List courses in progress with completion percentage
    - _Requirements: 8.1, 8.2, 8.3, 8.4, 8.5, 8.6_
  
  - [x] 15.6 Create Challenge Browser and Detail pages
    - Create challenge list page with filtering by difficulty
    - Create challenge detail page with description, starter code, and test cases
    - Integrate CodeEditor for solution submission
    - Display test results with pass/fail indicators and expected vs actual output
    - Show submission history
    - _Requirements: 5.1, 5.2, 5.3, 5.4, 5.5_
  
  - [x] 15.7 Create Course Browser and Lesson pages
    - Create course list page with filtering by level
    - Create course detail page with lesson list
    - Create lesson page with content rendering and code examples
    - Add lesson completion button
    - Show course progress indicator
    - _Requirements: 7.1, 7.2, 7.3, 7.4, 7.5, 7.6_
  
  - [x] 15.8 Create Project pages
    - Create project list page
    - Create project detail page with step-by-step interface
    - Show current step instructions and starter code
    - Add step validation and next step unlock
    - Display project completion status
    - _Requirements: 6.1, 6.2, 6.3, 6.4, 6.5_
  
  - [x] 15.9 Create Leaderboard page
    - Display top 100 students ranked by XP
    - Show rank, name, XP, and level for each entry
    - Highlight current user's position
    - _Requirements: 9.7, 9.8_
  
  - [x] 15.10 Implement code execution integration
    - Create WebSocket connection for real-time execution feedback
    - Handle execution status updates (Queued, Running, Completed, Failed, Timeout, MemoryExceeded)
    - Display output, errors, and execution time in terminal panel
    - _Requirements: 3.1, 3.6, 3.7, 3.8_
  
  - [x] 15.11 Implement AI code review integration
    - Add "Get Feedback" button in CodeEditor
    - Display AI feedback with categorized suggestions (Security, Performance, BestPractice, Architecture)
    - Show line numbers and code examples for each suggestion
    - _Requirements: 4.1, 4.2, 4.3, 4.4, 4.5, 4.6, 4.7_
  
  - [x] 15.12 Add responsive design and mobile support
    - Ensure all pages render correctly on desktop (1280x720+)
    - Ensure all pages render correctly on tablet (768x1024+)
    - Add mobile navigation menu
    - _Requirements: 15.4, 15.5_
  
  - [x] 15.13 Write integration tests for frontend
    - Test authentication flow (login, registration, logout)
    - Test challenge submission and result display
    - Test course enrollment and lesson completion
    - Test dashboard data display
    - _Requirements: 1.1, 1.2, 1.3, 5.1, 5.2, 5.3, 7.1, 7.3, 8.1_


- [x] 16. Implement error handling and user feedback
  - [x] 16.1 Create error boundary components in frontend
    - Add React error boundaries for graceful error handling
    - Display user-friendly error messages
    - _Requirements: 16.3_
  
  - [x] 16.2 Implement toast notifications
    - Add toast library (e.g., react-hot-toast)
    - Show success notifications for completed actions
    - Show error notifications with clear messages
    - _Requirements: 16.1, 16.2, 16.3, 16.4_
  
  - [x] 16.3 Add loading states and skeletons
    - Create loading skeletons for dashboard, challenges, and courses
    - Add loading indicators for code execution and AI feedback
    - _Requirements: 15.6_
  
  - [x] 16.4 Write unit tests for error handling
    - Test error boundary behavior
    - Test toast notification display
    - Test loading state transitions
    - _Requirements: 16.1, 16.2, 16.3, 16.4_


- [x] 17. Set up Docker containerization and deployment
  - [x] 17.1 Create Dockerfiles for all services
    - Create Dockerfile for API Gateway
    - Create Dockerfile for Auth Service
    - Create Dockerfile for Course Service
    - Create Dockerfile for Challenge Service
    - Create Dockerfile for Progress Service
    - Create Dockerfile for AI Tutor Service
    - Create Dockerfile for Execution Service
    - Create Dockerfile for Worker
    - Create Dockerfile for Next.js frontend
    - _Requirements: 3.2, 14.1_
  
  - [x] 17.2 Create Docker Compose configuration
    - Configure all services with dependencies
    - Set up PostgreSQL, Redis, and RabbitMQ containers
    - Configure environment variables and secrets
    - Set up networking between services
    - _Requirements: 10.6, 11.6, 12.1_
  
  - [x] 17.3 Create Kubernetes manifests (optional for production)
    - Create deployments for all services
    - Create services for internal communication
    - Create ingress for external access
    - Configure horizontal pod autoscaling for workers
    - _Requirements: 12.5_
  
  - [x] 17.4 Write deployment documentation
    - Document local development setup with Docker Compose
    - Document production deployment steps
    - Document environment variable configuration


- [x] 18. Implement monitoring and logging
  - [x] 18.1 Set up centralized logging with Serilog
    - Configure Serilog sinks for console and file output
    - Add structured logging with context (userId, requestId, traceId)
    - Configure log levels per environment
    - _Requirements: 16.5_
  
  - [x] 18.2 Add application metrics
    - Implement metrics collection with Prometheus
    - Track request counts, response times, error rates
    - Track queue depth and worker utilization
    - Track code execution times and success rates
    - _Requirements: 3.6_
  
  - [x] 18.3 Set up health checks
    - Add health check endpoints for all services
    - Check database connectivity
    - Check Redis connectivity
    - Check RabbitMQ connectivity
    - _Requirements: 11.4_
  
  - [x] 18.4 Create monitoring dashboards
    - Create Grafana dashboards for system metrics
    - Create alerts for critical errors and performance degradation


- [x] 19. Create seed data and sample content
  - [x] 19.1 Create sample courses and lessons
    - Create "ASP.NET Core Basics" course with 10 lessons
    - Create "Web API Development" course with 8 lessons
    - Create "Entity Framework Core" course with 6 lessons
    - Add lesson content with code examples and explanations
    - _Requirements: 7.1, 7.2, 7.4_
  
  - [x] 19.2 Create sample challenges
    - Create 10 Easy challenges (string manipulation, basic algorithms)
    - Create 10 Medium challenges (data structures, LINQ queries)
    - Create 10 Hard challenges (complex algorithms, design patterns)
    - Add test cases for each challenge
    - _Requirements: 5.1, 5.6_
  
  - [x] 19.3 Create sample guided projects
    - Create "Build a Todo API" project with 5 steps
    - Create "Build a Blog API" project with 7 steps
    - Create "Build an E-commerce API" project with 10 steps
    - _Requirements: 6.1, 6.2, 6.4_
  
  - [x] 19.4 Create database seeding script
    - Implement seed data migration
    - Add sample users for testing
    - _Requirements: 10.1, 10.2, 10.3, 10.4_


- [x] 20. Final checkpoint and end-to-end testing
  - [x] 20.1 Run all unit and property tests
    - Verify all tests pass
    - Check code coverage meets 80% threshold
    - _Requirements: All_
  
  - [x] 20.2 Run integration tests
    - Test complete challenge submission flow
    - Test course enrollment and lesson completion flow
    - Test code execution with queue and workers
    - Test AI feedback integration
    - Test leaderboard updates
    - _Requirements: 3.1, 3.2, 5.3, 7.3, 7.6, 9.7_
  
  - [x] 20.3 Run end-to-end tests
    - Test user registration and login
    - Test browsing and enrolling in courses
    - Test solving challenges and earning XP
    - Test viewing dashboard and leaderboard
    - Test code execution and AI feedback
    - _Requirements: 1.1, 1.2, 5.3, 7.3, 8.1, 9.7_
  
  - [x] 20.4 Perform security testing
    - Run OWASP dependency check
    - Run static analysis with SonarQube
    - Test authentication bypass attempts
    - Test code injection in execution engine
    - Validate rate limiting
    - _Requirements: 1.4, 11.5, 14.1, 14.4, 14.5_
  
  - [x] 20.5 Perform load testing
    - Test 1000 concurrent users browsing challenges
    - Test 100 concurrent code executions
    - Test 50 concurrent AI feedback requests
    - Verify performance targets (API < 200ms p95, execution < 5s p95)
    - _Requirements: 3.6, 4.6, 15.6_
  
  - [x] 20.6 Final checkpoint - Ensure all tests pass
    - Ensure all tests pass, ask the user if questions arise.

## Notes

- Each task references specific requirements for traceability
- Checkpoints ensure incremental validation at major milestones
- Property tests validate universal correctness properties from the design document
- Unit tests validate specific examples and edge cases
- Integration and end-to-end tests validate complete workflows
- The implementation uses C# with ASP.NET Core for backend services and TypeScript with Next.js for frontend
- Docker is used for code execution isolation and service containerization
- PostgreSQL for persistent storage, Redis for job queue and caching, RabbitMQ for event bus
