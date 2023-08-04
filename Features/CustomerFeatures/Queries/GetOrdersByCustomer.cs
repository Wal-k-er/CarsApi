using AutoMapper;
using Cars.Dto;
using Cars.Exceptions;
using Cars.Features.CountryFeatures.Queries;
using Cars.Interfaces;
using MediatR;

namespace Cars.Features.CustomerFeatures.Queries;

public class GetOrdersByCustomer : IRequest<ICollection<OrderDto>>
{
    public int Id { get; }

    public GetOrdersByCustomer(int id)
    {
        Id = id;
    }
}

public class GetOrdersByCustomerHandler : IRequestHandler<GetOrdersByCustomer, ICollection<OrderDto>>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;

    public GetOrdersByCustomerHandler(ICustomerRepository customerRepository, IMapper mapper)
    {
        _customerRepository = customerRepository;
        _mapper = mapper;
    }

    public async Task<ICollection<OrderDto>> Handle(GetOrdersByCustomer request, 
        CancellationToken cancellationToken)
    {
        if(!await _customerRepository.CustomerExists(request.Id))
            throw new EntityNotFoundException($"Не найден заказчик {request.Id}");
        
        var orders = await _customerRepository
            .GetOrdersByCustomer(request.Id);

        return _mapper.Map<ICollection<OrderDto>>(orders);
    }
}