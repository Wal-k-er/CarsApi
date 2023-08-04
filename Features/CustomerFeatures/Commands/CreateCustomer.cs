using AutoMapper;
using Cars.Dto;
using Cars.Interfaces;
using Cars.Models;
using MediatR;

namespace Cars.Features.CustomerFeatures.Commands;

public class CreateCustomer : IRequest<int>
{
    public CreateCustomer(CustomerDto customerDto)
    {
        Model = customerDto;
    }

    public CustomerDto Model { get; }

    public class CreateCustomerHandler : IRequestHandler<CreateCustomer, int>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;


        public CreateCustomerHandler(ICustomerRepository customerRepository,
            IMapper mapper)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateCustomer request, CancellationToken cancellationToken)
        {
            var model = request.Model;

            var entity = _mapper.Map<Customer>(model);

            await _customerRepository.CreateCustomer(entity);

            return entity.Id;
        }
    }
}