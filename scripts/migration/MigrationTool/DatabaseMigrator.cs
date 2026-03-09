using Npgsql;
using Microsoft.Data.SqlClient;
using Dapper;
using Serilog;
using System.Data;
using System.Diagnostics;

namespace MigrationTool;

public class DatabaseMigrator
{
    private readonly string _sourceConnectionString;
    private readonly string _targetConnectionString;
    private readonly int _batchSize;
    private readonly int _parallelTables;
    private readonly string[] _excludeTables;

    public DatabaseMigrator(
        string sourceConnectionString,
        string targetConnectionString,
        int batchSize,
        int parallelTables,
        string[] excludeTables)
    {
        _sourceConnectionString = sourceConnectionString;
        _targetConnectionString = targetConnectionString;
        _batchSize = batchSize;
        _parallelTables = parallelTables;
        _excludeTables = excludeTables;
    }

    public async Task MigrateAsync()
    {
        Log.Information("Starting database migration");
        Log.Information("Source: PostgreSQL");
        Log.Information("Target: SQL Server");
        Log.Information("Batch size: {BatchSize}", _batchSize);
        Log.Information("Parallel tables: {ParallelTables}", _parallelTables);

        var totalStopwatch = Stopwatch.StartNew();

        try
        {
            // Step 1: Test connections
            await TestConnectionsAsync();

            // Step 2: Get table list
            var tables = await GetTableListAsync();
            Log.Information("Found {TableCount} tables to migrate", tables.Count);

            // Step 3: Disable constraints
            await DisableConstraintsAsync();

            // Step 4: Migrate tables in parallel
            await MigrateTablesAsync(tables);

            // Step 5: Enable constraints
            await EnableConstraintsAsync();

            // Step 6: Verify migration
            await VerifyMigrationAsync(tables);

            totalStopwatch.Stop();
            Log.Information("Migration completed successfully in {Duration}", totalStopwatch.Elapsed);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Migration failed");
            throw;
        }
    }

    private async Task TestConnectionsAsync()
    {
        Log.Information("Testing database connections...");

        // Test PostgreSQL
        await using (var pgConn = new NpgsqlConnection(_sourceConnectionString))
        {
            await pgConn.OpenAsync();
            var version = await pgConn.QuerySingleAsync<string>("SELECT version()");
            Log.Information("PostgreSQL connected: {Version}", version.Split('\n')[0]);
        }

        // Test SQL Server
        await using (var sqlConn = new SqlConnection(_targetConnectionString))
        {
            await sqlConn.OpenAsync();
            var version = await sqlConn.QuerySingleAsync<string>("SELECT @@VERSION");
            Log.Information("SQL Server connected: {Version}", version.Split('\n')[0]);
        }
    }

