namespace Shared.Messaging;

/// <summary>
/// Configuration options for RabbitMQ connection
/// </summary>
public class RabbitMQOptions
{
    public const string SectionName = "RabbitMQ";

    /// <summary>
    /// RabbitMQ host name or IP address
    /// </summary>
    public string HostName { get; set; } = "localhost";

    /// <summary>
    /// RabbitMQ port (default: 5672)
    /// </summary>
    public int Port { get; set; } = 5672;

    /// <summary>
    /// RabbitMQ username
    /// </summary>
    public string UserName { get; set; } = "guest";

    /// <summary>
    /// RabbitMQ password
    /// </summary>
    public string Password { get; set; } = "guest";

    /// <summary>
    /// Virtual host (default: /)
    /// </summary>
    public string VirtualHost { get; set; } = "/";

    /// <summary>
    /// Maximum number of retry attempts for connection failures
    /// </summary>
    public int MaxRetryAttempts { get; set; } = 5;

    /// <summary>
    /// Initial retry delay in milliseconds
    /// </summary>
    public int RetryDelayMs { get; set; } = 1000;

    /// <summary>
    /// Connection timeout in seconds
    /// </summary>
    public int ConnectionTimeoutSeconds { get; set; } = 30;

    /// <summary>
    /// Requested heartbeat interval in seconds (0 to disable)
    /// </summary>
    public ushort RequestedHeartbeat { get; set; } = 60;
}
