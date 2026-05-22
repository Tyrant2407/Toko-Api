# 🏪 BelajarNet — POS Api & Inventory Management System

![Build Status](https://img.shields.io/badge/build-passing-brightgreen)
![License](https://img.shields.io/badge/license-MIT-blue)
![.NET](https://img.shields.io/badge/.NET-8.0-purple)
![Next.js](https://img.shields.io/badge/Next.js-16-black)
![Docker](https://img.shields.io/badge/Docker-Compose-2496ED?logo=docker)

**BelajarNet** adalah sistem manajemen inventaris dan *Point of Sale* (POS) berbasis web yang dirancang untuk memudahkan pengelolaan toko secara terintegrasi. Sistem ini memecahkan masalah umum dalam operasional toko kecil-menengah: pencatatan stok yang tidak akurat, pengelolaan piutang (*kasbon*) yang manual, dan ketiadaan laporan keuangan yang terstruktur.

Proyek ini terdiri dari dua bagian utama: **REST API** yang dibangun dengan .NET 8 dan **antarmuka pengguna** (*frontend*) berbasis Next.js 16. Keduanya dapat dijalankan secara bersamaan menggunakan **Docker Compose** dalam satu perintah, baik untuk lingkungan *development* maupun *production*.

---

## ✨ Fitur Utama

- **Manajemen Produk & Kategori** — Tambah, ubah, dan hapus produk beserta kategorinya. Dilengkapi filter pencarian dan paginasi.
- **Point of Sale (POS)** — Antarmuka kasir untuk mencari produk, menambahkan ke keranjang, dan memproses transaksi (*Lunas* atau *Kasbon*) dengan satu klik.
- **Sistem Piutang (Kasbon)** — Pelacakan tagihan yang belum lunas secara *real-time*, dengan dukungan pembayaran **cicilan parsial** dan pembayaran penuh (*lunas*). Dilengkapi *progress bar* visual per tagihan.
- **Dasbor Analitik** — Ringkasan statistik inventaris: total produk, estimasi nilai aset, stok kritis, produk hampir kedaluwarsa, dan total kategori aktif.
- **Manajemen Keuangan** — Pencatatan arus kas (*cash flow*) yang otomatis terintegrasi dengan setiap transaksi penjualan dan pelunasan kasbon.
- **Autentikasi Berbasis Peran (RBAC)** — Tiga level akses: **Admin** (akses penuh), **Kasir** (POS & kasbon), dan **Customer** (akses terbatas). Menggunakan JWT Bearer Token.
- **Notifikasi Stok Kritis** — Peringatan otomatis ketika stok produk jatuh di bawah ambang batas yang dapat dikonfigurasi.
- **Kontainerisasi Penuh** — Seluruh layanan (API, Database, Frontend) terkemas dalam Docker Compose untuk kemudahan *deployment* dan konsistensi lingkungan.
- **Logging Terstruktur** — Log server menggunakan Serilog, dirotasi harian ke file `logs/`, dan dapat dipantau melalui konsol.

---

## 🏗️ Arsitektur & Teknologi

Sistem ini mengikuti arsitektur **multi-tier** yang memisahkan antarmuka, logika bisnis, dan data secara tegas.

| Lapisan | Teknologi | Peran |
|---|---|---|
| **Frontend** | Next.js 16 (App Router) | Antarmuka pengguna berbasis React dengan SSR; menangani routing, autentikasi *client-side*, dan pemanggilan API. |
| **State Management** | TanStack Query (React Query) v5 | Manajemen *server state*, caching data API, dan invalidasi cache otomatis pasca mutasi. |
| **HTTP Client** | Axios | Komunikasi dengan REST API; dilengkapi *interceptor* untuk injeksi JWT dan penanganan error 401/403 secara terpusat. |
| **Backend API** | ASP.NET Core (.NET 8) | REST API dengan pola Repository-Service-Controller; menangani logika bisnis, validasi, dan keamanan. |
| **ORM** | Entity Framework Core 8 | Pemetaan model ke database dan manajemen migrasi skema. |
| **Database** | MySQL 8.0 | Penyimpanan data utama (produk, transaksi, keuangan, kategori). |
| **Autentikasi** | JWT Bearer (HS256) | Otentikasi *stateless*; token berumur 8 jam mengandung klaim `Role` untuk otorisasi berbasis peran. |
| **Logging** | Serilog + Sinks.File | Log terstruktur ke konsol (dev) dan file harian (prod), dengan request logging otomatis via `UseSerilogRequestLogging()`. |
| **Dokumentasi API** | Swagger / Swashbuckle | UI eksplorasi API interaktif yang dapat diakses di `/swagger` pada mode Development. |
| **Kontainerisasi** | Docker & Docker Compose | Orkestrasi tiga layanan (database, API, frontend) dalam jaringan bridge terisolasi. |
| **Pengujian** | xUnit + Testcontainers (Backend) / Playwright (Frontend) | *Integration test* dengan database MySQL nyata di container + *End-to-End test* berbasis browser. |

---

## ✅ Prasyarat

Pastikan perangkat lunak berikut sudah terinstal sebelum memulai:

| Perangkat Lunak | Versi Minimum | Keterangan |
|---|---|---|
| [.NET SDK](https://dotnet.microsoft.com/download/dotnet/8.0) | **8.0** | Untuk menjalankan backend API secara lokal |
| [Node.js](https://nodejs.org/) | **20 LTS** | Untuk menjalankan frontend Next.js secara lokal |
| [MySQL Server](https://dev.mysql.com/downloads/mysql/) | **8.0** | Untuk menjalankan backend secara lokal (opsional jika memakai Docker) |
| [Docker Desktop](https://www.docker.com/products/docker-desktop/) | **4.x** | Untuk menjalankan seluruh sistem via Docker Compose |
| [Git](https://git-scm.com/) | Terbaru | Untuk meng-*clone* repositori |

---

## 🚀 Panduan Instalasi Lokal

### Opsi A: Menjalankan dengan Docker Compose (Direkomendasikan)

Cara paling mudah dan cepat untuk menjalankan seluruh sistem tanpa konfigurasi manual.

**1. Clone repositori:**
```bash
git clone https://github.com/username/belajarnet.git
cd belajarnet
```

**2. Buat file konfigurasi environment:**
```bash
cp .env.example .env
```

Lalu edit file **`.env`** dan isi nilai yang sesuai:
```dotenv
# ── Database ──────────────────────────────────────────────────────────────────
MYSQL_ROOT_PASSWORD=rahasia_database_anda
MYSQL_DATABASE=toko_db

# ── JWT (minimal 32 karakter untuk keamanan HS256) ────────────────────────────
JWT_KEY=GantiDenganKunciRahasiaMinimal32Karakter!

# ── CORS ──────────────────────────────────────────────────────────────────────
CORS_ALLOWED_ORIGINS=http://localhost:3000

# ── Frontend ──────────────────────────────────────────────────────────────────
NEXT_PUBLIC_API_URL=http://localhost:5174/api
```

**3. Build dan jalankan semua layanan:**
```bash
docker compose up -d --build
```

Perintah ini akan membangun image Docker untuk API dan Frontend, menjalankan MySQL, menjalankan migrasi database secara otomatis, dan memulai seluruh layanan.

---

### Opsi B: Menjalankan Secara Lokal (Tanpa Docker)

Gunakan opsi ini untuk keperluan pengembangan aktif dengan *hot-reload*.

**1. Clone repositori:**
```bash
git clone https://github.com/username/belajarnet.git
cd belajarnet
```

**2. Konfigurasi Backend (.NET API):**

Buka file **`dotNet/TokoApi/appsettings.Development.json`** dan sesuaikan *connection string* MySQL Anda:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=toko_db;User=root;Password=password_mysql_anda;"
  }
}
```

**3. Instal dependency dan jalankan migrasi database:**
```bash
cd dotNet/TokoApi
dotnet restore
dotnet ef database update
```

**4. Konfigurasi Frontend (Next.js):**

Buat file **`toko-frontend/.env.local`**:
```bash
cd ../../toko-frontend
cp ../.env.example .env.local
```

Pastikan nilai `NEXT_PUBLIC_API_URL` menunjuk ke port API lokal:
```dotenv
NEXT_PUBLIC_API_URL=http://localhost:5174/api
```

**5. Instal dependency frontend:**
```bash
npm install
```

---

## ▶️ Menjalankan Aplikasi

### Mode Development (Lokal)

Buka **dua terminal terpisah** dan jalankan masing-masing:

**Terminal 1 — Backend API:**
```bash
cd dotNet/TokoApi
dotnet run
```
API akan berjalan di: **`http://localhost:5174`**
Dokumentasi Swagger tersedia di: **`http://localhost:5174/swagger`**

**Terminal 2 — Frontend Next.js:**
```bash
cd toko-frontend
npm run dev
```
Aplikasi web akan berjalan di: **`http://localhost:3000`**

### Mode Production (Docker)

```bash
# Jalankan seluruh sistem (development, dengan port database ter-expose)
docker compose up -d

# Jalankan mode production (resource limits, tanpa expose port internal)
docker compose -f docker-compose.yml -f docker-compose.prod.yml --env-file .env up -d --build

# Hentikan seluruh layanan
docker compose down

# Hentikan dan hapus semua volume (termasuk data database)
docker compose down -v
```

| Layanan | URL |
|---|---|
| Frontend (Next.js) | `http://localhost:3000` |
| Backend API | `http://localhost:5174/api` |
| Swagger UI | `http://localhost:5174/swagger` |
| MySQL Database | `localhost:3307` |

---

## 🔑 Akun Demo

> **Perhatian:** Akun ini hanya untuk keperluan demonstrasi dan pengujian. Ganti dengan sistem autentikasi berbasis database di lingkungan produksi.

| Username | Password | Role | Akses |
|---|---|---|---|
| `rizky` | `admin123` | **Admin** | Akses penuh: Dasbor, Produk, Kategori, Keuangan, Piutang, POS |
| `siti` | `kasir123` | **Kasir** | Terbatas: POS & Manajemen Kasbon |
| `putra` | `user123` | **Customer** | Akses terbatas (hanya *read*) |

---

## 🗂️ Struktur Direktori

```
BelajarNet/
├── 📁 dotNet/
│   ├── 📁 TokoApi/                  # Proyek backend ASP.NET Core
│   │   ├── 📁 Controllers/          # HTTP endpoint (Auth, Produk, Transaksi, dll.)
│   │   ├── 📁 DTOs/                 # Data Transfer Objects (Request & Response)
│   │   ├── 📁 Extensions/           # Extension methods (DI registration)
│   │   ├── 📁 Mappings/             # Profil AutoMapper
│   │   ├── 📁 Middlewares/          # Global Exception Handler
│   │   ├── 📁 Migrations/           # Migrasi skema EF Core
│   │   ├── 📁 Models/               # Entity model (Produk, Transaksi, dll.)
│   │   ├── 📁 Repositories/         # Lapisan akses data (Repository pattern)
│   │   ├── 📁 Services/             # Lapisan logika bisnis (Service pattern)
│   │   ├── 📁 logs/                 # File log Serilog (rotasi harian, diabaikan git)
│   │   ├── AppDbContext.cs          # EF Core DbContext
│   │   ├── Program.cs               # Entry point & konfigurasi middleware
│   │   ├── appsettings.json         # Konfigurasi aplikasi (non-sensitif)
│   │   └── Dockerfile               # Docker image untuk API
│   │
│   └── 📁 TokoApi.Tests/            # Proyek pengujian .NET
│       ├── 📁 Infrastructure/       # WebApplicationFactory & Database Seeder
│       ├── 📁 IntegrationTests/     # Skenario integration test (TransaksiController)
│       └── ProdukControllerTests.cs # Unit test untuk ProdukController
│
├── 📁 toko-frontend/                # Proyek frontend Next.js
│   ├── 📁 src/
│   │   ├── 📁 app/                  # Next.js App Router
│   │   │   ├── 📁 login/            # Halaman login
│   │   │   ├── 📁 pos/              # Halaman Point of Sale
│   │   │   ├── 📁 finance/          # Halaman manajemen keuangan
│   │   │   ├── 📁 piutang/          # Halaman manajemen piutang
│   │   │   └── page.tsx             # Halaman dasbor utama
│   │   ├── 📁 components/           # Komponen React yang dapat digunakan ulang
│   │   │   ├── 📁 dashboard/        # Komponen spesifik dasbor
│   │   │   └── 📁 ui/               # Komponen UI generik (Button, Input, Modal)
│   │   ├── 📁 hooks/                # Custom React hooks (useAuth, useProduk, dll.)
│   │   ├── 📁 lib/                  # Konfigurasi Axios (api.ts)
│   │   ├── 📁 providers/            # React Query provider
│   │   └── middleware.ts            # Middleware Next.js (proteksi rute)
│   ├── 📁 tests/                    # Playwright E2E tests
│   ├── playwright.config.ts         # Konfigurasi Playwright
│   └── Dockerfile                   # Docker image untuk frontend
│
├── docker-compose.yml               # Konfigurasi Docker Compose (base)
├── docker-compose.override.yml      # Override untuk development
├── docker-compose.prod.yml          # Override untuk production
├── .env.example                     # Template environment variables
└── README.md                        # Dokumentasi proyek ini
```

---

## 🧪 Menjalankan Pengujian

### Backend — Integration & Unit Tests

Pengujian backend menggunakan **xUnit** dan **Testcontainers** (MySQL nyata di Docker) sehingga Docker harus aktif.

```bash
# Jalankan dari folder root
dotnet test dotNet/TokoApi.Tests/TokoApi.Tests.csproj

# Dengan output verbose
dotnet test dotNet/TokoApi.Tests/TokoApi.Tests.csproj --logger "console;verbosity=normal"
```

### Frontend — End-to-End Tests (Playwright)

```bash
cd toko-frontend

# Pastikan browser Playwright sudah terinstal (jalankan sekali)
npx playwright install chromium

# Jalankan semua E2E test (server dev akan dijalankan otomatis)
npx playwright test

# Jalankan dengan tampilan browser (non-headless, untuk debugging)
npx playwright test --headed

# Lihat laporan HTML hasil pengujian
npx playwright show-report
```

Atau dari **folder root** (`BelajarNet`):
```bash
npx --prefix toko-frontend playwright test --config=toko-frontend/playwright.config.ts
```

---

## 🤝 Panduan Kontribusi

Kontribusi sangat terbuka dan disambut baik. Ikuti langkah berikut:

1. **Fork** repositori ini ke akun GitHub Anda.
2. Buat **branch** fitur baru dari `main`:
   ```bash
   git checkout -b feat/nama-fitur-anda
   ```
3. Lakukan perubahan dan pastikan semua pengujian lulus:
   ```bash
   dotnet test dotNet/TokoApi.Tests/TokoApi.Tests.csproj
   npx --prefix toko-frontend playwright test --config=toko-frontend/playwright.config.ts
   ```
4. **Commit** dengan pesan yang deskriptif mengikuti format [Conventional Commits](https://www.conventionalcommits.org/):
   ```bash
   git commit -m "feat: tambahkan fitur laporan penjualan harian"
   ```
5. **Push** branch ke repositori fork Anda:
   ```bash
   git push origin feat/nama-fitur-anda
   ```
6. Buka **Pull Request** ke branch `main` repositori utama dan isi deskripsi perubahan secara lengkap.

### Konvensi Kode
- **Backend:** Ikuti gaya C# standar Microsoft. Setiap endpoint baru harus memiliki *summary* XML dan diuji melalui integration test.
- **Frontend:** Gunakan TypeScript secara ketat (`strict: true`). Logika API harus diekstrak ke dalam custom hook di folder `hooks/`.

---

## 📄 Lisensi

Proyek ini dilisensikan di bawah **MIT License**. Lihat file [LICENSE](LICENSE) untuk detail lengkap.

```
MIT License

Copyright (c) 2026 BelajarNet Contributors

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction...
```
