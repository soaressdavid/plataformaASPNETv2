using Shared.Entities;

namespace Shared.Data;

public static partial class DbSeeder
{
    public static void SeedData(ApplicationDbContext context)
    {
        Console.WriteLine("=== Starting SeedData ===");
        
        // Seed Curriculum Levels FIRST and ensure they're saved
        var levelCount = context.Set<CurriculumLevel>().Count();
        Console.WriteLine($"Current level count: {levelCount}");
        
        if (levelCount == 0)
        {
            Console.WriteLine("Seeding curriculum levels...");
            SeedCurriculumLevels(context);
            // Explicitly save changes to ensure levels are in database before courses
            context.SaveChanges();
            
            levelCount = context.Set<CurriculumLevel>().Count();
            Console.WriteLine($"After seeding, level count: {levelCount}");
        }
        else
        {
            Console.WriteLine("Curriculum levels already seeded.");
        }

        // Seed Courses and Lessons (Portuguese content only)
        var courseCount = context.Courses.Count();
        Console.WriteLine($"Current course count: {courseCount}");
        
        if (courseCount == 0)
        {
            Console.WriteLine("Seeding courses...");
            SeedRealCourses(context);
        }
        else
        {
            Console.WriteLine("Courses already seeded.");
        }

        // Note: Challenges and Projects seeding removed - will be added when real content is ready
        Console.WriteLine("=== SeedData completed ===");
    }

