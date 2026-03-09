using Docker.DotNet;
using Shared.Interfaces;
using Shared.Services;
using StackExchange.Redis;
using Worker.Service;
using Serilog;
using Serilog.Formatting.Compact;
using Prometheus;

var builder = Host.CreateApplicationBuilder(args);

// Configure Serilog for centralized logging (Worker Service)
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .Enrich.WithProperty("ServiceName", "ExecutionWorker")
    .Enrich.WithProperty("Environment", builder.Environment.EnvironmentName)
    .Enrich.WithMachineName()
    .WriteTo.Console(
        outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] [{ServiceName}] {Message:lj} {Properties:j}{NewLine}{Exception}")
    .WriteTo.File(
        formatter: new CompactJsonFormatter(),
        path: "logs/ExecutionWorker-.log",
        rollingInterval: RollingInterval.Day,
        retainedFileCountLimit: 30,
        fileSizeLimitBytes: 100_000_000,
        rollOnFileSizeLimit: true)
    .CreateLogger();

builder.Services.AddSerilog();


// Configure Redis connection
var redisConnectionString = builder.Configuration.GetConnectionString("Redis") 
    ?? "localhost:6379";
var redis = ConnectionMultiplexer.Connect(redisConnectionString);
builder.Services.AddSingleton<IConnectionMultiplexer>(redis);

// Configure Docker client
var dockerUri = builder.Configuration["Docker:Uri"] ?? "unix:///var/run/docker.sock";
var dockerClient = new DockerClientConfiguration(new Uri(dockerUri)).CreateClient();
builder.Services.AddSingleton(dockerClient);

// Register services
builder.Services.AddSingleton<IJobQueueService, RedisJobQueueService>();
builder.Services.AddSingleton<IProhibitedCodeScanner, ProhibitedCodeScanner>();
builder.Services.AddSingleton<IDockerContainerManager, DockerContainerManager>();

// Register the worker
builder.Services.AddHostedService<ExecutionWorker>();
builder.Services.AddHostedService<QueueMetricsCollector>();

// Start Prometheus metrics server on port 9090
// Commented out due to Windows permission issues - metrics can be accessed via ASP.NET Core endpoint instead
// var metricsServer = new MetricServer(port: 9090);
// metricsServer.Start();

var host = builder.Build();

try
{
    Log.Information("Starting Execution Worker Service");
    host.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Execution Worker Service terminated unexpectedly");
}
finally
{
    // metricsServer.Stop();
    Log.CloseAndFlush();
}
