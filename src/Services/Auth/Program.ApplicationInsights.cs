using Shared.Telemetry;

namespace Auth.Service;

/// <summary>
/// Example integration of Application Insights in Auth Service
/// This file demonstrates how to integrate Application Insights telemetry
/// </summary>
public static class ApplicationInsightsIntegration
{
    /// <summary>
    /// Configures Application Insights for the Auth Service
    /// </summary>
    public static IServiceCollection AddAuthServiceTelemetry(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Add Application Insights telemetry
        services.AddApplicationInsightsTelemetry(configuration, "AuthService");

        // Add HTTP context accessor for correlation
        services.AddHttpContextAccessor();

        // Add custom telemetry tracker
        services.AddSingleton<ICustomTelemetryTracker, CustomTelemetryTracker>();

        return services;
    }

    /// <summary>
    /// Configures Application Insights middleware
    /// </summary>
    public static IApplicationBuilder UseAuthServiceTelemetry(this IApplicationBuilder app)
    {
        // Add correlation ID middleware (must be early in pipeline)
        app.UseMiddleware<Shared.Middleware.CorrelationIdMiddleware>();

        return app;
    }
}

/// <summary>
/// Example usage in endpoints
/// </summary>
public static class TelemetryExamples
{
    public static void ConfigureEndpointsWithTelemetry(WebApplication app)
    {
        // Example: Login endpoint with telemetry
        app.MapPost("/api/auth/login", async (
            LoginRequest request,
            ICustomTelemetryTracker telemetry,
            ILogger<Program> logger) =>
        {
            var sw = System.Diagnostics.Stopwatch.StartNew();
            
            try
            {
                // Track user activity
                telemetry.TrackUserActivity(
                    request.Email,
                    "LoginAttempt",
                    new Dictionary<string, string>
                    {
                        ["Email"] = request.Email,
                        ["Timestamp"] = DateTime.UtcNow.ToString("O")
                    }
                );

                // Perform login logic
                var result = await PerformLogin(request);
                
                sw.Stop();

                if (result.Success)
                {
                    // Track successful login
                    telemetry.TrackUserActivity(
                        result.UserId.ToString(),
                        "LoginSuccess",
                        new Dictionary<string, string>
                        {
                            ["Email"] = request.Email,
                            ["Duration"] = sw.ElapsedMilliseconds.ToString()
                        }
                    );

                    // Track API performance
                    telemetry.TrackApiPerformance(
                        "/api/auth/login",
                        "POST",
                        sw.ElapsedMilliseconds,
                        200
                    );

                    return Results.Ok(result);
                }
                else
                {
                    // Track failed login
                    telemetry.TrackUserActivity(
                        request.Email,
                        "LoginFailure",
                        new Dictionary<string, string>
                        {
                            ["Email"] = request.Email,
                            ["Reason"] = result.ErrorMessage ?? "Unknown"
                        }
                    );

                    telemetry.TrackApiPerformance(
                        "/api/auth/login",
                        "POST",
                        sw.ElapsedMilliseconds,
                        401
                    );

                    return Results.Unauthorized();
                }
            }
            catch (Exception ex)
            {
                sw.Stop();
                
                logger.LogError(ex, "Login error for {Email}", request.Email);
                
                telemetry.TrackApiPerformance(
                    "/api/auth/login",
                    "POST",
                    sw.ElapsedMilliseconds,
                    500
                );

                return Results.Problem(ex.Message);
            }
        });

        // Example: Register endpoint with telemetry
        app.MapPost("/api/auth/register", async (
            RegisterRequest request,
            ICustomTelemetryTracker telemetry,
            ILogger<Program> logger) =>
        {
            var sw = System.Diagnostics.Stopwatch.StartNew();
            
            try
            {
                // Track registration attempt
                telemetry.TrackUserActivity(
                    request.Email,
                    "RegistrationAttempt",
                    new Dictionary<string, string>
                    {
                        ["Email"] = request.Email,
                        ["Name"] = request.Name
                    }
                );

                // Perform registration logic
                var result = await PerformRegistration(request);
                
                sw.Stop();

                if (result.Success)
                {
                    // Track successful registration
                    telemetry.TrackUserActivity(
                        result.UserId.ToString(),
                        "RegistrationSuccess",
                        new Dictionary<string, string>
                        {
                            ["Email"] = request.Email,
                            ["Duration"] = sw.ElapsedMilliseconds.ToString()
                        }
                    );

                    // Track business event
                    telemetry.TrackBusinessEvent(
                        "UserRegistered",
                        new Dictionary<string, string>
                        {
                            ["UserId"] = result.UserId.ToString(),
                            ["Email"] = request.Email
                        },
                        new Dictionary<string, double>
                        {
                            ["RegistrationTime"] = sw.ElapsedMilliseconds
                        }
                    );

                    telemetry.TrackApiPerformance(
                        "/api/auth/register",
                        "POST",
                        sw.ElapsedMilliseconds,
                        200
                    );

                    return Results.Ok(result);
                }
                else
                {
                    telemetry.TrackApiPerformance(
                        "/api/auth/register",
                        "POST",
                        sw.ElapsedMilliseconds,
                        400
                    );

                    return Results.BadRequest(new { error = result.ErrorMessage });
                }
            }
            catch (Exception ex)
            {
                sw.Stop();
                
                logger.LogError(ex, "Registration error for {Email}", request.Email);
                
                telemetry.TrackApiPerformance(
                    "/api/auth/register",
                    "POST",
                    sw.ElapsedMilliseconds,
                    500
                );

                return Results.Problem(ex.Message);
            }
        });
    }

    private static Task<LoginResult> PerformLogin(LoginRequest request)
    {
        // Placeholder - implement actual login logic
        return Task.FromResult(LoginResult.Successful(
            Guid.NewGuid(),
            "Test User",
            request.Email,
            "test-token",
            DateTime.UtcNow.AddHours(1)
        ));
    }

    private static Task<RegisterResult> PerformRegistration(RegisterRequest request)
    {
        // Placeholder - implement actual registration logic
        return Task.FromResult(RegisterResult.Successful(
            Guid.NewGuid(),
            "test-token",
            DateTime.UtcNow.AddHours(1)
        ));
    }
}
