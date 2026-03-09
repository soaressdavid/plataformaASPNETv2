using Shared.Entities;

namespace Shared.Services;

/// <summary>
/// Manages the 15-level curriculum structure and provides navigation methods.
/// </summary>
public class CurriculumManager
{
    private readonly List<CurriculumLevel> _levels;

    public CurriculumManager()
    {
        _levels = InitializeLevels();
    }

    /// <summary>
    /// Gets all 16 curriculum levels (0-15).
    /// </summary>
    public List<CurriculumLevel> GetAllLevels()
    {
        return _levels;
    }

    /// <summary>
    /// Gets a specific level by its ID.
    /// </summary>
    public CurriculumLevel? GetLevelById(Guid levelId)
    {
        return _levels.FirstOrDefault(l => l.Id == levelId);
    }

    /// <summary>
    /// Gets a specific level by its number (0-15).
    /// </summary>
    public CurriculumLevel? GetLevelByNumber(int levelNumber)
    {
        return _levels.FirstOrDefault(l => l.Number == levelNumber);
    }

    /// <summary>
    /// Gets all courses for a specific level.
    /// </summary>
    public List<Course> GetCoursesByLevel(Guid levelId)
    {
        var level = GetLevelById(levelId);
        return level?.Courses.ToList() ?? new List<Course>();
    }

    /// <summary>
    /// Gets the next level in the progression path.
    /// </summary>
    public CurriculumLevel? GetNextLevel(int currentLevelNumber)
    {
        if (currentLevelNumber >= 15)
            return null;

        return _levels.FirstOrDefault(l => l.Number == currentLevelNumber + 1);
    }

    /// <summary>
    /// Initializes all 16 curriculum levels (0-15) with metadata.
    /// </summary>
    private List<CurriculumLevel> InitializeLevels()
    {
        return new List<CurriculumLevel>
        {
            CreateLevel0(),
            CreateLevel1(),
            CreateLevel2(),
            CreateLevel3(),
            CreateLevel4(),
            CreateLevel5(),
            CreateLevel6(),
            CreateLevel7(),
            CreateLevel8(),
            CreateLevel9(),
            CreateLevel10(),
            CreateLevel11(),
            CreateLevel12(),
            CreateLevel13(),
            CreateLevel14(),
            CreateLevel15()
        };
    }

