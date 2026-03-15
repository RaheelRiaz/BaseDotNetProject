# CleanMvcApp — .NET 8 MVC Clean Architecture Starter

A production-ready .NET 8 MVC starter template using **Clean Layered Architecture**, **ASP.NET Core Identity**, and **AdminLTE 3** admin UI.

---

## Architecture

```
Controller → Service → Repository → DbContext
```

| Layer | Project | Responsibility |
|---|---|---|
| Domain | `CleanMvcApp.Domain` | Entities only, no dependencies |
| Application | `CleanMvcApp.Application` | Interfaces, Services, DTOs |
| Infrastructure | `CleanMvcApp.Infrastructure` | EF Core, Repositories, Identity seeding |
| Web | `CleanMvcApp.Web` | Controllers, Views, ViewModels |

---

## Tech Stack

- **.NET 8** / ASP.NET Core MVC
- **Entity Framework Core** (SQL Server, Code First)
- **ASP.NET Core Identity** (custom UI — no scaffold)
- **AdminLTE 3** (Bootstrap 5 admin template)
- **FontAwesome** (via AdminLTE)

---

## Quick Start

### 1. Clone and open

```bash
git clone <repo-url>
cd BaseDotNetProject
```

### 2. Download AdminLTE assets

Download AdminLTE 3 from https://adminlte.io/ and extract into:

```
CleanMvcApp.Web/wwwroot/adminlte/
```

Required folders: `dist/` and `plugins/`

OR via npm (run from `CleanMvcApp.Web/`):

```bash
npm install admin-lte@^3.2
xcopy /E /I node_modules\admin-lte wwwroot\adminlte
```

### 3. Configure the database

Edit `CleanMvcApp.Web/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=CleanMvcAppDb;Trusted_Connection=True;"
  }
}
```

### 4. Apply migrations

```bash
cd CleanMvcApp.Web
dotnet ef migrations add InitialCreate --project ../CleanMvcApp.Infrastructure
dotnet ef database update
```

### 5. Run

```bash
dotnet run --project CleanMvcApp.Web
```

Navigate to `https://localhost:5001`

---

## Default Admin Account

After first run, the database is seeded with:

| Field | Value |
|---|---|
| Email | `admin@cleanmvc.com` |
| Password | `Admin@123456` |
| Role | `Admin` |

---

## Features

- **Authentication**: Login, Register, Forgot/Reset Password, Access Denied (all custom UI)
- **Authorization**: Role-based (`Admin` = full CRUD, `User` = read-only)
- **Products**: Full CRUD for admins, view-only for users
- **Dashboard**: AdminLTE sidebar with active menu highlighting
- **Security**: Anti-forgery tokens, secure cookies, password hashing, lockout policy

---

## Solution Structure

```
CleanMvcApp.sln
├── CleanMvcApp.Domain/
│   └── Entities/
│       ├── BaseEntity.cs
│       └── Product.cs
├── CleanMvcApp.Application/
│   ├── Interfaces/
│   │   ├── IRepository.cs
│   │   ├── IProductRepository.cs
│   │   └── IProductService.cs
│   ├── Services/
│   │   └── ProductService.cs
│   └── DTOs/
│       └── ProductDto.cs
├── CleanMvcApp.Infrastructure/
│   ├── Data/
│   │   ├── ApplicationDbContext.cs
│   │   └── DbInitializer.cs
│   ├── Repositories/
│   │   ├── Repository.cs
│   │   └── ProductRepository.cs
│   └── Identity/
│       └── IdentitySeeder.cs
└── CleanMvcApp.Web/
    ├── Controllers/
    │   ├── AccountController.cs
    │   ├── DashboardController.cs
    │   ├── HomeController.cs
    │   └── ProductController.cs
    ├── ViewModels/
    ├── Views/
    │   ├── Shared/ (_AdminLayout, _Layout, _Sidebar)
    │   ├── Account/ (Login, Register, ForgotPassword, ResetPassword, AccessDenied)
    │   ├── Dashboard/
    │   └── Product/ (Index, Create, Edit, Details, Delete)
    ├── wwwroot/
    ├── Program.cs
    └── appsettings.json
```

---

## EF Core Migrations (from solution root)

```bash
dotnet ef migrations add <MigrationName> \
  --project CleanMvcApp.Infrastructure \
  --startup-project CleanMvcApp.Web

dotnet ef database update \
  --project CleanMvcApp.Infrastructure \
  --startup-project CleanMvcApp.Web
```
