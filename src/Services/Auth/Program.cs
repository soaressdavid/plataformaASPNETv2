using Shared.Logging;
using Shared.HealthChecks;
using Serilog;
using Auth.Service;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog for centralized logging
builder.ConfigureSerilog("AuthService");

// PRODUCTION FIX: Force SQL Server connection string to prevent any override
var connectionString = "Server=localhost,1433;Database=aspnet_learning_platform;User Id=sa;Password=P@ssw0rd!2026#SecurePlatform;TrustServerCertificate=True";
builder.Configuration["ConnectionStrings:DefaultConnection"] = connectionString;

// PRODUCTION FIX: Force RabbitMQ configuration
builder.Configuration["RabbitMQ:Host"] = "localhost";
builder.Configuration["RabbitMQ:Port"] = "5672";
builder.Configuration["RabbitMQ:Username"] = "platform_user";
builder.Configuration["RabbitMQ:Password"] = "SimplePass123";

// Configure JWT Token Service with generated keys
var rsa = RSA.Create(2048);
var privateKey = new RsaSecurityKey(rsa);
var publicKey = new RsaSecurityKey(rsa);
var jwtTokenService = new JwtTokenService(privateKey, publicKey);
builder.Services.AddSingleton(jwtTokenService);

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

// Add health checks
builder.Services.AddPlatformHealthChecks(builder.Configuration, "AuthService");

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// Enable CORS
app.UseCors("AllowAll");

app.UseHttpsRedirection();

// Add Serilog request logging with enrichment
app.UseSerilogRequestLogging();

// Add logging enrichment middleware
app.UseMiddleware<LoggingEnrichmentMiddleware>();

// Map health check endpoint
app.MapPlatformHealthChecks("/health");

// Temporary in-memory user storage for testing (replace with database later)
var users = new System.Collections.Concurrent.ConcurrentDictionary<string, (Guid Id, string Name, string Email, string PasswordHash)>();

// Seed default test users for easy access
var defaultUsers = new[]
{
    (Id: Guid.NewGuid(), Name: "Alice Johnson", Email: "alice@example.com", Password: "password123"),
    (Id: Guid.NewGuid(), Name: "Bob Smith", Email: "bob@example.com", Password: "securepass456"),
    (Id: Guid.NewGuid(), Name: "Carol Davis", Email: "carol@example.com", Password: "mypassword789"),
    (Id: Guid.NewGuid(), Name: "David Wilson", Email: "david@example.com", Password: "testpass321"),
    (Id: Guid.NewGuid(), Name: "Emma Martinez", Email: "emma@example.com", Password: "demouser2024"),
    (Id: Guid.NewGuid(), Name: "Test User", Email: "test@test.com", Password: "Test123!")
};

foreach (var user in defaultUsers)
{
    var passwordHash = BCrypt.Net.BCrypt.HashPassword(user.Password);
    users[user.Email.ToLower()] = (user.Id, user.Name, user.Email, passwordHash);
}

Log.Information("Auth Service initialized with {Count} default test users", users.Count);

// Register endpoint
app.MapPost("/api/auth/register", async (RegisterRequest request, ILogger<Program> logger, JwtTokenService tokenService) =>
{
    try
    {
        logger.LogInformation("Register attempt - Name: {Name}, Email: {Email}, Password length: {Length}", 
            request.Name, request.Email, request.Password?.Length ?? 0);

        // Validate inputs
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            logger.LogWarning("Registration failed: Name is required");
            return Results.BadRequest(new { error = "Name is required" });
        }

        if (string.IsNullOrWhiteSpace(request.Email))
        {
            logger.LogWarning("Registration failed: Email is required");
            return Results.BadRequest(new { error = "Email is required" });
        }

        if (string.IsNullOrWhiteSpace(request.Password))
        {
            logger.LogWarning("Registration failed: Password is required");
            return Results.BadRequest(new { error = "Password is required" });
        }

        if (request.Password.Length < 6)
        {
            logger.LogWarning("Registration failed: Password too short ({Length} characters)", request.Password.Length);
            return Results.BadRequest(new { error = "Password must be at least 6 characters long" });
        }

        // Check if email already exists
        if (users.ContainsKey(request.Email.ToLower()))
        {
            logger.LogWarning("Registration failed: Email already registered - {Email}", request.Email);
            return Results.BadRequest(new { error = "Email is already registered" });
        }

        // Create new user
        var userId = Guid.NewGuid();
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
        
        users[request.Email.ToLower()] = (userId, request.Name, request.Email, passwordHash);

        logger.LogInformation("User registered successfully - UserId: {UserId}, Email: {Email}", userId, request.Email);

        // Generate JWT token
        var token = tokenService.GenerateToken(userId, request.Email, request.Name);
        var expiresAt = DateTime.UtcNow.Add(tokenService.TokenExpiration);

        return Results.Ok(new
        {
            userId = userId,
            token = token,
            expiresAt = expiresAt
        });
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Registration error");
        return Results.Problem(ex.Message);
    }
});

// Login endpoint
app.MapPost("/api/auth/login", async (LoginRequest request, JwtTokenService tokenService) =>
{
    try
    {
        // Validate inputs
        if (string.IsNullOrWhiteSpace(request.Email))
        {
            return Results.BadRequest(new { error = "Email is required" });
        }

        if (string.IsNullOrWhiteSpace(request.Password))
        {
            return Results.BadRequest(new { error = "Password is required" });
        }

        // Find user
        if (!users.TryGetValue(request.Email.ToLower(), out var user))
        {
            return Results.Unauthorized();
        }

        // Verify password
        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        {
            return Results.Unauthorized();
        }

        // Generate JWT token
        var token = tokenService.GenerateToken(user.Id, user.Email, user.Name);
        var expiresAt = DateTime.UtcNow.Add(tokenService.TokenExpiration);

        return Results.Ok(new
        {
            userId = user.Id,
            name = user.Name,
            email = user.Email,
            token = token,
            expiresAt = expiresAt
        });
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message);
    }
});

app.Run();

// Ensure logs are flushed on shutdown
Log.CloseAndFlush();

record RegisterRequest(string Name, string Email, string Password);
record LoginRequest(string Email, string Password);
