using Cars.Exceptions;
using Cars.Interfaces;
using MediatR;

namespace Cars.Features.OrderFeatures.Commands;

public class DeleteOrder : IRequest<int>
{
    public DeleteOrder(int orderId)
    {
        Id = orderId;
    }

    public int Id { get; }
}

public class DeleteOrderHandler : IRequestHandler<DeleteOrder, int>
{
    private readonly IOrderRepository _orderRepository;

    public DeleteOrderHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<int> Handle(DeleteOrder request, CancellationToken cancellationToken)
    {
        if (!await _orderRepository.OrdersExists(request.Id))
            throw new EntityNotFoundException($"Не найден заказ {request.Id}");

        var order = await _orderRepository.GetOrder(request.Id);

        await _orderRepository.DeleteOrder(order);

        return request.Id;
    }
}