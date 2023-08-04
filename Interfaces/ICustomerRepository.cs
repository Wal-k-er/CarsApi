using Cars.Models;

namespace Cars.Interfaces;

public interface ICustomerRepository
{
    Task<ICollection<Customer>> GetCustomers();
    Task<Customer> GetCustomer(int customerId);
    Task<ICollection<Order>> GetOrdersByCustomer(int customerId);
    Task<bool> CustomerExists(int customerId);
    Task CreateCustomer(Customer customer);
    Task UpdateCustomer(Customer customer);
    Task DeleteCustomer(Customer customer);
    Task Save();
}