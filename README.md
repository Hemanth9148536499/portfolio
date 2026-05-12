# 🚀 Portfolio App — ASP.NET Core · MySQL · GitHub Pages

A full-stack developer portfolio with a **C# / ASP.NET Core 8 REST API** backend, **MySQL** database, and a **static HTML/CSS/JS frontend** that deploys automatically to **GitHub Pages**.

```
┌─────────────────────────────┐        ┌──────────────────────────────┐
│    GitHub Pages             │  HTTP  │    ASP.NET Core 8 API        │
│  frontend/index.html  ──────┼───────►│  /api/projects               │
│  (your-username.github.io)  │        │  /api/skills                 │
└─────────────────────────────┘        │  /api/experience             │
                                       │  /api/contact                │
                                       └──────────┬───────────────────┘
                                                  │ EF Core
                                       ┌──────────▼───────────────────┐
                                       │         MySQL 8               │
                                       │       portfolio_db            │
                                       └──────────────────────────────┘
```

---

## 📋 Requirements

| Tool | Version | Notes |
|------|---------|-------|
| [.NET SDK](https://dotnet.microsoft.com/download) | **8.0+** | `dotnet --version` to check |
| [MySQL Server](https://dev.mysql.com/downloads/mysql/) | **8.0+** | Or use Docker below |
| [Git](https://git-scm.com/) | any | For GitHub deployment |
| [Docker + Compose](https://www.docker.com/) | optional | Easiest local setup |

---

## ⚡ Quick Start

### Option A — Docker (recommended)

```bash
# 1. Clone
git clone https://github.com/<YOUR_USERNAME>/<YOUR_REPO>.git
cd PortfolioApp

# 2. Start MySQL + API
docker compose up --build

# API is live at http://localhost:5000
# Swagger UI at http://localhost:5000/swagger
```

---

### Option B — Manual Setup

#### Step 1 — Set up MySQL

```bash
# Log in as root
mysql -u root -p

# Run the schema script
source database/schema.sql
# or: mysql -u root -p < database/schema.sql
```

#### Step 2 — Configure the connection string

Edit `backend/PortfolioAPI/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Port=3306;Database=portfolio_db;User=portfolio_user;Password=YourStrongPassword123!;"
  }
}
```

#### Step 3 — Apply EF Core migrations

```bash
cd backend/PortfolioAPI

# Install EF Core tools (once)
dotnet tool install --global dotnet-ef

# Create & apply migrations
dotnet ef migrations add InitialCreate
dotnet ef database update
```

#### Step 4 — Run the API

```bash
dotnet run
# API → http://localhost:5000
# Swagger → http://localhost:5000/swagger
```

#### Step 5 — Open the frontend locally

Open `frontend/index.html` in a browser, or use a live-server:

```bash
# VS Code: install "Live Server" extension, right-click index.html → Open with Live Server
# or npx:
npx serve frontend
```

---

## 🌐 Deploy to GitHub Pages

### 1. Push to GitHub

```bash
git init
git add .
git commit -m "Initial portfolio commit"
git remote add origin https://github.com/<YOUR_USERNAME>/<YOUR_REPO>.git
git push -u origin main
```

### 2. Enable GitHub Pages

Go to **Settings → Pages → Source: GitHub Actions** in your repo.

### 3. Add the API URL secret

Go to **Settings → Secrets → Actions → New secret**:

| Name | Value |
|------|-------|
| `API_BASE_URL` | `https://your-api.onrender.com` |

### 4. Update CORS in Program.cs

In `backend/PortfolioAPI/Program.cs`, add your GitHub Pages URL:

```csharp
policy.WithOrigins(
    "https://<YOUR_USERNAME>.github.io"
)
```

### 5. Push → auto-deploy fires 🎉

Every push to `main` triggers the GitHub Actions workflow which publishes `frontend/` to GitHub Pages.

**Your portfolio URL:** `https://<YOUR_USERNAME>.github.io/<YOUR_REPO>/`

---

## 🔌 REST API Reference

Base URL: `http://localhost:5000` (local) or your deployed URL

### Projects

| Method | Endpoint | Description |
|--------|----------|-------------|
| `GET` | `/api/projects` | List all projects |
| `GET` | `/api/projects?featured=true` | Featured projects only |
| `GET` | `/api/projects/{id}` | Get project by ID |
| `POST` | `/api/projects` | Create project |
| `PUT` | `/api/projects/{id}` | Update project |
| `DELETE` | `/api/projects/{id}` | Delete project |

### Skills

| Method | Endpoint | Description |
|--------|----------|-------------|
| `GET` | `/api/skills` | All skills |
| `GET` | `/api/skills?category=Backend` | Filter by category |
| `GET` | `/api/skills/categories` | List categories |
| `POST` | `/api/skills` | Add skill |
| `DELETE` | `/api/skills/{id}` | Remove skill |

### Experience

| Method | Endpoint | Description |
|--------|----------|-------------|
| `GET` | `/api/experience` | All entries (newest first) |
| `GET` | `/api/experience/{id}` | Single entry |
| `POST` | `/api/experience` | Add entry |
| `PUT` | `/api/experience/{id}` | Update entry |
| `DELETE` | `/api/experience/{id}` | Delete entry |

### Contact

| Method | Endpoint | Description |
|--------|----------|-------------|
| `POST` | `/api/contact` | Send a message (public) |
| `GET` | `/api/contact` | View all messages (admin) |
| `PATCH` | `/api/contact/{id}/read` | Mark as read |

---

## 📂 Project Structure

```
PortfolioApp/
├── .github/
│   └── workflows/
│       └── deploy-pages.yml    # Auto-deploy frontend to GitHub Pages
├── backend/
│   └── PortfolioAPI/
│       ├── Controllers/        # REST endpoints
│       ├── Data/               # DbContext + seed data
│       ├── DTOs/               # Request/response shapes
│       ├── Models/             # EF Core entities
│       ├── Dockerfile
│       ├── Program.cs
│       └── appsettings.json
├── database/
│   └── schema.sql              # MySQL setup script
├── frontend/
│   ├── css/style.css           # Full design system
│   ├── js/
│   │   ├── config.js           # ← Edit this: API URL + your info
│   │   ├── api.js              # API service layer
│   │   └── app.js              # Rendering + interactions
│   └── index.html
├── docker-compose.yml
├── .gitignore
└── README.md
```

---

## ✏️ Personalising Your Portfolio

1. **Edit `frontend/js/config.js`** — set your name, title, email, GitHub/LinkedIn URLs and API URL.
2. **Edit seed data in `Data/PortfolioDbContext.cs`** — add your real projects, skills and experience.
3. **Re-run migrations** after changing models: `dotnet ef migrations add <Name> && dotnet ef database update`

---

## 🚢 Deploy the API (free options)

| Platform | Notes |
|----------|-------|
| [Render.com](https://render.com) | Free tier, Docker deploy, add MySQL add-on |
| [Railway.app](https://railway.app) | Easy .NET + MySQL, free starter |
| [Azure App Service](https://azure.microsoft.com) | Free F1 tier, Azure Database for MySQL |
| [Fly.io](https://fly.io) | Docker-based, generous free allowance |

---

## 📄 License

MIT — free to use and customise.
