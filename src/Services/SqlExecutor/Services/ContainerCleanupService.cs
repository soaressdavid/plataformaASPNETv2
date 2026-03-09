namespace SqlExecutor.Service.Services;

/// <summary>
/// Background service that periodically cleans up inactive SQL containers
/// Validates: Requirement 2.3 (automatic cleanup after 30 min inactivity)
/// </summary>
public class ContainerCleanupService : BackgroundService
{
    private readonly ILogger<ContainerCleanupService> _logger;
    private readonly IServiceProvider _serviceProvider;
    private const int CleanupIntervalMinutes = 5;

    public ContainerCleanupService(
        ILogger<ContainerCleanupService> logger,
        IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Container Cleanup Service started");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await Task.Delay(TimeSpan.FromMinutes(CleanupIntervalMinutes), stoppingToken);

                _logger.LogInformation("Running container cleanup...");

                using var scope = _serviceProvider.CreateScope();
                var containerManager = scope.ServiceProvider.GetRequiredService<ISqlContainerManager>();
                
                await containerManager.CleanupInactiveContainersAsync(stoppingToken);

                _logger.LogInformation("Container cleanup completed");
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                // Normal shutdown
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during container cleanup");
            }
        }

        _logger.LogInformation("Container Cleanup Service stopped");
    }
}
