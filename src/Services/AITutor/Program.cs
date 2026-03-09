using AITutor.Service.Configuration;
using AITutor.Service.Services;
using Shared.Logging;
using Shared.Metrics;
using Shared.HealthChecks;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog for centralized logging
builder.ConfigureSerilog("AITutorService");

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddOpenApi();

// Configure Groq settings
builder.Services.Configure<GroqSettings>(
    builder.Configuration.GetSection(GroqSettings.SectionName));

// Register HttpClient for Groq API with timeout handling
builder.Services.AddHttpClient<IGroqApiClient, GroqApiClient>()
    .ConfigureHttpClient((serviceProvider, client) =>
    {
        var settings = serviceProvider.GetRequiredService<Microsoft.Extensions.Options.IOptions<GroqSettings>>().Value;
        client.Timeout = TimeSpan.FromSeconds(settings.TimeoutSeconds);
    });

// Register services
builder.Services.AddSingleton<CodeAnalysisPromptBuilder>();
builder.Services.AddScoped<IAITutorService, AITutorService>();

// Add health checks
builder.Services.AddPlatformHealthChecks(builder.Configuration, "AITutorService");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// Add Prometheus metrics
app.UsePrometheusMetrics();

// Add Serilog request logging with enrichment
app.UseSerilogRequestLogging();

// Add logging enrichment middleware
app.UseMiddleware<LoggingEnrichmentMiddleware>();

// Map health check endpoint
app.MapPlatformHealthChecks("/health");

app.MapControllers();

app.Run();

// Ensure logs are flushed on shutdown
Log.CloseAndFlush();
