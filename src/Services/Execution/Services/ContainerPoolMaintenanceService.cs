namespace Execution.Service.Services;

/// <summary>
/// Background service for container pool maintenance tasks
/// Performs periodic health checks and cleanup of expired containers
/// </summary>
public class ContainerPoolMaintenanceService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<ContainerPoolMaintenanceService> _logger;
    
    private const int HealthCheckIntervalSeconds = 30;
    private const int CleanupIntervalSeconds = 60;

    public ContainerPoolMaintenanceService(
        IServiceProvider serviceProvider,
        ILogger<ContainerPoolMaintenanceService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Container pool maintenance service started");

        // Start both maintenance tasks
        var healthCheckTask = PerformPeriodicHealthChecksAsync(stoppingToken);
        var cleanupTask = PerformPeriodicCleanupAsync(stoppingToken);

        await Task.WhenAll(healthCheckTask, cleanupTask);
    }

    private async Task PerformPeriodicHealthChecksAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await Task.Delay(TimeSpan.FromSeconds(HealthCheckIntervalSeconds), stoppingToken);

                using var scope = _serviceProvider.CreateScope();
                var poolManager = scope.ServiceProvider.GetRequiredService<IContainerPoolManager>();
                
                await poolManager.PerformHealthCheckAsync(stoppingToken);
            }
            catch (OperationCanceledException)
            {
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during health check");
            }
        }

        _logger.LogInformation("Health check task stopped");
    }

    private async Task PerformPeriodicCleanupAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await Task.Delay(TimeSpan.FromSeconds(CleanupIntervalSeconds), stoppingToken);

                using var scope = _serviceProvider.CreateScope();
                var poolManager = scope.ServiceProvider.GetRequiredService<IContainerPoolManager>();
                
                await poolManager.CleanupExpiredContainersAsync(stoppingToken);
                
                // Log pool stats
                var stats = await poolManager.GetPoolStatsAsync();
                _logger.LogInformation(
                    "Pool stats - Total: {Total}, Available: {Available}, InUse: {InUse}, Queue: {Queue}",
                    stats.TotalContainers, stats.AvailableContainers, stats.InUseContainers, stats.QueueLength);
            }
            catch (OperationCanceledException)
            {
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during cleanup");
            }
        }

        _logger.LogInformation("Cleanup task stopped");
    }
}
