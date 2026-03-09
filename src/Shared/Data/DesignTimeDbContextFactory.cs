using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Shared.Data;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        
        // Use connection string for design-time operations
        optionsBuilder.UseSqlServer("Server=localhost,1433;Database=aspnet_learning_platform;User Id=sa;Password=Platform@Pass123;TrustServerCertificate=True");

        return new ApplicationDbContext(optionsBuilder.Options);
    }
}
