# Script de Build Seguro - Resolve problemas de arquivos bloqueados
# Uso: .\build-safe.ps1 [-Configuration Release|Debug] [-Clean]

param(
    [string]$Configuration = "Release",
    [switch]$Clean = $false,
    [switch]$Test = $false
)

Write-Host "=== BUILD SEGURO - ASP.NET Learning Platform ===" -ForegroundColor Cyan
Write-Host "Configuração: $Configuration" -ForegroundColor Yellow

# Função para parar processos com retry
function Stop-ProcessesSafely {
    Write-Host "`n1. Parando processos dotnet e node..." -ForegroundColor Yellow
    
    $dotnetProcesses = Get-Process -Name "dotnet" -ErrorAction SilentlyContinue
    $nodeProcesses = Get-Process -Name "node" -ErrorAction SilentlyContinue
    
    if ($dotnetProcesses) {
        Write-Host "   Parando $($dotnetProcesses.Count) processos dotnet..." -ForegroundColor Gray
        $dotnetProcesses | Stop-Process -Force -ErrorAction SilentlyContinue
    }
    
    if ($nodeProcesses) {
        Write-Host "   Parando $($nodeProcesses.Count) processos node..." -ForegroundColor Gray
        $nodeProcesses | Stop-Process -Force -ErrorAction SilentlyContinue
    }
    
    # Aguardar processos terminarem
    Start-Sleep -Seconds 2
    
    # Verificar se ainda há processos
    $remaining = Get-Process -Name "dotnet","node" -ErrorAction SilentlyContinue
    if ($remaining) {
        Write-Host "   ⚠️ Alguns processos ainda estão rodando, tentando novamente..." -ForegroundColor Yellow
        Start-Sleep -Seconds 2
        $remaining | Stop-Process -Force -ErrorAction SilentlyContinue
        Start-Sleep -Seconds 1
    }
    
    Write-Host "   ✅ Processos parados" -ForegroundColor Green
}

# Função para limpar com retry
function Clean-BuildArtifacts {
    Write-Host "`n2. Limpando artefatos de build..." -ForegroundColor Yellow
    
    try {
        # Limpar usando dotnet clean
        dotnet clean --configuration $Configuration --verbosity quiet
        
        # Remover pastas bin e obj manualmente
        Write-Host "   Removendo pastas bin e obj..." -ForegroundColor Gray
        Get-ChildItem -Path . -Include bin,obj -Recurse -Directory -ErrorAction SilentlyContinue | 
            ForEach-Object {
                try {
                    Remove-Item $_.FullName -Recurse -Force -ErrorAction SilentlyContinue
                } catch {
                    Write-Host "   ⚠️ Não foi possível remover: $($_.FullName)" -ForegroundColor Yellow
                }
            }
        
        Write-Host "   ✅ Limpeza concluída" -ForegroundColor Green
    } catch {
        Write-Host "   ⚠️ Erro na limpeza: $($_.Exception.Message)" -ForegroundColor Yellow
    }
}

# Função para build com retry
function Build-WithRetry {
    param([int]$MaxRetries = 3)
    
    Write-Host "`n3. Compilando projeto..." -ForegroundColor Yellow
    
    for ($i = 1; $i -le $MaxRetries; $i++) {
        Write-Host "   Tentativa $i de $MaxRetries..." -ForegroundColor Gray
        
        $buildOutput = dotnet build --configuration $Configuration --verbosity minimal 2>&1 | Out-String
        
        # Verificar se houve erros
        $errors = ([regex]::Matches($buildOutput, "error")).Count
        $warnings = ([regex]::Matches($buildOutput, "warning")).Count
        
        if ($errors -eq 0) {
            Write-Host "   ✅ Build bem-sucedido!" -ForegroundColor Green
            Write-Host "   Warnings: $warnings" -ForegroundColor $(if ($warnings -eq 0) { "Green" } else { "Yellow" })
            return $true
        }
        
        Write-Host "   ❌ Build falhou com $errors erros" -ForegroundColor Red
        
        if ($i -lt $MaxRetries) {
            Write-Host "   Aguardando antes de tentar novamente..." -ForegroundColor Yellow
            Start-Sleep -Seconds 3
            
            # Parar processos novamente
            Stop-ProcessesSafely
        } else {
            Write-Host "`n=== ERROS DE BUILD ===" -ForegroundColor Red
            $buildOutput | Select-String -Pattern "error" | Select-Object -First 10 | ForEach-Object {
                Write-Host $_.Line -ForegroundColor Red
            }
            return $false
        }
    }
    
    return $false
}

# Função para executar testes
function Run-Tests {
    Write-Host "`n4. Executando testes..." -ForegroundColor Yellow
    
    $testOutput = dotnet test --configuration $Configuration --no-build --verbosity minimal 2>&1 | Out-String
    
    # Extrair estatísticas
    $testOutput | Select-String -Pattern "Passed|Failed|Total" | ForEach-Object {
        Write-Host "   $($_.Line)" -ForegroundColor Cyan
    }
    
    # Verificar se houve falhas
    if ($testOutput -match "Failed:.*[1-9]") {
        Write-Host "   ⚠️ Alguns testes falharam" -ForegroundColor Yellow
        return $false
    } else {
        Write-Host "   ✅ Todos os testes passaram!" -ForegroundColor Green
        return $true
    }
}

# EXECUÇÃO PRINCIPAL
try {
    $startTime = Get-Date
    
    # Passo 1: Parar processos
    Stop-ProcessesSafely
    
    # Passo 2: Limpar (se solicitado)
    if ($Clean) {
        Clean-BuildArtifacts
    }
    
    # Passo 3: Build com retry
    $buildSuccess = Build-WithRetry
    
    if (-not $buildSuccess) {
        Write-Host "`n❌ BUILD FALHOU!" -ForegroundColor Red
        exit 1
    }
    
    # Passo 4: Testes (se solicitado)
    if ($Test) {
        $testSuccess = Run-Tests
        
        if (-not $testSuccess) {
            Write-Host "`n⚠️ BUILD OK, MAS TESTES FALHARAM" -ForegroundColor Yellow
            exit 2
        }
    }
    
    # Sucesso!
    $endTime = Get-Date
    $duration = ($endTime - $startTime).TotalSeconds
    
    Write-Host "`n=== BUILD COMPLETO ===" -ForegroundColor Cyan
    Write-Host "✅ Build bem-sucedido em $([math]::Round($duration, 2)) segundos" -ForegroundColor Green
    Write-Host "Configuração: $Configuration" -ForegroundColor Yellow
    
    if ($Test) {
        Write-Host "✅ Testes: OK" -ForegroundColor Green
    }
    
    exit 0
    
} catch {
    Write-Host "`n❌ ERRO INESPERADO: $($_.Exception.Message)" -ForegroundColor Red
    exit 1
}
