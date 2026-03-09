#!/usr/bin/env pwsh

<#
.SYNOPSIS
    Run all load tests for the ASP.NET Core Learning Platform
.DESCRIPTION
    This script runs all three load test scenarios:
    1. Browse Challenges (1000 concurrent users)
    2. Code Execution (100 concurrent executions)
    3. AI Feedback (50 concurrent requests)
.PARAMETER BaseUrl
    The base URL of the API Gateway (default: http://localhost:5000)
.PARAMETER TestUsers
    Create test users before running tests (default: true)
.EXAMPLE
    ./run-all-tests.ps1
.EXAMPLE
    ./run-all-tests.ps1 -BaseUrl "https://api.example.com"
#>

param(
    [string]$BaseUrl = "http://localhost:5000",
    [bool]$TestUsers = $true
)

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "ASP.NET Core Learning Platform" -ForegroundColor Cyan
Write-Host "Load Testing Suite" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

# Check if k6 is installed
Write-Host "Checking k6 installation..." -ForegroundColor Yellow
$k6Version = k6 version 2>&1
if ($LASTEXITCODE -ne 0) {
    Write-Host "ERROR: k6 is not installed!" -ForegroundColor Red
    Write-Host ""
    Write-Host "Please install k6 from: https://k6.io/docs/getting-started/installation/" -ForegroundColor Yellow
    Write-Host ""
    Write-Host "Installation options:" -ForegroundColor Yellow
    Write-Host "  Windows (Chocolatey): choco install k6" -ForegroundColor White
    Write-Host "  Windows (Winget):     winget install k6" -ForegroundColor White
    Write-Host "  macOS (Homebrew):     brew install k6" -ForegroundColor White
    Write-Host "  Linux (Debian/Ubuntu): sudo apt-key adv --keyserver hkp://keyserver.ubuntu.com:80 --recv-keys C5AD17C747E3415A3642D57D77C6C491D6AC1D69" -ForegroundColor White
    Write-Host "                         echo 'deb https://dl.k6.io/deb stable main' | sudo tee /etc/apt/sources.list.d/k6.list" -ForegroundColor White
    Write-Host "                         sudo apt-get update" -ForegroundColor White
    Write-Host "                         sudo apt-get install k6" -ForegroundColor White
    exit 1
}

Write-Host "k6 version: $k6Version" -ForegroundColor Green
Write-Host ""

# Check if services are running
Write-Host "Checking if services are running at $BaseUrl..." -ForegroundColor Yellow
try {
    $healthCheck = Invoke-WebRequest -Uri "$BaseUrl/health" -Method Get -TimeoutSec 5 -ErrorAction Stop
    Write-Host "Services are running!" -ForegroundColor Green
} catch {
    Write-Host "WARNING: Could not reach services at $BaseUrl" -ForegroundColor Yellow
    Write-Host "Make sure the platform is running before executing load tests." -ForegroundColor Yellow
    Write-Host ""
    $continue = Read-Host "Continue anyway? (y/n)"
    if ($continue -ne "y") {
        exit 1
    }
}
Write-Host ""

# Create test users if requested
if ($TestUsers) {
    Write-Host "Creating test users..." -ForegroundColor Yellow
    
    $testUsers = @(
        @{ email = "test1@example.com"; name = "Test User 1"; password = "Test123!@#" },
        @{ email = "test2@example.com"; name = "Test User 2"; password = "Test123!@#" },
        @{ email = "test3@example.com"; name = "Test User 3"; password = "Test123!@#" },
        @{ email = "test4@example.com"; name = "Test User 4"; password = "Test123!@#" },
        @{ email = "test5@example.com"; name = "Test User 5"; password = "Test123!@#" }
    )
    
    foreach ($user in $testUsers) {
        try {
            $body = @{
                name = $user.name
                email = $user.email
                password = $user.password
            } | ConvertTo-Json
            
            $response = Invoke-WebRequest -Uri "$BaseUrl/api/auth/register" -Method Post -Body $body -ContentType "application/json" -ErrorAction SilentlyContinue
            Write-Host "  Created user: $($user.email)" -ForegroundColor Green
        } catch {
            if ($_.Exception.Response.StatusCode -eq 409) {
                Write-Host "  User already exists: $($user.email)" -ForegroundColor Gray
            } else {
                Write-Host "  Failed to create user: $($user.email) - $($_.Exception.Message)" -ForegroundColor Yellow
            }
        }
    }
    Write-Host ""
}

# Create results directory
$resultsDir = "load-tests/results"
if (-not (Test-Path $resultsDir)) {
    New-Item -ItemType Directory -Path $resultsDir | Out-Null
}

$timestamp = Get-Date -Format "yyyyMMdd_HHmmss"

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "Test 1: Browse Challenges" -ForegroundColor Cyan
Write-Host "Target: 1000 concurrent users" -ForegroundColor Cyan
Write-Host "Performance target: p95 < 200ms" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

$env:BASE_URL = $BaseUrl
k6 run --out json="$resultsDir/browse-challenges-$timestamp.json" load-tests/browse-challenges.js

Write-Host ""
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "Test 2: Code Execution" -ForegroundColor Cyan
Write-Host "Target: 100 concurrent executions" -ForegroundColor Cyan
Write-Host "Performance target: p95 < 5s" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

k6 run --out json="$resultsDir/code-execution-$timestamp.json" load-tests/code-execution.js

Write-Host ""
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "Test 3: AI Feedback" -ForegroundColor Cyan
Write-Host "Target: 50 concurrent requests" -ForegroundColor Cyan
Write-Host "Performance target: p95 < 10s" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

k6 run --out json="$resultsDir/ai-feedback-$timestamp.json" load-tests/ai-feedback.js

Write-Host ""
Write-Host "========================================" -ForegroundColor Green
Write-Host "All load tests completed!" -ForegroundColor Green
Write-Host "========================================" -ForegroundColor Green
Write-Host ""
Write-Host "Results saved to: $resultsDir" -ForegroundColor Yellow
Write-Host ""
Write-Host "To view detailed results, use k6 Cloud or analyze the JSON files." -ForegroundColor Yellow
Write-Host ""
