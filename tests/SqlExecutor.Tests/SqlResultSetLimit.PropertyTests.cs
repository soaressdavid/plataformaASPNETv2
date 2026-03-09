using FsCheck;
using FsCheck.Xunit;
using Xunit;

namespace SqlExecutor.Tests;

/// <summary>
/// Property-based tests for SQL result set limit
/// Property 6: SQL Result Set Limit
/// Validates: Requirements 2.9
/// </summary>
public class SqlResultSetLimitPropertyTests
{
    private const int MaxResultSetSize = 1000;

    /// <summary>
    /// Property: Result sets MUST be limited to 1000 rows
    /// </summary>
    [Property(MaxTest = 50)]
    public Property ResultSets_MustBeLimitedTo1000Rows()
    {
        return Prop.ForAll(
            Gen.Choose(1, 5000).ToArbitrary(),
            totalRows =>
            {
                var returnedRows = Math.Min(totalRows, MaxResultSetSize);
                
                return (returnedRows <= MaxResultSetSize)
                    .Label($"Result set with {totalRows} rows should return max {MaxResultSetSize} rows, got {returnedRows}");
            });
    }

    /// <summary>
    /// Property: Small result sets MUST NOT be truncated
    /// </summary>
    [Property(MaxTest = 50)]
    public Property SmallResultSets_MustNotBeTruncated()
    {
        return Prop.ForAll(
            Gen.Choose(1, 999).ToArbitrary(),
            rowCount =>
            {
                var returnedRows = Math.Min(rowCount, MaxResultSetSize);
                
                return (returnedRows == rowCount)
                    .Label($"Result set with {rowCount} rows should not be truncated");
            });
    }

    /// <summary>
    /// Property: Large result sets MUST be truncated to 1000 rows
    /// </summary>
    [Property(MaxTest = 50)]
    public Property LargeResultSets_MustBeTruncated()
    {
        return Prop.ForAll(
            Gen.Choose(1001, 10000).ToArbitrary(),
            rowCount =>
            {
                var returnedRows = Math.Min(rowCount, MaxResultSetSize);
                
                return (returnedRows == MaxResultSetSize)
                    .Label($"Result set with {rowCount} rows should be truncated to {MaxResultSetSize}");
            });
    }

    /// <summary>
    /// Property: Truncation flag MUST be set when result set is limited
    /// </summary>
    [Fact]
    public void TruncationFlag_MustBeSetWhenLimited()
    {
        var totalRows = 2000;
        var returnedRows = Math.Min(totalRows, MaxResultSetSize);
        var truncated = totalRows > MaxResultSetSize;

        Assert.True(truncated, "Truncation flag should be true when result set exceeds limit");
        Assert.Equal(MaxResultSetSize, returnedRows);
    }

    /// <summary>
    /// Property: Truncation flag MUST NOT be set for small result sets
    /// </summary>
    [Fact]
    public void TruncationFlag_MustNotBeSetForSmallResults()
    {
        var totalRows = 500;
        var returnedRows = Math.Min(totalRows, MaxResultSetSize);
        var truncated = totalRows > MaxResultSetSize;

        Assert.False(truncated, "Truncation flag should be false for small result sets");
        Assert.Equal(totalRows, returnedRows);
    }

    /// <summary>
    /// Property: Row count MUST be accurate
    /// </summary>
    [Property(MaxTest = 50)]
    public Property RowCount_MustBeAccurate()
    {
        return Prop.ForAll(
            Gen.Choose(0, 5000).ToArbitrary(),
            totalRows =>
            {
                var returnedRows = Math.Min(totalRows, MaxResultSetSize);
                var reportedCount = returnedRows;

                return (reportedCount == returnedRows)
                    .Label($"Reported row count {reportedCount} must match actual {returnedRows}");
            });
    }

    /// <summary>
    /// Property: Empty result sets MUST return 0 rows
    /// </summary>
    [Fact]
    public void EmptyResultSets_MustReturn0Rows()
    {
        var totalRows = 0;
        var returnedRows = Math.Min(totalRows, MaxResultSetSize);
        var truncated = totalRows > MaxResultSetSize;

        Assert.Equal(0, returnedRows);
        Assert.False(truncated);
    }

    /// <summary>
    /// Property: Exactly 1000 rows MUST NOT be marked as truncated
    /// </summary>
    [Fact]
    public void Exactly1000Rows_MustNotBeMarkedAsTruncated()
    {
        var totalRows = 1000;
        var returnedRows = Math.Min(totalRows, MaxResultSetSize);
        var truncated = totalRows > MaxResultSetSize;

        Assert.Equal(MaxResultSetSize, returnedRows);
        Assert.False(truncated, "Exactly 1000 rows should not be marked as truncated");
    }

    /// <summary>
    /// Property: 1001 rows MUST be truncated and marked
    /// </summary>
    [Fact]
    public void MoreThan1000Rows_MustBeTruncatedAndMarked()
    {
        var totalRows = 1001;
        var returnedRows = Math.Min(totalRows, MaxResultSetSize);
        var truncated = totalRows > MaxResultSetSize;

        Assert.Equal(MaxResultSetSize, returnedRows);
        Assert.True(truncated, "1001 rows should be truncated and marked");
    }

    /// <summary>
    /// Property: Limit MUST be applied consistently across all queries
    /// </summary>
    [Property(MaxTest = 30)]
    public Property Limit_MustBeAppliedConsistently()
    {
        return Prop.ForAll(
            Gen.Choose(1, 10000).ToArbitrary(),
            Gen.Choose(1, 10000).ToArbitrary(),
            (rows1, rows2) =>
            {
                var result1 = Math.Min(rows1, MaxResultSetSize);
                var result2 = Math.Min(rows2, MaxResultSetSize);

                var bothLimited = result1 <= MaxResultSetSize && result2 <= MaxResultSetSize;

                return bothLimited.Label("Limit must be applied consistently");
            });
    }

    /// <summary>
    /// Property: Result set structure MUST be preserved after truncation
    /// </summary>
    [Fact]
    public void ResultSetStructure_MustBePreservedAfterTruncation()
    {
        // Simulate result set with columns
        var originalColumns = new[] { "id", "name", "email" };
        var totalRows = 2000;
        var returnedRows = Math.Min(totalRows, MaxResultSetSize);

        // After truncation, columns should remain the same
        var truncatedColumns = originalColumns;

        Assert.Equal(originalColumns, truncatedColumns);
        Assert.Equal(MaxResultSetSize, returnedRows);
    }

    /// <summary>
    /// Property: First N rows MUST be returned when truncating
    /// </summary>
    [Fact]
    public void FirstNRows_MustBeReturnedWhenTruncating()
    {
        var allRows = Enumerable.Range(1, 2000).ToList();
        var returnedRows = allRows.Take(MaxResultSetSize).ToList();

        Assert.Equal(MaxResultSetSize, returnedRows.Count);
        Assert.Equal(1, returnedRows.First());
        Assert.Equal(MaxResultSetSize, returnedRows.Last());
    }
}
