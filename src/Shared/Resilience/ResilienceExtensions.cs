using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;

namespace Shared.Resilience;

/// <summary>
/// Extension methods for adding resilience policies to services
/// </summary>
public static class ResilienceExtensions
{
    /// <summary>
    /// Executes an action with database resilience policies
    /// </summary>
    public static async Task<T> ExecuteWithDatabaseResilienceAsync<T>(
        this ILogger logger,
        Func<Task<T>> action,
        int timeoutSeconds = 30)
    {
        var policy = PollyPolicies.GetCombinedDatabasePolicy(logger, timeoutSeconds);
        return await policy.ExecuteAsync(action);
    }

    /// <summary>
    /// Executes an action with database resilience policies (void return)
    /// </summary>
    public static async Task ExecuteWithDatabaseResilienceAsync(
        this ILogger logger,
        Func<Task> action,
        int timeoutSeconds = 30)
    {
        var policy = PollyPolicies.GetCombinedDatabasePolicy(logger, timeoutSeconds);
        await policy.ExecuteAsync(action);
    }

    /// <summary>
    /// Executes an HTTP request with resilience policies
    /// </summary>
    public static async Task<HttpResponseMessage> ExecuteWithHttpResilienceAsync(
        this ILogger logger,
        Func<Task<HttpResponseMessage>> action,
        int timeoutSeconds = 30)
    {
        var policy = PollyPolicies.GetCombinedHttpPolicy(logger, timeoutSeconds);
        return await policy.ExecuteAsync(action);
    }
}
