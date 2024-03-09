using Dapper;
using Domain.Models;
using Infrastructure.DataContext;

namespace Infrastructure.Services;

public class OrderService
{
    private readonly DapperContext _context;

    public OrderService()
    {
        _context = new DapperContext();
    }

    public async Task<string> Add(Order some, List<OrderDetail> orderDetails)
    {
        var sql = @"insert into Orders (CustomerId,OrderDate) values(@CustomerId,@OrderDate); ";
        await _context.Connection().ExecuteAsync(sql, some);
        var sql2 = @"select Max(OrderId) from Orders";
        var maxOrder = _context.Connection().QueryFirst<int>(sql2);
        some.OrderId = maxOrder;
        var orderDetailServ = new OrderDetailService();
        foreach (var item in orderDetails)
        {
            item.OrderId = some.OrderId;
            await orderDetailServ.Add(item);
        }

        foreach (var details in orderDetails)
        {
            decimal amount = details.Quantity * details.UnitPrice;
            some.TotalAmount += amount;
        }
        var sql3 = @"update Orders set TotalAmount = @TotalAmount where OrderId=@OrderId";
        _context.Connection().Execute(sql3, new { TotalAmount = some.TotalAmount, OrderId = some.OrderId });

        return $"Added successfully!";
    }


    public async Task<string> Delete(int id)
    {
        var sql = @"delete from Orders where OrderId = @OrderId";
        await _context.Connection().ExecuteAsync(sql, new { OrderId = id });
        return $"deleted successfully!";
    }

    public async Task<List<Order>> Get()
    {
        var sql = @"select * from Orders";
        var result = await _context.Connection().QueryAsync<Order>(sql);
        return result.ToList();
    }

    public async Task<string> Update(Order some)
    {
        var sql = @"update Orders set CustomerId=@CustomerId,OrderDate=@OrderDate,TotalAmount=@TotalAmount where OrderId=@OrderId ";
        await _context.Connection().ExecuteAsync(sql, some);
        return $"updated successfully!";
    }

    // 1
    public async Task<List<ListOfSome<Order, OrderDetailType>>> GetOrdersWithOrderDetails()
    {
        var sql1 = @"select OrderId from Orders;";
        var orderss_id = await _context.Connection().QueryAsync<int>(sql1);
        var orders_id = orderss_id.ToList();

        var sql2 = @"select * from Orders where OrderId=@OrderId;
        select * from OrderDetails as od
        join Products as p on p.ProductId = od.ProductId
        where od.OrderId=@OrderId;
        ";
        var ordersWithOrderDetails = new List<ListOfSome<Order, OrderDetailType>>();
        foreach (var id in orders_id)
        {
            using (var multiple = _context.Connection().QueryMultiple(sql2, new { OrderId = id }))
            {
                var orderAndOrderDetails = new ListOfSome<Order, OrderDetailType>();
                orderAndOrderDetails.Any = multiple.ReadFirst<Order>();
                orderAndOrderDetails.SomeList = multiple.Read<OrderDetailType>().ToList();
                ordersWithOrderDetails.Add(orderAndOrderDetails);
            }
        }
        return ordersWithOrderDetails;
    }

    // 2
    public async Task<List<Customer>> GetCustomersByOrderDate(DateTime date)
    {
        var sql1 = @"select * from Customers as c
        left join Orders as o on o.CustomerId = c.CustomerId
        where OrderDate >= @OrderDate;
        ";
        var result = await _context.Connection().QueryAsync<Customer>(sql1, new { OrderDate = date });
        return result.ToList();
    }

    // 5
    public async Task<List<Order>> GetCustomerOrdersByDate(DateTime orderDate, int customerId)
    {
        var sql = @"select * from Orders where OrderDate >= @OrderDate and 
        CustomerId = @CustomerId and 
        TotalAmount > 400";
        var result = await _context.Connection().QueryAsync<Order>(sql, new { OrderDate = orderDate, CustomerId = customerId });
        return result.ToList();
    }

}
