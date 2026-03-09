#!/usr/bin/env pwsh
<#
.SYNOPSIS
    Rolls back the migration by restoring from backup
.DESCRIPTION
    Restores the SQL Server database from the backup created before migration
#>

param(
    [string]$ConfigFile = "migrate-config.json",
    [string]$BackupFile
)

$ErrorActionPreference = "Stop"

Write-Host "=== Rollback Migration ===" -ForegroundColor Red

# Load configuration
$config = Get-Content $ConfigFile -Raw | ConvertFrom-Json

# Find backup file if not specified
if (-not $BackupFile) {
    $outputDir = $config.options.outputDirectory
    $backups = Get-ChildItem -Path $outputDir -Filter "backup_*.bak" | Sort-Object LastWriteTime -Descending
    
    if ($backups.Count -eq 0) {
        Write-Host "No backup files found in $outputDir" -ForegroundColor Red
        exit 1
    }
    
    $BackupFile = $backups[0].FullName
    Write-Host "Using most recent backup: $BackupFile" -ForegroundColor Yellow
}

if (-not (Test-Path $BackupFile)) {
    Write-Host "Backup file not found: $BackupFile" -ForegroundColor Red
    exit 1
}

Write-Host "`nBackup file: $BackupFile" -ForegroundColor Gray
$backupSize = (Get-Item $BackupFile).Length / 1MB
Write-Host "Backup size: $([math]::Round($backupSize, 2)) MB" -ForegroundColor Gray

# Confirm rollback
Write-Host "`n⚠️  WARNING: This will restore the database to its state before migration." -ForegroundColor Yellow
Write-Host "All data imported during migration will be lost." -ForegroundColor Yellow
$confirm = Read-Host "`nAre you sure you want to continue? (yes/no)"

if ($confirm -ne "yes") {
    Write-Host "Rollback cancelled" -ForegroundColor Yellow
    exit 0
}

Write-Host "`nConnecting to SQL Server..." -ForegroundColor Yellow
Write-Host "  Server: $($config.target.server)" -ForegroundColor Gray
Write-Host "  Database: $($config.target.database)" -ForegroundColor Gray

# Kill all connections to the database
Write-Host "`nKilling active connections..." -ForegroundColor Yellow
$killQuery = @"
USE master;
ALTER DATABASE [$($config.target.database)] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
"@

sqlcmd -S $config.target.server -U $config.target.username -P $config.target.password -Q $killQuery

if ($LASTEXITCODE -ne 0) {
    Write-Host "Failed to set database to single user mode" -ForegroundColor Red
    exit 1
}

# Restore database
Write-Host "`nRestoring database (this may take several minutes)..." -ForegroundColor Yellow

$restoreQuery = @"
USE master;
RESTORE DATABASE [$($config.target.database)] 
FROM DISK = N'$BackupFile' 
WITH REPLACE, STATS = 10;

ALTER DATABASE [$($config.target.database)] SET MULTI_USER;
"@

$startTime = Get-Date

sqlcmd -S $config.target.server -U $config.target.username -P $config.target.password -Q $restoreQuery

if ($LASTEXITCODE -ne 0) {
    Write-Host "Failed to restore database" -ForegroundColor Red
    
    # Try to set back to multi-user mode
    $multiUserQuery = "USE master; ALTER DATABASE [$($config.target.database)] SET MULTI_USER;"
    sqlcmd -S $config.target.server -U $config.target.username -P $config.target.password -Q $multiUserQuery
    
    exit 1
}

$endTime = Get-Date
$duration = $endTime - $startTime

Write-Host "`n✓ Database restored successfully!" -ForegroundColor Green
Write-Host "  Duration: $($duration.ToString('mm\:ss'))" -ForegroundColor Gray

# Verify restoration
Write-Host "`nVerifying restoration..." -ForegroundColor Yellow
$version = sqlcmd -S $config.target.server -U $config.target.username -P $config.target.password -d $config.target.database -Q "SELECT @@VERSION" -h -1

if ($LASTEXITCODE -eq 0) {
    Write-Host "✓ Database is accessible" -ForegroundColor Green
}
else {
    Write-Host "✗ Database verification failed" -ForegroundColor Red
    exit 1
}

Write-Host "`n✓ Rollback completed successfully!" -ForegroundColor Green
Write-Host "`nThe database has been restored to its state before migration." -ForegroundColor Gray
