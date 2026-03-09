using Microsoft.EntityFrameworkCore;
using Shared.Data;
using Shared.Models;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:3000", "https://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// Add DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowFrontend");
app.UseHttpsRedirection();

// IDE Session Endpoints

// Save IDE session
app.MapPost("/api/ide/session", async (IdeSessionRequest request, ApplicationDbContext db) =>
{
    try
    {
        // Serialize session state to JSON
        var sessionData = JsonSerializer.Serialize(request.SessionState);

        // Find existing session or create new one
        var session = await db.IdeSessions
            .FirstOrDefaultAsync(s => s.UserId == request.UserId);

        if (session == null)
        {
            session = new IdeSession
            {
                Id = Guid.NewGuid(),
                UserId = request.UserId,
                SessionData = sessionData,
                LastSavedAt = DateTime.UtcNow
            };
            db.IdeSessions.Add(session);
        }
        else
        {
            session.SessionData = sessionData;
            session.LastSavedAt = DateTime.UtcNow;
        }

        await db.SaveChangesAsync();

        return Results.Ok(new { message = "Session saved successfully", sessionId = session.Id });
    }
    catch (Exception ex)
    {
        return Results.Problem($"Error saving session: {ex.Message}");
    }
})
.WithName("SaveIdeSession")
.WithOpenApi();

// Load IDE session
app.MapGet("/api/ide/session/{userId}", async (string userId, ApplicationDbContext db) =>
{
    try
    {
        var session = await db.IdeSessions
            .FirstOrDefaultAsync(s => s.UserId == userId);

        if (session == null)
        {
            return Results.NotFound(new { message = "No session found for user" });
        }

        // Deserialize session data
        var sessionState = JsonSerializer.Deserialize<IdeSessionState>(session.SessionData);

        return Results.Ok(new
        {
            sessionId = session.Id,
            userId = session.UserId,
            sessionState,
            lastSavedAt = session.LastSavedAt
        });
    }
    catch (Exception ex)
    {
        return Results.Problem($"Error loading session: {ex.Message}");
    }
})
.WithName("LoadIdeSession")
.WithOpenApi();

// Delete IDE session
app.MapDelete("/api/ide/session/{userId}", async (string userId, ApplicationDbContext db) =>
{
    try
    {
        var session = await db.IdeSessions
            .FirstOrDefaultAsync(s => s.UserId == userId);

        if (session == null)
        {
            return Results.NotFound(new { message = "No session found for user" });
        }

        db.IdeSessions.Remove(session);
        await db.SaveChangesAsync();

        return Results.Ok(new { message = "Session deleted successfully" });
    }
    catch (Exception ex)
    {
        return Results.Problem($"Error deleting session: {ex.Message}");
    }
})
.WithName("DeleteIdeSession")
.WithOpenApi();

app.Run();

// Request DTOs
public record IdeSessionRequest(string UserId, IdeSessionState SessionState);
