using Microsoft.AspNetCore.Mvc;
using Analytics.Service.DTOs;
using Analytics.Service.Services;

namespace Analytics.Service.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AnalyticsController : ControllerBase
{
    private readonly IAnalyticsService _analyticsService;
    private readonly ILogger<AnalyticsController> _logger;

    public AnalyticsController(
        IAnalyticsService analyticsService,
        ILogger<AnalyticsController> logger)
    {
        _analyticsService = analyticsService;
        _logger = logger;
    }

    /// <summary>
    /// Track a telemetry event
    /// </summary>
    [HttpPost("track")]
    public async Task<IActionResult> TrackEvent([FromBody] TelemetryEvent telemetryEvent)
    {
        try
        {
            await _analyticsService.TrackEventAsync(telemetryEvent);
            return Ok(new { success = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error tracking event");
            return StatusCode(500, new { message = "Failed to track event" });
        }
    }

    /// <summary>
    /// Track lesson completion
    /// </summary>
    [HttpPost("track/lesson-completion")]
    public async Task<IActionResult> TrackLessonCompletion([FromBody] LessonCompletionEvent completionEvent)
    {
        try
        {
            await _analyticsService.TrackLessonCompletionAsync(completionEvent);
            return Ok(new { success = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error tracking lesson completion");
            return StatusCode(500, new { message = "Failed to track lesson completion" });
        }
    }

    /// <summary>
    /// Track challenge completion
    /// </summary>
    [HttpPost("track/challenge-completion")]
    public async Task<IActionResult> TrackChallengeCompletion([FromBody] ChallengeCompletionEvent completionEvent)
    {
        try
        {
            await _analyticsService.TrackChallengeCompletionAsync(completionEvent);
            return Ok(new { success = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error tracking challenge completion");
            return StatusCode(500, new { message = "Failed to track challenge completion" });
        }
    }

    /// <summary>
    /// Track content view
    /// </summary>
    [HttpPost("track/content-view")]
    public async Task<IActionResult> TrackContentView([FromBody] ContentViewEvent viewEvent)
    {
        try
        {
            await _analyticsService.TrackContentViewAsync(viewEvent);
            return Ok(new { success = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error tracking content view");
            return StatusCode(500, new { message = "Failed to track content view" });
        }
    }

    /// <summary>
    /// Track user activity
    /// </summary>
    [HttpPost("track/user-activity")]
    public async Task<IActionResult> TrackUserActivity([FromBody] UserActivityEvent activityEvent)
    {
        try
        {
            await _analyticsService.TrackUserActivityAsync(activityEvent);
            return Ok(new { success = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error tracking user activity");
            return StatusCode(500, new { message = "Failed to track user activity" });
        }
    }

    /// <summary>
    /// Get lesson completion metrics
    /// </summary>
    [HttpGet("lessons/{lessonId}/metrics")]
    public async Task<IActionResult> GetLessonMetrics(Guid lessonId)
    {
        try
        {
            var metrics = await _analyticsService.GetLessonMetricsAsync(lessonId);
            return Ok(metrics);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting lesson metrics for {LessonId}", lessonId);
            return StatusCode(500, new { message = "Failed to get lesson metrics" });
        }
    }

    /// <summary>
    /// Get lessons with low completion rate
    /// </summary>
    [HttpGet("lessons/low-completion")]
    public async Task<IActionResult> GetLowCompletionLessons([FromQuery] double threshold = 0.5)
    {
        try
        {
            var lessons = await _analyticsService.GetLowCompletionLessonsAsync(threshold);
            return Ok(lessons);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting low completion lessons");
            return StatusCode(500, new { message = "Failed to get low completion lessons" });
        }
    }

    /// <summary>
    /// Get retention metrics
    /// </summary>
    [HttpGet("retention")]
    public async Task<IActionResult> GetRetentionMetrics(
        [FromQuery] DateTime? startDate = null,
        [FromQuery] DateTime? endDate = null)
    {
        try
        {
            var metrics = await _analyticsService.GetRetentionMetricsAsync(
                startDate ?? DateTime.UtcNow.AddMonths(-3),
                endDate ?? DateTime.UtcNow
            );
            return Ok(metrics);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting retention metrics");
            return StatusCode(500, new { message = "Failed to get retention metrics" });
        }
    }

    /// <summary>
    /// Get active users metrics
    /// </summary>
    [HttpGet("active-users")]
    public async Task<IActionResult> GetActiveUsers(
        [FromQuery] DateTime? startDate = null,
        [FromQuery] DateTime? endDate = null)
    {
        try
        {
            var metrics = await _analyticsService.GetActiveUsersAsync(
                startDate ?? DateTime.UtcNow.AddMonths(-1),
                endDate ?? DateTime.UtcNow
            );
            return Ok(metrics);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting active users");
            return StatusCode(500, new { message = "Failed to get active users" });
        }
    }

    /// <summary>
    /// Get content drop-off analysis
    /// </summary>
    [HttpGet("content/{contentId}/dropoff")]
    public async Task<IActionResult> GetContentDropOff(Guid contentId)
    {
        try
        {
            var analysis = await _analyticsService.GetContentDropOffAsync(contentId);
            return Ok(analysis);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting drop-off for content {ContentId}", contentId);
            return StatusCode(500, new { message = "Failed to get drop-off analysis" });
        }
    }

    /// <summary>
    /// Get content with high drop-off rate
    /// </summary>
    [HttpGet("content/high-dropoff")]
    public async Task<IActionResult> GetHighDropOffContent([FromQuery] double threshold = 0.5)
    {
        try
        {
            var content = await _analyticsService.GetHighDropOffContentAsync(threshold);
            return Ok(content);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting high drop-off content");
            return StatusCode(500, new { message = "Failed to get high drop-off content" });
        }
    }

    /// <summary>
    /// Get dashboard metrics
    /// </summary>
    [HttpGet("dashboard")]
    public async Task<IActionResult> GetDashboardMetrics()
    {
        try
        {
            var metrics = await _analyticsService.GetDashboardMetricsAsync();
            return Ok(metrics);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting dashboard metrics");
            return StatusCode(500, new { message = "Failed to get dashboard metrics" });
        }
    }

    /// <summary>
    /// Get user engagement metrics
    /// </summary>
    [HttpGet("users/{userId}/engagement")]
    public async Task<IActionResult> GetUserEngagement(Guid userId)
    {
        try
        {
            var metrics = await _analyticsService.GetUserEngagementMetricsAsync(userId);
            return Ok(metrics);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting engagement for user {UserId}", userId);
            return StatusCode(500, new { message = "Failed to get user engagement" });
        }
    }

    /// <summary>
    /// Get user cohorts
    /// </summary>
    [HttpGet("cohorts")]
    public async Task<IActionResult> GetUserCohorts(
        [FromQuery] DateTime? startDate = null,
        [FromQuery] DateTime? endDate = null)
    {
        try
        {
            var cohorts = await _analyticsService.GetUserCohortsAsync(
                startDate ?? DateTime.UtcNow.AddMonths(-3),
                endDate ?? DateTime.UtcNow
            );
            return Ok(cohorts);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting user cohorts");
            return StatusCode(500, new { message = "Failed to get user cohorts" });
        }
    }
}
