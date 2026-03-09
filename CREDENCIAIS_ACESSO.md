# 🔐 Credenciais de Acesso - AspNet Learning Platform

## ⚠️ IMPORTANTE: Auth Service em Modo de Desenvolvimento

O Auth Service está atualmente usando **armazenamento em memória** (não persistente) para autenticação. Isso significa que:

- ✅ Você pode criar novos usuários através do frontend
- ✅ Os usuários criados funcionam durante a sessão
- ❌ Os usuários são perdidos quando o serviço é reiniciado
- ❌ Os usuários do database seeding NÃO estão disponíveis para login

## 🎯 Como Acessar a Plataforma

### Opção 1: Usar Usuários Padrão (Mais Rápido)

O sistema agora vem com 6 usuários de teste pré-configurados:

| Nome | Email | Senha |
|------|-------|-------|
| Alice Johnson | alice@example.com | password123 |
| Bob Smith | bob@example.com | securepass456 |
| Carol Davis | carol@example.com | mypassword789 |
| David Wilson | david@example.com | testpass321 |
| Emma Martinez | emma@example.com | demouser2024 |
| Test User | test@test.com | Test123! |

**Recomendado para testes rápidos**: Use `test@test.com` / `Test123!`

### Opção 2: Criar Nova Conta

1. Acesse http://localhost:3000
2. Clique em "Registrar" ou "Sign Up"
3. Preencha os dados:
   - Nome: Seu nome
   - Email: seu@email.com
   - Senha: mínimo 6 caracteres
4. Faça login com as credenciais criadas

## 🔧 Status Atual do Sistema

### Backend
- **Auth Service**: ✅ Funcionando (armazenamento em memória)
- **Outros Serviços**: ✅ Todos operacionais (7/7)
- **Database**: ✅ SQL Server rodando (mas Auth não usa ainda)

### Frontend
- **Next.js**: ✅ Funcionando em http://localhost:3000
- **Integração com API**: ✅ Conectado ao backend

## 📝 Notas Técnicas

### Por que Auth Service usa memória?

O código atual em `src/Services/Auth/Program.cs` usa:
```csharp
var users = new System.Collections.Concurrent.ConcurrentDictionary<string, (...)>();
```

Isso é uma implementação temporária para testes. Em produção, isso deve ser substituído por:
- Entity Framework Core com SQL Server
- Integração com o DbSeeder para usuários padrão
- Persistência de dados entre reinicializações

### Usuários do Seeding (Agora Disponíveis!)

Estes usuários foram adicionados ao Auth Service e estão prontos para uso:
- ✅ alice@example.com / password123
- ✅ bob@example.com / securepass456
- ✅ carol@example.com / mypassword789
- ✅ david@example.com / testpass321
- ✅ emma@example.com / demouser2024
- ✅ test@test.com / Test123!

## 🚀 Próximos Passos para Produção

Para tornar o Auth Service production-ready:

1. Substituir `ConcurrentDictionary` por Entity Framework
2. Conectar ao banco de dados SQL Server
3. Executar DbSeeder para criar usuários padrão
4. Implementar refresh tokens
5. Adicionar rate limiting
6. Implementar recuperação de senha

## ✅ Como Testar Agora

1. Acesse http://localhost:3000
2. Clique em "Registrar"
3. Crie uma nova conta
4. Faça login
5. Explore a plataforma!

---

**Última Atualização**: 08/03/2026 - 21:35  
**Status**: Sistema funcional com autenticação em memória
