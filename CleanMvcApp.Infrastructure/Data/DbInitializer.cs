using CleanMvcApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CleanMvcApp.Infrastructure.Data;

public static class DbInitializer
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        // Apply any pending migrations automatically
        await context.Database.MigrateAsync();

        // HasData() in OnModelCreating handles static seed data via migrations.
        // Use this method only for dynamic/environment-specific seeding.

        await SeedProductsAsync(context);
    }

    private static async Task SeedProductsAsync(ApplicationDbContext context)
    {
        // Skip if already seeded (HasData migration seed covers the base records)
        if (await context.Products.AnyAsync()) return;

        // This block only runs if the table is completely empty
        // (e.g., HasData was not used or migrations were not applied with seed)
        var products = new List<Product>
        {
            new() { Name = "Laptop Pro 15",       Description = "High-performance laptop with 16GB RAM and 512GB SSD",        Price = 1299.99m, CreatedDate = DateTime.UtcNow },
            new() { Name = "Wireless Mouse",       Description = "Ergonomic wireless mouse with long battery life",            Price = 29.99m,   CreatedDate = DateTime.UtcNow },
            new() { Name = "Mechanical Keyboard",  Description = "RGB backlit mechanical keyboard with Cherry MX switches",    Price = 89.99m,   CreatedDate = DateTime.UtcNow },
            new() { Name = "4K Monitor",           Description = "27-inch 4K UHD IPS display with HDR support",               Price = 449.99m,  CreatedDate = DateTime.UtcNow },
            new() { Name = "USB-C Hub",            Description = "7-in-1 USB-C hub with HDMI, USB 3.0, and SD card reader",   Price = 49.99m,   CreatedDate = DateTime.UtcNow },
        };

        await context.Products.AddRangeAsync(products);
        await context.SaveChangesAsync();
    }
}
