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
    
    public async Task<ICollection<Customer>> GetCustomers()
    {
        return await _context.Customers.OrderBy(p => p.Id).ToListAsync();
    }

    public async Task<Customer> GetCustomer(int customerId)
    {
        return await _context.Customers.FirstOrDefaultAsync(p => p.Id == customerId);
    }

    public async Task<ICollection<Order>> GetOrdersByCustomer(int customerId)
    {
        var orderByCustomer = await _context.Orders
            .Where(o => o.Customer.Id == customerId)
            .Include(oi=>oi.OrderItems)
            .ThenInclude(c=>c.Car).ToListAsync();
        return orderByCustomer;
    }

    public async Task<bool> CustomerExists(int customerId)
    {
        return await _context.Customers.AnyAsync(c => c.Id == customerId);
    }

    public async Task CreateCustomer(Customer customer)
    {
        _context.Add(customer);
        await Save();
    }

    public async Task UpdateCustomer(Customer customer)
    {
        _context.Update(customer);
        await Save();
    }

    public async Task DeleteCustomer(Customer customer)
    {
        _context.Remove(customer);
        await Save();
    }

    public async Task Save()
    {
        await _context.SaveChangesAsync();
    }
}