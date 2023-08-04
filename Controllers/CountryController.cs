using Cars.Dto;
using Cars.Exceptions;
using Cars.Features.CountryFeatures.Commands;
using Cars.Features.CountryFeatures.Queries;
using Cars.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Cars.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CountryController : Controller
{
    //Внедрение зависимостей

    private readonly IMediator _mediator;

    public CountryController(IMediator mediator)
    {
        _mediator = mediator;
    }

    //Получение списка стран
    [HttpGet]
    [ProducesResponseType(200, Type = typeof(ICollection<Country>))]
    public async Task<IActionResult> GetCountries()
    {
        var query = new GetAllCountries();
        var response = await _mediator.Send(query);

        return Ok(response);
    }

    //Получение информации о стране
    [HttpGet("{countryId}")]
    [ProducesResponseType(200, Type = typeof(Country))]
    [ProducesResponseType(400)]
    public async Task<IActionResult> GetCountry(int countryId)
    {
        try
        {
            var query = new GetCountryById(countryId);
            var response = await _mediator.Send(query);
            return Ok(response);
        }
        catch (EntityNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

    //Получение информации о производствах, находящихся в стране
    [HttpGet("{countryId}/manufactures")]
    [ProducesResponseType(200, Type = typeof(Country))]
    [ProducesResponseType(400)]
    public async Task<IActionResult> GetManufacturesByCountry(int countryId)
    {
        try
        {
            var query = new GetManufacturesByCountry(countryId);
            var response = await _mediator.Send(query);
            return Ok(response);
        }
        catch (EntityNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

    //Внесение информации о стране
    [HttpPost]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> CreateCountry([FromBody] CountryDto countryCreate)
    {
        try
        {
            var command = new CreateCountry(countryCreate);
            var response = await _mediator.Send(command);
            return Ok("Создана страна " + response);
        }
        catch (EntityNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

    //Обновление информации о стране
    [HttpPut("{countryId}")]
    [ProducesResponseType(400)]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> UpdateCategory(int countryId, [FromBody] CountryDto updatedCountry)
    {
        try
        {
            var command = new UpdateCountry(countryId, updatedCountry);
            var response = await _mediator.Send(command);
            return Ok("Обновлена страна " + response);
        }
        catch (EntityNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

    //Удаление информации о стране
    [HttpDelete("{countryId}")]
    [ProducesResponseType(400)]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> DeleteCountry(int countryId)
    {
        try
        {
            var command = new DeleteCountry(countryId);
            var response = await _mediator.Send(command);
            return Ok("Удалена страна " + response);
        }
        catch (EntityNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }
}