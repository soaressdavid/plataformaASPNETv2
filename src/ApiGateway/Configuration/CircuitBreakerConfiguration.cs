namespace ApiGateway.Configuration;

/// <summary>
/// Configuration for circuit breaker policy
/// </summary>
public class CircuitBreakerConfiguration
{
    /// <summary>
    /// Number of consecutive failures before opening the circuit
    /// </summary>
    public int FailureThreshold { get; set; } = 5;

    /// <summary>
    /// Duration in seconds to wait before attempting to close the circuit (half-open state)
    /// </summary>
    public int DurationOfBreakInSeconds { get; set; } = 30;
}
