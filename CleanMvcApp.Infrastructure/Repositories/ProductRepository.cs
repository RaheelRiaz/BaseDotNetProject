using CleanMvcApp.Application.Interfaces;
using CleanMvcApp.Domain.Entities;
using CleanMvcApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CleanMvcApp.Infrastructure.Repositories;

public class ProductRepository : Repository<Product>, IProductRepository
{
    public ProductRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Product>> SearchAsync(string keyword)
        => await _dbSet
            .Where(p => p.Name.Contains(keyword) || p.Description.Contains(keyword))
            .ToListAsync();
}
