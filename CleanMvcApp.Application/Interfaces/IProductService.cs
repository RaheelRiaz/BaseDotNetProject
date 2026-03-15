using CleanMvcApp.Application.DTOs;

namespace CleanMvcApp.Application.Interfaces;

public interface IProductService
{
    Task<IEnumerable<ProductDto>> GetAllProductsAsync();
    Task<ProductDto?> GetProductByIdAsync(int id);
    Task CreateProductAsync(ProductDto dto);
    Task UpdateProductAsync(ProductDto dto);
    Task DeleteProductAsync(int id);
}
