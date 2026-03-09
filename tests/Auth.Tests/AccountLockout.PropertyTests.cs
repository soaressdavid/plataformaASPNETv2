using Auth.Service;
using FsCheck;
using FsCheck.Xunit;
using Microsoft.EntityFrameworkCore;
using Shared.Data;
using Shared.Entities;
using Shared.Repositories;

namespace Auth.Tests;

/// <summary>
/// Property-based tests for account lockout mechanism.
/// Feature: aspnet-learning-platform
/// </summary>
public class AccountLockoutPropertyTests
{
    /// <summary>
    /// Property 28: Account Lockout
    /// **Validates: Requirements 51.12**
    /// 
    /// For any user account, after 5 consecutive failed login attempts, the account should be 
    /// locked for 30 minutes. During lockout, login attempts should fail with a lockout message.
    /// After lockout expires, the user should be able to login again.
    /// </summary>
    [Fact]
    public void AccountLockout_LocksAfter5FailedAttempts()
    {
        // Arrange
        var emailStr = "test@test.com";
        var passwordStr = "CorrectPassword123!";
        var wrongPasswordStr = "WrongPassword456!";

        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: $"TestDb_{Guid.NewGuid()}")
            .Options;

        using var context = new ApplicationDbContext(options);
        var userRepository = new UserRepository(context);
        var passwordHasher = new PasswordHasher();
        var tokenService = JwtTokenService.CreateWithGeneratedKeys();
        var authService = new AuthService(userRepository, passwordHasher, tokenService);

        // Create a user
        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = "Test User",
            Email = emailStr,
            PasswordHash = passwordHasher.HashPassword(passwordStr),
            FailedLoginAttempts = 0,
            LockoutEnd = null,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        context.Users.Add(user);
        context.SaveChanges();

        // Act - Attempt 5 failed logins
        for (int i = 0; i < 5; i++)
        {
            var result = authService.LoginAsync(emailStr, wrongPasswordStr).GetAwaiter().GetResult();
            
            if (i < 4)
            {
                // First 4 attempts should fail but not lock
                Assert.False(result.Success);
                Assert.False(result.IsLockedOut);
            }
            else
            {
                // 5th attempt should lock the account
                Assert.False(result.Success);
                Assert.True(result.IsLockedOut);
                Assert.Contains("locked", result.ErrorMessage.ToLower());
            }
        }

        // Assert - Verify account is locked
        var lockedUser = context.Users.First(u => u.Email == emailStr);
        Assert.Equal(5, lockedUser.FailedLoginAttempts);
        Assert.NotNull(lockedUser.LockoutEnd);
        Assert.True(lockedUser.IsLockedOut);

