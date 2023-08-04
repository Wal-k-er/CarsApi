using AutoMapper;
using Cars.Dto;
using Cars.Exceptions;
using Cars.Interfaces;
using Cars.Models;
using MediatR;

namespace Cars.Features.OrderFeatures.Commands;

public class UpdateOrder : IRequest<int>
{
    public UpdateOrder(int orderId, OrderDto orderDto)
    {
        OrderId = orderId;
        Model = orderDto;
    }

    public int OrderId { get; }
    public OrderDto Model { get; }
}

public class UpdateOrderHandler : IRequestHandler<UpdateOrder, int>
{
    private readonly ICarRepository _carRepository;
    private readonly IMapper _mapper;
    private readonly IOrderRepository _orderRepository;

    public UpdateOrderHandler(IOrderRepository orderRepository, ICarRepository carRepository,
        IMapper mapper)
    {
        _orderRepository = orderRepository;
        _carRepository = carRepository;
        _mapper = mapper;
    }

    public async Task<int> Handle(UpdateOrder request, CancellationToken cancellationToken)
    {
        if (!await _orderRepository.OrdersExists(request.OrderId))
            throw new EntityNotFoundException($"Не  заказ {request.OrderId}");

        var model = request.Model;

        var entity = _mapper.Map<Order>(model);
        entity.Id = request.OrderId;

        await _orderRepository.UpdateOrder(entity);

        await _orderRepository.DeleteOrderItems(entity);

        foreach (var orderItemDto in model.OrderItems)
        {
            if (!await _carRepository.CarExists(orderItemDto.CarId))
                throw new EntityNotFoundException($"Не найдена машина {orderItemDto.CarId}");
            var car = await _carRepository.GetCar(orderItemDto.CarId);
            var orderItem = new OrderItem
            {
                Order = entity,
                OrderId = entity.Id,
                Car = car,
                CarId = car.Id
            };
            await _orderRepository.CreateOrderItem(orderItem);
        }

        return entity.Id;
    }
}