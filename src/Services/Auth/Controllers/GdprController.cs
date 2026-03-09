using Microsoft.AspNetCore.Mvc;
using Shared.Interfaces;
using System.Text.Json;

namespace Auth.Service.Controllers;

/// <summary>
/// GDPR compliance endpoints for data export, deletion, and privacy management.
/// </summary>
[ApiController]
[Route("api/gdpr")]
public class GdprController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<GdprController> _logger;

    public GdprController(
        IUserRepository userRepository,
        ILogger<GdprController> logger)
    {
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Export all user data in JSON format (GDPR Article 20 - Right to Data Portability).
    /// </summary>
    /// <param name="userId">The user ID to export data for</param>
    /// <returns>JSON file containing all user data</returns>
    [HttpGet("export/{userId}")]
    [ProducesResponseType(typeof(FileContentResult), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> ExportUserData(Guid userId)
    {
        try
        {
            _logger.LogInformation("GDPR data export requested for user {UserId}", userId);

            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                _logger.LogWarning("Data export failed: User not found - UserId: {UserId}", userId);
                return NotFound(new { error = "User not found" });
            }

            // Collect all user data
            var userData = new
            {
                PersonalInformation = new
                {
                    user.Id,
                    user.Name,
                    user.Email,
                    user.CreatedAt,
                    user.UpdatedAt
                },
                AccountInformation = new
                {
                    user.FailedLoginAttempts,
                    user.LockoutEnd,
                    IsLockedOut = user.IsLockedOut
                },
                Progress = user.Progress != null ? new
                {
                    user.Progress.TotalXP,
                    user.Progress.CurrentLevel,
                    LearningStreak = user.Progress.LearningStreak,
                    user.Progress.LastActivityAt,
                    user.Progress.CreatedAt,
                    user.Progress.UpdatedAt
                } : null,
                Enrollments = user.Enrollments.Select(e => new
                {
                    e.Id,
                    e.CourseId,
                    e.EnrolledAt,
                    e.LastAccessedAt
                }).ToList(),
                Submissions = user.Submissions.Select(s => new
                {
                    s.Id,
                    s.ChallengeId,
                    s.Code,
                    s.Passed,
                    s.Result,
                    s.CreatedAt
                }).ToList(),
                LessonCompletions = user.LessonCompletions.Select(lc => new
                {
                    lc.Id,
                    lc.LessonId,
                    lc.CompletedAt
                }).ToList(),
                ProjectProgresses = user.ProjectProgresses.Select(pp => new
                {
                    pp.Id,
                    pp.ProjectId,
                    pp.CurrentStep,
                    pp.CompletedSteps,
                    pp.IsCompleted,
                    pp.CompletedAt,
                    pp.UpdatedAt
                }).ToList(),
                ExportMetadata = new
                {
                    ExportDate = DateTime.UtcNow,
                    ExportFormat = "JSON",
                    GdprCompliance = "Article 20 - Right to Data Portability"
                }
            };

            var json = JsonSerializer.Serialize(userData, new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            var bytes = System.Text.Encoding.UTF8.GetBytes(json);
            var fileName = $"user_data_{userId}_{DateTime.UtcNow:yyyyMMdd_HHmmss}.json";

            _logger.LogInformation("GDPR data export completed for user {UserId}, file size: {Size} bytes", 
                userId, bytes.Length);

            return File(bytes, "application/json", fileName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error exporting user data for user {UserId}", userId);
            return StatusCode(500, new { error = "Failed to export user data" });
        }
    }

    /// <summary>
    /// Delete all user data (GDPR Article 17 - Right to Erasure / Right to be Forgotten).
    /// This is a permanent operation and cannot be undone.
    /// </summary>
    /// <param name="userId">The user ID to delete</param>
    /// <param name="request">Deletion confirmation request</param>
    /// <returns>Confirmation of deletion</returns>
    [HttpDelete("delete/{userId}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> DeleteUserData(Guid userId, [FromBody] DeleteUserDataRequest request)
    {
        try
        {
            _logger.LogWarning("GDPR data deletion requested for user {UserId}", userId);

            // Validate confirmation
            if (!request.ConfirmDeletion)
            {
                return BadRequest(new { error = "Deletion must be confirmed" });
            }

            if (request.Email != null)
            {
                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null)
                {
                    _logger.LogWarning("Data deletion failed: User not found - UserId: {UserId}", userId);
                    return NotFound(new { error = "User not found" });
                }

                // Verify email matches for additional security
                if (!user.Email.Equals(request.Email, StringComparison.OrdinalIgnoreCase))
                {
                    _logger.LogWarning("Data deletion failed: Email mismatch - UserId: {UserId}", userId);
                    return BadRequest(new { error = "Email does not match user account" });
                }
            }

            // Perform hard delete (GDPR requires complete removal)
            var deleted = await _userRepository.HardDeleteAsync(userId);

            if (!deleted)
            {
                _logger.LogWarning("Data deletion failed: User not found - UserId: {UserId}", userId);
                return NotFound(new { error = "User not found" });
            }

            _logger.LogWarning("GDPR data deletion completed for user {UserId}", userId);

            return Ok(new
            {
                message = "User data has been permanently deleted",
                userId = userId,
                deletedAt = DateTime.UtcNow,
                gdprCompliance = "Article 17 - Right to Erasure"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting user data for user {UserId}", userId);
            return StatusCode(500, new { error = "Failed to delete user data" });
        }
    }

    /// <summary>
    /// Get user's privacy settings and consent status (GDPR Article 7 - Conditions for Consent).
    /// </summary>
    /// <param name="userId">The user ID</param>
    /// <returns>Privacy settings and consent status</returns>
    [HttpGet("privacy/{userId}")]
    [ProducesResponseType(typeof(PrivacySettingsResponse), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetPrivacySettings(Guid userId)
    {
        try
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return NotFound(new { error = "User not found" });
            }

            // TODO: Implement privacy settings storage
            // For now, return default settings
            var settings = new PrivacySettingsResponse
            {
                UserId = userId,
                DataProcessingConsent = true,
                MarketingConsent = false,
                AnalyticsConsent = true,
                ThirdPartyDataSharingConsent = false,
                ConsentDate = user.CreatedAt,
                LastUpdated = user.UpdatedAt
            };

            return Ok(settings);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting privacy settings for user {UserId}", userId);
            return StatusCode(500, new { error = "Failed to get privacy settings" });
        }
    }

    /// <summary>
    /// Update user's privacy settings and consent (GDPR Article 7 - Conditions for Consent).
    /// </summary>
    /// <param name="userId">The user ID</param>
    /// <param name="request">Updated privacy settings</param>
    /// <returns>Updated privacy settings</returns>
    [HttpPut("privacy/{userId}")]
    [ProducesResponseType(typeof(PrivacySettingsResponse), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> UpdatePrivacySettings(Guid userId, [FromBody] UpdatePrivacySettingsRequest request)
    {
        try
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return NotFound(new { error = "User not found" });
            }

            // TODO: Implement privacy settings storage
            // For now, just log the update
            _logger.LogInformation("Privacy settings updated for user {UserId}: DataProcessing={DataProcessing}, Marketing={Marketing}, Analytics={Analytics}, ThirdParty={ThirdParty}",
                userId, request.DataProcessingConsent, request.MarketingConsent, request.AnalyticsConsent, request.ThirdPartyDataSharingConsent);

            var settings = new PrivacySettingsResponse
            {
                UserId = userId,
                DataProcessingConsent = request.DataProcessingConsent,
                MarketingConsent = request.MarketingConsent,
                AnalyticsConsent = request.AnalyticsConsent,
                ThirdPartyDataSharingConsent = request.ThirdPartyDataSharingConsent,
                ConsentDate = user.CreatedAt,
                LastUpdated = DateTime.UtcNow
            };

            return Ok(settings);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating privacy settings for user {UserId}", userId);
            return StatusCode(500, new { error = "Failed to update privacy settings" });
        }
    }

    /// <summary>
    /// Get data retention policy information (GDPR Article 13 - Information to be Provided).
    /// </summary>
    /// <returns>Data retention policy</returns>
    [HttpGet("retention-policy")]
    [ProducesResponseType(typeof(DataRetentionPolicyResponse), 200)]
    public IActionResult GetDataRetentionPolicy()
    {
        var policy = new DataRetentionPolicyResponse
        {
            PersonalData = new RetentionPeriod
            {
                DataType = "Personal Information (Name, Email)",
                Period = "Account lifetime + 30 days after deletion request",
                Purpose = "User identification and authentication"
            },
            ActivityData = new RetentionPeriod
            {
                DataType = "Learning Activity (Submissions, Progress)",
                Period = "Account lifetime + 90 days for analytics",
                Purpose = "Learning progress tracking and platform improvement"
            },
            AnalyticsData = new RetentionPeriod
            {
                DataType = "Anonymous Analytics",
                Period = "2 years",
                Purpose = "Platform improvement and research"
            },
            BackupData = new RetentionPeriod
            {
                DataType = "Backup Data",
                Period = "30 days",
                Purpose = "Disaster recovery"
            },
            LegalBasis = "GDPR Article 6(1)(b) - Contract Performance, Article 6(1)(f) - Legitimate Interests",
            LastUpdated = new DateTime(2025, 3, 9)
        };

        return Ok(policy);
    }
}

// Request/Response Models

public record DeleteUserDataRequest
{
    public bool ConfirmDeletion { get; init; }
    public string? Email { get; init; }
}

public record PrivacySettingsResponse
{
    public Guid UserId { get; init; }
    public bool DataProcessingConsent { get; init; }
    public bool MarketingConsent { get; init; }
    public bool AnalyticsConsent { get; init; }
    public bool ThirdPartyDataSharingConsent { get; init; }
    public DateTime ConsentDate { get; init; }
    public DateTime LastUpdated { get; init; }
}

public record UpdatePrivacySettingsRequest
{
    public bool DataProcessingConsent { get; init; }
    public bool MarketingConsent { get; init; }
    public bool AnalyticsConsent { get; init; }
    public bool ThirdPartyDataSharingConsent { get; init; }
}

public record DataRetentionPolicyResponse
{
    public RetentionPeriod PersonalData { get; init; } = null!;
    public RetentionPeriod ActivityData { get; init; } = null!;
    public RetentionPeriod AnalyticsData { get; init; } = null!;
    public RetentionPeriod BackupData { get; init; } = null!;
    public string LegalBasis { get; init; } = string.Empty;
    public DateTime LastUpdated { get; init; }
}

public record RetentionPeriod
{
    public string DataType { get; init; } = string.Empty;
    public string Period { get; init; } = string.Empty;
    public string Purpose { get; init; } = string.Empty;
}
