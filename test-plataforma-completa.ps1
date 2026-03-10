#!/usr/bin/env pwsh

# 🧪 TESTE COMPLETO DA PLATAFORMA EDUCACIONAL
# Verifica se todos os componentes estão funcionando

Write-Host "🧪 TESTANDO PLATAFORMA EDUCACIONAL COMPLETA..." -ForegroundColor Cyan
Write-Host "=" * 60

# Função para testar URL
function Test-Url {
    param($url, $name)
    try {
        $response = Invoke-WebRequest -Uri $url -TimeoutSec 5 -UseBasicParsing
        if ($response.StatusCode -eq 200) {
            Write-Host "  ✅ $name funcionando" -ForegroundColor Green
            return $true
        }
    }
    catch {
        Write-Host "  ❌ $name com problema" -ForegroundColor Red
        return $false
    }
}

# Função para verificar processo
function Test-Process {
    param($processName, $serviceName)
    $process = Get-Process -Name $processName -ErrorAction SilentlyContinue
    if ($process) {
        Write-Host "  ✅ $serviceName rodando (PID: $($process.Id))" -ForegroundColor Green
        return $true
    } else {
        Write-Host "  ❌ $serviceName não encontrado" -ForegroundColor Red
        return $false
    }
}

# 1. VERIFICAR PROCESSOS
Write-Host "🔍 1. VERIFICANDO PROCESSOS..." -ForegroundColor Yellow
$processesOk = 0
$processesOk += Test-Process "node" "Frontend (Next.js)"
$processesOk += Test-Process "dotnet" "Serviços .NET"

# 2. VERIFICAR PORTAS
Write-Host "`n🌐 2. VERIFICANDO PORTAS..." -ForegroundColor Yellow
$portsOk = 0
$portsOk += Test-Url "http://localhost:3000" "Frontend"
$portsOk += Test-Url "http://localhost:5006/health" "Execution Service"
$portsOk += Test-Url "http://localhost:5008/health" "SqlExecutor Service"

# 3. TESTAR EXECUTORES
Write-Host "`n⚙️ 3. TESTANDO EXECUTORES..." -ForegroundColor Yellow

# Testar SQL Executor
try {
    $sqlTest = Invoke-RestMethod -Uri "http://localhost:5008/api/sql/execute" -Method Post -Body '{"query":"SELECT 1 as test"}' -ContentType "application/json"
    Write-Host "  ✅ SQL Executor funcionando" -ForegroundColor Green
    $executorsOk = 1
} catch {
    Write-Host "  ❌ SQL Executor com problema" -ForegroundColor Red
    $executorsOk = 0
}

# Testar C# Executor
try {
    $csharpTest = Invoke-RestMethod -Uri "http://localhost:5006/api/code/execute" -Method Post -Body '{"Code":"Console.WriteLine(\"Test\");"}' -ContentType "application/json"
    Write-Host "  ✅ C# Executor funcionando" -ForegroundColor Green
    $executorsOk += 1
} catch {
    Write-Host "  ❌ C# Executor com problema" -ForegroundColor Red
}

# 4. VERIFICAR ARQUIVOS ESSENCIAIS
Write-Host "`n📁 4. VERIFICANDO ARQUIVOS..." -ForegroundColor Yellow
$filesOk = 0

$essentialFiles = @(
    "frontend/lib/api/courses.ts",
    "frontend/lib/components/SqlExecutor.tsx",
    "frontend/lib/components/TerminalExecutor.tsx",
    "frontend/lib/components/AzureSimulator.tsx",
    "src/Services/SqlExecutor/Program.cs",
    "src/Services/Execution/Program.cs"
)

foreach ($file in $essentialFiles) {
    if (Test-Path $file) {
        Write-Host "  ✅ $file" -ForegroundColor Green
        $filesOk++
    } else {
        Write-Host "  ❌ $file não encontrado" -ForegroundColor Red
    }
}

# 5. VERIFICAR CURSOS E AULAS
Write-Host "`n📚 5. VERIFICANDO CURSOS..." -ForegroundColor Yellow

