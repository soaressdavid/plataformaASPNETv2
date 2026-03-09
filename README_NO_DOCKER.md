# 🚀 Guia Rápido - Rodar Projeto SEM Docker

Este guia mostra como rodar o projeto em um computador **sem Docker instalado**.

---

## ⚡ Setup Ultra Rápido (1 COMANDO!)

### Passo 1: Clonar o repositório
```powershell
git clone https://github.com/seu-usuario/seu-repo.git
cd seu-repo
```

### Passo 2: Executar setup automático
```powershell
./setup-no-docker.ps1
```

**Pronto!** O script faz tudo automaticamente:
- ✅ Detecta e configura banco de dados (LocalDB ou SQLite)
- ✅ Cria arquivos de configuração
- ✅ Instala dependências (backend + frontend)
- ✅ Aplica migrations
- ✅ Libera portas em uso
- ✅ Inicia todos os serviços (10 backend + 1 frontend)
- ✅ Verifica saúde dos serviços

### Passo 3: Acessar aplicação
Aguarde 30 segundos e acesse: **http://localhost:3000**

---

## 📋 Pré-requisitos

Apenas 2 coisas:
- ✅ .NET 9 SDK ([Download](https://dotnet.microsoft.com/download))
- ✅ Node.js 18+ ([Download](https://nodejs.org/))

SQL Server LocalDB é opcional (vem com Visual Studio). Se não tiver, o script usa SQLite automaticamente.

---

## 🎯 O que o script faz?

1. **Verifica pré-requisitos** (.NET SDK, Node.js)
2. **Configura banco de dados** (LocalDB ou SQLite)
3. **Cria configurações** para cada serviço
4. **Instala dependências** (backend e frontend)
5. **Aplica migrations** do banco de dados
6. **Libera portas** em uso
7. **Inicia 10 serviços backend** (em janelas minimizadas)
8. **Inicia frontend** (Next.js)
9. **Verifica saúde** dos serviços
10. **Mostra resumo** e instruções

Tudo em **1 comando**! 🎉

---

## 👤 Usuários de teste

O sistema vem com usuários pré-cadastrados:

| Email | Senha |
|-------|-------|
| test@test.com | Test123! |
| alice@example.com | password123 |
| bob@example.com | securepass456 |

---

## 🐛 Problemas?

### Script falha ao executar
```powershell
# Permitir execução de scripts
Set-ExecutionPolicy -ExecutionPolicy RemoteSigned -Scope CurrentUser
```

### Porta em uso
O script libera automaticamente. Se persistir:
```powershell
./cleanup-no-docker.ps1
./setup-no-docker.ps1
```

### Serviço não inicia
Verifique as janelas minimizadas do PowerShell para ver os logs.

---

## 🔄 Parar/Resetar

### Parar tudo e limpar
```powershell
./cleanup-no-docker.ps1
```

### Resetar e reiniciar
```powershell
./cleanup-no-docker.ps1
./setup-no-docker.ps1
```

---

## 📊 Serviços Iniciados

O script inicia automaticamente:

| Serviço | Porta | Status |
|---------|-------|--------|
| ApiGateway | 5000 | ✅ |
| Auth | 5001 | ✅ |
| Course | 5002 | ✅ |
| Progress | 5003 | ✅ |
| Challenge | 5004 | ✅ |
| AITutor | 5005 | ✅ |
| Execution | 5006 | ✅ |
| SqlExecutor | 5008 | ✅ |
| Notification | 5009 | ✅ |
| Analytics | 5010 | ✅ |
| Frontend | 3000 | ✅ |

---

## ⚠️ Limitações

### Execution Service
- Precisa de Docker para containers isolados
- Funciona com limitações (executa no processo principal)

### SqlExecutor
- Precisa de Docker para containers SQL isolados
- Funciona com banco principal (menos isolamento)

Essas limitações não impedem o uso do sistema, apenas reduzem o isolamento.

---

## 🎓 Próximos passos

Depois de rodar o projeto:

1. Acesse http://localhost:3000
2. Faça login com `test@test.com` / `Test123!`
3. Explore os cursos e aulas
4. Teste os executores (SQL, Terminal, Azure)
5. Veja notificações e analytics

---

**Versão:** 2.0 (Setup Automático)  
**Data:** 09/03/2026  
**Status:** ✅ 1 comando para rodar tudo!
