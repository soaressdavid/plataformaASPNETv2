#!/usr/bin/env pwsh
<#
.SYNOPSIS
    Remove arquivos obsoletos e duplicados do projeto

.DESCRIPTION
    Remove arquivos de documentação antiga, scripts obsoletos e arquivos temporários
    mantendo apenas os arquivos essenciais e atualizados

.PARAMETER DryRun
    Apenas mostra o que seria removido sem remover

.PARAMETER Confirm
    Pede confirmação antes de remover cada arquivo

.EXAMPLE
    .\limpar-arquivos-obsoletos.ps1 -DryRun
    Mostra o que seria removido

.EXAMPLE
    .\limpar-arquivos-obsoletos.ps1
    Remove todos os arquivos obsoletos
#>

param(
    [switch]$DryRun,
    [switch]$Confirm
)

$ErrorActionPreference = "Continue"

Write-Host "`n╔═══════════════════════════════════════════════════════════╗" -ForegroundColor Cyan
Write-Host "║                                                           ║" -ForegroundColor Cyan
Write-Host "║     Limpeza de Arquivos Obsoletos e Duplicados           ║" -ForegroundColor Cyan
Write-Host "║                                                           ║" -ForegroundColor Cyan
Write-Host "╚═══════════════════════════════════════════════════════════╝`n" -ForegroundColor Cyan

if ($DryRun) {
    Write-Host "🔍 MODO DRY-RUN: Apenas mostrando o que seria removido`n" -ForegroundColor Yellow
}

