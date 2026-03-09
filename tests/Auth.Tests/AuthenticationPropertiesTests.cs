using Auth.Service;
using FsCheck;
using FsCheck.Xunit;
using Microsoft.EntityFrameworkCore;
using Shared.Data;
using Shared.Entities;
using Shared.Repositories;

namespace Auth.Tests;

/// <summary>
/// Property-based tests for authentication functionality.
/// Feature: aspnet-learning-platform
/// </summary>
public class AuthenticationPropertiesTests : IDisposable
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

    /// <summary>
    /// Property 2: Authentication Round Trip
    /// **Validates: Requirements 1.2**
    /// 
    /// For any valid user registration, authenticating with the same credentials 
    /// should succeed and return a valid session token.
    /// </summary>
    [Property(MaxTest = 20)]
    public void AuthenticationRoundTrip_SucceedsForValidUser(NonEmptyString name, NonEmptyString email, NonEmptyString password)
    {
        // Arrange
        var context = CreateInMemoryContext();
        var authService = CreateAuthService(context);
        
        // Ensure name is not just whitespace
        var nameStr = name.Get.Trim();
        if (string.IsNullOrWhiteSpace(nameStr))
        {
            nameStr = "ValidName";
        }
        
        // Ensure email is not just whitespace
        var emailBase = email.Get.Trim();
        if (string.IsNullOrWhiteSpace(emailBase))
        {
            emailBase = "valid";
        }
        var emailStr = $"{emailBase}@example.com"; // Ensure valid email format
        
        var passwordStr = password.Get.Length >= 8 ? password.Get : password.Get + "12345678"; // Ensure min length

        // Act - Register
        var registerResult = authService.RegisterAsync(nameStr, emailStr, passwordStr).Result;

        // Act - Login with same credentials
        var loginResult = authService.LoginAsync(emailStr, passwordStr).Result;

        // Assert
        Assert.True(registerResult.Success);
        Assert.True(loginResult.Success);
        Assert.False(string.IsNullOrEmpty(loginResult.Token));
        Assert.Equal(registerResult.UserId, loginResult.UserId);
        Assert.Equal(emailStr, loginResult.Email);
        Assert.Equal(nameStr, loginResult.Name);
    }

    /// <summary>
    /// Property 3: Invalid Credentials Rejection
    /// **Validates: Requirements 1.3**
    /// 
    /// For any invalid credentials (wrong password, non-existent email, malformed input), 
    /// authentication attempts should be rejected with an appropriate error message.
    /// </summary>
    [Property(MaxTest = 20)]
    public void InvalidCredentials_AreRejected(NonEmptyString email, NonEmptyString password, NonEmptyString wrongPassword)
    {
        // Arrange
        var context = CreateInMemoryContext();
        var authService = CreateAuthService(context);
        
        var emailStr = $"{email.Get}@example.com";
        var passwordStr = password.Get.Length >= 8 ? password.Get : password.Get + "12345678";
        var wrongPasswordStr = wrongPassword.Get.Length >= 8 ? wrongPassword.Get : wrongPassword.Get + "87654321";
        
        // Ensure passwords are different
        if (passwordStr == wrongPasswordStr)
        {
            wrongPasswordStr = passwordStr + "X";
        }

        // Register a user
        var registerResult = authService.RegisterAsync("Test User", emailStr, passwordStr).Result;

        // Act - Try to login with wrong password
        var loginWithWrongPassword = authService.LoginAsync(emailStr, wrongPasswordStr).Result;

        // Act - Try to login with non-existent email
        var loginWithWrongEmail = authService.LoginAsync("nonexistent@example.com", passwordStr).Result;

        // Assert
        Assert.True(registerResult.Success);
        Assert.False(loginWithWrongPassword.Success);
        Assert.False(string.IsNullOrEmpty(loginWithWrongPassword.ErrorMessage));
        Assert.False(loginWithWrongEmail.Success);
        Assert.False(string.IsNullOrEmpty(loginWithWrongEmail.ErrorMessage));
    }

    /// <summary>
    /// Property: Empty credentials are rejected
    /// 
    /// For any empty or null credentials, authentication should fail with appropriate error.
    /// </summary>
    [Fact]
    public async Task EmptyCredentials_AreRejected()
    {
        // Arrange
        var context = CreateInMemoryContext();
        var authService = CreateAuthService(context);

        // Act & Assert - Empty email
        var result1 = await authService.LoginAsync("", "password123");
        Assert.False(result1.Success);
        Assert.NotEmpty(result1.ErrorMessage);

        // Act & Assert - Empty password
        var result2 = await authService.LoginAsync("test@example.com", "");
        Assert.False(result2.Success);
        Assert.NotEmpty(result2.ErrorMessage);

        // Act & Assert - Both empty
        var result3 = await authService.LoginAsync("", "");
        Assert.False(result3.Success);
        Assert.NotEmpty(result3.ErrorMessage);
    }

    /// <summary>
    /// Property: Registration with duplicate email fails
    /// 
    /// For any email that's already registered, attempting to register again should fail.
    /// </summary>
    [Property(MaxTest = 20)]
    public void DuplicateEmail_RegistrationFails(NonEmptyString name, NonEmptyString email, NonEmptyString password)
    {
        // Arrange
        var context = CreateInMemoryContext();
        var authService = CreateAuthService(context);
        
        // Ensure name is not just whitespace
        var nameStr = name.Get.Trim();
        if (string.IsNullOrWhiteSpace(nameStr))
        {
            nameStr = "ValidName";
        }
        
        // Ensure email is not just whitespace
        var emailBase = email.Get.Trim();
        if (string.IsNullOrWhiteSpace(emailBase))
        {
            emailBase = "valid";
        }
        var emailStr = $"{emailBase}@example.com";
        
        var passwordStr = password.Get.Length >= 8 ? password.Get : password.Get + "12345678";

        // Act - Register first time
        var firstRegistration = authService.RegisterAsync(nameStr, emailStr, passwordStr).Result;

        // Act - Try to register again with same email
        var secondRegistration = authService.RegisterAsync("Different Name", emailStr, "DifferentPass123").Result;

        // Assert
        Assert.True(firstRegistration.Success);
        Assert.False(secondRegistration.Success);
        Assert.False(string.IsNullOrEmpty(secondRegistration.ErrorMessage));
        Assert.Contains("already registered", secondRegistration.ErrorMessage, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Property: Short passwords are rejected
    /// 
    /// For any password shorter than 8 characters, registration should fail.
    /// </summary>
    [Property(MaxTest = 20)]
    public void ShortPassword_RegistrationFails(NonEmptyString name, NonEmptyString email)
    {
        // Arrange
        var context = CreateInMemoryContext();
        var authService = CreateAuthService(context);
        
        // Ensure name is not just whitespace
        var nameStr = name.Get.Trim();
        if (string.IsNullOrWhiteSpace(nameStr))
        {
            nameStr = "ValidName";
        }
        
        // Ensure email is not just whitespace
        var emailBase = email.Get.Trim();
        if (string.IsNullOrWhiteSpace(emailBase))
        {
            emailBase = "valid";
        }
        var emailStr = $"{emailBase}@example.com";
        
        var shortPassword = "short"; // Less than 8 characters

        // Act
        var result = authService.RegisterAsync(nameStr, emailStr, shortPassword).Result;

        // Assert
        Assert.False(result.Success);
        Assert.False(string.IsNullOrEmpty(result.ErrorMessage));
        Assert.Contains("8 characters", result.ErrorMessage, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Property: Successful registration creates user and returns token
    /// 
    /// For any valid registration, a user should be created and a token should be returned.
    /// </summary>
    [Property(MaxTest = 20)]
    public void SuccessfulRegistration_CreatesUserAndReturnsToken(NonEmptyString name, NonEmptyString email, NonEmptyString password)
    {
        // Arrange
        var context = CreateInMemoryContext();
        var authService = CreateAuthService(context);
        
        // Ensure name is not just whitespace
        var nameStr = name.Get.Trim();
        if (string.IsNullOrWhiteSpace(nameStr))
        {
            nameStr = "ValidName";
        }
        
        // Ensure email is not just whitespace
        var emailBase = email.Get.Trim();
        if (string.IsNullOrWhiteSpace(emailBase))
        {
            emailBase = "valid";
        }
        var emailStr = $"{emailBase}@example.com";
        
        var passwordStr = password.Get.Length >= 8 ? password.Get : password.Get + "12345678";

        // Act
        var result = authService.RegisterAsync(nameStr, emailStr, passwordStr).Result;

        // Assert
        Assert.True(result.Success);
        Assert.NotEqual(Guid.Empty, result.UserId);
        Assert.False(string.IsNullOrEmpty(result.Token));
        Assert.True(result.ExpiresAt > DateTime.UtcNow);
    }

    public void Dispose()
    {
        foreach (var context in _contexts)
        {
            context.Dispose();
        }
    }
}
