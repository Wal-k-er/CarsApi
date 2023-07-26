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
public class CategoryController : Controller
{
    
    //Внедрение зависимостей
    private readonly ICategoryRepository _categoryRepository;
    private readonly IValidator<CategoryDto> _validator;
    private readonly IMapper _mapper;

    public CategoryController(ICategoryRepository categoryRepository, 
        IValidator<CategoryDto> validator, IMapper mapper)
    {
        _categoryRepository = categoryRepository;
        _validator = validator;
        _mapper = mapper;
    }

    //Получение списка категорий
    [HttpGet]
    [ProducesResponseType(200, Type = typeof(ICollection<Category>))]
    public IActionResult GetCategories()
    {
        var categories = _mapper.Map<List<CategoryDto>>(_categoryRepository.GetCategories());
        
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        return Ok(categories);
    }

    //Получение информации о категории
    [HttpGet("{categoryId}")]
    [ProducesResponseType(200, Type = typeof(Category))]
    [ProducesResponseType(400)]
    public IActionResult GetCategory(int categoryId)
    {
        if (!_categoryRepository.CategoryExists(categoryId))
            return NotFound();
        
        var category = _mapper.Map<CategoryDto>(_categoryRepository.GetCategory(categoryId));
        
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        return Ok(category);
    }

    //Получение информации о машинах в категории
    [HttpGet("{categoryId}/cars")]
    [ProducesResponseType(200, Type = typeof(ICollection<Car>))]
    public IActionResult GetCarsByCategory(int categoryId)
    {
        if (!_categoryRepository.CategoryExists(categoryId))
            return NotFound();
        
        var carsByCategory = _mapper.Map<List<CarDto>>(_categoryRepository.GetCarsByCategory(categoryId));
        
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        return Ok(carsByCategory);
    }

    //Внесение информации о новой категории
    [HttpPost]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    public IActionResult CreateCategory([FromBody] CategoryDto categoryCreate)
    {
        var validationResult = _validator.Validate(categoryCreate);

        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }
        
        var category = _categoryRepository
            .GetCategories()
            .FirstOrDefault(c => c.Name.Trim().ToLower() == categoryCreate.Name.Trim().ToLower());
        
        if (category != null)
        {
            ModelState.AddModelError("", "Категория уже есть");
            return StatusCode(422, ModelState);
        }

        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        var categoryMap = _mapper.Map<Category>(categoryCreate);
        
        if (!_categoryRepository.CreateCategory(categoryMap))
        {
            ModelState.AddModelError("","В процессе сохранения что-то пошло не так");
            return StatusCode(500, ModelState);
        }

        return Ok("Успешно сохранено");
    }
    
    //Обновление информации о категории
    [HttpPut("{categoryId}")]
    [ProducesResponseType(400)]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public IActionResult UpdateCategory(int categoryId, [FromBody]CategoryDto updatedCategory)
    {
        var validationResult = _validator.Validate(updatedCategory);

        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        if (!_categoryRepository.CategoryExists(categoryId))
            return NotFound();

        if (!ModelState.IsValid)
            return BadRequest();

        var categoryMap = _mapper.Map<Category>(updatedCategory);
        categoryMap.Id = categoryId;

        if(!_categoryRepository.UpdateCategory(categoryMap))
        {
            ModelState.AddModelError("", "Что-то пошло не так в процессе обновления");
            return StatusCode(500, ModelState);
        }

        return NoContent();
    }

    //Удаление категории
    [HttpDelete("{categoryId}")]
    [ProducesResponseType(400)]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public IActionResult DeleteCategory(int categoryId)
    {
        if(!_categoryRepository.CategoryExists(categoryId))
        {
            return NotFound();
        }

        var categoryToDelete = _categoryRepository.GetCategory(categoryId);

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if(!_categoryRepository.DeleteCategory(categoryToDelete))
            ModelState.AddModelError("", "Что-то пошло не так в процессе удаления");
        
        return NoContent();
    }
}