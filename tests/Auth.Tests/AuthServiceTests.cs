using Auth.Service;
using Microsoft.EntityFrameworkCore;
using Shared.Data;
using Shared.Repositories;

namespace Auth.Tests;

/// <summary>
/// Unit tests for AuthService functionality.
/// Feature: aspnet-learning-platform
/// </summary>
public class AuthServiceTests : IDisposable
{
    private readonly List<ApplicationDbContext> _contexts = new();

    private ApplicationDbContext CreateInMemoryContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: $"TestDb_{Guid.NewGuid()}")
            .Options;

        var context = new ApplicationDbContext(options);
        _contexts.Add(context);
        return context;
    }

    private AuthService CreateAuthService(ApplicationDbContext context)
    {
        var userRepository = new UserRepository(context);
        var passwordHasher = new PasswordHasher();
        var tokenService = JwtTokenService.CreateWithGeneratedKeys();
        return new AuthService(userRepository, passwordHasher, tokenService);
    }

    [Fact]
    public async Task RegisterAsync_WithValidData_CreatesUserAndReturnsToken()
    {
        // Arrange
        var context = CreateInMemoryContext();
        var authService = CreateAuthService(context);

        // Act
        var result = await authService.RegisterAsync("John Doe", "john@example.com", "password123");

        // Assert
        Assert.True(result.Success);
        Assert.NotEqual(Guid.Empty, result.UserId);
        Assert.False(string.IsNullOrEmpty(result.Token));
        Assert.True(result.ExpiresAt > DateTime.UtcNow);
        Assert.Empty(result.ErrorMessage);
    }

    [Fact]
    public async Task RegisterAsync_WithEmptyName_ReturnsError()
    {
        // Arrange
        var context = CreateInMemoryContext();
        var authService = CreateAuthService(context);

        // Act
        var result = await authService.RegisterAsync("", "john@example.com", "password123");

        // Assert
        Assert.False(result.Success);
        Assert.Contains("Name is required", result.ErrorMessage);
        Assert.Equal(Guid.Empty, result.UserId);
        Assert.Empty(result.Token);
    }

    [Fact]
    public async Task RegisterAsync_WithEmptyEmail_ReturnsError()
    {
        // Arrange
        var context = CreateInMemoryContext();
        var authService = CreateAuthService(context);

        // Act
        var result = await authService.RegisterAsync("John Doe", "", "password123");

        // Assert
        Assert.False(result.Success);
        Assert.Contains("Email is required", result.ErrorMessage);
        Assert.Equal(Guid.Empty, result.UserId);
        Assert.Empty(result.Token);
    }

    [Fact]
    public async Task RegisterAsync_WithEmptyPassword_ReturnsError()
    {
        // Arrange
        var context = CreateInMemoryContext();
        var authService = CreateAuthService(context);

        // Act
        var result = await authService.RegisterAsync("John Doe", "john@example.com", "");

        // Assert
        Assert.False(result.Success);
        Assert.Contains("Password is required", result.ErrorMessage);
        Assert.Equal(Guid.Empty, result.UserId);
        Assert.Empty(result.Token);
    }

    [Fact]
    public async Task RegisterAsync_WithShortPassword_ReturnsError()
    {
        // Arrange
        var context = CreateInMemoryContext();
        var authService = CreateAuthService(context);

        // Act
        var result = await authService.RegisterAsync("John Doe", "john@example.com", "short");

        // Assert
        Assert.False(result.Success);
        Assert.Contains("at least 8 characters", result.ErrorMessage);
        Assert.Equal(Guid.Empty, result.UserId);
        Assert.Empty(result.Token);
    }

    [Fact]
    public async Task RegisterAsync_WithDuplicateEmail_ReturnsError()
    {
        // Arrange
        var context = CreateInMemoryContext();
        var authService = CreateAuthService(context);

        // Register first user
        await authService.RegisterAsync("John Doe", "john@example.com", "password123");

        // Act - Try to register with same email
        var result = await authService.RegisterAsync("Jane Doe", "john@example.com", "password456");

        // Assert
        Assert.False(result.Success);
        Assert.Contains("already registered", result.ErrorMessage);
        Assert.Equal(Guid.Empty, result.UserId);
        Assert.Empty(result.Token);
    }

    [Fact]
    public async Task LoginAsync_WithValidCredentials_ReturnsTokenAndUserInfo()
    {
        // Arrange
        var context = CreateInMemoryContext();
        var authService = CreateAuthService(context);

        // Register a user
        var registerResult = await authService.RegisterAsync("John Doe", "john@example.com", "password123");

        // Act
        var result = await authService.LoginAsync("john@example.com", "password123");

        // Assert
        Assert.True(result.Success);
        Assert.Equal(registerResult.UserId, result.UserId);
        Assert.Equal("John Doe", result.Name);
        Assert.Equal("john@example.com", result.Email);
        Assert.False(string.IsNullOrEmpty(result.Token));
        Assert.True(result.ExpiresAt > DateTime.UtcNow);
        Assert.Empty(result.ErrorMessage);
    }

    [Fact]
    public async Task LoginAsync_WithEmptyEmail_ReturnsError()
    {
        // Arrange
        var context = CreateInMemoryContext();
        var authService = CreateAuthService(context);

        // Act
        var result = await authService.LoginAsync("", "password123");

        // Assert
        Assert.False(result.Success);
        Assert.Contains("Email is required", result.ErrorMessage);
        Assert.Equal(Guid.Empty, result.UserId);
        Assert.Empty(result.Token);
    }

    [Fact]
    public async Task LoginAsync_WithEmptyPassword_ReturnsError()
    {
        // Arrange
        var context = CreateInMemoryContext();
        var authService = CreateAuthService(context);

        // Act
        var result = await authService.LoginAsync("john@example.com", "");

        // Assert
        Assert.False(result.Success);
        Assert.Contains("Password is required", result.ErrorMessage);
        Assert.Equal(Guid.Empty, result.UserId);
        Assert.Empty(result.Token);
    }

    [Fact]
    public async Task LoginAsync_WithNonExistentEmail_ReturnsError()
    {
        // Arrange
        var context = CreateInMemoryContext();
        var authService = CreateAuthService(context);

        // Act
        var result = await authService.LoginAsync("nonexistent@example.com", "password123");

        // Assert
        Assert.False(result.Success);
        Assert.Contains("Invalid email or password", result.ErrorMessage);
        Assert.Equal(Guid.Empty, result.UserId);
        Assert.Empty(result.Token);
    }

    [Fact]
    public async Task LoginAsync_WithIncorrectPassword_ReturnsError()
    {
        // Arrange
        var context = CreateInMemoryContext();
        var authService = CreateAuthService(context);

        // Register a user
        await authService.RegisterAsync("John Doe", "john@example.com", "password123");

        // Act - Try to login with wrong password
        var result = await authService.LoginAsync("john@example.com", "wrongpassword");

        // Assert
        Assert.False(result.Success);
        Assert.Contains("Invalid email or password", result.ErrorMessage);
        Assert.Equal(Guid.Empty, result.UserId);
        Assert.Empty(result.Token);
    }

    [Fact]
    public async Task LoginAsync_AfterRegistration_TokenIsValid()
    {
        // Arrange
        var context = CreateInMemoryContext();
        var authService = CreateAuthService(context);

        // Register a user
        var registerResult = await authService.RegisterAsync("John Doe", "john@example.com", "password123");

        // Act - Login
        var loginResult = await authService.LoginAsync("john@example.com", "password123");

        // Assert - Both tokens should be valid (not testing equality since they're generated at different times)
        Assert.True(registerResult.Success);
        Assert.True(loginResult.Success);
        Assert.False(string.IsNullOrEmpty(registerResult.Token));
        Assert.False(string.IsNullOrEmpty(loginResult.Token));
        Assert.Equal(registerResult.UserId, loginResult.UserId);
    }

    [Fact]
    public async Task RegisterAsync_StoresHashedPassword_NotPlaintext()
    {
        // Arrange
        var context = CreateInMemoryContext();
        var authService = CreateAuthService(context);
        var password = "password123";

        // Act
        var result = await authService.RegisterAsync("John Doe", "john@example.com", password);

        // Assert
        var user = await context.Users.FirstOrDefaultAsync(u => u.Id == result.UserId);
        Assert.NotNull(user);
        Assert.NotEqual(password, user.PasswordHash);
        Assert.StartsWith("$2", user.PasswordHash); // BCrypt hash prefix
    }

    public void Dispose()
    {
        foreach (var context in _contexts)
        {
            context.Dispose();
        }
    }
}
