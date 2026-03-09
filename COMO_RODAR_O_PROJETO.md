# 🚀 Como Rodar o Projeto Completo

## ✅ Status Atual
- **Backend**: 7/7 serviços funcionando (100%)
- **Frontend**: Configurado e pronto
- **Infraestrutura**: Docker rodando (SQL Server, RabbitMQ, Redis)

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
- ✅ Containers Docker (SQL Server, RabbitMQ, Redis)
- ✅ 7 microserviços backend (.NET)
- ✅ 1 worker service (background)
- ✅ Frontend Next.js

**Aguarde ~30 segundos** para todos os serviços iniciarem.

## 🌐 Acessar a Plataforma

Após iniciar, acesse:

- **Frontend (Interface)**: http://localhost:3000
- **API Gateway**: http://localhost:5000
- **RabbitMQ Management**: http://localhost:15672
  - User: `platform_user`
  - Password: `SimplePass123`

## 🔍 Verificar Status

```powershell
.\health-check.ps1
```

Deve mostrar todos os 7 serviços como "Saudável" ✅

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
  - Password: `P@ssw0rd!2026#SecurePlatform`
- **RabbitMQ** - localhost:5672 (AMQP) / localhost:15672 (Management UI)
- **Redis** - localhost:6379

## 🐛 Problemas Comuns

### 1. ~~Frontend não inicia~~ ✅ RESOLVIDO
O problema de instalação do Next.js foi corrigido. O script agora usa o Node.js diretamente.

### 2. Porta já em uso
**Erro**: `address already in use`

**Solução**:
```powershell
.\stop-all.ps1
# Aguarde 5 segundos
.\start-all.ps1
```

### 3. Docker não está rodando
**Erro**: `Docker não está rodando`

**Solução**:
1. Abra o Docker Desktop
2. Aguarde ele iniciar completamente
3. Execute `.\start-all.ps1` novamente

### 4. Serviço não responde
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
| `stop-all.ps1` | Para tudo |
| `restart-all.ps1` | Reinicia tudo |
| `health-check.ps1` | Verifica status de todos os serviços |
| `start-frontend.ps1` | Inicia apenas o frontend |
| `build-safe.ps1` | Compila o projeto sem executar |

## 🎓 Próximos Passos

1. Acesse http://localhost:3000
2. Crie uma conta ou faça login
3. Explore os cursos disponíveis
4. Comece a aprender!

## 📚 Documentação Adicional

- `STATUS_PRODUCAO_FINAL.md` - Status detalhado da plataforma
- `FRONTEND_README.md` - Guia específico do frontend
- `CONFIGURATION.md` - Configurações avançadas

## 💡 Dicas

- Use `.\health-check.ps1` frequentemente para monitorar os serviços
- Os serviços rodam em janelas minimizadas do PowerShell
- Logs são salvos automaticamente em `logs/`
- O frontend tem hot-reload ativado (mudanças aparecem automaticamente)

## 🎉 Tudo Pronto!

A plataforma está 100% funcional e pronta para uso. Divirta-se aprendendo! 🚀
