# Curriculum Documentation
**ASP.NET Core Learning Platform - Complete 16-Level Curriculum**

## Overview

The ASP.NET Core Learning Platform features a comprehensive 16-level curriculum (Levels 0-15) with 320 total lessons. Each level contains exactly 20 lessons covering specific topics in a progressive learning path from beginner to senior developer.

**Total Content:**
- 16 Curriculum Levels (0-15)
- 16 Courses (1 per level)
- 320 Lessons (20 per level)
- All content in Portuguese

## Curriculum Structure

### Level Progression

| Level | Title | Topics | Difficulty | Lessons |
|-------|-------|--------|------------|---------|
| 0 | Fundamentos de Programação | Variables, Data Types, Control Flow, Functions | Beginner | 20 |
| 1 | Programação Orientada a Objetos | Classes, Inheritance, Polymorphism, Encapsulation | Beginner | 20 |
| 2 | Estruturas de Dados e Algoritmos | Arrays, Lists, Trees, Graphs, Sorting, Searching | Intermediate | 20 |
| 3 | Banco de Dados e SQL | SQL Basics, JOINs, Indexes, Transactions, Normalization | Intermediate | 20 |
| 4 | Entity Framework Core | Code First, Migrations, Relationships, LINQ, Performance | Intermediate | 20 |
| 5 | ASP.NET Core Fundamentos | MVC, Middleware, DI, Configuration, Logging | Intermediate | 20 |
| 6 | Web APIs RESTful | REST Principles, HTTP Methods, Status Codes, Swagger | Advanced | 20 |
| 7 | Autenticação e Autorização | Identity, JWT, OAuth 2.0, Claims, Policies | Advanced | 20 |
| 8 | Testes Automatizados | Unit Tests, Integration Tests, TDD, Mocking | Advanced | 20 |
| 9 | Arquitetura de Software | Clean Architecture, DDD, CQRS, SOLID | Advanced | 20 |
| 10 | Microserviços | Service Discovery, API Gateway, Event-Driven | Advanced | 20 |
| 11 | Docker e Containers | Docker Fundamentals, Compose, Volumes, Networks | Advanced | 20 |
| 12 | Cloud Computing (Azure) | App Service, Azure SQL, Functions, DevOps | Advanced | 20 |
| 13 | CI/CD e DevOps | Git, GitHub Actions, Pipelines, Monitoring | Advanced | 20 |
| 14 | Design de Sistemas | Scalability, CAP Theorem, Caching, Load Balancing | Advanced | 20 |
| 15 | Liderança Técnica | Architecture Patterns, Code Review, Mentoring | Advanced | 20 |

### XP Requirements

Each level requires a specific amount of XP to unlock:

```
Level 0:  0 XP (always unlocked)
Level 1:  2,000 XP
Level 2:  4,000 XP
Level 3:  6,000 XP
Level 4:  8,000 XP
Level 5:  10,000 XP
Level 6:  12,000 XP
Level 7:  14,000 XP
Level 8:  16,000 XP
Level 9:  18,000 XP
Level 10: 20,000 XP
Level 11: 22,000 XP
Level 12: 24,000 XP
Level 13: 26,000 XP
Level 14: 28,000 XP
Level 15: 30,000 XP
```

## Lesson Template Format

Every lesson follows a standardized structure with the following components:

### 1. Lesson Metadata
```csharp
{
    Id: Guid,                    // Unique lesson identifier
    CourseId: Guid,              // Parent course reference
    Title: string,               // Lesson title in Portuguese
    Duration: string,            // e.g., "45 min"
    Difficulty: string,          // "Iniciante", "Intermediário", "Avançado"
    EstimatedMinutes: int,       // Numeric duration
    OrderIndex: int,             // 1-20 within course
    Version: int,                // Content version
    Prerequisites: string,       // JSON array of prerequisite lesson IDs
    StructuredContent: string    // JSON-serialized LessonContent
}
```

### 2. Structured Content (LessonContent)

```csharp
{
    Objectives: List<string>,           // 3-7 learning objectives
    Theory: List<TheorySection>,        // 2-4 theory sections
    CodeExamples: List<CodeExample>,    // 2-3 code examples
    Exercises: List<Exercise>,          // 3-4 exercises
    Summary: string                     // Lesson summary
}
```

### 3. Theory Section
```csharp
{
    Heading: string,     // Section title
    Content: string,     // 200-500 words of theory
    Order: int           // Section order
}
```

### 4. Code Example
```csharp
{
    Title: string,       // Example title
    Code: string,        // C# code (must compile)
    Language: string,    // "csharp"
    Explanation: string, // What the code demonstrates
    IsRunnable: bool     // Whether code can be executed
}
```

### 5. Exercise
```csharp
{
    Title: string,                  // Exercise title
    Description: string,            // What to implement
    Difficulty: ExerciseDifficulty, // Fácil, Médio, Difícil
    StarterCode: string,            // Initial code template
    Hints: List<string>             // 2-4 hints
}
```

