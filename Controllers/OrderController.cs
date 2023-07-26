using AutoMapper;
using Cars.Dto;
using Cars.Interfaces;
using Cars.Models;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Cars.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrderController: Controller
{
    //Внедрение зависимостей
    private readonly IOrderRepository _orderRepository;
    private readonly IMapper _mapper;
    private readonly ICustomerRepository _customerRepository;
    private readonly ICarRepository _carRepository;
    private readonly IValidator<OrderDto> _validator;

    public OrderController(IOrderRepository orderRepository, ICustomerRepository customerRepository, 
        ICarRepository carRepository, IValidator<OrderDto> validator, IMapper mapper)
    {
        _orderRepository = orderRepository;
        _customerRepository = customerRepository;
        _carRepository = carRepository;
        _validator = validator;
        _mapper = mapper;
    }
    
    //Получение информации о заказах
    [HttpGet]
    [ProducesResponseType(200, Type = typeof(ICollection<OrderDto>))]
    public IActionResult GetOrdersWithItems()
    {
        var orders = _mapper.Map<List<OrderDto>>(_orderRepository.GetOrders());
        
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        return Ok(orders);
    }
    
    //Получение информации о заказе
    [HttpGet("{orderId}")]
    [ProducesResponseType(200, Type = typeof(OrderDto))]
    [ProducesResponseType(400)]
    public IActionResult GetOrderWithItems(int orderId)
    {
        if (!_orderRepository.OrdersExists(orderId))
            return NotFound();
        
        var order = _mapper.Map<OrderDto>(_orderRepository.GetOrder(orderId));
        
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        return Ok(order);
    }
    
    //Получение списка машин, указаных в заказе
    [HttpGet("{orderId}/cars")]
    [ProducesResponseType(200, Type = typeof(ICollection<Car>))]
    public IActionResult GetCarsByOrder(int orderId)
    {
        if (!_orderRepository.OrdersExists(orderId))
            return NotFound();
        
        var carsByOrder = _mapper.Map<List<CarDto>>(_orderRepository.GetCarsByOrder(orderId));
        
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        return Ok(carsByOrder);
    }
    
    //Внесение информации о заказе
    [HttpPost]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    public IActionResult CreateOrder([FromBody] OrderDto orderCreate, [FromQuery] int customerId)
    {
        var validationResult = _validator.Validate(orderCreate);

        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }
        
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (!_customerRepository.CustomerExists(customerId))
            return NotFound();

        //Проверка наличия позиций заказа при создании
        if (orderCreate.OrderItems.Count == 0)
        {
            ModelState.AddModelError("","Отсутствуют позиции заказа");
            return BadRequest(ModelState);
        }
        
        var orderMap = _mapper.Map<Order>(orderCreate);

        orderMap.Customer = _customerRepository.GetCustomer(customerId);

        if (!_orderRepository.CreateOrder(orderMap))
        {
            ModelState.AddModelError("","В процессе сохранения что-то пошло не так");
            return StatusCode(500, ModelState);
        }
        
        // Создание позиций заказа
        foreach (var item in orderCreate.OrderItems)
        {
            var car = _carRepository.GetCar(item.CarId);
            
            if (car != null)
            {
                var orderitem = new OrderItem
                {
                    Order = orderMap,
                    Car = car,
                    CarId = car.Id,
                    OrderId = orderMap.Id
                };
                
                _orderRepository.CreateOrderItem(orderitem);
            }
        }

        return Ok("Успешно сохранено");
    }
    
    //Обновление информации о заказе
    [HttpPut("{orderId}")]
    [ProducesResponseType(400)]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public IActionResult UpdateOrder(int orderId, [FromBody]OrderDto updatedOrder, [FromQuery] int customerId)
    {
        var validationResult = _validator.Validate(updatedOrder);

        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        if (!_orderRepository.OrdersExists(orderId))
            return NotFound();

        if (!ModelState.IsValid)
            return BadRequest();

        var orderMap = _mapper.Map<Order>(updatedOrder);

        orderMap.Customer = _customerRepository.GetCustomer(customerId);
        
        if (!_orderRepository.UpdateOrder(orderMap))
        {
            ModelState.AddModelError("", "Что-то пошло не так в процессе обновления");
            return StatusCode(500, ModelState);
        }
        
        //Добавление новых позиций заказа
        var orderItems = _orderRepository.GetOrder(orderMap.Id).OrderItems;
        
        foreach (var item in updatedOrder.OrderItems)
        {
            if (!_carRepository.CarExists(item.CarId))
                return NotFound();
            var car = _carRepository.GetCar(item.CarId);
            if (!orderItems.Any(o=>o.Car==car))
            {
                var orderItem = new OrderItem
                {
                    Order = orderMap,
                    OrderId = orderMap.Id,
                    Car = car,
                    CarId = car.Id
                };
                _orderRepository.CreateOrderItem(orderItem);
            }
        }
        
        return NoContent();
    }

    //Удаление информации о заказе
    [HttpDelete("{orderId}")]
    [ProducesResponseType(400)]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public IActionResult DeleteOrder(int orderId)
    {
        if(!_orderRepository.OrdersExists(orderId))
        {
            return NotFound();
        }

        var orderToDelete = _orderRepository.GetOrder(orderId);

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if(!_orderRepository.DeleteOrder(orderToDelete))
        {
            ModelState.AddModelError("", "Что-то пошло не так в процессе удаления");
        }

        return NoContent();
    }
}