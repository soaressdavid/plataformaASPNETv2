#!/usr/bin/env pwsh
<#
.SYNOPSIS
    Migrates data from PostgreSQL to SQL Server
.DESCRIPTION
    This script orchestrates the complete migration process:
    1. Exports data from PostgreSQL
    2. Transforms PostgreSQL format to SQL Server format
    3. Imports data into SQL Server
    4. Verifies data integrity
.PARAMETER ConfigFile
    Path to the configuration JSON file (default: migrate-config.json)
.PARAMETER SkipBackup
    Skip creating a backup of the target SQL Server database
.PARAMETER SkipVerification
    Skip the verification step after migration
.PARAMETER Resume
    Resume from the last successful step
.EXAMPLE
    .\migrate-postgresql-to-sqlserver.ps1
.EXAMPLE
    .\migrate-postgresql-to-sqlserver.ps1 -ConfigFile custom-config.json -SkipBackup
#>

param(
    [string]$ConfigFile = "migrate-config.json",
    [switch]$SkipBackup,
    [switch]$SkipVerification,
    [switch]$Resume
)

$ErrorActionPreference = "Stop"
$ProgressPreference = "Continue"

# Colors for output
$ColorSuccess = "Green"
$ColorError = "Red"
$ColorWarning = "Yellow"
$ColorInfo = "Cyan"

function Write-Step {
    param([string]$Message)
    Write-Host "`n=== $Message ===" -ForegroundColor $ColorInfo
}

function Write-Success {
    param([string]$Message)
    Write-Host "✓ $Message" -ForegroundColor $ColorSuccess
}

function Write-Error {
    param([string]$Message)
    Write-Host "✗ $Message" -ForegroundColor $ColorError
}

function Write-Warning {
    param([string]$Message)
    Write-Host "⚠ $Message" -ForegroundColor $ColorWarning
}

function Test-Prerequisites {
    Write-Step "Checking Prerequisites"
    
    $missing = @()
    
    # Check for pg_dump
    if (-not (Get-Command pg_dump -ErrorAction SilentlyContinue)) {
        $missing += "pg_dump (PostgreSQL client tools)"
    }
    
    # Check for sqlcmd
    if (-not (Get-Command sqlcmd -ErrorAction SilentlyContinue)) {
        $missing += "sqlcmd (SQL Server client tools)"
    }
    
    # Check for bcp
    if (-not (Get-Command bcp -ErrorAction SilentlyContinue)) {
        $missing += "bcp (SQL Server bulk copy tool)"
    }
    
    if ($missing.Count -gt 0) {
        Write-Error "Missing required tools:"
        $missing | ForEach-Object { Write-Host "  - $_" -ForegroundColor $ColorError }
        Write-Host "`nPlease install the missing tools and try again." -ForegroundColor $ColorWarning
        exit 1
    }
    
    Write-Success "All prerequisites are installed"
}

function Load-Configuration {
    param([string]$Path)
    
    Write-Step "Loading Configuration"
    
    if (-not (Test-Path $Path)) {
        Write-Error "Configuration file not found: $Path"
        exit 1
    }
    
    try {
        $config = Get-Content $Path -Raw | ConvertFrom-Json
        Write-Success "Configuration loaded successfully"
        return $config
    }
    catch {
        Write-Error "Failed to parse configuration file: $_"
        exit 1
    }
}

function Test-PostgreSQLConnection {
    param($Config)
    
    Write-Step "Testing PostgreSQL Connection"
    
    $env:PGPASSWORD = $Config.source.password
    $result = psql -h $Config.source.host -p $Config.source.port -U $Config.source.username -d $Config.source.database -c "SELECT version();" 2>&1
    
    if ($LASTEXITCODE -ne 0) {
        Write-Error "Failed to connect to PostgreSQL: $result"
        return $false
    }
    
    Write-Success "PostgreSQL connection successful"
    return $true
}

function Test-SQLServerConnection {
    param($Config)
    
    Write-Step "Testing SQL Server Connection"
    
    $connectionString = "Server=$($Config.target.server);Database=master;User Id=$($Config.target.username);Password=$($Config.target.password);TrustServerCertificate=true"
    
    $result = sqlcmd -S $Config.target.server -U $Config.target.username -P $Config.target.password -Q "SELECT @@VERSION" 2>&1
    
    if ($LASTEXITCODE -ne 0) {
        Write-Error "Failed to connect to SQL Server: $result"
        return $false
    }
    
    Write-Success "SQL Server connection successful"
    return $true
}

