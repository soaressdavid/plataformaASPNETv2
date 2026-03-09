using Microsoft.Extensions.Logging;
using Progress.Service.Services;
using Shared.Interfaces;
using Shared.Messaging;
using Shared.Models;

namespace Progress.Service.Consumers;

/// <summary>
/// Consumes ChallengeCompleted events from RabbitMQ and triggers XP award in Progress Service
/// Validates: Requirement 11.6 - RabbitMQ for asynchronous communication
/// </summary>
public class ChallengeCompletedEventConsumer : EventConsumerBase<ChallengeCompletedEvent>
{
    private const string QueueName = "progress.challenge-completed";
    private const string ExchangeName = "platform.events";
    private const string RoutingKey = "challenge.completed.*"; // Wildcard to match all difficulties

    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger<ChallengeCompletedEventConsumer> _logger;

    public ChallengeCompletedEventConsumer(
        IRabbitMQConnectionManager connectionManager,
        IServiceScopeFactory serviceScopeFactory,
        ILogger<ChallengeCompletedEventConsumer> logger)
        : base(connectionManager, logger, QueueName, ExchangeName, RoutingKey)
    {
        _serviceScopeFactory = serviceScopeFactory ?? throw new ArgumentNullException(nameof(serviceScopeFactory));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Processes ChallengeCompleted event by awarding XP to the user
    /// </summary>
    protected override async Task ProcessEventAsync(ChallengeCompletedEvent @event, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Processing ChallengeCompleted event {EventId}: User {UserId} completed challenge {ChallengeId} ({Difficulty}) for {XP} XP",
            @event.EventId, @event.UserId, @event.ChallengeId, @event.Difficulty, @event.XpAwarded);

        try
        {
            // Create a scope to resolve scoped services
            using var scope = _serviceScopeFactory.CreateScope();
            var progressService = scope.ServiceProvider.GetRequiredService<ProgressService>();

            // Award XP to the user
            await progressService.AwardXPAsync(@event.UserId, @event.XpAwarded);

            _logger.LogInformation(
                "Successfully awarded {XP} XP to user {UserId} for completing challenge {ChallengeId}",
                @event.XpAwarded, @event.UserId, @event.ChallengeId);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Failed to award XP for ChallengeCompleted event {EventId}: User {UserId}, Challenge {ChallengeId}",
                @event.EventId, @event.UserId, @event.ChallengeId);
            throw; // Re-throw to trigger message requeue
        }
    }
}
