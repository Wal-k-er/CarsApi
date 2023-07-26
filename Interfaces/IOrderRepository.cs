using Cars.Dto;
using Cars.Models;

namespace Cars.Interfaces;

public interface IOrderRepository
{
    ICollection<Order> GetOrders();
    Order GetOrder(int orderId);
    ICollection<Car> GetCarsByOrder(int orderId);
    bool OrdersExists(int orderId);
    bool CreateOrder(Order order);
    bool CreateOrderItem(OrderItem orderItem);
    bool UpdateOrder(Order order);
    bool DeleteOrder(Order order);
    bool Save();
}