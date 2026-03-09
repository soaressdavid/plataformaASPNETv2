using FsCheck;
using FsCheck.Xunit;
using Xunit;
using SqlExecutor.Service.Services;

namespace SqlExecutor.Tests;

/// <summary>
/// Property-based tests for SQL destructive operation prevention
/// Property 7: SQL Destructive Operation Prevention
/// Validates: Requirements 2.14
/// </summary>
public class SqlDestructiveOperationPreventionPropertyTests
{
    /// <summary>
    /// Property: DROP DATABASE MUST be blocked
    /// </summary>
    [Theory]
    [InlineData("DROP DATABASE tempdb")]
    [InlineData("drop database mydb")]
    [InlineData("DROP DATABASE IF EXISTS testdb")]
    public void DropDatabase_MustBeBlocked(string query)
    {
        var validator = new SqlQueryValidator();
        var result = validator.Validate(query);

        Assert.False(result.IsValid, "DROP DATABASE should be blocked");
        Assert.Contains("DROP DATABASE", result.ErrorMessage ?? "", StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Property: SHUTDOWN MUST be blocked
    /// </summary>
    [Theory]
    [InlineData("SHUTDOWN")]
    [InlineData("shutdown")]
    [InlineData("SHUTDOWN WITH NOWAIT")]
    public void Shutdown_MustBeBlocked(string query)
    {
        var validator = new SqlQueryValidator();
        var result = validator.Validate(query);

        Assert.False(result.IsValid, "SHUTDOWN should be blocked");
    }

    /// <summary>
    /// Property: xp_cmdshell MUST be blocked
    /// </summary>
    [Theory]
    [InlineData("EXEC xp_cmdshell 'dir'")]
    [InlineData("xp_cmdshell 'whoami'")]
    [InlineData("EXECUTE xp_cmdshell 'ls'")]
    public void XpCmdshell_MustBeBlocked(string query)
    {
        var validator = new SqlQueryValidator();
        var result = validator.Validate(query);

        Assert.False(result.IsValid, "xp_cmdshell should be blocked");
    }

    /// <summary>
    /// Property: Safe operations MUST be allowed
    /// </summary>
    [Theory]
    [InlineData("SELECT * FROM users")]
    [InlineData("INSERT INTO users (name) VALUES ('John')")]
    [InlineData("UPDATE users SET name = 'Jane' WHERE id = 1")]
    [InlineData("DELETE FROM users WHERE id = 1")]
    [InlineData("CREATE TABLE test (id INT, name VARCHAR(50))")]
    [InlineData("ALTER TABLE test ADD email VARCHAR(100)")]
    [InlineData("DROP TABLE test")]
    public void SafeOperations_MustBeAllowed(string query)
    {
        var validator = new SqlQueryValidator();
        var result = validator.Validate(query);

        Assert.True(result.IsValid, $"Safe operation should be allowed: {query}");
    }

    /// <summary>
    /// Property: Validation MUST be case-insensitive
    /// </summary>
    [Theory]
    [InlineData("drop database test")]
    [InlineData("DROP DATABASE test")]
    [InlineData("DrOp DaTaBaSe test")]
    public void Validation_MustBeCaseInsensitive(string query)
    {
        var validator = new SqlQueryValidator();
        var result = validator.Validate(query);

        Assert.False(result.IsValid, "Validation should be case-insensitive");
    }

    /// <summary>
    /// Property: Multiple statements MUST be blocked
    /// </summary>
    [Theory]
    [InlineData("SELECT * FROM users; DROP TABLE users;")]
    [InlineData("INSERT INTO users VALUES (1, 'test'); DELETE FROM users;")]
    public void MultipleStatements_MustBeBlocked(string query)
    {
        var validator = new SqlQueryValidator();
        var result = validator.Validate(query);

        Assert.False(result.IsValid, "Multiple statements should be blocked");
        Assert.Contains("Multiple statements", result.ErrorMessage ?? "");
    }

    /// <summary>
    /// Property: BACKUP and RESTORE MUST be blocked
    /// </summary>
    [Theory]
    [InlineData("BACKUP DATABASE mydb TO DISK = 'backup.bak'")]
    [InlineData("RESTORE DATABASE mydb FROM DISK = 'backup.bak'")]
    public void BackupRestore_MustBeBlocked(string query)
    {
        var validator = new SqlQueryValidator();
        var result = validator.Validate(query);

        Assert.False(result.IsValid, "BACKUP/RESTORE should be blocked");
    }

    /// <summary>
    /// Property: GRANT, REVOKE, DENY MUST be blocked
    /// </summary>
    [Theory]
    [InlineData("GRANT SELECT ON users TO public")]
    [InlineData("REVOKE SELECT ON users FROM public")]
    [InlineData("DENY DELETE ON users TO public")]
    public void PermissionCommands_MustBeBlocked(string query)
    {
        var validator = new SqlQueryValidator();
        var result = validator.Validate(query);

        Assert.False(result.IsValid, "Permission commands should be blocked");
    }

    /// <summary>
    /// Property: Empty or null queries MUST be rejected
    /// </summary>
    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void EmptyQueries_MustBeRejected(string? query)
    {
        var validator = new SqlQueryValidator();
        var result = validator.Validate(query ?? "");

        Assert.False(result.IsValid, "Empty queries should be rejected");
    }

    /// <summary>
    /// Property: Validation MUST be deterministic
    /// </summary>
    [Property(MaxTest = 50)]
    public Property Validation_MustBeDeterministic()
    {
        return Prop.ForAll(
            Arb.From<string>(),
            query =>
            {
                if (string.IsNullOrWhiteSpace(query))
                    return true;

                var validator = new SqlQueryValidator();
                
                var result1 = validator.Validate(query);
                var result2 = validator.Validate(query);

                return (result1.IsValid == result2.IsValid)
                    .Label("Same query must produce consistent validation results");
            });
    }

    /// <summary>
    /// Property: Prohibited operations MUST be identified
    /// </summary>
    [Fact]
    public void ProhibitedOperations_MustBeIdentified()
    {
        var validator = new SqlQueryValidator();
        var query = "DROP DATABASE testdb";
        
        var result = validator.Validate(query);

        Assert.False(result.IsValid);
        Assert.NotNull(result.ProhibitedOperation);
        Assert.Contains("DROP DATABASE", result.ProhibitedOperation, StringComparison.OrdinalIgnoreCase);
    }
}
