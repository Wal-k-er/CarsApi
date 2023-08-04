using AutoMapper;
using Cars.Dto;
using Cars.Interfaces;
using MediatR;

namespace Cars.Features.CustomerFeatures.Queries;

public class GetAllCustomers : IRequest<ICollection<CustomerDto>>
{
}

public class GetAllCustomersHandler : IRequestHandler<GetAllCustomers, ICollection<CustomerDto>>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;

    public GetAllCustomersHandler(ICustomerRepository customerRepository, IMapper mapper)
    {
        _customerRepository = customerRepository;
        _mapper = mapper;
    }

    public async Task<ICollection<CustomerDto>> Handle(GetAllCustomers request, CancellationToken cancellationToken)
    {
        var customers = await _customerRepository.GetCustomers();

        return _mapper.Map<ICollection<CustomerDto>>(customers);
    }
}