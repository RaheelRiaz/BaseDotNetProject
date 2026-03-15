using CleanMvcApp.Application.DTOs;
using CleanMvcApp.Application.Interfaces;
using CleanMvcApp.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanMvcApp.Web.Controllers;

[Authorize]
public class ProductController : Controller
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    public async Task<IActionResult> Index()
    {
        var products = await _productService.GetAllProductsAsync();
        var viewModels = products.Select(MapToViewModel);
        return View(viewModels);
    }

    public async Task<IActionResult> Details(int id)
    {
        var product = await _productService.GetProductByIdAsync(id);
        if (product is null) return NotFound();
        return View(MapToViewModel(product));
    }

    [Authorize(Roles = "Admin")]
    public IActionResult Create() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(ProductViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        await _productService.CreateProductAsync(MapToDto(model));
        TempData["Success"] = "Product created successfully.";
        return RedirectToAction(nameof(Index));
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(int id)
    {
        var product = await _productService.GetProductByIdAsync(id);
        if (product is null) return NotFound();
        return View(MapToViewModel(product));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(int id, ProductViewModel model)
    {
        if (id != model.Id) return BadRequest();
        if (!ModelState.IsValid) return View(model);

        await _productService.UpdateProductAsync(MapToDto(model));
        TempData["Success"] = "Product updated successfully.";
        return RedirectToAction(nameof(Index));
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var product = await _productService.GetProductByIdAsync(id);
        if (product is null) return NotFound();
        return View(MapToViewModel(product));
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _productService.DeleteProductAsync(id);
        TempData["Success"] = "Product deleted successfully.";
        return RedirectToAction(nameof(Index));
    }

    private static ProductViewModel MapToViewModel(ProductDto dto) => new()
    {
        Id = dto.Id,
        Name = dto.Name,
        Description = dto.Description,
        Price = dto.Price,
        CreatedDate = dto.CreatedDate
    };

    private static ProductDto MapToDto(ProductViewModel vm) => new()
    {
        Id = vm.Id,
        Name = vm.Name,
        Description = vm.Description,
        Price = vm.Price
    };
}
