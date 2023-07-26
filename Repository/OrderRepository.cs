using System.Diagnostics;
using Cars.Data;
using Cars.Dto;
using Cars.Interfaces;
using Cars.Models;
using Microsoft.EntityFrameworkCore;

namespace Cars.Repository;

public class OrderRepository : IOrderRepository
{
    private readonly DataContext _context;

    public OrderRepository(DataContext context)
    {
        _context = context;
    }
    
    public ICollection<Order> GetOrders()
    {
        var orders = _context.Orders
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Car).ToList();
        return orders;
    }

    public Order GetOrder(int orderId)
    {
        var order = _context.Orders.Where(o => o.Id == orderId)
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Car).FirstOrDefault();
        return order;
    }

    public ICollection<Car> GetCarsByOrder(int orderId)
    {
        var carsByOrder = _context.OrderItems
            .Where(p => p.OrderId == orderId).Select(c=>c.Car).ToList();
        return carsByOrder;
    }

    public bool OrdersExists(int orderId)
    {
        return _context.Orders.Any(c => c.Id == orderId);
    }

    public bool CreateOrder(Order order)
    {
        _context.Orders.Add(order);
        return Save();
    }

    public bool CreateOrderItem(OrderItem orderItem)
    {
        _context.OrderItems.Add(orderItem);
        return Save();
    }

    public bool UpdateOrder(Order order)
    {
        _context.Orders.Update(order);
        return Save();
    }

    public bool DeleteOrder(Order order)
    {
        var orderItems = _context.OrderItems.Where(i => i.OrderId == order.Id);
        _context.RemoveRange(orderItems);
        _context.Remove(order);
        return Save();
    }

    public bool Save()
    {
        var saved = _context.SaveChanges();
        return saved > 0 ? true : false;
    }
}