## Content Creation Process

### Phase 1: Research (30-60 minutes)
1. Review topic documentation and best practices
2. Identify 20 lesson topics with logical progression
3. Determine key concepts and learning objectives
4. Find relevant code examples and exercises

### Phase 2: Planning (30-60 minutes)
1. Create lesson outline with titles
2. Define learning objectives for each lesson
3. Plan theory sections and code examples
4. Design exercises with varying difficulty

### Phase 3: Implementation (5-6 hours)
1. Create LevelXContentSeeder.cs file
2. Implement CreateLevelXCourse() method
3. Implement CreateLevelXLessons() method
4. Create CreateLesson1() through CreateLesson20() methods
5. Write detailed theory sections (200-500 words each)
6. Add compilable code examples with explanations
7. Create practical exercises with hints

### Phase 4: Validation (30 minutes)
1. Run LessonValidator on all lessons
2. Verify code examples compile
3. Check word counts and structure
4. Manual review of 3-5 sample lessons

### Phase 5: Integration (15 minutes)
1. Update DbSeeder.cs with SeedLevelX() method
2. Test database seeding
3. Verify API endpoints return correct data

## Quality Assurance Checklist

### Content Quality
- [ ] 3-7 learning objectives per lesson
- [ ] 2-4 theory sections per lesson
- [ ] Each theory section has 200-500 words
- [ ] Total lesson word count: 1000-3000 words
- [ ] 2-3 code examples per lesson
- [ ] All code examples compile without errors
- [ ] 3-4 exercises per lesson
- [ ] Exercises have at least 2 different difficulty levels
- [ ] All content in Portuguese
- [ ] Clear, concise, and pedagogically sound

### Technical Quality
- [ ] Lesson IDs follow convention: 10000000-0000-0000-XXXX-YYYYYYYYYYYY
- [ ] Course ID matches level: 10000000-0000-0000-0000-00000000000X
- [ ] Level ID matches: 00000000-0000-0000-0000-00000000000X
- [ ] OrderIndex is sequential (1-20)
- [ ] All required fields populated
- [ ] StructuredContent is valid JSON
- [ ] No duplicate lesson titles within level

### Integration Quality
- [ ] Course appears in GET /api/courses
- [ ] Course can be filtered by levelId
- [ ] All 20 lessons appear in GET /api/courses/{id}/lessons
- [ ] Individual lessons can be retrieved
- [ ] Backward compatibility maintained

## Developer Guide

### Adding a New Lesson

1. **Create Seeder File**
```csharp
// src/Shared/Data/LevelXContentSeeder.cs
public partial class LevelXContentSeeder
{
    private readonly Guid _courseId = Guid.Parse("10000000-0000-0000-0000-00000000000X");
    private readonly Guid _levelId = Guid.Parse("00000000-0000-0000-0000-00000000000X");
    
    public Course CreateLevelXCourse() { /* ... */ }
    public List<Lesson> CreateLevelXLessons() { /* ... */ }
    private Lesson CreateLesson1() { /* ... */ }
    // ... through CreateLesson20()
}
```

2. **Update DbSeeder**
```csharp
// src/Shared/Data/DbSeeder.cs
private static void SeedRealCourses(ApplicationDbContext context)
{
    // ... existing levels
    SeedLevelX(context);
}

private static void SeedLevelX(ApplicationDbContext context)
{
    var courseId = Guid.Parse("10000000-0000-0000-0000-00000000000X");
    if (context.Courses.Any(c => c.Id == courseId)) return;
    
    var seeder = new LevelXContentSeeder();
    context.Courses.Add(seeder.CreateLevelXCourse());
    context.SaveChanges();
    context.Lessons.AddRange(seeder.CreateLevelXLessons());
    context.SaveChanges();
}
```

3. **Validate Content**
```bash
# Run validation tool
cd scripts/ValidationTool
dotnet run

# Run tests
dotnet test
```

### Lesson ID Convention

Lesson IDs follow a specific pattern:

```
10000000-0000-0000-LLLL-NNNNNNNNNNNN
                   ^^^^  ^^^^^^^^^^^^
                   |     |
                   |     +-- Lesson number (1-20)
                   +-------- Level number (0001-0010 for levels 0-15)
```

Examples:
- Level 0, Lesson 1: `10000000-0000-0000-0001-000000000001`
- Level 5, Lesson 20: `10000000-0000-0000-0006-000000000014`
- Level 15, Lesson 10: `10000000-0000-0000-0010-00000000000A`

### Course ID Convention

```
10000000-0000-0000-0000-00000000000X
                                   ^
                                   |
                                   +-- Level number (1-10 for levels 0-15)
```

Examples:
- Level 0: `10000000-0000-0000-0000-000000000001`
- Level 5: `10000000-0000-0000-0000-000000000006`
- Level 15: `10000000-0000-0000-0000-000000000010`

### Level ID Convention

```
00000000-0000-0000-0000-00000000000X
                                   ^
                                   |
                                   +-- Level number (0-F for levels 0-15)
```

