namespace Shared.Models;

/// <summary>
/// Standardized error response following RFC 7807 (Problem Details)
/// </summary>
public class ErrorResponse
{
    /// <summary>
    /// A URI reference that identifies the problem type
    /// </summary>
    public string Type { get; set; } = "about:blank";

    /// <summary>
    /// A short, human-readable summary of the problem type
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// The HTTP status code
    /// </summary>
    public int Status { get; set; }

    /// <summary>
    /// A human-readable explanation specific to this occurrence
    /// </summary>
    public string Detail { get; set; } = string.Empty;

    /// <summary>
    /// A URI reference that identifies the specific occurrence
    /// </summary>
    public string? Instance { get; set; }

    /// <summary>
    /// Unique identifier for tracing this error
    /// </summary>
    public string TraceId { get; set; } = string.Empty;

    /// <summary>
    /// Timestamp when the error occurred
    /// </summary>
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Additional error-specific information
    /// </summary>
    public Dictionary<string, object>? Extensions { get; set; }

    /// <summary>
    /// Validation errors (if applicable)
    /// </summary>
    public Dictionary<string, string[]>? Errors { get; set; }
}
