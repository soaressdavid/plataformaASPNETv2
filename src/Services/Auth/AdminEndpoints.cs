using Microsoft.AspNetCore.Mvc;
using Shared.Interfaces;

namespace Auth.Service;

/// <summary>
/// Administrative endpoints for account management.
/// These endpoints should be protected with admin-only authorization in production.
/// </summary>
public static class AdminEndpoints
{
    /// <summary>
    /// Registers administrative endpoints for account management.
    /// </summary>
    public static void MapAdminEndpoints(this WebApplication app)
    {
        var adminGroup = app.MapGroup("/api/admin/accounts")
            .WithTags("Admin - Account Management");
            // TODO: Add admin authorization: .RequireAuthorization("AdminOnly");

        // Unlock account endpoint
        adminGroup.MapPost("/{userId}/unlock", async (
            Guid userId,
            [FromServices] IUserRepository userRepository,
            ILogger<Program> logger) =>
        {
            try
            {
                var user = await userRepository.GetByIdAsync(userId);
                if (user == null)
                {
                    logger.LogWarning("Unlock failed: User not found - UserId: {UserId}", userId);
                    return Results.NotFound(new { error = "User not found" });
                }

                // Reset lockout fields
                user.FailedLoginAttempts = 0;
                user.LockoutEnd = null;
                user.UpdatedAt = DateTime.UtcNow;

                await userRepository.UpdateAsync(user);

                logger.LogInformation("Account unlocked successfully - UserId: {UserId}, Email: {Email}", 
                    userId, user.Email);

                return Results.Ok(new
                {
                    message = "Account unlocked successfully",
                    userId = user.Id,
                    email = user.Email
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error unlocking account - UserId: {UserId}", userId);
                return Results.Problem(ex.Message);
            }
        })
        .WithName("UnlockAccount")
        .WithSummary("Unlock a locked user account")
        .WithDescription("Resets failed login attempts and removes lockout for a user account. Requires admin authorization.");

        // Get account lockout status
        adminGroup.MapGet("/{userId}/lockout-status", async (
            Guid userId,
            [FromServices] IUserRepository userRepository,
            ILogger<Program> logger) =>
        {
            try
            {
                var user = await userRepository.GetByIdAsync(userId);
                if (user == null)
                {
                    logger.LogWarning("Lockout status check failed: User not found - UserId: {UserId}", userId);
                    return Results.NotFound(new { error = "User not found" });
                }

                var isLockedOut = user.IsLockedOut;
                var remainingMinutes = isLockedOut && user.LockoutEnd.HasValue
                    ? (int)(user.LockoutEnd.Value - DateTime.UtcNow).TotalMinutes
                    : 0;

                return Results.Ok(new
                {
                    userId = user.Id,
                    email = user.Email,
                    isLockedOut = isLockedOut,
                    failedLoginAttempts = user.FailedLoginAttempts,
                    lockoutEnd = user.LockoutEnd,
                    remainingMinutes = remainingMinutes
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error checking lockout status - UserId: {UserId}", userId);
                return Results.Problem(ex.Message);
            }
        })
        .WithName("GetLockoutStatus")
        .WithSummary("Get account lockout status")
        .WithDescription("Returns the current lockout status and failed login attempts for a user account. Requires admin authorization.");

        // Unlock account by email (alternative endpoint)
        adminGroup.MapPost("/unlock-by-email", async (
            [FromBody] UnlockByEmailRequest request,
            [FromServices] IUserRepository userRepository,
            ILogger<Program> logger) =>
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.Email))
                {
                    return Results.BadRequest(new { error = "Email is required" });
                }

                var user = await userRepository.GetByEmailAsync(request.Email);
                if (user == null)
                {
                    logger.LogWarning("Unlock failed: User not found - Email: {Email}", request.Email);
                    return Results.NotFound(new { error = "User not found" });
                }

                // Reset lockout fields
                user.FailedLoginAttempts = 0;
                user.LockoutEnd = null;
                user.UpdatedAt = DateTime.UtcNow;

                await userRepository.UpdateAsync(user);

                logger.LogInformation("Account unlocked successfully - UserId: {UserId}, Email: {Email}", 
                    user.Id, user.Email);

                return Results.Ok(new
                {
                    message = "Account unlocked successfully",
                    userId = user.Id,
                    email = user.Email
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error unlocking account by email - Email: {Email}", request.Email);
                return Results.Problem(ex.Message);
            }
        })
        .WithName("UnlockAccountByEmail")
        .WithSummary("Unlock a locked user account by email")
        .WithDescription("Resets failed login attempts and removes lockout for a user account using email. Requires admin authorization.");
    }
}

public record UnlockByEmailRequest(string Email);
