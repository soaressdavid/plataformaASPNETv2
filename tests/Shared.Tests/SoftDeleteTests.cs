using Microsoft.EntityFrameworkCore;
using Shared.Data;
using Shared.Entities;
using Xunit;

namespace Shared.Tests;

public class SoftDeleteTests : IDisposable
{
    private readonly ApplicationDbContext _context;

    public SoftDeleteTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new ApplicationDbContext(options);
    }

    [Fact]
    public async Task Delete_ShouldSetIsDeletedToTrue_InsteadOfPhysicalDelete()
    {
        // Arrange
        var user = new User
        {
            Name = "Test User",
            Email = "test@example.com",
            PasswordHash = "hash123"
        };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var userId = user.Id;

        // Act - Delete the user
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();

        // Assert - User should still exist in database but with IsDeleted = true
        var deletedUser = await _context.GetIncludingDeleted<User>()
            .FirstOrDefaultAsync(u => u.Id == userId);

        Assert.NotNull(deletedUser);
        Assert.True(deletedUser.IsDeleted);
        Assert.NotEqual(default, deletedUser.UpdatedAt);
    }

    [Fact]
    public async Task Query_ShouldExcludeSoftDeletedEntities_ByDefault()
    {
        // Arrange
        var user1 = new User { Name = "User 1", Email = "user1@example.com", PasswordHash = "hash1" };
        var user2 = new User { Name = "User 2", Email = "user2@example.com", PasswordHash = "hash2" };
        
        _context.Users.AddRange(user1, user2);
        await _context.SaveChangesAsync();

        // Act - Delete user1
        _context.Users.Remove(user1);
        await _context.SaveChangesAsync();

        // Assert - Normal query should only return user2
        var activeUsers = await _context.Users.ToListAsync();
        Assert.Single(activeUsers);
        Assert.Equal("User 2", activeUsers[0].Name);
    }

    [Fact]
    public async Task GetIncludingDeleted_ShouldReturnAllEntities_IncludingSoftDeleted()
    {
        // Arrange
        var user1 = new User { Name = "User 1", Email = "user1@example.com", PasswordHash = "hash1" };
        var user2 = new User { Name = "User 2", Email = "user2@example.com", PasswordHash = "hash2" };
        
        _context.Users.AddRange(user1, user2);
        await _context.SaveChangesAsync();

        // Act - Delete user1
        _context.Users.Remove(user1);
        await _context.SaveChangesAsync();

        // Assert - GetIncludingDeleted should return both users
        var allUsers = await _context.GetIncludingDeleted<User>().ToListAsync();
        Assert.Equal(2, allUsers.Count);
    }

    [Fact]
    public async Task GetOnlyDeleted_ShouldReturnOnlySoftDeletedEntities()
    {
        // Arrange
        var user1 = new User { Name = "User 1", Email = "user1@example.com", PasswordHash = "hash1" };
        var user2 = new User { Name = "User 2", Email = "user2@example.com", PasswordHash = "hash2" };
        
        _context.Users.AddRange(user1, user2);
        await _context.SaveChangesAsync();

        // Act - Delete user1
        _context.Users.Remove(user1);
        await _context.SaveChangesAsync();

        // Assert - GetOnlyDeleted should return only user1
        var deletedUsers = await _context.GetOnlyDeleted<User>().ToListAsync();
        Assert.Single(deletedUsers);
        Assert.Equal("User 1", deletedUsers[0].Name);
    }

    [Fact]
    public async Task Restore_ShouldSetIsDeletedToFalse_AndMakeEntityQueryableAgain()
    {
        // Arrange
        var user = new User { Name = "Test User", Email = "test@example.com", PasswordHash = "hash123" };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        // Delete the user
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();

        // Act - Restore the user
        var deletedUser = await _context.GetOnlyDeleted<User>()
            .FirstOrDefaultAsync(u => u.Id == user.Id);
        Assert.NotNull(deletedUser);
        
        _context.Restore(deletedUser);
        await _context.SaveChangesAsync();

        // Assert - User should be queryable again
        var restoredUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == user.Id);
        Assert.NotNull(restoredUser);
        Assert.False(restoredUser.IsDeleted);
    }

    [Fact]
    public async Task HardDelete_ShouldPhysicallyRemoveEntity_FromDatabase()
    {
        // Arrange
        var user = new User { Name = "Test User", Email = "test@example.com", PasswordHash = "hash123" };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var userId = user.Id;

        // Act - Hard delete the user
        _context.HardDelete(user);
        await _context.SaveChangesAsync();

        // Assert - User should not exist even with IgnoreQueryFilters
        var deletedUser = await _context.GetIncludingDeleted<User>()
            .FirstOrDefaultAsync(u => u.Id == userId);
        Assert.Null(deletedUser);
    }

    [Fact]
    public async Task SoftDelete_ShouldUpdateUpdatedAtTimestamp()
    {
        // Arrange
        var user = new User { Name = "Test User", Email = "test@example.com", PasswordHash = "hash123" };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var originalUpdatedAt = user.UpdatedAt;
        await Task.Delay(10); // Small delay to ensure timestamp difference

        // Act - Delete the user
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();

        // Assert - UpdatedAt should be updated
        var deletedUser = await _context.GetIncludingDeleted<User>()
            .FirstOrDefaultAsync(u => u.Id == user.Id);
        
        Assert.NotNull(deletedUser);
        Assert.True(deletedUser.UpdatedAt > originalUpdatedAt);
    }

    [Fact]
    public async Task SoftDelete_ShouldWorkWithChallenges()
    {
        // Arrange
        var challenge = new Challenge
        {
            Title = "Test Challenge",
            Description = "Test Description",
            Difficulty = Difficulty.Easy,
            StarterCode = "// Start here"
        };
        _context.Challenges.Add(challenge);
        await _context.SaveChangesAsync();

        var challengeId = challenge.Id;

        // Act - Delete the challenge
        _context.Challenges.Remove(challenge);
        await _context.SaveChangesAsync();

        // Assert - Challenge should be soft deleted
        var deletedChallenge = await _context.GetIncludingDeleted<Challenge>()
            .FirstOrDefaultAsync(c => c.Id == challengeId);

        Assert.NotNull(deletedChallenge);
        Assert.True(deletedChallenge.IsDeleted);

        // Normal query should not return it
        var activeChallenge = await _context.Challenges
            .FirstOrDefaultAsync(c => c.Id == challengeId);
        Assert.Null(activeChallenge);
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
