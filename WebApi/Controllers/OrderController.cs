using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Domain.Models;

namespace WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class OrderController : ControllerBase
{
    private readonly OrderService _orderService;

    public OrderController()
    {
        _orderService = new OrderService();
    }

    [HttpPost("add-order")]
    public async Task<string> Add([FromQuery] Order some, List<OrderDetail> orderDetails)
    {
        return await _orderService.Add(some, orderDetails);
    }

    [HttpDelete("delete-order")]
    public async Task<string> Delete(int id)
    {
        return await _orderService.Delete(id);
    }

    [HttpGet("get-orders")]
    public async Task<List<Order>> Get()
    {
        return await _orderService.Get();
    }

    [HttpPut("update-order")]
    public async Task<string> Update(Order some)
    {
        return await _orderService.Update(some);
    }

    [HttpGet("get-orders-with-orderdetails")]
    public async Task<List<ListOfSome<Order, OrderDetailType>>> GetOrdersWithOrderDetails()
    {
        return await _orderService.GetOrdersWithOrderDetails();
    }

    [HttpGet("get-customers-by-orderdate")]
    public async Task<List<Customer>> GetCustomersByOrderDate(DateTime date)
    {
        return await _orderService.GetCustomersByOrderDate(date);
    }

    [HttpGet("get-customer-order-by-date")]
    public async Task<List<Order>> GetCustomerOrdersByDate(DateTime orderDate, int customerId)
    {
        return await _orderService.GetCustomerOrdersByDate(orderDate, customerId);
    }
}
