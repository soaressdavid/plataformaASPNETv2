using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shared.Data;

namespace Course.Tests;

/// <summary>
/// Custom WebApplicationFactory for Course.Service integration tests
/// Replaces SQL Server with InMemory database by setting environment to Test
/// </summary>
public class TestWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly string _databaseName;
    private bool _isSeeded = false;
    private readonly object _seedLock = new object();

    public TestWebApplicationFactory()
    {
        _databaseName = $"TestDb_{Guid.NewGuid()}";
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        // Set environment to Test
        builder.UseEnvironment("Test");
        
        builder.ConfigureServices(services =>
        {
            // Add InMemory database for testing
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseInMemoryDatabase(_databaseName);
            });
        });
    }

    public void EnsureSeeded(Action<ApplicationDbContext> seedAction)
    {
        lock (_seedLock)
        {
            if (_isSeeded) return;

            using var scope = Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            
            seedAction(context);
            context.SaveChanges();

            _isSeeded = true;
        }
    }

    public IServiceScope CreateScope()
    {
        return Services.CreateScope();
    }
}
