using AutoMapper;
using Cars.Dto;
using Cars.Exceptions;
using Cars.Interfaces;
using MediatR;

namespace Cars.Features.OrderFeatures.Queries;

public class GetOrderById : IRequest<OrderDto>
{
    public GetOrderById(int orderId)
    {
        Id = orderId;
    }

    public int Id { get; }
}

public class GetOrderByIdHandler : IRequestHandler<GetOrderById, OrderDto>
{
    private readonly IMapper _mapper;
    private readonly IOrderRepository _orderRepository;

    public GetOrderByIdHandler(IOrderRepository orderRepository, IMapper mapper)
    {
        _orderRepository = orderRepository;
        _mapper = mapper;
    }

    public async Task<OrderDto> Handle(GetOrderById request, CancellationToken cancellationToken)
    {
        if (!await _orderRepository.OrdersExists(request.Id))
            throw new EntityNotFoundException($"Не найден заказ {request.Id}");

        var order = await _orderRepository.GetOrder(request.Id);

        return _mapper.Map<OrderDto>(order);
    }
}