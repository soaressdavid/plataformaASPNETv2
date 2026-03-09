using Xunit;
using FsCheck;
using FsCheck.Xunit;
using Microsoft.EntityFrameworkCore;
using Shared.Data;
using Shared.Entities;
using Shared.Models;

namespace Tests;

/// <summary>
/// Property-Based Tests for Soft Delete Preservation
/// Feature: platform-evolution-saas
/// 
/// **Validates: Requirements 1.13**
/// 
/// Property 1: Soft Delete Preservation
/// For any entity that inherits from BaseEntity, when a delete operation is performed,
/// the entity SHALL remain in the database with IsDeleted set to true, rather than
/// being physically removed from the database.
/// </summary>
public class SoftDeletePreservation_PropertyTests
{
    private ApplicationDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: $"TestDb_{Guid.NewGuid()}")
            .Options;

        return new ApplicationDbContext(options);
    }

    /// <summary>
    /// Property 1: Soft Delete Preservation
    /// **Validates: Requirements 1.13**
    /// 
    /// GIVEN any entity that inherits from BaseEntity
    /// WHEN the entity is deleted using context.Remove()
    /// THEN the entity should still exist in the database with IsDeleted = true
    /// AND the entity should not be returned by normal queries
    /// AND the entity should be retrievable using IgnoreQueryFilters()
    /// </summary>
    [Property(MaxTest = 100)]
    public void Property1_SoftDelete_ShouldPreserveRecordWithIsDeletedTrue(NonEmptyString nameGen, NonEmptyString emailGen)
    {
        using var context = CreateContext();
        
        // Arrange - Create a user entity
        var name = nameGen.Get;
        var email = emailGen.Get;
        
        var user = new User
        {
            Name = name.Length > 100 ? name.Substring(0, 100) : name,
            Email = email.Length > 255 ? email.Substring(0, 255) : email,
            PasswordHash = "hash_" + Guid.NewGuid().ToString()
        };
        
        context.Users.Add(user);
        context.SaveChanges();
        
        var userId = user.Id;
        var originalCreatedAt = user.CreatedAt;

        // Act - Delete the user (soft delete)
        context.Users.Remove(user);
        context.SaveChanges();

        // Assert 1 - Entity should still exist in database with IsDeleted = true
        var deletedUser = context.GetIncludingDeleted<User>()
            .FirstOrDefault(u => u.Id == userId);
        
        Assert.NotNull(deletedUser);
        Assert.True(deletedUser.IsDeleted, $"Entity {userId} exists but IsDeleted is false");

        // Assert 2 - Entity should not be returned by normal queries
        var normalQuery = context.Users
            .FirstOrDefault(u => u.Id == userId);
        
        Assert.Null(normalQuery);

        // Assert 3 - UpdatedAt should be updated
        Assert.True(deletedUser.UpdatedAt > originalCreatedAt, 
            $"Entity {userId} UpdatedAt was not updated on soft delete");

        // Assert 4 - CreatedAt should remain unchanged
        Assert.Equal(originalCreatedAt, deletedUser.CreatedAt);
    }

    /// <summary>
    /// Property 1.1: Soft Delete Preservation for Challenges
    /// **Validates: Requirements 1.13**
    /// 
    /// GIVEN any Challenge entity
    /// WHEN the challenge is deleted
    /// THEN it should be soft deleted with IsDeleted = true
    /// </summary>
    [Property(MaxTest = 100)]
    public void Property1_1_SoftDelete_ShouldPreserveChallenges(NonEmptyString titleGen, NonEmptyString descGen)
    {
        using var context = CreateContext();
        
        // Arrange
        var title = titleGen.Get;
        var description = descGen.Get;
        
        var challenge = new Challenge
        {
            Title = title.Length > 200 ? title.Substring(0, 200) : title,
            Description = description,
            Difficulty = Difficulty.Easy,
            StarterCode = "// Start here"
        };
        
        context.Challenges.Add(challenge);
        context.SaveChanges();
        
        var challengeId = challenge.Id;

        // Act - Delete the challenge
        context.Challenges.Remove(challenge);
        context.SaveChanges();

        // Assert - Challenge should be soft deleted
        var deletedChallenge = context.GetIncludingDeleted<Challenge>()
            .FirstOrDefault(c => c.Id == challengeId);
        
        Assert.NotNull(deletedChallenge);
        Assert.True(deletedChallenge.IsDeleted, $"Challenge {challengeId} IsDeleted is false");

        // Normal query should not return it
        var normalQuery = context.Challenges
            .FirstOrDefault(c => c.Id == challengeId);
        
        Assert.Null(normalQuery);
    }

    /// <summary>
    /// Property 1.2: Soft Delete Preservation for Lessons
    /// **Validates: Requirements 1.13**
    /// 
    /// GIVEN any Lesson entity
    /// WHEN the lesson is deleted
    /// THEN it should be soft deleted with IsDeleted = true
    /// </summary>
    [Property(MaxTest = 100)]
    public void Property1_2_SoftDelete_ShouldPreserveLessons(NonEmptyString titleGen, NonEmptyString contentGen, PositiveInt orderGen)
    {
        using var context = CreateContext();
        
        // Arrange - Create a course first
        var course = new Course
        {
            Id = Guid.NewGuid(),
            Title = "Test Course",
            Description = "Test Description",
            Level = Shared.Entities.Level.Beginner
        };
        context.Courses.Add(course);
        context.SaveChanges();

        var title = titleGen.Get;
        var content = contentGen.Get;
        
        var lesson = new Lesson
        {
            Id = Guid.NewGuid(),
            Title = title.Length > 200 ? title.Substring(0, 200) : title,
            Content = content,
            CourseId = course.Id,
            OrderIndex = orderGen.Get % 1000,
            Difficulty = "Easy",
            EstimatedMinutes = 30,
            Duration = "30 minutes",
            StructuredContent = "{}"
        };
        
        context.Lessons.Add(lesson);
        context.SaveChanges();
        
        var lessonId = lesson.Id;

        // Act - Delete the lesson
        context.Lessons.Remove(lesson);
        context.SaveChanges();

        // Assert - Lesson should be soft deleted
        var deletedLesson = context.GetIncludingDeleted<Lesson>()
            .FirstOrDefault(l => l.Id == lessonId);
        
        Assert.NotNull(deletedLesson);
        Assert.True(deletedLesson.IsDeleted, $"Lesson {lessonId} IsDeleted is false");

        // Normal query should not return it
        var normalQuery = context.Lessons
            .FirstOrDefault(l => l.Id == lessonId);
        
        Assert.Null(normalQuery);
    }

    /// <summary>
    /// Property 1.3: Multiple Soft Deletes
    /// **Validates: Requirements 1.13**
    /// 
    /// GIVEN multiple entities of the same type
    /// WHEN all are deleted
    /// THEN all should be soft deleted and retrievable via IgnoreQueryFilters()
    /// </summary>
    [Property(MaxTest = 50)]
    public void Property1_3_SoftDelete_ShouldPreserveMultipleEntities(PositiveInt countGen)
    {
        using var context = CreateContext();
        
        // Arrange - Create multiple users
        var count = Math.Max(2, countGen.Get % 10);
        var users = new List<User>();
        
        for (int i = 0; i < count; i++)
        {
            var user = new User
            {
                Name = $"User {i}",
                Email = $"user{i}_{Guid.NewGuid()}@test.com",
                PasswordHash = $"hash_{i}"
            };
            users.Add(user);
            context.Users.Add(user);
        }
        context.SaveChanges();
        
        var userIds = users.Select(u => u.Id).ToList();

        // Act - Delete all users
        foreach (var user in users)
        {
            context.Users.Remove(user);
        }
        context.SaveChanges();

        // Assert - All users should be soft deleted
        var deletedUsers = context.GetIncludingDeleted<User>()
            .Where(u => userIds.Contains(u.Id))
            .ToList();
        
        Assert.Equal(count, deletedUsers.Count);
        Assert.All(deletedUsers, u => Assert.True(u.IsDeleted));

        // Normal query should return none
        var normalQuery = context.Users
            .Where(u => userIds.Contains(u.Id))
            .ToList();
        
        Assert.Empty(normalQuery);
    }

    /// <summary>
    /// Property 1.4: Soft Delete Does Not Affect Other Entities
    /// **Validates: Requirements 1.13**
    /// 
    /// GIVEN multiple entities where only some are deleted
    /// WHEN soft delete is performed
    /// THEN only deleted entities should have IsDeleted = true
    /// AND non-deleted entities should remain queryable
    /// </summary>
    [Property(MaxTest = 50)]
    public void Property1_4_SoftDelete_ShouldNotAffectOtherEntities(PositiveInt totalCountGen)
    {
        using var context = CreateContext();
        
        // Arrange - Create multiple users
        var totalCount = Math.Max(3, totalCountGen.Get % 10);
        var users = new List<User>();
        
        for (int i = 0; i < totalCount; i++)
        {
            var user = new User
            {
                Name = $"User {i}",
                Email = $"user{i}_{Guid.NewGuid()}@test.com",
                PasswordHash = $"hash_{i}"
            };
            users.Add(user);
            context.Users.Add(user);
        }
        context.SaveChanges();

        // Act - Delete only the first half
        var deleteCount = totalCount / 2;
        var usersToDelete = users.Take(deleteCount).ToList();
        var usersToKeep = users.Skip(deleteCount).ToList();
        
        foreach (var user in usersToDelete)
        {
            context.Users.Remove(user);
        }
        context.SaveChanges();

        // Assert - Deleted users should be soft deleted
        var deletedUserIds = usersToDelete.Select(u => u.Id).ToList();
        var deletedUsers = context.GetIncludingDeleted<User>()
            .Where(u => deletedUserIds.Contains(u.Id))
            .ToList();
        
        Assert.Equal(deleteCount, deletedUsers.Count);
        Assert.All(deletedUsers, u => Assert.True(u.IsDeleted));

        // Assert - Kept users should still be queryable normally
        var keptUserIds = usersToKeep.Select(u => u.Id).ToList();
        var activeUsers = context.Users
            .Where(u => keptUserIds.Contains(u.Id))
            .ToList();
        
        Assert.Equal(usersToKeep.Count, activeUsers.Count);
        Assert.All(activeUsers, u => Assert.False(u.IsDeleted));
    }

    /// <summary>
    /// Property 1.5: GetOnlyDeleted Returns Only Soft Deleted Entities
    /// **Validates: Requirements 1.13**
    /// 
    /// GIVEN entities where some are soft deleted
    /// WHEN querying with GetOnlyDeleted
    /// THEN only soft deleted entities should be returned
    /// </summary>
    [Property(MaxTest = 50)]
    public void Property1_5_GetOnlyDeleted_ShouldReturnOnlySoftDeletedEntities(PositiveInt totalCountGen)
    {
        using var context = CreateContext();
        
        // Arrange - Create multiple users
        var totalCount = Math.Max(3, totalCountGen.Get % 10);
        var users = new List<User>();
        
        for (int i = 0; i < totalCount; i++)
        {
            var user = new User
            {
                Name = $"User {i}",
                Email = $"user{i}_{Guid.NewGuid()}@test.com",
                PasswordHash = $"hash_{i}"
            };
            users.Add(user);
            context.Users.Add(user);
        }
        context.SaveChanges();

        // Act - Delete some users (at least 1)
        var deleteCount = Math.Max(1, totalCount / 2);
        var usersToDelete = users.Take(deleteCount).ToList();
        
        foreach (var user in usersToDelete)
        {
            context.Users.Remove(user);
        }
        context.SaveChanges();

        // Assert - GetOnlyDeleted should return only deleted users
        var onlyDeleted = context.GetOnlyDeleted<User>().ToList();
        
        var deletedUserIds = usersToDelete.Select(u => u.Id).ToHashSet();
        var returnedDeletedIds = onlyDeleted.Select(u => u.Id).ToHashSet();
        
        Assert.True(deletedUserIds.SetEquals(returnedDeletedIds), 
            "GetOnlyDeleted returned incorrect set of users");
        Assert.All(onlyDeleted, u => Assert.True(u.IsDeleted));
    }
}