    private async Task<List<string>> GetTableListAsync()
    {
        Log.Information("Getting table list from PostgreSQL...");

        await using var conn = new NpgsqlConnection(_sourceConnectionString);
        await conn.OpenAsync();

        var tables = (await conn.QueryAsync<string>(@"
            SELECT tablename 
            FROM pg_tables 
            WHERE schemaname = 'public' 
            ORDER BY tablename
        ")).ToList();

        // Filter excluded tables
        tables = tables.Where(t => !_excludeTables.Contains(t)).ToList();

        return tables;
    }

    private async Task DisableConstraintsAsync()
    {
        Log.Information("Disabling constraints and triggers...");

        await using var conn = new SqlConnection(_targetConnectionString);
        await conn.OpenAsync();

        await conn.ExecuteAsync("EXEC sp_MSforeachtable 'ALTER TABLE ? NOCHECK CONSTRAINT ALL'");
        await conn.ExecuteAsync("EXEC sp_MSforeachtable 'ALTER TABLE ? DISABLE TRIGGER ALL'");

        Log.Information("Constraints and triggers disabled");
    }

    private async Task EnableConstraintsAsync()
    {
        Log.Information("Enabling constraints and triggers...");

        await using var conn = new SqlConnection(_targetConnectionString);
        await conn.OpenAsync();

        await conn.ExecuteAsync("EXEC sp_MSforeachtable 'ALTER TABLE ? CHECK CONSTRAINT ALL'");
        await conn.ExecuteAsync("EXEC sp_MSforeachtable 'ALTER TABLE ? ENABLE TRIGGER ALL'");
        await conn.ExecuteAsync("EXEC sp_MSforeachtable 'DBCC CHECKIDENT(''?'', RESEED)'");

        Log.Information("Constraints and triggers enabled");
    }

    private async Task MigrateTablesAsync(List<string> tables)
    {
        Log.Information("Migrating {TableCount} tables...", tables.Count);

        var semaphore = new SemaphoreSlim(_parallelTables);
        var tasks = new List<Task>();

        foreach (var table in tables)
        {
            await semaphore.WaitAsync();

            tasks.Add(Task.Run(async () =>
            {
                try
                {
                    await MigrateTableAsync(table);
                }
                finally
                {
                    semaphore.Release();
                }
            }));
        }

        await Task.WhenAll(tasks);
    }

    private async Task MigrateTableAsync(string tableName)
    {
        var stopwatch = Stopwatch.StartNew();
        Log.Information("Migrating table: {TableName}", tableName);

        try
        {
            // Get row count from PostgreSQL
            await using var pgConn = new NpgsqlConnection(_sourceConnectionString);
            await pgConn.OpenAsync();

            var totalRows = await pgConn.QuerySingleAsync<long>($"SELECT COUNT(*) FROM {tableName}");
            Log.Information("{TableName}: {TotalRows} rows to migrate", tableName, totalRows);

            if (totalRows == 0)
            {
                Log.Information("{TableName}: No data to migrate", tableName);
                return;
            }

            // Get column information
            var columns = await GetTableColumnsAsync(pgConn, tableName);
            var columnList = string.Join(", ", columns.Select(c => $"\"{c}\""));

            // Migrate data in batches
            long migratedRows = 0;
            long offset = 0;

            await using var sqlConn = new SqlConnection(_targetConnectionString);
            await sqlConn.OpenAsync();

            // Check if table has identity column
            var hasIdentity = await HasIdentityColumnAsync(sqlConn, tableName);
            if (hasIdentity)
            {
                await sqlConn.ExecuteAsync($"SET IDENTITY_INSERT [{tableName}] ON");
            }

            while (offset < totalRows)
            {
                // Read batch from PostgreSQL
                var query = $"SELECT {columnList} FROM {tableName} ORDER BY 1 LIMIT {_batchSize} OFFSET {offset}";
                var rows = await pgConn.QueryAsync(query);

                // Insert batch into SQL Server
                foreach (var row in rows)
                {
                    var dict = (IDictionary<string, object>)row;
                    var insertColumns = string.Join(", ", dict.Keys.Select(k => $"[{k}]"));
                    var insertValues = string.Join(", ", dict.Keys.Select(k => $"@{k}"));
                    var insertQuery = $"INSERT INTO [{tableName}] ({insertColumns}) VALUES ({insertValues})";

                    await sqlConn.ExecuteAsync(insertQuery, dict);
                    migratedRows++;
                }

                offset += _batchSize;

                var progress = (double)migratedRows / totalRows * 100;
                Log.Information("{TableName}: {Progress:F1}% ({MigratedRows}/{TotalRows})",
                    tableName, progress, migratedRows, totalRows);
            }

            if (hasIdentity)
            {
                await sqlConn.ExecuteAsync($"SET IDENTITY_INSERT [{tableName}] OFF");
            }

            stopwatch.Stop();
            Log.Information("{TableName}: Migration completed in {Duration} ({RowsPerSecond:F0} rows/sec)",
                tableName, stopwatch.Elapsed, migratedRows / stopwatch.Elapsed.TotalSeconds);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Failed to migrate table: {TableName}", tableName);
            throw;
        }
    }

    private async Task<List<string>> GetTableColumnsAsync(NpgsqlConnection conn, string tableName)
    {
        var columns = await conn.QueryAsync<string>(@"
            SELECT column_name 
            FROM information_schema.columns 
            WHERE table_schema = 'public' AND table_name = @tableName 
            ORDER BY ordinal_position",
            new { tableName });

        return columns.ToList();
    }

    private async Task<bool> HasIdentityColumnAsync(SqlConnection conn, string tableName)
    {
        var result = await conn.QuerySingleOrDefaultAsync<int>(@"
            SELECT COUNT(*) 
            FROM sys.columns 
            WHERE object_id = OBJECT_ID(@tableName) AND is_identity = 1",
            new { tableName });

        return result > 0;
    }

    private async Task VerifyMigrationAsync(List<string> tables)
    {
        Log.Information("Verifying migration...");

        await using var pgConn = new NpgsqlConnection(_sourceConnectionString);
        await using var sqlConn = new SqlConnection(_targetConnectionString);

        await pgConn.OpenAsync();
        await sqlConn.OpenAsync();

        var allMatch = true;

        foreach (var table in tables)
        {
            var pgCount = await pgConn.QuerySingleAsync<long>($"SELECT COUNT(*) FROM {table}");
            var sqlCount = await sqlConn.QuerySingleAsync<long>($"SELECT COUNT(*) FROM [{table}]");

            if (pgCount == sqlCount)
            {
                Log.Information("✓ {TableName}: {Count} rows (match)", table, pgCount);
            }
            else
            {
                Log.Warning("✗ {TableName}: PostgreSQL={PgCount}, SQL Server={SqlCount} (mismatch)",
                    table, pgCount, sqlCount);
                allMatch = false;
            }
        }

        if (allMatch)
        {
            Log.Information("✓ All tables verified successfully!");
        }
        else
        {
            Log.Warning("✗ Some tables have mismatched row counts");
        }
    }
}