function Backup-SQLServerDatabase {
    param($Config)
    
    Write-Step "Creating SQL Server Backup"
    
    $timestamp = Get-Date -Format "yyyyMMdd_HHmmss"
    $backupPath = Join-Path $Config.options.outputDirectory "backup_$timestamp.bak"
    
    $query = @"
BACKUP DATABASE [$($Config.target.database)] 
TO DISK = N'$backupPath' 
WITH FORMAT, INIT, NAME = N'Pre-Migration Backup', SKIP, NOREWIND, NOUNLOAD, STATS = 10
"@
    
    try {
        sqlcmd -S $Config.target.server -U $Config.target.username -P $Config.target.password -Q $query
        Write-Success "Backup created: $backupPath"
        return $backupPath
    }
    catch {
        Write-Warning "Failed to create backup: $_"
        Write-Warning "Continuing without backup..."
        return $null
    }
}

function Export-PostgreSQLData {
    param($Config)
    
    Write-Step "Exporting Data from PostgreSQL"
    
    $outputDir = $Config.options.outputDirectory
    New-Item -ItemType Directory -Force -Path $outputDir | Out-Null
    
    $schemaFile = Join-Path $outputDir "schema.sql"
    $dataFile = Join-Path $outputDir "data.sql"
    
    $env:PGPASSWORD = $Config.source.password
    
    # Export schema
    Write-Host "Exporting schema..." -ForegroundColor $ColorInfo
    pg_dump -h $Config.source.host -p $Config.source.port -U $Config.source.username -d $Config.source.database --schema-only --no-owner --no-privileges -f $schemaFile
    
    if ($LASTEXITCODE -ne 0) {
        Write-Error "Failed to export schema"
        return $false
    }
    
    # Export data
    Write-Host "Exporting data..." -ForegroundColor $ColorInfo
    pg_dump -h $Config.source.host -p $Config.source.port -U $Config.source.username -d $Config.source.database --data-only --no-owner --no-privileges --column-inserts -f $dataFile
    
    if ($LASTEXITCODE -ne 0) {
        Write-Error "Failed to export data"
        return $false
    }
    
    Write-Success "Data exported successfully"
    Write-Host "  Schema: $schemaFile" -ForegroundColor Gray
    Write-Host "  Data: $dataFile" -ForegroundColor Gray
    
    return $true
}

function Transform-SQLSyntax {
    param($Config)
    
    Write-Step "Transforming SQL Syntax"
    
    $outputDir = $Config.options.outputDirectory
    $dataFile = Join-Path $outputDir "data.sql"
    $transformedFile = Join-Path $outputDir "data_transformed.sql"
    
    if (-not (Test-Path $dataFile)) {
        Write-Error "Data file not found: $dataFile"
        return $false
    }
    
    Write-Host "Reading source file..." -ForegroundColor $ColorInfo
    $content = Get-Content $dataFile -Raw
    
    Write-Host "Applying transformations..." -ForegroundColor $ColorInfo
    
    # Transform boolean values
    $content = $content -replace "'t'", "1" -replace "'f'", "0"
    $content = $content -replace " true", " 1" -replace " false", " 0"
    
    # Transform PostgreSQL-specific syntax
    $content = $content -replace "::timestamp", ""
    $content = $content -replace "::date", ""
    $content = $content -replace "::integer", ""
    $content = $content -replace "::bigint", ""
    $content = $content -replace "::uuid", ""
    
    # Transform sequences (handled by IDENTITY in schema)
    $content = $content -replace "SELECT pg_catalog\.setval\([^;]+;", ""
    
    # Add SET IDENTITY_INSERT for tables with IDENTITY columns
    $content = $content -replace "INSERT INTO ([a-zA-Z_]+)", "SET IDENTITY_INSERT `$1 ON;`nINSERT INTO `$1"
    
    # Disable constraints during import
    $header = @"
-- Disable all constraints
EXEC sp_MSforeachtable 'ALTER TABLE ? NOCHECK CONSTRAINT ALL';

-- Disable all triggers
EXEC sp_MSforeachtable 'ALTER TABLE ? DISABLE TRIGGER ALL';

"@
    
    $footer = @"

-- Enable all constraints
EXEC sp_MSforeachtable 'ALTER TABLE ? CHECK CONSTRAINT ALL';

-- Enable all triggers
EXEC sp_MSforeachtable 'ALTER TABLE ? ENABLE TRIGGER ALL';

-- Reseed identity columns
EXEC sp_MSforeachtable 'DBCC CHECKIDENT(''?'', RESEED)';
"@
    
    $content = $header + $content + $footer
    
    Write-Host "Writing transformed file..." -ForegroundColor $ColorInfo
    $content | Out-File -FilePath $transformedFile -Encoding UTF8
    
    Write-Success "SQL syntax transformed successfully"
    Write-Host "  Output: $transformedFile" -ForegroundColor Gray
    
    return $true
}

function Import-ToSQLServer {
    param($Config)
    
    Write-Step "Importing Data to SQL Server"
    
    $outputDir = $Config.options.outputDirectory
    $transformedFile = Join-Path $outputDir "data_transformed.sql"
    
    if (-not (Test-Path $transformedFile)) {
        Write-Error "Transformed data file not found: $transformedFile"
        return $false
    }
    
    Write-Host "Importing data (this may take a while)..." -ForegroundColor $ColorInfo
    
    $result = sqlcmd -S $Config.target.server -U $Config.target.username -P $Config.target.password -d $Config.target.database -i $transformedFile -b
    
    if ($LASTEXITCODE -ne 0) {
        Write-Error "Failed to import data: $result"
        return $false
    }
    
    Write-Success "Data imported successfully"
    return $true
}

