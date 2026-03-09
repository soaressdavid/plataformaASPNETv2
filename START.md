# 🚀 Como Iniciar o Projeto

## Início Rápido (1 Comando)

```powershell
.\start-all.ps1
```

Isso vai:
1. ✅ Carregar variáveis de ambiente (.env)
2. ✅ Iniciar Docker (SQL Server, RabbitMQ, Redis)
3. ✅ Compilar todos os projetos
4. ✅ Iniciar todos os 8 microserviços
5. ✅ Verificar saúde dos serviços

---

## 📋 Pré-requisitos

Antes de executar, certifique-se de ter:

- ✅ [.NET 10 SDK](https://dotnet.microsoft.com/download)
- ✅ [Docker Desktop](https://www.docker.com/products/docker-desktop)
- ✅ PowerShell 7+ (já vem no Windows 11)

---

## 🎯 Comandos Principais

### Iniciar Tudo
```powershell
.\start-all.ps1
```

### Parar Tudo
```powershell
# Pressione Ctrl+C no terminal onde rodou start-all.ps1
# OU execute:
.\stop-all.ps1
```

### Reiniciar
```powershell
.\restart-all.ps1
```

### Verificar Saúde
```powershell
.\health-check.ps1
```

### Ver Logs do Docker
```powershell
.\logs.ps1 -Docker
```

---

## 🔧 Opções Avançadas

### Iniciar sem recompilar (mais rápido)
```powershell
.\start-all.ps1 -SkipBuild
```

### Iniciar sem Docker (se já está rodando)
```powershell
.\start-all.ps1 -SkipDocker
```

### Iniciar apenas serviços .NET
```powershell
.\start-all.ps1 -ServicesOnly
```

---

## 🌐 URLs dos Serviços

Após iniciar, acesse:

| Serviço | URL | Descrição |
|---------|-----|-----------|
| **ApiGateway** | http://localhost:5000 | Gateway principal |
| **Swagger** | http://localhost:5000/swagger | Documentação API |
| Auth | http://localhost:5001 | Autenticação |
| Course | http://localhost:5002 | Cursos |
| Progress | http://localhost:5003 | Progresso |
| Challenge | http://localhost:5004 | Desafios |
| Execution | http://localhost:5005 | Execução de código |
| AITutor | http://localhost:5006 | Tutor IA |
| Worker | http://localhost:5007 | Worker background |

### Infraestrutura

| Serviço | URL | Credenciais |
|---------|-----|-------------|
| **RabbitMQ** | http://localhost:15672 | guest / guest |
| **SQL Server** | localhost:1433 | sa / (ver .env) |
| **Redis** | localhost:6379 | - |

---

## 🐛 Troubleshooting

### Porta já em uso
```powershell
# Ver processo na porta
netstat -ano | findstr :5000

# Parar todos os processos dotnet
Get-Process dotnet | Stop-Process -Force
```

### Docker não inicia
```powershell
# Verificar se Docker está rodando
docker ps

# Reiniciar Docker
docker-compose down
docker-compose up -d
```

### Erro de compilação
```powershell
# Limpar e recompilar
dotnet clean
dotnet build --no-incremental
```

### Serviço não responde
```powershell
# Verificar saúde
.\health-check.ps1

# Aguardar mais tempo (serviços podem levar 30s para iniciar)
Start-Sleep -Seconds 30
.\health-check.ps1
```

---

## 📝 Fluxo de Trabalho Típico

### Primeira vez
```powershell
# 1. Clonar repositório
git clone <repo-url>
cd AspNetLearningPlatform

# 2. Configurar .env (se necessário)
cp .env.example .env

# 3. Iniciar tudo
.\start-all.ps1

# 4. Aguardar ~30 segundos

# 5. Verificar saúde
.\health-check.ps1

# 6. Acessar http://localhost:5000/swagger
```

### Desenvolvimento diário
```powershell
# Iniciar (pula compilação se não mudou código)
.\start-all.ps1 -SkipBuild

# Trabalhar...

# Parar ao final do dia
.\stop-all.ps1
```

### Após mudanças no código
```powershell
# Reiniciar com recompilação
.\restart-all.ps1
```

---

## 🎨 Estrutura dos Scripts

```
start-all.ps1      → Inicia tudo (comando principal)
stop-all.ps1       → Para tudo
restart-all.ps1    → Reinicia tudo
health-check.ps1   → Verifica saúde dos serviços
logs.ps1           → Exibe logs
```

---

## 💡 Dicas

1. **Sempre use `.\start-all.ps1`** - É o jeito mais fácil
2. **Aguarde 30 segundos** - Serviços levam tempo para iniciar
3. **Use `health-check.ps1`** - Para verificar se tudo está OK
4. **Pressione Ctrl+C** - Para parar tudo de forma limpa
5. **Veja os logs** - Eles aparecem no terminal do start-all.ps1

---

## 🆘 Precisa de Ajuda?

1. Execute `.\health-check.ps1` para diagnóstico
2. Veja logs com `.\logs.ps1 -Docker`
3. Consulte `COMANDOS_RAPIDOS.md` para mais comandos
4. Verifique `CONFIGURATION.md` para configuração detalhada

---

## ✨ Pronto!

Agora você pode iniciar todo o projeto com apenas:

```powershell
.\start-all.ps1
```

E acessar em: **http://localhost:5000/swagger**

---

**Última atualização:** 8 de março de 2026
