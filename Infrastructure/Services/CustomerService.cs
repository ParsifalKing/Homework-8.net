using Dapper;
using Domain.Models;
using Infrastructure.DataContext;

namespace Infrastructure.Services;

public class CustomerService : IService<Customer>
{
    private readonly DapperContext _context;

    public CustomerService()
    {
        _context = new DapperContext();
    }

    public async Task<string> Add(Customer some)
    {
        var sql = @"insert into Customers (CustomerName,Email,Address) values(@CustomerName,@Email,@Address); ";
        await _context.Connection().ExecuteAsync(sql, some);
        return $"Added successfully!";
    }

    public async Task<string> Delete(int id)
    {
        var sql = @"delete from Customers where CustomerId = @CustomerId";
        await _context.Connection().ExecuteAsync(sql, new { CustomerId = id });
        return $"deleted successfully!";
    }

    public async Task<List<Customer>> Get()
    {
        var sql = @"select * from Customers";
        var result = await _context.Connection().QueryAsync<Customer>(sql);
        return result.ToList();
    }

    public async Task<string> Update(Customer some)
    {
        var sql = @"update Customers set CustomerName=@CustomerName,Email=@Email,Address=@Address where CustomerId=@CustomerId ";
        await _context.Connection().ExecuteAsync(sql, some);
        return $"updated successfully!";
    }


    // 4
    public async Task<List<CustomerAverageOrders>> GetAverageOrdersOfCustomers()
    {
        var sql1 = @"select CustomerId from Customers";
        var result1 = await _context.Connection().QueryAsync<int>(sql1);
        var customers_id = result1.ToList();

        var sql2 = @"select * from Customers where CustomerId=@CustomerId;
        select * from Orders where CustomerId = @CustomerId;
        ";

        var customersOrders = new List<CustomerAverageOrders>();
        foreach (var item in customers_id)
        {
            using (var multiple = _context.Connection().QueryMultiple(sql2, new { CustomerId = item }))
            {
                var orders = new CustomerAverageOrders();
                orders.Customer = multiple.ReadFirst<Customer>();
                var averageOrder = multiple.Read<Order>().ToList();
                foreach (var average in averageOrder)
                {
                    orders.AverageOfOrders += average.TotalAmount;
                }
                orders.AverageOfOrders /= averageOrder.Count;
                customersOrders.Add(orders);
            }
        }
        return customersOrders;
    }

}
