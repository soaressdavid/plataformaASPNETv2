# 🚀 AspNet Learning Platform - Guia de Inicialização

## ⚡ Início Ultra-Rápido

### Opção 1: Duplo Clique (Windows)
1. Dê duplo clique em **`START.cmd`**
2. Aguarde ~30 segundos
3. Acesse http://localhost:5000/swagger

### Opção 2: PowerShell (Recomendado)
```powershell
.\start-all.ps1
```

### Opção 3: Sem recompilar (mais rápido)
```powershell
.\start-all.ps1 -SkipBuild
```

---

## 📦 O que é iniciado?

Quando você executa `start-all.ps1`, o seguinte acontece automaticamente:

### 1️⃣ Infraestrutura Docker
- 🗄️ **SQL Server** (porta 1433) - Banco de dados
- 🐰 **RabbitMQ** (porta 5672, UI: 15672) - Message broker
- 📦 **Redis** (porta 6379) - Cache

### 2️⃣ Microserviços .NET
- 🌐 **ApiGateway** (porta 5000) - Gateway principal
- 🔐 **Auth Service** (porta 5001) - Autenticação/Autorização
- 📚 **Course Service** (porta 5002) - Gerenciamento de cursos
- 📊 **Progress Service** (porta 5003) - Progresso do usuário
- 🎯 **Challenge Service** (porta 5004) - Desafios de código
- ⚙️ **Execution Service** (porta 5005) - Execução de código
- 🤖 **AITutor Service** (porta 5006) - Tutor com IA
- 👷 **Worker Service** (porta 5007) - Processamento background

---

## 🎮 Comandos Disponíveis

| Comando | Descrição | Tempo |
|---------|-----------|-------|
| `.\start-all.ps1` | Inicia tudo do zero | ~60s |
| `.\start-all.ps1 -SkipBuild` | Inicia sem recompilar | ~30s |
| `.\stop-all.ps1` | Para tudo | ~5s |
| `.\restart-all.ps1` | Reinicia tudo | ~65s |
| `.\health-check.ps1` | Verifica saúde | ~5s |
| `.\logs.ps1 -Docker` | Exibe logs | - |

---

## 🌐 URLs de Acesso

### Aplicação
- **Swagger UI**: http://localhost:5000/swagger
- **ApiGateway**: http://localhost:5000
- **Health Check**: http://localhost:5000/health

### Infraestrutura
- **RabbitMQ Management**: http://localhost:15672
  - Usuário: `guest`
  - Senha: `guest`

### Serviços Individuais
- Auth: http://localhost:5001/health
- Course: http://localhost:5002/health
- Progress: http://localhost:5003/health
- Challenge: http://localhost:5004/health
- Execution: http://localhost:5005/health
- AITutor: http://localhost:5006/health
- Worker: http://localhost:5007/health

---

## 📋 Pré-requisitos

Antes de iniciar, certifique-se de ter instalado:

