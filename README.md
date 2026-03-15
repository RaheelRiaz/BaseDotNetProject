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

## How to Add a New Entity (Step-by-Step Guide)

Follow these steps every time you want to add a new feature (e.g. `Category`, `Order`, `Customer`).
The example below uses **`Category`** as the new entity.

---

### Step 1 — Create the Entity `CleanMvcApp.Domain/Entities/Category.cs`

```csharp
namespace CleanMvcApp.Domain.Entities;

public class Category : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}
```

> Inherit from `BaseEntity` to get `Id` and `CreatedDate` for free.

---

### Step 2 — Create the DTO `CleanMvcApp.Application/DTOs/CategoryDto.cs`

```csharp
namespace CleanMvcApp.Application.DTOs;

public class CategoryDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
}
```

> DTOs are what the Service layer passes to the Web layer — never expose raw entities to controllers.

---

### Step 3 — Create the Repository Interface `CleanMvcApp.Application/Interfaces/ICategoryRepository.cs`

```csharp
using CleanMvcApp.Domain.Entities;

namespace CleanMvcApp.Application.Interfaces;

public interface ICategoryRepository : IRepository<Category>
{
    // Add any Category-specific query methods here
    // e.g. Task<IEnumerable<Category>> GetActiveAsync();
}
```

> Only add methods here that are NOT already covered by `IRepository<T>` (GetAll, GetById, Add, Update, Delete).

---

### Step 4 — Create the Service Interface `CleanMvcApp.Application/Interfaces/ICategoryService.cs`

```csharp
using CleanMvcApp.Application.DTOs;

namespace CleanMvcApp.Application.Interfaces;

public interface ICategoryService
{
    Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync();
    Task<CategoryDto?> GetCategoryByIdAsync(int id);
    Task CreateCategoryAsync(CategoryDto dto);
    Task UpdateCategoryAsync(CategoryDto dto);
    Task DeleteCategoryAsync(int id);
}
```

---

### Step 5 — Implement the Service `CleanMvcApp.Application/Services/CategoryService.cs`

```csharp
using CleanMvcApp.Application.DTOs;
using CleanMvcApp.Application.Interfaces;
using CleanMvcApp.Domain.Entities;

namespace CleanMvcApp.Application.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryService(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync()
    {
        var items = await _categoryRepository.GetAllAsync();
        return items.Select(MapToDto);
    }

    public async Task<CategoryDto?> GetCategoryByIdAsync(int id)
    {
        var item = await _categoryRepository.GetByIdAsync(id);
        return item is null ? null : MapToDto(item);
    }

    public async Task CreateCategoryAsync(CategoryDto dto)
    {
        await _categoryRepository.AddAsync(new Category
        {
            Name = dto.Name,
            Description = dto.Description,
            CreatedDate = DateTime.UtcNow
        });
    }

    public async Task UpdateCategoryAsync(CategoryDto dto)
    {
        var item = await _categoryRepository.GetByIdAsync(dto.Id);
        if (item is null) return;
        item.Name = dto.Name;
        item.Description = dto.Description;
        await _categoryRepository.UpdateAsync(item);
    }

    public async Task DeleteCategoryAsync(int id)
        => await _categoryRepository.DeleteAsync(id);

    private static CategoryDto MapToDto(Category c) => new()
    {
        Id = c.Id,
        Name = c.Name,
        Description = c.Description,
        CreatedDate = c.CreatedDate
    };
}
```

> All business logic lives here. The controller only calls service methods.

---

### Step 6 — Implement the Repository `CleanMvcApp.Infrastructure/Repositories/CategoryRepository.cs`

```csharp
using CleanMvcApp.Application.Interfaces;
using CleanMvcApp.Domain.Entities;
using CleanMvcApp.Infrastructure.Data;

namespace CleanMvcApp.Infrastructure.Repositories;

public class CategoryRepository : Repository<Category>, ICategoryRepository
{
    public CategoryRepository(ApplicationDbContext context) : base(context)
    {
    }

    // Implement any extra methods from ICategoryRepository here
}
```

---

### Step 7 — Register the DbSet in `ApplicationDbContext.cs`

```csharp
// In CleanMvcApp.Infrastructure/Data/ApplicationDbContext.cs
public DbSet<Category> Categories => Set<Category>();
```

Optionally add seed data in `OnModelCreating`:

```csharp
builder.Entity<Category>().HasData(
    new Category { Id = 1, Name = "Electronics", Description = "Electronic items", CreatedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
    new Category { Id = 2, Name = "Accessories", Description = "Peripheral accessories", CreatedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) }
);
```

---

### Step 8 — Register DI in `Program.cs`

```csharp
// In CleanMvcApp.Web/Program.cs — add these two lines with the other registrations
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
```

---

### Step 9 — Create the ViewModel `CleanMvcApp.Web/ViewModels/CategoryViewModel.cs`

