# 🚀 Como Rodar o Projeto Completo

## ✅ Status Atual
- **Backend**: 8/8 serviços funcionando (100%)
- **Frontend**: Next.js configurado e pronto
- **Infraestrutura**: Docker com health checks (SQL Server, RabbitMQ, Redis)
- **Testes**: 816/818 passando (99.76%)
- **Compilação**: 100% sucesso

## 📋 Pré-requisitos

1. **Docker Desktop** - Deve estar rodando
2. **.NET 10 SDK** - Instalado
3. **Node.js v20+** - Instalado
4. **PowerShell 7+** - Instalado

## 🎯 Iniciar Tudo (Recomendado)

```powershell
.\start-all.ps1
```

Este comando inicia:
- ✅ Containers Docker (SQL Server, RabbitMQ, Redis) com health checks
- ✅ 8 microserviços backend (.NET) incluindo Worker Service
- ✅ Frontend Next.js (porta 3000)
- ✅ Verificação automática de saúde dos serviços

**Aguarde ~45 segundos** para todos os serviços iniciarem (SQL Server pode levar até 2 minutos na primeira vez).

## 🌐 Acessar a Plataforma

Após iniciar, acesse:

- **Frontend (Interface)**: http://localhost:3000
- **API Gateway**: http://localhost:5000
- **Swagger Documentation**: http://localhost:5000/swagger
- **RabbitMQ Management**: http://localhost:15672
  - User: `platform_user`
  - Password: `R@bb1tMQ!2026#Secure$Pass`

## 🔍 Verificar Status

```powershell
.\health-check.ps1
```

Deve mostrar todos os 8 serviços + frontend como "Saudável" ✅

## 🛑 Parar Tudo

```powershell
.\stop-all.ps1
```

Para todos os serviços e containers Docker.

## 🔄 Reiniciar

```powershell
.\restart-all.ps1
```

## 📊 Serviços Disponíveis

### Backend (.NET)
1. **ApiGateway** - http://localhost:5000 - Ponto de entrada principal
2. **Auth Service** - http://localhost:5001 - Autenticação e autorização
3. **Course Service** - http://localhost:5002 - Gerenciamento de cursos
4. **Progress Service** - http://localhost:5003 - Progresso do usuário
5. **Challenge Service** - http://localhost:5004 - Desafios de código
6. **AITutor Service** - http://localhost:5005 - Tutor com IA
7. **Execution Service** - http://localhost:5006 - Execução de código
8. **Worker Service** - Background - Processamento assíncrono

### Frontend
- **Next.js** - http://localhost:3000 - Interface do usuário

### Infraestrutura
- **SQL Server** - localhost:1433
  - User: `sa`
  - Password: `P@ssw0rd!2026#Secure$Platform`
- **RabbitMQ** - localhost:5672 (AMQP) / localhost:15672 (Management UI)
  - User: `platform_user`
  - Password: `R@bb1tMQ!2026#Secure$Pass`
- **Redis** - localhost:6379
  - Password: `R3d1s!S3cur3#2026$Pass`

## 🐛 Problemas Comuns

### 1. SQL Server demora para iniciar
**Sintoma**: Serviços não conectam ao banco

**Solução**:
```powershell
# O script aguarda automaticamente até 2 minutos
# Se ainda assim falhar, verifique:
.\health-check.ps1

# Teste a conexão manualmente:
docker exec aspnet-learning-sqlserver /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "P@ssw0rd!2026#Secure`$Platform" -Q "SELECT 1" -C

# Se necessário, reinicie o container:
docker restart aspnet-learning-sqlserver
```

### 2. Frontend não compila
**Sintoma**: Erro ao iniciar Next.js

**Solução**:
```powershell
cd frontend
npm install
cd ..
.\start-all.ps1 -SkipBuild
```

### 3. Porta já em uso
**Erro**: `address already in use`

**Solução**:
```powershell
.\stop-all.ps1
# Aguarde 5 segundos
.\start-all.ps1
```

### 4. Docker não está rodando
**Erro**: `Docker não está rodando`

**Solução**:
1. Abra o Docker Desktop
2. Aguarde ele iniciar completamente
3. Execute `.\start-all.ps1` novamente

### 5. Serviço não responde
**Solução**:
```powershell
# Verificar status
.\health-check.ps1

# Se algum serviço estiver com problema, reinicie
.\restart-all.ps1
```

## 📝 Scripts Disponíveis

| Script | Descrição |
|--------|-----------|
| `start-all.ps1` | Inicia tudo (backend + frontend + Docker) |
| `start-all.ps1 -SkipBuild` | Inicia sem recompilar |
| `start-all.ps1 -SkipDocker` | Inicia apenas serviços (Docker já rodando) |
| `stop-all.ps1` | Para tudo |
| `restart-all.ps1` | Reinicia tudo |
| `restart-all.ps1 -SkipBuild` | Reinicia sem recompilar |
| `health-check.ps1` | Verifica status de todos os serviços |
| `build-safe.ps1` | Compila o projeto sem executar |

## 🎓 Próximos Passos

1. Acesse http://localhost:3000
2. Crie uma conta ou faça login
3. Explore os cursos disponíveis
4. Comece a aprender!

## 📚 Documentação Adicional

- `GUIA_RAPIDO_INICIALIZACAO.md` - Guia rápido com comandos essenciais
- `SCRIPTS_ATUALIZADOS.md` - Detalhes das correções nos scripts
- `TODOS_TESTES_RESOLVIDOS.md` - Status dos testes
- `CONFIGURATION.md` - Configurações avançadas

## 💡 Dicas

- Use `.\health-check.ps1` frequentemente para monitorar os serviços
- Os serviços rodam em janelas minimizadas do PowerShell
- Use `.\stop-all.ps1` para fechar todas as janelas de uma vez
- O frontend tem hot-reload ativado (mudanças aparecem automaticamente)
- Containers Docker agora têm health checks configurados
- Todas as credenciais foram padronizadas entre os arquivos

## 🎉 Tudo Pronto!

A plataforma está 100% funcional e pronta para uso. Divirta-se aprendendo! 🚀
