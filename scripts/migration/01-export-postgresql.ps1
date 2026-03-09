#!/usr/bin/env pwsh
<#
.SYNOPSIS
    Exports data from PostgreSQL database
.DESCRIPTION
    Step 1 of the migration process: Exports schema and data from PostgreSQL
#>

param(
    [string]$ConfigFile = "migrate-config.json"
)

$ErrorActionPreference = "Stop"

Write-Host "=== Step 1: Export PostgreSQL Data ===" -ForegroundColor Cyan

# Load configuration
$config = Get-Content $ConfigFile -Raw | ConvertFrom-Json

# Create output directory
$outputDir = $config.options.outputDirectory
New-Item -ItemType Directory -Force -Path $outputDir | Out-Null

$schemaFile = Join-Path $outputDir "schema.sql"
$dataFile = Join-Path $outputDir "data.sql"
$tableListFile = Join-Path $outputDir "tables.txt"

# Set PostgreSQL password
$env:PGPASSWORD = $config.source.password

Write-Host "Connecting to PostgreSQL..." -ForegroundColor Yellow
Write-Host "  Host: $($config.source.host):$($config.source.port)" -ForegroundColor Gray
Write-Host "  Database: $($config.source.database)" -ForegroundColor Gray

# Test connection
$version = psql -h $config.source.host -p $config.source.port -U $config.source.username -d $config.source.database -t -c "SELECT version();"
if ($LASTEXITCODE -ne 0) {
    Write-Host "Failed to connect to PostgreSQL" -ForegroundColor Red
    exit 1
}
Write-Host "Connected successfully" -ForegroundColor Green
Write-Host "  Version: $($version.Trim())" -ForegroundColor Gray

# Get table list
Write-Host "`nGetting table list..." -ForegroundColor Yellow
$tables = psql -h $config.source.host -p $config.source.port -U $config.source.username -d $config.source.database -t -c "SELECT tablename FROM pg_tables WHERE schemaname='public' ORDER BY tablename;"
$tables | Out-File -FilePath $tableListFile -Encoding UTF8
Write-Host "Found $($tables.Count) tables" -ForegroundColor Green

# Get row counts
Write-Host "`nTable row counts:" -ForegroundColor Yellow
foreach ($table in $tables) {
    $table = $table.Trim()
    if ([string]::IsNullOrWhiteSpace($table)) { continue }
    
    $count = psql -h $config.source.host -p $config.source.port -U $config.source.username -d $config.source.database -t -c "SELECT COUNT(*) FROM $table;"
    Write-Host "  $table : $($count.Trim()) rows" -ForegroundColor Gray
}

# Export schema
Write-Host "`nExporting schema..." -ForegroundColor Yellow
pg_dump -h $config.source.host -p $config.source.port -U $config.source.username -d $config.source.database --schema-only --no-owner --no-privileges -f $schemaFile

if ($LASTEXITCODE -ne 0) {
    Write-Host "Failed to export schema" -ForegroundColor Red
    exit 1
}
Write-Host "Schema exported: $schemaFile" -ForegroundColor Green

# Export data
Write-Host "`nExporting data (this may take a while)..." -ForegroundColor Yellow
pg_dump -h $config.source.host -p $config.source.port -U $config.source.username -d $config.source.database --data-only --no-owner --no-privileges --column-inserts -f $dataFile

if ($LASTEXITCODE -ne 0) {
    Write-Host "Failed to export data" -ForegroundColor Red
    exit 1
}
Write-Host "Data exported: $dataFile" -ForegroundColor Green

# Get file sizes
$schemaSize = (Get-Item $schemaFile).Length / 1KB
$dataSize = (Get-Item $dataFile).Length / 1KB

Write-Host "`nExport Summary:" -ForegroundColor Cyan
Write-Host "  Schema file: $schemaFile ($([math]::Round($schemaSize, 2)) KB)" -ForegroundColor Gray
Write-Host "  Data file: $dataFile ($([math]::Round($dataSize, 2)) KB)" -ForegroundColor Gray
Write-Host "  Table list: $tableListFile" -ForegroundColor Gray

Write-Host "`n✓ Export completed successfully!" -ForegroundColor Green
Write-Host "Next step: Run 02-transform-data.ps1" -ForegroundColor Yellow
