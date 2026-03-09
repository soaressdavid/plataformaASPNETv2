# PostgreSQL to SQL Server Migration Guide

This directory contains scripts and tools for migrating data from PostgreSQL to SQL Server.

## Overview

The migration process consists of 4 main steps:
1. **Export**: Extract data from PostgreSQL using pg_dump
2. **Transform**: Convert PostgreSQL-specific syntax to SQL Server format
3. **Import**: Load data into SQL Server using bulk insert operations
4. **Verify**: Validate data integrity and row counts

## Prerequisites

- PostgreSQL client tools (pg_dump, psql)
- SQL Server client tools (sqlcmd, bcp)
- PowerShell 7+ or Bash
- .NET 8 SDK (for the C# migration tool)

## Quick Start

### Option 1: Using PowerShell Script (Recommended)

```powershell
# Edit the configuration in migrate-config.json first
.\migrate-postgresql-to-sqlserver.ps1
```

### Option 2: Using C# Migration Tool

```powershell
cd MigrationTool
dotnet run -- --source "Host=localhost;Port=5432;Database=aspnet_learning_platform;Username=user;Password=pass" --target "Server=localhost,1433;Database=aspnet_learning_platform;User Id=sa;Password=P@ssw0rd!2026#SecurePlatform"
```

### Option 3: Manual Step-by-Step

```powershell
# 1. Export from PostgreSQL
.\01-export-postgresql.ps1

# 2. Transform data
.\02-transform-data.ps1

# 3. Import to SQL Server
.\03-import-sqlserver.ps1

# 4. Verify migration
.\04-verify-migration.ps1
```

## Configuration

Edit `migrate-config.json` to configure your source and target databases:

```json
{
  "source": {
    "host": "localhost",
    "port": 5432,
    "database": "aspnet_learning_platform",
    "username": "platform_user",
    "password": "your_password"
  },
  "target": {
    "server": "localhost,1433",
    "database": "aspnet_learning_platform",
    "username": "sa",
    "password": "P@ssw0rd!2026#SecurePlatform"
  },
  "options": {
    "backupBeforeMigration": true,
    "verifyIntegrity": true,
    "batchSize": 1000,
    "excludeTables": []
  }
}
```

## Migration Process Details

### 1. Export Phase
- Uses `pg_dump` to export schema and data
- Creates separate files for schema and data
- Exports in plain SQL format for easier transformation

### 2. Transform Phase
- Converts PostgreSQL data types to SQL Server equivalents
- Transforms sequences to IDENTITY columns
- Converts PostgreSQL-specific functions
- Handles boolean to bit conversion
- Adjusts timestamp formats

### 3. Import Phase
- Creates schema in SQL Server
- Uses bulk insert operations for performance
- Handles foreign key constraints properly
- Implements retry logic for transient errors

### 4. Verify Phase
- Compares row counts between source and target
- Validates data integrity with checksums
- Generates detailed migration report

## Data Type Mappings

| PostgreSQL | SQL Server |
|------------|------------|
| serial | INT IDENTITY |
| bigserial | BIGINT IDENTITY |
| boolean | BIT |
| text | NVARCHAR(MAX) |
| varchar(n) | NVARCHAR(n) |
| timestamp | DATETIME2 |
| timestamptz | DATETIMEOFFSET |
| uuid | UNIQUEIDENTIFIER |
| json/jsonb | NVARCHAR(MAX) |
| array | NVARCHAR(MAX) (JSON) |

## Troubleshooting

### PostgreSQL Connection Issues
```powershell
# Test PostgreSQL connection
psql -h localhost -p 5432 -U platform_user -d aspnet_learning_platform -c "SELECT version();"
```

### SQL Server Connection Issues
```powershell
# Test SQL Server connection
sqlcmd -S localhost,1433 -U sa -P "P@ssw0rd!2026#SecurePlatform" -Q "SELECT @@VERSION"
```

### Migration Failures
- Check logs in `migration-logs/` directory
- Review `migration-errors.log` for detailed error messages
- Use `--resume` flag to continue from last successful step

## Performance Tips

- Increase batch size for faster imports (default: 1000)
- Disable indexes before import, rebuild after
- Use `--parallel` flag for multi-threaded import
- Consider migrating large tables separately

## Rollback

If migration fails, you can rollback:

```powershell
.\rollback-migration.ps1
```

This will restore the SQL Server database from the backup created before migration.

## Support

For issues or questions, refer to:
- Migration logs: `migration-logs/`
- Error log: `migration-errors.log`
- Verification report: `migration-report.html`
