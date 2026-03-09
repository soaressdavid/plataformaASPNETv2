using Auth.Service;
using FsCheck;
using FsCheck.Xunit;

namespace Auth.Tests;

/// <summary>
/// Property-based tests for password hashing functionality.
/// Feature: aspnet-learning-platform
/// </summary>
public class PasswordHashingPropertiesTests
{
    private readonly PasswordHasher _passwordHasher = new();

    /// <summary>
    /// Property 1: Password Hashing with Salt
    /// **Validates: Requirements 1.1, 1.4**
    /// 
    /// For any valid password, storing it in the system should result in a hash that is not equal 
    /// to the plaintext password, and hashing the same password twice should produce different 
    /// hashes due to unique salts.
    /// </summary>
    [Property(MaxTest = 20)]
    public void PasswordHashing_ProducesDifferentHashesForSamePassword(NonEmptyString password)
    {
        var passwordStr = password.Get;
        
        // Hash the same password twice
        var hash1 = _passwordHasher.HashPassword(passwordStr);
        var hash2 = _passwordHasher.HashPassword(passwordStr);
        
        // Verify all three conditions:
        // 1. Hash is not equal to plaintext password
        // 2. Second hash is not equal to plaintext password
        // 3. The two hashes are different (due to unique salts)
        Assert.NotEqual(passwordStr, hash1);
        Assert.NotEqual(passwordStr, hash2);
        Assert.NotEqual(hash1, hash2);
    }

    /// <summary>
    /// Property: Password verification works correctly
    /// 
    /// For any valid password, after hashing it, verifying the original password 
    /// against the hash should succeed.
    /// </summary>
    [Property(MaxTest = 20)]
    public void PasswordHashing_VerificationSucceedsForCorrectPassword(NonEmptyString password)
    {
        var passwordStr = password.Get;
        var hash = _passwordHasher.HashPassword(passwordStr);
        
        // Verification should succeed for the correct password
        Assert.True(_passwordHasher.VerifyPassword(passwordStr, hash));
    }

    /// <summary>
    /// Property: Password verification fails for incorrect passwords
    /// 
    /// For any two different passwords, verifying one against the hash of the other should fail.
    /// </summary>
    [Property(MaxTest = 20)]
    public void PasswordHashing_VerificationFailsForIncorrectPassword(NonEmptyString password1, NonEmptyString password2)
    {
        var pwd1 = password1.Get;
        var pwd2 = password2.Get;
        
        // Only test when passwords are different
        if (pwd1 == pwd2)
            return;
        
        var hash = _passwordHasher.HashPassword(pwd1);
        
        // Verification should fail for a different password
        Assert.False(_passwordHasher.VerifyPassword(pwd2, hash));
    }

    /// <summary>
    /// Property: Hash format is consistent
    /// 
    /// For any valid password, the hash should be a non-empty string that starts with BCrypt's 
    /// standard prefix ($2a$, $2b$, or $2y$).
    /// </summary>
    [Property(MaxTest = 20)]
    public void PasswordHashing_ProducesValidBCryptFormat(NonEmptyString password)
    {
        var hash = _passwordHasher.HashPassword(password.Get);
        
        // BCrypt hashes start with $2a$, $2b$, or $2y$ followed by work factor
        Assert.False(string.IsNullOrEmpty(hash));
        Assert.True(hash.StartsWith("$2a$") || hash.StartsWith("$2b$") || hash.StartsWith("$2y$"));
    }
}
