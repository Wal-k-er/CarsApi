using AutoMapper;
using Cars.Dto;
using Cars.Exceptions;
using Cars.Interfaces;
using Cars.Models;
using MediatR;

namespace Cars.Features.CustomerFeatures.Commands;

public class UpdateCustomer : IRequest<int>
{
    public UpdateCustomer(int customerId, CustomerDto customerDto)
    {
        CustomerId = customerId;
        Model = customerDto;
    }

    public int CustomerId { get; }
    public CustomerDto Model { get; }

    public class UpdateCustomerHandler : IRequestHandler<UpdateCustomer, int>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;

        public UpdateCustomerHandler(ICustomerRepository customerRepository,
            IMapper mapper)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
        }

        public async Task<int> Handle(UpdateCustomer request, CancellationToken cancellationToken)
        {
            if (!await _customerRepository.CustomerExists(request.CustomerId))
                throw new EntityNotFoundException($"Не найден заказчик {request.CustomerId}");

            var model = request.Model;

            var entity = _mapper.Map<Customer>(model);
            entity.Id = request.CustomerId;

            await _customerRepository.UpdateCustomer(entity);

            return entity.Id;
        }
    }
}