    private CurriculumLevel CreateLevel0()
    {
        return new CurriculumLevel
        {
            Id = Guid.NewGuid(),
            Number = 0,
            Title = "Programming Fundamentals",
            Description = "Learn the fundamental concepts of programming including variables, control flow, functions, and basic debugging. Perfect for absolute beginners with no prior programming experience.",
            RequiredXP = 0,
            Courses = new List<Course>(),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }

    private CurriculumLevel CreateLevel1()
    {
        return new CurriculumLevel
        {
            Id = Guid.NewGuid(),
            Number = 1,
            Title = "C# Basics",
            Description = "Master C# syntax, collections, string manipulation, and file I/O. Build a solid foundation in C# programming language fundamentals.",
            RequiredXP = 100,
            Courses = new List<Course>(),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }

    private CurriculumLevel CreateLevel2()
    {
        return new CurriculumLevel
        {
            Id = Guid.NewGuid(),
            Number = 2,
            Title = "Object-Oriented Programming",
            Description = "Understand classes, objects, encapsulation, inheritance, polymorphism, interfaces, and SOLID principles. Learn to design maintainable object-oriented systems.",
            RequiredXP = 250,
            Courses = new List<Course>(),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }

    private CurriculumLevel CreateLevel3()
    {
        return new CurriculumLevel
        {
            Id = Guid.NewGuid(),
            Number = 3,
            Title = "Data Structures & Algorithms",
            Description = "Explore stacks, queues, linked lists, trees, graphs, sorting and searching algorithms, and Big O notation. Build efficient algorithmic solutions.",
            RequiredXP = 450,
            Courses = new List<Course>(),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }

    private CurriculumLevel CreateLevel4()
    {
        return new CurriculumLevel
        {
            Id = Guid.NewGuid(),
            Number = 4,
            Title = "Database Fundamentals",
            Description = "Learn SQL basics including SELECT, INSERT, UPDATE, DELETE, joins, relationships, indexes, optimization, and transactions. Master relational database concepts.",
            RequiredXP = 700,
            Courses = new List<Course>(),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }

    private CurriculumLevel CreateLevel5()
    {
        return new CurriculumLevel
        {
            Id = Guid.NewGuid(),
            Number = 5,
            Title = "Entity Framework Core",
            Description = "Master DbContext, DbSet, migrations, seeding, LINQ queries, and relationships (1:1, 1:N, N:N). Build data-driven applications with EF Core.",
            RequiredXP = 1000,
            Courses = new List<Course>(),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }

    private CurriculumLevel CreateLevel6()
    {
        return new CurriculumLevel
        {
            Id = Guid.NewGuid(),
            Number = 6,
            Title = "ASP.NET Core Web APIs",
            Description = "Build RESTful APIs with ASP.NET Core. Learn REST principles, controllers, routing, model binding, validation, and dependency injection.",
            RequiredXP = 1350,
            Courses = new List<Course>(),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }

    private CurriculumLevel CreateLevel7()
    {
        return new CurriculumLevel
        {
            Id = Guid.NewGuid(),
            Number = 7,
            Title = "Authentication & Security",
            Description = "Implement JWT tokens, password hashing, authorization policies, CORS, and CSRF protection. Secure your web applications effectively.",
            RequiredXP = 1750,
            Courses = new List<Course>(),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }

    private CurriculumLevel CreateLevel8()
    {
        return new CurriculumLevel
        {
            Id = Guid.NewGuid(),
            Number = 8,
            Title = "Testing & Quality",
            Description = "Master unit testing with xUnit, integration testing, mocking with Moq, and test-driven development. Ensure code quality and reliability.",
            RequiredXP = 2200,
            Courses = new List<Course>(),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }

    private CurriculumLevel CreateLevel9()
    {
        return new CurriculumLevel
        {
            Id = Guid.NewGuid(),
            Number = 9,
            Title = "Microservices Architecture",
            Description = "Design and build microservices with service decomposition, API Gateway pattern, service discovery, and inter-service communication.",
            RequiredXP = 2700,
            Courses = new List<Course>(),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }

    private CurriculumLevel CreateLevel10()
    {
        return new CurriculumLevel
        {
            Id = Guid.NewGuid(),
            Number = 10,
            Title = "Message Queues & Events",
            Description = "Implement RabbitMQ, event-driven architecture, CQRS pattern, and Saga pattern. Build scalable asynchronous systems.",
            RequiredXP = 3250,
            Courses = new List<Course>(),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }

    private CurriculumLevel CreateLevel11()
    {
        return new CurriculumLevel
        {
            Id = Guid.NewGuid(),
            Number = 11,
            Title = "Docker & Containerization",
            Description = "Learn Docker fundamentals, Dockerfile creation, Docker Compose, and container orchestration basics. Containerize your applications.",
            RequiredXP = 3850,
            Courses = new List<Course>(),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }

    private CurriculumLevel CreateLevel12()
    {
        return new CurriculumLevel
        {
            Id = Guid.NewGuid(),
            Number = 12,
            Title = "Cloud Deployment - Azure",
            Description = "Deploy to Azure App Service, Azure SQL Database, Azure Storage, and implement CI/CD pipelines. Build cloud-native applications.",
            RequiredXP = 4500,
            Courses = new List<Course>(),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }

    private CurriculumLevel CreateLevel13()
    {
        return new CurriculumLevel
        {
            Id = Guid.NewGuid(),
            Number = 13,
            Title = "Performance & Scalability",
            Description = "Implement caching strategies with Redis, load balancing, database optimization, and async/await patterns. Build high-performance systems.",
            RequiredXP = 5200,
            Courses = new List<Course>(),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }

    private CurriculumLevel CreateLevel14()
    {
        return new CurriculumLevel
        {
            Id = Guid.NewGuid(),
            Number = 14,
            Title = "System Design",
            Description = "Master scalability patterns, CAP theorem, distributed systems, monitoring, and observability. Design large-scale systems.",
            RequiredXP = 5950,
            Courses = new List<Course>(),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }

    private CurriculumLevel CreateLevel15()
    {
        return new CurriculumLevel
        {
            Id = Guid.NewGuid(),
            Number = 15,
            Title = "Senior Engineering",
            Description = "Learn architecture patterns, technical leadership, code review best practices, and system design interviews. Advance to senior engineering roles.",
            RequiredXP = 6750,
            Courses = new List<Course>(),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }
}