1. **[.NET 10 SDK](https://dotnet.microsoft.com/download)**
   ```powershell
   dotnet --version  # Deve mostrar 10.x.x
   ```

2. **[Docker Desktop](https://www.docker.com/products/docker-desktop)**
   ```powershell
   docker --version  # Deve funcionar
   ```

3. **PowerShell 7+** (opcional, mas recomendado)
   ```powershell
   $PSVersionTable.PSVersion  # Deve mostrar 7.x
   ```

---

## 🔧 Configuração Inicial

### 1. Clonar o Repositório
```powershell
git clone <repository-url>
cd AspNetLearningPlatform
```

### 2. Configurar Variáveis de Ambiente
O arquivo `.env` já está configurado. Se precisar alterar:
```powershell
notepad .env
```

### 3. Iniciar pela Primeira Vez
```powershell
.\start-all.ps1
```

Aguarde aproximadamente 60 segundos na primeira execução (compilação + inicialização).

### 4. Verificar Saúde
```powershell
.\health-check.ps1
```

Você deve ver todos os serviços com ✅.

---

## 🎯 Fluxo de Trabalho Típico

### Manhã (Iniciar trabalho)
```powershell
# Se não mudou código desde ontem
.\start-all.ps1 -SkipBuild

# Se mudou código
.\start-all.ps1
```

### Durante o Dia (Desenvolvimento)
```powershell
# Após fazer mudanças no código
.\restart-all.ps1

# Verificar se tudo está OK
.\health-check.ps1

# Ver logs se algo der errado
.\logs.ps1 -Docker
```

### Noite (Finalizar trabalho)
```powershell
# Parar tudo
.\stop-all.ps1

# OU simplesmente pressione Ctrl+C no terminal do start-all.ps1
```

---

## 🐛 Troubleshooting

### Problema: "Porta já em uso"
```powershell
# Parar todos os processos dotnet
Get-Process dotnet -ErrorAction SilentlyContinue | Stop-Process -Force

# Verificar portas em uso
netstat -ano | findstr "5000 5001 5002"
```

### Problema: "Docker não inicia"
```powershell
# Verificar se Docker está rodando
docker ps

# Se não estiver, abra o Docker Desktop

# Reiniciar containers
docker-compose down
docker-compose up -d
```

### Problema: "Erro de compilação"
```powershell
# Limpar tudo e recompilar
dotnet clean
Remove-Item -Path "*/bin","*/obj" -Recurse -Force -ErrorAction SilentlyContinue
dotnet build --no-incremental
```

### Problema: "Serviço não responde"
```powershell
# Aguardar mais tempo (primeira inicialização pode levar 60s)
Start-Sleep -Seconds 30

# Verificar novamente
.\health-check.ps1

# Ver logs para diagnóstico
.\logs.ps1 -Docker
```

### Problema: "Erro de permissão no PowerShell"
```powershell
# Executar como administrador ou ajustar política
Set-ExecutionPolicy -ExecutionPolicy RemoteSigned -Scope CurrentUser
```

---

## 📊 Monitoramento

### Ver Status em Tempo Real
```powershell
# Executar em um terminal separado
while ($true) {
    Clear-Host
    .\health-check.ps1
    Start-Sleep -Seconds 10
}
```

### Ver Logs Contínuos
```powershell
# Docker logs
docker-compose logs -f

# Logs de um serviço específico
docker-compose logs -f sqlserver
docker-compose logs -f rabbitmq
```

---

## 🎨 Estrutura de Arquivos

```
AspNetLearningPlatform/
│
├── START.cmd              ← Duplo clique para iniciar (Windows)
├── STOP.cmd               ← Duplo clique para parar (Windows)
│
├── start-all.ps1          ← Script principal de inicialização
├── stop-all.ps1           ← Para todos os serviços
├── restart-all.ps1        ← Reinicia tudo
├── health-check.ps1       ← Verifica saúde dos serviços
├── logs.ps1               ← Exibe logs
│
├── START.md               ← Guia detalhado de uso
├── QUICK-START.txt        ← Referência rápida
├── COMANDOS_RAPIDOS.md    ← Todos os comandos
│
├── docker-compose.yml     ← Configuração Docker
├── .env                   ← Variáveis de ambiente
│
└── src/                   ← Código fonte
    ├── ApiGateway/
    ├── Services/
    │   ├── Auth/
    │   ├── Course/
    │   ├── Progress/
    │   ├── Challenge/
    │   ├── Execution/
    │   ├── AITutor/
    │   └── Worker/
    └── Shared/
```

---

## 💡 Dicas e Boas Práticas

### ✅ Faça
- Use `.\start-all.ps1 -SkipBuild` quando não mudou código
- Execute `.\health-check.ps1` após iniciar
- Aguarde 30-60 segundos na primeira inicialização
- Use Ctrl+C para parar de forma limpa
- Verifique logs com `.\logs.ps1 -Docker` se algo falhar

### ❌ Evite
- Fechar o terminal abruptamente (use Ctrl+C)
- Iniciar múltiplas vezes sem parar antes
- Modificar código enquanto serviços estão rodando
- Esquecer de parar Docker ao desligar o PC

---

## 🚀 Próximos Passos

Após iniciar com sucesso:

1. **Explore a API**
   - Acesse http://localhost:5000/swagger
   - Teste os endpoints disponíveis

2. **Autentique-se**
   - Use o endpoint `/api/auth/login`
   - Obtenha um token JWT

3. **Teste Funcionalidades**
   - Crie um curso
   - Registre progresso
   - Execute código
   - Interaja com o AI Tutor

4. **Monitore**
   - Veja mensagens no RabbitMQ
   - Verifique dados no SQL Server
   - Monitore cache no Redis

---

## 📚 Documentação Adicional

- **START.md** - Guia completo de inicialização
- **COMANDOS_RAPIDOS.md** - Referência de todos os comandos
- **CONFIGURATION.md** - Configuração detalhada
- **ANALISE_COMPLETA_MELHORIAS_OBRIGATORIAS.md** - Análise técnica
- **QUICK-START.txt** - Referência visual rápida

---

## 🆘 Suporte

Se encontrar problemas:

1. Execute `.\health-check.ps1` para diagnóstico
2. Veja logs com `.\logs.ps1 -Docker`
3. Consulte a seção Troubleshooting acima
4. Verifique a documentação em START.md

---

## ✨ Resumo

**Para iniciar tudo:**
```powershell
.\start-all.ps1
```

**Para parar tudo:**
```powershell
Ctrl+C  # ou  .\stop-all.ps1
```

**Para verificar:**
```powershell
.\health-check.ps1
```

**Acesse:**
http://localhost:5000/swagger

---

**Última atualização:** 8 de março de 2026  
**Versão:** 2.0
