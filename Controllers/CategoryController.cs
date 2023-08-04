using Cars.Dto;
using Cars.Exceptions;
using Cars.Features.CategoryFeatures.Commands;
using Cars.Features.CategoryFeatures.Queries;
using Cars.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Cars.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoryController : Controller
{
    //Внедрение зависимостей
    private readonly IMediator _mediator;

    public CategoryController(IMediator mediator)
    {
        _mediator = mediator;
    }

    //Получение списка категорий
    [HttpGet]
    [ProducesResponseType(200, Type = typeof(ICollection<Category>))]
    public async Task<IActionResult> GetCategories()
    {
        var query = new GetAllCategories();
        var response = await _mediator.Send(query);

        return Ok(response);
    }

    //Получение информации о категории
    [HttpGet("{categoryId}")]
    [ProducesResponseType(200, Type = typeof(Category))]
    [ProducesResponseType(400)]
    public async Task<IActionResult> GetCategory(int categoryId)
    {
        try
        {
            var query = new GetCategoryById(categoryId);
            var response = await _mediator.Send(query);
            return Ok(response);
        }
        catch (EntityNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

    //Получение информации о машинах в категории
    [HttpGet("{categoryId}/cars")]
    [ProducesResponseType(200, Type = typeof(ICollection<Car>))]
    public async Task<IActionResult> GetCarsByCategory(int categoryId)
    {
        try
        {
            var query = new GetCarsByCategory(categoryId);
            var response = await _mediator.Send(query);
            return Ok(response);
        }
        catch (EntityNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

    //Внесение информации о новой категории
    [HttpPost]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> CreateCategory([FromBody] CategoryDto categoryCreate)
    {
        try
        {
            var command = new CreateCategory(categoryCreate);
            var response = await _mediator.Send(command);
            return Ok("Создана категория " + response);
        }
        catch (EntityNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

    //Обновление информации о категории
    [HttpPut("{categoryId}")]
    [ProducesResponseType(400)]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> UpdateCategory(int categoryId, [FromBody] CategoryDto updatedCategory)
    {
        try
        {
            var command = new UpdateCategory(categoryId, updatedCategory);
            var response = await _mediator.Send(command);
            return Ok("Обновлена категория " + response);
        }
        catch (EntityNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

    //Удаление категории
    [HttpDelete("{categoryId}")]
    [ProducesResponseType(400)]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> DeleteCategory(int categoryId)
    {
        try
        {
            var command = new DeleteCategory(categoryId);
            var response = await _mediator.Send(command);
            return Ok("Удалена категория " + response);
        }
        catch (EntityNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }
}