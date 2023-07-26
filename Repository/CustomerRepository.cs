using Cars.Data;
using Cars.Interfaces;
using Cars.Models;
using Microsoft.EntityFrameworkCore;

namespace Cars.Repository;

public class CustomerRepository : ICustomerRepository
{
    private readonly DataContext _context;

    public CustomerRepository(DataContext context)
    {
        _context = context;
    }
    
    public ICollection<Customer> GetCustomers()
    {
        return _context.Customers.OrderBy(p => p.Id).ToList();
    }

    public Customer GetCustomer(int customerId)
    {
        return _context.Customers.Where(p => p.Id == customerId).FirstOrDefault();
    }

    public ICollection<Order> GetOrdersByCustomer(int customerId)
    {
        var orderByCustomer = _context.Orders
            .Where(o => o.Customer.Id == customerId)
            .Include(oi=>oi.OrderItems)
            .ThenInclude(c=>c.Car).ToList();
        return orderByCustomer;
    }

    public bool CustomerExists(int customerId)
    {
        return _context.Customers.Any(c => c.Id == customerId);
    }

    public bool CreateCustomer(Customer customer)
    {
        _context.Add(customer);
        return Save();
    }

    public bool UpdateCustomer(Customer customer)
    {
        _context.Update(customer);
        return Save();
    }

    public bool DeleteCustomer(Customer customer)
    {
        _context.Remove(customer);
        return Save();
    }

    public bool Save()
    {
        var saved = _context.SaveChanges();
        return saved > 0 ? true : false;
    }
}