```csharp
using System.ComponentModel.DataAnnotations;

namespace CleanMvcApp.Web.ViewModels;

public class CategoryViewModel
{
    public int Id { get; set; }

    [Required]
    [StringLength(200)]
    public string Name { get; set; } = string.Empty;

    [StringLength(1000)]
    public string Description { get; set; } = string.Empty;

    public DateTime CreatedDate { get; set; }
}
```

---

### Step 10 — Create the Controller `CleanMvcApp.Web/Controllers/CategoryController.cs`

```csharp
using CleanMvcApp.Application.DTOs;
using CleanMvcApp.Application.Interfaces;
using CleanMvcApp.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanMvcApp.Web.Controllers;

[Authorize]
public class CategoryController : Controller
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    public async Task<IActionResult> Index()
    {
        var items = await _categoryService.GetAllCategoriesAsync();
        return View(items.Select(MapToViewModel));
    }

    public async Task<IActionResult> Details(int id)
    {
        var item = await _categoryService.GetCategoryByIdAsync(id);
        if (item is null) return NotFound();
        return View(MapToViewModel(item));
    }

    [Authorize(Roles = "Admin")]
    public IActionResult Create() => View();

    [HttpPost, ValidateAntiForgeryToken, Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(CategoryViewModel model)
    {
        if (!ModelState.IsValid) return View(model);
        await _categoryService.CreateCategoryAsync(MapToDto(model));
        TempData["Success"] = "Category created successfully.";
        return RedirectToAction(nameof(Index));
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(int id)
    {
        var item = await _categoryService.GetCategoryByIdAsync(id);
        if (item is null) return NotFound();
        return View(MapToViewModel(item));
    }

    [HttpPost, ValidateAntiForgeryToken, Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(int id, CategoryViewModel model)
    {
        if (id != model.Id) return BadRequest();
        if (!ModelState.IsValid) return View(model);
        await _categoryService.UpdateCategoryAsync(MapToDto(model));
        TempData["Success"] = "Category updated successfully.";
        return RedirectToAction(nameof(Index));
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var item = await _categoryService.GetCategoryByIdAsync(id);
        if (item is null) return NotFound();
        return View(MapToViewModel(item));
    }

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken, Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _categoryService.DeleteCategoryAsync(id);
        TempData["Success"] = "Category deleted successfully.";
        return RedirectToAction(nameof(Index));
    }

    private static CategoryViewModel MapToViewModel(CategoryDto dto) => new()
    {
        Id = dto.Id, Name = dto.Name, Description = dto.Description, CreatedDate = dto.CreatedDate
    };

    private static CategoryDto MapToDto(CategoryViewModel vm) => new()
    {
        Id = vm.Id, Name = vm.Name, Description = vm.Description
    };
}
```

---

### Step 11 — Create Views under `CleanMvcApp.Web/Views/Category/`

Create these files (copy from `Views/Product/` and adjust the model type):

| File | Purpose |
|---|---|
| `Index.cshtml` | List all categories |
| `Create.cshtml` | Create form |
| `Edit.cshtml` | Edit form |
| `Details.cshtml` | View single record |
| `Delete.cshtml` | Delete confirmation |

Set layout on all: `Layout = "_AdminLayout";`

---

### Step 12 — Add to Sidebar `Views/Shared/_Sidebar.cshtml`

```html
<li class="nav-item">
    <a asp-controller="Category" asp-action="Index"
       class="nav-link @(controller == "Category" ? "active" : "")">
        <i class="nav-icon fas fa-tags"></i>
        <p>Categories</p>
    </a>
</li>
```

---

### Step 13 — Create and apply the migration

```bash
dotnet ef migrations add AddCategoryTable \
  --project CleanMvcApp.Infrastructure \
  --startup-project CleanMvcApp.Web

dotnet ef database update \
  --project CleanMvcApp.Infrastructure \
  --startup-project CleanMvcApp.Web
```

---

### Quick Reference Checklist

```
[ ] Domain    → CleanMvcApp.Domain/Entities/Category.cs
[ ] DTO       → CleanMvcApp.Application/DTOs/CategoryDto.cs
[ ] IRepo     → CleanMvcApp.Application/Interfaces/ICategoryRepository.cs
[ ] IService  → CleanMvcApp.Application/Interfaces/ICategoryService.cs
[ ] Service   → CleanMvcApp.Application/Services/CategoryService.cs
[ ] Repo      → CleanMvcApp.Infrastructure/Repositories/CategoryRepository.cs
[ ] DbContext → Add DbSet<Category> + optional HasData() seed
[ ] DI        → Register in Program.cs
[ ] ViewModel → CleanMvcApp.Web/ViewModels/CategoryViewModel.cs
[ ] Controller→ CleanMvcApp.Web/Controllers/CategoryController.cs
[ ] Views     → CleanMvcApp.Web/Views/Category/ (Index, Create, Edit, Details, Delete)
[ ] Sidebar   → Add nav-item in _Sidebar.cshtml
[ ] Migration → dotnet ef migrations add + database update
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