    private static void SeedCurriculumLevels(ApplicationDbContext context)
    {
        Console.WriteLine("Creating curriculum level objects...");
        
        // Create levels with explicit IDs
        var level0 = new CurriculumLevel { Number = 0, Title = "Fundamentos de Programação", Description = "Aprenda os conceitos básicos de programação, incluindo variáveis, tipos de dados, operadores, controle de fluxo e funções. Perfeito para iniciantes absolutos.", RequiredXP = 0, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow };
        level0.Id = Guid.Parse("00000000-0000-0000-0000-000000000000");
        
        var level1 = new CurriculumLevel { Number = 1, Title = "Programação Orientada a Objetos", Description = "Domine os conceitos de POO incluindo classes, objetos, herança, polimorfismo, encapsulamento e abstração. Construa aplicações mais organizadas e reutilizáveis.", RequiredXP = 2000, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow };
        level1.Id = Guid.Parse("00000000-0000-0000-0000-000000000001");
        
        var level2 = new CurriculumLevel { Number = 2, Title = "Estruturas de Dados e Algoritmos", Description = "Aprenda estruturas de dados fundamentais como arrays, listas, pilhas, filas, árvores e grafos. Domine algoritmos de busca, ordenação e otimização.", RequiredXP = 4000, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow };
        level2.Id = Guid.Parse("00000000-0000-0000-0000-000000000002");
        
        var level3 = new CurriculumLevel { Number = 3, Title = "Banco de Dados e SQL", Description = "Domine SQL e design de banco de dados. Aprenda sobre normalização, índices, transações, joins e otimização de queries.", RequiredXP = 6000, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow };
        level3.Id = Guid.Parse("00000000-0000-0000-0000-000000000003");
        
        var level4 = new CurriculumLevel { Number = 4, Title = "Entity Framework Core", Description = "Aprenda a trabalhar com Entity Framework Core, o ORM mais popular do .NET. Domine Code First, migrations, relacionamentos e LINQ.", RequiredXP = 8000, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow };
        level4.Id = Guid.Parse("00000000-0000-0000-0000-000000000004");
        
        var level5 = new CurriculumLevel { Number = 5, Title = "ASP.NET Core Fundamentos", Description = "Construa aplicações web modernas com ASP.NET Core. Aprenda MVC, Razor Pages, middleware, dependency injection e configuração.", RequiredXP = 10000, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow };
        level5.Id = Guid.Parse("00000000-0000-0000-0000-000000000005");
        
        var level6 = new CurriculumLevel { Number = 6, Title = "Web APIs RESTful", Description = "Crie APIs RESTful profissionais com ASP.NET Core. Aprenda sobre HTTP, versionamento, documentação com Swagger e boas práticas de API design.", RequiredXP = 12000, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow };
        level6.Id = Guid.Parse("00000000-0000-0000-0000-000000000006");
        
        var level7 = new CurriculumLevel { Number = 7, Title = "Autenticação e Autorização", Description = "Implemente segurança robusta em suas aplicações. Aprenda Identity, JWT, OAuth 2.0, claims-based authorization e políticas de acesso.", RequiredXP = 14000, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow };
        level7.Id = Guid.Parse("00000000-0000-0000-0000-000000000007");
        
        var level8 = new CurriculumLevel { Number = 8, Title = "Testes Automatizados", Description = "Garanta a qualidade do seu código com testes automatizados. Aprenda unit tests, integration tests, TDD, mocking e code coverage.", RequiredXP = 16000, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow };
        level8.Id = Guid.Parse("00000000-0000-0000-0000-000000000008");
        
        var level9 = new CurriculumLevel { Number = 9, Title = "Arquitetura de Software", Description = "Projete sistemas escaláveis e manuteníveis. Aprenda Clean Architecture, DDD, CQRS, padrões de design e princípios SOLID.", RequiredXP = 18000, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow };
        level9.Id = Guid.Parse("00000000-0000-0000-0000-000000000009");
        
        var level10 = new CurriculumLevel { Number = 10, Title = "Microserviços", Description = "Construa arquiteturas de microserviços. Aprenda sobre service discovery, API gateway, message queues, event-driven architecture e resiliência.", RequiredXP = 20000, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow };
        level10.Id = Guid.Parse("00000000-0000-0000-0000-00000000000A");
        
        var level11 = new CurriculumLevel { Number = 11, Title = "Docker e Containers", Description = "Containerize suas aplicações com Docker. Aprenda Dockerfile, Docker Compose, volumes, networks, registry e orquestração básica.", RequiredXP = 22000, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow };
        level11.Id = Guid.Parse("00000000-0000-0000-0000-00000000000B");
        
        var level12 = new CurriculumLevel { Number = 12, Title = "Cloud Computing com Azure", Description = "Deploy suas aplicações na nuvem. Aprenda Azure App Service, Azure SQL, Storage, Functions, Service Bus e práticas de cloud-native development.", RequiredXP = 24000, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow };
        level12.Id = Guid.Parse("00000000-0000-0000-0000-00000000000C");
        
        var level13 = new CurriculumLevel { Number = 13, Title = "CI/CD e DevOps", Description = "Automatize seu pipeline de desenvolvimento. Aprenda Git workflows, GitHub Actions, Azure DevOps, pipelines, monitoring e observability.", RequiredXP = 26000, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow };
        level13.Id = Guid.Parse("00000000-0000-0000-0000-00000000000D");
        
        var level14 = new CurriculumLevel { Number = 14, Title = "Design de Sistemas", Description = "Projete sistemas de larga escala. Aprenda sobre escalabilidade, disponibilidade, consistência, caching, load balancing e trade-offs arquiteturais.", RequiredXP = 28000, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow };
        level14.Id = Guid.Parse("00000000-0000-0000-0000-00000000000E");
        
        var level15 = new CurriculumLevel { Number = 15, Title = "Liderança Técnica", Description = "Desenvolva habilidades de liderança técnica. Aprenda code review, mentoria, documentação técnica, decisões arquiteturais e gestão de equipes técnicas.", RequiredXP = 30000, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow };
        level15.Id = Guid.Parse("00000000-0000-0000-0000-00000000000F");
        
        var levels = new[] { level0, level1, level2, level3, level4, level5, level6, level7, level8, level9, level10, level11, level12, level13, level14, level15 };

        Console.WriteLine($"Adding {levels.Length} curriculum levels to context...");
        context.Set<CurriculumLevel>().AddRange(levels);
        
        Console.WriteLine("Saving curriculum levels to database...");
        context.SaveChanges();
        Console.WriteLine("Curriculum levels saved successfully!");
    }

