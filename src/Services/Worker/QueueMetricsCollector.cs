using Shared.Interfaces;
using Shared.Metrics;

namespace Worker.Service;

/// <summary>
/// Background service that periodically collects queue metrics
/// Tracks queue depth for monitoring
/// </summary>
public class QueueMetricsCollector : BackgroundService
{
    private readonly ILogger<QueueMetricsCollector> _logger;
    private readonly IJobQueueService _jobQueue;
    private static readonly TimeSpan CollectionInterval = TimeSpan.FromSeconds(5);

    public QueueMetricsCollector(
        ILogger<QueueMetricsCollector> logger,
        IJobQueueService jobQueue)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _jobQueue = jobQueue ?? throw new ArgumentNullException(nameof(jobQueue));
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Queue Metrics Collector started");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                // Get queue depth
                var queueDepth = await _jobQueue.GetQueueDepthAsync();
                
                // Update metrics
                ApplicationMetrics.QueueDepth.Set(queueDepth);
                
                _logger.LogDebug("Queue depth: {QueueDepth}", queueDepth);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error collecting queue metrics");
            }

            await Task.Delay(CollectionInterval, stoppingToken);
        }

        _logger.LogInformation("Queue Metrics Collector stopped");
    }
}