# Ler arquivo de cursos
$coursesFile = "frontend/lib/api/courses.ts"
if (Test-Path $coursesFile) {
    $content = Get-Content $coursesFile -Raw
    
    # Contar cursos
    $courseMatches = [regex]::Matches($content, "id: '\d+'")
    $courseCount = ($courseMatches | Where-Object { $_.Value -match "id: '[1-9]|1[0-2]'" }).Count
    
    # Contar aulas
    $lessonMatches = [regex]::Matches($content, "courseId: '\d+'")
    $lessonCount = $lessonMatches.Count
    
    Write-Host "  ✅ $courseCount cursos encontrados" -ForegroundColor Green
    Write-Host "  ✅ $lessonCount aulas encontradas" -ForegroundColor Green
    
    if ($courseCount -eq 12 -and $lessonCount -gt 150) {
        Write-Host "  ✅ Conteúdo completo implementado" -ForegroundColor Green
        $contentOk = $true
    } else {
        Write-Host "  ⚠️ Conteúdo pode estar incompleto" -ForegroundColor Yellow
        $contentOk = $false
    }
} else {
    Write-Host "  ❌ Arquivo de cursos não encontrado" -ForegroundColor Red
    $contentOk = $false
}

# 6. RELATÓRIO FINAL
Write-Host "`n" + "=" * 60
Write-Host "📊 RELATÓRIO FINAL" -ForegroundColor Cyan
Write-Host "=" * 60

$totalScore = 0
$maxScore = 5

Write-Host "🔍 Processos: $processesOk/2" -ForegroundColor $(if($processesOk -eq 2) {"Green"} else {"Yellow"})
$totalScore += [Math]::Min($processesOk, 2) / 2

Write-Host "🌐 Serviços Web: $portsOk/3" -ForegroundColor $(if($portsOk -eq 3) {"Green"} else {"Yellow"})
$totalScore += [Math]::Min($portsOk, 3) / 3

Write-Host "⚙️ Executores: $executorsOk/2" -ForegroundColor $(if($executorsOk -eq 2) {"Green"} else {"Yellow"})
$totalScore += [Math]::Min($executorsOk, 2) / 2

Write-Host "📁 Arquivos: $filesOk/$($essentialFiles.Count)" -ForegroundColor $(if($filesOk -eq $essentialFiles.Count) {"Green"} else {"Yellow"})
$totalScore += $filesOk / $essentialFiles.Count

Write-Host "📚 Conteúdo: $(if($contentOk) {"✅ Completo"} else {"⚠️ Incompleto"})" -ForegroundColor $(if($contentOk) {"Green"} else {"Yellow"})
$totalScore += if($contentOk) {1} else {0}

$percentage = [Math]::Round(($totalScore / $maxScore) * 100, 1)

Write-Host "`n" + "=" * 60
if ($percentage -ge 90) {
    Write-Host "🎉 PLATAFORMA FUNCIONANDO PERFEITAMENTE!" -ForegroundColor Green
    Write-Host "✅ Score: $percentage% - Excelente!" -ForegroundColor Green
    Write-Host "🚀 Pronta para demonstração na faculdade!" -ForegroundColor Green
} elseif ($percentage -ge 70) {
    Write-Host "✅ PLATAFORMA FUNCIONANDO BEM!" -ForegroundColor Yellow
    Write-Host "⚠️ Score: $percentage% - Alguns ajustes podem ser necessários" -ForegroundColor Yellow
} else {
    Write-Host "❌ PLATAFORMA COM PROBLEMAS!" -ForegroundColor Red
    Write-Host "🔧 Score: $percentage% - Necessita correções" -ForegroundColor Red
}

Write-Host "`n🌐 Acesse: http://localhost:3000" -ForegroundColor Cyan
Write-Host "📖 Documentação: PLATAFORMA_EDUCACIONAL_FACULDADE.md" -ForegroundColor Cyan
Write-Host "🎯 Guia de Demo: GUIA_DEMONSTRACAO_FACULDADE.md" -ForegroundColor Cyan

Write-Host "`n" + "=" * 60
Write-Host "🧪 TESTE CONCLUÍDO!" -ForegroundColor Cyan