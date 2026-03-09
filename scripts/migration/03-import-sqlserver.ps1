#!/usr/bin/env pwsh
<#
.SYNOPSIS
    Imports transformed data into SQL Server
.DESCRIPTION
    Step 3 of the migration process: Imports data into SQL Server database
#>

param(
    [string]$ConfigFile = "migrate-config.json",
    [switch]$CreateBackup
)

$ErrorActionPreference = "Stop"

Write-Host "=== Step 3: Import to SQL Server ===" -ForegroundColor Cyan

# Load configuration
$config = Get-Content $ConfigFile -Raw | ConvertFrom-Json

$outputDir = $config.options.outputDirectory
$transformedFile = Join-Path $outputDir "data_transformed.sql"
$logFile = Join-Path $config.options.logDirectory "import_$(Get-Date -Format 'yyyyMMdd_HHmmss').log"

if (-not (Test-Path $transformedFile)) {
    Write-Host "Transformed data file not found: $transformedFile" -ForegroundColor Red
    Write-Host "Please run 02-transform-data.ps1 first" -ForegroundColor Yellow
    exit 1
}

# Create log directory
New-Item -ItemType Directory -Force -Path $config.options.logDirectory | Out-Null

Write-Host "Connecting to SQL Server..." -ForegroundColor Yellow
Write-Host "  Server: $($config.target.server)" -ForegroundColor Gray
Write-Host "  Database: $($config.target.database)" -ForegroundColor Gray

# Test connection
$version = sqlcmd -S $config.target.server -U $config.target.username -P $config.target.password -Q "SELECT @@VERSION" -h -1
if ($LASTEXITCODE -ne 0) {
    Write-Host "Failed to connect to SQL Server" -ForegroundColor Red
    exit 1
}
Write-Host "Connected successfully" -ForegroundColor Green

# Create backup if requested
if ($CreateBackup -or $config.options.backupBeforeMigration) {
    Write-Host "`nCreating database backup..." -ForegroundColor Yellow
    $timestamp = Get-Date -Format "yyyyMMdd_HHmmss"
    $backupPath = Join-Path $outputDir "backup_$timestamp.bak"
    
    $backupQuery = @"
BACKUP DATABASE [$($config.target.database)] 
TO DISK = N'$backupPath' 
WITH FORMAT, INIT, NAME = N'Pre-Migration Backup', SKIP, NOREWIND, NOUNLOAD, STATS = 10
"@
    
    try {
        sqlcmd -S $config.target.server -U $config.target.username -P $config.target.password -Q $backupQuery
        Write-Host "Backup created: $backupPath" -ForegroundColor Green
    }
    catch {
        Write-Host "Warning: Failed to create backup: $_" -ForegroundColor Yellow
        Write-Host "Continuing without backup..." -ForegroundColor Yellow
    }
}

# Get current row counts before import
Write-Host "`nGetting current row counts..." -ForegroundColor Yellow
$beforeCounts = @{}
$tables = sqlcmd -S $config.target.server -U $config.target.username -P $config.target.password -d $config.target.database -Q "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE' ORDER BY TABLE_NAME" -h -1

foreach ($table in $tables) {
    $table = $table.Trim()
    if ([string]::IsNullOrWhiteSpace($table)) { continue }
    
    $count = sqlcmd -S $config.target.server -U $config.target.username -P $config.target.password -d $config.target.database -Q "SELECT COUNT(*) FROM [$table]" -h -1
    $beforeCounts[$table] = [int]$count.Trim()
    Write-Host "  $table : $($beforeCounts[$table]) rows" -ForegroundColor Gray
}

# Import data
Write-Host "`nImporting data (this may take several minutes)..." -ForegroundColor Yellow
Write-Host "  Log file: $logFile" -ForegroundColor Gray

$startTime = Get-Date

try {
    # Execute the import script
    sqlcmd -S $config.target.server -U $config.target.username -P $config.target.password -d $config.target.database -i $transformedFile -o $logFile -b
    
    if ($LASTEXITCODE -ne 0) {
        Write-Host "Import failed! Check log file for details: $logFile" -ForegroundColor Red
        
        # Show last 20 lines of log
        Write-Host "`nLast 20 lines of log:" -ForegroundColor Yellow
        Get-Content $logFile -Tail 20 | ForEach-Object { Write-Host "  $_" -ForegroundColor Gray }
        
        exit 1
    }
}
catch {
    Write-Host "Import failed: $_" -ForegroundColor Red
    exit 1
}

$endTime = Get-Date
$duration = $endTime - $startTime

Write-Host "Import completed in $($duration.TotalSeconds) seconds" -ForegroundColor Green

# Get row counts after import
Write-Host "`nGetting new row counts..." -ForegroundColor Yellow
$afterCounts = @{}

foreach ($table in $tables) {
    $table = $table.Trim()
    if ([string]::IsNullOrWhiteSpace($table)) { continue }
    
    $count = sqlcmd -S $config.target.server -U $config.target.username -P $config.target.password -d $config.target.database -Q "SELECT COUNT(*) FROM [$table]" -h -1
    $afterCounts[$table] = [int]$count.Trim()
    
    $before = $beforeCounts[$table]
    $after = $afterCounts[$table]
    $diff = $after - $before
    
    if ($diff -gt 0) {
        Write-Host "  $table : $before → $after (+$diff rows)" -ForegroundColor Green
    }
    elseif ($diff -eq 0) {
        Write-Host "  $table : $after rows (no change)" -ForegroundColor Gray
    }
    else {
        Write-Host "  $table : $before → $after ($diff rows)" -ForegroundColor Yellow
    }
}

# Calculate total rows imported
$totalImported = ($afterCounts.Values | Measure-Object -Sum).Sum - ($beforeCounts.Values | Measure-Object -Sum).Sum

Write-Host "`nImport Summary:" -ForegroundColor Cyan
Write-Host "  Duration: $($duration.ToString('mm\:ss'))" -ForegroundColor Gray
Write-Host "  Total rows imported: $totalImported" -ForegroundColor Gray
Write-Host "  Average speed: $([math]::Round($totalImported / $duration.TotalSeconds, 2)) rows/sec" -ForegroundColor Gray
Write-Host "  Log file: $logFile" -ForegroundColor Gray

Write-Host "`n✓ Import completed successfully!" -ForegroundColor Green
Write-Host "Next step: Run 04-verify-migration.ps1" -ForegroundColor Yellow
