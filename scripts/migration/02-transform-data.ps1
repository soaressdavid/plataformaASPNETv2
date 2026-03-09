#!/usr/bin/env pwsh
<#
.SYNOPSIS
    Transforms PostgreSQL SQL to SQL Server format
.DESCRIPTION
    Step 2 of the migration process: Converts PostgreSQL-specific syntax to SQL Server
#>

param(
    [string]$ConfigFile = "migrate-config.json"
)

$ErrorActionPreference = "Stop"

Write-Host "=== Step 2: Transform SQL Syntax ===" -ForegroundColor Cyan

# Load configuration
$config = Get-Content $ConfigFile -Raw | ConvertFrom-Json

$outputDir = $config.options.outputDirectory
$dataFile = Join-Path $outputDir "data.sql"
$transformedFile = Join-Path $outputDir "data_transformed.sql"

if (-not (Test-Path $dataFile)) {
    Write-Host "Data file not found: $dataFile" -ForegroundColor Red
    Write-Host "Please run 01-export-postgresql.ps1 first" -ForegroundColor Yellow
    exit 1
}

Write-Host "Reading source file..." -ForegroundColor Yellow
$content = Get-Content $dataFile -Raw
$originalSize = $content.Length

Write-Host "Applying transformations..." -ForegroundColor Yellow

# Track transformations
$transformations = @()

# 1. Transform boolean values
Write-Host "  - Converting boolean values..." -ForegroundColor Gray
$before = $content
$content = $content -replace "'t'", "1" -replace "'f'", "0"
$content = $content -replace "\btrue\b", "1" -replace "\bfalse\b", "0"
if ($content -ne $before) {
    $transformations += "Boolean values (true/false → 1/0)"
}

# 2. Remove PostgreSQL type casts
Write-Host "  - Removing PostgreSQL type casts..." -ForegroundColor Gray
$before = $content
$content = $content -replace "::timestamp", ""
$content = $content -replace "::timestamptz", ""
$content = $content -replace "::date", ""
$content = $content -replace "::time", ""
$content = $content -replace "::integer", ""
$content = $content -replace "::bigint", ""
$content = $content -replace "::uuid", ""
$content = $content -replace "::text", ""
$content = $content -replace "::varchar", ""
if ($content -ne $before) {
    $transformations += "PostgreSQL type casts (::type)"
}

# 3. Transform sequences
Write-Host "  - Removing sequence operations..." -ForegroundColor Gray
$before = $content
$content = $content -replace "SELECT pg_catalog\.setval\([^;]+;", ""
if ($content -ne $before) {
    $transformations += "Sequence operations (handled by IDENTITY)"
}

# 4. Transform NOW() to GETDATE()
Write-Host "  - Converting date functions..." -ForegroundColor Gray
$before = $content
$content = $content -replace "\bNOW\(\)", "GETDATE()"
$content = $content -replace "\bCURRENT_TIMESTAMP\b", "GETDATE()"
if ($content -ne $before) {
    $transformations += "Date functions (NOW() → GETDATE())"
}

# 5. Transform string concatenation
Write-Host "  - Converting string concatenation..." -ForegroundColor Gray
$before = $content
$content = $content -replace "\|\|", "+"
if ($content -ne $before) {
    $transformations += "String concatenation (|| → +)"
}

# 6. Add SET IDENTITY_INSERT for tables
Write-Host "  - Adding IDENTITY_INSERT statements..." -ForegroundColor Gray
$content = $content -replace "INSERT INTO ([a-zA-Z_]+)", "SET IDENTITY_INSERT `$1 ON;`nINSERT INTO `$1"

# 7. Add header to disable constraints and triggers
Write-Host "  - Adding constraint/trigger management..." -ForegroundColor Gray
$header = @"
-- =============================================
-- PostgreSQL to SQL Server Migration
-- Generated: $(Get-Date -Format "yyyy-MM-dd HH:mm:ss")
-- =============================================

-- Disable all foreign key constraints
EXEC sp_MSforeachtable 'ALTER TABLE ? NOCHECK CONSTRAINT ALL';
PRINT 'Foreign key constraints disabled';

-- Disable all triggers
EXEC sp_MSforeachtable 'ALTER TABLE ? DISABLE TRIGGER ALL';
PRINT 'Triggers disabled';

-- Set options for bulk insert
SET NOCOUNT ON;
SET XACT_ABORT ON;

BEGIN TRANSACTION;
PRINT 'Transaction started';

"@

# 8. Add footer to re-enable constraints and triggers
$footer = @"

-- Commit transaction
COMMIT TRANSACTION;
PRINT 'Transaction committed';

-- Re-enable all foreign key constraints
EXEC sp_MSforeachtable 'ALTER TABLE ? CHECK CONSTRAINT ALL';
PRINT 'Foreign key constraints enabled';

-- Re-enable all triggers
EXEC sp_MSforeachtable 'ALTER TABLE ? ENABLE TRIGGER ALL';
PRINT 'Triggers enabled';

-- Reseed identity columns
EXEC sp_MSforeachtable 'DBCC CHECKIDENT(''?'', RESEED)';
PRINT 'Identity columns reseeded';

-- Update statistics
EXEC sp_MSforeachtable 'UPDATE STATISTICS ?';
PRINT 'Statistics updated';

PRINT 'Migration completed successfully';
"@

$content = $header + $content + $footer

# Write transformed file
Write-Host "Writing transformed file..." -ForegroundColor Yellow
$content | Out-File -FilePath $transformedFile -Encoding UTF8

$transformedSize = $content.Length
$sizeDiff = $transformedSize - $originalSize

Write-Host "`nTransformation Summary:" -ForegroundColor Cyan
Write-Host "  Original size: $([math]::Round($originalSize / 1KB, 2)) KB" -ForegroundColor Gray
Write-Host "  Transformed size: $([math]::Round($transformedSize / 1KB, 2)) KB" -ForegroundColor Gray
Write-Host "  Size difference: $([math]::Round($sizeDiff / 1KB, 2)) KB" -ForegroundColor Gray

Write-Host "`nApplied transformations:" -ForegroundColor Cyan
foreach ($t in $transformations) {
    Write-Host "  ✓ $t" -ForegroundColor Green
}

Write-Host "`nOutput file: $transformedFile" -ForegroundColor Gray

Write-Host "`n✓ Transformation completed successfully!" -ForegroundColor Green
Write-Host "Next step: Run 03-import-sqlserver.ps1" -ForegroundColor Yellow
