#!/usr/bin/env pwsh
<#
.SYNOPSIS
    Verifies data integrity after migration
.DESCRIPTION
    Step 4 of the migration process: Compares row counts and validates data integrity
#>

param(
    [string]$ConfigFile = "migrate-config.json"
)

$ErrorActionPreference = "Stop"

Write-Host "=== Step 4: Verify Migration ===" -ForegroundColor Cyan

# Load configuration
$config = Get-Content $ConfigFile -Raw | ConvertFrom-Json

$outputDir = $config.options.outputDirectory
$reportFile = Join-Path $outputDir "migration-report.html"
$csvFile = Join-Path $outputDir "migration-report.csv"

Write-Host "Connecting to databases..." -ForegroundColor Yellow

# Set PostgreSQL password
$env:PGPASSWORD = $config.source.password

# Test PostgreSQL connection
$pgVersion = psql -h $config.source.host -p $config.source.port -U $config.source.username -d $config.source.database -t -c "SELECT version();"
if ($LASTEXITCODE -ne 0) {
    Write-Host "Failed to connect to PostgreSQL" -ForegroundColor Red
    exit 1
}
Write-Host "  PostgreSQL: Connected" -ForegroundColor Green

# Test SQL Server connection
$sqlVersion = sqlcmd -S $config.target.server -U $config.target.username -P $config.target.password -Q "SELECT @@VERSION" -h -1
if ($LASTEXITCODE -ne 0) {
    Write-Host "Failed to connect to SQL Server" -ForegroundColor Red
    exit 1
}
Write-Host "  SQL Server: Connected" -ForegroundColor Green

# Get table list from PostgreSQL
Write-Host "`nGetting table list..." -ForegroundColor Yellow
$pgTables = psql -h $config.source.host -p $config.source.port -U $config.source.username -d $config.source.database -t -c "SELECT tablename FROM pg_tables WHERE schemaname='public' ORDER BY tablename;"

$report = @()
$allMatch = $true
$totalPgRows = 0
$totalSqlRows = 0

Write-Host "`nComparing row counts:" -ForegroundColor Yellow
Write-Host ("=" * 80) -ForegroundColor Gray
Write-Host ("{0,-30} {1,15} {2,15} {3,10}" -f "Table", "PostgreSQL", "SQL Server", "Status") -ForegroundColor Cyan
Write-Host ("=" * 80) -ForegroundColor Gray

foreach ($table in $pgTables) {
    $table = $table.Trim()
    if ([string]::IsNullOrWhiteSpace($table) -or $config.options.excludeTables -contains $table) {
        continue
    }
    
    # Get row count from PostgreSQL
    $pgCount = psql -h $config.source.host -p $config.source.port -U $config.source.username -d $config.source.database -t -c "SELECT COUNT(*) FROM $table;"
    $pgCount = [int]$pgCount.Trim()
    $totalPgRows += $pgCount
    
    # Get row count from SQL Server
    try {
        $sqlCount = sqlcmd -S $config.target.server -U $config.target.username -P $config.target.password -d $config.target.database -h -1 -Q "SELECT COUNT(*) FROM [$table]"
        $sqlCount = [int]$sqlCount.Trim()
        $totalSqlRows += $sqlCount
    }
    catch {
        $sqlCount = -1
    }
    
    $match = $pgCount -eq $sqlCount
    if (-not $match) {
        $allMatch = $false
    }
    
    $difference = $sqlCount - $pgCount
    $percentMatch = if ($pgCount -gt 0) { [math]::Round(($sqlCount / $pgCount) * 100, 2) } else { 0 }
    
    $report += [PSCustomObject]@{
        Table = $table
        PostgreSQL_Rows = $pgCount
        SQLServer_Rows = $sqlCount
        Difference = $difference
        Percent_Match = $percentMatch
        Status = if ($match) { "✓ Match" } else { "✗ Mismatch" }
    }
    
    $status = if ($match) { "✓ Match" } else { "✗ Mismatch ($difference)" }
    $color = if ($match) { "Green" } else { "Red" }
    
    Write-Host ("{0,-30} {1,15} {2,15} {3,10}" -f $table, $pgCount, $sqlCount, $status) -ForegroundColor $color
}

Write-Host ("=" * 80) -ForegroundColor Gray
Write-Host ("{0,-30} {1,15} {2,15}" -f "TOTAL", $totalPgRows, $totalSqlRows) -ForegroundColor Cyan
Write-Host ("=" * 80) -ForegroundColor Gray

# Save CSV report
$report | Export-Csv -Path $csvFile -NoTypeInformation
Write-Host "`nCSV report saved: $csvFile" -ForegroundColor Gray

