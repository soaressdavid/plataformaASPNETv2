# Comandos Rápidos - Referência

## 🚀 Início Rápido

### ⭐ NOVO: Iniciar Tudo com 1 Comando
```powershell
.\start-all.ps1
```
Este comando inicia TUDO: Docker, compilação e todos os 8 microserviços!

### Parar Tudo
```powershell
.\stop-all.ps1
```

### Reiniciar Tudo
```powershell
.\restart-all.ps1
```

### Verificar Saúde
```powershell
.\health-check.ps1
```

### Ver Logs
```powershell
.\logs.ps1 -Docker
```

---

## 📖 Guia Completo
Veja **START.md** para instruções detalhadas de uso dos novos scripts.

---

## 🔧 Comandos Manuais (Legado)

---

## 🔧 Compilação

### Build Completo
```powershell
dotnet clean
dotnet build --no-incremental
```

### Build Específico
```powershell
dotnet build src/Shared/Shared.csproj
dotnet build src/ApiGateway/ApiGateway.csproj
dotnet build src/Services/Course/Course.Service.csproj
```

### Verificar Erros
```powershell
dotnet build 2>&1 | Select-String "error"
```

### Contar Warnings
```powershell
dotnet build 2>&1 | Select-String "warning" | Measure-Object
```

---

## 🧪 Testes

### Rodar Todos
```powershell
dotnet test --verbosity normal
```

### Rodar Específico
```powershell
dotnet test tests/Course.Tests/Course.Tests.csproj
dotnet test tests/Shared.Tests/Shared.Tests.csproj
```

### Ver Falhas
```powershell
dotnet test --verbosity detailed | Select-String "Failed"
```

---

## 🐳 Docker

### Iniciar
```powershell
docker-compose up -d
```

### Parar
```powershell
docker-compose down
```

### Ver Logs
```powershell
docker-compose logs -f
```

### Reiniciar
```powershell
docker-compose restart
```

### Limpar Tudo
```powershell
docker-compose down -v
```

---

## 🔍 Verificações

### Senhas Hardcoded
```powershell
# Não deve retornar nada
git grep -i "password.*=" -- "*.json" "*.yml" | Select-String -NotMatch ".env|.example|\${"
```

### Arquivos .env
```powershell
cat .env
```

### Verificar .gitignore
```powershell
cat .gitignore | Select-String ".env"
```

---

## 🏃 Executar Serviços

### ApiGateway
```powershell
dotnet run --project src/ApiGateway
```

### CourseService
```powershell
dotnet run --project src/Services/Course
```

### AuthService
```powershell
dotnet run --project src/Services/Auth
```

### ProgressService
```powershell
dotnet run --project src/Services/Progress
```

---

## 🔄 Limpeza

### Limpar Build
```powershell
dotnet clean
```

### Limpar Completo
```powershell
Get-Process dotnet -ErrorAction SilentlyContinue | Stop-Process -Force
dotnet clean
Remove-Item -Path "*/bin" -Recurse -Force -ErrorAction SilentlyContinue
Remove-Item -Path "*/obj" -Recurse -Force -ErrorAction SilentlyContinue
```

---

## 📊 Análise

### Warnings por Arquivo
```powershell
dotnet build 2>&1 | Select-String "warning" | ForEach-Object {
    if ($_ -match '([^(]+)\(') { $matches[1] }
} | Group-Object | Sort-Object Count -Descending
```

### Erros por Tipo
```powershell
dotnet build 2>&1 | Select-String "error" | ForEach-Object {
    if ($_ -match 'error (CS\d+)') { $matches[1] }
} | Group-Object | Sort-Object Count -Descending
```

---

## 🚀 Deploy

### Build Release
```powershell
dotnet build --configuration Release
```

### Publish
```powershell
dotnet publish src/ApiGateway/ApiGateway.csproj -c Release -o publish/apigateway
dotnet publish src/Services/Course/Course.Service.csproj -c Release -o publish/courseservice
```

### Deploy Completo
```powershell
.\deploy.ps1
```

---

## 🔐 Segurança

### Gerar Senha Segura
```powershell
-join ((48..57) + (65..90) + (97..122) + (33,35,36,37,38,42,43,45,61,63,64) | Get-Random -Count 32 | ForEach-Object {[char]$_})
```

### Verificar Configuração
```powershell
# Verificar se senhas estão em variáveis
cat src/ApiGateway/appsettings.json | Select-String "Password"
cat src/Services/Course/appsettings.json | Select-String "Password"
```

---

## 📝 Git

### Status
```powershell
git status
```

### Adicionar Tudo (exceto .env)
```powershell
git add .
git status
```

### Commit
```powershell
git commit -m "feat: correções críticas de segurança e compilação"
```

### Ver Mudanças
```powershell
git diff
```

---

## 🎯 Atalhos Úteis

### Verificação Rápida
```powershell
# Tudo em um comando
.\verificar-status-final.ps1; dotnet build --no-incremental
```

### Reiniciar Completo
```powershell
# Parar tudo, limpar, recompilar
Get-Process dotnet -ErrorAction SilentlyContinue | Stop-Process -Force
docker-compose down
dotnet clean
dotnet build --no-incremental
docker-compose up -d
```

### Health Check Completo
```powershell
# Verificar tudo
.\verificar-status-final.ps1
.\health-check.ps1
dotnet build --no-incremental
```

---

## 📚 Documentação

### Ver Análise Completa
```powershell
cat ANALISE_COMPLETA_MELHORIAS_OBRIGATORIAS.md
```

### Ver Dashboard
```powershell
cat DASHBOARD_MELHORIAS.md
```

### Ver Status Final
```powershell
cat STATUS_FINAL_SISTEMA.md
```

### Ver Guia de Segurança
```powershell
cat SECURITY_GUIDE.md
```

---

## 🆘 Troubleshooting

### Porta em Uso
```powershell
# Ver processo na porta 5000
netstat -ano | findstr :5000

# Matar processo
Stop-Process -Id <PID> -Force
```

### Erro de Compilação
```powershell
# Limpar e recompilar
Get-Process dotnet -ErrorAction SilentlyContinue | Stop-Process -Force
dotnet clean
Start-Sleep -Seconds 2
dotnet build --no-incremental
```

### Docker Não Inicia
```powershell
# Verificar Docker
docker ps
docker-compose ps

# Reiniciar Docker
docker-compose down
docker-compose up -d
```

### Variáveis de Ambiente
```powershell
# Verificar se estão carregadas
[Environment]::GetEnvironmentVariable("DB_PASSWORD", "Process")
[Environment]::GetEnvironmentVariable("RABBITMQ_PASSWORD", "Process")
```

---

## 💡 Dicas

1. **Sempre verificar status antes de começar:**
   ```powershell
   .\verificar-status-final.ps1
   ```

2. **Parar processos antes de compilar:**
   ```powershell
   Get-Process dotnet -ErrorAction SilentlyContinue | Stop-Process -Force
   ```

3. **Usar scripts de automação:**
   ```powershell
   .\corrigir-compilacao-completa.ps1
   ```

4. **Verificar saúde regularmente:**
   ```powershell
   .\health-check.ps1
   ```

5. **Nunca commitar .env:**
   ```powershell
   git status  # Verificar antes de commit
   ```

---

**Última atualização:** 7 de março de 2026  
**Versão:** 1.0

