using Dapper;
using Domain.Models;
using Infrastructure.DataContext;

namespace Infrastructure.Services;

public class OrderDetailService
{
    private readonly DapperContext _context;

    public OrderDetailService()
    {
        _context = new DapperContext();
    }

    internal async Task<string> Add(OrderDetail some)
    {
        var sql1 = @"select StockQuantity from Products as p where p.ProductId=@ProductId";
        var stockQuantity = _context.Connection().QueryFirst<int>(sql1, new { ProductId = some.ProductId });
        if (stockQuantity < some.Quantity)
        {
            return $"stock have not this quantity of product";
        }
        var sql2 = @"select Price from Products as p where p.ProductId=@ProductId ";
        var unitPrice = _context.Connection().QueryFirst<decimal>(sql2, new { ProductId = some.ProductId });
        some.UnitPrice = unitPrice;
        var sql3 = @"insert into OrderDetails (OrderId,ProductId,Quantity,UnitPrice) values(@OrderId,@ProductId,@Quantity,@UnitPrice); ";
        await _context.Connection().ExecuteAsync(sql3, some);

        stockQuantity -= some.Quantity;
        var sql4 = @"update Products set StockQuantity=@StockQuantity where ProductId=@ProductId";
        await _context.Connection().ExecuteAsync(sql4, new { StockQuantity = stockQuantity, ProductId = some.ProductId });
        return $"Added successfully!";
    }

    public async Task<string> Delete(int id)
    {
        var sql = @"delete from OrderDetails where OrderDetailId = @OrderDetailId";
        await _context.Connection().ExecuteAsync(sql, new { OrderDetailId = id });
        return $"deleted successfully!";
    }

    public async Task<List<OrderDetail>> Get()
    {
        var sql = @"select * from OrderDetails";
        var result = await _context.Connection().QueryAsync<OrderDetail>(sql);
        return result.ToList();
    }

    public async Task<string> Update(OrderDetail some)
    {
        var sql = @"update OrderDetails set OrderId=@OrderId,ProductId=@ProductId,Quantity=@Quantity,UnitPrice=@UnitPrice where OrderDetailId=@OrderDetailId ";
        await _context.Connection().ExecuteAsync(sql, some);
        return $"updated successfully!";
    }

    // 3
    public async Task<List<ProductOrderCount>> GetProductsOrderCount()
    {
        var sql1 = @"select ProductId from Products";
        var result = await _context.Connection().QueryAsync<int>(sql1);
        var products_id = result.ToList();
        var sql2 = @"select Quantity from OrderDetails 
        where ProductId = @ProductId
        ";
        var productsOrderCount = new List<ProductOrderCount>();
        foreach (var item in products_id)
        {
            var productOrderCount = new ProductOrderCount();
            var orderDetails = _context.Connection().Query<int>(sql2, new { ProductId = item }).ToList();
            foreach (var item2 in orderDetails)
            {
                var count = 0;
                count += item2;
                productOrderCount.OrderCount = count;
            }
            productOrderCount.Product = _context.Connection().QueryFirst<Product>(@"select * from Products where ProductId = @ProductId", new { ProductId = item });
            productsOrderCount.Add(productOrderCount);
        }
        return productsOrderCount;
    }



}
