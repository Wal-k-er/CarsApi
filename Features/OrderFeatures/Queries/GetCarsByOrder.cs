using AutoMapper;
using Cars.Dto;
using Cars.Exceptions;
using Cars.Interfaces;
using MediatR;

namespace Cars.Features.OrderFeatures.Queries;

public class GetCarsByOrder : IRequest<ICollection<CarDto>>
{
    public GetCarsByOrder(int orderId)
    {
        Id = orderId;
    }

    public int Id { get; }
}

public class GetCarsByOrderHandler : IRequestHandler<GetCarsByOrder, ICollection<CarDto>>
{
    private readonly IMapper _mapper;
    private readonly IOrderRepository _orderRepository;

    public GetCarsByOrderHandler(IOrderRepository orderRepository, IMapper mapper)
    {
        _orderRepository = orderRepository;
        _mapper = mapper;
    }

    public async Task<ICollection<CarDto>> Handle(GetCarsByOrder request, CancellationToken cancellationToken)
    {
        if (!await _orderRepository.OrdersExists(request.Id))
            throw new EntityNotFoundException($"Не найден заказ {request.Id}");

        var cars = await _orderRepository
            .GetCarsByOrder(request.Id);

        return _mapper.Map<ICollection<CarDto>>(cars);
    }
}