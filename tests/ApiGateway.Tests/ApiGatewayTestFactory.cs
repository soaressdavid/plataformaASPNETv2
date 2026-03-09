using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
using System.Security.Cryptography;

namespace ApiGateway.Tests;

/// <summary>
/// Test factory for API Gateway integration tests.
/// Configures the test environment with mock services and test dependencies.
/// </summary>
public class ApiGatewayTestFactory : WebApplicationFactory<Program>
{
    private RSA? _rsa;
    
    public RSA Rsa => _rsa ?? throw new InvalidOperationException("RSA key not initialized. Ensure ConfigureWebHost has been called.");

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        // Generate RSA key pair for testing
        _rsa = RSA.Create(2048);
        var publicKey = new RsaSecurityKey(_rsa);

        builder.ConfigureAppConfiguration((context, config) =>
        {
            // Override configuration for testing
            config.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Jwt:PublicKeyPem"] = ExportPublicKeyToPem(_rsa),
                ["Redis:ConnectionString"] = "localhost:6379,abortConnect=false,connectTimeout=100,connectRetry=0,allowAdmin=false",
                ["CircuitBreaker:FailureThreshold"] = "5",
                ["CircuitBreaker:DurationOfBreakInSeconds"] = "30",
                
                // Configure YARP routes for testing
                ["ReverseProxy:Routes:auth-route:ClusterId"] = "auth-cluster",
                ["ReverseProxy:Routes:auth-route:Match:Path"] = "/api/auth/{**catch-all}",
                
                ["ReverseProxy:Routes:courses-route:ClusterId"] = "course-cluster",
                ["ReverseProxy:Routes:courses-route:Match:Path"] = "/api/courses/{**catch-all}",
                
                ["ReverseProxy:Routes:challenges-route:ClusterId"] = "challenge-cluster",
                ["ReverseProxy:Routes:challenges-route:Match:Path"] = "/api/challenges/{**catch-all}",
                
                ["ReverseProxy:Routes:progress-route:ClusterId"] = "progress-cluster",
                ["ReverseProxy:Routes:progress-route:Match:Path"] = "/api/progress/{**catch-all}",
                
                ["ReverseProxy:Routes:code-execute-route:ClusterId"] = "execution-cluster",
                ["ReverseProxy:Routes:code-execute-route:Match:Path"] = "/api/code/execute/{**catch-all}",
                
                ["ReverseProxy:Routes:code-review-route:ClusterId"] = "ai-tutor-cluster",
                ["ReverseProxy:Routes:code-review-route:Match:Path"] = "/api/code/review/{**catch-all}",
                
                // Configure clusters (pointing to non-existent services for testing)
                ["ReverseProxy:Clusters:auth-cluster:Destinations:destination1:Address"] = "http://localhost:9001",
                ["ReverseProxy:Clusters:course-cluster:Destinations:destination1:Address"] = "http://localhost:9002",
                ["ReverseProxy:Clusters:challenge-cluster:Destinations:destination1:Address"] = "http://localhost:9003",
                ["ReverseProxy:Clusters:progress-cluster:Destinations:destination1:Address"] = "http://localhost:9004",
                ["ReverseProxy:Clusters:ai-tutor-cluster:Destinations:destination1:Address"] = "http://localhost:9005",
                ["ReverseProxy:Clusters:execution-cluster:Destinations:destination1:Address"] = "http://localhost:9006"
            });
        });

        builder.ConfigureServices((context, services) =>
        {
            // Replace RSA key with test key
            services.Replace(ServiceDescriptor.Singleton(publicKey));

            // Replace Redis with mock - this will override the one from Program.cs
            services.Replace(ServiceDescriptor.Singleton<IConnectionMultiplexer>(CreateMockRedis()));
        });

        builder.UseEnvironment("Testing");
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _rsa?.Dispose();
        }
        base.Dispose(disposing);
    }

    private static string ExportPublicKeyToPem(RSA rsa)
    {
        var publicKey = rsa.ExportSubjectPublicKeyInfo();
        var base64 = Convert.ToBase64String(publicKey);
        
        var sb = new System.Text.StringBuilder();
        sb.AppendLine("-----BEGIN PUBLIC KEY-----");
        
        // Split into 64-character lines
        for (int i = 0; i < base64.Length; i += 64)
        {
            var length = Math.Min(64, base64.Length - i);
            sb.AppendLine(base64.Substring(i, length));
        }
        
        sb.AppendLine("-----END PUBLIC KEY-----");
        return sb.ToString();
    }

    private static IConnectionMultiplexer CreateMockRedis()
    {
        // Create a mock Redis connection for testing when Redis is not available
        var mockRedis = new Moq.Mock<IConnectionMultiplexer>();
        var mockDatabase = new Moq.Mock<IDatabase>();
        var mockServer = new Moq.Mock<IServer>();
        
        // Setup GetEndPoints to return an empty array (no endpoints)
        mockRedis
            .Setup(r => r.GetEndPoints(Moq.It.IsAny<bool>()))
            .Returns(Array.Empty<System.Net.EndPoint>());
        
        // Setup GetServer to return mock server (though it won't be called if no endpoints)
        mockRedis
            .Setup(r => r.GetServer(Moq.It.IsAny<System.Net.EndPoint>(), Moq.It.IsAny<object>()))
            .Returns(mockServer.Object);
        
        mockRedis
            .Setup(r => r.GetDatabase(Moq.It.IsAny<int>(), Moq.It.IsAny<object>()))
            .Returns(mockDatabase.Object);
        
        // Track token counts per key for rate limiting simulation
        var tokenCounts = new System.Collections.Concurrent.ConcurrentDictionary<string, double>();
        var lastRefillTimes = new System.Collections.Concurrent.ConcurrentDictionary<string, long>();
        
        // Setup mock database to simulate token bucket rate limiting
        mockDatabase
            .Setup(db => db.StringGetAsync(Moq.It.IsAny<RedisKey>(), Moq.It.IsAny<CommandFlags>()))
            .Returns<RedisKey, CommandFlags>((key, flags) =>
            {
                var keyStr = key.ToString();
                
                // Handle tokens key
                if (keyStr.EndsWith(":tokens"))
                {
                    var baseKey = keyStr.Substring(0, keyStr.Length - 7); // Remove ":tokens"
                    var tokens = tokenCounts.GetOrAdd(baseKey, 100.0); // Start with full bucket
                    return Task.FromResult((RedisValue)tokens);
                }
                
                // Handle lastRefill key
                if (keyStr.EndsWith(":lastRefill"))
                {
                    var baseKey = keyStr.Substring(0, keyStr.Length - 11); // Remove ":lastRefill"
                    var lastRefill = lastRefillTimes.GetOrAdd(baseKey, DateTimeOffset.UtcNow.ToUnixTimeSeconds());
                    return Task.FromResult((RedisValue)lastRefill);
                }
                
                return Task.FromResult(RedisValue.Null);
            });
        
        mockDatabase
            .Setup(db => db.StringSetAsync(Moq.It.IsAny<RedisKey>(), Moq.It.IsAny<RedisValue>(), Moq.It.IsAny<TimeSpan?>(), Moq.It.IsAny<When>(), Moq.It.IsAny<CommandFlags>()))
            .Returns<RedisKey, RedisValue, TimeSpan?, When, CommandFlags>((key, value, expiry, when, flags) =>
            {
                var keyStr = key.ToString();
                
                // Handle tokens key
                if (keyStr.EndsWith(":tokens"))
                {
                    var baseKey = keyStr.Substring(0, keyStr.Length - 7);
                    tokenCounts[baseKey] = (double)value;
                }
                
                // Handle lastRefill key
                if (keyStr.EndsWith(":lastRefill"))
                {
                    var baseKey = keyStr.Substring(0, keyStr.Length - 11);
                    lastRefillTimes[baseKey] = (long)value;
                }
                
                return Task.FromResult(true);
            });
        
        var mockTransaction = new Moq.Mock<ITransaction>();
        mockTransaction
            .Setup(t => t.ExecuteAsync(Moq.It.IsAny<CommandFlags>()))
            .Returns(Task.FromResult(true));
        
        mockTransaction
            .Setup(t => t.StringGetAsync(Moq.It.IsAny<RedisKey>(), Moq.It.IsAny<CommandFlags>()))
            .Returns<RedisKey, CommandFlags>((key, flags) =>
            {
                // Delegate to the main database mock
                return mockDatabase.Object.StringGetAsync(key, flags);
            });
        
        mockTransaction
            .Setup(t => t.StringSetAsync(Moq.It.IsAny<RedisKey>(), Moq.It.IsAny<RedisValue>(), Moq.It.IsAny<TimeSpan?>(), Moq.It.IsAny<When>(), Moq.It.IsAny<CommandFlags>()))
            .Returns<RedisKey, RedisValue, TimeSpan?, When, CommandFlags>((key, value, expiry, when, flags) =>
            {
                // Delegate to the main database mock
                return mockDatabase.Object.StringSetAsync(key, value, expiry, when, flags);
            });
        
        mockDatabase
            .Setup(db => db.CreateTransaction(Moq.It.IsAny<object>()))
            .Returns(mockTransaction.Object);
        
        return mockRedis.Object;
    }
}
