using AutoMapper;
using Cars.Dto;
using Cars.Interfaces;
using Cars.Models;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Cars.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CustomerController : Controller
{
    //Внедрение зависимостей
    private readonly ICustomerRepository _customerRepository;
    private readonly IValidator<CustomerDto> _validator;
    private readonly IMapper _mapper;

    public CustomerController(ICustomerRepository customerRepository, 
        IValidator<CustomerDto> validator, IMapper mapper)
    {
        _customerRepository = customerRepository;
        _validator = validator;
        _mapper = mapper;
    }

    //Получение информации о покупателях
    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IEnumerable<Customer>))]
    public IActionResult GetCustomers()
    {
        var customers = _mapper.Map<List<CustomerDto>>(_customerRepository.GetCustomers());

        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        return Ok(customers);
    }
    
    //Получение информации о покупателе
    [HttpGet("{customerId}")]
    [ProducesResponseType(200, Type = typeof(Customer))]
    [ProducesResponseType(400)]
    public IActionResult GetCustomer(int customerId)
    {
        if (!_customerRepository.CustomerExists(customerId))
            return NotFound();
        
        var customer = _mapper.Map<CustomerDto>(_customerRepository.GetCustomer(customerId));

        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        return Ok(customer);
    }
    
    //Получение информации о заказах покупателя
    [HttpGet("{customerId}/orders")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<Order>))]
    [ProducesResponseType(400)]
    public IActionResult GetOrdersByCustomer(int customerId)
    {
        if (!_customerRepository.CustomerExists(customerId))
            return NotFound();
        
        var orders = _mapper.Map<List<OrderDto>>(_customerRepository.GetOrdersByCustomer(customerId));

        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        return Ok(orders);
    }
    
    //Внесение информации о покупателе
    [HttpPost]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    public IActionResult CreateCustomer([FromBody] CustomerDto customerCreate)
    {
        var validationResult = _validator.Validate(customerCreate);

        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }
        
        var customer = _customerRepository
            .GetCustomers()
            .FirstOrDefault(c => c.FirstName.Trim().ToLower()+" "+ c.LastName.Trim().ToLower() == 
                                 customerCreate.FirstName.Trim().ToLower()+" "+
                                 customerCreate.LastName.Trim().ToLower());
        
        if (customer != null)
        {
            ModelState.AddModelError("", "Покупатель уже есть");
            return StatusCode(422, ModelState);
        }

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var customerMap = _mapper.Map<Customer>(customerCreate);
        
        if (!_customerRepository.CreateCustomer(customerMap))
        {
            ModelState.AddModelError("","В процессе сохранения что-то пошло не так");
            return StatusCode(500, ModelState);
        }

        return Ok("Успешно сохранено");
    }
    
    //Обновление информации о покупателе
    [HttpPut("{customerId}")]
    [ProducesResponseType(400)]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public IActionResult UpdateCategory(int customerId, [FromBody]CustomerDto updatedCustomer)
    {
        var validationResult = _validator.Validate(updatedCustomer);

        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        if (!_customerRepository.CustomerExists(customerId))
            return NotFound();

        if (!ModelState.IsValid)
            return BadRequest();

        var customerMap = _mapper.Map<Customer>(updatedCustomer);
        customerMap.Id = customerId;

        if(!_customerRepository.UpdateCustomer(customerMap))
        {
            ModelState.AddModelError("", "Что-то пошло не так в процессе обновления");
            return StatusCode(500, ModelState);
        }

        return NoContent();
    }

    //Удаление информации о покупателе
    [HttpDelete("{customerId}")]
    [ProducesResponseType(400)]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public IActionResult DeleteCustomer(int customerId)
    {
        if(!_customerRepository.CustomerExists(customerId))
            return NotFound();
        
        var customerToDelete = _customerRepository.GetCustomer(customerId);

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (!_customerRepository.DeleteCustomer(customerToDelete))
            ModelState.AddModelError("", "Что-то пошло не так в процессе удаления");

        return NoContent();
    }
}