# BelajarNet — Deployment Guide

## Prerequisites

- Docker Engine ≥ 24.x
- Docker Compose Plugin ≥ 2.x
- `.env` file created from `.env.example`

---

## Environment Setup

### Development

```bash
# 1. Clone and setup
git clone <repo-url>
cd BelajarNet
cp .env.example .env       # Fill in your local values

# 2. Start all services (DB + API + Frontend)
docker compose up -d

# 3. Apply DB migrations (first time only)
docker compose exec toko_api dotnet ef database update
```

Services:
| Service | URL |
|---------|-----|
| Frontend | http://localhost:3000 |
| API | http://localhost:5174 |
| Swagger | http://localhost:5174/swagger |
| MySQL | localhost:3307 |

### Production

```bash
# Build and deploy with production overrides
docker compose -f docker-compose.yml -f docker-compose.prod.yml --env-file .env up -d --build

# Apply migrations in production
docker compose exec toko_api dotnet ef database update
```

---

## Health Checks

```bash
# API health
curl http://localhost:5174/api/kategori

# DB connectivity (inside container)
docker compose exec toko_db mysqladmin ping -u root -p$MYSQL_ROOT_PASSWORD
```

---

## Logging

Structured logs are written to:
- **Console**: Real-time structured output (useful for `docker compose logs -f toko_api`)
- **File**: `dotNet/TokoApi/logs/tokoapi-YYYY-MM-DD.log` (rotated daily, kept 30 days)

View live logs:
```bash
docker compose logs -f toko_api
```

---

## Rollback Plan

> [!CAUTION]
> Follow these steps exactly if a deployment causes a regression or system down.

### Immediate Rollback (< 5 min)

```bash
# 1. Stop failing containers
docker compose down

# 2. Re-deploy previous stable image (if using image tags)
#    Edit docker-compose.yml image tags back to last known good version
docker compose -f docker-compose.yml -f docker-compose.prod.yml up -d

# 3. Verify health
curl http://localhost:5174/api/kategori
```

### Database Rollback (if migration failed)

```bash
# List migrations
docker compose exec toko_api dotnet ef migrations list

# Revert to previous migration (replace <MigrationName> with the previous one)
docker compose exec toko_api dotnet ef database update <MigrationName>
```

### Full Rollback from Git

```bash
# Find the last stable commit
git log --oneline -10

# Reset to last stable state
git checkout <commit-hash>

# Rebuild and redeploy
docker compose -f docker-compose.yml -f docker-compose.prod.yml up -d --build
```

---

## Demo Credentials

| Role | Username | Password | Access |
|------|----------|----------|--------|
| Admin | `rizky` | `admin123` | Dashboard penuh + POS |
| Kasir | `siti` | `kasir123` | POS + Tagihan Kasbon |
| Customer | `putra` | `user123` | Read-only (legacy) |

> [!WARNING]
> Ganti semua credential di atas sebelum deploy ke production dengan sistem autentikasi berbasis database + password hash (bcrypt).

---

## Security Checklist

- [x] JWT key diset via environment variable (bukan hardcoded)
- [x] Stack trace tidak ter-expose di production response
- [x] CORS hanya mengizinkan origin yang terdaftar
- [x] Port MySQL tidak ter-expose di production
- [x] Kasir tidak bisa akses Analytics/Dashboard Admin
- [x] Kategori hanya bisa dihapus jika tidak ada produk terkait
- [ ] Ganti demo users dengan user table + bcrypt hash (next sprint)
- [ ] Setup HTTPS/TLS di reverse proxy (nginx/caddy)
- [ ] Rate limiting untuk endpoint `/api/auth/login`
