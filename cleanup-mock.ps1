# ============================================
# CLEANUP MOCK - PARAR TODOS OS SERVIÇOS
# ============================================

Write-Host "🛑 Parando todos os serviços MOCK..." -ForegroundColor Yellow

# Parar processos nas portas
$ports = @(5000, 5001, 5002, 5006, 5008, 3000)
$stopped = 0

foreach ($port in $ports) {
    try {
        $connection = Get-NetTCPConnection -LocalPort $port -ErrorAction SilentlyContinue
        if ($connection) {
            Stop-Process -Id $connection.OwningProcess -Force -ErrorAction SilentlyContinue
            Write-Host "  ✅ Porta $port liberada" -ForegroundColor Green
            $stopped++
        }
    } catch {
        # Ignorar erros
    }
}

Write-Host "`n✅ $stopped serviços parados" -ForegroundColor Green
Write-Host "🎯 Ambiente limpo e pronto para novo setup" -ForegroundColor Cyan