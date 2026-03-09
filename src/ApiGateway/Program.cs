using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog simples
builder.Host.UseSerilog((context, configuration) =>
{
    configuration.WriteTo.Console();
});

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Add YARP reverse proxy
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

// Add health checks
builder.Services.AddHealthChecks();

var app = builder.Build();

// Use CORS
app.UseCors("AllowAll");

// Health check endpoint
app.MapHealthChecks("/health");

// Map reverse proxy
app.MapReverseProxy();

app.Run();