# Lista de arquivos para remover
$arquivosParaRemover = @(
    # Status/Relatórios Antigos
    "backend-services-started.md",
    "CURRENT_STATUS.md",
    "ESTADO_ATUAL_DO_PROJETO.md",
    "STATUS_ATUAL_SISTEMA.md",
    "STATUS_COMPLETO_SISTEMA.md",
    "STATUS_FINAL_100_PERCENT.md",
    "STATUS_FINAL_APIGATEWAY_CORRIGIDO.md",
    "STATUS_FINAL_SISTEMA.md",
    "STATUS_SISTEMA_ATUAL.md",
    "STATUS_VISUAL.md",
    "STATUS-FINAL.md",
    "STATUS-SERVICOS.md",
    "FINAL_STATUS_REPORT.md",
    "FINAL_VALIDATION_REPORT.md",
    "SUCCESS_REPORT.md",
    "EXECUTIVE_SUMMARY.md",
    "PROGRESS_SUMMARY.md",
    
    # Relatórios de Correção Antigos
    "CORRECAO_TESTES_DETALHADA.md",
    "CORRECAO_TESTES_FALHANDO.md",
    "CORRECTION_COMPLETE_REPORT.md",
    "RELATORIO_CORRECAO_AUTOMATICA.md",
    "RELATORIO_FINAL_CORRECOES.md",
    "RELATORIO_TESTES.md",
    "RESUMO_CORRECAO_TESTES.md",
    "RESUMO_FINAL_CORRECOES.md",
    
    # Relatórios de Fase/Progresso
    "FASE_2_COMPLETA.md",
    "FASE_3_COMPLETA.md",
    "FASE_4_COMPLETA.md",
    "FASE_5_6_7_COMPLETA.md",
    "FASE_8_CORRECOES_CRITICAS.md",
    "FASE_8_FINAL_COMPLETA.md",
    "RESUMO_FASE_8_FINAL.md",
    "PROGRESSO_CORRECOES_HOJE.md",
    "PROGRESSO_TRANSFORMACAO_EMPRESARIAL.md",
    "TRANSFORMACAO_EMPRESARIAL_COMPLETA.md",
    "ENTERPRISE_GRADE_PROGRESS.md",
    "PRODUCTION_READINESS_CHECK.md",
    
    # Planos de Ação Antigos
    "ACTION_PLAN_ENTERPRISE_GRADE.md",
    "ACTION_PLAN_LEVELS_7_15.md",
    "PLANO_EXECUCAO_HOJE.md",
    "TRABALHO_COMPLETO_HOJE.md",
    "RESUMO_EXECUTIVO_HOJE.md",
    
    # Documentação de Problemas Resolvidos
    "PROBLEMA_CORS_RESOLVIDO.md",
    "PROBLEMA_NETWORK_ERROR_RESOLVIDO.md",
    "PROBLEMA_RESOLVIDO_NETWORK_ERROR.md",
    "PROBLEMA_RESOLVIDO.md",
    "PROBLEMA_TOKEN_DESAPARECE.md",
    "ERRO_401_SOLUCAO.md",
    "CORS_CORRIGIDO_FINAL.md",
    "CORS_CORRIGIDO.md",
    "APIGATEWAY_CORRIGIDO.md",
    
    # Soluções/Instruções Antigas
    "SOLUCAO_COMPLETA_LOGIN.md",
    "SOLUCAO_FINAL_NETWORK_ERROR.md",
    "SOLUCAO_FINAL.md",
    "SOLUCAO_LOGIN.md",
    "SOLUCAO_TIMEOUT_FRONTEND.md",
    "SOLUCAO_TOKEN_NAO_SALVO.md",
    "INSTRUCOES_FINAIS_DEBUG.md",
    "INSTRUCOES_TESTE_LOGIN.md",
    "INSTRUCOES_VISUALIZACAO.md",
    
    # Debug/Diagnóstico Antigos
    "DEBUG_CONTEUDO.md",
    "DEBUG_LOGIN_LOGOUT.md",
    "DIAGNOSTICO.md",
    "FRONTEND_TIMEOUT_TROUBLESHOOTING.md",
    "DATABASE_SEEDING_FIX_NEEDED.md",
    
    # Resumos de Sessão
    "RESUMO_ANALISE_E_ACOES.md",
    "RESUMO_ESTADO_ATUAL.md",
    "RESUMO_FINAL_COMPLETO.md",
    "RESUMO_FINAL_CONTEXTO_TRANSFERIDO.md",
    "RESUMO_FINAL_DIA.md",
    "RESUMO_FINAL_LOGIN.md",
    "RESUMO_FINAL_SESSAO.md",
    "RESUMO_PROBLEMA_FINAL.md",
    "RESUMO_SESSAO_ATUAL.md",
    
    # Documentação de Implementação Completa
    "IMPLEMENTACAO_COMPLETA.md",
    "PROJETO_COMPLETO_FINAL.md",
    "PLATAFORMA-PRONTA.md",
    "TODAS_TASKS_COMPLETAS.md",
    "TASK_28_COMPLETA.md",
    
    # Documentação de Funcionalidades Específicas
    "LOGIN_FUNCIONANDO.md",
    "SUCESSO_LOGIN_COMPLETO.md",
    "FRONTEND_FUNCIONANDO.md",
    "TODOS-SERVICOS-FUNCIONANDO.md",
    "SERVICOS_RESOLVIDOS.md",
    "SERVICOS-DISPONIVEIS.md",
    "FRONTEND_BACKEND_ALIGNMENT_VERIFICATION.md",
    
    # Documentação de Currículo/Conteúdo
    "CURRICULUM_EXPANSION_ACTION_PLAN.md",
    "CURRICULUM_EXPANSION_COMPLETE.md",
    "CURRICULUM_EXPANSION_STATUS.md",
    "curriculum-expansion-final-status.md",
    "curriculum-expansion-progress-report.md",
    "CURSOS_DISPONIVEIS.md",
    "CURSOS-RESTAURADOS.md",
    "DADOS_REAIS_APENAS.md",
    "GERACAO_CURRICULO_COMPLETO.md",
    "PLANO_CURRICULO_COMPLETO.md",
    "EFFICIENT_CONTENT_STRATEGY.md",
    "CONTENT_QUALITY_IMPROVEMENTS.md",
    
    # Relatórios de Validação de Níveis
    "Level1ContentSummary.md",
    "level4_manual_validation_report.md",
    "level4_validation_results.json",
    "level5_expansion_progress_report.md",
    "level5_validation_report.md",
    "level5_validation_results.json",
    "level5-regeneration-success-report.md",
    "LEVEL6_EXPANSION_PROGRESS.md",
    "phase2_checkpoint_report.md",
    "phase2_checkpoint_validation.json",
    "phase2_validation_results.json",
    "validation_results.json",
    "analysis_results.json",
    
    # Scripts PowerShell Obsoletos
    "corrigir-apigateway-timeout.ps1",
    "corrigir-compilacao-completa.ps1",
    "corrigir-melhorias-criticas.ps1",
    "corrigir-tudo-automatico.ps1",
    "corrigir-warnings-null.ps1",
    "criar-usuario-teste.ps1",
    "deploy.ps1",
    "diagnosticar-e-corrigir-timeout.ps1",
    "executar-testes-unitarios.ps1",
    "fix-ports.ps1",
    "gerenciar.ps1",
    "iniciar-backend-simples.ps1",
    "iniciar-servicos.ps1",
    "iniciar-sistema.ps1",
    "iniciar.ps1",
    "parar.ps1",
    "recriar-banco-dados-reais.ps1",
    "reiniciar-frontend.ps1",
    "reiniciar-servicos-atualizados.ps1",
    "reiniciar-sistema-completo.ps1",
    "security-tests.ps1",
    "seed-database.ps1",
    "serve-test.ps1",
    "start-all-services.ps1",
    "start-minimal.ps1",
    "stop-all-services.ps1",
    "testar-login.ps1",
    "teste-completo.ps1",
    "verificar-status-final.ps1",
    "verificar.ps1",
    "verify-production-readiness.ps1",
    
    # Arquivos de Teste HTML/JSON
    "test-ai.json",
    "test-api.html",
    "test-frontend-api.html",
    "test-login-debug.html",
    
    # Outputs de Teste
    "shared-tests-output.txt",
    "test-detailed-output.txt",
    "test-output-detailed.txt",
    "test-result-shared.txt",
    "test-results-full.txt",
    "test-results-unit.txt",
    "test-results.txt",
    
    # Documentação Duplicada/Antiga
    "LEIA-ME-PRIMEIRO.md",
    "LEIA-ME.md",
    "INICIO_RAPIDO.md",
    "STARTUP_GUIDE.md",
    "COMO-USAR.md",
    "INDICE_DOCUMENTACAO.md",
    "DASHBOARD_MELHORIAS.md",
    "ANALISE_MELHORIAS_VS_IMPLEMENTACAO.md",
    
    # Guias de Implementação Específicos
    "DOCKER_BUILD_GUIDE.md",
    "HEALTH_CHECKS.md",
    "LOAD_TESTING.md",
    "LOGGING_IMPLEMENTATION.md",
    "METRICS_IMPLEMENTATION.md",
    "MONITORING_DASHBOARDS.md",
    "SECURITY_GUIDE.md",
    "SECURITY_TESTING.md",
    "MIGRACAO_SQL_SERVER.md",
    "TASK_20.4_SECURITY_TESTING_SUMMARY.md",
    "TASK_20.5_LOAD_TESTING_SUMMARY.md",
    "VISUAL_IMPROVEMENTS_SUMMARY.md",
    "TESTE_LOCALSTORAGE.md",
    
    # SQL de Seed Manual
    "seed-database-manually.sql",
    
    # Scripts Shell
    "restart-services.sh",
    
    # Configuração SonarQube
    "sonar-project.properties",
    
    # Arquivos C# de Validação Temporários
    "ValidateLevel1Content.cs",
    "tests/ContentSeederTests.cs",
    "tests/CurriculumAPIIntegrationTests.cs"
)

