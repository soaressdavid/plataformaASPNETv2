using SqlExecutor.Service.Services;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen(); // Temporariamente desabilitado

// Redis for session management
var redisConnection = builder.Configuration.GetConnectionString("Redis") ?? "localhost:6379";
builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(redisConnection));

// Register services
builder.Services.AddSingleton<ISqlContainerManager, SqlContainerManager>();
builder.Services.AddSingleton<SqlQueryValidator>();
builder.Services.AddSingleton<SqlSessionManager>();

// Health checks
builder.Services.AddHealthChecks()
    .AddRedis(redisConnection, name: "redis");

// CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Background service for cleanup
builder.Services.AddHostedService<ContainerCleanupService>();

var app = builder.Build();

// Configure the HTTP request pipeline
// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI();
// }

app.UseCors();
app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();
app.MapHealthChecks("/health");

app.Run();
