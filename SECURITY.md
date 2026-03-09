# 🔒 Política de Segurança

## ⚠️ Aviso Importante

Este é um projeto educacional/demonstração. As credenciais incluídas no código são apenas para fins de desenvolvimento local e NÃO devem ser usadas em produção.

## 🔐 Credenciais de Desenvolvimento

### Senhas Hardcoded (Apenas para Desenvolvimento Local)

As seguintes credenciais estão hardcoded no código para facilitar o setup local:

#### SQL Server
- **Usuário**: `sa`
- **Senha**: `P@ssw0rd!2026#SecurePlatform`
- **Porta**: 1433

#### RabbitMQ
- **Usuário**: `guest`
- **Senha**: `SimplePass123`
- **Porta**: 5672
- **Management UI**: 15672

#### Usuários de Teste
- `test@test.com` / `Test123!`
- `alice@example.com` / `password123`
- `bob@example.com` / `securepass456`

## 🚨 NUNCA Use Estas Credenciais em Produção!

### Para Produção, você DEVE:

1. **Usar Variáveis de Ambiente**
   ```bash
   export DB_PASSWORD="sua-senha-forte-aqui"
   export RABBITMQ_PASSWORD="sua-senha-rabbitmq-aqui"
   export JWT_SECRET="sua-chave-jwt-segura-aqui"
   ```

2. **Usar Secrets Management**
   - Azure Key Vault
   - AWS Secrets Manager
   - HashiCorp Vault
   - Kubernetes Secrets

3. **Gerar Senhas Fortes**
   ```bash
   # Gerar senha aleatória
   openssl rand -base64 32
   ```

4. **Configurar JWT Secret Seguro**
   ```bash
   # Gerar chave JWT
   openssl rand -base64 64
   ```

5. **Usar HTTPS/TLS**
   - Certificados SSL válidos
   - Forçar HTTPS em produção
   - Configurar HSTS

6. **Configurar Firewall**
   - Restringir acesso ao banco de dados
   - Permitir apenas IPs conhecidos
   - Usar VPN ou rede privada

## 🛡️ Melhores Práticas de Segurança

### Banco de Dados
- [ ] Usar usuário com privilégios mínimos (não `sa`)
- [ ] Habilitar criptografia de conexão
- [ ] Fazer backup regular
- [ ] Auditar acessos
- [ ] Usar prepared statements (já implementado)

### Autenticação
- [ ] Implementar rate limiting (já implementado)
- [ ] Usar refresh tokens (já implementado)
- [ ] Implementar 2FA (futuro)
- [ ] Expiração de tokens configurável
- [ ] Blacklist de tokens revogados

### API
- [ ] Validar todas as entradas (já implementado)
- [ ] Sanitizar outputs
- [ ] Implementar CORS restritivo em produção
- [ ] Usar HTTPS apenas
- [ ] Implementar API versioning

### Docker
- [ ] Não usar imagens `latest` em produção
- [ ] Escanear imagens por vulnerabilidades
- [ ] Usar usuários não-root
- [ ] Limitar recursos (CPU, memória)
- [ ] Usar secrets do Docker

### Código
- [ ] Não commitar senhas ou secrets
- [ ] Usar .gitignore adequadamente
- [ ] Fazer code review
- [ ] Usar análise estática de código
- [ ] Manter dependências atualizadas

## 🐛 Reportar Vulnerabilidades

Se você encontrar uma vulnerabilidade de segurança, por favor:

1. **NÃO** abra uma issue pública
2. Envie um email para: [seu-email-aqui]
3. Inclua:
   - Descrição da vulnerabilidade
   - Passos para reproduzir
   - Impacto potencial
   - Sugestão de correção (se possível)

## 📝 Checklist de Segurança para Produção

Antes de fazer deploy em produção:

### Configuração
- [ ] Todas as senhas foram alteradas
- [ ] Secrets estão em variáveis de ambiente ou vault
- [ ] JWT secret é forte e único
- [ ] CORS está configurado corretamente
- [ ] HTTPS está habilitado e forçado
- [ ] Certificados SSL são válidos

### Banco de Dados
- [ ] Usuário não é `sa` ou `root`
- [ ] Senha é forte (mínimo 16 caracteres)
- [ ] Conexão usa TLS/SSL
- [ ] Backup automático configurado
- [ ] Firewall permite apenas IPs necessários

### Aplicação
- [ ] Logs não expõem informações sensíveis
- [ ] Error messages não revelam detalhes internos
- [ ] Rate limiting está ativo
- [ ] Input validation está implementada
- [ ] Dependências estão atualizadas

### Infraestrutura
- [ ] Firewall configurado
- [ ] Portas desnecessárias fechadas
- [ ] Monitoramento ativo
- [ ] Alertas configurados
- [ ] Plano de resposta a incidentes

### Compliance
- [ ] LGPD/GDPR compliance (se aplicável)
- [ ] Política de privacidade
- [ ] Termos de uso
- [ ] Logs de auditoria

## 🔄 Atualizações de Segurança

Este projeto usa as seguintes dependências que devem ser mantidas atualizadas:

### Backend
- ASP.NET Core 10.0
- Entity Framework Core
- JWT Bearer Authentication
- BCrypt.Net

### Frontend
- Next.js 15
- React 19
- TypeScript

### Infraestrutura
- SQL Server
- RabbitMQ
- Docker

## 📚 Recursos Adicionais

- [OWASP Top 10](https://owasp.org/www-project-top-ten/)
- [ASP.NET Core Security](https://docs.microsoft.com/en-us/aspnet/core/security/)
- [Docker Security Best Practices](https://docs.docker.com/engine/security/)
- [JWT Best Practices](https://tools.ietf.org/html/rfc8725)

## ⚖️ Disclaimer

Este projeto é fornecido "como está", sem garantias de qualquer tipo. Os desenvolvedores não são responsáveis por qualquer uso inadequado ou violação de segurança resultante do uso deste código em produção sem as devidas precauções de segurança.

---

**Última Atualização**: 8 de Março de 2026  
**Versão**: 1.0.0
