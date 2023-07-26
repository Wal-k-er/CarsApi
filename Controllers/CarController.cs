using AutoMapper;
using Cars.Dto;
using Cars.Interfaces;
using Cars.Models;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Cars.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CarController: Controller
{
    //Внедрение зависимостей
    private readonly ICarRepository _carRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IValidator<CarDto> _validator;
    private readonly IMapper _mapper;
    
    public CarController(ICarRepository carRepository,
        ICategoryRepository categoryRepository,
        IValidator<CarDto> validator,IMapper mapper)
    {
        _carRepository = carRepository;
        _categoryRepository = categoryRepository;
        _validator = validator;
        _mapper = mapper;
    }

    //Получение списка машин
    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IEnumerable<Car>))]
    public IActionResult GetCars()
    {
        var cars = _mapper.Map<List<CarDto>>(_carRepository.GetCars());

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(cars);
    }

    //Получение информации о машине
    [HttpGet("{carId}")]
    [ProducesResponseType(200, Type = typeof(Car))]
    [ProducesResponseType(400)]
    public IActionResult GetCar(int carId)
    {
        if (!_carRepository.CarExists(carId))
            return NotFound();

        var car = _mapper.Map<CarDto>(_carRepository.GetCar(carId));

        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        return Ok(car);
    }
    
    //Получение списка заводов, на которых производится машина
    [HttpGet("{carId}/manufactures")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<Manufacture>))]
    public IActionResult GetCarManufactures(int carId)
    {
        if (!_carRepository.CarExists(carId))
            return NotFound();
        
        var manufactures = _mapper.Map<List<ManufactureDto>>(_carRepository.getCarManufactures(carId)) ;

        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        return Ok(manufactures);
    }
    
    //Внесение информации о новой машине
    [HttpPost]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    public IActionResult CreateCar([FromBody] CarDto carCreate, [FromQuery] int categoryId)
    {
        var validationResult = _validator.Validate(carCreate);

        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }
        
        //Проверка отсутствия вносимой машины в базе
        var car = _carRepository
            .GetCars()
            .FirstOrDefault(c => c.Name.Trim().ToLower() == carCreate.Name.Trim().ToLower());
        
        if (car != null)
        {
            ModelState.AddModelError("", "Машина уже есть");
            return StatusCode(422, ModelState);
        }

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var carMap = _mapper.Map<Car>(carCreate);
        carMap.Category = _categoryRepository.GetCategory(categoryId);

        if (!_carRepository.CreateCar(carMap))
        {
            ModelState.AddModelError("","В процессе сохранения что-то пошло не так");
            return StatusCode(500, ModelState);
        }

        return Ok("Успешно сохранено");
    }
    
    //Обновление информации о машине
    [HttpPut("{carId}")]
    [ProducesResponseType(400)]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public IActionResult UpdateCar(int carId, [FromBody]CarDto updatedCar)
    {
        var validationResult = _validator.Validate(updatedCar);

        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        if (!_carRepository.CarExists(carId))
            return NotFound();

        if (!ModelState.IsValid)
            return BadRequest();

        var carMap = _mapper.Map<Car>(updatedCar);
        carMap.Id = carId;

        if(!_carRepository.UpdateCar(carMap))
        {
            ModelState.AddModelError("", "Что-то пошло не так в процессе обновления");
            return StatusCode(500, ModelState);
        }

        return NoContent();
    }
    
    //Удаление информации о машине
    [HttpDelete("{carId}")]
    [ProducesResponseType(400)]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public IActionResult DeleteCar(int carId)
    {
        if(!_carRepository.CarExists(carId))
        {
            return NotFound();
        }

        var carToDelete = _carRepository.GetCar(carId);

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if(!_carRepository.DeleteCar(carToDelete))
            ModelState.AddModelError("", "Что-то пошло не так в процессе удаления");

        return NoContent();
    }
}