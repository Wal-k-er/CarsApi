using AutoMapper;
using Cars.Dto;
using Cars.Interfaces;
using Cars.Models;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Cars.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ManufactureController : Controller
{
    private readonly ICarRepository _carRepository;

    private readonly ICountryRepository _countryRepository;

    //Внедрение зависимостей
    private readonly IManufactureRepository _manufactureRepository;
    private readonly IMapper _mapper;
    private readonly IValidator<ManufactureDto> _validator;

    public ManufactureController(IManufactureRepository manufactureRepository,
        ICountryRepository countryRepository, ICarRepository carRepository,
        IValidator<ManufactureDto> validator, IMapper mapper)
    {
        _manufactureRepository = manufactureRepository;
        _countryRepository = countryRepository;
        _carRepository = carRepository;
        _validator = validator;
        _mapper = mapper;
    }

    //Получение списка производств
    [HttpGet]
    [ProducesResponseType(200, Type = typeof(ICollection<Manufacture>))]
    public IActionResult GetManufactures()
    {
        var manufactures = _mapper.Map<List<ManufactureDto>>(_manufactureRepository.GetManufactures());

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(manufactures);
    }

    //Получение информации о производстве
    [HttpGet("{manufactureId}")]
    [ProducesResponseType(200, Type = typeof(Manufacture))]
    [ProducesResponseType(400)]
    public IActionResult GetManufacture(int manufactureId)
    {
        if (!_manufactureRepository.ManufactureExists(manufactureId))
            return NotFound();

        var manufacture = _mapper.Map<ManufactureDto>(_manufactureRepository.GetManufacture(manufactureId));

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(manufacture);
    }

    //Получение списка машин, производящихся на заводе
    [HttpGet("{manufactureId}/cars")]
    [ProducesResponseType(200, Type = typeof(ICollection<Car>))]
    public IActionResult GetCarsByManufacture(int manufactureId)
    {
        if (!_manufactureRepository.ManufactureExists(manufactureId))
            return NotFound();

        var carsByManufacture = _mapper.Map<List<CarDto>>(
            _manufactureRepository.GetCarsByManufacture(manufactureId));

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(carsByManufacture);
    }

    //Внесение информации о производстве
    [HttpPost]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    public IActionResult CreateManufacture([FromBody] ManufactureDto manufactureCreate, [FromQuery] int countryId)
    {
        var validationResult = _validator.Validate(manufactureCreate);

        if (!validationResult.IsValid) return BadRequest(validationResult.Errors);

        var manufacture = _manufactureRepository
            .GetManufactures()
            .FirstOrDefault(c => c.Name.Trim().ToLower() ==
                                 manufactureCreate.Name.Trim().ToLower());

        if (manufacture != null)
        {
            ModelState.AddModelError("", "Производство уже есть");
            return StatusCode(422, ModelState);
        }

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var manufactureMap = _mapper.Map<Manufacture>(manufactureCreate);

        manufactureMap.Country = _countryRepository.GetCountry(countryId);

        if (!_manufactureRepository.CreateManufacture(manufactureMap))
        {
            ModelState.AddModelError("", "В процессе сохранения что-то пошло не так");
            return StatusCode(500, ModelState);
        }

        foreach (var manufacturedCar in manufactureCreate.ManufacturedCars)
        {
            var car = _carRepository.GetCar(manufacturedCar.CarId);
            var carManufacture = new CarManufacture
            {
                Manufacture = manufactureMap,
                ManufactureId = manufactureMap.Id,
                Car = car,
                CarId = car.Id
            };
            _manufactureRepository.CreateCarManufacture(carManufacture);
        }

        return Ok("Успешно сохранено");
    }

    //Обновление информации о производстве
    [HttpPut("{manufactureId}")]
    [ProducesResponseType(400)]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public IActionResult UpdateManufacture(int manufactureId, [FromBody] ManufactureDto updatedManufacture,
        [FromQuery] int countryId)
    {
        var validationResult = _validator.Validate(updatedManufacture);

        if (!validationResult.IsValid) return BadRequest(validationResult.Errors);

        if (!_manufactureRepository.ManufactureExists(manufactureId))
            return NotFound();

        if (!ModelState.IsValid)
            return BadRequest();

        var manufactureMap = _mapper.Map<Manufacture>(updatedManufacture);

        manufactureMap.Country = _countryRepository.GetCountry(countryId);
        if (!_manufactureRepository.UpdateManufacture(manufactureMap))
        {
            ModelState.AddModelError("", "Что-то пошло не так в процессе обновления");
            return StatusCode(500, ModelState);
        }

        var carManufactures = _manufactureRepository
            .GetManufacture(manufactureMap.Id).CarManufactures;
        foreach (var manufacturedCar in updatedManufacture.ManufacturedCars)
        {
            if (!_carRepository.CarExists(manufacturedCar.CarId))
                return NotFound();
            var car = _carRepository.GetCar(manufacturedCar.CarId);
            if (!carManufactures.Any(c => c.Car == car))
            {
                var carManufacture = new CarManufacture
                {
                    Manufacture = manufactureMap,
                    ManufactureId = manufactureMap.Id,
                    Car = car,
                    CarId = car.Id
                };
                _manufactureRepository.CreateCarManufacture(carManufacture);
            }
        }

        return NoContent();
    }

    //Удаление информации о производстве
    [HttpDelete("{manufactureId}")]
    [ProducesResponseType(400)]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public IActionResult DeleteManufacture(int manufactureId)
    {
        if (!_manufactureRepository.ManufactureExists(manufactureId)) return NotFound();

        var manufactureToDelete = _manufactureRepository.GetManufacture(manufactureId);

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (!_manufactureRepository.DeleteManufacture(manufactureToDelete))
            ModelState.AddModelError("", "Что-то пошло не так в процессе удаления");

        return NoContent();
    }
}