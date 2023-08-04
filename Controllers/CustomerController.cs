using Cars.Dto;
using Cars.Exceptions;
using Cars.Features.CustomerFeatures.Commands;
using Cars.Features.CustomerFeatures.Queries;
using Cars.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Cars.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CustomerController : Controller
{
    //Внедрение зависимостей
    private readonly IMediator _mediator;

    public CustomerController(IMediator mediator)
    {
        _mediator = mediator;
    }

    //Получение информации о покупателях
    [HttpGet]
    [ProducesResponseType(200, Type = typeof(ICollection<Customer>))]
    public async Task<IActionResult> GetCustomers()
    {
        var query = new GetAllCustomers();
        var response = await _mediator.Send(query);

        return Ok(response);
    }

    //Получение информации о покупателе
    [HttpGet("{customerId}")]
    [ProducesResponseType(200, Type = typeof(Customer))]
    [ProducesResponseType(400)]
    public async Task<IActionResult> GetCustomer(int customerId)
    {
        try
        {
            var query = new GetCustomerById(customerId);
            var response = await _mediator.Send(query);
            return Ok(response);
        }
        catch (EntityNotFoundException e)
        {
            return NotFound();
        }
    }

    //Получение информации о заказах покупателя
    [HttpGet("{customerId}/orders")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<Order>))]
    [ProducesResponseType(400)]
    public async Task<IActionResult> GetOrdersByCustomer(int customerId)
    {
        try
        {
            var query = new GetOrdersByCustomer(customerId);
            var response = await _mediator.Send(query);
            return Ok(response);
        }
        catch (EntityNotFoundException e)
        {
            return NotFound();
        }
    }

    //Внесение информации о покупателе
    [HttpPost]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> CreateCustomer([FromBody] CustomerDto customerCreate)
    {
        try
        {
            var command = new CreateCustomer(customerCreate);
            var response = await _mediator.Send(command);
            return Ok("Создан заказчик " + response);
        }
        catch (EntityNotFoundException e)
        {
            return NotFound();
        }
        catch (InvalidRequestBodyException)
        {
            return BadRequest();
        }
    }

    //Обновление информации о покупателе
    [HttpPut("{customerId}")]
    [ProducesResponseType(400)]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> UpdateCategory(int customerId, [FromBody] CustomerDto updatedCustomer)
    {
        try
        {
            var command = new UpdateCustomer(customerId, updatedCustomer);
            var response = await _mediator.Send(command);
            return Ok("Обновлён заказчик " + response);
        }
        catch (EntityNotFoundException e)
        {
            return NotFound();
        }
        catch (InvalidRequestBodyException)
        {
            return BadRequest();
        }
    }

    //Удаление информации о покупателе
    [HttpDelete("{customerId}")]
    [ProducesResponseType(400)]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> DeleteCustomer(int customerId)
    {
        try
        {
            var command = new DeleteCustomer(customerId);
            var response = await _mediator.Send(command);
            return Ok("Удалён заказчик " + response);
        }
        catch (EntityNotFoundException e)
        {
            return NotFound();
        }
    }
}