using System.CommandLine;
using Serilog;

namespace MigrationTool;

class Program
{
    static async Task<int> Main(string[] args)
    {
        // Configure logging
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .WriteTo.Console()
            .WriteTo.File("migration-log.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();

        try
        {
            var rootCommand = new RootCommand("PostgreSQL to SQL Server Migration Tool");

            var sourceOption = new Option<string>(
                name: "--source",
                description: "PostgreSQL connection string")
            { IsRequired = true };

            var targetOption = new Option<string>(
                name: "--target",
                description: "SQL Server connection string")
            { IsRequired = true };

            var batchSizeOption = new Option<int>(
                name: "--batch-size",
                description: "Batch size for bulk insert operations",
                getDefaultValue: () => 1000);

            var parallelOption = new Option<int>(
                name: "--parallel",
                description: "Number of parallel table migrations",
                getDefaultValue: () => 4);

            var excludeTablesOption = new Option<string[]>(
                name: "--exclude-tables",
                description: "Tables to exclude from migration",
                getDefaultValue: () => Array.Empty<string>());

            rootCommand.AddOption(sourceOption);
            rootCommand.AddOption(targetOption);
            rootCommand.AddOption(batchSizeOption);
            rootCommand.AddOption(parallelOption);
            rootCommand.AddOption(excludeTablesOption);

            rootCommand.SetHandler(async (source, target, batchSize, parallel, excludeTables) =>
            {
                var migrator = new DatabaseMigrator(source, target, batchSize, parallel, excludeTables);
                await migrator.MigrateAsync();
            }, sourceOption, targetOption, batchSizeOption, parallelOption, excludeTablesOption);

            return await rootCommand.InvokeAsync(args);
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Migration failed with unhandled exception");
            return 1;
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}
