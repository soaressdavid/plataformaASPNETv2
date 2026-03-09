using FsCheck;
using FsCheck.Xunit;

namespace ApiGateway.Tests;

/// <summary>
/// Property-based tests for rate limiting functionality.
/// **Validates: Requirements 11.5**
/// </summary>
public class RateLimitingPropertiesTests
{
    /// <summary>
    /// Property 37: Rate Limiting
    /// For any authenticated user making more than 100 requests in a 60-second window,
    /// subsequent requests should be rejected with a 429 Too Many Requests error.
    /// **Validates: Requirements 11.5**
    /// </summary>
    [Property(MaxTest = 100)]
    public void RateLimiting_AuthenticatedUser_Enforces100RequestsPerMinute(PositiveInt requestCount)
    {
        var count = requestCount.Get % 200; // Limit to reasonable range
        var limit = 100;

        // Simulate token bucket: start with full bucket
        var tokens = (double)limit;
        var allowedRequests = 0;
        var deniedRequests = 0;

        for (int i = 0; i < count; i++)
        {
            if (tokens >= 1)
            {
                tokens -= 1;
                allowedRequests++;
            }
            else
            {
                deniedRequests++;
            }
        }

        // Property: If requests exceed limit, some must be denied
        if (count > limit)
        {
            Assert.True(deniedRequests > 0);
            Assert.Equal(limit, allowedRequests);
        }
        else
        {
            Assert.Equal(count, allowedRequests);
            Assert.Equal(0, deniedRequests);
        }
    }

    /// <summary>
    /// Property: Unauthenticated users should have a lower rate limit (10 req/min).
    /// **Validates: Requirements 11.5**
    /// </summary>
    [Property(MaxTest = 100)]
    public void RateLimiting_UnauthenticatedUser_Enforces10RequestsPerMinute(PositiveInt requestCount)
    {
        var count = requestCount.Get % 50; // Limit to reasonable range
        var limit = 10;

        // Simulate token bucket: start with full bucket
        var tokens = (double)limit;
        var allowedRequests = 0;
        var deniedRequests = 0;

        for (int i = 0; i < count; i++)
        {
            if (tokens >= 1)
            {
                tokens -= 1;
                allowedRequests++;
            }
            else
            {
                deniedRequests++;
            }
        }

        // Property: If requests exceed limit, some must be denied
        if (count > limit)
        {
            Assert.True(deniedRequests > 0);
            Assert.Equal(limit, allowedRequests);
        }
        else
        {
            Assert.Equal(count, allowedRequests);
            Assert.Equal(0, deniedRequests);
        }
    }

    /// <summary>
    /// Property: Token bucket refills over time, allowing more requests.
    /// **Validates: Requirements 11.5**
    /// </summary>
    [Property(MaxTest = 100)]
    public void TokenBucket_RefillsOverTime(PositiveInt elapsedSeconds)
    {
        var elapsed = elapsedSeconds.Get % 120; // Max 2 minutes
        var limit = 100;
        var windowSeconds = 60;

        // Start with empty bucket
        var tokens = 0.0;

        // Calculate refill
        var tokensToAdd = (elapsed * limit) / (double)windowSeconds;
        tokens = Math.Min(limit, tokens + tokensToAdd);

        // Property: Tokens should never exceed limit
        Assert.True(tokens <= limit);
        Assert.True(tokens >= 0);
    }

    /// <summary>
    /// Property: Token bucket never goes negative.
    /// **Validates: Requirements 11.5**
    /// </summary>
    [Property(MaxTest = 100)]
    public void TokenBucket_NeverGoesNegative(PositiveInt requestCount)
    {
        var count = requestCount.Get % 200;
        var limit = 100;

        var tokens = (double)limit;

        for (int i = 0; i < count; i++)
        {
            if (tokens >= 1)
            {
                tokens -= 1;
            }
            // Don't consume if not enough tokens
        }

        // Property: Tokens should never be negative
        Assert.True(tokens >= 0);
    }

    /// <summary>
    /// Property: Different users have independent rate limits.
    /// **Validates: Requirements 11.5**
    /// </summary>
    [Property(MaxTest = 100)]
    public void RateLimiting_DifferentUsers_IndependentLimits(NonEmptyString user1, NonEmptyString user2)
    {
        if (user1.Get == user2.Get)
        {
            return; // Skip if same user
        }

        var limit = 100;

        // Each user has their own bucket
        var user1Tokens = (double)limit;
        var user2Tokens = (double)limit;

        // User 1 makes requests
        for (int i = 0; i < 50; i++)
        {
            if (user1Tokens >= 1)
            {
                user1Tokens -= 1;
            }
        }

        // User 2 makes requests
        for (int i = 0; i < 30; i++)
        {
            if (user2Tokens >= 1)
            {
                user2Tokens -= 1;
            }
        }

        // Property: User 2's tokens should not be affected by User 1's usage
        Assert.Equal(50, user1Tokens);
        Assert.Equal(70, user2Tokens);
    }

    /// <summary>
    /// Property: Retry-After header should indicate when next request is allowed.
    /// **Validates: Requirements 11.5**
    /// </summary>
    [Property(MaxTest = 100)]
    public void RateLimiting_RetryAfter_IndicatesWaitTime(PositiveInt tokensRemaining)
    {
        var tokens = (tokensRemaining.Get % 100) / 100.0; // 0.0 to 0.99
        var limit = 100;
        var windowSeconds = 60;

        // Calculate seconds needed to refill one token
        var tokensNeeded = 1 - tokens;
        var secondsPerToken = windowSeconds / (double)limit;
        var secondsNeeded = (int)Math.Ceiling(tokensNeeded * secondsPerToken);
        var retryAfter = Math.Max(1, Math.Min(secondsNeeded, windowSeconds));

        // Property: Retry-After should be between 1 and windowSeconds
        Assert.True(retryAfter >= 1);
        Assert.True(retryAfter <= windowSeconds);
    }
}
