using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;

namespace Auth.Service;

/// <summary>
/// Provides JWT token generation and validation using RS256 algorithm with 24-hour expiration.
/// </summary>
public class JwtTokenService
{
    private readonly RsaSecurityKey _privateKey;
    private readonly RsaSecurityKey _publicKey;
    private readonly JwtSecurityTokenHandler _tokenHandler;
    private const int TokenExpirationHours = 24;

    /// <summary>
    /// Initializes a new instance of the JwtTokenService with RSA key pair.
    /// </summary>
    /// <param name="privateKey">RSA private key for signing tokens</param>
    /// <param name="publicKey">RSA public key for validating tokens</param>
    public JwtTokenService(RsaSecurityKey privateKey, RsaSecurityKey publicKey)
    {
        _privateKey = privateKey ?? throw new ArgumentNullException(nameof(privateKey));
        _publicKey = publicKey ?? throw new ArgumentNullException(nameof(publicKey));
        _tokenHandler = new JwtSecurityTokenHandler();
    }

    /// <summary>
    /// Creates a new JwtTokenService with auto-generated RSA key pair.
    /// </summary>
    public static JwtTokenService CreateWithGeneratedKeys()
    {
        var rsa = RSA.Create(2048);
        var privateKey = new RsaSecurityKey(rsa);
        var publicKey = new RsaSecurityKey(rsa);
        return new JwtTokenService(privateKey, publicKey);
    }

    /// <summary>
    /// Generates a JWT token for the specified user with 24-hour expiration.
    /// </summary>
    /// <param name="userId">The user's unique identifier</param>
    /// <param name="email">The user's email address</param>
    /// <param name="name">The user's name</param>
    /// <returns>A signed JWT token string</returns>
    public string GenerateToken(Guid userId, string email, string name)
    {
        if (string.IsNullOrEmpty(email))
        {
            throw new ArgumentException("Email cannot be null or empty", nameof(email));
        }

        if (string.IsNullOrEmpty(name))
        {
            throw new ArgumentException("Name cannot be null or empty", nameof(name));
        }

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, email),
            new Claim(JwtRegisteredClaimNames.Name, name),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
        };

        var credentials = new SigningCredentials(_privateKey, SecurityAlgorithms.RsaSha256);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(TokenExpirationHours),
            SigningCredentials = credentials,
            Issuer = "aspnet-learning-platform",
            Audience = "aspnet-learning-platform-users"
        };

        var token = _tokenHandler.CreateToken(tokenDescriptor);
        return _tokenHandler.WriteToken(token);
    }

    /// <summary>
    /// Validates a JWT token and returns the claims principal if valid.
    /// </summary>
    /// <param name="token">The JWT token string to validate</param>
    /// <returns>A tuple containing validation success status, claims principal, and error message</returns>
    public (bool IsValid, ClaimsPrincipal? Principal, string? ErrorMessage) ValidateToken(string token)
    {
        if (string.IsNullOrEmpty(token))
        {
            return (false, null, "Token cannot be null or empty");
        }

        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = _publicKey,
            ValidateIssuer = true,
            ValidIssuer = "aspnet-learning-platform",
            ValidateAudience = true,
            ValidAudience = "aspnet-learning-platform-users",
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero // No tolerance for expiration
        };

        try
        {
            var principal = _tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);
            
            // Verify the token uses RS256 algorithm
            if (validatedToken is JwtSecurityToken jwtToken &&
                jwtToken.Header.Alg.Equals(SecurityAlgorithms.RsaSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                return (true, principal, null);
            }

            return (false, null, "Invalid token algorithm");
        }
        catch (SecurityTokenExpiredException)
        {
            return (false, null, "Token has expired");
        }
        catch (SecurityTokenInvalidSignatureException)
        {
            return (false, null, "Invalid token signature");
        }
        catch (SecurityTokenException ex)
        {
            return (false, null, $"Token validation failed: {ex.Message}");
        }
        catch (Exception ex)
        {
            return (false, null, $"Unexpected error during token validation: {ex.Message}");
        }
    }

    /// <summary>
    /// Extracts the user ID from a validated token.
    /// </summary>
    /// <param name="principal">The claims principal from a validated token</param>
    /// <returns>The user ID if found, otherwise null</returns>
    public Guid? GetUserIdFromPrincipal(ClaimsPrincipal principal)
    {
        if (principal == null)
        {
            return null;
        }

        // Try standard "sub" claim first
        var subClaim = principal.FindFirst(JwtRegisteredClaimNames.Sub);
        if (subClaim != null && Guid.TryParse(subClaim.Value, out var userId))
        {
            return userId;
        }

        // Try ClaimTypes.NameIdentifier as fallback
        var nameIdentifierClaim = principal.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
        if (nameIdentifierClaim != null && Guid.TryParse(nameIdentifierClaim.Value, out var userIdFromNameIdentifier))
        {
            return userIdFromNameIdentifier;
        }

        return null;
    }

    /// <summary>
    /// Gets the expiration time for tokens (24 hours).
    /// </summary>
    public TimeSpan TokenExpiration => TimeSpan.FromHours(TokenExpirationHours);
}