# Scripts Python para remover
$scriptsPythonParaRemover = @(
    "scripts/append_level5_lessons.py",
    "scripts/apply_lessons_9_20.py",
    "scripts/apply_level5_expansion.py",
    "scripts/batch_generate_level5_lessons.py",
    "scripts/complete_level5_generation.py",
    "scripts/complete_level5_lessons.py",
    "scripts/complete_level5_lessons.txt",
    "scripts/comprehensive_fix.py",
    "scripts/expand_all_remaining_lessons.py",
    "scripts/expand_lesson_content.py",
    "scripts/expand_lessons.py",
    "scripts/expand_level5_lessons_7_20.py",
    "scripts/expand_level5_lessons.py",
    "scripts/fix_compilation_errors.py",
    "scripts/fix_theory_sections.py",
    "scripts/gen_l5.py",
    "scripts/generate_all_12_lessons.py",
    "scripts/generate_all_levels.py",
    "scripts/generate_complete_lessons_9_20.py",
    "scripts/generate_lessons_9_20_complete.py",
    "scripts/generate_lessons_9_20_comprehensive.py",
    "scripts/generate_level_lessons.py",
    "scripts/generate_level5_complete.py",
    "scripts/generate_level5_lessons_6_20.py",
    "scripts/generate_level5_lessons_complete.py",
    "scripts/generate_remaining_curriculum.py",
    "scripts/generate_remaining_level5.py",
    "scripts/regenerate_level5.py",
    "scripts/level5_lessons_7_20_content.json",
    "scripts/level5_lessons_9_20_complete.json",
    "scripts/level5_premium_lessons_data.json",
    "scripts/quick_validate.py",
    "scripts/validate_all_levels.py",
    "scripts/validate_curriculum.py",
    "scripts/validate_level4_v2.py",
    "scripts/validate_level4.py",
    "scripts/validate_level5.py",
    "scripts/validate_phase2_checkpoint.py",
    "scripts/ValidateLevel4.cs"
)