# Generate HTML report
$htmlReport = @"
<!DOCTYPE html>
<html>
<head>
    <title>Migration Verification Report</title>
    <style>
        body { font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; margin: 20px; background: #f5f5f5; }
        .container { max-width: 1200px; margin: 0 auto; background: white; padding: 30px; border-radius: 8px; box-shadow: 0 2px 4px rgba(0,0,0,0.1); }
        h1 { color: #2c3e50; border-bottom: 3px solid #3498db; padding-bottom: 10px; }
        h2 { color: #34495e; margin-top: 30px; }
        .summary { background: #ecf0f1; padding: 20px; border-radius: 5px; margin: 20px 0; }
        .summary-item { display: inline-block; margin-right: 30px; }
        .summary-label { font-weight: bold; color: #7f8c8d; }
        .summary-value { font-size: 24px; color: #2c3e50; }
        table { width: 100%; border-collapse: collapse; margin: 20px 0; }
        th { background: #3498db; color: white; padding: 12px; text-align: left; }
        td { padding: 10px; border-bottom: 1px solid #ecf0f1; }
        tr:hover { background: #f8f9fa; }
        .match { color: #27ae60; font-weight: bold; }
        .mismatch { color: #e74c3c; font-weight: bold; }
        .status-match { background: #d4edda; color: #155724; padding: 5px 10px; border-radius: 3px; }
        .status-mismatch { background: #f8d7da; color: #721c24; padding: 5px 10px; border-radius: 3px; }
        .footer { margin-top: 30px; padding-top: 20px; border-top: 1px solid #ecf0f1; color: #7f8c8d; font-size: 12px; }
    </style>
</head>
<body>
    <div class="container">
        <h1>🔄 Migration Verification Report</h1>
        <p><strong>Generated:</strong> $(Get-Date -Format "yyyy-MM-dd HH:mm:ss")</p>
        
        <div class="summary">
            <div class="summary-item">
                <div class="summary-label">Total Tables</div>
                <div class="summary-value">$($report.Count)</div>
            </div>
            <div class="summary-item">
                <div class="summary-label">PostgreSQL Rows</div>
                <div class="summary-value">$totalPgRows</div>
            </div>
            <div class="summary-item">
                <div class="summary-label">SQL Server Rows</div>
                <div class="summary-value">$totalSqlRows</div>
            </div>
            <div class="summary-item">
                <div class="summary-label">Status</div>
                <div class="summary-value $(if ($allMatch) { 'match' } else { 'mismatch' })">
                    $(if ($allMatch) { '✓ All Match' } else { '✗ Mismatches Found' })
                </div>
            </div>
        </div>
        
        <h2>📊 Detailed Comparison</h2>
        <table>
            <thead>
                <tr>
                    <th>Table Name</th>
                    <th>PostgreSQL Rows</th>
                    <th>SQL Server Rows</th>
                    <th>Difference</th>
                    <th>Match %</th>
                    <th>Status</th>
                </tr>
            </thead>
            <tbody>
"@

foreach ($row in $report) {
    $statusClass = if ($row.Status -like "*Match*") { "status-match" } else { "status-mismatch" }
    $htmlReport += @"
                <tr>
                    <td><strong>$($row.Table)</strong></td>
                    <td>$($row.PostgreSQL_Rows)</td>
                    <td>$($row.SQLServer_Rows)</td>
                    <td>$($row.Difference)</td>
                    <td>$($row.Percent_Match)%</td>
                    <td><span class="$statusClass">$($row.Status)</span></td>
                </tr>
"@
}

$htmlReport += @"
            </tbody>
        </table>
        
        <div class="footer">
            <p><strong>Source Database:</strong> PostgreSQL - $($config.source.host):$($config.source.port)/$($config.source.database)</p>
            <p><strong>Target Database:</strong> SQL Server - $($config.target.server)/$($config.target.database)</p>
        </div>
    </div>
</body>
</html>
"@

$htmlReport | Out-File -FilePath $reportFile -Encoding UTF8
Write-Host "HTML report saved: $reportFile" -ForegroundColor Gray

# Open HTML report in browser
if ($IsWindows -or $env:OS -eq "Windows_NT") {
    Start-Process $reportFile
}

# Final summary
Write-Host "`nVerification Summary:" -ForegroundColor Cyan
Write-Host "  Total tables checked: $($report.Count)" -ForegroundColor Gray
Write-Host "  PostgreSQL total rows: $totalPgRows" -ForegroundColor Gray
Write-Host "  SQL Server total rows: $totalSqlRows" -ForegroundColor Gray
Write-Host "  Difference: $($totalSqlRows - $totalPgRows) rows" -ForegroundColor Gray

if ($allMatch) {
    Write-Host "`n✓ All tables verified successfully!" -ForegroundColor Green
    Write-Host "  All row counts match between PostgreSQL and SQL Server" -ForegroundColor Green
    Write-Host "`nMigration completed successfully! 🎉" -ForegroundColor Green
    exit 0
}
else {
    Write-Host "`n✗ Some tables have mismatched row counts" -ForegroundColor Red
    Write-Host "  Please review the detailed report: $reportFile" -ForegroundColor Yellow
    
    $mismatches = $report | Where-Object { $_.Status -like "*Mismatch*" }
    Write-Host "`nTables with mismatches:" -ForegroundColor Yellow
    foreach ($m in $mismatches) {
        Write-Host "  - $($m.Table): PG=$($m.PostgreSQL_Rows), SQL=$($m.SQLServer_Rows) (diff: $($m.Difference))" -ForegroundColor Red
    }
    
    exit 1
}