    private static void SeedRealCourses(ApplicationDbContext context)
    {
        // Level 0: Programming Fundamentals
        SeedLevel0(context);

        // Level 1: Object-Oriented Programming
        SeedLevel1(context);

        // Level 2: Data Structures and Algorithms
        SeedLevel2(context);

        // Level 3: Databases and SQL
        SeedLevel3(context);

        // Level 4: Entity Framework Core
        SeedLevel4(context);

        // Level 5: ASP.NET Core Fundamentals
        SeedLevel5(context);

        // Level 6: Web APIs RESTful
        SeedLevel6(context);

        // Level 7: Authentication and Authorization
        SeedLevel7(context);

        // Level 8: Automated Testing
        SeedLevel8(context);

        // Level 9: Software Architecture
        SeedLevel9(context);

        // Level 10: Microservices
        SeedLevel10(context);

        // Level 11: Docker and Containers
        SeedLevel11(context);

        // Level 12: Cloud Computing (Azure)
        SeedLevel12(context);

        // Level 13: CI/CD and DevOps
        SeedLevel13(context);

        // Level 14: System Design
        SeedLevel14(context);

        // Level 15: Technical Leadership
        SeedLevel15(context);
    }

    private static void SeedLevel0(ApplicationDbContext context)
    {
        var courseId = Guid.Parse("10000000-0000-0000-0000-000000000001");
        
        // Check if course already exists (idempotent seeding)
        if (context.Courses.Any(c => c.Id == courseId))
        {
            return;
        }

        var seeder = new Level0ContentSeeder();
        var course = seeder.CreateLevel0Course();
        var lessons = seeder.CreateLevel0Lessons();

        context.Courses.Add(course);
        context.SaveChanges();
        context.Lessons.AddRange(lessons);
        context.SaveChanges();
    }

    private static void SeedLevel1(ApplicationDbContext context)
    {
        var courseId = Guid.Parse("10000000-0000-0000-0000-000000000002");
        
        // Check if course already exists (idempotent seeding)
        if (context.Courses.Any(c => c.Id == courseId))
        {
            return;
        }

        var seeder = new Level1ContentSeeder();
        var course = seeder.CreateLevel1Course();
        var lessons = seeder.CreateLevel1Lessons();

        context.Courses.Add(course);
        context.SaveChanges();
        context.Lessons.AddRange(lessons);
        context.SaveChanges();
    }

    private static void SeedLevel2(ApplicationDbContext context)
    {
        var courseId = Guid.Parse("10000000-0000-0000-0000-000000000003");
        
        // Check if course already exists (idempotent seeding)
        if (context.Courses.Any(c => c.Id == courseId))
        {
            return;
        }

        var seeder = new Level2ContentSeeder();
        var course = seeder.CreateLevel2Course();
        var lessons = seeder.CreateLevel2Lessons();

        context.Courses.Add(course);
        context.SaveChanges();
        context.Lessons.AddRange(lessons);
        context.SaveChanges();
    }

    private static void SeedLevel3(ApplicationDbContext context)
    {
        var courseId = Guid.Parse("10000000-0000-0000-0000-000000000004");
        
        // Check if course already exists (idempotent seeding)
        if (context.Courses.Any(c => c.Id == courseId))
        {
            return;
        }

        var seeder = new Level3ContentSeeder();
        var course = seeder.CreateLevel3Course();
        var lessons = seeder.CreateLevel3Lessons();

        context.Courses.Add(course);
        context.SaveChanges();
        context.Lessons.AddRange(lessons);
        context.SaveChanges();
    }

    private static void SeedLevel4(ApplicationDbContext context)
    {
        var courseId = Guid.Parse("10000000-0000-0000-0000-000000000005");
        if (context.Courses.Any(c => c.Id == courseId)) return;
        var seeder = new Level4ContentSeeder();
        context.Courses.Add(seeder.CreateLevel4Course());
        context.SaveChanges();
        context.Lessons.AddRange(seeder.CreateLevel4Lessons());
        context.SaveChanges();
    }

    private static void SeedLevel5(ApplicationDbContext context)
    {
        var courseId = Guid.Parse("10000000-0000-0000-0000-000000000006");
        if (context.Courses.Any(c => c.Id == courseId)) return;
        var seeder = new Level5ContentSeeder();
        context.Courses.Add(seeder.CreateLevel5Course());
        context.SaveChanges();
        context.Lessons.AddRange(seeder.CreateLevel5Lessons());
        context.SaveChanges();
    }

    private static void SeedLevel6(ApplicationDbContext context)
    {
        var courseId = Guid.Parse("10000000-0000-0000-0000-000000000007");
        if (context.Courses.Any(c => c.Id == courseId)) return;
        var seeder = new Level6ContentSeeder();
        context.Courses.Add(seeder.CreateLevel6Course());
        context.SaveChanges();
        context.Lessons.AddRange(seeder.CreateLevel6Lessons());
        context.SaveChanges();
    }