Examples:
- Level 0: `00000000-0000-0000-0000-000000000000`
- Level 5: `00000000-0000-0000-0000-000000000005`
- Level 15: `00000000-0000-0000-0000-00000000000F`

## API Endpoints

### Get All Courses
```http
GET /api/courses
```

Returns all 16 courses.

### Get Courses by Level
```http
GET /api/courses?levelId={guid}
```

Returns courses for a specific level (typically 1 course per level).

### Get Course Details
```http
GET /api/courses/{courseId}
```

Returns detailed information about a specific course.

### Get Course Lessons
```http
GET /api/courses/{courseId}/lessons
```

Returns all 20 lessons for a course.

### Get Lesson Details
```http
GET /api/courses/{courseId}/lessons/{lessonId}
```

Returns full lesson details including structured content.

### Complete Lesson
```http
POST /api/courses/{courseId}/lessons/{lessonId}/complete
```

Marks a lesson as complete for the authenticated user.

## Testing

### Property-Based Tests
Located in `tests/CurriculumExpansion.PropertyTests.cs`

Tests 23 correctness properties across all 320 lessons:
1. Lesson Structure Completeness
2. Learning Objectives Count (3-7)
3. Theory Section Word Count (200-500)
4. Code Examples Minimum (2+)
5. Exercise Minimum and Variety (3+, 2+ difficulties)
6. Code Compilation Validity
7. Lesson Metadata Completeness
8. Unique Lesson Titles Within Level
9. Referential Integrity
10. API Returns Valid Data
11. API Response Metadata
12. API Response Format
13. Lesson Serialization Round-Trip
14. Prerequisite Existence
15. Total Lesson Word Count (1000-3000)
16. Level Progression Monotonicity
17. Project-Level Association
18. Project Completeness
19. Lesson Completion Timestamp
20. Completion Percentage Calculation
21. Next Lesson Recommendation
22. Level Unlock Logic
23. Level Access Restriction

### Unit Tests
Located in `tests/ContentSeederTests.cs`

Tests each seeder creates:
- Exactly 20 lessons
- Correct course metadata
- Sequential OrderIndex (1-20)
- Valid lesson IDs following convention
- Non-null StructuredContent

### Integration Tests
Located in `tests/CurriculumAPIIntegrationTests.cs`

Tests API endpoints:
- GET /api/courses returns 16 courses
- Level filtering works correctly
- Each course has 20 lessons
- Lesson retrieval includes full content
- Performance meets requirements (<500ms)
- Backward compatibility maintained

## Troubleshooting

### Common Issues

**Issue: Code examples don't compile**
- Solution: Run `python scripts/fix_compilation_errors.py` to auto-add missing using statements

**Issue: Theory sections too short**
- Solution: Expand sections to 200-500 words or adjust requirements

**Issue: Lesson word count below 1000**
- Solution: Add more detailed explanations, examples, or exercises

**Issue: Database seeding fails**
- Solution: Check for duplicate IDs, verify level references exist

**Issue: API returns 404 for new level**
- Solution: Verify DbSeeder includes SeedLevelX() call and database was reseeded

## Performance Considerations

### Database
- Use indexes on CourseId and LevelId for fast filtering
- Consider caching frequently accessed courses/lessons
- Implement pagination for large result sets

### API
- Target <200ms p95 for lesson retrieval
- Use compression for large JSON responses
- Implement ETags for caching

### Content
- Keep StructuredContent JSON under 50KB per lesson
- Lazy-load code examples and exercises
- Consider CDN for static content

## Future Enhancements

### Planned Features
1. **Projects**: Add capstone projects for each level
2. **Assessments**: Add quizzes and coding challenges
3. **Certificates**: Award certificates upon level completion
4. **Adaptive Learning**: Recommend lessons based on performance
5. **Community Features**: Discussion forums, peer review
6. **Gamification**: Badges, leaderboards, achievements

### Content Expansion
1. **Levels 6-15**: Expand generic content to specific, detailed content
2. **Video Lessons**: Add video explanations for complex topics
3. **Interactive Exercises**: Add in-browser code execution
4. **Real-World Projects**: Add industry-relevant projects
5. **Interview Prep**: Add technical interview questions

## Resources

### Documentation
- [ASP.NET Core Docs](https://docs.microsoft.com/aspnet/core)
- [Entity Framework Core Docs](https://docs.microsoft.com/ef/core)
- [C# Language Reference](https://docs.microsoft.com/dotnet/csharp)

### Tools
- Validation Tool: `scripts/ValidationTool/Program.cs`
- Compilation Fixer: `scripts/fix_compilation_errors.py`
- Level Validator: `scripts/validate_all_levels.py`

### Support
- GitHub Issues: Report bugs and request features
- Documentation: This file and design documents
- Tests: Run `dotnet test` for validation

---

**Last Updated:** 2026-03-07  
**Version:** 1.0  
**Maintainer:** Development Team
