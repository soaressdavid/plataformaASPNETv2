#!/usr/bin/env pwsh
<#
.SYNOPSIS
    Reinicia todos os serviços do AspNetLearningPlatform

.DESCRIPTION
    Para tudo e inicia novamente
#>

param(
    [switch]$SkipBuild
)

Write-Host "`n=== Reiniciando sistema ===" -ForegroundColor Magenta

# Parar tudo
Write-Host "`nParando serviços..." -ForegroundColor Cyan
.\stop-all.ps1

# Aguardar
Write-Host "`nAguardando 3 segundos..." -ForegroundColor Cyan
Start-Sleep -Seconds 3

# Iniciar tudo
Write-Host "`nIniciando serviços..." -ForegroundColor Cyan
if ($SkipBuild) {
    .\start-all.ps1 -SkipBuild
} else {
    .\start-all.ps1
}
