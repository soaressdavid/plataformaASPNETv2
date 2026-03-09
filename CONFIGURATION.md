# Configuração do Ambiente

## Desenvolvimento

1. Copiar .env.example para .env
2. Preencher senhas no .env
3. Rodar docker-compose up -d
4. Rodar dotnet run

## Produção

1. Configurar Azure Key Vault
2. Definir variáveis de ambiente
3. Rodar docker-compose -f docker-compose.production.yml up -d
4. Rodar ./deploy.ps1

## Variáveis Obrigatórias

- DB_CONNECTION_STRING
- REDIS_CONNECTION_STRING
- RABBITMQ_HOST
- RABBITMQ_USER
- RABBITMQ_PASSWORD
- JWT_SECRET
