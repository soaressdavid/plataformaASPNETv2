#!/usr/bin/env pwsh
<#
.SYNOPSIS
    Exibe logs dos serviços

.PARAMETER Service
    Nome do serviço específico (ApiGateway, Auth, Course, etc.)

.PARAMETER Docker
    Exibe logs dos containers Docker

.EXAMPLE
    .\logs.ps1
    Exibe logs de todos os serviços .NET

.EXAMPLE
    .\logs.ps1 -Service ApiGateway
    Exibe logs apenas do ApiGateway

.EXAMPLE
    .\logs.ps1 -Docker
    Exibe logs dos containers Docker
#>

param(
    [string]$Service,
    [switch]$Docker
)

if ($Docker) {
    Write-Host "=== Logs dos containers Docker ===" -ForegroundColor Green
    docker-compose logs -f
} elseif ($Service) {
    Write-Host "=== Logs do $Service ===" -ForegroundColor Green
    Write-Host "Funcionalidade em desenvolvimento..." -ForegroundColor Yellow
    Write-Host "Use 'docker-compose logs -f' para ver logs dos containers" -ForegroundColor Cyan
} else {
    Write-Host "=== Logs de todos os serviços ===" -ForegroundColor Green
    Write-Host "Os logs são exibidos no terminal onde você executou .\start-all.ps1" -ForegroundColor Cyan
    Write-Host "`nPara ver logs do Docker:" -ForegroundColor Yellow
    Write-Host "  .\logs.ps1 -Docker" -ForegroundColor White
}
