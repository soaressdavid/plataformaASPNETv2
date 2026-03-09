using Shared.Entities;

namespace Shared.Data;

public static partial class DbSeeder
{
    private static void SeedUsers(ApplicationDbContext context)
    {
        var users = new[]
        {
            new User
            {
                Id = Guid.NewGuid(),
                Name = "Alice Johnson",
                Email = "alice@example.com",
                // Password: "password123" - BCrypt hashed with work factor 12
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123", 12),
                CreatedAt = DateTime.UtcNow.AddDays(-30),
                UpdatedAt = DateTime.UtcNow.AddDays(-30)
            },
            new User
            {
                Id = Guid.NewGuid(),
                Name = "Bob Smith",
                Email = "bob@example.com",
                // Password: "securepass456" - BCrypt hashed with work factor 12
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("securepass456", 12),
                CreatedAt = DateTime.UtcNow.AddDays(-20),
                UpdatedAt = DateTime.UtcNow.AddDays(-20)
            },
            new User
            {
                Id = Guid.NewGuid(),
                Name = "Carol Davis",
                Email = "carol@example.com",
                // Password: "mypassword789" - BCrypt hashed with work factor 12
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("mypassword789", 12),
                CreatedAt = DateTime.UtcNow.AddDays(-15),
                UpdatedAt = DateTime.UtcNow.AddDays(-15)
            },
            new User
            {
                Id = Guid.NewGuid(),
                Name = "David Wilson",
                Email = "david@example.com",
                // Password: "testpass321" - BCrypt hashed with work factor 12
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("testpass321", 12),
                CreatedAt = DateTime.UtcNow.AddDays(-10),
                UpdatedAt = DateTime.UtcNow.AddDays(-10)
            },
            new User
            {
                Id = Guid.NewGuid(),
                Name = "Emma Martinez",
                Email = "emma@example.com",
                // Password: "demouser2024" - BCrypt hashed with work factor 12
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("demouser2024", 12),
                CreatedAt = DateTime.UtcNow.AddDays(-5),
                UpdatedAt = DateTime.UtcNow.AddDays(-5)
            }
        };

        context.Users.AddRange(users);
        context.SaveChanges();

        // Create Progress records for each user
        var progressRecords = users.Select(user => new Progress
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            TotalXP = 0,
            CurrentLevel = 1,
            LearningStreak = 0,
            LastActivityAt = user.CreatedAt,
            CreatedAt = user.CreatedAt
        });

        context.Progresses.AddRange(progressRecords);
        context.SaveChanges();
    }
}