        // Assert - Even correct password should fail during lockout
        var lockedResult = authService.LoginAsync(emailStr, passwordStr).GetAwaiter().GetResult();
        Assert.False(lockedResult.Success);
        Assert.True(lockedResult.IsLockedOut);
    }

    /// <summary>
    /// Property: Failed attempts reset on successful login
    /// 
    /// For any user account with failed login attempts, a successful login should reset 
    /// the failed attempts counter to 0.
    /// </summary>
    [Property(MaxTest = 20)]
    public void SuccessfulLogin_ResetsFailedAttempts(NonEmptyString email, NonEmptyString password, NonEmptyString wrongPassword)
    {
        // Arrange
        var emailStr = email.Get + "@test.com";
        var passwordStr = password.Get;
        var wrongPasswordStr = wrongPassword.Get;
        
        if (passwordStr == wrongPasswordStr)
        {
            wrongPasswordStr = passwordStr + "wrong";
        }

        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: $"TestDb_{Guid.NewGuid()}")
            .Options;

        using var context = new ApplicationDbContext(options);
        var userRepository = new UserRepository(context);
        var passwordHasher = new PasswordHasher();
        var tokenService = JwtTokenService.CreateWithGeneratedKeys();
        var authService = new AuthService(userRepository, passwordHasher, tokenService);

        // Create a user
        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = "Test User",
            Email = emailStr,
            PasswordHash = passwordHasher.HashPassword(passwordStr),
            FailedLoginAttempts = 0,
            LockoutEnd = null,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        context.Users.Add(user);
        context.SaveChanges();

        // Act - Attempt 3 failed logins
        for (int i = 0; i < 3; i++)
        {
            authService.LoginAsync(emailStr, wrongPasswordStr).GetAwaiter().GetResult();
        }

        // Verify failed attempts were recorded
        var userAfterFails = context.Users.First(u => u.Email == emailStr);
        Assert.Equal(3, userAfterFails.FailedLoginAttempts);

        // Act - Successful login
        var successResult = authService.LoginAsync(emailStr, passwordStr).GetAwaiter().GetResult();

        // Assert - Failed attempts should be reset
        Assert.True(successResult.Success);
        var userAfterSuccess = context.Users.First(u => u.Email == emailStr);
        Assert.Equal(0, userAfterSuccess.FailedLoginAttempts);
        Assert.Null(userAfterSuccess.LockoutEnd);
    }

    /// <summary>
    /// Property: Lockout duration is 30 minutes
    /// 
    /// For any locked account, the lockout end time should be approximately 30 minutes 
    /// from the time of the 5th failed attempt.
    /// </summary>
    [Property(MaxTest = 20)]
    public void AccountLockout_DurationIs30Minutes(NonEmptyString email, NonEmptyString password, NonEmptyString wrongPassword)
    {
        // Arrange
        var emailStr = email.Get + "@test.com";
        var passwordStr = password.Get;
        var wrongPasswordStr = wrongPassword.Get;
        
        if (passwordStr == wrongPasswordStr)
        {
            wrongPasswordStr = passwordStr + "wrong";
        }

        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: $"TestDb_{Guid.NewGuid()}")
            .Options;

        using var context = new ApplicationDbContext(options);
        var userRepository = new UserRepository(context);
        var passwordHasher = new PasswordHasher();
        var tokenService = JwtTokenService.CreateWithGeneratedKeys();
        var authService = new AuthService(userRepository, passwordHasher, tokenService);

        // Create a user
        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = "Test User",
            Email = emailStr,
            PasswordHash = passwordHasher.HashPassword(passwordStr),
            FailedLoginAttempts = 0,
            LockoutEnd = null,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        context.Users.Add(user);
        context.SaveChanges();

        var beforeLockout = DateTime.UtcNow;

        // Act - Attempt 5 failed logins to trigger lockout
        for (int i = 0; i < 5; i++)
        {
            authService.LoginAsync(emailStr, wrongPasswordStr).GetAwaiter().GetResult();
        }

        var afterLockout = DateTime.UtcNow;

        // Assert - Lockout end should be approximately 30 minutes from now
        var lockedUser = context.Users.First(u => u.Email == emailStr);
        Assert.NotNull(lockedUser.LockoutEnd);
        
        var expectedLockoutEnd = beforeLockout.AddMinutes(30);
        var actualLockoutEnd = lockedUser.LockoutEnd!.Value;
        
        // Allow 1 minute tolerance for test execution time
        var timeDifference = Math.Abs((actualLockoutEnd - expectedLockoutEnd).TotalMinutes);
        Assert.True(timeDifference < 1, $"Lockout duration should be 30 minutes, but was {(actualLockoutEnd - beforeLockout).TotalMinutes} minutes");
    }

    /// <summary>
    /// Property: Failed attempts increment correctly
    /// 
    /// For any user account, each failed login attempt should increment the 
    /// FailedLoginAttempts counter by exactly 1.
    /// </summary>
    [Property(MaxTest = 20)]
    public void FailedLogin_IncrementsAttemptsByOne(NonEmptyString email, NonEmptyString password, NonEmptyString wrongPassword, PositiveInt attempts)
    {
        // Arrange
        var emailStr = email.Get + "@test.com";
        var passwordStr = password.Get;
        var wrongPasswordStr = wrongPassword.Get;
        var attemptCount = Math.Min(attempts.Get, 4); // Max 4 to avoid lockout
        
        if (passwordStr == wrongPasswordStr)
        {
            wrongPasswordStr = passwordStr + "wrong";
        }

        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: $"TestDb_{Guid.NewGuid()}")
            .Options;

        using var context = new ApplicationDbContext(options);
        var userRepository = new UserRepository(context);
        var passwordHasher = new PasswordHasher();
        var tokenService = JwtTokenService.CreateWithGeneratedKeys();
        var authService = new AuthService(userRepository, passwordHasher, tokenService);

        // Create a user
        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = "Test User",
            Email = emailStr,
            PasswordHash = passwordHasher.HashPassword(passwordStr),
            FailedLoginAttempts = 0,
            LockoutEnd = null,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        context.Users.Add(user);
        context.SaveChanges();

        // Act - Attempt N failed logins
        for (int i = 0; i < attemptCount; i++)
        {
            authService.LoginAsync(emailStr, wrongPasswordStr).GetAwaiter().GetResult();
        }

        // Assert - Failed attempts should equal attempt count
        var userAfterFails = context.Users.First(u => u.Email == emailStr);
        Assert.Equal(attemptCount, userAfterFails.FailedLoginAttempts);
    }
}


