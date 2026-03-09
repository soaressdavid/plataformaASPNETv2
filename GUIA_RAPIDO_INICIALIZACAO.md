# 🚀 Guia Rápido de Inicialização

## Pré-requisitos

- ✅ Docker Desktop instalado e rodando
- ✅ .NET 10.0 SDK instalado
- ✅ Node.js 18+ instalado
- ✅ PowerShell 7+ (recomendado)

## Iniciar o Projeto (Primeira Vez)

```powershell
# 1. Instalar dependências do frontend
cd frontend
npm install
cd ..

# 2. Iniciar tudo
.\start-all.ps1
```

O script irá:
1. ✅ Carregar variáveis de ambiente do `.env`
2. ✅ Iniciar containers Docker (SQL Server, RabbitMQ, Redis)
3. ✅ Aguardar SQL Server estar pronto (até 2 minutos)
4. ✅ Compilar todos os projetos .NET
5. ✅ Iniciar 8 microserviços backend
6. ✅ Iniciar frontend Next.js
7. ✅ Verificar saúde dos serviços

## Iniciar Rapidamente (Sem Recompilar)

```powershell
.\start-all.ps1 -SkipBuild
```

## Verificar Status

```powershell
.\health-check.ps1
```

Exemplo de saída:
```
╔═══════════════════════════════════════════════════════════╗
║              HEALTH CHECK - Todos os Serviços             ║
╚═══════════════════════════════════════════════════════════╝

Verificando Frontend (Next.js)      ... ✅ Saudável
Verificando ApiGateway               ... ✅ Saudável
Verificando Auth Service             ... ✅ Saudável
Verificando Course Service           ... ✅ Saudável
Verificando Progress Service         ... ✅ Saudável
Verificando Challenge Service        ... ✅ Saudável
Verificando AITutor Service          ... ✅ Saudável
Verificando Execution Service        ... ✅ Saudável

--- Infraestrutura Docker ---

SQL Server                   ... ✅ Saudável
RabbitMQ                     ... ✅ Saudável
Redis                        ... ✅ Saudável
```

## Parar Tudo

```powershell
.\stop-all.ps1
```

## Reiniciar

```powershell
# Com recompilação
.\restart-all.ps1

# Sem recompilação
.\restart-all.ps1 -SkipBuild
```

## URLs de Acesso

### Frontend
- 🌐 **Aplicação**: http://localhost:3000

### Backend
- 🚀 **API Gateway**: http://localhost:5000
- 📚 **Swagger**: http://localhost:5000/swagger
- 🔐 **Auth Service**: http://localhost:5001
- 📖 **Course Service**: http://localhost:5002
- 📊 **Progress Service**: http://localhost:5003
- 🎯 **Challenge Service**: http://localhost:5004
- 🤖 **AITutor Service**: http://localhost:5005
- ⚙️ **Execution Service**: http://localhost:5006
- 🔄 **Worker Service**: http://localhost:5007

### Infraestrutura
- 🗄️ **SQL Server**: localhost:1433
- 🐰 **RabbitMQ Management**: http://localhost:15672
  - User: `platform_user`
  - Password: `R@bb1tMQ!2026#Secure$Pass`
- 📦 **Redis**: localhost:6379

## Credenciais de Acesso

### SQL Server
```
Server: localhost,1433
Database: aspnet_learning_platform
User: sa
Password: P@ssw0rd!2026#Secure$Platform
```

### RabbitMQ
```
Host: localhost:5672
Management UI: http://localhost:15672
User: platform_user
Password: R@bb1tMQ!2026#Secure$Pass
```

### Redis
```
Host: localhost:6379
Password: R3d1s!S3cur3#2026$Pass
```

## Troubleshooting

### ❌ SQL Server não inicia

```powershell
# Verificar logs do container
docker logs aspnet-learning-sqlserver

# Reiniciar container
docker restart aspnet-learning-sqlserver

# Recriar container (CUIDADO: apaga dados)
docker-compose down -v
docker-compose up -d
```

