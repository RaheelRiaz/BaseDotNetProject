using CleanMvcApp.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CleanMvcApp.Infrastructure.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Product> Products => Set<Product>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.Price).HasColumnType("decimal(18,2)");

            // Seed data — baked into migration, runs on database update
            entity.HasData(
                new Product { Id = 1, Name = "Laptop Pro 15", Description = "High-performance laptop with 16GB RAM and 512GB SSD", Price = 1299.99m, CreatedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new Product { Id = 2, Name = "Wireless Mouse", Description = "Ergonomic wireless mouse with long battery life", Price = 29.99m, CreatedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new Product { Id = 3, Name = "Mechanical Keyboard", Description = "RGB backlit mechanical keyboard with Cherry MX switches", Price = 89.99m, CreatedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new Product { Id = 4, Name = "4K Monitor", Description = "27-inch 4K UHD IPS display with HDR support", Price = 449.99m, CreatedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new Product { Id = 5, Name = "USB-C Hub", Description = "7-in-1 USB-C hub with HDMI, USB 3.0, and SD card reader", Price = 49.99m, CreatedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) }
            );
        });
    }
}
