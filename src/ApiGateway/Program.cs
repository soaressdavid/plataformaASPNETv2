using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

builder.Host.UseSerilog();

// Add CORS - Configurado com métodos e headers específicos para segurança
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins(
                "http://localhost:3000",
                "http://127.0.0.1:3000"
              )
              .WithMethods("GET", "POST", "PUT", "DELETE", "PATCH", "OPTIONS")
              .WithHeaders(
                "Content-Type",
                "Authorization",
                "X-Requested-With",
                "Accept",
                "Origin"
              )
              .AllowCredentials(); // Permite cookies e headers de autenticação
    });
});

// Add YARP reverse proxy com configuração simples
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

// Add health checks simples
builder.Services.AddHealthChecks();

var app = builder.Build();

// CORS deve ser o primeiro middleware
app.UseCors("AllowFrontend");

// Request logging
app.UseSerilogRequestLogging();

// Health check endpoint
app.MapHealthChecks("/health");

// Map reverse proxy - isso faz o roteamento para os serviços
app.MapReverseProxy();

app.Run();

Log.CloseAndFlush();
