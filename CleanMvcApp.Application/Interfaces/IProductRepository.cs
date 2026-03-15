using CleanMvcApp.Domain.Entities;

namespace CleanMvcApp.Application.Interfaces;

public interface IProductRepository : IRepository<Product>
{
    Task<IEnumerable<Product>> SearchAsync(string keyword);
}
