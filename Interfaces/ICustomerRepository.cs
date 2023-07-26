using Cars.Models;

namespace Cars.Interfaces;

public interface ICustomerRepository
{
    ICollection<Customer> GetCustomers();
    Customer GetCustomer(int customerId);
    ICollection<Order> GetOrdersByCustomer(int customerId);
    bool CustomerExists(int customerId);
    bool CreateCustomer(Customer customer);
    bool UpdateCustomer(Customer customer);
    bool DeleteCustomer(Customer customer);
    bool Save();
}