$todosArquivos = $arquivosParaRemover + $scriptsPythonParaRemover

$removidos = 0
$naoEncontrados = 0
$erros = 0

Write-Host "📋 Total de arquivos para processar: $($todosArquivos.Count)`n" -ForegroundColor Cyan

foreach ($arquivo in $todosArquivos) {
    if (Test-Path $arquivo) {
        if ($DryRun) {
            Write-Host "  🗑️  Seria removido: $arquivo" -ForegroundColor Yellow
            $removidos++
        } else {
            if ($Confirm) {
                $resposta = Read-Host "Remover '$arquivo'? (S/N)"
                if ($resposta -ne 'S' -and $resposta -ne 's') {
                    Write-Host "  ⏭️  Pulado: $arquivo" -ForegroundColor Gray
                    continue
                }
            }
            
            try {
                Remove-Item -Path $arquivo -Force -ErrorAction Stop
                Write-Host "  ✅ Removido: $arquivo" -ForegroundColor Green
                $removidos++
            } catch {
                Write-Host "  ❌ Erro ao remover: $arquivo - $($_.Exception.Message)" -ForegroundColor Red
                $erros++
            }
        }
    } else {
        $naoEncontrados++
    }
}

# Resumo
Write-Host "`n╔═══════════════════════════════════════════════════════════╗" -ForegroundColor Cyan
Write-Host "║                        RESUMO                             ║" -ForegroundColor Cyan
Write-Host "╚═══════════════════════════════════════════════════════════╝`n" -ForegroundColor Cyan

if ($DryRun) {
    Write-Host "  📊 Arquivos que seriam removidos: $removidos" -ForegroundColor Yellow
} else {
    Write-Host "  ✅ Arquivos removidos: $removidos" -ForegroundColor Green
}
Write-Host "  ℹ️  Arquivos não encontrados: $naoEncontrados" -ForegroundColor Gray
if ($erros -gt 0) {
    Write-Host "  ❌ Erros: $erros" -ForegroundColor Red
}

Write-Host "`n✨ Limpeza concluída!`n" -ForegroundColor Green

if ($DryRun) {
    Write-Host "💡 Para remover os arquivos, execute sem -DryRun:`n" -ForegroundColor Cyan
    Write-Host "   .\limpar-arquivos-obsoletos.ps1`n" -ForegroundColor White
}