    private static void SeedLevel7(ApplicationDbContext context)
    {
        var courseId = Guid.Parse("10000000-0000-0000-0000-000000000008");
        if (context.Courses.Any(c => c.Id == courseId)) return;
        var seeder = new Level7ContentSeeder();
        context.Courses.Add(seeder.CreateLevel7Course());
        context.SaveChanges();
        context.Lessons.AddRange(seeder.CreateLevel7Lessons());
        context.SaveChanges();
    }

    private static void SeedLevel8(ApplicationDbContext context)
    {
        var courseId = Guid.Parse("10000000-0000-0000-0000-000000000009");
        if (context.Courses.Any(c => c.Id == courseId)) return;
        var seeder = new Level8ContentSeeder();
        context.Courses.Add(seeder.CreateLevel8Course());
        context.SaveChanges();
        context.Lessons.AddRange(seeder.CreateLevel8Lessons());
        context.SaveChanges();
    }

    private static void SeedLevel9(ApplicationDbContext context)
    {
        var courseId = Guid.Parse("10000000-0000-0000-0000-00000000000A");
        if (context.Courses.Any(c => c.Id == courseId)) return;
        var seeder = new Level9ContentSeeder();
        context.Courses.Add(seeder.CreateLevel9Course());
        context.SaveChanges();
        context.Lessons.AddRange(seeder.CreateLevel9Lessons());
        context.SaveChanges();
    }

    private static void SeedLevel10(ApplicationDbContext context)
    {
        var courseId = Guid.Parse("10000000-0000-0000-0000-00000000000B");
        if (context.Courses.Any(c => c.Id == courseId)) return;
        var seeder = new Level10ContentSeeder();
        context.Courses.Add(seeder.CreateLevel10Course());
        context.SaveChanges();
        context.Lessons.AddRange(seeder.CreateLevel10Lessons());
        context.SaveChanges();
    }

    private static void SeedLevel11(ApplicationDbContext context)
    {
        var courseId = Guid.Parse("10000000-0000-0000-0000-00000000000C");
        if (context.Courses.Any(c => c.Id == courseId)) return;
        var seeder = new Level11ContentSeeder();
        context.Courses.Add(seeder.CreateLevel11Course());
        context.SaveChanges();
        context.Lessons.AddRange(seeder.CreateLevel11Lessons());
        context.SaveChanges();
    }

    private static void SeedLevel12(ApplicationDbContext context)
    {
        var courseId = Guid.Parse("10000000-0000-0000-0000-00000000000D");
        if (context.Courses.Any(c => c.Id == courseId)) return;
        var seeder = new Level12ContentSeeder();
        context.Courses.Add(seeder.CreateLevel12Course());
        context.SaveChanges();
        context.Lessons.AddRange(seeder.CreateLevel12Lessons());
        context.SaveChanges();
    }

    private static void SeedLevel13(ApplicationDbContext context)
    {
        var courseId = Guid.Parse("10000000-0000-0000-0000-00000000000E");
        if (context.Courses.Any(c => c.Id == courseId)) return;
        var seeder = new Level13ContentSeeder();
        context.Courses.Add(seeder.CreateLevel13Course());
        context.SaveChanges();
        context.Lessons.AddRange(seeder.CreateLevel13Lessons());
        context.SaveChanges();
    }

    private static void SeedLevel14(ApplicationDbContext context)
    {
        var courseId = Guid.Parse("10000000-0000-0000-0000-00000000000F");
        if (context.Courses.Any(c => c.Id == courseId)) return;
        var seeder = new Level14ContentSeeder();
        context.Courses.Add(seeder.CreateLevel14Course());
        context.SaveChanges();
        context.Lessons.AddRange(seeder.CreateLevel14Lessons());
        context.SaveChanges();
    }

    private static void SeedLevel15(ApplicationDbContext context)
    {
        var courseId = Guid.Parse("10000000-0000-0000-0000-000000000010");
        if (context.Courses.Any(c => c.Id == courseId)) return;
        var seeder = new Level15ContentSeeder();
        context.Courses.Add(seeder.CreateLevel15Course());
        context.SaveChanges();
        context.Lessons.AddRange(seeder.CreateLevel15Lessons());
        context.SaveChanges();
    }
}
