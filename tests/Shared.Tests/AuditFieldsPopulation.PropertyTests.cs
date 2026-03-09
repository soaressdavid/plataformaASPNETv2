using Xunit;
using FsCheck;
using FsCheck.Xunit;
using Microsoft.EntityFrameworkCore;
using Shared.Data;
using Shared.Entities;
using Shared.Models;

namespace Tests;

/// <summary>
/// Property-Based Tests for Audit Fields Population
/// Feature: platform-evolution-saas
/// 
/// **Validates: Requirements 1.4, 1.5, 1.6**
/// 
/// Property 2: Audit Fields Population
/// For any entity saved to the database, the CreatedAt field SHALL be set on first save,
/// the UpdatedAt field SHALL be set on every save, and IsDeleted SHALL default to false.
/// </summary>
public class AuditFieldsPopulation_PropertyTests
{
    private ApplicationDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: $"TestDb_{Guid.NewGuid()}")
            .Options;

        return new ApplicationDbContext(options);
    }

    /// <summary>
    /// Property 2.1: CreatedAt Set on Entity Creation
    /// **Validates: Requirements 1.4**
    /// 
    /// GIVEN any entity that inherits from BaseEntity
    /// WHEN the entity is created and saved
    /// THEN CreatedAt should be set to current UTC time
    /// AND UpdatedAt should be set to current UTC time
    /// AND IsDeleted should be false
    /// </summary>
    [Property(MaxTest = 100)]
    public void Property2_1_CreatedAt_ShouldBeSetOnEntityCreation(NonEmptyString nameGen, NonEmptyString emailGen)
    {
        using var context = CreateContext();
        
        // Arrange
        var beforeSave = DateTime.UtcNow;
        var name = nameGen.Get;
        var email = emailGen.Get;
        
        var user = new User
        {
            Name = name.Length > 100 ? name.Substring(0, 100) : name,
            Email = email.Length > 255 ? email.Substring(0, 255) : email,
            PasswordHash = "hash_" + Guid.NewGuid().ToString()
        };

        // Act
        context.Users.Add(user);
        context.SaveChanges();
        var afterSave = DateTime.UtcNow;

        // Assert - CreatedAt should be set to current UTC time
        Assert.True(user.CreatedAt >= beforeSave && user.CreatedAt <= afterSave,
            $"CreatedAt {user.CreatedAt} is not within expected range [{beforeSave}, {afterSave}]");

        // Assert - UpdatedAt should be set to current UTC time
        Assert.True(user.UpdatedAt >= beforeSave && user.UpdatedAt <= afterSave,
            $"UpdatedAt {user.UpdatedAt} is not within expected range [{beforeSave}, {afterSave}]");

        // Assert - IsDeleted should be false
        Assert.False(user.IsDeleted, "IsDeleted should be false on entity creation");
    }

    /// <summary>
    /// Property 2.2: UpdatedAt Updated on Entity Modification
    /// **Validates: Requirements 1.5**
    /// 
    /// GIVEN any entity that has been saved
    /// WHEN the entity is modified and saved again
    /// THEN UpdatedAt should be updated to current UTC time
    /// AND CreatedAt should remain unchanged
    /// </summary>
    [Property(MaxTest = 100)]
    public void Property2_2_UpdatedAt_ShouldBeUpdatedOnModification(NonEmptyString nameGen, NonEmptyString emailGen, NonEmptyString newNameGen)
    {
        using var context = CreateContext();
        
        // Arrange - Create and save entity
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
        
        var originalCreatedAt = user.CreatedAt;
        var originalUpdatedAt = user.UpdatedAt;
        
        // Wait a small amount to ensure time difference
        System.Threading.Thread.Sleep(10);
        
        // Act - Modify entity
        var beforeUpdate = DateTime.UtcNow;
        var newName = newNameGen.Get;
        user.Name = newName.Length > 100 ? newName.Substring(0, 100) : newName;
        context.SaveChanges();
        var afterUpdate = DateTime.UtcNow;

        // Assert - UpdatedAt should be updated
        Assert.True(user.UpdatedAt >= beforeUpdate && user.UpdatedAt <= afterUpdate,
            $"UpdatedAt {user.UpdatedAt} is not within expected range [{beforeUpdate}, {afterUpdate}]");

        // Assert - UpdatedAt should be greater than original
        Assert.True(user.UpdatedAt > originalUpdatedAt,
            $"UpdatedAt {user.UpdatedAt} should be greater than original {originalUpdatedAt}");

        // Assert - CreatedAt should remain unchanged
        Assert.Equal(originalCreatedAt, user.CreatedAt);
    }

    /// <summary>
    /// Property 2.3: IsDeleted Defaults to False
    /// **Validates: Requirements 1.6**
    /// 
    /// GIVEN any entity that inherits from BaseEntity
    /// WHEN the entity is created
    /// THEN IsDeleted should default to false
    /// </summary>
    [Property(MaxTest = 100)]
    public void Property2_3_IsDeleted_ShouldDefaultToFalse(NonEmptyString titleGen, NonEmptyString descGen)
    {
        using var context = CreateContext();
        
        // Arrange & Act - Create challenge entity
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

        // Assert - IsDeleted should be false
        Assert.False(challenge.IsDeleted, "IsDeleted should default to false");
    }

    /// <summary>
    /// Property 2.4: Audit Fields for Multiple Entity Types
    /// **Validates: Requirements 1.4, 1.5, 1.6**
    /// 
    /// GIVEN different entity types that inherit from BaseEntity
    /// WHEN entities are created
    /// THEN all should have audit fields properly populated
    /// </summary>
    [Property(MaxTest = 50)]
    public void Property2_4_AuditFields_ShouldBePopulatedForAllEntityTypes(
        NonEmptyString titleGen, 
        NonEmptyString contentGen, 
        PositiveInt orderGen)
    {
        using var context = CreateContext();
        
        var beforeSave = DateTime.UtcNow;
        
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

        // Act - Create lesson entity
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
        var afterSave = DateTime.UtcNow;

        // Assert - Course audit fields
        Assert.True(course.CreatedAt >= beforeSave && course.CreatedAt <= afterSave);
        Assert.True(course.UpdatedAt >= beforeSave && course.UpdatedAt <= afterSave);
        Assert.False(course.IsDeleted);

        // Assert - Lesson audit fields
        Assert.True(lesson.CreatedAt >= beforeSave && lesson.CreatedAt <= afterSave);
        Assert.True(lesson.UpdatedAt >= beforeSave && lesson.UpdatedAt <= afterSave);
        Assert.False(lesson.IsDeleted);
    }

    /// <summary>
    /// Property 2.5: CreatedAt Remains Unchanged After Multiple Updates
    /// **Validates: Requirements 1.4, 1.5**
    /// 
    /// GIVEN any entity that has been saved
    /// WHEN the entity is modified multiple times
    /// THEN CreatedAt should remain unchanged
    /// AND UpdatedAt should be updated each time
    /// </summary>
    [Property(MaxTest = 50)]
    public void Property2_5_CreatedAt_ShouldRemainUnchangedAfterMultipleUpdates(
        NonEmptyString nameGen, 
        NonEmptyString emailGen,
        PositiveInt updateCountGen)
    {
        using var context = CreateContext();
        
        // Arrange - Create entity
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
        
        var originalCreatedAt = user.CreatedAt;
        var lastUpdatedAt = user.UpdatedAt;
        
        // Act - Perform multiple updates
        var updateCount = Math.Max(2, updateCountGen.Get % 5);
        for (int i = 0; i < updateCount; i++)
        {
            System.Threading.Thread.Sleep(10); // Ensure time difference
            user.Name = $"Updated Name {i}";
            context.SaveChanges();
            
            // Assert - UpdatedAt should be updated
            Assert.True(user.UpdatedAt > lastUpdatedAt,
                $"UpdatedAt should increase on update {i}");
            lastUpdatedAt = user.UpdatedAt;
        }

        // Assert - CreatedAt should remain unchanged after all updates
        Assert.Equal(originalCreatedAt, user.CreatedAt);
    }

    /// <summary>
    /// Property 2.6: Audit Fields Populated for Batch Operations
    /// **Validates: Requirements 1.4, 1.5, 1.6**
    /// 
    /// GIVEN multiple entities added in a batch
    /// WHEN SaveChanges is called once
    /// THEN all entities should have audit fields properly populated
    /// </summary>
    [Property(MaxTest = 50)]
    public void Property2_6_AuditFields_ShouldBePopulatedForBatchOperations(PositiveInt countGen)
    {
        using var context = CreateContext();
        
        var beforeSave = DateTime.UtcNow;
        
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

        // Act - Save all at once
        context.SaveChanges();
        var afterSave = DateTime.UtcNow;

        // Assert - All users should have audit fields populated
        foreach (var user in users)
        {
            Assert.True(user.CreatedAt >= beforeSave && user.CreatedAt <= afterSave,
                $"User {user.Id} CreatedAt is not within expected range");
            Assert.True(user.UpdatedAt >= beforeSave && user.UpdatedAt <= afterSave,
                $"User {user.Id} UpdatedAt is not within expected range");
            Assert.False(user.IsDeleted,
                $"User {user.Id} IsDeleted should be false");
        }
    }

    /// <summary>
    /// Property 2.7: UpdatedAt Updated on Soft Delete
    /// **Validates: Requirements 1.5, 1.6**
    /// 
    /// GIVEN any entity that has been saved
    /// WHEN the entity is soft deleted
    /// THEN UpdatedAt should be updated
    /// AND IsDeleted should be set to true
    /// AND CreatedAt should remain unchanged
    /// </summary>
    [Property(MaxTest = 100)]
    public void Property2_7_UpdatedAt_ShouldBeUpdatedOnSoftDelete(NonEmptyString nameGen, NonEmptyString emailGen)
    {
        using var context = CreateContext();
        
        // Arrange - Create entity
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
        
        var originalCreatedAt = user.CreatedAt;
        var originalUpdatedAt = user.UpdatedAt;
        var userId = user.Id;
        
        System.Threading.Thread.Sleep(10); // Ensure time difference

        // Act - Soft delete
        var beforeDelete = DateTime.UtcNow;
        context.Users.Remove(user);
        context.SaveChanges();
        var afterDelete = DateTime.UtcNow;

        // Retrieve the soft-deleted entity
        var deletedUser = context.GetIncludingDeleted<User>()
            .FirstOrDefault(u => u.Id == userId);

        // Assert
        Assert.NotNull(deletedUser);
        Assert.True(deletedUser.IsDeleted, "IsDeleted should be true after soft delete");
        Assert.True(deletedUser.UpdatedAt >= beforeDelete && deletedUser.UpdatedAt <= afterDelete,
            $"UpdatedAt should be updated on soft delete");
        Assert.True(deletedUser.UpdatedAt > originalUpdatedAt,
            "UpdatedAt should be greater than original after soft delete");
        Assert.Equal(originalCreatedAt, deletedUser.CreatedAt);
    }
}
