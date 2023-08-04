using Cars.Dto;
using Cars.Exceptions;
using Cars.Features.OrderFeatures.Commands;
using Cars.Features.OrderFeatures.Queries;
using Cars.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Cars.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrderController : Controller
{
    //Внедрение зависимостей
    private readonly IMediator _mediator;

    public OrderController(IMediator mediator)
    {
        _mediator = mediator;
    }

    //Получение информации о заказах
    [HttpGet]
    [ProducesResponseType(200, Type = typeof(ICollection<OrderDto>))]
    public async Task<IActionResult> GetOrdersWithItems()
    {
        var query = new GetAllOrders();
        var response = await _mediator.Send(query);

        return Ok(response);
    }

    //Получение информации о заказе
    [HttpGet("{orderId}")]
    [ProducesResponseType(200, Type = typeof(OrderDto))]
    [ProducesResponseType(400)]
    public async Task<IActionResult> GetOrderWithItems(int orderId)
    {
        try
        {
            var query = new GetOrderById(orderId);
            var response = await _mediator.Send(query);
            return Ok(response);
        }
        catch (EntityNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

    //Получение списка машин, указаных в заказе
    [HttpGet("{orderId}/cars")]
    [ProducesResponseType(200, Type = typeof(ICollection<Car>))]
    public async Task<IActionResult> GetCarsByOrder(int orderId)
    {
        try
        {
            var query = new GetCarsByOrder(orderId);
            var response = await _mediator.Send(query);
            return Ok(response);
        }
        catch (EntityNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

    //Внесение информации о заказе
    [HttpPost]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> CreateOrder([FromBody] OrderDto orderCreate, [FromQuery] int customerId)
    {
        try
        {
            var command = new CreateOrder(orderCreate, customerId);
            var response = await _mediator.Send(command);
            return Ok("Создан заказ " + response);
        }
        catch (EntityNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

    //Обновление информации о заказе
    [HttpPut("{orderId}")]
    [ProducesResponseType(400)]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> UpdateOrder(int orderId, [FromBody] OrderDto updatedOrder,
        [FromQuery] int customerId)
    {
        try
        {
            var command = new UpdateOrder(orderId, updatedOrder);
            var response = await _mediator.Send(command);
            return Ok("Обновлён заказ " + response);
        }
        catch (EntityNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

    //Удаление информации о заказе
    [HttpDelete("{orderId}")]
    [ProducesResponseType(400)]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> DeleteOrder(int orderId)
    {
        try
        {
            var command = new DeleteOrder(orderId);
            var response = await _mediator.Send(command);
            return Ok("Удалён заказ " + response);
        }
        catch (EntityNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

    //Удаление информации о позиции заказа
    [HttpDelete("{orderId}/car/{carId}")]
    [ProducesResponseType(400)]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> DeleteOrderItem(int orderId, int carId)
    {
        try
        {
            var command = new DeleteOrderItem(orderId, carId);
            var response = await _mediator.Send(command);
            return Ok("Удалена позиция заказа " + response);
        }
        catch (EntityNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }
}