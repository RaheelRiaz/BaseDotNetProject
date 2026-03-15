using CleanMvcApp.Application.DTOs;
using CleanMvcApp.Application.Interfaces;
using CleanMvcApp.Domain.Entities;

namespace CleanMvcApp.Application.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
    {
        var products = await _productRepository.GetAllAsync();
        return products.Select(MapToDto);
    }

    public async Task<ProductDto?> GetProductByIdAsync(int id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        return product is null ? null : MapToDto(product);
    }

    public async Task CreateProductAsync(ProductDto dto)
    {
        var product = new Product
        {
            Name = dto.Name,
            Description = dto.Description,
            Price = dto.Price,
            CreatedDate = DateTime.UtcNow
        };
        await _productRepository.AddAsync(product);
    }

    public async Task UpdateProductAsync(ProductDto dto)
    {
        var product = await _productRepository.GetByIdAsync(dto.Id);
        if (product is null) return;

        product.Name = dto.Name;
        product.Description = dto.Description;
        product.Price = dto.Price;

        await _productRepository.UpdateAsync(product);
    }

    public async Task DeleteProductAsync(int id)
    {
        await _productRepository.DeleteAsync(id);
    }

    private static ProductDto MapToDto(Product product) => new()
    {
        Id = product.Id,
        Name = product.Name,
        Description = product.Description,
        Price = product.Price,
        CreatedDate = product.CreatedDate
    };
}
