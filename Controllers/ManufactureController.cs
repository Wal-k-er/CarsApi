using Cars.Dto;
using Cars.Exceptions;
using Cars.Features.ManufactureFeatures.Commands;
using Cars.Features.ManufactureFeatures.Queries;
using Cars.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Cars.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ManufactureController : Controller
{
    //Внедрение зависимости
    private readonly IMediator _mediator;

    public ManufactureController(IMediator mediator)
    {
        _mediator = mediator;
    }

    //Получение списка производств
    [HttpGet]
    [ProducesResponseType(200, Type = typeof(ICollection<Manufacture>))]
    public async Task<IActionResult> GetManufactures()
    {
        var query = new GetAllManufactures();
        var response = await _mediator.Send(query);

        return Ok(response);
    }

    //Получение информации о производстве
    [HttpGet("{manufactureId}")]
    [ProducesResponseType(200, Type = typeof(Manufacture))]
    [ProducesResponseType(400)]
    public async Task<IActionResult> GetManufacture(int manufactureId)
    {
        try
        {
            var query = new GetManufactureById(manufactureId);
            var response = await _mediator.Send(query);
            return Ok(response);
        }
        catch (EntityNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

    //Получение списка машин, производящихся на заводе
    [HttpGet("{manufactureId}/cars")]
    [ProducesResponseType(200, Type = typeof(ICollection<Car>))]
    public async Task<IActionResult> GetCarsByManufacture(int manufactureId)
    {
        try
        {
            var query = new GetCarsByManufacture(manufactureId);
            var response = await _mediator.Send(query);
            return Ok(response);
        }
        catch (EntityNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

    //Внесение информации о производстве
    [HttpPost]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> CreateManufacture([FromBody] ManufactureDto manufactureCreate,
        [FromQuery] int countryId)
    {
        try
        {
            var command = new CreateManufacture(manufactureCreate, countryId);
            var response = await _mediator.Send(command);
            return Ok("Создано производство " + response);
        }
        catch (EntityNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

    //Обновление информации о производстве
    [HttpPut("{manufactureId}")]
    [ProducesResponseType(400)]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> UpdateManufacture(int manufactureId, [FromBody] ManufactureDto updatedManufacture,
        [FromQuery] int countryId)
    {
        try
        {
            var command = new UpdateManufacture(manufactureId, updatedManufacture);
            var response = await _mediator.Send(command);
            return Ok("Обновлено производство " + response);
        }
        catch (EntityNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

    //Удаление информации о производстве
    [HttpDelete("{manufactureId}")]
    [ProducesResponseType(400)]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> DeleteManufacture(int manufactureId)
    {
        try
        {
            var command = new DeleteManufacture(manufactureId);
            var response = await _mediator.Send(command);
            return Ok("Удалено производство " + response);
        }
        catch (EntityNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

    //Удаление информации о машине на производстве
    [HttpDelete("{manufactureId}/car/{carId}")]
    [ProducesResponseType(400)]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> DeleteCarManufacture(int manufactureId, int carId)
    {
        try
        {
            var command = new DeleteCarManufacture(manufactureId, carId);
            var response = await _mediator.Send(command);
            return Ok("Удалена машина " + response);
        }
        catch (EntityNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }
}