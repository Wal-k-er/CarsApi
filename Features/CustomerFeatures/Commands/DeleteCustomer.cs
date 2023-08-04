using Cars.Exceptions;
using Cars.Interfaces;
using MediatR;

namespace Cars.Features.CustomerFeatures.Commands;

public class DeleteCustomer : IRequest<int>
{
    public DeleteCustomer(int customerId)
    {
        Id = customerId;
    }

    public int Id { get; }
    
    public class DeleteCustomerHandler : IRequestHandler<DeleteCustomer, int>
    {
        private readonly ICustomerRepository _customerRepository;

        public DeleteCustomerHandler(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<int> Handle(DeleteCustomer request, CancellationToken cancellationToken)
        {
            if (!await _customerRepository.CustomerExists(request.Id))
                throw new EntityNotFoundException($"Не найден заказчик {request.Id}");

            var customer = await _customerRepository.GetCustomer(request.Id);

            await _customerRepository.DeleteCustomer(customer);

            return request.Id;
        }
    }
}