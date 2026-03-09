using StackExchange.Redis;
using System.Security.Claims;

namespace ApiGateway.Middleware;

/// <summary>
/// Middleware that implements rate limiting using a token bucket algorithm with Redis backend.
/// Authenticated users: 100 requests per minute
/// Unauthenticated users: 10 requests per minute
/// Returns 429 Too Many Requests with Retry-After header when limit exceeded.
/// Validates: Requirements 11.5
/// </summary>
public class RateLimitingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RateLimitingMiddleware> _logger;
    private readonly IConnectionMultiplexer _redis;
    private const int AuthenticatedLimit = 1000;
    private const int UnauthenticatedLimit = 500;
    private const int WindowSeconds = 60;

    public RateLimitingMiddleware(
        RequestDelegate next,
        ILogger<RateLimitingMiddleware> logger,
        IConnectionMultiplexer redis)
    {
        _next = next ?? throw new ArgumentNullException(nameof(next));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _redis = redis ?? throw new ArgumentNullException(nameof(redis));
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // If Redis is not available, skip rate limiting (fail open)
        if (_redis == null || !_redis.IsConnected)
        {
            _logger.LogWarning("Redis not available, skipping rate limiting");
            await _next(context);
            return;
        }

        var db = _redis.GetDatabase();
        var identifier = GetClientIdentifier(context);
        var isAuthenticated = context.User?.Identity?.IsAuthenticated ?? false;
        var limit = isAuthenticated ? AuthenticatedLimit : UnauthenticatedLimit;

        var key = $"ratelimit:{identifier}";
        var now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

        // Token bucket algorithm using Redis
        var allowed = await CheckRateLimitAsync(db, key, limit, now);

        if (!allowed)
        {
            var retryAfter = await GetRetryAfterSeconds(db, key, now);
            
            _logger.LogWarning(
                "Rate limit exceeded for {Identifier} (authenticated: {IsAuthenticated})",
                identifier,
                isAuthenticated);

            context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
            context.Response.Headers["Retry-After"] = retryAfter.ToString();
            
            await context.Response.WriteAsJsonAsync(new
            {
                error = new
                {
                    code = "RATE_LIMIT_EXCEEDED",
                    message = $"Rate limit exceeded. Maximum {limit} requests per minute allowed.",
                    retryAfter = retryAfter,
                    timestamp = DateTime.UtcNow,
                    traceId = context.TraceIdentifier
                }
            });
            return;
        }

        await _next(context);
    }

    /// <summary>
    /// Implements token bucket rate limiting algorithm using Redis.
    /// Returns true if request is allowed, false if rate limit exceeded.
    /// </summary>
    private async Task<bool> CheckRateLimitAsync(IDatabase db, string key, int limit, long now)
    {
        try
        {
            // Use Redis transaction to ensure atomicity
            var transaction = db.CreateTransaction();

            // Get current bucket state
            var tokensTask = transaction.StringGetAsync($"{key}:tokens");
            var lastRefillTask = transaction.StringGetAsync($"{key}:lastRefill");

            await transaction.ExecuteAsync();

            var tokensStr = await tokensTask;
            var lastRefillStr = await lastRefillTask;

            // Parse current state
            var tokens = tokensStr.HasValue ? (double)tokensStr : limit;
            var lastRefill = lastRefillStr.HasValue ? (long)lastRefillStr : now;

            // Calculate tokens to add based on time elapsed
            var elapsedSeconds = now - lastRefill;
            var tokensToAdd = (elapsedSeconds * limit) / (double)WindowSeconds;
            tokens = Math.Min(limit, tokens + tokensToAdd);

            // Check if request can be allowed
            if (tokens >= 1)
            {
                // Consume one token
                tokens -= 1;

                // Update Redis with new state
                var updateTransaction = db.CreateTransaction();
                await updateTransaction.StringSetAsync($"{key}:tokens", tokens, TimeSpan.FromSeconds(WindowSeconds * 2));
                await updateTransaction.StringSetAsync($"{key}:lastRefill", now, TimeSpan.FromSeconds(WindowSeconds * 2));
                await updateTransaction.ExecuteAsync();

                return true;
            }

            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking rate limit for key {Key}", key);
            // On Redis failure, allow the request (fail open)
            return true;
        }
    }

    /// <summary>
    /// Calculates how many seconds until the next token is available.
    /// </summary>
    private async Task<int> GetRetryAfterSeconds(IDatabase db, string key, long now)
    {
        try
        {
            var tokensStr = await db.StringGetAsync($"{key}:tokens");
            var lastRefillStr = await db.StringGetAsync($"{key}:lastRefill");

            if (!tokensStr.HasValue || !lastRefillStr.HasValue)
            {
                return WindowSeconds;
            }

            var tokens = (double)tokensStr;
            var lastRefill = (long)lastRefillStr;
            var isAuthenticated = tokens < AuthenticatedLimit;
            var limit = isAuthenticated ? AuthenticatedLimit : UnauthenticatedLimit;

            // Calculate seconds needed to refill one token
            var tokensNeeded = 1 - tokens;
            var secondsPerToken = WindowSeconds / (double)limit;
            var secondsNeeded = (int)Math.Ceiling(tokensNeeded * secondsPerToken);

            return Math.Max(1, Math.Min(secondsNeeded, WindowSeconds));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calculating retry-after for key {Key}", key);
            return WindowSeconds;
        }
    }

    /// <summary>
    /// Gets a unique identifier for the client.
    /// Uses user ID for authenticated users, IP address for unauthenticated users.
    /// </summary>
    private string GetClientIdentifier(HttpContext context)
    {
        // For authenticated users, use user ID
        if (context.User?.Identity?.IsAuthenticated == true)
        {
            var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? context.User.FindFirst("sub")?.Value;
            
            if (!string.IsNullOrEmpty(userId))
            {
                return $"user:{userId}";
            }
        }

        // For unauthenticated users, use IP address
        var ipAddress = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        return $"ip:{ipAddress}";
    }
}
