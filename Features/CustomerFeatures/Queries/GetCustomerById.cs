using AutoMapper;
using Cars.Dto;
using Cars.Exceptions;
using Cars.Features.CountryFeatures.Queries;
using Cars.Interfaces;
using MediatR;

namespace Cars.Features.CustomerFeatures.Queries;

public class GetCustomerById : IRequest<CustomerDto>
{
    public GetCustomerById(int id)
    {
        Id = id;
    }

    public int Id { get; }
}

public class GetCustomerByIdHandler : IRequestHandler<GetCustomerById, CustomerDto>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;

    public GetCustomerByIdHandler(ICustomerRepository customerRepository, IMapper mapper)
    {
        _customerRepository = customerRepository;
        _mapper = mapper;
    }

    public async Task<CustomerDto> Handle(GetCustomerById request, CancellationToken cancellationToken)
    {
        if (!await _customerRepository.CustomerExists(request.Id))
            throw new EntityNotFoundException($"Не найден заказчик {request.Id}");

        var customer = await _customerRepository.GetCustomer(request.Id);

        return _mapper.Map<CustomerDto>(customer);
    }
}