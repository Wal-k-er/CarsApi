using Cars.Exceptions;
using Cars.Interfaces;
using MediatR;

namespace Cars.Features.OrderFeatures.Commands;

public class DeleteOrderItem : IRequest<int>
{
    public DeleteOrderItem(int orderId, int carId)
    {
        OrderId = orderId;
        CarId = carId;
    }

    public int OrderId { get; }
    public int CarId { get; }
}

public class DeleteOrderItemHandler : IRequestHandler<DeleteOrderItem, int>
{
    private readonly IOrderRepository _orderRepository;

    public DeleteOrderItemHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<int> Handle(DeleteOrderItem request, CancellationToken cancellationToken)
    {
        if (!await _orderRepository.OrdersExists(request.OrderId))
            throw new EntityNotFoundException($"Не найден заказ {request.OrderId}");

        if (!await _orderRepository.OrderItemExists(request.OrderId, request.CarId))
            throw new EntityNotFoundException($"Не найдена позиция заказа {request.CarId}");

        var orderItem = await _orderRepository.GetOrderItem(request.OrderId, request.CarId);

        await _orderRepository.DeleteOrderItem(orderItem);
        
        return request.CarId;
    }
}