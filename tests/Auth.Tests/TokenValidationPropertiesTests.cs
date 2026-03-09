using Auth.Service;
using FsCheck;
using FsCheck.Xunit;
using System.IdentityModel.Tokens.Jwt;

namespace Auth.Tests;

/// <summary>
/// Property-based tests for JWT token validation functionality.
/// Feature: aspnet-learning-platform
/// </summary>
public class TokenValidationPropertiesTests
{
    /// <summary>
    /// Property 4: Token Validation
    /// **Validates: Requirements 1.5**
    /// 
    /// For any authenticated request, the API gateway should validate the session token 
    /// before processing, accepting valid tokens and rejecting invalid or expired tokens.
    /// </summary>
    [Property(MaxTest = 20)]
    public void TokenValidation_AcceptsValidTokens(Guid userId, NonEmptyString email, NonEmptyString name)
    {
        // Arrange
        var tokenService = JwtTokenService.CreateWithGeneratedKeys();
        var token = tokenService.GenerateToken(userId, email.Get, name.Get);

        // Act
        var (isValid, principal, errorMessage) = tokenService.ValidateToken(token);

        // Assert
        Assert.True(isValid, $"Valid token should be accepted. Error: {errorMessage}");
        Assert.NotNull(principal);
        Assert.Null(errorMessage);
        
        // Verify claims are present
        var extractedUserId = tokenService.GetUserIdFromPrincipal(principal!);
        Assert.Equal(userId, extractedUserId);
    }

    /// <summary>
    /// Property: Token validation rejects invalid tokens
    /// 
    /// For any malformed or tampered token, validation should fail.
    /// </summary>
    [Property(MaxTest = 20)]
    public void TokenValidation_RejectsInvalidTokens(NonEmptyString invalidToken)
    {
        // Arrange
        var tokenService = JwtTokenService.CreateWithGeneratedKeys();
        var token = invalidToken.Get;
        
        // Skip if by chance the random string is a valid JWT format
        if (token.Split('.').Length == 3)
            return;

        // Act
        var (isValid, principal, errorMessage) = tokenService.ValidateToken(token);

        // Assert
        Assert.False(isValid);
        Assert.Null(principal);
        Assert.NotNull(errorMessage);
    }

    /// <summary>
    /// Property: Token validation rejects tokens signed with different keys
    /// 
    /// For any token signed with a different key, validation should fail.
    /// </summary>
    [Property(MaxTest = 20)]
    public void TokenValidation_RejectsTokensSignedWithDifferentKeys(Guid userId, NonEmptyString email, NonEmptyString name)
    {
        // Arrange
        var tokenService1 = JwtTokenService.CreateWithGeneratedKeys();
        var tokenService2 = JwtTokenService.CreateWithGeneratedKeys();
        
        // Generate token with first service
        var token = tokenService1.GenerateToken(userId, email.Get, name.Get);

        // Act - Try to validate with second service (different keys)
        var (isValid, principal, errorMessage) = tokenService2.ValidateToken(token);

        // Assert
        Assert.False(isValid);
        Assert.Null(principal);
        Assert.NotNull(errorMessage);
        Assert.Contains("signature", errorMessage, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Property: Token validation rejects empty or null tokens
    /// 
    /// For any null or empty token string, validation should fail with appropriate error.
    /// </summary>
    [Fact]
    public void TokenValidation_RejectsEmptyTokens()
    {
        // Arrange
        var tokenService = JwtTokenService.CreateWithGeneratedKeys();

        // Act & Assert - Empty token
        var (isValid1, principal1, errorMessage1) = tokenService.ValidateToken(string.Empty);
        Assert.False(isValid1);
        Assert.Null(principal1);
        Assert.NotNull(errorMessage1);

        // Act & Assert - Null token
        var (isValid2, principal2, errorMessage2) = tokenService.ValidateToken(null!);
        Assert.False(isValid2);
        Assert.Null(principal2);
        Assert.NotNull(errorMessage2);
    }

    /// <summary>
    /// Property: Generated tokens have correct expiration (24 hours)
    /// 
    /// For any generated token, it should expire in 24 hours.
    /// </summary>
    [Property(MaxTest = 20)]
    public void TokenGeneration_SetsCorrectExpiration(Guid userId, NonEmptyString email, NonEmptyString name)
    {
        // Arrange
        var tokenService = JwtTokenService.CreateWithGeneratedKeys();
        var beforeGeneration = DateTime.UtcNow;

        // Act
        var token = tokenService.GenerateToken(userId, email.Get, name.Get);

        // Parse the token to check expiration
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);
        var expiration = jwtToken.ValidTo;
        var afterGeneration = DateTime.UtcNow;

        // Assert - Token should expire approximately 24 hours from now
        var expectedExpiration = beforeGeneration.AddHours(24);
        var timeDifference = Math.Abs((expiration - expectedExpiration).TotalMinutes);
        
        // Allow 1 minute tolerance for test execution time
        Assert.True(timeDifference < 1, $"Token expiration should be 24 hours from generation. Difference: {timeDifference} minutes");
    }

    /// <summary>
    /// Property: Token contains correct user information
    /// 
    /// For any generated token, it should contain the correct user ID, email, and name in claims.
    /// </summary>
    [Property(MaxTest = 20)]
    public void TokenGeneration_ContainsCorrectUserInformation(Guid userId, NonEmptyString email, NonEmptyString name)
    {
        // Arrange
        var tokenService = JwtTokenService.CreateWithGeneratedKeys();

        // Act
        var token = tokenService.GenerateToken(userId, email.Get, name.Get);
        var (isValid, principal, _) = tokenService.ValidateToken(token);

        // Assert
        Assert.True(isValid);
        Assert.NotNull(principal);

        var extractedUserId = tokenService.GetUserIdFromPrincipal(principal!);
        var emailClaim = principal!.FindFirst(JwtRegisteredClaimNames.Email)?.Value 
                         ?? principal!.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
        var nameClaim = principal!.FindFirst(JwtRegisteredClaimNames.Name)?.Value
                        ?? principal!.FindFirst(System.Security.Claims.ClaimTypes.Name)?.Value;

        Assert.Equal(userId, extractedUserId);
        Assert.Equal(email.Get, emailClaim);
        Assert.Equal(name.Get, nameClaim);
    }
}
