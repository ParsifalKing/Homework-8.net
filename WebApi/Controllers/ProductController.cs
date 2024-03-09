using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Domain.Models;

namespace WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductController
{
    private readonly ProductService _productService;

    public ProductController()
    {
        _productService = new ProductService();
    }

    [HttpPost("add-product")]
    public async Task<string> Add(Product some)
    {
        return await _productService.Add(some);
    }

    [HttpDelete("delete-product")]
    public async Task<string> Delete(int id)
    {
        return await _productService.Delete(id);
    }

    [HttpGet("get-products")]
    public async Task<List<Product>> Get()
    {
        return await _productService.Get();
    }

    [HttpPut("update-product")]
    public async Task<string> Update(Product some)
    {
        return await _productService.Update(some);
    }
}
