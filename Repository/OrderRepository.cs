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
    
    public async Task<ICollection<Order>> GetOrders()
    {
        var orders = await _context.Orders
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Car).ToListAsync();
        return orders;
    }

    public async Task<Order> GetOrder(int orderId)
    {
        var order = await _context.Orders.Where(o => o.Id == orderId)
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Car).FirstOrDefaultAsync();
        return order;
    }

    public async Task<ICollection<Car>> GetCarsByOrder(int orderId)
    {
        var carsByOrder = await _context.OrderItems
            .Where(p => p.OrderId == orderId).Select(c=>c.Car).ToListAsync();
        return carsByOrder;
    }

    public async Task<bool> OrdersExists(int orderId)
    {
        return await _context.Orders.AnyAsync(c => c.Id == orderId);
    }

    public async Task CreateOrder(Order order)
    {
        _context.Orders.Add(order);
        await Save();
    }

    public async Task<OrderItem> GetOrderItem(int orderId, int carId)
    {
        var orderItem = await _context.OrderItems
            .Where(o => o.OrderId == orderId)
            .FirstOrDefaultAsync(c => c.CarId==carId);
        return orderItem;
    }

    public async Task<bool> OrderItemExists(int orderId, int carId)
    {
        return await _context.OrderItems.Where(o => o.OrderId == orderId)
            .AnyAsync(c => c.CarId == carId);
    }

    public async Task CreateOrderItem(OrderItem orderItem)
    {
        _context.OrderItems.Add(orderItem);
        await Save();
    }

    public async Task DeleteOrderItem(OrderItem orderItem)
    {
        _context.OrderItems.Remove(orderItem);
        await Save();
    }

    public async Task DeleteOrderItems(Order order)
    {
        var orderItems = _context.OrderItems
            .Where(o => o.Order == order);
        _context.RemoveRange(orderItems);
        await Save();
    }

    public async Task UpdateOrder(Order order)
    {
        _context.Orders.Update(order);
        await Save();
    }

    public async Task DeleteOrder(Order order)
    {
        await DeleteOrderItems(order);
        _context.Remove(order);
        await Save();
    }

    public async Task Save()
    {
        await _context.SaveChangesAsync();
    }
}