### ❌ Frontend não compila

```powershell
# Reinstalar dependências
cd frontend
rm -rf node_modules
rm package-lock.json
npm install
cd ..

# Iniciar novamente
.\start-all.ps1 -SkipBuild
```

### ❌ Serviços não conectam ao banco

```powershell
# 1. Verificar se SQL Server está saudável
.\health-check.ps1

# 2. Testar conexão manualmente
docker exec aspnet-learning-sqlserver /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "P@ssw0rd!2026#Secure`$Platform" -Q "SELECT 1" -C

# 3. Se falhar, aguardar mais tempo ou reiniciar
docker restart aspnet-learning-sqlserver
Start-Sleep -Seconds 30
.\start-all.ps1 -SkipDocker -SkipBuild
```

### ❌ Porta já está em uso

```powershell
# Verificar qual processo está usando a porta (exemplo: 5000)
netstat -ano | findstr :5000

# Parar o processo (substitua PID pelo número retornado)
Stop-Process -Id PID -Force

# Ou parar tudo e tentar novamente
.\stop-all.ps1
.\start-all.ps1
```

### ❌ Docker não está rodando

```
❌ Docker não está rodando. Inicie o Docker Desktop e tente novamente.
```

**Solução**: Abra o Docker Desktop e aguarde ele iniciar completamente.

## Comandos Úteis

### Ver logs de um container
```powershell
docker logs aspnet-learning-sqlserver
docker logs aspnet-learning-rabbitmq
docker logs aspnet-learning-redis
```

### Ver logs em tempo real
```powershell
docker logs -f aspnet-learning-sqlserver
```

### Conectar ao SQL Server
```powershell
docker exec -it aspnet-learning-sqlserver /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "P@ssw0rd!2026#Secure`$Platform" -C
```

### Conectar ao Redis
```powershell
docker exec -it aspnet-learning-redis redis-cli -a "R3d1s!S3cur3#2026`$Pass"
```

### Ver containers rodando
```powershell
docker ps
```

### Ver uso de recursos
```powershell
docker stats
```

## Desenvolvimento

### Rodar testes
```powershell
# Todos os testes
dotnet test

# Testes de um projeto específico
dotnet test tests/Course.Tests

# Com cobertura
dotnet test --collect:"XPlat Code Coverage"
```

### Compilar sem iniciar
```powershell
dotnet build
```

### Limpar builds
```powershell
dotnet clean
```

### Restaurar pacotes
```powershell
dotnet restore
```

## Estrutura de Janelas

Quando você executa `.\start-all.ps1`, o script abre várias janelas do PowerShell minimizadas:

- 1 janela para cada microserviço backend (8 janelas)
- 1 janela para o frontend Next.js

**Total**: 9 janelas minimizadas

Para ver os logs de um serviço específico, basta restaurar a janela correspondente.

## Próximos Passos

1. ✅ Execute `.\start-all.ps1`
2. ✅ Aguarde a mensagem de sucesso
3. ✅ Acesse http://localhost:3000
4. ✅ Explore a aplicação!

## Status do Projeto

- ✅ **Compilação**: 100% sucesso
- ✅ **Testes**: 816/818 passando (99.76%)
- ✅ **Testes de Integração**: 100% funcionando
- ✅ **Docker**: Configurado com health checks
- ✅ **Scripts**: Atualizados e testados
- ✅ **Credenciais**: Padronizadas em todos os arquivos

## Suporte

Se encontrar problemas:

1. Execute `.\health-check.ps1` para diagnóstico
2. Verifique os logs dos containers: `docker logs <container-name>`
3. Tente reiniciar: `.\restart-all.ps1`
4. Se persistir, recrie tudo:
   ```powershell
   .\stop-all.ps1
   docker-compose down -v
   .\start-all.ps1
   ```

---

**Dica**: Mantenha o Docker Desktop aberto enquanto desenvolve para evitar problemas de conexão.
