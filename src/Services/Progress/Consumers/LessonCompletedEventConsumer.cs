using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.Data;
using Shared.Entities;
using Shared.Interfaces;
using Shared.Messaging;
using Shared.Models;

namespace Progress.Service.Consumers;

/// <summary>
/// Consumes LessonCompleted events from RabbitMQ and updates course progress
/// Validates: Requirement 11.6 - RabbitMQ for asynchronous communication
/// </summary>
public class LessonCompletedEventConsumer : EventConsumerBase<LessonCompletedEvent>
{
    private const string QueueName = "progress.lesson-completed";
    private const string ExchangeName = "platform.events";
    private const string RoutingKey = "lesson.completed";

    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger<LessonCompletedEventConsumer> _logger;

    public LessonCompletedEventConsumer(
        IRabbitMQConnectionManager connectionManager,
        IServiceScopeFactory serviceScopeFactory,
        ILogger<LessonCompletedEventConsumer> logger)
        : base(connectionManager, logger, QueueName, ExchangeName, RoutingKey)
    {
        _serviceScopeFactory = serviceScopeFactory ?? throw new ArgumentNullException(nameof(serviceScopeFactory));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Processes LessonCompleted event by updating course progress
    /// </summary>
    protected override async Task ProcessEventAsync(LessonCompletedEvent @event, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Processing LessonCompleted event {EventId}: User {UserId} completed lesson {LessonId} in course {CourseId}",
            @event.EventId, @event.UserId, @event.LessonId, @event.CourseId);

        try
        {
            // Create a scope to resolve scoped services
            using var scope = _serviceScopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            // Check if lesson completion already exists (idempotency)
            var existingCompletion = await dbContext.LessonCompletions
                .FirstOrDefaultAsync(lc => lc.UserId == @event.UserId && lc.LessonId == @event.LessonId, cancellationToken);

            if (existingCompletion != null)
            {
                _logger.LogInformation(
                    "Lesson completion already exists for user {UserId} and lesson {LessonId}, skipping",
                    @event.UserId, @event.LessonId);
                return;
            }

            // Create lesson completion record
            var lessonCompletion = new LessonCompletion
            {
                Id = Guid.NewGuid(),
                UserId = @event.UserId,
                LessonId = @event.LessonId,
                CompletedAt = @event.OccurredAt
            };

            dbContext.LessonCompletions.Add(lessonCompletion);
            await dbContext.SaveChangesAsync(cancellationToken);

            _logger.LogInformation(
                "Successfully recorded lesson completion for user {UserId}, lesson {LessonId} in course {CourseId}",
                @event.UserId, @event.LessonId, @event.CourseId);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Failed to update course progress for LessonCompleted event {EventId}: User {UserId}, Lesson {LessonId}, Course {CourseId}",
                @event.EventId, @event.UserId, @event.LessonId, @event.CourseId);
            throw; // Re-throw to trigger message requeue
        }
    }
}
