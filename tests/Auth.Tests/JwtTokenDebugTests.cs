using Auth.Service;
using System.IdentityModel.Tokens.Jwt;

namespace Auth.Tests;

public class JwtTokenDebugTests
{
    [Fact]
    public void DebugTokenClaims()
    {
        // Arrange
        var tokenService = JwtTokenService.CreateWithGeneratedKeys();
        var userId = Guid.NewGuid();
        var email = "test@example.com";
        var name = "Test User";

        // Act
        var token = tokenService.GenerateToken(userId, email, name);
        var (isValid, principal, errorMessage) = tokenService.ValidateToken(token);

        // Debug output
        Console.WriteLine($"Token: {token}");
        Console.WriteLine($"IsValid: {isValid}");
        Console.WriteLine($"ErrorMessage: {errorMessage}");
        
        if (principal != null)
        {
            Console.WriteLine("\nClaims:");
            foreach (var claim in principal.Claims)
            {
                Console.WriteLine($"  {claim.Type}: {claim.Value}");
            }

            var extractedUserId = tokenService.GetUserIdFromPrincipal(principal);
            Console.WriteLine($"\nExtracted UserId: {extractedUserId}");
            Console.WriteLine($"Expected UserId: {userId}");
        }

        // Assert
        Assert.True(isValid);
        Assert.NotNull(principal);
    }
}
