using CleanMvcApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CleanMvcApp.Infrastructure.Data;

public static class DbInitializer
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        await context.Database.MigrateAsync();

        if (!context.Products.Any())
        {
            context.Products.AddRange(
                new Product { Name = "Sample Product 1", Description = "Description for product 1", Price = 29.99m, CreatedDate = DateTime.UtcNow },
                new Product { Name = "Sample Product 2", Description = "Description for product 2", Price = 49.99m, CreatedDate = DateTime.UtcNow },
                new Product { Name = "Sample Product 3", Description = "Description for product 3", Price = 99.99m, CreatedDate = DateTime.UtcNow }
            );
            await context.SaveChangesAsync();
        }
    }
}
