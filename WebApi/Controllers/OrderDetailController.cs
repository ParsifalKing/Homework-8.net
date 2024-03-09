using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Domain.Models;

namespace WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class OrderDetailController
{
    private readonly OrderDetailService _orderDetailService;

    public OrderDetailController()
    {
        _orderDetailService = new OrderDetailService();
    }

    // [HttpPost("add-orderDetail")]
    // public async Task<string> Add(OrderDetail some)
    // {
    //     return await _orderDetailService.Add(some);
    // }

    // [HttpDelete("delete-orderDetail")]
    // public async Task<string> Delete(int id)
    // {
    //     return await _orderDetailService.Delete(id);
    // }

    [HttpGet("get-orderDetails")]
    public async Task<List<OrderDetail>> Get()
    {
        return await _orderDetailService.Get();
    }

    [HttpGet("get-products-order-count")]
    public async Task<List<ProductOrderCount>> GetProductsOrderCount()
    {
        return await _orderDetailService.GetProductsOrderCount();

    }

    // [HttpPut("update-orderDetail")]
    // public async Task<string> Update(OrderDetail some)
    // {
    //     return await _orderDetailService.Update(some);
    // }
}
