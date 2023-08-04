using AutoMapper;
using Cars.Dto;
using Cars.Interfaces;
using MediatR;

namespace Cars.Features.OrderFeatures.Queries;

public class GetAllOrders : IRequest<ICollection<OrderDto>>
{
}

public class GetAllOrdersHandler : IRequestHandler<GetAllOrders, ICollection<OrderDto>>
{
    private readonly IMapper _mapper;
    private readonly IOrderRepository _orderRepository;

    public GetAllOrdersHandler(IOrderRepository orderRepository, IMapper mapper)
    {
        _orderRepository = orderRepository;
        _mapper = mapper;
    }

    public async Task<ICollection<OrderDto>> Handle(GetAllOrders request, CancellationToken cancellationToken)
    {
        var orders = await _orderRepository.GetOrders();
        return _mapper.Map<ICollection<OrderDto>>(orders);
    }
}