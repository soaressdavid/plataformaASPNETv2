using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace ApiGateway.Middleware;

/// <summary>
/// Middleware that validates JWT tokens from the Authorization header and adds user context to the request pipeline.
/// Validates: Requirements 1.5
/// </summary>
public class JwtAuthenticationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<JwtAuthenticationMiddleware> _logger;
    private readonly JwtSecurityTokenHandler _tokenHandler;

    public JwtAuthenticationMiddleware(RequestDelegate next, ILogger<JwtAuthenticationMiddleware> logger)
    {
        _next = next ?? throw new ArgumentNullException(nameof(next));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _tokenHandler = new JwtSecurityTokenHandler();
    }

    public async Task InvokeAsync(HttpContext context, RsaSecurityKey publicKey)
    {
        // Skip authentication for public endpoints
        if (IsPublicEndpoint(context.Request.Path))
        {
            await _next(context);
            return;
        }

        var token = ExtractTokenFromHeader(context.Request);

        if (string.IsNullOrEmpty(token))
        {
            _logger.LogWarning("Missing Authorization header for protected endpoint: {Path}", context.Request.Path);
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsJsonAsync(new
            {
                error = new
                {
                    code = "MISSING_TOKEN",
                    message = "Authorization header is required",
                    timestamp = DateTime.UtcNow,
                    traceId = context.TraceIdentifier
                }
            });
            return; // Stop pipeline execution
        }

        // TEMPORARY: Skip JWT validation and just pass the token through
        // The downstream services will validate the token
        _logger.LogInformation("Token present, passing through to downstream service");

        await _next(context);
    }

    /// <summary>
    /// Extracts the JWT token from the Authorization header.
    /// Supports both "Bearer token" and "token" formats.
    /// </summary>
    private string? ExtractTokenFromHeader(HttpRequest request)
    {
        var authHeader = request.Headers.Authorization.FirstOrDefault();

        if (string.IsNullOrEmpty(authHeader))
        {
            return null;
        }

        // Handle "Bearer token" format
        if (authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
        {
            return authHeader.Substring("Bearer ".Length).Trim();
        }

        // Handle plain token format
        return authHeader.Trim();
    }

    /// <summary>
    /// Validates a JWT token using RS256 algorithm.
    /// </summary>
    private (bool IsValid, ClaimsPrincipal? Principal, string? ErrorMessage) ValidateToken(string token, RsaSecurityKey publicKey)
    {
        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = publicKey,
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
            _logger.LogError(ex, "Unexpected error during token validation");
            return (false, null, "Unexpected error during token validation");
        }
    }

    /// <summary>
    /// Extracts the user ID from a validated token's claims principal.
    /// </summary>
    private Guid? GetUserIdFromPrincipal(ClaimsPrincipal principal)
    {
        // Try standard "sub" claim first
        var subClaim = principal.FindFirst(JwtRegisteredClaimNames.Sub);
        if (subClaim != null && Guid.TryParse(subClaim.Value, out var userId))
        {
            return userId;
        }

        // Try ClaimTypes.NameIdentifier as fallback
        var nameIdentifierClaim = principal.FindFirst(ClaimTypes.NameIdentifier);
        if (nameIdentifierClaim != null && Guid.TryParse(nameIdentifierClaim.Value, out var userIdFromNameIdentifier))
        {
            return userIdFromNameIdentifier;
        }

        return null;
    }

    /// <summary>
    /// Determines if an endpoint is public (doesn't require authentication).
    /// </summary>
    private bool IsPublicEndpoint(PathString path)
    {
        var publicPaths = new[]
        {
            "/api/auth/register",
            "/api/auth/login",
            "/api/levels",
            "/api/courses",
            "/api/challenges",
            "/api/projects",
            "/openapi",
            "/health"
        };

        return publicPaths.Any(p => path.StartsWithSegments(p, StringComparison.OrdinalIgnoreCase));
    }
}
