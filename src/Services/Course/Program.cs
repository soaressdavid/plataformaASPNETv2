using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.ResponseCompression;
using System.IO.Compression;
using Shared.Data;
using Shared.Entities;
using Shared.Logging;
using Shared.Metrics;
using Shared.HealthChecks;
using Shared.Middleware;
using FluentValidation;
using Course.Service.Services;

var builder = WebApplication.CreateBuilder(args);
builder.ConfigureSerilog("CourseService");

// Add response compression
builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
    options.Providers.Add<GzipCompressionProvider>();
});

builder.Services.Configure<GzipCompressionProviderOptions>(options =>
{
    options.Level = CompressionLevel.Fastest;
});

// Add controllers
builder.Services.AddControllers();

// Add database context
var connectionString = "Server=localhost,1433;Database=aspnet_learning_platform;User Id=sa;Password=P@ssw0rd!2026#SecurePlatform;TrustServerCertificate=True";
builder.Configuration["ConnectionStrings:DefaultConnection"] = connectionString;
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// Add memory cache
builder.Services.AddMemoryCache();

// Add caching service
builder.Services.AddScoped<CachedLevelsService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Add FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<Shared.Validators.CreateCourseRequestValidator>();

// Add health checks
builder.Services.AddPlatformHealthChecks(builder.Configuration, "CourseService");

var app = builder.Build();

// Use response compression
app.UseResponseCompression();

app.UseCors("AllowAll");

// Add correlation ID middleware (must be first)
app.UseMiddleware<CorrelationIdMiddleware>();

// Add enterprise-grade exception handling middleware
app.UseMiddleware<ExceptionHandlingMiddleware>();

// Add Prometheus metrics
app.UsePrometheusMetrics();

// Health check endpoint
app.MapPlatformHealthChecks("/health");

// Map controllers
app.MapControllers();

// NOTE: Database seeding moved to background task to prevent blocking startup
Task.Run(async () =>
{
    await Task.Delay(5000); // Wait 5 seconds for service to be fully started
    
    using (var scope = app.Services.CreateScope())
    {
        try
        {
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
            
            logger.LogInformation("Starting background database seeding...");
            context.Database.EnsureCreated();
            
            var levelCount = context.Set<CurriculumLevel>().Count();
            var courseCount = context.Courses.Count();
            
            logger.LogInformation($"Current state: {levelCount} levels, {courseCount} courses");
            
            if (levelCount == 0 || courseCount == 0)
            {
                logger.LogInformation("Seeding database in background...");
                DbSeeder.SeedData(context);
                logger.LogInformation("Database seeding completed successfully!");
                
                levelCount = context.Set<CurriculumLevel>().Count();
                courseCount = context.Courses.Count();
                var lessonCount = context.Lessons.Count();
                logger.LogInformation($"After seeding: {levelCount} levels, {courseCount} courses, {lessonCount} lessons");
            }
            else
            {
                logger.LogInformation("Database already seeded.");
            }
        }
        catch (Exception ex)
        {
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "An error occurred while seeding the database in background");
        }
    }
});

app.Run();
