# 🚀 INÍCIO RÁPIDO

## Para computador SEM Docker

### 1 comando para rodar tudo:

```powershell
git clone https://github.com/seu-usuario/seu-repo.git
cd seu-repo
./setup-no-docker.ps1
```

Aguarde 30 segundos e acesse: **http://localhost:3000**

**Login:** test@test.com  
**Senha:** Test123!

---

## Para computador COM Docker

```powershell
git clone https://github.com/seu-usuario/seu-repo.git
cd seu-repo
docker-compose up -d
cd frontend
npm install
npm run dev
```

Acesse: **http://localhost:3000**

---

## 📚 Documentação Completa

- **Sem Docker:** [README_NO_DOCKER.md](README_NO_DOCKER.md)
- **Com Docker:** [README.md](README.md)
- **Setup Detalhado:** [SETUP_SEM_DOCKER.md](SETUP_SEM_DOCKER.md)

---

## 🐛 Problemas?

### Sem Docker
```powershell
./cleanup-no-docker.ps1
./setup-no-docker.ps1
```

### Com Docker
```powershell
docker-compose down
docker-compose up -d
```

---

**Versão:** 2.0  
**Atualizado:** 09/03/2026
