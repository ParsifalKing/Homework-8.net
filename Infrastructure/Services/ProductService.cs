using Dapper;
using Domain.Models;
using Infrastructure.DataContext;

namespace Infrastructure.Services;

public class ProductService : IService<Product>
{

    private readonly DapperContext _context;

    public ProductService()
    {
        _context = new DapperContext();
    }

    public async Task<string> Add(Product some)
    {
        var sql = @"insert into Products (ProductName,Price,StockQuantity) values(@ProductName,@Price,@StockQuantity); ";
        await _context.Connection().ExecuteAsync(sql, some);
        return $"Added successfully!";
    }

    public async Task<string> Delete(int id)
    {
        var sql = @"delete from Products where ProductId = @ProductId";
        await _context.Connection().ExecuteAsync(sql, new { ProductId = id });
        return $"deleted successfully!";
    }

    public async Task<List<Product>> Get()
    {
        var sql = @"select * from Products";
        var result = await _context.Connection().QueryAsync<Product>(sql);
        return result.ToList();
    }

    public async Task<string> Update(Product some)
    {
        var sql = @"update Products set ProductName=@ProductName,Price=@Price,StockQuantity=@StockQuantity where ProductId=@ProductId ";
        await _context.Connection().ExecuteAsync(sql, some);
        return $"updated successfully!";
    }
}
