using System.Diagnostics;
using AutoMapper;
using Cars.Dto;
using Cars.Interfaces;
using Cars.Models;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Cars.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CountryController : Controller
{
    //Внедрение зависимостей
    private readonly ICountryRepository _countryRepository;
    private readonly IValidator<CountryDto> _validator;
    private readonly IMapper _mapper;

    public CountryController(ICountryRepository countryRepository, 
        IValidator<CountryDto> validator, IMapper mapper)
    {
        _countryRepository = countryRepository;
        _validator = validator;
        _mapper = mapper;
    }

    //Получение списка стран
    [HttpGet]
    [ProducesResponseType(200, Type = typeof(ICollection<Country>))]
    public IActionResult GetCountries()
    {
        var countries = _mapper.Map<List<CountryDto>>(_countryRepository.GetCountries());
        
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        return Ok(countries);
    }

    //Получение информации о стране
    [HttpGet("{countryId}")]
    [ProducesResponseType(200, Type = typeof(Country))]
    [ProducesResponseType(400)]
    public IActionResult GetCountry(int countryId)
    {
        if (!_countryRepository.CountryExsists(countryId))
            return NotFound();
        
        var country = _mapper.Map<CountryDto>(_countryRepository.GetCountry(countryId));
        
        return Ok(country);
    }

    //Получение информации о производствах, находящихся в стране
    [HttpGet("{countryId}/manufactures")]
    [ProducesResponseType(200, Type = typeof(Country))]
    [ProducesResponseType(400)]
    public IActionResult GetManufacturesByCountry(int countryId)
    {
        if (!_countryRepository.CountryExsists(countryId))
            return NotFound();
        
        var manufactures = _mapper.Map<List<ManufactureDto>>(
            _countryRepository.GetManufacturesByCountry(countryId));
        
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        return Ok(manufactures);
    }
    
    //Внесение информации о стране
    [HttpPost]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    public IActionResult CreateCountry([FromBody] CountryDto countryCreate)
    {
        var validationResult = _validator.Validate(countryCreate);

        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }
        
        var country = _countryRepository
            .GetCountries()
            .FirstOrDefault(c => c.Name.Trim().ToLower() == 
                                 countryCreate.Name.Trim().ToLower());
        
        if (country != null)
        {
            ModelState.AddModelError("", "Страна уже есть");
            return StatusCode(422, ModelState);
        }

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var countryMap = _mapper.Map<Country>(countryCreate);

        if (!_countryRepository.CreateCountry(countryMap))
        {
            ModelState.AddModelError("","В процессе сохранения что-то пошло не так");
            return StatusCode(500, ModelState);
        }

        return Ok("Успешно сохранено");
    }
    
    //Обновление информации о стране
    [HttpPut("{countryId}")]
    [ProducesResponseType(400)]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public IActionResult UpdateCategory(int countryId, [FromBody]CountryDto updatedCountry)
    {
        var validationResult = _validator.Validate(updatedCountry);

        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        if (!_countryRepository.CountryExsists(countryId))
            return NotFound();

        if (!ModelState.IsValid)
            return BadRequest();

        var countryMap = _mapper.Map<Country>(updatedCountry);
        countryMap.Id = countryId;

        if(!_countryRepository.UpdateCountry(countryMap))
        {
            ModelState.AddModelError("", "Что-то пошло не так в процессе обновления");
            return StatusCode(500, ModelState);
        }

        return NoContent();
    }

    //Удаление информации о стране
    [HttpDelete("{countryId}")]
    [ProducesResponseType(400)]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public IActionResult DeleteCountry(int countryId)
    {
        if(!_countryRepository.CountryExsists(countryId))
        {
            return NotFound();
        }

        var countryToDelete = _countryRepository.GetCountry(countryId);

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if(!_countryRepository.DeleteCountry(countryToDelete))
            ModelState.AddModelError("", "Что-то пошло не так в процессе удаления");

        return NoContent();
    }
}