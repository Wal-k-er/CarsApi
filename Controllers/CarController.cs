using Cars.Dto;
using Cars.Exceptions;
using Cars.Features.CarFeatures.Commands;
using Cars.Features.CarFeatures.Queries;
using Cars.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Cars.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CarController : Controller
{
    //Внедрение зависимостей

    private readonly IMediator _mediator;

    public CarController(IMediator mediator)
    {
        _mediator = mediator;
    }

    //Получение списка машин
    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IEnumerable<CarDto>))]
    public async Task<IActionResult> GetCars()
    {
        var query = new GetAllCars();
        var response = await _mediator.Send(query);

        return Ok(response);
    }

    //Получение информации о машине
    [HttpGet("{carId}")]
    [ProducesResponseType(200, Type = typeof(Car))]
    [ProducesResponseType(400)]
    public async Task<IActionResult> GetCar(int carId)
    {
        try
        {
            var query = new GetCarById(carId);
            var response = await _mediator.Send(query);
            return Ok(response);
        }
        catch (EntityNotFoundException e)
        {
            return NotFound();
        }
    }

    //Получение списка заводов, на которых производится машина
    [HttpGet("{carId}/manufactures")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<Manufacture>))]
    public async Task<IActionResult> GetCarManufactures(int carId)
    {
        try
        {
            var query = new GetCarManufactures(carId);
            var response = await _mediator.Send(query);
            return Ok(response);
        }
        catch (EntityNotFoundException e)
        {
            return NotFound();
        }
    }

    //Внесение информации о новой машине
    [HttpPost]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> CreateCar([FromBody] CarDto carCreate, [FromQuery] int categoryId)
    {
        try
        {
            var command = new CreateCar(carCreate, categoryId);
            var response = await _mediator.Send(command);
            return Ok("Создана машина " + response);
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

    //Обновление информации о машине
    [HttpPut("{carId}")]
    [ProducesResponseType(400)]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> UpdateCar(int carId, [FromBody] CarDto updatedCar)
    {
        try
        {
            var command = new UpdateCar(carId, updatedCar);
            var response = await _mediator.Send(command);
            return Ok("Обновлена машина " + response);
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

    //Удаление информации о машине
    [HttpDelete("{carId}")]
    [ProducesResponseType(400)]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> DeleteCar(int carId)
    {
        try
        {
            var command = new DeleteCar(carId);
            var response = await _mediator.Send(command);
            return Ok("Удалена машина " + response);
        }
        catch (EntityNotFoundException e)
        {
            return NotFound();
        }
    }
}