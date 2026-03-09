# SQL Server Database Schema

This directory contains the database schema configuration for the platform using SQL Server with Entity Framework Core.

## Overview

The database schema implements:
- **Audit Fields**: All entities inherit from `BaseEntity` with `CreatedAt`, `UpdatedAt`, and `IsDeleted` fields
- **Soft Delete**: Global query filters automatically exclude soft-deleted records
- **Performance Indexes**: Optimized indexes on key tables for query performance
- **Read Replicas**: Support for read replica configuration for query distribution

## Base Entity

All entities inherit from `BaseEntity` which provides:

```csharp
public abstract class BaseEntity
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }  // Automatically set on creation
    public DateTime UpdatedAt { get; set; }  // Automatically updated on modification
    public bool IsDeleted { get; set; }      // Soft delete flag
}
```

## Audit Field Management

The `ApplicationDbContext` automatically manages audit fields:
- `CreatedAt` is set when an entity is first added
- `UpdatedAt` is updated every time an entity is modified
- Both are set in UTC timezone

## Soft Delete

Soft delete is implemented using:
1. `IsDeleted` field on all entities
2. Global query filter that excludes deleted records from all queries
3. To include deleted records, use `.IgnoreQueryFilters()` in LINQ queries

Example:
```csharp
// Normal query - excludes deleted records
var activeUsers = await context.Users.ToListAsync();

// Include deleted records
var allUsers = await context.Users.IgnoreQueryFilters().ToListAsync();

// Soft delete a record
user.IsDeleted = true;
await context.SaveChangesAsync();
```

## Performance Indexes

### Users Table
- `IX_Users_Email` - Unique index on Email (filtered for non-deleted)
- `IX_Users_Name` - Index on Name (filtered for non-deleted)
- `IX_Users_CreatedAt` - Index on CreatedAt for time-based queries

### Lessons Table
- `IX_Lessons_CourseId_OrderIndex` - Composite index for course lesson ordering
- `IX_Lessons_CreatedAt` - Index on CreatedAt (filtered for non-deleted)

### Challenges Table
- `IX_Challenges_Difficulty` - Index on Difficulty (filtered for non-deleted)

### Submissions Table
- `IX_Submissions_UserId_ChallengeId` - Composite index for user submissions
- `IX_Submissions_CreatedAt` - Index on CreatedAt for time-based queries

### Progress Table
- `IX_Progresses_UserId` - Unique index on UserId
- `IX_Progresses_UserId_CreatedAt` - Composite index for user progress tracking

## SQL Server Configuration

### Basic Configuration

```csharp
services.AddSqlServerDbContext(configuration);
```

This configures:
- Connection string from `appsettings.json`
- Retry on failure (5 retries, 30s max delay)
- Command timeout (30 seconds)
- Query splitting for better performance with collections

### Configuration with Read Replicas

```csharp
services.AddSqlServerDbContextWithReadReplicas(configuration);
```

This configures:
- Primary connection for write operations
- Read replica connections for read operations (round-robin)
- Automatic retry on transient failures

### appsettings.json Configuration

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=PlatformDB;User Id=sa;Password=YourPassword;TrustServerCertificate=True;",
    "ReadReplicas": [
      "Server=read-replica-1;Database=PlatformDB;User Id=sa;Password=YourPassword;TrustServerCertificate=True;",
      "Server=read-replica-2;Database=PlatformDB;User Id=sa;Password=YourPassword;TrustServerCertificate=True;"
    ]
  },
  "Logging": {
    "EnableSensitiveDataLogging": false,
    "EnableDetailedErrors": false
  }
}
```

## Migrations

### Create a New Migration

```bash
dotnet ef migrations add MigrationName --project src/Shared/Shared.csproj --context ApplicationDbContext
```

### Apply Migrations

```bash
dotnet ef database update --project src/Shared/Shared.csproj --context ApplicationDbContext
```

### Remove Last Migration

```bash
dotnet ef migrations remove --project src/Shared/Shared.csproj --context ApplicationDbContext
```

## Read Replica Usage

When read replicas are configured, use the `IReadReplicaConnectionFactory` to get read replica connections:

```csharp
public class MyService
{
    private readonly IReadReplicaConnectionFactory _readReplicaFactory;
    
    public MyService(IReadReplicaConnectionFactory readReplicaFactory)
    {
        _readReplicaFactory = readReplicaFactory;
    }
    
    public async Task<List<User>> GetUsersAsync()
    {
        // For read operations, you can use read replicas
        var connectionString = _readReplicaFactory.GetReadReplicaConnectionString();
        
        // Create a new context with read replica connection
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        optionsBuilder.UseSqlServer(connectionString);
        
        using var context = new ApplicationDbContext(optionsBuilder.Options);
        return await context.Users.ToListAsync();
    }
}
```

## Entity Relationships

The schema includes the following main entities:

- **User**: Platform users with authentication
- **Course**: Educational courses organized by curriculum levels
- **Lesson**: Individual lessons within courses
- **Challenge**: Coding challenges with test cases
- **Submission**: User code submissions for challenges
- **Progress**: User progress tracking
- **Project**: Guided projects for hands-on learning
- **CurriculumLevel**: Hierarchical organization of courses

## Best Practices

1. **Always use soft delete**: Set `IsDeleted = true` instead of removing records
2. **Use indexes wisely**: The schema includes optimized indexes for common queries
3. **Leverage read replicas**: Use read replicas for read-heavy operations
4. **Monitor query performance**: Use Application Insights to track slow queries
5. **Keep migrations organized**: Create descriptive migration names
6. **Test migrations**: Always test migrations in a development environment first

## Troubleshooting

### Connection Issues

If you encounter connection issues:
1. Verify SQL Server is running
2. Check connection string in `appsettings.json`
3. Ensure firewall allows SQL Server connections
4. Verify SQL Server authentication mode

### Migration Issues

If migrations fail:
1. Check for pending migrations: `dotnet ef migrations list`
2. Verify database connection
3. Check for conflicting schema changes
4. Review migration code for errors

### Performance Issues

If queries are slow:
1. Check if indexes are being used (SQL Server Execution Plan)
2. Verify read replicas are configured correctly
3. Monitor query execution times in Application Insights
4. Consider adding additional indexes for specific query patterns
