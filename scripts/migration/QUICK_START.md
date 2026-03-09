# Quick Start Guide: PostgreSQL to SQL Server Migration

## Prerequisites Check

Before starting, ensure you have:

- [ ] PostgreSQL client tools installed (`pg_dump`, `psql`)
- [ ] SQL Server client tools installed (`sqlcmd`, `bcp`)
- [ ] PowerShell 7+ or Bash
- [ ] .NET 8 SDK (for C# migration tool)
- [ ] Access to both PostgreSQL and SQL Server databases
- [ ] Sufficient disk space for backups and exports

## Installation

### Windows

```powershell
# Install PostgreSQL client tools
winget install PostgreSQL.PostgreSQL

# Install SQL Server command-line tools
winget install Microsoft.SQLServer.2022.CommandLineUtilities

# Install PowerShell 7+
winget install Microsoft.PowerShell
```

### Linux/macOS

```bash
# Install PostgreSQL client tools
sudo apt-get install postgresql-client  # Ubuntu/Debian
brew install postgresql                  # macOS

# Install SQL Server command-line tools
# Follow: https://learn.microsoft.com/en-us/sql/linux/sql-server-linux-setup-tools

# Install PowerShell
# Follow: https://learn.microsoft.com/en-us/powershell/scripting/install/installing-powershell
```

## Configuration

1. **Edit `migrate-config.json`** with your database details:

```json
{
  "source": {
    "host": "your-postgresql-host",
    "port": 5432,
    "database": "your_database",
    "username": "your_username",
    "password": "your_password"
  },
  "target": {
    "server": "your-sqlserver-host,1433",
    "database": "your_database",
    "username": "sa",
    "password": "your_password"
  }
}
```

2. **Test connections**:

```powershell
# Test PostgreSQL
psql -h localhost -p 5432 -U your_username -d your_database -c "SELECT version();"

# Test SQL Server
sqlcmd -S localhost,1433 -U sa -P "your_password" -Q "SELECT @@VERSION"
```

## Migration Methods

### Method 1: Automated (Recommended)

Run the all-in-one migration script:

```powershell
cd scripts/migration
.\migrate-postgresql-to-sqlserver.ps1
```

This will:
1. ✓ Test connections
2. ✓ Create backup
3. ✓ Export data from PostgreSQL
4. ✓ Transform SQL syntax
5. ✓ Import to SQL Server
6. ✓ Verify data integrity

### Method 2: Step-by-Step

For more control, run each step individually:

```powershell
# Step 1: Export from PostgreSQL
.\01-export-postgresql.ps1

# Step 2: Transform data
.\02-transform-data.ps1

# Step 3: Import to SQL Server
.\03-import-sqlserver.ps1

# Step 4: Verify migration
.\04-verify-migration.ps1
```

### Method 3: C# Migration Tool

For advanced scenarios with better performance:

```powershell
cd MigrationTool
dotnet run -- \
  --source "Host=localhost;Port=5432;Database=mydb;Username=user;Password=pass" \
  --target "Server=localhost,1433;Database=mydb;User Id=sa;Password=pass" \
  --batch-size 1000 \
  --parallel 4
```

## Common Scenarios

### Scenario 1: First-time Migration

```powershell
# Use default settings
.\migrate-postgresql-to-sqlserver.ps1
```

### Scenario 2: Large Database (>10GB)

```powershell
# Increase batch size and parallelism
cd MigrationTool
dotnet run -- \
  --source "..." \
  --target "..." \
  --batch-size 5000 \
  --parallel 8
```

### Scenario 3: Exclude Specific Tables

Edit `migrate-config.json`:

```json
{
  "options": {
    "excludeTables": [
      "__EFMigrationsHistory",
      "temp_table",
      "audit_log"
    ]
  }
}
```

### Scenario 4: Migration Failed - Rollback

```powershell
.\rollback-migration.ps1
```

## Verification

After migration, verify the data:

1. **Check row counts**:

```sql
-- PostgreSQL
SELECT tablename, n_live_tup FROM pg_stat_user_tables ORDER BY tablename;

-- SQL Server
SELECT t.name, SUM(p.rows) as rows
FROM sys.tables t
JOIN sys.partitions p ON t.object_id = p.object_id
WHERE p.index_id IN (0,1)
GROUP BY t.name
ORDER BY t.name;
```

2. **Review migration report**:

Open `migration-output/migration-report.html` in your browser.

3. **Test application**:

Update your application's connection string and run tests.

## Troubleshooting

### Issue: "pg_dump: command not found"

**Solution**: Install PostgreSQL client tools (see Installation section)

### Issue: "sqlcmd: command not found"

**Solution**: Install SQL Server command-line tools (see Installation section)

### Issue: Connection timeout

**Solution**: 
- Check firewall settings
- Verify database server is running
- Test connection with `psql` or `sqlcmd`

### Issue: Permission denied

**Solution**:
- Verify database credentials
- Ensure user has SELECT permission on PostgreSQL
- Ensure user has INSERT permission on SQL Server

### Issue: Data type conversion errors

**Solution**:
- Review `migration-logs/` for specific errors
- Check data type mappings in `migrate-config.json`
- Some PostgreSQL types may need manual conversion

### Issue: Row count mismatch

**Solution**:
- Check `migration-report.html` for details
- Review `migration-errors.log`
- Re-run specific table migration
- Check for data truncation or conversion issues

## Performance Tips

1. **Increase batch size** for faster imports:
   ```json
   "batchSize": 5000
   ```

2. **Use parallel table migration**:
   ```json
   "parallelTables": 8
   ```

3. **Disable indexes before import** (manual):
   ```sql
   -- Disable indexes
   ALTER INDEX ALL ON [TableName] DISABLE;
   
   -- After import
   ALTER INDEX ALL ON [TableName] REBUILD;
   ```

4. **Use C# migration tool** for better performance on large datasets

## Post-Migration Checklist

- [ ] Verify all row counts match
- [ ] Test application functionality
- [ ] Update connection strings in all environments
- [ ] Update documentation
- [ ] Train team on SQL Server differences
- [ ] Monitor performance
- [ ] Schedule regular backups
- [ ] Decommission PostgreSQL (after verification period)

## Support

For issues or questions:

1. Check `migration-logs/` directory
2. Review `migration-errors.log`
3. Check `migration-report.html`
4. Refer to main README.md

## Next Steps

After successful migration:

1. Update application connection strings
2. Test all application features
3. Monitor SQL Server performance
4. Set up regular backup schedule
5. Update deployment scripts
6. Update CI/CD pipelines

## Rollback Plan

If you need to rollback:

```powershell
# Restore from backup
.\rollback-migration.ps1

# Or specify backup file
.\rollback-migration.ps1 -BackupFile "path/to/backup.bak"
```

**Note**: Keep PostgreSQL database running for at least 30 days after migration as a safety measure.
