using Cars.Dto;
using Cars.Models;

namespace Cars.Interfaces;

public interface IOrderRepository
{
    Task<ICollection<Order>> GetOrders();
    Task<Order> GetOrder(int orderId);
    Task<ICollection<Car>> GetCarsByOrder(int orderId);
    Task<bool> OrdersExists(int orderId);
    Task CreateOrder(Order order);
    Task<OrderItem> GetOrderItem(int orderId, int carId);
    Task<bool> OrderItemExists(int orderId, int carId);
    Task CreateOrderItem(OrderItem orderItem);
    Task DeleteOrderItem(OrderItem orderItem);
    Task DeleteOrderItems(Order order);
    Task UpdateOrder(Order order);
    Task DeleteOrder(Order order);
    Task Save();
}