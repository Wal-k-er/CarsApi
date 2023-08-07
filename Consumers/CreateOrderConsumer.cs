using Cars.Features.OrderFeatures.Commands;
using Cars.Interfaces;
using MassTransit;

namespace Cars.Consumers;

public class CreateOrderConsumer : IConsumer<CreateOrder>
{
    private readonly ICustomerRepository _customerRepository;

    public CreateOrderConsumer(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }
    public async Task Consume(ConsumeContext<CreateOrder> context)
    {
        var order = context.Message.Model;
        var dateCreated = order.CreatedDate.Day+"."+order.CreatedDate.Month+"."+order.CreatedDate.Year;
        var customerId = context.Message.CustomerId;
        var customer = await _customerRepository.GetCustomer(customerId);
        Console.WriteLine($"Заказ № {order.Id} создан {dateCreated}, количество товаров {order.OrderItems.Count} " +
                          $"заказчиком {customerId} {customer.FirstName} {customer.LastName}");
    }
}