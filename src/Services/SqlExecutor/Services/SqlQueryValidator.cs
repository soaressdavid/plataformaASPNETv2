using System.Text.RegularExpressions;

namespace SqlExecutor.Service.Services;

/// <summary>
/// Validates SQL queries to prevent destructive operations
/// Validates: Requirements 2.14
/// </summary>
public class SqlQueryValidator
{
    private static readonly string[] AllowedCommands = new[]
    {
        "SELECT", "INSERT", "UPDATE", "DELETE", 
        "CREATE TABLE", "ALTER TABLE", "DROP TABLE",
        "CREATE INDEX", "DROP INDEX"
    };

    private static readonly string[] ProhibitedCommands = new[]
    {
        "DROP DATABASE", "CREATE DATABASE", "ALTER DATABASE",
        "SHUTDOWN", "xp_cmdshell", "sp_configure",
        "EXEC", "EXECUTE", "BACKUP", "RESTORE",
        "GRANT", "REVOKE", "DENY"
    };

    private static readonly Regex ProhibitedPattern = new(
        @"\b(DROP\s+DATABASE|CREATE\s+DATABASE|ALTER\s+DATABASE|SHUTDOWN|xp_cmdshell|sp_configure|BACKUP|RESTORE|GRANT|REVOKE|DENY)\b",
        RegexOptions.IgnoreCase | RegexOptions.Compiled
    );

    /// <summary>
    /// Validates a SQL query and returns validation result
    /// </summary>
    public ValidationResult Validate(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            return new ValidationResult
            {
                IsValid = false,
                ErrorMessage = "Query cannot be empty"
            };
        }

        // Check for prohibited commands
        var prohibitedMatch = ProhibitedPattern.Match(query);
        if (prohibitedMatch.Success)
        {
            return new ValidationResult
            {
                IsValid = false,
                ErrorMessage = $"Prohibited operation detected: {prohibitedMatch.Value}",
                ProhibitedOperation = prohibitedMatch.Value
            };
        }

        // Check for multiple statements (prevent SQL injection)
        var statementCount = query.Split(';', StringSplitOptions.RemoveEmptyEntries).Length;
        if (statementCount > 1)
        {
            return new ValidationResult
            {
                IsValid = false,
                ErrorMessage = "Multiple statements are not allowed. Execute one statement at a time."
            };
        }

        return new ValidationResult { IsValid = true };
    }

    /// <summary>
    /// Checks if a command is in the allowed list
    /// </summary>
    public bool IsAllowedCommand(string command)
    {
        var upperCommand = command.Trim().ToUpperInvariant();
        return AllowedCommands.Any(allowed => upperCommand.StartsWith(allowed));
    }
}

/// <summary>
/// Result of SQL query validation
/// </summary>
public class ValidationResult
{
    public bool IsValid { get; set; }
    public string? ErrorMessage { get; set; }
    public string? ProhibitedOperation { get; set; }
}
