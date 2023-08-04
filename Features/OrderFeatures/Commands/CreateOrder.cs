using AutoMapper;
using Cars.Dto;
using Cars.Exceptions;
using Cars.Interfaces;
using Cars.Models;
using MediatR;

namespace Cars.Features.OrderFeatures.Commands;

public class CreateOrder : IRequest<int>
{
    public CreateOrder(OrderDto orderDto, int customerId)
    {
        Model = orderDto;
        CustomerId = customerId;
    }

    public OrderDto Model { get; }

    public int CustomerId { get; set; }
}

public class CreateOrderHandler : IRequestHandler<CreateOrder, int>
{
    private readonly ICarRepository _carRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;
    private readonly IOrderRepository _orderRepository;

    public CreateOrderHandler(IOrderRepository orderRepository, ICustomerRepository customerRepository,
        ICarRepository carRepository, IMapper mapper)
    {
        _orderRepository = orderRepository;
        _customerRepository = customerRepository;
        _carRepository = carRepository;
        _mapper = mapper;
    }

    public async Task<int> Handle(CreateOrder request, CancellationToken cancellationToken)
    {
        var model = request.Model;

        var entity = _mapper.Map<Order>(model);
        if (!await _customerRepository.CustomerExists(request.CustomerId))
            throw new EntityNotFoundException($"Не найден заказчик {request.CustomerId}");

        var customer = await _customerRepository.GetCustomer(request.CustomerId);

        entity.Customer = customer;

        await _orderRepository.CreateOrder(entity);
        foreach (var orderItem in model.OrderItems)
        {
            var car = await _carRepository.GetCar(orderItem.CarId);
            var orderItemEntity = new OrderItem
            {
                Order = entity,
                OrderId = entity.Id,
                Car = car,
                CarId = car.Id
            };
            await _orderRepository.CreateOrderItem(orderItemEntity);
        }

        return entity.Id;
    }
}