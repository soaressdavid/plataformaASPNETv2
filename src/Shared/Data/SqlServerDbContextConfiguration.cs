using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Shared.Data;

public static class SqlServerDbContextConfiguration
{
    public static IServiceCollection AddSqlServerDbContext(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(connectionString, sqlOptions =>
            {
                // Enable retry on failure for transient errors
                sqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(30),
                    errorNumbersToAdd: null);

                // Command timeout
                sqlOptions.CommandTimeout(30);

                // Enable query splitting for better performance with collections
                sqlOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
            });

            // Enable sensitive data logging in development
            if (configuration.GetValue<bool>("Logging:EnableSensitiveDataLogging"))
            {
                options.EnableSensitiveDataLogging();
            }

            // Enable detailed errors in development
            if (configuration.GetValue<bool>("Logging:EnableDetailedErrors"))
            {
                options.EnableDetailedErrors();
            }
        });

        return services;
    }

    public static IServiceCollection AddSqlServerDbContextWithReadReplicas(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var primaryConnectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        var readReplicaConnectionStrings = configuration.GetSection("ConnectionStrings:ReadReplicas")
            .Get<string[]>() ?? Array.Empty<string>();

        // Primary connection for writes
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(primaryConnectionString, sqlOptions =>
            {
                sqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(30),
                    errorNumbersToAdd: null);

                sqlOptions.CommandTimeout(30);
                sqlOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
            });

            if (configuration.GetValue<bool>("Logging:EnableSensitiveDataLogging"))
            {
                options.EnableSensitiveDataLogging();
            }

            if (configuration.GetValue<bool>("Logging:EnableDetailedErrors"))
            {
                options.EnableDetailedErrors();
            }
        });

        // Register read replica connection factory if available
        if (readReplicaConnectionStrings.Length > 0)
        {
            services.AddSingleton<IReadReplicaConnectionFactory>(
                new ReadReplicaConnectionFactory(readReplicaConnectionStrings));
        }

        return services;
    }
}

/// <summary>
/// Factory for creating read replica connections
/// </summary>
public interface IReadReplicaConnectionFactory
{
    string GetReadReplicaConnectionString();
}

public class ReadReplicaConnectionFactory : IReadReplicaConnectionFactory
{
    private readonly string[] _connectionStrings;
    private int _currentIndex = 0;

    public ReadReplicaConnectionFactory(string[] connectionStrings)
    {
        _connectionStrings = connectionStrings ?? throw new ArgumentNullException(nameof(connectionStrings));
        if (_connectionStrings.Length == 0)
        {
            throw new ArgumentException("At least one connection string is required.", nameof(connectionStrings));
        }
    }

    public string GetReadReplicaConnectionString()
    {
        // Round-robin selection
        var index = Interlocked.Increment(ref _currentIndex) % _connectionStrings.Length;
        return _connectionStrings[index];
    }
}