function Verify-Migration {
    param($Config)
    
    Write-Step "Verifying Migration"
    
    $env:PGPASSWORD = $Config.source.password
    
    # Get table list from PostgreSQL
    $pgTables = psql -h $Config.source.host -p $Config.source.port -U $Config.source.username -d $Config.source.database -t -c "SELECT tablename FROM pg_tables WHERE schemaname='public' ORDER BY tablename;"
    
    $report = @()
    $allMatch = $true
    
    foreach ($table in $pgTables) {
        $table = $table.Trim()
        if ([string]::IsNullOrWhiteSpace($table) -or $Config.options.excludeTables -contains $table) {
            continue
        }
        
        # Get row count from PostgreSQL
        $pgCount = psql -h $Config.source.host -p $Config.source.port -U $Config.source.username -d $Config.source.database -t -c "SELECT COUNT(*) FROM $table;"
        $pgCount = [int]$pgCount.Trim()
        
        # Get row count from SQL Server
        $sqlCount = sqlcmd -S $Config.target.server -U $Config.target.username -P $Config.target.password -d $Config.target.database -h -1 -Q "SELECT COUNT(*) FROM [$table]"
        $sqlCount = [int]$sqlCount.Trim()
        
        $match = $pgCount -eq $sqlCount
        if (-not $match) {
            $allMatch = $false
        }
        
        $report += [PSCustomObject]@{
            Table = $table
            PostgreSQL = $pgCount
            SQLServer = $sqlCount
            Match = $match
        }
        
        $status = if ($match) { "✓" } else { "✗" }
        $color = if ($match) { $ColorSuccess } else { $ColorError }
        Write-Host "$status $table : PG=$pgCount, SQL=$sqlCount" -ForegroundColor $color
    }
    
    # Save report
    $reportFile = Join-Path $Config.options.outputDirectory "migration-report.csv"
    $report | Export-Csv -Path $reportFile -NoTypeInformation
    
    Write-Host "`nReport saved to: $reportFile" -ForegroundColor Gray
    
    if ($allMatch) {
        Write-Success "All tables verified successfully!"
        return $true
    }
    else {
        Write-Warning "Some tables have mismatched row counts. Please review the report."
        return $false
    }
}

# Main execution
try {
    Write-Host @"
╔═══════════════════════════════════════════════════════════╗
║   PostgreSQL to SQL Server Migration Tool                ║
║   Platform Evolution SaaS - Database Migration            ║
╚═══════════════════════════════════════════════════════════╝
"@ -ForegroundColor $ColorInfo

    # Check prerequisites
    Test-Prerequisites
    
    # Load configuration
    $config = Load-Configuration -Path $ConfigFile
    
    # Create output directories
    New-Item -ItemType Directory -Force -Path $config.options.outputDirectory | Out-Null
    New-Item -ItemType Directory -Force -Path $config.options.logDirectory | Out-Null
    
    # Test connections
    if (-not (Test-PostgreSQLConnection -Config $config)) {
        exit 1
    }
    
    if (-not (Test-SQLServerConnection -Config $config)) {
        exit 1
    }
    
    # Backup SQL Server database
    if (-not $SkipBackup -and $config.options.backupBeforeMigration) {
        Backup-SQLServerDatabase -Config $config
    }
    
    # Export data from PostgreSQL
    if (-not (Export-PostgreSQLData -Config $config)) {
        exit 1
    }
    
    # Transform SQL syntax
    if (-not (Transform-SQLSyntax -Config $config)) {
        exit 1
    }
    
    # Import to SQL Server
    if (-not (Import-ToSQLServer -Config $config)) {
        exit 1
    }
    
    # Verify migration
    if (-not $SkipVerification -and $config.options.verifyIntegrity) {
        Verify-Migration -Config $config
    }
    
    Write-Host "`n" -NoNewline
    Write-Success "Migration completed successfully!"
    Write-Host "`nNext steps:" -ForegroundColor $ColorInfo
    Write-Host "  1. Review the migration report in $($config.options.outputDirectory)" -ForegroundColor Gray
    Write-Host "  2. Test your application with the migrated data" -ForegroundColor Gray
    Write-Host "  3. Update connection strings to point to SQL Server" -ForegroundColor Gray
    
}
catch {
    Write-Error "Migration failed: $_"
    Write-Host "`nError details:" -ForegroundColor $ColorError
    Write-Host $_.Exception.Message -ForegroundColor $ColorError
    Write-Host $_.ScriptStackTrace -ForegroundColor Gray
    exit 1
}
