using Shared.Logging;
using Shared.Metrics;
using Shared.HealthChecks;
using Shared.Data;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog for centralized logging
builder.ConfigureSerilog("ProgressService");

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// PRODUCTION FIX: Force SQL Server connection string to prevent any override
var connectionString = "Server=localhost,1433;Database=aspnet_learning_platform;User Id=sa;Password=P@ssw0rd!2026#SecurePlatform;TrustServerCertificate=True";
builder.Configuration["ConnectionStrings:DefaultConnection"] = connectionString;

// Add database context
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// Add services to the container.
builder.Services.AddOpenApi();

// Add health checks with explicit configuration (SQL Server only, RabbitMQ disabled temporarily)
builder.Services.AddHealthChecks()
    .AddSqlServerHealthCheck(
        connectionString,
        name: "sqlserver",
        tags: new[] { "database", "sqlserver" },
        timeout: TimeSpan.FromSeconds(5));

var app = builder.Build();

// NOTE: Database migrations and seeding moved to separate command
// Run manually: dotnet run --project src/Services/Progress/Progress.Service.csproj -- migrate
// This prevents blocking the service startup

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// Enable CORS
app.UseCors("AllowAll");

app.UseHttpsRedirection();

// Add Prometheus metrics
app.UsePrometheusMetrics();

// Add Serilog request logging with enrichment
app.UseSerilogRequestLogging();

// Add logging enrichment middleware
app.UseMiddleware<LoggingEnrichmentMiddleware>();

// Map health check endpoint
app.MapPlatformHealthChecks("/health");

// Dashboard endpoint
app.MapGet("/api/progress/dashboard", () =>
{
    return Results.Ok(new
    {
        currentXP = 150,
        currentLevel = 1,
        xpToNextLevel = 50,
        solvedChallenges = new
        {
            easy = 3,
            medium = 1,
            hard = 0
        },
        completedProjects = 0,
        learningStreak = 1,
        coursesInProgress = new[]
        {
            new
            {
                courseId = Guid.NewGuid(),
                title = "ASP.NET Core Basics",
                completionPercentage = 25.0
            }
        }
    });
});

// Leaderboard endpoint
app.MapGet("/api/leaderboard", () =>
{
    return Results.Ok(new
    {
        entries = new[]
        {
            new { rank = 1, name = "John Doe", xp = 1500, level = 3 },
            new { rank = 2, name = "Jane Smith", xp = 1200, level = 3 },
            new { rank = 3, name = "Bob Johnson", xp = 900, level = 2 }
        }
    });
});

app.Run();

// Ensure logs are flushed on shutdown
Log.CloseAndFlush();
